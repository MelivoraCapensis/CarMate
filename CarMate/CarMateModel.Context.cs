﻿//------------------------------------------------------------------------------
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
    
        public DbSet<Aliases> Aliases { get; set; }
        public DbSet<CarBrands> CarBrands { get; set; }
        public DbSet<CarDetails> CarDetails { get; set; }
        public DbSet<CarEvents> CarEvents { get; set; }
        public DbSet<CarModels> CarModels { get; set; }
        public DbSet<CarModifications> CarModifications { get; set; }
        public DbSet<Cars> Cars { get; set; }
        public DbSet<CarTransmission> CarTransmission { get; set; }
        public DbSet<CarTransmissionLang> CarTransmissionLang { get; set; }
        public DbSet<CarTypes> CarTypes { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<CategoryLog> CategoryLog { get; set; }
        public DbSet<Countries> Countries { get; set; }
        public DbSet<EventTypes> EventTypes { get; set; }
        public DbSet<EventTypesLang> EventTypesLang { get; set; }
        public DbSet<FuelCategories> FuelCategories { get; set; }
        public DbSet<Languages> Languages { get; set; }
        public DbSet<LogBook> LogBook { get; set; }
        public DbSet<Placemarks> Placemarks { get; set; }
        public DbSet<Prices> Prices { get; set; }
        public DbSet<Regions> Regions { get; set; }
        public DbSet<UnitDistance> UnitDistance { get; set; }
        public DbSet<UnitDistanceLang> UnitDistanceLang { get; set; }
        public DbSet<UnitFuelConsumption> UnitFuelConsumption { get; set; }
        public DbSet<UnitVolume> UnitVolume { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<VendorAliases> VendorAliases { get; set; }
        public DbSet<Vendors> Vendors { get; set; }
        public DbSet<webpages_Membership> webpages_Membership { get; set; }
        public DbSet<webpages_OAuthMembership> webpages_OAuthMembership { get; set; }
        public DbSet<CarNodes> CarNodes { get; set; }
        public DbSet<CarNodesLang> CarNodesLang { get; set; }
    }
}
