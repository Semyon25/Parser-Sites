using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Parser
{
    class ParseYM
    {

        private readonly string prefix = "https://market.yandex.ru/search?&text=";


        public ObservableCollection<Product> Find(RequestParametres request)
        {
            if (request.Request == string.Empty || request.Count < 1)
                throw new Exception("Неверный запрос!");

            int attempt = 0;
            ObservableCollection<Product> answer = null;
            while (true)
            {
                string searchText = request.Request;
                searchText = searchText.Replace(" ", "+");
                string html = GetHTML(prefix + searchText);

                if (html == string.Empty)
                    throw new Exception("Не удалось получить данные");
                try
                {
                    answer = DefineProducts(html, request.Count);
                    break;
                }
                catch (ArgumentException)
                {
                    attempt++;
                    if (attempt == 2) throw new Exception("Не удалось получить данные веб-страницы по вашему запросу");
                }
            }
            return answer;
        }

        private ObservableCollection<Product> DefineProducts(string html, int count)
        {
            int index = 0;
            ObservableCollection<Product> products = new ObservableCollection<Product>();
            for(int i = 0; i<count; ++i)
            {
                int indexBegin = html.IndexOf("id=\"product", index);
                int indexEnd = html.IndexOf("id=\"product", ++indexBegin);
                if (indexBegin == -1 || indexEnd == -1)
                {
                    if (i==0) throw new ArgumentException();
                    else throw new Exception("Доступное количество товаров: " + i.ToString());
                }
                    

                index = indexEnd;
                string productInfo = html.Substring(indexBegin, indexEnd - indexBegin);

                // Поиск картинки
                string pat = "<img class=\"image\" src=\"";
                int idxPicture = productInfo.IndexOf(pat) + 26;
                string pictureLinkProduct = "https://";
                while (productInfo[idxPicture] != '"')
                {
                    pictureLinkProduct += productInfo[idxPicture];
                    ++idxPicture;
                }

                // Поиск названия 
                string titleProduct = "";
                pat = "title=\"";
                int idxTitle = productInfo.IndexOf(pat, idxPicture);
                idxTitle += 7;
                while (productInfo[idxTitle] != '\"')
                {
                    titleProduct += productInfo[idxTitle];
                    ++idxTitle;
                }

                // Поиск цены
                string priceProductStr = "";
                pat = "<div class=\"price\">";
                int idxPrice = productInfo.IndexOf(pat, 0);
                idxPrice += 19;
                while (productInfo[idxPrice] != '<')
                {
                    priceProductStr += productInfo[idxPrice];
                    ++idxPrice;
                }

                // Поиск ссылки
                string linkProduct = "https://market.yandex.ru";
                pat = "href=\"";
                int idxHref = productInfo.IndexOf(pat, 0);
                idxHref += 6;
                while (productInfo[idxHref] != '"')
                {
                    linkProduct += productInfo[idxHref];
                    ++idxHref;
                }

                products.Add(new Product(titleProduct, priceProductStr, linkProduct, pictureLinkProduct));
            }
            return products;
        }


        private string GetHTML(string uri)
        {
            StringBuilder sb = new StringBuilder();
            byte[] buf = new byte[8192];
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            int count = 0;
            do
            {
                count = resStream.Read(buf, 0, buf.Length);
                if (count != 0)
                {
                    sb.Append(Encoding.Default.GetString(buf, 0, count));
                }
            }
            while (count > 0);
            
            return ConvertFrom1251ToUtf8(sb.ToString());
        }

        string ConvertFrom1251ToUtf8(string text)
        {
            Encoding utf8 = Encoding.GetEncoding("UTF-8");
            Encoding win1251 = Encoding.GetEncoding("Windows-1251");

            byte[] utf8Bytes = win1251.GetBytes(text);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);

            return win1251.GetString(win1251Bytes);
        }



    }
}
