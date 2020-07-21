using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class PaymentViewModel
    {
        public class PaymentWrap
        {
            public List<StudentPaymentInfo> StudentPayment {get; set;}
            public List<RemodulePaymentInfo> RemodulePayment { get; set; }
        }

        public class StudentPaymentInfo
        {
            public int ID { get; set; }
            public int StudentID { get; set; }
            public int CourseID { get; set; }
            public string CourseName { get; set; }
            public int BatchID { get; set; }
            public string BatchName { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public Nullable<System.DateTime> ModifiedDate { get; set; }
            public string ModifiedBy { get; set; }

            public List<PaymentDetailInfo> StudentPaymentDetailInfo { get; set; }

        }

        public class PaymentDetailInfo
        {
            public int ID { get; set; }
            public int MIUPaymentID { get; set; }
            public int TermID { get; set; }
            public string TermName { get; set; }
            public string Amount { get; set; }
            public Nullable<System.DateTime> PaymentDueDate { get; set; }
            public string PaymentStatus { get; set; }
            public Nullable<System.DateTime> PaymentReceivedDate { get; set; }
            public string Benefit { get; set; }
            public string Remark { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public Nullable<System.DateTime> ModifiedDate { get; set; }
            public string ModifiedBy { get; set; }
        }

        public class RemodulePaymentInfo
        {
            public int ID { get; set; }
            public int StudentID { get; set; }
            public int CourseID { get; set; }
            public string CourseName { get; set; }
            public Nullable<int> RemoduleBatchID { get; set; }
            public int BatchID { get; set; }
            public string BatchName { get; set; }
            public int RemoduleID { get; set; }
            public string ModuleName { get; set; }
            public string Amount { get; set; }
            public System.DateTime PaymentReceivedDate { get; set; }
            public string Remark { get; set; }
            public Nullable<System.DateTime> CreatedDate { get; set; }
            public string CreatedBy { get; set; }
            public Nullable<System.DateTime> ModifiedDate { get; set; }
            public string ModifiedBy { get; set; }
        }

        
        public class MIUPaymentInfo
        {
            public double TotalEarned { get; set; }
            public double EstimatedIncome { get; set; }
            public double OverduePayment { get; set; }

            public List<MIUPaymentDetailInfo> MIUPaymentDetailInfo { get; set; }
        }

        public class MIUPaymentDetailInfo
        {
            public int StudentID { get; set; }
            public string ProfilePicture { get; set; }
            public string StudentName { get; set; }
            public string LoginName { get; set; }
            public string CourseName { get; set; }
            public string BatchName { get; set; }
            public string TermName { get; set; }
            public int Amount { get; set; }
            public Nullable<System.DateTime> ReceivedOn { get; set; }
            public Nullable<System.DateTime> PaymentDueDate { get; set; } 
            public string PaymentStatus { get; set; }
            public string Benefit { get; set; }
        }
    }
}