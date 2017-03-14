using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PapayaX2.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "* A value is required.")]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "* A value is required.")]
        [Display(Name = "New Password")]
        [MinLength(5, ErrorMessage = "* Value cannot be less than 5 characters.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "* A value is required.")]
        [Display(Name = "Confirm Password")]
        [MinLength(5, ErrorMessage = "* Value cannot be less than 5 characters.")]
        [Compare("NewPassword", ErrorMessage = "* The new password and confirmation password do not match.")]
        public string ConfPassword { get; set; }
    }
}