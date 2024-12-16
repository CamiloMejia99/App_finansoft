using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Tools
{
    public class PrintingController : Controller
    {
        // GET: Tools/Printing
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult PrintAsPDF(Action act)
        {
            return View();
        }
    }
}