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
    public class MotivosDevolucionController : Controller
    {
        private AccountingContext db = new AccountingContext();
        // GET: FabricaCreditos/MotivosDevolucion
        public ActionResult Index()
        {
            return View(db.FCMotivosDevolucion.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([System.Web.Http.FromBody] FCMotivosDevolucion FCMotivosDevolucion)
        {
            if (ModelState.IsValid)
            {
                db.FCMotivosDevolucion.Add(FCMotivosDevolucion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(FCMotivosDevolucion);

        }

        public ActionResult Edit(int id)
        {
            FCMotivosDevolucion ep = db.FCMotivosDevolucion.Find(id);
            return View(ep);
        }

        [HttpPost]
        public ActionResult Edit([System.Web.Http.FromBody] FCMotivosDevolucion FCMotivosDevolucion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(FCMotivosDevolucion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(FCMotivosDevolucion);
        }
    }
}