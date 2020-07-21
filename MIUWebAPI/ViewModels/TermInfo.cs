using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MIUWebAPI.ViewModels.ReportAttendanceInfo;

namespace MIUWebAPI.ViewModels
{
    public class TermInfo
    {
        public int ID { get; set; }
        public int BatchID { get; set; }
        public string TermCode { get; set; }
        public string TermName { get; set; }
        public int TermType { get; set; }
        public string YearName { get; set; }
        public bool WeekDays { get; set; }
        public bool WeekEnds { get; set; }
        public Nullable<bool> Morning { get; set; }
        public Nullable<bool> Evening { get; set; }
        public int Duration { get; set; }
        public string ExternalLink { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> CompletionDate { get; set; }
        public int sortOrder { get; set; }
        public string Remark { get; set; }
        public string ModuleCodeList { get; set; }
        public int First { get; set; }
        public int Second { get; set; }
        public int Total { get; set; }
    }

    public class AttendanceTermInfo
    {
        public int ID { get; set; }
        public int BatchID { get; set; }
        public string TermCode { get; set; }
        public string TermName { get; set; }
        public string ModuleCodeList { get; set; }
        public int First { get; set; }
        public int Second { get; set; }
        public int Total { get; set; }
    }
}