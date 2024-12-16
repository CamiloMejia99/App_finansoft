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
using FNTC.Finansoft.Accounting.DTO.Scoring;
using FNTC.Finansoft.Accounting.DTO.FormularioVinculacion;
using FNTC.Finansoft.Accounting.DTO.Terceros;

namespace FNTC.Finansoft.UI.Areas.Nomina.Controllers
{
    public class PlanoEmpresasController : Controller
    {
        private AccountingContext db = new AccountingContext();

        public ActionResult lista()//(string nombre)
        {
            //return PartialView(db.JerarquiaDescuento.Where(p=>p.NOMBRE.Contains(nombre)).ToList());
            return PartialView(db.PlanoEmpresa.ToList());
        }


        // GET: Nomina/PlanoEmpresas
        public ActionResult Index()
        {
            return View(db.PlanoEmpresa.ToList());
        }

        // GET: Nomina/PlanoEmpresas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanoEmpresa planoEmpresa = db.PlanoEmpresa.Find(id);
            if (planoEmpresa == null)
            {
                return HttpNotFound();
            }
            return View(planoEmpresa);
        }

        // GET: Nomina/PlanoEmpresas/Create
        public ActionResult Create()
        {
            ViewBag.guardado = "N";
            ViewBag.action = "Create";
            ViewBag.boton = "Nuevo";
            var claseplanos = new FNTC.Finansoft.Accounting.BLL.ClaseDePlanos.ClaseDePlanosBLL().GetClaseDePlano();
            ViewBag.CP = claseplanos;



            List<SelectListItem> Terceros = new List<SelectListItem>();   // Creo una lista
            Terceros.Add(new SelectListItem { Text = "Seleccione Una Empresa", Value = "" });
            //var FormatoVinculacion = (from pc in db.formatoVinculacions select pc).Single();
            var TercerosVinculacion = (from pc in db.Terceros where pc.CLASEID == "31" select pc);

            IList<Tercero> ListaDeTerceros = TercerosVinculacion.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeTerceros)		// recorro los elementos de la db
            {
                Terceros.Add(new SelectListItem { Text = item.NIT + " || " + item.NOMBRE, Value = item.NIT });  // agrego los elementos de la db a la primera lista que cree

                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Terceros = Terceros;


            return View();
        }


        // POST: Nomina/ClaseDePlanos/Delete/5
        [HttpPost]

        public ActionResult ValidarEmpresa(string id, string ID_Plano)
        {
            //int CodigoEmp = Int32.Parse(id);
            int IdPlano = Int32.Parse(ID_Plano);

            int PlanoEmpresaContar = (from pc in db.PlanoEmpresa where pc.NOMPLANO == IdPlano && pc.CODIGOEMP == id select pc).Count();
            //int CodigoEmpContar = (from pc in db.PlanoEmpresa where pc.CODIGOEMP == CodigoEmp select pc).Count();
            List<string> validar = new List<string>();
            string validacion;


            if (PlanoEmpresaContar >= 1)
            {
                validacion = "Ya Existe Un Plano Con Esas Características";
            }

            else
            {
                validacion = "true";
            }
            validar.Add(validacion.ToString());
            //return RedirectToAction("Index");
            //return Json(false, JsonRequestBehavior.AllowGet);
            return Json(validar, JsonRequestBehavior.AllowGet);
        }

        // POST: Nomina/ClaseDePlanos/Delete/5
        [HttpPost]

