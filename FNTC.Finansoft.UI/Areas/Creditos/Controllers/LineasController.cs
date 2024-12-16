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
    public class LineasController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: /Lineas/
        public ActionResult Index()
        {
            return View(db.Lineas.ToList());
        }

        // GET: /Lineas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lineas lineas = db.Lineas.Find(id);
            if (lineas == null)
            {
                return HttpNotFound();
            }
            return View(lineas);
        }

        // GET: /Lineas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Lineas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Lineas_Id,Lineas_Descripcion,Lineas_Codigo,Lineas_Activo")] Lineas lineas)
        {
            if (ModelState.IsValid)
            {
                db.Lineas.Add(lineas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lineas);
        }

        // GET: /Lineas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lineas lineas = db.Lineas.Find(id);
            if (lineas == null)
            {
                return HttpNotFound();
            }
            return View(lineas);
        }

        // POST: /Lineas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Lineas_Id,Lineas_Descripcion,Lineas_Codigo,Lineas_Activo")] Lineas lineas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lineas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lineas);
        }

        // GET: /Lineas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lineas lineas = db.Lineas.Find(id);
            if (lineas == null)
            {
                return HttpNotFound();
            }
            return View(lineas);
        }

        // POST: /Lineas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lineas lineas = db.Lineas.Find(id);
            db.Lineas.Remove(lineas);
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
