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
    public class TipoCarteraConsumoController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Scoring/TipoCarteraConsumo
        public ActionResult Index()
        {
            return View(db.ScoringTipoCarteraConsumos.ToList());
        }

        // GET: Scoring/TipoCarteraConsumo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringTipoCarteraConsumo scoringTipoCarteraConsumo = db.ScoringTipoCarteraConsumos.Find(id);
            if (scoringTipoCarteraConsumo == null)
            {
                return HttpNotFound();
            }
            return View(scoringTipoCarteraConsumo);
        }

        // GET: Scoring/TipoCarteraConsumo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Scoring/TipoCarteraConsumo/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,NroDias,PorcentajeProvision,Categoria")] ScoringTipoCarteraConsumo scoringTipoCarteraConsumo)
        {
            if (ModelState.IsValid)
            {
                db.ScoringTipoCarteraConsumos.Add(scoringTipoCarteraConsumo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scoringTipoCarteraConsumo);
        }

        // GET: Scoring/TipoCarteraConsumo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringTipoCarteraConsumo scoringTipoCarteraConsumo = db.ScoringTipoCarteraConsumos.Find(id);
            if (scoringTipoCarteraConsumo == null)
            {
                return HttpNotFound();
            }
            return View(scoringTipoCarteraConsumo);
        }

        // POST: Scoring/TipoCarteraConsumo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,NroDias,PorcentajeProvision,Categoria")] ScoringTipoCarteraConsumo scoringTipoCarteraConsumo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scoringTipoCarteraConsumo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scoringTipoCarteraConsumo);
        }

        // GET: Scoring/TipoCarteraConsumo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringTipoCarteraConsumo scoringTipoCarteraConsumo = db.ScoringTipoCarteraConsumos.Find(id);
            if (scoringTipoCarteraConsumo == null)
            {
                return HttpNotFound();
            }
            return View(scoringTipoCarteraConsumo);
        }

        // POST: Scoring/TipoCarteraConsumo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScoringTipoCarteraConsumo scoringTipoCarteraConsumo = db.ScoringTipoCarteraConsumos.Find(id);
            db.ScoringTipoCarteraConsumos.Remove(scoringTipoCarteraConsumo);
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
