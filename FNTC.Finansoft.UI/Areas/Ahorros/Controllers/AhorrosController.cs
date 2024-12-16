using FNTC.Finansoft.Accounting.DTO.Shared;
using FNTC.Finansoft.BLL.Ahorros;
using FNTC.Finansoft.BLL.Aportes;
using FNTC.Finansoft.DTO.Ahorros;
using FNTC.Finansoft.DTO;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using FNTC.Finansoft.DTO.Respuestas;
using FNTC.Finansoft.Accounting.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FNTC.Finansoft.Accounting.DTO.Ahorros;
using FNTC.Finansoft.UI.Tools;
using System.Net;
using System.Data.Entity;

namespace FNTC.Finansoft.Areas.Ahorros.Controllers
{
  
    [Authorize]
    public class AhorrosController : Controller
    {
        private AccountingContext db = new AccountingContext();
        NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;
        #region Ahorro Permanente 
        public ActionResult AhorroPermante()
        {
            var configuracionGeneral = new BLLAportes().ObtenerConfiguracionGeneral();
            ViewBag.configuracionGeneral = configuracionGeneral;
            var configuracionAportes = new BLLAportes().ObtenerConfiguracion();
            var configuracionAhorro = new BLLAhorros().ObtenerConfiguraciones("FAP").FirstOrDefault();
            if (TempData["respuesta"] != null) configuracionAhorro.Respuesta = (DTORespuesta)TempData["respuesta"];
            ViewBag.ConfiguracionAhorros = configuracionAhorro;
            ViewBag.ConfiguracionAportes = configuracionAportes;

            //lista de terceros
            //==================
            var personas = db.Terceros.Where(ap => ap.ESASOCIADO == 1).ToList();
            //var personasNoAfiliadas = personas.Where(a => !db.FichasAportes.Where(f => f.idPersona.ToString().Equals(a.NIT)).Any()).ToList();

            //lista de terceros(responsable)
            List<SelectListItem> Terceros = new List<SelectListItem>();   // Creo una lista
            Terceros.Add(new SelectListItem { Text = "Seleccione Un Tercero", Value = "" });
            IList<Tercero> ListaDeTerceros = personas;// extraigo los elementos desde la DB

            foreach (var item in ListaDeTerceros)		// recorro los elementos de la db
            {
                Terceros.Add(new SelectListItem { Text = item.NOMBRE + " " + item.NOMBRE2+" "+item.APELLIDO1+" "+item.APELLIDO2 + "|" + item.NIT, Value = item.NIT.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.tercerosNoAfiliados = Terceros;

            return View();
        }
        public ActionResult ConfiguracionFAP()
        {




            var configuracionAportes = new BLLAportes().ObtenerConfiguracion();
            ViewBag.configuracionAportes = configuracionAportes;
            var configuracionAhorros = new BLLAhorros().ObtenerConfiguraciones("FAP").FirstOrDefault();
            var respuesta = new DTORespuesta();
            if (ViewBag.respuesta != null) respuesta = ViewBag.respuesta;
            ViewBag.respuesta = respuesta;
            return View(configuracionAhorros);
        }
        [HttpPost]
        public ActionResult ConfiguracionFAP(DTOConfiguracionAhorros configuracion)
        {
            configuracion.tipoAhorro = "FAP";
            var respuesta = new BLLAhorros().GuardarConfiguracion(configuracion);
            ViewBag.respuesta = respuesta;
            return ConfiguracionFAP();
        }
        // FAP = "Ficha Ahorro Permanente"
        public ActionResult CrearFAP(DTOFichasAhorros fichaAhorro)
        {
            fichaAhorro.tipoFicha = "FAP";
            var respuesta = new BLLAhorros().CrearFichaAhorroFAP(fichaAhorro,"registrar");
            TempData["respuesta"] = respuesta;
            return RedirectToAction("AhorroPermante");
        }
        public ActionResult EditarFAP(DTOFichasAhorros fichaAhorro)
        {
            fichaAhorro.tipoFicha = "FAP";
            var respuesta = new BLLAhorros().CrearFichaAhorroFAP(fichaAhorro, "modificar");
            TempData["respuesta"] = respuesta;

            return RedirectToAction("AhorroPermante");
        }
        public JsonResult ObtenerFichasAhorroPermanente()
        {
            var fichasAhorrosPermanente = new BLLAhorros().ObtenerFichasAhorros("FAP");
            var jsonDatatables = new DTODataTables<DTOFichasAhorros> {
                data = fichasAhorrosPermanente                
            };
            return Json(jsonDatatables, JsonRequestBehavior.AllowGet);
        }
        public JsonResult BuscarAsociadosNoAfiliados(string busqueda)
        {
            var asociadosNoAfiliados = new BLLAhorros().BuscarAsociadosNoAfilados(busqueda,"FAP");            
            return Json(asociadosNoAfiliados, JsonRequestBehavior.AllowGet);
        }
        #endregion Ahorro Permanente 

        #region Ahorro CDAT
        public ActionResult AhorroCDAT()
        {
            return View();
        }
        public ActionResult ConfiguracionFACDAT()
        {
            List<SelectListItem> Cuentas = new List<SelectListItem>();   // Creo una lista
            Cuentas.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });
            //var FormatoVinculacion = (from pc in db.formatoVinculacions selec pc).Single();
            //var CuentaAuxiliar = (from pc in db.PlanCuentas select pc).LongCount();

            // IList<>  = db.PlanCuentas.ToList();// extraigo los elementos desde la DB
            IList<CuentaMayor> ListaDeCuentas = db.PlanCuentas.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeCuentas)		// recorro los elementos de la db
            {
                //var codigo = item.CODIGO;
                if(item.CODIGO.Length==9)
                {
                    Cuentas.Add(new SelectListItem { Text = item.CODIGO + " || " + item.NOMBRE, Value = item.CODIGO });  // agrego los elementos de la db a la primera lista que cree

                }


                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Cuentas = Cuentas;

            List<SelectListItem> ClaseComprobantes = new List<SelectListItem>();   // Creo una lista
            ClaseComprobantes.Add(new SelectListItem { Text = "Seleccione Una fuente contable", Value = "" });
            IList<ClaseComprobante> ListaDeComprobantes = db.ClaseComprobante.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeComprobantes)		// recorro los elementos de la db
            {
                ClaseComprobantes.Add(new SelectListItem { Text = item.Codigo + " || " + item.Nombre, Value = item.Codigo });  // agrego los elementos de la db a la primera lista que cree

                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.ClaseComprobantes = ClaseComprobantes;

            List<SelectListItem> Terceros = new List<SelectListItem>();   // Creo una lista
            Terceros.Add(new SelectListItem { Text = "Seleccione Un tercero", Value = "" });
            IList<Tercero> ListaDeTerceros = db.Terceros.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeTerceros)		// recorro los elementos de la db
            {
            
                Terceros.Add(new SelectListItem { Text = item.NIT + " || " + item.NOMBRE1 + " " + item.NOMBRE2 + " " + item.APELLIDO1 + " " + item.APELLIDO2 , Value = item.NIT });  // agrego los elementos de la db a la primera lista que cree

                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Terceros = Terceros;




            if (TempData["respuesta"] != null) ViewBag.Respuesta = TempData["respuesta"];
            return View();
        }
        [HttpPost]
        public ActionResult ConfiguracionFACDAT(DTOConfiguracionAhorros configuracionFACDAT)
        {
            if (configuracionFACDAT.valorMinimo != null)
            {
                configuracionFACDAT.valorMinimo = configuracionFACDAT.valorMinimo.Replace('.'.ToString(), "");
            }
            if (configuracionFACDAT.valorMaximo != null)
            {
                configuracionFACDAT.valorMaximo = configuracionFACDAT.valorMaximo.Replace('.'.ToString(), "");
            }
            configuracionFACDAT.tipoAhorro = "FACDAT";
            TempData["respuesta"] = new BLLAhorros().GuardarConfiguracionFACDAT(configuracionFACDAT, "Registrar");
            return RedirectToAction("ConfiguracionFACDAT");
        }
        public ActionResult EditarConfiguracionesFACDAT(DTOConfiguracionAhorros configuracionCDAT)
        {
            if (configuracionCDAT.valorMinimo != null)
            {
                configuracionCDAT.valorMinimo = configuracionCDAT.valorMinimo.Replace('.'.ToString(), "");
            }
            if (configuracionCDAT.valorMaximo != null)
            {
                configuracionCDAT.valorMaximo = configuracionCDAT.valorMaximo.Replace('.'.ToString(), "");
            }
            TempData["respuesta"] = new BLLAhorros().GuardarConfiguracionFACDAT(configuracionCDAT, "Modificar");
            return RedirectToAction("ConfiguracionFACDAT");
        }
        public JsonResult ObtenerConfiguracionesFACDAT()
        {
            //aqui poner ceparador de miles para mostrar
            var configuracionesFCDAT = new BLLAhorros().ObtenerConfiguraciones("FACDAT");
            var resultado = new DTODataTables<DTOConfiguracionAhorros>()
            {
                data = configuracionesFCDAT
           
            };
           
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CrearFACDAT(DTOFichasAhorros fichaAhorroCDAT)
        {
            if (fichaAhorroCDAT.valorTitulo != null)
            {
                fichaAhorroCDAT.valorTitulo = fichaAhorroCDAT.valorTitulo.Replace('.'.ToString(), "");
            }

            fichaAhorroCDAT.tipoFicha = "FACDAT";
            var respuesta = new BLLAhorros().CrearFACDAT(fichaAhorroCDAT, "Registrar");
            return RedirectToAction("ConfiguracionFACDAT");
        }
        public ActionResult EditarFACDAT(DTOFichasAhorros fichaAhorroCDAT)
        {
            if (fichaAhorroCDAT.valorTitulo != null)
            {
                fichaAhorroCDAT.valorTitulo = fichaAhorroCDAT.valorTitulo.Replace('.'.ToString(), "");
            }
           // fichaAhorroCDAT.fechaApertura = DateTime.Now;
            fichaAhorroCDAT.tipoFicha = "FACDAT";
            var respuesta = new BLLAhorros().CrearFACDAT(fichaAhorroCDAT, "Modificar");
            return RedirectToAction("ConfiguracionFACDAT");
           
        }
        public JsonResult BuscarAsociadosNoAfiliadosFACDAT(string busqueda)
        {
            var asociadosNoAfiliados = new BLLAhorros().BuscarAsociadosNoAfilados(busqueda, "FACDAT");
            return Json(asociadosNoAfiliados, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ObtenerAfiliadosConfiguracionesFACDAT(int idConfiguracionCDAT)
        {
            var FichasCDAT = new BLLAhorros().ObtenerAfiliadosConfiguracionesAhorros(idConfiguracionCDAT);
            var resultado = new DTODataTables<DTOFichasAhorros>()
            {
                data = FichasCDAT
            };
            return Json(resultado, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDatosTercero(string NIT)
        {
            var tercero = (from pc in db.Terceros where pc.NIT == NIT select pc).Single();
            //var Agencias = (from pc in db.agencias where pc.codigoagencia == tercero.DEPENDENCIA select pc.nombreagencia).Single();
            var Agencias = (from pc in db.agencias where pc.codigoagencia == tercero.DEPENDENCIA select pc.nombreagencia).Single();
            //var Categoria = (from pc in db.ScoringVariableCategorias where pc.NombreCategoria == NombreCategoria select pc).Single();
            List<string> codigos = new List<string>();
            codigos.Add(Agencias.ToString());
            return Json(codigos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult validarMaxMin(string idConfiguracion)
        {
            int idCon = Int32.Parse(idConfiguracion);
            var configuracion = (from pc in db.Configuracion where pc.id == idCon select pc).Single();
            //var Agencias = (from pc in db.agencias where pc.codigoagencia == tercero.DEPENDENCIA select pc.nombreagencia).Single();
            //var Categoria = (from pc in db.ScoringVariableCategorias where pc.NombreCategoria == NombreCategoria select pc).Single();
            List<string> codigos = new List<string>();
            codigos.Add(configuracion.valorMinimo.ToString());
            codigos.Add(configuracion.valorMaximo.ToString());
            return Json(codigos, JsonRequestBehavior.AllowGet);

            

        }
    
        public JsonResult validarIdPersona(string idPersona, string id1, string idConfiguracion)
        {



            if (id1 == "")
            {
                int idConfiguracion1 = Int32.Parse(idConfiguracion);

                var personas = (from pc in db.FichasAhorros where pc.idPersona == idPersona && pc.idConfiguracion == idConfiguracion1 select pc).Count();
                //var Agencias = (from pc in db.agencias where pc.codigoagencia == tercero.DEPENDENCIA select pc.nombreagencia).Single();
                //var Categoria = (from pc in db.ScoringVariableCategorias where pc.NombreCategoria == NombreCategoria select pc).Single();
                List<string> codigos = new List<string>();

                if (personas == 0)
                {
                    codigos.Add("true");
                }
                else
                {
                    codigos.Add("false");
                }

                return Json(codigos, JsonRequestBehavior.AllowGet);
            }
            else
            {

                int id111 = Int32.Parse(id1);
                int idConfiguracion1 = Int32.Parse(idConfiguracion);

                var personas1 = (from D in db.FichasAhorros
                                 where D.idPersona == idPersona && D.idConfiguracion == idConfiguracion1
                                 select D).Except(from D in db.FichasAhorros
                                                  where D.id == id111 && D.idConfiguracion == idConfiguracion1
                                                  select D).Count();






                //var Agencias = (from pc in db.agencias where pc.codigoagencia == tercero.DEPENDENCIA select pc.nombreagencia).Single();
                //var Categoria = (from pc in db.ScoringVariableCategorias where pc.NombreCategoria == NombreCategoria select pc).Single();
                List<string> codigos = new List<string>();

                if (personas1 == 0)
                {
                    codigos.Add("true");
                }
                else
                {
                    codigos.Add("false");
                }

                return Json(codigos, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion





        #region AHORRO CONTRACTUAL

        public ActionResult ConfiguracionFAC()
        {
            var respuesta = (DTORespuesta)TempData["respuesta"];
            if (respuesta != null && respuesta.Mensaje != null)
            {
                TempData["bandera"] = respuesta.Correcto;
                TempData["mensaje"] = respuesta.Mensaje;
            }
            ViewData["configuracion"] = new SelectList(db.ConfigAhorrosContractuales.Select(x => new { Id = x.Id.ToString(), NOMBRE = x.NombreConfiguracion + " (" + x.Prefijo + ")" }), "Id", "NOMBRE");
            ViewData["periodicidad"] = new SelectList(db.Tipo_Periodo.Select(x => new { Id = x.Tipo_Periodo_Id.ToString(), NOMBRE = x.Tipo_Periodo_Descripcion }), "Id", "NOMBRE");
            return View();
        }

        [Authorize(Roles = "Admin")] 
        public ActionResult ConfiguracionAhorroContractual()
        {
            var respuesta = (DTORespuesta)TempData["respuesta"];
            if (respuesta!=null && respuesta.Mensaje!=null)
            {
                TempData["bandera"] = respuesta.Correcto;
                TempData["mensaje"] = respuesta.Mensaje;
            }
            ViewData["TiposComprobantes"] = new SelectList(db.TiposComprobantes.Select(x => new { CODIGO = x.CODIGO, NOMBRE = x.CODIGO+" - "+x.NOMBRE}), "CODIGO", "NOMBRE");
            ViewData["Cuentas"] = new SelectList(db.PlanCuentas.Where(x=>x.CODIGO.Length>=8).Select(x => new { CODIGO= x.CODIGO,NOMBRE = x.CODIGO+" - "+x.NOMBRE}), "CODIGO", "NOMBRE");
            return View();
        }

        

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult GetConfigAhorroContractual()
        {
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            
            var data = db.ConfigAhorrosContractuales.ToList();
            string botones = "",comprobante="",cuenta="",cuentaCausacion="",cuentaGasto="";
            var result = data.Select((x, index) => new[] {
                x.Id.ToString(),
                x.NombreConfiguracion,
                x.Prefijo,
                x.ValorMinimo.ToString("N0",formato),
                x.ValorMaximo.ToString("N0",formato),
                comprobante = x.TipoComprobanteFK.CODIGO+" - "+x.TipoComprobanteFK.NOMBRE,
                x.PlazoMinimo.ToString(),
                x.TasaEfectivaMinima.ToString("N2"),
                x.TasaEfectivaMaxima.ToString("N2"),
                cuenta = x.CuentaFK.CODIGO+" - "+x.CuentaFK.NOMBRE,
                cuentaCausacion = x.CuentaCausacionFK.CODIGO+" - "+x.CuentaCausacionFK.NOMBRE,
                cuentaGasto = x.CuentaGastoFK.CODIGO+" - "+x.CuentaGastoFK.NOMBRE,
                x.SeCausa ? "SI" : "NO",
                x.Morosidad ? "SI" : "NO",
                x.SeCausaEnMora ? "SI" : "NO",
                x.CuotasGraciaMora.ToString(),
                x.RetiroAnticipado ? "SI" : "NO",
                x.Estado ? "Activo" : "Inactivo",
                botones= "<a href='/Ahorros/Ahorros/EditarConfigAhorroContractual?id="+x.Id+"' id='"+x.Id+"' class='btn btn-warning btn-xs fa fa-pencil'></a>"
            });
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = 500000000;
            var json = Json(new { data = result }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;
            
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetFichasAhorroContractual()
        {
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";

            var data = await db.FichasAhorroContractual.ToListAsync();
            string botones = "", configuracion = "", asociado = "", periodo = "", cuentaGasto = "";
            var result = data.Select((x, index) => new[] {
                x.Id.ToString(),
                configuracion = x.ConfACFK!=null ? x.ConfACFK.NombreConfiguracion+" ("+x.ConfACFK.Prefijo+")" : "",
                x.IdAsociado,
                asociado = x.TerceroFK!=null ? (x.TerceroFK.NombreComercial+""+x.TerceroFK.NOMBRE1+" "+x.TerceroFK.NOMBRE2+" "+x.TerceroFK.APELLIDO1+" "+x.TerceroFK.APELLIDO2).ToUpper() : "",
                x.NumeroCuenta,
                x.ValorCuota.ToString("N0"),
                x.TotalAhorro.ToString("N0"),
                x.Interes.ToString("N2",formato),
                x.Plazo.ToString(),
                x.TasaEfectiva.ToString("N2",formato),
                periodo = x.TipoPeriodoFK!=null ? x.TipoPeriodoFK.Tipo_Periodo_Descripcion : "",
                x.FechaApertura.ToString("dd-MM-yyyy"),
                x.FechaVencimiento.ToString("dd-MM-yyyy"),
                x.Estado ? "Activo" : "Inactivo",
                botones= "<a href='/Ahorros/Ahorros/EditarFichaAC?id="+x.Id+"' id='"+x.Id+"' class='btn btn-warning btn-xs fa fa-pencil'></a>"
            });
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = 500000000;
            var json = Json(new { data = result }, JsonRequestBehavior.AllowGet);
            json.MaxJsonLength = 500000000;
            return json;

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CrearConfigAhorroContractual(ConfigAhorroContractual model)
        {
            var request = new DTORespuesta();
            try
            {
                model.ValorMinimo = Convert.ToDecimal(model.AuxValorMinimo.Replace(".", ""));
                model.ValorMaximo = Convert.ToDecimal(model.AuxValorMaximo.Replace(".", ""));
                model.FechaRegistro = Fecha.GetFechaColombia();
                model.UserId = User.Identity.Name;
                model.AuxTasaEfectivaMinima = model.AuxTasaEfectivaMinima.Replace(".", ",");
                model.TasaEfectivaMinima = Convert.ToDecimal(model.AuxTasaEfectivaMinima);
                model.AuxTasaEfectivaMaxima = model.AuxTasaEfectivaMaxima.Replace(".", ",");
                model.TasaEfectivaMaxima = Convert.ToDecimal(model.AuxTasaEfectivaMaxima);

                var errors = ModelState.Values.SelectMany(v => v.Errors);

                if (ModelState.IsValid)
                {
                    var respuesta = await new BLLAhorros().GuardarConfAhoCont(model);
                    if (respuesta) {
                        request = request.GenerarRespuestaBasica(respuesta);
                        TempData["respuesta"] = request;
                        return RedirectToAction("ConfiguracionAhorroContractual", "Ahorros");
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            TempData["respuesta"] = request = request.GenerarRespuestaBasica(false);
            ViewData["TiposComprobantes"] = new SelectList(db.TiposComprobantes.Select(x => new { CODIGO = x.CODIGO, NOMBRE = x.CODIGO + " - " + x.NOMBRE }), "CODIGO", "NOMBRE");
            ViewData["Cuentas"] = new SelectList(db.PlanCuentas.Where(x => x.CODIGO.Length >= 8).Select(x => new { CODIGO = x.CODIGO, NOMBRE = x.CODIGO + " - " + x.NOMBRE }), "CODIGO", "NOMBRE");
            return RedirectToAction("ConfiguracionAhorroContractual", "Ahorros");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> EditarConfigAhorroContractual(ConfigAhorroContractual model)
        {
            var request = new DTORespuesta();
            try
            {
                model.ValorMinimo = Convert.ToDecimal(model.AuxValorMinimo.Replace(".", ""));
                model.ValorMaximo = Convert.ToDecimal(model.AuxValorMaximo.Replace(".", ""));
                model.AuxTasaEfectivaMinima = model.AuxTasaEfectivaMinima.Replace(".", ",");
                model.TasaEfectivaMinima = Convert.ToDecimal(model.AuxTasaEfectivaMinima);
                model.AuxTasaEfectivaMaxima = model.AuxTasaEfectivaMaxima.Replace(".", ",");
                model.TasaEfectivaMaxima = Convert.ToDecimal(model.AuxTasaEfectivaMaxima);

                var errors = ModelState.Values.SelectMany(v => v.Errors);

                if (ModelState.IsValid)
                {
                    var respuesta = await new BLLAhorros().EditarConfAhoCont(model);
                    if (respuesta)
                    {
                        request = request.GenerarRespuestaBasica(respuesta);
                        TempData["respuesta"] = request;
                        return RedirectToAction("ConfiguracionAhorroContractual", "Ahorros");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["respuesta"] = request = request.GenerarRespuestaBasica(false);
                return RedirectToAction("ConfiguracionAhorroContractual", "Ahorros");
            }
            
            ViewData["TiposComprobantes"] = new SelectList(db.TiposComprobantes.Select(x => new { CODIGO = x.CODIGO, NOMBRE = x.CODIGO + " - " + x.NOMBRE }), "CODIGO", "NOMBRE");
            ViewData["Cuentas"] = new SelectList(db.PlanCuentas.Where(x => x.CODIGO.Length >= 8).Select(x => new { CODIGO = x.CODIGO, NOMBRE = x.CODIGO + " - " + x.NOMBRE }), "CODIGO", "NOMBRE");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarFichaAC(FichaAhorroContractual model)
        {
            var request = new DTORespuesta();
            try
            {
                model.ValorCuota = Convert.ToDecimal(model.AuxValorCuota.Replace(".", ""));
                model.TasaEfectiva = Convert.ToDecimal(model.AuxTasaEfectiva.Replace(".", ","));
                model.FechaVencimiento = Convert.ToDateTime(CalcularFechaVencimiento(model.Plazo, model.FechaApertura.ToString()));
                

                var errors = ModelState.Values.SelectMany(v => v.Errors);

                if (ModelState.IsValid)
                {
                    var respuesta = await new BLLAhorros().EditarFichaAC(model);
                    if (respuesta)
                    {
                        request = request.GenerarRespuestaBasica(respuesta);
                        TempData["respuesta"] = request;
                        return RedirectToAction("ConfiguracionFAC", "Ahorros");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["respuesta"] = request = request.GenerarRespuestaBasica(false);
                return RedirectToAction("ConfiguracionFAC", "Ahorros");
            }

            ViewData["TiposComprobantes"] = new SelectList(db.TiposComprobantes.Select(x => new { CODIGO = x.CODIGO, NOMBRE = x.CODIGO + " - " + x.NOMBRE }), "CODIGO", "NOMBRE");
            ViewData["Cuentas"] = new SelectList(db.PlanCuentas.Where(x => x.CODIGO.Length >= 8).Select(x => new { CODIGO = x.CODIGO, NOMBRE = x.CODIGO + " - " + x.NOMBRE }), "CODIGO", "NOMBRE");
            return View(model);
        }

        public ActionResult EditarConfigAhorroContractual(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.ConfigAhorrosContractuales.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }
            data.AuxTasaEfectivaMinima = data.TasaEfectivaMinima.ToString("N2");
            data.AuxTasaEfectivaMaxima = data.TasaEfectivaMaxima.ToString("N2");
            data.AuxValorMinimo = data.ValorMinimo.ToString("N0", formato);
            data.AuxValorMaximo = data.ValorMaximo.ToString("N0", formato);

            ViewData["TiposComprobantes"] = new SelectList(db.TiposComprobantes.Select(x => new { CODIGO = x.CODIGO, NOMBRE = x.CODIGO + " - " + x.NOMBRE }), "CODIGO", "NOMBRE",data.IdComprobante);
            ViewData["Cuentas"] = new SelectList(db.PlanCuentas.Where(x => x.CODIGO.Length >= 8).Select(x => new { CODIGO = x.CODIGO, NOMBRE = x.CODIGO + " - " + x.NOMBRE }), "CODIGO", "NOMBRE",data.IdCuenta);
            return View(data);
        }

        public ActionResult EditarFichaAC(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.FichasAhorroContractual.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }
            data.AuxTasaEfectiva = data.TasaEfectiva.ToString("N2");
            data.AuxValorCuota = data.ValorCuota.ToString("N0", formato);
            data.AuxFechaVencimiento = data.FechaVencimiento.ToString("dd/MM/yyyy");

            ViewData["configuracion"] = new SelectList(db.ConfigAhorrosContractuales.Select(x => new { Id = x.Id.ToString(), NOMBRE = x.NombreConfiguracion + " (" + x.Prefijo + ")" }), "Id", "NOMBRE",data.IdConfiguracion);
            ViewData["periodicidad"] = new SelectList(db.Tipo_Periodo.Select(x => new { Id = x.Tipo_Periodo_Id.ToString(), NOMBRE = x.Tipo_Periodo_Descripcion }), "Id", "NOMBRE",data.IdTipoPeriodo);
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CrearFichaAC(FichaAhorroContractual model)
        {
            var request = new DTORespuesta();
            try
            {
                model.Id = Guid.NewGuid().ToString();
                model.ValorCuota = Convert.ToDecimal(model.AuxValorCuota.Replace(".", ""));
                model.TasaEfectiva = Convert.ToDecimal(model.AuxTasaEfectiva.Replace(".", ","));
                model.FechaVencimiento = Convert.ToDateTime(CalcularFechaVencimiento(model.Plazo, model.FechaApertura.ToString()));
                model.FechaSistema = FechaLocal.GetFechaColombia();
                model.UserId = User.Identity.Name;


                var errors = ModelState.Values.SelectMany(v => v.Errors);

                if (ModelState.IsValid)
                {
                    var respuesta = await new BLLAhorros().GuardarFichaAC(model);
                    if (respuesta)
                    {
                        request = request.GenerarRespuestaBasica(respuesta);
                        TempData["respuesta"] = request;
                        return RedirectToAction("ConfiguracionFAC", "Ahorros");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            TempData["respuesta"] = request = request.GenerarRespuestaBasica(false);
            ViewData["TiposComprobantes"] = new SelectList(db.TiposComprobantes.Select(x => new { CODIGO = x.CODIGO, NOMBRE = x.CODIGO + " - " + x.NOMBRE }), "CODIGO", "NOMBRE");
            ViewData["Cuentas"] = new SelectList(db.PlanCuentas.Where(x => x.CODIGO.Length >= 8).Select(x => new { CODIGO = x.CODIGO, NOMBRE = x.CODIGO + " - " + x.NOMBRE }), "CODIGO", "NOMBRE");
            return RedirectToAction("ConfiguracionFAC", "Ahorros");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<JsonResult> GetConfigAhoCont(int id)
        {

            var model = await new BLLAhorros().GetConfigAhoCont(id);
            return new JsonResult { Data = new { status = model != null ? true : false, model } };

        }

        [HttpPost]
        public async Task<JsonResult> GetPlazoAndTasa(int id)
        {
            var data = await new BLLAhorros().GetPlazoAndTasa(id);
            if(data == null)
                return new JsonResult { Data = new { status = false } };

            var array = data.ToArray();
            return new JsonResult { Data = new { status = true,plazo=array[0],tasa=array[1].Replace(",","."),cuotaMinima= array[2] } };
        }

        [HttpPost]
        public async Task<JsonResult> VerificaRangoCuotaAC(int idConfig, string valor)
        {
            return await new BLLAhorros().VerificaRangoCuotaAC(idConfig, valor);
        }
        
        [HttpPost]
        public async Task<JsonResult> VerificaRangoPlazoAC(int idConfig, string valor)
        {
            return await new BLLAhorros().VerificaRangoPlazoAC(idConfig, valor);
        }

        [HttpPost]
        public async Task<JsonResult> VerificaRangoTasaAC(int idConfig, string valor)
        {
            return await new BLLAhorros().VerificaRangoTasaAC(idConfig, valor);
        }
        [HttpPost]
        public async Task<JsonResult> VerificaTerceroByFichaAC(string nit, int idConfig)
        {
            
            bool bandera = false;
            string numeroCuenta = "";
            try
            {
                var ficha = await db.FichasAhorroContractual.Where(x => x.IdAsociado == nit && x.IdConfiguracion == idConfig).FirstOrDefaultAsync();
                if (ficha != null)
                {
                    bandera = true;
                }
                else {
                    string prefijo = await db.ConfigAhorrosContractuales.Where(x => x.Id == idConfig).Select(x => x.Prefijo).FirstOrDefaultAsync();
                    numeroCuenta=prefijo+nit;
                }

                
            }
            catch (Exception ex)
            {
                bandera=false;
            }
            return new JsonResult { Data = new { status = bandera, numeroCuenta } };
        }

        public JsonResult GetFechaVencimiento(int plazo, string fecha)
        { 
            var fechaVencimiento = CalcularFechaVencimiento(plazo,fecha);
            return new JsonResult { Data = new { fechaVencimiento } };

        }

        private string CalcularFechaVencimiento(int plazo, string fecha)
        {
            var nuevaFecha = Convert.ToDateTime(fecha);
            nuevaFecha = nuevaFecha.AddMonths(plazo);
            return nuevaFecha.ToString("dd/MM/yyyy");
        }
        #endregion
    }
}