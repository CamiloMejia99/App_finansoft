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
    public class TipoCarteraComercialController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Scoring/TipoCarteraComercial
        public ActionResult Index()
        {
            return View(db.ScoringTipoCarteraComerciales.ToList());
        }

        // GET: Scoring/TipoCarteraComercial/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringTipoCarteraComercial scoringTipoCarteraComercial = db.ScoringTipoCarteraComerciales.Find(id);
            if (scoringTipoCarteraComercial == null)
            {
                return HttpNotFound();
            }
            return View(scoringTipoCarteraComercial);
        }

        // GET: Scoring/TipoCarteraComercial/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Scoring/TipoCarteraComercial/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Categoria,NroDias,PorcentajeProvision")] ScoringTipoCarteraComercial scoringTipoCarteraComercial)
        {
            if (ModelState.IsValid)
            {
                db.ScoringTipoCarteraComerciales.Add(scoringTipoCarteraComercial);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scoringTipoCarteraComercial);
        }

        // GET: Scoring/TipoCarteraComercial/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringTipoCarteraComercial scoringTipoCarteraComercial = db.ScoringTipoCarteraComerciales.Find(id);
            if (scoringTipoCarteraComercial == null)
            {
                return HttpNotFound();
            }
            return View(scoringTipoCarteraComercial);
        }

        // POST: Scoring/TipoCarteraComercial/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Categoria,NroDias,PorcentajeProvision")] ScoringTipoCarteraComercial scoringTipoCarteraComercial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scoringTipoCarteraComercial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scoringTipoCarteraComercial);
        }

        // GET: Scoring/TipoCarteraComercial/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringTipoCarteraComercial scoringTipoCarteraComercial = db.ScoringTipoCarteraComerciales.Find(id);
            if (scoringTipoCarteraComercial == null)
            {
                return HttpNotFound();
            }
            return View(scoringTipoCarteraComercial);
        }

        // POST: Scoring/TipoCarteraComercial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScoringTipoCarteraComercial scoringTipoCarteraComercial = db.ScoringTipoCarteraComerciales.Find(id);
            db.ScoringTipoCarteraComerciales.Remove(scoringTipoCarteraComercial);
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
