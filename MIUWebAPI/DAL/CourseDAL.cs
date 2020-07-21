using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using MIUWebAPI.Models;
using MIUWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MIUWebAPI.DAL
{
    public class CourseDAL
    {
        public Task<List<CourseInfo>> GetCourseList()
        {

            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {                   
                    List<CourseInfo> infoList = new List<CourseInfo>();
                    List<Course> dataList = db.Courses.Where(a => a.IsDelete != true).ToList();
                    foreach (var data in dataList)
                    {
                        CourseInfo info = new CourseInfo();
                        PropertyCopier<Course, CourseInfo>.Copy(data, info);                       
                        infoList.Add(info);
                    }
                    return infoList;
                }
            });

        }

        public Task<List<CourseInfo>> GetCourseListForLecturer(int LecturerID)
        {

            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<CourseInfo> infoList = new List<CourseInfo>();
                    //List<User> stdList = new List<User>();
                    try
                    {
                        var course = (from td in db.TermDetails
                                      join t in db.Terms on td.TermID equals t.ID
                                      join b in db.Batches on t.BatchID equals b.ID
                                      join c in db.Courses on b.CourseID equals c.ID
                                      where td.LectureID == LecturerID
                                      //&& b.IsDelete != true
                                      //&& c.IsDelete != true
                                      //&& DateTime.Now >= t.StartDate && DateTime.Now <= t.CompletionDate
                                      select c).Distinct();

                        foreach (var data in course)
                        {
                            CourseInfo info = new CourseInfo();
                            PropertyCopier<Course, CourseInfo>.Copy(data, info);
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


        public Task<List<CourseInfo>> GetCourseListForAssessor(int AssessorID)
        {

            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<CourseInfo> infoList = new List<CourseInfo>();
                    //List<User> stdList = new List<User>();
                    try
                    {
                        var course = (from td in db.TermDetails
                                      join t in db.Terms on td.TermID equals t.ID
                                      join b in db.Batches on t.BatchID equals b.ID
                                      join c in db.Courses on b.CourseID equals c.ID
                                      where td.AccessorID == AssessorID
                                      && b.IsDelete != true
                                      && c.IsDelete != true
                                      //&& DateTime.Now >= t.StartDate && DateTime.Now <= t.CompletionDate
                                      select c).Distinct();

                        foreach (var data in course)
                        {
                            CourseInfo info = new CourseInfo();
                            PropertyCopier<Course, CourseInfo>.Copy(data, info);

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
    }
}