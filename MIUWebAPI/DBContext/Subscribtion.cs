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
    
    public partial class Subscribtion
    {
        public int ID { get; set; }
        public int NotiID { get; set; }
        public int NotiTypeID { get; set; }
        public int UserID { get; set; }
        public int CommonID { get; set; }
        public bool IsSeen { get; set; }
    
        public virtual Notification Notification { get; set; }
        public virtual NotificationType NotificationType { get; set; }
        public virtual User User { get; set; }
    }
}