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
    public class ClaseDePlanosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        public ClaseDePlanosController()
        {
            ViewBag.TipoPlano = new List<SelectListItem>()
            {
                new SelectListItem(){Value = "DISCRIMINADO", Text = "DISCRIMINADO"}
            };
            ViewBag.TipoRecepcion = new List<SelectListItem>()
            {
                new SelectListItem(){Value = "RECIBE", Text = "RECIBE"},
                new SelectListItem(){Value = "ENVIA", Text = "ENVIA"}
            };
            ViewBag.Extencion = new List<SelectListItem>()
            {
                new SelectListItem(){Value = "EX", Text = "EXCEL"}
            };
            ViewBag.Delimitador = new List<SelectListItem>()
            {
                new SelectListItem(){Value = "PUNTOYCOMA", Text = "PUNTO Y COMA"},
                new SelectListItem(){Value = "COMA", Text = "COMA"},
                new SelectListItem(){Value = "NINGUNO", Text = "NINGUNO"}
            };

        }



        // GET: Nomina/ClaseDePlanos
        public ActionResult Index()
        {
            return View(db.ClaseDePlano.ToList());
        }

        // GET: Nomina/ClaseDePlanos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaseDePlano claseDePlano = db.ClaseDePlano.Find(id);
            if (claseDePlano == null)
            {
                return HttpNotFound();
            }
            return View(claseDePlano);
        }

        // GET: Nomina/ClaseDePlanos/Create
        public ActionResult Create()
        {
            ViewBag.action = "Create";
            ViewBag.boton = "Nuevo";

            return View();
        }

        // POST: Nomina/ClaseDePlanos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,NOMBRE,TIPOPLANO,TIPORECEPCION,EXTENSION,DELIMITADOR")] ClaseDePlano claseDePlano)
        {
            if (ModelState.IsValid)
            {
                db.ClaseDePlano.Add(claseDePlano);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(claseDePlano);
        }

        // GET: Nomina/ClaseDePlanos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaseDePlano claseDePlano = db.ClaseDePlano.Find(id);
            if (claseDePlano == null)
            {
                return HttpNotFound();
            }

            ViewBag.action = "edit";
            ViewBag.boton = "Editar";
            //return PartialView("Create", claseDePlano);
            return View(claseDePlano);
        }

        // POST: Nomina/ClaseDePlanos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,NOMBRE,TIPOPLANO,TIPORECEPCION,EXTENSION,DELIMITADOR")] ClaseDePlano claseDePlano)
        {
            if (ModelState.IsValid)
            {
                db.Entry(claseDePlano).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //return PartialView("Create",claseDePlano);
            return View(claseDePlano);
        }

        [HttpPost]

        public ActionResult ValidacionNombre(string Nombre)
        {

            using (var ctx = new AccountingContext())
            {
                if ((from s in ctx.ClaseDePlano where s.NOMBRE == Nombre select s).Count() != 0)
                {

                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }
        // GET: Nomina/ClaseDePlanos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClaseDePlano claseDePlano = db.ClaseDePlano.Find(id);
            if (claseDePlano == null)
            {
                return HttpNotFound();
            }

            return View(claseDePlano);
        }

        // POST: Nomina/ClaseDePlanos/Delete/5
        [HttpPost]

        public ActionResult DeleteConfirmed(string id)
        {
            int id1 = 0;
            bool result = int.TryParse(id, out id1);
            ClaseDePlano claseDePlano = db.ClaseDePlano.Find(id1);
            db.ClaseDePlano.Remove(claseDePlano);
            db.SaveChanges();
            ViewBag.guardado = "S";
            //return PartialView("Index");
            return RedirectToAction("Index");
        }

        // POST: Nomina/ClaseDePlanos/Delete/5
        [HttpPost]

        public ActionResult ValidarPlano(string id)
        {
            int IdPlano = Int32.Parse(id);
            int ArchivoPlanoContar = (from pc in db.ArchivoPlano where pc.CLASEPLANO == IdPlano select pc).Count();
            int PlanoEmpresaContar = (from pc in db.PlanoEmpresa where pc.NOMPLANO == IdPlano select pc).Count();
            List<string> validar = new List<string>();
            string validacion;
            if (ArchivoPlanoContar >= 1 && PlanoEmpresaContar == 0)
            {
                validacion = "Debe Borrar Primero El Plano De Archivos Planos";
            }
            else
                if (PlanoEmpresaContar >= 1 && ArchivoPlanoContar == 0)
            {
                validacion = "Debe Borrar Primero El Plano De Planos Empresa";
            }
            else
                if ((PlanoEmpresaContar >= 1) && (ArchivoPlanoContar >= 1))
            {
                validacion = "Debe Borrar Primero El Plano De Archivos Planos y Planos Empresa";
            }
            else
            {
                validacion = "true";
            }
            validar.Add(validacion.ToString());
            return Json(validar, JsonRequestBehavior.AllowGet);
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
