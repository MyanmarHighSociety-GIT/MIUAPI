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
    
    public partial class AnnouncementVisibility
    {
        public int ID { get; set; }
        public int AnnouncementId { get; set; }
        public string VisibilityName { get; set; }
    
        public virtual Announcement Announcement { get; set; }
    }
}
