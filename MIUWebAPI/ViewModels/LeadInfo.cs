using MIUWebAPI.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class LeadReturn
    {
        public LeadCount LeadCount { get; set; }
        public List<LeadInfo> LeadInfos { get; set; }
    }

    public class LeadCount
    {
        public int TotalLeadPercent { get; set; }
        public int ContactedPercent { get; set; }
        public int RegisteredPercent { get; set; }
        public int MissedPercent { get; set; }
        public int TotalLead { get; set; }
        public int Contacted { get; set; }
        public int Registered { get; set; }
        public int Missed { get; set; }
    }

    public class LeadInfo
    {
        public int ID { get; set; }
        public Nullable<int> LeadOwner { get; set; }
        public string LeadType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> Probability { get; set; }
        public Nullable<int> ProspectusYear { get; set; }
        public string Mobile { get; set; }
        public string HomePhone { get; set; }
        public string Industry { get; set; }
        public string Email { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string LeadSource { get; set; }
        public string Gender { get; set; }
        public Nullable<int> Country { get; set; }
        public string CountryName { get; set; }
        public Nullable<int> State { get; set; }
        public string StateName { get; set; }
        public string Township { get; set; }
        public Nullable<int> City { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }
        public string Remark { get; set; }
        public string Status { get; set; }
        public string ContactName { get; set; }
        public Nullable<int> CourseID { get; set; }
        public string CourseName { get; set; }
        public string ContactNumber { get; set; }
        public string Relationship { get; set; }
        public string ContactHomePhone { get; set; }
        public string ConvertedStudentID { get; set; }
        public string ConvertedContactID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedUser { get; set; }
        public string CreatedUpdatedUserProfile { get; set; }

        public List<FamilyMemberInfo> FamilyMembers { get; set; }
    }

    public class FamilyMemberInfo
    {
        public int? ID { get; set; }
        public string FullName { get; set; }
        public string ProfilePicture { get; set; }
    }

    public class LeadActivityInfo
    {
        public int ID { get; set; }
        public Nullable<int> LeadID { get; set; }
        public string Action { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string Subject { get; set; }
        public string Remark { get; set; }
        public Nullable<int> RelatedTo { get; set; }
        public string Related { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string UpdatedUser { get; set; }
    }

    public class LeadRelatedTo
    {
        public int ID { get; set; }
        public string Title { get; set; }
    }
}