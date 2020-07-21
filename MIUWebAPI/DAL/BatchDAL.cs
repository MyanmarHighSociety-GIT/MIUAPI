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
    public class BatchDAL
    {
        public Task<List<BatchInfo>> GetBatchByCourseID(int courseID)
        {

            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {                   
                    List<BatchInfo> infoList = new List<BatchInfo>();
                    List<Batch> dataList = db.Batches.Where(a => a.CourseID == courseID && a.IsDelete != true).ToList();
                    foreach (var data in dataList)
                    {
                        BatchInfo info = new BatchInfo();
                        PropertyCopier<Batch, BatchInfo>.Copy(data, info);                       
                        infoList.Add(info);
                    }
                    return infoList;
                }
            });

        }

        public Task<List<BatchInfo>> GetBatchByCourseIDForLecturer(int courseID, int LecturerID)
        {

            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<BatchInfo> infoList = new List<BatchInfo>();

                    infoList = (

                    from td in db.TermDetails
                    join t in db.Terms
                    on td.TermID equals t.ID
                    join b in db.Batches
                    on t.BatchID equals b.ID
                    join c in db.Courses
                    on b.CourseID equals c.ID

                    where td.LectureID == LecturerID &&
                          b.CourseID == courseID

                    select new BatchInfo
                    {
                        ID = b.ID,
                        BatchCode = b.BatchCode,
                        BatchName = b.BatchName
                    }

                    ).ToList();

                    infoList = (

                    from info in infoList
                    group info by new
                    {
                        info.ID,
                        info.BatchCode,
                        info.BatchName

                    }
                    into g
                    select new BatchInfo
                    {
                        ID = g.Key.ID,
                        BatchCode = g.Key.BatchCode,
                        BatchName = g.Key.BatchName
                    }
                    ).ToList();

                    return infoList;
                }
            });

        }       
    }
}