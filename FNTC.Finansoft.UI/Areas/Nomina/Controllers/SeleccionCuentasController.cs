using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using FNTC.Finansoft.Accounting.DTO.Nomina;
using FNTC.Finansoft.Accounting.DAL.Nomina;
using FNTC.Finansoft.Accounting.DTO;

namespace FNTC.Finansoft.UI.Areas.Nomina.Controllers
{
    public class SeleccionCuentasController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Nomina/SeleccionCuentas

        public SeleccionCuentasController()
        {

        }

        public ActionResult List()//(string nombre)
        {
            return PartialView(db.SeleccionCuenta.Include(p => p.Cuenta).OrderBy(p => p.CODIGO).ToList());
        }
        public ActionResult Index()
        {
            return View(db.SeleccionCuenta.Include(p => p.Cuenta).OrderBy(p => p.CODIGO).ToList());
        }

        // GET: Nomina/SeleccionCuentas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeleccionCuenta seleccionCuenta = db.SeleccionCuenta.Find(id);
            if (seleccionCuenta == null)
            {
                return HttpNotFound();
            }
            return View(seleccionCuenta);
        }

        // GET: Nomina/SeleccionCuentas/Create
        public ActionResult Create()
        {

            ViewBag.action = "Create";
            ViewBag.boton = "Nuevo";
            ViewBag.guardado = "N";
            return View();
        }

        // POST: Nomina/SeleccionCuentas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CODIGO,TIPOCUENTA")] SeleccionCuenta seleccionCuenta)
        {
            ViewBag.guardado = "N";
            if (ModelState.IsValid)
            {

                db.SeleccionCuenta.Add(seleccionCuenta);
                db.SaveChanges();
                ViewBag.guardado = "S";
                return View(seleccionCuenta);

            }
            //return PartialView(true);
            return View(seleccionCuenta);
        }



        [HttpPost]
        public ActionResult ValidacionCuenta(string Cuenta)
        {
            //  var query=(from s in db.Prestamos orderby s.id descending select s)
            using (var ctx = new AccountingContext())
            {
                if ((from s in ctx.SeleccionCuenta where s.CODIGO == Cuenta select s).Count() != 0)
                {

                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }

        // GET: Nomina/SeleccionCuentas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeleccionCuenta seleccionCuenta = db.SeleccionCuenta.Find(id);
            if (seleccionCuenta == null)
            {
                return HttpNotFound();
            }

            ViewBag.action = "Edit";
            ViewBag.boton = "Editar";
            ViewBag.guardado = "N";
            return PartialView(seleccionCuenta);

        }

        // POST: Nomina/SeleccionCuentas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CODIGO,TIPOCUENTA")] SeleccionCuenta seleccionCuenta)
        {
            ViewBag.guardado = "N";
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(seleccionCuenta).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.guardado = "S";
                }
                catch
                {

                }


            }
            return PartialView(seleccionCuenta);
        }

        // GET: Nomina/SeleccionCuentas/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SeleccionCuenta seleccionCuenta = db.SeleccionCuenta.Find(id);
            if (seleccionCuenta == null)
            {
                return HttpNotFound();
            }
            //return View(seleccionCuenta);
            return PartialView(seleccionCuenta);

        }

        // POST: Nomina/SeleccionCuentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {

            int id1 = 0;
            bool result = int.TryParse(id, out id1);
            SeleccionCuenta seleccionCuenta = db.SeleccionCuenta.Find(id1);
            db.SeleccionCuenta.Remove(seleccionCuenta);
            db.SaveChanges();
            ViewBag.guardado = "S";
            return PartialView(seleccionCuenta);
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
