using FNTC.Finansoft.Accounting.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Controllers
{
    public class UsersAdminController : Controller
    {

        AccountingContext db = new AccountingContext();
        // GET: UsersAdmin
        [Authorize]
        public JsonResult ValidacionUser(string UserName)
        {
            return Json(!db.AspNetUsersApp.Any(lo => lo.UserName == UserName), JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public JsonResult ValidacionEmail(string Email)
        {
            return Json(!db.AspNetUsersApp.Any(lo => lo.Email == Email), JsonRequestBehavior.AllowGet);
        }
    }
}