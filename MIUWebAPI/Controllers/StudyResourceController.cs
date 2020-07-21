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
using static MIUWebAPI.ViewModels.StudyResourceInfo;

namespace MIUWebAPI.Controllers
{
    [Authorize]
    public class StudyResourceController : ApiController
    {
        public async Task<HttpResponseMessage> GetStudyResource(int ModuleID, string Sorting)
        {
            try
            {
                StudyResourceDAL dal = new StudyResourceDAL();
                ReturnStudyResource data = await dal.GetStudyResource(ModuleID,Sorting);
                if (data != null)
                {
                    return Request.CreateResponse<ReturnStudyResource>(HttpStatusCode.OK, data);
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
