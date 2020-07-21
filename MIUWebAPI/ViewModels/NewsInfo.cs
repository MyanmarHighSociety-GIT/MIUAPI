using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class NewsInfo
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int NewsCategoryId { get; set; }
        public string CoverPhotoPath { get; set; }
        public string CoverPhotoName { get; set; }
        public System.DateTime PublishDate { get; set; }
        public bool IsDraft { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<long> ViewerCount { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsFavorite { get; set; }
        public Nullable<System.DateTime> UpdatedDatetime { get; set; }

        public string Description { get; set; }
    }
    public class NewsContentInfo
    {
        public long Id { get; set; }
        public long NewsId { get; set; }
        public string TextContent { get; set; }
        public string PhotoPath { get; set; }
        public string DocPath { get; set; }
        public string DocName { get; set; }
        public int Sequence { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string ContentPath { get; set; }
        public string ContentType { get; set; }
        public Nullable<System.DateTime> UpdatedDatetime { get; set; }
    }
    public class TopNewsInfo
    {
        public long NewsId { get; set; }
        public string Title { get; set; }
        public string TextContent { get; set; }
        public string PhotoPath { get; set; }        
        public System.DateTime CreatedDatetime { get; set; }
    }

    public class NotiNews
    {
        public NewsInfo NewsInfo { get; set; }

        public List<NewsContentInfo> NewsContentInfos { get; set; }
    }
}