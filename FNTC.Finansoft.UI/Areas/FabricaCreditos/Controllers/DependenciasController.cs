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
    public class DependenciasController : Controller
    {
        private AccountingContext db = new AccountingContext();
        // GET: FabricaCreditos/Dependencias
        public ActionResult Index()
        {
            return View(db.FCDependencias.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([System.Web.Http.FromBody] FCDependencias FCDependencias)
        {
            if (ModelState.IsValid)
            {
                db.FCDependencias.Add(FCDependencias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(FCDependencias);

        }

        public ActionResult Edit(int id)
        {
            FCDependencias ep = db.FCDependencias.Find(id);
            return View(ep);
        }

        [HttpPost]
        public ActionResult Edit([System.Web.Http.FromBody] FCDependencias FCDependencias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(FCDependencias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(FCDependencias);
        }
    }
}