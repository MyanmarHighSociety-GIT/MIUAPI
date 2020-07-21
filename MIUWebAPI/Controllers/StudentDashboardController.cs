using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
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
    public class StudentDashboardController : ApiController
    {

        [HttpGet]
        [ActionName("GetStudentDashboard")]
        public async Task<HttpResponseMessage> GetStudentDashboard(int userID, int batchID, string role, int currentIndex, int maxRows)
        {
            try
            {
                MIUEntities db = new MIUEntities();
                StudentDashboardInfo data = new StudentDashboardInfo();
                EventCalendarDAL dal = new EventCalendarDAL();
                NewsDAL newsDAL = new NewsDAL();
                ResultDAL resultDAL = new ResultDAL();
                AnnouncementDAL announcementDAL = new AnnouncementDAL();
                ReportAttendanceDAL reportAttendanceDAL = new ReportAttendanceDAL();
                data.EventCalendar = await dal.GetAllEventCalendar(userID, role);
                data.News = await newsDAL.GetNews(userID, currentIndex, maxRows);
                string batchCode = db.Batches.Where(x => x.ID == batchID).Select(x => x.BatchCode).SingleOrDefault();
                data.StudentDashboard = await resultDAL.GetResult(userID, batchCode);
                data.Announcement = await announcementDAL.GetLatestAnnouncement();
                //data.AttRateAndPercent = await reportAttendanceDAL.GetAttRateAndPercent(batchID, userID);

                if (data != null)
                {
                    return Request.CreateResponse<StudentDashboardInfo>(HttpStatusCode.OK, data);
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
