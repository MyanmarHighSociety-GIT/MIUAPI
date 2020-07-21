using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class TimeTableInfo
    {
        public class LecturerTimeTable
        {
            public int LecturerID { get; set; }
            public string FullName { get; set; }
            public string LoginName { get; set; }
            public string ProfilePicture { get; set; }
            public string ProfilePicture2 { get; set; }
            public string CourseName { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public string ModuleCode { get; set; }
            public string ModuleName { get; set; }
            public DateTime? TimeTableDate { get; set; }
        }

        public class StudentTimeTable
        {
            public int TimeTableDetailID { get; set; }
            public int LectureID { get; set; }
            public string LectureName { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public string RoomID { get; set; }
            public string Section { get; set; }
            public int ModuleID { get; set; }
            public string ModuleName { get; set; }
            public string ProfilePicture { get; set; }
            public DateTime? TimeTableDate { get; set; }
            public bool? IsAttending { get; set; }
            public string Attendance { get; set; }
        }
    }
}