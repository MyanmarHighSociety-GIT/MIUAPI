using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class MIUResourceInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public Nullable<int> IsFolder { get; set; }
        public int FileCount { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string Remark { get; set; }
        public Nullable<int> Type { get; set; }
        public string Keyword { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string FileSize { get; set; }
    }

    public class MIUResourceFolderDetailInfo
    {
        public int ID { get; set; }
        public int MIUResourceID { get; set; }
        public string FilePath { get; set; }
        public string Name { get; set; }
        public string Keyword { get; set; }
        public string FileSize { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}