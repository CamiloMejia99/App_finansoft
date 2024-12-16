using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using FNTC.Finansoft.Accounting.DTO.ActivosFijos;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.BLL;
using Newtonsoft.Json;
using System.Globalization;

namespace FNTC.Finansoft.UI.Areas.ActivosFijos.Controllers
{
    public class ActivosFijosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: ActivosFijos/ActivosFijos
        public ActionResult Index()
        {
            return View(db.ActivosFijos.ToList());
        }

        // GET: ActivosFijos/ActivosFijos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BActivosFijos bActivosFijos = db.ActivosFijos.Find(id);
            if (bActivosFijos == null)
            {
                return HttpNotFound();
            }
            return View(bActivosFijos);
        }

        // GET: ActivosFijos/ActivosFijos/Create
        public ActionResult Create()
        {
            //lista de Grupos
            List<SelectListItem> Grupos = new List<SelectListItem>();   // Creo una lista
            Grupos.Add(new SelectListItem { Text = "Seleccione Un Grupo", Value = "" });
            IList<GruposActivosFijos> ListaDeGrupos = (from l in db.GruposActivosFijos select l).ToList();

            foreach (var item in ListaDeGrupos)		// recorro los elementos de la db
            {

                Grupos.Add(new SelectListItem { Text = item.nombre, Value = item.id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.grupoId = Grupos;

            //lista de Clases
            List<SelectListItem> Clases = new List<SelectListItem>();   // Creo una lista
            Clases.Add(new SelectListItem { Text = "Seleccione Una Clase", Value = "" });
            IList<ClaseDeActivo> ListaDeClases = (from l in db.ClaseDeActivo select l).ToList();

            foreach (var item in ListaDeClases)		// recorro los elementos de la db
            {

                Clases.Add(new SelectListItem { Text = item.nombre, Value = item.id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.claseActivoId = Clases;

            //lista de Ubicaciones
            List<SelectListItem> Ubicaciones = new List<SelectListItem>();   // Creo una lista
            Ubicaciones.Add(new SelectListItem { Text = "Seleccione Una Ubicacion", Value = "" });
            IList<UbicacionFisica> ListaDeUbicaciones = (from l in db.UbicacionFisica select l).ToList();

            foreach (var item in ListaDeUbicaciones)		// recorro los elementos de la db
            {

                Ubicaciones.Add(new SelectListItem { Text = item.nombre, Value = item.id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.ubicacionFisicaId = Ubicaciones;

            //lista de terceros(responsable)
            List<SelectListItem> Terceros = new List<SelectListItem>();   // Creo una lista
            Terceros.Add(new SelectListItem { Text = "Seleccione Un Tercero", Value = "" });
            IList<Tercero> ListaDeTerceros = db.Terceros.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeTerceros)		// recorro los elementos de la db
            {
                Terceros.Add(new SelectListItem { Text = item.NOMBRE1 + " " + item.NOMBRE2 + " " + item.APELLIDO1 + " " + item.APELLIDO2 + " || " + item.NIT, Value = item.NIT.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }

            ViewBag.responsableNIT = Terceros;

            //lista de centro de costos
            List<SelectListItem> CC = new List<SelectListItem>();   // Creo una lista
            CC.Add(new SelectListItem { Text = "Seleccione CC", Value = "" });
            IList<CentroCosto> ListaCC = db.CentrosCostos.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaCC)		// recorro los elementos de la db
            {
                CC.Add(new SelectListItem { Text = item.Nombre, Value = item.Codigo.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.centroCostosId = CC;

            var cuentasLista = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentasParaGruposActivosFijos();

            //codCuentaGasto
            List<SelectListItem> codCuentaGasto = new List<SelectListItem>();
            codCuentaGasto.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });
            foreach (var item in cuentasLista)
            {
                codCuentaGasto.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.codCuentaGasto = codCuentaGasto;

            //codCuentaActivo
            List<SelectListItem> codCuentaActivo = new List<SelectListItem>();
            codCuentaActivo.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });
            foreach (var item in cuentasLista)
            {
                codCuentaActivo.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.codCuentaActivo = codCuentaActivo;

            //codCuentaDepreciacion
            List<SelectListItem> codCuentaDepreciacion = new List<SelectListItem>();
            codCuentaDepreciacion.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });
            foreach (var item in cuentasLista)
            {
                codCuentaDepreciacion.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.codCuentaDepreciacion = codCuentaDepreciacion;

            //codCuentaGastoDepreciacion
            List<SelectListItem> codCuentaGastoDepreciacion = new List<SelectListItem>();
            codCuentaGastoDepreciacion.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });
            foreach (var item in cuentasLista)
            {
                codCuentaGastoDepreciacion.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.codCuentaGastoDepreciacion = codCuentaGastoDepreciacion;

            //terceroMov
            List<SelectListItem> terceroMov = new List<SelectListItem>();   // Creo una lista
            terceroMov.Add(new SelectListItem { Text = "Seleccione Un Tercero", Value = "" });
            IList<Tercero> ListaTerceroMov = db.Terceros.ToList();// extraigo los elementos desde la DB
            foreach (var item in ListaTerceroMov)		// recorro los elementos de la db
            {
                terceroMov.Add(new SelectListItem { Text = item.NOMBRE1 + " " + item.NOMBRE2 + " " + item.APELLIDO1 + " " + item.APELLIDO2 + " || " + item.NIT, Value = item.NIT.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.terceroMov = terceroMov;

            //tipoComprobanteMov
            List<SelectListItem> tipoComprobanteMov = new List<SelectListItem>();   // Creo una lista
            tipoComprobanteMov.Add(new SelectListItem { Text = "Seleccione Un Comprobante", Value = "" });
            IList<TipoComprobante> ListatipoComprobanteMov = db.TiposComprobantes.ToList();
            foreach (var item in ListatipoComprobanteMov)
            {
                tipoComprobanteMov.Add(new SelectListItem { Text = item.CODIGO + " || " + item.NOMBRE, Value = item.CODIGO });
            }
            ViewBag.tipoComprobanteMov = tipoComprobanteMov;

            return View();
        }

        public JsonResult GetConsecutivoActivoFijo()
        {           
            var LastRegister = db.ActivosFijos.Count();
            if(LastRegister == 0)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var Ultimo = db.ActivosFijos.OrderByDescending(x => x.id).First().id;
                return Json(Ultimo, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: ActivosFijos/ActivosFijos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,grupoId,nombreActivo,claseActivoId,descripcion,ubicacionFisicaId,responsableNIT,numeroActivo,numeroSerie,fechaDeCompra,centroCostosId,costoDeCompra,metodoDepreciacion,vecesDepreciarFiscal,vecesDepreciarNIIF,valorSalvamentoFiscal,valorResidualNIIF,valorRazonable,valorLibros,depreciacionAnterior,codCuentaGasto,codCuentaActivo,codCuentaDepreciacion,codCuentaGastoDepreciacion,terceroMov,tipoComprobanteMov")] BActivosFijos bActivosFijos)
        {
            //lista de Grupos
            List<SelectListItem> Grupos = new List<SelectListItem>();   // Creo una lista
            Grupos.Add(new SelectListItem { Text = "Seleccione Un Grupo", Value = "" });
            IList<GruposActivosFijos> ListaDeGrupos = (from l in db.GruposActivosFijos select l).ToList();

            foreach (var item in ListaDeGrupos)		// recorro los elementos de la db
            {

                Grupos.Add(new SelectListItem { Text = item.nombre, Value = item.id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.grupoId = Grupos;

            //lista de Clases
            List<SelectListItem> Clases = new List<SelectListItem>();   // Creo una lista
            Clases.Add(new SelectListItem { Text = "Seleccione Una Clase", Value = "" });
            IList<ClaseDeActivo> ListaDeClases = (from l in db.ClaseDeActivo select l).ToList();

            foreach (var item in ListaDeClases)		// recorro los elementos de la db
            {

                Clases.Add(new SelectListItem { Text = item.nombre, Value = item.id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.claseActivoId = Clases;

            //lista de Ubicaciones
            List<SelectListItem> Ubicaciones = new List<SelectListItem>();   // Creo una lista
            Ubicaciones.Add(new SelectListItem { Text = "Seleccione Una Ubicacion", Value = "" });
            IList<UbicacionFisica> ListaDeUbicaciones = (from l in db.UbicacionFisica select l).ToList();

            foreach (var item in ListaDeUbicaciones)		// recorro los elementos de la db
            {

                Ubicaciones.Add(new SelectListItem { Text = item.nombre, Value = item.id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.ubicacionFisicaId = Ubicaciones;

            //lista de terceros(responsable)
            List<SelectListItem> Terceros = new List<SelectListItem>();   // Creo una lista
            Terceros.Add(new SelectListItem { Text = "Seleccione Un Tercero", Value = "" });
            IList<Tercero> ListaDeTerceros = db.Terceros.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeTerceros)		// recorro los elementos de la db
            {
                Terceros.Add(new SelectListItem { Text = item.NOMBRE1 + " " + item.NOMBRE2 + " " + item.APELLIDO1 + " " + item.APELLIDO2 + " || " + item.NIT, Value = item.NIT.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }

            ViewBag.responsableNIT = Terceros;

            //lista de centro de costos
            List<SelectListItem> CC = new List<SelectListItem>();   // Creo una lista
            CC.Add(new SelectListItem { Text = "Seleccione CC", Value = "" });
            IList<CentroCosto> ListaCC = db.CentrosCostos.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaCC)		// recorro los elementos de la db
            {
                CC.Add(new SelectListItem { Text = item.Nombre, Value = item.Codigo.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.centroCostosId = CC;

            var cuentasLista = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentasParaGruposActivosFijos();

            //codCuentaGasto
            List<SelectListItem> codCuentaGasto = new List<SelectListItem>();
            codCuentaGasto.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });
            foreach (var item in cuentasLista)
            {
                codCuentaGasto.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.codCuentaGasto = codCuentaGasto;

            //codCuentaActivo
            List<SelectListItem> codCuentaActivo = new List<SelectListItem>();
            codCuentaActivo.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });
            foreach (var item in cuentasLista)
            {
                codCuentaActivo.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.codCuentaActivo = codCuentaActivo;

            //codCuentaDepreciacion
            List<SelectListItem> codCuentaDepreciacion = new List<SelectListItem>();
            codCuentaDepreciacion.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });
            foreach (var item in cuentasLista)
            {
                codCuentaDepreciacion.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.codCuentaDepreciacion = codCuentaDepreciacion;

            //codCuentaGastoDepreciacion
            List<SelectListItem> codCuentaGastoDepreciacion = new List<SelectListItem>();
            codCuentaGastoDepreciacion.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });
            foreach (var item in cuentasLista)
            {
                codCuentaGastoDepreciacion.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.codCuentaGastoDepreciacion = codCuentaGastoDepreciacion;

            //terceroMov
            List<SelectListItem> terceroMov = new List<SelectListItem>();   // Creo una lista
            terceroMov.Add(new SelectListItem { Text = "Seleccione Un Tercero", Value = "" });
            IList<Tercero> ListaTerceroMov = db.Terceros.ToList();// extraigo los elementos desde la DB
            foreach (var item in ListaTerceroMov)		// recorro los elementos de la db
            {
                terceroMov.Add(new SelectListItem { Text = item.NOMBRE1 + " " + item.NOMBRE2 + " " + item.APELLIDO1 + " " + item.APELLIDO2 + " || " + item.NIT, Value = item.NIT.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.terceroMov = terceroMov;

            //tipoComprobanteMov
            List<SelectListItem> tipoComprobanteMov = new List<SelectListItem>();   // Creo una lista
            tipoComprobanteMov.Add(new SelectListItem { Text = "Seleccione Un Comprobante", Value = "" });
            IList<TipoComprobante> ListatipoComprobanteMov = db.TiposComprobantes.ToList();
            foreach (var item in ListatipoComprobanteMov)
            {
                tipoComprobanteMov.Add(new SelectListItem { Text = item.CODIGO + " || " + item.NOMBRE, Value = item.CODIGO });
            }
            ViewBag.tipoComprobanteMov = tipoComprobanteMov;        

            if (ModelState.IsValid)
            {
                decimal valorTotalDepreciacion = 0;
                //CALCULO DIAS A DEPRECIAR
                int diasAntesMes = DateTime.Now.Day;
                var fechaFinDepreciacion = bActivosFijos.fechaDeCompra.AddMonths(bActivosFijos.vecesDepreciarFiscal);
                TimeSpan totalDiasDepreciacion = fechaFinDepreciacion - bActivosFijos.fechaDeCompra;
                var valorDepreciacionPorDia = bActivosFijos.costoDeCompra / totalDiasDepreciacion.Days;
                var fechaCorte = DateTime.Now.AddDays(-diasAntesMes);
                TimeSpan diferenciaDias = DateTime.Now.AddDays(-diasAntesMes) - bActivosFijos.fechaDeCompra;
                if (diferenciaDias.Days > 0)
                {
                    valorTotalDepreciacion = valorDepreciacionPorDia * diferenciaDias.Days;
                }
                //FIN CALCULO DIAS A DEPRECIAR

                if ((bActivosFijos.codCuentaActivo == bActivosFijos.codCuentaGasto) || ((bActivosFijos.codCuentaActivo == bActivosFijos.codCuentaDepreciacion)) || ((bActivosFijos.codCuentaGasto == bActivosFijos.codCuentaDepreciacion)))
                {
                    ModelState.AddModelError("", "Conflicto De Cuentas!");
                    return View(bActivosFijos);
                }

                bActivosFijos.valorLibros = bActivosFijos.costoDeCompra - bActivosFijos.depreciacionAnterior;
                db.ActivosFijos.Add(bActivosFijos);
                
                var Comprobante = db.TiposComprobantes.FirstOrDefault(x => x.CODIGO == bActivosFijos.tipoComprobanteMov & x.INACTIVO == false);

                var comprobanteNew = new Comprobante()
                {
                    TIPO = bActivosFijos.tipoComprobanteMov,
                    NUMERO = Comprobante.CONSECUTIVO,
                    ANO = Convert.ToString(bActivosFijos.fechaDeCompra.Year),
                    MES = Convert.ToString(bActivosFijos.fechaDeCompra.Month),
                    DIA = Convert.ToString(bActivosFijos.fechaDeCompra.Day),
                    CCOSTO = bActivosFijos.centroCostosId.ToString(),
                    DETALLE = "COMPRA ACTIVO",
                    TERCERO = bActivosFijos.terceroMov,
                    CTAFPAGO = bActivosFijos.codCuentaGasto,
                    VRTOTAL = bActivosFijos.costoDeCompra,
                    SUMDBCR = 0,
                    FECHARealiz = DateTime.Now,
                    ANULADO = false
                };
                db.Comprobantes.Add(comprobanteNew);


                List<Movimiento> listaDeMovimientos = new List<Movimiento>();
                var mov1 = new Movimiento()
                {
                    TIPO = bActivosFijos.tipoComprobanteMov,
                    NUMERO = Comprobante.CONSECUTIVO,
                    CUENTA = bActivosFijos.codCuentaGasto,
                    TERCERO = bActivosFijos.terceroMov,
                    DETALLE = "COMPRA ACTIVO",
                    DEBITO = 0,
                    CREDITO = bActivosFijos.costoDeCompra,
                    BASE = 0,
                    CCOSTO = bActivosFijos.centroCostosId.ToString(),
                    FECHAMOVIMIENTO = bActivosFijos.fechaDeCompra
                };
                listaDeMovimientos.Add(mov1);

                var mov2 = new Movimiento()
                {
                    TIPO = bActivosFijos.tipoComprobanteMov,
                    NUMERO = Comprobante.CONSECUTIVO,
                    CUENTA = bActivosFijos.codCuentaActivo,
                    TERCERO = bActivosFijos.terceroMov,
                    DETALLE = "COMPRA ACTIVO",
                    DEBITO = bActivosFijos.costoDeCompra,
                    CREDITO = 0,
                    BASE = 0,
                    CCOSTO = bActivosFijos.centroCostosId.ToString(),
                    FECHAMOVIMIENTO = bActivosFijos.fechaDeCompra
                };
                listaDeMovimientos.Add(mov2);

                var result = false;

                var comprobanteConst = new ComprobanteBO();
                result = comprobanteConst.AsentarMovimiento(listaDeMovimientos, Convert.ToInt32(Comprobante.CONSECUTIVO), bActivosFijos.tipoComprobanteMov);

                if (result)
                {
                    db.SaveChanges();

                    var historialActivo = new HistorialActivosFijos()
                    {
                        idActivo = bActivosFijos.id,
                        fecha = DateTime.Now,
                        concepto = "COMPRA ACTIVO",
                        valorEnLibros = bActivosFijos.costoDeCompra - bActivosFijos.depreciacionAnterior,
                        valorMovimiento = bActivosFijos.costoDeCompra,
                        tipoComprobante = bActivosFijos.tipoComprobanteMov,
                        numeroComprobante = Comprobante.CONSECUTIVO,
                        tipoMovimiento = 1
                    };
                    db.HistorialActivosFijos.Add(historialActivo);

                    if (valorTotalDepreciacion > 0)
                    {
                        string ComprobanteDepreciacion = "";
                        var traerConsecutivo = new ComprobanteBO();
                        ComprobanteDepreciacion = traerConsecutivo.proximoConsecutivo(bActivosFijos.tipoComprobanteMov);

                        var comprobanteNewDepreciacion = new Comprobante()
                        {
                            TIPO = bActivosFijos.tipoComprobanteMov,
                            NUMERO = ComprobanteDepreciacion,
                            ANO = Convert.ToString(fechaCorte.Year),
                            MES = Convert.ToString(fechaCorte.Month),
                            DIA = Convert.ToString(fechaCorte.Day),
                            CCOSTO = bActivosFijos.centroCostosId.ToString(),
                            DETALLE = "DEPRECIACION",
                            TERCERO = bActivosFijos.terceroMov,
                            CTAFPAGO = bActivosFijos.codCuentaDepreciacion,
                            VRTOTAL = valorTotalDepreciacion,
                            SUMDBCR = 0,
                            FECHARealiz = DateTime.Now,
                            ANULADO = false
                        };
                        db.Comprobantes.Add(comprobanteNewDepreciacion);


                        List<Movimiento> listaDeMovimientosDepreciacion = new List<Movimiento>();
                        var mov1Depreciacion = new Movimiento()
                        {
                            TIPO = bActivosFijos.tipoComprobanteMov,
                            NUMERO = ComprobanteDepreciacion,
                            CUENTA = bActivosFijos.codCuentaDepreciacion,
                            TERCERO = bActivosFijos.terceroMov,
                            DETALLE = "DEPRECIACION",
                            DEBITO = 0,
                            CREDITO = valorTotalDepreciacion,
                            BASE = 0,
                            CCOSTO = bActivosFijos.centroCostosId.ToString(),
                            FECHAMOVIMIENTO = fechaCorte
                        };
                        listaDeMovimientosDepreciacion.Add(mov1Depreciacion);

                        var mov2Depreciacion = new Movimiento()
                        {
                            TIPO = bActivosFijos.tipoComprobanteMov,
                            NUMERO = ComprobanteDepreciacion,
                            CUENTA = bActivosFijos.codCuentaGastoDepreciacion,
                            TERCERO = bActivosFijos.terceroMov,
                            DETALLE = "DEPRECIACION",
                            DEBITO = valorTotalDepreciacion,
                            CREDITO = 0,
                            BASE = 0,
                            CCOSTO = bActivosFijos.centroCostosId.ToString(),
                            FECHAMOVIMIENTO = fechaCorte
                        };
                        listaDeMovimientosDepreciacion.Add(mov2Depreciacion);

                        var resultDepreciacion = false;

                        var comprobanteConstDepreciacion = new ComprobanteBO();
                        resultDepreciacion = comprobanteConstDepreciacion.AsentarMovimiento(listaDeMovimientosDepreciacion, Convert.ToInt32(ComprobanteDepreciacion), bActivosFijos.tipoComprobanteMov);

                        var historialActivoDepreciacion = new HistorialActivosFijos()
                        {
                            idActivo = bActivosFijos.id,
                            fecha = fechaCorte,
                            concepto = "DEPRECIACION",
                            valorEnLibros = bActivosFijos.costoDeCompra - bActivosFijos.depreciacionAnterior - valorTotalDepreciacion,
                            valorMovimiento = valorTotalDepreciacion,
                            tipoComprobante = bActivosFijos.tipoComprobanteMov,
                            numeroComprobante = ComprobanteDepreciacion,
                            tipoMovimiento = 2
                        };
                        db.HistorialActivosFijos.Add(historialActivoDepreciacion);
                        bActivosFijos.valorLibros = bActivosFijos.costoDeCompra - bActivosFijos.depreciacionAnterior - valorTotalDepreciacion;
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }                               
            }
            return View(bActivosFijos);
        }

        [HttpPost]
        public ActionResult getHistorico(int id)
        {
            var listaHistorial = db.HistorialActivosFijos.Where(p => p.idActivo == id).ToList();
            string json = JsonConvert.SerializeObject(listaHistorial);
            return Json(json);
        }

        [HttpPost]
        public ActionResult contabilizarMejora(int id, string cuentaMejora, string comprobanteMejora, string terceroMejora, decimal valorMejora, string conceptoMejora)
        {
            BActivosFijos activosfijos = db.ActivosFijos.Find(id);
            if(activosfijos.codCuentaActivo == cuentaMejora)
            {
                return Json(2, JsonRequestBehavior.AllowGet);
            }
            else
            {
                string consecutivo = "";
                var traerConsecutivo = new ComprobanteBO();
                consecutivo = traerConsecutivo.proximoConsecutivo(comprobanteMejora);

                var comprobanteNewMejora = new Comprobante()
                {
                    TIPO = comprobanteMejora,
                    NUMERO = consecutivo,
                    ANO = Convert.ToString(DateTime.Now.Year),
                    MES = Convert.ToString(DateTime.Now.Month),
                    DIA = Convert.ToString(DateTime.Now.Day),
                    CCOSTO = activosfijos.centroCostosId.ToString(),
                    DETALLE = conceptoMejora,
                    TERCERO = terceroMejora,
                    CTAFPAGO = cuentaMejora,
                    VRTOTAL = valorMejora,
                    SUMDBCR = 0,
                    FECHARealiz = DateTime.Now,
                    ANULADO = false
                };
                db.Comprobantes.Add(comprobanteNewMejora);

                List<Movimiento> listaDeMovimientos = new List<Movimiento>();
                var mov1 = new Movimiento()
                {
                    TIPO = comprobanteMejora,
                    NUMERO = consecutivo,
                    CUENTA = activosfijos.codCuentaActivo,
                    TERCERO = terceroMejora,
                    DETALLE = conceptoMejora,
                    DEBITO = valorMejora,
                    CREDITO = 0,
                    BASE = 0,
                    CCOSTO = activosfijos.centroCostosId.ToString(),
                    FECHAMOVIMIENTO = DateTime.Now
                };
                listaDeMovimientos.Add(mov1);

                var mov2 = new Movimiento()
                {
                    TIPO = comprobanteMejora,
                    NUMERO = consecutivo,
                    CUENTA = cuentaMejora,
                    TERCERO = terceroMejora,
                    DETALLE = conceptoMejora,
                    DEBITO = 0,
                    CREDITO = valorMejora,
                    BASE = 0,
                    CCOSTO = activosfijos.centroCostosId.ToString(),
                    FECHAMOVIMIENTO = DateTime.Now
                };
                listaDeMovimientos.Add(mov2);

                var result = false;

                var comprobanteConst = new ComprobanteBO();
                result = comprobanteConst.AsentarMovimiento(listaDeMovimientos, Convert.ToInt32(consecutivo), comprobanteMejora);

                if(result)
                {
                    var historialActivo = new HistorialActivosFijos()
                    {
                        idActivo = id,
                        fecha = DateTime.Now,
                        concepto = "MEJORA",
                        valorEnLibros = activosfijos.valorLibros + valorMejora,
                        valorMovimiento = valorMejora,
                        tipoComprobante = comprobanteMejora,
                        numeroComprobante = consecutivo,
                        tipoMovimiento = 3
                    };
                    db.HistorialActivosFijos.Add(historialActivo);
                    activosfijos.valorLibros = activosfijos.valorLibros + valorMejora;

                    db.SaveChanges();

                    return Json(1, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(3, JsonRequestBehavior.AllowGet);
        }

        // GET: ActivosFijos/ActivosFijos/Edit/5
        public ActionResult Edit(int? id)
        {
            //terceroMejora
            List<SelectListItem> terceroMejora = new List<SelectListItem>();   // Creo una lista
            terceroMejora.Add(new SelectListItem { Text = "Seleccione Un Tercero", Value = "" });
            IList<Tercero> ListaterceroMejora = db.Terceros.ToList();// extraigo los elementos desde la DB
            foreach (var item in ListaterceroMejora)		// recorro los elementos de la db
            {
                terceroMejora.Add(new SelectListItem { Text = item.NOMBRE1 + " " + item.NOMBRE2 + " " + item.APELLIDO1 + " " + item.APELLIDO2 + " || " + item.NIT, Value = item.NIT.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.terceroMejora = terceroMejora;

            //comprobanteMejora
            List<SelectListItem> comprobanteMejora = new List<SelectListItem>();   // Creo una lista
            comprobanteMejora.Add(new SelectListItem { Text = "Seleccione Un Comprobante", Value = "" });
            IList<TipoComprobante> ListacomprobanteMejora = db.TiposComprobantes.ToList();
            foreach (var item in ListacomprobanteMejora)
            {
                comprobanteMejora.Add(new SelectListItem { Text = item.CODIGO + " || " + item.NOMBRE, Value = item.CODIGO });
            }
            ViewBag.comprobanteMejora = comprobanteMejora;

            //cuentaMejora
            var cuentasLista = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentasParaGruposActivosFijos();
            List<SelectListItem> cuentaMejora = new List<SelectListItem>();
            cuentaMejora.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });
            foreach (var item in cuentasLista)
            {
                cuentaMejora.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.cuentaMejora = cuentaMejora;

            BActivosFijos activosfijos = db.ActivosFijos.Find(id);

            //ViewBag.grupoId = new SelectList(db.GruposActivosFijos, "id", "nombre", activosfijos.grupoId);
            //ViewBag.claseActivoId = new SelectList(db.ClaseDeActivo, "id", "nombre", activosfijos.claseActivoId);
            //ViewBag.ubicacionFisicaId = new SelectList(db.UbicacionFisica, "id", "nombre", activosfijos.ubicacionFisicaId);
            //ViewBag.responsableNIT = new SelectList(db.Terceros, "NIT", "NOMBRE1" + " " + "NOMBRE2" + " " + "APELLIDO1" + " " + "APELLIDO2" + " || " + "NIT", activosfijos.responsableNIT);
            //ViewBag.centroCostosId = new SelectList(db.CentrosCostos, "Codigo", "Nombre", activosfijos.centroCostosId);

            //ViewBag.codCuentaGasto = new SelectList(db.PlanCuentas, "CODIGO", "CODIGO" + " || " + "NOMBRE", activosfijos.codCuentaGasto);     
            //ViewBag.codCuentaActivo = new SelectList(db.PlanCuentas, "CODIGO", "CODIGO" + " || " + "NOMBRE", activosfijos.codCuentaActivo);
            //ViewBag.codCuentaDepreciacion = new SelectList(db.PlanCuentas, "CODIGO", "CODIGO" + " || " + "NOMBRE", activosfijos.codCuentaDepreciacion);
            //ViewBag.terceroMov = new SelectList(db.Terceros, "NIT", "NOMBRE1" + " " + "NOMBRE2" + " " + "APELLIDO1" + " " + "APELLIDO2" + " || " + "NIT", activosfijos.terceroMov);
            //ViewBag.tipoComprobanteMov = new SelectList(db.TiposComprobantes, "CODIGO", "CODIGO" + " || " + "NOMBRE", activosfijos.tipoComprobanteMov);     
            //lista de Grupos
            List<SelectListItem> Grupos = new List<SelectListItem>();   // Creo una lista
            Grupos.Add(new SelectListItem { Text = activosfijos.GruposActivosFijos.nombre, Value = activosfijos.grupoId.ToString() });
            IList<GruposActivosFijos> ListaDeGrupos = (from l in db.GruposActivosFijos select l).ToList();

            foreach (var item in ListaDeGrupos)		// recorro los elementos de la db
            {
                Grupos.Add(new SelectListItem { Text = item.nombre, Value = item.id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.grupoId = Grupos;

            //lista de Clases
            List<SelectListItem> Clases = new List<SelectListItem>();   // Creo una lista
            Clases.Add(new SelectListItem { Text = activosfijos.ClaseDeActivo.nombre, Value = activosfijos.claseActivoId.ToString() });
            IList<ClaseDeActivo> ListaDeClases = (from l in db.ClaseDeActivo select l).ToList();

            foreach (var item in ListaDeClases)		// recorro los elementos de la db
            {

                Clases.Add(new SelectListItem { Text = item.nombre, Value = item.id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.claseActivoId = Clases;

            //lista de Ubicaciones
            List<SelectListItem> Ubicaciones = new List<SelectListItem>();   // Creo una lista
            Ubicaciones.Add(new SelectListItem { Text = activosfijos.UbicacionFisica.nombre, Value = activosfijos.ubicacionFisicaId.ToString() });
            IList<UbicacionFisica> ListaDeUbicaciones = (from l in db.UbicacionFisica select l).ToList();

            foreach (var item in ListaDeUbicaciones)		// recorro los elementos de la db
            {

                Ubicaciones.Add(new SelectListItem { Text = item.nombre, Value = item.id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.ubicacionFisicaId = Ubicaciones;

            //lista de terceros(responsable)
            List<SelectListItem> Terceros = new List<SelectListItem>();   // Creo una lista
            Terceros.Add(new SelectListItem { Text = activosfijos.Terceros1.NOMBRE + " " + activosfijos.Terceros1.NOMBRE2 + " " + activosfijos.Terceros1.APELLIDO1 + " " + activosfijos.Terceros1.APELLIDO2 + " || " + activosfijos.Terceros1.NIT, Value = activosfijos.responsableNIT });
            IList<Tercero> ListaDeTerceros = db.Terceros.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeTerceros)		// recorro los elementos de la db
            {
                Terceros.Add(new SelectListItem { Text = item.NOMBRE1 + " " + item.NOMBRE2 + " " + item.APELLIDO1 + " " + item.APELLIDO2 + " || " + item.NIT, Value = item.NIT.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }

            ViewBag.responsableNIT = Terceros;

            //lista de centro de costos
            List<SelectListItem> CC = new List<SelectListItem>();   // Creo una lista
            CC.Add(new SelectListItem { Text = activosfijos.CentroCosto.Nombre, Value = activosfijos.centroCostosId });
            IList<CentroCosto> ListaCC = db.CentrosCostos.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaCC)		// recorro los elementos de la db
            {
                CC.Add(new SelectListItem { Text = item.Nombre, Value = item.Codigo.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.centroCostosId = CC;

            //codCuentaGasto
            List<SelectListItem> codCuentaGasto = new List<SelectListItem>();
            codCuentaGasto.Add(new SelectListItem { Text = activosfijos.CuentaMayor1.NOMBRE + " || " + activosfijos.CuentaMayor1.CODIGO, Value = activosfijos.codCuentaGasto });
            foreach (var item in cuentasLista)
            {
                codCuentaGasto.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.codCuentaGasto = codCuentaGasto;

            //codCuentaActivo
            List<SelectListItem> codCuentaActivo = new List<SelectListItem>();
            codCuentaActivo.Add(new SelectListItem { Text = activosfijos.CuentaMayor2.NOMBRE + " || " + activosfijos.CuentaMayor2.CODIGO, Value = activosfijos.codCuentaActivo });
            foreach (var item in cuentasLista)
            {
                codCuentaActivo.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.codCuentaActivo = codCuentaActivo;

            //codCuentaDepreciacion
            List<SelectListItem> codCuentaDepreciacion = new List<SelectListItem>();
            codCuentaDepreciacion.Add(new SelectListItem { Text = activosfijos.CuentaMayor3.NOMBRE + " || " + activosfijos.CuentaMayor3.CODIGO, Value = activosfijos.codCuentaDepreciacion });
            foreach (var item in cuentasLista)
            {
                codCuentaDepreciacion.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.codCuentaDepreciacion = codCuentaDepreciacion;

            //codCuentaGastoDepreciacion
            List<SelectListItem> codCuentaGastoDepreciacion = new List<SelectListItem>();
            codCuentaGastoDepreciacion.Add(new SelectListItem { Text = activosfijos.CuentaMayor4.NOMBRE + " || " + activosfijos.CuentaMayor4.CODIGO, Value = activosfijos.codCuentaGastoDepreciacion });
            foreach (var item in cuentasLista)
            {
                codCuentaGastoDepreciacion.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.codCuentaGastoDepreciacion = codCuentaGastoDepreciacion;

            //terceroMov
            List<SelectListItem> terceroMov = new List<SelectListItem>();   // Creo una lista
            terceroMov.Add(new SelectListItem { Text = activosfijos.Terceros2.NOMBRE + " " + activosfijos.Terceros2.NOMBRE2 + " " + activosfijos.Terceros2.APELLIDO1 + " " + activosfijos.Terceros2.APELLIDO2 + " || " + activosfijos.Terceros2.NIT, Value = activosfijos.terceroMov });
            IList<Tercero> ListaTerceroMov = db.Terceros.ToList();// extraigo los elementos desde la DB
            foreach (var item in ListaTerceroMov)		// recorro los elementos de la db
            {
                terceroMov.Add(new SelectListItem { Text = item.NOMBRE1 + " " + item.NOMBRE2 + " " + item.APELLIDO1 + " " + item.APELLIDO2 + " || " + item.NIT, Value = item.NIT.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.terceroMov = terceroMov;

            //tipoComprobanteMov
            List<SelectListItem> tipoComprobanteMov = new List<SelectListItem>();   // Creo una lista
            tipoComprobanteMov.Add(new SelectListItem { Text = activosfijos.TipoComprobante.CODIGO + " || " + activosfijos.TipoComprobante.NOMBRE, Value = activosfijos.tipoComprobanteMov });
            IList<TipoComprobante> ListatipoComprobanteMov = db.TiposComprobantes.ToList();
            foreach (var item in ListatipoComprobanteMov)
            {
                tipoComprobanteMov.Add(new SelectListItem { Text = item.CODIGO + " || " + item.NOMBRE, Value = item.CODIGO });
            }
            ViewBag.tipoComprobanteMov = tipoComprobanteMov;


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BActivosFijos bActivosFijos = db.ActivosFijos.Find(id);
            if (bActivosFijos == null)
            {
                return HttpNotFound();
            }
            return View(bActivosFijos);
        }

        // POST: ActivosFijos/ActivosFijos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,grupoId,nombreActivo,claseActivoId,descripcion,ubicacionFisicaId,responsableNIT,numeroActivo,numeroSerie,fechaDeCompra,centroCostosId,costoDeCompra,metodoDepreciacion,vecesDepreciarFiscal,vecesDepreciarNIIF,valorSalvamentoFiscal,valorResidualNIIF,valorRazonable,valorLibros,depreciacionAnterior,codCuentaGasto,codCuentaActivo,codCuentaDepreciacion,codCuentaGastoDepreciacion,terceroMov,tipoComprobanteMov")] BActivosFijos bActivosFijos)
        {
            //terceroMejora
            List<SelectListItem> terceroMejora = new List<SelectListItem>();   // Creo una lista
            terceroMejora.Add(new SelectListItem { Text = "Seleccione Un Tercero", Value = "" });
            IList<Tercero> ListaterceroMejora = db.Terceros.ToList();// extraigo los elementos desde la DB
            foreach (var item in ListaterceroMejora)		// recorro los elementos de la db
            {
                terceroMejora.Add(new SelectListItem { Text = item.NOMBRE1 + " " + item.NOMBRE2 + " " + item.APELLIDO1 + " " + item.APELLIDO2 + " || " + item.NIT, Value = item.NIT.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.terceroMejora = terceroMejora;

            //comprobanteMejora
            List<SelectListItem> comprobanteMejora = new List<SelectListItem>();   // Creo una lista
            comprobanteMejora.Add(new SelectListItem { Text = "Seleccione Un Comprobante", Value = "" });
            IList<TipoComprobante> ListacomprobanteMejora = db.TiposComprobantes.ToList();
            foreach (var item in ListacomprobanteMejora)
            {
                comprobanteMejora.Add(new SelectListItem { Text = item.CODIGO + " || " + item.NOMBRE, Value = item.CODIGO });
            }
            ViewBag.comprobanteMejora = comprobanteMejora;

            //cuentaMejora
            var cuentasLista = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentasParaGruposActivosFijos();
            List<SelectListItem> cuentaMejora = new List<SelectListItem>();
            cuentaMejora.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });
            foreach (var item in cuentasLista)
            {
                cuentaMejora.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.cuentaMejora = cuentaMejora;

            //lista de Grupos
            List<SelectListItem> Grupos = new List<SelectListItem>();   // Creo una lista
            Grupos.Add(new SelectListItem { Text = "Seleccione Un Grupo", Value = "" });
            IList<GruposActivosFijos> ListaDeGrupos = (from l in db.GruposActivosFijos select l).ToList();

            foreach (var item in ListaDeGrupos)		// recorro los elementos de la db
            {
                Grupos.Add(new SelectListItem { Text = item.nombre, Value = item.id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.grupoId = Grupos;

            //lista de Clases
            List<SelectListItem> Clases = new List<SelectListItem>();   // Creo una lista
            Clases.Add(new SelectListItem { Text = "Seleccione Una Clase", Value = "" });
            IList<ClaseDeActivo> ListaDeClases = (from l in db.ClaseDeActivo select l).ToList();

            foreach (var item in ListaDeClases)		// recorro los elementos de la db
            {

                Clases.Add(new SelectListItem { Text = item.nombre, Value = item.id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.claseActivoId = Clases;

            //lista de Ubicaciones
            List<SelectListItem> Ubicaciones = new List<SelectListItem>();   // Creo una lista
            Ubicaciones.Add(new SelectListItem { Text = "Seleccione Una Ubicacion", Value = "" });
            IList<UbicacionFisica> ListaDeUbicaciones = (from l in db.UbicacionFisica select l).ToList();

            foreach (var item in ListaDeUbicaciones)		// recorro los elementos de la db
            {

                Ubicaciones.Add(new SelectListItem { Text = item.nombre, Value = item.id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.ubicacionFisicaId = Ubicaciones;

            //lista de terceros(responsable)
            List<SelectListItem> Terceros = new List<SelectListItem>();   // Creo una lista
            Terceros.Add(new SelectListItem { Text = "Seleccione Un Tercero", Value = "" });
            IList<Tercero> ListaDeTerceros = db.Terceros.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeTerceros)		// recorro los elementos de la db
            {
                Terceros.Add(new SelectListItem { Text = item.NOMBRE1 + " " + item.NOMBRE2 + " " + item.APELLIDO1 + " " + item.APELLIDO2 + " || " + item.NIT, Value = item.NIT.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }

            ViewBag.responsableNIT = Terceros;

            //lista de centro de costos
            List<SelectListItem> CC = new List<SelectListItem>();   // Creo una lista
            CC.Add(new SelectListItem { Text = "Seleccione CC", Value = "" });
            IList<CentroCosto> ListaCC = db.CentrosCostos.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaCC)		// recorro los elementos de la db
            {
                CC.Add(new SelectListItem { Text = item.Nombre, Value = item.Codigo.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.centroCostosId = CC;

            //codCuentaGasto
            List<SelectListItem> codCuentaGasto = new List<SelectListItem>();
            codCuentaGasto.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });
            foreach (var item in cuentasLista)
            {
                codCuentaGasto.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.codCuentaGasto = codCuentaGasto;

            //codCuentaActivo
            List<SelectListItem> codCuentaActivo = new List<SelectListItem>();
            codCuentaGasto.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });
            foreach (var item in cuentasLista)
            {
                codCuentaActivo.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.codCuentaActivo = codCuentaActivo;

            //codCuentaDepreciacion
            List<SelectListItem> codCuentaDepreciacion = new List<SelectListItem>();
            codCuentaGasto.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });
            foreach (var item in cuentasLista)
            {
                codCuentaDepreciacion.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.codCuentaDepreciacion = codCuentaDepreciacion;

            //codCuentaGastoDepreciacion
            List<SelectListItem> codCuentaGastoDepreciacion = new List<SelectListItem>();
            codCuentaGastoDepreciacion.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });
            foreach (var item in cuentasLista)
            {
                codCuentaGastoDepreciacion.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.codCuentaGastoDepreciacion = codCuentaGastoDepreciacion;

            //terceroMov
            List<SelectListItem> terceroMov = new List<SelectListItem>();   // Creo una lista
            terceroMov.Add(new SelectListItem { Text = "Seleccione Un Tercero", Value = "" });
            IList<Tercero> ListaTerceroMov = db.Terceros.ToList();// extraigo los elementos desde la DB
            foreach (var item in ListaTerceroMov)		// recorro los elementos de la db
            {
                terceroMov.Add(new SelectListItem { Text = item.NOMBRE1 + " " + item.NOMBRE2 + " " + item.APELLIDO1 + " " + item.APELLIDO2 + " || " + item.NIT, Value = item.NIT.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            ViewBag.terceroMov = terceroMov;

            //tipoComprobanteMov
            List<SelectListItem> tipoComprobanteMov = new List<SelectListItem>();   // Creo una lista
            tipoComprobanteMov.Add(new SelectListItem { Text = "Seleccione Un Comprobante", Value = "" });
            IList<TipoComprobante> ListatipoComprobanteMov = db.TiposComprobantes.ToList();
            foreach (var item in ListatipoComprobanteMov)
            {
                tipoComprobanteMov.Add(new SelectListItem { Text = item.CODIGO + " || " + item.NOMBRE, Value = item.CODIGO });
            }
            ViewBag.tipoComprobanteMov = tipoComprobanteMov;

            if (ModelState.IsValid)
            {
                db.Entry(bActivosFijos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bActivosFijos);
        }

        // GET: ActivosFijos/ActivosFijos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BActivosFijos bActivosFijos = db.ActivosFijos.Find(id);
            if (bActivosFijos == null)
            {
                return HttpNotFound();
            }
            return View(bActivosFijos);
        }

        // GET: ActivosFijos/ActivosFijos/eliminar/5
        public void eliminar (int? id)
        {
            if (id != null)
            {
                BActivosFijos bActivosFijos = db.ActivosFijos.Find(id);
                var historiales = db.HistorialActivosFijos.Where(x => x.idActivo == bActivosFijos.numeroActivo).ToList();
                foreach (var item in historiales)
                {
                    var movs = db.Movimientos.Where(x => x.NUMERO == item.numeroComprobante && x.TIPO == item.tipoComprobante).ToList();
                    foreach (var item2 in movs)
                    {
                        db.Movimientos.Remove(item2);
                    }
                    var comprobante = db.Comprobantes.Where(x => x.NUMERO == item.numeroComprobante && x.TIPO == item.tipoComprobante).Single();
                    db.Comprobantes.Remove(comprobante);
                    db.HistorialActivosFijos.Remove(item);
                }               
                db.ActivosFijos.Remove(bActivosFijos);
                db.SaveChanges();
            }
        }

        // POST: ActivosFijos/ActivosFijos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BActivosFijos bActivosFijos = db.ActivosFijos.Find(id);
            db.ActivosFijos.Remove(bActivosFijos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult _VerDepreciacion(int id)
        {
            ViewBag.Mostrar = "Mostrar con ViewBag";
            return PartialView("_VerDepreciacion");
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
