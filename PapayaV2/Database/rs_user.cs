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
    
    public partial class rs_user
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rs_user()
        {
            this.rs_assets = new HashSet<rs_assets>();
            this.rs_bookings = new HashSet<rs_bookings>();
            this.rs_bookings1 = new HashSet<rs_bookings>();
            this.rs_bookings2 = new HashSet<rs_bookings>();
            this.rs_bookings3 = new HashSet<rs_bookings>();
            this.rs_bookings4 = new HashSet<rs_bookings>();
            this.rs_loan_form = new HashSet<rs_loan_form>();
            this.rs_loan_form1 = new HashSet<rs_loan_form>();
            this.rs_smslogs = new HashSet<rs_smslogs>();
        }
    
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public int EmployeeId { get; set; }
        public int GroupId { get; set; }
        public string UserType { get; set; }
        public bool IsBackEnd { get; set; }
        public bool FlagActive { get; set; }
        public string UserEntry { get; set; }
        public Nullable<System.DateTime> DateEntry { get; set; }
        public string UserUpdate { get; set; }
        public Nullable<System.DateTime> DateUpdate { get; set; }
        public int DivId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rs_assets> rs_assets { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rs_bookings> rs_bookings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rs_bookings> rs_bookings1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rs_bookings> rs_bookings2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rs_bookings> rs_bookings3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rs_bookings> rs_bookings4 { get; set; }
        public virtual rs_division rs_division { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rs_loan_form> rs_loan_form { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rs_loan_form> rs_loan_form1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rs_smslogs> rs_smslogs { get; set; }
        public virtual rs_user_group rs_user_group { get; set; }
    }
}
