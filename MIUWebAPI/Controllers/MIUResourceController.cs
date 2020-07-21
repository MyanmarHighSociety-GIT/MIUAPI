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
    public class MIUResourceController : ApiController
    {
        //public async Task<HttpResponseMessage> GetMIUResourceKeyword()
        //{
        //    try
        //    {
        //        MIUResourceDAL dal = new MIUResourceDAL();
        //        List<string> data = await dal.GetMIUResourceKeyword();
        //        if (data != null)
        //        {
        //            return Request.CreateResponse<List<string>>(HttpStatusCode.OK, data);
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
        [ActionName("GetMIUResource")]
        public async Task<HttpResponseMessage> GetMIUResource(string Sorting = "", string Name = "")
        {
            try
            {
                MIUResourceDAL dal = new MIUResourceDAL();
                List<MIUResourceInfo> data = await dal.GetMIUResource(Sorting, Name);
                if (data != null)
                {
                    return Request.CreateResponse<List<MIUResourceInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("GetMIUResourceFolderDetail")]
        public async Task<HttpResponseMessage> GetMIUResourceFolderDetail(int MIUResourceID, string Sorting = "", string Name = "")
        {
            try
            {
                MIUResourceDAL dal = new MIUResourceDAL();
                List<MIUResourceFolderDetailInfo> data = await dal.GetMIUResourceFolderDetail(MIUResourceID, Sorting, Name);
                if (data != null)
                {
                    return Request.CreateResponse<List<MIUResourceFolderDetailInfo>>(HttpStatusCode.OK, data);
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
