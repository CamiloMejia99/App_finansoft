using FNTC.Finansoft.Accounting.BLL.FabricaCreditosBll;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.FabricaCreditos;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.FabricaCreditos.Views.Modificar
{
    public class ModificarInfController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: FabricaCredito Vista
        public ActionResult SolicitudesMod()

        {
            var pendiente = "Pendiente"; 
            var Operario = "Fase-N1-Op";
            return View(db.FCSolicitudes.Where(S => S.estado == Operario || S.estado == pendiente).ToList());
        }
        public ActionResult AgregarDoc()

        {
            
            return View(db.FCSolicitudes.Where(S => S.estado == "C-Aprobado").ToList());
        }
        public ActionResult AgregarDocumentacion(int id)
        {
            ViewBag.idSolicitud = id;
            ViewBag.PreEstado = new ConfiguracionBll().ObtenerPreEstado(id);
            ViewBag.EstadoRC = new ConfiguracionBll().ObtenerEstadoRC(id);
            ViewBag.EstadoDoc = new ConfiguracionBll().ObtenerEstadoDoc(id);
            ViewBag.Estado = new ConfiguracionBll().ObtenerEstados(id);
            return View(db.FCSolicitudes.Where(S => S.idSolicitud == id).ToList());

        }
        public ActionResult EditarPrestamo(int id)

        {
            ViewBag.idSolicitud = id;
            ViewBag.Salario = new ConfiguracionBll().ObtenerSalario(id);
            ViewBag.Estado = new ConfiguracionBll().ObtenerEstado(id);
            return View(db.FCSolicitudes.Where(S => S.idSolicitud == id).ToList());

        }
        public ActionResult EditarDatos(int idSolicitud)
        {
            try
            {
                using (var db = new AccountingContext())
                {

                    ViewBag.idSolicitud = idSolicitud;
                    FCSolicitudes EditarPres = db.FCSolicitudes.Where(a => a.idSolicitud == idSolicitud).FirstOrDefault();
                    return View(EditarPres);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult EditarDatos(FCSolicitudes fCSolicitudes, int idSolicitud) 
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    var idSoli = idSolicitud;
                    FCSolicitudes ANF = db.FCSolicitudes.Find(fCSolicitudes.idSolicitud);
                    ANF.idActividad = fCSolicitudes.idActividad;
                    ANF.idDependencia = fCSolicitudes.idDependencia;
                    ANF.idSede = fCSolicitudes.idSede;
                    ANF.idCentralDeRiesgo = fCSolicitudes.idCentralDeRiesgo;
                    ANF.idPrioridad = fCSolicitudes.idPrioridad;

                    db.SaveChanges();


                    return RedirectToAction("EditarPrestamo", new { id = idSoli });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult AnalisisFinancieroIE(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.idSolicitud = id;
                    FCSolicitudes AnaFin = db.FCSolicitudes.Where(a => a.idSolicitud == id).FirstOrDefault();
                    return View(AnaFin);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AnalisisFinancieroIE(FCSolicitudes AnFin, int idSolicitud /*FCTareas tarea*/)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    var idSoli = idSolicitud;
                    FCSolicitudes ANFI = db.FCSolicitudes.Find(AnFin.idSolicitud);
                    ANFI.Salario = AnFin.Salario;
                    ANFI.OtrosIngresos = AnFin.OtrosIngresos;
                    ANFI.Compras = AnFin.Compras;
                    ANFI.Descripcion = AnFin.Descripcion;
                    ANFI.Renta = AnFin.Renta;
                    ANFI.Prestamo = AnFin.Prestamo;
                    ANFI.Transporte = AnFin.Transporte;
                    ANFI.Alimentacion = AnFin.Alimentacion;
                    ANFI.RDO = AnFin.RDO;
                    ANFI.Servicios = AnFin.Servicios;
                    ANFI.IntPM = AnFin.IntPM;
                    ANFI.OtrosGastos = AnFin.OtrosGastos;
                    ANFI.DescripcionG = AnFin.DescripcionG;
                    ANFI.Ventas = AnFin.Ventas;
                    ANFI.IngresosNegocio = AnFin.IngresosNegocio;
                    ANFI.PrestamosCI = AnFin.PrestamosCI;
                    ANFI.Arriendo = AnFin.Arriendo;
                    ANFI.SueldoPS = AnFin.SueldoPS;
                    ANFI.ServiciosPublicos = AnFin.ServiciosPublicos;
                    ANFI.OtrosEgresos = AnFin.OtrosEgresos;
                    ANFI.DescripcionEgresos = AnFin.DescripcionEgresos;
                    ANFI.CajaBancos = AnFin.CajaBancos;
                    ANFI.Inversiones = AnFin.Inversiones;
                    ANFI.CuentasPorCobrar = AnFin.CuentasPorCobrar;
                    ANFI.Mercancias = AnFin.Mercancias;
                    ANFI.MueblesYEnseres = AnFin.MueblesYEnseres;
                    ANFI.Vehiculos = AnFin.Vehiculos;
                    ANFI.TerrenosYEdificios = AnFin.TerrenosYEdificios;
                    ANFI.OtrosActivos = AnFin.OtrosActivos;
                    ANFI.DescripcionActivos = AnFin.DescripcionActivos;
                    ANFI.Proveedores = AnFin.Proveedores;
                    ANFI.Obligaciones = AnFin.Obligaciones;
                    ANFI.OtrosPasivos = AnFin.OtrosPasivos;

                    db.SaveChanges();

                    //tarea.estadoAnalista = "Pendiente";
                    //db.FCTareas.Add(tarea);
                    //db.SaveChanges();



                    return RedirectToAction("EditarPrestamo", new { id = idSoli });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult ReferenciasCodeudor(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.idSolicitud = id;
                    var actividad = new ConfiguracionBll().ReferenciasCodeudorObtener(id);
                    var nReferencias = new int[] { actividad.nReferencias, actividad.nCodeudores };
                    ViewBag.nReferencias = nReferencias;

                    return View(db.DataReferenciasCodeudorFC.Where(S => S.IDSOLICITUD == id && S.INFADICIONAL == "Referencia").ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Codeudor(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.idSolicitud = id;
                    var actividad = new ConfiguracionBll().ReferenciasCodeudorObtener(id);
                    var nReferencias = new int[] { actividad.nReferencias, actividad.nCodeudores };
                    ViewBag.nReferencias = nReferencias;

                    return View(db.DataReferenciasCodeudorFC.Where(S => S.IDSOLICITUD == id && S.INFADICIONAL == "Codeudor").ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult DocumentacionP(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.idSolicitud = id;
                    ViewBag.idAsociado = new ConfiguracionBll().ObtenerAsociado(id);
                    return View(db.FCDocumentosAsociados.Where(a => a.idSolicitud == id && a.Estado != "Y" ).ToList());

                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult DocumentacionAgregar(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.idSolicitud = id;
                    ViewBag.idAsociado = new ConfiguracionBll().ObtenerAsociado(id);
                    return View(db.FCDocumentosAsociados.Where(a => a.idSolicitud == id && a.Estado != "Y").ToList());

                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult AgregarDocu(int id)
        {
            ViewBag.idSolicitud = id;
            ViewBag.idAsociado = new ConfiguracionBll().ObtenerAsociado(id);

            return View();
        }
        [HttpPost]
        public ActionResult AgregarDocu(HttpPostedFileBase file, FCDocumentosAsociados fCDocumentosAsociados, int id)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/File"), Path.GetFileNameWithoutExtension(file.FileName));
                    Guid Id = Guid.NewGuid();
                    string NombreArchivo = file.FileName;
                    var Ex = fCDocumentosAsociados.Extencion;
                    var direccion = (path + Id.ToString() + Ex);
                    file.SaveAs(path + Id.ToString() + Ex);

                    var idSoli = id;
                    fCDocumentosAsociados.direccionDocumento = direccion;
                    fCDocumentosAsociados.documentoVerificado = "Aprobada  ";
                    db.FCDocumentosAsociados.Add(fCDocumentosAsociados);
                    db.SaveChanges();

                    return RedirectToAction("DocumentacionAgregar", new { id = idSoli });

                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }



            return View(fCDocumentosAsociados);
        }
        public ActionResult CrearCode(int id)
        {
            ViewBag.idSolicitud = id;

            return View();
        }
        [HttpPost]
        public ActionResult CrearCode(DataReferenciasCodeudorFC dataReferenciasCodeudorFC, int id)

        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    var idSolicitu = id;
                    dataReferenciasCodeudorFC.INFADICIONAL = "Codeudor";
                    db.DataReferenciasCodeudorFC.Add(dataReferenciasCodeudorFC);
                    db.SaveChanges();
                    return RedirectToAction("Codeudor", new { id = idSolicitu });

                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult CrearRefeCode(int id)
        {
            ViewBag.idSolicitud = id;

            return View();
        }
        [HttpPost]
        public ActionResult CrearRefeCode(DataReferenciasCodeudorFC dataReferenciasCodeudorFC, int id)

        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    var idSolicitu = id;
                    dataReferenciasCodeudorFC.INFADICIONAL = "Referencia";
                    db.DataReferenciasCodeudorFC.Add(dataReferenciasCodeudorFC);
                    db.SaveChanges();
                    return RedirectToAction("ReferenciasCodeudor", new { id = idSolicitu });

                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult CrearDocu(int id)
        {
            ViewBag.idSolicitud = id;
            ViewBag.idAsociado = new ConfiguracionBll().ObtenerAsociado(id);

            return View();
        }
        [HttpPost]
        public ActionResult CrearDocu(HttpPostedFileBase file, FCDocumentosAsociados fCDocumentosAsociados, int id)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/File"), Path.GetFileNameWithoutExtension(file.FileName));
                    Guid Id = Guid.NewGuid();
                    string NombreArchivo = file.FileName;
                    var Ex = fCDocumentosAsociados.Extencion;
                    var direccion = (path + Id.ToString() + Ex);
                    file.SaveAs(path + Id.ToString() + Ex);

                    var idSoli = id;
                    fCDocumentosAsociados.direccionDocumento = direccion;
                    fCDocumentosAsociados.documentoVerificado = "-";
                    fCDocumentosAsociados.comentarios = "-";
                    db.FCDocumentosAsociados.Add(fCDocumentosAsociados);
                    db.SaveChanges();

                    return RedirectToAction("DocumentacionP", new { id = idSoli });

                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }



            return View(fCDocumentosAsociados);
        }
        public FileResult Imagen(string ruta)
        {
            var rutas = ruta;
            var ex = new ConfiguracionBll().ObtenerEX(ruta);
            var Nombre = new ConfiguracionBll().ObtenerNombre(ruta);
            return File(rutas, "aplication/" + ex, "*" + Nombre + ex);
        }
        public ActionResult CambioEstado(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {

                    ViewBag.idSolicitud = id;
                    ViewBag.idAsociado = new ConfiguracionBll().ObtenerAsociado(id);
                    FCSolicitudes Solicitu = db.FCSolicitudes.Where(a => a.idSolicitud == id).FirstOrDefault();
                    return View(Solicitu);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult CambioEstado(FCSolicitudes fCSolicitudes, FCPasosAp fCPasosAp, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    var IDd = id;
                    var Idaso = 0;
                    var IdSolid = 0;
                    FCSolicitudes FaseI = db.FCSolicitudes.Find(fCSolicitudes.idSolicitud);
                    Idaso = fCSolicitudes.idAsociado;
                    IdSolid = fCSolicitudes.idSolicitud;
                    FaseI.estado = fCSolicitudes.estado;
                    db.SaveChanges();

                    fCPasosAp.EstadoOp = "Face-1";
                    fCPasosAp.EstadoAnRC = "---";
                    fCPasosAp.EstadoAnDoc = "---";
                    fCPasosAp.ComentarioAnRC = "---";
                    fCPasosAp.ComentarioAnDoc = "---";


                    fCPasosAp.idsolicitud = IdSolid;
                    fCPasosAp.idasociado = Idaso;
                    db.FCPasosAp.Add(fCPasosAp);
                    db.SaveChanges();

                    return RedirectToAction("EditarPrestamo", new { id = IdSolid });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult EliminarDoc(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.idSolicitud = new ConfiguracionBll().ObtenerSolicitudE(id);
                    FCDocumentosAsociados document = db.FCDocumentosAsociados.Where(a => a.idDoc == id).FirstOrDefault();
                    return View(document);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult EliminarDoc(FCDocumentosAsociados fCDocumentosAsociados)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    
                    FCDocumentosAsociados Eliminar = db.FCDocumentosAsociados.Find(fCDocumentosAsociados.idDoc);
                   
                    Eliminar.Estado = "Y";
                    var IdSolid = fCDocumentosAsociados.idSolicitud;
                    Eliminar.MotivosEliminacion = fCDocumentosAsociados.MotivosEliminacion;
                    db.SaveChanges();


                    return RedirectToAction("DocumentacionP", new { id = IdSolid });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public ActionResult EditarRC(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.IdRC = id;
                    ViewBag.IdSolicitud = new ConfiguracionBll().ObtenerSolicitud(id); 
                    DataReferenciasCodeudorFC Modif = db.DataReferenciasCodeudorFC.Where(a => a.IDREFCODE == id).FirstOrDefault();
                    return View(Modif);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult EditarRC(DataReferenciasCodeudorFC dataReferenciasCodeudorFC, int IdSolicitud)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    
                    DataReferenciasCodeudorFC Mod = db.DataReferenciasCodeudorFC.Find(dataReferenciasCodeudorFC.IDREFCODE);
                    var idSolicitud = IdSolicitud;
                    Mod.PARENTESCO = dataReferenciasCodeudorFC.PARENTESCO;
                    Mod.NOMBRE1 = dataReferenciasCodeudorFC.NOMBRE1;
                    Mod.NOMBRE2 = dataReferenciasCodeudorFC.NOMBRE2;
                    Mod.APELLIDO1 = dataReferenciasCodeudorFC.APELLIDO1;
                    Mod.APELLIDO2 = dataReferenciasCodeudorFC.APELLIDO2;
                    Mod.EDAD = dataReferenciasCodeudorFC.EDAD;
                    Mod.TELEFONO = dataReferenciasCodeudorFC.TELEFONO;
                    Mod.CELULAR = dataReferenciasCodeudorFC.CELULAR;
                    Mod.DIRECCIONRESIDENCIA = dataReferenciasCodeudorFC.DIRECCIONRESIDENCIA;
                    Mod.ACTIVIDADECONOMICA = dataReferenciasCodeudorFC.ACTIVIDADECONOMICA;
                    Mod.TIEMPODECONOCER = dataReferenciasCodeudorFC.TIEMPODECONOCER;

                    db.SaveChanges();


                    return RedirectToAction("ReferenciasCodeudor", new { id = idSolicitud });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult EditarCodeudor(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.IdRC = id;
                    ViewBag.IdSolicitud = new ConfiguracionBll().ObtenerSolicitud(id);
                    DataReferenciasCodeudorFC Modif = db.DataReferenciasCodeudorFC.Where(a => a.IDREFCODE == id).FirstOrDefault();
                    return View(Modif);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult EditarCodeudor(DataReferenciasCodeudorFC dataReferenciasCodeudorFC, int IdSolicitud)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {

                    DataReferenciasCodeudorFC Mod = db.DataReferenciasCodeudorFC.Find(dataReferenciasCodeudorFC.IDREFCODE);
                    var idSolicitud = IdSolicitud;
                    Mod.APELLIDO1 = dataReferenciasCodeudorFC.APELLIDO1;
                    Mod.APELLIDO2 = dataReferenciasCodeudorFC.APELLIDO2;
                    Mod.NOMBRE1 = dataReferenciasCodeudorFC.NOMBRE1;
                    Mod.NOMBRE2 = dataReferenciasCodeudorFC.NOMBRE2;
                    Mod.TIPODEDOCUMENTO = dataReferenciasCodeudorFC.TIPODEDOCUMENTO;
                    Mod.NIT = dataReferenciasCodeudorFC.NIT;
                    Mod.LUGARDEEXPEDICION = dataReferenciasCodeudorFC.LUGARDEEXPEDICION;
                    Mod.TIEMPODECONOCER = dataReferenciasCodeudorFC.TIEMPODECONOCER;
                    Mod.LUGARDENACIMIENTO = dataReferenciasCodeudorFC.LUGARDENACIMIENTO;
                    Mod.EDAD = dataReferenciasCodeudorFC.EDAD;
                    Mod.DIRECCIONRESIDENCIA = dataReferenciasCodeudorFC.DIRECCIONRESIDENCIA;
                    Mod.PARENTESCO = dataReferenciasCodeudorFC.PARENTESCO;
                    Mod.TELEFONO = dataReferenciasCodeudorFC.TELEFONO;
                    Mod.CELULAR = dataReferenciasCodeudorFC.CELULAR;
                    Mod.INGRESOSMENSUALES = dataReferenciasCodeudorFC.INGRESOSMENSUALES;
                    Mod.EGRESOSMENSUALES = dataReferenciasCodeudorFC.EGRESOSMENSUALES;
                    Mod.ACTIVIDADECONOMICA = dataReferenciasCodeudorFC.ACTIVIDADECONOMICA;
                    Mod.DESCRIPCIONDEBIENES = dataReferenciasCodeudorFC.DESCRIPCIONDEBIENES;
                    Mod.CANTIDAD = dataReferenciasCodeudorFC.CANTIDAD;
                    Mod.VALOR = dataReferenciasCodeudorFC.VALOR;
                    
                    db.SaveChanges();


                    return RedirectToAction("Codeudor", new { id = idSolicitud });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}