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
    public class ResultController : ApiController
    {
        [HttpGet]
        [ActionName("GetResult")]
        public async Task<HttpResponseMessage> GetResult(int userID, string batchCode)
        {
            try
            {
                ResultDAL dal = new ResultDAL();
                ResultInfo data = await dal.GetResult(userID,batchCode);
                if (data != null)
                {
                    return Request.CreateResponse<ResultInfo>(HttpStatusCode.OK, data);
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
        [ActionName("GetResultDetail")]
        public async Task<HttpResponseMessage> GetResultDetail(int userID, string moduleCode,string moduleStatus,int termDetailID,int assignmentSubmissionID)
        {
            try
            {
                ResultDAL dal = new ResultDAL();
                ResultDetailInfo data = await dal.GetResultDetail(userID, moduleCode,moduleStatus,termDetailID,assignmentSubmissionID);
                if (data != null)
                {
                    return Request.CreateResponse<ResultDetailInfo>(HttpStatusCode.OK, data);
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
        [ActionName("SaveResultIssue")]
        public async Task<HttpResponseMessage> SaveResultIssue(int userID, int termDetailID,string reason,string description)
        {
            try
            {
                ResultDAL dal = new ResultDAL();
                JsonResponse response = await dal.SaveResultIssue(userID, termDetailID,reason,description);
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
        [ActionName("GetResultForSuperAdmin")] //BatchID = 3080
        public async Task<HttpResponseMessage> GetResultForSuperAdmin(int batchID)
        {
            try
            {
                ResultDAL dal = new ResultDAL();
                List<ResultForReturnInfo> data = await dal.GetResultForSuperAdmin(batchID);
                if (data != null)
                {
                    return Request.CreateResponse<List<ResultForReturnInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("GetResultForSuperAdminDetail")] //BatchID = 3080
        public async Task<HttpResponseMessage> GetResultForSuperAdminDetail(int batchID, int moduleID, string grading)
        {
            try
            {
                ResultDAL dal = new ResultDAL();
                ResultForSuperAdminDetailInfo data = await dal.GetResultForSuperAdminDetail(batchID, moduleID, grading);
                if (data != null)
                {
                    return Request.CreateResponse<ResultForSuperAdminDetailInfo>(HttpStatusCode.OK, data);
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
        [ActionName("GetResultForAccessor")] // accessorID = 555 , BatchID = 6134
        public async Task<HttpResponseMessage> GetResultForAccessor(int accessorID, string month = "", string year = "")
        {
            try
            {
                ResultDAL dal = new ResultDAL();
                if (String.IsNullOrEmpty(month) || month == "\"\"")
                {
                    DateTime dateTime = DateTime.Now;
                    month = dateTime.ToString("MMM");
                }
                if (String.IsNullOrEmpty(year) || year == "\"\"")
                {
                    DateTime dateTime = DateTime.Now;
                    year = dateTime.ToString("yyyy");
                }
                List<ResultForReturnInfo> data = await dal.GetResultForAccessor(accessorID, month, year);
                if (data != null)
                {
                    return Request.CreateResponse<List<ResultForReturnInfo>>(HttpStatusCode.OK, data);
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

        //[HttpGet]
        //[ActionName("GetResultForSuperAdminDetail")]
        //public async Task<HttpResponseMessage> GetResultForSuperAdminDetail(int courseID, int batchID, string year, int moduleID, string grading)
        //{
        //    try
        //    {
        //        ResultDAL dal = new ResultDAL();
        //        ResultForSuperAdminDetailInfo data = await dal.GetResultForSuperAdminDetail(courseID, batchID, year,moduleID,grading);
        //        if (data != null)
        //        {
        //            return Request.CreateResponse<ResultForSuperAdminDetailInfo>(HttpStatusCode.OK, data);
        //        }
        //        else
        //        {
        //            return Request.CreateErrorResponse(HttpStatusCode.NotFound, Constants.ErrorNotFound);
        //        }
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        var controllerName = ControllerContext.RouteData.Values["controller"].ToString();
        //        var actionName = ControllerContext.RouteData.Values["action"].ToString();
        //        Logger log = new Logger();
        //        log.ErrorLog(ex, controllerName, actionName);
        //        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, Constants.ErrorSysError);
        //    }

        //}
    }
}
