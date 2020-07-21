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

namespace MIUWebAPI.Controllers
{
    [Authorize]
    public class CourseController : ApiController
    {
        [HttpGet]
        [ActionName("GetCourseList")]
        public async Task<HttpResponseMessage> GetCourseList()
        {
            try
            {
                CourseDAL dal= new CourseDAL();
                List<CourseInfo> data = await dal.GetCourseList();
                if (data != null)
                {
                    Logger log = new Logger();
                    log.ErrorLog(new DbEntityValidationException(), "Course", "GetCourse");
                    return Request.CreateResponse<List<CourseInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("GetCourseListForLecturer")]
        public async Task<HttpResponseMessage> GetCourseListForLecturer(int LecturerID)
        {
            try
            {
                CourseDAL dal = new CourseDAL();
                List<CourseInfo> data = await dal.GetCourseListForLecturer(LecturerID);
                if (data != null)
                {
                    Logger log = new Logger();
                    log.ErrorLog(new DbEntityValidationException(), "Course", "GetCourse");
                    return Request.CreateResponse<List<CourseInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("GetCourseListForAssessor")]
        public async Task<HttpResponseMessage> GetCourseListForAssessor(int AssessorID)
        {
            try
            {
                CourseDAL dal = new CourseDAL();
                List<CourseInfo> data = await dal.GetCourseListForAssessor(AssessorID);
                if (data != null)
                {
                    Logger log = new Logger();
                    log.ErrorLog(new DbEntityValidationException(), "Course", "GetCourse");
                    return Request.CreateResponse<List<CourseInfo>>(HttpStatusCode.OK, data);
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
