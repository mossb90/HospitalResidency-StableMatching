//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ResidencyMATCH
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class StableMatchEntities : DbContext
    {
        public StableMatchEntities()
            : base("name=StableMatchEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<DoctorPreference> DoctorPreferences { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<HospitalPreference> HospitalPreferences { get; set; }
        public virtual DbSet<Hospital> Hospitals { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    }
}
