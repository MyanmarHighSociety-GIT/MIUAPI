using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using MIUWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MIUWebAPI.DAL
{
    public class StudentDashboardDAL
    {
        public Task<List<UserInfo>> GetStudentDashboard(int batchID, string StudentName)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<UserInfo> infoList = new List<UserInfo>();
                    List<User> stdList = new List<User>();
                    try
                    {
                        if (batchID != 0)
                        {
                            stdList = (from a in db.BatchDetails
                                       join b in db.Users on a.StudentID equals b.ID
                                       where a.BatchID == batchID && b.UserType == 1 && b.IsDelete != true
                                       select b).ToList();

                            if (StudentName != "")
                            {
                                stdList = stdList.Where(x => x.FullName.Contains(StudentName)).ToList();
                            }

                            foreach (var data in stdList)
                            {
                                UserInfo info = new UserInfo();
                                PropertyCopier<User, UserInfo>.Copy(data, info);

                                infoList.Add(info);
                            }
                        }
                        return infoList;
                    }
                    catch (Exception)
                    {
                        return infoList;
                    }
                }
            });
        }

        public Task<List<LectureCourseListInfo>> GetLectureCourseList(int accessorID)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<LectureCourseListInfo> infoList = new List<LectureCourseListInfo>();
                    //List<User> stdList = new List<User>();
                    try
                    {
                        var course = (from td in db.TermDetails
                                      join t in db.Terms on td.TermID equals t.ID
                                      join b in db.Batches on t.BatchID equals b.ID
                                      join c in db.Courses on b.CourseID equals c.ID
                                      where td.LectureID == accessorID
                                      && b.IsDelete != true
                                      && c.IsDelete != true
                                      //&& DateTime.Now >= t.StartDate && DateTime.Now <= t.CompletionDate
                                      group c.CourseName by c.ID into g
                                      select new { ID = g.Key, CourseName = g.ToList() }).ToList();

                        foreach (var data in course)
                        {
                            LectureCourseListInfo info = new LectureCourseListInfo();
                            info.CourseID = data.ID;
                            info.CourseName = data.CourseName.FirstOrDefault();

                            infoList.Add(info);
                        }

                    }
                    catch (Exception)
                    {

                    }

                    return infoList;
                }
            });
        }
        
        public Task<List<LectureModuleListInfo>> GetLectureModuleList(int accessorID)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<LectureModuleListInfo> infoList = new List<LectureModuleListInfo>();                    
                    try
                    {
                        infoList = db.TermDetails.Where(a => a.LectureID == accessorID).Select(m => new LectureModuleListInfo {ModuleCode= m.Module.ModuleCode,ID= m.Module.ID }).Distinct().ToList();
                    }
                    catch (Exception)
                    {

                    }
                    return infoList;
                }
            });
        }

        public Task<LectureDashboardInfo> GetLectureDashboard(int accessorID, int courseID) // accessorID = 545, courseID = 9
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    LectureDashboardInfo infoList = new LectureDashboardInfo();
                    List<LectureResultInfo> LectureResultInfo = new List<LectureResultInfo>();
                    //List<User> stdList = new List<User>();
                    //try
                    //{
                        var currentTime = DateTime.Now.ToString("h:mm tt", CultureInfo.InvariantCulture);

                        var time = DateTime.ParseExact(currentTime, "h:mm tt", CultureInfo.InvariantCulture);
                        if (courseID == 0)
                        {

                            var module = (from td in db.TermDetails
                                          join ttd in db.TimeTableDetails on td.ID equals ttd.TermDetailID
                                          join t in db.Terms on td.TermID equals t.ID
                                          join b in db.Batches on t.BatchID equals b.ID
                                          join c in db.Courses on b.CourseID equals c.ID
                                          join m in db.Modules on td.ModuleID equals m.ID
                                          where td.LectureID == accessorID
                                          && ttd.Date == DbFunctions.TruncateTime(DateTime.Now)
                                          select new LectureNextClass { CourseID = c.ID, CourseName = c.CourseName, ModuleID = td.ModuleID, ModuleName = m.ModuleName, Date = ttd.Date, StartTime = ttd.StartTime, EndTime = ttd.EndTime }).ToList();


                            foreach (var data in module)
                            {
                                if (DateTime.Now <= Convert.ToDateTime(data.StartTime))
                                {
                                    courseID = data.CourseID;
                                    infoList.Course = data.CourseName;
                                    infoList.Module = data.ModuleName;
                                    infoList.StartTime = data.StartTime;
                                    infoList.EndTime = data.EndTime;

                                    break;
                                }
                            }

                            LectureResultInfo = GetLectureResult(accessorID, courseID);
                        }
                        else
                        {
                            var module = (from td in db.TermDetails
                                          join ttd in db.TimeTableDetails on td.ID equals ttd.TermDetailID
                                          join t in db.Terms on td.TermID equals t.ID
                                          join b in db.Batches on t.BatchID equals b.ID
                                          join c in db.Courses on b.CourseID equals courseID //With CourseID
                                          join m in db.Modules on td.ModuleID equals m.ID
                                          where td.LectureID == accessorID
                                          && ttd.Date == DbFunctions.TruncateTime(DateTime.Now)
                                          select new LectureNextClass { CourseID = c.ID, CourseName = c.CourseName, ModuleID = td.ModuleID, ModuleName = m.ModuleName, Date = ttd.Date, StartTime = ttd.StartTime, EndTime = ttd.EndTime }).ToList();


                            foreach (var data in module)
                            {
                                if (DateTime.Now <= Convert.ToDateTime(data.StartTime))
                                {
                                    infoList.Course = data.CourseName;
                                    infoList.Module = data.ModuleName;
                                    infoList.StartTime = data.StartTime;
                                    infoList.EndTime = data.EndTime;

                                    break;
                                }
                            }
                            LectureResultInfo = GetLectureResult(accessorID, courseID);
                        }
                        infoList.LectureResultInfos = LectureResultInfo;
                        return infoList;
                    //}
                    //catch (Exception ex)
                    //{
                    //    return infoList;
                    //}
                }
            });
        }

        public List<LectureResultInfo> GetLectureResult(int accessorID, int courseID)
        {
            using (MIUEntities db = new MIUEntities())
            {
                List<LectureResultInfo> LectureResultInfo = new List<LectureResultInfo>();

                var batchList = db.AssignmentSubmissions
                                    .Where(x => x.Assignment.Status == 0
                                        && x.Assignment.SubmissionType == 1
                                        && x.Assignment.TermDetail.LectureID == accessorID
                                        && x.Assignment.TermDetail.Term.Batch.Course.ID == courseID)
                                    .GroupBy(x => x.Assignment.TermDetail.Term.Batch.ID)
                                    .Select(y => new { BatchID = y.Key, BatchName = y.FirstOrDefault().Assignment.TermDetail.Term.Batch.BatchName });

                foreach (var batch in batchList)
                {
                    LectureResultInfo lectureResultInfo = new LectureResultInfo();

                    var assignmentSubmission = db.AssignmentSubmissions
                                                .Where(x => x.Assignment.Status == 0
                                                && x.Assignment.SubmissionType == 1
                                                && x.Assignment.TermDetail.LectureID == accessorID
                                                && x.Assignment.TermDetail.Term.Batch.CourseID == courseID
                                                && x.Assignment.TermDetail.Term.Batch.ID == batch.BatchID).ToList();

                    int ModuleID = assignmentSubmission.FirstOrDefault().Assignment.TermDetail.ModuleID;

                    lectureResultInfo.Batch = batch.BatchName;
                    lectureResultInfo.Course = db.Batches.Where(x => x.ID == batch.BatchID).Select(x => x.Course.CourseName).FirstOrDefault();
                    lectureResultInfo.Module = db.Modules.Where(x => x.ID == ModuleID).Select(x => x.ModuleName).SingleOrDefault();
                    lectureResultInfo.Submission = "1st";

                    List<AccessorComment> acList = new List<AccessorComment>();
                    List<IVComment> ivList = new List<IVComment>();
                    foreach (var assignmentSub in assignmentSubmission)
                    {

                        AccessorComment ac = new AccessorComment();
                        IVComment iv = new IVComment();
                        ac = db.AccessorComments.Where(x => x.AssignmentSubmissionID == assignmentSub.ID && x.AccessorID != null).FirstOrDefault();
                        iv = db.IVComments.Where(x => x.AssignmentSubmissionID == assignmentSub.ID && x.IVID != null).FirstOrDefault();

                        if (ac != null)
                            acList.Add(ac);

                        if (iv != null)
                            ivList.Add(iv);
                    }

                    if (acList != null && acList.Count > 0)
                    {
                        int? AccessorID = acList.FirstOrDefault().AccessorID;
                        var AccessorName = db.Users.Where(x => x.ID == AccessorID).FirstOrDefault();
                        lectureResultInfo.Assessor = AccessorName.FullName;
                        lectureResultInfo.Checked = acList.Count();
                    }
                    else
                    {
                        lectureResultInfo.Assessor = "-";
                    }

                    if (ivList != null && ivList.Count > 0)
                    {
                        int? IVID = ivList.FirstOrDefault().IVID;
                        var IVName = db.Users.Where(x => x.ID == IVID).FirstOrDefault();
                        lectureResultInfo.IV = IVName.FullName;
                    }
                    else
                    {
                        lectureResultInfo.IV = "-";
                    }

                    var totalstd = db.BatchDetails.Where(x => x.BatchID == batch.BatchID).ToList();
                    lectureResultInfo.Unpublished = totalstd.Count() - assignmentSubmission.Count();
                    lectureResultInfo.Submitted = assignmentSubmission.Count();
                    lectureResultInfo.NotChecked = lectureResultInfo.Submitted - lectureResultInfo.Checked;

                    LectureResultInfo.Add(lectureResultInfo);

                }
                return LectureResultInfo;
            }
        }

    }
}