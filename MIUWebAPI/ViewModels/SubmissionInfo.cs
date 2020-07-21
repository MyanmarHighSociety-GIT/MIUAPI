using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class SubmissionInfo
    {
        public string ModuleName { get; set; }
        public string CourseName { get; set; }
        public string BatchCode { get; set; }
        public int Submitted { get; set; }
        public int NoSubmission { get; set; }
        public int AssessorCheck { get; set; }
        public int IVCheck { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public int? SubmissionType { get; set; }
        public int BatchID { get; set; }
        public int TermDetailID { get; set; }
    }
    public class SubmissionDetailInfo
    {
        public string ModuleName { get; set; }
        public string CourseName { get; set; }
        public string BatchCode { get; set; }
        public int Submitted { get; set; }
        public int NoSubmission { get; set; }
        public string AssessorName { get; set; }
        public int AssessorCheck { get; set; }
        public int AssessorLeft { get; set; }
        public string IVName { get; set; }
        public int IVCheck { get; set; }
        public int IVLeft { get; set; }
        public System.DateTime DueDate { get; set; }
        public List<SubmissionStudentInfo> StudentList { get; set; }
    }
    public class SubmissionStudentInfo
    {
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public string LoginName { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public string StudentPhoto { get; set; }
    }
    public class SubmissionStudentDetailInfo
    {
        public string ModuleName { get; set; }
        public string CourseName { get; set; }
        public string BatchCode { get; set; }       
        public string AssessorName { get; set; }       
        public string IVName { get; set; }       
        public System.DateTime DueDate { get; set; }
        public string StudentName { get; set; }
        public string StudentPhoto { get; set; }
        public DateTime SubmissionDate { get; set; }
        public List<SubmissionFile> FileList { get; set; }
        public bool IsAssessorCheck { get; set; }
        public bool IsIVCheck { get; set; }
        public bool IsResultOut { get; set; }
    }
    public class SubmissionFile
    {
        public string FileName { get; set; }
        public string IsMain { get; set; }
    }
}