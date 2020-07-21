using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class FeedbackInfo
    {

        public class FeedbackReasonList
        {
            public int ID { get; set; }
            public string Reason { get; set; }
        }

        public class StudentFeedbackInfo
        {
            public int ID { get; set; }
            public int ReasonID { get; set; }
            public string Comment { get; set; }
            public int React { get; set; }
            public string CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string ModifiedBy { get; set; }
            public Nullable<System.DateTime> ModifiedDate { get; set; }
        }

    }
}