using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO.FabricaCreditos;
using FNTC.Finansoft.Accounting.DTO.ControlCartera;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using System.Data.Entity;
using FNTC.Finansoft.Accounting.BLL.FabricaCreditosBll;

namespace FNTC.Finansoft.UI.Areas.FabricaCreditos.Controllers
{
    public class CarteraController : Controller
    {
        private AccountingContext db = new AccountingContext();
        // GET: FabricaCreditos/Cartera
        public ActionResult CarteraIni()
        {
            return View();
        }
        public ActionResult ConfiguracionCartera()
        {

            return View(db.CRClasesDeGestion.ToList());
        }
        public ActionResult CrearConfiguracionCartera()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearConfiguracionCartera(CRClasesDeGestion cRClasesDeGestion)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (AccountingContext db = new AccountingContext())
                {

                    db.CRClasesDeGestion.Add(cRClasesDeGestion);
                    db.SaveChanges();
                    return RedirectToAction("ConfiguracionCartera");

                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al Agregar ");
                return View();
            }
        }
        public ActionResult EditarConfiguracionCartera(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    CRClasesDeGestion cRClasesDeGestion = db.CRClasesDeGestion.Where(a => a.idClase == id).FirstOrDefault();
                    return View(cRClasesDeGestion);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarConfiguracionCartera(CRClasesDeGestion cRClasesDeGestion)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    CRClasesDeGestion crClasesDeGestion = db.CRClasesDeGestion.Find(cRClasesDeGestion.idClase);
                    crClasesDeGestion.Estado = cRClasesDeGestion.Estado;

                    db.SaveChanges();
                    return RedirectToAction("ConfiguracionCartera");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GestionContacto()
        {

            return View(db.CRGestionContacto.ToList());
        }
        public ActionResult CrearGestionContacto()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearGestionContacto(CRGestionContacto cRGestionContacto)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (AccountingContext db = new AccountingContext())
                {

                    db.CRGestionContacto.Add(cRGestionContacto);
                    db.SaveChanges();
                    return RedirectToAction("GestionContacto");

                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al Agregar ");
                return View();
            }
        }
        public ActionResult EditarGestionContacto(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    CRGestionContacto cRGestionContacto = db.CRGestionContacto.Where(a => a.idGestionContacto == id).FirstOrDefault();
                    return View(cRGestionContacto);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarGestionContacto(CRGestionContacto cRGestionContacto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    CRGestionContacto crGestionContacto = db.CRGestionContacto.Find(cRGestionContacto.idGestionContacto);
                    crGestionContacto.Estado = cRGestionContacto.Estado;

                    db.SaveChanges();
                    return RedirectToAction("GestionContacto");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GestionRespuesta()
        {

            return View(db.CRGestionRespuesta.ToList());
        }
        public ActionResult CrearGestionRespuesta()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearGestionRespuesta(CRGestionRespuesta cRGestionRespuesta)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (AccountingContext db = new AccountingContext())
                {

                    db.CRGestionRespuesta.Add(cRGestionRespuesta);
                    db.SaveChanges();
                    return RedirectToAction("GestionRespuesta");

                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al Agregar ");
                return View();
            }
        }
        public ActionResult EditarGestionRespuesta(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    CRGestionRespuesta cRGestionRespuesta = db.CRGestionRespuesta.Where(a => a.idGestionRespuesta == id).FirstOrDefault();
                    return View(cRGestionRespuesta);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarGestionRespuesta(CRGestionRespuesta cRGestionRespuesta)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    CRGestionRespuesta crGestionRespuesta = db.CRGestionRespuesta.Find(cRGestionRespuesta.idGestionRespuesta);
                    crGestionRespuesta.Estado = cRGestionRespuesta.Estado;

                    db.SaveChanges();
                    return RedirectToAction("GestionRespuesta");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult ConveniosTipoDeConvenio()
        {

            return View(db.CRConveniosTipoDeConvenios.ToList());
        }
        public ActionResult CrearConveniosTipoDeConvenio()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearConveniosTipoDeConvenio(CRConveniosTipoDeConvenios cRConveniosTipoDeConvenios)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (AccountingContext db = new AccountingContext())
                {

                    db.CRConveniosTipoDeConvenios.Add(cRConveniosTipoDeConvenios);
                    db.SaveChanges();
                    return RedirectToAction("ConveniosTipoDeConvenio");

                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al Agregar ");
                return View();
            }
        }
        public ActionResult EditarConveniosTipoDeConvenio(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    CRConveniosTipoDeConvenios cRConveniosTipoDeConvenios = db.CRConveniosTipoDeConvenios.Where(a => a.idTipoDeConvenios == id).FirstOrDefault();
                    return View(cRConveniosTipoDeConvenios);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarConveniosTipoDeConvenio(CRConveniosTipoDeConvenios cRConveniosTipoDeConvenios)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    CRConveniosTipoDeConvenios crConveniosTipoDeConvenios = db.CRConveniosTipoDeConvenios.Find(cRConveniosTipoDeConvenios.idTipoDeConvenios);
                    crConveniosTipoDeConvenios.Estado = cRConveniosTipoDeConvenios.Estado;

                    db.SaveChanges();
                    return RedirectToAction("ConveniosTipoDeConvenio");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult CarteraG()
        {
            return View();
        }
        public ActionResult FRProximosAVencer()
        {
            using (var db = new AccountingContext())
            {

                var fechaRegistrob = DateTime.Now;
                DateTime Fechas = Convert.ToDateTime(fechaRegistrob);
                Fechas = Fechas.AddDays(15);

                return View(db.TotalesCreditos.Where(x => x.FechaProximoPago >= fechaRegistrob && x.FechaProximoPago <= Fechas && x.Estado == "AD").ToList());
            }
        }
        public ActionResult DetallesDeCreditosCR(string id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
                    ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
                    ViewBag.Pagare = id;
                    ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
                    ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
                    ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
                    ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
                    ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
                    ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    var FechaProximo = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    var fechaRegistrob = DateTime.Now;
                    ViewBag.FechaActual = fechaRegistrob;
                    return View(db.ControlCreditos.Where(S => S.Pagare == id ).ToList());
                }
                //&& S.FechaPago == FechaProximo
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult CrearNuevaNotificacion(string id)

        {
            ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
            ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
            ViewBag.Pagare = id;
            ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
            ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
            ViewBag.IdPrestamo = new ConfiguracionBll().ObtenerIdPrestamoCR(id);
            ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
            ViewBag.EstadoCredito = new ConfiguracionBll().ObtenerEstadoCredito(id);
            ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
            ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
            ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearNuevaNotificacion(CRNotificacionesCartera cRNotificacionesCartera, string id)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (AccountingContext db = new AccountingContext())
                {
                    var pagare = id;
                    cRNotificacionesCartera.Pagare = id;
                    cRNotificacionesCartera.IdPrestamo = cRNotificacionesCartera.IdPrestamo;
                    cRNotificacionesCartera.FechaRegistro = DateTime.Now;
                    cRNotificacionesCartera.EstadoCredito = cRNotificacionesCartera.EstadoCredito;
                    cRNotificacionesCartera.Proceso = "CPV";
                    db.CRNotificacionesCartera.Add(cRNotificacionesCartera);
                    db.SaveChanges();
                    return RedirectToAction("DetallesDeCreditosCR", new { id = pagare });

                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al Agregar ");
                return View();
            }
        }
        public ActionResult HistorialCartera(string id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.Pagare = id;
                    return View(db.CRNotificacionesCartera.Where(S => S.Pagare == id && S.Proceso == "CPV").ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Movimientos(string id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.Pagare = id;
                    return View(db.factOpCajaConsCuotaCredito.Where(S => S.pagare == id).ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult ListaClase()
        {
            using (var db = new AccountingContext())
            {
                return PartialView(db.CRClasesDeGestion.Where(x => x.Estado == "Si").ToList());
            }
        }
        public ActionResult ListaContacto()
        {
            using (var db = new AccountingContext())
            {
                return PartialView(db.CRGestionContacto.Where(x => x.Estado == "Si").ToList());
            }
        }
        public ActionResult ListaRespuesta()
        {
            using (var db = new AccountingContext())
            {
                return PartialView(db.CRGestionRespuesta.Where(x => x.Estado == "Si").ToList());
            }
        }
        public ActionResult ListaCompromisosCartera()
        {
            using (var db = new AccountingContext())
            {
                return PartialView(db.CRConveniosTipoDeConvenios.Where(x => x.Estado == "Si").ToList());
            }
        }

        public ActionResult FRCreditosVencidos()
        {
            using (var db = new AccountingContext())
            {

                var fechaRegistrob = DateTime.Now;

                return View(db.TotalesCreditos.Where(x => x.FechaProximoPago <= fechaRegistrob && x.Estado == "AD").ToList());
            }
        }
        public ActionResult DetallesDeCreditosVencidos(string id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
                    ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
                    ViewBag.Pagare = id;
                    ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
                    ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
                    ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
                    ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
                    ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
                    ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    var FechaProximo = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    var fechaRegistrob = DateTime.Now;
                    ViewBag.FechaActual = fechaRegistrob;
                    return View(db.ControlCreditos.Where(S => S.Pagare == id).ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult CrearNuevaNotificacionCreditosVencidos(string id)

        {
            ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
            ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
            ViewBag.Pagare = id;
            ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
            ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
            ViewBag.IdPrestamo = new ConfiguracionBll().ObtenerIdPrestamoCR(id);
            ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
            ViewBag.EstadoCredito = new ConfiguracionBll().ObtenerEstadoCredito(id);
            ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
            ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
            ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearNuevaNotificacionCreditosVencidos(CRNotificacionesCartera cRNotificacionesCartera, string id)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (AccountingContext db = new AccountingContext())
                {
                    var pagare = id;
                    cRNotificacionesCartera.Pagare = id;
                    cRNotificacionesCartera.IdPrestamo = cRNotificacionesCartera.IdPrestamo;
                    cRNotificacionesCartera.FechaRegistro = DateTime.Now;
                    cRNotificacionesCartera.EstadoCredito = cRNotificacionesCartera.EstadoCredito;
                    cRNotificacionesCartera.Proceso = "CAV";
                    db.CRNotificacionesCartera.Add(cRNotificacionesCartera);
                    db.SaveChanges();
                    return RedirectToAction("DetallesDeCreditosVencidos", new { id = pagare });

                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al Agregar ");
                return View();
            }
        }
        public ActionResult HistorialCarteraCreditosVencidos(string id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.Pagare = id;
                    return View(db.CRNotificacionesCartera.Where(S => S.Pagare == id && S.Proceso == "CAV").ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult FRCreditosEnMora()
        {
            using (var db = new AccountingContext())
            {

                var fechaRegistrob = DateTime.Now;
                DateTime Fechas = Convert.ToDateTime(fechaRegistrob);
                Fechas = Fechas.AddDays(-30);

                return View(db.TotalesCreditos.Where(x => x.FechaProximoPago <= fechaRegistrob && x.Estado == "EM" && x.FechaProximoPago >= Fechas && x.DiasMora <= 30).ToList());
            }
        }
        public ActionResult DetallesDeCreditosEnMora(string id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
                    ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
                    ViewBag.Pagare = id;
                    ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
                    ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
                    ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
                    ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
                    ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
                    ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    var FechaProximo = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    var fechaRegistrob = DateTime.Now;
                    ViewBag.FechaActual = fechaRegistrob;
                    return View(db.ControlCreditos.Where(S => S.Pagare == id ).ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult CrearNuevaNotificacionCreditosEnMora(string id)

        {
            ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
            ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
            ViewBag.Pagare = id;
            ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
            ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
            ViewBag.IdPrestamo = new ConfiguracionBll().ObtenerIdPrestamoCR(id);
            ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
            ViewBag.EstadoCredito = new ConfiguracionBll().ObtenerEstadoCredito(id);
            ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
            ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
            ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearNuevaNotificacionCreditosEnMora(CRNotificacionesCartera cRNotificacionesCartera, string id)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (AccountingContext db = new AccountingContext())
                {
                    var pagare = id;
                    cRNotificacionesCartera.Pagare = id;
                    cRNotificacionesCartera.IdPrestamo = cRNotificacionesCartera.IdPrestamo;
                    cRNotificacionesCartera.FechaRegistro = DateTime.Now;
                    cRNotificacionesCartera.EstadoCredito = cRNotificacionesCartera.EstadoCredito;
                    cRNotificacionesCartera.Proceso = "CEM";
                    db.CRNotificacionesCartera.Add(cRNotificacionesCartera);
                    db.SaveChanges();
                    return RedirectToAction("DetallesDeCreditosEnMora", new { id = pagare });

                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al Agregar ");
                return View();
            }
        }
        public ActionResult HistorialCarteraCreditosEnMora(string id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.Pagare = id;
                    return View(db.CRNotificacionesCartera.Where(S => S.Pagare == id && S.Proceso == "CEM").ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GestionCitas()
        {
            using (var db = new AccountingContext())
            {

                var fechaRegistrob = DateTime.Now;
                DateTime Fechas = Convert.ToDateTime(fechaRegistrob);
                Fechas = Fechas.AddDays(-30);

                return View(db.TotalesCreditos.Where(x => x.FechaProximoPago < fechaRegistrob && x.Estado == "EM" && x.Gestion == null).ToList());
            }
        }
        public ActionResult DetallesDelCreditoGC(string id) 
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
                    ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
                    ViewBag.Pagare = id;
                    ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
                    ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
                    ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
                    ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
                    ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
                    ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    var FechaProximo = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    var fechaRegistrob = DateTime.Now;
                    ViewBag.FechaActual = fechaRegistrob;
                    return View(db.ControlCreditos.Where(S => S.Pagare == id ).ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult CrearNuevaCitaGC(string id)

        {
            ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
            ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
            ViewBag.Pagare = id;
            ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
            ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
            ViewBag.IdPrestamo = new ConfiguracionBll().ObtenerIdPrestamoCR(id);
            ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
            ViewBag.EstadoCredito = new ConfiguracionBll().ObtenerEstadoCredito(id);
            ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
            ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
            ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearNuevaCitaGC(CRNotificacionesCartera cRNotificacionesCartera, string id)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (AccountingContext db = new AccountingContext())
                {
                    var pagare = id;
                    cRNotificacionesCartera.Pagare = id;
                    cRNotificacionesCartera.IdPrestamo = cRNotificacionesCartera.IdPrestamo;
                    cRNotificacionesCartera.FechaRegistro = DateTime.Now;
                    cRNotificacionesCartera.EstadoCredito = cRNotificacionesCartera.EstadoCredito;
                    cRNotificacionesCartera.Proceso = "GDC";
                    db.CRNotificacionesCartera.Add(cRNotificacionesCartera);
                    db.SaveChanges();
                    return RedirectToAction("DetallesDelCreditoGC", new { id = pagare });

                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al Agregar ");
                return View();
            }
        }
        public ActionResult HistorialDeCitasGC(string id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.Pagare = id;
                    return View(db.CRNotificacionesCartera.Where(S => S.Pagare == id && S.Proceso == "GDC").ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult FRGestionCreditosEnMora(string id) 
        {
            try
            {
                using (var db = new AccountingContext())
                {

                    ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
                    ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
                    ViewBag.Pagare = id;
                    ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
                    ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
                    ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
                    ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
                    ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
                    ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    ViewBag.IntereMoraTotal = new ConfiguracionBll().ObtenerIntereMoraTotal(id);
                    ViewBag.IntereMoraPendiente = new ConfiguracionBll().ObtenerIntereMoraPendiente(id);
                    ViewBag.DiasMora = new ConfiguracionBll().ObtenerDiasMora(id);
                    ViewBag.EstadoCre = new ConfiguracionBll().ObtenerEstadoCre(id); 
                    var fechaRegistrob = DateTime.Now;
                    ViewBag.FechaActual = fechaRegistrob;


                    return View();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult CrearNuevaCita(string id)

        {
            ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
            ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
            ViewBag.Pagare = id;
            ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
            ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
            ViewBag.IdPrestamo = new ConfiguracionBll().ObtenerIdPrestamoCR(id);
            ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
            ViewBag.EstadoCredito = new ConfiguracionBll().ObtenerEstadoCredito(id);
            ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
            ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
            ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearNuevaCita(CRNotificacionesCartera cRNotificacionesCartera, string id)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (AccountingContext db = new AccountingContext())
                {
                    var pagare = id;
                    cRNotificacionesCartera.Pagare = id;
                    cRNotificacionesCartera.IdPrestamo = cRNotificacionesCartera.IdPrestamo;
                    cRNotificacionesCartera.FechaRegistro = DateTime.Now;
                    cRNotificacionesCartera.EstadoCredito = cRNotificacionesCartera.EstadoCredito;
                    db.CRNotificacionesCartera.Add(cRNotificacionesCartera);
                    db.SaveChanges();
                    return RedirectToAction("FRGestionCreditosEnMora", new { id = pagare });

                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al Agregar ");
                return View();
            }
        }
        
        
        public ActionResult DetallesDeCreditosVencidosCR(string id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
                    ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
                    ViewBag.Pagare = id;
                    ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
                    ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
                    ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
                    ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
                    ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
                    ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    var FechaProximo = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    var fechaRegistrob = DateTime.Now;
                    ViewBag.FechaActual = fechaRegistrob;
                    return View(db.ControlCreditos.Where(S => S.Pagare == id && S.FechaPago == FechaProximo).ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult DetallesDeCreditosEnMoraCR(string id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
                    ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
                    ViewBag.Pagare = id;
                    ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
                    ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
                    ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
                    ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
                    ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
                    ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    var FechaProximo = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    var fechaRegistrob = DateTime.Now;
                    ViewBag.FechaActual = fechaRegistrob;
                    return View(db.ControlCreditos.Where(S => S.Pagare == id && S.FechaPago == FechaProximo).ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult AdministracionCompromisos()
        {
            using (var db = new AccountingContext())
            {

                var fechaRegistrob = DateTime.Now;
                DateTime Fechas = Convert.ToDateTime(fechaRegistrob);
                Fechas = Fechas.AddDays(-30);

                return View(db.TotalesCreditos.Where(x => x.Gestion == "AC1").ToList());
            }
        }
        public ActionResult AdministracionCompromisosDetallesDelCreditoGC(string id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
                    ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
                    ViewBag.Pagare = id;
                    ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
                    ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
                    ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
                    ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
                    ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
                    ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    ViewBag.IDTotalCre = new ConfiguracionBll().ObtenerIdTotalesCre(id);
                    var FechaProximo = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    var fechaRegistrob = DateTime.Now;
                    ViewBag.FechaActual = fechaRegistrob;
                    return View(db.ControlCreditos.Where(S => S.Pagare == id).ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult AdministracionCompromisosCrearNuevaCitaGC(string id)

        {
            ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
            ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
            ViewBag.Pagare = id;
            ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
            ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
            ViewBag.IdPrestamo = new ConfiguracionBll().ObtenerIdPrestamoCR(id);
            ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
            ViewBag.EstadoCredito = new ConfiguracionBll().ObtenerEstadoCredito(id);
            ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
            ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
            ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdministracionCompromisosCrearNuevaCitaGC(CRNotificacionesCartera cRNotificacionesCartera, string id)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (AccountingContext db = new AccountingContext())
                {
                    var pagare = id;
                    cRNotificacionesCartera.Pagare = id;
                    cRNotificacionesCartera.IdPrestamo = cRNotificacionesCartera.IdPrestamo;
                    cRNotificacionesCartera.FechaRegistro = DateTime.Now;
                    cRNotificacionesCartera.EstadoCredito = cRNotificacionesCartera.EstadoCredito;
                    cRNotificacionesCartera.Proceso = "ACC";
                    db.CRNotificacionesCartera.Add(cRNotificacionesCartera);
                    db.SaveChanges();
                    return RedirectToAction("AdministracionCompromisosDetallesDelCreditoGC", new { id = pagare });

                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al Agregar ");
                return View();
            }
        }

        public ActionResult AdministracionCompromisosHistorialDeCitasGC(string id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.Pagare = id;
                    return View(db.CRNotificacionesCartera.Where(S => S.Pagare == id && S.Proceso == "ACC").ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult AdministracionTransferenciaCompromisos(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.IdtotalCre = id;
                    ViewBag.pagare = new ConfiguracionBll().ObtenerPagareCR(id);
                    var pagare = new ConfiguracionBll().ObtenerPagareCR(id);
                    ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(pagare);
                    ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(pagare);
                    ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(pagare);
                    ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(pagare);
                    ViewBag.IdPrestamo = new ConfiguracionBll().ObtenerIdPrestamoCR(pagare);
                    ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(pagare);
                    ViewBag.EstadoCredito = new ConfiguracionBll().ObtenerEstadoCredito(pagare);
                    ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(pagare);
                    ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(pagare);
                    ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(pagare);

                    TotalesCreditos totalesCreditosC = db.TotalesCreditos.Where(a => a.Id == id).FirstOrDefault();
                    return View(totalesCreditosC);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public ActionResult AdministracionTransferenciaCompromisos(TotalesCreditos totalesCreditosCR, CRControlTransferenciaCartera cRControlTransferenciaCartera, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {

                    TotalesCreditos TotalCR = db.TotalesCreditos.Find(totalesCreditosCR.Id);
                    var Pagare = totalesCreditosCR.Pagare;
                    TotalCR.Gestion = totalesCreditosCR.Gestion;
                    db.SaveChanges();

                    var useractual = User.Identity.Name;
                    cRControlTransferenciaCartera.DetallesDeTransaccion = cRControlTransferenciaCartera.DetallesDeTransaccion;
                    cRControlTransferenciaCartera.Transaccion = totalesCreditosCR.Gestion;
                    cRControlTransferenciaCartera.Usuario = useractual;
                    cRControlTransferenciaCartera.Fecha = DateTime.Now;
                    cRControlTransferenciaCartera.Pagare = totalesCreditosCR.Pagare;
                    db.CRControlTransferenciaCartera.Add(cRControlTransferenciaCartera);
                    db.SaveChanges();

                    return RedirectToAction("AdministracionCompromisosDetallesDelCreditoGC", new { id = Pagare });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GestionCompromisos() 
        {
            using (var db = new AccountingContext())
            {

                var fechaRegistrob = DateTime.Now;
                DateTime Fechas = Convert.ToDateTime(fechaRegistrob);
                Fechas = Fechas.AddDays(-30);

                return View(db.TotalesCreditos.Where(x => x.FechaProximoPago < fechaRegistrob && x.Estado == "EM" && x.Gestion == null).ToList());
            }
        }
        
        public ActionResult GestionComprimisosDetallesDelCreditoGC(string id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
                    ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
                    ViewBag.Pagare = id;
                    ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
                    ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
                    ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
                    ViewBag.IDTotalCre = new ConfiguracionBll().ObtenerIdTotalesCre(id);
                    ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
                    ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
                    ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    var FechaProximo = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    var fechaRegistrob = DateTime.Now;
                    ViewBag.FechaActual = fechaRegistrob;
                    return View(db.ControlCreditos.Where(S => S.Pagare == id ).ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GestionComprimisosCrearNuevaCitaGC(string id)

        {
            ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
            ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
            ViewBag.Pagare = id;
            ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
            ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
            ViewBag.IdPrestamo = new ConfiguracionBll().ObtenerIdPrestamoCR(id);
            ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
            ViewBag.EstadoCredito = new ConfiguracionBll().ObtenerEstadoCredito(id);
            ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
            ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
            ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GestionComprimisosCrearNuevaCitaGC(CRNotificacionesCartera cRNotificacionesCartera, string id)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (AccountingContext db = new AccountingContext())
                {
                    var pagare = id;
                    cRNotificacionesCartera.Pagare = id;
                    cRNotificacionesCartera.IdPrestamo = cRNotificacionesCartera.IdPrestamo;
                    cRNotificacionesCartera.FechaRegistro = DateTime.Now;
                    cRNotificacionesCartera.EstadoCredito = cRNotificacionesCartera.EstadoCredito;
                    cRNotificacionesCartera.Proceso = "COM";
                    db.CRNotificacionesCartera.Add(cRNotificacionesCartera);
                    db.SaveChanges();
                    return RedirectToAction("GestionComprimisosDetallesDelCreditoGC", new { id = pagare });

                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al Agregar ");
                return View();
            }
        }
        
        public ActionResult GestionComprimisosHistorialDeCitasGC(string id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.Pagare = id;
                    return View(db.CRNotificacionesCartera.Where(S => S.Pagare == id && S.Proceso == "COM").ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult TransferenciaGestionComprimisos(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.IdtotalCre = id;
                    ViewBag.pagare = new ConfiguracionBll().ObtenerPagareCR(id);
                    var pagare = new ConfiguracionBll().ObtenerPagareCR(id);
                    ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(pagare);
                    ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(pagare);
                    ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(pagare);
                    ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(pagare);
                    ViewBag.IdPrestamo = new ConfiguracionBll().ObtenerIdPrestamoCR(pagare);
                    ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(pagare);
                    ViewBag.EstadoCredito = new ConfiguracionBll().ObtenerEstadoCredito(pagare);
                    ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(pagare);
                    ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(pagare);
                    ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(pagare);

                    TotalesCreditos totalesCreditosC = db.TotalesCreditos.Where(a => a.Id == id).FirstOrDefault();
                    return View(totalesCreditosC);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public ActionResult TransferenciaGestionComprimisos(TotalesCreditos totalesCreditosCR, CRControlTransferenciaCartera cRControlTransferenciaCartera, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {

                    TotalesCreditos TotalCR = db.TotalesCreditos.Find(totalesCreditosCR.Id);
                    var Pagare = totalesCreditosCR.Pagare;
                    TotalCR.Gestion = totalesCreditosCR.Gestion;
                    db.SaveChanges();

                    var useractual = User.Identity.Name;
                    cRControlTransferenciaCartera.DetallesDeTransaccion = cRControlTransferenciaCartera.DetallesDeTransaccion;
                    cRControlTransferenciaCartera.Transaccion = totalesCreditosCR.Gestion;
                    cRControlTransferenciaCartera.Usuario = useractual;
                    cRControlTransferenciaCartera.Fecha = DateTime.Now;
                    cRControlTransferenciaCartera.Pagare = totalesCreditosCR.Pagare;
                    db.CRControlTransferenciaCartera.Add(cRControlTransferenciaCartera);
                    db.SaveChanges();

                    return RedirectToAction("GestionComprimisosDetallesDelCreditoGC", new { id = Pagare });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GestionPreJuridico()
        {
            using (var db = new AccountingContext())
            {

                var fechaRegistrob = DateTime.Now;
                DateTime Fechas = Convert.ToDateTime(fechaRegistrob);
                Fechas = Fechas.AddDays(-30);

                return View(db.TotalesCreditos.Where(x => x.Gestion == "PJ1").ToList());
            }
        }

        public ActionResult GestionPreJuridicoDetallesDelCreditoGC(string id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
                    ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
                    ViewBag.Pagare = id;
                    ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
                    ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
                    ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
                    ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
                    ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
                    ViewBag.IDTotalCre = new ConfiguracionBll().ObtenerIdTotalesCre(id);
                    ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    var FechaProximo = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    var fechaRegistrob = DateTime.Now;
                    ViewBag.FechaActual = fechaRegistrob;
                    return View(db.ControlCreditos.Where(S => S.Pagare == id ).ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GestionPreJuridicoCrearNuevaCitaGC(string id)

        {
            ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
            ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
            ViewBag.Pagare = id;
            ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
            ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
            ViewBag.IdPrestamo = new ConfiguracionBll().ObtenerIdPrestamoCR(id);
            ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
            ViewBag.EstadoCredito = new ConfiguracionBll().ObtenerEstadoCredito(id);
            ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
            ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
            ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GestionPreJuridicoCrearNuevaCitaGC(CRNotificacionesCartera cRNotificacionesCartera, string id)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (AccountingContext db = new AccountingContext())
                {
                    var pagare = id;
                    cRNotificacionesCartera.Pagare = id;
                    cRNotificacionesCartera.IdPrestamo = cRNotificacionesCartera.IdPrestamo;
                    cRNotificacionesCartera.FechaRegistro = DateTime.Now;
                    cRNotificacionesCartera.EstadoCredito = cRNotificacionesCartera.EstadoCredito;
                    cRNotificacionesCartera.Proceso = "PRJ";
                    db.CRNotificacionesCartera.Add(cRNotificacionesCartera);
                    db.SaveChanges();
                    return RedirectToAction("GestionPreJuridicoDetallesDelCreditoGC", new { id = pagare });

                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al Agregar ");
                return View();
            }
        }

        public ActionResult GestionPreJuridicoHistorialDeCitasGC(string id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.Pagare = id;
                    return View(db.CRNotificacionesCartera.Where(S => S.Pagare == id && S.Proceso == "PRJ").ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult TransferenciaGestionPreJuridico(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.IdtotalCre = id;
                    ViewBag.pagare = new ConfiguracionBll().ObtenerPagareCR(id);
                    var pagare = new ConfiguracionBll().ObtenerPagareCR(id);
                    ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(pagare);
                    ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(pagare);
                    ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(pagare);
                    ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(pagare);
                    ViewBag.IdPrestamo = new ConfiguracionBll().ObtenerIdPrestamoCR(pagare);
                    ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(pagare);
                    ViewBag.EstadoCredito = new ConfiguracionBll().ObtenerEstadoCredito(pagare);
                    ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(pagare);
                    ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(pagare);
                    ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(pagare);

                    TotalesCreditos totalesCreditosC = db.TotalesCreditos.Where(a => a.Id == id).FirstOrDefault();
                    return View(totalesCreditosC);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public ActionResult TransferenciaGestionPreJuridico(TotalesCreditos totalesCreditosCR, CRControlTransferenciaCartera cRControlTransferenciaCartera, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {

                    TotalesCreditos TotalCR = db.TotalesCreditos.Find(totalesCreditosCR.Id);
                    var Pagare = totalesCreditosCR.Pagare;
                    TotalCR.Gestion = totalesCreditosCR.Gestion;
                    db.SaveChanges();

                    var useractual = User.Identity.Name;
                    cRControlTransferenciaCartera.DetallesDeTransaccion = cRControlTransferenciaCartera.DetallesDeTransaccion;
                    cRControlTransferenciaCartera.Transaccion = totalesCreditosCR.Gestion;
                    cRControlTransferenciaCartera.Usuario = useractual;
                    cRControlTransferenciaCartera.Fecha = DateTime.Now;
                    cRControlTransferenciaCartera.Pagare = totalesCreditosCR.Pagare;
                    db.CRControlTransferenciaCartera.Add(cRControlTransferenciaCartera);
                    db.SaveChanges();

                    return RedirectToAction("GestionPreJuridicoDetallesDelCreditoGC", new { id = Pagare });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GestionJuridico()
        {
            using (var db = new AccountingContext())
            {

                var fechaRegistrob = DateTime.Now;
                DateTime Fechas = Convert.ToDateTime(fechaRegistrob);
                Fechas = Fechas.AddDays(-30);

                return View(db.TotalesCreditos.Where(x => x.Gestion == "JR1").ToList());

            }
        }
        public ActionResult DetallesDeCreditosCRJuridico(string id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
                    ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
                    ViewBag.Pagare = id;
                    ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
                    ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
                    ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
                    ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
                    ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
                    ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    ViewBag.IDTotalCre = new ConfiguracionBll().ObtenerIdTotalesCre(id);
                    var FechaProximo = new ConfiguracionBll().ObtenerFechaProximoPago(id);
                    var fechaRegistrob = DateTime.Now;
                    ViewBag.FechaActual = fechaRegistrob;
                    return View(db.ControlCreditos.Where(S => S.Pagare == id ).ToList());
                }
               
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult CrearNuevaNotificacionJuridico(string id)

        {
            ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(id);
            ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(id);
            ViewBag.Pagare = id;
            ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(id);
            ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(id);
            ViewBag.IdPrestamo = new ConfiguracionBll().ObtenerIdPrestamoCR(id);
            ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(id);
            ViewBag.EstadoCredito = new ConfiguracionBll().ObtenerEstadoCredito(id);
            ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(id);
            ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(id);
            ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(id);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearNuevaNotificacionJuridico(CRNotificacionesCartera cRNotificacionesCartera, string id)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (AccountingContext db = new AccountingContext())
                {
                    var pagare = id;
                    cRNotificacionesCartera.Pagare = id;
                    cRNotificacionesCartera.IdPrestamo = cRNotificacionesCartera.IdPrestamo;
                    cRNotificacionesCartera.FechaRegistro = DateTime.Now;
                    cRNotificacionesCartera.EstadoCredito = cRNotificacionesCartera.EstadoCredito;
                    cRNotificacionesCartera.Proceso = "GDJ";
                    db.CRNotificacionesCartera.Add(cRNotificacionesCartera);
                    db.SaveChanges();
                    return RedirectToAction("DetallesDeCreditosCRJuridico", new { id = pagare });

                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al Agregar ");
                return View();
            }
        }
        public ActionResult HistorialCarteraJuridico(string id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.Pagare = id;
                    return View(db.CRNotificacionesCartera.Where(S => S.Pagare == id && S.Proceso == "GDJ").ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult TransferenciaJuridico(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    ViewBag.IdtotalCre = id;
                    ViewBag.pagare = new ConfiguracionBll().ObtenerPagareCR(id);
                    var pagare = new ConfiguracionBll().ObtenerPagareCR(id);
                    ViewBag.NoIdentificacion = new ConfiguracionBll().ObtenerNoIdetificacion(pagare);
                    ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoCR(pagare);
                    ViewBag.VTDelPrestamo = new ConfiguracionBll().ObtenerVTDelPrestamo(pagare);
                    ViewBag.Plazo = new ConfiguracionBll().ObtenerPlazoCR(pagare);
                    ViewBag.IdPrestamo = new ConfiguracionBll().ObtenerIdPrestamoCR(pagare);
                    ViewBag.CapitalTotal = new ConfiguracionBll().ObtenerCapitalTotalCR(pagare);
                    ViewBag.EstadoCredito = new ConfiguracionBll().ObtenerEstadoCredito(pagare);
                    ViewBag.SaldoCapital = new ConfiguracionBll().ObtenerSaldoCapitalCR(pagare);
                    ViewBag.TotalInteCorrie = new ConfiguracionBll().ObtenerTotalInteCorrie(pagare);
                    ViewBag.FechaProximoPago = new ConfiguracionBll().ObtenerFechaProximoPago(pagare);

                    TotalesCreditos totalesCreditosC = db.TotalesCreditos.Where(a => a.Id == id).FirstOrDefault();
                    return View(totalesCreditosC);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public ActionResult TransferenciaJuridico(TotalesCreditos totalesCreditosCR, CRControlTransferenciaCartera cRControlTransferenciaCartera, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {

                    TotalesCreditos TotalCR = db.TotalesCreditos.Find(totalesCreditosCR.Id);
                    var Pagare = totalesCreditosCR.Pagare;
                    TotalCR.Gestion = totalesCreditosCR.Gestion;
                    db.SaveChanges();

                    var useractual = User.Identity.Name;
                    cRControlTransferenciaCartera.DetallesDeTransaccion = cRControlTransferenciaCartera.DetallesDeTransaccion;
                    cRControlTransferenciaCartera.Transaccion = totalesCreditosCR.Gestion;
                    cRControlTransferenciaCartera.Usuario = useractual;
                    cRControlTransferenciaCartera.Fecha = DateTime.Now;
                    cRControlTransferenciaCartera.Pagare = totalesCreditosCR.Pagare;
                    db.CRControlTransferenciaCartera.Add(cRControlTransferenciaCartera);
                    db.SaveChanges();

                    return RedirectToAction("DetallesDeCreditosCRJuridico", new { id = Pagare });
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult ResumenGestionCartera() 
        {
            using (var db = new AccountingContext())
            {

                var fechaRegistrob = DateTime.Now;
                DateTime Fechas = Convert.ToDateTime(fechaRegistrob);
                Fechas = Fechas.AddDays(-30);

                return View(db.TotalesCreditos.Where(x => x.Gestion == "AC1" || x.Gestion == "PJ1" || x.Gestion == "JR1").ToList());
            }
        }


    }
}