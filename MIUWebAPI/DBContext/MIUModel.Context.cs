﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MIUEntities : DbContext
    {
        public MIUEntities()
            : base("name=MIUEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AccessorComment> AccessorComments { get; set; }
        public virtual DbSet<AccessorCommentDetail> AccessorCommentDetails { get; set; }
        public virtual DbSet<AlumniRegister> AlumniRegisters { get; set; }
        public virtual DbSet<Announcement> Announcements { get; set; }
        public virtual DbSet<AnnouncementVisibility> AnnouncementVisibilities { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserAuthentication> AspNetUserAuthentications { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Assignment> Assignments { get; set; }
        public virtual DbSet<AssignmentBackup> AssignmentBackups { get; set; }
        public virtual DbSet<AssignmentBackupDetail> AssignmentBackupDetails { get; set; }
        public virtual DbSet<AssignmentBackupUser> AssignmentBackupUsers { get; set; }
        public virtual DbSet<AssignmentCriteria> AssignmentCriterias { get; set; }
        public virtual DbSet<AssignmentCriteriaDetail> AssignmentCriteriaDetails { get; set; }
        public virtual DbSet<AssignmentSubmission> AssignmentSubmissions { get; set; }
        public virtual DbSet<AssignmentSubmissionDetail> AssignmentSubmissionDetails { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<AttendanceTotal> AttendanceTotals { get; set; }
        public virtual DbSet<AuditLog> AuditLogs { get; set; }
        public virtual DbSet<BatchDetail> BatchDetails { get; set; }
        public virtual DbSet<BatchPaymentInfo> BatchPaymentInfoes { get; set; }
        public virtual DbSet<CityInfo> CityInfoes { get; set; }
        public virtual DbSet<CountryInfo> CountryInfoes { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Education> Educations { get; set; }
        public virtual DbSet<Enquiry> Enquiries { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<EventCalendar> EventCalendars { get; set; }
        public virtual DbSet<EventCalendarOrganizer> EventCalendarOrganizers { get; set; }
        public virtual DbSet<EventCalendarPhoto> EventCalendarPhotoes { get; set; }
        public virtual DbSet<EventCalendarReact> EventCalendarReacts { get; set; }
        public virtual DbSet<EventCalendarSpeaker> EventCalendarSpeakers { get; set; }
        public virtual DbSet<EventDetail> EventDetails { get; set; }
        public virtual DbSet<FavoriteList> FavoriteLists { get; set; }
        public virtual DbSet<FeedBack> FeedBacks { get; set; }
        public virtual DbSet<InstallmentPlan> InstallmentPlans { get; set; }
        public virtual DbSet<InstallmentPlanDetail> InstallmentPlanDetails { get; set; }
        public virtual DbSet<IVComment> IVComments { get; set; }
        public virtual DbSet<IvNoti> IvNotis { get; set; }
        public virtual DbSet<JobPosition> JobPositions { get; set; }
        public virtual DbSet<LeadCourse> LeadCourses { get; set; }
        public virtual DbSet<LeadNotification> LeadNotifications { get; set; }
        public virtual DbSet<LearningOutcome> LearningOutcomes { get; set; }
        public virtual DbSet<LectureNoti> LectureNotis { get; set; }
        public virtual DbSet<ManagementComment> ManagementComments { get; set; }
        public virtual DbSet<ManagementResourceFile> ManagementResourceFiles { get; set; }
        public virtual DbSet<MangNoti> MangNotis { get; set; }
        public virtual DbSet<ModuleDetail> ModuleDetails { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<NewsCategory> NewsCategories { get; set; }
        public virtual DbSet<NewsContent> NewsContents { get; set; }
        public virtual DbSet<NewsfeedVisibility> NewsfeedVisibilities { get; set; }
        public virtual DbSet<NewsViewer> NewsViewers { get; set; }
        public virtual DbSet<NewsVisibility> NewsVisibilities { get; set; }
        public virtual DbSet<Note> Notes { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<NotiVisibility> NotiVisibilities { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<RecommendedEbook> RecommendedEbooks { get; set; }
        public virtual DbSet<Remodule> Remodules { get; set; }
        public virtual DbSet<RemoduleDetail> RemoduleDetails { get; set; }
        public virtual DbSet<ResultReportIssue> ResultReportIssues { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Scholarship> Scholarships { get; set; }
        public virtual DbSet<ScholarshipDetail> ScholarshipDetails { get; set; }
        public virtual DbSet<StateInfo> StateInfoes { get; set; }
        public virtual DbSet<StudentNoti> StudentNotis { get; set; }
        public virtual DbSet<StudyResource> StudyResources { get; set; }
        public virtual DbSet<StudyResourceDetail> StudyResourceDetails { get; set; }
        public virtual DbSet<Subscribtion> Subscribtions { get; set; }
        public virtual DbSet<SystemModule> SystemModules { get; set; }
        public virtual DbSet<SystemModulePermission> SystemModulePermissions { get; set; }
        public virtual DbSet<SystemSubModule> SystemSubModules { get; set; }
        public virtual DbSet<SystemSubModuleAction> SystemSubModuleActions { get; set; }
        public virtual DbSet<SystemTimeTable> SystemTimeTables { get; set; }
        public virtual DbSet<Template> Templates { get; set; }
        public virtual DbSet<TemplateDetail> TemplateDetails { get; set; }
        public virtual DbSet<TemplateDetailSub> TemplateDetailSubs { get; set; }
        public virtual DbSet<Term> Terms { get; set; }
        public virtual DbSet<TermDetail> TermDetails { get; set; }
        public virtual DbSet<TimeSetting> TimeSettings { get; set; }
        public virtual DbSet<TimeTableDetail> TimeTableDetails { get; set; }
        public virtual DbSet<TodayQuote> TodayQuotes { get; set; }
        public virtual DbSet<UploadFile> UploadFiles { get; set; }
        public virtual DbSet<UploadFolder> UploadFolders { get; set; }
        public virtual DbSet<UserDetail> UserDetails { get; set; }
        public virtual DbSet<WorkingExperience> WorkingExperiences { get; set; }
        public virtual DbSet<NewsNoti> NewsNotis { get; set; }
        public virtual DbSet<vProgramPlan> vProgramPlans { get; set; }
        public virtual DbSet<MIUResourceFolderDetail> MIUResourceFolderDetails { get; set; }
        public virtual DbSet<TwoFactorAuthenticatedUser> TwoFactorAuthenticatedUsers { get; set; }
        public virtual DbSet<EmergencyContact> EmergencyContacts { get; set; }
        public virtual DbSet<FeedbackReason> FeedbackReasons { get; set; }
        public virtual DbSet<ResetPasswordTime> ResetPasswordTimes { get; set; }
        public virtual DbSet<DownloadHistory> DownloadHistories { get; set; }
        public virtual DbSet<MICResourceFile> MICResourceFiles { get; set; }
        public virtual DbSet<StudentFeedback> StudentFeedbacks { get; set; }
        public virtual DbSet<Holiday> Holidays { get; set; }
        public virtual DbSet<Batch> Batches { get; set; }
        public virtual DbSet<Module> Modules { get; set; }
        public virtual DbSet<WebNotification> WebNotifications { get; set; }
        public virtual DbSet<EmailBroadcast> EmailBroadcasts { get; set; }
        public virtual DbSet<EmailBroadcastUser> EmailBroadcastUsers { get; set; }
        public virtual DbSet<LeadDoc> LeadDocs { get; set; }
        public virtual DbSet<ParentStudent> ParentStudents { get; set; }
        public virtual DbSet<PaymentSetting> PaymentSettings { get; set; }
        public virtual DbSet<PaymentSettingDetail> PaymentSettingDetails { get; set; }
        public virtual DbSet<WebNotificationDetail> WebNotificationDetails { get; set; }
        public virtual DbSet<MIUPayment> MIUPayments { get; set; }
        public virtual DbSet<MIUPaymentDetail> MIUPaymentDetails { get; set; }
        public virtual DbSet<RemodulePayment> RemodulePayments { get; set; }
        public virtual DbSet<LeadActivity> LeadActivities { get; set; }
        public virtual DbSet<MobileDeviceToken> MobileDeviceTokens { get; set; }
        public virtual DbSet<Lead> Leads { get; set; }
        public virtual DbSet<FamilyMember> FamilyMembers { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}
