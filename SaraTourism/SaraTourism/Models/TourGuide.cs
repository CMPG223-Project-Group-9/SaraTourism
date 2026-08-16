using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.Models
{
    public class TourGuide
    {
        public int TourGuideID { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public bool IsActive { get; set; }
    }
}