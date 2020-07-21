using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MIUWebAPI.ViewModels
{
    public class StudyResourceInfo
    {
        public class ReturnStudyResource
        {
            public LearningOutcomeInfo LearningOutcomeInfo { get; set; }
            public StudyResourceInfoList StudyResourceInfo { get; set; }
            public List<StudyResourceDetailInfo> ModuleMaterials { get; set; }
            public List<RecommendedEbookInfo> RecommendedEbookInfo { get; set; }
        }

        public class LearningOutcomeInfo
        {
            public List<LearningOutcomeInfoList> LearningOutcomeInfoList { get; set; }
        }

        public class LearningOutcomeInfoList
        {

            public int ID { get; set; }
            public Nullable<int> ModuleID { get; set; }
            public string NumericFormat { get; set; }
            public string Description { get; set; }
            public Nullable<int> OrderID { get; set; }

            public List<AssignmentCriteriaInfo> AssignmentCriteriaInfo { get; set; }

            public List<AssignmentCriteriaDetailInfo> AssignmentCriteriaDetailInfo { get; set; }
        }
        
        public class AssignmentCriteriaInfo
        {
            public int ID { get; set; }
            public Nullable<int> LearningOutcomeID { get; set; }
            public string NumericFormat { get; set; }
            public string Description { get; set; }
            public Nullable<int> Grading { get; set; }
            public Nullable<int> OrderID { get; set; }
        }

        public class AssignmentCriteriaDetailInfo
        {
            public int ID { get; set; }
            public Nullable<int> AssignmentCriteriaID { get; set; }
            public string Description { get; set; }
            public Nullable<int> OrderID { get; set; }
            public string Negative { get; set; }
            public string Positive { get; set; }
        }

        public class StudyResourceInfoList
        {
            public int ID { get; set; }
            public Nullable<int> CourseID { get; set; }
            public Nullable<int> ModuleID { get; set; }
            public string Introduction { get; set; }
            public string Reference { get; set; }
        }

        public class StudyResourceDetailInfo
        {
            public int ID { get; set; }
            public Nullable<int> StudyResourceID { get; set; }
            public string Name { get; set; }
            public Nullable<int> IsFolder { get; set; }
            public Nullable<int> ParentID { get; set; }
            public Nullable<System.DateTime> ModifiedDate { get; set; }
            public string Remark { get; set; }
            public Nullable<int> Type { get; set; }
            public string FileSize { get; set; }
        }

        public class RecommendedEbookInfo
        {
            public int ID { get; set; }
            public Nullable<int> StudyResourceID { get; set; }
            public string Name { get; set; }
            public Nullable<int> IsFolder { get; set; }
            public Nullable<int> ParentID { get; set; }
            public string Remark { get; set; }
            public Nullable<int> Type { get; set; }
            public Nullable<System.DateTime> ModifiedDate { get; set; }
            public string FileSize { get; set; }
        }

    }
}