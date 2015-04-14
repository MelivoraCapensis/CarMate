using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarMate.Models
{
    public class VendorItem:IEquatable<VendorItem>
    {
        public string Name { set; get; }
        public string[] Categories { set; get; }
        public double?[] Prices { set; get; }
        public bool Equals(VendorItem other)
        {
            if (this.Name == other.Name)
            {
                return true;
            }
            else 
            {
                return false;
            }

        }
    }
    public class CheaperFuelItem : IEquatable<CheaperFuelItem>,IComparable
    {
        public string Name { set; get; }
        public double Price { set; get; }
        public bool Equals(CheaperFuelItem other)
        {
            if (this.Name == other.Name)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;
            CheaperFuelItem otherCheaperFuelItem = obj as CheaperFuelItem;
            if (otherCheaperFuelItem != null)
                return this.Price.CompareTo(otherCheaperFuelItem.Price);
            else return 1;
        }

    }
}