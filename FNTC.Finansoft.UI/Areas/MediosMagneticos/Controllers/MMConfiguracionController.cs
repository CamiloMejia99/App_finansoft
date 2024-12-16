
using FNTC.Finansoft.Accounting.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO.MediosMagneticos;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Framework;
using FNTC.Framework.BO;
using System.Text;
using FNTC.Finansoft.Accounting.DTO.Informes;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using OfficeOpenXml;
using System.Data.SqlClient;
using System.Globalization;



namespace FNTC.Finansoft.UI.Areas.MediosMagneticos.Controllers
{

    public class MMConfiguracionController : Controller
    {


        private AccountingContext db = new AccountingContext();

        public List<SelectListItem> aniosItems = new List<SelectListItem>();

        public List<SelectListItem> aniosF = new List<SelectListItem>();

        public ActionResult Index()
        {
            return View(db.ConfigMedMag.ToList());
        }
        public ActionResult CrearNuevaConfig()
        {

            int fechaActual = Int32.Parse(DateTime.Now.ToString("yyyy"));

            ViewBag.anioItems = new SelectList((from a in db.Movimientos
                                             where a.FECHAMOVIMIENTO.Year != fechaActual
                                             select new { value = a.FECHAMOVIMIENTO.Year, text = a.FECHAMOVIMIENTO.Year }).Distinct().ToList(), "value", "text");

            ViewBag.action = "Create";
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearNuevaConfig(ConfigMedMag configMedMag, FormCollection coll)
        {
            var AnioInt = Int32.Parse(coll["Anios"]);
            try
            {
                var verifica = db.ConfigMedMag.Where(x => x.anvigente == AnioInt && x.formato == configMedMag.formato && x.concepto == configMedMag.concepto && x.categoria == configMedMag.categoria && x.acumuladopor == configMedMag.acumuladopor && x.CuentaContable == configMedMag.CuentaContable).Any();
                if (!verifica)
                {
                if (ModelState.IsValid)
                    {
                        var cuencunt = configMedMag.CuentaContable;
                        var Cuenta = cuencunt + "";
                        configMedMag.CuentaContable = Cuenta;

                        var format = configMedMag.formato;
                        configMedMag.formato = format;

                        var concept = configMedMag.concepto;
                        configMedMag.concepto = concept;

                        var catego = configMedMag.categoria;
                        configMedMag.categoria = catego;

                        var acum = configMedMag.acumuladopor;
                        configMedMag.acumuladopor = acum;

                        configMedMag.anvigente = AnioInt;
                        db.ConfigMedMag.Add(configMedMag);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error al Agregar ");
                        return View();
                     }
                }
                else
                {
                    TempData["error"] = "El formato ya se encuentra parametrizado";
                    return RedirectToAction("CrearNuevaConfig");
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al Agregar ");
                return View();
            }
 
        }
   
        public ActionResult PlanCuentasMovimientos(int anvigenteID)
        {
            var informacion =(from m in db.Movimientos
                              join a in db.PlanCuentas on m.CUENTA equals a.CODIGO
                              where ((m.FECHAMOVIMIENTO.Year) == anvigenteID)
                              select new { m.CUENTA, a.NOMBRE }).Distinct().ToList();
         
            return Json(informacion
                    .Select(x => new SelectListItem()
                    {
                        Text = x.CUENTA + " - " + x.NOMBRE,
                        Value = x.CUENTA
                    }).ToList().OrderBy(x => x.Value), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Formatos()
        {
            return PartialView(db.formatos.ToList());
        }
        public ActionResult ListaAcumulados()
        {
            return PartialView(db.acumuladopor.ToList());
        }

        [Compress]
        public ActionResult GetConceptoMD(int formatoID)
        {
                var Concep = db.conceptos.Where(d => d.idFormato == formatoID);

                return Json(Concep
                    .Select(x => new SelectListItem()
                    {
                        Text = x.codigoConceptos.ToString(),
                        Value = x.idConceptos.ToString()
                    }).ToList().OrderBy(x => x.Text), JsonRequestBehavior.AllowGet);
            
        }
        [Compress]
        public ActionResult GetCategoriaMD(int formatoID)
        {
                var CategoriaMD = db.categorias.Where(d => d.idFormato == formatoID);

                return Json(CategoriaMD
                    .Select(x => new SelectListItem()
                    {
                        Text = x.descripcion.ToString(),
                        Value = x.idCategoria.ToString()
                    }).ToList().OrderBy(x => x.Value), JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult Eliminar(int Id)
        {
                ConfigMedMag dato = db.ConfigMedMag.Find(Id);
                db.ConfigMedMag.Remove(dato);
                db.SaveChanges();
                return RedirectToAction("Index");     

        }

        [Compress]
        public ActionResult FormatoPorAnio(int anioID)
        {
            var  cuentas= db.ConfigMedMag.Where(d => d.anvigente == anioID);

            return Json(cuentas
                .Select(x => new SelectListItem()
                {
                    Text = x.format.codigoFormato.ToString() +" - "+x.CuentaContable +" - "+ x.categori.descripcion.ToString(),
                    Value = x.idConfigMedMag.ToString()
                }).ToList().OrderBy(x => x.Text), JsonRequestBehavior.AllowGet);

        }

        public ActionResult ListaRegistros()
        {

            var anios = (from a in db.ConfigMedMag
                         select new { a.anvigente }).Distinct().ToList();

            aniosF = new List<SelectListItem>();
            foreach (var item in anios)
            {
                aniosF.Add(new SelectListItem
                {
                    Text = item.anvigente.ToString(),
                    Value = item.anvigente.ToString()

                });
            }
            ViewBag.aniosF = aniosF;
            return View();
        }

    }
}
