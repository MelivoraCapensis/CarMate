using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WcfServiceDb;
using MapXamarinForm.Models;

namespace MapXamarinForm.Data
{
    public static class DbConverter
    {
        public static void CategoryDALToCategoryProxy(CategoryProxy category, Category categoryDAL)
        {
           
            categoryDAL.CategoryName= category.CategoryName;
            categoryDAL.DateCreate=category.DateCreate;
            categoryDAL.State = category.State;
            categoryDAL.CategoryId = category.Id;

        }
        public static void CountryDALToContryProxy(CountryProxy country, Country countryDAL)
        {
           
            countryDAL.CountryName = country.CountryName;
            countryDAL.DateCreate = country.DateCreate;
            countryDAL.State = country.State;
            countryDAL.CountryId = country.Id;
        }
        public static void RegionDALToRegionProxy(RegionProxy region, Region regionDAL)
        {
          
            regionDAL.CountryId = region.CountryId;
            regionDAL.RegionName = region.RegionName;
            regionDAL.DateCreate = region.DateCreate;
            regionDAL.State = region.State;
            regionDAL.RegionId = region.Id;
            regionDAL.ServCountryId = region.CountryId;
        }
        public static void VendorDALToVendorProxy(VendorProxy vendor, Vendor vendorDAL)
        {
            
            vendorDAL.CountryId = vendor.CountryId;
            vendorDAL.VendorName = vendor.VendorName;
            vendorDAL.DateCreate = vendor.DateCreate;
            vendorDAL.State = vendor.State;
            vendorDAL.VendorId = vendor.Id;
            vendorDAL.ServCountryId = vendor.CountryId;
            
        }
        public static void PlacemarkDALToPlacemarkProxy(PlacemarkProxy placemark, Placemark placemarkDAL)
        {
            placemarkDAL.Latitude = (decimal)placemark.Latitude;
            placemarkDAL.Longitude = (decimal)placemark.Longitude;
            placemarkDAL.Description = placemark.Description;
            placemarkDAL.DateCreate = placemark.DateCreate;
            placemarkDAL.State = placemark.State;
            placemarkDAL.PlacemarkId = placemark.Id;
            placemarkDAL.ServCountryId = placemark.CountryId;
            placemarkDAL.ServRegionId = placemark.RegionId;
            placemarkDAL.ServCategoryId = placemark.CategoryId;
            placemarkDAL.ServVendorId = placemark.VendorId;
        }
        public static void FuelCategoryDALToFuelCategoryProxy(FuelCategoryProxy fuelcategory, FuelCategory fuelcategoryDAL)
        {
            fuelcategoryDAL.FuelCategoryName = fuelcategory.FuelCategoryName;
            fuelcategoryDAL.DateCreate = fuelcategory.DateCreate;
            fuelcategoryDAL.State = fuelcategory.State;
            fuelcategoryDAL.FuelCategoryId = fuelcategory.Id;
            fuelcategoryDAL.ServCountryId = fuelcategory.CountryId;
        }
        public static void PriceDALToPriceProxy(PriceProxy price, Price priceDAL)
        {
            priceDAL.PriceCost = price.PriceCost;
            priceDAL.UserPrice = price.UserPrice;
            priceDAL.dateCreate = price.dateCreate;
            priceDAL.State = price.State;
            priceDAL.PriceId = price.Id;
            priceDAL.ServPlacemarkId = price.Placemarkid;
            priceDAL.ServVendorId = price.VendorId;
            priceDAL.ServRegionId = price.RegionId;
            priceDAL.ServFuelId = price.FuelId;
        }
        public static void UserDALToUserProxy(UserProxy user, User userDAL)
        {

           
            userDAL.FirstName = user.FirstName;
            userDAL.LastName = user.LastName;
            userDAL.UserEmail = user.UserEmail;
            userDAL.UserPassword = user.UserPassword;
            userDAL.Nickname = user.Nickname;
            userDAL.ImgPath = user.ImgPath;
            userDAL.DateRegister = user.DateRegister;
            userDAL.DateCreate = user.DateCreate;
            userDAL.State = user.State;
            userDAL.UserId = user.Id;
            userDAL.ServRegionId = user.RegionId;
            userDAL.ServUnitDistanceId = user.UnitDistanceId;
            userDAL.ServUnitVolumeId = user.UnitVolumeId;
            userDAL.ServUnitFuelConsumptionId = user.UnitFuelConsumptionId;

        }
        public static void UnitFuelConsumptionDALToUnitFuelConsumptionProxy(UnitFuelConsumptionProxy unitFuelConsumption, UnitFuelConsumption unitFuelConsumptionDAL)
        {

            unitFuelConsumptionDAL.NameUnit = unitFuelConsumption.NameUnit;
            unitFuelConsumptionDAL.Correction = (decimal)unitFuelConsumption.Correction;
            unitFuelConsumptionDAL.UnitFuelConsumptionId = unitFuelConsumption.Id;
        }
        public static void UnitDistanceDALToUnitDistanceProxy(UnitDistanceProxy unitDistance, UnitDistance unitDistanceDAL)
        {

            unitDistanceDAL.NameUnit = unitDistance.NameUnit;
            unitDistanceDAL.Correction = (decimal)unitDistance.Correction;
            unitDistanceDAL.ServUnitFuelConsumptionId = unitDistance.UnitFuelConsumptionId;
            unitDistanceDAL.UnitDistanceId = unitDistance.Id;

        }
        public static void UnitVolumeDALToUnitVolumeProxy(UnitVolumeProxy unitVolume, UnitVolume unitVolumeDAL)
        {

            unitVolumeDAL.NameUnit = unitVolume.NameUnit;
            unitVolumeDAL.Correction = (decimal)unitVolume.Correction;
            unitVolumeDAL.ServUnitFuelConsumptionId = unitVolume.UnitFuelConsumptionId;
            unitVolumeDAL.UnitVolumeId = unitVolume.Id;
        }
        public static void VendorAliasDALToVendorAliasProxy(VendorAliasProxy vendorAlias, VendorAlias vendorAliasDAL)
        {

            vendorAliasDAL.ServVendorId = vendorAlias.VendorId;
            vendorAliasDAL.ServAliasId = vendorAlias.AliasId;
            vendorAliasDAL.VendorAliasId = vendorAlias.Id;

        }

