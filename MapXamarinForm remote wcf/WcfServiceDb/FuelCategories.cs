//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WcfServiceDb
{
    using System;
    using System.Collections.Generic;
    
    public partial class FuelCategories
    {
        public FuelCategories()
        {
            this.CarEvents = new HashSet<CarEvents>();
            this.Cars = new HashSet<Cars>();
            this.Prices = new HashSet<Prices>();
        }
    
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string Category { get; set; }
        public System.DateTime DateCreate { get; set; }
        public int State { get; set; }
    
        public virtual ICollection<CarEvents> CarEvents { get; set; }
        public virtual ICollection<Cars> Cars { get; set; }
        public virtual Countries Countries { get; set; }
        public virtual ICollection<Prices> Prices { get; set; }
    }
}
