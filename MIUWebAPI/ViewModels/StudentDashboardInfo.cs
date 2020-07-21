using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static MIUWebAPI.ViewModels.ReportAttendanceInfo;

namespace MIUWebAPI.ViewModels
{
    public class StudentDashboardInfo
    {
        //public AttRateAndPercent AttRateAndPercent { get; set; }
        public ResultInfo StudentDashboard { get; set; }
        public AnnouncementInfo Announcement { get; set; }
        public List<NewsInfo> News { get; set; }
        public List<EventCalendarInfo> EventCalendar { get; set; }
    }

    public class LectureDashboardInfo
    {
        public string Course { get; set; }
        public string Module { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public List<LectureResultInfo> LectureResultInfos { get; set; }
        public AnnouncementInfo Announcement { get; set; }
        public List<NewsInfo> News { get; set; }
        public List<EventCalendarInfo> EventCalendar { get; set; }
    }

    public class LectureResultInfo
    {
        public string Course { get; set; }
        public string Batch { get; set; }
        public string Module { get; set; }
        public string Submission { get; set; }
        public string Assessor { get; set; }
        public string IV { get; set; }
        public int Submitted { get; set; }
        public int Unpublished { get; set; }
        public int Checked { get; set; }
        public int NotChecked { get; set; }
    }

    public class LectureNextClass
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public DateTime? Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class LectureCourseListInfo
    {
        public int CourseID {get;set;}
        public string CourseName {get;set;}
    }
    public class LectureModuleListInfo
    {
        public int ID { get; set; }
        public string ModuleCode { get; set; }
    }
}