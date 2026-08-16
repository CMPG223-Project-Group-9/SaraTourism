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

namespace SaraTourism.Controllers
{
    [RequireTourist]
    public class BookingController : Controller
    {
        private readonly BookingRepository _bookingRepo = new BookingRepository();
        private readonly ReviewRepository _reviewRepo = new ReviewRepository();

        // GET /Booking/MyBookings
        public ActionResult MyBookings()
        {
            int touristId = Convert.ToInt32(Session["TouristID"]);
            var bookings = _bookingRepo.GetByTourist(touristId);
            return View(bookings);
        }

        // POST /Booking/SubmitReview - called from the review pop-up on My Bookings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitReview(int bookingId, int rating, string comment)
        {
            try
            {
                _reviewRepo.Insert(bookingId, rating, comment);
                TempData["ReviewSuccess"] = "Thanks for your review!";
            }
            catch (MySqlException ex) when (ex.Number == 1644) // SIGNAL SQLSTATE '45000' from sp_TourReview_Insert
            {
                TempData["ReviewError"] = "This booking isn't marked as completed yet, so it can't be reviewed.";
            }

            return RedirectToAction("MyBookings");
        }
    }
}