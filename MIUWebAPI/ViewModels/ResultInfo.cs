using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class ResultInfo
    {
        public string TermName { get; set; }
        public int AttendanceRate { get; set; }
        public string CourseName { get; set; }
        public string CourseStatus { get; set; }
        public string NextSubmissionDate { get; set; }
        public string CurrentModuleName { get; set; }
        public string CurrentModuleResult { get; set; }
        public DateTime? FinalSubmissionDate { get; set; }
        public List<GradingModule> GradingModuleList { get; set; }
    }
    public class GradingModule
    {
        public string ModuleCode { get; set; }
        public string ModuleStatus { get; set; }
        public int TermDetailID { get; set; }
        public int AssignmentSubmissionID { get; set; }
    }
    public class ResultDetailInfo
    {
        public string ModuleCode { get; set; }
        public string ModuleStatus { get; set; }
        public int TermDetailID { get; set; }
        public int AssignmentSubmissionID { get; set; }
        public string AssessorName { get; set; }
        public string IVName { get; set; }
        public string Submission { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public List<ResultFile> FileList { get; set; }
    }
    public class ResultFile
    {
        public string FileName { get; set; }
        public string IsMain { get; set; }
        public string IsPDF { get; set; }
        public string FilePath { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }

    public class ResultForReturnInfo
    {
        public int BatchID { get; set; }
        public string BatchName { get; set; }
        public string ModuleName { get; set; }
        public int ModuleID { get; set; }
        public DateTime? ResultDate { get; set; }
        public int Distinct { get; set; }
        public int Merit { get; set; }
        public int Pass { get; set; }
        public int Fail { get; set; }
        public int Redo { get; set; }
        public int NS { get; set; }
    }
    public class ResultForSuperAdminDetailInfo
    {
        public string CourseName { get; set; }
        public string BatchCode { get; set; }
        public string ModuleName { get; set; }
        public DateTime ResultDate { get; set; }
        public string AssessorName { get; set; }
        public string AssessorPhoto { get; set; }
        public string IVName { get; set; }
        public string IVPhoto { get; set; }
        public int ResultCount { get; set; }
        public List<ResultStudentInfo> StudentList { get; set; }
    }
    public class ResultStudentInfo
    {
        public string FullName { get; set; }
        public string LoginName { get; set; }
        public string StudentPhoto { get; set; }
    }

}