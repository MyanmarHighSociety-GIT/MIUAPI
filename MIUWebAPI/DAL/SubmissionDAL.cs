using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using MIUWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using MIUWebAPI.Models;

namespace MIUWebAPI.DAL
{
    public class SubmissionDAL
    {
        public Task<List<SubmissionInfo>> GetSubmissionForSuperAdmin(int userID, int? batchID, int currentIndex, int maxRows)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    if(batchID == 0)
                    {
                        var AssignmentSubmissionDetails = db.AssignmentSubmissionDetails.ToList().LastOrDefault();

                        int studentID = db.AssignmentSubmissions.Where(x => x.StudentID == AssignmentSubmissionDetails.AssignmentSubmission.StudentID).ToList().Select(x => x.StudentID).FirstOrDefault();

                        batchID = db.BatchDetails.Where(x => x.StudentID == studentID).ToList().Select(x => x.BatchID).FirstOrDefault();
                    }
                    int skip = currentIndex <= 1 ? 0 : (currentIndex - 1) * maxRows;
                    int take = maxRows;

                    List<SubmissionInfo> infoList = new List<SubmissionInfo>();

                    string courseName = db.Batches.Where(a => a.ID == batchID).Select(a => a.Course.CourseName).SingleOrDefault();
                    string batchCode = db.Batches.Where(a => a.ID == batchID).Select(a => a.BatchCode).SingleOrDefault();

                    List<TermDetail> termDetailList = db.TermDetails.Where(x => x.Term.BatchID == batchID && x.Assignment == true).OrderBy(x => x.sortOrder).ToList();
                    List<User> stdList = db.BatchDetails.Where(x => x.BatchID == batchID).Join(db.Users, b => b.StudentID, u => u.ID, (b, u) => new { u }).Select(a => a.u).Where(a => a.UserType == 1).ToList();

