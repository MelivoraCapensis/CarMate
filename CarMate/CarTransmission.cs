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
    
    public partial class CarTransmission
    {
        public CarTransmission()
        {
            this.Cars = new HashSet<Cars>();
        }
    
        public int Id { get; set; }
        public string NameTransmission { get; set; }
    
        public virtual ICollection<Cars> Cars { get; set; }
    }
}
