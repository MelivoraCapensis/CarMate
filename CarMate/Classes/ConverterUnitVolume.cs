using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarMate.Classes
{
    public static class ConverterUnitVolume
    {
        public static double ConvertVolumeFromLiters(string unitName, double volume)
        {
            if (String.IsNullOrEmpty(unitName))
                throw new ArgumentNullException();
            switch (unitName)
            {
                case Consts.UnitVolumeNameLiter:
                    break;
                case Consts.UnitVolumeNameGallon:
                    return ConvertLitersToGallon(volume);
                //default:
                //    return distance;
                //    break;
            }
            return volume;
        }

        public static double ConvertVolumeToLiters(string unitName, double volume)
        {
            if (String.IsNullOrEmpty(unitName))
                throw new ArgumentNullException();
            switch (unitName)
            {
                case Consts.UnitVolumeNameLiter:
                    break;
                case Consts.UnitVolumeNameGallon:
                    return ConvertGallonToLiters(volume);
                //default:
                //    return distance;
                //    break;
            }
            return volume;
        }

        public static double ConvertLitersToGallon(double volume)
        {
            using (CarMateEntities db = new CarMateEntities())
            {
                var unitVolume =
                    db.UnitVolume.FirstOrDefault(
                        x => x.NameUnit.Equals(Consts.UnitVolumeNameGallon));

                if (unitVolume != null)
                {
                    var modifiedvolume = unitVolume.Correction*volume;
                    return modifiedvolume;
                }
            }
            throw new NullReferenceException();
            //return distance;
        }

        public static double ConvertGallonToLiters(double volume)
        {
            using (CarMateEntities db = new CarMateEntities())
            {
                var unitVolume =
                    db.UnitVolume.FirstOrDefault(
                        x => x.NameUnit.Equals(Consts.UnitVolumeNameGallon));

                if (unitVolume != null)
                {
                    var modifiedvolume = volume/unitVolume.Correction;
                    return modifiedvolume;
                }
            }
            throw new NullReferenceException();
            //return distance;
        }
    }
}