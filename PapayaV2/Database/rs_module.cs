//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PapayaX2.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class rs_module
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rs_module()
        {
            this.rs_action = new HashSet<rs_action>();
            this.rs_module1 = new HashSet<rs_module>();
        }
    
        public int ModuleId { get; set; }
        public Nullable<int> ParentModuleId { get; set; }
        public string Name { get; set; }
        public string Controller { get; set; }
        public Nullable<bool> HaveChild { get; set; }
        public Nullable<int> MenuOrder { get; set; }
        public Nullable<int> ModuleOrder { get; set; }
        public bool IsBackEnd { get; set; }
        public bool FlagActive { get; set; }
        public string UserEntry { get; set; }
        public Nullable<System.DateTime> DateEntry { get; set; }
        public string UserUpdate { get; set; }
        public Nullable<System.DateTime> DateUpdate { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rs_action> rs_action { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rs_module> rs_module1 { get; set; }
        public virtual rs_module rs_module2 { get; set; }
    }
}
