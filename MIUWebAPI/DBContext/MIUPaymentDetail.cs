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
    
    public partial class MIUPaymentDetail
    {
        public int ID { get; set; }
        public int MIUPaymentID { get; set; }
        public int TermID { get; set; }
        public string Amount { get; set; }
        public Nullable<System.DateTime> PaymentDueDate { get; set; }
        public string PaymentStatus { get; set; }
        public Nullable<System.DateTime> PaymentReceivedDate { get; set; }
        public string Benefit { get; set; }
        public string Remark { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    
        public virtual MIUPayment MIUPayment { get; set; }
        public virtual Term Term { get; set; }
    }
}
