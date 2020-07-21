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
    public class BatchController : ApiController
    {
        [HttpGet]
        [ActionName("GetBatchByCourseID")]
        public async Task<HttpResponseMessage> GetBatchByCourseID(int courseID)
        {
            try
            {
                BatchDAL dal = new BatchDAL();
                List<BatchInfo> data = await dal.GetBatchByCourseID(courseID);
                if (data != null)
                {
                    return Request.CreateResponse<List<BatchInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("GetBatchByCourseIDForLecturer")]
        public async Task<HttpResponseMessage> GetBatchByCourseIDForLecturer(int courseID, int LecturerID)
        {
            try
            {
                BatchDAL dal = new BatchDAL();
                List<BatchInfo> data = await dal.GetBatchByCourseIDForLecturer(courseID, LecturerID);
                if (data != null)
                {
                    return Request.CreateResponse<List<BatchInfo>>(HttpStatusCode.OK, data);
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
