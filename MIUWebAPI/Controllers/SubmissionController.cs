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
    //[Authorize]
    public class SubmissionController : ApiController
    {
        [HttpGet]
        [ActionName("GetSubmissionForSuperAdmin")]
        public async Task<HttpResponseMessage> GetSubmissionForSuperAdmin(int userID, int batchID, int currentIndex, int maxRows)
        {
            try
            {
                SubmissionDAL dal = new SubmissionDAL();
                List<SubmissionInfo> data = await dal.GetSubmissionForSuperAdmin(userID, batchID, currentIndex, maxRows);
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
        [HttpGet]
        [ActionName("GetSubmissionForLecture")]
        public async Task<HttpResponseMessage> GetSubmissionForLecture(int accessorID, int batchID = 0)
        {
            try
            {
                SubmissionDAL dal = new SubmissionDAL();
                List<SubmissionInfo> data = await dal.GetSubmissionForLecture(accessorID, batchID);
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

        [HttpGet]
        [ActionName("GetSubmissionForSuperAdminDetail")]
        public async Task<HttpResponseMessage> GetSubmissionForSuperAdminDetail(int batchID, int termDetailID, int submissionType)
        {
            try
            {
                SubmissionDAL dal = new SubmissionDAL();
                SubmissionDetailInfo data = await dal.GetSubmissionForSuperAdminDetail(batchID, termDetailID, submissionType);
                if (data != null)
                {
                    return Request.CreateResponse<SubmissionDetailInfo>(HttpStatusCode.OK, data);
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
        [ActionName("GetSubmissionForSuperAdminStudentDetail")]
        public async Task<HttpResponseMessage> GetSubmissionForSuperAdminStudentDetail(int batchID, int termDetailID, int submissionType, int studentID)
        {
            try
            {
                SubmissionDAL dal = new SubmissionDAL();
                SubmissionStudentDetailInfo data = await dal.GetSubmissionForSuperAdminStudentDetail(batchID, termDetailID, submissionType,studentID);
                if (data != null)
                {
                    return Request.CreateResponse<SubmissionStudentDetailInfo>(HttpStatusCode.OK, data);
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
        [ActionName("ApproveSubmission")]
        public async Task<HttpResponseMessage> ApproveSubmission(int batchID, int termDetailID, int submissionType)
        {
            try
            {
                SubmissionDAL dal = new SubmissionDAL();
                JsonResponse response = await dal.ApproveSubmission(batchID, termDetailID, submissionType);
                if (response != null && response.Flag)
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
