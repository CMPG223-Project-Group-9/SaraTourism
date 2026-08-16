using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using MySql.Data.MySqlClient;
using SaraTourism.DAL;
using SaraTourism.Filters;
using SaraTourism.Models;
using SaraTourism.Models.ViewModels;

namespace SaraTourism.Controllers
{
    [RequireTourist]
    public class ActivityController : Controller
    {
        private readonly ActivityRepository _activityRepo = new ActivityRepository();
        private readonly BookingRepository _bookingRepo = new BookingRepository();
        private readonly PaymentRepository _paymentRepo = new PaymentRepository();

        // GET /Activity - the full Activities (Services) listing page, with search
        public ActionResult Index(string q, string location)
        {
            var activities = _activityRepo.GetAll();

            if (!string.IsNullOrWhiteSpace(q))
                activities = activities.Where(a => a.ActivityName.IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0).ToList();

            if (!string.IsNullOrWhiteSpace(location))
                activities = activities.Where(a => a.Location == location).ToList();

            ViewBag.Query = q;
            ViewBag.Location = location;
            return View(activities);
        }

        // GET /Activity/Details/5 - activity details + booking sidebar
        public ActionResult Details(int id)
        {
            var activity = _activityRepo.GetById(id);
            if (activity == null) return HttpNotFound();

            var model = new BookingCreateViewModel
            {
                ActivityID = activity.ActivityID,
                Activity = activity,
                BookingDate = DateTime.Today.AddDays(1),
                NumberOfPeople = 1
            };
            return View(model);
        }

        // POST /Activity/Book - confirms a booking from the Details page sidebar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Book(BookingCreateViewModel model)
        {
            var activity = _activityRepo.GetById(model.ActivityID);
            if (activity == null) return HttpNotFound();
            model.Activity = activity;

            if (model.BookingDate.Date < DateTime.Today)
                ModelState.AddModelError("BookingDate", "Booking date cannot be in the past.");

            if (model.NumberOfPeople > activity.MaxCapacity)
                ModelState.AddModelError("NumberOfPeople", "Number of people exceeds this activity's maximum capacity.");

            if (!ModelState.IsValid)
                return View("Details", model);

            if (!_bookingRepo.CheckAvailability(activity.ActivityID, model.BookingDate, model.NumberOfPeople))
            {
                ModelState.AddModelError("NumberOfPeople", "Not enough spots left for this activity on the selected date.");
                return View("Details", model);
            }

            int touristId = Convert.ToInt32(Session["TouristID"]);
            var startTime = new TimeSpan(6, 0, 0); // default start time; a future iteration could let the tourist pick a slot
            var endTime = startTime.Add(TimeSpan.FromMinutes(activity.DurationMinutes));

            var booking = new Booking
            {
                TouristID = touristId,
                ActivityID = activity.ActivityID,
                TourGuideID = null, // guide is allocated later by an admin, not at booking time
                AdminID = null,
                BookingDate = model.BookingDate,
                StartTime = startTime,
                EndTime = endTime,
                NumberOfPeople = model.NumberOfPeople,
                TotalAmount = model.EstimatedTotal
            };

            int newBookingId = _bookingRepo.Insert(booking);

            // Simulated payment capture - a real gateway integration (Stripe/PayFast/etc.)
            // would replace this direct insert with a redirect to a hosted payment page.
            _paymentRepo.Insert(newBookingId, model.EstimatedTotal, PaymentMethod.Card);

            TempData["BookingSuccess"] = "Your booking is confirmed! We'll assign a tour guide shortly.";
            return RedirectToAction("MyBookings", "Booking");
        }
    }
}