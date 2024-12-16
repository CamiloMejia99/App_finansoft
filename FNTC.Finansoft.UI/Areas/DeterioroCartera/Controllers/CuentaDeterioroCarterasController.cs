using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.DeterioroCartera;

namespace FNTC.Finansoft.UI.Areas.DeterioroCartera.Controllers
{
    [Authorize]
    public class CuentaDeterioroCarterasController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: DeterioroCartera/CuentaDeterioroCarteras
        public ActionResult Index()
        {
            var cuentaDeterioroCartera = db.CuentaDeterioroCartera.Include(c => c.CuentaDeterioro).Include(c => c.CuentaGastosProvision).Include(c => c.TipoComprobante);
            return View(cuentaDeterioroCartera.ToList());
        }//ho

        // GET: DeterioroCartera/CuentaDeterioroCarteras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CuentaDeterioroCartera cuentaDeterioroCartera = db.CuentaDeterioroCartera.Find(id);
            if (cuentaDeterioroCartera == null)
            {
                return HttpNotFound();
            }
            return View(cuentaDeterioroCartera);
        }

        // GET: DeterioroCartera/CuentaDeterioroCarteras/Create
        public ActionResult Create()
        {
            ViewBag.Auxiliares = GetCuentas();
            ViewBag.IdPlanCuentaDeterioro = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE");
            ViewBag.IdPlanCuentaGastosProvision = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE");
            ViewBag.TComprobante = new SelectList(db.TiposComprobantes, "CODIGO", "CODIGO");
            return View();
        }

        // POST: DeterioroCartera/CuentaDeterioroCarteras/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdPlanCuentaDeterioro,IdPlanCuentaGastosProvision,TComprobante,NombreSeleccion")] CuentaDeterioroCartera cuentaDeterioroCartera)
        {
            if (ModelState.IsValid)
            {
                db.CuentaDeterioroCartera.Add(cuentaDeterioroCartera);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Auxiliares = GetCuentas();
            ViewBag.IdPlanCuentaDeterioro = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE", cuentaDeterioroCartera.IdPlanCuentaDeterioro);
            ViewBag.IdPlanCuentaGastosProvision = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE", cuentaDeterioroCartera.IdPlanCuentaGastosProvision);
            ViewBag.TComprobante = new SelectList(db.TiposComprobantes, "CODIGO", "CODIGO", cuentaDeterioroCartera.TComprobante);
            return View(cuentaDeterioroCartera);
        }

        // GET: DeterioroCartera/CuentaDeterioroCarteras/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.Auxiliares = GetCuentas();
            ViewBag.Comprobantes = GetTipoComprobante();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CuentaDeterioroCartera cuentaDeterioroCartera = db.CuentaDeterioroCartera.Find(id);
            if (cuentaDeterioroCartera == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdPlanCuentaDeterioro = new SelectList(db.PlanCuentas, "CODIGO", "CODIGO", cuentaDeterioroCartera.IdPlanCuentaDeterioro);
            ViewBag.IdPlanCuentaGastosProvision = new SelectList(db.PlanCuentas, "CODIGO", "CODIGO", cuentaDeterioroCartera.IdPlanCuentaGastosProvision);
            ViewBag.TComprobante = new SelectList(db.TiposComprobantes, "CODIGO", "CODIGO", cuentaDeterioroCartera.TComprobante);
            return View(cuentaDeterioroCartera);
        }

        // POST: DeterioroCartera/CuentaDeterioroCarteras/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdPlanCuentaDeterioro,IdPlanCuentaGastosProvision,TComprobante,NombreSeleccion")] CuentaDeterioroCartera cuentaDeterioroCartera)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cuentaDeterioroCartera).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Auxiliares = GetCuentas();
            ViewBag.Comprobantes = GetTipoComprobante();
           // ViewBag.IdPlanCuentaDeterioro = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE", cuentaDeterioroCartera.IdPlanCuentaDeterioro);
            //ViewBag.IdPlanCuentaGastosProvision = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE", cuentaDeterioroCartera.IdPlanCuentaGastosProvision);
            //ViewBag.TComprobante = new SelectList(db.TiposComprobantes, "CODIGO", "CLASEComprobante", cuentaDeterioroCartera.TComprobante);
            return View(cuentaDeterioroCartera);
        }

        // GET: DeterioroCartera/CuentaDeterioroCarteras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CuentaDeterioroCartera cuentaDeterioroCartera = db.CuentaDeterioroCartera.Find(id);
            if (cuentaDeterioroCartera == null)
            {
                return HttpNotFound();
            }
            return View(cuentaDeterioroCartera);
        }

        // POST: DeterioroCartera/CuentaDeterioroCarteras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CuentaDeterioroCartera cuentaDeterioroCartera = db.CuentaDeterioroCartera.Find(id);
            db.CuentaDeterioroCartera.Remove(cuentaDeterioroCartera);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public object GetCuentas()
        {
            var ctas = new
                                AccountingContext()
                                .PlanCuentas.ToList()
                                .Select(X => new { CODIGO = X.CODIGO, NOMBRE = X.CODIGO + " - " + X.NOMBRE  });

            return ctas;
        }
        public object GetTipoComprobante()
        {
            var ctas = new
                                AccountingContext()
                                .TiposComprobantes.ToList()
                                .Select(X => new { CODIGO = X.CODIGO, NOMBRE = X.CODIGO + " - " + X.NOMBRE   });

            return ctas;
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
