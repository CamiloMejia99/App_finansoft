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
    public class GarantiasController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Creditos/Garantias
        public ActionResult Index()
        {
            var garantias = db.Garantias.Include(g => g.Clase_Garantias);
            return View(garantias.ToList());
        }

        public JsonResult GetVisitCustomer(string Areas, string term = "")
        {
            var objCustomerlist = db.Garantias
                            .Where(c => c.Garantias_Codigo.ToUpper()
                            .Contains(term.ToUpper()))
                            .Select(c => new { Name = c.Garantias_Codigo, ID = c.Garantias_Id })
                            .Distinct().ToList();
            return Json(objCustomerlist, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetClaseGarantia(int IdClase)
        {
            //var clase = db.Garantias.Where(x => x.Garantias_Id == IdClase).Select(x => new {x.Garantias_Id });
            var clase = from s in db.Clase_Garantias where s.Clase_Garantias_Id == IdClase select s.Clase_Garantias_Descripcion;

            return Json(clase, JsonRequestBehavior.AllowGet);
        }
        // GET: Creditos/Garantias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Garantias garantias = db.Garantias.Find(id);
            if (garantias == null)
            {
                return HttpNotFound();
            }
            return View(garantias);
        }

        // GET: Creditos/Garantias/Create
        public ActionResult Create()
        {
            ViewBag.Clase_Garantias_Id = new SelectList(db.Clase_Garantias, "Clase_Garantias_Id", "Clase_Garantias_Descripcion");
            return View();
        }

        // POST: Creditos/Garantias/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Garantias_Id,Garantias_Activo,Garantias_Codigo,Garantias_Descripcion,Garantias_Tipo,Clase_Garantias_Id,Garantias_Codeudor,Garantias_Hipotecarias,Garantias_Porcentaje_Credito_Pagado")] Garantias garantias)
        {
            if (ModelState.IsValid)
            {
                db.Garantias.Add(garantias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Clase_Garantias_Id = new SelectList(db.Clase_Garantias, "Clase_Garantias_Id", "Clase_Garantias_Descripcion", garantias.Clase_Garantias_Id);

            return View(garantias);
        }

        // GET: Creditos/Garantias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Garantias garantias = db.Garantias.Find(id);
            if (garantias == null)
            {
                return HttpNotFound();
            }
            ViewBag.Clase_Garantias_Id = new SelectList(db.Clase_Garantias, "Clase_Garantias_Id", "Clase_Garantias_Descripcion", garantias.Clase_Garantias_Id);
            return View(garantias);
        }

        // POST: Creditos/Garantias/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Garantias_Id,Garantias_Activo,Garantias_Codigo,Garantias_Descripcion,Garantias_Tipo,Clase_Garantias_Id,Garantias_Codeudor,Garantias_Hipotecarias,Garantias_Porcentaje_Credito_Pagado")] Garantias garantias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(garantias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Clase_Garantias_Id = new SelectList(db.Clase_Garantias, "Clase_Garantias_Id", "Clase_Garantias_Descripcion", garantias.Clase_Garantias_Id);

            return View(garantias);
        }

        // GET: Creditos/Garantias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Garantias garantias = db.Garantias.Find(id);
            if (garantias == null)
            {
                return HttpNotFound();
            }
            return View(garantias);
        }

        // POST: Creditos/Garantias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Garantias garantias = db.Garantias.Find(id);
            db.Garantias.Remove(garantias);
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
