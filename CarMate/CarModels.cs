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
    
    public partial class CarModels
    {
        public CarModels()
        {
            this.Cars = new HashSet<Cars>();
            this.CarModifications = new HashSet<CarModifications>();
        }
    
        public int Id { get; set; }
        public int BrandId { get; set; }
        public string Model { get; set; }
    
        public virtual CarBrands CarBrands { get; set; }
        public virtual ICollection<Cars> Cars { get; set; }
        public virtual ICollection<CarModifications> CarModifications { get; set; }
    }
}
