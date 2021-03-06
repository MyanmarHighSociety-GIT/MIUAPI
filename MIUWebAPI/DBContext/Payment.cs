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
    
    public partial class Payment
    {
        public int ID { get; set; }
        public Nullable<int> StudentID { get; set; }
        public Nullable<int> BatchID { get; set; }
        public Nullable<int> TermID { get; set; }
        public Nullable<System.DateTime> PaidDate { get; set; }
        public string VoucherNo { get; set; }
        public string PaidAmount { get; set; }
        public Nullable<int> Discount { get; set; }
        public string TotalAmount { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    
        public virtual Term Term { get; set; }
        public virtual Batch Batch { get; set; }
        public virtual User User { get; set; }
    }
}
