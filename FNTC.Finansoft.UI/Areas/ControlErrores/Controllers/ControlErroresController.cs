using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.ControlErrores.Controllers
{
    public class ControlErroresController : Controller
    {
        // GET: Error404
        public ActionResult Error404()
        {
            return View();
        }
        public ActionResult Error401()
        {
            return View();
        }
        public ActionResult Error500()
        {
            return View();
        }
        public ActionResult Error502()
        {
            return View();
        }
        public ActionResult Error504()
        {
            return View();
        }

        // GET: ErrorGeneral
        public ActionResult ErrorGeneral()
        {
            return View();
        }

    }
}






    
       

