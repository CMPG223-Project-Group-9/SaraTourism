using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SaraTourism.DAL;


namespace SaraTourism.Controllers
{
    public class HomeController : Controller
    {
        private readonly ActivityRepository _activityRepo = new ActivityRepository();

        public ActionResult Index()
        {
            // "Popular activities" on the homepage = top 3 by popularity
            var popular = _activityRepo.GetAllWithPopularity();
            if (popular.Count > 3) popular = popular.GetRange(0, 3);
            return View(popular);
        }
    }
}