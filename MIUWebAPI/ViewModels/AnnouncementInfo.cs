using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class AnnouncementInfo
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Priority { get; set; }
        public bool IsFavorite { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}