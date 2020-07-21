using MIUWebAPI.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class ReportAttendanceInfo
    {
        public class AttendanceGrid
        {
            public AttendanceGrid()
            {
                TermDetail = new List<TermInfo>();
                //ModuleCodeList = new List<string>();
                MonthList = new List<int>();
                //Rows = new List<AttendanceGridRow>();
            }
            public int StudentID { get; set; }
            public string StudentName { get; set; }
            public string LoginName { get; set; }
            public string ProfilePicture { get; set; }
            public string ProfilePicture2 { get; set; }
            public string BatchName { get; set; }
            public string CourseName { get; set; }
            public string AttendanceRateTermName { get; set; }
            public int AttendanceRatePercent { get; set; }
            public List<TermInfo> TermDetail { get; set; }
            //public List<string> ModuleCodeList { get; set; }
            public List<int> MonthList { get; set; }
            //public List<AttendanceGridRow> Rows { get; set; }
        }

        public class AttendanceCell
        {
            public int Value { get; set; }
        }

        public class AttRateAndPercent
        {
            public int CurrentTermId { get; set; }
            public string AttendanceRateTermName { get; set; }
            public int AttenddanceRatePercent { get; set; }
        }
        
        public class StudentAttendance
        {
            public StudentAttendance()
            {
                //TermDetail = new List<TermInfo>();
                //ModuleCodeList = new List<string>();
                MonthList = new List<int>();
                //Rows = new List<AttendanceGridRow>();
            }
            public string AttendanceRateTermName { get; set; }
            public int AttendanceRatePercent { get; set; }
            public List<AttendanceTermInfo> AttendanceDetail { get; set; }
            //public List<string> ModuleCodeList { get; set; }
            public List<int> MonthList { get; set; }
            //public List<AttendanceGridRow> Rows { get; set; }
        }
    }
}