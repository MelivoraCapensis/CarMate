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
    
    public partial class Users
    {
        public Users()
        {
            this.Cars = new HashSet<Cars>();
        }
    
        public int Id { get; set; }
        public Nullable<int> RegionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string Nickname { get; set; }
        public string ImgPath { get; set; }
        public System.DateTime DateRegister { get; set; }
        public System.DateTime DateCreate { get; set; }
        public int State { get; set; }
        public int UnitDistanceId { get; set; }
        public int UnitVolumeId { get; set; }
        public int UnitFuelConsumptionId { get; set; }
        public Nullable<int> LanguageId { get; set; }
        public Nullable<int> DefaultCarId { get; set; }
    
        public virtual ICollection<Cars> Cars { get; set; }
        public virtual Languages Languages { get; set; }
        public virtual Regions Regions { get; set; }
        public virtual UnitDistance UnitDistance { get; set; }
        public virtual UnitFuelConsumption UnitFuelConsumption { get; set; }
        public virtual UnitVolume UnitVolume { get; set; }
    }
}
