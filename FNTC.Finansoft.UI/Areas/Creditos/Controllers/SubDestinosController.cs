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
    public class SubDestinosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Creditos/SubDestinos
        public ActionResult Index()
        {
            var subdestinos = db.SubDestinos.Include(s => s.Destinos);
            return View(db.SubDestinos.ToList());
        }

        // GET: Creditos/SubDestinos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubDestinos subDestinos = db.SubDestinos.Find(id);
            if (subDestinos == null)
            {
                return HttpNotFound();
            }
            return View(subDestinos);
        }

        // GET: Creditos/SubDestinos/Create
        public ActionResult Create()
        {
            ViewBag.Destino_Id = new SelectList(db.Destinos, "Destino_Id", "Destino_Descripcion");
            return View();
        }

        // POST: Creditos/SubDestinos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Subdestino_Id,Subdestino_Codigo,Subdestino_Descripcion,Destino_Id,Subdestino_Activo")] SubDestinos subDestinos)
        {
            if (ModelState.IsValid)
            {
                db.SubDestinos.Add(subDestinos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Destino_Id = new SelectList(db.Destinos, "Destino_Id", "Destino_Codigo", subDestinos.Destino_Id);
            return View(subDestinos);
        }

        // GET: Creditos/SubDestinos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubDestinos subDestinos = db.SubDestinos.Find(id);
            if (subDestinos == null)
            {
                return HttpNotFound();
            }
            ViewBag.Destino_Id = new SelectList(db.Destinos, "Destino_Id", "Destino_Codigo", subDestinos.Destino_Id);
            return View(subDestinos);
        }

        // POST: Creditos/SubDestinos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Subdestino_Id,Subdestino_Codigo,Subdestino_Descripcion,Destino_Id,Subdestino_Activo")] SubDestinos subDestinos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subDestinos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Destino_Id = new SelectList(db.Destinos, "Destino_Id", "Destino_Codigo", subDestinos.Destino_Id);
            return View(subDestinos);
        }

        // GET: Creditos/SubDestinos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubDestinos subDestinos = db.SubDestinos.Find(id);
            if (subDestinos == null)
            {
                return HttpNotFound();
            }
            return View(subDestinos);
        }

        // POST: Creditos/SubDestinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubDestinos subDestinos = db.SubDestinos.Find(id);
            db.SubDestinos.Remove(subDestinos);
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
