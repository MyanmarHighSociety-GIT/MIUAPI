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
    
    public partial class RecommendedEbook
    {
        public int ID { get; set; }
        public Nullable<int> StudyResourceID { get; set; }
        public string Name { get; set; }
        public Nullable<int> IsFolder { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string Remark { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string FileSize { get; set; }
    
        public virtual StudyResource StudyResource { get; set; }
    }
}
