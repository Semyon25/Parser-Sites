using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    class MainModel : INotifyPropertyChanged
    {

        ObservableCollection<Product> products;
        public ObservableCollection<Product> Products
        {
            get => products;
            set { products = value; OnPropertyChanged(); }
        }


        public MainModel()
        {

        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
