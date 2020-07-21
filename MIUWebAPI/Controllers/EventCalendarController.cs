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
    public class EventCalendarController : ApiController
    {
        [HttpGet]
        [ActionName("GetAllEventCalendar")]
        public async Task<HttpResponseMessage> GetAllEventCalendar(int userID, string role)
        {
            try
            {
                EventCalendarDAL dal = new EventCalendarDAL();
                List<EventCalendarInfo> data = await dal.GetAllEventCalendar(userID, role);
                if (data != null)
                {
                    return Request.CreateResponse<List<EventCalendarInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("GetEventCalendar")]
        public async Task<HttpResponseMessage> GetEventCalendar(int userID, string role, int currentIndex, int maxRows)
        {
            try
            {
                EventCalendarDAL dal = new EventCalendarDAL();
                List<EventCalendarInfo> data = await dal.GetEventCalendar(userID, role, currentIndex, maxRows);
                if (data != null)
                {
                    return Request.CreateResponse<List<EventCalendarInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("GetEventCalendarDetail")]
        public async Task<HttpResponseMessage> GetEventCalendarDetail(int eventID, int userId)
        {
            try
            {
                EventCalendarDAL dal = new EventCalendarDAL();
                EventCalendarInfo data = await dal.GetEventCalendarDetail(eventID, userId);
                if (data != null)
                {
                    return Request.CreateResponse<EventCalendarInfo>(HttpStatusCode.OK, data);
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
        [ActionName("EventCalendarReact")]
        public async Task<HttpResponseMessage> EventCalendarReact(int userID, int eventID,string action)
        {
            try
            {
                EventCalendarDAL dal = new EventCalendarDAL();
                JsonResponse response = await dal.EventCalendarReact(userID, eventID, action);
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

        [HttpGet]
        [ActionName("GetEventCalendarFavorite")]
        public async Task<HttpResponseMessage> GetEventCalendarFavorite(int userID, string role)
        {
            try
            {
                EventCalendarDAL dal = new EventCalendarDAL();
                List<EventCalendarInfo> data = await dal.GetEventCalendarFavorite(userID, role);
                if (data != null)
                {
                    return Request.CreateResponse<List<EventCalendarInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("UpdateEventCalendarFavorite")]
        public async Task<HttpResponseMessage> UpdateEventCalendarFavorite(int userID, int favID, string fav)
        {
            try
            {
                EventCalendarDAL dal = new EventCalendarDAL();
                JsonResponse response = await dal.UpdateEventCalendarFavorite(userID, favID, fav);
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
