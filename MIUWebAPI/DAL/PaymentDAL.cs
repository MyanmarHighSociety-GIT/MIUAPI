using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static MIUWebAPI.ViewModels.PaymentViewModel;

namespace MIUWebAPI.DAL
{
    public class PaymentDAL
    {
        public Task<List<StudentPaymentInfo>> GetStudentPayment(int batchID, int userID) // 13202, 32184 
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<MIUPayment> data = db.MIUPayments.Where(x => x.BatchID == batchID && x.StudentID == userID).ToList();

                    List<StudentPaymentInfo> infoList = new List<StudentPaymentInfo>();
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            StudentPaymentInfo info = new StudentPaymentInfo();
                            List<PaymentDetailInfo> infoDetailList = new List<PaymentDetailInfo>();
                            PropertyCopier<MIUPayment, StudentPaymentInfo>.Copy(item, info);
                            info.BatchName = item.Batch.BatchName;
                            info.CourseName = item.Course.CourseName;
                            infoList.Add(info);
                            foreach (var item1 in item.MIUPaymentDetails)
                            {
                                PaymentDetailInfo infoDetail = new PaymentDetailInfo();
                                PropertyCopier<MIUPaymentDetail, PaymentDetailInfo>.Copy(item1, infoDetail);
                                if (String.IsNullOrEmpty(item1.PaymentStatus) && item1.PaymentDueDate < DateTime.Now)
                                {
                                    infoDetail.PaymentStatus = "Overdued";
                                }
                                infoDetail.TermName = item1.Term.TermName;
                                infoDetailList.Add(infoDetail);
                            }
                            info.StudentPaymentDetailInfo = infoDetailList;
                        }

                    }

                    return infoList;
                }
            });
        }

        public Task<List<RemodulePaymentInfo>> GetRemoduleStudent(int batchID, int userID) // 3082, 6171
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<RemodulePaymentInfo> infoList = new List<RemodulePaymentInfo>();

                    List<RemodulePayment> dataList = db.RemodulePayments.Where(x => x.StudentID == userID && x.RemoduleBatchID != null).ToList();

                    foreach (var data in dataList)
                    {
                        RemodulePaymentInfo info = new RemodulePaymentInfo();
                        PropertyCopier<RemodulePayment, RemodulePaymentInfo>.Copy(data, info);
                        info.BatchName = db.Batches.Where(x => x.ID == data.RemoduleBatchID).Select(x => x.BatchName).SingleOrDefault();
                        info.CourseName = data.Course.CourseName;
                        info.ModuleName = data.Module.ModuleName;
                        infoList.Add(info);
                    }

                    return infoList;
                }
            });
        }


        public Task<List<MIUPaymentInfo>> GetPayment(int courseID, int batchID, int? userID, string year, string month, string type, int currentIndex, int maxRows)
        {
            int Year = int.Parse(year);
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    int skip = currentIndex <= 1 ? 0 : (currentIndex - 1) * maxRows;
                    List<MIUPaymentInfo> infoList = new List<MIUPaymentInfo>();

                    List<MIUPaymentDetailInfo> infoDetailList = new List<MIUPaymentDetailInfo>();

                    var Month = DateTime.Now.Month;
                    if (month != "")
                    {
                        Month = Convert.ToDateTime(month + "01," + DateTime.Now.Year).Month;
                    }
                    List<MIUPayment> data = db.MIUPayments.Where(x => (courseID == 0 ? true : x.CourseID == courseID)
                                                            && (batchID == 0 ? true : x.BatchID == batchID)
                                                            && (userID == 0 ? true : x.StudentID == userID)
                                                            ).ToList();

                    List<MIUPaymentDetail> dataDetail = new List<MIUPaymentDetail>();
                    MIUPaymentInfo info = new MIUPaymentInfo();
                    foreach (var item in data)
                    {
                        dataDetail = item.MIUPaymentDetails.Where(x =>
                        
                        (   x.MIUPaymentID == item.ID   ) 

                        && (
                        
                            //payment duedate
                                (
                                    x.PaymentDueDate.Value.Year == Year
                                    && x.PaymentDueDate.Value.Month == Month
                                )
                            ||

                            //payent received date
                                (
                                    x.PaymentReceivedDate != null &&
                                    x.PaymentReceivedDate.Value.Year == Year
                                    && x.PaymentReceivedDate.Value.Month == Month
                                )
                            )

                        ).ToList();


                        if (type == "Received")
                        {
                            dataDetail = dataDetail.Where(x => x.PaymentStatus == type).ToList();
                        }
                        else if (type == "Overdued")
                        {
                            dataDetail = dataDetail.Where(x => x.PaymentDueDate.Value.Date < DateTime.Now.Date && x.PaymentStatus == null).ToList();
                        }
                        else
                        {
                            dataDetail = dataDetail.OrderByDescending(x => x.PaymentReceivedDate).ToList();
                        }


                        foreach (var item1 in dataDetail)
                        {
                            int amount = 0;
                            int.TryParse(item1.Amount, out amount);
                            MIUPaymentDetailInfo detailInfo = new MIUPaymentDetailInfo()
                            {
                                StudentID = item.User.ID,
                                ProfilePicture = MIUFileServer.GetFileUrl("ProfileImages", item.User.ProfilePicture),
                                StudentName = item.User.FullName,
                                LoginName = item.User.LoginName,
                                CourseName = item.Course.CourseName,
                                BatchName = item.Batch.BatchName,
                                TermName = item1.Term.TermName,
                                ReceivedOn = item1.PaymentReceivedDate,
                                PaymentDueDate = item1.PaymentDueDate,
                                PaymentStatus = item1.PaymentStatus,
                                Benefit = item1.Benefit,
                                Amount = amount,
                            };

                            //if (String.IsNullOrEmpty(item1.PaymentStatus) && item1.PaymentDueDate < DateTime.Now.Date)
                            //{
                            //    detailInfo.PaymentStatus = "Overdued";
                            //    info.OverduePayment += amount;
                            //}
                            //else if (item1.PaymentStatus == "Received")
                            //{
                            //    info.TotalEarned += amount;
                            //}
                            //else
                            //{
                            //    detailInfo.PaymentStatus = "Pending";
                            //    pending += amount;
                            //}
                            //info.EstimatedIncome = info.OverduePayment + info.TotalEarned + pending;

                            infoDetailList.Add(detailInfo);
                            info.MIUPaymentDetailInfo = infoDetailList.Skip(skip).Take(maxRows).ToList();
                        }
                    }

                    //to calculate total amount will calculate depends on BatchID, CourseID, StudentID

                    //total earned
                    var totalEarned = (
                        from p in db.MIUPayments
                        join pd in db.MIUPaymentDetails
                        on   p.ID equals pd.MIUPaymentID
                        where 
                        (
                            (courseID == 0 ? true : p.CourseID == courseID)
                            && (batchID == 0 ? true : p.BatchID == batchID)
                            && (userID == 0 ? true : p.StudentID == userID)
                            && (pd.PaymentReceivedDate.Value.Year == Year)
                            && ( string.IsNullOrEmpty(month) ? true : pd.PaymentReceivedDate.Value.Month == Month ) 
                            && (pd.PaymentStatus == "Received" )
                        )
                        select pd.Amount
                    ).ToList();

                    foreach (var earn in totalEarned)
                    {
                        double en = Convert.ToDouble(earn);
                        info.TotalEarned += en;
                    }

                    //overdue will calculate status == pending
                    var now = DateTime.Now;
                    var totalOverdue = (
                        from p in db.MIUPayments
                        join pd in db.MIUPaymentDetails
                        on p.ID equals pd.MIUPaymentID
                        where
                        (
                            (courseID == 0 ? true : p.CourseID == courseID)
                            && (batchID == 0 ? true : p.BatchID == batchID)
                            && (userID == 0 ? true : p.StudentID == userID)
                            && pd.PaymentDueDate.Value.Year == Year && pd.PaymentDueDate.Value.Month == Month &&
                            (pd.PaymentStatus == "Pending" || pd.PaymentStatus == null || pd.PaymentStatus == "") &&
                            DbFunctions.TruncateTime(pd.PaymentDueDate.Value) < DbFunctions.TruncateTime(now)
                        )
                        select pd.Amount
                    ).ToList();


                    foreach (var earn in totalOverdue)
                    {
                        double en = Convert.ToDouble(earn);
                        info.OverduePayment += en;
                    }

                    //Estimate income
                    var totalEstimateIncome = (
                        from p in db.MIUPayments
                        join pd in db.MIUPaymentDetails
                        on p.ID equals pd.MIUPaymentID
                        where
                        (
                            (courseID == 0 ? true : p.CourseID == courseID)
                            && (batchID == 0 ? true : p.BatchID == batchID)
                            && (userID == 0 ? true : p.StudentID == userID)
                            && pd.PaymentDueDate.Value.Year == Year && pd.PaymentDueDate.Value.Month == Month 
                        )
                        select pd.Amount
                    ).ToList();


                    foreach (var earn in totalEstimateIncome)
                    {
                        double en = Convert.ToDouble(earn);
                        info.EstimatedIncome += en;
                    }

                    infoList.Add(info);
                    
                    return infoList;
                }
            });
        }


    }
}