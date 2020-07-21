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
    
    public partial class News
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public News()
        {
            this.NewsContents = new HashSet<NewsContent>();
            this.NewsNotis = new HashSet<NewsNoti>();
            this.NewsVisibilities = new HashSet<NewsVisibility>();
        }
    
        public long Id { get; set; }
        public string Title { get; set; }
        public int NewsCategoryId { get; set; }
        public string CoverPhotoPath { get; set; }
        public System.DateTime PublishDate { get; set; }
        public bool IsDraft { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDatetime { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDatetime { get; set; }
    
        public virtual NewsCategory NewsCategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NewsContent> NewsContents { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NewsNoti> NewsNotis { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NewsVisibility> NewsVisibilities { get; set; }
    }
}
