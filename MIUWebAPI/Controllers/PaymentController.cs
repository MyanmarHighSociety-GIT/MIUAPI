using MIUWebAPI.DAL;
using MIUWebAPI.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using static MIUWebAPI.ViewModels.PaymentViewModel;

namespace MIUWebAPI.Controllers
{
    public class PaymentController : ApiController
    {
        [Authorize]
        public async Task<HttpResponseMessage> GetPayment(int courseID = 0, int batchID = 0, int? userID = 0, string year = "", string month = "", string type = "Total", int currentIndex = 1, int maxRows = 5)
        {
            try
            {
                if (String.IsNullOrEmpty(year) || year == "\"\"")
                {
                    DateTime dateTime = DateTime.Now;
                    year = dateTime.ToString("yyyy");
                }
                PaymentDAL dal = new PaymentDAL();
                List<MIUPaymentInfo> data = await dal.GetPayment(courseID, batchID, userID, year, month, type, currentIndex, maxRows);
                if (data != null)
                {
                    return Request.CreateResponse<List<MIUPaymentInfo>>(HttpStatusCode.OK, data);
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


        public async Task<HttpResponseMessage> GetStudentPayment(int batchID, int userID)
        {
            try
            {
                PaymentDAL dal = new PaymentDAL();
                List<StudentPaymentInfo> StudentPaymentInfo = await dal.GetStudentPayment(batchID, userID);
                List<RemodulePaymentInfo> RemodulePaymentInfoList = await dal.GetRemoduleStudent(batchID, userID);
                PaymentWrap paymentWrap = new PaymentWrap();
                paymentWrap.StudentPayment = StudentPaymentInfo;
                paymentWrap.RemodulePayment = RemodulePaymentInfoList;
                if (paymentWrap != null)
                {
                    return Request.CreateResponse<PaymentWrap>(HttpStatusCode.OK, paymentWrap);
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
