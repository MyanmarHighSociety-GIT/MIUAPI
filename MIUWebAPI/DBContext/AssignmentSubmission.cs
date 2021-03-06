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
    
    public partial class AssignmentSubmission
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AssignmentSubmission()
        {
            this.AccessorComments = new HashSet<AccessorComment>();
            this.AssignmentSubmissionDetails = new HashSet<AssignmentSubmissionDetail>();
            this.IVComments = new HashSet<IVComment>();
            this.IvNotis = new HashSet<IvNoti>();
            this.LectureNotis = new HashSet<LectureNoti>();
            this.ManagementComments = new HashSet<ManagementComment>();
            this.MangNotis = new HashSet<MangNoti>();
            this.StudentNotis = new HashSet<StudentNoti>();
        }
    
        public int ID { get; set; }
        public int AssignmentID { get; set; }
        public Nullable<int> ModuleID { get; set; }
        public int StudentID { get; set; }
        public string FeedbackToLecture { get; set; }
        public Nullable<int> Feedback1 { get; set; }
        public Nullable<int> Feedback2 { get; set; }
        public Nullable<int> Feedback3 { get; set; }
        public Nullable<int> Feedback4 { get; set; }
        public Nullable<int> Feedback5 { get; set; }
        public Nullable<int> Stage { get; set; }
        public Nullable<int> Feedback6 { get; set; }
        public string Remark { get; set; }
        public string AssignmentTitle { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessorComment> AccessorComments { get; set; }
        public virtual Assignment Assignment { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssignmentSubmissionDetail> AssignmentSubmissionDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IVComment> IVComments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IvNoti> IvNotis { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LectureNoti> LectureNotis { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ManagementComment> ManagementComments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MangNoti> MangNotis { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentNoti> StudentNotis { get; set; }
        public virtual User User { get; set; }
    }
}
