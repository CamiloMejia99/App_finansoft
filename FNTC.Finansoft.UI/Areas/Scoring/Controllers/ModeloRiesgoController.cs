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
    public class ModeloRiesgoController : Controller
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

        // GET: Scoring/ModeloRiesgo/Edit
        public ActionResult Edit()
        {

            var ViewModelScoringModeloRiesgo = new ViewModelScoringModeloRiesgoRiesgos()
            {
            RiesgoImpago = (from pc in db.ScoringModeloRiesgos where pc.id == 1 select pc.Estado).Single(),
            RiesgoCreditoIndividual = (from pc in db.ScoringModeloRiesgos where pc.id == 2 select pc.Estado).Single(),
            RiesgoDeCartera = (from pc in db.ScoringModeloRiesgos where pc.id == 3 select pc.Estado).Single(),
            RiesgoDeCalificacion = (from pc in db.ScoringModeloRiesgos where pc.id == 4 select pc.Estado).Single(),

            };
     
            return View(ViewModelScoringModeloRiesgo);
        }

        // POST: Scoring/ModeloRiesgo/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RiesgoImpago,RiesgoCreditoIndividual,RiesgoDeCartera,RiesgoDeCalificacion")] ViewModelScoringModeloRiesgoRiesgos ViewModelScoringModeloRiesgo)
        {

            if (ModelState.IsValid)
            {
                var RiesgoImpago = (from pc in db.ScoringModeloRiesgos where pc.id == 1 select pc).Single();
                RiesgoImpago.Estado = ViewModelScoringModeloRiesgo.RiesgoImpago;
                db.Entry(RiesgoImpago).State = EntityState.Modified;

                var RiesgoCreditoIndividual = (from pc in db.ScoringModeloRiesgos where pc.id == 2 select pc).Single();
                RiesgoCreditoIndividual.Estado = ViewModelScoringModeloRiesgo.RiesgoCreditoIndividual;
                db.Entry(RiesgoCreditoIndividual).State = EntityState.Modified;

                var RiesgoDeCartera = (from pc in db.ScoringModeloRiesgos where pc.id == 3 select pc).Single();
                RiesgoDeCartera.Estado = ViewModelScoringModeloRiesgo.RiesgoDeCartera;
                db.Entry(RiesgoDeCartera).State = EntityState.Modified;

                var RiesgoDeCalificacion = (from pc in db.ScoringModeloRiesgos where pc.id == 4 select pc).Single();
                RiesgoDeCalificacion.Estado = ViewModelScoringModeloRiesgo.RiesgoDeCalificacion;
                db.Entry(RiesgoDeCalificacion).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("../Scoring/");
            }
            return View(ViewModelScoringModeloRiesgo);
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
