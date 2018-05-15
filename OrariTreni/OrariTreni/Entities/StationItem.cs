using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace OrariTreni.Entities
{
    class StationItem : INotifyPropertyChanged
    {
        string stationName;
        public event PropertyChangedEventHandler PropertyChanged;

        public string StationName
        {
            get
            {
                return stationName;
            }
            set
            {
                if (stationName != value)
                {
                    stationName = value;
                    OnPropertyChanged("StationName");
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
