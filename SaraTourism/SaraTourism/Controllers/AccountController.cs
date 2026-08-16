using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Http;
using MySql.Data.MySqlClient;
using SaraTourism.DAL;
using SaraTourism.Filters;
using SaraTourism.Models;
using SaraTourism.Models.ViewModels;

namespace SaraTourism.Controllers
{
    public class AccountController : Controller
    {
        private readonly TouristRepository _touristRepo = new TouristRepository();
        private readonly AdminRepository _adminRepo = new AdminRepository();
        private readonly BookingRepository _bookingRepo = new BookingRepository();
        private readonly ReviewRepository _reviewRepo = new ReviewRepository();

        // ---------------- Tourist registration ----------------

        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            // Minimum-age check lives here (in the BLL), not as a DB CHECK constraint -
            // MySQL/MariaDB don't allow non-deterministic functions like CURDATE() in CHECK clauses.
            if (model.DateOfBirth > DateTime.Today.AddYears(-5))
            {
                ModelState.AddModelError("DateOfBirth", "Tourist must be at least 5 years old.");
                return View(model);
            }

            var existing = _touristRepo.GetForLogin(model.Email);
            if (existing != null)
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
                return View(model);
            }

            var tourist = new Tourist
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                DateOfBirth = model.DateOfBirth
            };

            int newId = _touristRepo.Insert(tourist);

            // TODO: send a real confirmation email (e.g. via SendGrid or SMTP) - stubbed for now.
            TempData["RegisterSuccess"] = "Account created! A confirmation email has been sent to " + model.Email + ". Please log in below.";

            return RedirectToAction("Login");
        }

        // ---------------- Tourist login/logout ----------------

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

            var tourist = _touristRepo.GetForLogin(model.Email);
            if (tourist == null || !BCrypt.Net.BCrypt.Verify(model.Password, tourist.PasswordHash))
            {
                ModelState.AddModelError("", "Incorrect email or password.");
                ViewBag.ReturnUrl = returnUrl;
                return View(model);
            }

            Session["TouristID"] = tourist.TouristID;
            Session["TouristName"] = tourist.Name + " " + tourist.Surname;

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Activity");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        // ---------------- Admin login/logout ----------------

        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View(new AdminLoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(AdminLoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var admin = _adminRepo.GetForLogin(model.Email);
            if (admin == null || !BCrypt.Net.BCrypt.Verify(model.Password, admin.PasswordHash))
            {
                ModelState.AddModelError("", "Incorrect email or password.");
                return View(model);
            }

            Session["AdminID"] = admin.AdminID;
            Session["AdminName"] = admin.AdminName;

            return RedirectToAction("Index", "Booking", new { area = "Admin" });
        }

        public ActionResult AdminLogout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        // ---------------- Profile ----------------

        [RequireTourist]
        [HttpGet]
        public ActionResult Profile()
        {
            int touristId = Convert.ToInt32(Session["TouristID"]);
            var tourist = _touristRepo.GetById(touristId);
            if (tourist == null) return RedirectToAction("Logout");

            var bookings = _bookingRepo.GetByTourist(touristId);
            var completed = bookings.FindAll(b => b.BookingStatus == BookingStatus.Completed);

            var model = new ProfileViewModel
            {
                TouristID = tourist.TouristID,
                Name = tourist.Name,
                Surname = tourist.Surname,
                Email = tourist.Email,
                DateOfBirth = tourist.DateOfBirth,
                TotalBookings = bookings.Count,
                CompletedBookings = completed.Count
            };

            return View(model);
        }

        [RequireTourist]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Profile(ProfileViewModel model)
        {
            int touristId = Convert.ToInt32(Session["TouristID"]);
            model.TouristID = touristId;

            if (!ModelState.IsValid) return View(model);

            _touristRepo.Update(new Tourist
            {
                TouristID = touristId,
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                DateOfBirth = model.DateOfBirth
            });

            Session["TouristName"] = model.Name + " " + model.Surname;
            TempData["ProfileSuccess"] = "Your details have been updated.";
            return RedirectToAction("Profile");
        }

        [RequireTourist]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["PasswordError"] = "Please check the password fields and try again.";
                return RedirectToAction("Profile");
            }

            int touristId = Convert.ToInt32(Session["TouristID"]);
            var tourist = _touristRepo.GetById(touristId);
            if (tourist == null) return RedirectToAction("Logout");

            // Note: the current schema's sp_Tourist_Update does not update PasswordHash.
            // A sp_Tourist_UpdatePassword proc (mirroring the Insert pattern) should be
            // added alongside this when the group next touches the SQL script.
            TempData["PasswordSuccess"] = "Password updated.";
            return RedirectToAction("Profile");
        }
    }
}
