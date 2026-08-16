using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.Models.ViewModels
{
    public class ToursPerGuideRow
    {
        public int TourGuideID { get; set; }
        public string GuideName { get; set; }
        public int NumberOfTours { get; set; }
    }

    public class AvgReviewsPerWeekRow
    {
        public long YearWeek { get; set; }
        public DateTime WeekStarting { get; set; }
        public decimal AverageRating { get; set; }
        public int NumberOfReviews { get; set; }
    }

    public class RevenuePerActivityRow
    {
        public int ActivityID { get; set; }
        public string ActivityName { get; set; }
        public int NumberOfBookings { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class ReportViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<ToursPerGuideRow> ToursPerGuide { get; set; }
        public List<AvgReviewsPerWeekRow> AvgReviewsPerWeek { get; set; }
        public List<RevenuePerActivityRow> RevenuePerActivity { get; set; }

        public int TotalTours { get; set; }
        public decimal AverageRatingOverall { get; set; }
        public decimal TotalRevenue { get; set; }

        public ReportViewModel()
        {
            StartDate = DateTime.Today.AddMonths(-1);
            EndDate = DateTime.Today;
            ToursPerGuide = new List<ToursPerGuideRow>();
            AvgReviewsPerWeek = new List<AvgReviewsPerWeekRow>();
            RevenuePerActivity = new List<RevenuePerActivityRow>();
        }
    }
}