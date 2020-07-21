using MIUWebAPI.DAL;
using MIUWebAPI.Helper;
using MIUWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using static MIUWebAPI.ViewModels.FeedbackInfo;

namespace MIUWebAPI.Controllers
{
    [Authorize]
    public class FeedbackController : ApiController
    {
        [HttpGet]
        [ActionName("GetFeedbackReasonList")]
        public async Task<HttpResponseMessage> GetFeedbackReasonList()
        {
            try
            {
                FeedbackDAL dal = new FeedbackDAL();
                List<FeedbackReasonList> data = await dal.GetFeedbackReasonList();
                if (data != null)
                {
                    return Request.CreateResponse<List<FeedbackReasonList>>(HttpStatusCode.OK, data);
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
        [ActionName("PostFeedback")]
        public async Task<HttpResponseMessage> PostFeedback(int reasonID, string comment, int react, string fullname)
        {
            try
            {
                FeedbackDAL dal = new FeedbackDAL();
                JsonResponse data = await dal.PostFeedback(reasonID, comment, react, fullname);
                if (data != null && data.Flag)
                {
                    return Request.CreateResponse<JsonResponse>(HttpStatusCode.OK, data);
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
