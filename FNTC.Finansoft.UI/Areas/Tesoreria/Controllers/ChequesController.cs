using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Tesoreria;

namespace FNTC.Finansoft.UI.Areas.Tesoreria.Controllers
{
    public class ChequesController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Tesoreria/Cheques
        public ActionResult Index()
        {
            return View(db.TsorCheques.ToList());
        }

        // GET: Tesoreria/Cheques/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TsorCheque tsorCheque = db.TsorCheques.Find(id);
            if (tsorCheque == null)
            {
                return HttpNotFound();
            }
            return View(tsorCheque);
        }

        // GET: Tesoreria/Cheques/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tesoreria/Cheques/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,codigoChequera,consecutivo,fecha,valor,confirmado,anulado,NITTercero,usuario")] TsorCheque tsorCheque)
        {
            if (ModelState.IsValid)
            {
                db.TsorCheques.Add(tsorCheque);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tsorCheque);
        }

        // GET: Tesoreria/Cheques/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TsorCheque tsorCheque = db.TsorCheques.Find(id);
            if (tsorCheque == null)
            {
                return HttpNotFound();
            }
            return View(tsorCheque);
        }

        // POST: Tesoreria/Cheques/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codigoChequera,consecutivo,fecha,valor,confirmado,anulado,NITTercero,usuario")] TsorCheque tsorCheque)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tsorCheque).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tsorCheque);
        }

        // GET: Tesoreria/Cheques/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TsorCheque tsorCheque = db.TsorCheques.Find(id);
            if (tsorCheque == null)
            {
                return HttpNotFound();
            }
            return View(tsorCheque);
        }

        // POST: Tesoreria/Cheques/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TsorCheque tsorCheque = db.TsorCheques.Find(id);
            db.TsorCheques.Remove(tsorCheque);
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
