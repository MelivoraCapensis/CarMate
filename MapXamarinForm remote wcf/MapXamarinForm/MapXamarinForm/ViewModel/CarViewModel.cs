using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapXamarinForm.ViewModel
{
    public class CarViewModel:INotifyPropertyChanged
    {
        string brand, model, modification, odometr, tank, datebuy, fuelcategory;
        int car_id;
        int serv_fuelcategory_id;
        bool isCheked;
        public event PropertyChangedEventHandler PropertyChanged;
        public string Brand
        {
            get { return brand; }
            set
            {
                if (brand != value)
                {
                    brand = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Brand"));
                    }
                }
            }
        }
        public string Model
        {
            get { return model; }
            set
            {
                if (model != value)
                {
                    model = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Model"));
                    }
                }
            }
        }
        public string Modification
        {
            get { return modification; }
            set
            {
                if (modification != value)
                {
                    modification = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Modification"));
                    }
                }
            }
        }
        public string Odometr
        {
            get { return odometr; }
            set
            {
                if (odometr != value)
                {
                    odometr = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Odometr"));
                    }
                }
            }
        }
        public string Tank
        {
            get { return tank; }
            set
            {
                if (tank != value)
                {
                    tank = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Tank"));
                    }
                }
            }
        }
        public string Datebuy
        {
            get { return datebuy; }
            set
            {
                if (datebuy != value)
                {
                    datebuy = value;                   
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Datebuy"));
                    }
                }
            }
        }
        public string Fuelcategory
        {
            get { return fuelcategory; }
            set
            {
                if (fuelcategory != value)
                {
                    fuelcategory = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Fuelcategory"));
                    }
                }
            }
        }
        public int Car_id
        {
            get { return car_id; }
            set
            {
                if (car_id != value)
                {
                    car_id = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Car_id"));
                    }
                }
            }
        }
        public int Serv_fuelcategory_id
        {
            get { return serv_fuelcategory_id; }
            set
            {
                if (serv_fuelcategory_id != value)
                {
                    serv_fuelcategory_id = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Serv_fuelcategory_id"));
                    }
                }
            }
        }
        public bool IsCheked
        {
            get { return isCheked; }
            set
            {
                if (isCheked != value)
                {
                    isCheked = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsCheked"));
                    }
                }
            }
        }
    }
}
