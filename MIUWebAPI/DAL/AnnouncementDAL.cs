using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using MIUWebAPI.Models;
using MIUWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MIUWebAPI.DAL
{
    public class AnnouncementDAL
    {
        public Task<List<AnnouncementInfo>> GetAnnouncement(int userID, int currentIndex, int maxRows)
        {

            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    int skip = currentIndex <= 1 ? 0 : (currentIndex - 1) * maxRows;
                    List<AnnouncementInfo> infoList = new List<AnnouncementInfo>();
                    try
                    {
                        //DateTime startDate = DateTime.Now.AddDays(-7);
                        //DateTime endDate = DateTime.Now;
                        string visibilityName = db.Users.Where(x => x.ID == userID).SingleOrDefault().Role;
                        List<Announcement> dataList = db.Announcements
                                                        .Where(w => w.isDraft!= true 
                                                        //&& DbFunctions.TruncateTime(w.ToDate) >= startDate && DbFunctions.TruncateTime(w.FromDate) <= endDate    
                                                        //visibility
                                                            && (
                                                                w.AnnouncementVisibilities.Any(x => x.VisibilityName == "Public")
                                                                || w.AnnouncementVisibilities.Any(x=> x.VisibilityName == visibilityName)
                                                            )

                                                        ).OrderByDescending(a => a.CreatedDate)
                                                        .Skip(skip).Take(maxRows).ToList();

                        foreach (var data in dataList)
                        {
                            AnnouncementInfo info = new AnnouncementInfo();
                            if (data != null)
                            {
                                PropertyCopier<Announcement, AnnouncementInfo>.Copy(data, info);
                                info.Content = data.Description != null ? data.Description : data.Content;
                                info.IsFavorite = db.FavoriteLists.Any(a => a.FavoriteID == data.ID && a.UserID == userID && a.FavoriteType == "Announcement");
                                infoList.Add(info);
                            }

                        }
                    }
                    catch (DbEntityValidationException ex)
                    {
                        Logger log = new Logger();
                        log.ErrorLog(ex, "Announcement", "GetAnnouncement");
                    }
                    
                    return infoList;
                }
            });

        }

        public Task<List<AnnouncementInfo>> GetAnnouncementFavorite(int userID, int currentIndex, int maxRows)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    int skip = currentIndex <= 1 ? 0 : (currentIndex - 1) * maxRows;
                    List<AnnouncementInfo> dataList = (from ann in db.Announcements
                                                       join fav in db.FavoriteLists
                                                       on ann.ID equals fav.FavoriteID
                                                       where fav.FavoriteType == "Announcement"
                                                       && fav.UserID == userID
                                                       orderby ann.CreatedDate descending
                                                       select new AnnouncementInfo()
                                                       {
                                                           ID = ann.ID,
                                                           Title = ann.Title,
                                                           Description = ann.Description,
                                                           Content = ann.Description != null ? ann.Description : ann.Content,
                                                           Priority = ann.Priority,
                                                           IsFavorite = true,
                                                           CreatedDate = ann.CreatedDate
                                                       }).Skip(skip).Take(maxRows).ToList<AnnouncementInfo>();
                    return dataList;
                }
            });
        }

        public Task<JsonResponse> UpdateAnnouncementFavorite(int userID, int favID, string fav)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    TimeSpan time = new TimeSpan(00, 06, 30, 0);
                    if (fav == "Favorite")
                    {
                        FavoriteList newFav = new FavoriteList()
                        {
                            UserID = userID,
                            FavoriteID = favID,
                            FavoriteType = "Announcement",
                            CreatedDate = DateTime.UtcNow.Add(time)
                        };
                        db.FavoriteLists.Add(newFav);
                    }
                    else
                    {
                        FavoriteList favToUpdate = db.FavoriteLists.Where(a => a.UserID == userID && a.FavoriteID == favID && a.FavoriteType == "Announcement").SingleOrDefault();
                        db.FavoriteLists.Remove(favToUpdate);
                    }
                    db.SaveChanges();
                    return new JsonResponse() { Flag=true,Message="Successfully Favorite"};
                }
            });
        }


        public Task<AnnouncementInfo> GetLatestAnnouncement()
        {

            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    DateTime startDate = DateTime.Now.AddDays(-7);
                    DateTime endDate = DateTime.Now;
                    AnnouncementInfo info = new AnnouncementInfo();
                    Announcement data = db.Announcements.Where(w => w.Priority == "High" && w.Status == false && w.isDraft != true).OrderByDescending(w => w.CreatedDate).FirstOrDefault();
                    if(data != null)
                    {
                        PropertyCopier<Announcement, AnnouncementInfo>.Copy(data, info);
                        info.Content = data.Description != null ? data.Description : data.Content;
                    }
                    
                    return info;
                }
            });

        }
    }
}