                    foreach (var termDetail in termDetailList)
                    {

                        #region First

                        bool isFirstSubmission = db.Assignments.Any(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 1);
                        //int i = 0;
                        if (isFirstSubmission)
                        {
                            SubmissionInfo info = new SubmissionInfo();
                            info.TermDetailID = termDetail.ID;
                            info.SubmissionType = 1;
                            info.BatchID = batchID.Value;
                            bool isFirstApprove = db.Assignments.Any(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 1 && a.Status == 1 && a.Status == 2);

                            if (!isFirstApprove)
                            {

                                info.ModuleName = db.Modules.Where(a => a.ID == termDetail.ModuleID).Select(a => a.ModuleName).SingleOrDefault();
                                info.CourseName = courseName;
                                info.BatchCode = batchCode;

                                //int assignment = db.Assignments.Where(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 1 && (a.Status == 0 || a.Status == null)).Select(a => a.ID).SingleOrDefault();// not approve record
                                int assignment = db.Assignments.Where(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 1).Select(a => a.ID).SingleOrDefault();// not approve record

                                List<AssignmentSubmission> assSubmissionList = db.AssignmentSubmissions.Where(a => a.AssignmentID == assignment).ToList();

                                List<int> assSubList = assSubmissionList.Select(a => a.ID).ToList();

                                List<int> assSubStdList = assSubmissionList.Where(a => a.Assignment.TermDetail.Term.BatchID == batchID).Select(a => a.StudentID).ToList();

                                string ivName = db.IVComments.Where(a => assSubList.Contains(a.AssignmentSubmissionID)).Select(a => a.User.FullName).FirstOrDefault();

                                int assessorCheck = db.AccessorComments.Join(db.AssignmentSubmissions.Where(a => a.AssignmentID == assignment), acc => acc.ID, ass => ass.AssignmentID, (acc, ass) => new { acc }).Distinct().Select(a => a.acc.AssignmentSubmissionID).Count();
                                //int ivCheck = db.IVComments.Join(db.AssignmentSubmissions.Where(a => a.AssignmentID == assignment), iv => iv.ID, ass => ass.AssignmentID, (iv, ass) => new { iv }).Distinct().Select(a => a.iv.AssignmentSubmissionID).Count();
                                int ivCheck = db.IVComments.Where(a => assSubList.Contains(a.AssignmentSubmissionID)).Select(a => a.AssignmentSubmissionID).Distinct().Count();

                                //info.AssessorName = assessorName;
                                info.AssessorCheck = assessorCheck;
                                //info.AssessorLeft = info.Submitted - info.AssessorCheck;

                                //info.IVName = ivName;
                                info.IVCheck = ivCheck;
                                //info.IVLeft = info.Submitted - info.IVCheck;

                                if (termDetail.FirstSubmission != null)
                                {
                                    info.DueDate = termDetail.FirstSubmission.Value;
                                };

                                //DateTime? date = db.AssignmentSubmissionDetails.Where(x => x.AssignmentSubmissionID == assi).Select(x => x.ModifiedDate.Value).LastOrDefault();

                                var stdRealCount = stdList.Where(a => assSubStdList.Contains(a.ID)).Count();

                                //foreach (var std in stdList)
                                //{
                                //    var stdAssignmentID = assSubmissionList.Where(x => x.StudentID == std.StudentID && x.Assignment.TermDetailID == termDetailID).FirstOrDefault();

                                //    var stdAssDetail = db.AssignmentSubmissionDetails.Where(x => x.AssignmentSubmissionID == stdAssignmentID.ID).FirstOrDefault();

                                //    std.SubmissionDate = stdAssDetail.ModifiedDate;
                                //}

                                info.Submitted = stdRealCount;
                                //info.IVLeft = info.Submitted - info.IVCheck;
                                info.NoSubmission = stdList.Count() - info.Submitted;
                            }


                            infoList.Add(info);
                            continue;
                        }

                        #endregion

                        #region Second

                        bool isFinalSubmission = db.Assignments.Any(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 2);

                        if (isFinalSubmission)
                        {
                            SubmissionInfo info = new SubmissionInfo();
                            info.TermDetailID = termDetail.ID;
                            info.TermDetailID = termDetail.ID;
                            info.SubmissionType = 2;
                            info.BatchID = batchID.Value;
                            bool isSecondApprove = db.Assignments.Any(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 2 && a.Status == 2);
                            if (!isSecondApprove)
                            {

                                info.ModuleName = db.Modules.Where(a => a.ID == termDetail.ModuleID).Select(a => a.ModuleName).SingleOrDefault();
                                info.CourseName = courseName;
                                info.BatchCode = batchCode;

                                //int assignment = db.Assignments.Where(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 1 && (a.Status == 0 || a.Status == null)).Select(a => a.ID).SingleOrDefault();// not approve record
                                int assignment = db.Assignments.Where(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 1).Select(a => a.ID).SingleOrDefault();// not approve record

                                List<AssignmentSubmission> assSubmissionList = db.AssignmentSubmissions.Where(a => a.AssignmentID == assignment).ToList();

                                List<int> assSubList = assSubmissionList.Select(a => a.ID).ToList();

                                List<int> assSubStdList = assSubmissionList.Where(a => a.Assignment.TermDetail.Term.BatchID == batchID).Select(a => a.StudentID).ToList();

                                string ivName = db.IVComments.Where(a => assSubList.Contains(a.AssignmentSubmissionID)).Select(a => a.User.FullName).FirstOrDefault();

                                int assessorCheck = db.AccessorComments.Join(db.AssignmentSubmissions.Where(a => a.AssignmentID == assignment), acc => acc.ID, ass => ass.AssignmentID, (acc, ass) => new { acc }).Distinct().Select(a => a.acc.AssignmentSubmissionID).Count();
                                //int ivCheck = db.IVComments.Join(db.AssignmentSubmissions.Where(a => a.AssignmentID == assignment), iv => iv.ID, ass => ass.AssignmentID, (iv, ass) => new { iv }).Distinct().Select(a => a.iv.AssignmentSubmissionID).Count();
                                int ivCheck = db.IVComments.Where(a => assSubList.Contains(a.AssignmentSubmissionID)).Select(a => a.AssignmentSubmissionID).Distinct().Count();

                                //info.AssessorName = assessorName;
                                info.AssessorCheck = assessorCheck;
                                //info.AssessorLeft = info.Submitted - info.AssessorCheck;

                                //info.IVName = ivName;
                                info.IVCheck = ivCheck;
                                //info.IVLeft = info.Submitted - info.IVCheck;

                                if (termDetail.FirstSubmission != null)
                                {
                                    info.DueDate = termDetail.FirstSubmission.Value;
                                };

                                //DateTime? date = db.AssignmentSubmissionDetails.Where(x => x.AssignmentSubmissionID == assi).Select(x => x.ModifiedDate.Value).LastOrDefault();

                                var stdRealCount = stdList.Where(a => assSubStdList.Contains(a.ID)).Count();

                                //foreach (var std in stdList)
                                //{
                                //    var stdAssignmentID = assSubmissionList.Where(x => x.StudentID == std.StudentID && x.Assignment.TermDetailID == termDetailID).FirstOrDefault();

                                //    var stdAssDetail = db.AssignmentSubmissionDetails.Where(x => x.AssignmentSubmissionID == stdAssignmentID.ID).FirstOrDefault();

                                //    std.SubmissionDate = stdAssDetail.ModifiedDate;
                                //}

                                info.Submitted = stdRealCount;
                                //info.IVLeft = info.Submitted - info.IVCheck;
                                info.NoSubmission = stdList.Count() - info.Submitted;
                                infoList.Add(info);
                            }
                        }

                        #endregion

                    }
                    return infoList.Skip(skip).Take(take).ToList();
                }
            });
        }

        public Task<SubmissionDetailInfo> GetSubmissionForSuperAdminDetail(int batchID, int termDetailID, int submissionType)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    SubmissionDetailInfo info = new SubmissionDetailInfo();

                    string courseName = db.Batches.Where(a => a.ID == batchID).Select(a => a.Course.CourseName).SingleOrDefault();
                    string batchCode = db.Batches.Where(a => a.ID == batchID).Select(a => a.BatchCode).SingleOrDefault();

                    TermDetail termDetail = db.TermDetails.Where(x => x.ID == termDetailID && x.Assignment == true).OrderBy(x => x.sortOrder).SingleOrDefault();
                    //List<User> stdList = db.BatchDetails.Where(x => x.BatchID == batchID).Join(db.Users, b => b.StudentID, u => u.ID, (b, u) => new { u }).Select(a => a.u).Where(a => a.UserType == 1).ToList();
                    List<User> stdList = db.BatchDetails.Where(x => x.BatchID == batchID && x.User2.UserType == 1).Select(x => x.User2).ToList();

                    var accessorID = db.TermDetails.Where(a => a.ID == termDetailID).Select(a => a.AccessorID).SingleOrDefault();
                    string assessorName = "";
                    if (accessorID != null)
                    {
                        int assessorID = int.Parse(accessorID.ToString());
                        // int ivID = db.IVComments.Where(a => a.AssignmentSubmissionID == assignmentSubmissionID).Select(a => a.IVID).SingleOrDefault();

                        assessorName = db.Users.Where(a => a.ID == assessorID).Select(a => a.FullName).SingleOrDefault();
                    }
                    
                    // string ivName= db.Users.Where(a => a.ID == ivID).Select(a => a.FullName).SingleOrDefault(); ;

                    if (submissionType == 1)
                    {
                        #region First

                        //bool isFirstApprove = db.Assignments.Any(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 1 && a.Status == 1 && a.Status == 2);

                        //if (!isFirstApprove)
                        //{

                            info.ModuleName = db.Modules.Where(a => a.ID == termDetail.ModuleID).Select(a => a.ModuleName).SingleOrDefault();
                            info.CourseName = courseName;
                            info.BatchCode = batchCode;

                            //int assignment = db.Assignments.Where(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 1 && (a.Status == 0 || a.Status == null)).Select(a => a.ID).SingleOrDefault();// not approve record
                            int assignment = db.Assignments.Where(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 1).Select(a => a.ID).SingleOrDefault();// not approve record
                            
                            List<AssignmentSubmission> assSubmissionList = db.AssignmentSubmissions.Where(a => a.AssignmentID == assignment).ToList();

                            List<int> assSubList = assSubmissionList.Select(a => a.ID).ToList();

                            List<int> assSubStdList = assSubmissionList.Where(a => a.Assignment.TermDetail.Term.BatchID == batchID).Select(a => a.StudentID).ToList(); 

                            string ivName = db.IVComments.Where(a => assSubList.Contains(a.AssignmentSubmissionID)).Select(a => a.User.FullName).FirstOrDefault();

                            int assessorCheck = db.AccessorComments.Join(db.AssignmentSubmissions.Where(a => a.AssignmentID == assignment), acc => acc.ID, ass => ass.AssignmentID, (acc, ass) => new { acc }).Distinct().Select(a => a.acc.AssignmentSubmissionID).Count();
                            //int ivCheck = db.IVComments.Join(db.AssignmentSubmissions.Where(a => a.AssignmentID == assignment), iv => iv.ID, ass => ass.AssignmentID, (iv, ass) => new { iv }).Distinct().Select(a => a.iv.AssignmentSubmissionID).Count();
                            int ivCheck = db.IVComments.Where(a => assSubList.Contains(a.AssignmentSubmissionID)).Select(a => a.AssignmentSubmissionID).Distinct().Count();

                            info.AssessorName = assessorName;
                            info.AssessorCheck = assessorCheck;
                            info.AssessorLeft = info.Submitted - info.AssessorCheck;

                            info.IVName = ivName;
                            info.IVCheck = ivCheck;
                            //info.IVLeft = info.Submitted - info.IVCheck;

                            if(termDetail.FirstSubmission != null)
                            {
                                info.DueDate = termDetail.FirstSubmission.Value;
                            };

                            //DateTime? date = db.AssignmentSubmissionDetails.Where(x => x.AssignmentSubmissionID == assi).Select(x => x.ModifiedDate.Value).LastOrDefault();

                            info.StudentList = stdList.Where(a => assSubStdList.Contains(a.ID)).Select(a => new SubmissionStudentInfo { StudentID = a.ID, StudentName = a.FullName, LoginName = a.LoginName, StudentPhoto = MIUFileServer.GetFileUrl("ProfileImages", a.ProfilePicture) }).ToList();

                            foreach (var std in info.StudentList)
                            {
                                var stdAssignmentID = assSubmissionList.Where(x => x.StudentID == std.StudentID && x.Assignment.TermDetailID == termDetailID).FirstOrDefault();

                                var stdAssDetail = db.AssignmentSubmissionDetails.Where(x => x.AssignmentSubmissionID == stdAssignmentID.ID).FirstOrDefault();

                                std.SubmissionDate = stdAssDetail.ModifiedDate;
                            }

                            info.Submitted = info.StudentList.Count();
                            info.IVLeft = info.Submitted - info.IVCheck;
                            info.NoSubmission = stdList.Count() - info.Submitted;
                        //}

                        #endregion
                    }
                    else
                    {
                        #region Second

                        //bool isSecondApprove = db.Assignments.Any(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 2 && a.Status == 2);

                        //if (!isSecondApprove)
                        //{
                            info.ModuleName = db.Modules.Where(a => a.ID == termDetail.ModuleID).Select(a => a.ModuleName).SingleOrDefault();
                            info.CourseName = courseName;
                            info.BatchCode = batchCode;

                            //int assignment = db.Assignments.Where(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 2 && (a.Status == 0 || a.Status == null)).Select(a => a.ID).SingleOrDefault();// not approve record
                            int assignment = db.Assignments.Where(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 2).Select(a => a.ID).SingleOrDefault();

                            List<AssignmentSubmission> assSubmissionList = db.AssignmentSubmissions.Where(a => a.AssignmentID == assignment).ToList();

                            List<int> assSubList = assSubmissionList.Where(a => a.Assignment.TermDetail.Term.BatchID == batchID).Select(a => a.ID).ToList();

                            List<int> assSubStdList = assSubmissionList.Where(a => a.Assignment.TermDetail.Term.BatchID == batchID).Select(a => a.StudentID).ToList();

                            string ivName = db.IVComments.Where(a => assSubList.Contains(a.AssignmentSubmissionID)).Select(a => a.User.FullName).FirstOrDefault();

                            int assessorCheck = db.AccessorComments.Join(db.AssignmentSubmissions.Where(a => a.AssignmentID == assignment), acc => acc.ID, ass => ass.AssignmentID, (acc, ass) => new { acc }).Distinct().Select(a => a.acc.AssignmentSubmissionID).Count();
                            //int ivCheck = db.IVComments.Join(db.AssignmentSubmissions.Where(a => a.AssignmentID == assignment), iv => iv.ID, ass => ass.AssignmentID, (iv, ass) => new { iv }).Distinct().Select(a => a.iv.AssignmentSubmissionID).Count();
                            int ivCheck = db.IVComments.Where(a => assSubList.Contains(a.AssignmentSubmissionID)).Select(a => a.AssignmentSubmissionID).Distinct().Count();
                            //info.Submitted = assSubList.Count();
                            //info.NoSubmission = stdList.Count() - assSubList.Count();

                            info.AssessorName = assessorName;
                            info.AssessorCheck = assessorCheck;
                            info.AssessorLeft = info.Submitted - info.AssessorCheck;

                            info.IVName = ivName;
                            info.IVCheck = ivCheck;

                            if(termDetail.FirstSubmission != null)
                            {
                                info.DueDate = termDetail.FirstSubmission.Value;
                            }

                            //info.StudentList = stdList.Where(a => assSubStdList.Contains(a.ID)).Select(a => new SubmissionStudentInfo { StudentID = a.ID, StudentName = a.FullName, SubmissionDate = a.ModifiedDate != null ? a.ModifiedDate : a.CreatedDate, LoginName = a.LoginName }).ToList();
                            info.StudentList = stdList.Where(a => assSubStdList.Contains(a.ID)).Select(a => new SubmissionStudentInfo { StudentID = a.ID, StudentName = a.FullName, LoginName = a.LoginName, StudentPhoto = MIUFileServer.GetFileUrl("ProfileImages", a.ProfilePicture) }).ToList();

                            foreach (var std in info.StudentList)
                            {
                                var stdAssignmentID = assSubmissionList.Where(x => x.StudentID == std.StudentID && x.Assignment.TermDetailID == termDetailID).FirstOrDefault();

                                var stdAssDetail = db.AssignmentSubmissionDetails.Where(x => x.AssignmentSubmissionID == stdAssignmentID.ID).FirstOrDefault();

                                std.SubmissionDate = stdAssDetail.ModifiedDate;
                            }
                        //}

                        info.Submitted = info.StudentList.Count();
                        info.IVLeft = info.Submitted - info.IVCheck;
                        info.NoSubmission = stdList.Count() - info.Submitted;

                        #endregion
                    }
                    return info;
                }
            });
        }

        public Task<SubmissionStudentDetailInfo> GetSubmissionForSuperAdminStudentDetail(int batchID, int termDetailID, int submissionType, int studentID)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    SubmissionStudentDetailInfo info = new SubmissionStudentDetailInfo();

                    string courseName = db.Batches.Where(a => a.ID == batchID).Select(a => a.Course.CourseName).SingleOrDefault();
                    string batchCode = db.Batches.Where(a => a.ID == batchID).Select(a => a.BatchCode).SingleOrDefault();

                    TermDetail termDetail = db.TermDetails.Where(x => x.ID == termDetailID && x.Assignment == true).OrderBy(x => x.sortOrder).SingleOrDefault();
                    User student = db.Users.Where(a => a.ID == studentID).SingleOrDefault();

                    if (submissionType == 1)
                    {
                        #region First

                        bool isFirstApprove = db.Assignments.Any(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 1 && a.Status == 1 && a.Status == 2);

                        //if (!isFirstApprove)
                        //{

                            info.ModuleName = db.Modules.Where(a => a.ID == termDetail.ModuleID).Select(a => a.ModuleName).SingleOrDefault();
                            info.CourseName = courseName;
                            info.BatchCode = batchCode;

                            //int assignment = db.Assignments.Where(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 1 && (a.Status == 0 || a.Status == null)).Select(a => a.ID).SingleOrDefault();// not approve record
                            int assignment = db.Assignments.Where(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 1).Select(a => a.ID).SingleOrDefault();// all record first submission

                            int assSub = db.AssignmentSubmissions.Where(a => a.AssignmentID == assignment && a.StudentID == studentID).Select(a => a.ID).SingleOrDefault();

                            int assessorID = int.Parse(db.TermDetails.Where(a => a.ID == termDetailID).Select(a => a.AccessorID).SingleOrDefault().ToString());
                            int ivID = db.IVComments.Where(a => a.AssignmentSubmissionID == assSub).Select(a => a.IVID).SingleOrDefault();

                            string assessorName = db.Users.Where(a => a.ID == assessorID).Select(a => a.FullName).SingleOrDefault();
                            string ivName = db.Users.Where(a => a.ID == ivID).Select(a => a.FullName).SingleOrDefault();

                            info.AssessorName = assessorName;

                            info.IVName = ivName;

                            if(termDetail.FirstSubmission != null)
                            {
                                info.DueDate = termDetail.FirstSubmission.Value;
                            }

                            info.StudentName = student.FullName;

                            //info.StudentPhoto = MIUFileServer.GetFileUrl("NoImage", "StudentProfile.png");
                            info.StudentPhoto = MIUFileServer.GetFileUrl("ProfileImages", student.ProfilePicture);
                            //info.SubmissionDate = db.AssignmentSubmissionDetails.Where(a => a.AssignmentSubmissionID == assSub && a.AssignmentSubmission.StudentID == studentID).OrderByDescending(a => a.ModifiedDate).Select(a => a.ModifiedDate.Value).FirstOrDefault();

                            try
                            {
                                info.SubmissionDate = db.AssignmentSubmissionDetails.Where(a => a.AssignmentSubmissionID == assSub && a.AssignmentSubmission.StudentID == studentID).OrderByDescending(a => a.ModifiedDate).Select(a => a.ModifiedDate.Value).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }

                            info.IsAssessorCheck = assSub == 0 ? false : true;

                            info.IsIVCheck = ivID == 0 ? false : true;

                            info.IsResultOut = false;

                            info.FileList = db.AssignmentSubmissionDetails.Where(a => a.AssignmentSubmissionID == assSub).Select(a => new SubmissionFile { FileName = a.FileName, IsMain = a.IsMain }).ToList();


                        //}

                        #endregion
                    }
                    else
                    {
                        #region Second

                        bool isSecondApprove = db.Assignments.Any(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 2 && a.Status == 2);

                        //if (!isSecondApprove)
                        //{
                            info.ModuleName = db.Modules.Where(a => a.ID == termDetail.ModuleID).Select(a => a.ModuleName).SingleOrDefault();
                            info.CourseName = courseName;
                            info.BatchCode = batchCode;

                            //int assignment = db.Assignments.Where(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 2 && (a.Status == 0 || a.Status == null)).Select(a => a.ID).SingleOrDefault();// not approve record
                            int assignment = db.Assignments.Where(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 2).Select(a => a.ID).SingleOrDefault();// all record final submission

                            int assSub = db.AssignmentSubmissions.Where(a => a.AssignmentID == assignment && a.StudentID == studentID).Select(a => a.ID).SingleOrDefault();

                            int assessorID = int.Parse(db.TermDetails.Where(a => a.ID == termDetailID).Select(a => a.AccessorID).SingleOrDefault().ToString());
                            int ivID = db.IVComments.Where(a => a.AssignmentSubmissionID == assSub).Select(a => a.IVID).SingleOrDefault();

                            string assessorName = db.Users.Where(a => a.ID == assessorID).Select(a => a.FullName).SingleOrDefault();
                            string ivName = db.Users.Where(a => a.ID == ivID).Select(a => a.FullName).SingleOrDefault(); ;

                            int assessorCheck = db.AccessorComments.Join(db.AssignmentSubmissions.Where(a => a.AssignmentID == assignment), acc => acc.ID, ass => ass.AssignmentID, (acc, ass) => new { acc }).Distinct().Select(a => a.acc.AssignmentSubmissionID).Count();
                            int ivCheck = db.IVComments.Join(db.AssignmentSubmissions.Where(a => a.AssignmentID == assignment), iv => iv.ID, ass => ass.AssignmentID, (iv, ass) => new { iv }).Distinct().Select(a => a.iv.AssignmentSubmissionID).Count();

                            info.AssessorName = assessorName;

                            info.IVName = ivName;

                            if(termDetail.FirstSubmission != null)
                            {
                                info.DueDate = termDetail.FirstSubmission.Value;
                            }

                            info.StudentName = student.FullName;

                            //info.StudentPhoto = MIUFileServer.GetFileUrl("NoImage", "StudentProfile.png");
                            info.StudentPhoto = MIUFileServer.GetFileUrl("ProfileImages", student.ProfilePicture);

                            try
                            {
                                info.SubmissionDate = db.AssignmentSubmissionDetails.Where(a => a.AssignmentSubmissionID == assSub && a.AssignmentSubmission.StudentID == studentID).OrderByDescending(a => a.ModifiedDate).Select(a => a.ModifiedDate.Value).FirstOrDefault();
                            }
                            catch (Exception ex)
                            {

                            }

                            info.IsAssessorCheck = assSub == 0 ? false : true;

                            info.IsIVCheck = ivID == 0 ? false : true;

                            info.IsResultOut = false;

                            info.FileList = db.AssignmentSubmissionDetails.Where(a => a.AssignmentSubmissionID == assSub).Select(a => new SubmissionFile { FileName = a.FileName, IsMain = a.IsMain }).ToList();

                        //}

                        #endregion
                    }
                    return info;
                }
            });
        }

        public Task<List<SubmissionInfo>> GetSubmissionForLecture(int accessorID, int batchID = 0)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    //int skip = currentIndex <= 1 ? 0 : (currentIndex - 1) * maxRows;
                    //int take = maxRows;
                    List<SubmissionInfo> infoList = new List<SubmissionInfo>();

                    //var lectureSubmissionList1 = db.AssignmentSubmissions.Where(x =>
                    //                   //x.Assignment.Status == 0
                    //                    x.Assignment.SubmissionType == 1
                    //                   && x.Assignment.TermDetail.LectureID == accessorID
                    //                   //&& x.Assignment.TermDetail.Term.Batch.IsDelete != true
                    //                   //&& x.Assignment.TermDetail.Module.IsDelete != true
                    //                   && (batchID == 0 ? true : x.Assignment.TermDetail.Term.Batch.ID == batchID)

                    //                   ).ToList();

                    //var batches = lectureSubmissionList1.Select(x => x.Assignment).Select(y => y.TermDetail).Select(z => z.Term.Batch).GroupBy(b=> b).Select(c=> c.Key).ToList();

                    #region First
                    var lectureSubmissionList = db.AssignmentSubmissions
                                                .Where(x =>
                                                //x.Assignment.Status == 0
                                                x.Assignment.SubmissionType == 1
                                                && x.Assignment.TermDetail.LectureID == accessorID
                                                && (batchID == 0 ? true : x.Assignment.TermDetail.Term.Batch.ID == batchID )
                                                //&& x.Assignment.TermDetail.Term.Batch.IsDelete != true
                                                && x.Assignment.TermDetail.Module.IsDelete != true

                                                ).ToList();

                    var courseList = lectureSubmissionList.Where(x => x.Assignment.TermDetail.LectureID == accessorID)
                                    .GroupBy(x => x.Assignment.TermDetail.Term.Batch.Course.ID)
                                    .Select(y => new { CourseID = y.Key, CourseName = y.FirstOrDefault().Assignment.TermDetail.Term.Batch.Course.CourseName });

                    foreach (var course in courseList)
                    {
                        var batchList = lectureSubmissionList.Where(x => x.Assignment.TermDetail.Term.Batch.Course.ID == course.CourseID)
                                        .GroupBy(x => x.Assignment.TermDetail.Term.Batch.ID)
                                        .Select(y => new { BatchID = y.Key, BatchName = y.FirstOrDefault().Assignment.TermDetail.Term.Batch.BatchName });

                        string courseName = db.Courses.Where(x => x.ID == course.CourseID).Select(x => x.CourseName).SingleOrDefault();

                        foreach (var batch in batchList)
                        {
                            string batchCode = db.Batches.Where(a => a.ID == batch.BatchID).Select(a => a.BatchCode).SingleOrDefault();

                            List<TermDetail> termDetailList = db.TermDetails.Where(x => x.Term.BatchID == batch.BatchID && x.Assignment == true).OrderBy(x => x.sortOrder).ToList();
                            List<User> stdList = db.BatchDetails.Where(x => x.BatchID == batch.BatchID).Select(a => a.User2).Where(a => a.UserType == 1 && a.IsDelete != true).ToList();
                            //List<User> stdList = db.BatchDetails.Where(x => x.BatchID == batch.BatchID).Join(db.Users, b => b.StudentID, u => u.ID, (b, u) => new { u }).Select(a => a.u).Where(a => a.UserType == 1).ToList();


                            var moduleList = lectureSubmissionList
                                            .Where(x => x.Assignment.TermDetail.Term.Batch.ID == batch.BatchID)
                                            .GroupBy(x => x.Assignment.TermDetail.Module.ID)
                                            .Select(y => new { ModuleID = y.Key, ModuleName = y.FirstOrDefault().Assignment.TermDetail.Module.ModuleName }).ToList();


                            foreach (var module in moduleList)
                            {

                                var lectureSub = lectureSubmissionList
                                                .Where(x => x.Assignment.TermDetail.Term.Batch.CourseID == course.CourseID
                                                 && x.Assignment.TermDetail.Term.Batch.ID == batch.BatchID
                                                 && x.Assignment.TermDetail.Module.ID == module.ModuleID).ToList();

                                SubmissionInfo info = new SubmissionInfo();
                                info.BatchID = batch.BatchID;
                                info.ModuleName = module.ModuleName;
                                info.CourseName = courseName;
                                info.BatchCode = batchCode;
                                info.SubmissionType = lectureSub.FirstOrDefault().Assignment.SubmissionType;
                                info.Submitted = lectureSub.Count();
                                info.NoSubmission = stdList.Count() - info.Submitted;
                                info.TermDetailID = lectureSub.FirstOrDefault().Assignment.TermDetailID;
                                info.DueDate = lectureSub.FirstOrDefault().Assignment.TermDetail.FirstSubmission;

                                List<AccessorComment> acList = new List<AccessorComment>();
                                List<IVComment> ivList = new List<IVComment>();
                                foreach (var submission in lectureSub)
                                {
                                    AccessorComment ac = new AccessorComment();
                                    IVComment iv = new IVComment();
                                    ac = db.AccessorComments.Where(x => x.AssignmentSubmissionID == submission.ID && x.AccessorID != null).FirstOrDefault();
                                    iv = db.IVComments.Where(x => x.AssignmentSubmissionID == submission.ID && x.IVID != null).FirstOrDefault();

                                    if (ac != null)
                                        acList.Add(ac);

                                    if (iv != null)
                                        ivList.Add(iv);
                                }

                                info.AssessorCheck = acList.Count();
                                info.IVCheck = ivList.Count();

                                infoList.Add(info);
                            }


                            //foreach (var termDetail in termDetailList)
                            //{

                            //    #region First

                            //    bool isFirstApprove = db.Assignments.Any(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 1 && a.Status == 1 && a.Status == 2);
                            //    //int i = 0;
                            //    if (!isFirstApprove)
                            //    {
                            //        SubmissionInfo info = new SubmissionInfo();
                            //        info.ModuleName = db.Modules.Where(a => a.ID == termDetail.ModuleID).Select(a => a.ModuleName).SingleOrDefault();
                            //        info.CourseName = courseName;
                            //        info.BatchCode = batchCode;

                            //        int assignment = db.Assignments.Where(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 1 && (a.Status == 0 || a.Status == null)).Select(a => a.ID).SingleOrDefault();// not approve record

                            //        List<int> assSubList = db.AssignmentSubmissions.Where(a => a.AssignmentID == assignment).Select(a => a.ID).ToList();

                            //        int assessorCheck = db.AccessorComments.Where(a => assSubList.Contains(a.AssignmentSubmissionID)).Select(a => a.AssignmentSubmissionID).Distinct().Count();
                            //        int ivCheck = db.IVComments.Where(a => assSubList.Contains(a.AssignmentSubmissionID)).Select(a => a.AssignmentSubmissionID).Distinct().Count();

                            //        info.Submitted = assSubList.Count();
                            //        info.NoSubmission = stdList.Count() - assSubList.Count();
                            //        info.AssessorCheck = assessorCheck;
                            //        info.IVCheck = ivCheck;
                            //        info.SubmissionType = 1;
                            //        if(termDetail.FirstSubmission != null)
                            //        {
                            //            info.DueDate = termDetail.FirstSubmission.Value;
                            //        }
                            //        info.BatchID = termDetail.Term.Batch.ID;
                            //        info.TermDetailID = termDetail.ID;

                            //        infoList.Add(info);
                            //    }

                            //    #endregion

                            //    #region Second

                            //    bool isSecondApprove = db.Assignments.Any(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 2 && a.Status == 2);

                            //    if (!isFirstApprove)
                            //    {
                            //        SubmissionInfo info = new SubmissionInfo();
                            //        info.ModuleName = db.Modules.Where(a => a.ID == termDetail.ModuleID).Select(a => a.ModuleName).SingleOrDefault();
                            //        info.CourseName = courseName;
                            //        info.BatchCode = batchCode;

                            //        int assignment = db.Assignments.Where(a => a.TermDetailID == termDetail.ID && a.SubmissionType == 2 && (a.Status == 0 || a.Status == null)).Select(a => a.ID).SingleOrDefault();// not approve record

                            //        List<int> assSubList = db.AssignmentSubmissions.Where(a => a.AssignmentID == assignment).Select(a => a.ID).ToList();

                            //        int assessorCheck = db.AccessorComments.Where(a => assSubList.Contains(a.AssignmentSubmissionID)).Select(a => a.AssignmentSubmissionID).Distinct().Count();
                            //        int ivCheck = db.IVComments.Where(a => assSubList.Contains(a.AssignmentSubmissionID)).Select(a => a.AssignmentSubmissionID).Distinct().Count();

                            //        info.Submitted = assSubList.Count();
                            //        info.NoSubmission = stdList.Count() - assSubList.Count();
                            //        info.AssessorCheck = assessorCheck;
                            //        info.IVCheck = ivCheck;
                            //        info.SubmissionType = 2;
                            //        if(termDetail.FinalSubmission != null)
                            //        {
                            //            info.DueDate = termDetail.FinalSubmission;
                            //        }
                            //        info.BatchID = termDetail.Term.Batch.ID;
                            //        info.TermDetailID = termDetail.ID;
                            //        infoList.Add(info);
                            //    }

                            //    #endregion

                            //}
                        }


                    }
                    #endregion


                    #region Final
                    var lectureSubmissionList2 = db.AssignmentSubmissions
                                                .Where(x => 
                                                //x.Assignment.Status == 2
                                                x.Assignment.SubmissionType == 2
                                                && x.Assignment.TermDetail.LectureID == accessorID
                                                && (batchID == 0 ? true : x.Assignment.TermDetail.Term.Batch.ID == batchID)
                                                //&& x.Assignment.TermDetail.Term.Batch.IsDelete != true
                                                //&& x.Assignment.TermDetail.Module.IsDelete != true

                                                ).ToList();

                    var courseList2 = lectureSubmissionList2.Where(x => x.Assignment.TermDetail.LectureID == accessorID)
                                    .GroupBy(x => x.Assignment.TermDetail.Term.Batch.Course.ID)
                                    .Select(y => new { CourseID = y.Key, CourseName = y.FirstOrDefault().Assignment.TermDetail.Term.Batch.Course.CourseName });

                    foreach (var course in courseList2)
                    {
                        var batchList2 = lectureSubmissionList2.Where(x => x.Assignment.TermDetail.Term.Batch.Course.ID == course.CourseID)
                                        .GroupBy(x => x.Assignment.TermDetail.Term.Batch.ID)
                                        .Select(y => new { BatchID = y.Key, BatchName = y.FirstOrDefault().Assignment.TermDetail.Term.Batch.BatchName });

                        string courseName = db.Courses.Where(x => x.ID == course.CourseID).Select(x => x.CourseName).SingleOrDefault();

                        foreach (var batch in batchList2)
                        {
                            string batchCode = db.Batches.Where(a => a.ID == batch.BatchID).Select(a => a.BatchCode).SingleOrDefault();

                            List<TermDetail> termDetailList = db.TermDetails.Where(x => x.Term.BatchID == batch.BatchID && x.Assignment == true).OrderBy(x => x.sortOrder).ToList();
                            List<User> stdList = db.BatchDetails.Where(x => x.BatchID == batch.BatchID).Select(a => a.User2).Where(a => a.UserType == 1 && a.IsDelete != true).ToList();
                            //List<User> stdList = db.BatchDetails.Where(x => x.BatchID == batch.BatchID).Join(db.Users, b => b.StudentID, u => u.ID, (b, u) => new { u }).Select(a => a.u).Where(a => a.UserType == 1).ToList();


                            var moduleList2 = lectureSubmissionList2
                                            .Where(x => x.Assignment.TermDetail.Term.Batch.ID == batch.BatchID)
                                            .GroupBy(x => x.Assignment.TermDetail.Module.ID)
                                            .Select(y => new { ModuleID = y.Key, ModuleName = y.FirstOrDefault().Assignment.TermDetail.Module.ModuleName }).ToList();


                            foreach (var module in moduleList2)
                            {

                                var lectureSub = lectureSubmissionList2
                                                .Where(x => x.Assignment.TermDetail.Term.Batch.CourseID == course.CourseID
                                                 && x.Assignment.TermDetail.Term.Batch.ID == batch.BatchID
                                                 && x.Assignment.TermDetail.Module.ID == module.ModuleID).ToList();

                                SubmissionInfo info = new SubmissionInfo();
                                info.BatchID = batch.BatchID;
                                info.ModuleName = module.ModuleName;
                                info.CourseName = courseName;
                                info.BatchCode = batchCode;
                                info.SubmissionType = lectureSub.FirstOrDefault().Assignment.SubmissionType;
                                info.Submitted = lectureSub.Count();
                                info.NoSubmission = stdList.Count() - info.Submitted;
                                info.TermDetailID = lectureSub.FirstOrDefault().Assignment.TermDetailID;
                                info.DueDate = lectureSub.FirstOrDefault().Assignment.TermDetail.FinalSubmission;

                                List<AccessorComment> acList = new List<AccessorComment>();
                                List<IVComment> ivList = new List<IVComment>();
                                foreach (var submission in lectureSub)
                                {
                                    AccessorComment ac = new AccessorComment();
                                    IVComment iv = new IVComment();
                                    ac = db.AccessorComments.Where(x => x.AssignmentSubmissionID == submission.ID && x.AccessorID != null).FirstOrDefault();
                                    iv = db.IVComments.Where(x => x.AssignmentSubmissionID == submission.ID && x.IVID != null).FirstOrDefault();

                                    if (ac != null)
                                        acList.Add(ac);

                                    if (iv != null)
                                        ivList.Add(iv);
                                }

                                info.AssessorCheck = acList.Count();
                                info.IVCheck = ivList.Count();

                                infoList.Add(info);
                            }
                        }


                    }
                    #endregion


                    return infoList;
                }
            });
        }

        public Task<JsonResponse> ApproveSubmission(int batchID, int termDetailID, int submissionType)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    try
                    {
                        Assignment asg = db.Assignments.FirstOrDefault(x => x.TermDetailID == termDetailID);
                        TermDetail termDetail = db.TermDetails.Where(x => x.ID == termDetailID).SingleOrDefault();
                        Module module = db.Modules.Find(termDetail.ModuleID);
                        Batch batch = db.Batches.Where(x => x.ID == batchID).SingleOrDefault();

                        if (asg.Status != 1)
                        {
                            asg.IssueDate = DateTime.Now;
                        }

                        asg.Status = Convert.ToInt32(1);

                        TermDetail td = db.TermDetails.Where(x => x.ID == asg.TermDetailID).FirstOrDefault();

                        //var test = 1;

                        if (td.Tick1 == null && asg.SubmissionType == 1)
                        {
                            var assingmentID = td.Assignments.FirstOrDefault().ID;
                            var sublist = db.AssignmentSubmissions.Where(x => x.AssignmentID == assingmentID);
                            foreach (var item in sublist)
                            {
                                StudentNoti studentnoti = new StudentNoti();
                                if (item.AccessorComments.Count() != 0)
                                {
                                    studentnoti.Grading = item.AccessorComments.OrderBy(d => d.CreatedDate).LastOrDefault().GradingOverall;
                                    studentnoti.AssignmentSubmissionID = item.ID;
                                    studentnoti.StudentID = item.StudentID;
                                    studentnoti.Type = "first";
                                    studentnoti.IsSeen = false;
                                    studentnoti.CreatedDate = DateTime.Now;
                                    db.StudentNotis.Add(studentnoti);
                                }
                            }
                            db.SaveChanges();

                            td.Tick1 = true;
                            td.FirstResultIssue = DateTime.Now;
                            DateTime datenow = DateTime.Today;
                            DateTime finalsub = datenow.AddDays(14);
                            td.FinalSubmission = finalsub;
                        }
                        else if (td.Tick2 == null)
                        {
                            var assingmentID = td.Assignments.FirstOrDefault().ID;
                            var sublist = db.AssignmentSubmissions.Where(x => x.AssignmentID == assingmentID);
                            foreach (var item in sublist)
                            {
                                StudentNoti studentnoti = new StudentNoti();
                                if (item.AccessorComments.Count() != 0)
                                {
                                    studentnoti.Grading = item.AccessorComments.FirstOrDefault().GradingOverall;
                                    studentnoti.AssignmentSubmissionID = item.ID;
                                    studentnoti.StudentID = item.StudentID;
                                    studentnoti.Type = "final";
                                    studentnoti.IsSeen = false;
                                    studentnoti.CreatedDate = DateTime.Now;
                                    db.StudentNotis.Add(studentnoti);
                                }
                            }
                            db.SaveChanges();

                            td.Tick2 = true;
                            td.FinalResultIssue = DateTime.Now;
                        }

                        return new JsonResponse() { Flag = true, Message = "Successfully Saved" };
                    }
                    catch (Exception ex)
                    {
                        return new JsonResponse() { Flag = true, Message = ex.Message };
                    }
                }
            });
        }
    }
}