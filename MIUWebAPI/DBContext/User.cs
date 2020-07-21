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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.AccessorComments = new HashSet<AccessorComment>();
            this.AssignmentBackupUsers = new HashSet<AssignmentBackupUser>();
            this.AssignmentSubmissions = new HashSet<AssignmentSubmission>();
            this.Attendances = new HashSet<Attendance>();
            this.Batches = new HashSet<Batch>();
            this.BatchDetails = new HashSet<BatchDetail>();
            this.BatchDetails1 = new HashSet<BatchDetail>();
            this.BatchDetails2 = new HashSet<BatchDetail>();
            this.Educations = new HashSet<Education>();
            this.EmailBroadcastUsers = new HashSet<EmailBroadcastUser>();
            this.EmergencyContacts = new HashSet<EmergencyContact>();
            this.Enquiries = new HashSet<Enquiry>();
            this.FamilyMembers = new HashSet<FamilyMember>();
            this.FeedBacks = new HashSet<FeedBack>();
            this.IVComments = new HashSet<IVComment>();
            this.Leads = new HashSet<Lead>();
            this.LectureNotis = new HashSet<LectureNoti>();
            this.ManagementComments = new HashSet<ManagementComment>();
            this.MIUPayments = new HashSet<MIUPayment>();
            this.MobileDeviceTokens = new HashSet<MobileDeviceToken>();
            this.ModuleDetails = new HashSet<ModuleDetail>();
            this.Notes = new HashSet<Note>();
            this.Notes1 = new HashSet<Note>();
            this.NotiVisibilities = new HashSet<NotiVisibility>();
            this.ParentStudents = new HashSet<ParentStudent>();
            this.ParentStudents1 = new HashSet<ParentStudent>();
            this.Payments = new HashSet<Payment>();
            this.Remodules = new HashSet<Remodule>();
            this.RemodulePayments = new HashSet<RemodulePayment>();
            this.ResetPasswordTimes = new HashSet<ResetPasswordTime>();
            this.ResultReportIssues = new HashSet<ResultReportIssue>();
            this.ScholarshipDetails = new HashSet<ScholarshipDetail>();
            this.StudentNotis = new HashSet<StudentNoti>();
            this.Subscribtions = new HashSet<Subscribtion>();
            this.TermDetails = new HashSet<TermDetail>();
            this.TermDetails1 = new HashSet<TermDetail>();
            this.TermDetails2 = new HashSet<TermDetail>();
            this.UploadFolders = new HashSet<UploadFolder>();
            this.WorkingExperiences = new HashSet<WorkingExperience>();
        }
    
        public int ID { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> Race { get; set; }
        public string Role { get; set; }
        public Nullable<int> Religion { get; set; }
        public string MaritalStaus { get; set; }
        public Nullable<int> Gender { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string ProfilePicture { get; set; }
        public string ProfilePicture2 { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string HomePhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string EmergencyContactPerson { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string EmergencyEmail { get; set; }
        public string FBAccount { get; set; }
        public string EmailAccount { get; set; }
        public string ContactNumber { get; set; }
        public Nullable<int> Relationship { get; set; }
        public Nullable<int> UserType { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> ApplicationDate { get; set; }
        public string NRC { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string JobPosition { get; set; }
        public Nullable<int> JobType { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> OrderDatetime { get; set; }
        public Nullable<int> IsLogined { get; set; }
        public Nullable<System.DateTime> OrderDatetimeForAlumni { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AccessorComment> AccessorComments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssignmentBackupUser> AssignmentBackupUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssignmentSubmission> AssignmentSubmissions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Attendance> Attendances { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Batch> Batches { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BatchDetail> BatchDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BatchDetail> BatchDetails1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BatchDetail> BatchDetails2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Education> Educations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmailBroadcastUser> EmailBroadcastUsers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmergencyContact> EmergencyContacts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Enquiry> Enquiries { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FamilyMember> FamilyMembers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FeedBack> FeedBacks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IVComment> IVComments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Lead> Leads { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LectureNoti> LectureNotis { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ManagementComment> ManagementComments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MIUPayment> MIUPayments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MobileDeviceToken> MobileDeviceTokens { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ModuleDetail> ModuleDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Note> Notes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Note> Notes1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NotiVisibility> NotiVisibilities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParentStudent> ParentStudents { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ParentStudent> ParentStudents1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Remodule> Remodules { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RemodulePayment> RemodulePayments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResetPasswordTime> ResetPasswordTimes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ResultReportIssue> ResultReportIssues { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ScholarshipDetail> ScholarshipDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentNoti> StudentNotis { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Subscribtion> Subscribtions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TermDetail> TermDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TermDetail> TermDetails1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TermDetail> TermDetails2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UploadFolder> UploadFolders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WorkingExperience> WorkingExperiences { get; set; }
    }
}
