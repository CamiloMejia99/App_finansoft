using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Email;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.parametros.Controllers
{
    public class ParametrosEmailController : Controller
    {
        private AccountingContext db = new AccountingContext();
        // GET: parametros/ParametrosEmail
        public ActionResult Index()
        {
            return View(db.ConfiguracionCorreo.ToList());
        }

        public ActionResult CrearEmail()
        {

            return View();
        }
        [HttpPost]
        public ActionResult CrearEmail([System.Web.Http.FromBody] ConfiguracionCorreo configuracionCorreo)
        {
            var estadoEmail = configuracionCorreo.estado;
            if (estadoEmail == "1")
            {
                db.Database.ExecuteSqlCommand("update ConfiguracionCorreo set Estado = 0 where Estado = 1");
                db.ConfiguracionCorreo.Add(configuracionCorreo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                db.ConfiguracionCorreo.Add(configuracionCorreo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }

        public ActionResult EditarEmail(int id)
        {
            ConfiguracionCorreo ep = db.ConfiguracionCorreo.Find(id);
            return View(ep);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarEmail([System.Web.Http.FromBody] ConfiguracionCorreo configuracionCorreo)
        {
            var estadoEmail = configuracionCorreo.estado;
            if (estadoEmail == "1")
            {
                db.Database.ExecuteSqlCommand("update ConfiguracionCorreo set Estado = 0 where Estado = 1");
                db.Entry(configuracionCorreo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                db.Entry(configuracionCorreo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

    }
}