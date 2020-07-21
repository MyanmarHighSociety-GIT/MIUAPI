using MIUWebAPI.DAL;
using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
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
    public class DashboardController : ApiController
    {
        [HttpGet]
        [ActionName("GetLectureCourseList")]
        public async Task<HttpResponseMessage> GetLectureCourseList(int accessorID)
        {
            try
            {
                MIUEntities db = new MIUEntities();
                List<LectureCourseListInfo> data = new List<LectureCourseListInfo>();
                StudentDashboardDAL dal = new StudentDashboardDAL();
                //data.AttRateAndPercent = await reportAttendanceDAL.GetAttRateAndPercent(batchID, userID);
                data = await dal.GetLectureCourseList(accessorID);

                if (data != null)
                {
                    return Request.CreateResponse<List<LectureCourseListInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("GetLectureModuleList")]
        public async Task<HttpResponseMessage> GetLectureModuleList(int accessorID)
        {
            try
            {
                MIUEntities db = new MIUEntities();
                List<LectureModuleListInfo> data = new List<LectureModuleListInfo>();
                StudentDashboardDAL dal = new StudentDashboardDAL();                
                data = await dal.GetLectureModuleList(accessorID);

                if (data != null)
                {
                    return Request.CreateResponse<List<LectureModuleListInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("GetLectureDashboard")]
        public async Task<HttpResponseMessage> GetLectureDashboard(int accessorID, string role, int courseID = 0)
        {
            try
            {
                MIUEntities db = new MIUEntities();
                LectureDashboardInfo data = new LectureDashboardInfo();
                StudentDashboardDAL dal = new StudentDashboardDAL();
                NewsDAL newsDAL = new NewsDAL();
                ResultDAL resultDAL = new ResultDAL();
                AnnouncementDAL announcementDAL = new AnnouncementDAL();
                EventCalendarDAL eventDAL = new EventCalendarDAL();
                //data.AttRateAndPercent = await reportAttendanceDAL.GetAttRateAndPercent(batchID, userID);
                data = await dal.GetLectureDashboard(accessorID, courseID);
                data.Announcement = await announcementDAL.GetLatestAnnouncement();
                data.News = newsDAL.GetNews(accessorID, 1, 5).Result;
                data.EventCalendar = eventDAL.GetAllEventCalendar(accessorID, role).Result;
                //data.EventCalendar = eventDAL.GetEventCalendar(accessorID, 1, 5).Result;

                if (data != null)
                {
                    return Request.CreateResponse<LectureDashboardInfo>(HttpStatusCode.OK, data);
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
        [ActionName("GetDashboardSubmission")]
        public async Task<HttpResponseMessage> GetDashboardSubmission()
        {
            try
            {
                MIUEntities db = new MIUEntities();
                List<SubmissionInfo> data = new List<SubmissionInfo>();
                SubmissionDAL dal = new SubmissionDAL();
                //data.AttRateAndPercent = await reportAttendanceDAL.GetAttRateAndPercent(batchID, userID);
                data = await dal.GetSubmissionForSuperAdmin(893, 0, 1, 5);

                
                if (data != null)
                {
                    return Request.CreateResponse<List<SubmissionInfo>>(HttpStatusCode.OK, data);
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
