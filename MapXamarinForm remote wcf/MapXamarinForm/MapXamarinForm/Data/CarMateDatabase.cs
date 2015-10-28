using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;
using MapXamarinForm.Models;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensions.Attributes;
using SQLiteNetExtensionsAsync.Extensions;
using SQLite.Net;
using SQLite.Net.Async;
using WcfServiceDb;
using System.Threading;
using SQLite.Net.Interop;

 
namespace MapXamarinForm.Data
{
    public class CarMateDatabase
    {
        
        SQLiteAsyncConnection database;
        //static SemaphoreSlim mutex = new SemaphoreSlim(1,1);

        public  CarMateDatabase(ISQLitePlatform sqlitePlatform,string dbPath)
        {

            //initialize a new SQLiteAsyncConnection
            if (database == null)
            {
                var connectionFunc = new Func<SQLiteConnectionWithLock>(() =>
                                   new SQLiteConnectionWithLock
                                   (
                                       sqlitePlatform,
                                       new SQLiteConnectionString(dbPath, storeDateTimeAsTicks: false)
                                   ));


                database = new SQLiteAsyncConnection(connectionFunc);

                database.CreateTableAsync<Category>().Wait();
                database.CreateTableAsync<Country>().Wait();
                database.CreateTableAsync<Region>().Wait();
                database.CreateTableAsync<Vendor>().Wait();
                database.CreateTableAsync<Placemark>().Wait();
                database.CreateTableAsync<FuelCategory>().Wait();
                database.CreateTableAsync<Price>().Wait();
                database.CreateTableAsync<User>().Wait();
                database.CreateTableAsync<UnitFuelConsumption>().Wait();
                database.CreateTableAsync<UnitDistance>().Wait();
                database.CreateTableAsync<UnitVolume>().Wait();
                database.CreateTableAsync<Alias>().Wait();
                database.CreateTableAsync<VendorAlias>().Wait();
                database.CreateTableAsync<CarType>().Wait();
                database.CreateTableAsync<CarBrand>().Wait();
                database.CreateTableAsync<CarModel>().Wait();
                database.CreateTableAsync<CarModification>().Wait();
                database.CreateTableAsync<Car>().Wait();
                database.CreateTableAsync<CarTransmission>().Wait();
                database.CreateTableAsync<CarNode>().Wait();
                database.CreateTableAsync<CarDetail>().Wait();
                database.CreateTableAsync<EventType>().Wait();
                database.CreateTableAsync<CarEvent>().Wait();
                database.CreateTableAsync<CategoryLog>().Wait();
                database.CreateTableAsync<LogBook>().Wait();

            }

            

        }
        #region Insert
        public async Task<bool> InsertCar(Car addcar)
        {
            try {
                await database.RunInTransactionAsync((tran) => 
                {
                    tran.Insert(addcar);
                });
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        public async Task<bool> InsertUser(User adduser)
        {
            try
            {
                await database.RunInTransactionAsync((tran) =>
                {
                    tran.Insert(adduser);
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }




        #endregion
        #region Update
        public async Task<bool> UpdateUser(User update_user)
        {
            int userId = update_user.Id;
            User exist_user;
            List<Car> lcar = new List<Car>();
            try
            {
                exist_user = await (from c in database.Table<User>()
                                     where c.Id == userId
                                     select c).FirstOrDefaultAsync();
                exist_user.FirstName = update_user.FirstName;
                exist_user.LastName = update_user.LastName;
                exist_user.UserEmail = update_user.UserEmail;
                exist_user.UserPassword = update_user.UserPassword;
                lcar = await (from c in database.Table<Car>()
                              where c.UserId == update_user.Id
                              select c).ToListAsync();
                update_user.Cars = lcar;
                
                await database.RunInTransactionAsync(tran =>
                {
                    tran.UpdateWithChildren(update_user);
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
        #region SynchronizationByCountry
        public async Task<bool> InsertAllPrices(List<Price> lprice)
        {

            try
            {
                await database.RunInTransactionAsync((tran) =>
                    {
                       
                        tran.InsertAll(lprice);
                       
                    });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
     
        public async Task<bool> InsertAllPlacemarks(List<Placemark> lplacemark)
        {
            
            try
            {
               await database.RunInTransactionAsync(tran => {
                
                 tran.InsertAll(lplacemark);
               
                });
               
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
      
        public async Task<bool> InsertAllRegions(List<Region> lregion)
        {
            
            try
            {
                await database.RunInTransactionAsync(tran =>
                {
                   
                    tran.InsertAll(lregion);
                    
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> InsertAllVendors(List<Vendor> lvendor)
        {

            try
            {
                await database.RunInTransactionAsync((tran) =>
                   {
                       
                       tran.InsertAll(lvendor);
                     
                   });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
       
        public async Task<bool> InsertAllVendorAlias(List<VendorAlias> lvendoralias)
        {

            try
            {
                await database.RunInTransactionAsync((tran) =>
                   {
                       
                       tran.InsertAll(lvendoralias);
                       
                   });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
       
        public async Task<bool> InsertAllFuelCategories(List<FuelCategory> lfuelCategory)
        {

            try
            {
                await database.RunInTransactionAsync((tran) =>
                   {
                       
                       tran.InsertAll(lfuelCategory);
                      
                   });
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
       
        #endregion
        #region SynchronizationByManual
       
        public async Task<bool> InsertAllCategories(List<Category> lcategory)
        {
            int result = 0;
            try
            {
                result = await database.InsertAllAsync(lcategory);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
      
        public async Task<bool> InsertAllAliases(List<Alias> lalias)
        {
            int result = 0;
            try
            {
                result = await database.InsertAllAsync(lalias);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> InsertAllCarTypes(List<CarType> lcarType)
        {
            int result = 0;
            try
            {
                result = await database.InsertAllAsync(lcarType);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> InsertAllCarBrands(List<CarBrand> lcarBrand)
        {
            int result = 0;
            try
            {
                result = await database.InsertAllAsync(lcarBrand);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> InsertAllCarModels(List<CarModel> lcarModel)
        {
            int result = 0;
            try
            {
                result = await database.InsertAllAsync(lcarModel);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> InsertAllCarModifications(List<CarModification> lcarModification)
        {
            int result = 0;
            try
            {
                result = await database.InsertAllAsync(lcarModification);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> InsertAllCarNodes(List<CarNode> lcarNode)
        {
            int result = 0;
            try
            {
                result = await database.InsertAllAsync(lcarNode);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> InsertAllEventTypes(List<EventType> leventType)
        {
            int result = 0;
            try
            {
                result = await database.InsertAllAsync(leventType);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> InsertAllCarDetails(List<CarDetail> lcarDetail)
        {
            int result = 0;
            try
            {
                result = await database.InsertAllAsync(lcarDetail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> InsertAllCategoryLogs(List<CategoryLog> lcategoryLog)
        {
            int result = 0;
            try
            {
                result = await database.InsertAllAsync(lcategoryLog);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> InsertAllCarTransmission(List<CarTransmission> lсarTransmission)
        {
            int result = 0;
            try
            {
                result = await database.InsertAllAsync(lсarTransmission);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> InsertAllUnitFuelConsumption(List<UnitFuelConsumption> lunitFuelConsumption)
        {
            int result = 0;
            try
            {
                result = await database.InsertAllAsync(lunitFuelConsumption);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> InsertAllUnitDistance(List<UnitDistance> lunitDistance)
        {
            int result = 0;
            try
            {
                result = await database.InsertAllAsync(lunitDistance);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> InsertAllUnitVolume(List<UnitVolume> lunitVolume)
        {
            int result = 0;
            try
            {
                result = await database.InsertAllAsync(lunitVolume);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        #region SynchronizationByUser
        public async Task<bool> InsertAllCars(List<Car> lcar)
        {
            int result = 0;
            try
            {
                result = await database.InsertAllAsync(lcar);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> InsertAllCarEvents(List<CarEvent> lcarEvent)
        {
            int result = 0;
            try
            {
                result = await database.InsertAllAsync(lcarEvent);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> InsertAllCarLogBook(List<LogBook> llogBook)
        {
            int result = 0;
            try
            {
                result = await database.InsertAllAsync(llogBook);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
        #region Add from Wcf
        public async Task<User> AddNewUserAsync(UserProxy proxy)
        {
            int result = 0;
            User userLdb;
          
            try 
            {
                userLdb = new User 
                {                
                    FirstName=proxy.FirstName,
                    LastName=proxy.LastName,
                    UserEmail=proxy.UserEmail,
                    UserPassword=proxy.UserPassword,
                    Nickname=proxy.Nickname,
                    ImgPath=proxy.ImgPath,
                    DateRegister=proxy.DateRegister,
                    DateCreate=proxy.DateCreate,
                    State=1,
                    UserId=proxy.UserId,
                    ServRegionId=proxy.RegionId,
                    ServUnitDistanceId=proxy.UnitDistanceId,
                    ServUnitVolumeId=proxy.UnitVolumeId,
                    ServUnitFuelConsumptionId=proxy.UnitFuelConsumptionId

                };
                result = await database.InsertAsync(userLdb);
                int userServId = userLdb.UserId;
                int userLdbId = userLdb.Id;
                userLdb = await database.GetAsync<User>(userLdbId);
                if (userServId != 0)
                {
                    List<Car> carsLdb = await (from c in database.Table<Car>()
                                               where c.ServUserId == userServId
                                               select c).ToListAsync();
                    List<Car> lcar_with_children = new List<Car>();
                    foreach (var car in carsLdb)
                    {
                        var car_with_children = await database.GetWithChildrenAsync<Car>(car.Id, recursive: true)
                            .ConfigureAwait(false);
                        lcar_with_children.Add(car_with_children);
                    }
                    userLdb.Cars = lcar_with_children;
                    await database.RunInTransactionAsync(tran => {
                        tran.UpdateWithChildren(userLdb);
                    });
                    
                    
                }
                return userLdb;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<Country> AddNewCountryAsync(CountryProxy proxy)
        {
            int result = 0;
            Country countryLdb;
          
            try 
            {
                countryLdb = new Country
                    {
                        CountryName = proxy.CountryName,
                        DateCreate = proxy.DateCreate,
                        State = 0,
                        CountryId = proxy.CountryId,

                    };
                result = await database.InsertAsync(countryLdb);


                //int countryServId = countryLdb.CountryId;
                //int countryLdbId = countryLdb.Id;


                //List<Region> regionsLdb = await (from r in database.Table<Region>()
                //                                 where r.ServCountryId == countryServId
                //                                    select r).ToListAsync();
                //List<Placemark> placemarksLdb = await (from p in database.Table<Placemark>()
                //                                       where p.ServCountryId == countryServId
                //                                       select p).ToListAsync();
                //List<Vendor> vendorLdb = await (from v in database.Table<Vendor>()
                //                                     where v.ServCountryId == countryServId
                //                                          select v).ToListAsync();
                //List<FuelCategory>fuelcategoryLdb = await (from f in database.Table<FuelCategory>()
                //                                     where f.ServCountryId == countryServId
                //                                     select f).ToListAsync();
              
                //countryLdb.Placemarks = placemarksLdb;
                //countryLdb.Regions = regionsLdb;
                //countryLdb.Vendors = vendorLdb;
                //countryLdb.FuelCategories = fuelcategoryLdb;
                //await database.RunInTransactionAsync(tran =>
                //{
                //    tran.UpdateWithChildren(countryLdb);
                //});
                //await database.UpdateWithChildrenAsync(countryLdb);

                return countryLdb;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task AddNewCar(Car car)
        {
            try
            {
                int serv_CarId = car.CarId;
                await database.InsertAsync(car);
                int local_car_id = car.Id;
                car = await database.GetAsync<Car>(local_car_id);
                List<CarEvent> lcarEvent = await (from c in database.Table<CarEvent>()
                                                  where c.ServCarId == serv_CarId      
                                                  select c).ToListAsync();
                car.CarEvents = lcarEvent;
                await database.UpdateWithChildrenAsync(car);
            }
            catch (Exception)
            { }
        }

        #endregion
        public async Task AddNewCarEvent(CarEvent carevent)
        {
            try{
                int carId = carevent.CarId;
                await database.InsertAsync(carevent);
                var car = await database.GetWithChildrenAsync<Car>(carId, recursive: true)
                    .ConfigureAwait(false);
                if(car.CarEvents!=null)
                {
                    car.CarEvents.Add(carevent);
                    await database.UpdateWithChildrenAsync(car);
                }
            }
            catch(Exception)
            {}
            
            
        }
        #region Update With Children
        //init foreign key CarId Car:Car CarEvent
        //init foreign key CarId Car:Car CarDetail
        public async Task<bool> UpdateCarAsync(int servCarId)
        {
            List<CarEvent> carEvents = new List<CarEvent>();
            List<CarDetail> carDetails = new List<CarDetail>();
            Car updateCar = new Car();
            try {
                
                carEvents = await (from ce in database.Table<CarEvent>()
                                   where ce.ServCarId == servCarId
                                   select ce).ToListAsync();
                carDetails = await (from ce in database.Table<CarDetail>()
                                    where ce.ServCarId == servCarId
                                    select ce).ToListAsync();

                updateCar = await (from c in database.Table<Car>()
                                   where c.CarId == servCarId
                                   select c).FirstOrDefaultAsync();

                updateCar.CarEvents = carEvents;
                updateCar.CarDetails = carDetails;
                
                await database.RunInTransactionAsync(tran =>
                {
                    tran.UpdateWithChildren(updateCar);
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> UpdateUserByCar(int userId,Car add_car)
        {
            User update_user = new User();
            List<Car> lcar = new List<Car>();
            try
            {
                update_user = await (from c in database.Table<User>()
                                     where c.Id == userId
                                     select c).FirstOrDefaultAsync();
                lcar = await (from c in database.Table<Car>()
                              where c.UserId == update_user.Id
                              select c).ToListAsync();
                update_user.Cars = lcar;
                update_user.Cars.Add(add_car);
                await database.RunInTransactionAsync(tran =>
                {
                    tran.UpdateWithChildren(update_user);
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
        
        #region Add from Local db
       
  
        public async Task<List<FuelCategory>> GetFuelCategoryByCountry(int countryId)
        {
            List<FuelCategory> fuelcategories;
            try 
            {
                fuelcategories = await (from f in database.Table<FuelCategory>()
                                        where f.ServCountryId == countryId
                                        select f).ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
            return fuelcategories;
        }
     
       
        public async Task<List<EventType>> GetEventTypes()
        {
            List<EventType> event_types;
            try {
                event_types = await (database.Table<EventType>()).ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
            return event_types;
        }
        public async Task<List<CarType>> GetCarTypes()
        {
            List<CarType> car_types;
            try
            {
                car_types = await (database.Table<CarType>()).ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
            return car_types;
        }
        public async Task<List<Car>>GetCarByServUserId(int userId)
        {
            List<Car> cars;
            try{
                cars = await (from c in database.Table<Car>()
                              where c.ServUserId == userId
                              select c).ToListAsync();

            }
            catch(Exception)
            {
                return null;
            }
            return cars;
        }
        public async Task<List<Car>> GetCarByLocalUserId(int userId)
        {
            List<Car> cars;
            try
            {
                cars = await (from c in database.Table<Car>()
                              where c.UserId == userId
                              select c).ToListAsync();

            }
            catch (Exception)
            {
                return null;
            }
            return cars;
        }
        public async Task<CarModel> GetCarModel(int modelId)
        {
            try {
                var ldbModel = await (from c in database.Table<CarModel>()
                                      where c.CarModelId == modelId
                                      select c).FirstOrDefaultAsync();
                return ldbModel;
            }
            catch (Exception)
            {
                return null;
            }
           
        }
        public async Task<CarModification> GetCarModification(int carModificationId)
        {
            try
            {
                var ldbModification = await (from c in database.Table<CarModification>()
                                      where c.CarModificationId == carModificationId
                                      select c).FirstOrDefaultAsync();
                return ldbModification;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public async Task<FuelCategory> GetFuelCategory(int fuelCategoryId)
        {
            try
            {
                var ldbFuelcategory = await (from c in database.Table<FuelCategory>()
                                             where c.FuelCategoryId == fuelCategoryId
                                             select c).FirstOrDefaultAsync();
                return ldbFuelcategory;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public async Task<CarBrand> GetCarBrand(int brandId)
        {
            try {
                var ldbBrand = await (from b in database.Table<CarBrand>()
                                      where b.CarBrandId == brandId
                                      select b).FirstOrDefaultAsync();
                return ldbBrand;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
        
        #region Get from Local db With Children
    
     
       
    
        public async Task<User> GetUserWithChildren()
        {
            try
            {
                // await semaphore.WaitAsync();
                
                try
                {



                    var task1 = await database.Table<User>().FirstAsync();
                    //var res1 = await task1;

                        ////int id = temp.Id;
                        ////var userWithChildren = await database
                        ////    .GetWithChildrenAsync<User>(id, recursive: true)
                        ////    .ConfigureAwait(false);
                        ////return userWithChildren;
                    //return res1;
                    return task1;
                        
                   
                }
                finally
                {
                    //semaphore.Release();
                    
                }

            }
            catch (SQLiteException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<Placemark>> GetPlacemark()
        {
            try
            {
                var lplacemark = await database.Table<Placemark>().ToListAsync();
                   
                return lplacemark;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<Price>> GetPriceByPlacemark(int servPlacemarkId)
        {
            try {
                var pricePlacemark = await (from p in database.Table<Price>()
                                            where p.ServPlacemarkId == servPlacemarkId
                                            select p).ToListAsync();
                return pricePlacemark;
            }

            catch (Exception)
            {
                return null;
            }
        }
        #endregion
        #region User
        public async Task<bool> IsExistsUserAsync()
        {
            try {
                User current = await database.GetAsync<User>(1);
                if (current != null)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<User> GetUserAsync()
        {
            try
            {
                User current = await database.GetAsync<User>(1);
                return current;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
        #region UserFuelEvent
        //все ивенты авто 
        public async Task<List<CarEvent>> GetCarEvent(int carId)
        {
            List<CarEvent> lCarEvent;
            try {
                lCarEvent = await (from c in database.Table<CarEvent>()
                                   where c.CarId == carId
                                   select c).ToListAsync();

                return lCarEvent;
            }
            catch (Exception)
            {
                return null;
            }
        }
      
        //последний ивен авто
        public async Task<CarEvent> GetLastEventCar(int carId)
        {
           
            try {
                var task1 = GetCarEvent(carId);//список всех ивентов
                var res1 = await task1;
                List<CarEvent> lCarEvent = res1;
                CarEvent lastCarEvent = lCarEvent.OrderBy(x => x.DateCreate).Last();//последнее событие
                int lastCarEventId = lastCarEvent.Id;
                int servEventTypeId = lastCarEvent.ServEventTypeId;
                int servFuelCategoryId = lastCarEvent.ServFuelCategoryId;

                EventType currentEventType = await (from e in database.Table<EventType>()
                                                    where e.EventTypeId == servEventTypeId
                                                    select e).FirstOrDefaultAsync();
                currentEventType.CarEvents = new List<CarEvent> { lastCarEvent };
                FuelCategory fuelCategory = await (from e in database.Table<FuelCategory>()
                                                   where e.FuelCategoryId == servFuelCategoryId
                                                   select e).FirstOrDefaultAsync();
                fuelCategory.CarEvents = new List<CarEvent> { lastCarEvent };
                await database.UpdateWithChildrenAsync(currentEventType);
                await database.UpdateWithChildrenAsync(fuelCategory);
                var carEventWithChildren = await database
                    .GetWithChildrenAsync<CarEvent>(lastCarEventId, recursive: true)
                    .ConfigureAwait(false);
                return carEventWithChildren;                        
            }
            catch (Exception)
            {
                return null;
            }         
        }

        public async Task<Placemark> GetPlacemarkByCurrentEventWithChildren(CarEvent currentEvent)
        {
            decimal latitute = currentEvent.Latitute;
            decimal longtitute=currentEvent.Longitute;
            int currentPlacemarkId;
            Placemark currentPlacemark;
            Vendor currentVendor;
            try {
                currentPlacemark = await (from p in database.Table<Placemark>()
                                          where p.Latitude == latitute && p.Longitude == longtitute
                                          select p).FirstOrDefaultAsync();
                currentPlacemarkId = currentPlacemark.Id;
                int vendorId = currentPlacemark.ServVendorId;
                currentVendor = await (from v in database.Table<Vendor>()
                                       where v.VendorId == vendorId
                                       select v).FirstOrDefaultAsync();
                currentVendor.Placemarks = new List<Placemark> { currentPlacemark };
                await database.UpdateWithChildrenAsync(currentVendor);
                var placemarkWithChildren = await database
                    .GetWithChildrenAsync<Placemark>(currentPlacemarkId, recursive: true)
                    .ConfigureAwait(false);
                
                return placemarkWithChildren;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<CarBrand>> GetListBrandCar(int serCarTypeId)
        {
            List<CarBrand> lcarbrand;
            try {
               
                lcarbrand=await(from cb in database.Table<CarBrand>()
                                    where cb.ServCarTypeId==serCarTypeId
                                    select cb).ToListAsync();
                return lcarbrand;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<CarModel>> GetListModelCar(int servBrandId)
        {
            List<CarModel> lcarmodel;
            try
            {

                lcarmodel = await (from cm in database.Table<CarModel>()
                                   where cm.ServBrandId == servBrandId
                                   select cm).ToListAsync();
                return lcarmodel;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<CarModification>> GetListModificationCar(int servModelId)
        {
            List<CarModification> lcarmodification;
            try
            {

                lcarmodification = await (from cm in database.Table<CarModification>()
                                          where cm.ServModelId == servModelId
                                   select cm).ToListAsync();
                return lcarmodification;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion
    }
}
