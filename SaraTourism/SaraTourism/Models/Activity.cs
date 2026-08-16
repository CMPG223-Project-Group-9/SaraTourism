using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.Models
{
    public class Activity
    {
        public int ActivityID { get; set; }
        public string ActivityName { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public string Location { get; set; }
        public int DurationMinutes { get; set; }
        public decimal PricePerPerson { get; set; }
        public int MaxCapacity { get; set; }
        public bool IsActive { get; set; }

        // Populated only by sp_Activity_GetAllWithPopularity
        public int TimesBooked { get; set; }

        public string DurationDisplay
        {
            get
            {
                int hours = DurationMinutes / 60;
                int mins = DurationMinutes % 60;
                if (hours > 0 && mins > 0) return string.Format("{0}h {1}min", hours, mins);
                if (hours > 0) return string.Format("{0} hour{1}", hours, hours > 1 ? "s" : "");
                return string.Format("{0} min", mins);
            }
        }
    }
}