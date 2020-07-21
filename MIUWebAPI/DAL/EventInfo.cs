using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.DAL
{
    public class EventInfo
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Venue { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public EventDetailInfo EventDetailInfo { get; set; }
    }

    public class EventDetailInfo
    {
        public int ID { get; set; }
        public Nullable<int> EventID { get; set; }
        public string FileName { get; set; }
        public string IsMain { get; set; }
    }
}