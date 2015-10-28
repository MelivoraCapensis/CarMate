using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WcfServiceDb
{
    public static class DataAcessLoyer
    {
        //get countries data
        public static List<Countries> GetCountries()
        {
            List<Countries> countries;
            try 
            {
                using (CarMateEntities db = new CarMateEntities())
                {
                    countries = db.Countries.Select(x => x).ToList();
                }
                return countries;
            }
            catch (Exception)
            {
                return null;
            }
           
        }
        //get categories data
        public static List<Categories> GetCategories()
        {
            List<Categories> categories;
            try {
                using (CarMateEntities db = new CarMateEntities())
                {
                    categories = db.Categories.Select(x => x).ToList();
                }
                return categories;
            }
            catch (Exception)
            {
                return null;
            }
          
        }
        //get regions data by country
        public static List<Regions> GetRegions(int countryId)
        {

            List<Regions> regions;
            try {
                using (CarMateEntities db = new CarMateEntities())
                {
                    regions = db.Regions
                        .Where(x => x.CountryId == countryId)
                        .Select(x => x)
                        .ToList();
                }
                return regions;
            }
            catch (Exception)
            {
                return null;
            }          
        }
        //get vendors data by country
        public static List<Vendors> GetVendors(int countryId)
        {
            List<Vendors> vendors ;
            try {
                using (CarMateEntities db = new CarMateEntities())
                {
                    vendors = db.Vendors
                        .Where(x => x.countryId == countryId)
                        .Select(x => x).ToList();
                }
                return vendors;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        //get placemarks data
        public static List<Placemarks> GetPlacemarks(int countryId,int fuelcategoryId)
        {
            List<Placemarks> placemarks;
            try {
                using (CarMateEntities db = new CarMateEntities())
                {
                    placemarks = db.Placemarks
                        .Where(x => x.CountryId == countryId)
                        .Select(x => x).ToList();
                }
                return placemarks;
            
            }
            catch (Exception)
            {
                return null;
            }           
        }
        //get fuelcategory data
        public static List<FuelCategories> GetFuelCategories(int countryId)
        {
            List<FuelCategories> fuelCategories;
            try {
                using (CarMateEntities db = new CarMateEntities())
                {
                    fuelCategories = db.FuelCategories
                        .Where(x => x.CountryId == countryId)
                        .Select(x => x).ToList();
                }
                return fuelCategories;
            
            }
            catch (Exception)
            {
                return null;
            }
          
        }
        public static List<Prices> GetPrices(int countryId)
        {
            List<Prices> prices ;           
            using (CarMateEntities db = new CarMateEntities())
            {
                prices = db.Prices.Include("Regions")
                    .Where(x => x.Regions.CountryId == countryId)
                    .Select(x => x).ToList();
            }
            return prices;
        }
        //get price data by placemarks
        public static List<Prices> GetPrices(int countryId, int fuelcategoryId)
        {
            List<Prices> prices = new List<Prices>();
            List<Placemarks> placemarks = new List<Placemarks>();
            List<Prices> placemarkPrices;
            using (CarMateEntities db = new CarMateEntities())
            {
                placemarks = GetPlacemarks(countryId, fuelcategoryId);
                foreach (var v in placemarks)
                {
                    placemarkPrices = db.Prices.Where(x => x.PlacemarkId == v.Id && x.FuelId==fuelcategoryId).Select(x => x).ToList();
                    if (placemarkPrices != null)
                    {
                        prices.AddRange(placemarkPrices);
                    }                  
                }               
            }
            return prices;
        }
       
        //get vendorAliases data vendors of country
        public static List<VendorAliases> GetVendorAliases(int countryId)
        {
            List<VendorAliases> vendorAliases = new List<VendorAliases>();
            List<Vendors> vendors = new List<Vendors>();
            List<VendorAliases> vendorAliasesbyVendor;
            using (CarMateEntities db = new CarMateEntities())
            {
                vendors = GetVendors(countryId);
                foreach (var v in vendors)
                {
                    vendorAliasesbyVendor = db.VendorAliases.Where(x=>x.VendorId==v.id).Select(x => x).ToList();
                    if (vendorAliasesbyVendor != null)
                    {
                        vendorAliases.AddRange(vendorAliasesbyVendor);
                    }
                }
                
            }
            return vendorAliases;
        }
        //get aliases data
        public static List<Aliases> GetAliases()
        {
            try {

                List<Aliases> aliases;
                using (CarMateEntities db = new CarMateEntities())
                {
                    aliases = db.Aliases.Select(x => x).ToList();
                }
                return aliases;
            }
            catch (Exception)
            {
                return null;
            }
           
        }
        //get carTypes data
        public static List<CarTypes> GetCarTypes()
        {
            try 
            {
                  
                List<CarTypes> carTypes;
                using (CarMateEntities db = new CarMateEntities())
                {
                    carTypes = db.CarTypes.Select(x => x).ToList();
                }
                return carTypes;
            }
            catch (Exception)
            {
                return null;
            }
          
        }
         //get carBrand data
        public static List<CarBrands> GetCarBrands()
        {
            try 
            {
                List<CarBrands> carBrands;
                using (CarMateEntities db = new CarMateEntities())
                {
                    carBrands = db.CarBrands.Select(x => x).ToList();
                }
                return carBrands;
            }
            catch (Exception)
            {
                return null;
            }
           
        }
        //get carModels data
        public static List<CarModels> GetCarModels()
        {
            try 
            {
                List<CarModels> carModels;
                using (CarMateEntities db = new CarMateEntities())
                {
                    carModels = db.CarModels.Select(x => x).ToList();
                }
                return carModels;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        //get carModifications data
        public static List<CarModifications> GetCarModifications()
        {
            try {
                List<CarModifications> carModifications;
                using (CarMateEntities db = new CarMateEntities())
                {
                    carModifications = db.CarModifications.Select(x => x).ToList();
                }
                return carModifications;
            }
            catch (Exception)
            {
                return null;
            }
           
        }
       
        //get carNode data
        public static List<CarNode> GetCarNodes()
        {
            try {
                List<CarNode> carNodes;
                using (CarMateEntities db = new CarMateEntities())
                {
                    carNodes = db.CarNode.Select(x => x).ToList();
                }
                return carNodes;
            
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        //get carDetails data
        public static List<CarDetails> GetCarDetails()
        {
            try {
                List<CarDetails> carDetails;
                using (CarMateEntities db = new CarMateEntities())
                {
                    carDetails = db.CarDetails.Select(x => x).ToList();
                }
                return carDetails;
            
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        //get eventTypes data
        public static List<EventTypes> GetEventTypes()
        {
            try {
                List<EventTypes> eventTypes;
                using (CarMateEntities db = new CarMateEntities())
                {
                    eventTypes = db.EventTypes.Select(x => x).ToList();
                }
                return eventTypes;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
      
        //get categoryLog data
        public static List<CategoryLog> GetCategoryLogs()
        {
            try {
                List<CategoryLog> categoryLogs = new List<CategoryLog>();
                using (CarMateEntities db = new CarMateEntities())
                {
                    categoryLogs = db.CategoryLog.Select(x => x).ToList();
                }
                return categoryLogs;
            
            }
            catch (Exception)
            {
                return null;
            }
           
        }
      
        //get cartransmission data
        public static List<CarTransmission> GetCarTransmissions()
        {
            try {
                List<CarTransmission> carTransmission;
                using (CarMateEntities db = new CarMateEntities())
                {
                    carTransmission = db.CarTransmission.Select(x => x).ToList();

                }
                return carTransmission;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        //get unitfuelconsumption data
        public static List<UnitFuelConsumption> GetUnitFuelConsumption()
        {
            try {
                List<UnitFuelConsumption> unitFuelConsumption ;
                using (CarMateEntities db = new CarMateEntities())
                {
                    unitFuelConsumption = db.UnitFuelConsumption.Select(x => x).ToList();
                }
                return unitFuelConsumption;
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        //get unitdistance data
        public static List<UnitDistance> GetUnitDistance()
        {
            try {
                List<UnitDistance> unitDistance;
                using (CarMateEntities db = new CarMateEntities())
                {
                    unitDistance = db.UnitDistance.Select(x => x).ToList();
                }
                return unitDistance;
            
            }
            catch (Exception) 
            {
                return null;
            }          
        }
        //get unitvolume 
        public static List<UnitVolume> GetUnitVolume()
        {
            try {
                List<UnitVolume> unitVolume = new List<UnitVolume>();
                using (CarMateEntities db = new CarMateEntities())
                {
                    unitVolume = db.UnitVolume.Select(x => x).ToList();
                }
                return unitVolume;
            
            }
            catch (Exception)
            {
                return null;
            }
            
        }
        //get userdata
        public static Users GetUser(string nickName, string userPassword)
        {
            Users user;
            try 
            {              
                using (CarMateEntities db = new CarMateEntities())
                {
                    user = db.Users.Where(x => x.Nickname == nickName && x.UserPassword == userPassword).Select(x => x).FirstOrDefault();
                }
                return user;           
            }
            catch (Exception)
            {
                return null;
            }           
        }
        public static bool IsRegester(string nickName, string userPassword)
        {
            try { 
                using(CarMateEntities db=new CarMateEntities())
                {
                    Users user = db.Users.Where(x => x.Nickname == nickName && x.UserPassword == userPassword).Select(x => x).FirstOrDefault();
                    if(user!=null)
                        return true;
                    else
                    return false;
                }
            }
            catch (Exception)
            {
                    return false;
            }
        }
        public static List<Cars> GetCar(int userId)
        {
            
            try 
            {
                List<Cars> cars;
                using(CarMateEntities db=new CarMateEntities())
                {
                    cars = db.Cars.Where(x => x.UserId == userId).Select(x => x).ToList();
                }
                return cars;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static List<CarEvents> GetCarEvent(int carId)
        {
            List<CarEvents> carEvents;
            try
            {
                using (CarMateEntities db = new CarMateEntities())
                {
                    carEvents = db.CarEvents.Where(x => x.CarId == carId).Select(x => x).ToList();
                }
                return carEvents;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static List<LogBook> GetLogBook(int carId)
        {
            List<LogBook> logBooks;
            try {
                using (CarMateEntities db = new CarMateEntities())
                {
                    logBooks = db.LogBook.Where(x => x.CarId==carId).Select(x => x).ToList();
                }
                return logBooks;
            
            }
            catch (Exception)
            {
                return null;
            }
        }
        
    }
}