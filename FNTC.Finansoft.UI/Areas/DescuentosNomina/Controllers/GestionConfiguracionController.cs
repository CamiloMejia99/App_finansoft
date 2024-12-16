using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.DescuentosNomina;
using FNTC.Finansoft.Accounting.BLL.DescuentosNomina;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace FNTC.Finansoft.UI.Areas.DescuentosNomina.Controllers
{
    
    [Authorize(Roles = "Admin")]

    public class GestionConfiguracionController : Controller
    {
        private AccountingContext db = new AccountingContext();

        #region OrdenPrioridades
        
        public ActionResult ClasificacionPagos()
        {

            return View(db.TipoPagos.ToList());
        }
        public ActionResult CrearNuevaClasificacion()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CrearNuevaClasificacion(TipoPagos tipoPagos)
        {
            try
            {

                using (var ctx = new AccountingContext())
                {
                    if ((from s in ctx.TipoPagos where s.NombrePago == tipoPagos.NombrePago select s).Count() != 0)
                    {
                        return Json(new { ok = false, msg = "NombreExistente" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var respuesta = new GestionConfiguracionBLL().AgregarClasifiacion(tipoPagos);
                        if (respuesta == true)
                        {
                            return Json(new { ok = true, toRedirect = Url.Action("ClasificacionPagos") }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { ok = false, msg = "ErrorAlGuardar" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex + "Error al Guardar" }, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult EditarClasificacion(string id)
        {
           
            try
            {
                using (var db = new AccountingContext())
                {
                    TipoPagos tipopagos = db.TipoPagos.Where(a => a.IdTiposPagos == id).FirstOrDefault();
                    return View(tipopagos); 
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult EditarClasificacion(TipoPagos tipo)
        {
            try
            {

                using (var ctx = new AccountingContext())
                {

                    var respuesta = new GestionConfiguracionBLL().EditarClasificacion(tipo);
                    if (respuesta == true)
                    {
                        return Json(new { ok = true, toRedirect = Url.Action("ClasificacionPagos") }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { ok = false, msg = "ErrorAlGuardar" }, JsonRequestBehavior.AllowGet);
                    }

                }

                
            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex + "Error al Guardar" }, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult OrdenDePrioridades()
        {

            return View(db.OrdenDePrioridadPagos.ToList());
        }
        public ActionResult CrearNuevaCuenta()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CrearNuevaCuenta(OrdenDePrioridadPagos ordenDePrioridadPagos)
        {
            try
            {

                using (var ctx = new AccountingContext())
                {
                    if ((from s in ctx.OrdenDePrioridadPagos where s.CodigoCuenta == ordenDePrioridadPagos.CodigoCuenta select s).Count() != 0)
                    {

                        return Json(new { ok = false, msg = "CuentaExistente" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if ((from s in ctx.PlanCuentas where s.CODIGO == ordenDePrioridadPagos.CodigoCuenta select s).Count() != 0)
                        {
                            if ((from s in ctx.OrdenDePrioridadPagos where s.OrdenPagos == ordenDePrioridadPagos.OrdenPagos select s).Count() != 0)
                            {
                                return Json(new { ok = false, msg = "TipoDePagoAsignado" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                var respuesta = new GestionConfiguracionBLL().AgregarCuenta(ordenDePrioridadPagos);
                                if (respuesta == true)
                                {
                                    return Json(new { ok = true, toRedirect = Url.Action("OrdenDePrioridades") }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    return Json(new { ok = false, msg = "ErrorAlGuardar" }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        else
                        {
                            return Json(new { ok = false, msg = "LaCuentaNoExiste" }, JsonRequestBehavior.AllowGet);
                        }

                        

                    }
                }


            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex + "Error al Guardar" }, JsonRequestBehavior.AllowGet);

            }
        }
        [HttpPost]
        public JsonResult GetCuentasPrioridad(string cadena)
        {
            var cad = (from x in db.PlanCuentas
                       where x.CODIGO.Length == 9
                       select new { Id = x.CODIGO, Nombre = (x.NOMBRE).ToUpper() })
                .Where(x => x.Id.Contains(cadena) || x.Nombre.Contains(cadena)).ToList();
            return Json(cad, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getTipos()
        {
            var lista = db.TipoPagos.ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getOrden()
        {
            var lista = db.TipoPagos.ToList();

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        } 
        public ActionResult EditarCuenta(string id)
        {
            var Cuenta = db.OrdenDePrioridadPagos.Where(a => a.IdOrdenPrioridadPagos == id).Select(a => a.CodigoCuenta).FirstOrDefault();
            var NombreCuenta = db.PlanCuentas.Where(a => a.CODIGO == Cuenta).Select(a => a.NOMBRE).FirstOrDefault();
            ViewBag.Nombre = NombreCuenta;
            var CuentaOrden = db.OrdenDePrioridadPagos.Where(a => a.IdOrdenPrioridadPagos == id).Select(a => a.OrdenPagos).FirstOrDefault();
            var NombreOrden = db.TipoPagos.Where(a => a.IdTiposPagos == CuentaOrden).Select(a => a.NombrePago).FirstOrDefault();
            ViewBag.NombreOrden = NombreOrden;
            var CuentaOrdens = db.OrdenDePrioridadPagos.Where(a => a.IdOrdenPrioridadPagos == id).Select(a => a.OrdenPagos).FirstOrDefault();
            var CuentaOrde = db.TipoPagos.Where(a => a.IdTiposPagos == CuentaOrdens).Select(a => a.Orden).FirstOrDefault();
            ViewBag.CuentaOrdens = CuentaOrde;
            try
            {
                using (var db = new AccountingContext())
                {
                    OrdenDePrioridadPagos ordenDePrioridadPagos = db.OrdenDePrioridadPagos.Where(a => a.IdOrdenPrioridadPagos == id).FirstOrDefault();
                    return View(ordenDePrioridadPagos);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult EditarCuenta(OrdenDePrioridadPagos ordenDePrioridadPagos)
        {
            try
            {

                using (var ctx = new AccountingContext())
                {
                   
                        var respuesta = new GestionConfiguracionBLL().EditarCuenta(ordenDePrioridadPagos);
                        if (respuesta == true)
                        {
                            return Json(new { ok = true, toRedirect = Url.Action("OrdenDePrioridades") }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { ok = false, msg = "ErrorAlGuardar" }, JsonRequestBehavior.AllowGet);
                        }
                    
                }


            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex + "Error al Guardar" }, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult ListaOrden(int id)
        {
            using (var db = new AccountingContext())
            {
                ViewBag.IdOrden = id;
                return PartialView(db.TipoPagos.ToList());
            }
        }
        public ActionResult ListaPrioPagos(string id)
        {
            using (var db = new AccountingContext())
            {
                var orden = db.OrdenDePrioridadPagos.Where(a => a.IdOrdenPrioridadPagos == id).Select(a => a.OrdenPagos).FirstOrDefault();
                ViewBag.IdOrden = orden;
                return PartialView(db.TipoPagos.ToList());
            }
        }
        public ActionResult DetallesOrden(string id) 
        {
            var Cuenta = db.OrdenDePrioridadPagos.Where(a => a.IdOrdenPrioridadPagos == id).Select(a => a.CodigoCuenta).FirstOrDefault();
            var NombreCuenta = db.PlanCuentas.Where(a => a.CODIGO == Cuenta).Select(a => a.NOMBRE).FirstOrDefault();
            ViewBag.Nombre = NombreCuenta;
            var CuentaOrden = db.OrdenDePrioridadPagos.Where(a => a.IdOrdenPrioridadPagos == id).Select(a => a.OrdenPagos).FirstOrDefault();
            var NombreOrden = db.TipoPagos.Where(a => a.IdTiposPagos == CuentaOrden).Select(a => a.NombrePago).FirstOrDefault();
            ViewBag.NombreOrden = NombreOrden;
            try
            {
                using (var db = new AccountingContext())
                {
                    OrdenDePrioridadPagos ordenDePrioridadPagos = db.OrdenDePrioridadPagos.Where(a => a.IdOrdenPrioridadPagos == id).FirstOrDefault();
                    return View(ordenDePrioridadPagos);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult EliminarOrden(string id)
        {
            var Cuenta = db.OrdenDePrioridadPagos.Where(a => a.IdOrdenPrioridadPagos == id).Select(a => a.CodigoCuenta).FirstOrDefault();
            var NombreCuenta = db.PlanCuentas.Where(a => a.CODIGO == Cuenta).Select(a => a.NOMBRE).FirstOrDefault();
            ViewBag.Nombre = NombreCuenta;
            var CuentaOrden = db.OrdenDePrioridadPagos.Where(a => a.IdOrdenPrioridadPagos == id).Select(a => a.OrdenPagos).FirstOrDefault();
            var NombreOrden = db.TipoPagos.Where(a => a.IdTiposPagos == CuentaOrden).Select(a => a.NombrePago).FirstOrDefault();
            ViewBag.NombreOrden = NombreOrden;
            try
            {
                using (var db = new AccountingContext())
                {
                    OrdenDePrioridadPagos ordenDePrioridadPagos = db.OrdenDePrioridadPagos.Where(a => a.IdOrdenPrioridadPagos == id).FirstOrDefault();
                    return View(ordenDePrioridadPagos);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarOrden(OrdenDePrioridadPagos ordenDePrioridadPagos, ControlDeMovimientos controlDeMovimientos)
        {

            var Comprobacion = new GestionConfiguracionBLL().Eliminar(ordenDePrioridadPagos, controlDeMovimientos);
            if (Comprobacion)
            {
                return RedirectToAction("OrdenDePrioridades");
            }
            else
            {
                return RedirectToAction("EliminarOrden", new { id = ordenDePrioridadPagos.IdOrdenPrioridadPagos });
            }
            
        }
        #endregion

        #region ContraPartida

        public ActionResult ContraPartida()
        {
            return View(db.ContraPartida.ToList());
        }
        public ActionResult CrearContraPartida()
        {
            return View(); 
        }
        [HttpPost]
        public ActionResult CrearContraPartida(ContraPartida contraPartida)
        {
            try
            {

                using (var ctx = new AccountingContext())
                {
                    if ((from s in ctx.PlanCuentas where s.CODIGO == contraPartida.CodigoCuenta select s).Count() == 0)
                    {
                        return Json(new { ok = false, msg = "CNoExistente" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var respuesta = new GestionConfiguracionBLL().ActualizarContraPartida(contraPartida);
                        if (respuesta == true)
                        {
                            return Json(new { ok = true, toRedirect = Url.Action("ContraPartida") }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { ok = false, msg = "ErrorAlGuardar" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex + "Error al Guardar" }, JsonRequestBehavior.AllowGet);

            }

        }

        #endregion

        #region SaldosSobrantes
        public ActionResult SaldosSobrantes()
        {
            return View(db.SaldosSobrantes.ToList());
        }
        public ActionResult CrearSaldosSobrantes()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CrearSaldosSobrantes(SaldosSobrantes saldosSobrantes)
        {
            try
            {

                using (var ctx = new AccountingContext())
                {
                    if ((from s in ctx.PlanCuentas where s.CODIGO == saldosSobrantes.CodigoCuenta select s).Count() == 0)
                    {
                        return Json(new { ok = false, msg = "CNoExistente" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var respuesta = new GestionConfiguracionBLL().ActualizarSaldosSobrantes(saldosSobrantes);
                        if (respuesta == true)
                        {
                            return Json(new { ok = true, toRedirect = Url.Action("SaldosSobrantes") }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new { ok = false, msg = "ErrorAlGuardar" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                

            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex + "Error al Guardar" }, JsonRequestBehavior.AllowGet);

            }

        }

        #endregion
    }
}