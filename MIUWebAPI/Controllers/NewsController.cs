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
    public class NewsController : ApiController
    {
        [HttpGet]
        [ActionName("GetNews")]
        public async Task<HttpResponseMessage> GetNews(int userID,int currentIndex,int maxRows)
        {
            try
            {
                NewsDAL dal = new NewsDAL();
                List<NewsInfo> data =await dal.GetNews(userID,currentIndex,maxRows);
                if (data != null)
                {
                    return Request.CreateResponse<List<NewsInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("GetTopNews")]
        public async Task<HttpResponseMessage> GetTopNews(int userID)
        {
            try
            { 
                NewsDAL dal = new NewsDAL();
                TopNewsInfo data =await dal.GetTopNews(userID);
                if (data != null)
                {
                    return Request.CreateResponse<TopNewsInfo>(HttpStatusCode.OK, data);
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
        //[ActionName("GetRecentNews")]
        //public async Task<HttpResponseMessage> GetRecentNews(int userID,int perviousDay)
        //{
        //    try
        //    {
        //        NewsDAL dal = new NewsDAL();
        //        List<NewsInfo> data =await dal.GetRecentNews(userID, perviousDay);
        //        if (data != null)
        //        {
        //            return Request.CreateResponse<List<NewsInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("GetMostViewerNews")]
        public async Task<HttpResponseMessage> GetMostViewerNews(int userID,int currentIndex,int maxRows)
        {
            try
            {
                NewsDAL dal = new NewsDAL();
                List<NewsInfo> data =await dal.GetMostViewerNews(userID,currentIndex, maxRows);
                if (data != null)
                {
                    return Request.CreateResponse<List<NewsInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("GetNewsDetail")]
        public async Task<HttpResponseMessage> GetNewsDetail(int newsID, int userID)
        {
            try
            {
                NewsDAL dal = new NewsDAL();
                List<NewsContentInfo> data =await dal.GetNewsDetail(newsID, userID);
                if (data != null)
                {
                    return Request.CreateResponse<List<NewsContentInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("IncreaseViewerCount")]
        public async Task<HttpResponseMessage> IncreaseViewerCount(int userID, int newsID)
        {
            try
            {
                NewsDAL dal = new NewsDAL();
                JsonResponse response = await dal.IncreaseViewerCount(userID, newsID);
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
        [ActionName("GetNewsFavorite")]
        public async Task<HttpResponseMessage> GetNewsFavorite(int userID, int currentIndex, int maxRows)
        {
            try
            {
                NewsDAL dal = new NewsDAL();
                List<NewsInfo> data =await dal.GetNewsFavorite(userID, currentIndex, maxRows);
                if (data != null)
                {
                    return Request.CreateResponse<List<NewsInfo>>(HttpStatusCode.OK, data);
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
        [ActionName("UpdateNewsFavorite")]
        public async Task<HttpResponseMessage> UpdateNewsFavorite(int userID, int favID, string fav)
        {
            try
            {
                NewsDAL dal = new NewsDAL();
                JsonResponse response = await dal.UpdateNewsFavorite(userID, favID, fav);
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
        [ActionName("GetNotiNews")]
        public async Task<HttpResponseMessage> GetNotiNews(int userID, int newsID)
        {
            try
            {
                NotiNews notiNews = new NotiNews();
                NewsDAL dal = new NewsDAL();
                NewsInfo data = await dal.GetNewsByID(userID, newsID);
                notiNews.NewsInfo = data;
                notiNews.NewsContentInfos = await dal.GetNewsDetail(newsID, userID);

                if (data != null)
                {
                    return Request.CreateResponse<NotiNews>(HttpStatusCode.OK, notiNews);
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
