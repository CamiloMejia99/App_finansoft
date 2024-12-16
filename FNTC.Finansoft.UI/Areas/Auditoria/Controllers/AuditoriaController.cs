using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Auditoria;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Auditoria.Controllers
{
    public class AuditoriaController : Controller
    {
        private AccountingContext db = new AccountingContext();
        // GET: Auditoria/Auditoria
        public ActionResult Index()
        {
            var listado = db.Empresa.ToList();
            return View(listado);
        }

        public ActionResult IndexTipoEmpresa()
        {
            var listado = db.TipoEmpresa.ToList();
            return View(listado);
        }

        public ActionResult IndexEmpresa()
        {
            var listado = db.Empresa.ToList();
            return View(listado);
        }

        //............................
        public ActionResult CreateTipoEmpresa()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTipoEmpresa([System.Web.Http.FromBody] TipoEmpresa TipoEmpresa)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    TipoEmpresa.tipo = TipoEmpresa.tipo.ToUpper();
                    db.TipoEmpresa.Add(TipoEmpresa);
                    db.SaveChanges();
                    return RedirectToAction("IndexTipoEmpresa");
                }
                catch (DbEntityValidationException dbE)
                {

                }

            }
            return View(TipoEmpresa);
        }

        public ActionResult EditTipoEmpresa(int id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tipoEmpresa = db.TipoEmpresa.Find(id);
            if (tipoEmpresa == null)
            {
                return HttpNotFound();
            }

            return View(tipoEmpresa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTipoEmpresa([System.Web.Http.FromBody] TipoEmpresa TipoEmpresa)
        {

            if (ModelState.IsValid)
            {
                TipoEmpresa.tipo = TipoEmpresa.tipo.ToUpper();
                db.Entry(TipoEmpresa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexTipoEmpresa");
            }
            return View(TipoEmpresa);
        }


        [HttpPost]
        public JsonResult DeleteTipoEmpresa(int id)
        {
            //int idd = Convert.ToInt32(id);
            var tipoEmpresa = db.TipoEmpresa.Find(id);
            if (tipoEmpresa == null)
            {


                //planilla planilla = db.Planilla.Find(id);
                //db.Planilla.Remove(planilla);
                //db.SaveChanges();

                return new JsonResult { Data = new { status = false } };

            }
            else
            {

                int estaEnEmpresa = db.Empresa.Where(x => x.tipoEmpresa == id).Count();
                if (estaEnEmpresa > 0)
                {
                    var message = "No se puede borrar, existe un registro con este tipo de empresa.";
                    return new JsonResult { Data = new { status = false, mensaje = message } };
                }
                db.TipoEmpresa.Remove(tipoEmpresa);
                db.SaveChanges();
                return new JsonResult { Data = new { status = true } };
            }

        }



        //................................
        public ActionResult CreateEmpresa()
        {
            //inicio select list para tipos de empresa
            List<SelectListItem> tiposEmpresa = new List<SelectListItem>();
            tiposEmpresa.Add(new SelectListItem { Text = "Seleccione...", Value = "" });
            IList<TipoEmpresa> listadoTiposEmpresa = db.TipoEmpresa.ToList();


            foreach (var item in listadoTiposEmpresa)		// recorro los elementos de la db
            {
                tiposEmpresa.Add(new SelectListItem { Text = item.tipo, Value = item.id.ToString() });

            }
            //fin select list para tipos de empresa


            ViewBag.InfoTipoEmpresa = tiposEmpresa;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEmpresa([System.Web.Http.FromBody] Empresaa Empresa)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    db.Empresa.Add(Empresa);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException dbE)
                {

                }

            }

            //inicio select list para tipos de empresa
            List<SelectListItem> tiposEmpresa = new List<SelectListItem>();
            IList<TipoEmpresa> listadoTiposEmpresa = db.TipoEmpresa.ToList();


            foreach (var item in listadoTiposEmpresa)		// recorro los elementos de la db
            {
                tiposEmpresa.Add(new SelectListItem { Text = item.tipo, Value = item.id.ToString() });

            }
            //fin select list para tipos de empresa


            ViewBag.InfoTipoEmpresa = tiposEmpresa;

            return View(Empresa);
        }

        public ActionResult EditEmpresa(int id)
        {
            //inicio select list para tipos de empresa
            List<SelectListItem> tiposEmpresa = new List<SelectListItem>();
            tiposEmpresa.Add(new SelectListItem { Text = "Seleccione...", Value = "" });
            IList<TipoEmpresa> listadoTiposEmpresa = db.TipoEmpresa.ToList();


            foreach (var item in listadoTiposEmpresa)		// recorro los elementos de la db
            {
                tiposEmpresa.Add(new SelectListItem { Text = item.tipo, Value = item.id.ToString() });

            }
            //fin select list para tipos de empresa


            ViewBag.InfoTipoEmpresa = tiposEmpresa;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var Empresa = db.Empresa.Find(id);
            if (Empresa == null)
            {
                return HttpNotFound();
            }

            return View(Empresa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmpresa([System.Web.Http.FromBody] Empresaa Empresa)
        {
            //inicio select list para tipos de empresa
            List<SelectListItem> tiposEmpresa = new List<SelectListItem>();
            tiposEmpresa.Add(new SelectListItem { Text = "Seleccione...", Value = "" });
            IList<TipoEmpresa> listadoTiposEmpresa = db.TipoEmpresa.ToList();


            foreach (var item in listadoTiposEmpresa)		// recorro los elementos de la db
            {
                tiposEmpresa.Add(new SelectListItem { Text = item.tipo, Value = item.id.ToString() });

            }
            //fin select list para tipos de empresa


            ViewBag.InfoTipoEmpresa = tiposEmpresa;
            if (ModelState.IsValid)
            {

                db.Entry(Empresa).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(Empresa);
        }

        public JsonResult DeleteEmpresa(int id)
        {
            //int idd = Convert.ToInt32(id);
            var Empresa = db.Empresa.Find(id);
            if (Empresa == null)
            {
                return new JsonResult { Data = new { status = false } };
            }
            else
            {

                db.Empresa.Remove(Empresa);
                db.SaveChanges();
                return new JsonResult { Data = new { status = true } };
            }

        }

        [HttpPost]
        public JsonResult VerificaTiposEmpresa()
        {
            var message = "";
            int TipoEmpresa = db.TipoEmpresa.Count();
            if (TipoEmpresa > 0)
            {
                int empresa = db.Empresa.Count();
                if (empresa > 0)
                {
                    message = "Su empresa ya está registrada!";
                    return new JsonResult { Data = new { status = false, mensaje = message } };
                }
                return new JsonResult { Data = new { status = true } };
            }
            else
            {
                message = "Primero debe crear al menos un tipo de empresa en la opción 'Tipos de Empresa'";
                return new JsonResult { Data = new { status = false, mensaje = message } };
            }

        }

        [HttpPost]
        public JsonResult CreateVerificaNombreTipoEmpresa(string nombre)
        {

            nombre = nombre.ToUpper();
            int verifica = db.TipoEmpresa.Where(x => x.tipo == nombre).Count();
            if(verifica > 0)
            {
                return new JsonResult { Data = new { status = true } };
            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }
            

        }
        [HttpPost]
        public JsonResult EditVerificaNombreTipoEmpresa(string nombre, int id)
        {

            nombre = nombre.ToUpper();
            var verifica = db.TipoEmpresa.ToList();
            if (verifica.Count > 0)
            {
                verifica = verifica.Where(x => x.id != id).ToList();
                int n = verifica.Where(x => x.tipo == nombre).Count();
                if(n > 0)
                {
                    return new JsonResult { Data = new { status = true } };
                }

                return new JsonResult { Data = new { status = false } };
            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }


        }

        [HttpPost]
        public JsonResult GetTercero(string nit)
        {

            var tercero = db.Terceros.Where(x => x.NIT == nit && x.ESASOCIADO == 3).FirstOrDefault();
            if (tercero == null)
            {
                string message = "No se encuentra un registro con el NIT: " + nit;
                return new JsonResult { Data = new { status = false, mensaje = message } };

            }
            else
            {
                string name = tercero.NombreComercial;
                return new JsonResult { Data = new { status = true, nombre = name } };
            }

        }

    }
}