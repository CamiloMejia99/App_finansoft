using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Scoring;

namespace FNTC.Finansoft.UI.Areas.Scoring.Controllers
{
    [Authorize]
    public class ScoringController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Scoring/ModeloRiesgo
        public ActionResult Index()
        {
            return View(db.ScoringModeloRiesgos.ToList());
        }

        // GET: Scoring/ModeloRiesgo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringModeloRiesgo scoringModeloRiesgo = db.ScoringModeloRiesgos.Find(id);
            if (scoringModeloRiesgo == null)
            {
                return HttpNotFound();
            }
            return View(scoringModeloRiesgo);
        }

        // GET: Scoring/ModeloRiesgo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Scoring/ModeloRiesgo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ModeloRiesgo,Estado")] ScoringModeloRiesgo scoringModeloRiesgo)
        {
            if (ModelState.IsValid)
            {
                db.ScoringModeloRiesgos.Add(scoringModeloRiesgo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scoringModeloRiesgo);
        }

        // GET: Scoring/ModeloRiesgo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringModeloRiesgo scoringModeloRiesgo = db.ScoringModeloRiesgos.Find(id);
            if (scoringModeloRiesgo == null)
            {
                return HttpNotFound();
            }
            return View(scoringModeloRiesgo);
        }

        // POST: Scoring/ModeloRiesgo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ModeloRiesgo,Estado")] ScoringModeloRiesgo scoringModeloRiesgo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scoringModeloRiesgo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scoringModeloRiesgo);
        }

        // GET: Scoring/ModeloRiesgo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringModeloRiesgo scoringModeloRiesgo = db.ScoringModeloRiesgos.Find(id);
            if (scoringModeloRiesgo == null)
            {
                return HttpNotFound();
            }
            return View(scoringModeloRiesgo);
        }

        // POST: Scoring/ModeloRiesgo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScoringModeloRiesgo scoringModeloRiesgo = db.ScoringModeloRiesgos.Find(id);
            db.ScoringModeloRiesgos.Remove(scoringModeloRiesgo);
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
