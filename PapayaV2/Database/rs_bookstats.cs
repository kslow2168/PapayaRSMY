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
    
    public partial class rs_bookstats
    {
        public int StatsId { get; set; }
        public int DateId { get; set; }
        public int AssetId { get; set; }
        public string Actions { get; set; }
        public long BookingCounts { get; set; }
    
        public virtual rs_assets rs_assets { get; set; }
        public virtual rs_dates rs_dates { get; set; }
    }
}
