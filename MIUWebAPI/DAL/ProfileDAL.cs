using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using MIUWebAPI.Models;
using MIUWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static MIUWebAPI.Helper.SendEmail;

namespace MIUWebAPI.DAL
{
    public class ProfileDAL
    {
        public Task<Profile> GetProfile(int UserID)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    Profile info = new Profile();
                    try
                    {
                        User data = db.Users.Where(x => x.IsDelete != true /*&& x.UserType == 1*/ && x.ID == UserID).SingleOrDefault();
                        
                        BatchInfo batchInfo = new BatchInfo();
                        CourseInfo courseInfo = new CourseInfo();
                        if (data != null)
                        {
                            PropertyCopier<User, Profile>.Copy(data, info);
                            info.ProfilePicture = MIUFileServer.GetFileUrl("ProfileImages", data.ProfilePicture);
                            Batch batch = db.BatchDetails.Where(x => x.StudentID == data.ID).Select(x => x.Batch).Where(x => x.IsDelete != true).FirstOrDefault();
                            if (batch != null)
                            {
                                PropertyCopier<Batch, BatchInfo>.Copy(batch, batchInfo);
                                Course course = db.Courses.Where(x => x.IsDelete != true && x.ID == batch.CourseID).SingleOrDefault();
                                if (course != null)
                                {
                                    PropertyCopier<Course, CourseInfo>.Copy(course, courseInfo);
                                }
                            }

                            info.Dob = info.DOB == null ? "-" : info.DOB.Value.ToString("yyyy-MM-dd");

                            info.ApplicationDateString = info.ApplicationDate == null ? "-" : info.ApplicationDate.Value.ToString("yyyy-MM-dd");

                            info.Batch = batchInfo;
                            info.Course = courseInfo;
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

        public Task<EditProfile> GetEditProfile(int UserID)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        User data = db.Users.Where(x => x.IsDelete != true && x.ID == UserID).SingleOrDefault();
                        EditProfile info = new EditProfile();
                        BatchInfo batchInfo = new BatchInfo();
                        CourseInfo courseInfo = new CourseInfo();
                        if (data != null)
                        {
                            PropertyCopier<User, EditProfile>.Copy(data, info);
                            if (data.DOB != null)
                            {
                                info.DOB = data.DOB.Value.Date;
                            }
                            if (!string.IsNullOrEmpty(data.ProfilePicture))
                            {
                                info.FileName = MIUFileServer.GetFileUrl("ProfileImages", data.ProfilePicture);
                            }
                            
                        };

                        return info;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            });
        }


        public Task<JsonResponse> EditProfile(EditProfile EditProfile)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        User user = db.Users.Where(x => x.ID == EditProfile.ID).SingleOrDefault();
                        List<User> userList = db.Users.Where(x => x.IsDelete != true && x.EmailAccount == EditProfile.EmailAccount).ToList();

                        var list = userList.ToList();

                        var emailExists = userList.Where(x => x.ID != EditProfile.ID).ToList();

                        if (user == null)
                        {
                            return new JsonResponse() { Flag = true, Message = "User is not found" };
                        }
                        else if(user != null && emailExists.Count() > 0)
                        {
                            return new JsonResponse() { Flag = true, Message = "Email already exists" };
                        }
                        else
                        {
                            string ImageName = "";

                            if (!String.IsNullOrEmpty(EditProfile.FileName) && EditProfile.FileName != "\"\"" && EditProfile.FileName != "")
                            {
                                Guid guid = Guid.NewGuid();
                                ImageName = guid.ToString() + "_" + Path.GetFileName(EditProfile.FileName);
                                user.ProfilePicture = ImageName;

                                if(!string.IsNullOrEmpty(EditProfile.Base64Image) && EditProfile.Base64Image != "\"\"" && EditProfile.Base64Image != "")
                                {
                                    byte[] Base64Image = Convert.FromBase64String(EditProfile.Base64Image);
                                    MIUFileServer.SaveToFileServer("ProfileImages", ImageName, Base64Image);
                                }
                            }
                            //user.ID = EditProfile.ID;
                            //user.FullName = EditProfile.FullName;
                            //user.Address = EditProfile.Address;
                            //user.DOB = EditProfile.DOB.Date;
                            user.EmailAccount = EditProfile.EmailAccount;
                            user.ContactNumber = EditProfile.ContactNumber;
                            user.MobilePhoneNumber = EditProfile.ContactNumber;
                            user.ModifiedBy = EditProfile.ModifiedBy;
                            user.ModifiedDate = EditProfile.ModifiedDate;
                            user.OrderDatetime = DateTime.Now;

                            AspNetUser aspNetUser = db.AspNetUsers.Where(x => x.UserName == user.LoginName).SingleOrDefault();
                            aspNetUser.Email = EditProfile.EmailAccount;

                            db.SaveChanges();
                            return new JsonResponse(){ Flag=true,Message="Successfully Updated"};
                        }
                    }
                    catch (Exception ex)
                    {
                        return new JsonResponse() { Flag = false, Message = ex.Message }; 
                    }
                }
            });
        }

        public Task<List<LectureModuleInfo>> GetLectureModuleList(int UserID, int currentIndex, int maxRows)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        List<LectureModuleInfo> infoList = new List<LectureModuleInfo>();
                        var moduleLists = db.TermDetails.Where(x => x.LectureID == UserID && x.Module.IsDelete != true).Select(x => new { ModuleID = x.Module.ID, ModuleName = x.Module.ModuleName, ModuleCode = x.Module.ModuleCode }).ToList();

                        var data = moduleLists.GroupBy(x => x.ModuleID).Skip(currentIndex).Take(maxRows).ToList();

                        foreach (var item in data)
                        {
                            LectureModuleInfo info = new LectureModuleInfo();

                            info.ID = item.Key;
                            info.ModuleName = item.FirstOrDefault().ModuleName;
                            info.ModuleCode = item.FirstOrDefault().ModuleCode;

                            infoList.Add(info);
                        }

                        infoList = infoList.OrderBy(x => x.ModuleName).ToList();

                        return infoList;
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            });
        }

        public Task<JsonResponse> UpdateDeviceToken(int userID, string token)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        User user = db.Users.Where(x => x.ID == userID && x.IsDelete != true).SingleOrDefault();

                        var tokenDevice = db.MobileDeviceTokens.Where(x => x.UserID == userID && x.DeviceToken == token).ToList();
                        MobileDeviceToken data = new MobileDeviceToken();
                        data.UserID = userID;
                        data.DeviceToken = token;
                        data.CreatedBy = user.FullName;
                        data.CreatedDate = DateTime.Now;
                        data.UpdatedBy = user.FullName;
                        data.UpdatedDate = DateTime.Now;

                        if (tokenDevice.Count == 0)
                        {
                            db.MobileDeviceTokens.Add(data);

                            db.SaveChanges();
                        }
                        
                        return new JsonResponse() { Flag = true, Message = "Create Successfully" };
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            });
        }

        public Task<JsonResponse> LogOut(int userID, string token)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        //User user = db.Users.Where(x => x.ID == userID && x.IsDelete != true).SingleOrDefault();

                        List<MobileDeviceToken> tokenDevice = db.MobileDeviceTokens.Where(x => x.UserID == userID && x.DeviceToken == token).ToList();

                        foreach (var item in tokenDevice)
                        {
                            db.MobileDeviceTokens.Remove(item);
                        }

                        db.SaveChanges();
                        
                        return new JsonResponse() { Flag = true, Message = "Success" };
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            });
        }
    }
}