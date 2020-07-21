//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MIUWebAPI.DBContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class MICResourceFile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MICResourceFile()
        {
            this.DownloadHistories = new HashSet<DownloadHistory>();
            this.MIUResourceFolderDetails = new HashSet<MIUResourceFolderDetail>();
        }
    
        public int ID { get; set; }
        public string Name { get; set; }
        public Nullable<int> IsFolder { get; set; }
        public Nullable<int> ParentID { get; set; }
        public string Remark { get; set; }
        public Nullable<int> Type { get; set; }
        public string Keyword { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string FileSize { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DownloadHistory> DownloadHistories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MIUResourceFolderDetail> MIUResourceFolderDetails { get; set; }
    }
}