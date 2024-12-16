using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.Accounting.DTO;

namespace FNTC.Finansoft.UI.Areas.Creditos.Controllers
{
    public class CostosPrestamosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Creditos/CostosPrestamos
        public ActionResult Index()
        {
            var costosPrestamos = db.CostosPrestamos.Include(c => c.Costos_Adicionales);
            return View(costosPrestamos.ToList());
        }

        // GET: Creditos/CostosPrestamos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CostosPrestamos costosPrestamos = db.CostosPrestamos.Find(id);
            if (costosPrestamos == null)
            {
                return HttpNotFound();
            }
            return View(costosPrestamos);
        }

        // GET: Creditos/CostosPrestamos/Create
        public ActionResult Create()
        {
            ViewBag.CA_Id = new SelectList(db.Costos_Adicionales, "CA_Id", "CA_Nombre");
            return View();
        }

        // POST: Creditos/CostosPrestamos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id_CostoPretamo,CA_Id,Pagare")] CostosPrestamos costosPrestamos)
        {
            if (ModelState.IsValid)
            {
                db.CostosPrestamos.Add(costosPrestamos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CA_Id = new SelectList(db.Costos_Adicionales, "CA_Id", "CA_Nombre", costosPrestamos.CA_Id);
            return View(costosPrestamos);
        }

        // GET: Creditos/CostosPrestamos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CostosPrestamos costosPrestamos = db.CostosPrestamos.Find(id);
            if (costosPrestamos == null)
            {
                return HttpNotFound();
            }
            ViewBag.CA_Id = new SelectList(db.Costos_Adicionales, "CA_Id", "CA_Nombre", costosPrestamos.CA_Id);
            return View(costosPrestamos);
        }

        // POST: Creditos/CostosPrestamos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id_CostoPretamo,CA_Id,Pagare")] CostosPrestamos costosPrestamos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(costosPrestamos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CA_Id = new SelectList(db.Costos_Adicionales, "CA_Id", "CA_Nombre", costosPrestamos.CA_Id);
            return View(costosPrestamos);
        }

        // GET: Creditos/CostosPrestamos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CostosPrestamos costosPrestamos = db.CostosPrestamos.Find(id);
            if (costosPrestamos == null)
            {
                return HttpNotFound();
            }
            return View(costosPrestamos);
        }

        // POST: Creditos/CostosPrestamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CostosPrestamos costosPrestamos = db.CostosPrestamos.Find(id);
            db.CostosPrestamos.Remove(costosPrestamos);
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
