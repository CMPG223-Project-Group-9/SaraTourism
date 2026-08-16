using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using SaraTourism.DAL;
using SaraTourism.Filters;
using SaraTourism.Models.ViewModels;

namespace SaraTourism.Areas.Admin.Controllers
{
    [RequireAdmin]
    public class ReportController : Controller
    {
        private readonly ReportRepository _reportRepo = new ReportRepository();

        public ActionResult Index(DateTime? start, DateTime? end)
        {
            var model = new ReportViewModel
            {
                StartDate = start ?? DateTime.Today.AddMonths(-1),
                EndDate = end ?? DateTime.Today
            };

            model.ToursPerGuide = _reportRepo.ToursPerGuide(model.StartDate, model.EndDate);
            model.AvgReviewsPerWeek = _reportRepo.AvgReviewsPerWeek(model.StartDate, model.EndDate);
            model.RevenuePerActivity = _reportRepo.RevenuePerActivity(model.StartDate, model.EndDate);

            model.TotalTours = model.ToursPerGuide.Sum(r => r.NumberOfTours);
            model.TotalRevenue = model.RevenuePerActivity.Sum(r => r.TotalRevenue);
            model.AverageRatingOverall = model.AvgReviewsPerWeek.Count > 0
                ? Math.Round(model.AvgReviewsPerWeek.Average(r => r.AverageRating), 2)
                : 0;

            return View(model);
        }
    }
}