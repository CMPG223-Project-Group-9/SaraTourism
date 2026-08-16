using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.Models.ViewModels
{
    public class AllocateGuideViewModel
    {
        public int BookingID { get; set; }
        public Booking Booking { get; set; }
        public List<TourGuide> AvailableGuides { get; set; }
        public int SelectedTourGuideID { get; set; }

        public AllocateGuideViewModel()
        {
            AvailableGuides = new List<TourGuide>();
        }
    }
}