        public static void AliasDALToAliasProxy(AliasProxy alias, Alias aliasDAL)
        {

            aliasDAL.AliasName = alias.AliasName;
            aliasDAL.AliasId = alias.Id;
        }
        public static void CarTypeDALToCarTypeProxy(CarTypeProxy carType, CarType carTypeDAL)
        {
            
            carTypeDAL.CarTypeName = carType.CarTypeName;
            carTypeDAL.CarTypeId = carType.Id;
        }
        public static void CarBrandDALToCarBrandProxy(CarBrandProxy carBrand, CarBrand carBrandDAL)
        {

            carBrandDAL.ServCarTypeId = carBrand.CarTypeId;
            carBrandDAL.Brand = carBrand.Brand;
            carBrandDAL.CarBrandId = carBrand.Id;
        }
        public static void CarModelDALToCarModelProxy(CarModelProxy carModel, CarModel carModelDAL)
        {

            carModelDAL.ServBrandId = carModel.BrandId;
            carModelDAL.model = carModel.model;
            carModelDAL.CarModelId = carModel.Id;
        }
        public static void CarModificationDALToCarModificationProxy(CarModificationProxy carModification, CarModification carModificationDAL)
        {

            carModificationDAL.ServModelId = carModification.ModelId;
            carModificationDAL.Modification = carModification.Modification;
            carModificationDAL.HorsePower = carModification.HorsePower;
            carModificationDAL.YearBegin = carModification.YearBegin;
            carModificationDAL.YearEnd = carModification.YearEnd;
            carModificationDAL.CarModificationId = carModification.Id;
        }
        public static void CarDALToCarProxy(CarProxy car, Car carDAL)
        {

           
            carDAL.Odometer = car.Odometer;
            carDAL.Tank = car.Tank ;
            carDAL.Consumption = car.Consumption;
            carDAL.ImgPath = car.ImgPath;
            carDAL.DateBuy = car.DateBuy ;
            carDAL.DateCreate = car.DateCreate;
            carDAL.State = car.State;
            carDAL.ServUserId = car.UserId;
            carDAL.ServModelId = car.ModelId;
            carDAL.ServModificationId = car.ModificationId;
            carDAL.ServFuelCategoryId = car.FuelCategoryId;
            carDAL.ServCarTransmissionID = car.CarTransmissionID;
            carDAL.Rating = car.Rating;
            carDAL.CarId = car.Id;
        }
        public static void CarTransmissionDALToCarTransmissionProxy(CarTransmissionProxy carTransmission, CarTransmission carTransmissionDAL)
        {

            carTransmissionDAL.NameTransmission = carTransmission.NameTransmission;
            carTransmissionDAL.CarTransmissionId = carTransmission.Id;
        }
        public static void CarNodeDALToCarNodeProxy(CarNodeProxy carNode, CarNode carNodeDAL)
        {

            carNodeDAL.Name = carNode.Name;
            carNodeDAL.CarNodeId = carNode.Id;
        }
        public static void CarDetailDALToCarDetailProxy(CarDetailProxy carDetail, CarDetail carDetailDAL)
        {

            carDetailDAL.ServCarId = carDetail.CarId;
            carDetailDAL.ServCarNodeId = carDetail.CarNodeId;
            carDetailDAL.CarDetailId = carDetail.Id;
        }
        public static void EventTypeDALToEventTypeProxy(EventTypeProxy eventType, EventType eventTypeDAL)
        {

            eventTypeDAL.Name = eventType.Name;
            eventTypeDAL.EventTypeId = eventType.Id;
        }
        public static void CarEventDALToCarEventProxy(CarEventProxy carEvent, CarEvent carEventDAL)
        {

            carEventDAL.ServCarId = carEvent.CarId;
            carEventDAL.ServEventTypeId = carEvent.EventTypeId;
            carEventDAL.ServFuelCategoryId = carEvent.FuelCategoryId;
            carEventDAL.Odometer = carEvent.Odometer;
            carEventDAL.CostTotal = (decimal)carEvent.CostTotal;
            carEventDAL.DateEvent = carEvent.DateEvent;
            carEventDAL.Comment = carEvent.Comment;
            carEventDAL.FuelCount = carEvent.FuelCount;
            carEventDAL.PricePerLitr = carEvent.PricePerLitr;
            carEventDAL.Latitute = carEvent.Latitute;
            carEventDAL.Longitute = carEvent.Longitute;
            carEventDAL.IsFullTank = carEvent.IsFullTank;
            carEventDAL.IsMissedFilling = carEvent.IsMissedFilling;
            carEventDAL.NameEvent = carEvent.NameEvent;
            carEventDAL.DateCreate = carEvent.DateCreate;
            carEventDAL.State = carEvent.State;
            carEventDAL.CarEventId = carEvent.Id;
        }

        public static void LogBookDALToLogBookProxy(LogBookProxy logBook, LogBook logBookDAL)
        {

            logBookDAL.ServCategoryLogid = logBook.CategoryLogid;
            logBookDAL.ServChargingid = logBook.Chargingid;
            logBookDAL.ServCarId = logBook.CarId;
            logBookDAL.Total_cost = logBook.Total_cost;
            logBookDAL.DateCharging = logBook.DateCharging;
            logBookDAL.LogBookId = logBook.Id;
        }
        public static void CategoryLogDALToCategoryLogProxy(CategoryLogProxy categoryLog, CategoryLog categoryLogDAL)
        {
          
            categoryLogDAL.CategoryLogName = categoryLog.CategoryLogName;
            categoryLogDAL.CategoryLogId = categoryLog.Id;
        }
    }
}
