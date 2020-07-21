using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MIUWebAPI.DBContext;
using MIUWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static MIUWebAPI.Helper.SendEmail;
using static MIUWebAPI.ViewModels.SettingInfo;

namespace MIUWebAPI.DAL
{
    public class SettingDAL
    {
        //public Task<JsonResponse> ChangePassword(ChangePassword req)
        //{
        //    return Task.Run(async () =>
        //    {
        //        using (MIUEntities db = new MIUEntities())
        //        {
        //            try
        //            {
        //                User user = db.Users.Where(x => x.ID == req.UserID).FirstOrDefault();
        //                if (user == null)
        //                {
        //                    return new JsonResponse() { Flag = true, Message = "User is not found" }; ;
        //                }
        //                else
        //                {
        //                    string Message = "";
        //                    if (user.Password == req.CurrentPassword)
        //                    {
        //                        if (req.NewPassword == req.ComfirmPassword)
        //                        {
        //                            user.Password = req.NewPassword;
        //                            db.SaveChanges();
        //                            Message = "Update Password Successfully!.";
        //                        }
        //                        else
        //                        {
        //                            Message = "New password and confirm password are not same.";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Message = "Current Password is incorrect.";
        //                    }
        //                    //db.SaveChanges();
        //                    return new JsonResponse() { Flag = true, Message = Message };
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                return new JsonResponse() { Flag = false, Message = ex.Message };
        //            }
        //        }
        //    });
        //}

