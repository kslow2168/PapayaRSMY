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
    
    public partial class rs_accessories
    {
        public int AccId { get; set; }
        public int AssetId { get; set; }
        public string Accessories { get; set; }
        public string Description { get; set; }
        public string Remarks { get; set; }
    
        public virtual rs_assets rs_assets { get; set; }
    }
}
