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
    public class ParentController : ApiController
    {
        [HttpGet]
        [ActionName("GetStudent")]
        public async Task<HttpResponseMessage> GetStudent(int UserID) //32168
        {
            try
            {
                ParentDAL dal = new ParentDAL();
                List<Profile> data = await dal.GetStudent(UserID);
                if (data != null)
                {
                    return Request.CreateResponse<List<Profile>>(HttpStatusCode.OK, data);
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
