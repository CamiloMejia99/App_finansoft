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
    public class TipoCarteraViviendaController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Scoring/TipoCarteraVivienda
        public ActionResult Index()
        {
            return View(db.ScoringTipoCarteraViviendas.ToList());
        }

        // GET: Scoring/TipoCarteraVivienda/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringTipoCarteraVivienda scoringTipoCarteraVivienda = db.ScoringTipoCarteraViviendas.Find(id);
            if (scoringTipoCarteraVivienda == null)
            {
                return HttpNotFound();
            }
            return View(scoringTipoCarteraVivienda);
        }

        // GET: Scoring/TipoCarteraVivienda/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Scoring/TipoCarteraVivienda/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Categoria,NroDias,PorcentajeProvision")] ScoringTipoCarteraVivienda scoringTipoCarteraVivienda)
        {
            if (ModelState.IsValid)
            {
                db.ScoringTipoCarteraViviendas.Add(scoringTipoCarteraVivienda);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scoringTipoCarteraVivienda);
        }

        // GET: Scoring/TipoCarteraVivienda/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringTipoCarteraVivienda scoringTipoCarteraVivienda = db.ScoringTipoCarteraViviendas.Find(id);
            if (scoringTipoCarteraVivienda == null)
            {
                return HttpNotFound();
            }
            return View(scoringTipoCarteraVivienda);
        }

        // POST: Scoring/TipoCarteraVivienda/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Categoria,NroDias,PorcentajeProvision")] ScoringTipoCarteraVivienda scoringTipoCarteraVivienda)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scoringTipoCarteraVivienda).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scoringTipoCarteraVivienda);
        }

        // GET: Scoring/TipoCarteraVivienda/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringTipoCarteraVivienda scoringTipoCarteraVivienda = db.ScoringTipoCarteraViviendas.Find(id);
            if (scoringTipoCarteraVivienda == null)
            {
                return HttpNotFound();
            }
            return View(scoringTipoCarteraVivienda);
        }

        // POST: Scoring/TipoCarteraVivienda/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScoringTipoCarteraVivienda scoringTipoCarteraVivienda = db.ScoringTipoCarteraViviendas.Find(id);
            db.ScoringTipoCarteraViviendas.Remove(scoringTipoCarteraVivienda);
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
