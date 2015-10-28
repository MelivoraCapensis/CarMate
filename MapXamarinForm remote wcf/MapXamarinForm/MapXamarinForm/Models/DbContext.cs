using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLiteNetExtensions.Extensions;
using SQLiteNetExtensions.Attributes;
using Xamarin.Forms;
using SQLite.Net.Attributes;

namespace MapXamarinForm.Models
{
    public class Category
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public System.DateTime DateCreate { get; set; }
        public int State { get; set; }
        public int CategoryId { get; set; }//server id

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Placemark> Placemarks { set; get; }
    }
    public class Country
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string CountryName { get; set; }
        public System.DateTime DateCreate { get; set; }
        public int State { get; set; }
        public int CountryId { set; get; } //server id

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Placemark> Placemarks { set; get; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Region> Regions { set; get; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Vendor> Vendors { set; get; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<FuelCategory> FuelCategories { set; get; }

    }
    public class Region
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(Country))]
        public int CountryId { get; set; }
        public string RegionName { get; set; }
        public System.DateTime DateCreate { get; set; }
        public int State { get; set; }
        public int RegionId { get; set; }//server id
        public int ServCountryId { get; set; }//server countryId

        [ManyToOne]
        public Country Country { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Placemark> Placemarks { set; get; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Price> Prices { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<User> Users { get; set; }
    }
    public class Vendor
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(Country))]
        public int CountryId { get; set; }
        public string VendorName { get; set; }
        public System.DateTime DateCreate { get; set; }
        public int State { get; set; }
        public int VendorId { get; set; }//server id
        public int ServCountryId { get; set; }//server countryId

        [ManyToOne]
        public Country Country { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Placemark> Placemarks { set; get; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Price> Prices { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<VendorAlias> VendorAliases { get; set; }
    }
    public class Placemark
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(Country))]
        public int CountryId { get; set; }
        [ForeignKey(typeof(Region))]
        public int RegionId { get; set; }
        [ForeignKey(typeof(Category))]
        public int CategoryId { get; set; }
        [ForeignKey(typeof(Vendor))]
        public int VendorId { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string Description { get; set; }
        public System.DateTime DateCreate { get; set; }
        public int State { get; set; }
        public int PlacemarkId { get; set; }//server id
        public int ServCountryId { get; set; }//server countryId       
        public int ServRegionId { get; set; }//server regionId 
        public int ServCategoryId { get; set; }//server categoryId
        public int ServVendorId { get; set; }//server vendorId

        [ManyToOne]
        public Country Country { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Price> Prices { get; set; }
        [ManyToOne]
        public Region Region { get; set; }
        [ManyToOne]
        public Category Category { get; set; }
        [ManyToOne]
        public Vendor Vendor { get; set; }
    }
    /// <summary>
    /// ///////////////////////////////////////////
    /// </summary>
    public class FuelCategory
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(Country))]
        public int CountryId { get; set; }
        public string FuelCategoryName { get; set; }
        public System.DateTime DateCreate { get; set; }
        public int State { get; set; }
        public int FuelCategoryId { get; set; }//server id
        public int ServCountryId { get; set; }//server countryId   
        
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<CarEvent> CarEvents { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Car> Cars { get; set; }
        [ManyToOne]
        public Country Country { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Price> Prices { get; set; }
    }
    public class Price
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(Placemark))]
        public int Placemarkid { get; set; }
        [ForeignKey(typeof(Vendor))]
        public int VendorId { get; set; }
        [ForeignKey(typeof(Region))]
        public int RegionId { get; set; }
        [ForeignKey(typeof(FuelCategory))]
        public int FuelId { get; set; }
        public decimal PriceCost { get; set; }
        public decimal UserPrice { get; set; }
        public System.DateTime dateCreate { get; set; }
        public int State { get; set; }
        public int PriceId { get; set; }//server id
        public int ServPlacemarkId { get; set; }//server placemarkId
        public int ServVendorId { get; set; }//server vendorId
        public int ServRegionId { get; set; }//server regionId
        public int ServFuelId { get; set; }//server fuelId

        [ManyToOne]
        public Placemark Placemark { get; set; }
        [ManyToOne]
        public Vendor Vendor { get; set; }
        [ManyToOne]
        public Region Region { get; set; }
        [ManyToOne]
        public FuelCategory FuelCategory { get; set; }
    }
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(Region))]
        public int RegionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string Nickname { get; set; }
        public string ImgPath { get; set; }
        public System.DateTime DateRegister { get; set; }
        public System.DateTime DateCreate { get; set; }
        public int State { get; set; }
        [ForeignKey(typeof(UnitDistance))]
        public int UnitDistanceId { get; set; }
        [ForeignKey(typeof(UnitVolume))]
        public int UnitVolumeId { get; set; }
        [ForeignKey(typeof(UnitFuelConsumption))]
        public int UnitFuelConsumptionId { get; set; }
        public int UserId { get; set; }//server id
        public int ServRegionId { get; set; }//server regionId
        public int ServUnitDistanceId { get; set; }
        public int ServUnitVolumeId { get; set; }
        public int ServUnitFuelConsumptionId { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Car> Cars { get; set; }
        [ManyToOne]
        public Region Region { get; set; }
        [ManyToOne]
        public UnitDistance UnitDistance { get; set; }
        [ManyToOne]
        public UnitFuelConsumption UnitFuelConsumption { get; set; }
        [ManyToOne]
        public UnitVolume UnitVolume { get; set; }
    }
    public class UnitFuelConsumption
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string NameUnit { get; set; }
        public decimal Correction { get; set; }
        public int UnitFuelConsumptionId { get; set; }//server id

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<UnitDistance> UnitDistances { set; get;}
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<UnitVolume> UnitVolumes { set; get; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<User> Users { get; set; }

    }
    public class UnitDistance
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string NameUnit { set; get; }
        public decimal Correction { set; get; }
        [ForeignKey(typeof(UnitFuelConsumption))]
        public int UnitFuelConsumptionId { get; set; }
        public int UnitDistanceId { get; set; }//server id
        public int ServUnitFuelConsumptionId { get; set; }

        [ManyToOne]
        public UnitFuelConsumption UnitFuelConsumption { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<User> Users { get; set; }

    }
    public class UnitVolume
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string NameUnit { get; set; }
        public decimal Correction { get; set; }
        [ForeignKey(typeof(UnitFuelConsumption))]
        public int UnitFuelConsumptionId { get; set; }
        public int UnitVolumeId { get; set; }//server id
        public int ServUnitFuelConsumptionId { get; set; }

        [ManyToOne]
        public UnitFuelConsumption UnitFuelConsumption { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<User> Users { get; set; }

    }
    public class VendorAlias
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(Vendor))]
        public int VendorId { get; set; }
        [ForeignKey(typeof(Alias))]
        public int AliasId { get; set; }
        public int VendorAliasId { get; set; }//server id
        public int ServVendorId { get; set; }//server vendorId
        public int ServAliasId { get; set; }
        [ManyToOne]
        public Alias Alias { get; set; }
        [ManyToOne]
        public Vendor Vendor { get; set; }
    }
    public class Alias
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string AliasName { get; set; }
        public int AliasId { get; set; }//server id

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<VendorAlias> VendorAliases { get; set; }
    }
    public class CarType
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string CarTypeName { get; set; }
        public int CarTypeId { get; set; }//server id

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<CarBrand> CarBrands { get; set; }
    }
    public class CarBrand
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(CarType))]
        public int CarTypeId { get; set; }
        public string Brand { get; set; }
        public int CarBrandId { get; set; }//server id
        public int ServCarTypeId { get; set; }
        [ManyToOne]
        public CarType CarType { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<CarModel> CarModels { get; set; }
    }
    public class CarModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(CarBrand))]
        public int BrandId { get; set; }
        public string model { get; set; }
        public int CarModelId { get; set; }//server id
        public int ServBrandId { get; set; }
        [ManyToOne]
        public CarBrand CarBrand { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Car> Cars { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<CarModification> CarModifications { get; set; }
    }
    public class CarModification
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(CarModel))]
        public int ModelId { get; set; }
        public string Modification { get; set; }
        public string HorsePower { get; set; }
        public string YearBegin { get; set; }
        public string YearEnd { get; set; }
        public int CarModificationId { get; set; }//server id
        public int ServModelId { get; set; }
        [ManyToOne]
        public CarModel CarModel { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Car> Cars { get; set; }
    }
    public class Car
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(User))]
        public int UserId { get; set; }
        [ForeignKey(typeof(CarModel))]
        public int ModelId { get; set; }
        [ForeignKey(typeof(CarModification))]
        public int ModificationId { get; set; }
        [ForeignKey(typeof(FuelCategory))]
        public int FuelCategoryId { get; set; }
        [ForeignKey(typeof(CarTransmission))]
        public int CarTransmissionID { set; get; }
        public int Odometer { get; set; }
        public int Tank { get; set; }
        public decimal Consumption { get; set; }
        public string ImgPath { get; set; }
        public DateTime DateBuy { get; set; }
        public System.DateTime DateCreate { get; set; }
        public int State { get; set; }    
        public int Rating { set; get; }
        public int CarId { get; set; }//server id

      
        public int ServUserId { get; set; }
        public int ServModelId { get; set; }
        public int ServModificationId { get; set; }
        public int ServFuelCategoryId { get; set; }
        public int ServCarTransmissionID { set; get; }

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<CarDetail> CarDetails { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<CarEvent> CarEvents { get; set; }
        [ManyToOne]
        public CarModel CarModel { get; set; }
        [ManyToOne]
        public CarModification CarModification { get; set; }
        [ManyToOne]
        public CarTransmission CarTransmission { get; set; }
        [ManyToOne]
        public FuelCategory FuelCategory { get; set; }
        [ManyToOne]
        public User User { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<LogBook> LogBook { get; set; }
    }
    public class CarTransmission
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string NameTransmission { get; set; }
        public int CarTransmissionId { set; get; }//server id

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Car> Cars { set; get; }
    }
    public class CarNode
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CarNodeId { get; set; }//server id

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<CarDetail> CarDetails { get; set; }
    }
    public class CarDetail
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(Car))]
        public int CarId { get; set; }
        [ForeignKey(typeof(CarNode))]
        public int CarNodeId { get; set; }
        public int CarDetailId { get; set; }//server id
        public int ServCarId { get; set; }
        public int ServCarNodeId { get; set; }

        [ManyToOne]
        public Car Car { get; set; }
        [ManyToOne]
        public CarNode CarNode { get; set; }
    }
    public class EventType
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int EventTypeId { get; set; }//server id

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<CarEvent> CarEvents { get; set; }
    }
    public class CarEvent
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(Car))]
        public int CarId { get; set; }
        [ForeignKey(typeof(EventType))]
        public int EventTypeId { get; set; }
        [ForeignKey(typeof(FuelCategory))]
        public int FuelCategoryId { get; set; }

        public int Odometer { get; set; }
        public decimal CostTotal { get; set; }
        public System.DateTime DateEvent { get; set; }
        public string Comment { get; set; }
        public decimal FuelCount { get; set; }
        public decimal PricePerLitr { get; set; }
        public decimal Latitute { get; set; }
        public decimal Longitute { get; set; }
        public int IsFullTank { get; set; }
        public int IsMissedFilling { get; set; }
        public string NameEvent { get; set; }
        public System.DateTime DateCreate { get; set; }
        public int State { get; set; }
        public int CarEventId { get; set; }//server id
        public int ServCarId { get; set; }
        public int ServEventTypeId { get; set; }
        public int ServFuelCategoryId { get; set; }
        [ManyToOne]
        public EventType EventType { get; set; }
        [ManyToOne]
        public FuelCategory FuelCategory { get; set; }
        [ManyToOne]
        public Car Car { get; set; }
        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<LogBook> LogBook { get; set; }
    }

    public class LogBook
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(CategoryLog))]
        public int CategoryLogid { get; set; }
        [ForeignKey(typeof(CarEvent))]
        public int Chargingid { get; set; }
        [ForeignKey(typeof(Car))]
        public int CarId { get; set; }
        public decimal Total_cost { get; set; }
        public System.DateTime DateCharging { get; set; }
        public int LogBookId { get; set; }//server id
        public int ServCategoryLogid { get; set; }
        public int ServChargingid { get; set; }
        public int ServCarId { get; set; }

        [ManyToOne]
        public CarEvent CarEvent { get; set; }
        [ManyToOne]
        public Car Car { get; set; }
        [ManyToOne]
        public CategoryLog CategoryLog { get; set; }
    }
    public class CategoryLog
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string CategoryLogName { get; set; }
        public int CategoryLogId { get; set; }//server id


        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<LogBook> LogBook { get; set; }
    }
}
