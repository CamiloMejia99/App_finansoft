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
    public class GruposController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: ActivosFijos/Grupos
        public ActionResult Index()
        {
            return View(db.GruposActivosFijos.ToList());
        }

        // GET: ActivosFijos/Grupos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GruposActivosFijos gruposActivosFijos = db.GruposActivosFijos.Find(id);
            if (gruposActivosFijos == null)
            {
                return HttpNotFound();
            }
            return View(gruposActivosFijos);
        }

        // GET: ActivosFijos/Grupos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ActivosFijos/Grupos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,codigo,nombre")] GruposActivosFijos gruposActivosFijos)
        {
            if (ModelState.IsValid)
            {
                db.GruposActivosFijos.Add(gruposActivosFijos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gruposActivosFijos);
        }

        // GET: ActivosFijos/Grupos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GruposActivosFijos gruposActivosFijos = db.GruposActivosFijos.Find(id);
            if (gruposActivosFijos == null)
            {
                return HttpNotFound();
            }
            return View(gruposActivosFijos);
        }

        // POST: ActivosFijos/Grupos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codigo,nombre")] GruposActivosFijos gruposActivosFijos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gruposActivosFijos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gruposActivosFijos);
        }

        // GET: ActivosFijos/Grupos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GruposActivosFijos gruposActivosFijos = db.GruposActivosFijos.Find(id);
            if (gruposActivosFijos == null)
            {
                return HttpNotFound();
            }
            return View(gruposActivosFijos);
        }

        // POST: ActivosFijos/Grupos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GruposActivosFijos gruposActivosFijos = db.GruposActivosFijos.Find(id);
            db.GruposActivosFijos.Remove(gruposActivosFijos);
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
