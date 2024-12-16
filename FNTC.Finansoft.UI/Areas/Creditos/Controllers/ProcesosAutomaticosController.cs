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
    public class ProcesosAutomaticosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Creditos/ProcesosAutomaticos
        public ActionResult Index()
        {
            return View(db.ProcesosAutomaticos.ToList());
        }

        // GET: Creditos/ProcesosAutomaticos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProcesosAutomaticos procesosAutomaticos = db.ProcesosAutomaticos.Find(id);
            if (procesosAutomaticos == null)
            {
                return HttpNotFound();
            }
            return View(procesosAutomaticos);
        }

        // GET: Creditos/ProcesosAutomaticos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Creditos/ProcesosAutomaticos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,causacionPrestamos,causacionAhorros,realizarDebitoAutomatico,realizarCierresActivosFijos,realizarCierresActivosDiferidos,horaProceso,copiaSeguridadAutomatica")] ProcesosAutomaticos procesosAutomaticos)
        {
            if (ModelState.IsValid)
            {
                db.ProcesosAutomaticos.Add(procesosAutomaticos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(procesosAutomaticos);
        }

        // GET: Creditos/ProcesosAutomaticos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProcesosAutomaticos procesosAutomaticos = db.ProcesosAutomaticos.Find(id);
            if (procesosAutomaticos == null)
            {
                return HttpNotFound();
            }
            return View(procesosAutomaticos);
        }

        // POST: Creditos/ProcesosAutomaticos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,causacionPrestamos,causacionAhorros,realizarDebitoAutomatico,realizarCierresActivosFijos,realizarCierresActivosDiferidos,horaProceso,copiaSeguridadAutomatica")] ProcesosAutomaticos procesosAutomaticos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(procesosAutomaticos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { Controller = "Lineas", Area = "Creditos" });
            }
            return View(procesosAutomaticos);
        }

        // GET: Creditos/ProcesosAutomaticos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProcesosAutomaticos procesosAutomaticos = db.ProcesosAutomaticos.Find(id);
            if (procesosAutomaticos == null)
            {
                return HttpNotFound();
            }
            return View(procesosAutomaticos);
        }

        // POST: Creditos/ProcesosAutomaticos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProcesosAutomaticos procesosAutomaticos = db.ProcesosAutomaticos.Find(id);
            db.ProcesosAutomaticos.Remove(procesosAutomaticos);
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
