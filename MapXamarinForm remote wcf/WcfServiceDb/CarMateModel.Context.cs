﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CarMateEntities : DbContext
    {
        public CarMateEntities()
            : base("name=CarMateEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Aliases> Aliases { get; set; }
        public virtual DbSet<CarBrands> CarBrands { get; set; }
        public virtual DbSet<CarDetails> CarDetails { get; set; }
        public virtual DbSet<CarEvents> CarEvents { get; set; }
        public virtual DbSet<CarModels> CarModels { get; set; }
        public virtual DbSet<CarModifications> CarModifications { get; set; }
        public virtual DbSet<CarNode> CarNode { get; set; }
        public virtual DbSet<Cars> Cars { get; set; }
        public virtual DbSet<CarTransmission> CarTransmission { get; set; }
        public virtual DbSet<CarTypes> CarTypes { get; set; }
        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<CategoryLog> CategoryLog { get; set; }
        public virtual DbSet<Countries> Countries { get; set; }
        public virtual DbSet<EventTypes> EventTypes { get; set; }
        public virtual DbSet<FuelCategories> FuelCategories { get; set; }
        public virtual DbSet<LogBook> LogBook { get; set; }
        public virtual DbSet<Placemarks> Placemarks { get; set; }
        public virtual DbSet<Prices> Prices { get; set; }
        public virtual DbSet<Regions> Regions { get; set; }
        public virtual DbSet<UnitDistance> UnitDistance { get; set; }
        public virtual DbSet<UnitFuelConsumption> UnitFuelConsumption { get; set; }
        public virtual DbSet<UnitVolume> UnitVolume { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<VendorAliases> VendorAliases { get; set; }
        public virtual DbSet<Vendors> Vendors { get; set; }
    }
}