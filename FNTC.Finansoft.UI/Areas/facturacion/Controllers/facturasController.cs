using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Facturacion;

namespace FNTC.Finansoft.UI.Areas.facturacion.Controllers
{
    public class facturasController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: facturacion/facturas
        public ActionResult Index()
        {
            return View(db.factura.ToList());
        }

        // GET: facturacion/facturas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            factura factura = db.factura.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // GET: facturacion/facturas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: facturacion/facturas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,terceroNit,payCash,payCredit,payTdebit,payTcredit,totalDiscount,date,observation,total,tipo,saldoCredito,fechaPagoCredito,codConsecutivo,numeroFactura,valorTotalExcentos,valorTotalExcluidos,baseIVA19,baseIVA5,valorIVA19,valorIVA5")] factura factura)
        {
            if (ModelState.IsValid)
            {
                db.factura.Add(factura);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(factura);
        }

        // GET: facturacion/facturas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            factura factura = db.factura.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // POST: facturacion/facturas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,terceroNit,payCash,payCredit,payTdebit,payTcredit,totalDiscount,date,observation,total,tipo,saldoCredito,fechaPagoCredito,codConsecutivo,numeroFactura,valorTotalExcentos,valorTotalExcluidos,baseIVA19,baseIVA5,valorIVA19,valorIVA5")] factura factura)
        {
            if (ModelState.IsValid)
            {
                db.Entry(factura).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(factura);
        }

        // GET: facturacion/facturas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            factura factura = db.factura.Find(id);
            if (factura == null)
            {
                return HttpNotFound();
            }
            return View(factura);
        }

        // POST: facturacion/facturas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            factura factura = db.factura.Find(id);
            db.factura.Remove(factura);
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
