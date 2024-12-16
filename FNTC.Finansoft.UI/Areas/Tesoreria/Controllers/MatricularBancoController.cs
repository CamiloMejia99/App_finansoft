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
    public class MatricularBancoController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Tesoreria/MatricularBanco
        public ActionResult Index()
        {
            return View(db.TsorMatricularBancos.Include(c => c.agencias).Include(a => a.Parameter).Include(ac => ac.Parameter1).Include(ab => ab.TsorBancos).ToList());
        }

        // GET: Tesoreria/MatricularBanco/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TsorMatricularBanco tsorMatricularBanco = db.TsorMatricularBancos.Find(id);
            if (tsorMatricularBanco == null)
            {
                return HttpNotFound();
            }
            return View(tsorMatricularBanco);
        }

        // GET: Tesoreria/MatricularBanco/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tesoreria/MatricularBanco/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,codigo,NIT,numeroCuenta,tipoCuenta,codigoagencia,formatoComprobante,formatoImpresion")] TsorMatricularBanco tsorMatricularBanco)
        {
            if (ModelState.IsValid)
            {
                db.TsorMatricularBancos.Add(tsorMatricularBanco);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tsorMatricularBanco);
        }

        // GET: Tesoreria/MatricularBanco/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TsorMatricularBanco tsorMatricularBanco = db.TsorMatricularBancos.Find(id);
            if (tsorMatricularBanco == null)
            {
                return HttpNotFound();
            }
            return View(tsorMatricularBanco);
        }

        // POST: Tesoreria/MatricularBanco/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codigo,NIT,numeroCuenta,tipoCuenta,codigoagencia,formatoComprobante,formatoImpresion")] TsorMatricularBanco tsorMatricularBanco)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tsorMatricularBanco).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tsorMatricularBanco);
        }

        // GET: Tesoreria/MatricularBanco/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TsorMatricularBanco tsorMatricularBanco = db.TsorMatricularBancos.Find(id);
            if (tsorMatricularBanco == null)
            {
                return HttpNotFound();
            }
            return View(tsorMatricularBanco);
        }

        // POST: Tesoreria/MatricularBanco/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TsorMatricularBanco tsorMatricularBanco = db.TsorMatricularBancos.Find(id);
            db.TsorMatricularBancos.Remove(tsorMatricularBanco);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Compress]
        public JsonResult GetCuentasAA(string term = "", int page = 1, int count = 10, int type = 2)
        {
            var cuentas = new List<TsorBanco>();

            using (var ctx = new AccountingContext())
            {
                cuentas = ctx.TsorBancos.Include(c => c.CuentaMayor).//probar el include para traer el nombre de la cuenta
                Where(pc => pc.codigo.ToString().Contains(term) || pc.CuentaMayor.NOMBRE.Contains(term))
                  .OrderBy(o => o.codigo).ToList();
            }
            var ctas = cuentas;
            return Json(ctas, JsonRequestBehavior.AllowGet);
        }

        [Compress]
        public JsonResult GetBancosMatriculados(string term = "", int page = 1, int count = 10, int type = 2)
        {
            var cuentas = new List<TsorBanco>();

            using (var ctx = new AccountingContext())
            {
                cuentas = ctx.TsorBancos.Include(c => c.CuentaMayor).//probar el include para traer el nombre de la cuenta
                Where(pc => pc.codigo.ToString().Contains(term) || pc.CuentaMayor.NOMBRE.Contains(term))
                  .OrderBy(o => o.codigo).ToList();
            }
            var ctas = cuentas;
            return Json(ctas, JsonRequestBehavior.AllowGet);
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
