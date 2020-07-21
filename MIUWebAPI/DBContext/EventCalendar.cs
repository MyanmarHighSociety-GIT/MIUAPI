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
    
    public partial class EventCalendar
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EventCalendar()
        {
            this.EventCalendarOrganizers = new HashSet<EventCalendarOrganizer>();
            this.EventCalendarPhotoes = new HashSet<EventCalendarPhoto>();
            this.EventCalendarReacts = new HashSet<EventCalendarReact>();
            this.EventCalendarSpeakers = new HashSet<EventCalendarSpeaker>();
            this.LeadActivities = new HashSet<LeadActivity>();
        }
    
        public int ID { get; set; }
        public string Title { get; set; }
        public string EventVisibility { get; set; }
        public string DressCode { get; set; }
        public string Description { get; set; }
        public Nullable<bool> AllDay { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Location { get; set; }
        public Nullable<bool> RSVP { get; set; }
        public Nullable<bool> IsLimit { get; set; }
        public Nullable<int> NumberLimit { get; set; }
        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string TwitterLink { get; set; }
        public string LinkedInLink { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedUser { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EventCalendarOrganizer> EventCalendarOrganizers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EventCalendarPhoto> EventCalendarPhotoes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EventCalendarReact> EventCalendarReacts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EventCalendarSpeaker> EventCalendarSpeakers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LeadActivity> LeadActivities { get; set; }
    }
}
