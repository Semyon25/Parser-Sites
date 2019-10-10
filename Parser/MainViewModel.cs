using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Parser
{
    class MainViewModel : INotifyPropertyChanged
    {
        private RequestParametres req = new RequestParametres();
        private ParseYM parser = new ParseYM();
        private MainModel model = new MainModel();

        public string Request
        {
            get => req.Request;
            set { req.Request = value; OnPropertyChanged(); }
        }

        public int Count
        {
            get => req.Count;
            set { req.Count = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Product> Products
        {
            get => model.Products;
            set { model.Products = value; OnPropertyChanged(); }
        }


        public MainViewModel()
        {
            FindCommand = new RelayCommand(par => Find());
            ReportCommand = new RelayCommand(par => Report());
        }


        private void Report()
        {
            try
            {
                ExcelReport excelReport = new ExcelReport();
                excelReport.CreateReport(Products, req.Request);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Find()
        {
            try
            {
                Products = parser.Find(req);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public RelayCommand FindCommand { get; }
        public RelayCommand ReportCommand { get; }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
