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
    using System.Collections.Generic;
    
    public partial class DoctorPreference
    {
        public int DoctorID { get; set; }
        public int ChoiceHospital1 { get; set; }
        public Nullable<int> ChoiceHospital2 { get; set; }
        public Nullable<int> ChoiceHospital3 { get; set; }
        public Nullable<int> ChoiceHospital4 { get; set; }
        public Nullable<int> ChoiceHospital5 { get; set; }
    
        public virtual Doctor Doctor { get; set; }
    }
}
