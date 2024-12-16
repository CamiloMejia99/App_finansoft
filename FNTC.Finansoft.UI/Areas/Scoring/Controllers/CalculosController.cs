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
    public class CalculosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Scoring/Calculos
        public ActionResult Index()
        {
            return View(db.ScoringCalculos.ToList());
        }

        // GET: Scoring/Calculos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringCalculo scoringCalculo = db.ScoringCalculos.Find(id);
            if (scoringCalculo == null)
            {
                return HttpNotFound();
            }
            return View(scoringCalculo);
        }

        // GET: Scoring/Calculos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Scoring/Calculos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,NombreFormula,Concepto,Formula,Porcentaje")] ScoringCalculo scoringCalculo)
        {
            if (ModelState.IsValid)
            {
                db.ScoringCalculos.Add(scoringCalculo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scoringCalculo);
        }

        // GET: Scoring/Calculos/Edit/5
        public ActionResult Edit()
        {
            var Formula1 = (from pc in db.ScoringCalculos where pc.id == 1 select pc).Single();
            var Formula2 = (from pc in db.ScoringCalculos where pc.id == 2 select pc).Single();
           
            var FormulaPorcentaje = " " + Formula1.Formula + Formula1.Porcentaje + "%";

            var ViewModelScoringCalculo = new ViewModelScoringCalculos()
            {
                
                NombreFormula = Formula1.NombreFormula,
                Concepto = Formula1.Concepto,
                Formula = FormulaPorcentaje,
                Porcentaje = Formula1.Porcentaje,
                NombreFormula2 = Formula2.NombreFormula,
                Concepto2 = Formula2.Concepto,
                Formula2 = Formula2.Formula,
                Porcentaje2 = Formula2.Porcentaje,

            };

            return View(ViewModelScoringCalculo);
        }

        // POST: Scoring/Calculos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Porcentaje")] ViewModelScoringCalculos ViewModelScoringCalculo)
        {
            if (ModelState.IsValid)
            {
                var Porcentaje = (from pc in db.ScoringCalculos where pc.id == 1 select pc).Single();
                Porcentaje.Porcentaje = ViewModelScoringCalculo.Porcentaje;
                db.Entry(Porcentaje).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("../Scoring/");
            }
            return View(ViewModelScoringCalculo);
        }

        // GET: Scoring/Calculos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringCalculo scoringCalculo = db.ScoringCalculos.Find(id);
            if (scoringCalculo == null)
            {
                return HttpNotFound();
            }
            return View(scoringCalculo);
        }

        // POST: Scoring/Calculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScoringCalculo scoringCalculo = db.ScoringCalculos.Find(id);
            db.ScoringCalculos.Remove(scoringCalculo);
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
