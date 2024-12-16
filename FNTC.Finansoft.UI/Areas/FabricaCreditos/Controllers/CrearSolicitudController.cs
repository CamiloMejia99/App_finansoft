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

namespace FNTC.Finansoft.UI.Areas.FabricaCreditos.Controllers
{
    [Authorize]
    public class CrearSolicitudController : Controller
    {
        private AccountingContext db = new AccountingContext();
        
        // GET: FabricaCredito Vista
        public ActionResult Solicitudes()

        {
            return View(db.FCSolicitudes.ToList());
        }


        public JsonResult VerificarEstadoAsociado(string idAsociado)
        {
            var mensaje = "existente";
            var estadoFabrica = new ConfiguracionBll().obtenerConfiguracionPredeterminada();
            if (estadoFabrica == null)
            {
                mensaje = "SinConfi";
            }
            else
            {
                var consulta2 = new ConfiguracionBll().ObtenerNombresAsociado(idAsociado);
                if (consulta2 == null)
                {
                    mensaje = "inexistente";
                }
                else
                {
                    var consulta = new ConfiguracionBll().TieneSolicitudesPendientes(idAsociado);
                    if (consulta)
                    {
                        mensaje = "ExisteSolicitud";
                    }
                    else
                    {
                        mensaje = consulta2;
                    }
                }
            }
            return Json(mensaje, JsonRequestBehavior.AllowGet);
        }// VerificarEstadoAsociado
        //++++++ Solicitudes
        public ActionResult ListaCreditos(string id)
        {
            var idcre = (id);
            var idPrestamosSolicitud = true;

            using (var db = new AccountingContext())
            {
                return PartialView(db.Prestamos.Where(x => x.NIT == idcre && x.estado != idPrestamosSolicitud).ToList());
            }
        }
        public ActionResult ListaActividad()
        {
            using (var db = new AccountingContext())
            {
                return PartialView(db.FCActividades.ToList());
            }
        }
        public ActionResult ListaCentral()
        {
            using (var db = new AccountingContext())
            {
                return PartialView(db.CentralRiesgo.ToList());
            }
        }
        public ActionResult ListaPrioridad()
        {
            var Activo = ("Si");
            using (var db = new AccountingContext())
            {
                return PartialView(db.FCConfiguracion.Where(x => x.activa == Activo).ToList());

            }
        }
        public ActionResult ListaDependencia()
        {
            using (var db = new AccountingContext())
            {
                var NA = ("N/A");
                return PartialView(db.FCDependencias.Where(x => x.nombreDependencia != NA).ToList());
            }
        }
        public ActionResult ListaSedes()
        {
            using (var db = new AccountingContext())
            {
                return PartialView(db.FCSedes.ToList());
            }
        }
        public ActionResult ListaDocumento()
        {
            using (var db = new AccountingContext())
            {
                return PartialView(db.FCDocumentos.ToList());
            }
        }
        public ActionResult ListaMotivos()
        {
            using (var db = new AccountingContext())
            {
                var NA = ("-");
                return PartialView(db.FCMotivosDevolucion.Where(x => x.nombreMotivo != NA).ToList());
                
            }
        }

        public ActionResult CrearSolicitud(string id)
        {
            ViewBag.idAsociado = id;
            ViewBag.nombreAsociado = new ConfiguracionBll().ObtenerNombresAsociado(id);

            return View();
        }


