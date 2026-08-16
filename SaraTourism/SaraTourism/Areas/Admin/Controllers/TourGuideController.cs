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

namespace SaraTourism.Areas.Admin.Controllers
{
    [RequireAdmin]
    public class TourGuideController : Controller
    {
        private readonly TourGuideRepository _guideRepo = new TourGuideRepository();

        public ActionResult Index()
        {
            return View(_guideRepo.GetAll());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new TourGuide());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TourGuide model)
        {
            if (!ModelState.IsValid) return View(model);
            _guideRepo.Insert(model);
            TempData["GuideSuccess"] = "Tour guide added.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var guide = _guideRepo.GetById(id);
            if (guide == null) return HttpNotFound();
            return View(guide);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TourGuide model)
        {
            if (!ModelState.IsValid) return View(model);
            _guideRepo.Update(model);
            TempData["GuideSuccess"] = "Tour guide updated.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            _guideRepo.Delete(id);
            TempData["GuideSuccess"] = "Tour guide removed.";
            return RedirectToAction("Index");
        }
    }
}