using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using SaraTourism.DAL;
using SaraTourism.Filters;

namespace SaraTourism.Areas.Admin.Controllers
{
    [RequireAdmin]
    public class TouristController : Controller
    {
        private readonly TouristRepository _touristRepo = new TouristRepository();
        private readonly BookingRepository _bookingRepo = new BookingRepository();

        public ActionResult Index()
        {
            var tourists = _touristRepo.GetAll();
            // Booking count per tourist for the admin table
            ViewBag.BookingCounts = new System.Collections.Generic.Dictionary<int, int>();
            foreach (var t in tourists)
            {
                var count = _bookingRepo.GetByTourist(t.TouristID).Count;
                ((System.Collections.Generic.Dictionary<int, int>)ViewBag.BookingCounts)[t.TouristID] = count;
            }
            return View(tourists);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deactivate(int id)
        {
            _touristRepo.Deactivate(id);
            TempData["TouristSuccess"] = "Tourist account deactivated.";
            return RedirectToAction("Index");
        }
    }
}