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
    
    public partial class AttendanceTotal
    {
        public int ID { get; set; }
        public Nullable<int> TermDetailID { get; set; }
        public Nullable<int> Total { get; set; }
    
        public virtual TermDetail TermDetail { get; set; }
    }
}
