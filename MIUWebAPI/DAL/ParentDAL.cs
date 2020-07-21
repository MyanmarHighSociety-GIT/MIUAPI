using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using MIUWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MIUWebAPI.DAL
{
    public class ParentDAL
    {
        public Task<List<Profile>> GetStudent(int UserID)
        {

            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<Profile> infoList = new List<Profile>();
                    try
                    {
                        List<User> dataList = db.ParentStudents.Where(x => x.ParentID == UserID).Select(x => x.User1).ToList();
                        foreach (var data in dataList)
                        {
                            Profile info = new Profile();
                            //PropertyCopier<User, Profile>.Copy(data, info);
                            //info.ProfilePicture = MIUFileServer.GetFileUrl("ProfileImages", info.ProfilePicture);
                            //info.IsFavorite = db.FavoriteLists.Any(a => a.FavoriteID == data.ID && a.UserID == userID && a.FavoriteType == "Announcement");

                            BatchInfo batchInfo = new BatchInfo();
                            CourseInfo courseInfo = new CourseInfo();
                            if (data != null)
                            {
                                PropertyCopier<User, Profile>.Copy(data, info);
                                info.ProfilePicture = MIUFileServer.GetFileUrl("ProfileImages", data.ProfilePicture);
                                Batch batch = db.BatchDetails.Where(x => x.StudentID == data.ID).Select(x => x.Batch).Where(x => x.IsDelete != true).FirstOrDefault();
                                if (batch != null)
                                {
                                    PropertyCopier<Batch, BatchInfo>.Copy(batch, batchInfo);
                                    Course course = db.Courses.Where(x => x.IsDelete != true && x.ID == batch.CourseID).SingleOrDefault();
                                    if (course != null)
                                    {
                                        PropertyCopier<Course, CourseInfo>.Copy(course, courseInfo);
                                    }
                                }

                                info.Dob = info.DOB == null ? "-" : info.DOB.Value.ToString("yyyy-MM-dd");

                                info.ApplicationDateString = info.ApplicationDate == null ? "-" : info.ApplicationDate.Value.ToString("yyyy-MM-dd");

                                info.Batch = batchInfo;
                                info.Course = courseInfo;
                                
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
    }
}