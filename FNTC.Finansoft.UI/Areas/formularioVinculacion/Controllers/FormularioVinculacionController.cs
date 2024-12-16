using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.FormularioVinculacion;
using FNTC.Finansoft.UI.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.Accounting.BLL.FormulariosSolicitud;
using FNTC.Finansoft.Accounting.DTO.FormulariosSolicitud;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using FNTC.Finansoft.Accounting.DTO.Geo;
using FNTC.Finansoft.Accounting.DTO.FormulariosSolicitudCredito;
using System.Configuration;
using System.Data.SqlClient;

namespace FNTC.Finansoft.UI.Areas.formularioVinculacion.Controllers
{
    [Authorize]//autorizacion en login
    public class FormularioVinculacionController : Controller
    {
        // GET: formularioVinculacion/FormularioVinculacion

        private AccountingContext db = new AccountingContext();

        public List<SelectListItem> Terceros = new List<SelectListItem>();

        public string botonInfo = "<a CLASS='btn btn-inline btn-xs btn-success fa fa-download OPCIONES' title='Descargar'onclick='DescargarFormulario(this)'></a>&nbsp;&nbsp;" +
                        "<a CLASS='btn btn-inline btn-xs btn-primary fa fa-pencil OPCIONES' title='Editar' onclick='EditarFormulario(this)'></a>";

        public ActionResult Index()
        {
            ViewBag.Terceros = new SelectList((from m in db.Terceros
                                               select new { value = m.NIT, text = m.NIT + " || " + m.NombreComercial + " " + m.NOMBRE1 + " " + m.NOMBRE2 + " " + m.APELLIDO1 + " " + m.APELLIDO2 }).ToList(), "value", "text");
            return View();
        }

        public FileResult FormularioVacio()
        {
            var rutas = System.AppDomain.CurrentDomain.BaseDirectory + "File/Solicitud_Afiliacion.pdf";
            var ex = ".pdf";
            return File(rutas, "aplication/" + ex, "Solicitud_Afiliacion" + ex);
        }


        public FileResult DescargarFormularioAsociado(string documento)
        {
            var formulario = (from a in db.FormularioSolicitudAfiliacion
                              where a.IdTercero == documento
                              select new { a.NombreArchivoPdf, a.TokenPdf }).FirstOrDefault();

            var extension = Path.GetExtension(formulario.TokenPdf);

            var rutas = System.AppDomain.CurrentDomain.BaseDirectory + "File/FormulariosAfiliacion_Aportes/" + formulario.TokenPdf;

            return File(rutas, "aplication/" + extension, formulario.NombreArchivoPdf + extension);

        }

