using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using MIUWebAPI.DAL;
using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using MIUWebAPI.Models;
using MIUWebAPI.Providers;
using MIUWebAPI.Results;
using static MIUWebAPI.Helper.SendEmail;
using static MIUWebAPI.ViewModels.SettingInfo;
//using System.Web.UI.WebControls;

namespace MIUWebAPI.Controllers
{
    [Authorize]
    public class SettingController : ApiController
    {
        //private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        public SettingController()
        {
        }
        public SettingController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpPost]
        [ActionName("PasswordChange")]
        public async Task<HttpResponseMessage> PasswordChange(ChangePassword req)
        {
            try
            {
                SettingDAL dal = new SettingDAL();
                //JsonResponse response = await PasswordChanges(req);
                JsonResponse response = new JsonResponse();
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        User user = db.Users.Where(x => x.ID == req.UserID).FirstOrDefault();
                        if (user == null)
                        {
                            response.Flag = true;
                            response.Message = "User not found.";
                        }
                        else
                        {
                            string Message = "";
                            if (user.Password == req.CurrentPassword)
                            {
                                if (req.NewPassword == req.ComfirmPassword)
                                {
                                    user.Password = req.NewPassword;
                                    var AspNetUserID = db.AspNetUsers.Where(x => x.UserName == user.LoginName).Select(x => x.Id).FirstOrDefault();
                                    IdentityResult result = await UserManager.ChangePasswordAsync(AspNetUserID, req.CurrentPassword, req.NewPassword);

                                    if (!result.Succeeded)
                                    {
                                        Message = "Fail to change password!";
                                    }
                                    else
                                    {
                                        db.SaveChanges();
                                        Message = "Update Password Successfully!.";
                                    }
                                }
                                else
                                {
                                    Message = "New password and confirm password are not same.";
                                }
                            }
                            else
                            {
                                Message = "Current Password is incorrect.";
                            }
                            //db.SaveChanges();
                            response.Flag = true;
                            response.Message = Message;
                        }
                    }
                    catch (Exception ex)
                    {
                        //return new JsonResponse() { Flag = true, Message = Message };
                        response.Flag = true;
                        response.Message = ex.Message;
                    }

                }
                if (response != null && response.Flag)
                //if (response)
                {
                    return Request.CreateResponse<JsonResponse>(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, MIUWebAPI.Helper.Constants.ErrorNotFound);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var actionName = ControllerContext.RouteData.Values["action"].ToString();
                Logger log = new Logger();
                log.ErrorLog(ex, controllerName, actionName);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MIUWebAPI.Helper.Constants.ErrorSysError);
            }
        }

        [HttpPost]
        [ActionName("ChangePhoneNumber")]
        public async Task<HttpResponseMessage> ChangePhoneNumber(int UserID, string PhoneNumber)
        {
            try
            {
                SettingDAL dal = new SettingDAL();
                JsonResponse response = await dal.ChangePhoneNumber(UserID, PhoneNumber);
                if (response != null && response.Flag)
                {
                    return Request.CreateResponse<JsonResponse>(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, MIUWebAPI.Helper.Constants.ErrorNotFound);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var actionName = ControllerContext.RouteData.Values["action"].ToString();
                Logger log = new Logger();
                log.ErrorLog(ex, controllerName, actionName);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MIUWebAPI.Helper.Constants.ErrorSysError);
            }
        }

        [HttpPost]
        [ActionName("TwoFactorAuthentication")]
        public async Task<HttpResponseMessage> TwoFactorAuthentication(string Email)
        {
            try
            {
                SettingDAL dal = new SettingDAL();
                JsonResponse response = await dal.TwoFactorAuthentication(Email);
                if (response != null && response.Flag)
                {
                    return Request.CreateResponse<JsonResponse>(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, MIUWebAPI.Helper.Constants.ErrorNotFound);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var actionName = ControllerContext.RouteData.Values["action"].ToString();
                Logger log = new Logger();
                log.ErrorLog(ex, controllerName, actionName);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MIUWebAPI.Helper.Constants.ErrorSysError);
            }
        }

        [HttpPost]
        [ActionName("TwoFactorVerify")]
        public async Task<HttpResponseMessage> TwoFactorVerify(string AspNetUserID, string Code)
        {
            try
            {
                SettingDAL dal = new SettingDAL();
                JsonResponse response = await dal.TwoFactorVerify(AspNetUserID, Code);
                if (response != null && response.Flag)
                {
                    return Request.CreateResponse<JsonResponse>(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, MIUWebAPI.Helper.Constants.ErrorNotFound);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var actionName = ControllerContext.RouteData.Values["action"].ToString();
                Logger log = new Logger();
                log.ErrorLog(ex, controllerName, actionName);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MIUWebAPI.Helper.Constants.ErrorSysError);
            }
        }

        [HttpPost]
        [ActionName("IsTwoFactorVerifyUser")]
        public async Task<HttpResponseMessage> IsTwoFactorVerifyUser(int UserID)
        {
            try
            {
                SettingDAL dal = new SettingDAL();
                JsonResponse response = await dal.IsTwoFactorVerifyUser(UserID);
                if (response != null && response.Flag)
                {
                    return Request.CreateResponse<JsonResponse>(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, MIUWebAPI.Helper.Constants.ErrorNotFound);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var actionName = ControllerContext.RouteData.Values["action"].ToString();
                Logger log = new Logger();
                log.ErrorLog(ex, controllerName, actionName);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MIUWebAPI.Helper.Constants.ErrorSysError);
            }
        }

        [HttpPost]
        [ActionName("TurnOffTwoFactor")]
        public async Task<HttpResponseMessage> TurnOffTwoFactor(int UserID)
        {
            try
            {
                SettingDAL dal = new SettingDAL();
                JsonResponse response = await dal.TurnOffTwoFactor(UserID);
                if (response != null && response.Flag)
                {
                    return Request.CreateResponse<JsonResponse>(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, MIUWebAPI.Helper.Constants.ErrorNotFound);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var actionName = ControllerContext.RouteData.Values["action"].ToString();
                Logger log = new Logger();
                log.ErrorLog(ex, controllerName, actionName);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MIUWebAPI.Helper.Constants.ErrorSysError);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("ForgotPassword")]
        public async Task<HttpResponseMessage> ForgotPassword(string email)
        {
            try
            {
                SettingDAL dal = new SettingDAL();
                JsonResponse response = await dal.ForgotPassword(email);
                if (response != null && response.Flag)
                {
                    return Request.CreateResponse<JsonResponse>(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, MIUWebAPI.Helper.Constants.ErrorNotFound);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var actionName = ControllerContext.RouteData.Values["action"].ToString();
                Logger log = new Logger();
                log.ErrorLog(ex, controllerName, actionName);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MIUWebAPI.Helper.Constants.ErrorSysError);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ActionName("ForgotPasswordConfrim")]
        public async Task<HttpResponseMessage> ForgotPasswordConfrim(string AspNetUserID, string Code)
        {
            try
            {
                SettingDAL dal = new SettingDAL();
                JsonResponse response = await dal.ForgotPasswordConfrim(AspNetUserID, Code);
                if (response != null && response.Flag)
                {
                    return Request.CreateResponse<JsonResponse>(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, MIUWebAPI.Helper.Constants.ErrorNotFound);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var actionName = ControllerContext.RouteData.Values["action"].ToString();
                Logger log = new Logger();
                log.ErrorLog(ex, controllerName, actionName);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MIUWebAPI.Helper.Constants.ErrorSysError);
            }
        }



        [HttpPost]
        [AllowAnonymous]
        [ActionName("ResetPassword")]
        public async Task<HttpResponseMessage> ResetPassword(ResetPassword req)
        {
            try
            {
                SettingDAL dal = new SettingDAL();
                //JsonResponse response = await PasswordChanges(req);
                JsonResponse response = new JsonResponse();
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        User user = db.Users.Where(x => x.EmailAccount == req.Email).FirstOrDefault();
                        if (user == null)
                        {
                            response.Flag = true;
                            response.Message = "User not found.";
                        }
                        else
                        {
                            string Message = "";
                            if (req.NewPassword == req.ComfirmPassword)
                            {
                                //user.Password = req.NewPassword;
                                var AspNetUserID = db.AspNetUsers.Where(x => x.UserName == user.LoginName).Select(x => x.Id).FirstOrDefault();
                                IdentityResult result = await UserManager.ChangePasswordAsync(AspNetUserID, user.Password, req.NewPassword);

                                if (!result.Succeeded)
                                {
                                    Message = "Fail to reset password!";
                                }
                                else
                                {
                                    user.Password = req.NewPassword;
                                    db.SaveChanges();
                                    Message = "Password Reset Successfully!.";
                                }
                            }
                            else
                            {
                                Message = "New password and confirm password are not same.";
                            }
                            //db.SaveChanges();
                            response.Flag = true;
                            response.Message = Message;
                        }
                    }
                    catch (Exception ex)
                    {
                        //return new JsonResponse() { Flag = true, Message = Message };
                        response.Flag = true;
                        response.Message = ex.Message;
                    }

                }
                if (response != null && response.Flag)
                //if (response)
                {
                    return Request.CreateResponse<JsonResponse>(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, MIUWebAPI.Helper.Constants.ErrorNotFound);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var actionName = ControllerContext.RouteData.Values["action"].ToString();
                Logger log = new Logger();
                log.ErrorLog(ex, controllerName, actionName);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, MIUWebAPI.Helper.Constants.ErrorSysError);
            }
        }
        //public Task<JsonResponse> PasswordChanges(ChangePassword req)
        //{
        //    return Task.Run(async() =>
        //    {
        //        using (MIUEntities db = new MIUEntities())
        //        {
        //            try
        //            {
        //                User user = db.Users.Where(x => x.ID == req.UserID).FirstOrDefault();
        //                if (user == null)
        //                {
        //                    return new JsonResponse() { Flag = true, Message = "User not found." };
        //                }
        //                else
        //                {
        //                    string Message = "";
        //                    if (user.Password == req.CurrentPassword)
        //                    {
        //                        if (req.NewPassword == req.ComfirmPassword)
        //                        {
        //                            user.Password = req.NewPassword;
        //                            var AspNetUserID = db.AspNetUsers.Where(x => x.UserName == user.LoginName).Select(x => x.Id).FirstOrDefault();
        //                            IdentityResult result = await UserManager.ChangePasswordAsync(AspNetUserID, req.CurrentPassword, req.NewPassword);

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
        //                //return new JsonResponse() { Flag = true, Message = Message };
        //                return new JsonResponse() { Flag = false, Message = ex.Message };
        //            }

        //        }
        //    });
        //}

    }
}
