using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.Facturacion;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.facturacion.Controllers
{
    public class FacConfiguracionController : Controller
    {
        AccountingContext db = new AccountingContext();
        // GET: facturacion/FacConfiguracion
        public ActionResult Index()
        {
            var configuracion = db.FacConfiguracion.Where(x => x.id == 1).FirstOrDefault();
            if(configuracion!=null)
            {
                return View(configuracion);
            }
            else
            {
                return View();
            }
            
        }

        public ActionResult Create()
        {
            List<SelectListItem> tiposComprobante = new List<SelectListItem>();
            List<SelectListItem> cuentas = new List<SelectListItem>();

            IList<TipoComprobante> listTiposComprobantes = db.TiposComprobantes.ToList();
            IList<CuentaMayor> listCuentas = db.PlanCuentas.Where(x => x.CODIGO.Length == 9).ToList();
            tiposComprobante.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            foreach (var item in listTiposComprobantes)
            {
                tiposComprobante.Add(new SelectListItem { Text = item.CODIGO+"|"+item.NOMBRE, Value = item.CODIGO });
            }
            cuentas.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            foreach (var item in listCuentas)
            {
                cuentas.Add(new SelectListItem { Text = item.CODIGO+"|"+item.NOMBRE, Value = item.CODIGO });
            }

            ViewBag.tiposComprobante = tiposComprobante;
            ViewBag.cuentas = cuentas;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([System.Web.Http.FromBody] FacConfiguracion FacConfiguracion)
        {
            FacConfiguracion.id = 1;
            if (ModelState.IsValid)
            {
                using (var ctx = new AccountingContext())
                {
                    try
                    {
                        ctx.FacConfiguracion.Add(FacConfiguracion);
                        ctx.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException dbE)
                    {

                    }
                }
            }

            List<SelectListItem> tiposComprobante = new List<SelectListItem>();
            List<SelectListItem> cuentas = new List<SelectListItem>();

            IList<TipoComprobante> listTiposComprobantes = db.TiposComprobantes.ToList();
            IList<CuentaMayor> listCuentas = db.PlanCuentas.Where(x => x.CODIGO.Length == 9).ToList();
            tiposComprobante.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            foreach (var item in listTiposComprobantes)
            {
                tiposComprobante.Add(new SelectListItem { Text = item.CODIGO + "|" + item.NOMBRE, Value = item.CODIGO });
            }
            cuentas.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            foreach (var item in listCuentas)
            {
                cuentas.Add(new SelectListItem { Text = item.CODIGO + "|" + item.NOMBRE, Value = item.CODIGO });
            }

            ViewBag.tiposComprobante = tiposComprobante;
            ViewBag.cuentas = cuentas;

            return View(FacConfiguracion);
        }

        public ActionResult Edit()
        {
            var FacConfiguracion = db.FacConfiguracion.Find(1);
            if(FacConfiguracion==null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> tiposComprobante = new List<SelectListItem>();
            List<SelectListItem> cuentas = new List<SelectListItem>();

            IList<TipoComprobante> listTiposComprobantes = db.TiposComprobantes.ToList();
            IList<CuentaMayor> listCuentas = db.PlanCuentas.Where(x => x.CODIGO.Length == 9).ToList();
            tiposComprobante.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            foreach (var item in listTiposComprobantes)
            {
                tiposComprobante.Add(new SelectListItem { Text = item.CODIGO + "|" + item.NOMBRE, Value = item.CODIGO });
            }
            cuentas.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            foreach (var item in listCuentas)
            {
                cuentas.Add(new SelectListItem { Text = item.CODIGO + "|" + item.NOMBRE, Value = item.CODIGO });
            }

            ViewBag.tiposComprobante = tiposComprobante;
            ViewBag.cuentas = cuentas;

            return View(FacConfiguracion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([System.Web.Http.FromBody] FacConfiguracion FacConfiguracion )
        {

            if (ModelState.IsValid)
            {
                db.Entry(FacConfiguracion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            List<SelectListItem> tiposComprobante = new List<SelectListItem>();
            List<SelectListItem> cuentas = new List<SelectListItem>();

            IList<TipoComprobante> listTiposComprobantes = db.TiposComprobantes.ToList();
            IList<CuentaMayor> listCuentas = db.PlanCuentas.Where(x => x.CODIGO.Length == 9).ToList();
            tiposComprobante.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            foreach (var item in listTiposComprobantes)
            {
                tiposComprobante.Add(new SelectListItem { Text = item.CODIGO + "|" + item.NOMBRE, Value = item.CODIGO });
            }
            cuentas.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            foreach (var item in listCuentas)
            {
                cuentas.Add(new SelectListItem { Text = item.CODIGO + "|" + item.NOMBRE, Value = item.CODIGO });
            }

            ViewBag.tiposComprobante = tiposComprobante;
            ViewBag.cuentas = cuentas;

            return View(FacConfiguracion);
        }

        [HttpPost]
        public JsonResult esCreateOEdit()
        {
            //int idd = Convert.ToInt32(id);
            var config = db.FacConfiguracion.Find(1);
            if (config == null)
            {
                return new JsonResult { Data = new { status = false } };

            }
            else
            {
                //return Json(1, JsonRequestBehavior.AllowGet);
                return new JsonResult { Data = new { status = true } };
            }

        }

        [HttpPost]
        public JsonResult verificaConfiguracion()
        {
            //int idd = Convert.ToInt32(id);
            var config = db.FacConfiguracion.Find(1);
            if (config == null)
            {
                return new JsonResult { Data = new { status = false } };

            }
            else
            {
                
                return new JsonResult { Data = new { status = true } };
            }

        }

    }
}