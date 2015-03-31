//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarMate
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vendors
    {
        public Vendors()
        {
            this.Placemarks = new HashSet<Placemarks>();
            this.Prices = new HashSet<Prices>();
            this.VendorAliases = new HashSet<VendorAliases>();
        }
    
        public int id { get; set; }
        public int countryId { get; set; }
        public string vendor { get; set; }
        public byte[] icon { get; set; }
        public System.DateTime dateCreate { get; set; }
        public int state { get; set; }
    
        public virtual Countries Countries { get; set; }
        public virtual ICollection<Placemarks> Placemarks { get; set; }
        public virtual ICollection<Prices> Prices { get; set; }
        public virtual ICollection<VendorAliases> VendorAliases { get; set; }
    }
}