        [HttpPost]
        public ActionResult EditarFormulario(string documento, string fechaAf, HttpPostedFileBase archivo)
        {
            try
             {
                DateTime fechaActual = Fecha.GetFechaColombia();
                DateTime fechaAfiliacion;
                if (fechaAf != "")
                    fechaAfiliacion = Convert.ToDateTime(fechaAf);
                else
                    fechaAfiliacion = fechaActual;

                string pathAux = System.AppDomain.CurrentDomain.BaseDirectory + "File/FormulariosAfiliacion_Aportes";
                var ext = false;
                var _contenido = new byte[archivo.ContentLength];
                archivo.InputStream.Read(_contenido, 0, archivo.ContentLength);
                int indiceDelUltimoPunto = archivo.FileName.LastIndexOf('.');
                string _nombre = archivo.FileName.Substring(0, indiceDelUltimoPunto);
                string _extension = archivo.FileName.Substring(indiceDelUltimoPunto + 1,
                                    archivo.FileName.Length - indiceDelUltimoPunto - 1);

                if (_extension == "pdf" || _extension == "docx")
                    ext = true;

                if (ext)
                {
                    var formulario = (from a in db.FormularioSolicitudAfiliacion
                                      where a.IdTercero == documento
                                      select new { a.NombreArchivoPdf, a.TokenPdf, a.Id }).FirstOrDefault();

                    var ruta = System.AppDomain.CurrentDomain.BaseDirectory + "File/FormulariosAfiliacion_Aportes/" + formulario.TokenPdf;
                    if (System.IO.File.Exists(ruta))
                    {
                        System.IO.File.Delete(ruta);
                    }

                    var aux = Path.GetFileNameWithoutExtension(formulario.TokenPdf);
                    var nuevoToken = aux + "." + _extension;

                    FormularioSolicitudAfiliacion form = db.FormularioSolicitudAfiliacion.Find(formulario.Id);

                    form.TokenPdf = nuevoToken;
                    form.NombreArchivoPdf = _nombre;
                    form.FechaSolicitud = fechaAfiliacion;
                    form.FechaSistema = fechaActual;

                    string nombreToken = Path.Combine(pathAux, nuevoToken); //ruta y nombre con el que se guarda en el servidor
                    archivo.SaveAs(nombreToken);
                    db.SaveChanges();

                    TempData["exito"] = "Formulario actualizado con éxito";
                    return RedirectToAction("/Index");
                }
                else
                {
                    TempData["error"] = "Error al actualizar el formulario (formato incorrecto)";
                    return RedirectToAction("/Index");

                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Ha ocurrido un error en la actualización";
                return RedirectToAction("/Index");
            }
        }

        public ActionResult Editar(string documento)
        {
            try
            {
                var nombreCompleto = db.Terceros.Where(x => x.NIT == documento).Select(x => x.NOMBRE1 + " " + x.NOMBRE2 + " " + x.APELLIDO1 + " " + x.APELLIDO2).First();
                var Fecha = db.FormularioSolicitudAfiliacion.Where(x => x.IdTercero == documento).FirstOrDefault().FechaSolicitud.ToString("yyyy-MM-dd");
                TempData["documento"] = documento;
                TempData["nombre"] = nombreCompleto;
                ViewBag.Fecha = Fecha;
                return PartialView();
            }
            catch
            {
                TempData["error"] = "Ha ocurrido un error ";
                return RedirectToAction("/Index");
            }

        }

        public ActionResult NuevaSolicitud()
        {
            return View();
        }


        [HttpPost]

        public JsonResult BuscarTercero(string documento)
        {
            var nombreCompleto = "";
            var buscar = db.Terceros.Where(x => x.NIT == documento).Any();
            if (buscar)
            {
                nombreCompleto = db.Terceros.Where(x => x.NIT == documento).Select(x => x.NOMBRE1 + " " + x.NOMBRE2 + " " + x.APELLIDO1 + " " + x.APELLIDO2).First();
            }
            if (buscar)
                return new JsonResult { Data = new { status = true, nombre = "Datos Encontrados: " + nombreCompleto } };

            else
                return new JsonResult { Data = new { status = false } };
        }


        [HttpPost]
        public ActionResult GuardarFormularios(string documento, string fechaAf, HttpPostedFileBase archivo)
        {
            try
            {
                var ext = false;
                var buscar = db.FormularioSolicitudAfiliacion.Where(x => x.IdTercero == documento).Any();
                var _contenido = new byte[archivo.ContentLength];
                archivo.InputStream.Read(_contenido, 0, archivo.ContentLength);

                int indiceDelUltimoPunto = archivo.FileName.LastIndexOf('.');
                string _nombre = archivo.FileName.Substring(0, indiceDelUltimoPunto);
                string _extension = archivo.FileName.Substring(indiceDelUltimoPunto + 1,
                                    archivo.FileName.Length - indiceDelUltimoPunto - 1);

                if (_extension == "pdf" || _extension == "docx")
                    ext = true;

                if (!buscar && ext)
                {
                    var Id = Guid.NewGuid().ToString();
                    DateTime fechaActual = Fecha.GetFechaColombia();
                    DateTime fechaAfiliacion;
                    var user = User.Identity.Name;
                    string pathAux = System.AppDomain.CurrentDomain.BaseDirectory + "File/FormulariosAfiliacion_Aportes";
                    if (fechaAf == "")
                        fechaAfiliacion = fechaActual;
                    else
                        fechaAfiliacion = Convert.ToDateTime(fechaAf);

                    var nombreCompleto = db.Terceros.Where(x => x.NIT == documento).Select(x => x.NOMBRE1 + " " + x.NOMBRE2 + " " + x.APELLIDO1 + " " + x.APELLIDO2).First();

                    if (archivo != null && archivo.ContentLength > 0)
                    {
                        var ex = "." + _extension;
                        string nombreToken = Path.Combine(pathAux, Id + ex); //ruta y nombre con el que se guarda en el servidor
                        var nombreBD = Id + ex;
                        var formulario = new FormularioSolicitudAfiliacion()
                        {
                            IdTercero = documento,
                            TokenPdf = nombreBD,
                            NombreArchivoPdf = _nombre,
                            FechaSolicitud = fechaAfiliacion,
                            FechaSistema = fechaActual,
                            UserLog = user,
                            Estado = true
                        };

                        db.FormularioSolicitudAfiliacion.Add(formulario);
                        db.SaveChanges();
                        archivo.SaveAs(nombreToken);
                        TempData["exito"] = "Formulario guardado con éxito";
                        return RedirectToAction("/NuevaSolicitud");
                    }
                    else
                    {
                        TempData["error"] = "Ha ocurrido un error, vuelva a cargar el documento, verifique que el archivo sea el adecuado";
                        return RedirectToAction("/NuevaSolicitud");
                    }
                }
                else
                {
                    if (buscar)
                    {
                        TempData["error"] = "El asociado ya tiene un formulario de solicitud cargado en el sistema";
                    }
                    else if (!ext)
                    {
                        TempData["error"] = "El documento debe ser PDF o WORD";
                    }
                    return RedirectToAction("/NuevaSolicitud");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Ha ocurrido un error, vuelva a cargar el documento";
                return RedirectToAction("/NuevaSolicitud");
            }
        }


        public ActionResult GetForDocumento(string documento)
        {

            var formularios = (from a in db.FormularioSolicitudAfiliacion
                               join b in db.Terceros on a.IdTercero equals b.NIT
                               where a.IdTercero == documento
                               orderby a.FechaSolicitud
                               select new { a.IdTercero, b.NOMBRE, b.APELLIDO1, b.APELLIDO2, a.FechaSolicitud, a.Estado }).ToList();
            var botones = "";
            var result = formularios.Select(
                (x, index) => new[]
                {
                        x.IdTercero,
                        x.NOMBRE+" "+x.APELLIDO1+" "+x.APELLIDO2,
                        x.FechaSolicitud.ToShortDateString(),
                        botones = botonInfo
                });

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = 500000000;
            var json = Json(new { data = result }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;
        }


        public ActionResult GetForFecha(string idFechaDesde, string idFechaHasta)
        {
            DateTime Desde = Convert.ToDateTime(idFechaDesde);
            DateTime fechHasta = Convert.ToDateTime(idFechaHasta);

            var formularios = (from a in db.FormularioSolicitudAfiliacion
                               join b in db.Terceros on a.IdTercero equals b.NIT
                               where a.FechaSolicitud >= Desde && a.FechaSolicitud <= fechHasta
                               orderby a.FechaSolicitud
                               select new { a.IdTercero, b.NOMBRE, b.APELLIDO1, b.APELLIDO2, a.FechaSolicitud, a.Estado }).ToList();
            var botones = "";
            var result = formularios.Select(
                (x, index) => new[]
                {
                        x.IdTercero,
                        x.NOMBRE+" "+x.APELLIDO1+" "+x.APELLIDO2,
                        x.FechaSolicitud.ToShortDateString(),
                        botones =  botonInfo

                });

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = 500000000;
            var json = Json(new { data = result }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;

        }

        public ActionResult GetForFechaDocumento(string documento, string idFechaDesde, string idFechaHasta)
        {
            DateTime Desde = Convert.ToDateTime(idFechaDesde);
            DateTime fechHasta = Convert.ToDateTime(idFechaHasta);

            var formularios = (from a in db.FormularioSolicitudAfiliacion
                               join b in db.Terceros on a.IdTercero equals b.NIT
                               where a.IdTercero == documento && a.FechaSolicitud >= Desde && a.FechaSolicitud <= fechHasta
                               orderby a.FechaSolicitud
                               select new { a.IdTercero, b.NOMBRE, b.APELLIDO1, b.APELLIDO2, a.FechaSolicitud, a.Estado }).ToList();
            var botones = "";
            var result = formularios.Select(
                (x, index) => new[]
                {
                        x.IdTercero,
                        x.NOMBRE+" "+x.APELLIDO1+" "+x.APELLIDO2,
                        x.FechaSolicitud.ToShortDateString(),
                        botones = botonInfo

                });

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = 500000000;
            var json = Json(new { data = result }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;

        }

        [HttpPost]
        public JsonResult GetFormularios(string documento, string idFechaDesde, string idFechaHasta)
        {
            var formularios = (from a in db.FormularioSolicitudAfiliacion
                               join b in db.Terceros on a.IdTercero equals b.NIT
                               where a.IdTercero == documento
                               orderby a.FechaSolicitud
                               select new { a.IdTercero, b.NOMBRE, b.APELLIDO1, b.APELLIDO2, a.FechaSolicitud, a.Estado }).ToList();
            var botones = "";
            var result = formularios.Select(
                (x, index) => new[]
                {
                        x.IdTercero,
                        x.NOMBRE,
                        x.FechaSolicitud.ToShortDateString(),
                        botones = botonInfo
                });

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = 500000000;
            var json = Json(new { data = result }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;

            if (documento != "" && idFechaDesde != "" && idFechaHasta != "")
            {
                json = (JsonResult)GetForFechaDocumento(documento, idFechaDesde, idFechaHasta);
            }
            else if (documento == "" && idFechaDesde != "" && idFechaHasta != "")
            {
                json = (JsonResult)GetForFecha(idFechaDesde, idFechaHasta);
            }
            else if (documento != "")
            {
                json = (JsonResult)GetForDocumento(documento);
            }
            return json;
        }
    }
}