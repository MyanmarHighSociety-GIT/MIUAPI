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
    
    public partial class TemplateDetailSub
    {
        public int ID { get; set; }
        public int TemplateDetailID { get; set; }
        public int ModuleID { get; set; }
        public bool UpdateBatch { get; set; }
        public bool ShowInProgramPlan { get; set; }
        public string Takecare { get; set; }
        public int sortOrder { get; set; }
    
        public virtual TemplateDetail TemplateDetail { get; set; }
        public virtual Module Module { get; set; }
    }
}
