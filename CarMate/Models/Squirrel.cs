using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarMate.Models
{
    public  class Squirrel:IEquatable<Squirrel>
    {
        public string Lat { set; get; }
        public string Long { set; get; }
        public string Vendore { set; get; }
        public string ImagePath { set; get; }
        public string Country { set; get; }
        public string Category { set; get; }
        public string Url { set; get; }
        public string NameApi { set; get; }
        public string StreetRoute { set; get; }
        public string StreetNumber { set; get; }
        public string Adress { set; get; }
        public string City { set; get; }
        public string Region { set; get; }
        public string Radius { set; get; }
        public string[] Prices { set; get; }
        public string[] FuelCategories { set; get; }
        public List<System.Web.Mvc.SelectListItem> PoinCat { set; get; }
        public int SeelctedCat { set; get; }
        public List<System.Web.Mvc.SelectListItem> RadiusCat { set; get; }
        public int SeelctedRad { set; get; }
        public CountryModel CountriesModel { set; get; }
        public bool Equals(Squirrel other)
        {
            if (this.Lat == other.Lat & this.Long == other.Long)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
