using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using MIUWebAPI.Models;
using MIUWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace MIUWebAPI.DAL
{
    public class NotificationDAL
    {
        public Task<List<NotificationInfo>> GetNotification(int userID, string userRole)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<NotificationInfo> infoList = new List<NotificationInfo>();
                    if (userRole == "Students")
                    {
                        List<StudentNoti> stdNoti = db.StudentNotis.Where(x => x.StudentID == userID).OrderByDescending(o => o.CreatedDate).ToList();
                        foreach (var noti in stdNoti)
                        {
                            string result = "";
                            string moduleName = noti.AssignmentSubmission.Assignment.TermDetail.Module.ModuleName;
                            switch (noti.Grading)
                            {
                                case 1:
                                    result = "Pass";
                                    break;
                                case 2:
                                    result = "Fail";
                                    break;
                                case 3:
                                    result = "Redo";
                                    break;
                                case 4:
                                    result = "Merit";
                                    break;
                                case 5:
                                    result = "Distinction";
                                    break;
                                default:
                                    result = "";
                                    break;
                            }

                            NotificationInfo info = new NotificationInfo()
                            {
                                ID = noti.StudentNotiID,
                                NotiType = "Assignment",
                                Description = string.Format("Your result for {0} ({1}) is {2}", moduleName, noti.Type, result),
                                CreatedDate = noti.CreatedDate
                            };
                            infoList.Add(info);
                        }
                    }
                    return infoList;
                }
            });
        }

        public Task<JsonResult> GetNotiCount(int userID)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    //int TotalNotiCount = db.Notifications.Where(x => x.NotiTypeId != 1).Count();
                    //int IsSeenNotiCount = db.NotiVisibilities.Where(x => x.UserID == userID).Count();

                    List<Notification> NotiList = (from n in db.Notifications
                                                   where n.NotiTypeId != 1 &&
                                                   (!db.NotiVisibilities.Any(nv => nv.NotiID == n.ID && nv.UserID == userID))
                                                   select n).ToList();

                    List<Notification> SubscriptionList = (from n in db.Notifications
                                                           join s in db.Subscribtions.Where(s => s.UserID == userID) on n.ID equals s.NotiID
                                                           where n.NotiTypeId == 1 && (!db.NotiVisibilities.Any(nv => nv.NotiID == n.ID && nv.UserID == userID))
                                                           select n).ToList();

                    int NotiCount = NotiList.Count + SubscriptionList.Count;

                    List<NotiListInfo> WebNotilist = GetWebNotification(userID);

                    NotiCount = NotiCount + WebNotilist.Count();

                    return new JsonResult() { Flag = true, Result = NotiCount };
                }
            });
        }

        public Task<NotiInfo> GetNotificationList(int userID, int currentIndex, int maxRow)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    NotiInfo Notifications = new NotiInfo();

                    try
                    {
                        User user = db.Users.Where(x => x.ID == userID && x.IsDelete != true).SingleOrDefault();
                        Notifications.NotiCount = 0;
                        var skip = (currentIndex - 1) * maxRow;

                        List<Notification> NotiList = (from n in db.Notifications
                                                       where n.NotiTypeId != 1 && (!db.NotiVisibilities.Any(nv => nv.NotiID == n.ID && nv.UserID == userID) || db.NotiVisibilities.Any(nv => nv.NotiID == n.ID && nv.UserID == userID))
                                                       select n).ToList();

                        List<Notification> SubscriptionList = (from n in db.Notifications
                                                                   //join nv in db.NotiVisibilities.Where(nv => nv.UserID != userID) on n.ID equals nv.NotiID into noti from x in noti.DefaultIfEmpty()
                                                               join s in db.Subscribtions.Where(s => s.UserID == userID) on n.ID equals s.NotiID
                                                               where (!db.NotiVisibilities.Any(nv => nv.NotiID == n.ID && nv.UserID == userID) || db.NotiVisibilities.Any(nv => nv.NotiID == n.ID && nv.UserID == userID)) && n.NotiTypeId == 1
                                                               select n).ToList();

                        List<NotiVisibility> notiVisibilities = new List<NotiVisibility>();
                        List<NotiListInfo> NotiListInfo = new List<NotiListInfo>();

                        foreach (var data in NotiList)
                        {
                            NotiListInfo info = new NotiListInfo();
                            PropertyCopier<Notification, NotiListInfo>.Copy(data, info);

                            var NotiType = db.NotificationTypes.Where(x => x.ID == data.NotiTypeId).SingleOrDefault();

                            if (NotiType.ID == 1)
                            {
                                LeadNotification LeadNoti = new LeadNotification();
                                LeadNoti = db.LeadNotifications.Where(x => x.NotificationId == data.ID).FirstOrDefault();
                                if(LeadNoti != null)
                                {
                                    info.RelatedTableID = LeadNoti.LeadId;
                                    info.RelatedTableName = "Lead";
                                }
                            }
                            else if (NotiType.ID == 2)
                            {
                                NewsNoti NewNoti = new NewsNoti();
                                NewNoti = db.NewsNotis.Where(x => x.NotiID == data.ID).FirstOrDefault();
                                if(NewNoti != null)
                                {
                                    info.RelatedTableID = NewNoti.NewsID;
                                    info.RelatedTableName = "News";
                                }
                                
                            }

                            //info.RelatedTableID = db.
                            NotiListInfo.Add(info); //Get Notification List

                            NotiVisibility notiVisibility = new NotiVisibility();
                            notiVisibilities = db.NotiVisibilities.Where(x => x.UserID == userID && x.NotiID == data.ID).ToList();
                            if (notiVisibilities.Count == 0)
                            {
                                notiVisibility.NotiID = data.ID;
                                notiVisibility.UserID = userID;
                                notiVisibility.IsSeen = false;
                                db.NotiVisibilities.Add(notiVisibility);
                            }
                            else
                            {
                                foreach (var notiVis in notiVisibilities)
                                {
                                    foreach (var NotiListing in NotiListInfo)
                                    {
                                        if (NotiListing.UserId != null && NotiListing.ID == notiVis.NotiID && NotiListing.UserId == notiVis.UserID && notiVis.IsSeen == true)
                                        {
                                            NotiListing.IsSeen = true;
                                        }
                                        else if (NotiListing.ID == notiVis.NotiID && notiVis.IsSeen == true)
                                        {
                                            NotiListing.IsSeen = true;
                                        }
                                    }
                                }
                            }
                        }

                        foreach (var data in SubscriptionList)
                        {
                            NotiListInfo info = new NotiListInfo();
                            PropertyCopier<Notification, NotiListInfo>.Copy(data, info);

                            var NotiType = db.NotificationTypes.Where(x => x.ID == data.NotiTypeId).SingleOrDefault();

                            if (NotiType.ID == 1)
                            {
                                LeadNotification LeadNoti = db.LeadNotifications.Where(x => x.NotificationId == data.ID).FirstOrDefault();
                                if(LeadNoti != null)
                                {
                                    info.RelatedTableID = LeadNoti.LeadId;
                                    info.RelatedTableName = "Lead";
                                }
                            }
                            else if (NotiType.ID == 2)
                            {
                                NewsNoti NewNoti = db.NewsNotis.Where(x => x.NotiID == data.ID).FirstOrDefault();
                                if(NewNoti != null)
                                {
                                    info.RelatedTableID = NewNoti.NewsID;
                                    info.RelatedTableName = "News";
                                }
                            }

                            //info.RelatedTableID = db.
                            NotiListInfo.Add(info); //Get Notification List

                            NotiVisibility notiVisibility = new NotiVisibility();
                            notiVisibilities = db.NotiVisibilities.Where(x => x.UserID == userID && x.NotiID == data.ID).ToList();
                            if (notiVisibilities.Count == 0)
                            {
                                notiVisibility.NotiID = data.ID;
                                notiVisibility.UserID = userID;
                                notiVisibility.IsSeen = false;
                                db.NotiVisibilities.Add(notiVisibility);
                            }
                            else
                            {
                                foreach (var notiVis in notiVisibilities)
                                {
                                    foreach (var NotiListing in NotiListInfo)
                                    {
                                        if (NotiListing.ID == notiVis.NotiID && NotiListing.UserId == notiVis.UserID && notiVis.IsSeen == true)
                                        {
                                            NotiListing.IsSeen = true;
                                            //NotiListInfo.Add(NotiListing);
                                        }
                                    }
                                }
                            }
                        }

                        List<NotiListInfo> WebNotiList = new List<NotiListInfo>();
                        WebNotiList = GetWebNotification(userID);

                        NotiListInfo.AddRange(WebNotiList);

                        NotiListInfo.ForEach(x => { if (x.UpdatedDate == null) { x.UpdatedDate = x.CreatedDate; x.UpdatedBy = x.CreatedBy; } });// if updated date is null set updated date value from created date.

                        Notifications.NotiList = NotiListInfo.OrderByDescending(x => x.CreatedDate).Skip(skip).Take(maxRow).ToList();
                        db.SaveChanges(); //Add to IsSeenNotification Table

                        int TotalNotiCount = db.Notifications.Where(x => x.NotiTypeId != 1).Count();
                        int IsSeenNotiCount = db.NotiVisibilities.Where(x => x.UserID == userID).Count();

                        Notifications.NotiCount = 0;
                    }
                    catch (System.Exception ex)
                    {
                        //throw ex;
                    }

                    return Notifications;
                }
            });
        }


        public Task<JsonResponse> ChangeSeen(int userID, int NotiID)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        if(NotiID != 0)
                        {
                            NotiVisibility notiVisibility = db.NotiVisibilities.Where(x => x.NotiID == NotiID && x.UserID == userID).SingleOrDefault();

                            notiVisibility.IsSeen = true;
                            db.SaveChanges();

                            return new JsonResponse() { Flag = true, Message = "IsSeen changed." };
                        }

                        else
                        {
                            return new JsonResponse() { Flag = true, Message = "No Changes(Coming soon)" };
                        }
                        //return true;
                    }
                    catch (Exception ex)
                    {
                        return new JsonResponse() { Flag = true, Message = ex.Message };
                    }

                }
            });
        }

        public List<NotiListInfo> GetWebNotification(int userID)
        {
            using (MIUEntities db = new MIUEntities())
            {
                List<WebNotification> WebNotifications = new List<WebNotification>();
                List<NotiListInfo> notiListInfos = new List<NotiListInfo>();
                try
                {
                    User user = db.Users.Where(x => x.ID == userID && x.IsDelete != true).SingleOrDefault();
                    if (user != null)
                    {
                        var linqBatch = (from td in db.TermDetails
                                         join t in db.Terms on td.TermID equals t.ID
                                         join b in db.Batches on t.BatchID equals b.ID
                                         where td.AccessorID == user.ID
                                         group b by b.ID into res
                                         select new { ID = res.Key, Count = res.Count() }).ToList();
                        var batchIdList = linqBatch.OrderBy(x => x.ID).Select(x => x.ID).ToList();
                        int NotiTypeID = 0;
                        WebNotifications = db.WebNotifications.ToList();
                        if (user.Role == "Students")
                        {
                            Batch batch = db.BatchDetails.Where(x => x.StudentID == user.ID).Select(x => x.Batch).FirstOrDefault();
                            WebNotifications = WebNotifications.Where(x => x.BatchID == batch.ID && x.Recipient == "Student").ToList();
                            NotiTypeID = 3;
                        }
                        else if (user.Role == "Alumni")
                        {
                            Batch batch = db.BatchDetails.Where(x => x.StudentID == user.ID).Select(x => x.Batch).FirstOrDefault();
                            WebNotifications = WebNotifications.Where(x => x.Recipient == "Alumni" && x.BatchID == batch.ID && x.GraduatedYear == 2019).ToList();
                            NotiTypeID = 4;
                        }
                        else if (user.Role == "Enquiry")
                        {
                            Batch batch = db.BatchDetails.Where(x => x.StudentID == user.ID).Select(x => x.Batch).FirstOrDefault();
                            WebNotifications = WebNotifications.Where(x => x.Recipient == "Enquiry" && x.CourseID == batch.Course.ID && x.GraduatedYear == 2019).ToList();
                            NotiTypeID = 5;
                        }
                        else if (user.Role == "Lectures" || user.UserType == 2)
                        {
                            WebNotifications = WebNotifications.Where(x => x.Recipient.Contains("Lecturer") && batchIdList.Contains(x.BatchID == null ? 0 : x.BatchID.Value)).ToList();
                            NotiTypeID = 6;

                        }
                        else if (user.Role == "ProgramManager")
                        {
                            List <Batch> batches = db.Batches.Where(x => x.ProgramManagerID == user.ID && x.IsDelete != true).ToList();

                            List<int> batchId = batches.Select(x => x.ID).ToList();

                            WebNotifications = WebNotifications.Where(x => x.Recipient == "ProgramManager" && batchId.Contains(x.BatchID == null ? 0 : x.BatchID.Value)).ToList();
                            NotiTypeID = 7;
                        }
                        else if (user.Role == "InternalVerifier")
                        {
                            var batches = db.IVComments.Where(x => x.IVID == user.ID && x.AssignmentSubmission.Assignment.TermDetail.Term.Batch.IsDelete != true)
                                            .GroupBy(g => g.AssignmentSubmission.Assignment.TermDetail.Term.Batch.ID)
                                            .Select(x => new { ID = x.Key, Count = x.Count() }).ToList();

                            var ivbatchIdList = batches.Select(x => x.ID).ToList();

                            WebNotifications = WebNotifications.Where(x => x.Recipient == "IV" && ivbatchIdList.Contains(x.BatchID == null ? 0 : x.BatchID.Value)).ToList();
                            NotiTypeID = 8;
                        }
                        else if (user.Role == "Assessors")
                        {

                            WebNotifications = WebNotifications.Where(x => x.Recipient == "Assessor" && batchIdList.Contains(x.BatchID == null ? 0 : x.BatchID.Value)).ToList();
                            NotiTypeID = 9;
                        }
                        else if (user.Role == "Finance")
                        {
                            WebNotifications = WebNotifications.Where(x => x.Recipient == "Finance").ToList();
                            NotiTypeID = 10;
                        }
                        else if (user.Role == "StudentOfficer")
                        {
                            WebNotifications = WebNotifications.Where(x => x.Recipient == "StudentOfficer").ToList();
                            NotiTypeID = 11;
                        }
                        else
                        {
                            WebNotifications = new List<WebNotification>();
                        }


                        foreach (var item in WebNotifications)
                        {
                            NotiListInfo notiListInfo = new NotiListInfo();

                            notiListInfo.Message = item.Message;
                            notiListInfo.UserId = userID;
                            notiListInfo.NotiTypeId = NotiTypeID;
                            notiListInfo.CreatedBy = item.CreatedBy;
                            notiListInfo.CreatedDate = item.CreatedDate;
                            notiListInfo.IsSeen = true;
                            notiListInfos.Add(notiListInfo);
                            
                        }
                    }
                }
                catch (Exception )
                {

                }
                return notiListInfos;
            }
        }

    }
}