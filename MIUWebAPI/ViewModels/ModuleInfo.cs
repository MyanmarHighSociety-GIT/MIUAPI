using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class ModuleInfo
    {
        public int ID { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public string Remark { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public string CreditUnit { get; set; }
    }

    public class LectureModuleInfo
    {
        public int ID { get; set; }
        public string ModuleCode { get; set; }
        public string ModuleName { get; set; }
    }

    public class ModuleReferenceInfo
    {       
        public string Reference { get; set; }
    }
}