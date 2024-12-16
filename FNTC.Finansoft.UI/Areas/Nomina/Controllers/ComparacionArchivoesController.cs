using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Nomina;

namespace FNTC.Finansoft.UI.Areas.Nomina.Controllers
{
    public class ComparacionArchivoesController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Nomina/ComparacionArchivoes
        public ActionResult Index()
        {
            ViewBag.PeriodoDed = new List<SelectListItem>()
            {
                new SelectListItem(){Value = "SEMANAL", Text = "SEMANAL"},
                new SelectListItem(){Value = "QUINCENAL", Text = "QUINCENAL"},
                new SelectListItem(){Value = "MENSUAL", Text = "MENSUAL"},
                new SelectListItem(){Value = "TRIMESTRAL", Text = "TRIMESTRAL"},
                new SelectListItem(){Value = "SEMESTRAL", Text = "SEMESTRAL"},
                new SelectListItem(){Value = "ANUAL", Text = "ANUAL"}
            };
            var EMPRES = new Finansoft.Accounting.BLL.PlanoEmpresas.PlanoEmpresasBLL().GetPlanoEmpresa();
            ViewBag.EMP = EMPRES;
            var tipo = new FNTC.Finansoft.Accounting.BLL.ArchivoPlanos.ArchivoPlanosBLL().GetArchivoPlanos();
            ViewBag.AP = tipo;
            var plano = new Finansoft.Accounting.BLL.ClaseDePlanos.ClaseDePlanosBLL().GetClaseDePlano();
            ViewBag.CP = plano;
            //return View(db.Discriminacion.ToList());
            //return Create(); 
            return View("Create");
        }

        public ActionResult List()//(string nombre)
        {
            return PartialView(db.SeleccionCuenta.Include(p => p.Cuenta).OrderBy(p => p.CODIGO).ToList());
        }
        public ActionResult ListadoComparacion()
        {
            return View(db.SeleccionCuenta.Include(p => p.Cuenta).OrderBy(p => p.CODIGO).ToList());
        }


        // GET: Nomina/ComparacionArchivoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComparacionArchivo comparacionArchivo = db.ComparacionArchivo.Find(id);
            if (comparacionArchivo == null)
            {
                return HttpNotFound();
            }
            return View(comparacionArchivo);
        }

        // GET: Nomina/ComparacionArchivoes/Create
        public ActionResult Create()
        {
            var EMPRES = new Finansoft.Accounting.BLL.PlanoEmpresas.PlanoEmpresasBLL().GetPlanoEmpresa();
            ViewBag.EMP = EMPRES;
            var tipo = new FNTC.Finansoft.Accounting.BLL.ArchivoPlanos.ArchivoPlanosBLL().GetArchivoPlanos();
            ViewBag.AP = tipo;
            return View();
        }

        // POST: Nomina/ComparacionArchivoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,EMPRESA,PERDEDUCC,PERIODO,CAMBIO,ORDEND,PLANO,RUTA")] ComparacionArchivo comparacionArchivo)
        {
            if (ModelState.IsValid)
            {
                db.ComparacionArchivo.Add(comparacionArchivo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(comparacionArchivo);
        }

        // GET: Nomina/ComparacionArchivoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComparacionArchivo comparacionArchivo = db.ComparacionArchivo.Find(id);
            if (comparacionArchivo == null)
            {
                return HttpNotFound();
            }
            return View(comparacionArchivo);
        }

        // POST: Nomina/ComparacionArchivoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EMPRESA,PERDEDUCC,PERIODO,CAMBIO,ORDEND,PLANO,RUTA")] ComparacionArchivo comparacionArchivo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comparacionArchivo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(comparacionArchivo);
        }

        // GET: Nomina/ComparacionArchivoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ComparacionArchivo comparacionArchivo = db.ComparacionArchivo.Find(id);
            if (comparacionArchivo == null)
            {
                return HttpNotFound();
            }
            return View(comparacionArchivo);
        }

        // POST: Nomina/ComparacionArchivoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ComparacionArchivo comparacionArchivo = db.ComparacionArchivo.Find(id);
            db.ComparacionArchivo.Remove(comparacionArchivo);
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
