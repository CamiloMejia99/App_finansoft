using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Dashboard.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Dashboard/Default

        public ActionResult Index()
        {
            var manager = new UserManager<ApplicationUser>(new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(new ApplicationDbContext()));
            var currentUser = manager.FindById(User.Identity.GetUserId());
            string usuario = User.Identity.Name;
            return View();
        }

        [Authorize(Roles = "Contabilidad, Admin")]
        public ActionResult catalogos(string titulo, string menu)
        {

            ViewBag.titulo = titulo;
            ViewBag.menu = menu;
            TempData["menu"] = menu;
            return View();
        }

        public ActionResult catalogos2(string url, string menu)
        {

            // var _url = "~/" + url;
            var area = url.Split('/')[0];
            var action = url.Split('/')[2];
            var controller = url.Split('/')[1];

            TempData["area"] = area;
            TempData["menu"] = menu;
            // ViewBag.action = action;
            // ViewBag.controller = controller;

            //return PartialView("~/Areas/Accounting/Views/CentroCosto/Create.cshtml");
            return RedirectToAction(action, controller, new { Area = area });
            //  return PartialView("~/Views/shared/Error.cshtml");
        }

        [Authorize(Roles = "Modulos, Admin")]
        public ActionResult descuentosNomina(string titulo, string menu)
        {

            ViewBag.titulo = titulo;
            ViewBag.menu = menu;
            TempData["menu"] = menu;
            return View();
        }
    }
}