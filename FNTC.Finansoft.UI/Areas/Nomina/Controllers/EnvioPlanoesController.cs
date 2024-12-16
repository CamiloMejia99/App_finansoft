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
using System.IO;
using FNTC.Finansoft.Accounting.BLL.FabricaCreditosBll;

namespace FNTC.Finansoft.UI.Areas.Nomina
{
    public class EnvioPlanoesController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Nomina/EnvioPlanoes
        
        public ActionResult VistaEnvioPlanos()
        {
            try
            {
                using (var db = new AccountingContext())
                {

                    return View(db.EnvioPlano.ToList());

                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase file, EnvioPlano envioPlano)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/File"), Path.GetFileNameWithoutExtension(file.FileName));
                    Guid Id = Guid.NewGuid();
                    string NombreArchivo = file.FileName;
                    var Ex = envioPlano.Extencion;
                    var direccion = (path + Id.ToString() + Ex);
                    file.SaveAs(path + Id.ToString() + Ex);

                    envioPlano.DireccionPlano = direccion;
                    envioPlano.NombreArchivoPlano = envioPlano.NombreArchivoPlano;
                    envioPlano.Extencion = envioPlano.Extencion;
                    db.EnvioPlano.Add(envioPlano);
                    db.SaveChanges();

                    return RedirectToAction("VistaEnvioPlanos");

                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "No ha especificado un archivo.";
            }



            return View(envioPlano);
        }
        public FileResult Imagen(string ruta)
        {
            var rutas = ruta;
            var ex = new ConfiguracionBll().ObtenerEXTENCION(ruta);
            var Nombre = new ConfiguracionBll().ObtenerNombres(ruta);
            return File(rutas, "aplication/" + ex, "*" + Nombre + ex);
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
