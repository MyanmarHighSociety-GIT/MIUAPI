using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class BatchInfo
    {
        public int ID { get; set; }
        public int CourseID { get; set; }
        public int TemplateID { get; set; }
        public string BatchCode { get; set; }
        public string BatchName { get; set; }
        public Nullable<int> ProgramManagerID { get; set; }
        public string Remark { get; set; }
        public string Year { get; set; }
        public string Intake { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<bool> IsDelete { get; set; }
    }
}