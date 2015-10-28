using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfServiceDb
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDataBaseService" in both code and config file together.
    [ServiceContract]
    public interface IDataBaseService
    {

        #region SynchronizationByCountry
        [OperationContract]
        List<CountryProxy> GetCountries();//
        [OperationContract]
        List<CategoryProxy> GetCategories();//
        [OperationContract]
        List<RegionProxy> GetRegions(int countryId);//
        [OperationContract]
        List<VendorProxy> GetVendors(int countryId);//
        [OperationContract]
        List<PlacemarkProxy> GetPlacemarks(int countryId, int fuelcategoryId);//
        [OperationContract]
        List<FuelCategoryProxy> GetFuelCategories(int countryId);//
        //[OperationContract]
        //List<PriceProxy> GetPrices(int countryId, int fuelcategoryId);//
        [OperationContract]
        List<PriceProxy> GetPrices(int countryId);
        [OperationContract]
        List<VendorAliasProxy> GetVendorAliases(int countryId);
        #endregion
     
        #region SynchronizationByUser
        [OperationContract]
        bool IsRegister(string nickName, string userPassword);
        [OperationContract]
        UserProxy GetUser(string nickName,string userPassword);
        [OperationContract]
        List<CarProxy> GetCar(int userId);
        [OperationContract]
        List<CarEventProxy> GetCarEvent(int carId);
        [OperationContract]
        List<LogBookProxy> GetLogBook(int carId);

        [OperationContract]
        List<AliasProxy> GetAliases();//
        [OperationContract]
        List<CarTypeProxy> GetCarTypes();//
        [OperationContract]
        List<CarBrandProxy> GetCarBrands();//
        [OperationContract]
        List<CarModelProxy> GetCarModels();//
        [OperationContract]
        List<CarModificationProxy> GetCarModifications();//
        [OperationContract]
        List<CarNodeProxy> GetCarNodes();//
        [OperationContract]
        List<EventTypeProxy> GetEventTypes();//
        [OperationContract]
        List<CarDetailProxy> GetCarDetails();//
        [OperationContract]
        List<CategoryLogProxy> GetCategoryLogs();//
        [OperationContract]
        List<CarTransmissionProxy> GetCarTransmissions();//
        [OperationContract]
        List<UnitFuelConsumptionProxy> GetUnitFuelConsumption();//
        [OperationContract]
        List<UnitDistanceProxy> GetUnitDistance();//
        [OperationContract]
        List<UnitVolumeProxy> GetUnitVolume();//
        #endregion

    }
    [DataContract]
    public class CategoryProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public System.DateTime DateCreate { get; set; }
        [DataMember]
        public int State { get; set; }
        [DataMember]
        public int CategoryId { get; set; }

        [DataMember]
        public List<PlacemarkProxy> Placemarks { set; get; }
    }
    [DataContract]
    public class CountryProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string CountryName { get; set; }
        [DataMember]
        public System.DateTime DateCreate { get; set; }
        [DataMember]
        public int State { get; set; }
        [DataMember]
        public int CountryId { get; set; }

        [DataMember]
        public List<PlacemarkProxy> Placemarks { set; get; }
        [DataMember]
        public List<RegionProxy> Regions { set; get; }
        [DataMember]
        public List<VendorProxy> Vendors { set; get; }
        [DataMember]
        public List<FuelCategoryProxy> FuelCategories { set; get; }

    }
    [DataContract]
    public class RegionProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CountryId { get; set; }//server db id
        [DataMember]
        public string RegionName { get; set; }
        [DataMember]
        public System.DateTime DateCreate { get; set; }
        [DataMember]
        public int State { get; set; }
        [DataMember]
        public int RegionId { get; set; }
       

        [DataMember]
        public CountryProxy Country { get; set; }
        [DataMember]
        public List<PlacemarkProxy> Placemarks { set; get; }
        [DataMember]
        public List<PriceProxy> Prices { get; set; }
        [DataMember]
        public List<UserProxy> Users { get; set; }
    }
    [DataContract]
    public class VendorProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CountryId { get; set; }//server db id
        [DataMember]
        public string VendorName { get; set; }
        [DataMember]
        public System.DateTime DateCreate { get; set; }
        [DataMember]
        public int State { get; set; }
        [DataMember]
        public int VendorId { get; set; }

        [DataMember]
        public CountryProxy Country { get; set; }
        [DataMember]
        public List<PlacemarkProxy> Placemarks { set; get; }
        [DataMember]
        public List<PriceProxy> Prices { get; set; }
        [DataMember]
        public List<VendorAliasProxy> VendorAliases { get; set; }
    }
    [DataContract]
    public class PlacemarkProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public int RegionId { get; set; }
        [DataMember]
        public int CategoryId { get; set; }
        [DataMember]
        public int VendorId { get; set; }
        [DataMember]
        public decimal Latitude { get; set; }
        [DataMember]
        public decimal Longitude { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public System.DateTime DateCreate { get; set; }
        [DataMember]
        public int State { get; set; }
        [DataMember]
        public int PlacemarkId { get; set; }

        [DataMember]
        public CountryProxy Country { get; set; }
        [DataMember]
        public List<PriceProxy> Prices { get; set; }
        [DataMember]
        public RegionProxy Region { get; set; }
        [DataMember]
        public CategoryProxy Category { get; set; }
        [DataMember]
        public VendorProxy Vendor { get; set; }
    }
    [DataContract]
    public class FuelCategoryProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CountryId { get; set; }
        [DataMember]
        public string FuelCategoryName { get; set; }
        [DataMember]
        public System.DateTime DateCreate { get; set; }
        [DataMember]
        public int State { get; set; }
        [DataMember]
        public int FuelCategoryId { get; set; }

        [DataMember]
        public List<CarEventProxy> CarEvents { get; set; }
        [DataMember]
        public CountryProxy Country { get; set; }
        [DataMember]
        public List<PriceProxy> Prices { get; set; }
    }
    [DataContract]
    public class PriceProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int Placemarkid { get; set; }
        [DataMember]
        public int VendorId { get; set; }
        [DataMember]
        public int RegionId { get; set; }
        [DataMember]
        public int FuelId { get; set; }
        [DataMember]
        public decimal PriceCost { get; set; }
        [DataMember]
        public decimal UserPrice { get; set; }
        [DataMember]
        public System.DateTime dateCreate { get; set; }
        [DataMember]
        public int State { get; set; }
        [DataMember]
        public int PriceId { get; set; }

        [DataMember]
        public PlacemarkProxy Placemark { get; set; }
        [DataMember]
        public VendorProxy Vendor { get; set; }
        [DataMember]
        public RegionProxy Region { get; set; }
        [DataMember]
        public FuelCategoryProxy FuelCategory { get; set; }
    }
    [DataContract]
    public class UserProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int RegionId { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string UserEmail { get; set; }
        [DataMember]
        public string UserPassword { get; set; }
        [DataMember]
        public string Nickname { get; set; }
        [DataMember]
        public string ImgPath { get; set; }
        [DataMember]
        public System.DateTime DateRegister { get; set; }
        [DataMember]
        public System.DateTime DateCreate { get; set; }
        [DataMember]
        public int State { get; set; }
        [DataMember]
        public int UnitDistanceId { get; set; }
        [DataMember]
        public int UnitVolumeId { get; set; }
        [DataMember]
        public int UnitFuelConsumptionId { get; set; }
        [DataMember]
        public int UserId { get; set; }

        [DataMember]
        public List<CarProxy> Cars { get; set; }
        [DataMember]
        public RegionProxy Region { get; set; }
        [DataMember]
        public UnitDistanceProxy UnitDistance { get; set; }
        [DataMember]
        public UnitFuelConsumptionProxy UnitFuelConsumption { get; set; }
        [DataMember]
        public UnitVolumeProxy UnitVolume { get; set; }
    }
    [DataContract]
    public class UnitFuelConsumptionProxy
    {
        [DataMember]
        public int Id { get; set; }
         [DataMember]
        public string NameUnit { get; set; }
         [DataMember]
        public decimal Correction { get; set; }
         [DataMember]
        public int UnitFuelConsumptionId { get; set; }//server id  
         [DataMember]
        public List<UnitDistanceProxy> UnitDistances { set; get; }
         [DataMember]
        public List<UnitVolumeProxy> UnitVolumes { set; get; }
         [DataMember]
        public List<UserProxy> Users { get; set; }

    }
    [DataContract]
    public class UnitDistanceProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string NameUnit { set; get; }
        [DataMember]
        public decimal Correction { set; get; }
        [DataMember]
        public int UnitFuelConsumptionId { get; set; }
        [DataMember]
        public int UnitDistanceId { get; set; }//server id
        [DataMember]
        public UnitFuelConsumptionProxy UnitFuelConsumption { get; set; }
        [DataMember]
        public List<UserProxy> Users { get; set; }

    }
    [DataContract]
    public class UnitVolumeProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string NameUnit { get; set; }
        [DataMember]
        public decimal Correction { get; set; }
        [DataMember]
        public int UnitFuelConsumptionId { get; set; }
        [DataMember]
        public int UnitVolumeId { get; set; }//server id

        [DataMember]
        public UnitFuelConsumptionProxy UnitFuelConsumption { get; set; }
        [DataMember]
        public List<UserProxy> Users { get; set; }

    }
    [DataContract]
    public class VendorAliasProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int VendorId { get; set; }
        [DataMember]
        public int AliasId { get; set; }
        [DataMember]
        public int VendorAliasId { get; set; }

        [DataMember]
        public AliasProxy Alias { get; set; }
        [DataMember]
        public VendorProxy Vendor { get; set; }
    }
    [DataContract]
    public class AliasProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string AliasName { get; set; }
        [DataMember]
        public int AliasId { get; set; }

        [DataMember]
        public List<VendorAliasProxy> VendorAliases { get; set; }
    }
    [DataContract]
    public class CarTypeProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string CarTypeName { get; set; }
        [DataMember]
        public int CarTypeId { get; set; }

        [DataMember]
        public List<CarBrandProxy> CarBrands { get; set; }
    }
    [DataContract]
    public class CarBrandProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CarTypeId { get; set; }
        [DataMember]
        public string Brand { get; set; }
        [DataMember]
        public int CarBrandId { get; set; }

        [DataMember]
        public CarTypeProxy CarType { get; set; }
        [DataMember]
        public List<CarModelProxy> CarModels { get; set; }
    }
    [DataContract]
    public class CarModelProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int BrandId { get; set; }
        [DataMember]
        public string model { get; set; }
        [DataMember]
        public int CarModelId { get; set; }

        [DataMember]
        public CarBrandProxy CarBrand { get; set; }
        [DataMember]
        public List<CarProxy> Cars { get; set; }
        [DataMember]
        public List<CarModificationProxy> CarModifications { get; set; }
    }
    [DataContract]
    public class CarModificationProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int ModelId { get; set; }
        [DataMember]
        public string Modification { get; set; }
        [DataMember]
        public string HorsePower { get; set; }
        [DataMember]
        public string YearBegin { get; set; }
        [DataMember]
        public string YearEnd { get; set; }
        [DataMember]
        public int CarModificationId { get; set; }

        [DataMember]
        public CarModelProxy CarModel { get; set; }
        [DataMember]
        public List<CarProxy> Cars { get; set; }
    }
    [DataContract]
    public class CarProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int ModelId { get; set; }
        [DataMember]
        public int ModificationId { get; set; }
        [DataMember]
        public int Odometer { get; set; }
        [DataMember]
        public int Tank { get; set; }
        [DataMember]
        public decimal Consumption { get; set; }
        [DataMember]
        public string ImgPath { get; set; }
        [DataMember]
        public DateTime DateBuy { get; set; }
        [DataMember]
        public System.DateTime DateCreate { get; set; }
        [DataMember]
        public int State { get; set; }
        [DataMember]
        public int FuelCategoryId { get; set; }
        [DataMember]
        public int CarTransmissionID { set; get; }
         [DataMember]
        public int Rating { set; get; }
        [DataMember]
        public int CarId { get; set; }

        [DataMember]
        public List<CarDetailProxy> CarDetails { get; set; }
        [DataMember]
        public List<CarEventProxy> CarEvents { get; set; }
        [DataMember]
        public CarModelProxy CarModel { get; set; }
        [DataMember]
        public CarModificationProxy CarModification { get; set; }
        [DataMember]
        public CarTransmissionProxy CarTransmission { get; set; }
       
        [DataMember]
        public UserProxy User { get; set; }
        [DataMember]
        public List<LogBookProxy> LogBook { get; set; }
    }
    [DataContract]
    public class CarTransmissionProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string NameTransmission { get; set; }
        [DataMember]
        public int CarTransmissionId { set; get; }//server id
        [DataMember]
        public List<CarProxy> Cars { set; get; }
    }
    [DataContract]
    public class CarNodeProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int CarNodeId { get; set; }

        [DataMember]
        public List<CarDetailProxy> CarDetails { get; set; }
    }
    [DataContract]
    public class CarDetailProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CarId { get; set; }
        [DataMember]
        public int CarNodeId { get; set; }
        [DataMember]
        public int CarDetailId { get; set; }

        [DataMember]
        public CarProxy Car { get; set; }
        [DataMember]
        public CarNodeProxy CarNode { get; set; }
    }
    [DataContract]
    public class EventTypeProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int EventTypeId { get; set; }

        [DataMember]
        public List<CarEventProxy> CarEvents { get; set; }
    }
    [DataContract]
    public class CarEventProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CarId { get; set; }
        [DataMember]
        public int EventTypeId { get; set; }
        [DataMember]
        public int FuelCategoryId { get; set; }
        [DataMember]
        public int Odometer { get; set; }
        [DataMember]
        public decimal CostTotal { get; set; }
        [DataMember]
        public System.DateTime DateEvent { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public decimal FuelCount { get; set; }
        [DataMember]
        public decimal PricePerLitr { get; set; }
        [DataMember]
        public decimal Latitute { get; set; }
        [DataMember]
        public decimal Longitute { get; set; }
        [DataMember]
        public int IsFullTank { get; set; }
        [DataMember]
        public int IsMissedFilling { get; set; }
        [DataMember]
        public string NameEvent { get; set; }
        [DataMember]
        public System.DateTime DateCreate { get; set; }
        [DataMember]
        public int State { get; set; }
        [DataMember]
        public int CarEventId { get; set; }

        [DataMember]
        public EventTypeProxy EventType { get; set; }
        [DataMember]
        public FuelCategoryProxy FuelCategory { get; set; }
        [DataMember]
        public CarProxy Car { get; set; }
        [DataMember]
        public List<LogBookProxy> LogBook { get; set; }
    }
    [DataContract]
    public class LogBookProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public int CategoryLogid { get; set; }
        [DataMember]
        public int Chargingid { get; set; }
        [DataMember]
        public int CarId { get; set; }
        [DataMember]
        public decimal Total_cost { get; set; }
        [DataMember]
        public System.DateTime DateCharging { get; set; }
        [DataMember]
        public int LogBookId { get; set; }
        
        [DataMember]
        public CarEventProxy CarEvent { get; set; }
        [DataMember]
        public CarProxy Car { get; set; }
        [DataMember]
        public CategoryLogProxy CategoryLog { get; set; }
    }
    [DataContract]
    public class CategoryLogProxy
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string CategoryLogName { get; set; }
        [DataMember]
        public int CategoryLogId { get; set; }

        [DataMember]
        public List<LogBookProxy> LogBook { get; set; }
    }
}
