using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.Models.ViewModels
{
    public class ProfileViewModel
    {
        public int TouristID { get; set; }

        [Required(ErrorMessage = "Please enter your name.")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your surname.")]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Please enter your email address.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of birth")]
        public DateTime DateOfBirth { get; set; }

        // Profile summary stats, not editable
        public int TotalBookings { get; set; }
        public int CompletedBookings { get; set; }
        public double AverageRatingGiven { get; set; }

    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Please enter a new password.")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please confirm your new password.")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm new password")]
        public string ConfirmNewPassword { get; set; }
    }

}