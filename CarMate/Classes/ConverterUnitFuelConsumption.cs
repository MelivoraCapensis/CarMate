using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarMate.Classes
{
    public static class ConverterUnitFuelConsumption
    {
        public static double ConverterFuelConsumptionLoad(string unitName, double fuelConsumption)
        {
            if (String.IsNullOrEmpty(unitName))
                throw new ArgumentNullException();
            switch (unitName)
            {
                case Consts.UnitFuelConsumptionNameLitersOn100Km:
                    break;
                case Consts.UnitFuelConsumptionNameMileOnGallon:
                    return ConvertLitersOn100KmToMileOnGallon(fuelConsumption);
                //default:
                //    return distance;
                //    break;
            }
            return fuelConsumption;
        }

        public static double ConverterFuelConsumptionSave(string unitName, double fuelConsumption)
        {
            if (String.IsNullOrEmpty(unitName))
                throw new ArgumentNullException();
            
            switch (unitName)
            {
                case Consts.UnitFuelConsumptionNameLitersOn100Km:
                    break;
                case Consts.UnitFuelConsumptionNameMileOnGallon:
                    return ConvertMileOnGallonToLitersOn100Km(fuelConsumption);
                //default:
                //    return distance;
                //    break;
            }
            return fuelConsumption;
        }

        public static double ConvertLitersOn100KmToMileOnGallon(double fuelConsumption)
        {
            using (CarMateEntities db = new CarMateEntities())
            {
                var unitFuelConsumption =
                    db.UnitFuelConsumption.FirstOrDefault(
                        x => x.NameUnit.Equals(Consts.UnitFuelConsumptionNameMileOnGallon));

                if (unitFuelConsumption != null)
                {
                    var modifiedfuelConsumption = unitFuelConsumption.Correction/fuelConsumption;
                    return modifiedfuelConsumption;
                }
            }
            throw new NullReferenceException();
            //return distance;
        }

        public static double ConvertMileOnGallonToLitersOn100Km(double fuelConsumption)
        {
            using (CarMateEntities db = new CarMateEntities())
            {
                var unitFuelConsumption =
                    db.UnitFuelConsumption.FirstOrDefault(
                        x => x.NameUnit.Equals(Consts.UnitFuelConsumptionNameMileOnGallon));

                if (unitFuelConsumption != null)
                {
                    var modifiedfuelConsumption = unitFuelConsumption.Correction/fuelConsumption;
                    return modifiedfuelConsumption;
                }
            }
            throw new NullReferenceException();
            //return distance;
        }
    }
}