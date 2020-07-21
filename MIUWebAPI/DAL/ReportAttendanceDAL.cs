using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using MIUWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static MIUWebAPI.ViewModels.ReportAttendanceInfo;

namespace MIUWebAPI.DAL
{
    public class ReportAttendanceDAL
    {
        public Task<List<UserInfo>> GetReportAttendanceList(int batchID, string StudentName)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<UserInfo> infoList = new List<UserInfo>();
                    List<User> stdList = new List<User>();
                    //try
                    //{
                    if (batchID != 0)
                    {
                        stdList = (from a in db.BatchDetails
                                   join b in db.Users on a.StudentID equals b.ID
                                   where a.BatchID == batchID && b.UserType == 1 && b.IsDelete != true
                                   select b).OrderBy(x => x.FullName).ToList();

                        if (StudentName != "")
                        {
                            stdList = stdList.Where(x => x.FullName.Contains(StudentName)).ToList();
                        }

                        foreach (var data in stdList)
                        {
                            UserInfo info = new UserInfo();
                            PropertyCopier<User, UserInfo>.Copy(data, info);
                            info.ProfilePicture = MIUFileServer.GetFileUrl("ProfileImages", data.ProfilePicture);
                            info.ProfilePicture2 = MIUFileServer.GetFileUrl("ProfileImages", data.ProfilePicture2);
                            infoList.Add(info);
                        }
                    }
                    return infoList;
                    //}
                    //catch (Exception)
                    //{
                    //    return infoList;
                    //}
                }
            });
        }

        public Task<AttendanceGrid> GetReportAttendance(int batchID, int studentID)
        {
            return Task.Run(async () =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    //try
                    //{
                    if (batchID != 0)
                    {

                        string modulecodelist = "";
                        AttendanceGrid grid = new AttendanceGrid();

                        //var terms = db.Terms.AsNoTracking().Where(x => x.BatchID == batchID).Select(x => new { ID = x.ID, TermName = x.TermName, StartDate = x.StartDate, sortOrer = x.sortOrder }).OrderBy(y => y.sortOrer).ToList();
                        List<Term> terms = db.Terms.AsNoTracking().Where(x => x.BatchID == batchID).OrderBy(y => y.sortOrder).ToList();

                        AttRateAndPercent attRateAndPercent = new AttRateAndPercent();
                        attRateAndPercent = await GetAttRateAndPercent(batchID, studentID);

                        grid.AttendanceRateTermName = attRateAndPercent.AttendanceRateTermName;
                        grid.AttendanceRatePercent = attRateAndPercent.AttenddanceRatePercent;

                        List<TermInfo> terminfo = new List<TermInfo>();

                        foreach (var data in terms)
                        {
                            TermInfo info = new TermInfo();
                            PropertyCopier<Term, TermInfo>.Copy(data, info);
                            //info.IsFavorite = db.FavoriteLists.Any(a => a.FavoriteID == data.ID && a.UserID == userID && a.FavoriteType == "Announcement");
                            terminfo.Add(info);
                        }

                        var termlist = db.Terms.AsNoTracking().Where(x => x.BatchID == batchID).Select(x => x.ID).ToList();
                        var frommonth = DateTime.Now.AddMonths(-24);
                        var tomonth = DateTime.Now.AddMonths(24);
                        var attendances = db.Attendances.AsNoTracking().Where(x => x.Date >= frommonth && x.Date <= tomonth && termlist.Contains(x.TermID)).ToList();

                        grid.TermDetail = terminfo;

                        //For Module Code List Of Header

                        foreach (var term in terms)
                        {
                            List<TermDetail> tds = db.TermDetails.AsNoTracking().Where(td => td.TermID == term.ID && td.Attendence == true).ToList();

                            modulecodelist = "";
                            foreach (var td in tds)
                            {
                                if (modulecodelist != "")
                                    modulecodelist += ",";
                                modulecodelist += td.Module.ModuleCode;
                            }

                            foreach (TermInfo item in grid.TermDetail)
                            {
                                if (item.ID == term.ID)
                                {
                                    item.ModuleCodeList = modulecodelist;
                                }
                            }

                            //grid.ModuleCodeList.Add(modulecodelist);

                            int count = attendances.Where(a => a.TermID == term.ID).GroupBy(a => a.Month).Count();
                            grid.MonthList.Add(count);
                        }


                        //For Row of Grid
                        List<UserInfo> stdList = new List<UserInfo>();
                        if (studentID != 0)
                        {
                            stdList = (from a in db.BatchDetails
                                       join b in db.Users on a.StudentID equals b.ID
                                       where a.BatchID == batchID && b.ID == studentID && b.UserType == 1 && b.IsDelete != true
                                       select new UserInfo { ID = b.ID, FullName = b.FullName, LoginName = b.LoginName, ProfilePicture = b.ProfilePicture, ProfilePicture2 = b.ProfilePicture2 }).ToList();
                        }
                        else
                        {
                            stdList = (from a in db.BatchDetails
                                       join b in db.Users on a.StudentID equals b.ID
                                       where a.BatchID == batchID && b.UserType == 1 && b.IsDelete != true
                                       select new UserInfo { ID = b.ID, FullName = b.FullName, LoginName = b.LoginName, ProfilePicture = b.ProfilePicture, ProfilePicture2 = b.ProfilePicture2 }).ToList();
                            //stdList = db.BatchDetails.AsNoTracking().Where(x => x.BatchID == batchID).Select(x => x.User).Where(x => x.UserType == 1 && x.IsDelete == null || x.IsDelete != true).ToList();
                        }

                        //var frommonth = DateTime.Now.AddMonths(-6);
                        //var tomonth = DateTime.Now.AddMonths(6);
                        //var attendances = db.Attendances.Where(x => x.Date >= frommonth && x.Date <= tomonth);

                        //decimal SutdentCountYear1 = 0;
                        //decimal SutdentCountYear2 = 0;
                        //decimal StudentLastYear = 0;
                        //decimal StudentFoundation = 0;

                        if (stdList.Count() > 0)
                        {
                            foreach (UserInfo student in stdList)
                            {
                                //AttendanceGridRow row = new AttendanceGridRow();
                                //row.StudentName = student.FullName;
                                //row.StudentID = student.LoginName;
                                var attendance = attendances.Where(x => x.StudentID == student.ID);

                                decimal TotalYear1 = 0;
                                decimal TotalYear2 = 0;
                                decimal TotalLastYear = 0;
                                decimal TotalFoundation = 0;
                                int Year2Count = 0;
                                int LastYearCount = 0;
                                int FoundationCount = 0;
                                foreach (var term in terms)
                                {
                                    int percentageValue1 = 0; int percentageValue2 = 0;

                                    //double attendingCount = 0;
                                    double totalCount1 = 0; double totalCount2 = 0; double totalCount = 0; double attendingTotalCount = 0;

                                    int daysOfFirstMonth = endOfMonth(term.StartDate.Value.Month, term.StartDate.Value.Year) - term.StartDate.Value.Day;
                                    int firstMonth = daysOfFirstMonth > 5 ? term.StartDate.Value.Month : term.StartDate.Value.AddMonths(1).Month;
                                    totalCount1 = attendance.Where(x => x.TermID == term.ID && x.Month <= firstMonth).Select(y => y.ID).Count();
                                    totalCount2 = attendance.Where(x => x.TermID == term.ID && x.Month > firstMonth).Select(y => y.ID).Count();
                                    totalCount = attendance.Where(x => x.TermID == term.ID).Select(y => y.ID).Count();

                                    AttendanceCell cell1 = new AttendanceCell();
                                    AttendanceCell cell2 = new AttendanceCell();
                                    AttendanceCell totalcell = new AttendanceCell();

                                    totalCount1 = attendance.Where(x => x.TermID == term.ID && x.Month <= firstMonth).Select(y => y.ID).Count();
                                    if (totalCount1 > 0)
                                    {
                                        var attendingList1 = attendance.Where(x => x.TermID == term.ID && x.IsAttending == true && x.Month <= firstMonth).Select(y => y.ID).Count();

                                        if (attendingList1 != 0 && attendingList1 > 0)
                                        {
                                            attendingTotalCount += attendingList1;
                                            percentageValue1 = (int)Math.Round(((attendingList1 / totalCount1) * 100));
                                        }

                                    }
                                    if (totalCount2 > 0)
                                    {
                                        var attendingList2 = attendance.Where(x => x.TermID == term.ID && x.IsAttending == true && x.Month > firstMonth).Select(y => y.ID).Count();
                                        if (attendingList2 != 0 && attendingList2 > 0)
                                        {
                                            //attendingCount = count2;
                                            attendingTotalCount += attendingList2;
                                            percentageValue2 = (int)Math.Round(((attendingList2 / totalCount2) * 100));
                                            //percentageValue2 = (int)(((attendingCount / totalCount2) * 100));
                                            //percentagedouble2 = (count2 / totalCount2) * 100;
                                        }
                                    }
                                    cell1.Value = percentageValue1;
                                    cell2.Value = percentageValue2;
                                    //totalcell.Value = (int)Math.Round((((double)percentageValue1+(double)percentageValue2)*100)/200);
                                    //totalcell.Value = (int)Math.Round((((double)percentagedouble1 + (double)percentagedouble2) * 100) / 200);
                                    if (totalCount > 0)
                                    {
                                        //totalCount = attendance.Where(x => x.TermID == term.ID).Count();
                                        totalcell.Value = (int)Math.Round(((attendingTotalCount / totalCount) * 100));

                                        if (term.TermCode == "Term-2" || term.TermCode == "Term-3" || term.TermCode == "Term-4" || term.TermCode == "Term-5")
                                        {
                                            TotalYear1 += totalcell.Value;
                                        }
                                        else if (term.TermCode == "Term-6" || term.TermCode == "Term-7" || term.TermCode == "Term-8" || term.TermCode == "Term-9")
                                        {
                                            Year2Count++;
                                            TotalYear2 += totalcell.Value;
                                        }
                                        else if (term.TermCode == "Term-10" || term.TermCode == "Term-11" || term.TermCode == "Term-12" || term.TermCode == "Term-13")
                                        {
                                            LastYearCount++;
                                            TotalLastYear += totalcell.Value;
                                        }
                                        else
                                        {
                                            FoundationCount++;
                                            TotalFoundation += totalcell.Value;
                                        }
                                    }
                                    else
                                    {
                                        totalcell.Value = 0;

                                        if (term.TermCode == "Term-2" || term.TermCode == "Term-3" || term.TermCode == "Term-4" || term.TermCode == "Term-5")
                                        {
                                            TotalYear1 += totalcell.Value;
                                        }
                                        else if (term.TermCode == "Term-6" || term.TermCode == "Term-7" || term.TermCode == "Term-8" || term.TermCode == "Term-9")
                                        {
                                            Year2Count++;
                                            TotalYear2 += totalcell.Value;
                                        }
                                        else if (term.TermCode == "Term-10" || term.TermCode == "Term-11" || term.TermCode == "Term-12" || term.TermCode == "Term-13")
                                        {
                                            LastYearCount++;
                                            TotalLastYear += totalcell.Value;
                                        }
                                        else
                                        {
                                            FoundationCount++;
                                            TotalFoundation += totalcell.Value;
                                        }
                                    }
                                    //row.cells.Add(cell1);
                                    //row.cells.Add(cell2);
                                    //row.cells.Add(totalcell);
                                    grid.StudentID = student.ID;
                                    grid.StudentName = student.FullName;
                                    grid.LoginName = student.LoginName;
                                    grid.BatchName = term.Batch.BatchName;
                                    grid.CourseName = term.Batch.Course.CourseName;
                                    grid.ProfilePicture = MIUFileServer.GetFileUrl("ProfileImages", student.ProfilePicture);
                                    grid.ProfilePicture2 = MIUFileServer.GetFileUrl("ProfileImages", student.ProfilePicture2);
                                    //grid.ProfilePicture2 = student.ProfilePicture2;
                                    foreach (TermInfo item in grid.TermDetail)
                                    {
                                        if (item.ID == term.ID)
                                        {
                                            //item.StudentID = student.ID;
                                            //item.StudentName = student.FullName;
                                            item.First = percentageValue1;
                                            item.Second = percentageValue2;
                                            item.Total = totalcell.Value;
                                        }
                                    }
                                }
                                //decimal a = 0;
                                //decimal b = 0;
                                //decimal c = 0;
                                //decimal d = 0;

                                //if (TotalYear1 > 0)
                                //{
                                //    a = Math.Round(TotalYear1 / 4);
                                //}
                                //if (TotalYear2 > 0)
                                //{
                                //    b = Math.Round(TotalYear2 / Year2Count);
                                //}
                                //if (TotalFoundation > 0)
                                //{
                                //    c = Math.Round(TotalFoundation / FoundationCount);
                                //}
                                //if (TotalLastYear > 0)
                                //{
                                //    d = Math.Round(TotalLastYear / LastYearCount);
                                //}

                                //if (a >= 70)
                                //{
                                //    SutdentCountYear1++;
                                //}
                                //if (b >= 70)
                                //{
                                //    SutdentCountYear2++;
                                //}
                                //if (c >= 70)
                                //{
                                //    StudentFoundation++;
                                //}
                                //if (d >= 70)
                                //{
                                //    StudentLastYear++;
                                //}
                                //grid.Rows.Add(row);
                            }
                        }
                        int index = 0;
                        foreach (var item in grid.TermDetail)
                        {
                            if (index == 0)
                            {
                                item.YearName = "Foundation";
                            }
                            else if (index > 0 && index <= 4)
                            {
                                item.YearName = "Year 1";
                            }
                            else if (index > 4 && index <= 8)
                            {
                                item.YearName = "Year 2";
                            }
                            else
                            {
                                item.YearName = "";
                            }
                            index++;
                        }
                        return grid;
                    }
                    else
                        return null;
                    //}
                    //catch (Exception)
                    //{
                    //    return null;
                    //}
                }
            });
        }


        public Task<StudentAttendance> GetStudentAttendance(int batchID, int studentID)
        {
            return Task.Run(async () =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    //try
                    //{
                    if (batchID != 0)
                    {

                        string modulecodelist = "";
                        StudentAttendance grid = new StudentAttendance();

                        //var terms = db.Terms.AsNoTracking().Where(x => x.BatchID == batchID).Select(x => new { ID = x.ID, TermName = x.TermName, StartDate = x.StartDate, sortOrer = x.sortOrder }).OrderBy(y => y.sortOrer).ToList();
                        List<Term> terms = db.Terms.AsNoTracking().Where(x => x.BatchID == batchID).OrderBy(y => y.sortOrder).ToList();

                        AttRateAndPercent attRateAndPercent = new AttRateAndPercent();
                        attRateAndPercent = await GetAttRateAndPercent(batchID, studentID);

                        grid.AttendanceRateTermName = attRateAndPercent.AttendanceRateTermName;
                        grid.AttendanceRatePercent = attRateAndPercent.AttenddanceRatePercent;

                        List<AttendanceTermInfo> terminfo = new List<AttendanceTermInfo>();

                        foreach (var data in terms)
                        {
                            AttendanceTermInfo info = new AttendanceTermInfo();
                            PropertyCopier<Term, AttendanceTermInfo>.Copy(data, info);
                            //info.IsFavorite = db.FavoriteLists.Any(a => a.FavoriteID == data.ID && a.UserID == userID && a.FavoriteType == "Announcement");
                            terminfo.Add(info);
                        }

                        var termlist = db.Terms.AsNoTracking().Where(x => x.BatchID == batchID).Select(x => x.ID).ToList();
                        var frommonth = DateTime.Now.AddMonths(-24);
                        var tomonth = DateTime.Now.AddMonths(24);
                        var attendances = db.Attendances.AsNoTracking().Where(x => x.Date >= frommonth && x.Date <= tomonth && termlist.Contains(x.TermID)).ToList();

                        grid.AttendanceDetail = terminfo;

                        //For Module Code List Of Header

                        foreach (var term in terms)
                        {
                            List<TermDetail> tds = db.TermDetails.AsNoTracking().Where(td => td.TermID == term.ID && td.Attendence == true).ToList();

                            modulecodelist = "";
                            foreach (var td in tds)
                            {
                                if (modulecodelist != "")
                                    modulecodelist += ",";
                                modulecodelist += td.Module.ModuleCode;
                            }

                            foreach (AttendanceTermInfo item in grid.AttendanceDetail)
                            {
                                if (item.ID == term.ID)
                                {
                                    item.ModuleCodeList = modulecodelist;
                                }
                            }

                            //grid.ModuleCodeList.Add(modulecodelist);

                            int count = attendances.Where(a => a.TermID == term.ID).GroupBy(a => a.Month).Count();
                            grid.MonthList.Add(count);
                        }


                        //For Row of Grid
                        List<UserInfo> stdList = new List<UserInfo>();
                        if (studentID != 0)
                        {
                            stdList = (from a in db.BatchDetails
                                       join b in db.Users on a.StudentID equals b.ID
                                       where a.BatchID == batchID && b.ID == studentID && b.UserType == 1 && b.IsDelete != true
                                       select new UserInfo { ID = b.ID, FullName = b.FullName, LoginName = b.LoginName, ProfilePicture = b.ProfilePicture, ProfilePicture2 = b.ProfilePicture2 }).ToList();
                        }
                        else
                        {
                            stdList = (from a in db.BatchDetails
                                       join b in db.Users on a.StudentID equals b.ID
                                       where a.BatchID == batchID && b.UserType == 1 && b.IsDelete != true
                                       select new UserInfo { ID = b.ID, FullName = b.FullName, LoginName = b.LoginName, ProfilePicture = b.ProfilePicture, ProfilePicture2 = b.ProfilePicture2 }).ToList();
                            //stdList = db.BatchDetails.AsNoTracking().Where(x => x.BatchID == batchID).Select(x => x.User).Where(x => x.UserType == 1 && x.IsDelete == null || x.IsDelete != true).ToList();
                        }

                        //var frommonth = DateTime.Now.AddMonths(-6);
                        //var tomonth = DateTime.Now.AddMonths(6);
                        //var attendances = db.Attendances.Where(x => x.Date >= frommonth && x.Date <= tomonth);

                        //decimal SutdentCountYear1 = 0;
                        //decimal SutdentCountYear2 = 0;
                        //decimal StudentLastYear = 0;
                        //decimal StudentFoundation = 0;

                        if (stdList.Count() > 0)
                        {
                            foreach (UserInfo student in stdList)
                            {
                                //AttendanceGridRow row = new AttendanceGridRow();
                                //row.StudentName = student.FullName;
                                //row.StudentID = student.LoginName;
                                var attendance = attendances.Where(x => x.StudentID == student.ID);

                                decimal TotalYear1 = 0;
                                decimal TotalYear2 = 0;
                                decimal TotalLastYear = 0;
                                decimal TotalFoundation = 0;
                                int Year2Count = 0;
                                int LastYearCount = 0;
                                int FoundationCount = 0;
                                foreach (var term in terms)
                                {
                                    int percentageValue1 = 0; int percentageValue2 = 0;

                                    //double attendingCount = 0;
                                    double totalCount1 = 0; double totalCount2 = 0; double totalCount = 0; double attendingTotalCount = 0;

                                    int daysOfFirstMonth = endOfMonth(term.StartDate.Value.Month, term.StartDate.Value.Year) - term.StartDate.Value.Day;
                                    int firstMonth = daysOfFirstMonth > 5 ? term.StartDate.Value.Month : term.StartDate.Value.AddMonths(1).Month;
                                    totalCount1 = attendance.Where(x => x.TermID == term.ID && x.Month <= firstMonth).Select(y => y.ID).Count();
                                    totalCount2 = attendance.Where(x => x.TermID == term.ID && x.Month > firstMonth).Select(y => y.ID).Count();
                                    totalCount = attendance.Where(x => x.TermID == term.ID).Select(y => y.ID).Count();

                                    AttendanceCell cell1 = new AttendanceCell();
                                    AttendanceCell cell2 = new AttendanceCell();
                                    AttendanceCell totalcell = new AttendanceCell();

                                    totalCount1 = attendance.Where(x => x.TermID == term.ID && x.Month <= firstMonth).Select(y => y.ID).Count();
                                    if (totalCount1 > 0)
                                    {
                                        var attendingList1 = attendance.Where(x => x.TermID == term.ID && x.IsAttending == true && x.Month <= firstMonth).Select(y => y.ID).Count();

                                        if (attendingList1 != 0 && attendingList1 > 0)
                                        {
                                            attendingTotalCount += attendingList1;
                                            percentageValue1 = (int)Math.Round(((attendingList1 / totalCount1) * 100));
                                        }

                                    }
                                    if (totalCount2 > 0)
                                    {
                                        var attendingList2 = attendance.Where(x => x.TermID == term.ID && x.IsAttending == true && x.Month > firstMonth).Select(y => y.ID).Count();
                                        if (attendingList2 != 0 && attendingList2 > 0)
                                        {
                                            //attendingCount = count2;
                                            attendingTotalCount += attendingList2;
                                            percentageValue2 = (int)Math.Round(((attendingList2 / totalCount2) * 100));
                                            //percentageValue2 = (int)(((attendingCount / totalCount2) * 100));
                                            //percentagedouble2 = (count2 / totalCount2) * 100;
                                        }
                                    }
                                    cell1.Value = percentageValue1;
                                    cell2.Value = percentageValue2;
                                    //totalcell.Value = (int)Math.Round((((double)percentageValue1+(double)percentageValue2)*100)/200);
                                    //totalcell.Value = (int)Math.Round((((double)percentagedouble1 + (double)percentagedouble2) * 100) / 200);
                                    if (totalCount > 0)
                                    {
                                        //totalCount = attendance.Where(x => x.TermID == term.ID).Count();
                                        totalcell.Value = (int)Math.Round(((attendingTotalCount / totalCount) * 100));

                                        if (term.TermCode == "Term-2" || term.TermCode == "Term-3" || term.TermCode == "Term-4" || term.TermCode == "Term-5")
                                        {
                                            TotalYear1 += totalcell.Value;
                                        }
                                        else if (term.TermCode == "Term-6" || term.TermCode == "Term-7" || term.TermCode == "Term-8" || term.TermCode == "Term-9")
                                        {
                                            Year2Count++;
                                            TotalYear2 += totalcell.Value;
                                        }
                                        else if (term.TermCode == "Term-10" || term.TermCode == "Term-11" || term.TermCode == "Term-12" || term.TermCode == "Term-13")
                                        {
                                            LastYearCount++;
                                            TotalLastYear += totalcell.Value;
                                        }
                                        else
                                        {
                                            FoundationCount++;
                                            TotalFoundation += totalcell.Value;
                                        }
                                    }
                                    else
                                    {
                                        totalcell.Value = 0;

                                        if (term.TermCode == "Term-2" || term.TermCode == "Term-3" || term.TermCode == "Term-4" || term.TermCode == "Term-5")
                                        {
                                            TotalYear1 += totalcell.Value;
                                        }
                                        else if (term.TermCode == "Term-6" || term.TermCode == "Term-7" || term.TermCode == "Term-8" || term.TermCode == "Term-9")
                                        {
                                            Year2Count++;
                                            TotalYear2 += totalcell.Value;
                                        }
                                        else if (term.TermCode == "Term-10" || term.TermCode == "Term-11" || term.TermCode == "Term-12" || term.TermCode == "Term-13")
                                        {
                                            LastYearCount++;
                                            TotalLastYear += totalcell.Value;
                                        }
                                        else
                                        {
                                            FoundationCount++;
                                            TotalFoundation += totalcell.Value;
                                        }
                                    }
                                    //row.cells.Add(cell1);
                                    //row.cells.Add(cell2);
                                    //row.cells.Add(totalcell);
                                    //grid.StudentID = student.ID;
                                    //grid.StudentName = student.FullName;
                                    //grid.LoginName = student.LoginName;
                                    //grid.BatchName = term.Batch.BatchName;
                                    //grid.CourseName = term.Batch.Course.CourseName;
                                    //grid.ProfilePicture = student.ProfilePicture;
                                    //grid.ProfilePicture2 = student.ProfilePicture2;
                                    foreach (AttendanceTermInfo item in grid.AttendanceDetail)
                                    {
                                        if (item.ID == term.ID)
                                        {
                                            //item.StudentID = student.ID;
                                            //item.StudentName = student.FullName;
                                            item.First = percentageValue1;
                                            item.Second = percentageValue2;
                                            item.Total = totalcell.Value;
                                        }
                                    }
                                }
                                //decimal a = 0;
                                //decimal b = 0;
                                //decimal c = 0;
                                //decimal d = 0;

                                //if (TotalYear1 > 0)
                                //{
                                //    a = Math.Round(TotalYear1 / 4);
                                //}
                                //if (TotalYear2 > 0)
                                //{
                                //    b = Math.Round(TotalYear2 / Year2Count);
                                //}
                                //if (TotalFoundation > 0)
                                //{
                                //    c = Math.Round(TotalFoundation / FoundationCount);
                                //}
                                //if (TotalLastYear > 0)
                                //{
                                //    d = Math.Round(TotalLastYear / LastYearCount);
                                //}

                                //if (a >= 70)
                                //{
                                //    SutdentCountYear1++;
                                //}
                                //if (b >= 70)
                                //{
                                //    SutdentCountYear2++;
                                //}
                                //if (c >= 70)
                                //{
                                //    StudentFoundation++;
                                //}
                                //if (d >= 70)
                                //{
                                //    StudentLastYear++;
                                //}
                                //grid.Rows.Add(row);
                            }
                        }
                        return grid;
                    }
                    else
                        return null;
                    //}
                    //catch (Exception)
                    //{
                    //    return null;
                    //}
                }
            });
        }

        private int endOfMonth(int month, int year)
        {
            int endOfDay = 0;
            if (month == 9 || month == 4 || month == 6 || month == 11)
                endOfDay = 30;
            else if (month == 2 && (year % 4) == 0)
                endOfDay = 29;
            else if (month == 2)
                endOfDay = 28;
            else
                endOfDay = 31;
            return endOfDay;
        }

        public int GetAttendanceRateForDashboard(int termID, int studentID)
        {
            using (MIUEntities db = new MIUEntities())
            {
                int attendanceRate = 0;

                try
                {
                    var totalTerms = db.Attendances.Where(x => x.TermID == termID);


                    if (studentID != 0)
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
                catch (Exception)
                {
                    return attendanceRate;
                }
            }

        }

        public Task<AttRateAndPercent> GetAttRateAndPercent(int batchID, int studentID)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    AttRateAndPercent res = new AttRateAndPercent();
                    try
                    {
                        List<int> ids = db.Terms.Where(x => x.BatchID == batchID).Select(x => x.ID).ToList<int>();
                        IQueryable<TermDetail> termDetailInfo = null;
                        termDetailInfo = db.TermDetails.Where(x => ids.Contains(x.TermID));

                        var firstSubmissionDates = termDetailInfo.Where(x => x.Term.StartDate < DateTime.Now && x.FirstSubmission >= DateTime.Now);
                        var finalSubmissionDates = termDetailInfo.Where(x => x.FirstResult < DateTime.Now && x.FinalSubmission >= DateTime.Now);
                        //var firstSubmissionDates = tds.Where(x => x.Term.StartDate < DateTime.Now && x.FirstSubmission >= DateTime.Now);
                        //var finalSubmissionDates = tds.Where(x => x.FirstResult < DateTime.Now && x.FinalSubmission >= DateTime.Now);

                        if (firstSubmissionDates != null && firstSubmissionDates.Count() > 0)
                        {
                            var submission = firstSubmissionDates.FirstOrDefault();
                            res.AttendanceRateTermName = submission.Term.TermName;
                            res.CurrentTermId = submission.Term.ID;

                            res.AttenddanceRatePercent = GetAttendanceRateForDashboard(submission.Term.ID, studentID);
                        }
                        else if (finalSubmissionDates != null && finalSubmissionDates.Count() > 0)
                        {
                            var submission = finalSubmissionDates.FirstOrDefault();
                            res.AttendanceRateTermName = submission.Term.TermName;
                            res.CurrentTermId = submission.Term.ID;

                            res.AttenddanceRatePercent = GetAttendanceRateForDashboard(submission.Term.ID, studentID);
                        }
                    }
                    catch (Exception)
                    {

                    }
                    return res;
                }
            });
        }


    }
}