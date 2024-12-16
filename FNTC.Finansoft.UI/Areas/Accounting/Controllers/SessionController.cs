/*
 Cachea los parametros requeridos para alivianar la App
 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Accounting.Controllers
{
    public class SessionController : Controller
    {
        // GET: Accounting/Session
        public ActionResult Index()
        {
            return View();
        }
    }
}