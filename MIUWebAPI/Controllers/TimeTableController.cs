using MIUWebAPI.DAL;
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
using static MIUWebAPI.ViewModels.TimeTableInfo;

namespace MIUWebAPI.Controllers
{
    [Authorize]
    public class TimeTableController : ApiController
    {
        [HttpGet]
        [ActionName("GetTimeTableList")] //Lecturer
        public async Task<HttpResponseMessage> GetTimeTableList(int BatcheID, string StudentName = "")
        {
            try
            {
                TimeTableDAL dal = new TimeTableDAL();
                List<UserInfo> data = await dal.GetTimeTableList(BatcheID, StudentName);
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
        [ActionName("GetLecturerTimeTable")] //Lecturer
        public async Task<HttpResponseMessage> GetLecturerTimeTable(int batchID, int lecturerID, string date = "") //7163, 555, 2019-08-23
        {
            try
            {
                TimeTableDAL dal = new TimeTableDAL();
                List<LecturerTimeTable> data = await dal.GetLecturerTimeTable(batchID, lecturerID, date);
                if (data != null)
                {
                    return Request.CreateResponse<List<LecturerTimeTable>>(HttpStatusCode.OK, data);
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
        [ActionName("GetAllLecturerTimeTable")] //All Lecturer
        public async Task<HttpResponseMessage> GetAllLecturerTimeTable(int batchID, string date = "")
        {
            try
            {
                TimeTableDAL dal = new TimeTableDAL();
                List<LecturerTimeTable> data = await dal.GetAllLecturerTimeTable(batchID, date);
                if (data != null)
                {
                    return Request.CreateResponse<List<LecturerTimeTable>>(HttpStatusCode.OK, data);
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
        [ActionName("GetStudentTimeTable")] // BatchID = 8170, UserID = 26127, Date = 2019-11-18
        public async Task<HttpResponseMessage> GetStudentTimeTable(int userID, int batchID, string date = "")
        {
            try
            {
                TimeTableDAL dal = new TimeTableDAL();
                List<StudentTimeTable> data = await dal.GetStudentTimeTable(userID, batchID, date);
                if (data != null)
                {
                    return Request.CreateResponse<List<StudentTimeTable>>(HttpStatusCode.OK, data);
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
        [ActionName("GetHoliday")]
        public async Task<HttpResponseMessage> GetHoliday(string Month, int Year)
        {
            try
            {
                TimeTableDAL dal = new TimeTableDAL();
                HolidayWrap data = await dal.GetHoliday(Month, Year);
                if (data != null)
                {
                    return Request.CreateResponse<HolidayWrap>(HttpStatusCode.OK, data);
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
