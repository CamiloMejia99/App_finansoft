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
using FNTC.Finansoft.Accounting.DTO.Tesoreria;

namespace FNTC.Finansoft.UI.Areas.Tesoreria.Controllers
{
    public class BancosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Tesoreria/Bancos
        public ActionResult Index()
        {
            return View(db.TsorBancos.ToList());
        }

        // GET: Tesoreria/Bancos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TsorBanco tsorBanco = db.TsorBancos.Find(id);
            if (tsorBanco == null)
            {
                return HttpNotFound();
            }
            return View(tsorBanco);
        }

        // GET: Tesoreria/Bancos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tesoreria/Bancos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,codigo,cuenta")] TsorBanco tsorBanco)
        {
            if (ModelState.IsValid)
            {
                db.TsorBancos.Add(tsorBanco);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tsorBanco);
        }

        // GET: Tesoreria/Bancos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TsorBanco tsorBanco = db.TsorBancos.Find(id);
            if (tsorBanco == null)
            {
                return HttpNotFound();
            }
            return View(tsorBanco);
        }

        // POST: Tesoreria/Bancos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,codigo,cuenta")] TsorBanco tsorBanco)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tsorBanco).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tsorBanco);
        }

        // GET: Tesoreria/Bancos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TsorBanco tsorBanco = db.TsorBancos.Find(id);
            if (tsorBanco == null)
            {
                return HttpNotFound();
            }
            return View(tsorBanco);
        }

        public JsonResult GetUltimoCodigoBanco()
        {
            var LastRegister = db.TsorBancos.Count();
            if (LastRegister == 0)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Ultimo = db.TsorBancos.OrderByDescending(x => x.id).First().codigo;
                return Json(Ultimo, JsonRequestBehavior.AllowGet);
            }
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
        public JsonResult GetAgencias(string term = "", int page = 1, int count = 10, int type = 2)
        {
            var agencias = new List<agencias>();

            using (var ctx = new AccountingContext())
            {
                agencias = ctx.agencias.//probar el include para traer el nombre de la cuenta
                Where(pc => pc.codigoagencia.ToString().Contains(term) || pc.nombreagencia.Contains(term))
                  .OrderBy(o => o.codigoagencia).ToList();
            }
            var agen = agencias;
            return Json(agen, JsonRequestBehavior.AllowGet);
        }

        // POST: Tesoreria/Bancos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TsorBanco tsorBanco = db.TsorBancos.Find(id);
            db.TsorBancos.Remove(tsorBanco);
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
