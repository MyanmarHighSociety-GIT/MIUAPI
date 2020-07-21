using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using MIUWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using MIUWebAPI.Models;

namespace MIUWebAPI.DAL
{
    public class NewsDAL
    {
        public Task<List<NewsInfo>> GetNews(int userID, int currentIndex, int maxRows)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    int skip = currentIndex <= 1 ? 0 : (currentIndex - 1) * maxRows;
                    List<NewsInfo> infoList = new List<NewsInfo>();
                    List<NewsContentInfo> contentinfoList = new List<NewsContentInfo>();
                    List<News> dataList = db.News.Where(x => x.IsDeleted != true && x.IsDraft != true).OrderByDescending(a => a.PublishDate).OrderByDescending(a => a.CreatedDatetime).Skip(skip).Take(maxRows).ToList();
                    foreach (var data in dataList)
                    {
                        NewsInfo info = new NewsInfo();
                        PropertyCopier<News, NewsInfo>.Copy(data, info);
                        info.IsFavorite = db.FavoriteLists.Any(a => a.FavoriteID == data.Id && a.UserID == userID && a.FavoriteType == "News");
                        info.CoverPhotoPath = MIUFileServer.GetFileUrl("News/CoverPhoto", info.CoverPhotoPath);
                        info.Description = db.NewsContents.Where(x => x.NewsId == data.Id).Select(x => x.TextContent).FirstOrDefault();
                        infoList.Add(info);
                    }
                    return infoList;
                }
            });
        }

        public Task<TopNewsInfo> GetTopNews(int userID)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    News data = db.News.Where(x => x.IsDeleted != true && x.IsDraft != true).OrderByDescending(a => a.PublishDate).OrderByDescending(a => a.CreatedDatetime).Take(1).FirstOrDefault();
                    TopNewsInfo info = new TopNewsInfo()
                    {
                        NewsId = data.Id,
                        Title = data.Title,
                        CreatedDatetime = data.CreatedDatetime,
                        PhotoPath = MIUFileServer.GetFileUrl("News/CoverPhoto", data.CoverPhotoPath),
                        TextContent = db.NewsContents.Where(a => a.NewsId == data.Id && a.TextContent != null).Take(1).Select(a => a.TextContent).SingleOrDefault()
                    };
                    return info;
                }
            });
        }

        public Task<List<NewsInfo>> GetRecentNews(int userID, int perviousDay)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    DateTime fromDate = DateTime.Now.AddDays(-perviousDay);//last pervious day
                    DateTime toDate = DateTime.Now;//today
                    List<NewsInfo> infoList = new List<NewsInfo>();
                    List<News> dataList = db.News.Where(a => a.IsDeleted != true && a.IsDraft != true && DbFunctions.TruncateTime(a.PublishDate) >= DbFunctions.TruncateTime(fromDate) && DbFunctions.TruncateTime(a.PublishDate) <= DbFunctions.TruncateTime(toDate)).ToList();
                    foreach (var data in dataList)
                    {
                        NewsInfo info = new NewsInfo();
                        PropertyCopier<News, NewsInfo>.Copy(data, info);
                        info.IsFavorite = db.FavoriteLists.Any(a => a.FavoriteID == data.Id && a.UserID == userID && a.FavoriteType == "News");
                        info.CoverPhotoPath = MIUFileServer.GetFileUrl("News/CoverPhoto", info.CoverPhotoPath);
                        infoList.Add(info);
                    }
                    return infoList;
                }
            });
        }

        public Task<List<NewsInfo>> GetMostViewerNews(int userID, int currentIndex, int maxRows)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    var mostViewer = db.NewsViewers.GroupBy(a => a.NewsID).Select(g => new { NewsID = g.Key, ViewCount = g.Count() }).OrderByDescending(a => a.ViewCount);

                    List<NewsInfo> dataList = db.News.Where(x => x.IsDeleted != true && x.IsDraft != true).Join(mostViewer, n => n.Id, nv => nv.NewsID, (n, nv) =>
                                                 new NewsInfo
                                                 {
                                                     Id = n.Id,
                                                     Title = n.Title,
                                                     NewsCategoryId = n.NewsCategoryId,
                                                     CoverPhotoPath = n.CoverPhotoPath,
                                                     PublishDate = n.PublishDate,
                                                     IsDraft = n.IsDraft,
                                                     IsDeleted = n.IsDeleted,
                                                     CreatedBy = n.CreatedBy,
                                                     CreatedDatetime = n.CreatedDatetime,
                                                     UpdatedBy = n.UpdatedBy,
                                                     UpdatedDatetime = n.UpdatedDatetime
                                                 }).ToList();


                    foreach (var data in dataList)
                    {
                        data.IsFavorite = db.FavoriteLists.Any(a => a.FavoriteID == data.Id && a.UserID == userID && a.FavoriteType == "News");
                        data.CoverPhotoPath = MIUFileServer.GetFileUrl("News/CoverPhoto", data.CoverPhotoPath);
                    }
                    int skip = currentIndex <= 1 ? 0 : (currentIndex - 1) * maxRows;
                    return dataList.Skip(skip).Take(maxRows).ToList();
                }
            });
        }

        public Task<List<NewsContentInfo>> GetNewsDetail(int newsID, int userID)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<NewsContent> dataList = db.NewsContents.Where(a => a.NewsId == newsID).ToList();
                    List<NewsContentInfo> infoList = new List<NewsContentInfo>();
                    foreach (var data in dataList)
                    {
                        NewsContentInfo info = new NewsContentInfo();
                        PropertyCopier<NewsContent, NewsContentInfo>.Copy(data, info);

                        if (!string.IsNullOrEmpty(info.TextContent))
                        {
                            info.ContentType = "Text";
                        }
                        else if (!string.IsNullOrEmpty(info.PhotoPath))
                        {
                            info.ContentType = "Photo";
                            info.ContentPath = MIUFileServer.GetFileUrl("News/ContentPhoto", info.PhotoPath);
                        }
                        else
                        {
                            info.ContentType = "File";
                            info.ContentPath = MIUFileServer.GetFileUrl("News/ContentFile", info.DocPath);
                        }
                        infoList.Add(info);
                    }
                    return infoList;
                }
            });
        }

        public Task<JsonResponse> IncreaseViewerCount(int userID, int newsID)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    bool isViewed = db.NewsViewers.Any(a => a.UserID == userID && a.NewsID == newsID);
                    if (!isViewed)
                    {
                        NewsViewer newsViewer = new NewsViewer()
                        {
                            NewsID = newsID,
                            UserID = userID
                        };
                        db.NewsViewers.Add(newsViewer);
                        db.SaveChanges();
                    }
                    return new JsonResponse() { Flag = true, Message = "Successfully Increased" };
                }
            });
        }

        public Task<List<NewsInfo>> GetNewsFavorite(int userID, int currentIndex, int maxRows)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    int skip = currentIndex <= 1 ? 0 : (currentIndex - 1) * maxRows;
                    List<NewsInfo> dataList = (from ann in db.News
                                               join fav in db.FavoriteLists
                                               on ann.Id equals fav.FavoriteID
                                               where fav.FavoriteType == "News"
                                               && fav.UserID == userID
                                               && ann.IsDeleted != true
                                               && ann.IsDraft != true
                                               orderby ann.UpdatedDatetime != null ? ann.UpdatedDatetime : ann.CreatedDatetime descending
                                               select new NewsInfo()
                                               {
                                                   Id = ann.Id,
                                                   Title = ann.Title,
                                                   NewsCategoryId = ann.NewsCategoryId,
                                                   CoverPhotoName = ann.CoverPhotoPath,
                                                   IsFavorite = true,
                                                   PublishDate = ann.PublishDate,
                                                   IsDraft = ann.IsDraft,
                                                   IsDeleted = ann.IsDeleted,
                                                   CreatedBy = ann.CreatedBy,
                                                   CreatedDatetime = ann.CreatedDatetime,
                                                   UpdatedBy = ann.UpdatedBy,
                                                   UpdatedDatetime = ann.UpdatedDatetime,
                                               }).Skip(skip).Take(maxRows).ToList<NewsInfo>();
                    dataList.ForEach(a => a.CoverPhotoPath = MIUFileServer.GetFileUrl("News/CoverPhoto", a.CoverPhotoName));
                    return dataList;
                }
            });
        }

        public Task<JsonResponse> UpdateNewsFavorite(int userID, int favID, string fav)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    if (fav == "Favorite")
                    {
                        bool exist = db.FavoriteLists.Any(a=>a.UserID==userID && a.FavoriteID==favID && a.FavoriteType=="News");
                        if(!exist)
                        {
                            TimeSpan time = new TimeSpan(00, 06, 30, 0);
                            FavoriteList newFav = new FavoriteList()
                            {
                                UserID = userID,
                                FavoriteID = favID,
                                FavoriteType = "News",
                                CreatedDate = DateTime.UtcNow.Add(time)
                        };
                            db.FavoriteLists.Add(newFav);
                            db.SaveChanges();
                        }                        
                        return new JsonResponse() { Flag = true, Message = "Successfully Favorited" };
                    }
                    else
                    {
                        FavoriteList favToUpdate = db.FavoriteLists.Where(a => a.UserID == userID && a.FavoriteID == favID && a.FavoriteType == "News").SingleOrDefault();
                        if (favToUpdate != null)
                        {
                            db.FavoriteLists.Remove(favToUpdate);
                            db.SaveChanges();
                        }                        
                        return new JsonResponse() { Flag = true, Message = "Successfully Unfavorited" };
                    }                    
                }
            });
        }

        public Task<NewsInfo> GetNewsByID(int userID, int newsID)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    News data = db.News.Where(x => x.Id == newsID && x.IsDeleted != true && x.IsDraft != true).SingleOrDefault();

                    NewsInfo info = new NewsInfo();
                    PropertyCopier<News, NewsInfo>.Copy(data, info);
                    info.IsFavorite = db.FavoriteLists.Any(a => a.FavoriteID == data.Id && a.UserID == userID && a.FavoriteType == "News");
                    info.CoverPhotoPath = MIUFileServer.GetFileUrl("News/CoverPhoto", info.CoverPhotoPath);
                    info.Description = db.NewsContents.Where(x => x.NewsId == data.Id).Select(x => x.TextContent).FirstOrDefault();

                    return info;
                }
            });
        }
    }
}