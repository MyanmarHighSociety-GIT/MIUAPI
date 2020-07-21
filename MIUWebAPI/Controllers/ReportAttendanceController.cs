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
using static MIUWebAPI.ViewModels.ReportAttendanceInfo;

namespace MIUWebAPI.Controllers
{
    [Authorize]
    public class ReportAttendanceController : ApiController
    {
        [HttpGet]
        [ActionName("GetReportAttendanceList")]
        public async Task<HttpResponseMessage> GetReportAttendanceList(int BatcheID, string StudentName = "")
        {
            try
            {
                ReportAttendanceDAL dal = new ReportAttendanceDAL();
                List<UserInfo> data = await dal.GetReportAttendanceList(BatcheID, StudentName);
                if (data != null)
                {
                    return Request.CreateResponse<List<UserInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("GetReportAttendance")]
        public async Task<HttpResponseMessage> GetReportAttendance(int BatcheID, int StudentID)
        {
            try
            {
                ReportAttendanceDAL dal = new ReportAttendanceDAL();
                AttendanceGrid data = await dal.GetReportAttendance(BatcheID, StudentID);
                if (data != null)
                {
                    return Request.CreateResponse<AttendanceGrid>(HttpStatusCode.OK, data);
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
        [ActionName("GetStudentAttendance")]
        public async Task<HttpResponseMessage> GetStudentAttendance(int BatcheID, int StudentID)
        {
            try
            {
                ReportAttendanceDAL dal = new ReportAttendanceDAL();
                StudentAttendance data = await dal.GetStudentAttendance(BatcheID, StudentID);
                if (data != null)
                {
                    return Request.CreateResponse<StudentAttendance>(HttpStatusCode.OK, data);
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
