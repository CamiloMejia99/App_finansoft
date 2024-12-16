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
    public class IncrementaController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Creditos/Incrementa
        public ActionResult Index()
        {
            return View(db.Incrementa.ToList());
        }

        // GET: Creditos/Incrementa/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incrementa incrementa = db.Incrementa.Find(id);
            if (incrementa == null)
            {
                return HttpNotFound();
            }
            return View(incrementa);
        }

        // GET: Creditos/Incrementa/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Creditos/Incrementa/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Incrementa_Id,Incrementa_Descripcion")] Incrementa incrementa)
        {
            if (ModelState.IsValid)
            {
                db.Incrementa.Add(incrementa);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(incrementa);
        }

        // GET: Creditos/Incrementa/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incrementa incrementa = db.Incrementa.Find(id);
            if (incrementa == null)
            {
                return HttpNotFound();
            }
            return View(incrementa);
        }

        // POST: Creditos/Incrementa/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Incrementa_Id,Incrementa_Descripcion")] Incrementa incrementa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(incrementa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(incrementa);
        }

        // GET: Creditos/Incrementa/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Incrementa incrementa = db.Incrementa.Find(id);
            if (incrementa == null)
            {
                return HttpNotFound();
            }
            return View(incrementa);
        }

        // POST: Creditos/Incrementa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Incrementa incrementa = db.Incrementa.Find(id);
            db.Incrementa.Remove(incrementa);
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
