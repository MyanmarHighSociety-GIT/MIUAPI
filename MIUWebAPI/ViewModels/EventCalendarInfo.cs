using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class EventCalendarInfo
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string EventVisibility { get; set; }
        public string DressCode { get; set; }
        public string Description { get; set; }
        public Nullable<bool> AllDay { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Location { get; set; }
        public Nullable<bool> RSVP { get; set; }
        public Nullable<bool> IsLimit { get; set; }
        public Nullable<int> NumberLimit { get; set; }
        public string FacebookLink { get; set; }
        public string InstagramLink { get; set; }
        public string TwitterLink { get; set; }
        public string LinkedInLink { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsGoing { get; set; }
        public bool IsInterested { get; set; }
        public int TotalGoing { get; set; }
        public int TotalInterested { get; set; }
        public string CoverPhotoName { get; set; }
        public string CoverPhotoPath { get; set; }
        public List<EventCalendarPhotoInfo> photoList { get; set; }
    }
    public class EventCalendarPhotoInfo
    {
        public int ID { get; set; }
        public Nullable<int> EventID { get; set; }
        public string PhotoName { get; set; }
        public Nullable<bool> IsCover { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedUser { get; set; }
    }
}