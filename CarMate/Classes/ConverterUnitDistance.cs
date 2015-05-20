using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarMate.Classes
{
    public static class ConverterUnitDistance
    {

        public static double ConvertDistanceFromKm(string unitName, double distance)
        {
            if (String.IsNullOrEmpty(unitName))
                throw new ArgumentNullException();
            switch (unitName)
            {
                case Consts.UnitDistanceNameKm:
                    break;
                case Consts.UnitDistanceNameMile:
                    return ConvertKilometersToMile(distance);
                //default:
                //    return distance;
                //    break;
            }
            return distance;
        }

        public static double ConvertDistanceToKm(string unitName, double distance)
        {
            if (String.IsNullOrEmpty(unitName))
                throw new ArgumentNullException();
            switch (unitName)
            {
                case Consts.UnitDistanceNameKm:
                    break;
                case Consts.UnitDistanceNameMile:
                    return ConvertMileToKilometers(distance);
                //default:
                //    return distance;
                //    break;
            }
            return distance;
        }



        public static double ConvertKilometersToMile(double distance)
        {
            using (CarMateEntities db = new CarMateEntities())
            {
                var correctionUnitDistance =
                    db.UnitDistance.FirstOrDefault(x => x.NameUnit.Equals(Consts.UnitDistanceNameMile));

                if (correctionUnitDistance != null)
                {
                    var modifiedDistance = correctionUnitDistance.Correction * distance;
                    return modifiedDistance;
                }
                
            }
            throw new NullReferenceException();
            //return distance;
        }

        public static double ConvertMileToKilometers(double distance)
        {
            using (CarMateEntities db = new CarMateEntities())
            {
                var correctionUnitDistance =
                    db.UnitDistance.FirstOrDefault(x => x.NameUnit.Equals(Consts.UnitDistanceNameMile));

                if (correctionUnitDistance != null)
                {
                    var modifiedDistance = distance/correctionUnitDistance.Correction;
                    return modifiedDistance;
                }
            }
            throw new NullReferenceException();
            //return distance;
        }
    }
}