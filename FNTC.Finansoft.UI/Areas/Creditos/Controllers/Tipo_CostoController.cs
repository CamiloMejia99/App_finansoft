using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.MCreditos;

namespace FNTC.Finansoft.UI.Areas.Creditos.Controllers
{
    public class Tipo_CostoController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Creditos/Tipo_Costo
        public ActionResult Index()
        {
            return View(db.Tipo_Costo.ToList());
        }

        // GET: Creditos/Tipo_Costo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipo_Costo tipo_Costo = db.Tipo_Costo.Find(id);
            if (tipo_Costo == null)
            {
                return HttpNotFound();
            }
            return View(tipo_Costo);
        }

        // GET: Creditos/Tipo_Costo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Creditos/Tipo_Costo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tipo_Costo_Id,Tipo_Costo_Descripcion")] Tipo_Costo tipo_Costo)
        {
            if (ModelState.IsValid)
            {
                db.Tipo_Costo.Add(tipo_Costo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipo_Costo);
        }

        // GET: Creditos/Tipo_Costo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipo_Costo tipo_Costo = db.Tipo_Costo.Find(id);
            if (tipo_Costo == null)
            {
                return HttpNotFound();
            }
            return View(tipo_Costo);
        }

        // POST: Creditos/Tipo_Costo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Tipo_Costo_Id,Tipo_Costo_Descripcion")] Tipo_Costo tipo_Costo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipo_Costo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipo_Costo);
        }

        // GET: Creditos/Tipo_Costo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipo_Costo tipo_Costo = db.Tipo_Costo.Find(id);
            if (tipo_Costo == null)
            {
                return HttpNotFound();
            }
            return View(tipo_Costo);
        }

        // POST: Creditos/Tipo_Costo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tipo_Costo tipo_Costo = db.Tipo_Costo.Find(id);
            db.Tipo_Costo.Remove(tipo_Costo);
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