        [HttpPost]
        public ActionResult CrearSolicitud(FCSolicitudes fCSolicitudes, string id, ControlAccesoFC fccontrol,VerificacionAsociado verificacionAsociado)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    using (var db = new AccountingContext())
                    {
                        var useractual = User.Identity.Name;
                        var idAsociado = id;
                        fCSolicitudes.Descripcion = "---";
                        fCSolicitudes.DescripcionG = "---";
                        fCSolicitudes.ComentariosAnalista = "---";
                        fCSolicitudes.ComentariosEnte = "---";
                        fCSolicitudes.DescripcionEgresos = "---";
                        fCSolicitudes.DescripcionActivos = "---";
                        fCSolicitudes.Motivos = 1;
                        fCSolicitudes.estado = "Pendiente";
                        fCSolicitudes.PreEstado = "Pendiente";
                        fCSolicitudes.fechaRegistro = DateTime.Now;
                        db.FCSolicitudes.Add(fCSolicitudes);
                        db.SaveChanges();

                        var CodAsociado = fCSolicitudes.idAsociado;
                        var CodSolicitud = fCSolicitudes.idSolicitud;
                        fccontrol.Usuario = useractual;
                        fccontrol.fecha = DateTime.Now;
                        fccontrol.IdSolicitud = CodSolicitud;
                        fccontrol.IdAsociado = CodAsociado;
                        fccontrol.Actividad = "OPERARIO CREA NUEVA SOLICITUD";
                        db.ControlAccesoFC.Add(fccontrol);
                        db.SaveChanges();

                        verificacionAsociado.idsolicitud = CodSolicitud;
                        verificacionAsociado.idasociado = CodAsociado;
                        
                        db.VerificacionAsociado.Add(verificacionAsociado);
                        db.SaveChanges();

                        var CodVerificacionAso = verificacionAsociado.IdVerificacion;
                        fCSolicitudes.IdVerificacionAso = CodVerificacionAso;
                        db.SaveChanges();

                        return RedirectToAction("AnalisisFinanciero", new { id = idAsociado });
                    }

                }
                catch (Exception)
                {

                    throw;
                }

            }
            return View(fCSolicitudes);
        }
        public ActionResult VistaAnalista()

        {
            
            var Operario = "Fase-N1-Op";
            return View(db.FCSolicitudes.Where(S => S.estado == Operario).ToList());
        }
        public ActionResult VistaEnte()

        {
            var useractual = User.Identity.Name;
            ViewBag.User = useractual;
            var Analista = "Fase-N2-An";
            var Dependencia = new ConfiguracionBll().ObtenerDependencia(useractual);
            

            return View(db.FCSolicitudes.Where(S => S.estado == Analista && S.idDependencia == Dependencia).ToList()) ;
        }
        public ActionResult VistaInfo()

        {
            var useractual = User.Identity.Name;
            ViewBag.User = useractual;
            var InformacionFC = "C-Aprobado";
            
            return View(db.FCSolicitudes.Where(S => S.estado == InformacionFC).ToList());
        }

        public ActionResult Analisis(int id)
        {
            using (var db = new AccountingContext())
            {
                FCSolicitudes Solicitud = db.FCSolicitudes.Find(id);
                return View(Solicitud);
            }
        }
        public ActionResult AnalisisFC(int id)

        {
            ViewBag.idSolicitud = id;
            ViewBag.PreEstado = new ConfiguracionBll().ObtenerPreEstado(id); 
            ViewBag.EstadoRC = new ConfiguracionBll().ObtenerEstadoRC(id); 
            ViewBag.EstadoDoc = new ConfiguracionBll().ObtenerEstadoDoc(id); 
            ViewBag.Estado = new ConfiguracionBll().ObtenerEstados(id);

            return View(db.FCSolicitudes.Where(S => S.idSolicitud == id).ToList()); 

        }
        public ActionResult EnteFC(int id)
        {
            ViewBag.idSolicitud = id;
            ViewBag.PreEstado = new ConfiguracionBll().ObtenerPreEstado(id);
            ViewBag.EstadoRC = new ConfiguracionBll().ObtenerEstadoRC(id);
            ViewBag.EstadoDoc = new ConfiguracionBll().ObtenerEstadoDoc(id);
            ViewBag.Estado = new ConfiguracionBll().ObtenerEstados(id);
            return View(db.FCSolicitudes.Where(S => S.idSolicitud == id).ToList());

        }
        public ActionResult InfoFC(int id)
        { 
            ViewBag.idSolicitud = id;
            ViewBag.PreEstado = new ConfiguracionBll().ObtenerPreEstado(id);
            ViewBag.EstadoRC = new ConfiguracionBll().ObtenerEstadoRC(id);
            ViewBag.EstadoDoc = new ConfiguracionBll().ObtenerEstadoDoc(id);
            ViewBag.Estado = new ConfiguracionBll().ObtenerEstados(id);
            return View(db.FCSolicitudes.Where(S => S.idSolicitud == id).ToList());

        }
        public ActionResult PrevisPrestamo(int id)

        {
            ViewBag.idSolicitud = id;
            ViewBag.Salario = new ConfiguracionBll().ObtenerSalario(id);
            ViewBag.Estado = new ConfiguracionBll().ObtenerEstado(id);
            return View(db.FCSolicitudes.Where(S => S.idSolicitud == id).ToList());

        }


        public ActionResult AnalisisFinanciero(string id)

        {
            var idAsociado = int.Parse(id);
            return View(db.FCSolicitudes.Where(S => S.idAsociado == idAsociado).ToList());

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
        public ActionResult CodeudorEnte(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.idSolicitud = id;
                    var actividad = new ConfiguracionBll().ReferenciasCodeudorObtener(id);
                    var nReferencias = new int[] { actividad.nReferencias, actividad.nCodeudores };
                    ViewBag.nReferencias = nReferencias;

                    return View(db.DataReferenciasCodeudorFC.Where(S => S.IDSOLICITUD == id && S.INFADICIONAL == "Codeudor" && S.VERIFICACION == "Aprobada  ").ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult CodeudorInfo(int id) 
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.idSolicitud = id;
                    var actividad = new ConfiguracionBll().ReferenciasCodeudorObtener(id);
                    var nReferencias = new int[] { actividad.nReferencias, actividad.nCodeudores };
                    ViewBag.nReferencias = nReferencias;

                    return View(db.DataReferenciasCodeudorFC.Where(S => S.IDSOLICITUD == id && S.INFADICIONAL == "Codeudor" && S.VERIFICACION == "Aprobada  ").ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
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
        public ActionResult DocumentacionP(int id)
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
        public ActionResult DocumentacionAnalista(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.idSolicitud = id;
                    ViewBag.idAsociado = new ConfiguracionBll().ObtenerAsociado(id);
                    ViewBag.EstadoAnDoc = new ConfiguracionBll().ObtenerEstadoAnDoc(id);
                    return View(db.FCDocumentosAsociados.Where(a => a.idSolicitud == id && a.Estado != "Y").ToList());
                    
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult DocumentacionEnte(int id)
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
        public ActionResult DocumentacionInfo(int id)
        { 
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.idSolicitud = id;
                    var Verificacion = "Aprobada  ";
                    ViewBag.idAsociado = new ConfiguracionBll().ObtenerAsociado(id);
                    return View(db.FCDocumentosAsociados.Where(a => a.idSolicitud == id && a.Estado != "Y" && a.documentoVerificado == Verificacion).ToList());

                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult ReferenciasCodeudorAnalista(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.idSolicitud = id;
                    ViewBag.EstadoAnRC = new ConfiguracionBll().ObtenerEstadoAnRC(id);
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
        public ActionResult CodeudorAnalista(int id)
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
        public ActionResult ReferenciasCodeudoEnte(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.idSolicitud = id;
                    var actividad = new ConfiguracionBll().ReferenciasCodeudorObtener(id);
                    var nReferencias = new int[] { actividad.nReferencias, actividad.nCodeudores };
                    ViewBag.nReferencias = nReferencias;

                    return View(db.DataReferenciasCodeudorFC.Where(S => S.IDSOLICITUD == id && S.INFADICIONAL == "Referencia" && S.VERIFICACION == "Aprobada  ").ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult ReferenciasCodeudoInfo(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.idSolicitud = id;
                    var actividad = new ConfiguracionBll().ReferenciasCodeudorObtener(id);
                    
                    var nReferencias = new int[] { actividad.nReferencias, actividad.nCodeudores };
                    ViewBag.nReferencias = nReferencias;

                    return View(db.DataReferenciasCodeudorFC.Where(S => S.IDSOLICITUD == id && S.INFADICIONAL == "Referencia" && S.VERIFICACION == "Aprobada  ").ToList());
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


        public FileResult Download(string ImageName)
        {
            var FileVirtualPath = "~/File/" + ImageName;
            return File(FileVirtualPath, "application/force- download", Path.GetFileName(FileVirtualPath));
        }

        private List<string> GetFiles()
        {
            var dir = new System.IO.DirectoryInfo(Server.MapPath("~/File"));
            System.IO.FileInfo[] fileNames = dir.GetFiles("*.*");

            List<string> items = new List<string>();
            foreach (var file in fileNames)
            {
                items.Add(file.Name);
            }

            return items;
        }





        //---------------------------------------------------------------------------------------------
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
        //EditarEstadoDoc

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

                    return RedirectToAction("PrevisPrestamo", new { id = IdSolid });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult CambioEstadoAproAnalis(int id)
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
        public ActionResult CambioEstadoAproAnalis(FCSolicitudes fCSolicitudes, int id)
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
                    FCSolicitudes FaseII = db.FCSolicitudes.Find(fCSolicitudes.idSolicitud);
                    Idaso = fCSolicitudes.idAsociado;
                    IdSolid = fCSolicitudes.idSolicitud;
                    FaseII.estado = fCSolicitudes.estado;
                    FaseII.ComentariosAnalista = fCSolicitudes.ComentariosAnalista;
                    db.SaveChanges();


                    return RedirectToAction("AnalisisFC", new { id = IDd });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult AprobacionEnte(int id)
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
        public ActionResult AprobacionEnte(FCSolicitudes fCSolicitudes, int id) 
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
                    FCSolicitudes FaseIII = db.FCSolicitudes.Find(fCSolicitudes.idSolicitud);
                    Idaso = fCSolicitudes.idAsociado;
                    IdSolid = fCSolicitudes.idSolicitud;
                    FaseIII.estado = fCSolicitudes.estado;
                    FaseIII.ComentariosEnte = fCSolicitudes.ComentariosEnte;
                    db.SaveChanges();
                    

                    return RedirectToAction("EnteFC", new { id = IDd });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult NoAprobacionEnte(int id)
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
        public ActionResult NoAprobacionEnte(FCSolicitudes fCSolicitudes, int id)
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
                    FCSolicitudes FaseIII = db.FCSolicitudes.Find(fCSolicitudes.idSolicitud); 
                    Idaso = fCSolicitudes.idAsociado;
                    IdSolid = fCSolicitudes.idSolicitud;
                    FaseIII.estado = fCSolicitudes.estado;
                    FaseIII.ComentariosEnte = fCSolicitudes.ComentariosEnte;
                    FaseIII.Motivos = fCSolicitudes.Motivos;
                    db.SaveChanges();


                    return RedirectToAction("EnteFC", new { id = IDd });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult CambioEstadoNoAproAnalis(int id)
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
        public ActionResult CambioEstadoNoAproAnalis(FCSolicitudes fCSolicitudes, int id)
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
                    FCSolicitudes FaseII = db.FCSolicitudes.Find(fCSolicitudes.idSolicitud);
                    Idaso = fCSolicitudes.idAsociado;
                    IdSolid = fCSolicitudes.idSolicitud;
                    FaseII.estado = fCSolicitudes.estado;
                    FaseII.ComentariosAnalista = fCSolicitudes.ComentariosAnalista;
                    FaseII.Motivos = fCSolicitudes.Motivos;
                    db.SaveChanges();


                    return RedirectToAction("AnalisisFC", new { id = IDd });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult CambioEstadoRefCode(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {

                    ViewBag.idSolicitud = id;
                    ViewBag.idAsociado = new ConfiguracionBll().ObtenerAsociado(id);
                    ViewBag.EstadoAnRC = new ConfiguracionBll().ObtenerEstadoAnRC(id);
                    FCPasosAp paso = db.FCPasosAp.Where(a => a.idsolicitud == id).FirstOrDefault();
                    return View(paso);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult CambioEstadoRefCode(FCPasosAp fCPasosAp, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    var IDd = id;
                    FCPasosAp FaseRC = db.FCPasosAp.Find(fCPasosAp.idAp);
                    FaseRC.EstadoAnRC = "AprobadoRC";
                    FaseRC.ComentarioAnRC = fCPasosAp.ComentarioAnRC;
                    db.SaveChanges();

                    return RedirectToAction("AnalisisFC", new { id = IDd });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult CambioEstadoDoc(int id) 
        {
            try
            {
                using (var db = new AccountingContext())
                {

                    ViewBag.idSolicitud = id;
                    ViewBag.idAsociado = new ConfiguracionBll().ObtenerAsociado(id);
                    ViewBag.EstadoAnDoc = new ConfiguracionBll().ObtenerEstadoAnDoc(id); 
                    FCPasosAp paso = db.FCPasosAp.Where(a => a.idsolicitud == id).FirstOrDefault();
                    return View(paso);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult CambioEstadoDoc(FCPasosAp fCPasosAp, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    var IDd = id;
                    FCPasosAp FaseDoc = db.FCPasosAp.Find(fCPasosAp.idAp);
                    FaseDoc.EstadoAnDoc = "AprobadoDo";
                    FaseDoc.ComentarioAnDoc = fCPasosAp.ComentarioAnDoc;
                    db.SaveChanges();

                    return RedirectToAction("AnalisisFC", new { id = IDd });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult EditarEstadoDoc(int IDs)
        {
            try
            {
                using (var db = new AccountingContext())
                {

                    ViewBag.IDDoc = IDs;
                    FCDocumentosAsociados EditarD = db.FCDocumentosAsociados.Where(a => a.idDoc == IDs).FirstOrDefault();
                    return View(EditarD);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public ActionResult EditarEstadoDoc(FCDocumentosAsociados fCDocumentosAsociados, int IDs)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    var IDd = IDs;
                    FCDocumentosAsociados doc = db.FCDocumentosAsociados.Find(fCDocumentosAsociados.idDoc);
                    var IdSolid = fCDocumentosAsociados.idSolicitud;
                    doc.documentoVerificado = fCDocumentosAsociados.documentoVerificado;
                    doc.comentarios = fCDocumentosAsociados.comentarios;
                    db.SaveChanges();

                    return RedirectToAction("DocumentacionAnalista", new { id = IdSolid });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult EditarEstado(int IDREFCODE) 
        {
            try
            {
                using (var db = new AccountingContext())
                {
                   
                    ViewBag.NombreReferencia = new ConfiguracionBll().ObtenerNombreRefe(IDREFCODE);
                    ViewBag.idSolicitudA = new ConfiguracionBll().ObtenerIdSoliA(IDREFCODE); 
                    ViewBag.IDREFCOD = IDREFCODE;
                    DataReferenciasCodeudorFC EditarPres = db.DataReferenciasCodeudorFC.Where(a => a.IDREFCODE == IDREFCODE).FirstOrDefault();
                    return View(EditarPres);
                }

            } 
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public ActionResult EditarEstado(DataReferenciasCodeudorFC dataReferenciasCodeudorFC, int IDREFCODE)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    var IDREFCO = IDREFCODE;
                    DataReferenciasCodeudorFC ANFa = db.DataReferenciasCodeudorFC.Find(dataReferenciasCodeudorFC.IDREFCODE);
                    var IdSolid = dataReferenciasCodeudorFC.IDSOLICITUD;
                    ANFa.VNOMBRECOMPLETO = dataReferenciasCodeudorFC.VNOMBRECOMPLETO;
                    ANFa.BNOMBRECOMPLETO = dataReferenciasCodeudorFC.BNOMBRECOMPLETO;
                    ANFa.VTELEFONOOCELULAR = dataReferenciasCodeudorFC.VTELEFONOOCELULAR;
                    ANFa.BTELEFONOOCELULAR = dataReferenciasCodeudorFC.BTELEFONOOCELULAR;
                    ANFa.VDIRECCIONRESIDENCIA = dataReferenciasCodeudorFC.VDIRECCIONRESIDENCIA;
                    ANFa.BDIRECCIONRESIDENCIA = dataReferenciasCodeudorFC.BDIRECCIONRESIDENCIA;
                    ANFa.VACTIVIDADECONOMICA = dataReferenciasCodeudorFC.VACTIVIDADECONOMICA;
                    ANFa.BACTIVIDADECONOMICA = dataReferenciasCodeudorFC.BACTIVIDADECONOMICA;
                    ANFa.VPARENTESCO = dataReferenciasCodeudorFC.VPARENTESCO;
                    ANFa.BPARENTESCO = dataReferenciasCodeudorFC.BPARENTESCO;
                    ANFa.VTIEMPODECONOCER = dataReferenciasCodeudorFC.VTIEMPODECONOCER;
                    ANFa.BTIEMPODECONOCER = dataReferenciasCodeudorFC.BTIEMPODECONOCER;
                    ANFa.VERIFICACION = dataReferenciasCodeudorFC.VERIFICACION;
                    db.SaveChanges();

                    return RedirectToAction("ReferenciasCodeudorAnalista", new { id = IdSolid }); 
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult EditarEstadoCodeudor(int IDREFCODE)
        {
            try
            {
                using (var db = new AccountingContext())
                {

                    ViewBag.NombreReferencia = new ConfiguracionBll().ObtenerNombreRefe(IDREFCODE);
                    ViewBag.idSolicitudA = new ConfiguracionBll().ObtenerIdSoliA(IDREFCODE);
                    ViewBag.IDREFCOD = IDREFCODE;
                    DataReferenciasCodeudorFC EditarPres = db.DataReferenciasCodeudorFC.Where(a => a.IDREFCODE == IDREFCODE).FirstOrDefault();
                    return View(EditarPres);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost] 
        public ActionResult EditarEstadoCodeudor(DataReferenciasCodeudorFC dataReferenciasCodeudorFC, int IDREFCODE)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    var IDREFCO = IDREFCODE;
                    DataReferenciasCodeudorFC ANFa = db.DataReferenciasCodeudorFC.Find(dataReferenciasCodeudorFC.IDREFCODE);
                    var IdSolid = dataReferenciasCodeudorFC.IDSOLICITUD;
                    ANFa.VNOMBRECOMPLETO = dataReferenciasCodeudorFC.VNOMBRECOMPLETO;
                    ANFa.BNOMBRECOMPLETO = dataReferenciasCodeudorFC.BNOMBRECOMPLETO;
                    ANFa.VTELEFONOOCELULAR = dataReferenciasCodeudorFC.VTELEFONOOCELULAR;
                    ANFa.BTELEFONOOCELULAR = dataReferenciasCodeudorFC.BTELEFONOOCELULAR;
                    ANFa.VDIRECCIONRESIDENCIA = dataReferenciasCodeudorFC.VDIRECCIONRESIDENCIA;
                    ANFa.BDIRECCIONRESIDENCIA = dataReferenciasCodeudorFC.BDIRECCIONRESIDENCIA;
                    ANFa.VACTIVIDADECONOMICA = dataReferenciasCodeudorFC.VACTIVIDADECONOMICA;
                    ANFa.BACTIVIDADECONOMICA = dataReferenciasCodeudorFC.BACTIVIDADECONOMICA;
                    ANFa.VPARENTESCO = dataReferenciasCodeudorFC.VPARENTESCO;
                    ANFa.BPARENTESCO = dataReferenciasCodeudorFC.BPARENTESCO;
                    ANFa.VTIEMPODECONOCER = dataReferenciasCodeudorFC.VTIEMPODECONOCER;
                    ANFa.BTIEMPODECONOCER = dataReferenciasCodeudorFC.BTIEMPODECONOCER;
                    ANFa.VCORREO = dataReferenciasCodeudorFC.VCORREO;
                    ANFa.CORREO= dataReferenciasCodeudorFC.CORREO;
                    ANFa.VINGRESOS = dataReferenciasCodeudorFC.VINGRESOS;
                    ANFa.BINGRESOS = dataReferenciasCodeudorFC.BINGRESOS;
                    ANFa.VEGRESOS = dataReferenciasCodeudorFC.VEGRESOS;
                    ANFa.BEGRESOS = dataReferenciasCodeudorFC.BEGRESOS;
                    ANFa.VERIFICACION = dataReferenciasCodeudorFC.VERIFICACION;
                    db.SaveChanges();

                    return RedirectToAction("CodeudorAnalista", new { id = IdSolid });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult EditarPrestamo(int idSolicitud)
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
        public ActionResult EditarPrestamo(FCSolicitudes fCSolicitudes, int idSolicitud)
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


                    return RedirectToAction("PrevisPrestamo", new { id = idSoli });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult VerificacionFC(int IdVerificacion)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    var idVerific = IdVerificacion;
                    ViewBag.Nombre = new ConfiguracionBll().ObtenerNombreAso(idVerific);
                    ViewBag.idasociado = new ConfiguracionBll().ObteneridAso(idVerific);
                    ViewBag.Fecha = new ConfiguracionBll().ObtenerFechaSol(idVerific);
                    ViewBag.idSolicitud = new ConfiguracionBll().ObtenerSolicitudAso(idVerific); 
                    VerificacionAsociado EditarVer = db.VerificacionAsociado.Where(a => a.IdVerificacion == IdVerificacion).FirstOrDefault();
                    return View(EditarVer);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult VerificacionFC(VerificacionAsociado verificacionAsociado , int idverificacion)
        {
            try 
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    var idVerific = idverificacion;

                    VerificacionAsociado ANF = db.VerificacionAsociado.Find(verificacionAsociado.IdVerificacion);
                    ANF.direccionresidencia = verificacionAsociado.direccionresidencia;
                    ANF.observacionesdir = verificacionAsociado.observacionesdir;
                    ANF.telefonocelular = verificacionAsociado.telefonocelular;
                    ANF.observacionestele = verificacionAsociado.observacionestele;
                    ANF.correoelectronico = verificacionAsociado.correoelectronico;
                    ANF.observacionescor = verificacionAsociado.observacionescor;
                    ANF.ocupacionoficio = verificacionAsociado.ocupacionoficio;
                    ANF.observacionesocu = verificacionAsociado.observacionesocu;
                    ANF.estadocivil = verificacionAsociado.estadocivil;
                    ANF.observacionesest = verificacionAsociado.observacionesest;
                    ANF.nombredelconyuge = verificacionAsociado.nombredelconyuge;
                    ANF.observacionesnomcon = verificacionAsociado.observacionesnomcon;
                    ANF.telefonodelconyuge = verificacionAsociado.telefonodelconyuge;
                    ANF.observacionestelecon = verificacionAsociado.observacionestelecon;
                    ANF.tipovivienda = verificacionAsociado.tipovivienda;
                    ANF.observacionestip = verificacionAsociado.observacionestip;
                    ANF.nombredelnegocio = verificacionAsociado.nombredelnegocio;
                    ANF.observacionesnomnego = verificacionAsociado.observacionesnomnego;
                    ANF.direcciontrabajo = verificacionAsociado.direcciontrabajo;
                    ANF.observacionesdiretrab = verificacionAsociado.observacionesdiretrab;
                    ANF.tiempodelnegocio = verificacionAsociado.tiempodelnegocio;
                    ANF.observacionestiemp = verificacionAsociado.observacionestiemp;
                    ANF.noempleados = verificacionAsociado.noempleados;
                    ANF.observacionesnoempl = verificacionAsociado.observacionesnoempl;
                    ANF.ingresosmensuales = verificacionAsociado.ingresosmensuales;
                    ANF.observacionesingres = verificacionAsociado.observacionesingres;
                    ANF.gastosmensuales = verificacionAsociado.gastosmensuales;
                    ANF.observacionesgastos = verificacionAsociado.observacionesgastos;

                    var IdSolid = verificacionAsociado.idsolicitud;

                    db.SaveChanges();
                    
                    return RedirectToAction("AnalisisFC", new { id = IdSolid });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public FileResult Imagen(string ruta)
        {
            var rutas = ruta;
            var ex = new ConfiguracionBll().ObtenerEX(ruta);
            var Nombre = new ConfiguracionBll().ObtenerNombre(ruta);
            return File(rutas, "aplication/" + ex, "*"+ Nombre + ex); 
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



                    return RedirectToAction("PrevisPrestamo", new { id = idSoli });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Evaluacion(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.idSolicitud = id;
                    ViewBag.Nombre = new ConfiguracionBll().ObtenerNombreDelAsociado(id);
                    ViewBag.ValorPrestamo = new ConfiguracionBll().ObtenerValorPrestamo(id);
                    ViewBag.NoCuotasPrestamo = new ConfiguracionBll().ObtenerNoCuotasPrestamo(id);
                    ViewBag.NombreDependencia = new ConfiguracionBll().ObtenerNombreDependencia(id);
                    ViewBag.Min = new ConfiguracionBll().ObtenerMin(id);
                    ViewBag.Max = new ConfiguracionBll().ObtenerMax(id);
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
        public ActionResult Evaluacion(FCSolicitudes AnFin, int idSolicitud /*FCTareas tarea*/)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    var idSoli = idSolicitud;
                    FCSolicitudes ANFI = db.FCSolicitudes.Find(AnFin.idSolicitud);
                    ANFI.PreEstado = AnFin.PreEstado;
                    db.SaveChanges();

                    //tarea.estadoAnalista = "Pendiente";
                    //db.FCTareas.Add(tarea);
                    //db.SaveChanges();



                    return RedirectToAction("AnalisisFC", new { id = idSoli });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}




