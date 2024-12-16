using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

using FNTC.Finansoft.Accounting.DTO.Fichas;

using FNTC.Finansoft.Accounting.DTO.Shared;
using FNTC.Finansoft.BLL.Ahorros;
using FNTC.Finansoft.BLL.Aportes;
using FNTC.Finansoft.DTO.Ahorros;
using FNTC.Finansoft.DTO.Aportes;
using FNTC.Finansoft.DTO.Respuestas;
using System.Globalization;
using FNTC.Finansoft.Accounting.DTO.Aportes;
using FNTC.Finansoft.UI.Areas.Accounting.Controllers.PlandeCuentas;

namespace FNTC.Finansoft.Areas.Aportes.Controllers
{
    public class AportesController : Controller
    {
        NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;

        public ActionResult GetUsuario()
        {

            if (!User.Identity.IsAuthenticated)
            {
                var usuario = "noRegistrado";
                return Json(usuario, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var usuario = User.Identity.Name;
                return Json(usuario, JsonRequestBehavior.AllowGet);
            }
        }
        private AccountingContext db = new AccountingContext();
        // GET: Aportes/Aportes
        public ActionResult Index()
        {
            var personas = db.Terceros.Where(ap => ap.ESASOCIADO == 1).ToList();
            //var personasNoAfiliadas = personas.Where(a => !db.FichasAportes.Where(f => f.idPersona.ToString().Equals(a.NIT)).Any()).ToList();

            //lista de terceros(responsable)
            List<SelectListItem> Terceros = new List<SelectListItem>();   // Creo una lista
            Terceros.Add(new SelectListItem { Text = "Seleccione Un Tercero", Value = "" });
            IList<Tercero> ListaDeTerceros = personas;// extraigo los elementos desde la DB

            foreach (var item in ListaDeTerceros)		// recorro los elementos de la db
            {
                Terceros.Add(new SelectListItem { Text = item.NOMBRE + " " + item.APELLIDO1 + "|" + item.NIT, Value = item.NIT.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.tercerosNoAfiliados = Terceros;

            var configuracionGeneral = new BLLAportes().ObtenerConfiguracionGeneral();
            var configuracion = new BLLAportes().ObtenerConfiguracion();
            ViewBag.ConfiguracionAhorros = new BLLAhorros().ObtenerConfiguraciones("FAP").FirstOrDefault();
            if (TempData["respuesta"] != null) configuracion.Respuesta = (DTORespuesta)TempData["respuesta"];
            ViewBag.configuracion = configuracion;
            ViewBag.configuracionGeneral = configuracionGeneral;
            return View();
        }

        
        public ActionResult OtrasCuentas()
        {
            if (TempData["message"] != null)
            {
                ViewBag.message = TempData["message"].ToString();
            }
            return View();
        }

        [HttpPost]
        public JsonResult GetAgenciaTercero(string NIT)
        {
            var nombreAgencia = "";
            if (NIT != "")
            {
                var IdAgencia = (from pc in db.Terceros where pc.NIT == NIT select pc.DEPENDENCIA).Single();
                nombreAgencia = (from pc in db.agencias where pc.codigoagencia == IdAgencia select pc.nombreagencia).Single();
            }
            return Json(nombreAgencia, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetInfoAporte(string NIT)
        {
            var InfoTercero = (from pc in db.Terceros where pc.NIT == NIT select pc).Single();
            var FichaAporte = (from pc in db.FichasAportes where pc.idPersona == NIT select pc).Single();
            var IdAgencia = (from pc in db.Terceros where pc.NIT == NIT select pc.DEPENDENCIA).Single();
            var nombreAgencia = (from pc in db.agencias where pc.codigoagencia == IdAgencia select pc.nombreagencia).Single();

            var result = new {
                identificacion = NIT,
                nombre = InfoTercero.NOMBRE1 + " " + InfoTercero.NOMBRE2 + " " + InfoTercero.APELLIDO1 + " " + InfoTercero.APELLIDO2,
                agencia = nombreAgencia,
                porcentaje = FichaAporte.porcentaje,
                valor = FichaAporte.valor,
                formadepago = FichaAporte.tipoPago,
                activa = FichaAporte.activa,
                asesor = FichaAporte.asesor
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFichasByTercero(string NIT, string cuenta)
        {

            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";

            int n = cuenta.Length;
            cuenta = cuenta.Substring(n - 9);
            if (NIT == null || NIT == "")
            {
                return new JsonResult { Data = new { status = false } };
            }

            List<Array> cuentas = new List<Array>();
            var hayCuenta = db.Configuracion1.Where(x => x.idCuenta == cuenta && x.activo == true).FirstOrDefault();
            if (hayCuenta != null)
            {
                var listaAportes = db.FichasAportes.Where(x => x.idPersona == NIT).ToList();
                if (listaAportes.Count > 0)
                {
                    foreach (var item in listaAportes)
                    {
                        string[] array = new string[2];
                        array[0] = item.numeroCuenta;
                        array[1] = Convert.ToInt64(item.totalAportes).ToString("N0", formato);
                        cuentas.Add(array);

                    }

                }
            }

            return new JsonResult { Data = new { result = cuentas, status = true } };
            //  return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CrearFichaAporte(DTOFichasAportes fichaAporte)
        {
            var salari = fichaAporte.valor;
            var replacement = salari.Replace(','.ToString(), "");
            fichaAporte.valor = replacement;


            fichaAporte.totalAportes = Convert.ToString(0);
            var respuesta = new BLLAportes().CrearFichaAporte(fichaAporte);
            TempData["respuesta"] = respuesta;
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult ObtenerCuotaSobreSalario(string nit)
        {
            string cuota = new BLLAportes().ObtenerCuotaSobreSalario(nit);
            return new JsonResult { Data = new { cuota } };
        }

        [HttpPost]
        public JsonResult VerificaAporte(string IdPersona)
        {
            var respuesta = new BLLAportes().VerificaAporte(IdPersona);
            return new JsonResult { Data = new { respuesta } };
        }

        [HttpPost]
        public ActionResult EditarFichaAporte(DTOFichasAportes fichaAporte)
        {
            var salari = fichaAporte.valor;
            var replacement = salari.Replace(','.ToString(), "");
            fichaAporte.valor = replacement;

            var respuesta = new BLLAportes().ActualizarFichaAporte(fichaAporte);
            TempData["respuesta"] = respuesta;
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            string totalaportes = db.Database.SqlQuery<string>("SELECT totalAportes FROM apo.FichasAportes WHERE idPersona='" + id + "'").FirstOrDefault();
            var num = Convert.ToInt32(totalaportes);
            if (num == 0)
            {
                int noOfRowDeleted = db.Database.ExecuteSqlCommand("DELETE FROM apo.FichasAportes WHERE idPersona='" + id + "'");
            }
            return RedirectToAction("Index");
        }

        public ActionResult Configuracion()
        {
            ViewBag.TiposCalculoCuota = new BLLAportes().ObtenerTiposCuotasCalculo();
            var configuracionGeneral = new BLLAportes().ObtenerConfiguracionGeneral();
            var configuracion = new BLLAportes().ObtenerConfiguracion();
            var respuesta = new DTORespuesta();
            if (ViewBag.respuesta != null) respuesta = ViewBag.respuesta;
            ViewBag.configuracionGeneral = configuracionGeneral;
            ViewBag.respuesta = respuesta;
            return View(configuracion);
        }

        [HttpPost]
        public ActionResult Configuracion(DTOConfiguracionAportes configuracion)
        {
            ViewBag.respuesta = new BLLAportes().GuardarConfiguracion(configuracion);
            ModelState.Clear();
            return Configuracion();
        }

        ////respuestas Json
        //public JsonResult BuscarCuentasContables(string busqueda) {
        //    var cuentas = new BLLAportes().BuscarCuentasContables(busqueda);
        //    return Json(cuentas, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult BuscarAsociados(string busqueda)
        {
            var personas = db.Terceros.Where(aa => aa.NIT.Contains(busqueda) && aa.ESASOCIADO == 1).ToList();
            var personasNoAfiliadas = personas.Where(a => !db.FichasAportes.Where(f => f.idPersona.ToString().Equals(a.NIT)).Any()).ToList();

            //lista de terceros(responsable)
            List<SelectListItem> Terceros = new List<SelectListItem>();   // Creo una lista
            Terceros.Add(new SelectListItem { Text = "Seleccione Un Tercero", Value = "" });
            IList<Tercero> ListaDeTerceros = personasNoAfiliadas;// extraigo los elementos desde la DB

            foreach (var item in ListaDeTerceros)		// recorro los elementos de la db
            {
                Terceros.Add(new SelectListItem { Text = item.NOMBRE + " " + item.APELLIDO1, Value = item.NIT.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.tercerosNoAfiliados = Terceros;

            //var personas = new BLLAportes().BuscarAsociadosNoAfilados(busqueda);
            return Json(personas, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ObtenerAfiliadosAportes()
        {
            //var dal = new FNTC.Finansoft.Accounting.DAL.TercerosDAL();
            // var terceros = dal.GetTerceros(term);
            var terceros = db.FichasAportes.Include(c => c.Terceros).Where(x => x.activa == true).ToList();
            //se daño el automapper
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<Tercero, TerceroDTO>();
            //    //  cfg.AddProfile<FooProfile>();
            //});

            //var map = config.CreateMapper();
            var _tercerosDTO = terceros;
            //terceros.ForEach(x => _tercerosDTO.Add(map.Map<TerceroDTO>(x)));


            // var botonEditar = "<button class='fa fa-pencil edit' onclick='edit(this);'></button>";
            var dataTabledata = _tercerosDTO.Select((x, index)
            => new[] { x.numeroCuenta, x.idPersona, x.Terceros.NOMBRE1+" "+x.Terceros.NOMBRE2+" "+x.Terceros.APELLIDO1+" "+x.Terceros.APELLIDO2, x.tipoPago, x.porcentaje, x.valor,  x.valorCuota, x.totalAportes, x.fechaApertura.ToString(), x.activa.ToString()
            });
            return Json(new { data = dataTabledata }, JsonRequestBehavior.AllowGet);


            //var lista = new BLLAportes().ObtenerAfiliadosAportes();
            //var lista = db.FichasAportes.ToList();

            //var count = lista.Count;
            /*
            var result = new DTODataTables<FichasAportes>
            {
                data = lista,
            };
            */
            //var adsa = result;
            //return Json(new { data = lista}, JsonRequestBehavior.AllowGet);
            /*
            return new JsonResult()
            {
                Data = result,                    
                MaxJsonLength = Int32.MaxValue
            };
            */

        }

        public ActionResult ObtenerOtrasCuentasAportes()
        {
            var OtrasCuentasAportes = new BLLAportes().ObtenerOtrasCuentasAportes();
            var jsonDatatables = new DTODataTables<DTOCuentaDistribucionAporte>
            {
                data = OtrasCuentasAportes
            };
            return Json(jsonDatatables, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ObtenerDetallesFichasAportes(string numeroFicha)
        {
            try
            {
                var lista = new BLLAportes().ObtenerDetallesFichas(numeroFicha);
                var count = lista.Count;
                var result = new DTODataTables<DTODetallesFichas>
                {
                    data = lista,
                };
                //return Json(result, JsonRequestBehavior.AllowGet);
                return new JsonResult()
                {
                    Data = result,
                    MaxJsonLength = Int32.MaxValue
                };
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
        //respuestas Json

        public List<Tercero> ConsultarFichasAportes()
        {
            using (var contexto = new AccountingContext())
            {
                return contexto.Terceros.ToList();
                //return contexto.FichasAportes.ToList();
            }
        }

        public ActionResult CreateOtrasCuentasAportes()
        {
            var CuentasAuxiliares = new PlanDeCuentasController().GetCuentasAuxiliares().ToList().Select(x => new SelectListItem { Text = x.CODIGO + " || " + x.NOMBRE, Value = x.CODIGO, Selected = false });

            ViewBag.CuentasAuxiliares = CuentasAuxiliares;
            ViewBag.Modo = "crear";
            return PartialView();
        }

        [HttpPost]
        public ActionResult CreateOtrasCuentasAportes(CuentaDistribucionAporte CuentaDistribucion)
        {
            var CuentasAuxiliares = new PlanDeCuentasController().GetCuentasAuxiliares().ToList().Select(x => new SelectListItem { Text = x.CODIGO + " || " + x.NOMBRE, Value = x.CODIGO, Selected = false });
            ViewBag.CuentasAuxiliares = CuentasAuxiliares;
            ViewBag.Modo = "crear";
            var respuesta = new BLLAportes().CreateOtrasCuentasAportes(CuentaDistribucion);
            if (respuesta.Correcto)
            {
                TempData["message"] = respuesta.Mensaje;
                string redireccionar = "/Aportes/Aportes/OtrasCuentas";
                return Json(new { success = true, redireccionar });
            }
            else {
                TempData["message"] = null;
                return PartialView(CuentaDistribucion);
            }
        }

        
        public ActionResult EditOtrasCuentasAporte(int Id)
        {
            var CuentasAuxiliares = new PlanDeCuentasController().GetCuentasAuxiliares().ToList().Select(x => new SelectListItem { Text = x.CODIGO + " || " + x.NOMBRE, Value = x.CODIGO, Selected = false });
            ViewBag.CuentasAuxiliares = CuentasAuxiliares;
            ViewBag.Modo = "editar";

            var data = new BLLAportes().GetCuentaDistribucion(Id);
            if (data == null)
            {
                return HttpNotFound();
            }

            data.AuxPorcentaje = data.Porcentaje.ToString();
            return PartialView(data);  
        }


        [HttpPost]
        public JsonResult VerificaExisteOtrasCuentas(string cuenta)
        {
            var respuesta = new BLLAportes().VerificaExisteOtrasCuentas(cuenta);
            return new JsonResult { Data = new { respuesta } };
        }

        [HttpPost]
        public JsonResult CalcularPorcentaje(string porcentaje)
        {
            var respuesta = new BLLAportes().CalcularPorcentaje(porcentaje);
            return new JsonResult { Data = new { respuesta = respuesta.Correcto, mensaje = respuesta.Mensaje } };
        }

        [HttpPost]
        public JsonResult EliminarCuentaDistribucion(int Id)
        {
            var respuesta = new BLLAportes().EliminarCuentaDistribucion(Id);
            return new JsonResult { Data = new { status = respuesta.Correcto, mensaje = respuesta.Mensaje } };
        }

        [HttpGet]
        public ActionResult VerificarConfiguracion()
        {
            var respuesta = new BLLAportes().VeriricarConfiguracion();
            return Json(respuesta.Correcto,JsonRequestBehavior.AllowGet);
        }

    }
}