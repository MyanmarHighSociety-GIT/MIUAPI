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
    
    public partial class SystemSubModule
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SystemSubModule()
        {
            this.SystemModulePermissions = new HashSet<SystemModulePermission>();
            this.SystemSubModuleActions = new HashSet<SystemSubModuleAction>();
        }
    
        public int Id { get; set; }
        public int MainModuleId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    
        public virtual SystemModule SystemModule { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SystemModulePermission> SystemModulePermissions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SystemSubModuleAction> SystemSubModuleActions { get; set; }
    }
}
