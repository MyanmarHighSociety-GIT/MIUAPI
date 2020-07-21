using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class CourseInfo
    {
        public int ID { get; set; }
        public string CourseName { get; set; }
        public string Remark { get; set; }
        public Nullable<int> StudentCount { get; set; }
        public string Code { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string ModuleName { get; set; }
    }
}