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
    public class TipoCarteraMicrocreditosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Scoring/ScoringTipoCarteraMicrocreditoes
        public ActionResult Index()
        {
            return View(db.ScoringTipoCarteraMicrocreditos.ToList());
        }

        // GET: Scoring/ScoringTipoCarteraMicrocreditoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringTipoCarteraMicrocredito scoringTipoCarteraMicrocredito = db.ScoringTipoCarteraMicrocreditos.Find(id);
            if (scoringTipoCarteraMicrocredito == null)
            {
                return HttpNotFound();
            }
            return View(scoringTipoCarteraMicrocredito);
        }

        // GET: Scoring/ScoringTipoCarteraMicrocreditoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Scoring/ScoringTipoCarteraMicrocreditoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Categoria,NroDias,PorcentajeProvision")] ScoringTipoCarteraMicrocredito scoringTipoCarteraMicrocredito)
        {
            if (ModelState.IsValid)
            {
                db.ScoringTipoCarteraMicrocreditos.Add(scoringTipoCarteraMicrocredito);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scoringTipoCarteraMicrocredito);
        }

        // GET: Scoring/ScoringTipoCarteraMicrocreditoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringTipoCarteraMicrocredito scoringTipoCarteraMicrocredito = db.ScoringTipoCarteraMicrocreditos.Find(id);
            if (scoringTipoCarteraMicrocredito == null)
            {
                return HttpNotFound();
            }
            return View(scoringTipoCarteraMicrocredito);
        }

        // POST: Scoring/ScoringTipoCarteraMicrocreditoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Categoria,NroDias,PorcentajeProvision")] ScoringTipoCarteraMicrocredito scoringTipoCarteraMicrocredito)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scoringTipoCarteraMicrocredito).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scoringTipoCarteraMicrocredito);
        }

        // GET: Scoring/ScoringTipoCarteraMicrocreditoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringTipoCarteraMicrocredito scoringTipoCarteraMicrocredito = db.ScoringTipoCarteraMicrocreditos.Find(id);
            if (scoringTipoCarteraMicrocredito == null)
            {
                return HttpNotFound();
            }
            return View(scoringTipoCarteraMicrocredito);
        }

        // POST: Scoring/ScoringTipoCarteraMicrocreditoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScoringTipoCarteraMicrocredito scoringTipoCarteraMicrocredito = db.ScoringTipoCarteraMicrocreditos.Find(id);
            db.ScoringTipoCarteraMicrocreditos.Remove(scoringTipoCarteraMicrocredito);
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
