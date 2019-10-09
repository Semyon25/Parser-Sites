using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    class RequestParametres : INotifyPropertyChanged
    {
        private string request;
        private int count;

        public string Request
        {
            get { return request; }
            set { request = value; OnPropertyChanged(); }
        }
        public int Count
        {
            get { return count; }
            set { count = value; OnPropertyChanged(); }
        }



        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
