//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MIUWebAPI.DBContext
{
    using System;
    using System.Collections.Generic;
    
    public partial class AssignmentCriteria
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AssignmentCriteria()
        {
            this.AssignmentCriteriaDetails = new HashSet<AssignmentCriteriaDetail>();
        }
    
        public int ID { get; set; }
        public Nullable<int> LearningOutcomeID { get; set; }
        public string NumericFormat { get; set; }
        public string Description { get; set; }
        public Nullable<int> Grading { get; set; }
        public Nullable<int> OrderID { get; set; }
    
        public virtual LearningOutcome LearningOutcome { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssignmentCriteriaDetail> AssignmentCriteriaDetails { get; set; }
    }
}