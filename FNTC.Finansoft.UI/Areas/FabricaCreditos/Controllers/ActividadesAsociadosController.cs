using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.FabricaCreditos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.FabricaCreditos.Controllers
{
    //prueba de git
    public class ActividadesAsociadosController : Controller
    {
        private AccountingContext db = new AccountingContext();
        // GET: FabricaCreditos/ActividadesAsociados
        public ActionResult Index()
        {
            return View(db.FCActividades.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([System.Web.Http.FromBody] FCActividades FCActividades)
        {
            if (ModelState.IsValid)
            {
                db.FCActividades.Add(FCActividades);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(FCActividades);
        }

        public ActionResult Edit(int id)
        {
            FCActividades ep = db.FCActividades.Find(id);
            return View(ep);
        }

        [HttpPost]
        public ActionResult Edit([System.Web.Http.FromBody] FCActividades FCActividades)
        {
            if (ModelState.IsValid)
            {
                db.Entry(FCActividades).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(FCActividades);
        }
    }
}