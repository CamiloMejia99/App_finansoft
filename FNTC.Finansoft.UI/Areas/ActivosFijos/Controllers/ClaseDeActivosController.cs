using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.ActivosFijos;

namespace FNTC.Finansoft.UI.Areas.ActivosFijos.Controllers
{
    public class ClaseDeActivosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: ActivosFijos/ClaseDeActivos
        public ActionResult Index()
        {
            return View(db.ClaseDeActivo.ToList());
        }

        // GET: ActivosFijos/ClaseDeActivos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaseDeActivo claseDeActivo = db.ClaseDeActivo.Find(id);
            if (claseDeActivo == null)
            {
                return HttpNotFound();
            }
            return View(claseDeActivo);
        }

        // GET: ActivosFijos/ClaseDeActivos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActivosFijos/ClaseDeActivos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,codigo,nombre")] ClaseDeActivo claseDeActivo)
        {
            if (ModelState.IsValid)
            {
                db.ClaseDeActivo.Add(claseDeActivo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(claseDeActivo);
        }

        // GET: ActivosFijos/ClaseDeActivos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaseDeActivo claseDeActivo = db.ClaseDeActivo.Find(id);
            if (claseDeActivo == null)
            {
                return HttpNotFound();
            }
            return View(claseDeActivo);
        }

        // POST: ActivosFijos/ClaseDeActivos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codigo,nombre")] ClaseDeActivo claseDeActivo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(claseDeActivo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(claseDeActivo);
        }

        // GET: ActivosFijos/ClaseDeActivos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaseDeActivo claseDeActivo = db.ClaseDeActivo.Find(id);
            if (claseDeActivo == null)
            {
                return HttpNotFound();
            }
            return View(claseDeActivo);
        }

        // POST: ActivosFijos/ClaseDeActivos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClaseDeActivo claseDeActivo = db.ClaseDeActivo.Find(id);
            db.ClaseDeActivo.Remove(claseDeActivo);
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
