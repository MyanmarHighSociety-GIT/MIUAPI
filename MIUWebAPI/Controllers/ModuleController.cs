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
    public class ModuleController : ApiController
    {
        public async Task<HttpResponseMessage> GetModuleList(int StudentID)
        {
            try
            {
                ModuleDAL dal = new ModuleDAL();
                List<ModuleInfo> data = await dal.GetModuleList(StudentID);
                if (data != null)
                {
                    return Request.CreateResponse<List<ModuleInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("GetAdminModuleList")]
        public async Task<HttpResponseMessage> GetAdminModuleList(int BatchID)
        {
            try
            {
                ModuleDAL dal = new ModuleDAL();
                List<ModuleInfo> data = await dal.GetAdminModuleList(BatchID);
                if (data != null)
                {
                    return Request.CreateResponse<List<ModuleInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("GetModuleReference")]
        public async Task<HttpResponseMessage> GetModuleReference(int ModuleID)
        {
            try
            {
                ModuleDAL dal = new ModuleDAL();
                List<ModuleReferenceInfo> data = await dal.GetModuleReference(ModuleID);
                if (data != null)
                {
                    return Request.CreateResponse<List<ModuleReferenceInfo>>(HttpStatusCode.OK, data);
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
