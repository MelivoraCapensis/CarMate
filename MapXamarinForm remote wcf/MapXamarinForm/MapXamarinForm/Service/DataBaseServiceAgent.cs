using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WcfServiceDb;

namespace MapXamarinForm.Service
{
    public class DataBaseServiceAgent
    {
        private static EndpointAddress endPoint = new EndpointAddress("http://carmate21-org.1gb.ua/DataBaseService.svc");
        private static BasicHttpBinding binding;

        static DataBaseServiceAgent()
        {
            binding = CreateBasicHttpBinding();
        }
        private static BasicHttpBinding CreateBasicHttpBinding()
        {
            BasicHttpBinding binding = new BasicHttpBinding 
            {
                Name = "basicHttpBinding",
                MaxBufferSize = 2147483647,
                MaxReceivedMessageSize = 2147483647
            };
            TimeSpan timeout = new TimeSpan(0, 10, 30);
            binding.SendTimeout = timeout;
            binding.OpenTimeout = timeout;
            binding.ReceiveTimeout = timeout;
            return binding;
        }
       
        #region SynchronizationByCountry
        public async static Task<RegionProxy[]> DoGetRegions(int countryId)
        {
            IDataBaseService _client;
            try 
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<RegionProxy[]>.Factory.FromAsync(_client.BeginGetRegions, _client.EndGetRegions, countryId, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<VendorProxy[]> DoGetVendors(int countryId)
        {
            IDataBaseService _client;
            try 
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<VendorProxy[]>.Factory.FromAsync(_client.BeginGetVendors, _client.EndGetVendors, countryId, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<PlacemarkProxy[]> DoGetPlacemarks(int countryId,int fuelcategoryId)
        {
            IDataBaseService _client;
            try 
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<PlacemarkProxy[]>.Factory.FromAsync(_client.BeginGetPlacemarks, _client.EndGetPlacemarks, countryId, fuelcategoryId, null);
                await res;
                return res.Result;
            }
            catch(Exception)
            {
                throw;
            }
        }
        public async static Task<FuelCategoryProxy[]> DoGetFuelCategories(int countryId)
        {
            IDataBaseService _client;
            try 
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<FuelCategoryProxy[]>.Factory.FromAsync(_client.BeginGetFuelCategories, _client.EndGetFuelCategories, countryId, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<PriceProxy[]> DoGetPrices(int countryId)
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<PriceProxy[]>.Factory.FromAsync(_client.BeginGetPrices, _client.EndGetPrices, countryId, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<VendorAliasProxy[]> DoGetVendorAliases(int countryId)
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<VendorAliasProxy[]>.Factory.FromAsync(_client.BeginGetVendorAliases, _client.EndGetVendorAliases, countryId,  null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Guides
        public async static Task<CountryProxy[]> DoGetCountries()
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<CountryProxy[]>.Factory.FromAsync(_client.BeginGetCountries, _client.EndGetCountries, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<CategoryProxy[]> DoGetCategories()
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<CategoryProxy[]>.Factory.FromAsync(_client.BeginGetCategories, _client.EndGetCategories, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async static Task<AliasProxy[]> DoGetAliases()
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<AliasProxy[]>.Factory.FromAsync(_client.BeginGetAliases, _client.EndGetAliases, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<CarTypeProxy[]> DoGetCarTypes()
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<CarTypeProxy[]>.Factory.FromAsync(_client.BeginGetCarTypes, _client.EndGetCarTypes, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<CarBrandProxy[]> DoGetCarBrands()
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<CarBrandProxy[]>.Factory.FromAsync(_client.BeginGetCarBrands, _client.EndGetCarBrands, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<CarModelProxy[]> DoGetCarModels()
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<CarModelProxy[]>.Factory.FromAsync(_client.BeginGetCarModels, _client.EndGetCarModels, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<CarModificationProxy[]> DoGetCarModifications()
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<CarModificationProxy[]>.Factory.FromAsync(_client.BeginGetCarModifications, _client.EndGetCarModifications, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<CarNodeProxy[]> DoGetCarNodes()
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<CarNodeProxy[]>.Factory.FromAsync(_client.BeginGetCarNodes, _client.EndGetCarNodes , null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<EventTypeProxy[]> DoGetEventTypes()
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<EventTypeProxy[]>.Factory.FromAsync(_client.BeginGetEventTypes, _client.EndGetEventTypes, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<CarDetailProxy[]> DoGetCarDetails()
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<CarDetailProxy[]>.Factory.FromAsync(_client.BeginGetCarDetails, _client.EndGetCarDetails, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<CategoryLogProxy[]> DoGetCategoryLogs()
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<CategoryLogProxy[]>.Factory.FromAsync(_client.BeginGetCategoryLogs, _client.EndGetCategoryLogs, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<CarTransmissionProxy[]> DoGetCarTransmissions()
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<CarTransmissionProxy[]>.Factory.FromAsync(_client.BeginGetCarTransmissions, _client.EndGetCarTransmissions, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<UnitFuelConsumptionProxy[]> DoGetUnitFuelConsumption()
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<UnitFuelConsumptionProxy[]>.Factory.FromAsync(_client.BeginGetUnitFuelConsumption, _client.EndGetUnitFuelConsumption, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<UnitDistanceProxy[]> DoGetUnitDistance()
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<UnitDistanceProxy[]>.Factory.FromAsync(_client.BeginGetUnitDistance, _client.EndGetUnitDistance, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<UnitVolumeProxy[]> DoGetUnitVolume()
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<UnitVolumeProxy[]>.Factory.FromAsync(_client.BeginGetUnitVolume, _client.EndGetUnitVolume, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region SynchronizationByUser
        public async static Task<bool> DoIsRegister(string nickName, string userPassword)
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<bool>.Factory.FromAsync(_client.BeginIsRegister, _client.EndIsRegister, nickName, userPassword,null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<UserProxy> DoGetUser(string nickName, string userPassword)
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<UserProxy>.Factory.FromAsync(_client.BeginGetUser, _client.EndGetUser, nickName, userPassword, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<CarProxy[]> DoGetCar(int userId)
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<CarProxy[]>.Factory.FromAsync(_client.BeginGetCar, _client.EndGetCar, userId, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<CarEventProxy[]> DoGetCarEvent(int carId)
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<CarEventProxy[]>.Factory.FromAsync(_client.BeginGetCarEvent, _client.EndGetCarEvent, carId, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async static Task<LogBookProxy[]> DoGetLogBook(int carId)
        {
            IDataBaseService _client;
            try
            {
                _client = new DataBaseServiceClient(binding, endPoint);
                var res = Task<LogBookProxy[]>.Factory.FromAsync(_client.BeginGetLogBook, _client.EndGetLogBook, carId, null);
                await res;
                return res.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
