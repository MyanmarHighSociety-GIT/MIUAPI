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
    public class EventCalendarDAL
    {
        public Task<List<EventCalendarInfo>> GetEventCalendar(int userID, string role, int currentIndex, int maxRows)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    string loginName = db.Users.Where(a => a.ID == userID).Select(a => a.LoginName).SingleOrDefault();
                    int skip = currentIndex <= 1 ? 0 : (currentIndex - 1) * maxRows;
                    List<EventCalendarInfo> infoList = new List<EventCalendarInfo>();
                    //List<EventCalendar> dataList = db.EventCalendars.OrderByDescending(a => a.UpdatedDate != null ? a.UpdatedDate : a.CreatedDate).Skip(skip).Take(maxRows).ToList();
                    List<EventCalendar> dataList = db.EventCalendars.Where(x => x.EventVisibility.Contains("Public") || x.EventVisibility.Contains(role)).OrderByDescending(a => a.StartDate).Skip(skip).Take(maxRows).ToList();
                    foreach (var data in dataList)
                    {
                        EventCalendarInfo info = new EventCalendarInfo();
                        PropertyCopier<EventCalendar, EventCalendarInfo>.Copy(data, info);
                        string photoName = db.EventCalendarPhotoes.Where(a => a.EventID == data.ID && a.IsCover == true).Select(a => a.PhotoName).SingleOrDefault();
                        info.IsFavorite = db.FavoriteLists.Any(a => a.FavoriteID == data.ID && a.UserID == userID && a.FavoriteType == "EventCalendar");
                        info.IsGoing = db.EventCalendarReacts.Any(a => a.EventID == data.ID && a.LoginName == loginName && a.Action == "Going" && a.Flag==true);
                        info.IsInterested = db.EventCalendarReacts.Any(a => a.EventID == data.ID && a.LoginName == loginName && a.Action == "Interested" && a.Flag == true);
                        info.TotalGoing = db.EventCalendarReacts.Where(a => a.EventID == data.ID && a.Action == "Going" && a.Flag == true).Count();
                        info.TotalInterested = db.EventCalendarReacts.Where(a => a.EventID == data.ID && a.Action == "Interested" && a.Flag == true).Count();
                        if (!string.IsNullOrEmpty(photoName))
                        {
                            info.CoverPhotoPath = MIUFileServer.GetFileUrl("EventCalendar/Photo", photoName);
                        }
                        
                        infoList.Add(info);
                    }
                    return infoList;
                }
            });
        }

        public Task<List<EventCalendarInfo>> GetAllEventCalendar(int userID, string role)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    string loginName = db.Users.Where(a => a.ID == userID).Select(a => a.LoginName).SingleOrDefault();
                    List<EventCalendarInfo> infoList = new List<EventCalendarInfo>();
                    List<EventCalendar> dataList = db.EventCalendars.Where(x => x.EventVisibility.Contains("Public") || x.EventVisibility.Contains(role)).OrderByDescending(a => a.UpdatedDate != null ? a.UpdatedDate : a.CreatedDate).ToList();
                    foreach (var data in dataList)
                    {
                        EventCalendarInfo info = new EventCalendarInfo();
                        PropertyCopier<EventCalendar, EventCalendarInfo>.Copy(data, info);
                        string photoName = db.EventCalendarPhotoes.Where(a => a.EventID == data.ID && a.IsCover == true).Select(a => a.PhotoName).SingleOrDefault();
                        info.IsFavorite = db.FavoriteLists.Any(a => a.FavoriteID == data.ID && a.UserID == userID && a.FavoriteType == "EventCalendar");
                        info.IsGoing = db.EventCalendarReacts.Any(a => a.EventID == data.ID && a.LoginName == loginName && a.Action == "Going" && a.Flag==true);
                        info.IsInterested = db.EventCalendarReacts.Any(a => a.EventID == data.ID && a.LoginName == loginName && a.Action == "Interested" && a.Flag == true);
                        info.TotalGoing = db.EventCalendarReacts.Where(a => a.EventID == data.ID && a.Action == "Going" && a.Flag == true).Count();
                        info.TotalInterested = db.EventCalendarReacts.Where(a => a.EventID == data.ID && a.Action == "Interested" && a.Flag == true).Count();
                        if (!string.IsNullOrEmpty(photoName))
                        {
                            info.CoverPhotoPath = MIUFileServer.GetFileUrl("EventCalendar/Photo", photoName);
                        }
                        
                        infoList.Add(info);
                    }
                    return infoList;
                }
            });
        }
      
        public Task<EventCalendarInfo> GetEventCalendarDetail(int eventID, int userID)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<EventCalendarPhoto> dataList = db.EventCalendarPhotoes.Where(a => a.EventID == eventID).ToList();
                    List<EventCalendarPhotoInfo> infoList = new List<EventCalendarPhotoInfo>();
                    foreach (var data in dataList)
                    {
                        EventCalendarPhotoInfo info = new EventCalendarPhotoInfo();
                        PropertyCopier<EventCalendarPhoto, EventCalendarPhotoInfo>.Copy(data, info);  
                        info.PhotoName = MIUFileServer.GetFileUrl("EventCalendar/Photo", info.PhotoName);
                        infoList.Add(info);
                    }
                    string loginName = db.Users.Where(a => a.ID == userID).Select(a => a.LoginName).SingleOrDefault();
                    EventCalendar eventData = db.EventCalendars.Where(a => a.ID == eventID).SingleOrDefault();
                    EventCalendarInfo eventInfo = new EventCalendarInfo();
                    PropertyCopier<EventCalendar, EventCalendarInfo>.Copy(eventData, eventInfo);
                    eventInfo.IsFavorite = db.FavoriteLists.Any(a => a.FavoriteID == eventData.ID && a.UserID == userID && a.FavoriteType == "EventCalendar");
                    eventInfo.IsGoing = db.EventCalendarReacts.Any(a => a.EventID == eventData.ID && a.LoginName == loginName && a.Action == "Going" && a.Flag == true);
                    eventInfo.IsInterested = db.EventCalendarReacts.Any(a => a.EventID == eventData.ID && a.LoginName == loginName && a.Action == "Interested" && a.Flag == true);
                    eventInfo.TotalGoing = db.EventCalendarReacts.Where(a => a.EventID == eventData.ID && a.Action == "Going" && a.Flag == true).Count();
                    eventInfo.TotalInterested = db.EventCalendarReacts.Where(a => a.EventID == eventData.ID && a.Action == "Interested" && a.Flag == true).Count();
                    eventInfo.CoverPhotoPath = MIUFileServer.GetFileUrl("EventCalendar/Photo", infoList.Where(a => a.IsCover == true).Select(a => a.PhotoName).SingleOrDefault());
                    eventInfo.photoList = infoList;
                    return eventInfo;
                }
            });
        }

        public Task<JsonResponse> EventCalendarReact(int userID, int eventID, string action)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    action = action.ToUpper()=="GOING" ? "Going" : "Interested";
                    bool reactFlag = false;
                    string loginName = db.Users.Where(a => a.ID == userID).Select(a => a.LoginName).SingleOrDefault();
                    EventCalendarReact react = db.EventCalendarReacts.Where(a => a.LoginName == loginName && a.Action == action && a.EventID == eventID).SingleOrDefault();
                    if (react != null)
                    {
                        reactFlag = react.Flag == true ? false : true;

                        react.Flag = react.Flag == true ? false : true;
                    }
                    else
                    {
                        EventCalendarReact newReact = new EventCalendarReact();
                        newReact.LoginName = loginName;
                        newReact.EventID = eventID;
                        newReact.Action = action;
                        newReact.Flag = true;
                        newReact.CreatedDate = DateTime.Now;
                        newReact.CreatedUser = db.Users.Where(a=>a.ID==userID).Select(a=>a.LoginName).SingleOrDefault();
                        db.EventCalendarReacts.Add(newReact);

                        reactFlag = true;
                    }
                    db.SaveChanges();
                    return new JsonResponse() { Flag=true,Message="Successfully Reacted"};
                }
            });
        }

        public Task<List<EventCalendarInfo>> GetEventCalendarFavorite(int userID, string role)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<EventCalendarInfo> dataList = (from ann in db.EventCalendars
                                               join fav in db.FavoriteLists
                                               on ann.ID equals fav.FavoriteID
                                               join eventPhoto in db.EventCalendarPhotoes
                                               on ann.ID equals eventPhoto.EventID
                                               where fav.FavoriteType == "EventCalendar" 
                                               && (ann.EventVisibility.Contains("Public") || ann.EventVisibility.Contains(role)) 
                                               && eventPhoto.IsCover==true && fav.UserID == userID       
                                               //orderby ann.UpdatedDate != null ? ann.UpdatedDate : ann.CreatedDate descending
                                               orderby ann.StartDate
                                               select new EventCalendarInfo()
                                               {
                                                   ID = ann.ID,
                                                   Title = ann.Title,
                                                   StartDate = ann.StartDate,
                                                   EndDate=ann.EndDate,
                                                   StartTime=ann.StartTime,
                                                   EndTime=ann.EndTime,
                                                   Location=ann.Location,
                                                   CoverPhotoName = eventPhoto.PhotoName,
                                                   IsFavorite = true                                      
                                               }).ToList<EventCalendarInfo>();
                    string loginName = db.Users.Where(a => a.ID == userID).Select(a => a.LoginName).SingleOrDefault();
                    foreach (var info in dataList)
                    {   
                        info.IsGoing = db.EventCalendarReacts.Any(a => a.EventID == info.ID && a.LoginName == loginName && a.Action == "Going" && a.Flag == true);
                        info.IsInterested = db.EventCalendarReacts.Any(a => a.EventID == info.ID && a.LoginName == loginName && a.Action == "Interested" && a.Flag == true);
                        info.TotalGoing = db.EventCalendarReacts.Where(a => a.EventID == info.ID && a.Action == "Going" && a.Flag == true).Count();
                        info.TotalInterested = db.EventCalendarReacts.Where(a => a.EventID == info.ID && a.Action == "Interested" && a.Flag == true).Count();
                        info.CoverPhotoPath = MIUFileServer.GetFileUrl("EventCalendar/Photo", info.CoverPhotoName);                        
                    }
                    return dataList;
                  
                }
            });
        }

        public Task<JsonResponse> UpdateEventCalendarFavorite(int userID, int favID, string fav)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    string msg = "";
                    if (fav == "Favorite")
                    {
                        var isDuplicate = db.FavoriteLists.Where(x => x.FavoriteID == favID && x.UserID == userID).Count() > 0;
                        if(!isDuplicate)
                        {
                            FavoriteList newFav = new FavoriteList()
                            {
                                UserID = userID,
                                FavoriteID = favID,
                                FavoriteType = "EventCalendar",
                                CreatedDate = DateTime.Now
                            };
                            db.FavoriteLists.Add(newFav);
                        }

                        msg = "Favorite";
                    }
                    else
                    {
                        FavoriteList favToUpdate = db.FavoriteLists.Where(a => a.UserID == userID && a.FavoriteID == favID && a.FavoriteType == "EventCalendar").SingleOrDefault();
                        db.FavoriteLists.Remove(favToUpdate);
                        msg = "UnFavorite";
                    }
                    db.SaveChanges();
                    return new JsonResponse() { Flag=true,Message= msg };
                }
            });
        }
    }
}