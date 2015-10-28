using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WcfServiceDb
{
    public static class DataConverter
    {       
        public static void CategoryDALToCategoryProxy(CategoryProxy category, Categories categoryDAL)
        {
            category.Id = categoryDAL.Id;
            category.CategoryName = categoryDAL.Category;
            category.DateCreate = categoryDAL.DateCreate;
            category.State = categoryDAL.State;
            category.CategoryId = categoryDAL.Id;
 
        }
        public static void CountryDALToContryProxy(CountryProxy country, Countries countryDAL)
        {
            country.Id = countryDAL.Id;
            country.CountryName = countryDAL.Country;
            country.DateCreate = countryDAL.DateCreate;
            country.State = countryDAL.State;
            country.CountryId = countryDAL.Id;
        }
        public static void RegionDALToRegionProxy(RegionProxy region, Regions regionDAL)
        {
            region.Id = regionDAL.Id;
            region.CountryId = regionDAL.CountryId;
            region.RegionName = regionDAL.Region;
            region.DateCreate = regionDAL.DateCreate;
            region.State = regionDAL.State;
            region.RegionId = regionDAL.Id;
        }
        public static void VendorDALToVendorProxy(VendorProxy vendor, Vendors vendorDAL)
        {
            vendor.Id = vendorDAL.id;
            vendor.CountryId = vendorDAL.countryId;
            vendor.VendorName = vendorDAL.vendor;
            vendor.DateCreate = vendorDAL.dateCreate;
            vendor.State = vendorDAL.state;
            vendor.VendorId = vendorDAL.id;
        }
        public static void PlacemarkDALToPlacemarkProxy(PlacemarkProxy placemark, Placemarks placemarkDAL)
        {
            placemark.Id = placemarkDAL.Id;
            placemark.CountryId = placemarkDAL.CountryId;
            placemark.RegionId = placemarkDAL.RegionId == null ? 1 : (int)placemarkDAL.RegionId;
            placemark.CategoryId = placemarkDAL.CategoryId;
            placemark.VendorId = placemarkDAL.VendorId == null ? 1 : (int)placemarkDAL.VendorId;
            placemark.Latitude = (decimal)placemarkDAL.Latitude;
            placemark.Longitude = (decimal)placemarkDAL.Longitude;
            placemark.Description = placemarkDAL.Description;
            placemark.DateCreate = placemarkDAL.DateCreate;
            placemark.State = placemarkDAL.State;
            placemark.PlacemarkId = placemarkDAL.Id;
        }
        public static void FuelCategoryDALToFuelCategoryProxy(FuelCategoryProxy fuelcategory,FuelCategories fuelcategoryDAL)
        {
            fuelcategory.Id = fuelcategoryDAL.Id;
            fuelcategory.CountryId = fuelcategoryDAL.CountryId;
            fuelcategory.FuelCategoryName = fuelcategoryDAL.Category;
            fuelcategory.DateCreate = fuelcategoryDAL.DateCreate;
            fuelcategory.State = fuelcategoryDAL.State;
            fuelcategory.FuelCategoryId = fuelcategoryDAL.Id;
        }
        public static void PriceDALToPriceProxy(PriceProxy price, Prices priceDAL)
        {
            price.Id = priceDAL.Id;
            price.Placemarkid = priceDAL.PlacemarkId;
            price.VendorId = priceDAL.VendorId;
            price.RegionId = priceDAL.RegionId;
            price.FuelId = priceDAL.FuelId;
            price.PriceCost = priceDAL.Price == null ? 0 : (decimal)priceDAL.Price;
            price.UserPrice = priceDAL.UserPrice == null ? 0 : (decimal)priceDAL.UserPrice;
            price.dateCreate =priceDAL.DateCreate;
            price.State =priceDAL.State;
            price.PriceId = priceDAL.Id;
        }
        public static void UserDALToUserProxy(UserProxy user, Users userDAL)
        {
            user.Id = userDAL.Id;
            user.RegionId = userDAL.RegionId == null ? 1 : (int)userDAL.RegionId;
            user.FirstName = userDAL.FirstName;
            user.LastName = userDAL.LastName;
            user.UserEmail = userDAL.UserEmail;
            user.UserPassword = userDAL.UserPassword;
            user.Nickname = userDAL.Nickname;
            user.ImgPath = userDAL.ImgPath;
            user.DateRegister = userDAL.DateRegister;
            user.DateCreate = userDAL.DateCreate;
            user.State = userDAL.State;
            user.UnitDistanceId = userDAL.UnitDistanceId;
            user.UnitVolumeId = userDAL.UnitVolumeId;
            user.UnitFuelConsumptionId = userDAL.UnitFuelConsumptionId;

            user.UserId = userDAL.Id;
        }
        public static void UnitFuelConsumptionDALToUnitFuelConsumptionProxy(UnitFuelConsumptionProxy unitFuelConsumption, UnitFuelConsumption unitFuelConsumptionDAL)
        {
            unitFuelConsumption.Id = unitFuelConsumptionDAL.Id;
            unitFuelConsumption.NameUnit = unitFuelConsumptionDAL.NameUnit;
            unitFuelConsumption.Correction =(decimal) unitFuelConsumptionDAL.Correction;
            unitFuelConsumption.UnitFuelConsumptionId = unitFuelConsumptionDAL.Id;
        }
        public static void UnitDistanceDALToUnitDistanceProxy(UnitDistanceProxy unitDistance, UnitDistance unitDistanceDAL)
        {
            unitDistance.Id = unitDistanceDAL.Id;
            unitDistance.NameUnit = unitDistanceDAL.NameUnit;
            unitDistance.Correction = (decimal)unitDistanceDAL.Correction;
            unitDistance.UnitFuelConsumptionId = unitDistanceDAL.UnitFuelConsumptionId;
            unitDistance.UnitDistanceId = unitDistanceDAL.Id;

        }
        public static void UnitVolumeDALToUnitVolumeProxy(UnitVolumeProxy unitVolume, UnitVolume unitVolumeDAL)
        {
            unitVolume.Id = unitVolumeDAL.Id;
            unitVolume.NameUnit = unitVolumeDAL.NameUnit;
            unitVolume.Correction = (decimal)unitVolumeDAL.Correction;
            unitVolume.UnitFuelConsumptionId = unitVolumeDAL.UnitFuelConsumptionId;
            unitVolume.UnitVolumeId = unitVolumeDAL.Id;
        }
        public static void VendorAliasDALToVendorAliasProxy(VendorAliasProxy vendorAlias, VendorAliases vendorAliasDAL)
        {
            vendorAlias.Id = vendorAliasDAL.Id;
            vendorAlias.VendorId = vendorAliasDAL.VendorId;
            vendorAlias.AliasId = vendorAliasDAL.AliasId;
            vendorAlias.VendorAliasId = vendorAliasDAL.Id;
            
        }
       
        public static void AliasDALToAliasProxy(AliasProxy alias,Aliases aliasDAL)
        {
            alias.Id=aliasDAL.Id;
            alias.AliasName = aliasDAL.Alias;
            alias.AliasId = aliasDAL.Id;
        }
        public static void CarTypeDALToCarTypeProxy(CarTypeProxy carType, CarTypes carTypeDAL)
        {
            carType.Id = carTypeDAL.Id;
            carType.CarTypeName = carTypeDAL.CarType;
            carType.CarTypeId = carTypeDAL.Id;
        }
        public static void CarBrandDALToCarBrandProxy(CarBrandProxy carBrand, CarBrands carBrandDAL)
        {
            carBrand.Id = carBrandDAL.Id;
            carBrand.CarTypeId = carBrandDAL.CarTypeId;
            carBrand.Brand = carBrandDAL.Brand;
            carBrand.CarBrandId = carBrandDAL.Id;
        }
        public static void CarModelDALToCarModelProxy(CarModelProxy carModel, CarModels carModelDAL)
        {
            carModel.Id = carModelDAL.Id;
            carModel.BrandId = carModelDAL.BrandId;
            carModel.model = carModelDAL.Model;
            carModel.CarModelId = carModelDAL.Id;
        }
        public static void CarModificationDALToCarModificationProxy(CarModificationProxy carModification, CarModifications carModificationDAL)
        {
            carModification.Id = carModificationDAL.Id;
            carModification.ModelId = carModificationDAL.ModelId;
            carModification.Modification = carModificationDAL.Modification;
            carModification.HorsePower = carModificationDAL.HorsePower;
            carModification.YearBegin = carModificationDAL.YearBegin;
            carModification.YearEnd = carModificationDAL.YearEnd;
            carModification.CarModificationId = carModificationDAL.Id;
        }
        public static void CarDALToCarProxy(CarProxy car, Cars carDAL)
        {
            car.Id = carDAL.Id;
            car.UserId = carDAL.UserId;
            car.ModelId = carDAL.ModelId;
            car.ModificationId = carDAL.ModificationId == null ? 1 : (int)carDAL.ModificationId;
            car.Odometer = carDAL.Odometer == null ? 0 : (int)carDAL.Odometer;
            car.Tank = carDAL.Tank == null ? 0 :(int) carDAL.Tank;
            car.Consumption = carDAL.Consumption == null ? 0 : (int)carDAL.Consumption;
            car.ImgPath = carDAL.ImgPath;
            car.DateBuy = carDAL.DateBuy == null ? DateTime.Now : (DateTime)carDAL.DateBuy;
            car.DateCreate = carDAL.DateCreate;
            car.State = carDAL.State;
            car.FuelCategoryId = carDAL.FuelCategoryId == null ? 1 : (int)carDAL.FuelCategoryId;
            car.CarTransmissionID = carDAL.CarTransmissionId == null ? 1 : (int)carDAL.CarTransmissionId;
            car.Rating = carDAL.Rating == null ? 1 : (int)carDAL.Rating;
            car.CarId = carDAL.Id;
        }
        public static void CarTransmissionDALToCarTransmissionProxy(CarTransmissionProxy carTransmission, CarTransmission carTransmissionDAL)
        {
            carTransmission.Id = carTransmissionDAL.Id;
            carTransmission.NameTransmission = carTransmissionDAL.NameTransmission;
            carTransmission.CarTransmissionId = carTransmissionDAL.Id;
        }
        public static void CarNodeDALToCarNodeProxy(CarNodeProxy carNode, CarNode carNodeDAL)
        {
            carNode.Id = carNodeDAL.Id;
            carNode.Name = carNodeDAL.Name;
            carNode.CarNodeId = carNodeDAL.Id;
        }
        public static void CarDetailDALToCarDetailProxy(CarDetailProxy carDetail, CarDetails carDetailDAL)
        {
            carDetail.Id = carDetailDAL.Id;
            carDetail.CarId = carDetailDAL.CarId;            
            carDetail.CarNodeId = carDetailDAL.CarNodeId;
            carDetail.CarDetailId = carDetailDAL.Id;
        }
        public static void EventTypeDALToEventTypeProxy(EventTypeProxy eventType, EventTypes eventTypeDAL)
        {
            eventType.Id = eventTypeDAL.Id;
            eventType.Name = eventTypeDAL.Name;
            eventType.EventTypeId = eventTypeDAL.Id;
        }
        public static void CarEventDALToCarEventProxy(CarEventProxy carEvent, CarEvents carEventDAL)
        {
            carEvent.Id = carEventDAL.Id;
            carEvent.CarId = carEventDAL.CarId;
            carEvent.EventTypeId = carEventDAL.EventTypeId;
            carEvent.FuelCategoryId = carEventDAL.FuelCategoryId == null ? 1 : (int)carEventDAL.FuelCategoryId;
            carEvent.Odometer = carEventDAL.Odometer == null ? 0 : (int)carEventDAL.Odometer;
            carEvent.CostTotal =(decimal) carEventDAL.CostTotal;
            carEvent.DateEvent = carEventDAL.DateEvent;
            carEvent.Comment = carEventDAL.Comment;
            carEvent.FuelCount =carEventDAL.FuelCount==null? 0:(decimal)carEventDAL.FuelCount;
            carEvent.PricePerLitr = carEventDAL.PricePerLitr == null ? 1 : (decimal)carEventDAL.PricePerLitr;
            carEvent.Latitute = carEventDAL.Latitute == null ? 0 : (decimal)carEventDAL.Latitute;
            carEvent.Longitute = carEventDAL.Longitute == null ? 0 : (decimal)carEventDAL.Longitute;
            carEvent.IsFullTank = carEventDAL.IsFullTank == true ? 1 : 0;
            carEvent.IsMissedFilling = carEventDAL.IsMissedFilling==true ? 1:0;
            carEvent.NameEvent = carEventDAL.NameEvent;
            carEvent.DateCreate = carEventDAL.DateCreate;
            carEvent.State = carEventDAL.State;
            carEvent.CarEventId = carEventDAL.Id;
        }
       
        public static void LogBookDALToLogBookProxy(LogBookProxy logBook, LogBook logBookDAL)
        {
            logBook.Id = logBookDAL.Id;
            logBook.CategoryLogid = logBookDAL.CategoryId;
            logBook.Chargingid = logBookDAL.ChargingId;
            logBook.CarId = logBookDAL.CarId;
            logBook.Total_cost = logBookDAL.TotalCost == null ? 0 : (decimal)logBookDAL.TotalCost;
            logBook.DateCharging = logBookDAL.DateCharging == null ? DateTime.Now : (DateTime)logBookDAL.DateCharging;
            logBook.LogBookId = logBookDAL.Id;
        }
        public static void CategoryLogDALToCategoryLogProxy(CategoryLogProxy categoryLog, CategoryLog categoryLogDAL)
        {
            categoryLog.Id = categoryLogDAL.Id;
            categoryLog.CategoryLogName = categoryLogDAL.Category;
            categoryLog.CategoryLogId = categoryLogDAL.Id;
        }
    }
}
