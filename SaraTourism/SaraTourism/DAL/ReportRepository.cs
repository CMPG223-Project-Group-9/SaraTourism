using System;
using System.Data;
using SaraTourism.DAL;
using SaraTourism.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaraTourism.DAL
{
    public class ReportRepository
    {
        public List<ToursPerGuideRow> ToursPerGuide(DateTime start, DateTime end)
        {
            var table = DbHelper.ExecuteProcTable("sp_Report_ToursPerGuide",
                DbHelper.Param("p_StartDate", start.Date), DbHelper.Param("p_EndDate", end.Date));
            var list = new List<ToursPerGuideRow>();
            foreach (DataRow r in table.Rows)
            {
                list.Add(new ToursPerGuideRow
                {
                    TourGuideID = r.GetInt("TourGuideID"),
                    GuideName = r.GetStr("GuideName"),
                    NumberOfTours = r.GetInt("NumberOfTours")
                });
            }
            return list;
        }

        public List<AvgReviewsPerWeekRow> AvgReviewsPerWeek(DateTime start, DateTime end)
        {
            var table = DbHelper.ExecuteProcTable("sp_Report_AvgReviewsPerWeek",
                DbHelper.Param("p_StartDate", start.Date), DbHelper.Param("p_EndDate", end.Date));
            var list = new List<AvgReviewsPerWeekRow>();
            foreach (DataRow r in table.Rows)
            {
                list.Add(new AvgReviewsPerWeekRow
                {
                    YearWeek = r.GetLong("YearWeek"),
                    WeekStarting = r.GetDate("WeekStarting"),
                    AverageRating = r.GetDec("AverageRating"),
                    NumberOfReviews = r.GetInt("NumberOfReviews")
                });
            }
            return list;
        }

        public List<RevenuePerActivityRow> RevenuePerActivity(DateTime start, DateTime end)
        {
            var table = DbHelper.ExecuteProcTable("sp_Report_RevenuePerActivity",
                DbHelper.Param("p_StartDate", start.Date), DbHelper.Param("p_EndDate", end.Date));
            var list = new List<RevenuePerActivityRow>();
            foreach (DataRow r in table.Rows)
            {
                list.Add(new RevenuePerActivityRow
                {
                    ActivityID = r.GetInt("ActivityID"),
                    ActivityName = r.GetStr("ActivityName"),
                    NumberOfBookings = r.GetInt("NumberOfBookings"),
                    TotalRevenue = r.GetDec("TotalRevenue")
                });
            }
            return list;
        }
    }
}