using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarMate.DAL;

namespace CarMate.Classes
{
    public static class ConverterUnitDistance
    {

        public static double ConverterDistanceLoad(RepositoryProvider db, int laguageId, string unitName, double distance)
        {
            if (String.IsNullOrEmpty(unitName))
                throw new ArgumentNullException();

            unitName = db.UnitDistance.SelectAnalogue(unitName);
            
            switch (unitName)
            {
                case Consts.UnitDistanceNameKm:
                    break;
                case Consts.UnitDistanceNameMile:
                    return ConvertKilometersToMile(db, laguageId, distance);
                //default:
                //    return distance;
                //    break;
            }
            return distance;
        }

        public static double ConverterDistanceSave(RepositoryProvider db, int laguageId, string unitName, double distance)
        {
            if (String.IsNullOrEmpty(unitName))
                throw new ArgumentNullException();

            unitName = db.UnitDistance.SelectAnalogue(unitName);

            switch (unitName)
            {
                case Consts.UnitDistanceNameKm:
                    break;
                case Consts.UnitDistanceNameMile:
                    return ConvertMileToKilometers(db, laguageId, distance);
                //default:
                //    return distance;
                //    break;
            }
            return distance;
        }



        public static double ConvertKilometersToMile(RepositoryProvider db, int laguageId, double distance)
        {
            //using (CarMateEntities db = new CarMateEntities())
            //{
                var correctionUnitDistance =
                    //db.UnitDistance.FirstOrDefault(x => x.NameUnit.Equals(Consts.UnitDistanceNameMile));
                    db.UnitDistance.Select(Consts.RussianLanguage).FirstOrDefault(x => x.NameUnit.Equals(Consts.UnitDistanceNameMile));

                if (correctionUnitDistance != null)
                {
                    var modifiedDistance = correctionUnitDistance.Correction * distance;
                    return modifiedDistance;
                }
                
            //}
            throw new NullReferenceException();
            //return distance;
        }

        public static double ConvertMileToKilometers(RepositoryProvider db, int laguageId, double distance)
        {
            //using (CarMateEntities db = new CarMateEntities())
            //{
                var correctionUnitDistance =
                    //db.UnitDistance.FirstOrDefault(x => x.NameUnit.Equals(Consts.UnitDistanceNameMile));
                    db.UnitDistance.Select(Consts.RussianLanguage).FirstOrDefault(x => x.NameUnit.Equals(Consts.UnitDistanceNameMile));

                if (correctionUnitDistance != null)
                {
                    var modifiedDistance = distance/correctionUnitDistance.Correction;
                    return modifiedDistance;
                }
            //}
            throw new NullReferenceException();
            //return distance;
        }
    }
}