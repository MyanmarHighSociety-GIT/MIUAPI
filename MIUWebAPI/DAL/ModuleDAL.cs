using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using MIUWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace MIUWebAPI.DAL
{
    public class ModuleDAL
    {
        public Task<List<ModuleInfo>> GetModuleList(int StudentID)
        {

            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    //List<ModuleInfo> dataList = (from m in db.Modules
                    //                             join td in db.TermDetails on m.ID equals td.ModuleID
                    //                             join t in db.Terms on td.TermID equals t.ID
                    //                             join b in db.Batches on t.BatchID equals b.ID
                    //                             join bd in db.BatchDetails on b.ID equals bd.BatchID
                    //                             where bd.StudentID == StudentID
                    //                             group m by m.ID into data
                    //                             select new ModuleInfo
                    //                             {
                    //                                 ID = data.FirstOrDefault().ID,
                    //                                 ModuleName = data.FirstOrDefault().ModuleName,
                    //                                 ModuleCode = data.FirstOrDefault().ModuleCode
                    //                             }).ToList();
                    //return dataList;
                    //copy from web
                    int id = db.BatchDetails.Where(x => x.StudentID == StudentID).FirstOrDefault().BatchID;
                    List<int> ids = db.TermDetails.Where(x => x.Term.BatchID == id).Select(x => x.ModuleID).ToList<int>(); // original
                    var batch = db.Batches.Where(x => x.ID == id).FirstOrDefault(); //edit by kzm 12 May
                    if (batch != null)
                    {
                        ids = db.TemplateDetailSubs.Where(x => x.TemplateDetail.Template.CourseID == batch.CourseID).Select(x => x.ModuleID).Distinct().ToList<int>(); //edit by kzm 12 May
                    }
                    var modules = db.Modules.Where(e => e.IsDelete != true && ids.Contains(e.ID)).OrderBy(m => m.ModuleName);
                    List<ModuleInfo> dataList = modules.Select(x =>

                    new ModuleInfo
                    {
                        ID = x.ID,
                        ModuleName = x.ModuleName,
                        ModuleCode = x.ModuleCode
                    }
                    ).ToList();

                    return dataList;
                }
            });

        }

        public Task<List<ModuleInfo>> GetAdminModuleList(int BatchID)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<ModuleInfo> dataList = new List<ModuleInfo>();

                    dataList = (from m in db.Modules
                                join td in db.TermDetails on m.ID equals td.ModuleID
                                join t in db.Terms on td.TermID equals t.ID
                                join b in db.Batches on t.BatchID equals b.ID
                                join bd in db.BatchDetails on b.ID equals bd.BatchID
                                where bd.BatchID == BatchID
                                group m by m.ID into data
                                select new ModuleInfo
                                {
                                    ID = data.FirstOrDefault().ID,
                                    ModuleName = data.FirstOrDefault().ModuleName,
                                    ModuleCode = data.FirstOrDefault().ModuleCode,

                                }).OrderBy(a => a.ModuleName).ToList();
                    return dataList;
                }
            });
        }

        public Task<List<ModuleReferenceInfo>> GetModuleReference(int ModuleID)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    List<ModuleReferenceInfo> data = new List<ModuleReferenceInfo>();

                    List<StudyResource> rec = db.StudyResources.Where(a => a.ModuleID == ModuleID && !string.IsNullOrEmpty(a.Reference)).ToList();
                    
                    foreach(StudyResource std in rec)
                    {
                        ModuleReferenceInfo info = new ModuleReferenceInfo()
                        {
                            Reference = std.Reference
                        };
                        data.Add(info);
                    }

                    return data;
                }
            });
        }
    }
}