        public ActionResult ValidarEmpresaEdit(string id, string ID_Plano, string Codigo1)
        {
            int id1 = Int32.Parse(Codigo1);
            int IdPlano = Int32.Parse(ID_Plano);

            int PlanoEmpresaContar = (from pc in db.PlanoEmpresa where pc.NOMPLANO == IdPlano && pc.CODIGOEMP == id select pc).Count();
            //int CodigoEmpContar = (from pc in db.PlanoEmpresa where pc.CODIGOEMP == CodigoEmp select pc).Count();
            List<string> validar = new List<string>();
            string validacion;
            int ContarID = (from pc in db.PlanoEmpresa where pc.id == id1 && pc.CODIGOEMP == id && pc.NOMPLANO == IdPlano select pc).Count();

            if (PlanoEmpresaContar >= 1 && ContarID < 1)
            {

                validacion = "Ya existe Otro Plano Con Estas Caracteristicas";
            }

            else
            if (ContarID >= 1)
            {
                validacion = "No Se ha Realizado Ninguna Modificación";
            }
            else
            {
                validacion = "true";
            }
            validar.Add(validacion.ToString());
            //return RedirectToAction("Index");
            //return Json(false, JsonRequestBehavior.AllowGet);
            return Json(validar, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDatosTerceros(string NIT)
        {

            var Tercero = (from pc in db.Terceros where pc.NIT == NIT select pc).Single();
            List<string> codigos = new List<string>();
            codigos.Add(Tercero.NIT.ToString());
            codigos.Add(Tercero.NOMBRE.ToString());
            return Json(codigos, JsonRequestBehavior.AllowGet);
        }

        // POST: Nomina/PlanoEmpresas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlanoEmpresa planoEmpresa)
        {

            if (ModelState.IsValid)
            {
                db.PlanoEmpresa.Add(planoEmpresa);
                db.SaveChanges();
                ViewBag.guardado = "S";
                return PartialView(planoEmpresa);
                //return View(planoEmpresa);
            }
            var claseplanos = new FNTC.Finansoft.Accounting.BLL.ClaseDePlanos.ClaseDePlanosBLL().GetClaseDePlano();
            ViewBag.CP = claseplanos;
            var archivoPlano = new FNTC.Finansoft.Accounting.BLL.ArchivoPlanos.ArchivoPlanosBLL().GetArchivoPlanos();
            ViewBag.AP = archivoPlano;
            return PartialView(planoEmpresa);
        }

        // GET: Nomina/PlanoEmpresas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanoEmpresa planoEmpresa = db.PlanoEmpresa.Find(id);
            if (planoEmpresa == null)
            {
                return HttpNotFound();
            }
            ViewBag.action = "Edit";
            ViewBag.boton = "Editar";
            ViewBag.guardado = "N";
            var claseplanos = new FNTC.Finansoft.Accounting.BLL.ClaseDePlanos.ClaseDePlanosBLL().GetClaseDePlano();
            ViewBag.CP = claseplanos;




            List<SelectListItem> Terceros = new List<SelectListItem>();   // Creo una lista

            //var FormatoVinculacion = (from pc in db.formatoVinculacions select pc).Single();
            var TercerosVinculacion = (from pc in db.Terceros where pc.CLASEID == "31" select pc);

            IList<Tercero> ListaDeTerceros = TercerosVinculacion.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeTerceros)		// recorro los elementos de la db
            {
                Terceros.Add(new SelectListItem { Text = item.NIT + " || " + item.NOMBRE, Value = item.NIT });  // agrego los elementos de la db a la primera lista que cree

                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Terceros = Terceros;


            return View(planoEmpresa);
        }

        // POST: Nomina/PlanoEmpresas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PlanoEmpresa planoEmpresa)
        {
            ViewBag.guardado = "N";
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(planoEmpresa).State = EntityState.Modified;
                    db.SaveChanges();
                    ViewBag.guardado = "S";
                }
                catch
                {

                }

            }
            var claseplanos = new FNTC.Finansoft.Accounting.BLL.ClaseDePlanos.ClaseDePlanosBLL().GetClaseDePlano();
            ViewBag.CP = claseplanos;
            return PartialView(planoEmpresa);
        }


        [HttpPost]

        public ActionResult ValidacionCodigo(string Codigo)
        {
            //  var query=(from s in db.Prestamos orderby s.id descending select s)
            using (var ctx = new AccountingContext())
            {
                if ((from s in ctx.PlanoEmpresa where s.CODIGOEMP == Codigo select s).Count() != 0)
                {

                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }
        // GET: Nomina/PlanoEmpresas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PlanoEmpresa planoEmpresa = db.PlanoEmpresa.Find(id);
            if (planoEmpresa == null)
            {
                return HttpNotFound();
            }
            ViewBag.guardado = "N";
            return PartialView(planoEmpresa);

        }


        // POST: Nomina/PlanoEmpresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            try
            {
                PlanoEmpresa planoEmpresa = db.PlanoEmpresa.Find(id);
                db.PlanoEmpresa.Remove(planoEmpresa);
                db.SaveChanges();
                ViewBag.guardado = "S";
                return PartialView(planoEmpresa);
            }
            catch (Exception ex)
            {

                Console.Write("Error Al Eliminar", ex);

            }

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        private void Init(ref PlanoEmpresa dto)
        {

            /*dto._claseplanos = new FNTC.Framework.Params.DAL.ParamsDAL()
                .GetParamValues("NOMBRE").Select(x => new SelectListItem() { Text = x.Valor, Value = x.Codigo}).ToList();
            dto._claseplanos = db.ClaseDePlano.Select(x => new SelectListItem() { Text = x.NOMBRE, Value = x.NOMBRE }).ToList();*/


        }
    }
}
