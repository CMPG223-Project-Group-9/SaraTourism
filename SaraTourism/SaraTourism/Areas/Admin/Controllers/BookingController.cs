using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using SaraTourism.DAL;
using SaraTourism.Filters;
using SaraTourism.Models;
using SaraTourism.Models.ViewModels;

namespace SaraTourism.Areas.Admin.Controllers
{
    [RequireAdmin]
    public class BookingController : Controller
    {
        private readonly BookingRepository _bookingRepo = new BookingRepository();
        private readonly TourGuideRepository _guideRepo = new TourGuideRepository();

        // GET /Admin/Booking - lists every booking in the system
        public ActionResult Index(string status)
        {
            var bookings = _bookingRepo.GetAll();

            if (!string.IsNullOrEmpty(status))
            {
                BookingStatus parsed;
                if (Enum.TryParse(status, true, out parsed))
                    bookings = bookings.FindAll(b => b.BookingStatus == parsed);
            }

            ViewBag.StatusFilter = status;
            return View(bookings);
        }

        // GET /Admin/Booking/Allocate/5 - shows the "allocate guide" panel for one booking
        public ActionResult Allocate(int id)
        {
            var booking = _bookingRepo.GetById(id);
            if (booking == null) return HttpNotFound();

            var model = new AllocateGuideViewModel
            {
                BookingID = booking.BookingID,
                Booking = booking,
                AvailableGuides = _guideRepo.GetAvailable(booking.BookingDate, booking.StartTime, booking.EndTime)
            };
            return View(model);
        }

        // POST /Admin/Booking/Allocate - confirms the guide allocation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Allocate(AllocateGuideViewModel model)
        {
            int adminId = Convert.ToInt32(Session["AdminID"]);
            _bookingRepo.AllocateGuide(model.BookingID, model.SelectedTourGuideID, adminId);
            TempData["BookingActionSuccess"] = "Tour guide allocated. Booking is now Confirmed.";
            return RedirectToAction("Index");
        }

        // POST /Admin/Booking/ConfirmArrival/5 - the tourist showed up; unlocks their review
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmArrival(int id)
        {
            _bookingRepo.ConfirmArrival(id);
            TempData["BookingActionSuccess"] = "Booking marked as Completed.";
            return RedirectToAction("Index");
        }

        // POST /Admin/Booking/Cancel/5 - no-show
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancel(int id)
        {
            _bookingRepo.Cancel(id);
            TempData["BookingActionSuccess"] = "Booking cancelled.";
            return RedirectToAction("Index");
        }
    }
}