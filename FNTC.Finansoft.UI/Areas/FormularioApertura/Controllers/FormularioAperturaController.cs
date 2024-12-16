using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.FormularioAperturaCtaAhorros;
using FNTC.Finansoft.UI.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.FormularioApertura.Controllers
{
    [Authorize]//autorizacion en login

  
    public class FormularioAperturaController : Controller
    {
        private AccountingContext db = new AccountingContext();

        public List<SelectListItem> Terceros = new List<SelectListItem>();

        public string botonInfo = "<a CLASS='btn btn-inline btn-xs btn-success fa fa-download OPCIONES' title='Descargar'onclick='DescargarFormulario(this)'></a>&nbsp;&nbsp;" +
                        "<a CLASS='btn btn-inline btn-xs btn-primary fa fa-pencil OPCIONES' title='Editar' onclick='EditarFormulario(this)'></a>";

        // GET: FormularioApertura/FormularioApertura
        public ActionResult Index()
        {
            ViewBag.Terceros = new SelectList((from m in db.Terceros
                                               select new { value = m.NIT, text = m.NIT + " || " + m.NombreComercial + " " + m.NOMBRE1 + " " + m.NOMBRE2 + " " + m.APELLIDO1 + " " + m.APELLIDO2 }).ToList(), "value", "text");
            return View();
        }

        public FileResult FormularioEnBlanco()
        {
            var rutas = System.AppDomain.CurrentDomain.BaseDirectory + "File/FormatoAperturaCuentaAhorros.pdf";
            var ex = ".pdf";
            return File(rutas, "aplication/" + ex, "FormatoAperturaCuentaAhorros" + ex);
        }

        public FileResult DescargarFormularioAsociado(string documento)
        {
            var formulario = (from a in db.FormulariosApertura
                              where a.IdTercero == documento
                              select new { a.NombreArchivoPdf, a.TokenPdf }).FirstOrDefault();

            var extension = Path.GetExtension(formulario.TokenPdf);

            var rutas = System.AppDomain.CurrentDomain.BaseDirectory + "File/FormulariosAperturaCta/" + formulario.TokenPdf;

            return File(rutas, "aplication/" + extension, formulario.NombreArchivoPdf + extension);

        }

        public ActionResult NuevoFormulario()
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
                var buscar = db.FormulariosApertura.Where(x => x.IdTercero == documento).Any();
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
                    string pathAux = System.AppDomain.CurrentDomain.BaseDirectory + "File/FormulariosAperturaCta";
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
                        var formulario = new FormulariosApertura()
                        {
                            IdTercero = documento,
                            TokenPdf = nombreBD,
                            NombreArchivoPdf = _nombre,
                            FechaApertura = fechaAfiliacion,
                            FechaSistema = fechaActual,
                            UserLog = user,
                            Estado = true
                        };

                        db.FormulariosApertura.Add(formulario);
                        db.SaveChanges();
                        archivo.SaveAs(nombreToken);
                        TempData["exito"] = "Formulario guardado con éxito";
                        return RedirectToAction("/NuevoFormulario");
                    }
                    else
                    {
                        TempData["error"] = "Ha ocurrido un error, vuelva a cargar el documento, verifique que el archivo sea el adecuado";
                        return RedirectToAction("/NuevoFormulario");
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
                    return RedirectToAction("/NuevoFormulario");
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = "Ha ocurrido un error, vuelva a cargar el documento";
                return RedirectToAction("/NuevoFormulario");
            }
        }


        [HttpPost]
        public JsonResult GetFormularios(string documento, string idFechaDesde, string idFechaHasta)
        {
            var formularios = (from a in db.FormulariosApertura
                               join b in db.Terceros on a.IdTercero equals b.NIT
                               where a.IdTercero == documento
                               orderby a.FechaApertura
                               select new { a.IdTercero, b.NOMBRE, b.APELLIDO1, b.APELLIDO2, a.FechaApertura }).ToList();
            var botones = "";
            var result = formularios.Select(
                (x, index) => new[]
                {
                        x.IdTercero,
                        x.NOMBRE,
                        x.FechaApertura.ToShortDateString(),
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

        public ActionResult GetForDocumento(string documento)
        {

            var formularios = (from a in db.FormulariosApertura
                               join b in db.Terceros on a.IdTercero equals b.NIT
                               where a.IdTercero == documento
                               orderby a.FechaApertura
                               select new { a.IdTercero, b.NOMBRE, b.APELLIDO1, b.APELLIDO2, a.FechaApertura }).ToList();
            var botones = "";
            var result = formularios.Select(
                (x, index) => new[]
                {
                        x.IdTercero,
                        x.NOMBRE+" "+x.APELLIDO1+" "+x.APELLIDO2,
                        x.FechaApertura.ToShortDateString(),
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

            var formularios = (from a in db.FormulariosApertura
                               join b in db.Terceros on a.IdTercero equals b.NIT
                               where a.FechaApertura >= Desde && a.FechaApertura <= fechHasta
                               orderby a.FechaApertura
                               select new { a.IdTercero, b.NOMBRE, b.APELLIDO1, b.APELLIDO2, a.FechaApertura }).ToList();
            var botones = "";
            var result = formularios.Select(
                (x, index) => new[]
                {
                        x.IdTercero,
                        x.NOMBRE+" "+x.APELLIDO1+" "+x.APELLIDO2,
                        x.FechaApertura.ToShortDateString(),
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

            var formularios = (from a in db.FormulariosApertura
                               join b in db.Terceros on a.IdTercero equals b.NIT
                               where a.IdTercero == documento && a.FechaApertura >= Desde && a.FechaApertura <= fechHasta
                               orderby a.FechaApertura
                               select new { a.IdTercero, b.NOMBRE, b.APELLIDO1, b.APELLIDO2, a.FechaApertura,}).ToList();
            var botones = "";
            var result = formularios.Select(
                (x, index) => new[]
                {
                        x.IdTercero,
                        x.NOMBRE+" "+x.APELLIDO1+" "+x.APELLIDO2,
                        x.FechaApertura.ToShortDateString(),
                        botones = botonInfo

                });

            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = 500000000;
            var json = Json(new { data = result }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;

        }

        public ActionResult Editar(string documento)
        {
            try
            {
                var nombreCompleto = db.Terceros.Where(x => x.NIT == documento).Select(x => x.NOMBRE1 + " " + x.NOMBRE2 + " " + x.APELLIDO1 + " " + x.APELLIDO2).First();
                var Fecha = db.FormulariosApertura.Where(x => x.IdTercero == documento).FirstOrDefault().FechaApertura.ToString("yyyy-MM-dd");
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

        [HttpPost]
        public ActionResult EditarFormulario(string documento, string fechaAf, HttpPostedFileBase archivo)
        {
            try
            {
                DateTime fechaActual = Fecha.GetFechaColombia();
                DateTime fechaApertura;
                if (fechaAf != "")
                    fechaApertura = Convert.ToDateTime(fechaAf);
                else
                    fechaApertura = fechaActual;

                string pathAux = System.AppDomain.CurrentDomain.BaseDirectory + "File/FormulariosAperturaCta";
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
                    var formulario = (from a in db.FormulariosApertura
                                      where a.IdTercero == documento
                                      select new { a.NombreArchivoPdf, a.TokenPdf, a.Id }).FirstOrDefault();

                    var ruta = System.AppDomain.CurrentDomain.BaseDirectory + "File/FormulariosAperturaCta/" + formulario.TokenPdf;
                    if (System.IO.File.Exists(ruta))
                    {
                        System.IO.File.Delete(ruta);
                    }

                    var aux = Path.GetFileNameWithoutExtension(formulario.TokenPdf);
                    var nuevoToken = aux + "." + _extension;

                    FormulariosApertura form = db.FormulariosApertura.Find(formulario.Id);
                    
                    form.TokenPdf = nuevoToken;
                    form.NombreArchivoPdf = _nombre;
                    form.FechaApertura = fechaApertura;
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


    }
    

}