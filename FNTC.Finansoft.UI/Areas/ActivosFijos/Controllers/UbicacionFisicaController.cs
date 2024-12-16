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
    public class UbicacionFisicaController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: ActivosFijos/UbicacionFisica
        public ActionResult Index()
        {
            return View(db.UbicacionFisica.ToList());
        }

        // GET: ActivosFijos/UbicacionFisica/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UbicacionFisica ubicacionFisica = db.UbicacionFisica.Find(id);
            if (ubicacionFisica == null)
            {
                return HttpNotFound();
            }
            return View(ubicacionFisica);
        }

        // GET: ActivosFijos/UbicacionFisica/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActivosFijos/UbicacionFisica/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,codigo,nombre")] UbicacionFisica ubicacionFisica)
        {
            if (ModelState.IsValid)
            {
                db.UbicacionFisica.Add(ubicacionFisica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ubicacionFisica);
        }

        // GET: ActivosFijos/UbicacionFisica/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UbicacionFisica ubicacionFisica = db.UbicacionFisica.Find(id);
            if (ubicacionFisica == null)
            {
                return HttpNotFound();
            }
            return View(ubicacionFisica);
        }

        // POST: ActivosFijos/UbicacionFisica/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codigo,nombre")] UbicacionFisica ubicacionFisica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ubicacionFisica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ubicacionFisica);
        }

        // GET: ActivosFijos/UbicacionFisica/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UbicacionFisica ubicacionFisica = db.UbicacionFisica.Find(id);
            if (ubicacionFisica == null)
            {
                return HttpNotFound();
            }
            return View(ubicacionFisica);
        }

        // POST: ActivosFijos/UbicacionFisica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UbicacionFisica ubicacionFisica = db.UbicacionFisica.Find(id);
            db.UbicacionFisica.Remove(ubicacionFisica);
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
