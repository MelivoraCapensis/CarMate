using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using MapXamarinForm.Data;
using MapXamarinForm.Models;
using MapXamarinForm.Service;
using WcfServiceDb;

namespace MapXamarinForm
{
    public class SynchronizationHelper
    {
        public async static Task<string> SynchronizePriceByCountry(int countryId)
        {
            string text="";
            int count = 0;
            //intsert prices by counrtry to locl db
            //get data prices wcf service
            var task1 = DataBaseServiceAgent.DoGetPrices(countryId);
            var res1 = await task1;
            count = 0;
            List<Price> lprices = new List<Price>();
            foreach (var p in res1)
            {
                Price temp = new Price();
                DbConverter.PriceDALToPriceProxy(p, temp);
                lprices.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllPrices(lprices);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add Prices: {0} sucess\r\n", count);
            else
                text += String.Format("Add Prices: error\r\n");
            return text;
        }
        public async static Task<string> SynchronizePlacemarkByCountry(int countryId)
        {
            string text = "";
            int count = 0;
            //intsert placemarks by counrtry to locl db
            //get data placemarks wcf service
            var task1 = DataBaseServiceAgent.DoGetPlacemarks(countryId, 3);
            var res1 = await task1;
            count = 0;
            List<Placemark> lplacemark = new List<Placemark>();
            foreach (var p in res1)
            {
                Placemark temp = new Placemark();
                DbConverter.PlacemarkDALToPlacemarkProxy(p, temp);
                lplacemark.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllPlacemarks(lplacemark);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add Placemarks: {0} sucess\r\n", count);
            else
                text += String.Format("Add Placemarks: error\r\n");
            return text;

        }
        public async static Task<string> SynchronizeRegionByCountry(int countryId)
        {
            string text = "";
            int count = 0;
            //intsert region by counrtry to locl db
            //get data regions

            //get data regions wcf service
            var task1 = DataBaseServiceAgent.DoGetRegions(countryId);
            var res1 = await task1;
            count = 0;
            List<Region> lregion = new List<Region>();
            foreach (var r in res1)
            {
                Region temp = new Region();
                DbConverter.RegionDALToRegionProxy(r, temp);
                lregion.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllRegions(lregion);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add Regions: {0} sucess\r\n", count);
            else
                text += String.Format("Add Regions:  error\r\n");
            return text;
        }
       
        public async static Task<string> SynchronizeVendorAliasByCountry(int countryId)
        {
            string text = "";
            int count = 0;
            var task1 = DataBaseServiceAgent.DoGetVendorAliases(countryId);
            var res1 = await task1;
            count = 0;
            List<VendorAlias> lvendoralias = new List<VendorAlias>();
            foreach (var v in res1)
            {
                VendorAlias temp = new VendorAlias();
                DbConverter.VendorAliasDALToVendorAliasProxy(v, temp);
                lvendoralias.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllVendorAlias(lvendoralias);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add VendorAlias: {0} sucess\r\n", count);
            else
                text += String.Format("Add VendorAlias:  error\r\n");
            return text;
        }
        public async static Task<string> SynchronizeVendorByCountry(int countryId)
        {
            string text = "";
            int count = 0;
            //intsert vendors by counrtry to locl db
            //get data vendors wcf service
            var task1 = DataBaseServiceAgent.DoGetVendors(countryId);//wcf
            var res1 = await task1;
            count = 0;
            List<Vendor> lvendor = new List<Vendor>();
            foreach (var v in res1)
            {
                Vendor temp = new Vendor();
                DbConverter.VendorDALToVendorProxy(v, temp);
                lvendor.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllVendors(lvendor);//ldb
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add Vendors: {0} sucess\r\n", count);
            else
                text += String.Format("Add Vendors: error\r\n");
            return text;

        }
        public async static Task<string> SynchronizeFuelCategoryByCountry(int countryId)
        {
            string text = "";
            int count = 0;
            //intsert fuelcategories by counrtry to locl db
            //get data fuelcategories wcf service
            var task1 = DataBaseServiceAgent.DoGetFuelCategories(countryId);
            var res1 = await task1;
            count = 0;
            List<FuelCategory> lfuelcategory = new List<FuelCategory>();
            foreach (var p in res1)
            {
                FuelCategory temp = new FuelCategory();
                DbConverter.FuelCategoryDALToFuelCategoryProxy(p, temp);
                lfuelcategory.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllFuelCategories(lfuelcategory);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add FuelCategory: {0} sucess\r\n", count);
            else
                text += String.Format("Add FuelCategory: {0} error\r\n");
            return text;

        }
        public async static Task<string> SynchronizeCategoryByCountry(int countryId)
        {
            string text = "";
            int count = 0;
            //inset country to local db            
            //get data categories wcf service
            var task = DataBaseServiceAgent.DoGetCategories();
            var res = await task;
            List<Category> lcategory = new List<Category>();
            foreach (var c in res)
            {
                Category temp = new Category();
                DbConverter.CategoryDALToCategoryProxy(c, temp);
                lcategory.Add(temp);
                count++;
            }
            var task1 = App.Database.InsertAllCategories(lcategory);
            var res1 = await task1;
            if (res1 == true)
                text += String.Format("Add Categories: {0} sucess\r\n", count);
            else
                text += String.Format("Add Categories: error\r\n");
            return text;
        }

        public async static Task<string> SynchronizeCarByUser(int userId)
        {
            string text = "";
            int count = 0;
            var task1 = DataBaseServiceAgent.DoGetCar(userId);
            var res1 = await task1;
            List<Car> lcar = new List<Car>();
            foreach (var c in res1)
            {
                Car temp = new Car();
                DbConverter.CarDALToCarProxy(c, temp);
                lcar.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllCars(lcar);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add Cars: {0} sucess\r\n", count);
            else
                text += String.Format("Add Cars: error\r\n");
            return text;
        }
        
        public async static Task<string> SynchronizeCarEvent(int carId)
        {
            string text = "";
            int count = 0;
            var task1 = DataBaseServiceAgent.DoGetCarEvent(carId);
            var res1 = await task1;
            List<CarEvent> lcarevent = new List<CarEvent>();
            foreach (var c in res1)
            {
                CarEvent temp = new CarEvent();
                DbConverter.CarEventDALToCarEventProxy(c, temp);
                lcarevent.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllCarEvents(lcarevent);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add CarEvents: {0} sucess\r\n", count);
            else
                text += String.Format("Add CarEvents: error\r\n");
            return text;
        }
        public async static Task<string> SynchronizeLogBook(int carId)
        {
            string text = "";
            int count = 0;
            var task1 = DataBaseServiceAgent.DoGetLogBook(carId);
            var res1 = await task1;
            List<LogBook> llogbook = new List<LogBook>();
            foreach (var l in res1)
            {
                LogBook temp = new LogBook();
                DbConverter.LogBookDALToLogBookProxy(l, temp);
                llogbook.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllCarLogBook(llogbook);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add LogBooks: {0} sucess\r\n", count);
            else
                text += String.Format("Add LogBooks: error\r\n");
            return text;
        }
        public async static Task<string> SynchronizeAliases()
        {
            string text = "";
            int count = 0;
            var task1 = DataBaseServiceAgent.DoGetAliases();
            var res1 = await task1;
            List<Alias> lalias = new List<Alias>();
            foreach (var a in res1)
            {
                Alias temp = new Alias();
                DbConverter.AliasDALToAliasProxy(a, temp);
                lalias.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllAliases(lalias);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add Aliases: {0} sucess\r\n", count);
            else
                text += String.Format("Add Aliases: error\r\n");
            return text;
        }
        public async static Task<string> SynchronizeCarTypes()
        {
            string text = "";
            int count = 0;
            var task1 = DataBaseServiceAgent.DoGetCarTypes();
            var res1 = await task1;
            List<CarType> lcartype = new List<CarType>();
            foreach (var c in res1)
            {
                CarType temp = new CarType();
                DbConverter.CarTypeDALToCarTypeProxy(c,temp);
                lcartype.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllCarTypes(lcartype);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add CarType: {0} sucess\r\n", count);
            else
                text += String.Format("Add CarType: error\r\n");
            return text;
        }
        public async static Task<string> SynchronizeCarBrands()
        {
            string text = "";
            int count = 0;
            var task1 = DataBaseServiceAgent.DoGetCarBrands();
            var res1 = await task1;
            List<CarBrand> lcarbrand = new List<CarBrand>();
            foreach (var c in res1)
            {
                CarBrand temp = new CarBrand();
                DbConverter.CarBrandDALToCarBrandProxy(c, temp);
                lcarbrand.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllCarBrands(lcarbrand);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add CarBrand: {0} sucess\r\n", count);
            else
                text += String.Format("Add CarBrand: error\r\n");
            return text;
        }
        public async static Task<string> SynchronizeCarModels()
        {
            string text = "";
            int count = 0;
            var task1 = DataBaseServiceAgent.DoGetCarModels();
            var res1 = await task1;
            List<CarModel> lcarmodel = new List<CarModel>();
            foreach (var c in res1)
            {
                CarModel temp = new CarModel();
                DbConverter.CarModelDALToCarModelProxy(c, temp);
                lcarmodel.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllCarModels(lcarmodel);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add CarModels: {0} sucess\r\n", count);
            else
                text += String.Format("Add CarModels: error\r\n");
            return text;
        }
        public async static Task<string> SynchronizeCarModifications()
        {
            string text = "";
            int count = 0;
            var task1 = DataBaseServiceAgent.DoGetCarModifications();
            var res1 = await task1;
            List<CarModification> lcarmodification = new List<CarModification>();
            foreach (var c in res1)
            {
                CarModification temp = new CarModification();
                DbConverter.CarModificationDALToCarModificationProxy(c, temp);
                lcarmodification.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllCarModifications(lcarmodification);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add CarModifications: {0} sucess\r\n", count);
            else
                text += String.Format("Add CarModifications: error\r\n");
            return text;
        }
        public async static Task<string> SynchronizeCarNodes()
        {
            string text = "";
            int count = 0;
            var task1 = DataBaseServiceAgent.DoGetCarNodes();
            var res1 = await task1;
            List<CarNode> lcarnode = new List<CarNode>();
            foreach (var c in res1)
            {
                CarNode temp = new CarNode();
                DbConverter.CarNodeDALToCarNodeProxy(c, temp);
                lcarnode.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllCarNodes(lcarnode);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add CarNodes: {0} sucess\r\n", count);
            else
                text += String.Format("Add CarNodes: error\r\n");
            return text;
        }
        public async static Task<string> SynchronizeCarDetails()
        {
            string text = "";
            int count = 0;
            var task1 = await DataBaseServiceAgent.DoGetCarDetails();
            List<CarDetail> lcardetail = new List<CarDetail>();
            foreach (var cd in task1)
            {
                CarDetail temp= new CarDetail();
                DbConverter.CarDetailDALToCarDetailProxy(cd, temp);
                lcardetail.Add(temp);
                count++;
            }
            var task2 =await App.Database.InsertAllCarDetails(lcardetail);
            if(task2==true)
                text += String.Format("Add CarDetail: {0} sucess\r\n", count);
            else
                text += String.Format("Add CarDetail: error\r\n");
            return text;

        }
        public async static Task<string> SynchronizeEventTypes()
        {
            string text = "";
            int count = 0;
            var task1 = DataBaseServiceAgent.DoGetEventTypes();
            var res1 = await task1;
            List<EventType> leventtype = new List<EventType>();
            foreach (var c in res1)
            {
                EventType temp = new EventType();
                DbConverter.EventTypeDALToEventTypeProxy(c, temp);
                leventtype.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllEventTypes(leventtype);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add EventTypes: {0} sucess\r\n", count);
            else
                text += String.Format("Add EventTypes: error\r\n");
            return text;
        }
        public async static Task<string> SynchronizeCategoryLogs()
        {
            string text = "";
            int count = 0;
            var task1 = DataBaseServiceAgent.DoGetCategoryLogs();
            var res1 = await task1;
            List<CategoryLog> lcategorylog = new List<CategoryLog>();
            foreach (var c in res1)
            {
                CategoryLog temp = new CategoryLog();
                DbConverter.CategoryLogDALToCategoryLogProxy(c, temp);
                lcategorylog.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllCategoryLogs(lcategorylog);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add CategoryLog: {0} sucess\r\n", count);
            else
                text += String.Format("Add CategoryLog: error\r\n");
            return text;
        }
        public async static Task<string> SynchronizeCarTransmissions()
        {
            string text = "";
            int count = 0;
            var task1 = DataBaseServiceAgent.DoGetCarTransmissions();
            var res1 = await task1;
            List<CarTransmission> lcartransmision = new List<CarTransmission>();
            foreach (var c in res1)
            {
                CarTransmission temp = new CarTransmission();
                DbConverter.CarTransmissionDALToCarTransmissionProxy(c, temp);
                lcartransmision.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllCarTransmission(lcartransmision);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add CarTransmission: {0} sucess\r\n", count);
            else
                text += String.Format("Add CarTransmission: error\r\n");
            return text;
        }
        public async static Task<string> SynchronizeUnitFuelConsumption()
        {
            string text = "";
            int count = 0;
            var task1 = DataBaseServiceAgent.DoGetUnitFuelConsumption();
            var res1 = await task1;
            List<UnitFuelConsumption> lunitfuleconsump = new List<UnitFuelConsumption>();
            foreach (var c in res1)
            {
                UnitFuelConsumption temp = new UnitFuelConsumption();
                DbConverter.UnitFuelConsumptionDALToUnitFuelConsumptionProxy(c,temp);
                lunitfuleconsump.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllUnitFuelConsumption(lunitfuleconsump);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add UnitFuelConsumption: {0} sucess\r\n", count);
            else
                text += String.Format("Add UnitFuelConsumption: error\r\n");
            return text;
        }
        public async static Task<string> SynchronizeUnitDistance()
        {
            string text = "";
            int count = 0;
            var task1 = DataBaseServiceAgent.DoGetUnitDistance();
            var res1 = await task1;
            List<UnitDistance> lunitdistance = new List<UnitDistance>();
            foreach (var c in res1)
            {
                UnitDistance temp = new UnitDistance();
                DbConverter.UnitDistanceDALToUnitDistanceProxy(c, temp);
                lunitdistance.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllUnitDistance(lunitdistance);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add UnitDistance: {0} sucess\r\n", count);
            else
                text += String.Format("Add UnitDistance: error\r\n");
            return text;
        }
        public async static Task<string> SynchronizeUnitVolume()
        {
            string text = "";
            int count = 0;
            var task1 = DataBaseServiceAgent.DoGetUnitVolume();
            var res1 = await task1;
            List<UnitVolume> lunitvolume = new List<UnitVolume>();
            foreach (var c in res1)
            {
                UnitVolume temp = new UnitVolume();
                DbConverter.UnitVolumeDALToUnitVolumeProxy(c, temp);
                lunitvolume.Add(temp);
                count++;
            }
            var task2 = App.Database.InsertAllUnitVolume(lunitvolume);
            var res2 = await task2;
            if (res2 == true)
                text += String.Format("Add UnitVolume: {0} sucess\r\n", count);
            else
                text += String.Format("Add UnitVolume: error\r\n");
            return text;
        }
        public async static Task<string> SynchronizeBy()
        {
            string text = "";
            DateTime begin, end;
            begin = DateTime.Now;

            #region CarType
            var task1 = SynchronizeCarTypes();
            var res1 = await task1;
            text += res1;
            #endregion

            #region CarBrand
            var task2 = SynchronizeCarBrands();
            var res2 = await task2;
            text += res2;
            #endregion

            #region CarModel
            var task3 = SynchronizeCarModels();
            var res3 = await task3;
            text += res3;
            #endregion

            #region CarModification
            var task4 = SynchronizeCarModifications();
            var res4 = await task4;
            text += res4;
            #endregion

            #region CarNodes
            var task5 = SynchronizeCarNodes();
            var res5 = await task5;
            //text += res5;
            #endregion

            #region CarDetail
            var task12 = await SynchronizeCarDetails();
            //text += task12;
            #endregion

            #region EventTypes
            var task6 = SynchronizeEventTypes();
            var res6 = await task6;
            text += res6;
            #endregion

            #region CategoryLog
            var task7 = SynchronizeCategoryLogs();
            var res7 = await task7;
            //text += res7;
            #endregion

            //#region CarTransmission
            //var task8 = SynchronizeCarTransmissions();
            //var res8 = await task8;
            ////text += res8;
            //#endregion

            #region UnitFuelConsumption
            var task9 = SynchronizeUnitFuelConsumption();
            var res9 = await task9;
            //text += res9;
            #endregion

            //#region UnitFuelConsumption
            //var task10 = SynchronizeUnitDistance();
            //var res10 = await task10;
            ////text += res10;
            //#endregion

            #region UnitVolume
            var task11 = SynchronizeUnitVolume();
            var res11 = await task11;
            //text += res11;
            #endregion
            end = DateTime.Now;
            text += String.Format("Synchronization local db by server db success begin at {0} end at {1}", begin, end);
            return text;
        }
        public async static Task<string> SynchronizeByUser(UserProxy userProxy)
        {
            string text = "";

            DateTime begin, end;
            int userId = userProxy.UserId;
            begin = DateTime.Now;
            #region CarsId
           
            int count = 0;
            var task1 =await  DataBaseServiceAgent.DoGetCar(userId);
          
            List<Car> lcar = new List<Car>();
            List<int> lcarId = new List<int>();
            foreach (var c in task1)
            {
                Car temp = new Car();
                DbConverter.CarDALToCarProxy(c, temp);
                lcar.Add(temp);
                lcarId.Add(c.CarId);
                await SynchronizeLogBook(c.CarId);
                count++;
            }
            
            List<CarEvent> lcarevent = new List<CarEvent>();
            foreach (var carId in lcarId)
            {
                var task2 = await DataBaseServiceAgent.DoGetCarEvent(carId);
                foreach (var c in task2)
                {
                    CarEvent temp = new CarEvent();
                    DbConverter.CarEventDALToCarEventProxy(c, temp);
                    lcarevent.Add(temp);
                    count++;
                }
            }
            await App.Database.InsertAllCarEvents(lcarevent);
            foreach (var c in lcar)
            {
                await App.Database.AddNewCar(c);
            }
            await App.Database.AddNewUserAsync(userProxy);
            #endregion
            return text;
        }
       
        public async static Task<string> SynchronizeByCountry(CountryProxy countryProxy)
        {
            string text = "";
           
            DateTime begin, end;
            int countryId = countryProxy.CountryId;
            begin = DateTime.Now;

            #region Prices
            var task1 = SynchronizePriceByCountry(countryId);
            var res1 = await task1;
            text += res1;
            #endregion

            #region Placemarks
            var task2 = SynchronizePlacemarkByCountry(countryId);
            var res2 = await task2;
            text += res2;
            #endregion

            #region Regions
            var task3 = SynchronizeRegionByCountry(countryId);
            var res3 = await task3;
            text += res3;
            #endregion

            #region Aliases
            var task9 = SynchronizeAliases();
            var res9 = await task9;
            //text += res9;
            #endregion

            #region VendorAlias
            var task8 = SynchronizeVendorAliasByCountry(countryId);
            var res8 = await task8;
            //text += res8;
            #endregion

            #region Vendor
            var task4 = SynchronizeVendorByCountry(countryId);
            var res4 = await task4;
            text += res4;
            #endregion

            #region FuelCategories
            var task5 = SynchronizeFuelCategoryByCountry(countryId);
            var res5 = await task5;
            text += res5;
            #endregion

            #region Category
            var task6 = SynchronizeCategoryByCountry(countryId);
            var res6 = await task6;
            text += res6;
            #endregion

            #region Country
            //insert country to local db
            var task7 = App.Database.AddNewCountryAsync(countryProxy);
            var res7 = await task7;

            #endregion
            end = DateTime.Now;
            text += String.Format("Synchronization local db by server db success begin at {0} end at {1}", begin, end);
            return text;
        }
    }
}
