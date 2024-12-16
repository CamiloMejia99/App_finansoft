using FNTC.Finansoft.Accounting.DAL.Tools;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.EstadoDeCuenta;
using FNTC.Finansoft.Accounting.DTO.Fichas;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using FNTC.Finansoft.BLL.Aportes;
using FNTC.Finansoft.UI.Areas.Creditos.Controllers;
using FNTC.Finansoft.UI.Areas.Terceros.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.EstadoCuenta.Controllers
{
    [Authorize]
    public class EstadoDeCuentaController : Controller
    {
        private AccountingContext db = new AccountingContext();

        public struct DateTimeSpan
        {
            private readonly int years;
            private readonly int months;
            private readonly int days;
            private readonly int hours;
            private readonly int minutes;
            private readonly int seconds;
            private readonly int milliseconds;

            public DateTimeSpan(int years, int months, int days, int hours, int minutes, int seconds, int milliseconds)
            {
                this.years = years;
                this.months = months;
                this.days = days;
                this.hours = hours;
                this.minutes = minutes;
                this.seconds = seconds;
                this.milliseconds = milliseconds;
            }

            public int Years { get { return years; } }
            public int Months { get { return months; } }
            public int Days { get { return days; } }
            public int Hours { get { return hours; } }
            public int Minutes { get { return minutes; } }
            public int Seconds { get { return seconds; } }
            public int Milliseconds { get { return milliseconds; } }

            enum Phase { Years, Months, Days, Done }

            public static DateTimeSpan CompareDates(DateTime date1, DateTime date2)
            {
                if (date2 < date1)
                {
                    var sub = date1;
                    date1 = date2;
                    date2 = sub;
                }

                DateTime current = date1;
                int years = 0;
                int months = 0;
                int days = 0;

                Phase phase = Phase.Years;
                DateTimeSpan span = new DateTimeSpan();
                int officialDay = current.Day;

                while (phase != Phase.Done)
                {
                    switch (phase)
                    {
                        case Phase.Years:
                            if (current.AddYears(years + 1) > date2)
                            {
                                phase = Phase.Months;
                                current = current.AddYears(years);
                            }
                            else
                            {
                                years++;
                            }
                            break;
                        case Phase.Months:
                            if (current.AddMonths(months + 1) > date2)
                            {
                                phase = Phase.Days;
                                current = current.AddMonths(months);
                                if (current.Day < officialDay && officialDay <= DateTime.DaysInMonth(current.Year, current.Month))
                                    current = current.AddDays(officialDay - current.Day);
                            }
                            else
                            {
                                months++;
                            }
                            break;
                        case Phase.Days:
                            if (current.AddDays(days + 1) > date2)
                            {
                                current = current.AddDays(days);
                                var timespan = date2 - current;
                                span = new DateTimeSpan(years, months, days, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
                                phase = Phase.Done;
                            }
                            else
                            {
                                days++;
                            }
                            break;
                    }
                }

                return span;
            }
        }


        private IEnumerable<SelectListItem> ListaTerceros;

        public void llenarTerceros()
        {
            ListaTerceros = new TercerosController().ConsultarTerceros().ToList().Select(p => new SelectListItem { Text = p.NIT + " || " + p.NombreComercial + " " + p.NOMBRE1 + " " + p.NOMBRE2 + " " + p.APELLIDO1 + " " + p.APELLIDO2, Value = p.NIT, Selected = false }); 
        }

        // GET: EstadoCuenta/EstadoDeCuenta
        public ActionResult Index()
        {
            llenarTerceros();
            ViewBag.Terceros = ListaTerceros;
            return View();
        }

        public ActionResult Index2()
        {
            //lista de terceros(responsable)
            List<SelectListItem> Terceros = new List<SelectListItem>();   // Creo una lista
            Terceros.Add(new SelectListItem { Text = "Seleccione Un Asociado", Value = "" });
            IList<FichasAportes> ListaDeTerceros = db.FichasAportes.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeTerceros)		// recorro los elementos de la db
            {
                var tercero = (from pc in db.Terceros where pc.NIT == item.idPersona select pc).Single();
                Terceros.Add(new SelectListItem { Text = item.idPersona + " || " + tercero.NOMBRE1 + " " + tercero.NOMBRE2 + " " + tercero.APELLIDO1 + " " + tercero.APELLIDO2, Value = item.idPersona });  // agrego los elementos de la db a la primera lista que cree
            }

            ViewBag.Terceros = Terceros;
            return View();
        }

        public ActionResult _ver(string NIT)
        {
            return PartialView();
        }

        public JsonResult GetDatosAportes(string NIT, string cuenta,int? IdProducto)
        {
            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;

            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            string nombreProducto = "";
            //consultamos el producto sea ahorro o aporte
            if (IdProducto != null) { 
                nombreProducto = db.TiposProducto.Where(x => x.IdProducto == IdProducto).Select(x=>x.Nombre).FirstOrDefault().ToUpper();
            }

            IList<FactOpcaja> ListaDeFacturas = db.FactOpcaja.Where(x => x.nit_propietario_cuenta == NIT && x.numero_cuenta == cuenta).ToList();
            List<Array> codigos = new List<Array>();
            foreach (var item in ListaDeFacturas)
            {
                string[] nombres = new string[4];
                nombres[0] = item.fecha.ToString();
                nombres[1] = item.operacion;
                nombres[2] = item.valor_efectivo.ToString();
                nombres[3] = "<a class='comprobante' data-tipo='"+item.TIPO.ToString()+"' data-numero='"+item.NUMERO.ToString()+"'  style='cursor:pointer'>"+item.TIPO.ToString()+"-"+item.NUMERO.ToString()+"</a>";

                codigos.Add(nombres);
            }

            decimal consig = ListaDeFacturas.Where(x => x.operacion == "Consignación" || x.operacion == "Consignacion Movimiento").Select(x => x.valor_efectivo).Sum();
            decimal ret = ListaDeFacturas.Where(x => x.operacion == "Retiro Efectivo" || x.operacion == "Retiro Movimiento").Select(x => x.valor_efectivo).Sum();
            decimal tot = consig - ret;

            return new JsonResult { Data = new { status = true,nombreProducto, codigos, consignaciones = consig.ToString("N0", formato), retiros = ret.ToString("N0", formato), total = tot.ToString("N0", formato) } };
        }

        #region AHORRO PERMANENTE
        public async Task<JsonResult> GetDatosAhorroPermanente(string NIT)//lista las fichas de ahorro permanente en el estado de cuenta
        {
            var bandera = false;
            var list = new List<ViewModelAhorroPermanenteEstadoCuenta>();
            try
            {
                var fichasAP = await db.FichasAhorros.Where(x => x.idPersona == NIT).ToListAsync();
                foreach (var item in fichasAP)
                {
                    var model = new ViewModelAhorroPermanenteEstadoCuenta()
                    {
                        Cuenta = item.numeroCuenta,
                        FechaApertura = item.fechaApertura != null ? Convert.ToDateTime(item.fechaApertura).ToString("dd-MM-yyyy") : "--//--//--",
                        SaldoActual = GetFormatNumberMiles(Convert.ToDecimal(item.totalAhorros), 0),
                        Rendimiento = "0",
                        Estado = (item.activo==true) ? "Activo" : "Inactivo"
                    };
                    bandera = true;
                    list.Add(model);
                }
                
            }
            catch (Exception ex)
            {
            }
            return new JsonResult { Data = new { status = bandera,list } };
        }

        #endregion

        #region AHORRO CONTRACTUAL
        public async Task<JsonResult> GetDatosAhorroContractual(string NIT)//lista las fichas de ahorro contractual en el estado de cuenta
        {
            var bandera = false;
            var list = new List<ViewModelAhorrosContractualEstadoCuenta>();
            try
            {
                var fichasAC = await db.FichasAhorroContractual.Where(x => x.IdAsociado == NIT).ToListAsync();
                foreach (var item in fichasAC)
                {
                    var model = new ViewModelAhorrosContractualEstadoCuenta()
                    {
                        Cuenta = item.NumeroCuenta,
                        TipoAhorro = item.ConfACFK.NombreConfiguracion.ToUpper(),
                        Plazo = item.Plazo.ToString(),
                        FechaApertura = item.FechaApertura.ToString("dd-MM-yyyy"),
                        FechaVencimiento = item.FechaVencimiento.ToString("dd-MM-yyyy"),
                        TEM = GetFormatNumberMiles(item.TasaEfectiva,2),
                        ValorCuota = GetFormatNumberMiles(item.ValorCuota,0),
                        TotalAhorros = GetFormatNumberMiles(item.TotalAhorro,0),
                        Rendimientos = GetFormatNumberMiles(item.Interes,0),
                        SaldoTotal = GetFormatNumberMiles(item.TotalAhorro+item.Interes,0),
                        Estado = (item.Estado) ? "Activo" : "Inactivo"
                    };
                    bandera = true;
                    list.Add(model);
                }

            }
            catch (Exception ex)
            {
            }
            return new JsonResult { Data = new { status = bandera, list } };
        }
        #endregion

        #region APORTE EXTRAORDINARIO
        public async Task<JsonResult> GetDatosAporteExtra(string NIT)//lista las fichas de aporte extraordinario en el estado de cuenta
        {
            var bandera = false;
            var list = new List<ViewModelAporteExtraEstadoCuenta>();
            try
            {
                var fichasAE = await db.FichaAfiliadosAporteEx.Where(x => x.idPersona == NIT).ToListAsync();
                foreach (var item in fichasAE)
                {
                    var model = new ViewModelAporteExtraEstadoCuenta()
                    {
                        Cuenta = item.NumeroCuenta,
                        FechaApertura = item.FechaApertura.ToString("dd-MM-yyyy"),
                        SaldoActual = GetFormatNumberMiles(item.totalAportesEx, 0),
                        Estado = (item.Estado) ? "Activo" : "Inactivo"
                    };
                    bandera = true;
                    list.Add(model);
                }

            }
            catch (Exception ex)
            {
            }
            return new JsonResult { Data = new { status = bandera, list } };
        }
        #endregion


        public JsonResult GetDatosCuotasPagadas(string pagare)
        {
            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";

            var Pagos = db.factOpCajaConsCuotaCredito.Where(x => x.pagare == pagare).ToList();
            var DataCredito = db.Creditos.Where(x => x.Pagare == pagare).FirstOrDefault();
            string Nit = "", Agencia = "";
            decimal CapitalInicial = 0;

            if(DataCredito!=null)
            {
                Nit = DataCredito.Creditos_Cedula;
                Agencia = (DataCredito.terceroFK != null) ? DataCredito.terceroFK.agenciaFK.nombreagencia : "";
                CapitalInicial = DataCredito.Capital;
            }

            decimal TotalConsignado = 0;

            List<Array> codigos = new List<Array>();
            foreach (var item in Pagos)
            {
                string[] Data = new string[12];

                Data[0] = item.fecha.ToString("dd-MM-yyyy");
                Data[1] = item.fecha.ToString("hh:mm:ss");
                Data[2] = item.factura;
                Data[3] = item.numeroCuota;
                Data[4] = Convert.ToDecimal(item.valorConsignado).ToString("N0",formato);
                Data[5] = Convert.ToDecimal(item.abonoCapital).ToString("N0",formato);
                Data[6] = Convert.ToDecimal(item.interesCorriente).ToString("N0",formato);
                Data[7] = Convert.ToDecimal(item.interesMora).ToString("N0",formato);
                Data[8] = Convert.ToDecimal(item.seguros).ToString("N0",formato);
                Data[9] = Convert.ToDecimal(item.CtoAdmon).ToString("N0",formato);
                Data[10] = Convert.ToDecimal(item.saldoCapital).ToString("N0",formato);
                Data[11] = "<a class='comprobante' data-tipo='" + item.TipoComprobante.ToString() + "' data-numero='" + item.NumeroComprobante.ToString() + "'  style='cursor:pointer'>" + item.TipoComprobante.ToString() + "-" + item.NumeroComprobante.ToString() + "</a>";

                codigos.Add(Data);

                TotalConsignado += Convert.ToDecimal(item.valorConsignado);
            }

            return new JsonResult { Data = new { cod = codigos, sumaConsig = TotalConsignado.ToString("N0", formato), Nit, Agencia, CapitalInicial = CapitalInicial.ToString("N0", formato)  } };
        }

        public JsonResult GetDatosEstadoCuentaCreditos(string NIT)
        {
            var tercero = (from pc in db.Terceros where pc.NIT == NIT select pc).Single();
            var fichaAporte = (from fp in db.FichasAportes where fp.idPersona == NIT select fp).Single();

            var creditoCount = (from fp in db.Creditos where fp.Creditos_Cedula == NIT select fp).Count();

            List<string> codigos = new List<string>();

            if (creditoCount > 0)
            {
                var credito = (from fp in db.Creditos where fp.Creditos_Cedula == NIT select fp).First();
                var prestamo = (from fp in db.Prestamos where fp.id == credito.Prestamo_Id select fp).First();
                var datosAmortizacion = (from pc in db.Amortizaciones where pc.pagare == credito.Pagare select pc).FirstOrDefault();
                var cuota = datosAmortizacion.valorCuota;
                var pagos = (from fp in db.factOpCajaConsCuotaCredito where fp.pagare == credito.Pagare select fp).ToList();
                var saldoCapital = credito.Capital.ToString();
                int valorConsignado = 0;
                if (pagos.Count>0)
                {
                    int valor = 0;
                    foreach(var item in pagos)
                    {
                        valor = 0;
                        valor = Convert.ToInt32(item.valorConsignado);
                        valorConsignado = valorConsignado + valor;
                    }
                    var consulsta = db.factOpCajaConsCuotaCredito.OrderByDescending(a => a.pagare).Where(b => b.pagare == credito.Pagare).First();
                    saldoCapital = consulsta.saldoCapital;
                }

                codigos.Add(prestamo.fechadesembolso.ToString());
                codigos.Add(cuota);
                codigos.Add(credito.Pagare);
                codigos.Add(saldoCapital);
                codigos.Add(prestamo.Plazo.ToString());
            }

           

            return Json(codigos, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetDatosEstadoCuenta(string NIT)
        {
            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";

            var modelTercero = new ViewModelInfoTerceroEstadoCuenta();
            var modelFichasAportes = new List<ViewModelAporteEstadoCuenta>();

            
            #region descomentar cuando realicen aportes
            try
            {
                //agregamos la información del asociado
                var tercero = await db.Terceros.Where(x => x.NIT == NIT).FirstOrDefaultAsync();
                if (tercero != null)
                {
                    modelTercero.Nombre = TercerosController.GetNombreTercero(tercero);
                    modelTercero.Salario = tercero.SALARIO != null ? tercero.SALARIO.ToString() : "0";
                    modelTercero.Antiguedad = TercerosController.GetAntiguedadMeses(tercero).ToString();
                    modelTercero.Agencia = (tercero.agenciaFK != null) ? tercero.agenciaFK.nombreagencia : "";
                }

                //agregamos información de las fichas de aporte
                var fichasAporte = await db.FichasAportes.Where(x => x.idPersona == NIT).ToListAsync();

                foreach (var item in fichasAporte)
                {
                    var saldoMora = await BLLAportes.GetDeudaMoraAporteOrdinarioAsync(item);
                    var modelFicha = new ViewModelAporteEstadoCuenta()
                    {
                        Cuenta = item.numeroCuenta,
                        FechaApertura = Convert.ToDateTime(item.fechaApertura).ToString("dd-MM-yyyy"),
                        Cuota = Convert.ToDecimal(item.valor).ToString("N0", formato),
                        SaldoActual = Convert.ToDecimal(item.totalAportes).ToString("N0", formato),
                        SaldoMora = saldoMora.ToString("N0", formato),
                        Intereses = "0",
                        SaldoCanje = "0",
                        Estado = item.activa == true ? "Activa" : "Inactiva"
                    };
                    modelFichasAportes.Add(modelFicha);
                }
                
            }
            catch (Exception e)
            {
                
            }
            #endregion
            return new JsonResult { Data = new { modelTercero,modelFichasAportes } };

        }


        [HttpPost]
        public JsonResult GetCreditos(string NIT)
        {
            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;

            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";

            decimal totalCapital = 0, totalSaldoMora = 0, totalG = 0,TotalCreditoAux;
            
          
            List<Array> prestamos = new List<Array>();
            
            var Creditos = (from c in db.Creditos
                            join t in db.TotalesCreditos on c.Pagare equals t.Pagare
                            where c.Creditos_Cedula == NIT
                            select new { c,t}
                        ).ToList();

            foreach (var item in Creditos)
            {

                var DatosCreditos = db.ControlCreditos.Where(x => x.Pagare == item.c.Pagare && x.EstadoEnCredito == "EM").ToList();
                decimal SaldoEnMora = (DatosCreditos.Count>0) ? DatosCreditos.Select(x => x.ValorCuota).Sum() : 0;
                var InteresDividido = item.c.prestamoFK.Interes / 100;
                var opera = Math.Pow(Convert.ToDouble((1 + InteresDividido)), Convert.ToDouble(-item.c.Creditos_Plazo));
                var Cuota = (item.c.Capital) * (InteresDividido / (1 - Convert.ToDecimal(opera)));
                int CtoAdmon = new PrestamosController().GetCostoAdministracion((Int32)item.c.prestamoFK.Capital, (Int32)item.c.prestamoFK.Plazo, (Int32)(item.c.prestamoFK.ValorPeriodo / 30));
                Cuota = Math.Round(Cuota+item.c.prestamoFK.ValorSeguro+CtoAdmon, 0, MidpointRounding.ToEven);
                totalSaldoMora += SaldoEnMora;
                TotalCreditoAux = item.t.SaldoCapital + item.t.InteresCorrientePendiente + item.t.InteresMoraPendiente + item.t.SeguroPendiente;
                totalG += TotalCreditoAux;

                string[] prestamo = new string[7];

                prestamo[0] = item.c.Pagare;
                prestamo[1] = item.c.prestamoFK.fechadesembolso.ToString("yyyy-MM-dd");
                prestamo[2] = Cuota.ToString("N0", formato);
                prestamo[3] = item.t.SaldoCapital.ToString("N0", formato);
                prestamo[4] = item.c.prestamoFK.Plazo.ToString("N0", formato);
                prestamo[5] = (SaldoEnMora).ToString("N0", formato);
                prestamo[6] = (TotalCreditoAux).ToString("N0", formato);

                prestamos.Add(prestamo);
            }

            totalCapital = (Creditos.Count > 0) ? Creditos.Select(x => x.t.SaldoCapital).Sum() : 0;

            
            return new JsonResult { Data = new { status = true, prestamoss = prestamos, totalCreditos = totalCapital.ToString("N0", formato), totalSaldoMora = totalSaldoMora.ToString("N0", formato), totalGeneral = totalG.ToString("N0", formato) } };
             

        }

        [HttpPost]
        public JsonResult GetDataAmortizacion(string pagare)
        {

            int num = db.Amortizaciones.Where(a => a.pagare == pagare).Count();
            if(num>0)
            {
                var consulta = db.Amortizaciones.OrderByDescending(b => b.id).Where(a => a.pagare == pagare && a.calendarioDePagos != "FECHA").FirstOrDefault();
                if(consulta != null)
                {
                    var response = new List<object>
                    {
                        new
                        {
                            cuotaa = consulta.valorCuota,
                            saldoCapitall = consulta.saldoCapital


                        }

                    };
                        return Json(response);
                }
                else
                {
                    var CA = db.Amortizaciones.Where(a => a.pagare == pagare).FirstOrDefault();
                    var CP = db.Prestamos.Where(a => a.Pagare == pagare).FirstOrDefault();
                    var response = new List<object>
                    {
                        new
                        {
                            cuotaa = CA.valorCuota,
                            saldoCapitall = CP.Capital


                        }

                    };
                    return Json(response);

                }
                
               
            }

            return Json("NO");
        }



            // GET: EstadoCuenta/EstadoDeCuenta/Details/5
            public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EstadoCuenta/EstadoDeCuenta/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EstadoCuenta/EstadoDeCuenta/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: EstadoCuenta/EstadoDeCuenta/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EstadoCuenta/EstadoDeCuenta/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: EstadoCuenta/EstadoDeCuenta/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EstadoCuenta/EstadoDeCuenta/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private string GetFormatNumberMiles(decimal numero,int parteDecimal)
        {
            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            string valor = "";
            try
            {
                valor = numero.ToString("N" + parteDecimal, formato);
            }
            catch (Exception ex)
            {
            }
            return valor;
        }
    }
}
