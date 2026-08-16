using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.Models
{
    public class TourReview
    {
        public int ReviewID { get; set; }
        public int BookingID { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }

        // Populated by sp_TourReview_GetAll
        public DateTime BookingDate { get; set; }
        public string ActivityName { get; set; }
        public string GuideName { get; set; }
    }
}
}