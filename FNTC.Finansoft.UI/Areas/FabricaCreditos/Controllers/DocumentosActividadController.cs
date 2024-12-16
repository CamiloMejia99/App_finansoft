using FNTC.Finansoft.Accounting.BLL.FabricaCreditosBll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.FabricaCreditos.Controllers
{
    public class DocumentosActividadController : Controller
    {
        // GET: FabricaCreditos/DocumentosActividad
        public ActionResult Index()
        {
            ViewBag.actividades = new ConfiguracionBll().obtenerActividades();
            ViewBag.documentos = new ConfiguracionBll().obtenerDocumentos();
            return View();
        }
        [HttpPost]
        public ActionResult Index(FormCollection frmDocs)
        {
            var idActividad = int.Parse(frmDocs.Get("selActividad"));
            var documentos = frmDocs.GetValues("documents[]");
            var respuesta = new ConfiguracionBll().guardarDocumentosPorActividad(documentos, idActividad);
            return RedirectToAction("Index");
        }

        public JsonResult ObtenerDocsPorActividad(string id)
        {
            var docs = new ConfiguracionBll().obtenerDocumentosPorActividad(id);
            return Json(docs, JsonRequestBehavior.AllowGet);
        }// DocsPorActividad
    }
}