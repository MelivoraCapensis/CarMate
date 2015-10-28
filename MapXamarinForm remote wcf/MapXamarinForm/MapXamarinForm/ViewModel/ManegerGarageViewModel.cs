using MapXamarinForm.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapXamarinForm.ViewModel
{
    public class ManegerGarageViewModel:INotifyPropertyChanged
    {
        Dictionary<string, int> typeToCar = new Dictionary<string, int>();
        Dictionary<string, int> modelToCar = new Dictionary<string, int>();
        Dictionary<string, int> brandToCar = new Dictionary<string, int>();
        Dictionary<string, int> modificationToCar = new Dictionary<string, int>();
        Dictionary<string, int> fuelcategoryToCar = new Dictionary<string, int>();
        string carOdometr;
        string carTank;
        int cartypeId;
        int modelId;
        int brandId;
        int modificationId;
        int fuelcategoryId;
        DateTime datebuy;
        public event PropertyChangedEventHandler PropertyChanged;
        public string CarOdometr
        {
            get { return carOdometr; }
            set
            {
                if (carOdometr != value)
                {
                    carOdometr = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CarOdometr"));
                    }
                }
            }
        }
        public string CarTank
        {
            get { return carTank; }
            set
            {
                if (carTank != value)
                {
                    carTank = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CarOdometr"));
                    }
                }
            }
        }
        public int ModelId
        {
            get { return modelId; }
            set
            {
                if (modelId != value)
                {
                    modelId = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ModelId"));
                    }
                }
            }
        }
        public int CartypeId
        {
            get { return cartypeId; }
            set
            {
                if (cartypeId != value)
                {
                    cartypeId = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("CartypeId"));
                    }
                }
            }
        }
        public int BrandId
        {
            get { return brandId; }
            set
            {
                if (brandId != value)
                {
                    brandId = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("BrandId"));
                    }
                }
            }
        }
        public int ModificationId
        {
            get { return modificationId; }
            set
            {
                if (modificationId != value)
                {
                    modificationId = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("BrandId"));
                    }
                }
            }
        }
        public int FuelcategoryId
        {
            get { return fuelcategoryId; }
            set
            {
                if (fuelcategoryId != value)
                {
                    fuelcategoryId = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("BrandId"));
                    }
                }
            }
        }
        public DateTime Datebuy
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
        public Dictionary<string, int> TypeToCar
        {
            get { return typeToCar; }
            set
            {
                if (typeToCar != value)
                {
                    typeToCar = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("TypeToCar"));
                    }
                }
            }
        }
        public Dictionary<string, int> FuelcategoryToCar
        {
            get { return fuelcategoryToCar; }
            set
            {
                if (fuelcategoryToCar != value)
                {
                    fuelcategoryToCar = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("FuelcategoryToCar"));
                    }
                }
            }
        }
        public Dictionary<string, int> BrandToCar
        {
            get { return brandToCar; }
            set
            {
                if (brandToCar != value)
                {
                    brandToCar = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("BrandToCar"));
                    }
                }
            }
        }
        public Dictionary<string, int> ModelToCar
        {
            get { return modelToCar; }
            set
            {
                if (modelToCar != value)
                {
                    modelToCar = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("BrandToCar"));
                    }
                }
            }
        }
        public Dictionary<string, int> ModificationToCar
        {
            get { return modificationToCar; }
            set
            {
                if (modificationToCar != value)
                {
                    modificationToCar = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("ModificationToCar"));
                    }
                }
            }
        }

        public async Task InitFuelCategoryToCar()
        {
            List<FuelCategory> fuel_categories = await App.Database.GetFuelCategoryByCountry(1);
            if (fuel_categories != null)
            {
                foreach (var fc in fuel_categories)
                {
                   
                    this.FuelcategoryToCar.Add(fc.FuelCategoryName, fc.FuelCategoryId);
                }
            }
        }
        public async Task InitTypeCar()
        {
            List<CarType> type_cars = await App.Database.GetCarTypes();
            if (type_cars != null)
            {
                foreach (var tc in type_cars)
                {
                    this.TypeToCar.Add(tc.CarTypeName, tc.CarTypeId);
                }
            }
        }
        public async Task InitBrandToCar(int servCarTypeId)
        {
            List<CarBrand> car_brands = await App.Database.GetListBrandCar(servCarTypeId);
            if (car_brands != null)
            {
                this.BrandToCar.Clear();
                foreach (var cb in car_brands)
                {
                    this.BrandToCar.Add(cb.Brand, cb.CarBrandId);
                }
            }
        }
        public async Task InitModelToCar(int servBrandId)
        {
            List<CarModel> car_models = await App.Database.GetListModelCar(servBrandId);
            if (car_models != null)
            {
                this.ModelToCar.Clear();
                foreach (var cm in car_models)
                {
                    this.ModelToCar.Add(cm.model, cm.CarModelId);
                }
            }
        }
        public async Task InitModificationToCar(int servModificationId)
        {
            List<CarModification> car_models = await App.Database.GetListModificationCar(servModificationId);
            if (car_models != null)
            {
                this.ModificationToCar.Clear();
                foreach (var cm in car_models)
                {
                    this.ModificationToCar.Add(cm.Modification, cm.CarModificationId);
                }
            }
        }
        public async Task<bool> ExecuteSaveCarCommand()
        {
            int serv_brandId = this.BrandId;
            int serv_carModelId = this.ModelId;
            int serv_carModificationId = this.ModificationId;
            int serv_fuelCategoryId = this.FuelcategoryId;
            int odometr,tank;
            Int32.TryParse(this.CarOdometr, out odometr);
            Int32.TryParse(this.CarTank, out tank);
            int userId = App.UserViewModel.Local_user_id;
            Car new_car = new Car 
            {
                ServFuelCategoryId = serv_fuelCategoryId,
                ServModelId = serv_carModelId,
                ServModificationId = serv_carModificationId,         
                Odometer = odometr,
                Tank = tank,
                DateBuy=this.Datebuy,
                DateCreate=DateTime.Now,
                State=1
            };
            var task1 = await App.Database.InsertCar(new_car);

            if (task1 == true)
            {
                var task2 = await App.Database.UpdateUserByCar(userId, new_car);
                if (task2 == true)
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

    }
}
