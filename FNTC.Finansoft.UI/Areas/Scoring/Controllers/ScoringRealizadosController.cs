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
    public class ScoringRealizadosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Scoring/ScoringRealizados
        public ActionResult Index()
        {
            return View(db.ScoringScoringRealizados.ToList());
        }

        // GET: Scoring/PruebaScoringRealizados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringScoringRealizado scoringScoringRealizado = db.ScoringScoringRealizados.Find(id);
            if (scoringScoringRealizado == null)
            {
                return HttpNotFound();
            }
            return View(scoringScoringRealizado);
        }
        // POST: Scoring/PruebaScoringRealizados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScoringScoringRealizado scoringScoringRealizado = db.ScoringScoringRealizados.Find(id);
            db.ScoringScoringRealizados.Remove(scoringScoringRealizado);
            db.SaveChanges();
            return PartialView(scoringScoringRealizado);
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
