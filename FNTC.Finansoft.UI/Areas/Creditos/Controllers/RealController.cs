
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
    public class RealController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: /Real/
        public ActionResult Index()
        {
            return View(db.Real.ToList());
        }

        // GET: /Real/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Real real = db.Real.Find(id);
            if (real == null)
            {
                return HttpNotFound();
            }
            return View(real);
        }

        // GET: /Real/Create
        public ActionResult _Real()
        {
            //saca el ultimo id de de la tabla prestamos
            var ultimpid = db.Prestamos.Max(u => u.id);
            ViewBag.ultimoid = ultimpid;

            return View();
        }

        // POST: /Real/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult _Real([Bind(Include = "Real_Valor,PagareId")] Real real)
        {
            if (ModelState.IsValid)
            {
                db.Real.Add(real);
                db.SaveChanges();
                return RedirectToAction("Index", "Lineas");
            }

            return View(real);
        }

        // GET: /Real/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Real real = db.Real.Find(id);
            if (real == null)
            {
                return HttpNotFound();
            }
            return View(real);
        }

        // POST: /Real/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Real_Id,Real_Valor,Prestamos_id")] Real real)
        {
            if (ModelState.IsValid)
            {
                db.Entry(real).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(real);
        }

        // GET: /Real/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Real real = db.Real.Find(id);
            if (real == null)
            {
                return HttpNotFound();
            }
            return View(real);
        }

        // POST: /Real/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Real real = db.Real.Find(id);
            db.Real.Remove(real);
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
