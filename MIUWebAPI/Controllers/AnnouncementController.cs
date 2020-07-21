using MIUWebAPI.DAL;
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
    public class AnnouncementController : ApiController
    {
        [HttpGet]
        [ActionName("GetAnnouncement")]
        public async Task<HttpResponseMessage> GetAnnouncement(int userID, int currentIndex, int maxRows)
        {
            try
            {
                AnnouncementDAL dal = new AnnouncementDAL();
                List<AnnouncementInfo> data =await dal.GetAnnouncement(userID, currentIndex, maxRows);
                if (data != null)
                {
                    return Request.CreateResponse<List<AnnouncementInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("GetAnnouncementFavorite")]
        public async Task<HttpResponseMessage> GetAnnouncementFavorite(int userID, int currentIndex, int maxRows)
        {
            try
            {
                AnnouncementDAL dal = new AnnouncementDAL();
                List<AnnouncementInfo> data =await dal.GetAnnouncementFavorite(userID, currentIndex, maxRows);
                if (data != null)
                {
                    return Request.CreateResponse<List<AnnouncementInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("UpdateAnnouncementFavorite")]
        public async Task<HttpResponseMessage> UpdateAnnouncementFavorite(int userID, int favID, string fav)
        {
            try
            {
                AnnouncementDAL dal = new AnnouncementDAL();
                JsonResponse response = await dal.UpdateAnnouncementFavorite(userID,favID,fav);
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
        
    }
}
