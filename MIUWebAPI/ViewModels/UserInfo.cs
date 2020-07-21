using MIUWebAPI.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class UserInfo
    {
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
    }

    public class Profile
    {
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
        public string Dob { get; set; }
        public string ProfilePicture { get; set; }
        public string ProfilePicture2 { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string HomePhoneNumber { get; set; }
        public string MobilePhoneNumber { get; set; }
        public string EmergencyContactPerson { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string FBAccount { get; set; }
        public string EmailAccount { get; set; }
        public string ContactNumber { get; set; }
        public Nullable<int> Relationship { get; set; }
        public Nullable<int> UserType { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> ApplicationDate { get; set; }
        public string ApplicationDateString { get; set; }
        public string NRC { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string JobPosition { get; set; }
        public Nullable<int> JobType { get; set; }
        public Nullable<bool> IsDelete { get; set; }

        public BatchInfo Batch { get; set; }
        public CourseInfo Course { get; set; }
    }

    public class EditProfile
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string EmailAccount { get; set; }
        public string ContactNumber { get; set; }
        public string FileName { get; set; }
        public string Base64Image { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}