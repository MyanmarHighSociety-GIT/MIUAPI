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
    
    public partial class LeadActivity
    {
        public int ID { get; set; }
        public Nullable<int> LeadID { get; set; }
        public string Action { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public string Remark { get; set; }
        public Nullable<int> RelatedTo { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedUser { get; set; }
    
        public virtual EventCalendar EventCalendar { get; set; }
        public virtual Lead Lead { get; set; }
    }
}
