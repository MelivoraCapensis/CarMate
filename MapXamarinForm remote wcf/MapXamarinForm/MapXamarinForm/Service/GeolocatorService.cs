using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Geolocator.Plugin;
using Geolocator.Plugin.Abstractions;


namespace MapXamarinForm.Service
{
    public class GeolocatorService:INotifyPropertyChanged
    {

        private readonly IGeolocator _locator;
        private int _timeout = 10000;
        private double _latitude = 0;
        private double _longitude = 0;
        private DateTime _timestamp = DateTime.MinValue;

        public GeolocatorService(double accuracy = 50)
        {
            _locator = CrossGeolocator.Current;
            _locator.DesiredAccuracy = accuracy;
        }

        public int Timeout
        {
            get
            {
                return _timeout;
            }
            set
            {
                SetProperty(ref _timeout, value);
            }
        }

        public DateTime Timestamp
        {
            get
            {
                return _timestamp;
            }
            set
            {
                SetProperty(ref _timestamp, value);
            }
        }

        public double Latitude
        {
            get
            {
                return _latitude;
            }
            set
            {
                SetProperty(ref _latitude, value);
            }
        }

        public double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                SetProperty(ref _longitude, value);
            }
        }

        public async Task<bool> GetLocation()
        {
            Position currentpos = null;

            try
            {
                currentpos = await _locator.GetPositionAsync(timeout: _timeout);
              
            }
            catch (Exception) 
            {
                
            }

            if (currentpos != null)
            {
                Latitude = currentpos.Latitude;
                Longitude = currentpos.Longitude;
                Timestamp = currentpos.Timestamp.DateTime;

                return true;
            }
            else
            {
                return false;
            }
        }
           
        public event PropertyChangedEventHandler PropertyChanged;

        bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value)) return false;
            storage = value; OnPropertyChanged(propertyName); return true;
        }
        protected virtual void OnPropertyChanged(string propertyName) { PropertyChangedEventHandler handler = PropertyChanged; if (handler != null)             handler(this, new PropertyChangedEventArgs(propertyName)); } 
    }
}
