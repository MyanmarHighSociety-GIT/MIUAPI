using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class NotificationInfo
    {
        public int ID { get; set; }
        public string NotiType { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }

    public class NotiInfo
    {
        public int NotiCount { get; set; }

        public List<NotiListInfo> NotiList { get; set; }
    }

    public class NotiListInfo
    {
        public int ID { get; set; }
        public int NotiTypeId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Message { get; set; }
        public bool IsSeen { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        public long RelatedTableID { get; set; }
        public string RelatedTableName { get; set; }
    }

    public class IsSeenNotiInfo
    {
        public int ID { get; set; }
        public int NotiID { get; set; }
        public int UserID { get; set; }
        public bool IsSeen { get; set; }
    }

    public class LeadNotificationInfo
    {
        public int ID { get; set; }
        public int NotificationId { get; set; }
        public int LeadId { get; set; }
    }
}