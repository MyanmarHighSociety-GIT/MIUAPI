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
    public class EnquiryController : ApiController
    {
        [HttpGet]
        [ActionName("GetLead")]
        public async Task<HttpResponseMessage> GetLead(int CourseID = 0, string Month = "", string Year = "")
        {
            try
            {
                if (String.IsNullOrEmpty(Month) || Month == "\"\"")
                {
                    DateTime dateTime = DateTime.Now;
                    Month = dateTime.ToString("MMM");
                }
                if(String.IsNullOrEmpty(Year) || Year == "\"\"")
                {
                    DateTime dateTime = DateTime.Now;
                    Year = dateTime.ToString("yyyy");
                }
                EnquiryDAL dal = new EnquiryDAL();
                LeadReturn data = await dal.GetLead(CourseID, Month, Year);
                if (data != null)
                {
                    return Request.CreateResponse<LeadReturn>(HttpStatusCode.OK, data);
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
        //[ActionName("GetLead")]
        //public async Task<HttpResponseMessage> GetLead()
        //{
        //    try
        //    {
        //        EnquiryDAL dal = new EnquiryDAL();
        //        List<LeadInfo> data = await dal.GetLead();
        //        if (data != null)
        //        {
        //            return Request.CreateResponse<List<LeadInfo>>(HttpStatusCode.OK, data);
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


        [HttpGet]
        [ActionName("GetLeadByID")]
        public async Task<HttpResponseMessage> GetLeadByID(int ID)
        {
            try
            {
                EnquiryDAL dal = new EnquiryDAL();
                LeadInfo data = await dal.GetLeadByID(ID);
                if(data != null)
                {
                    return Request.CreateResponse<LeadInfo>(HttpStatusCode.OK, data);
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
        [ActionName("GetLeadRelatedTo")]
        public async Task<HttpResponseMessage> GetLeadRelatedTo()
        {
            try
            {
                EnquiryDAL dal = new EnquiryDAL();
                List<LeadRelatedTo> data = await dal.GetLeadRelatedTo();
                if(data != null)
                {
                    return Request.CreateResponse<List<LeadRelatedTo>>(HttpStatusCode.OK, data);
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
        [ActionName("GetLeadActivity")]
        public async Task<HttpResponseMessage> GetLeadActivity(int ID, string Action)
        {
            try
            {
                EnquiryDAL dal = new EnquiryDAL();
                List<LeadActivityInfo> data = await dal.GetLeadActivity(ID, Action);
                if(data != null)
                {
                    return Request.CreateResponse<List<LeadActivityInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("AddLeadActivity")]
        public async Task<HttpResponseMessage> AddLeadActivity(LeadActivityInfo leadActivityInfo)
        {
            try
            {
                EnquiryDAL dal = new EnquiryDAL();
                JsonResponse response = await dal.AddLeadActivity(leadActivityInfo);
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

    }
}
