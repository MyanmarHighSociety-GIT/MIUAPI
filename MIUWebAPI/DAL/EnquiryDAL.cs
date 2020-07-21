using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using MIUWebAPI.Models;
using MIUWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MIUWebAPI.DAL
{
    public class EnquiryDAL
    {
        public Task<List<EnquiryInfo>> GetEnquiry()
        {

            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<EnquiryInfo> infoList = new List<EnquiryInfo>();
                    try
                    {
                        DateTime startDate = DateTime.Now.AddDays(-7);
                        DateTime endDate = DateTime.Now;
                        List<Enquiry> dataList = db.Enquiries.ToList();
                        foreach (var data in dataList)
                        {
                            EnquiryInfo info = new EnquiryInfo();
                            PropertyCopier<Enquiry, EnquiryInfo>.Copy(data, info);
                            //info.IsFavorite = db.FavoriteLists.Any(a => a.FavoriteID == data.ID && a.UserID == userID && a.FavoriteType == "Announcement");
                            infoList.Add(info);
                        }
                        return infoList;

                    }
                    catch (Exception)
                    {

                        return infoList;
                    }
                }
            });
        }

        public Task<LeadReturn> GetLead(int CourseID, string Date, string Year) //Date = Oct
        {
            var month = Convert.ToDateTime(Date + "01," + DateTime.Now.Year).Month;
            int year = int.Parse(Year);
            //DateTime dateTime = Convert.ToDateTime(Date);
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    LeadReturn ret = new LeadReturn();
                    try
                    {
                        List<Lead> lead = new List<Lead>();
                        List<LeadInfo> leadInfos = new List<LeadInfo>();
                        LeadCount leadCount = new LeadCount();
                        if (CourseID == 0)
                        {
                            lead = db.Leads.Where(a => DbFunctions.TruncateTime(a.CreatedDate).Value.Month == month && DbFunctions.TruncateTime(a.CreatedDate).Value.Year == year).OrderByDescending(x => x.UpdatedDate != null ? x.UpdatedDate : x.CreatedDate).ToList();
                        }
                        else
                        {
                            lead = db.Leads.Where(a => a.CourseID == CourseID && DbFunctions.TruncateTime(a.CreatedDate).Value.Month == month && DbFunctions.TruncateTime(a.CreatedDate).Value.Year == year).OrderByDescending(x => x.UpdatedDate != null ? x.UpdatedDate : x.CreatedDate).ToList();
                        }

                        foreach (var data in lead)
                        {
                            LeadInfo leadinfo = new LeadInfo();
                            PropertyCopier<Lead, LeadInfo>.Copy(data, leadinfo);
                            leadinfo.CourseName = db.Courses.Where(a => a.ID == data.CourseID).Select(a => a.CourseName).SingleOrDefault();
                            //var familyMembers = db.FamilyMembers.Where(a => a.LeadID == data.ID).Select(x => new FamilyMemberInfo { ID = x.FamilyMemberID, FullName = x.User.FullName, ProfilePicture = MIUFileServer.GetFileUrl("ProfileImages", x.User.ProfilePicture) }).ToList();
                            var familyMembers = db.FamilyMembers.Where(a => a.LeadID == data.ID).Select(x => x.User).ToList();
                            leadinfo.FamilyMembers = new List<FamilyMemberInfo>();
                            foreach (var member in familyMembers)
                            {
                                var fMember = new FamilyMemberInfo
                                {
                                    ID = member.ID,
                                    FullName = member.FullName,
                                    ProfilePicture = MIUFileServer.GetFileUrl("ProfileImages", member.ProfilePicture)
                                };
                                leadinfo.FamilyMembers.Add(fMember);
                            }
                            if (data.Country != null)
                            {
                                leadinfo.CountryName = db.CountryInfoes.Where(x => x.ID == data.Country).Select(x => x.Name).FirstOrDefault();
                            }
                            if (data.State != null)
                            {
                                leadinfo.StateName = db.StateInfoes.Where(x => x.ID == data.State).Select(x => x.Name).FirstOrDefault();
                            }
                            if (data.City != null)
                            {
                                leadinfo.CityName = db.CityInfoes.Where(x => x.ID == data.City).Select(x => x.Name).FirstOrDefault();
                            }
                            if (leadinfo.Status == "Lead")
                            {
                                leadinfo.Status = "1";
                            }
                            else if (leadinfo.Status == "Contacted")
                            {
                                leadinfo.Status = "2";
                            }
                            else if (leadinfo.Status == "UploadedDocument")
                            {
                                leadinfo.Status = "3";
                            }
                            else if(leadinfo.Status == "Student")
                            {
                                leadinfo.Status = "4";
                            }

                            if (!string.IsNullOrEmpty(leadinfo.UpdatedUser))
                            {
                                var UserProfile = db.Users.Where(x => x.LoginName == leadinfo.UpdatedUser).Select(x => x.ProfilePicture).FirstOrDefault();
                                leadinfo.CreatedUpdatedUserProfile = MIUFileServer.GetFileUrl("ProfileImages", UserProfile);
                            }
                            else if (!string.IsNullOrEmpty(leadinfo.UpdatedUser))
                            {
                                var UserProfile = db.Users.Where(x => x.LoginName == leadinfo.CreatedUser).Select(x => x.ProfilePicture).FirstOrDefault();
                                leadinfo.CreatedUpdatedUserProfile = MIUFileServer.GetFileUrl("ProfileImages", UserProfile);
                            }

                            leadInfos.Add(leadinfo);
                        }

                        leadCount.TotalLeadPercent = 100;
                        leadCount.TotalLead = lead.Count();
                        leadCount.Contacted = lead.Where(a => a.Status == "Contacted").Count();
                        leadCount.ContactedPercent = (leadCount.Contacted * 100) / leadCount.TotalLead;
                        leadCount.Registered = lead.Where(a => a.Status == "Registered").Count();
                        leadCount.RegisteredPercent = (leadCount.Registered * 100) / leadCount.TotalLead;
                        leadCount.Missed = 0;
                        leadCount.MissedPercent = (leadCount.Missed * 100) / leadCount.TotalLead;

                        ret.LeadCount = leadCount;
                        ret.LeadInfos = leadInfos;
                        return ret;
                    }
                    catch (Exception)
                    {
                        return ret;
                    }

                }
            });
        }

        //public Task<List<LeadInfo>> GetLead()
        //{
        //    return Task.Run(() =>
        //    {
        //        using (MIUEntities db = new MIUEntities())
        //        {
        //            DateTime startDate = DateTime.Now.AddDays(-7);
        //            DateTime endDate = DateTime.Now;
        //            List<LeadInfo> infoList = new List<LeadInfo>();
        //            try
        //            {
        //                List<Lead> dataList = db.Leads.ToList();
        //                foreach (var data in dataList)
        //                {
        //                    LeadInfo info = new LeadInfo();
        //                    PropertyCopier<Lead, LeadInfo>.Copy(data, info);
        //                    info.CourseName = db.Courses.Where(a => a.ID == data.CourseID).Select(a => a.CourseName).SingleOrDefault();
        //                    info.FamilyMembers = db.FamilyMembers.Where(a => a.LeadID == data.ID).ToList();
        //                    infoList.Add(info);
        //                }
        //                return infoList;
        //            }
        //            catch (Exception)
        //            {

        //                return infoList;
        //            }

        //        }
        //    });
        //}

        public Task<LeadInfo> GetLeadByID(int ID)
        {

            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    DateTime startDate = DateTime.Now.AddDays(-7);
                    DateTime endDate = DateTime.Now;
                    Lead data = db.Leads.Where(l => l.ID == ID).SingleOrDefault();
                    LeadInfo info = new LeadInfo();
                    try
                    {
                        PropertyCopier<Lead, LeadInfo>.Copy(data, info);
                        info.CourseName = db.Courses.Where(a => a.ID == data.CourseID).Select(a => a.CourseName).SingleOrDefault();
                        //info.FamilyMembers = data.FamilyMembers;
                        var familyMembers = db.FamilyMembers.Where(a => a.LeadID == data.ID).Select(x => x.User).ToList();
                        info.FamilyMembers = new List<FamilyMemberInfo>();
                        foreach (var member in familyMembers)
                        {
                            var fMember = new FamilyMemberInfo
                            {
                                ID = member.ID,
                                FullName = member.FullName,
                                ProfilePicture = MIUFileServer.GetFileUrl("ProfileImages", member.ProfilePicture)
                            };
                            info.FamilyMembers.Add(fMember);
                        }
                        if (info.Country != null)
                        {
                            info.CountryName = db.CountryInfoes.Where(x => x.ID == info.Country).Select(x => x.Name).FirstOrDefault();
                        }
                        if(info.State != null)
                        {
                            info.StateName = db.StateInfoes.Where(x => x.ID == info.State).Select(x => x.Name).FirstOrDefault();
                        }
                        if(info.City != null)
                        {
                            info.CityName = db.CityInfoes.Where(x => x.ID == info.City).Select(x => x.Name).FirstOrDefault();
                        }
                        if (info.Status == "Lead")
                        {
                            info.Status = "1";
                        }
                        else if (info.Status == "Contacted")
                        {
                            info.Status = "2";
                        }
                        else if (info.Status == "Registered")
                        {
                            info.Status = "3";
                        }
                        else
                        {
                            info.Status = "4";
                        }
                        return info;
                    }
                    catch (Exception)
                    {
                        return info;
                    }
                }
            });
        }

        public Task<List<LeadActivityInfo>> GetLeadActivity(int ID, string Action)
        {

            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<LeadActivityInfo> infoList = new List<LeadActivityInfo>();
                    try
                    {
                        List<LeadActivity> dataList = db.LeadActivities.Where(a => a.LeadID == ID && a.Action == Action).OrderByDescending(x => x.CreatedDate).ToList();
                        foreach (var data in dataList)
                        {
                            LeadActivityInfo info = new LeadActivityInfo();
                            PropertyCopier<LeadActivity, LeadActivityInfo>.Copy(data, info);
                            info.Related = db.EventCalendars.Where(x => info.RelatedTo == null ? true : x.ID == info.RelatedTo).ToString();
                            if (Action == "Email")
                            {
                                info.FromName = db.Users.Where(a => a.LoginName == data.FromName).Select(a => a.EmailAccount).SingleOrDefault();
                                info.ToName = db.Leads.Where(a => a.ID == data.LeadID).Select(a => a.Email).SingleOrDefault();
                            }
                            infoList.Add(info);
                        }
                        return infoList;
                    }
                    catch (Exception ex)
                    {
                        //throw ex;
                        return infoList;
                    }
                }
            });
        }

        public Task<JsonResponse> AddLeadActivity(LeadActivityInfo leadActivityInfo)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        TimeSpan time = new TimeSpan(00, 06, 30, 0);
                        LeadActivity leadActivity = new LeadActivity()
                        {
                            LeadID = leadActivityInfo.LeadID,
                            Action = leadActivityInfo.Action,
                            FromName = leadActivityInfo.FromName,
                            ToName = leadActivityInfo.ToName,
                            Subject = leadActivityInfo.Subject,
                            Remark = leadActivityInfo.Remark,
                            RelatedTo = leadActivityInfo.RelatedTo,
                            CreatedDate = DateTime.UtcNow.Add(time),
                            CreatedUser = leadActivityInfo.CreatedUser
                        };
                        db.LeadActivities.Add(leadActivity);

                        db.SaveChanges();
                        return new JsonResponse() { Flag = true, Message = "Successfully Saved" };
                    }
                    catch (Exception ex)
                    {
                        return new JsonResponse() { Flag = true, Message = ex.Message };
                    }
                }
            });
        }

        public Task<List<LeadRelatedTo>> GetLeadRelatedTo()
        {

            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<LeadRelatedTo> List = new List<LeadRelatedTo>();
                    try
                    {
                        var RelatedTo = (from e in db.EventCalendars select e).ToList();

                        foreach (var data in RelatedTo)
                        {
                            LeadRelatedTo info = new LeadRelatedTo();
                            info.ID = data.ID;
                            info.Title = data.Title;

                            List.Add(info);
                        }
                    }
                    catch (Exception ex)
                    {
                        return List;
                    }

                    return List;
                }
            });
        }
    }
}