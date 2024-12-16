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
    public class Forma_PagoController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Creditos/Forma_Pago
        public ActionResult Index()
        {
            return View(db.Forma_Pago.ToList());
        }

        // GET: Creditos/Forma_Pago/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forma_Pago forma_Pago = db.Forma_Pago.Find(id);
            if (forma_Pago == null)
            {
                return HttpNotFound();
            }
            return View(forma_Pago);
        }

        // GET: Creditos/Forma_Pago/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Creditos/Forma_Pago/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Forma_Pago_Id,Forma_Pago_Descripcion")] Forma_Pago forma_Pago)
        {
            if (ModelState.IsValid)
            {
                db.Forma_Pago.Add(forma_Pago);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(forma_Pago);
        }

        // GET: Creditos/Forma_Pago/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forma_Pago forma_Pago = db.Forma_Pago.Find(id);
            if (forma_Pago == null)
            {
                return HttpNotFound();
            }
            return View(forma_Pago);
        }

        // POST: Creditos/Forma_Pago/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Forma_Pago_Id,Forma_Pago_Descripcion")] Forma_Pago forma_Pago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(forma_Pago).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(forma_Pago);
        }

        // GET: Creditos/Forma_Pago/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Forma_Pago forma_Pago = db.Forma_Pago.Find(id);
            if (forma_Pago == null)
            {
                return HttpNotFound();
            }
            return View(forma_Pago);
        }

        // POST: Creditos/Forma_Pago/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Forma_Pago forma_Pago = db.Forma_Pago.Find(id);
            db.Forma_Pago.Remove(forma_Pago);
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
