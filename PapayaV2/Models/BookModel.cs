using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapayaX2.Database;
using System.ComponentModel.DataAnnotations;

namespace PapayaX2.Models
{
    public class BookModel
    {
        public rs_assets Asset { get; set; }
        public rs_bookings booking { get; set; }
        
        public BookModel()
        {
            Asset = new rs_assets();
            booking = new rs_bookings();
        }
    }
    public class BookData
    {
        public rs_assets Asset { get; set; }

        public List<rs_bookings> Bookings { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExtendedDate { get; set; }

        public bool IsExtend { get; set; }

        public int AssetId { get; set; }

        public int BookId { get; set; }
        public int LoanLocationId { get; set; }

        [Required(ErrorMessage = "* A value is required.")]
        public string Remarks { get; set; }
        
        public bool IsBookOnBehalf { get; set; }

        public int ResquestorId { get; set; }

        [Required(ErrorMessage = "* A value is required.")]
        public string BookPurpose { get; set; }

        public string BookedDateStr { get; set; }

        public BookData()
        {
            Bookings = new List<rs_bookings>();
        }
    }

    public class SystemAsset
    {
        public rs_assets SubAsset { get; set; }

        public bool IsSelected { get; set; }
    }

    public class SystemBookData
    {
        public rs_assets System { get; set; }

        public List<SystemAsset> Assets { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ExtendedDate { get; set; }

        public bool IsExtend { get; set; }

        public int SystemId { get; set; }
        public int LoanLocationId { get; set; }

        [Required(ErrorMessage = "* A value is required.")]
        public string Remarks { get; set; }

        public bool IsBookOnBehalf { get; set; }

        public int ResquestorId { get; set; }

        [Required(ErrorMessage = "* A value is required.")]
        public string BookPurpose { get; set; }

        public string BookedDateStr { get; set; }

        public SystemBookData()
        {
            Assets = new List<SystemAsset>();
        }
    }


    public class BookSearchModel
    {

        public List<rs_bookings> Bookings { get; set; }

        public int AssetId { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int BookedBy { get; set; }

        public BookSearchModel()
        {
            Bookings = new List<rs_bookings>();
        }
    }
    
}