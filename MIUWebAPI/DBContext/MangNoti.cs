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
    
    public partial class MangNoti
    {
        public int MangNotiID { get; set; }
        public Nullable<int> AssignmentSubmissionID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string Type { get; set; }
        public Nullable<bool> IsSeen { get; set; }
    
        public virtual AssignmentSubmission AssignmentSubmission { get; set; }
    }
}