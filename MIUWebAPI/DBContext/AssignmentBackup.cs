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
    
    public partial class AssignmentBackup
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AssignmentBackup()
        {
            this.AssignmentBackupDetails = new HashSet<AssignmentBackupDetail>();
        }
    
        public long Id { get; set; }
        public Nullable<System.DateTime> BackupDate { get; set; }
        public string StartTime { get; set; }
        public string Duration { get; set; }
        public string CreatedBy { get; set; }
        public string Status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AssignmentBackupDetail> AssignmentBackupDetails { get; set; }
    }
}