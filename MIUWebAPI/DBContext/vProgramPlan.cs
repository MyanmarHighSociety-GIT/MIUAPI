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
    
    public partial class vProgramPlan
    {
        public int TermID { get; set; }
        public int BatchID { get; set; }
        public string TermCode { get; set; }
        public string TermName { get; set; }
        public int TermType { get; set; }
        public string ExternalLink { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> CompletionDate { get; set; }
        public int TermSortOrder { get; set; }
        public int TermDetailID { get; set; }
        public int ModuleID { get; set; }
        public int sortOrder { get; set; }
        public int LectureID { get; set; }
        public Nullable<System.DateTime> FirstSubmission { get; set; }
        public Nullable<System.DateTime> FirstResult { get; set; }
        public Nullable<System.DateTime> FinalSubmission { get; set; }
        public Nullable<System.DateTime> FinalResult { get; set; }
        public string Remark { get; set; }
        public Nullable<int> DetailCount { get; set; }
        public Nullable<int> StudentCount { get; set; }
        public Nullable<int> AttendentRate { get; set; }
        public Nullable<bool> Tick1 { get; set; }
        public Nullable<int> Pass1 { get; set; }
        public Nullable<System.DateTime> FirstResultIssue { get; set; }
        public Nullable<bool> Tick2 { get; set; }
        public Nullable<int> Pass2 { get; set; }
        public Nullable<System.DateTime> FinalResultIssue { get; set; }
    }
}
