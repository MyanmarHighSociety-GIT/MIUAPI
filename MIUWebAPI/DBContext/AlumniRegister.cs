//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MIUWebAPI.DBContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class AlumniRegister
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string NRIC { get; set; }
        public string StudentID { get; set; }
        public Nullable<int> Year { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string Township { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string ApplicantType { get; set; }
        public string ClassLevel { get; set; }
        public Nullable<bool> IsReceiveEmail { get; set; }
        public Nullable<bool> IsCVBook { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
