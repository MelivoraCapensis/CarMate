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
    
    public partial class LogBook
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int ChargingId { get; set; }
        public int CarId { get; set; }
        public Nullable<float> TotalCost { get; set; }
        public Nullable<System.DateTime> DateCharging { get; set; }
    
        public virtual CarEvents CarEvents { get; set; }
        public virtual CategoryLog CategoryLog { get; set; }
        public virtual Cars Cars { get; set; }
    }
}
