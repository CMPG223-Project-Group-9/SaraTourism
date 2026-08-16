using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.Models
{
    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Completed,
        Cancelled
    }

    public class Booking
    {
        public int BookingID { get; set; }
        public int TouristID { get; set; }
        public int ActivityID { get; set; }
        public int? TourGuideID { get; set; }
        public int? AdminID { get; set; }
        public DateTime BookingDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int NumberOfPeople { get; set; }
        public decimal TotalAmount { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public DateTime CreatedDate { get; set; }

        // Populated by joined queries (sp_Booking_GetAll / GetById / GetByTourist)
        public string TouristName { get; set; }
        public string TouristSurname { get; set; }
        public string ActivityName { get; set; }
        public string GuideName { get; set; }

        public string StatusLabel { get { return BookingStatus.ToString(); } }
    }
}