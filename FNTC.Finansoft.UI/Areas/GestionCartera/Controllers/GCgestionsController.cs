using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.GestionCartera;

namespace FNTC.Finansoft.UI.Areas.GestionCartera.Controllers
{
    public class GCgestionsController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: GestionCartera/GCgestions
        public ActionResult Index()
        {
            return View(db.GCgestion.ToList());
        }

        // GET: GestionCartera/GCgestions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GCgestion gCgestion = db.GCgestion.Find(id);
            if (gCgestion == null)
            {
                return HttpNotFound();
            }
            return View(gCgestion);
        }

        // GET: GestionCartera/GCgestions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GestionCartera/GCgestions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,idAsociado,ClaseGestion,FechaGestion,RespuestaGestion,ResOpcionalGestion,ContactoGestion,GestionVerificada")] GCgestion gCgestion)
        {
            if (ModelState.IsValid)
            {
                db.GCgestion.Add(gCgestion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gCgestion);
        }

        // GET: GestionCartera/GCgestions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GCgestion gCgestion = db.GCgestion.Find(id);
            if (gCgestion == null)
            {
                return HttpNotFound();
            }
            return View(gCgestion);
        }

        // POST: GestionCartera/GCgestions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,idAsociado,ClaseGestion,FechaGestion,RespuestaGestion,ResOpcionalGestion,ContactoGestion,GestionVerificada")] GCgestion gCgestion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gCgestion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gCgestion);
        }

        // GET: GestionCartera/GCgestions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GCgestion gCgestion = db.GCgestion.Find(id);
            if (gCgestion == null)
            {
                return HttpNotFound();
            }
            return View(gCgestion);
        }

        // POST: GestionCartera/GCgestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GCgestion gCgestion = db.GCgestion.Find(id);
            db.GCgestion.Remove(gCgestion);
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