        public Task<JsonResponse> ChangePhoneNumber(int UserID, string PhoneNumber)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        User user = db.Users.Where(x => x.ID == UserID).FirstOrDefault();
                        if (user == null)
                        {
                            return new JsonResponse() { Flag = true, Message = "User is not found" }; ;
                        }
                        else
                        {

                            user.ContactNumber = PhoneNumber;
                            db.SaveChanges();
                            string Message = "Update Phone Number Successfully!.";

                            return new JsonResponse() { Flag = true, Message = Message };
                        }
                        //db.SaveChanges();

                    }
                    catch (Exception ex)
                    {
                        return new JsonResponse() { Flag = false, Message = ex.Message };
                    }
                }
            });
        }

        public Task<JsonResponse> TwoFactorAuthentication(string email)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        User user = db.Users.Where(x => x.EmailAccount == email).FirstOrDefault();
                        if (user == null)
                        {
                            return new JsonResponse() { Flag = true, Message = "User is not found" }; ;
                        }
                        else
                        {
                            Random rnd = new Random();
                            string code = rnd.Next(1, 999999).ToString("D6");
                            AspNetUser aspNetUser = db.AspNetUsers.Where(x => x.Email == email).FirstOrDefault();
                            var check = (from a in db.TwoFactorAuthenticatedUsers where a.AspNetUserID == aspNetUser.Id select a.AspNetUserID).FirstOrDefault();
                            TwoFactorAuthenticatedUser auth = new TwoFactorAuthenticatedUser();
                            if (check == null)
                            {
                                auth = new TwoFactorAuthenticatedUser()
                                {
                                    AspNetUserID = aspNetUser.Id,
                                    Code = code,
                                    CreatedDate = DateTime.Now,
                                    UpdatedDate = DateTime.Now
                                };
                                db.TwoFactorAuthenticatedUsers.Add(auth);
                                db.SaveChanges();
                            }
                            else
                            {
                                auth = new TwoFactorAuthenticatedUser();
                                auth = db.TwoFactorAuthenticatedUsers.Where(a => a.AspNetUserID == aspNetUser.Id).FirstOrDefault();
                                auth.Code = code;
                                auth.UpdatedDate = DateTime.Now;
                                db.SaveChanges();
                            }
                            code = "Your two factor authentication code is below \n" + code;
                            EmailManager.SendEmail(code, email, "Two Factor Authentication Code");

                        //db.SaveChanges();
                        return new JsonResponse() { Flag = true, Message = "Successfully Sent", ReferenceKey = auth.AspNetUserID };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new JsonResponse() { Flag = false, Message = ex.Message };
                    }
                }
            });
        }

        public Task<JsonResponse> TwoFactorVerify(string AspNetUserID, string Code)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        TwoFactorAuthenticatedUser user = db.TwoFactorAuthenticatedUsers.Where(x => x.AspNetUserID == AspNetUserID && x.Code == Code).SingleOrDefault();

                        if (user == null)
                        {
                            return new JsonResponse() { Flag = true, Message = "User is not found" };
                        }
                        else
                        {
                            string Message = "Your code is expired.";
                            if (user.UpdatedDate.AddMinutes(2).ToUniversalTime() > DateTime.Now.ToUniversalTime())
                            {
                                user.IsVerified = true;
                            //db.TwoFactorAuthenticatedUsers.Add(user);
                            db.SaveChanges();
                                Message = "Two-Factor Authentication is On.";
                            }
                            return new JsonResponse() { Flag = true, Message = Message };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new JsonResponse() { Flag = false, Message = ex.Message };
                    }
                }
            });
        }

        public Task<JsonResponse> IsTwoFactorVerifyUser(int UserID)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        User user = db.Users.Where(x => x.ID == UserID).SingleOrDefault();
                        if (user == null)
                        {
                            return new JsonResponse() { Flag = true, Message = "User is not found" };
                        }
                        else
                        {
                            string Message = "";
                            var aspNetUserID = db.AspNetUsers.Where(x => x.UserName == user.LoginName).Select(x => x.Id).SingleOrDefault();
                            bool isVerify = db.TwoFactorAuthenticatedUsers.Any(x => x.AspNetUserID == aspNetUserID && x.IsVerified == true);
                            if (isVerify)
                            {
                                Message = "Two-Factor Authentication is On.";
                            }
                            return new JsonResponse() { Flag = true, Message = Message };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new JsonResponse() { Flag = false, Message = ex.Message };
                    }
                }
            });
        }

        public Task<JsonResponse> TurnOffTwoFactor(int UserID)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        User user = db.Users.Where(x => x.ID == UserID).SingleOrDefault();
                        if (user == null)
                        {
                            return new JsonResponse() { Flag = true, Message = "User is not found" };
                        }
                        else
                        {
                        //bool flag = false;
                        string Message = "";
                            var aspNetUserID = db.AspNetUsers.Where(x => x.UserName == user.LoginName).Select(x => x.Id).SingleOrDefault();
                            TwoFactorAuthenticatedUser twoFactorUser = db.TwoFactorAuthenticatedUsers.Where(x => x.AspNetUserID == aspNetUserID).SingleOrDefault();

                            if (twoFactorUser != null)
                            {
                                db.TwoFactorAuthenticatedUsers.Remove(twoFactorUser);
                                db.SaveChanges();
                                Message = "Two-Factor Authentication is Off.";
                            //flag = true;
                        }
                            return new JsonResponse() { Flag = true, Message = Message };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new JsonResponse() { Flag = false, Message = ex.Message };
                    }
                }
            });
        }
        
        public Task<JsonResponse> ForgotPassword(string email)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        User user = db.Users.Where(x => x.EmailAccount == email).FirstOrDefault();
                        if (user == null)
                        {
                            return new JsonResponse() { Flag = true, Message = "User is not found" }; ;
                        }
                        else
                        {
                            Random rnd = new Random();
                            string code = rnd.Next(1, 999999).ToString("D6");
                            AspNetUser aspNetUser = db.AspNetUsers.Where(x => x.Email == email).FirstOrDefault();
                            var check = (from a in db.AspNetUserAuthentications where a.UserID == aspNetUser.Id select a.UserID).FirstOrDefault();
                            AspNetUserAuthentication auth = new AspNetUserAuthentication();
                            if (check == null)
                            {
                                auth = new AspNetUserAuthentication()
                                {
                                    UserID = aspNetUser.Id,
                                    Code = code,
                                    CreatedDate = DateTime.Now
                                };
                                db.AspNetUserAuthentications.Add(auth);
                                db.SaveChanges();
                            }
                            else
                            {
                                auth = new AspNetUserAuthentication();
                                auth = db.AspNetUserAuthentications.Where(a => a.UserID == aspNetUser.Id).FirstOrDefault();
                                auth.Code = code;
                                auth.CreatedDate = DateTime.Now;
                                db.SaveChanges();
                            }
                            code = "Your two factor authentication code is below \n" + code;
                            EmailManager.SendEmail(code, email, "Two Factor Authentication Code");

                            //db.SaveChanges();
                            return new JsonResponse() { Flag = true, Message = "Successfully Sent", ReferenceKey = auth.UserID };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new JsonResponse() { Flag = false, Message = ex.Message };
                    }
                }
            });
        }

        public Task<JsonResponse> ForgotPasswordConfrim(string AspNetUserID, string Code)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        AspNetUserAuthentication user = db.AspNetUserAuthentications.Where(x => x.UserID == AspNetUserID && x.Code == Code).SingleOrDefault();

                        if (user == null)
                        {
                            return new JsonResponse() { Flag = true, Message = "User is not found" };
                        }
                        else
                        {
                            string Message = "Your code is expired.";
                            if (user.CreatedDate.AddMinutes(2).ToUniversalTime() > DateTime.Now.ToUniversalTime())
                            {
                                //user.IsVerified = true;
                                //db.TwoFactorAuthenticatedUsers.Add(user);
                                //db.SaveChanges();
                                
                                Message = "Comfirm ForgotPassword";
                            }
                            return new JsonResponse() { Flag = true, Message = Message };
                        }
                    }
                    catch (Exception ex)
                    {
                        return new JsonResponse() { Flag = false, Message = ex.Message };
                    }
                }
            });
        }



    }
}