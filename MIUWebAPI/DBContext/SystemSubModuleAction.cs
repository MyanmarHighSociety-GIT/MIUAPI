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
    
    public partial class SystemSubModuleAction
    {
        public int Id { get; set; }
        public int SystemSubModuleID { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
    
        public virtual SystemSubModule SystemSubModule { get; set; }
    }
}