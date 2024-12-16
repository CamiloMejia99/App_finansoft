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
    public class CentralRiesgoController : Controller
    {
        AccountingContext db = new AccountingContext();
        // GET: FabricaCreditos/CentralRiesgo
        public ActionResult Index()
        {

            return View(db.CentralRiesgo.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([System.Web.Http.FromBody] CentralRiesgo CentralRiesgo)
        {
            if (ModelState.IsValid)
            {
                db.CentralRiesgo.Add(CentralRiesgo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(CentralRiesgo);
        }

        public ActionResult Edit(int id)
        {
            CentralRiesgo ep = db.CentralRiesgo.Find(id);
            return View(ep);
        }

        [HttpPost]
        public ActionResult Edit([System.Web.Http.FromBody] CentralRiesgo CentralRiesgo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(CentralRiesgo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(CentralRiesgo);
        }
    }
}