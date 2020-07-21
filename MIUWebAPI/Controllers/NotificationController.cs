using MIUWebAPI.DAL;
using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using MIUWebAPI.Models;
using MIUWebAPI.ViewModels;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MIUWebAPI.Controllers
{
    [Authorize]
    public class NotificationController : ApiController
    {
        [HttpGet] 
        [ActionName("GetNotification")] //NOT USE
        public async Task<HttpResponseMessage> GetNotification(int userID,string userRole)
        {
            try
            {
                NotificationDAL dal = new NotificationDAL();
                List<NotificationInfo> data =await dal.GetNotification(userID,userRole);
                if (data != null)
                {
                    return Request.CreateResponse<List<NotificationInfo>>(HttpStatusCode.OK, data);
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

        [HttpGet] 
        [ActionName("GetNotiCount")]
        public async Task<HttpResponseMessage> GetNotiCount(int UserID)
        {
            try
            {
                NotificationDAL dal = new NotificationDAL();
                JsonResult data = await dal.GetNotiCount(UserID);
                if (data != null)
                {
                    //int NotiCount = data.Count;
                    return Request.CreateResponse<JsonResult>(HttpStatusCode.OK, data);
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

        [HttpGet] 
        [ActionName("GetNotificationList")]
        public async Task<HttpResponseMessage> GetNotificationList(int UserID, int currentIndex, int maxRow)
        {
            try
            {
                NotificationDAL dal = new NotificationDAL();
                NotiInfo data = await dal.GetNotificationList(UserID, currentIndex, maxRow);
                if (data != null)
                {
                    return Request.CreateResponse<NotiInfo>(HttpStatusCode.OK, data);
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

        [HttpGet] 
        [ActionName("ChangeSeen")]
        public async Task<HttpResponseMessage> ChangeSeen(int UserID, int NotiID)
        {
            try
            {
                NotificationDAL dal = new NotificationDAL();
                JsonResponse data = await dal.ChangeSeen(UserID, NotiID);
                if (data != null)
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
