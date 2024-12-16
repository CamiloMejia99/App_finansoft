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
    public class ChequerasController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Tesoreria/Chequeras
        public ActionResult Index()
        {
            return View(db.TsorConsecutivosChequeras.ToList());
        }

        // GET: Tesoreria/Chequeras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TsorConsecutivosChequera tsorConsecutivosChequera = db.TsorConsecutivosChequeras.Find(id);
            if (tsorConsecutivosChequera == null)
            {
                return HttpNotFound();
            }
            return View(tsorConsecutivosChequera);
        }

        // GET: Tesoreria/Chequeras/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tesoreria/Chequeras/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,codigoBancoMatriculado,codigoChequera,estado,consecutivoInicial,consecutivoFinal,consecutivoActual,alertaChequesAgotados,numeroAlertaChequesAgotados")] TsorConsecutivosChequera tsorConsecutivosChequera)
        {
            if (ModelState.IsValid)
            {
                db.TsorConsecutivosChequeras.Add(tsorConsecutivosChequera);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tsorConsecutivosChequera);
        }

        // GET: Tesoreria/Chequeras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TsorConsecutivosChequera tsorConsecutivosChequera = db.TsorConsecutivosChequeras.Find(id);
            if (tsorConsecutivosChequera == null)
            {
                return HttpNotFound();
            }
            return View(tsorConsecutivosChequera);
        }

        [HttpPost]
        public JsonResult GetUltimoCodigoChequeras()
        {
            var LastRegister = db.TsorConsecutivosChequeras.Count();
            if (LastRegister == 0)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Ultimo = db.TsorConsecutivosChequeras.OrderByDescending(x => x.id).First().codigoChequera;
                return Json(Ultimo, JsonRequestBehavior.AllowGet);
            }
        }

        [Compress]
        public JsonResult GetBancosMatriculados(string term = "", int page = 1, int count = 10, int type = 2)
        {
            var cuentas = new List<TsorMatricularBanco>();
            using (var ctx = new AccountingContext())
            {
                cuentas = ctx.TsorMatricularBancos.Include(c => c.TsorBancos.CuentaMayor).Include(c => c.Parameter).Include(c => c.Parameter1).Include(c => c.agencias).
                Where(pc => pc.codigo.ToString().Contains(term) || pc.TsorBancos.CuentaMayor.NOMBRE.Contains(term))
                  .OrderBy(o => o.codigo).ToList();
            }
            var ctas = cuentas;
            return Json(ctas, JsonRequestBehavior.AllowGet);
        }

        // POST: Tesoreria/Chequeras/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codigoBancoMatriculado,codigoChequera,estado,consecutivoInicial,consecutivoFinal,consecutivoActual,alertaChequesAgotados,numeroAlertaChequesAgotados")] TsorConsecutivosChequera tsorConsecutivosChequera)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tsorConsecutivosChequera).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tsorConsecutivosChequera);
        }

        // GET: Tesoreria/Chequeras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TsorConsecutivosChequera tsorConsecutivosChequera = db.TsorConsecutivosChequeras.Find(id);
            if (tsorConsecutivosChequera == null)
            {
                return HttpNotFound();
            }
            return View(tsorConsecutivosChequera);
        }

        [HttpPost]
        public JsonResult GetConsecutivoOtrasChequeras(string ConsecutivoIni, string Banco)
        {
            var bancoInt = Int32.Parse(Banco);
            var chequerasCount = db.TsorConsecutivosChequeras.Where(x => x.codigoBancoMatriculado == bancoInt).OrderByDescending(x => x.id).Count();
            if (chequerasCount == 0)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Ultimo = db.TsorConsecutivosChequeras.OrderByDescending(x => x.id).First().codigoChequera;

                var chequeras = db.TsorConsecutivosChequeras.Where(x => x.codigoBancoMatriculado == bancoInt).OrderByDescending(x => x.id).First().consecutivoFinal;
                return Json(chequeras, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: Tesoreria/Chequeras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TsorConsecutivosChequera tsorConsecutivosChequera = db.TsorConsecutivosChequeras.Find(id);
            db.TsorConsecutivosChequeras.Remove(tsorConsecutivosChequera);
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
