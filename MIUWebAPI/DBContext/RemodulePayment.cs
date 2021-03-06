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
    
    public partial class RemodulePayment
    {
        public int ID { get; set; }
        public int StudentID { get; set; }
        public int CourseID { get; set; }
        public int BatchID { get; set; }
        public int RemoduleID { get; set; }
        public string Amount { get; set; }
        public System.DateTime PaymentReceivedDate { get; set; }
        public string Remark { get; set; }
        public Nullable<int> RemoduleBatchID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    
        public virtual Batch Batch { get; set; }
        public virtual Batch Batch1 { get; set; }
        public virtual Course Course { get; set; }
        public virtual Module Module { get; set; }
        public virtual User User { get; set; }
    }
}
