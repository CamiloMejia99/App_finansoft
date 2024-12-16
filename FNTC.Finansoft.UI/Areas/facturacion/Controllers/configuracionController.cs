using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Facturacion;

namespace FNTC.Finansoft.UI.Areas.facturacion.Controllers
{
    public class configuracionController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: facturacion/configuracion
        public ActionResult Index()
        {
            return View(db.configuracionFact.ToList());
        }

        // GET: facturacion/configuracion/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            configuracionFact configuracionFact = db.configuracionFact.Find(id);
            if (configuracionFact == null)
            {
                return HttpNotFound();
            }
            return View(configuracionFact);
        }

        // GET: facturacion/configuracion/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: facturacion/configuracion/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,codConsecutivo,inicio,final,consecutivoActual")] configuracionFact configuracionFact)
        {
            if (ModelState.IsValid)
            {
                db.configuracionFact.Add(configuracionFact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(configuracionFact);
        }

        // GET: facturacion/configuracion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            configuracionFact configuracionFact = db.configuracionFact.Find(id);
            if (configuracionFact == null)
            {
                return HttpNotFound();
            }
            return View(configuracionFact);
        }

        // POST: facturacion/configuracion/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codConsecutivo,inicio,final,consecutivoActual")] configuracionFact configuracionFact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(configuracionFact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "facturacion", new { area = "facturacion" });
                    
            }
            return View(configuracionFact);
        }

        // GET: facturacion/configuracion/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            configuracionFact configuracionFact = db.configuracionFact.Find(id);
            if (configuracionFact == null)
            {
                return HttpNotFound();
            }
            return View(configuracionFact);
        }

        // POST: facturacion/configuracion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            configuracionFact configuracionFact = db.configuracionFact.Find(id);
            db.configuracionFact.Remove(configuracionFact);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
