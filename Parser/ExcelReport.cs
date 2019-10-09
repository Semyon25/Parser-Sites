using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;

namespace Parser
{
    class ExcelReport
    {


        public void CreateReport(ObservableCollection<Product> products, string title = "")
        {
            if (products == null || products.Count == 0)
                throw new Exception("Нет данных для создания отчета!");

            Application excel;
            Workbook workBook;
            Worksheet workSheet;
            excel = new Application();
            excel.Visible = true;
            workBook = excel.Workbooks.Add();
            workSheet = (Worksheet)excel.ActiveSheet;
            Range rg = null;
            rg = workSheet.get_Range("A1", "A1"); rg.ColumnWidth = 30; // ширина столбцов
            rg = workSheet.get_Range("B1", "B1"); rg.ColumnWidth = 10; // ширина столбцов
            rg = workSheet.get_Range("C1", "C1"); rg.ColumnWidth = 40; // ширина столбцов
            rg = workSheet.get_Range("A2", "A100"); rg.RowHeight = 150; // высота строк
            workSheet.get_Range("A1", "D100").WrapText = true;
            rg = workSheet.get_Range("A1", "D100"); rg.Cells.HorizontalAlignment = XlHAlign.xlHAlignCenter; // выравнивание по центру

            workSheet.Cells[1, "A"] = "Название";
            workSheet.Cells[1, "B"] = "Цена";
            workSheet.Cells[1, "C"] = "Изображение";

            int n = 2;
            foreach(var product in products)
            {
                rg = workSheet.get_Range("A" + n.ToString());
                workSheet.Hyperlinks.Add(rg, product.Url, Type.Missing, "Перейти по ссылке", product.Name);
                workSheet.Cells[n, "B"] = product.Price;
                //workSheet.Cells[n, "C"] = product.UrlImage;
                rg = workSheet.get_Range("C" + n.ToString());
                workSheet.Shapes.AddPicture(product.UrlImage, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoCTrue, rg.Left+0.5, rg.Top+0.5, rg.Width-1,rg.Height-1);
                n++;
            }

            rg = workSheet.get_Range("A1", "C" + (n-1).ToString()); rg.Cells.Borders.LineStyle = XlLineStyle.xlContinuous; // рисуем границы
            rg = workSheet.get_Range("A1", "C1"); rg.Font.Bold = true; // заголовок делаем жирным
            rg = workSheet.get_Range("A1", "C1"); rg.Font.Size = 15; // размер шрифта заголовка

            DateTime dateTime = DateTime.Now;
            string nameFile = "\\Reports\\" + title + " " + dateTime.Day.ToString() + "." + dateTime.Month.ToString() + "." + dateTime.Year.ToString() + " " + dateTime.Hour.ToString() + "-" + dateTime.Minute.ToString() + "-" + dateTime.Second.ToString() + ".xlsx";
            workBook.SaveAs(Environment.CurrentDirectory + nameFile);
            workBook.Close();
            excel.Quit();
        }



        


    }
}
