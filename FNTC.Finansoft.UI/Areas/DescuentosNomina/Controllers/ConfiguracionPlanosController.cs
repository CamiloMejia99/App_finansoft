using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.DescuentosNomina;
using FNTC.Finansoft.Accounting.BLL.DescuentosNomina;


namespace FNTC.Finansoft.UI.Areas.DescuentosNomina.Controllers
{
    [Authorize]
    public class ConfiguracionPlanosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        #region EstructuraPlanos

        public ActionResult EstructuraPlanos()
        {

            return View(db.EstructuraPlanos.ToList());
        }
        public ActionResult CrearEstructuraPlanos()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CrearEstructuraPlanos(EstructuraPlanos estructuraPlanos)
        {
            try
            {

                using (var ctx = new AccountingContext())
                {
                    if ((from s in ctx.EstructuraPlanos where s.NombreEstructuraPlanos == estructuraPlanos.NombreEstructuraPlanos select s).Count() != 0)
                    {
                        return Json(new { ok = false, msg = "NombreExistente" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var respuesta = new GestionConfiguracionBLL().AgregarEstructuraPlanos(estructuraPlanos);
                        if (respuesta == true)
                        {
                            return Json(new { ok = true, toRedirect = Url.Action("EstructuraPlanos") }, JsonRequestBehavior.AllowGet);
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
        public ActionResult EditarEstructuraPlanos(int id)
        {
            
            try
            {
                using (var db = new AccountingContext())
                {
                    EstructuraPlanos estructuraPlanos = db.EstructuraPlanos.Where(a => a.IdEstructuraPlanos == id).FirstOrDefault();
                    return View(estructuraPlanos);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult EditarEstructuraPlanos(EstructuraPlanos estructuraPlanos)
        {
            try
            {

                using (var ctx = new AccountingContext())
                {

                    var respuesta = new GestionConfiguracionBLL().EditarEstructuraPlanos(estructuraPlanos);
                    if (respuesta == true)
                    {
                        return Json(new { ok = true, toRedirect = Url.Action("EstructuraPlanos") }, JsonRequestBehavior.AllowGet);
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
        public ActionResult EliminarEstructuraPlanos(int id)
        {
            var ValidarUsoCP = (from s in db.ConformacionDeLosPlanos where s.IdPlanos == id select s).Count() != 0;
            var ValidarUsoRE = (from s in db.RelacionPlanosEmpresa where s.CodigoPlano == id select s).Count() != 0;
            var ValidarUsoDP = (from s in db.RelacionPlanosDiscriminacion where s.IdPlano == id select s).Count() != 0;
            var ValidarUsoDisPlan = (from s in db.DatosDiscriminacionPlanos where s.idPlano == id select s).Count() != 0;
            var consolidado = 0;
            if (ValidarUsoCP == true || ValidarUsoRE == true || ValidarUsoDP == true || ValidarUsoDisPlan == true)
            {
                consolidado = 1;
            }
            else
            {
                consolidado = 2;
            }

            ViewBag.Consolidado = consolidado;

            try
            {
                using (var db = new AccountingContext())
                {
                    EstructuraPlanos estructuraPlanos = db.EstructuraPlanos.Where(a => a.IdEstructuraPlanos == id).FirstOrDefault();
                    return View(estructuraPlanos);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarEstructuraPlanos(EstructuraPlanos estructuraPlanos, ControlDeMovimientos controlDeMovimientos)
        {
            try
            {
                var Comprobacion = new GestionConfiguracionBLL().EliminarEstructuraPlanos(estructuraPlanos, controlDeMovimientos);
                if (Comprobacion)
                {
                    return RedirectToAction("EstructuraPlanos");
                }
                else
                {
                    return RedirectToAction("EliminarEstructuraPlanos", new { id = estructuraPlanos.IdEstructuraPlanos });
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region ConformacionDeLosPlanos

        public ActionResult ConformacionDeLosPlanos()
        {

            return View(db.EstructuraPlanos.ToList());
        }
        public ActionResult ConformacionDeLosPlanosEstructura(int idPlano)
        {
            ViewBag.IDPLANO = idPlano;
            ViewBag.NombrePlano = (from s in db.EstructuraPlanos where s.IdEstructuraPlanos == idPlano select s.NombreEstructuraPlanos).FirstOrDefault();

            return View(db.ConformacionDeLosPlanos.Where(a => a.IdPlanos == idPlano).ToList());

        }
        public ActionResult ListaCampos()
        {
            using (var db = new AccountingContext())
            {
                return PartialView(db.CamposRelacionDis.ToList());
            }
        }

        public ActionResult NuevoCampo(int id)
        {
            ViewBag.IDPLANO = id;
            ViewBag.NombrePlano = (from s in db.EstructuraPlanos where s.IdEstructuraPlanos == id select s.NombreEstructuraPlanos).FirstOrDefault();

            return View();
        }
        
        [HttpPost]
        public ActionResult NuevoCampo(ConformacionDeLosPlanos conformacionDeLosPlanos)
        {
            try
            {

                using (var ctx = new AccountingContext())
                {
                    if ((from s in ctx.ConformacionDeLosPlanos where s.NombreCampo == conformacionDeLosPlanos.NombreCampo && s.IdPlanos == conformacionDeLosPlanos.IdPlanos select s).Count() != 0)
                    {
                        return Json(new { ok = false, msg = "NombreExistente" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if ((from s in ctx.ConformacionDeLosPlanos where s.Campo == conformacionDeLosPlanos.Campo && s.IdPlanos == conformacionDeLosPlanos.IdPlanos select s).Count() == 0)
                        {
                            var respuesta = new GestionConfiguracionBLL().AgregarNuevoCampo(conformacionDeLosPlanos);
                            if (respuesta == true)
                            {
                                return Json(new { ok = true, toRedirect = Url.Action("ConformacionDeLosPlanosEstructura", "ConfiguracionPlanos", new { idPlano = conformacionDeLosPlanos.IdPlanos }) }, JsonRequestBehavior.AllowGet);
                            }

                            else
                            {
                                return Json(new { ok = false, msg = "ErrorAlGuardar" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { ok = false, msg = "CampoYaSeleccionado" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex + "Error al Guardar" }, JsonRequestBehavior.AllowGet);

            }

        }
        public ActionResult EditarCampo(int id)
        {
            ViewBag.IDPLANO = (from s in db.ConformacionDeLosPlanos where s.IdConformacionDeLosPlanos == id select s.IdPlanos).FirstOrDefault();
            var idPlano = (from s in db.ConformacionDeLosPlanos where s.IdConformacionDeLosPlanos == id select s.IdPlanos).FirstOrDefault();
            ViewBag.NombrePlano = (from s in db.EstructuraPlanos where s.IdEstructuraPlanos == idPlano select s.NombreEstructuraPlanos).FirstOrDefault();

            try
            {
                using (var db = new AccountingContext())
                {
                    ConformacionDeLosPlanos conformacionDeLosPlanos = db.ConformacionDeLosPlanos.Where(a => a.IdConformacionDeLosPlanos == id).FirstOrDefault();
                    return View(conformacionDeLosPlanos);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult EditarCampo(ConformacionDeLosPlanos conformacionDeLosPlanos)
        {
            try
            {

                using (var ctx = new AccountingContext())
                {

                    var respuesta = new GestionConfiguracionBLL().EditarCampo(conformacionDeLosPlanos);
                    if (respuesta == true)
                    {
                        return Json(new { ok = true, toRedirect = Url.Action("ConformacionDeLosPlanosEstructura", "ConfiguracionPlanos", new { idPlano = conformacionDeLosPlanos.IdPlanos }) }, JsonRequestBehavior.AllowGet);
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
        public ActionResult ListaOrden(int id, int idPlano) 
        {
            ViewBag.idOrden = id;
            using (var db = new AccountingContext())
            {
                return PartialView(db.ConformacionDeLosPlanos.Where(a => a.IdPlanos == idPlano).ToList());
            }
        }
        public ActionResult EliminarCampo(int id)
        {
            ViewBag.IDPLANO = (from s in db.ConformacionDeLosPlanos where s.IdConformacionDeLosPlanos == id select s.IdPlanos).FirstOrDefault();
            var idPlano = (from s in db.ConformacionDeLosPlanos where s.IdConformacionDeLosPlanos == id select s.IdPlanos).FirstOrDefault();
            ViewBag.NombrePlano = (from s in db.EstructuraPlanos where s.IdEstructuraPlanos == idPlano select s.NombreEstructuraPlanos).FirstOrDefault();

            try
            {
                using (var db = new AccountingContext())
                {
                    ConformacionDeLosPlanos conformacionDeLosPlanos = db.ConformacionDeLosPlanos.Where(a => a.IdConformacionDeLosPlanos == id).FirstOrDefault();
                    return View(conformacionDeLosPlanos);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarCampo(ConformacionDeLosPlanos conformacionDeLosPlanos, ControlDeMovimientos controlDeMovimientos)
        {

            var Comprobacion = new GestionConfiguracionBLL().EliminarCampo(conformacionDeLosPlanos, controlDeMovimientos);
            if (Comprobacion)
            {
                return Json(new { ok = true, toRedirect = Url.Action("ConformacionDeLosPlanosEstructura", "ConfiguracionPlanos", new { idPlano = conformacionDeLosPlanos.IdPlanos }) }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ok = false, msg = "ErrorAlGuardar" }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region RelacionPlanosEmpresa

        public ActionResult RelacionPlanosEmpresa()
        {
            return View(db.RelacionPlanosEmpresa.Where(a => a.CodigoPlano == 0).ToList());
        }
        [HttpPost]
        public JsonResult GetTerceroEmpresa(string cadena)
        {
         
            var cad = (from x in db.Terceros where x.CLASEID == "31"
                       select new { Id = x.NIT, Nombre = (x.NombreComercial + x.NOMBRE1 + " " + x.NOMBRE2 + " " + x.APELLIDO1 + " " + x.APELLIDO2).ToUpper() })
                .Where(x => x.Id.Contains(cadena) || x.Nombre.Contains(cadena)).ToList();
            return Json(cad, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CrearRelacionPlanosEmpresa()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CrearRelacionPlanosEmpresa(RelacionPlanosEmpresa relacionPlanosEmpresa)
        {
            try
            {

                using (var ctx = new AccountingContext())
                {
                    if ((from s in ctx.RelacionPlanosEmpresa where s.CodigoEmpresa == relacionPlanosEmpresa.CodigoEmpresa && s.CodigoPlano == 0 select s).Count() != 0)
                    {
                        return Json(new { ok = false, msg = "EmpresaYaExiste" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if ((from s in ctx.Terceros where s.NIT == relacionPlanosEmpresa.CodigoEmpresa && s.CLASEID == "31" select s).Count() != 0)
                        {
                            var respuesta = new GestionConfiguracionBLL().AgregarRelacionPlanosEmpresa(relacionPlanosEmpresa);
                            if (respuesta == true)
                            {
                                return Json(new { ok = true, toRedirect = Url.Action("RelacionPlanosEmpresa") }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { ok = false, msg = "ErrorAlGuardar" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { ok = false, msg = "EmpresaNoExiste" }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex + "Error al Guardar" }, JsonRequestBehavior.AllowGet);

            }

        }
        public ActionResult PlanosEmpresa(int idRelacion)
        {
            var idRelacionEmpresa = idRelacion;
            var idEmpresa = (from s in db.RelacionPlanosEmpresa where s.IdRelacionPlanosEmpresa == idRelacionEmpresa select s.CodigoEmpresa).FirstOrDefault();
            ViewBag.idEmpresa = idEmpresa;
            ViewBag.NombreEmpresa = (from s in db.Terceros where s.NIT == idEmpresa select s.NOMBRE).FirstOrDefault();
              
            return View(db.RelacionPlanosEmpresa.Where(a => a.CodigoEmpresa == idEmpresa && a.CodigoPlano != 0).ToList());

        }
        [HttpPost]
        public JsonResult GetPlanos(string cadena)
        {

            var cad = (from x in db.EstructuraPlanos
                       where x.EstadoEstructuraPlanos == true
                       select new { Id = x.IdEstructuraPlanos, Nombre = (x.NombreEstructuraPlanos ).ToUpper() })
                .Where(x => x.Nombre.Contains(cadena)).ToList();
            return Json(cad, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ListaPlanos()
        {
            
            using (var db = new AccountingContext())
            {
                return PartialView(db.EstructuraPlanos.Where(a => a.EstadoEstructuraPlanos == true).ToList());
            }
        }
        public ActionResult CrearPlanosEmpresa(string idEmpresa)
        {
            ViewBag.idEmpresa = idEmpresa;
            var IdInterna = (from s in db.RelacionPlanosEmpresa where s.CodigoEmpresa == idEmpresa select s.IdRelacionPlanosEmpresa).FirstOrDefault();
            ViewBag.IdInterna = IdInterna;
            return View();
        }
        [HttpPost]
        public ActionResult CrearPlanosEmpresa(RelacionPlanosEmpresa relacionPlanosEmpresa)
        {
            try
            {

                using (var ctx = new AccountingContext())
                {
                    if ((from s in ctx.RelacionPlanosEmpresa where s.CodigoPlano == relacionPlanosEmpresa.CodigoPlano && s.CodigoEmpresa == relacionPlanosEmpresa.CodigoEmpresa select s).Count() != 0)
                    {
                        return Json(new { ok = false, msg = "PlanoYaExiste" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var respuesta = new GestionConfiguracionBLL().AgregarRelacionPlanos(relacionPlanosEmpresa);
                        if (respuesta == true)
                        {
                            var idInter = relacionPlanosEmpresa.IdRelacionPlanosEmpresa;
                            return Json(new { ok = true, toRedirect = Url.Action("PlanosEmpresa", "ConfiguracionPlanos", new { idRelacion = idInter }) }, JsonRequestBehavior.AllowGet);
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