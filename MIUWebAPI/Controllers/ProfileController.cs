using MIUWebAPI.DAL;
using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using MIUWebAPI.Models;
using MIUWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MIUWebAPI.Controllers
{
    [Authorize]
    public class ProfileController : ApiController
    {
        public async Task<HttpResponseMessage> GetProfile(int UserID)
        {
            try
            {
                ProfileDAL dal = new ProfileDAL();
                Profile data = await dal.GetProfile(UserID);
                if (data != null)
                {
                    return Request.CreateResponse<Profile>(HttpStatusCode.OK, data);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, Constants.ErrorNotFound);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var actionName = ControllerContext.RouteData.Values["action"].ToString();
                Logger log = new Logger();
                log.ErrorLog(ex, controllerName, actionName);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Constants.ErrorSysError);
            }
        }

        public async Task<HttpResponseMessage> GetEditProfile(int UserID)
        {
            try
            {
                ProfileDAL dal = new ProfileDAL();
                EditProfile data = await dal.GetEditProfile(UserID);
                if (data != null)
                {
                    return Request.CreateResponse<EditProfile>(HttpStatusCode.OK, data);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, Constants.ErrorNotFound);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var actionName = ControllerContext.RouteData.Values["action"].ToString();
                Logger log = new Logger();
                log.ErrorLog(ex, controllerName, actionName);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Constants.ErrorSysError);
            }
        }

        [HttpPost]
        [ActionName("EditProfile")]
        public async Task<HttpResponseMessage> EditProfile(EditProfile editProfile)
        {
            try
            {
                ProfileDAL dal = new ProfileDAL();
                JsonResponse response = await dal.EditProfile(editProfile);
                if (response!=null && response.Flag)
                {
                    return Request.CreateResponse<JsonResponse>(HttpStatusCode.OK, response);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, Constants.ErrorNotFound);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var actionName = ControllerContext.RouteData.Values["action"].ToString();
                Logger log = new Logger();
                log.ErrorLog(ex, controllerName, actionName);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Constants.ErrorSysError);
            }
        }

        [HttpPost]
        [ActionName("GetLectureModuleList")]
        public async Task<HttpResponseMessage> GetLectureModuleList(int UserID, int currentIndex, int maxRows)
        {
            try
            {
                ProfileDAL dal = new ProfileDAL();
                List<LectureModuleInfo> data = await dal.GetLectureModuleList(UserID, currentIndex, maxRows);
                if (data != null)
                {
                    return Request.CreateResponse<List<LectureModuleInfo>>(HttpStatusCode.OK, data);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, Constants.ErrorNotFound);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var actionName = ControllerContext.RouteData.Values["action"].ToString();
                Logger log = new Logger();
                log.ErrorLog(ex, controllerName, actionName);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Constants.ErrorSysError);
            }
        }

        [HttpPost]
        [ActionName("UpdateDeviceToken")]
        public async Task<HttpResponseMessage> UpdateDeviceToken(int userID, string token)
        {
            try
            {
                ProfileDAL dal = new ProfileDAL();
                JsonResponse data = await dal.UpdateDeviceToken(userID, token);
                if (data != null && data.Flag)
                {
                    return Request.CreateResponse<JsonResponse>(HttpStatusCode.OK, data);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, Constants.ErrorNotFound);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var actionName = ControllerContext.RouteData.Values["action"].ToString();
                Logger log = new Logger();
                log.ErrorLog(ex, controllerName, actionName);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Constants.ErrorSysError);
            }
        }

        [HttpPost]
        [ActionName("LogOut")]
        public async Task<HttpResponseMessage> LogOut(int userID, string token)
        {
            try
            {
                ProfileDAL dal = new ProfileDAL();
                JsonResponse data = await dal.LogOut(userID, token);
                if (data != null && data.Flag)
                {
                    return Request.CreateResponse<JsonResponse>(HttpStatusCode.OK, data);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, Constants.ErrorNotFound);
                }
            }
            catch (DbEntityValidationException ex)
            {
                var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
                var actionName = ControllerContext.RouteData.Values["action"].ToString();
                Logger log = new Logger();
                log.ErrorLog(ex, controllerName, actionName);
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Constants.ErrorSysError);
            }
        }

    }
}
