using FNTC.Finansoft.Accounting.BLL;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.Documentos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace FNTC.Finansoft.UI.Areas.Documentos.Controllers
{
    public class DocumentoSoporteController : Controller
    {
        AccountingContext db = new AccountingContext();
        // GET: Documentos/DocumentoSoporte
        public ActionResult Index()
        {
            var data = db.Comprobantes.Where(x => x.TIPO.StartsWith("DS")).ToList();
            return View(data);
        }

        public ActionResult IndexConfiguracion()
        {
            var data = db.ConfigDocumentoSoporte.ToList();
            return View(data);
        }

        [HttpPost]
        public JsonResult VerificaConfiguracion()
        {

            var data = db.ConfigDocumentoSoporte.ToList();
            if (data.Count > 0)
            {
                return new JsonResult { Data = new { status = true } };

            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }

        }

        

        public ActionResult CreateConfiguracion()
        {
            var tiposComprobantes = GetTiposComprobante(1);

            ViewBag.tiposComprobantes = tiposComprobantes;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfiguracion([System.Web.Http.FromBody] ConfigDocumentoSoporte ConfigDocuemntoSoporte)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new AccountingContext())
                {
                    try
                    {
                        ConfigDocuemntoSoporte.estado = true;
                        ctx.ConfigDocumentoSoporte.Add(ConfigDocuemntoSoporte);
                        ctx.SaveChanges();
                        return RedirectToAction("IndexConfiguracion");
                    }
                    catch (DbEntityValidationException dbE)
                    {

                    }
                }
            }

            var tiposComprobantes = GetTiposComprobante(1);

            ViewBag.tiposComprobantes = tiposComprobantes;
            return View(ConfigDocuemntoSoporte);
        }

        public ActionResult EditConfiguracion(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.ConfigDocumentoSoporte.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }

            var tiposComprobantes = GetTiposComprobante(2);

            ViewBag.tiposComprobantes = tiposComprobantes;
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditConfiguracion([System.Web.Http.FromBody] ConfigDocumentoSoporte ConfigDocumentoSoporte)
        {

            if (ModelState.IsValid)
            {
                db.Entry(ConfigDocumentoSoporte).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("IndexConfiguracion");
            }

            var tiposComprobantes = GetTiposComprobante(2);

            ViewBag.tiposComprobantes = tiposComprobantes;
            return View(ConfigDocumentoSoporte);

        }

        public ActionResult GetDocumentoSoporte(string tipo, string numero)
        {
            //debio construit un comporbante BO
            var comprobante = new FNTC.Finansoft.Accounting.BLL.Comprobantes.ComprobantesBLL().GetComprobante(tipo, numero);
            Session["pruebas"] = comprobante;
            TempData["comprobante"] = comprobante;
            //return new Areas.Accounting.Controllers.Movimientos.MovimientosController().Pruebas();
            return RedirectToAction("../Accounting/Movimientos/GetDocumentoSoporte", new { output = "pdf" });
        }

        public List<SelectListItem> GetTiposComprobante(int opcion)
        {
            
            var DS = db.ConfigDocumentoSoporte.ToList();
            List<SelectListItem> tipos = new List<SelectListItem>();
            tipos.Add(new SelectListItem { Text = "-Seleccione--", Value = "" });
            IList<TipoComprobante> listado = db.TiposComprobantes.Where(x => x.CLASEComprobante == "DS").ToList();
            
                if(opcion==1)
                {
                    foreach (var item in listado)
                    {
                        var bandera = DS.Where(x => x.tipoComprobante == item.CODIGO).FirstOrDefault();
                        if (bandera == null)
                        {
                            tipos.Add(new SelectListItem { Text = item.CODIGO + " | " + item.NOMBRE, Value = item.CODIGO });
                        }

                    }
                }else if(opcion==2)
                {
                    foreach (var item in listado)
                    {
                       tipos.Add(new SelectListItem { Text = item.CODIGO + " | " + item.NOMBRE, Value = item.CODIGO });
                    }
                }
                

            

            return tipos;
        }

        [HttpPost]
        public JsonResult VerificaTipoComprobante(string tipo)
        {

            var data = db.ConfigDocumentoSoporte.Where(x => x.tipoComprobante == tipo).FirstOrDefault();
            if (data!=null)
            {
                return new JsonResult { Data = new { status = true } };

            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }

        }
    }
}