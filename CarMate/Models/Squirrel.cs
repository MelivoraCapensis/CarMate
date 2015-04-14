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
        public string Adress { set; get; }
        public string Category { set; get; }
        public string Radius { set; get; }
        public double?[] Prices { set; get; }
        public string[] FuelCategories { set; get; }
        public string Gascheaper { set; get; }
        public string[] ChekedFuelCategories { set; get; }      
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
