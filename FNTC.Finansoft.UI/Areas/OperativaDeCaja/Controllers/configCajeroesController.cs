using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;

namespace FNTC.Finansoft.UI.Areas.OperativaDeCaja.Controllers
{
    public class configCajeroesController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: OperativaDeCaja/configCajeroes
        public ActionResult Index()
        {
            var configCajero = db.configCajero.Include(c => c.Caja).Include(c => c.CentrosCostos).Include(c => c.CentrosCostos1).Include(c => c.PlanCuentas).Include(c => c.PlanCuentas1).Include(c => c.PlanCuentas2).Include(c => c.PlanCuentas3).Include(c => c.Terceros).Include(c => c.TiposComprobantes).Include(c => c.TiposComprobantes1).Include(c => c.TiposComprobantes2);
            return View(configCajero.ToList());
        }

        // GET: OperativaDeCaja/configCajeroes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            configCajero configCajero = db.configCajero.Find(id);
            if (configCajero == null)
            {
                return HttpNotFound();
            }
            return View(configCajero);
        }

        // GET: OperativaDeCaja/configCajeroes/Create
        public ActionResult Create()
        {
            ViewBag.Codigo_caja = new SelectList(db.Caja, "Codigo_caja", "Nombre_caja");
            ViewBag.centrocosto = new SelectList(db.CentrosCostos, "Codigo", "Nombre");
            ViewBag.CentroCostoCaja = new SelectList(db.CentrosCostos, "Codigo", "Nombre");
            ViewBag.Contr_banco = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE");
            ViewBag.Contr_otro = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE");
            ViewBag.Cta_efectivo = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE");
            ViewBag.Cta_cheque = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE");
            ViewBag.Nit_cajero = new SelectList(db.Terceros, "NIT", "DIGVER");
            ViewBag.Compr_ingreso = new SelectList(db.TiposComprobantes, "CODIGO", "CLASEComprobante");
            ViewBag.Compr_egreso = new SelectList(db.TiposComprobantes, "CODIGO", "CLASEComprobante");
            ViewBag.Tipocomprobante_caja = new SelectList(db.TiposComprobantes, "CODIGO", "CLASEComprobante");
            return View();
        }

        // POST: OperativaDeCaja/configCajeroes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nit_cajero,Codigo_caja,Compr_ingreso,Compr_egreso,Contr_banco,Contr_otro,Cta_efectivo,Cta_cheque,centrocosto,CentroCostoCaja,Tipocomprobante_caja")] configCajero configCajero)
        {
            if (ModelState.IsValid)
            {
                db.configCajero.Add(configCajero);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Codigo_caja = new SelectList(db.Caja, "Codigo_caja", "Nombre_caja", configCajero.Codigo_caja);
            ViewBag.centrocosto = new SelectList(db.CentrosCostos, "Codigo", "Nombre", configCajero.centrocosto);
            ViewBag.CentroCostoCaja = new SelectList(db.CentrosCostos, "Codigo", "Nombre", configCajero.CentroCostoCaja);
            ViewBag.Contr_banco = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE", configCajero.Contr_banco);
            ViewBag.Contr_otro = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE", configCajero.Contr_otro);
            ViewBag.Cta_efectivo = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE", configCajero.Cta_efectivo);
            ViewBag.Cta_cheque = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE", configCajero.Cta_cheque);
            ViewBag.Nit_cajero = new SelectList(db.Terceros, "NIT", "DIGVER", configCajero.Nit_cajero);
            ViewBag.Compr_ingreso = new SelectList(db.TiposComprobantes, "CODIGO", "CLASEComprobante", configCajero.Compr_ingreso);
            ViewBag.Compr_egreso = new SelectList(db.TiposComprobantes, "CODIGO", "CLASEComprobante", configCajero.Compr_egreso);
            ViewBag.Tipocomprobante_caja = new SelectList(db.TiposComprobantes, "CODIGO", "CLASEComprobante", configCajero.Tipocomprobante_caja);
            return View(configCajero);
        }

        // GET: OperativaDeCaja/configCajeroes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            configCajero configCajero = db.configCajero.Find(id);
            if (configCajero == null)
            {
                return HttpNotFound();
            }
            ViewBag.Codigo_caja = new SelectList(db.Caja, "Codigo_caja", "Nombre_caja", configCajero.Codigo_caja);
            ViewBag.centrocosto = new SelectList(db.CentrosCostos, "Codigo", "Nombre", configCajero.centrocosto);
            ViewBag.CentroCostoCaja = new SelectList(db.CentrosCostos, "Codigo", "Nombre", configCajero.CentroCostoCaja);
            ViewBag.Contr_banco = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE", configCajero.Contr_banco);
            ViewBag.Contr_otro = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE", configCajero.Contr_otro);
            ViewBag.Cta_efectivo = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE", configCajero.Cta_efectivo);
            ViewBag.Cta_cheque = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE", configCajero.Cta_cheque);
            ViewBag.Nit_cajero = new SelectList(db.Terceros, "NIT", "DIGVER", configCajero.Nit_cajero);
            ViewBag.Compr_ingreso = new SelectList(db.TiposComprobantes, "CODIGO", "CLASEComprobante", configCajero.Compr_ingreso);
            ViewBag.Compr_egreso = new SelectList(db.TiposComprobantes, "CODIGO", "CLASEComprobante", configCajero.Compr_egreso);
            ViewBag.Tipocomprobante_caja = new SelectList(db.TiposComprobantes, "CODIGO", "CLASEComprobante", configCajero.Tipocomprobante_caja);
            return View(configCajero);
        }

        // POST: OperativaDeCaja/configCajeroes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Nit_cajero,Codigo_caja,Compr_ingreso,Compr_egreso,Contr_banco,Contr_otro,Cta_efectivo,Cta_cheque,centrocosto,CentroCostoCaja,Tipocomprobante_caja")] configCajero configCajero)
        {
            if (ModelState.IsValid)
            {
                db.Entry(configCajero).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Codigo_caja = new SelectList(db.Caja, "Codigo_caja", "Nombre_caja", configCajero.Codigo_caja);
            ViewBag.centrocosto = new SelectList(db.CentrosCostos, "Codigo", "Nombre", configCajero.centrocosto);
            ViewBag.CentroCostoCaja = new SelectList(db.CentrosCostos, "Codigo", "Nombre", configCajero.CentroCostoCaja);
            ViewBag.Contr_banco = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE", configCajero.Contr_banco);
            ViewBag.Contr_otro = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE", configCajero.Contr_otro);
            ViewBag.Cta_efectivo = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE", configCajero.Cta_efectivo);
            ViewBag.Cta_cheque = new SelectList(db.PlanCuentas, "CODIGO", "NOMBRE", configCajero.Cta_cheque);
            ViewBag.Nit_cajero = new SelectList(db.Terceros, "NIT", "DIGVER", configCajero.Nit_cajero);
            ViewBag.Compr_ingreso = new SelectList(db.TiposComprobantes, "CODIGO", "CLASEComprobante", configCajero.Compr_ingreso);
            ViewBag.Compr_egreso = new SelectList(db.TiposComprobantes, "CODIGO", "CLASEComprobante", configCajero.Compr_egreso);
            ViewBag.Tipocomprobante_caja = new SelectList(db.TiposComprobantes, "CODIGO", "CLASEComprobante", configCajero.Tipocomprobante_caja);
            return View(configCajero);
        }

        // GET: OperativaDeCaja/configCajeroes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            configCajero configCajero = db.configCajero.Find(id);
            if (configCajero == null)
            {
                return HttpNotFound();
            }
            return View(configCajero);
        }

        // POST: OperativaDeCaja/configCajeroes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            configCajero configCajero = db.configCajero.Find(id);
            db.configCajero.Remove(configCajero);
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
