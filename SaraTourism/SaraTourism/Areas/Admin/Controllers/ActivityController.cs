using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web;
using System.Web.Mvc;
using SaraTourism.DAL;
using SaraTourism.Filters;
using SaraTourism.Models;

namespace SaraTourism.Areas.Admin.Controllers
{
    [RequireAdmin]
    public class ActivityController : Controller
    {
        private readonly ActivityRepository _activityRepo = new ActivityRepository();

        // GET /Admin/Activity - list with popularity, for "sort by popularity"
        public ActionResult Index(string sort)
        {
            var activities = _activityRepo.GetAllWithPopularity();

            switch (sort)
            {
                case "price":
                    activities.Sort((a, b) => a.PricePerPerson.CompareTo(b.PricePerPerson));
                    break;
                case "name":
                    activities.Sort((a, b) => string.Compare(a.ActivityName, b.ActivityName, StringComparison.OrdinalIgnoreCase));
                    break;
                default: // popularity (already the default order from the stored procedure)
                    break;
            }

            ViewBag.Sort = sort;
            return View(activities);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new Activity());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Activity model, HttpPostedFileBase photo)
        {
            if (!ModelState.IsValid) return View(model);

            model.ImagePath = SavePhotoIfProvided(photo);
            _activityRepo.Insert(model);

            TempData["ActivitySuccess"] = "Activity added.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var activity = _activityRepo.GetById(id);
            if (activity == null) return HttpNotFound();
            return View(activity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Activity model, HttpPostedFileBase photo)
        {
            if (!ModelState.IsValid) return View(model);

            var existing = _activityRepo.GetById(model.ActivityID);
            if (existing == null) return HttpNotFound();

            // Keep the existing photo unless a new one was uploaded
            model.ImagePath = SavePhotoIfProvided(photo) ?? existing.ImagePath;
            _activityRepo.Update(model);

            TempData["ActivitySuccess"] = "Activity updated.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            _activityRepo.Delete(id);
            TempData["ActivitySuccess"] = "Activity removed.";
            return RedirectToAction("Index");
        }

        /// <summary>Saves an uploaded activity photo under ~/Content/uploads/activities and returns its site-relative path, or null if no file was provided.</summary>
        private string SavePhotoIfProvided(HttpPostedFileBase photo)
        {
            if (photo == null || photo.ContentLength == 0) return null;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var ext = Path.GetExtension(photo.FileName).ToLowerInvariant();
            if (Array.IndexOf(allowedExtensions, ext) < 0)
            {
                ModelState.AddModelError("", "Photo must be a JPG or PNG file.");
                return null;
            }

            var uploadsFolder = Server.MapPath("~/Content/uploads/activities");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString("N") + ext;
            var fullPath = Path.Combine(uploadsFolder, fileName);
            photo.SaveAs(fullPath);

            return "/Content/uploads/activities/" + fileName;
        }
    }
}