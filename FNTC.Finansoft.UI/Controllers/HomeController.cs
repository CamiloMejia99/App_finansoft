using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {


            return RedirectToAction("Index", new { Controller = "Default", Area = "Dashboard" });
        }

        [HttpPost]
        public ActionResult Index(FormCollection col)
        {
            ViewBag.posted = col;
            return View();
        }


        public ActionResult Menu()
        {
            return PartialView();
        }
    }
}