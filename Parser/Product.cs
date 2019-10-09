using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    class Product : INotifyPropertyChanged
    {
        private string name;
        private string url;
        private string urlImage;
        private int price;

        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(); }
        }

        public string Url
        {
            get => url;
            set { url = value; OnPropertyChanged(); }
        }
        public string UrlImage
        {
            get => urlImage;
            set { urlImage = value; OnPropertyChanged(); }
        }
        public int Price
        {
            get => price;
            set { price = value; OnPropertyChanged(); }
        }


        public Product(string _name, string _price, string _url, string _urlImage)
        {
            Name = _name;
            Price = ConvertPriceFromStringToInt(_price);
            Url = _url;
            UrlImage = _urlImage;
        }


        int ConvertPriceFromStringToInt(string text)
        {
            string num = "";
            foreach(char c in text)
            {
                if (Char.IsDigit(c)) num += c;
            }
            return Convert.ToInt32(num);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
