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
    public class Clase_GarantiasController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Creditos/Clase_Garantias
        public ActionResult Index()
        {
            return View(db.Clase_Garantias.ToList());
        }

        // GET: Creditos/Clase_Garantias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clase_Garantias clase_Garantias = db.Clase_Garantias.Find(id);
            if (clase_Garantias == null)
            {
                return HttpNotFound();
            }
            return View(clase_Garantias);
        }

        // GET: Creditos/Clase_Garantias/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Creditos/Clase_Garantias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Clase_Garantias_Id,Clase_Garantias_Descripcion")] Clase_Garantias clase_Garantias)
        {
            if (ModelState.IsValid)
            {
                db.Clase_Garantias.Add(clase_Garantias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clase_Garantias);
        }

        // GET: Creditos/Clase_Garantias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clase_Garantias clase_Garantias = db.Clase_Garantias.Find(id);
            if (clase_Garantias == null)
            {
                return HttpNotFound();
            }
            return View(clase_Garantias);
        }

        // POST: Creditos/Clase_Garantias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Clase_Garantias_Id,Clase_Garantias_Descripcion")] Clase_Garantias clase_Garantias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clase_Garantias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clase_Garantias);
        }

        // GET: Creditos/Clase_Garantias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clase_Garantias clase_Garantias = db.Clase_Garantias.Find(id);
            if (clase_Garantias == null)
            {
                return HttpNotFound();
            }
            return View(clase_Garantias);
        }

        // POST: Creditos/Clase_Garantias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clase_Garantias clase_Garantias = db.Clase_Garantias.Find(id);
            db.Clase_Garantias.Remove(clase_Garantias);
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
