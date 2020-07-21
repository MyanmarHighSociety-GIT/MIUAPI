using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class ParentStudentInfo
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public Nullable<int> StudentID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}