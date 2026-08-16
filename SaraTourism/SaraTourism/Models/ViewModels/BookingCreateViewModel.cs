using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.Models.ViewModels
{
    public class BookingCreateViewModel
    {
        public int ActivityID { get; set; }
        public Activity Activity { get; set; }

        [Required(ErrorMessage = "Please choose a date.")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

        [Required(ErrorMessage = "Please enter the number of people.")]
        [Range(1, 100, ErrorMessage = "Number of people must be at least 1.")]
        [Display(Name = "Number of people")]
        public int NumberOfPeople { get; set; }

        [Required(ErrorMessage = "Please enter a contact email.")]
        [EmailAddress]
        [Display(Name = "Contact email")]
        public string ContactEmail { get; set; }

        public decimal EstimatedTotal
        {
            get { return Activity != null ? Activity.PricePerPerson * NumberOfPeople : 0; }
        }
    }
}