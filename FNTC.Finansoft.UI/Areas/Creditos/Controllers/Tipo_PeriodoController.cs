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
    public class Tipo_PeriodoController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Creditos/Tipo_Periodo
        public ActionResult Index()
        {
            return View(db.Tipo_Periodo.ToList());
        }

        // GET: Creditos/Tipo_Periodo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipo_Periodo tipo_Periodo = db.Tipo_Periodo.Find(id);
            if (tipo_Periodo == null)
            {
                return HttpNotFound();
            }
            return View(tipo_Periodo);
        }

        // GET: Creditos/Tipo_Periodo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Creditos/Tipo_Periodo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tipo_Periodo_Id,Tipo_Periodo_Descripcion,Tipo_Periodo_Valor")] Tipo_Periodo tipo_Periodo)
        {
            if (ModelState.IsValid)
            {
                db.Tipo_Periodo.Add(tipo_Periodo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipo_Periodo);
        }

        // GET: Creditos/Tipo_Periodo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipo_Periodo tipo_Periodo = db.Tipo_Periodo.Find(id);
            if (tipo_Periodo == null)
            {
                return HttpNotFound();
            }
            return View(tipo_Periodo);
        }

        // POST: Creditos/Tipo_Periodo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Tipo_Periodo_Id,Tipo_Periodo_Descripcion,Tipo_Periodo_Valor")] Tipo_Periodo tipo_Periodo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipo_Periodo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipo_Periodo);
        }

        // GET: Creditos/Tipo_Periodo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tipo_Periodo tipo_Periodo = db.Tipo_Periodo.Find(id);
            if (tipo_Periodo == null)
            {
                return HttpNotFound();
            }
            return View(tipo_Periodo);
        }

        // POST: Creditos/Tipo_Periodo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tipo_Periodo tipo_Periodo = db.Tipo_Periodo.Find(id);
            db.Tipo_Periodo.Remove(tipo_Periodo);
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
