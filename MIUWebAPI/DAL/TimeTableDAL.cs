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
using static MIUWebAPI.ViewModels.ReportAttendanceInfo;
using static MIUWebAPI.ViewModels.TimeTableInfo;

namespace MIUWebAPI.DAL
{
    public class TimeTableDAL
    {

        public Task<List<UserInfo>> GetTimeTableList(int batchID, string LecturerName)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<UserInfo> stdList = new List<UserInfo>();
                    try
                    {

                        if (batchID != 0)
                        {
                            stdList = (from a in db.vProgramPlans
                                       join b in db.Users on a.LectureID equals b.ID
                                       where a.BatchID == batchID && b.UserType == 2 && b.IsDelete != true
                                       group b by b.ID into u
                                       select new UserInfo
                                       {
                                           FullName = u.FirstOrDefault().FullName,
                                           //LoginName = u.FirstOrDefault().LoginName,
                                           ID = u.FirstOrDefault().ID,
                                           LoginName = u.FirstOrDefault().LoginName,
                                           ProfilePicture = u.FirstOrDefault().ProfilePicture,
                                           ProfilePicture2 = u.FirstOrDefault().ProfilePicture2
                                       }).OrderBy(x => x.FullName).ToList();

                            if (LecturerName != "")
                            {
                                stdList = stdList.Where(x => x.FullName.Contains(LecturerName)).ToList();
                            }
                        }
                        return stdList;
                    }
                    catch (Exception)
                    {
                        return stdList;
                    }

                }
            });
        }

        public Task<List<LecturerTimeTable>> GetLecturerTimeTable(int batchID, int lecturerID, string date)
        {
            return Task.Run(() =>
            {
                DateTime dateTime = DateTime.Now;
                using (MIUEntities db = new MIUEntities())
                {
                    List<LecturerTimeTable> timeTables = new List<LecturerTimeTable>();
                    if (date != "\"\"" && !String.IsNullOrEmpty(date))
                    {
                        dateTime = Convert.ToDateTime(date);
                    }
                    if (batchID != 0)
                    {
                        timeTables = (from p in db.vProgramPlans
                                      join u in db.Users on p.LectureID equals u.ID
                                      join b in db.Batches on p.BatchID equals b.ID
                                      join m in db.Modules on p.ModuleID equals m.ID
                                      join c in db.Courses on b.CourseID equals c.ID
                                      join t in db.TimeTableDetails on p.TermDetailID equals t.TermDetailID
                                      where b.ID == batchID && u.ID == lecturerID && DbFunctions.TruncateTime(t.Date) == dateTime.Date
                                      select new LecturerTimeTable
                                      {
                                          LecturerID = u.ID,
                                          LoginName = u.LoginName,
                                          FullName = u.FullName,
                                          ProfilePicture = u.ProfilePicture,
                                          ProfilePicture2 = u.ProfilePicture2,
                                          CourseName = c.CourseName,
                                          ModuleCode = m.ModuleCode,
                                          ModuleName = m.ModuleName,
                                          StartTime = t.StartTime,
                                          EndTime = t.EndTime,
                                          TimeTableDate = t.Date
                                      }).ToList();

                        foreach(var data in timeTables)
                        {
                            data.ProfilePicture = MIUFileServer.GetFileUrl("ProfileImages", data.ProfilePicture);
                            data.ProfilePicture2 = data.ProfilePicture2 != null? MIUFileServer.GetFileUrl("ProfileImages", data.ProfilePicture2):"";
                        }

                    }

                    return timeTables;
                }
            });
        }

        public Task<List<LecturerTimeTable>> GetAllLecturerTimeTable(int batchID, string date)
        {
            return Task.Run(() =>
            {
                DateTime dateTime = DateTime.Now;
                using (MIUEntities db = new MIUEntities())
                {
                    List<LecturerTimeTable> timeTables = new List<LecturerTimeTable>();
                    if (date != "\"\"" && !String.IsNullOrEmpty(date))
                    {
                        dateTime = Convert.ToDateTime(date);
                    }
                    if (batchID != 0)
                    {
                        timeTables = (from p in db.vProgramPlans
                                      join u in db.Users on p.LectureID equals u.ID
                                      join b in db.Batches on p.BatchID equals b.ID
                                      join m in db.Modules on p.ModuleID equals m.ID
                                      join c in db.Courses on b.CourseID equals c.ID
                                      join t in db.TimeTableDetails on p.TermDetailID equals t.TermDetailID
                                      where b.ID == batchID  && DbFunctions.TruncateTime(t.Date) == dateTime.Date
                                      select new LecturerTimeTable
                                      {
                                          LecturerID = u.ID,
                                          LoginName = u.LoginName,
                                          FullName = u.FullName,
                                          ProfilePicture = u.ProfilePicture,
                                          ProfilePicture2 = u.ProfilePicture2,
                                          CourseName = c.CourseName,
                                          ModuleCode = m.ModuleCode,
                                          ModuleName = m.ModuleName,
                                          StartTime = t.StartTime,
                                          EndTime = t.EndTime,
                                          TimeTableDate = t.Date
                                      }).ToList();

                    }

                    foreach (var data in timeTables)
                    {
                        data.ProfilePicture = MIUFileServer.GetFileUrl("ProfileImages", data.ProfilePicture);
                        data.ProfilePicture2 = data.ProfilePicture2 != null ? MIUFileServer.GetFileUrl("ProfileImages", data.ProfilePicture2) : "";
                    }

                    return timeTables;
                }
            });
        }

        public Task<List<StudentTimeTable>> GetStudentTimeTable(int userID, int batchID, string date)
        {
            return Task.Run(() =>
            {
                DateTime dateTime = DateTime.Now;
                using (MIUEntities db = new MIUEntities())
                {
                    List<LecturerTimeTable> timeTables = new List<LecturerTimeTable>();
                    List<StudentTimeTable> StudentTimeTables = new List<StudentTimeTable>();
                    CommonDAL commonDAL = new CommonDAL();
                    AttRateAndPercent attRateAndPercent = commonDAL.GetAttRateAndPercent(batchID, userID);
                    if (date != "\"\"" && !String.IsNullOrEmpty(date))
                    {
                        dateTime = Convert.ToDateTime(date);
                    }
                    if (attRateAndPercent.CurrentTermId != 0)
                    {
                        //timeTables = (from p in db.vProgramPlans
                        //              join u in db.Users on p.LectureID equals u.ID
                        //              join b in db.Batches on p.BatchID equals b.ID
                        //              join m in db.Modules on p.ModuleID equals m.ID
                        //              join c in db.Courses on b.CourseID equals c.ID
                        //              join t in db.TimeTableDetails on p.TermDetailID equals t.TermDetailID
                        //              where b.ID == batchID  && DbFunctions.TruncateTime(t.Date) == dateTime.Date
                        //              select new LecturerTimeTable
                        //              {
                        //                  LecturerID = u.ID,
                        //                  LoginName = u.LoginName,
                        //                  FullName = u.FullName,
                        //                  ProfilePicture = u.ProfilePicture,
                        //                  ProfilePicture2 = u.ProfilePicture2,
                        //                  CourseName = c.CourseName,
                        //                  ModuleCode = m.ModuleCode,
                        //                  ModuleName = m.ModuleName,
                        //                  StartTime = t.StartTime,
                        //                  EndTime = t.EndTime,
                        //                  TimeTableDate = t.Date
                        //              }).ToList();

                        StudentTimeTables = (from td in db.TimeTableDetails
                                             join ts in db.TimeSettings on td.TimeSettingID equals ts.ID
                                             join trd in db.TermDetails on td.TermDetailID equals trd.ID
                                             join m in db.Modules on trd.ModuleID equals m.ID
                                             join u in db.Users on trd.LectureID equals u.ID
                                             join t in db.Attendances
                                             //.Where(t => t.TermID == attRateAndPercent.CurrentTermId && t.StudentID == userID) 
                                             on td.ID equals t.TimeTableDetailID
                                             where t.StudentID == userID
                                             //td.TermID == attRateAndPercent.CurrentTermId
                                             //&& 
                                             //td.TermDetailID == t.TermDetailID
                                             //&& t.TermID == attRateAndPercent.CurrentTermId
                                             &&
                                             DbFunctions.TruncateTime(td.Date) == dateTime.Date
                                             orderby td.ID
                                             select new StudentTimeTable
                                             {
                                                 TimeTableDetailID = td.ID,
                                                 Section = ts.Section,
                                                 StartTime = td.StartTime.ToString(),
                                                 EndTime = td.EndTime.ToString(),
                                                 TimeTableDate = td.Date,
                                                 LectureID = trd.LectureID,
                                                 LectureName = u.FullName,
                                                 ProfilePicture = u.ProfilePicture,
                                                 ModuleID = m.ID,
                                                 ModuleName = m.ModuleName,
                                                 IsAttending = t.IsAttending,
                                                 Attendance = t.IsAttending == true ? "Attended" : "Missed"
                                            }).ToList();
                    }
                    //string strnow = DateTime.Now.ToString("hh:mm tt", CultureInfo.InvariantCulture);
                    //DateTime now = DateTime.ParseExact(strnow, "hh:mm tt", CultureInfo.InvariantCulture);
                    foreach (var data in StudentTimeTables)
                    {
                        data.ProfilePicture = MIUFileServer.GetFileUrl("ProfileImages", data.ProfilePicture);
                        if (data.TimeTableDate.Value.Date > DateTime.Now.Date)
                        {
                            data.Attendance = "";
                        }
                        else if(data.TimeTableDate.Value.Date == DateTime.Now.Date)
                        {
                            TimeSpan start;
                            TimeSpan end;
                            TimeSpan now = DateTime.Now.TimeOfDay;

                            if (data.StartTime.Contains("PM"))
                            {
                                string starttime = data.StartTime.Replace("PM", "");

                                string[] startStr = starttime.Split(':');
                                startStr[0] = (int.Parse(startStr[0]) + 12).ToString();
                                start = new TimeSpan(int.Parse(startStr[0]), int.Parse(startStr[1]),0);
                            }
                            else
                            {
                                string starttime = data.StartTime.Replace("AM", "");
                                string[] startStr = starttime.Split(':');
                                startStr[0] = (int.Parse(startStr[0])).ToString();
                                start = new TimeSpan(int.Parse(startStr[0]), int.Parse(startStr[1]), 0);
                            }

                            if (data.EndTime.Contains("PM"))
                            {
                                string endtime = data.EndTime.Replace("PM", "");

                                string[] endStr = endtime.Split(':');
                                endStr[0] = (int.Parse(endStr[0]) + 12).ToString();
                                end = new TimeSpan(int.Parse(endStr[0]), int.Parse(endStr[1]), 0);
                            }
                            else
                            {
                                string endtime = data.EndTime.Replace("AM", "");
                                string[] endStr = endtime.Split(':');
                                endStr[0] = (int.Parse(endStr[0])).ToString();
                                end = new TimeSpan(int.Parse(endStr[0]), int.Parse(endStr[1]), 0);
                            }

                            if ((now < start) && (now < end))
                            {
                                data.Attendance = "";
                            }
                        }
                        //string strstart = DateTime.Now.ToString("dd-MMM-yy") + " " + data.StartTime.ToString();
                        //string strend = DateTime.Now.ToString("dd-MMM-yy") + " " + data.EndTime.ToString();
                        //DateTime end = DateTime.ParseExact(strend, "hh:mm:ss tt", CultureInfo.InvariantCulture);
                        //DateTime start = DateTime.ParseExact(strstart, "hh:mm:ss tt", CultureInfo.InvariantCulture);


                        //if (DbFunctions.TruncateTime(data.TimeTableDate) == DateTime.Now.Date && start >= now && end <= now)
                        //{
                        //    data.Attendance = "Attending";
                        //}
                        //else if((DbFunctions.TruncateTime(data.TimeTableDate) == DateTime.Now.Date && end > now) || data.TimeTableDate > DateTime.Now)
                        //{
                        //    data.Attendance = "";
                        //}
                    }

                    return StudentTimeTables;
                }
            });
        }

        public Task<HolidayWrap> GetHoliday(string Month, int Year)
        {
            return Task.Run(() =>
            {
                string s = Year + "-" + Month;
                DateTime dt = DateTime.ParseExact(s, "yyyy-MMM", CultureInfo.InvariantCulture);
                using (MIUEntities db = new MIUEntities())
                {
                    HolidayWrap holidayWrap = new HolidayWrap();
                    List<HolidayInfo> HolidayInfos = new List<HolidayInfo>();

                    List<Holiday> holiday = db.Holidays.Where(x => x.Date.Year == dt.Year && x.Date.Month == dt.Month).OrderBy(x => x.Date).ToList();

                    foreach (var data in holiday)
                    {
                        HolidayInfo info = new HolidayInfo();

                        PropertyCopier<Holiday, HolidayInfo>.Copy(data, info);
                        info.Week = data.Date.ToString("ddd");
                        info.Day = data.Date.Day.ToString();
                        HolidayInfos.Add(info);
                    }

                    holidayWrap.HolidayInfo = HolidayInfos;
                    holidayWrap.TotalHoliday = HolidayInfos.Count;

                    return holidayWrap;
                }
            });
        }
    }
}