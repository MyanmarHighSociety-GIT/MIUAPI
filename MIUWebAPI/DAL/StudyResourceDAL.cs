using MIUWebAPI.DBContext;
using MIUWebAPI.Helper;
using MIUWebAPI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using static MIUWebAPI.ViewModels.StudyResourceInfo;

namespace MIUWebAPI.DAL
{
    public class StudyResourceDAL
    {
        public Task<ReturnStudyResource> GetStudyResource(int moduleID,string Sorting)
        {
            return Task.Run(() =>
            {
                using (MIUEntities db = new MIUEntities())
                {
                    
                    ReturnStudyResource StudyResource = new ReturnStudyResource();
                    List<RecommendedEbookInfo> BookInfoList = new List<RecommendedEbookInfo>();
                    List<StudyResourceDetailInfo> SRInfoList = new List<StudyResourceDetailInfo>();
                    StudyResourceInfoList studyResourceInfoList = new StudyResourceInfoList();
                    try
                    {
                        #region LearningOutcome

                        LearningOutcomeInfo learningOutcomeInfo = new LearningOutcomeInfo();

                        List<LearningOutcome> lcs = db.LearningOutcomes.Where(x => x.ModuleID == moduleID).ToList();

                        List<LearningOutcomeInfoList> LearningOutcomeInfoList = new List<LearningOutcomeInfoList>();

                        List<AssignmentCriteriaInfo> AssignmentCriterias = new List<AssignmentCriteriaInfo>();

                        List<AssignmentCriteriaDetailInfo> AssignmentCriteriaDetails = new List<AssignmentCriteriaDetailInfo>();
                        foreach (var lc in lcs)
                        {
                            LearningOutcomeInfoList LearningOutcome = new LearningOutcomeInfoList();
                            LearningOutcome.AssignmentCriteriaInfo = new List<AssignmentCriteriaInfo>();
                            LearningOutcome.AssignmentCriteriaDetailInfo = new List<AssignmentCriteriaDetailInfo>();
                            PropertyCopier<LearningOutcome, LearningOutcomeInfoList>.Copy(lc, LearningOutcome);

                            foreach (var ac in lc.AssignmentCriterias)
                            {
                                if(lc.ID == ac.LearningOutcomeID)
                                {
                                    AssignmentCriteriaInfo AssignmentCriteriaInfo = new AssignmentCriteriaInfo();
                                    PropertyCopier<AssignmentCriteria, AssignmentCriteriaInfo>.Copy(ac, AssignmentCriteriaInfo);
                                    LearningOutcome.AssignmentCriteriaInfo.Add(AssignmentCriteriaInfo);
                                }
                                

                                foreach (var acd in ac.AssignmentCriteriaDetails)
                                {

                                    if(acd.AssignmentCriteriaID == ac.ID)
                                    {
                                        AssignmentCriteriaDetailInfo AssignmentCriteriaDetailInfo = new AssignmentCriteriaDetailInfo();
                                        PropertyCopier<AssignmentCriteriaDetail, AssignmentCriteriaDetailInfo>.Copy(acd, AssignmentCriteriaDetailInfo);
                                        LearningOutcome.AssignmentCriteriaDetailInfo.Add(AssignmentCriteriaDetailInfo);
                                    }
                                    
                                }
                            }
                            LearningOutcomeInfoList.Add(LearningOutcome);
                        }

                        learningOutcomeInfo.LearningOutcomeInfoList = LearningOutcomeInfoList;
                        
                        #endregion

                        var items = db.StudyResources.Where(s => s.ModuleID == moduleID);
                        if (items != null && items.Count() > 0)
                        {
                            StudyResource studyResource = new StudyResource();
                            studyResource = items.First();
                            PropertyCopier<StudyResource, StudyResourceInfoList>.Copy(studyResource, studyResourceInfoList);

                            List<StudyResourceDetail> StudyResourceDetails = studyResource.StudyResourceDetails.ToList();
                            foreach (var StudyResourceDetail in StudyResourceDetails)
                            {
                                StudyResourceDetailInfo srinfo = new StudyResourceDetailInfo();
                                PropertyCopier<StudyResourceDetail, StudyResourceDetailInfo>.Copy(StudyResourceDetail, srinfo);
                                srinfo.Name = MIUFileServer.GetFileUrl("Resource/"+ moduleID, srinfo.Name);
                                SRInfoList.Add(srinfo);
                            }

                            List<RecommendedEbook> recommendedEbooks = studyResource.RecommendedEbooks.ToList();
                            foreach (var recommendedEbook in recommendedEbooks)
                            {
                                RecommendedEbookInfo bookinfo = new RecommendedEbookInfo();
                                PropertyCopier<RecommendedEbook, RecommendedEbookInfo>.Copy(recommendedEbook, bookinfo);
                                bookinfo.Name = MIUFileServer.GetFileUrl("Resource/"+ moduleID, bookinfo.Name);
                                BookInfoList.Add(bookinfo);
                            }
                        }
                        StudyResource.LearningOutcomeInfo = learningOutcomeInfo;
                       
                        switch (Sorting)
                        {
                            case "all": StudyResource.RecommendedEbookInfo = BookInfoList.OrderByDescending(a => a.ModifiedDate).ToList(); break;
                            case "ascending": StudyResource.RecommendedEbookInfo = BookInfoList.OrderBy(a => a.Name).ToList(); break;
                            case "descending": StudyResource.RecommendedEbookInfo = BookInfoList.OrderByDescending(a => a.Name).ToList(); break;
                        }

                        switch (Sorting)
                        {
                            case "all": StudyResource.ModuleMaterials = SRInfoList.OrderByDescending(a=>a.ModifiedDate).ToList();break;
                            case "ascending": StudyResource.ModuleMaterials = SRInfoList.OrderBy(a=>a.Name).ToList(); break;
                            case "descending": StudyResource.ModuleMaterials = SRInfoList.OrderByDescending(a => a.Name).ToList(); break;
                        }
                        
                        StudyResource.StudyResourceInfo.Reference = studyResourceInfoList.Reference;
                        return StudyResource;
                    }
                    catch (Exception)
                    {
                        return StudyResource;
                    }
                }
            });
        }
    }
}