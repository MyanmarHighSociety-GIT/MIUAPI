using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using MIUWebAPI.Models;
using MIUWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace MIUWebAPI.DAL
{
    public class ResultDAL
    {
        private MIUEntities db = new MIUEntities();
        public Task<ResultInfo> GetResult(int userID, string batchCode)
        {
            return Task.Run(() =>
            {
                ResultInfo info = new ResultInfo();
                try
                {
                    List<GradingModule> gradingList = new List<GradingModule>();

                    User students = db.Users.Where(u => u.ID == userID && u.IsDelete != true).FirstOrDefault();

                    if (students != null)
                    {
                        var batchDetail = db.BatchDetails.Where(x => x.StudentID == students.ID).FirstOrDefault();

                        CommonDAL common = new CommonDAL();
                        IQueryable<TermDetail> termDetailInfo = common.GetTermDetailsByBatch(batchDetail.BatchID);

                        termDetailInfo = termDetailInfo.Where(x => x.Assignment == true);

                        //Batch batch = db.Batches.Where(a => a.BatchCode == batchCode).SingleOrDefault();
                        ////List<BatchDetail> batchDetail = db.BatchDetails.Where(a => a.BatchID == batch.ID).ToList();
                        //List<Term> term = db.Terms.Where(a => a.BatchID == batch.ID).ToList();
                        //List<int> ids = db.Terms.Where(x => x.BatchID == batch.ID).Select(x => x.ID).ToList<int>();
                        //var termDetail = db.TermDetails.Where(x => ids.Contains(x.TermID));

                        NextSubmission nextSub = GetCurrentBatchInformation(batchDetail, termDetailInfo, userID);
                        var CurrentResult = GetCurrentResult(userID);

                        info.CourseName = db.Courses.Where(a => a.ID == batchDetail.Batch.CourseID).Select(a => a.CourseName).SingleOrDefault();
                        info.AttendanceRate = nextSub.AttendanceRate;
                        info.NextSubmissionDate = nextSub.NextSubmissionDate;
                        info.TermName = nextSub.CurrentTermName;
                        info.CurrentModuleName = CurrentResult.CurrentModuleName;
                        info.CurrentModuleResult = CurrentResult.CurrentModuleResult;

                        foreach (var detail in termDetailInfo)
                        {
                            GradingModule mg = new GradingModule();
                            AssignmentData assignmentData = GetAssignmentData(detail, userID);

                            switch (assignmentData.GradingStatus)
                            {
                                case 1:
                                    mg.ModuleStatus = "Pass";
                                    break;
                                case 2:
                                    mg.ModuleStatus = "Fail";
                                    break;
                                case 3:
                                    mg.ModuleStatus = "Redo";
                                    break;
                                case 4:
                                    mg.ModuleStatus = "Merit";
                                    break;
                                case 5:
                                    mg.ModuleStatus = "Distinct";
                                    break;
                                case 6:
                                    mg.ModuleStatus = "Submitted";
                                    break;
                                case 7:
                                    mg.ModuleStatus = "Avaliable";
                                    break;
                                case 8:
                                    mg.ModuleStatus = "N/A";
                                    break;
                                case 9:
                                    mg.ModuleStatus = "Late";
                                    break;
                                default:
                                    break;
                            }
                            mg.ModuleCode = detail.Module.ModuleCode;
                            mg.TermDetailID = detail.ID;
                            mg.AssignmentSubmissionID = assignmentData.AssignmentSubmissionID;
                            if (mg.ModuleCode != "ENG")
                            {
                                gradingList.Add(mg);
                            }

                        }
                        info.GradingModuleList = gradingList;
                    }
                }
                catch (Exception)
                {
                }
                


                return info;

            });
        }

        public Task<ResultDetailInfo> GetResultDetail(int userID, string moduleCode, string moduleStatus, int termDetailID, int assignmentSubmissionID)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    ResultDetailInfo info = new ResultDetailInfo();
                    try
                    {
                        info.ModuleCode = moduleCode;
                        info.ModuleStatus = moduleStatus;
                        info.TermDetailID = termDetailID;
                        info.AssignmentSubmissionID = assignmentSubmissionID;
                        //var Submission = (from aa in db.AssignmentSubmissions
                        //                  join a in db.Assignments on aa.AssignmentID equals a.ID
                        //                  where aa.StudentID == userID && a.TermDetailID == termDetailID
                        //                  select a).FirstOrDefault();

                        var Sub = db.AssignmentSubmissions.Where(x => x.StudentID == userID && x.Assignment.TermDetailID == termDetailID).ToList();

                        if (Sub != null)
                        {
                            info.Submission = Sub.LastOrDefault().Assignment.SubmissionType == 1 ? "1st" : "Final";
                        }

                        var CurrentResult = GetCurrentResult(userID);
                        if (moduleStatus == "Pass" || moduleStatus == "Fail" || moduleStatus == "Redo" || moduleStatus == "Merit" || moduleStatus == "Distinct" || moduleStatus == "Submitted")
                        {
                            info.SubmissionDate = db.AssignmentSubmissionDetails.Where(x => x.AssignmentSubmissionID == assignmentSubmissionID).OrderByDescending(x => x.ID).Select(x => x.ModifiedDate).FirstOrDefault();
                        }
                        else
                        {
                            info.SubmissionDate = CurrentResult.FinalSubmissionDate;
                        }


                        AssignmentSubmission assSub = db.AssignmentSubmissions.Where(a => a.ID == assignmentSubmissionID).SingleOrDefault();
                        int? assessorID = 0;
                        List<ResultFile> fileList = new List<ResultFile>();

                        if (assignmentSubmissionID != 0)
                        {
                            var assessorList = db.AccessorComments.Where(a => a.AssignmentSubmissionID == assignmentSubmissionID && a.Stage == assSub.Stage).ToList();
                            //assessorID = db.AccessorComments.Where(a => a.AssignmentSubmissionID == assignmentSubmissionID && a.Stage == assSub.Stage).Select(a => a.AccessorID).FirstOrDefault();
                            int batchID = db.BatchDetails.Where(x => x.StudentID == userID).Select(x => x.Batch.ID).FirstOrDefault();
                            int moduleID = db.Modules.Where(x => x.ModuleCode == moduleCode).Select(x => x.ID).FirstOrDefault();

                            var assignments = db.Assignments.Where(x => x.TermDetail.Term.BatchID == batchID && x.TermDetail.ModuleID == moduleID && x.Status == 1).OrderBy(x => x.TermDetail.sortOrder).ToList();

                            //info.AssessorName = assignments.FirstOrDefault().TermDetail.User.FullName;
                            //int assignmentID = assignments.LastOrDefault().ID;
                            try
                            {
                                //var ab = from aa in db.AssignmentSubmissions
                                //         join a in db.Assignments on aa.AssignmentID equals a.ID
                                //         join td in db.TermDetails on a.TermDetailID equals td.ID
                                //         join t in db.Terms on td.TermID equals t.ID
                                //         join iv in db.IVComments on aa.ID equals iv.AssignmentSubmissionID
                                //         where aa.StudentID == userID && aa.AssignmentID == assignmentID && td.ModuleID == moduleID
                                //         select iv;
                                //var abc = db.AssignmentSubmissions.AsNoTracking();

                                //var a = abc.Where(x => x.StudentID == userID && x.AssignmentID == assignmentID && x.Assignment.TermDetail.ModuleID == moduleID);
                                info.AssessorName = assignments.FirstOrDefault().TermDetail.User.FullName;
                                int assignmentID = assignments.LastOrDefault().ID;

                                var a = db.IVComments.ToList().Where(x => x.AssignmentSubmissionID == assignmentSubmissionID);

                                info.IVName = a.FirstOrDefault().User.FullName;
                            }
                            catch (Exception ex)
                            {
                                string message = ex.Message.ToString();
                            }
                            

                            //var ivID = db.IVComments.Where(a => a.AssignmentSubmissionID == assignmentSubmissionID).Select(a => a.IVID).FirstOrDefault();
                            
                            //info.IVName = db.Users.Where(a => a.ID == ivID).Select(a => a.FullName).SingleOrDefault();
                            List<AssignmentSubmissionDetail> detailList = db.AssignmentSubmissionDetails.Where(a => a.AssignmentSubmissionID == assignmentSubmissionID).ToList();

                            foreach (var detail in detailList)
                            {
                                ResultFile file = new ResultFile()
                                {
                                    FileName = detail.FileName,
                                    IsMain = detail.IsMain,
                                    IsPDF = detail.IsNewPDF,
                                    FilePath = MIUFileServer.GetFileUrl("StudentFiles", detail.FileName),
                                    ModifiedDate = detail.ModifiedDate
                                };

                                fileList.Add(file);
                            }
                        }

                        //assessorID = int.TryParse( != null);

                        //if (assessorID == 0)
                        //{
                        //    assessorID = db.TermDetails.Where(a => a.ID == termDetailID).Select(a => a.AccessorID).SingleOrDefault();
                        //}

                        //info.AssessorName = db.Users.Where(a => a.ID == assessorID).Select(a => a.FullName).SingleOrDefault();


                        info.FileList = fileList.OrderByDescending(x => x.IsMain).ToList();
                    }
                    catch (Exception ex)
                    {
                        var message = ex.Message;
                    }
                    
                    //info.FileList = fileList;
                    return info;
                }

            });
        }

        public Task<JsonResponse> SaveResultIssue(int userID, int termDetailID, string reason, string description)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    TermDetail termDetail = db.TermDetails.Where(a => a.ID == termDetailID).SingleOrDefault();
                    Term term = db.Terms.Where(a => a.ID == termDetail.TermID).SingleOrDefault();
                    Batch batch = db.Batches.Where(a => a.ID == term.BatchID).SingleOrDefault();
                    ResultReportIssue issue = new ResultReportIssue()
                    {
                        UserId = userID,
                        BatchId = batch.ID,
                        TermId = term.ID,
                        TermDetailId = termDetail.ID,
                        ModuleId = termDetail.ModuleID,
                        Reason = reason,
                        Description = description,
                        CreatedDate = DateTime.Now,
                        CreatedBy = db.Users.Where(a => a.ID == userID).Select(a => a.LoginName).SingleOrDefault(),
                    };
                    db.ResultReportIssues.Add(issue);
                    db.SaveChanges();
                    return new JsonResponse() { Flag = true, Message = "Successfully Saved" };
                }

            });
        }

        //public Task<List<ResultForSuperAdminInfo>> GetResultForSuperAdmin(int courseID, int batchID, string year)
        //{
        //    return Task.Run(() =>
        //    {
        //        using (MIUEntities db = new MIUEntities())
        //        {
        //            List<ResultForSuperAdminInfo> infoList = new List<ResultForSuperAdminInfo>() {
        //                new ResultForSuperAdminInfo{ BatchID=batchID,
        //                                             ModuleName ="Organization Behavior",
        //                                             ModuleID=12,
        //                                             ResultDate =DateTime.Now,
        //                                             Distinct =2,
        //                                             Merit =10,
        //                                             Pass =30,
        //                                             Fail =3,
        //                                             NS =2},
        //                new ResultForSuperAdminInfo{ BatchID=batchID,
        //                                             ModuleName ="Financial Accounting & Reporting",
        //                                             ModuleID=12,
        //                                             ResultDate =DateTime.Now,
        //                                             Distinct =2,
        //                                             Merit =10,
        //                                             Pass =30,
        //                                             Fail =3,
        //                                             NS =2},
        //                new ResultForSuperAdminInfo{ BatchID=batchID,
        //                                             ModuleName ="Business Decision Making",
        //                                             ModuleID=12,
        //                                             ResultDate =DateTime.Now,
        //                                             Distinct =2,
        //                                             Merit =10,
        //                                             Pass =30,
        //                                             Fail =3,
        //                                             NS =2},
        //                new ResultForSuperAdminInfo{ BatchID=batchID,
        //                                             ModuleName ="Research Project",
        //                                             ModuleID=12,
        //                                             ResultDate =DateTime.Now,
        //                                             Distinct =2,
        //                                             Merit =10,
        //                                             Pass =30,
        //                                             Fail =3,
        //                                             NS =2}
        //            };


        //            return infoList;
        //        }

        //    });
        //}

        public Task<List<ResultForReturnInfo>> GetResultForSuperAdmin(int batchID)
        {
            return Task.Run(() =>
            {
                List<ResultForReturnInfo> infoList = new List<ResultForReturnInfo>();
                using (MIUEntities db = new MIUEntities())
                {

                    var grid = getResultGrid(batchID, 0);

                    for (int h = 0; h < grid.Headers.Count; h++)
                    {
                        ResultForReturnInfo resultForSuperAdminInfo = new ResultForReturnInfo();
                        if (h != grid.Headers.Count - 1)
                        {
                            if (grid.Headers[h].TermDetail.Module.ModuleName.Equals(grid.Headers[h + 1].TermDetail.Module.ModuleName))
                            {
                                h += 1;
                                resultForSuperAdminInfo.ModuleName = grid.Headers[h].TermDetail.Module.ModuleName;
                                resultForSuperAdminInfo.ModuleID = grid.Headers[h].TermDetail.Module.ID;
                            }
                            else
                            {
                                resultForSuperAdminInfo.ModuleName = grid.Headers[h].TermDetail.Module.ModuleName;
                                resultForSuperAdminInfo.ModuleID = grid.Headers[h].TermDetail.Module.ID;
                            }
                        }
                        else
                        {
                            resultForSuperAdminInfo.ModuleName = grid.Headers[h].TermDetail.Module.ModuleName;
                            resultForSuperAdminInfo.ModuleID = grid.Headers[h].TermDetail.Module.ID;
                        }

                        //res.StudentList = resStd;

                        if (grid.Rows != null && grid.Rows.Count() > 0)
                        {
                            for (int r = 0; r < grid.Rows.Count; r++)
                            {
                                int cols = grid.Headers.Count;
                                int no = r + 1;

                                for (int c = 0; c < grid.Rows[r].cells.Count; c++)
                                {
                                    try
                                    {
                                        //var assigStdList = grid.Rows[r].cells[c].Value.Assignment.AssignmentSubmissions.Select(x => x.StudentID).ToList();

                                        //List<User> stdList = db.BatchDetails.Where(x => x.BatchID == batchID && !assigStdList.Contains(x.StudentID)).Select(x => x.User2).Where(x => x.UserType == 1).ToList();
                                        ////List<ResultStudentInfo> resStd = new List<ResultStudentInfo>();
                                        //foreach (var std in stdList)
                                        //{
                                        //    resultForSuperAdminInfo.NS++;
                                        //}
                                        AccessorComment comment = grid.Rows[r].cells[c].Value.AccessorComments.OrderByDescending(x => x.Stage).OrderByDescending(d => d.CreatedDate).ToList().FirstOrDefault();

                                        if (comment != null && grid.Headers[h].TermDetail.Module.ID == comment.AssignmentSubmission.Assignment.TermDetail.ModuleID)
                                        {
                                            if (comment.GradingOverall == 1 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 1)
                                            {
                                                resultForSuperAdminInfo.Pass++;
                                            }
                                            else if (comment.GradingOverall == 2 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 1)
                                            {
                                                resultForSuperAdminInfo.Fail++;
                                            }

                                            else if (comment.GradingOverall == 3 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 1)
                                            {
                                                resultForSuperAdminInfo.Redo++;
                                            }

                                            else if (comment.GradingOverall == 4 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 1)
                                            {
                                                resultForSuperAdminInfo.Merit++;
                                            }

                                            else if (comment.GradingOverall == 5 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 1)
                                            {
                                                resultForSuperAdminInfo.Distinct++;
                                            }
                                            

                                            //For Final Submission
                                            //if (comment.GradingOverall == 1 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 2)
                                            //{
                                            //    ++secondP;
                                            //}

                                            //else if (comment.GradingOverall == 2 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 2)
                                            //{
                                            //    ++secondF;
                                            //}

                                            //else if (comment.GradingOverall == 3 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 2)
                                            //{
                                            //    ++secondR;
                                            //}

                                            //else if (comment.GradingOverall == 4 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 2)
                                            //{
                                            //    ++secondM;
                                            //}

                                            //else if (comment.GradingOverall == 5 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 2)
                                            //{
                                            //    ++secondD;
                                            //}
                                        }
                                        //else if(comment == null && grid.Headers[h].TermDetail.Module.ID == comment.AssignmentSubmission.Assignment.TermDetail.ModuleID)
                                        //{
                                        //    resultForSuperAdminInfo.NS++;
                                        //    //var assignmentSubmission = db.AssignmentSubmissions.Where(x => x.Assignment.TermDetail.Term.Batch.ID == batchID && x.Assignment.TermDetail.ModuleID == comment.AssignmentSubmission.Assignment.TermDetail.ModuleID && x.Assignment.SubmissionType == 1).Select(x => x.StudentID).ToList();

                                        //    //List<User> stdList = db.BatchDetails.Where(x => x.BatchID == batchID && !assignmentSubmission.Contains(x.StudentID)).Select(x => x.User2).Where(x => x.UserType == 1).ToList();
                                        //    //List<ResultStudentInfo> resStd = new List<ResultStudentInfo>();
                                        //    //foreach (var std in stdList)
                                        //    //{
                                        //    //    resultForSuperAdminInfo.NS++;
                                        //    //}
                                        //}
                                    }
                                    catch (Exception)
                                    {
                                        
                                    }

                                }
                            }
                            
                        }
                        //var total = resultForSuperAdminInfo.Pass + resultForSuperAdminInfo.Redo + resultForSuperAdminInfo.Merit + resultForSuperAdminInfo.Distinct + resultForSuperAdminInfo.Fail;
                        //resultForSuperAdminInfo.NS = grid.TotalStudent - total;
                        var assignmentSubmission = db.AssignmentSubmissions.Where(x => x.Assignment.TermDetail.Term.Batch.ID == batchID && x.Assignment.TermDetail.ModuleID ==  resultForSuperAdminInfo.ModuleID && x.Assignment.SubmissionType == 1).Select(x => x.StudentID).ToList();

                        List<User> stdList = db.BatchDetails.Where(x => x.BatchID == batchID && !assignmentSubmission.Contains(x.StudentID)).Select(x => x.User2).Where(x => x.UserType == 1).ToList();
                        List<ResultStudentInfo> resStd = new List<ResultStudentInfo>();
                        foreach (var std in stdList)
                        {
                            resultForSuperAdminInfo.NS++;
                        }
                        //var ResultDate = grid.Headers[0].IssueDate == null ? "" : grid.Headers[0].IssueDate.Value.Day + " " + grid.Headers[0].IssueDate.Value.ToString("MMM") + " " + grid.Headers[0].IssueDate.Value.Year;
                        var ResultDate = grid.Headers[h].IssueDate == null ? "" : grid.Headers[h].IssueDate.Value.Day + " " + grid.Headers[h].IssueDate.Value.ToString("MMM") + " " + grid.Headers[h].IssueDate.Value.Year;
                        resultForSuperAdminInfo.ResultDate = Convert.ToDateTime(ResultDate);

                        resultForSuperAdminInfo.BatchID = batchID;
                        resultForSuperAdminInfo.BatchName = db.Batches.Where(x=> x.ID == batchID).SingleOrDefault().BatchName;
                        infoList.Add(resultForSuperAdminInfo);
                    }
                };

                return infoList;

            });
        }

        public Task<ResultForSuperAdminDetailInfo> GetResultForSuperAdminDetail(int batchID, int moduleID, string grading)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    string courseName = db.Batches.Where(x => x.ID == batchID).Select(x => x.Course.CourseName).SingleOrDefault();
                    string batchCode = db.Batches.Where(a => a.ID == batchID).Select(a => a.BatchCode).SingleOrDefault();
                    string moduleName = db.Modules.Where(a => a.ID == moduleID).Select(a => a.ModuleName).SingleOrDefault();

                    ResultForSuperAdminDetailInfo res = new ResultForSuperAdminDetailInfo();
                    res.CourseName = courseName;
                    res.BatchCode = batchCode;
                    res.ModuleName = moduleName;
                    var grid = getResultGrid(batchID, moduleID);


                    if (grading == "NS")
                    {
                        var assignmentSubmission = db.AssignmentSubmissions.Where(x => x.Assignment.TermDetail.Term.Batch.ID == batchID && x.Assignment.TermDetail.ModuleID == moduleID && x.Assignment.SubmissionType == 1).Select(x => x.StudentID).ToList();

                        List<User> stdList = db.BatchDetails.Where(x => x.BatchID == batchID && !assignmentSubmission.Contains(x.StudentID)).Select(x => x.User2).Where(x => x.UserType == 1).ToList();
                        List<ResultStudentInfo> resStd = new List<ResultStudentInfo>();
                        foreach (var std in stdList)
                        {
                            ResultStudentInfo info = new ResultStudentInfo();

                            info.FullName = std.FullName;
                            info.LoginName = std.LoginName;
                            info.StudentPhoto = MIUFileServer.GetFileUrl("ProfileImages", std.ProfilePicture);

                            resStd.Add(info);
                        }
                        res.StudentList = resStd;
                    }
                    else
                    {
                        for (int h = 0; h < grid.Headers.Count; h++)
                        {
                            ResultForReturnInfo resultForSuperAdminInfo = new ResultForReturnInfo();
                            if (grid.Rows != null && grid.Rows.Count() > 0)
                            {
                                List<ResultStudentInfo> resStd = new List<ResultStudentInfo>();
                                for (int r = 0; r < grid.Rows.Count; r++)
                                {
                                    int cols = grid.Headers.Count;
                                    int no = r + 1;

                                    for (int c = 0; c < grid.Rows[r].cells.Count; c++)
                                    {
                                        //ResultStudentInfo info = new ResultStudentInfo();
                                        try
                                        {
                                            IVComment iVComment = grid.Rows[r].cells[c].Value.IVComments.OrderByDescending(d => d.CreatedDate).ToList().FirstOrDefault();
                                            AccessorComment comment = grid.Rows[r].cells[c].Value.AccessorComments.OrderByDescending(x => x.Stage).OrderByDescending(d => d.CreatedDate).ToList().FirstOrDefault();

                                            if (grid.Headers[0].TermDetail.User.FullName != null)
                                            {
                                                //res.AssessorName = comment.User.FullName;
                                                res.AssessorName = grid.Headers[0].TermDetail.User.FullName;
                                                res.AssessorPhoto = MIUFileServer.GetFileUrl("ProfileImages", grid.Headers[0].TermDetail.User.ProfilePicture);
                                            }

                                            if (iVComment != null)
                                            {
                                                res.IVName = iVComment.User.FullName;
                                                res.IVPhoto = MIUFileServer.GetFileUrl("ProfileImages", iVComment.User.ProfilePicture);
                                            }



                                            if (comment != null && grid.Headers[h].TermDetail.Module.ID == comment.AssignmentSubmission.Assignment.TermDetail.ModuleID)
                                            {
                                                if (grading == "Pass" && comment.GradingOverall == 1 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 1)
                                                {
                                                    ResultStudentInfo info = new ResultStudentInfo();
                                                    info.FullName = comment.AssignmentSubmission.User.FullName;
                                                    info.LoginName = comment.AssignmentSubmission.User.LoginName;
                                                    info.StudentPhoto = MIUFileServer.GetFileUrl("ProfileImages", comment.AssignmentSubmission.User.ProfilePicture);

                                                    resStd.Add(info);
                                                    //resultForSuperAdminInfo.Pass++;
                                                }
                                                else if (grading == "Fail" && comment.GradingOverall == 2 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 1)
                                                {
                                                    ResultStudentInfo info = new ResultStudentInfo();
                                                    info.FullName = comment.AssignmentSubmission.User.FullName;
                                                    info.LoginName = comment.AssignmentSubmission.User.LoginName;
                                                    info.StudentPhoto = MIUFileServer.GetFileUrl("ProfileImages", comment.AssignmentSubmission.User.ProfilePicture);

                                                    resStd.Add(info);
                                                    resultForSuperAdminInfo.Fail++;
                                                }

                                                else if (grading == "Redo" && comment.GradingOverall == 3 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 1)
                                                {
                                                    ResultStudentInfo info = new ResultStudentInfo();
                                                    info.FullName = comment.AssignmentSubmission.User.FullName;
                                                    info.LoginName = comment.AssignmentSubmission.User.LoginName;
                                                    info.StudentPhoto = MIUFileServer.GetFileUrl("ProfileImages", comment.AssignmentSubmission.User.ProfilePicture);

                                                    resStd.Add(info);
                                                    resultForSuperAdminInfo.Redo++;
                                                }

                                                else if (grading == "Merit" && comment.GradingOverall == 4 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 1)
                                                {
                                                    ResultStudentInfo info = new ResultStudentInfo();
                                                    info.FullName = comment.AssignmentSubmission.User.FullName;
                                                    info.LoginName = comment.AssignmentSubmission.User.LoginName;
                                                    info.StudentPhoto = MIUFileServer.GetFileUrl("ProfileImages", comment.AssignmentSubmission.User.ProfilePicture);

                                                    resStd.Add(info);
                                                    resultForSuperAdminInfo.Merit++;
                                                }

                                                else if (grading == "Distinct" && comment.GradingOverall == 5 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 1)
                                                {
                                                    ResultStudentInfo info = new ResultStudentInfo();
                                                    info.FullName = comment.AssignmentSubmission.User.FullName;
                                                    info.LoginName = comment.AssignmentSubmission.User.LoginName;
                                                    info.StudentPhoto = MIUFileServer.GetFileUrl("ProfileImages", comment.AssignmentSubmission.User.ProfilePicture);

                                                    resStd.Add(info);
                                                    resultForSuperAdminInfo.Distinct++;
                                                }


                                                //For Final Submission
                                                //if (comment.GradingOverall == 1 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 2)
                                                //{
                                                //    ++secondP;
                                                //}

                                                //else if (comment.GradingOverall == 2 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 2)
                                                //{
                                                //    ++secondF;
                                                //}

                                                //else if (comment.GradingOverall == 3 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 2)
                                                //{
                                                //    ++secondR;
                                                //}

                                                //else if (comment.GradingOverall == 4 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 2)
                                                //{
                                                //    ++secondM;
                                                //}

                                                //else if (comment.GradingOverall == 5 && grid.Rows[r].cells[c].Value.Assignment.SubmissionType == 2)
                                                //{
                                                //    ++secondD;
                                                //}
                                            }
                                        }
                                        catch (Exception)
                                        {

                                        }
                                    }
                                }

                                res.StudentList = resStd;
                            }
                        }
                    }


                    var ResultDate = grid.Headers[0].IssueDate == null ? "" : grid.Headers[0].IssueDate.Value.Day + " " + grid.Headers[0].IssueDate.Value.ToString("MMM") + " " + grid.Headers[0].IssueDate.Value.Year;
                    res.ResultDate = Convert.ToDateTime(ResultDate);
                    res.ResultCount = res.StudentList.Count;
                    return res;
                }

            });
        }



        public Task<List<ResultForReturnInfo>> GetResultForAccessor(int accessorID, string month, string year)
        {
            var Month = Convert.ToDateTime(month + "01," + DateTime.Now.Year).Month;
            int Year = int.Parse(year);
            return Task.Run(() =>
            {
                List<ResultForReturnInfo> infoList = new List<ResultForReturnInfo>();
                using (MIUEntities db = new MIUEntities())
                {

                    //var grid = getResultGrid(0, 0, accessorID);

                    //var headers = grid.Headers.GroupBy(x => x.TermDetail.Module.ID).ToList();

                    //List<Module> module = db.Modules.Where(x => x.IsDelete != true).ToList();

                    var batches = (from td in db.TermDetails
                                   join t in db.Terms on td.TermID equals t.ID
                                   join b in db.Batches on t.BatchID equals b.ID
                                   join bd in db.BatchDetails on b.ID equals bd.BatchID
                                   join m in db.Modules on td.ModuleID equals m.ID
                                   where td.LectureID == accessorID
                                   //&& b.ID == batchID
                                   group new { b, m } by
                                   new { b.ID, b.BatchName, td.ModuleID, m.ModuleName }).ToList();

                    foreach (var batch in batches)
                    {

                        //foreach (var header in module)
                        //{
                        ResultForReturnInfo resultForSuperAdminInfo = new ResultForReturnInfo();

                        var users = (from bd in db.BatchDetails
                                     join u in db.Users on bd.StudentID equals u.ID
                                     join b in db.Batches on bd.BatchID equals b.ID
                                     join t in db.Terms on b.ID equals t.BatchID
                                     join td in db.TermDetails on t.ID equals td.TermID
                                     where td.ModuleID == batch.Key.ModuleID && b.ID == batch.Key.ID && u.IsDelete != true
                                     select u).ToList();
                        try
                        {
                            List<AccessorComment> a = db.AccessorComments.Where(x => x.AssignmentSubmission.Assignment.TermDetail.LectureID == accessorID && x.AssignmentSubmission.Assignment.TermDetail.Term.Batch.ID == batch.Key.ID && x.AssignmentSubmission.Assignment.IssueDate != null).ToList();

                            List<AccessorComment> comments = a.Where(x => x.AssignmentSubmission.Assignment.IssueDate.Value.Year == Year && x.AssignmentSubmission.Assignment.IssueDate.Value.Month == Month).ToList();

                            foreach (var comment in comments)
                            {
                                if (comment != null && batch.Key.ModuleID == comment.AssignmentSubmission.Assignment.TermDetail.ModuleID)
                                {

                                    var resultDate = comment.AssignmentSubmission.Assignment.IssueDate == null ? "" : comment.AssignmentSubmission.Assignment.IssueDate.Value.Day + "" + comment.AssignmentSubmission.Assignment.IssueDate.Value.ToString("MMM") + " " + comment.AssignmentSubmission.Assignment.IssueDate.Value.Year;

                                    resultForSuperAdminInfo.BatchID = batch.Key.ID;
                                    resultForSuperAdminInfo.BatchName = batch.Key.BatchName;
                                    resultForSuperAdminInfo.ResultDate = comment.AssignmentSubmission.Assignment.IssueDate;
                                    resultForSuperAdminInfo.ModuleName = batch.Key.ModuleName;
                                    resultForSuperAdminInfo.ModuleID = batch.Key.ModuleID;
                                    if (comment.GradingOverall == 1 && comment.AssignmentSubmission.Assignment.SubmissionType == 1)
                                    {
                                        resultForSuperAdminInfo.Pass++;
                                    }
                                    else if (comment.GradingOverall == 2 && comment.AssignmentSubmission.Assignment.SubmissionType == 1)
                                    {
                                        resultForSuperAdminInfo.Fail++;
                                    }

                                    else if (comment.GradingOverall == 3 && comment.AssignmentSubmission.Assignment.SubmissionType == 1)
                                    {
                                        resultForSuperAdminInfo.Redo++;
                                    }

                                    else if (comment.GradingOverall == 4 && comment.AssignmentSubmission.Assignment.SubmissionType == 1)
                                    {
                                        resultForSuperAdminInfo.Merit++;
                                    }

                                    else if (comment.GradingOverall == 5 && comment.AssignmentSubmission.Assignment.SubmissionType == 1)
                                    {
                                        resultForSuperAdminInfo.Distinct++;
                                    }

                                    var total = resultForSuperAdminInfo.Pass + resultForSuperAdminInfo.Redo + resultForSuperAdminInfo.Merit + resultForSuperAdminInfo.Distinct + resultForSuperAdminInfo.Fail;
                                    resultForSuperAdminInfo.NS = users.Count - total;

                                    if(resultForSuperAdminInfo.NS < 0)
                                    {
                                        resultForSuperAdminInfo.NS = 0;
                                    }

                                }

                            }
                            if (!string.IsNullOrEmpty(resultForSuperAdminInfo.ModuleName))
                            {
                                infoList.Add(resultForSuperAdminInfo);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        //}
                    }
                    //infoList = infoList.Where(x => x.BatchName.Contains("2018")).ToList();
                };

                return infoList;

            });
        }

        //public Task<ResultForSuperAdminDetailInfo> GetResultForSuperAdminDetail(int courseID, int batchID, string year, int moduleID, string grading)
        //{
        //    return Task.Run(() =>
        //    {
        //        using (MIUEntities db = new MIUEntities())
        //        {
        //            string courseName = db.Courses.Where(a => a.ID == courseID).Select(a => a.CourseName).SingleOrDefault();
        //            string batchCode = db.Batches.Where(a => a.ID == batchID).Select(a => a.BatchCode).SingleOrDefault();
        //            string moduleName = db.Modules.Where(a => a.ID == moduleID).Select(a => a.ModuleName).SingleOrDefault();
        //            ResultForSuperAdminDetailInfo info = new ResultForSuperAdminDetailInfo()
        //            {
        //                CourseName = courseName,
        //                BatchCode = batchCode,
        //                ModuleName = moduleName,
        //                ResultDate = DateTime.Now,
        //                AssessorName = "",
        //                AssessorPhoto = MIUFileServer.GetFileUrl("ProfileImages", "00000000-0000-0000-0000-000000000000_27540572_191047288295422_2591374543161757214_n.jpg"),
        //                IVName = "",
        //                IVPhoto = MIUFileServer.GetFileUrl("ProfileImages", "00000000-0000-0000-0000-000000000000_27540572_191047288295422_2591374543161757214_n.jpg"),

        //            };

        //            ResultStudentInfo info1 = new ResultStudentInfo
        //            {
        //                FullName = "John Wick",
        //                LoginName = "UB-001",
        //                StudentPhoto = MIUFileServer.GetFileUrl("ProfileImages", "00000000-0000-0000-0000-000000000000_27540572_191047288295422_2591374543161757214_n.jpg"),
        //            };

        //            ResultStudentInfo info2 = new ResultStudentInfo
        //            {
        //                FullName = "John Doe",
        //                LoginName = "UB-001",
        //                StudentPhoto = MIUFileServer.GetFileUrl("ProfileImages", "00000000-0000-0000-0000-000000000000_27540572_191047288295422_2591374543161757214_n.jpg"),
        //            };

        //            ResultStudentInfo info3 = new ResultStudentInfo
        //            {
        //                FullName = "Morgan",
        //                LoginName = "UB-001",
        //                StudentPhoto = MIUFileServer.GetFileUrl("ProfileImages", "00000000-0000-0000-0000-000000000000_27540572_191047288295422_2591374543161757214_n.jpg"),
        //            };

        //            List<ResultStudentInfo> infoList = new List<ResultStudentInfo>();
        //            infoList.Add(info1);
        //            infoList.Add(info2);
        //            infoList.Add(info3);

        //            info.StudentList = infoList;

        //            return info;
        //        }

        //    });
        //}

        public AssignmentData GetAssignmentData(TermDetail termDetail, int studentID)
        {
            AssignmentStatus assignmentStatus = GetSubmissionTypeAndAssignmentStatus(termDetail, 0);
            AssignmentData assignmentData = new AssignmentData();
            int gradingStatus = 0, assignmentID = 0;

            if (assignmentStatus.Status == 2)//InActive
            {
                gradingStatus = 8;//NotReady
            }
            else if (assignmentStatus.Status == 1//Active
                       || assignmentStatus.Status == 3//Expired
                       )
            {
                //Grading grading = GetGradingByTermDetailAndStudentID(termDetail.ID, studentID); //original
                Grading grading = GetGradingByTermDetailAndStudentID(termDetail.ID, studentID, assignmentStatus.Status);


                if (grading.FirstResult == 3 && (grading.FinalResult == 0 || grading.FinalResult == 2) && termDetail.Tick1 != null && termDetail.Tick2 == null)
                {
                    var assignment = db.Assignments.Where(x => x.TermDetailID == termDetail.ID && x.SubmissionType == 2);
                    int assignmentid = 0;
                    if (assignment != null || assignment.Count() != 0)
                    {
                        assignmentid = assignment.FirstOrDefault().ID;
                    }
                    var redoassignment = db.AssignmentSubmissions.Where(x => x.AssignmentID == assignmentid && x.StudentID == studentID);
                    if (grading.FinalResult == 0 && redoassignment.Count() != 0)
                    {
                        grading.FinalResult = 6;
                    }
                }

                if (grading.FirstResult != 0 || grading.FinalResult != 0)
                {
                    int result = 0;
                    int submissionType = 0;

                    if (grading.FinalResult != 0)
                    {
                        result = grading.FinalResult;
                        submissionType = 2;// Final;
                    }
                    else
                    {
                        result = grading.FirstResult;
                        submissionType = 1;//.First;
                    }

                    if (IsResultGenerated(termDetail.ID, submissionType))
                    {
                        gradingStatus = result;
                        var assignmentSubmissions = GetAssignmentSubmissions(termDetail.ID, studentID, submissionType);
                        if (assignmentSubmissions != null && assignmentSubmissions.Count() > 0)
                        {
                            assignmentID = assignmentSubmissions.FirstOrDefault().ID;
                        }
                    }
                    else
                    {
                        if (submissionType == 2)
                        {
                            var assignmentSubmissions = GetAssignmentSubmissions(termDetail.ID, studentID, 2);
                            if (assignmentSubmissions != null && assignmentSubmissions.Count() > 0)
                            {
                                gradingStatus = 6;// (int)EntityEnum.SubmissionStatus.Submitted;
                                assignmentID = assignmentSubmissions.FirstOrDefault().ID;

                            }
                            else if (assignmentStatus.Status == 3)// (int)EntityEnum.AssignmentStatus.Expired)
                                gradingStatus = 9;// (int)EntityEnum.SubmissionStatus.Late;
                            else
                                gradingStatus = 7;// (int)EntityEnum.SubmissionStatus.PendingSubmission;
                        }
                        else
                        {
                            var assignmentSubmissions = GetAssignmentSubmissions(termDetail.ID, studentID, submissionType);
                            if (assignmentSubmissions != null && assignmentSubmissions.Count() > 0)
                            {
                                assignmentID = assignmentSubmissions.FirstOrDefault().ID;
                            }
                            gradingStatus = 6;// (int)EntityEnum.SubmissionStatus.Submitted;
                        }
                    }
                }
                else
                {
                    var assignmentSubmissions = GetAssignmentSubmissions(termDetail.ID, studentID, 1);
                    if (assignmentSubmissions != null && assignmentSubmissions.Count() > 0)
                    {
                        gradingStatus = 6;// (int)EntityEnum.SubmissionStatus.Submitted;
                        assignmentID = assignmentSubmissions.FirstOrDefault().ID;
                    }
                    else if (assignmentStatus.Status == 3)
                        gradingStatus = 9;// (int)EntityEnum.SubmissionStatus.Late;
                    else
                        gradingStatus = 7;// (int)EntityEnum.SubmissionStatus.PendingSubmission;
                }
            }
            assignmentData.AssignmentSubmissionID = assignmentID;
            assignmentData.GradingStatus = gradingStatus;

            return assignmentData;

        }

        public IQueryable<TermDetail> GetTermDetailsByBatch(int batchID)
        {
            IQueryable<TermDetail> termDetailInfo = null;
            List<int> ids = db.Terms.Where(x => x.BatchID == batchID).Select(x => x.ID).ToList<int>();
            termDetailInfo = db.TermDetails.Where(x => ids.Contains(x.TermID));
            return termDetailInfo;
        }

        public AssignmentStatus GetSubmissionTypeAndAssignmentStatus(TermDetail termDetail, int submissionType)
        {

            //Time ranges for assignment submission.
            TimeSpan EndSubmission = new TimeSpan(17, 0, 0);
            TimeSpan Now = DateTime.Now.TimeOfDay;


            AssignmentStatus assignmentStatus = new AssignmentStatus();
            Utility utility = new Utility();

            //Added by Dolay on 16th Oct 2018
            Term term = db.Terms.Where(x => x.ID == termDetail.TermID).FirstOrDefault();
            if (term.Batch.CourseID == 9)
            {
                List<TimeTableDetail> ttd = db.TimeTableDetails.Where(x => x.TermID == termDetail.TermID).ToList();
                List<string> res = ttd.Select(y => y.StartTime).Distinct().ToList();
                if (res.Count == 2)
                {
                    foreach (var item in res)
                    {
                        if (item.Contains("9:30 AM"))
                        {
                            EndSubmission = new TimeSpan(13, 0, 0);
                        }
                    }
                }
            }
            if (termDetail.ModuleID != 18) //No need to submit assignment for english module
            {
                //if(termDetail.Assignment.)
                if (termDetail.FirstSubmission.Value.ToString("dd MMM yyyy") == DateTime.Now.ToString("dd MMM yyyy") && Now > EndSubmission)
                {
                    assignmentStatus.Status = 9;//Expired;
                }
                else if (termDetail.FinalSubmission.Value.ToString("dd MMM yyyy") == DateTime.Now.ToString("dd MMM yyyy") && Now > EndSubmission)
                {
                    assignmentStatus.Status = 3;//Expired;
                }
                else if ((utility.GreaterThan(DateTime.Now, termDetail.FirstSubmission.Value)
                  ) && submissionType == 1)//First)
                {
                    assignmentStatus.SubmissionType = 1;//first
                    assignmentStatus.Status = 3;//.Expired;
                }
                else if ((utility.LessThanOrEqual(DateTime.Now, termDetail.FirstSubmission.Value)
                        && utility.GreaterThan(DateTime.Now, termDetail.Term.StartDate.Value))
                    && submissionType != 2)//final
                {
                    assignmentStatus.SubmissionType = 1;//First;
                    assignmentStatus.Status = 1;//Active;
                }
                else if (utility.LessThanOrEqual(DateTime.Now, termDetail.FinalSubmission.Value)
                            && utility.GreaterThan(DateTime.Now, termDetail.FirstResult.Value))
                {
                    assignmentStatus.SubmissionType = 2;//final ;
                    assignmentStatus.Status = 1;// Active;
                }
                else if (utility.LessThan(DateTime.Now, termDetail.Term.StartDate.Value)
                            || (submissionType == 2 && utility.LessThan(DateTime.Now, termDetail.FirstResult.Value)))
                {
                    assignmentStatus.Status = 2;//.Inactive;
                }
                else if (utility.EqualDate(DateTime.Now, termDetail.FirstResult.Value))
                {
                    assignmentStatus.Status = 1;// Active;
                    assignmentStatus.SubmissionType = 1;// First;
                }
                else if (utility.EqualDate(DateTime.Now, termDetail.FinalResult.Value))
                {
                    assignmentStatus.Status = 1;//.Active;
                    assignmentStatus.SubmissionType = 2;//.Final;
                }
                else
                {
                    assignmentStatus.Status = 3;// Expired;
                }
            }

            return assignmentStatus;

        }

        public Grading GetGradingByTermDetailAndStudentID(int termDetailID, int studentID, int status)
        {
            Grading grading = new Grading();
            grading.FirstResult = GetResult(termDetailID, studentID, 1);
            if (status == 3 && grading.FirstResult == 3)
            {
                grading.FinalResult = GetResultExpired(termDetailID, studentID, 2);
            }
            else
            {
                grading.FinalResult = GetResult(termDetailID, studentID, 2);
            }
            return grading;
        }

        public int GetResult(int termDetailID, int studentID, int submissionType)
        {
            int result = 0;
            var assignmentSubmissions = GetAssignmentSubmissions(termDetailID, studentID, submissionType);


            if (assignmentSubmissions != null && assignmentSubmissions.Count() > 0)
            {
                var assignmentSubmission = assignmentSubmissions.ToList().LastOrDefault();
                var gradings = assignmentSubmission.AccessorComments.OrderBy(x => x.Stage).OrderBy(d => d.CreatedDate);

                if (gradings != null && gradings.Count() > 0)
                {
                    var grading = gradings.ToList().LastOrDefault();
                    result = grading.GradingOverall.Value;
                }
            }
            return result;
        }

        public int GetResultExpired(int termDetailID, int studentID, int submissionType)
        {
            int result = 0;
            var assignmentSubmissions = GetAssignmentSubmissions(termDetailID, studentID, submissionType);

            if (assignmentSubmissions != null && assignmentSubmissions.Count() > 0)
            {
                var assignmentSubmission = assignmentSubmissions.ToList().LastOrDefault();
                var gradings = assignmentSubmission.AccessorComments.OrderBy(x => x.Stage).OrderBy(d => d.CreatedDate);

                if (gradings != null && gradings.Count() > 0)
                {
                    var grading = gradings.ToList().LastOrDefault();
                    result = grading.GradingOverall.Value;
                }
            }
            else if (IsResultGenerated(termDetailID, submissionType))
            {
                result = 2;
            }
            return result;
        }
        public IQueryable<AssignmentSubmission> GetAssignmentSubmissions(int termDetailID, int studentID, int submissionType)
        {

            var assignmentSubmissions = db.AssignmentSubmissions.Where(x => x.StudentID == studentID && x.Assignment.TermDetailID == termDetailID && x.Assignment.SubmissionType == submissionType).OrderBy(x => x.Stage);
            return assignmentSubmissions;


        }
        public bool IsResultGenerated(int termDetailID, int submissionType)
        {

            var assignment = db.Assignments.Where(x => x.TermDetailID == termDetailID && x.Status == 1 && x.SubmissionType == submissionType);
            if (assignment != null && assignment.Count() > 0)
                return true;

            return false;


        }

        public NextSubmission GetCurrentBatchInformation(BatchDetail batchDetail, IQueryable<TermDetail> termDetailInfo, int userID)
        {
            NextSubmission nextSub = new NextSubmission();
            var firstSubmissionDates = termDetailInfo.Where(x => x.Term.StartDate < DateTime.Now && x.FirstSubmission >= DateTime.Now);
            var finalSubmissionDates = termDetailInfo.Where(x => x.FirstResult < DateTime.Now && x.FinalSubmission >= DateTime.Now);

            if (firstSubmissionDates != null && firstSubmissionDates.Count() > 0)
            {
                var submission = firstSubmissionDates.FirstOrDefault();
                nextSub.CurrentTermName = submission.Term.TermName;
                //nextSub.CurrentModuleResult = submission;
                var moduleName = db.Modules.Where(x => x.ID == submission.ModuleID).SingleOrDefault().ModuleName;
                nextSub.NextSubmissionModule = moduleName;

                nextSub.CurrentModuleName = moduleName;
                nextSub.TermDetail = submission.Term.TermDetails.Where(x => x.ModuleID == submission.Module.ID).FirstOrDefault();

                nextSub.AttendanceRate = GetAttendanceRateForDashboard(submission.TermID, userID);
                nextSub.NextSubmissionDate = submission.FirstSubmission.Value.ToString("dd MMM yy");

                nextSub.Submisstion = "1st";
            }
            else if (finalSubmissionDates != null && finalSubmissionDates.Count() > 0)
            {
                var submission = finalSubmissionDates.FirstOrDefault();
                var moduleName = db.Modules.Where(x => x.ID == submission.ModuleID).SingleOrDefault().ModuleName;
                nextSub.NextSubmissionModule = moduleName;

                nextSub.AttendanceRate = GetAttendanceRateForDashboard(submission.TermID, userID);
                nextSub.NextSubmissionDate = submission.FinalSubmission.Value.ToString("dd MMM yy");
                nextSub.Submisstion = "Final";
            }
            else
            {
                nextSub.NextSubmissionModule = "This assignment is not loger active!";
            }
            return nextSub;
        }
        public int GetAttendanceRateForDashboard(int termID, int studentID)
        {
            int attendanceRate = 0;
            ////var totalTerms = db.Attendances.Where(x => x.TermDetailID == termDetailID);
            //var termdetail = db.TermDetails.Find(termDetailID);
            //var termID = termdetail.TermID;
            var totalTerms = db.Attendances.Where(x => x.TermID == termID);


            if (studentID != 0)
                //totalTerms = db.Attendances.Where(x => x.TermDetailID == termDetailID && x.StudentID == studentID);
                totalTerms = db.Attendances.Where(x => x.TermID == termID && x.StudentID == studentID);

            if (totalTerms != null)
            {
                var totalAttendance = totalTerms.Where(x => x.IsAttending == true);
                if (totalAttendance != null && totalAttendance.Count() != 0 && totalTerms.Count() != 0)
                {
                    int val1 = totalAttendance.Count();
                    double val2 = totalTerms.Count();

                    attendanceRate = (int)Math.Round((val1 / val2) * 100);
                }
            }
            return attendanceRate;
        }

        #region LastResult
        public ResultInfo GetCurrentResult(int userID)
        {
            ResultInfo info = new ResultInfo();
            var CurrentResult = (from aa in db.AssignmentSubmissions
                                 join a in db.Assignments on aa.AssignmentID equals a.ID
                                 join td in db.TermDetails on a.TermDetailID equals td.ID
                                 join t in db.Terms on td.TermID equals t.ID
                                 join ac in db.AccessorComments on aa.ID equals ac.AssignmentSubmissionID
                                 join m in db.Modules on td.ModuleID equals m.ID
                                 where aa.StudentID == userID
                                 select new { m.ModuleCode, ac.GradingOverall, t.StartDate, t.CompletionDate, aa.ID, td.FirstSubmission, td.FinalSubmission })
                                 .OrderByDescending(aa => aa.ID)
                                 .FirstOrDefault();
            //Grading grading = GetGradingByTermDetailAndStudentID(nextSub.TermDetail.ID, userID, 1);
            if(CurrentResult != null)
            {
                info.CurrentModuleName = CurrentResult.ModuleCode;
                info.FinalSubmissionDate = CurrentResult.FinalSubmission;

                int? gradingstatus = CurrentResult.GradingOverall;
                switch (gradingstatus)
                {
                    case 1:
                        info.CurrentModuleResult = "Pass";
                        break;
                    case 2:
                        info.CurrentModuleResult = "Fail";
                        break;
                    case 3:
                        info.CurrentModuleResult = "Redo";
                        break;
                    case 4:
                        info.CurrentModuleResult = "Merit";
                        break;
                    case 5:
                        info.CurrentModuleResult = "Distinct";
                        break;
                    case 6:
                        info.CurrentModuleResult = "Submitted";
                        break;
                    case 7:
                        info.CurrentModuleResult = "Avaliable";
                        break;
                    case 8:
                        info.CurrentModuleResult = "N/A";
                        break;
                    case 9:
                        info.CurrentModuleResult = "Late";
                        break;
                    default:
                        break;
                }
            }
            
            return info;
        }



        private ResultGrid getResultGrid(int batchID, int? moduleID)
        {
            var assignmentSubmissionDB = db.AssignmentSubmissions.AsNoTracking();
            var assignmentDB = db.Assignments.AsNoTracking();

            ResultGrid grid = new ResultGrid();
            if (batchID != 0 && (moduleID != 0 && moduleID != null))
            {
                //grid.Headers = db.Assignments.Where(x => x.TermDetail.Term.BatchID == batchID && x.TermDetail.ModuleID == moduleID && x.Status == 1).OrderBy(x => x.TermDetail.sortOrder).ToList();
                var assignmentFirst = db.Assignments.Where(x => x.TermDetail.Term.BatchID == batchID && x.TermDetail.ModuleID == moduleID && x.Status == 1 && x.SubmissionType == 1).OrderBy(x => x.TermDetail.sortOrder).ToList();
                var assignmentFinal = db.Assignments.Where(x => x.TermDetail.Term.BatchID == batchID && x.TermDetail.ModuleID == moduleID && x.Status == 1 && x.SubmissionType == 2).OrderBy(x => x.TermDetail.sortOrder).ToList();

                var mixAssignments = assignmentFinal;

                var studentIDFinal = assignmentFinal.Select(x => x.AssignmentSubmissions.Select(xx => xx.StudentID)).ToList();

                //will add to mix if not contain in finalList
                foreach (var ass in assignmentFirst)
                {
                    var studentIDFirst = ass.AssignmentSubmissions.Select(x => x.StudentID).ToList();
                    if (!studentIDFinal.Any(x=> x == studentIDFinal))
                    {
                        mixAssignments.Add(ass);
                    }
                }

                grid.Headers = mixAssignments;

                List<User> stdList;

                stdList = db.BatchDetails.Where(x => x.BatchID == batchID).Select(x => x.User2).Where(x => x.UserType == 1).ToList();

                string firstIV = ""; string secondIV = "";
                foreach (User student in stdList)
                {
                    ResultGridRow row = new ResultGridRow();
                    row.StudentName = student.FullName;
                    row.LoginName = student.LoginName;
                    foreach (Assignment assign in grid.Headers)
                    {
                        ResultCell cell = new ResultCell();
                        cell.Value = assignmentSubmissionDB.Where(x => x.StudentID == student.ID && x.AssignmentID == assign.ID && x.Assignment.TermDetail.ModuleID == moduleID).OrderBy(x => x.Stage).ToList().LastOrDefault();
                        row.cells.Add(cell);
                        if (cell.Value != null && cell.Value.IVComments.Count() > 0)
                        {
                            string IVName = cell.Value.IVComments.FirstOrDefault().User.FullName;
                            if (assign.SubmissionType == 1)
                            {
                                if (!(firstIV.Contains(IVName)))
                                {
                                    if (firstIV == "")
                                        firstIV = IVName;
                                    else
                                        firstIV = firstIV + "," + IVName;
                                }
                            }
                            else
                            {
                                if (!(secondIV.Contains(IVName)))
                                {
                                    if (secondIV == "")
                                        secondIV = IVName;
                                    else
                                        secondIV = secondIV + "," + IVName;
                                }
                            }
                        }
                    }
                    grid.Rows.Add(row);
                }
                return grid;
            }
            else if (batchID != 0 && moduleID == 0)
            {
                //grid.Headers = assignmentDB.Where(x => x.TermDetail.Term.BatchID == batchID && x.Status == 1).OrderBy(x => x.TermDetail.sortOrder).ToList();
                var assignmentFirst = db.Assignments.Where(x => x.TermDetail.Term.BatchID == batchID && x.Status == 1 && x.SubmissionType == 1).OrderBy(x => x.TermDetail.sortOrder).ToList();
                var assignmentFinal = db.Assignments.Where(x => x.TermDetail.Term.BatchID == batchID && x.Status == 1 && x.SubmissionType == 2).OrderBy(x => x.TermDetail.sortOrder).ToList();

                var mixAssignments = assignmentFinal;

                var studentIDFinal = assignmentFinal.Select(x => x.AssignmentSubmissions.Select(xx => xx.StudentID)).ToList();

                //will add to mix if not contain in finalList
                foreach (var ass in assignmentFirst)
                {
                    var studentIDFirst = ass.AssignmentSubmissions.Select(x => x.StudentID).ToList();
                    if (!studentIDFinal.Any(x => x == studentIDFinal))
                    {
                        mixAssignments.Add(ass);
                    }
                }
                grid.Headers = mixAssignments;
                List<User> stdList = db.BatchDetails.Where(x => x.BatchID == batchID).Select(x => x.User2).Where(x => x.UserType == 1).ToList();
                grid.TotalStudent = stdList.Count;
                foreach (User student in stdList)
                {
                    ResultGridRow row = new ResultGridRow();
                    row.StudentName = student.FullName;
                    row.LoginName = student.LoginName;
                    foreach (Assignment assign in grid.Headers)
                    {

                        //Assignment assign = db.Assignments.Where(x => x.TermDetail.ModuleID == module.ID).OrderBy(x => x.SubmissionType).ToList().LastOrDefault();

                        ResultCell cell = new ResultCell();

                        cell.Value = assignmentSubmissionDB.Where(x => x.StudentID == student.ID && x.AssignmentID == assign.ID).OrderBy(x => x.Stage).ToList().LastOrDefault();

                        row.cells.Add(cell);
                    }
                    grid.Rows.Add(row);
                }
                return grid;
            }
            //else if (accessorID != 0)
            //{

            //    //grid.Headers = assignmentDB.Where(x => x.TermDetail.Term.BatchID == batchID && x.Status == 1).OrderBy(x => x.TermDetail.sortOrder).ToList();
            //    grid.Headers = db.AccessorComments.Where(x => x.AccessorID == accessorID).Select(x => x.AssignmentSubmission.Assignment).OrderBy(x => x.TermDetail.ModuleID).ToList();
            //    List<User> stdList = db.BatchDetails.Select(x => x.User2).Where(x => x.UserType == 1).ToList();
            //    grid.TotalStudent = stdList.Count;
            //    //foreach (User student in stdList)
            //    //{
            //    ResultGridRow row = new ResultGridRow();
            //    //row.StudentName = student.FullName;
            //    //row.LoginName = student.LoginName;
            //    foreach (Assignment assign in grid.Headers)
            //    {

            //        //Assignment assign = db.Assignments.Where(x => x.TermDetail.ModuleID == module.ID).OrderBy(x => x.SubmissionType).ToList().LastOrDefault();

            //        ResultCell cell = new ResultCell();

            //        cell.Value = assignmentSubmissionDB.Where(x => x.AssignmentID == assign.ID).OrderBy(x => x.Stage).ToList().LastOrDefault();

            //        row.cells.Add(cell);
            //    }
            //    grid.Rows.Add(row);
            //    //}
            //    return grid;
            //}
            else
                return null;
        }

        #endregion
        public class AssignmentData
        {
            public int GradingStatus { get; set; }
            public int AssignmentSubmissionID { get; set; }

        }
        public class AssignmentStatus
        {
            public int SubmissionType { get; set; }
            public int Status { get; set; }
        }
        public class Grading
        {
            public int FirstResult { get; set; }
            public int FinalResult { get; set; }

        }
        public class NextSubmission
        {
            public string NextSubmissionDate { get; set; }
            public string NextSubmissionModule { get; set; }
            public int AttendanceRate { get; set; }
            public string CurrentTermName { get; set; }
            public string CurrentModuleName { get; set; }
            public string CurrentModuleResult { get; set; }
            public string Submisstion { get; set; }
            public TermDetail TermDetail { get; set; }
        }

        #region SuperAdminResult
        public class ResultGrid
        {
            public ResultGrid()
            {
                Headers = new List<Assignment>();
                Rows = new List<ResultGridRow>();
            }

            public List<Assignment> Headers { get; set; }
            public List<ResultGridRow> Rows { get; set; }
            public int TotalStudent { get; set; }
            //private String[] _ApproveStatus;
            //public String[] ApproveStatus
            //{
            //    get
            //    {
            //        _ApproveStatus = new String[this.Headers.Count];


            //        for (int c = 0; c < this.Headers.Count; c++)
            //        {
            //            String status = "Pending";

            //            int ivcomments = 0;
            //            int rejects = 0;

            //            for (int r = 0; r < this.Rows.Count; r++)
            //            {
            //                if (this.Rows.ToArray()[r].cells.ToArray()[c].Value.IVComments != null)
            //                {
            //                    ivcomments += 1;

            //                    if (this.Rows.ToArray()[r].cells.ToArray()[c].Value.IVComments.Any(x => x.Status == 1))
            //                    {
            //                        rejects = 0;
            //                        break;
            //                    }

            //                }

            //            }

            //        }

            //        return _ApproveStatus;

            //    }
            //    private set {; }
            //}

        }

        public class ResultCell
        {
            public AssignmentSubmission Value { get; set; }
            public bool IsOverSubmissionDate
            {
                get
                {
                    if (Value != null)
                    {
                        if (Value.Assignment.SubmissionType == 1)
                            return Value.Assignment.TermDetail.FirstSubmission < DateTime.Now;
                        else
                            return Value.Assignment.TermDetail.FinalSubmission < DateTime.Now;
                    }
                    else return false;
                }
                private set {; }
            }
        }

        public class ResultGridRow
        {
            public ResultGridRow()
            {
                cells = new List<ResultCell>();
            }
            public String StudentName { get; set; }
            public String LoginName { get; set; }
            public List<ResultCell> cells { get; set; }
        }
        #endregion

    }
}