using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using WebMatrix.WebData;

namespace WcfServiceDb
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DataBaseService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DataBaseService.svc or DataBaseService.svc.cs at the Solution Explorer and start debugging.
    public class DataBaseService : IDataBaseService
    {
        //get countries data
        public List<CountryProxy> GetCountries()
        {
            List<Countries> countries;
            countries = DataAcessLoyer.GetCountries();
            List<CountryProxy> countriesproxy = new List<CountryProxy>();
            if (countries != null)
            {
                foreach (var countryDAL in countries)
                {
                    CountryProxy country = new CountryProxy();
                    DataConverter.CountryDALToContryProxy(country, countryDAL);
                    countriesproxy.Add(country);
                }
            }           
            return countriesproxy;
        }
        //get categories data
        public List<CategoryProxy> GetCategories()
        {
            List<Categories> categories;
            categories = DataAcessLoyer.GetCategories();
            List<CategoryProxy> categoriesProxy = new List<CategoryProxy>();
            if (categories != null)
            {
                foreach (var categoryDAL in categories)
                {
                    CategoryProxy category = new CategoryProxy();
                    DataConverter.CategoryDALToCategoryProxy(category, categoryDAL);
                    categoriesProxy.Add(category);
                }
            }         
            return categoriesProxy;
        }
        //get regions data by country
        public List<RegionProxy> GetRegions(int countryId)
        {
            List<Regions> regions;
            regions = DataAcessLoyer.GetRegions(countryId);
            List<RegionProxy> regionsproxy = new List<RegionProxy>();
            if (regions != null)
            {
                foreach (var regionDAL in regions)
                {
                    RegionProxy region = new RegionProxy();
                    DataConverter.RegionDALToRegionProxy(region, regionDAL);
                    regionsproxy.Add(region);
                }
            }
            
            return regionsproxy;
        }
        //get vendors data by country
        public List<VendorProxy> GetVendors(int countryId)
        {
            List<Vendors> vendors;
            vendors = DataAcessLoyer.GetVendors(countryId);
            List<VendorProxy> vendorsproxy = new List<VendorProxy>();
            if (vendors != null)
            {
                foreach (var vendorDAL in vendors)
                {
                    VendorProxy vendor = new VendorProxy();
                    DataConverter.VendorDALToVendorProxy(vendor, vendorDAL);
                    vendorsproxy.Add(vendor);
                }
            }        
            return vendorsproxy;
        }
        //get placemarks data
        public List<PlacemarkProxy> GetPlacemarks(int countryId, int fuelcategoryId)
        {
            List<Placemarks> placemarks;
            placemarks = DataAcessLoyer.GetPlacemarks(countryId, fuelcategoryId);
            List<PlacemarkProxy> placemarksproxy = new List<PlacemarkProxy>();
            if (placemarks != null)
            {
                foreach (var placemarkDAL in placemarks)
                {
                    PlacemarkProxy placemark = new PlacemarkProxy();
                    DataConverter.PlacemarkDALToPlacemarkProxy(placemark, placemarkDAL);
                    placemarksproxy.Add(placemark);
                }
            }           
            return placemarksproxy;
        }
        //get fuelcategory data
        public List<FuelCategoryProxy> GetFuelCategories(int countryId)
        {
            List<FuelCategories> fuelCategories;
            fuelCategories = DataAcessLoyer.GetFuelCategories(countryId);
            List<FuelCategoryProxy> fuelCategoryproxy = new List<FuelCategoryProxy>();
            if (fuelCategories != null)
            {
                foreach (var fuelcategoryDAL in fuelCategories)
                {
                    FuelCategoryProxy fuelcategory = new FuelCategoryProxy();
                    DataConverter.FuelCategoryDALToFuelCategoryProxy(fuelcategory, fuelcategoryDAL);
                    fuelCategoryproxy.Add(fuelcategory);
                }
            }           
            return fuelCategoryproxy;
        }
        //get price data by placemarks
        public List<PriceProxy> GetPrices(int countryId, int fuelcategoryId)
        {
            List<Prices> prices = new List<Prices>();
            prices = DataAcessLoyer.GetPrices(countryId, fuelcategoryId);
            List<PriceProxy> pricesproxy = new List<PriceProxy>();
            foreach (var priceDAL in prices)
            {
                PriceProxy price = new PriceProxy();
                DataConverter.PriceDALToPriceProxy(price, priceDAL);
                pricesproxy.Add(price);
            }
            return pricesproxy;
        }
        public List<PriceProxy> GetPrices(int countryId)
        {
            List<Prices> prices = new List<Prices>();
            prices = DataAcessLoyer.GetPrices(countryId);
            List<PriceProxy> pricesproxy = new List<PriceProxy>();
            foreach (var priceDAL in prices)
            {
                PriceProxy price = new PriceProxy();
                DataConverter.PriceDALToPriceProxy(price, priceDAL);
                pricesproxy.Add(price);
            }
            return pricesproxy;
        }
        //get vendorAliases data vendors of country
        public List<VendorAliasProxy> GetVendorAliases(int countryId)
        {
            List<VendorAliases> vendorAliases = new List<VendorAliases>();
            vendorAliases = DataAcessLoyer.GetVendorAliases(countryId);
            List<VendorAliasProxy> vendorAliasesproxy = new List<VendorAliasProxy>();
            foreach (var vendorAliasDAL in vendorAliases)
            {
                VendorAliasProxy vendorAlias = new VendorAliasProxy();
                DataConverter.VendorAliasDALToVendorAliasProxy(vendorAlias, vendorAliasDAL);
                vendorAliasesproxy.Add(vendorAlias);
            }
            return vendorAliasesproxy;
        }
        //get alias data 
        public  List<AliasProxy> GetAliases()
        {            
            List<Aliases> aliases;
            aliases = DataAcessLoyer.GetAliases();
            List<AliasProxy> aliasesproxy = new List<AliasProxy>();
            if (aliases != null)
            {
                foreach (var aliasDAL in aliases)
                {
                    AliasProxy alias = new AliasProxy();
                    DataConverter.AliasDALToAliasProxy(alias, aliasDAL);
                    aliasesproxy.Add(alias);
                }
            }          
            return aliasesproxy;
        }
        //get carTypes data
        public  List<CarTypeProxy> GetCarTypes()
        {
            List<CarTypes> carTypes;
            carTypes = DataAcessLoyer.GetCarTypes();
            List<CarTypeProxy> carTypesproxy = new List<CarTypeProxy>();
            if (carTypes != null)
            {              
                foreach (var carTypeDAL in carTypes)
                {
                    CarTypeProxy carType = new CarTypeProxy();
                    DataConverter.CarTypeDALToCarTypeProxy(carType, carTypeDAL);
                    carTypesproxy.Add(carType);
                }
            }
            return carTypesproxy;
        }
        //get carBrand data
        public  List<CarBrandProxy> GetCarBrands()
        {
            List<CarBrands> carBrands;
            carBrands = DataAcessLoyer.GetCarBrands();
            List<CarBrandProxy> carBrandsproxy = new List<CarBrandProxy>();
            if (carBrands != null)
            {
                foreach (var carBrandDAL in carBrands)
                {
                    CarBrandProxy carBrand = new CarBrandProxy();
                    DataConverter.CarBrandDALToCarBrandProxy(carBrand, carBrandDAL);
                    carBrandsproxy.Add(carBrand);
                }
            }
            return carBrandsproxy;
        }
        //get carModels data
        public  List<CarModelProxy> GetCarModels()
        {
            List<CarModels> carModels;
            carModels = DataAcessLoyer.GetCarModels();
            List<CarModelProxy> carModelsproxy = new List<CarModelProxy>();
            if (carModels != null)
            {
                foreach (var carModelDAL in carModels)
                {
                    CarModelProxy carModel = new CarModelProxy();
                    DataConverter.CarModelDALToCarModelProxy(carModel, carModelDAL);
                    carModelsproxy.Add(carModel);
                }
            }        
            return carModelsproxy;
        }
        //get carModifications data
        public  List<CarModificationProxy> GetCarModifications()
        {
            List<CarModifications> carModifications;
            carModifications = DataAcessLoyer.GetCarModifications();
            List<CarModificationProxy> carModificationsproxy = new List<CarModificationProxy>();
            if (carModificationsproxy != null)
            {
                foreach (var carModificationDAL in carModifications)
                {
                    CarModificationProxy carModification = new CarModificationProxy();
                    DataConverter.CarModificationDALToCarModificationProxy(carModification, carModificationDAL);
                    carModificationsproxy.Add(carModification);
                }
            }           
            return carModificationsproxy;
        }
        
        //get carNode data
        public  List<CarNodeProxy> GetCarNodes()
        {
            List<CarNode> carNodes;
            carNodes = DataAcessLoyer.GetCarNodes();
            List<CarNodeProxy> carNodesproxy = new List<CarNodeProxy>();
            if (carNodes != null)
            {
                foreach (var carNodeDAL in carNodes)
                {
                    CarNodeProxy carNode = new CarNodeProxy();
                    DataConverter.CarNodeDALToCarNodeProxy(carNode, carNodeDAL);
                    carNodesproxy.Add(carNode);
                }
                
            }
            return carNodesproxy;
        }
        //get carDetails data
        public  List<CarDetailProxy> GetCarDetails()
        {
            List<CarDetails> carDetails;
            carDetails = DataAcessLoyer.GetCarDetails();
            List<CarDetailProxy> carDetailsproxy = new List<CarDetailProxy>();
            if (carDetails != null)
            {
                foreach (var carDetailDAL in carDetails)
                {
                    CarDetailProxy carDetail = new CarDetailProxy();
                    DataConverter.CarDetailDALToCarDetailProxy(carDetail, carDetailDAL);
                    carDetailsproxy.Add(carDetail);
                }
            }           
            return carDetailsproxy;
        }
        //get eventTypes data
        public  List<EventTypeProxy> GetEventTypes()
        {
            List<EventTypes> eventTypes;
            eventTypes = DataAcessLoyer.GetEventTypes();
            List<EventTypeProxy> eventTypesproxy = new List<EventTypeProxy>();
            if (eventTypes != null)
            {
                foreach (var eventTypeDAL in eventTypes)
                {
                    EventTypeProxy eventType = new EventTypeProxy();
                    DataConverter.EventTypeDALToEventTypeProxy(eventType, eventTypeDAL);
                    eventTypesproxy.Add(eventType);
                }
            }           
            return eventTypesproxy;
        }
 
        //get categoryLog data
        public  List<CategoryLogProxy> GetCategoryLogs()
        {
            List<CategoryLog> categoryLogs;
            categoryLogs = DataAcessLoyer.GetCategoryLogs();
            List<CategoryLogProxy> categoryLogsproxy = new List<CategoryLogProxy>();
            if (categoryLogs != null)
            {
                foreach (var categoryLogDAL in categoryLogs)
                {
                    CategoryLogProxy categoryLog = new CategoryLogProxy();
                    DataConverter.CategoryLogDALToCategoryLogProxy(categoryLog, categoryLogDAL);
                    categoryLogsproxy.Add(categoryLog);
                }
            }            
            return categoryLogsproxy;
        }
     

        public List<CarTransmissionProxy> GetCarTransmissions()
        {
            List<CarTransmission> carTransmissions;
            carTransmissions = DataAcessLoyer.GetCarTransmissions();
            List<CarTransmissionProxy> carTransmissionproxy = new List<CarTransmissionProxy>();
            if (carTransmissions != null)
            {
                foreach (var carTransmissionDAL in carTransmissions)
                {
                    CarTransmissionProxy carTransmission = new CarTransmissionProxy();
                    DataConverter.CarTransmissionDALToCarTransmissionProxy(carTransmission, carTransmissionDAL);
                    carTransmissionproxy.Add(carTransmission);
                }
            }         
            return carTransmissionproxy;
        }

        public List<UnitFuelConsumptionProxy> GetUnitFuelConsumption()
        {
            List<UnitFuelConsumption> unitFuelConsumptions;
            unitFuelConsumptions = DataAcessLoyer.GetUnitFuelConsumption();
            List<UnitFuelConsumptionProxy> unitFuelConsumptionProxy = new List<UnitFuelConsumptionProxy>();
            if (unitFuelConsumptions != null)
            {
                for (int i = 0; i < unitFuelConsumptions.Count; i++)
                {
                    UnitFuelConsumptionProxy unitFuelConsumption = new UnitFuelConsumptionProxy();
                    DataConverter.UnitFuelConsumptionDALToUnitFuelConsumptionProxy(unitFuelConsumption, unitFuelConsumptions[i]);
                    unitFuelConsumptionProxy.Add(unitFuelConsumption);
                }
            }
            
            return unitFuelConsumptionProxy;
        }

        public List<UnitDistanceProxy> GetUnitDistance()
        {
            List<UnitDistance> unitDistances;
            unitDistances = DataAcessLoyer.GetUnitDistance();
            List<UnitDistanceProxy> unitDistanceProxy = new List<UnitDistanceProxy>();
            if (unitDistances != null)
            {
                foreach (var unitDistanceDAL in unitDistances)
                {
                    UnitDistanceProxy unitDistance = new UnitDistanceProxy();
                    DataConverter.UnitDistanceDALToUnitDistanceProxy(unitDistance, unitDistanceDAL);
                    unitDistanceProxy.Add(unitDistance);
                }
            }
           
            return unitDistanceProxy;
        }

        public List<UnitVolumeProxy> GetUnitVolume()
        {
            List<UnitVolume> unitVolumes;
            unitVolumes = DataAcessLoyer.GetUnitVolume();
            List<UnitVolumeProxy> unitVolumeProxy = new List<UnitVolumeProxy>();
            if (unitVolumes != null)
            {
                foreach (var unitVolumeDAL in unitVolumes)
                {
                    UnitVolumeProxy unitVolume = new UnitVolumeProxy();
                    DataConverter.UnitVolumeDALToUnitVolumeProxy(unitVolume, unitVolumeDAL);
                    unitVolumeProxy.Add(unitVolume);
                }
            }
            
            return unitVolumeProxy;
        }
        #region SynchronizationByUser
       
        public UserProxy GetUser(string nickName, string userPassword)
        {
            Users user = DataAcessLoyer.GetUser(nickName, userPassword);
            UserProxy userProxy = new UserProxy();
            if (user!=null)
                DataConverter.UserDALToUserProxy(userProxy, user);
            return userProxy;
        }
        public bool IsRegister(string nickName, string userPassword)
        {
            bool flag = DataAcessLoyer.IsRegester(nickName, userPassword);
            if (flag == true)
                return true;
            else
                return false;
        }
        public List<CarProxy> GetCar(int userId)
        {
            List<Cars> cars;
            cars = DataAcessLoyer.GetCar(userId);
            List<CarProxy>carsProxy=new List<CarProxy>();
            if (cars != null)
            {
                foreach (var carDAL in cars)
                {
                    CarProxy car = new CarProxy();
                    DataConverter.CarDALToCarProxy(car, carDAL);
                    carsProxy.Add(car);
                }
            }           
            return carsProxy;
        }
        public List<CarEventProxy> GetCarEvent(int carId)
        {
            List<CarEvents> carEvents;
            carEvents = DataAcessLoyer.GetCarEvent(carId);
            List<CarEventProxy> carEventProxy = new List<CarEventProxy>();
            if (carEvents != null)
            {
                foreach (var carEventDAL in carEvents)
                {
                    CarEventProxy carEvent = new CarEventProxy();
                    DataConverter.CarEventDALToCarEventProxy(carEvent, carEventDAL);
                    carEventProxy.Add(carEvent);
                }
            }
            return carEventProxy;
        }
        public List<LogBookProxy> GetLogBook(int carId)
        {
            List<LogBook> logBooks;
            logBooks = DataAcessLoyer.GetLogBook(carId);
            List<LogBookProxy> logBookProxy = new List<LogBookProxy>();
            if (logBooks != null)
            {
                foreach (var LogBookDAL in logBooks)
                {
                    LogBookProxy logBook = new LogBookProxy();
                    DataConverter.LogBookDALToLogBookProxy(logBook, LogBookDAL);
                    logBookProxy.Add(logBook);
                }
            }
            return logBookProxy;
        }
        #endregion
    }
}
