using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class EnquiryInfo
    {
        public int ID { get; set; }
        public string Subject { get; set; }
        public Nullable<int> PostedBy { get; set; }
        public string EnquiryDetails { get; set; }
        public Nullable<System.DateTime> Posted_DateTime { get; set; }
        public string Remarks { get; set; }
        public Nullable<int> Status { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ContactEmail { get; set; }
        public string Source { get; set; }
    }
}