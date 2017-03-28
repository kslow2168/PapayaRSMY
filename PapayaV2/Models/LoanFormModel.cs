using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PapayaX2.Database;
using System.ComponentModel.DataAnnotations;

namespace PapayaX2.Models
{
    public class LoanFormViewModel
    {
        public rs_loan_form LoanForm { get; set; }
        public IEnumerable<rs_bookings> AvailableBookings { get; set; }
        public IEnumerable<rs_bookings> SelectedBookings { get; set; }
        public PostedBookings Bookings { get; set; }
    }

    public class PostedBookings
    {
        public string[] BookingIds { get; set; }
    }


    public class LoanFormSearchModel
    {

        public List<rs_loan_form> Loans { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int BookedBy { get; set; }
        public int LoanId { get; set; }
        public LoanFormSearchModel()
        {
            Loans = new List<rs_loan_form>();
        }
    }
}