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
    
    public partial class rs_company
    {
        public int CompanyId { get; set; }
        public string CompanyCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string FaxNumber { get; set; }
        public string CompanyUrl { get; set; }
        public bool FlagActive { get; set; }
        public string UserEntry { get; set; }
        public Nullable<System.DateTime> DateEntry { get; set; }
        public string UserUpdate { get; set; }
        public Nullable<System.DateTime> DateUpdate { get; set; }
    }
}
