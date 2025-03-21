using System.Linq;
using Rotativa;
using System;
using System.Web.Mvc;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Web;
using Newtonsoft.Json;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using System.Globalization;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using FNTC.Finansoft.Accounting.DTO.Contabilidad.Parametros;
using DocumentFormat.OpenXml.Office2010.PowerPoint;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using System.Data.SqlClient;
using FNTC.Finansoft.Accounting.DTO.Informes;

namespace FNTC.Finansoft.UI.Areas.Accounting.Controllers.Movimientos.Informes
{
    public class Informes2Controller : Controller
    {

        public IEnumerable<object> saldo { get; private set; }
        AccountingContext db = new AccountingContext();

        public void setAnuladoMovimientos()
        {
            var movimientos = db.Movimientos.ToList();
            var comprobantes = db.Comprobantes.Where(x => x.ANULADO == true).ToList();
            foreach (var item in comprobantes)
            {
                var mov = movimientos.Where(x => x.TIPO == item.TIPO && x.NUMERO == item.NUMERO).ToList();
                foreach (var item2 in mov)
                {
                    var modify = db.Movimientos.Find(item2.ID);
                    modify.ANULADO = true;
                    db.Entry(modify).State = System.Data.Entity.EntityState.Modified;
                }
            }
            db.SaveChanges();
        }



        public void llenarSaldosCuentas2()
        {
            var movimientos = db.Movimientos.Where(x => x.Comprobante.ANULADO == false).ToList();
            var auxmov = (from mov in movimientos
                          orderby mov.CUENTA
                          select new { mov.FECHAMOVIMIENTO.Year, mov.FECHAMOVIMIENTO.Month, mov.CUENTA }
                          ).Distinct().ToList();

            foreach (var item in auxmov)
            {
                var sc = db.SaldosCuentas.Where(x => x.CODIGO == item.CUENTA && x.ANO == item.Year && x.MES == item.Month).FirstOrDefault();
                decimal saldo = 0;
                var data = movimientos.Where(x => x.CUENTA == item.CUENTA && x.FECHAMOVIMIENTO.Year == item.Year && x.FECHAMOVIMIENTO.Month == item.Month).ToList();
                decimal debito = data.Select(x => x.DEBITO).Sum();
                decimal credito = data.Select(x => x.CREDITO).Sum();
                if (item.CUENTA.StartsWith("1") || item.CUENTA.StartsWith("5") || item.CUENTA.StartsWith("6") || item.CUENTA.StartsWith("7") || item.CUENTA.StartsWith("9"))
                {
                    saldo = debito - credito;
                }
                else
                {
                    saldo = credito - debito;
                }
                if (sc == null)
                {
                    var saldoCuenta = new SaldoCuenta()
                    {
                        CODIGO = item.CUENTA,
                        ANO = item.Year,
                        MES = item.Month,
                        MDEBITO = debito,
                        MCREDITO = credito,
                        SALDO = saldo,
                        TIPO = null

                    };

                    db.SaldosCuentas.Add(saldoCuenta);

                }
                else
                {

                    sc.MDEBITO += debito;
                    sc.MCREDITO += credito;
                    sc.SALDO += saldo;
                    db.Entry(sc).State = System.Data.Entity.EntityState.Modified;
                }//fin if
                db.SaveChanges();
            }
        }

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

        public void llenarSaldosCuentas()
        {
            int j = 1;
            var cuentas = db.Movimientos.Where(x => x.Comprobante.ANULADO == false).ToList();
            foreach (var item in cuentas)
            {
                var sc = db.SaldosCuentas.Where(x => x.CODIGO == item.CUENTA && x.ANO == item.FECHAMOVIMIENTO.Year && x.MES == item.FECHAMOVIMIENTO.Month).FirstOrDefault();
                var st = db.SaldosTerceros.Where(x => x.CODIGO == item.CUENTA && x.TERCERO == item.TERCERO && x.ANO == item.FECHAMOVIMIENTO.Year && x.MES == item.FECHAMOVIMIENTO.Month).FirstOrDefault();
                var sCC = db.SaldosCCs.Where(x => x.CUENTA == item.CUENTA && x.TERCERO == item.TERCERO && x.ANO == item.FECHAMOVIMIENTO.Year && x.MES == item.FECHAMOVIMIENTO.Month).FirstOrDefault();
                decimal saldo = 0;
                if (item.CUENTA.StartsWith("1") || item.CUENTA.StartsWith("5") || item.CUENTA.StartsWith("6") || item.CUENTA.StartsWith("7") || item.CUENTA.StartsWith("9"))
                {
                    saldo = item.DEBITO - item.CREDITO;
                }
                else
                {
                    saldo = item.CREDITO - item.DEBITO;
                }

                if (sc == null)
                {
                    var saldoCuenta = new SaldoCuenta()
                    {
                        CODIGO = item.CUENTA,
                        ANO = item.FECHAMOVIMIENTO.Year,
                        MES = item.FECHAMOVIMIENTO.Month,
                        MDEBITO = item.DEBITO,
                        MCREDITO = item.CREDITO,
                        SALDO = saldo,
                        TIPO = j.ToString()

                    };

                    db.SaldosCuentas.Add(saldoCuenta);

                }
                else
                {

                    sc.MDEBITO += item.DEBITO;
                    sc.MCREDITO += item.CREDITO;
                    sc.SALDO += saldo;
                    sc.TIPO = j.ToString();
                    db.Entry(sc).State = System.Data.Entity.EntityState.Modified;
                }//fin if

                if (st == null)
                {
                    var saldoTercero = new SaldosTercero()
                    {
                        CODIGO = item.CUENTA,
                        TERCERO = item.TERCERO,
                        ANO = item.FECHAMOVIMIENTO.Year,
                        MES = item.FECHAMOVIMIENTO.Month,
                        MDEBITO = item.DEBITO,
                        MCREDITO = item.CREDITO,
                        SALDO = saldo
                    };

                    db.SaldosTerceros.Add(saldoTercero);

                }
                else
                {

                    st.MDEBITO += item.DEBITO;
                    st.MCREDITO += item.CREDITO;
                    st.SALDO += saldo;
                    db.Entry(st).State = System.Data.Entity.EntityState.Modified;
                }//fin if
                if (sCC == null)
                {
                    var saldoCC = new SaldoCC()
                    {
                        CUENTA = item.CUENTA,
                        TERCERO = item.TERCERO,
                        CCOSTO = item.CCOSTO,
                        ANO = item.FECHAMOVIMIENTO.Year,
                        MES = item.FECHAMOVIMIENTO.Month,
                        MDEBITO = item.DEBITO,
                        MCREDITO = item.CREDITO,
                        SALDO = saldo


                    };

                    db.SaldosCCs.Add(saldoCC);

                }
                else
                {

                    sCC.MDEBITO += item.DEBITO;
                    sCC.MCREDITO += item.CREDITO;
                    sCC.SALDO += saldo;
                    db.Entry(sCC).State = System.Data.Entity.EntityState.Modified;
                }//fin if
                j++;

            }//fin foreach
            db.SaveChanges();
        }

        public void SetSeguro()
        {
            var dataHC = db.HistorialCreditos.Where(x => x.estado == "normal" || x.estado == "enMora" && x.valorCosto == 0).ToList();
            var dataPrestamos = db.Prestamos.ToList();

            foreach (var item in dataHC)
            {
                if (item.numeroCuota > 0)
                {
                    var data = dataPrestamos.Where(x => x.Pagare == item.pagare).FirstOrDefault();
                    if (data != null)
                    {
                        var update = db.HistorialCreditos.Find(item.id);
                        update.valorCosto = data.costoAdicionalEnEltiempo;
                        db.Entry(update).State = System.Data.Entity.EntityState.Modified;
                    }
                }

            }
            db.SaveChanges();

        }

        public Informes2Controller()
        {
            //            System.Web.Mvc.RazorViewEngine rve = (RazorViewEngine)ViewEngines.Engines
            //  .Where(e => e.GetType() == typeof(RazorViewEngine))
            //  .FirstOrDefault();

            //            string[] additionalPartialViewLocations = new[] { 
            //  "~/Areas/Accounting/Views/Informes/Imprimir"
            //};

            //            if (rve != null)
            //            {
            //                rve.PartialViewLocationFormats = rve.PartialViewLocationFormats
            //                  .Union(additionalPartialViewLocations)
            //                  .ToArray();
            //            }
        }

        // GET: Accounting/Comprobantes
        public ActionResult Index()
        {

            //inicio select list para terceros
            List<SelectListItem> terceros = new List<SelectListItem>();
            terceros.Add(new SelectListItem { Text = "Documento", Value = "" });
            IList<Tercero> listadoTerceros = db.Terceros.ToList();


            foreach (var item in listadoTerceros)		// recorro los elementos de la db
            {
                terceros.Add(new SelectListItem { Text = item.NIT + " | " + item.NOMBRE, Value = item.NIT });  // agrego los elementos de la db a la primera lista que cree

            }
            //fin select list para terceros

            //selec list para niveles de cuentas
            List<SelectListItem> niveles = new List<SelectListItem>();
            niveles.Add(new SelectListItem { Text = "Nivel", Value = "" });
            niveles.Add(new SelectListItem { Text = "1", Value = "1" });
            niveles.Add(new SelectListItem { Text = "2", Value = "2" });
            niveles.Add(new SelectListItem { Text = "3", Value = "3" });
            niveles.Add(new SelectListItem { Text = "4", Value = "4" });
            niveles.Add(new SelectListItem { Text = "5", Value = "5" });
            //fin list niveles cuentas

            //selec list para tipos de informe 
            List<SelectListItem> tiposInforme = new List<SelectListItem>();
            tiposInforme.Add(new SelectListItem { Text = "Tipo de informe", Value = "" });
            tiposInforme.Add(new SelectListItem { Text = "Discriminado", Value = "1" });
            tiposInforme.Add(new SelectListItem { Text = "Acumulado", Value = "2" });
            //fin list tipos de informe

            //inicio select list para cuentas
            List<SelectListItem> cuentas = new List<SelectListItem>();
            cuentas.Add(new SelectListItem { Text = "Cuenta", Value = "" });
            IList<CuentaMayor> listadoCuentas = db.PlanCuentas.Where(x => x.CODIGO.Length == 9).ToList();


            foreach (var item in listadoCuentas)		// recorro los elementos de la db
            {
                cuentas.Add(new SelectListItem { Text = item.CODIGO + " | " + item.NOMBRE, Value = item.CODIGO });  // agrego los elementos de la db a la primera lista que cree

            }
            //fin select list para cuentas

            //inicio select list para centro de costos
            List<SelectListItem> costos = new List<SelectListItem>();
            costos.Add(new SelectListItem { Text = "Centro de costo", Value = "" });
            IList<CentroCosto> listadoCentroCostos = db.CentrosCostos.ToList();


            foreach (var item in listadoCentroCostos)
            {
                costos.Add(new SelectListItem { Text = item.Nombre, Value = item.Codigo });  // agrego los elementos de la db a la primera lista que cree

            }
            //fin select list para centro de costos

            //inicio select list para agencias
            List<SelectListItem> agencias = new List<SelectListItem>();
            agencias.Add(new SelectListItem { Text = "Agencia", Value = "" });
            IList<agencias> listadoAgencias = db.agencias.ToList();


            foreach (var item in listadoAgencias)
            {
                agencias.Add(new SelectListItem { Text = item.nombreagencia, Value = item.codigoagencia.ToString() });  // agrego los elementos de la db a la primera lista que cree

            }
            //fin select list para agencias

            ViewBag.cuentas = cuentas;
            ViewBag.terceros = terceros;
            ViewBag.agencias = agencias;
            ViewBag.nivel = niveles;
            ViewBag.costos = costos;
            ViewBag.tipoInforme = tiposInforme;

            return View();
        }

        [HttpPost]
        public JsonResult GetCertificadoRetencion(string tercero, DateTime fechaDesde, DateTime fechaHasta)
        {
            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;

            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";

            DateTime fHasta = new DateTime(fechaHasta.Year, fechaHasta.Month, fechaHasta.Day, 23, 59, 59);
            string fecha = "";
            Movimiento dataInfo = new Movimiento();
            string nombre = "";
            string documento = "";
            string anio = "AÑO GRAVABLE " + fechaHasta.Year;
            string direccion = "";

            var data = db.Movimientos.Where(x => x.TERCERO == tercero && (x.FECHAMOVIMIENTO >= fechaDesde && x.FECHAMOVIMIENTO <= fHasta) && x.CUENTA.StartsWith("2445")).ToList();
            if (data.Count > 0)
            {
                fecha = "De: " + GetMesCorto(fechaDesde.Month) + " " + fechaDesde.Day + "/" + fechaDesde.Year + "  A: " + GetMesCorto(fechaHasta.Month) + " " + fechaHasta.Day + "/" + fechaHasta.Year;
                dataInfo = data.FirstOrDefault();
                nombre = dataInfo.terceroFK.NOMBRE1 + " " + dataInfo.terceroFK.NOMBRE2 + " " + dataInfo.terceroFK.APELLIDO1 + " " + dataInfo.terceroFK.APELLIDO2;
                documento = dataInfo.TERCERO;
                direccion = dataInfo.terceroFK.DIR;

                List<Array> model = new List<Array>();
                foreach (var item in data)
                {
                    string[] info = new string[4];
                    info[0] = item.DETALLE;
                    info[1] = item.BASE.ToString("N0", formato);
                    info[2] = item.cuentaFK.Porcentaje.ToString();
                    info[3] = item.CREDITO.ToString("N0", formato);

                    model.Add(info);
                }
                decimal tot = data.Select(x => x.CREDITO).Sum();

                return new JsonResult { Data = new { status = true, datos = model, fecha, nombre, documento, total = tot.ToString("N0", formato), anio, direccion } };
            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }


        }

        public ActionResult CertificadoRetenciones()
        {

            //inicio select list para terceros
            List<SelectListItem> terceros = new List<SelectListItem>();
            terceros.Add(new SelectListItem { Text = "Documento", Value = "" });
            var listadoTerceros = (from ter in db.Movimientos
                                   where ter.CUENTA.StartsWith("2445")
                                   select new { ter.TERCERO, ter.terceroFK }
                                   ).Distinct().ToList();


            foreach (var item in listadoTerceros)		// recorro los elementos de la db
            {
                string nombre = item.terceroFK.NOMBRE1 + " " + item.terceroFK.NOMBRE2 + " " + item.terceroFK.APELLIDO1 + " " + item.terceroFK.APELLIDO2;
                terceros.Add(new SelectListItem { Text = item.TERCERO + " | " + nombre, Value = item.TERCERO });  // agrego los elementos de la db a la primera lista que cree

            }
            //fin select list para terceros


            ViewBag.terceros = terceros;

            return View();
        }

        public ActionResult Image()
        {
            var model = new Formato();
            return new ViewAsPdf(model);
            //return View();

        }

        public ActionResult Test()
        {
            return View();
        }

        public ActionResult Getyears()
        {
            using (var ctx = new AccountingContext())
            {
                var result = ctx.SaldosCuentas.Select(s => s.ANO).Distinct().OrderBy(x => x);
                return Json(new { years = result.ToList() }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult getPeriodos(int anio)
        {
            List<Array> periodos = new List<Array>();

            string[] array1 = new string[2];
            array1[0] = 1.ToString();
            array1[1] = "1 Enero " + anio + " - 31 Marzo " + anio;
            string[] array2 = new string[2];
            array2[0] = 2.ToString();
            array2[1] = "1 Abril " + anio + " - 30 Junio " + anio;
            string[] array3 = new string[2];
            array3[0] = 3.ToString();
            array3[1] = "1 Julio " + anio + " - 30 Septiembre " + anio;
            string[] array4 = new string[2];
            array4[0] = 4.ToString();
            array4[1] = "1 Octubre " + anio + " - 31 Diciembre " + anio;


            periodos.Add(array1);
            periodos.Add(array2);
            periodos.Add(array3);
            periodos.Add(array4);

            return new JsonResult { Data = new { status = true, periodos } };

        }

        public ActionResult GetHSaldos()
        {
            using (var ctx = new AccountingContext())
            {
                var result = ctx.HSaldosCuentas.Select(h => h.ANO).Distinct().OrderBy(x => x);
                return Json(new { years = result.ToList() }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetCuentas(string term)
        {
            using (var ctx = new AccountingContext())
            {
                var result = ctx.PlanCuentas.Where(x => x.CODIGO.Contains(term) || x.NOMBRE.Contains(term));
                var cuentas = result.Select(x => new { id = x.CODIGO, text = x.NOMBRE });
                return Content(JsonConvert.SerializeObject(new { results = cuentas.ToList() }, Formatting.None), "application/json");
            }
        }

        public ActionResult Getcomprobantes(string term)
        {
            using (var ctx = new AccountingContext())
            {
                var libros = ctx.Comprobantes.Where(l => l.TIPO.Contains(term) || l.NUMERO.Contains(term));
                //var libros = result.Select(l => new { id = l.TIPO, text = l.NUMERO });
                return Content(JsonConvert.SerializeObject(new { results = libros.ToList() }, Formatting.None), "application/json");
            }
        }

        public ActionResult GetLibro(string term)
        {
            using (var ctx = new AccountingContext())
            {
                var mayor = ctx.Movimientos.Where(l => l.TIPO.Contains(term) || l.NUMERO.Contains(term));
                //var libros = result.Select(l => new { id = l.TIPO, text = l.NUMERO });
                return Content(JsonConvert.SerializeObject(value: new { results = mayor.ToList() }, formatting: Formatting.None), "application/json");
            }
        }

        public string GetMes(int n)
        {
            string mes = "";
            if (n == 1)
            {
                mes = "ENERO";
            }
            else if (n == 2)
            {
                mes = "FEBRERO";
            }
            else if (n == 3)
            {
                mes = "MARZO";
            }
            else if (n == 4)
            {
                mes = "ABRIL";
            }
            else if (n == 5)
            {
                mes = "MAYO";
            }
            else if (n == 6)
            {
                mes = "JUNIO";
            }
            else if (n == 7)
            {
                mes = "JULIO";
            }
            else if (n == 8)
            {
                mes = "AGOSTO";
            }
            else if (n == 9)
            {
                mes = "SEPTIEMBRE";
            }
            else if (n == 10)
            {
                mes = "OCTUBRE";
            }
            else if (n == 11)
            {
                mes = "NOVIEMBRE";
            }
            else if (n == 12)
            {
                mes = "DICIEMBRE";
            }

            return mes;

        }

        public string GetMesCorto(int n)
        {
            string mes = "";
            if (n == 1)
            {
                mes = "ENE";
            }
            else if (n == 2)
            {
                mes = "FEB";
            }
            else if (n == 3)
            {
                mes = "MAR";
            }
            else if (n == 4)
            {
                mes = "ABR";
            }
            else if (n == 5)
            {
                mes = "MAY";
            }
            else if (n == 6)
            {
                mes = "JUN";
            }
            else if (n == 7)
            {
                mes = "JUL";
            }
            else if (n == 8)
            {
                mes = "AGO";
            }
            else if (n == 9)
            {
                mes = "SEP";
            }
            else if (n == 10)
            {
                mes = "OCT";
            }
            else if (n == 11)
            {
                mes = "NOV";
            }
            else if (n == 12)
            {
                mes = "DIC";
            }

            return mes;

        }

        [HttpPost]
        public ActionResult ExcelSaldos(FormCollection coll)
        {
            // Do some stuff
            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;

            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";

            var anioInicio = Int32.Parse(coll["year2"]);
            //var hastaMes = Int32.Parse(coll["mes2"]);
            var chktodos = coll["chkTodos"];
            var fechaDesde3 = coll["fechaDesde3"];
            var fechaHasta3 = coll["fechaHasta3"];
            var chkfechaDesembolso = coll["chkFechaDesembolso"];
            var desdeSaldo = Int32.Parse(coll["monthini"]);
            var desdeSaldoAno = Int32.Parse(coll["year"]);
            var hastaSaldo = Int32.Parse(coll["monthfin"]);
            var informe = Int32.Parse(coll["informe"]);
            var cuenta = coll["Cuenta"];
            var documento = coll["Tercero"];
            var archivo = "";
            var ctx = new AccountingContext();
            var saldos = (from sal in ctx.SaldosCuentas
                          join cuentas in ctx.PlanCuentas on sal.CODIGO equals cuentas.CODIGO
                          select new { sal, cuentas }).ToList();
            var Hsaldos = (from Hsal in ctx.HSaldosCuentas
                           join cuentas in ctx.PlanCuentas on Hsal.CODIGO equals cuentas.CODIGO
                           select new { Hsal, cuentas }).ToList();
            var movimientos = (from mov in ctx.Movimientos
                               join cuentas in ctx.PlanCuentas on mov.CUENTA equals cuentas.CODIGO
                               join com in ctx.Comprobantes on new { mov.TIPO, mov.NUMERO } equals new { com.TIPO, com.NUMERO }
                               join com1 in ctx.Terceros on mov.TERCERO equals com1.NIT
                               select new { mov, cuentas, com, com1 }).ToList();

            var morosidad = (from fichas in ctx.FichasAportes
                             join fac in ctx.FactOpcaja on fichas.idPersona
                                   equals fac.nit_propietario_cuenta
                             select new
                             {
                                 fichas,
                                 fac.nombre_propietario_cuenta
                             }).ToList().Distinct();


            var morosidad2 = db.FichasAportes.ToList();


            if (informe == 37) archivo = "attachment;filename=balanceComprobacion.xlsx";
            else if (informe == 1) archivo = "attachment;filename=balanceDetallado.xlsx";
            else if (informe == 22) archivo = "attachment;filename=AuxiliarPorTerceros.xlsx";
            else if (informe == 2) archivo = "attachment;filename=AuxiliarCuentas.xlsx";
            else if (informe == 3) archivo = "attachment;filename=estadoResultados.xlsx";
            else if (informe == 6) archivo = "attachment;filename=librodiario.xlsx";
            else if (informe == 7) archivo = "attachment;filename=libromayor.xlsx";
            else if (informe == 8) archivo = "attachment;filename=Estado de Situacion Financiera.xlsx";
            else if (informe == 9) archivo = "attachment;filename=balanceDetalladoniif.xlsx";
            else if (informe == 10) archivo = "attachment;filename=Auxiliarniif.xlsx";
            else if (informe == 12) archivo = "attachment;filename=estadoResultadosIntegral.xlsx";
            else if (informe == 13) archivo = "attachment;filename=EstadodeCambiosenelPatrimonio.xlsx";
            else if (informe == 14) archivo = "attachment;filename=EstadoDeResultadosIntegral.xlsx";
            else if (informe == 11) archivo = "attachment;filename=EstadodeFlujosdeEfectivo.xlsx";
            else if (informe == 17) archivo = "attachment;filename=MorosidadDeAportes.xlsx";
            else if (informe == 30) archivo = "attachment;filename=Terceros.xlsx";
            else if (informe == 31) archivo = "attachment;filename=individualDeAportes.xlsx";
            else if (informe == 32) archivo = "attachment;filename=creditos.xlsx";
            else if (informe == 33) archivo = "attachment;filename=ahorros.xlsx";
            else if (informe == 36) archivo = "attachment;filename=creditos.xlsx";
            else if (informe == 4) archivo = "attachment;filename=BalanceGeneral.xlsx";
            else if (informe == 39) archivo = "attachment;filename=aportesAfecha.xlsx";
            else if (informe == 40) archivo = "attachment;filename=AfiliacionesPorAsesor.xlsx";
            else if (informe == 41) archivo = "attachment;filename=AfiliacionesPorAgencia.xlsx";
            else if (informe == 42) archivo = "attachment;filename=AuxiliarDetalladoTerceros.xlsx";
            else if (informe == 45) archivo = "attachment;filename=ReporteUIAF.xlsx";
            else if (informe == 46) archivo = "attachment;filename=IndividualDeCarteraDeCredito.xlsx";
            else archivo = "attachment;filename=balanceGeneral.xlsx";



            #region Filtros

            if (cuenta != "")
            {
                saldos = saldos.Where(p => p.sal.CODIGO == cuenta).ToList();
                movimientos = movimientos.Where(m => m.mov.CUENTA == cuenta).ToList();
            }

            if (documento != "")
            {
                movimientos = movimientos.Where(m => m.mov.TERCERO == documento).ToList();
            }



            if (desdeSaldo == 0)
            {
                if (hastaSaldo != 0)
                {
                    saldos = saldos.Where(p => p.sal.ANO == desdeSaldoAno && p.sal.MES <= hastaSaldo).ToList();
                    movimientos = movimientos.Where(m => m.mov.FECHAMOVIMIENTO.Year == desdeSaldoAno && m.mov.FECHAMOVIMIENTO.Month <= hastaSaldo).ToList();
                }
                else
                {
                    saldos = saldos.Where(p => p.sal.ANO == desdeSaldoAno).ToList();
                    movimientos = movimientos.Where(m => m.mov.FECHAMOVIMIENTO.Year == desdeSaldoAno).ToList();
                }

            }
            else
            {
                if (hastaSaldo == 0)
                {
                    saldos = saldos.Where(p => p.sal.MES == desdeSaldo && p.sal.ANO == desdeSaldoAno).ToList();
                    movimientos = movimientos.Where(m => m.mov.FECHAMOVIMIENTO.Month == desdeSaldo && m.mov.FECHAMOVIMIENTO.Year == desdeSaldoAno).ToList();
                }
                if (desdeSaldo <= hastaSaldo)
                {
                    saldos = saldos.Where(p => p.sal.MES >= desdeSaldo && p.sal.MES <= hastaSaldo && p.sal.ANO == desdeSaldoAno).ToList();
                    movimientos = movimientos.Where(m => m.mov.FECHAMOVIMIENTO.Month >= desdeSaldo && m.mov.FECHAMOVIMIENTO.Month <= hastaSaldo && m.mov.FECHAMOVIMIENTO.Year == desdeSaldoAno).ToList();
                }
                else
                {
                    saldos = saldos.Where(p => p.sal.MES >= desdeSaldo && p.sal.ANO == desdeSaldoAno).ToList();
                    movimientos = movimientos.Where(m => m.mov.FECHAMOVIMIENTO.Month >= desdeSaldo && m.mov.FECHAMOVIMIENTO.Year == desdeSaldoAno).ToList();
                }
            }

            if (desdeSaldo == 0)
            {
                if (hastaSaldo != 0)
                {

                    saldos = saldos.Where(p => p.sal.ANO == desdeSaldoAno && p.sal.MES <= hastaSaldo).ToList();


                    morosidad = morosidad.Where(m => m.fichas.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno && m.fichas.fechaApertura.GetValueOrDefault().Month <= hastaSaldo).ToList();

                    morosidad = morosidad.Where(m => m.fichas.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno && m.fichas.fechaApertura.GetValueOrDefault().Month <= hastaSaldo).ToList();
                }
                else
                {
                    saldos = saldos.Where(p => p.sal.ANO == desdeSaldoAno).ToList();
                    morosidad = morosidad.Where(m => m.fichas.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno).ToList();
                }

            }
            else
            {
                if (hastaSaldo == 0)
                {
                    saldos = saldos.Where(p => p.sal.MES == desdeSaldo && p.sal.ANO == desdeSaldoAno).ToList();
                    morosidad = morosidad.Where(m => m.fichas.fechaApertura.GetValueOrDefault().Month == desdeSaldo && m.fichas.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno).ToList();
                }
                if (desdeSaldo <= hastaSaldo)
                {
                    saldos = saldos.Where(p => p.sal.MES >= desdeSaldo && p.sal.MES <= hastaSaldo && p.sal.ANO == desdeSaldoAno).ToList();
                    morosidad = morosidad.Where(m => m.fichas.fechaApertura.GetValueOrDefault().Month >= desdeSaldo && m.fichas.fechaApertura.GetValueOrDefault().Month <= hastaSaldo && m.fichas.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno).ToList();
                }
                else
                {
                    saldos = saldos.Where(p => p.sal.MES >= desdeSaldo && p.sal.ANO == desdeSaldoAno).ToList();
                    morosidad = morosidad.Where(m => m.fichas.fechaApertura.GetValueOrDefault().Month >= desdeSaldo && m.fichas.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno).ToList();
                }
            }


            saldos = saldos.OrderBy(s => s.sal.CODIGO).ThenBy(s => s.sal.ANO).ThenBy(s => s.sal.MES).ToList();
            morosidad = morosidad.OrderBy(m => m.fichas.id).ThenBy(m => m.fichas.fechaApertura.GetValueOrDefault()).ToList();

            #endregion

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", archivo);

            using (ExcelPackage pack = new ExcelPackage())
            {


                if (informe == 37)
                {

                    var fDesde = coll["fechaDesde"];
                    var fHasta = coll["fechaHasta"];
                    var chkTercero = coll["chkTercero"];
                    var nivel = coll["nivel"];
                    var costo = coll["costo"];

                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("Balance De Comprobación");
                    if (chkTercero == "on")
                    {
                        ws.Cells["A" + 2].Value = "COOPERATIVA DE APORTE Y CRÉDITO ASOPASCUALINOS";
                        ws.Cells["A" + 3].Value = "BALANCE DE COMPROBACIÓN";

                        ws.Cells["A2:J2"].Merge = true;//une columnas en una fila
                        ws.Cells["A3:J3"].Merge = true;

                        ws.Cells["B" + 5].Value = "CUENTA";
                        ws.Cells["C" + 5].Value = "NOMBRE CUENTA";
                        ws.Cells["E" + 5].Value = "DOCUMENTO TERCERO";
                        ws.Cells["F" + 5].Value = "NOMBRE TERCERO";
                        ws.Cells["G" + 5].Value = "SALDO INICIAL";
                        ws.Cells["H" + 5].Value = "DÉBITO";
                        ws.Cells["I" + 5].Value = "CRÉDITO";
                        ws.Cells["J" + 5].Value = "SALDO";
                    }
                    else
                    {
                        ws.Cells["A" + 2].Value = "COOPERATIVA DE APORTE Y CRÉDITO ASOPASCUALINOS";
                        ws.Cells["A" + 3].Value = "BALANCE DE COMPROBACIÓN";

                        ws.Cells["A2:H2"].Merge = true;//une columnas en una fila
                        ws.Cells["A3:H3"].Merge = true;

                        ws.Cells["B" + 5].Value = "CUENTA";
                        ws.Cells["C" + 5].Value = "NOMBRE CUENTA";
                        ws.Cells["E" + 5].Value = "SALDO INICIAL";
                        ws.Cells["F" + 5].Value = "DÉBITO";
                        ws.Cells["G" + 5].Value = "CRÉDITO";
                        ws.Cells["H" + 5].Value = "SALDO";
                    }

                    int j = 7;
                    List<Movimiento> movtosSaldos = new List<Movimiento>();
                    List<Movimiento> movtosActuales = new List<Movimiento>();
                    List<CuentaMayor> auxiliar = new List<CuentaMayor>();


                    if (fHasta != "" && fDesde != "")
                    {
                        DateTime fh = Convert.ToDateTime(fHasta);
                        DateTime fd = Convert.ToDateTime(fDesde);
                        DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                        DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                        movtosActuales = db.Movimientos.Where(x => x.FECHAMOVIMIENTO <= fechHasta && x.Comprobante.ANULADO == false).ToList();
                        if (costo != "")
                        {
                            movtosActuales = movtosActuales.Where(x => x.CCOSTO == costo).ToList();
                        }
                        movtosSaldos = movtosActuales.Where(x => x.FECHAMOVIMIENTO < fechDesde).ToList();
                        movtosActuales = movtosActuales.Where(x => x.FECHAMOVIMIENTO >= fechDesde).ToList();

                    }
                    else if (fDesde != "")
                    {

                        DateTime fd = Convert.ToDateTime(fDesde);
                        DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                        movtosActuales = db.Movimientos.Where(x => x.FECHAMOVIMIENTO == fechDesde && x.Comprobante.ANULADO == false).ToList();
                        if (costo != "")
                        {
                            movtosActuales = movtosActuales.Where(x => x.CCOSTO == costo).ToList();
                        }
                        movtosSaldos = movtosActuales.Where(x => x.FECHAMOVIMIENTO < fechDesde).ToList();

                    }

                    var cuentas = db.PlanCuentas.ToList();

                    //nivel 1
                    if (nivel == "1")
                    {
                        auxiliar = cuentas.Where(x => x.CODIGO.Length == 1).ToList();
                    }
                    else if (nivel == "2")
                    {
                        auxiliar = cuentas.Where(x => x.CODIGO.Length == 1 || x.CODIGO.Length == 2).ToList();
                    }
                    else if (nivel == "3")
                    {
                        auxiliar = cuentas.Where(x => x.CODIGO.Length == 1 || x.CODIGO.Length == 2 || x.CODIGO.Length == 4).ToList();
                    }
                    else if (nivel == "4")
                    {
                        auxiliar = cuentas.Where(x => x.CODIGO.Length == 1 || x.CODIGO.Length == 2 || x.CODIGO.Length == 4 || x.CODIGO.Length == 6).ToList();
                    }
                    else if (nivel == "5")
                    {
                        auxiliar = cuentas.Where(x => x.CODIGO.Length == 1 || x.CODIGO.Length == 2 || x.CODIGO.Length == 4 || x.CODIGO.Length == 6 || x.CODIGO.Length == 9).ToList();


                        //var fecha = DateTime.UtcNow;
                        //slPC = db.Database.SqlQuery<spBalanceComprobacionL5>(
                        //    "dbo.spBalanceDeComprobacionL5 @fecha",
                        //    new SqlParameter("@fecha", fecha)
                        //    ).ToList();
                    }
                    List<spBalanceComprobacionL5> slPC = new List<spBalanceComprobacionL5>();
                    DateTime fdAuxilar = Convert.ToDateTime(fDesde);
                    DateTime fDesdeAuxiliar = new DateTime(fdAuxilar.Year, 1, 1, 0, 0, 0);

                    if (auxiliar.Count > 0)
                    {

                        if (chkTercero != "on")
                        {
                            decimal saldoInicial = 0, debitoActual = 0, creditoActual = 0, debitoAnterior = 0, creditoAnterior = 0, saldo = 0;
                            foreach (var item2 in auxiliar)
                            {
                                var dataActual = movtosActuales.Where(x => x.CUENTA.StartsWith(item2.CODIGO)).ToList();
                                var dataAnterior = movtosSaldos.Where(x => x.CUENTA.StartsWith(item2.CODIGO)).ToList();
                                var dataAnteriorAuxiliar = dataAnterior.Where(x => x.FECHAMOVIMIENTO >= fDesdeAuxiliar).ToList();

                                if (dataActual.Count != 0 || dataAnterior.Count != 0)
                                {

                                    //ws.Cells["B" + j].Value = item2.CODIGO;
                                    //ws.Cells["C" + j].Value = item2.NOMBRE;

                                    if (dataAnterior.Count == 0 || item2.CODIGO.StartsWith("4") || item2.CODIGO.StartsWith("5") || item2.CODIGO.StartsWith("6") || item2.CODIGO.StartsWith("7"))
                                    {
                                        debitoAnterior = dataAnteriorAuxiliar.Select(x => x.DEBITO).Sum();
                                        creditoAnterior = dataAnteriorAuxiliar.Select(x => x.CREDITO).Sum();
                                    }
                                    else
                                    {
                                        debitoAnterior = dataAnterior.Select(x => x.DEBITO).Sum();
                                        creditoAnterior = dataAnterior.Select(x => x.CREDITO).Sum();
                                    }

                                    debitoActual = dataActual.Select(x => x.DEBITO).Sum();
                                    creditoActual = dataActual.Select(x => x.CREDITO).Sum();

                                    if (item2.NATURALEZA == "D")
                                    {
                                        saldoInicial = debitoAnterior - creditoAnterior;
                                        saldo = (debitoActual - creditoActual) + saldoInicial;
                                    }
                                    else
                                    {
                                        saldoInicial = creditoAnterior - debitoAnterior;
                                        saldo = (creditoActual - debitoActual) + saldoInicial;
                                    }

                                    //ws.Cells["E" + j].Value = saldoInicial.ToString("N0", formato);
                                    //ws.Cells["F" + j].Value = debitoActual.ToString("N0", formato);
                                    //ws.Cells["G" + j].Value = creditoActual.ToString("N0", formato);
                                    //ws.Cells["H" + j].Value = saldo.ToString("N0", formato);

                                    var objeto = new spBalanceComprobacionL5()
                                    {
                                        codigo = item2.CODIGO,
                                        nombre = item2.NOMBRE,
                                        SaldoInicial = saldoInicial.ToString("N0", formato),
                                        Debito = debitoActual.ToString("N0", formato),
                                        Credito = creditoActual.ToString("N0", formato),
                                        Saldo = saldo.ToString("N0", formato)
                                    };
                                    slPC.Add(objeto);

                                    //j++;
                                }

                            }
                        }
                        else
                        {
                            decimal saldoInicial = 0, debitoActual = 0, creditoActual = 0, debitoAnterior = 0, creditoAnterior = 0, saldo = 0;
                            foreach (var item2 in auxiliar)
                            {
                                var dataActual = movtosActuales.Where(x => x.CUENTA.StartsWith(item2.CODIGO)).ToList();
                                var dataAnterior = movtosSaldos.Where(x => x.CUENTA.StartsWith(item2.CODIGO)).ToList();
                                var dataAnteriorAuxiliar = dataAnterior.Where(x => x.FECHAMOVIMIENTO >= fDesdeAuxiliar).ToList();

                                if (dataActual.Count != 0 || dataAnterior.Count != 0)
                                {
                                    ws.Cells["B" + j].Value = item2.CODIGO;
                                    ws.Cells["C" + j].Value = item2.NOMBRE;
                                    if (item2.CODIGO.Length != 9)
                                    {
                                        if (dataAnterior.Count == 0 || item2.CODIGO.StartsWith("4") || item2.CODIGO.StartsWith("5") || item2.CODIGO.StartsWith("6") || item2.CODIGO.StartsWith("7"))
                                        {
                                            debitoAnterior = dataAnteriorAuxiliar.Select(x => x.DEBITO).Sum();
                                            creditoAnterior = dataAnteriorAuxiliar.Select(x => x.CREDITO).Sum();
                                        }
                                        else
                                        {
                                            debitoAnterior = dataAnterior.Select(x => x.DEBITO).Sum();
                                            creditoAnterior = dataAnterior.Select(x => x.CREDITO).Sum();
                                        }

                                        debitoActual = dataActual.Select(x => x.DEBITO).Sum();
                                        creditoActual = dataActual.Select(x => x.CREDITO).Sum();

                                        if (item2.NATURALEZA == "D")
                                        {
                                            saldoInicial = debitoAnterior - creditoAnterior;
                                            saldo = (debitoActual - creditoActual) + saldoInicial;
                                        }
                                        else
                                        {
                                            saldoInicial = creditoAnterior - debitoAnterior;
                                            saldo = (creditoActual - debitoActual) + saldoInicial;
                                        }


                                        ws.Cells["G" + j].Value = saldoInicial.ToString("N0", formato);
                                        ws.Cells["H" + j].Value = debitoActual.ToString("N0", formato);
                                        ws.Cells["I" + j].Value = creditoActual.ToString("N0", formato);
                                        ws.Cells["J" + j].Value = saldo.ToString("N0", formato);

                                        j++;
                                    }//fin if != 9
                                    else
                                    {
                                        if (dataAnterior.Count == 0 || item2.CODIGO.StartsWith("4") || item2.CODIGO.StartsWith("5") || item2.CODIGO.StartsWith("6") || item2.CODIGO.StartsWith("7"))
                                        {
                                            var info = (from da in dataActual
                                                        select new { da.TERCERO, da.CUENTA, da.terceroFK }
                                                        ).OrderBy(x => x.CUENTA).Distinct();

                                            foreach (var item3 in info)
                                            {
                                                var actual = dataActual.Where(x => x.TERCERO == item3.TERCERO && x.CUENTA == item2.CODIGO).ToList();
                                                var anterior = dataAnteriorAuxiliar.Where(x => x.TERCERO == item3.TERCERO && x.CUENTA == item2.CODIGO).ToList();


                                                debitoAnterior = anterior.Select(x => x.DEBITO).Sum();
                                                creditoAnterior = anterior.Select(x => x.CREDITO).Sum();

                                                debitoActual = actual.Select(x => x.DEBITO).Sum();
                                                creditoActual = actual.Select(x => x.CREDITO).Sum();


                                                if (item2.NATURALEZA == "D")
                                                {
                                                    saldoInicial = debitoAnterior - creditoAnterior;
                                                    saldo = (debitoActual - creditoActual) + saldoInicial;
                                                }
                                                else
                                                {
                                                    saldoInicial = creditoAnterior - debitoAnterior;
                                                    saldo = (creditoActual - debitoActual) + saldoInicial;
                                                }

                                                ws.Cells["B" + j].Value = item2.CODIGO;
                                                ws.Cells["C" + j].Value = item2.NOMBRE;
                                                ws.Cells["E" + j].Value = item3.TERCERO;
                                                if (item3.terceroFK != null)
                                                {
                                                    ws.Cells["F" + j].Value = item3.terceroFK.NOMBRE1 + " " + item3.terceroFK.NOMBRE2 + " " + item3.terceroFK.APELLIDO1 + " " + item3.terceroFK.APELLIDO2;
                                                }
                                                else
                                                {
                                                    ws.Cells["F" + j].Value = "";
                                                }
                                                ws.Cells["G" + j].Value = saldoInicial.ToString("N0", formato);
                                                ws.Cells["H" + j].Value = debitoActual.ToString("N0", formato);
                                                ws.Cells["I" + j].Value = creditoActual.ToString("N0", formato);
                                                ws.Cells["J" + j].Value = saldo.ToString("N0", formato);

                                                j++;

                                            }

                                        }
                                        else
                                        {
                                            var info = (from da in dataActual
                                                        select new { da.TERCERO, da.CUENTA, da.terceroFK }
                                                        ).OrderBy(x => x.CUENTA).Distinct();

                                            foreach (var item3 in info)
                                            {
                                                var actual = dataActual.Where(x => x.TERCERO == item3.TERCERO && x.CUENTA == item2.CODIGO).ToList();
                                                var anterior = dataAnterior.Where(x => x.TERCERO == item3.TERCERO && x.CUENTA == item2.CODIGO).ToList();


                                                debitoAnterior = anterior.Select(x => x.DEBITO).Sum();
                                                creditoAnterior = anterior.Select(x => x.CREDITO).Sum();

                                                debitoActual = actual.Select(x => x.DEBITO).Sum();
                                                creditoActual = actual.Select(x => x.CREDITO).Sum();


                                                if (item2.NATURALEZA == "D")
                                                {
                                                    saldoInicial = debitoAnterior - creditoAnterior;
                                                    saldo = (debitoActual - creditoActual) + saldoInicial;
                                                }
                                                else
                                                {
                                                    saldoInicial = creditoAnterior - debitoAnterior;
                                                    saldo = (creditoActual - debitoActual) + saldoInicial;
                                                }

                                                ws.Cells["B" + j].Value = item2.CODIGO;
                                                ws.Cells["C" + j].Value = item2.NOMBRE;
                                                ws.Cells["E" + j].Value = item3.TERCERO;
                                                if (item3.terceroFK != null)
                                                {
                                                    ws.Cells["F" + j].Value = item3.terceroFK.NOMBRE1 + " " + item3.terceroFK.NOMBRE2 + " " + item3.terceroFK.APELLIDO1 + " " + item3.terceroFK.APELLIDO2;
                                                }
                                                else
                                                {
                                                    ws.Cells["F" + j].Value = "";
                                                }

                                                ws.Cells["G" + j].Value = saldoInicial.ToString("N0", formato);
                                                ws.Cells["H" + j].Value = debitoActual.ToString("N0", formato);
                                                ws.Cells["I" + j].Value = creditoActual.ToString("N0", formato);
                                                ws.Cells["J" + j].Value = saldo.ToString("N0", formato);

                                                j++;

                                            }
                                        }


                                    }//fin else != 9

                                }//fin if dataActual != 0

                            }//fin for item2
                        }


                    }

                    foreach (var ob in slPC)
                    {
                        ws.Cells["B" + j].Value = ob.codigo;
                        ws.Cells["C" + j].Value = ob.nombre;
                        ws.Cells["E" + j].Value = ob.SaldoInicial;
                        ws.Cells["F" + j].Value = ob.Debito;
                        ws.Cells["G" + j].Value = ob.Credito;
                        ws.Cells["H" + j].Value = ob.Saldo;
                        j++;
                    }
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();//siempre al final de todo. le da tamaño ajustado a cada columna



                }
                else if (informe == 1)
                {
                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("balanceDetallado");
                    ws.Cells["A" + 1].Value = "CUENTA";
                    ws.Cells["B" + 1].Value = "AÑO";
                    ws.Cells["C" + 1].Value = "MES";
                    ws.Cells["D" + 1].Value = "DETALLE";
                    ws.Cells["E" + 1].Value = "SALDO ANTERIOR";
                    ws.Cells["G" + 1].Value = "CREDITO";
                    ws.Cells["F" + 1].Value = "DEBITO";
                    ws.Cells["H" + 1].Value = "SALDO";
                    int i = 2;
                    var cuenta_act = "";
                    decimal saldo_ant = 0;
                    decimal saldo_act = 0;

                    foreach (var item in movimientos)
                    {
                        if (cuenta_act != item.mov.CUENTA)
                        {
                            saldo_ant = GetSaldos(desdeSaldoAno, desdeSaldo, item.mov.CUENTA, documento);
                            cuenta_act = item.mov.CUENTA;
                        }
                        if (item.cuentas.NATURALEZA == "D")
                        {
                            saldo_act = saldo_ant + item.mov.DEBITO - item.mov.CREDITO;
                        }
                        else
                        {
                            saldo_act = saldo_ant + item.mov.CREDITO - item.mov.DEBITO;
                        }

                        ws.Cells["A" + i].Value = item.mov.CUENTA;
                        ws.Cells["B" + i].Value = item.mov.FECHAMOVIMIENTO.Year;
                        ws.Cells["C" + i].Value = item.mov.FECHAMOVIMIENTO.Month;
                        ws.Cells["D" + i].Value = item.mov.DETALLE;
                        ws.Cells["E" + i].Value = saldo_ant;
                        ws.Cells["G" + i].Value = item.mov.CREDITO;
                        ws.Cells["F" + i].Value = item.mov.DEBITO;
                        ws.Cells["H" + i].Value = saldo_act;
                        saldo_ant = saldo_act;
                        i++;
                    }


                }

                else if (informe == 9)
                {
                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("balanceDetalladoNIIf");
                    ws.Cells["A" + 1].Value = "CUENTA";
                    ws.Cells["B" + 1].Value = "AÑO";
                    ws.Cells["C" + 1].Value = "MES";
                    ws.Cells["D" + 1].Value = "DETALLE";
                    ws.Cells["E" + 1].Value = "SALDO ANTERIOR";
                    ws.Cells["G" + 1].Value = "CREDITO";
                    ws.Cells["F" + 1].Value = "DEBITO";
                    ws.Cells["H" + 1].Value = "SALDO";
                    int i = 2;
                    var cuenta_act = "";
                    decimal saldo_ant = 0;
                    decimal saldo_act = 0;
                    foreach (var item in movimientos)
                    {
                        if (cuenta_act != item.mov.CUENTA)
                        {
                            saldo_ant = GetSaldos(desdeSaldoAno, desdeSaldo, item.mov.CUENTA, documento);
                            cuenta_act = item.mov.CUENTA;
                        }
                        if (item.cuentas.NATURALEZA == "D")
                        {
                            saldo_act = saldo_ant + item.mov.DEBITO - item.mov.CREDITO;
                        }
                        else
                        {
                            saldo_act = saldo_ant + item.mov.CREDITO - item.mov.DEBITO;
                        }
                        ws.Cells["A" + i].Value = item.mov.CUENTA;
                        ws.Cells["B" + i].Value = item.mov.FECHAMOVIMIENTO.Year;
                        ws.Cells["C" + i].Value = item.mov.FECHAMOVIMIENTO.Month;
                        ws.Cells["D" + i].Value = item.mov.DETALLE;
                        ws.Cells["E" + i].Value = saldo_ant;
                        ws.Cells["G" + i].Value = item.mov.CREDITO;
                        ws.Cells["F" + i].Value = item.mov.DEBITO;
                        ws.Cells["H" + i].Value = saldo_act;
                        saldo_ant = saldo_act;
                        i++;
                    }
                }
                else if (informe == 22)
                {

                    var movimiento = db.Movimientos.Where(x => x.Comprobante.ANULADO == false).ToList();
                    var fechaDesde = coll["fechaDesde"];
                    var fechaHasta = coll["fechaHasta"];
                    if (cuenta != "")
                    {
                        movimiento = movimiento.Where(x => x.CUENTA == cuenta).ToList();
                    }
                    if (documento != "")
                    {
                        movimiento = movimiento.Where(x => x.TERCERO == documento).ToList();
                    }
                    if (fechaDesde != "")
                    {
                        DateTime auxfd = Convert.ToDateTime(fechaDesde);
                        DateTime fd = new DateTime(auxfd.Year, auxfd.Month, auxfd.Day, 0, 0, 0);
                        if (fechaHasta != "")
                        {
                            DateTime auxfh = Convert.ToDateTime(fechaHasta);
                            DateTime fh = new DateTime(auxfh.Year, auxfh.Month, auxfh.Day, 23, 59, 59);
                            movimiento = movimiento.Where(X => X.FECHAMOVIMIENTO >= fd && X.FECHAMOVIMIENTO <= fh).ToList();
                        }
                        else
                        {
                            movimiento = movimiento.Where(X => X.FECHAMOVIMIENTO.Year == fd.Year && X.FECHAMOVIMIENTO.Month == fd.Month && X.FECHAMOVIMIENTO.Day == fd.Day).ToList();
                        }
                    }

                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("AuxiliarPorTerceros");
                    ws.Cells["A" + 1].Value = "CÓDIGO";
                    ws.Cells["B" + 1].Value = "NOMBRE";
                    ws.Cells["C" + 1].Value = "TERCERO";
                    ws.Cells["D" + 1].Value = "NOMBRE TERCERO";
                    ws.Cells["E" + 1].Value = "COMPROBANTE";
                    ws.Cells["F" + 1].Value = "FECHA";
                    ws.Cells["G" + 1].Value = "DÉBITO";
                    ws.Cells["H" + 1].Value = "CRÉDITO";
                    ws.Cells["I" + 1].Value = "SALDO";

                    var mov = (from m in movimiento
                               orderby m.CUENTA
                               orderby m.TERCERO
                               select new { m.CUENTA, m.TERCERO, m.cuentaFK, m.terceroFK }).Distinct().ToList();
                    int i = 3;
                    decimal saldo = 0, saldoTotal = 0;
                    foreach (var item in mov)
                    {
                        var dataMov = movimiento.Where(x => x.CUENTA == item.CUENTA && x.TERCERO == item.TERCERO).OrderBy(x => x.FECHAMOVIMIENTO).ToList();
                        decimal debito = dataMov.Select(x => x.DEBITO).Sum();
                        decimal credito = dataMov.Select(x => x.CREDITO).Sum();
                        ws.Cells["A" + i].Value = item.CUENTA;
                        ws.Cells["B" + i].Value = item.cuentaFK.NOMBRE;
                        ws.Cells["C" + i].Value = item.TERCERO;
                        ws.Cells["D" + i].Value = item.terceroFK.NOMBRE1 + " " + item.terceroFK.NOMBRE2 + " " + item.terceroFK.APELLIDO1 + " " + item.terceroFK.APELLIDO2; ;
                        string naturaleza = item.cuentaFK.NATURALEZA;
                        i++;
                        foreach (var item2 in dataMov)
                        {
                            if (naturaleza == "D")
                            {
                                saldo = item2.DEBITO - item2.CREDITO;
                            }
                            else
                            {
                                saldo = item2.CREDITO - item2.DEBITO;
                            }

                            ws.Cells["E" + i].Value = item2.TIPO + " " + item2.NUMERO;
                            ws.Cells["F" + i].Value = item2.FECHAMOVIMIENTO.ToString("yyyy-MM-dd");
                            ws.Cells["G" + i].Value = item2.DEBITO.ToString("N0", formato);
                            ws.Cells["H" + i].Value = item2.CREDITO.ToString("N0", formato);
                            ws.Cells["I" + i].Value = saldo.ToString("N0", formato);
                            i++;
                        }
                        if (naturaleza == "D")
                        {
                            saldoTotal = debito - credito;
                        }
                        else
                        {
                            saldoTotal = credito - debito;
                        }
                        ws.Cells["F" + i].Value = "TOTAL";
                        ws.Cells["G" + i].Value = debito.ToString("N0", formato);
                        ws.Cells["H" + i].Value = credito.ToString("N0", formato);
                        ws.Cells["I" + i].Value = saldoTotal.ToString("N0", formato);
                        i += 2;
                    }
                }

                else if (informe == 10)
                {
                    movimientos = movimientos.OrderBy(m => m.mov.CUENTA).ThenBy(m => m.mov.FECHAMOVIMIENTO).ToList();

                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("AuxiliarNiif");
                    ws.Cells["A" + 1].Value = "CUENTA";
                    ws.Cells["B" + 1].Value = "AÑO";
                    ws.Cells["C" + 1].Value = "MES";
                    ws.Cells["D" + 1].Value = "DIA";
                    ws.Cells["E" + 1].Value = "TERCERO";
                    ws.Cells["F" + 1].Value = "NOMBRE";
                    ws.Cells["G" + 1].Value = "DETALLE";
                    ws.Cells["H" + 1].Value = "DEBITO";
                    ws.Cells["I" + 1].Value = "CREDITO";
                    ws.Cells["J" + 1].Value = "SALDO";
                    int i = 2;
                    var cuenta_act = "";
                    decimal saldo_ant = 0;
                    decimal saldo_act = 0;
                    foreach (var item in movimientos)
                    {
                        if (cuenta_act != item.mov.CUENTA)
                        {
                            saldo_ant = GetSaldos(desdeSaldoAno, desdeSaldo, item.mov.CUENTA, documento);
                            cuenta_act = item.mov.CUENTA;
                        }
                        if (item.cuentas.NATURALEZA == "D")
                        {
                            saldo_act = saldo_ant + item.mov.DEBITO - item.mov.CREDITO;
                        }
                        else
                        {
                            saldo_act = saldo_ant + item.mov.CREDITO - item.mov.DEBITO;
                        }
                        ws.Cells["A" + i].Value = item.mov.CUENTA;
                        ws.Cells["B" + i].Value = item.mov.FECHAMOVIMIENTO.Year;
                        ws.Cells["C" + i].Value = item.mov.FECHAMOVIMIENTO.Month;
                        ws.Cells["D" + i].Value = item.mov.FECHAMOVIMIENTO.Day;
                        ws.Cells["E" + i].Value = item.mov.TERCERO;
                        ws.Cells["F" + i].Value = item.com1.NOMBRE;
                        ws.Cells["G" + i].Value = item.com.DETALLE;
                        ws.Cells["H" + i].Value = item.mov.DEBITO;
                        ws.Cells["I" + i].Value = item.mov.CREDITO;
                        ws.Cells["J" + i].Value = saldo_act;
                        saldo_ant = saldo_act;
                        i++;
                    }
                }
                else if (informe == 8)
                {

                    string dia = "", anio = "";
                    var fecha = coll["fechaHasta5"];
                    var costo = coll["costo"];

                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("Estado de Situación Financiera");
                    ws.Cells["A" + 2].Value = "COOPERATIVA DE APORTE Y CRÉDITO ASOPASCUALINOS";
                    ws.Cells["A" + 3].Value = "ESTADO DE SITUACIÓN FINANCIERA";
                    ws.Cells["A" + 4].Value = "(En miles de pesos colombianos)";

                    ws.Cells["A2:E2"].Merge = true;//une columnas en una fila
                    ws.Cells["A3:E3"].Merge = true;
                    ws.Cells["A4:E4"].Merge = true;

                    if (fecha != "")
                    {
                        DateTime f = Convert.ToDateTime(fecha);
                        DateTime fechFinActual = new DateTime(f.Year, f.Month, f.Day, 23, 59, 59);
                        DateTime fechMovIniActual = new DateTime(f.Year, 1, 1, 0, 0, 0);
                        DateTime fechMovIniAnterior = new DateTime(f.Year - 1, 1, 1, 0, 0, 0);
                        DateTime fechFinAnterior = new DateTime(f.Year - 1, f.Month, f.Day, 23, 59, 59);
                        ws.Cells["B" + 6].Value = "Años terminados el " + f.Day.ToString() + " de " + GetMes(f.Month);
                        ws.Cells["D" + 6].Value = f.Day + " de " + GetMes(f.Month) + " de " + f.Year;
                        ws.Cells["E" + 6].Value = f.Day + " de " + GetMes(f.Month) + " de " + (f.Year - 1).ToString();
                        ws.Cells["B" + 8].Value = "ACTIVO";
                        int j = 9;

                        //consultas

                        var listMovimientos = db.Movimientos.Where(x => ((x.FECHAMOVIMIENTO >= fechMovIniActual && x.FECHAMOVIMIENTO <= fechFinActual) || (x.FECHAMOVIMIENTO >= fechMovIniAnterior && x.FECHAMOVIMIENTO <= fechFinAnterior)) && x.Comprobante.ANULADO == false).ToList();
                        if (costo != "")
                        {
                            listMovimientos = listMovimientos.Where(x => x.CCOSTO == costo).ToList();
                        }

                        var planCuentas = db.PlanCuentas.ToList();

                        var cuentasNIIF = planCuentas.Where(x => x.EsCuentaNIIF == true).ToList();

                        decimal credito = 0, debito = 0, saldo = 0, totalAnterior = 0, totalPatrimonioActual = 0, totalPatrimonioAnterior = 0, superPatrimonioActual = 0, superPatrimonioAnterior = 0;
                        decimal totActual = 0;
                        decimal superTotalActual = 0, superTotalAnterior = 0;
                        if (cuentasNIIF.Count > 0)
                        {
                            //ACTIVOS
                            var activos = cuentasNIIF.Where(x => x.CODIGO.StartsWith("1")).OrderBy(x => x.CODIGO).ToList();
                            foreach (var item in activos)
                            {
                                totActual = 0;
                                totalAnterior = 0;
                                var cuentas = planCuentas.Where(x => x.CTANIIF == item.CODIGO).OrderBy(x => x.CODIGO).ToList();
                                foreach (var item2 in cuentas)
                                {

                                    var dataActual = listMovimientos.Where(x => x.CUENTA == item2.CODIGO && x.FECHAMOVIMIENTO.Year == f.Year).ToList();
                                    if (dataActual != null)
                                    {
                                        credito = dataActual.Select(x => x.CREDITO).Sum();
                                        debito = dataActual.Select(x => x.DEBITO).Sum();
                                        if (item2.NATURALEZA == "D") { saldo = debito - credito; } else { saldo = credito - debito; }
                                        totActual += saldo;

                                    }

                                    var dataAnterior = listMovimientos.Where(x => x.CUENTA == item2.CODIGO && x.FECHAMOVIMIENTO.Year == (f.Year - 1)).ToList();
                                    if (dataAnterior != null)
                                    {
                                        credito = dataAnterior.Select(x => x.CREDITO).Sum();
                                        debito = dataAnterior.Select(x => x.DEBITO).Sum();
                                        if (item2.NATURALEZA == "D") { saldo = debito - credito; } else { saldo = credito - debito; }
                                        totalAnterior += saldo;

                                    }

                                }

                                if (totActual != 0 || totalAnterior != 0)
                                {
                                    ws.Cells["B" + j].Value = item.NOMBRE;
                                    ws.Cells["D" + j].Value = totActual.ToString("N0", formato);
                                    ws.Cells["E" + j].Value = totalAnterior.ToString("N0", formato);
                                    superTotalActual += totActual;
                                    superTotalAnterior += totalAnterior;
                                    totActual = 0;
                                    totalAnterior = 0;
                                    j++;
                                }

                            }
                            j++;
                            ws.Cells["C" + j].Value = "TOTAL ACTIVO";
                            ws.Cells["D" + j].Value = superTotalActual.ToString("N0", formato);
                            ws.Cells["E" + j].Value = superTotalAnterior.ToString("N0", formato);
                            j += 2;

                            ws.Cells["B" + j].Value = "PASIVO";

                            j++;
                            superTotalActual = 0;
                            superTotalAnterior = 0;


                            //PASIVOS
                            var pasivos = cuentasNIIF.Where(x => x.CODIGO.StartsWith("2")).OrderBy(x => x.CODIGO).ToList();
                            foreach (var item in pasivos)
                            {
                                totActual = 0;
                                totalAnterior = 0;
                                var cuentas = planCuentas.Where(x => x.CTANIIF == item.CODIGO).OrderBy(x => x.CODIGO).ToList();
                                foreach (var item2 in cuentas)
                                {
                                    var dataActual = listMovimientos.Where(x => x.CUENTA == item2.CODIGO && x.FECHAMOVIMIENTO.Year == f.Year).ToList();
                                    if (dataActual != null)
                                    {
                                        credito = dataActual.Select(x => x.CREDITO).Sum();
                                        debito = dataActual.Select(x => x.DEBITO).Sum();
                                        if (item2.NATURALEZA == "D") { saldo = debito - credito; } else { saldo = credito - debito; }
                                        totActual += saldo;

                                    }


                                    var dataAnterior = listMovimientos.Where(x => x.CUENTA == item2.CODIGO && x.FECHAMOVIMIENTO.Year == (f.Year - 1)).ToList();
                                    if (dataAnterior != null)
                                    {
                                        credito = dataAnterior.Select(x => x.CREDITO).Sum();
                                        debito = dataAnterior.Select(x => x.DEBITO).Sum();
                                        if (item2.NATURALEZA == "D") { saldo = debito - credito; } else { saldo = credito - debito; }
                                        totalAnterior += saldo;

                                    }

                                }
                                if (totActual != 0 || totalAnterior != 0)
                                {
                                    ws.Cells["B" + j].Value = item.NOMBRE;
                                    ws.Cells["D" + j].Value = totActual.ToString("N0", formato);
                                    ws.Cells["E" + j].Value = totalAnterior.ToString("N0", formato);
                                    superTotalActual += totActual;
                                    superTotalAnterior += totalAnterior;
                                    totActual = 0;
                                    totalAnterior = 0;
                                    j++;
                                }

                            }
                            j++;
                            ws.Cells["C" + j].Value = "TOTAL PASIVO";
                            ws.Cells["D" + j].Value = superTotalActual.ToString("N0", formato);
                            ws.Cells["E" + j].Value = superTotalAnterior.ToString("N0", formato);
                            j += 2;

                            ws.Cells["B" + j].Value = "PATRIMONIO";
                            j++;

                            //PATRIMONIO
                            var patrimonio = cuentasNIIF.Where(x => x.CODIGO.StartsWith("3")).OrderBy(x => x.CODIGO).ToList();
                            foreach (var item in patrimonio)
                            {
                                totalPatrimonioActual = 0;
                                totalPatrimonioAnterior = 0;
                                var cuentas = planCuentas.Where(x => x.CTANIIF == item.CODIGO).OrderBy(x => x.CODIGO).ToList();
                                foreach (var item2 in cuentas)
                                {
                                    var dataActual = listMovimientos.Where(x => x.CUENTA == item2.CODIGO && x.FECHAMOVIMIENTO.Year == f.Year).ToList();
                                    if (dataActual != null)
                                    {
                                        credito = dataActual.Select(x => x.CREDITO).Sum();
                                        debito = dataActual.Select(x => x.DEBITO).Sum();
                                        if (item2.NATURALEZA == "D") { saldo = debito - credito; } else { saldo = credito - debito; }
                                        totalPatrimonioActual += saldo;

                                    }


                                    var dataAnterior = listMovimientos.Where(x => x.CUENTA == item2.CODIGO && x.FECHAMOVIMIENTO.Year == (f.Year - 1)).ToList();
                                    if (dataAnterior != null)
                                    {
                                        credito = dataAnterior.Select(x => x.CREDITO).Sum();
                                        debito = dataAnterior.Select(x => x.DEBITO).Sum();
                                        if (item2.NATURALEZA == "D") { saldo = debito - credito; } else { saldo = credito - debito; }
                                        totalPatrimonioAnterior += saldo;

                                    }

                                }

                                if (totalPatrimonioActual != 0 || totalPatrimonioAnterior != 0)
                                {
                                    ws.Cells["B" + j].Value = item.NOMBRE;
                                    ws.Cells["D" + j].Value = totalPatrimonioActual.ToString("N0", formato);
                                    ws.Cells["E" + j].Value = totalPatrimonioAnterior.ToString("N0", formato);
                                    superPatrimonioActual += totalPatrimonioActual;
                                    superPatrimonioAnterior += totalPatrimonioAnterior;
                                    j++;
                                }
                            }
                            j++;
                            ws.Cells["C" + j].Value = "TOTAL PATRIMONIO";
                            ws.Cells["D" + j].Value = superPatrimonioActual.ToString("N0", formato);
                            ws.Cells["E" + j].Value = superPatrimonioAnterior.ToString("N0", formato);
                            j++;

                            ws.Cells["C" + j].Value = "TOTAL PASIVO Y PATRIMONIO";
                            ws.Cells["D" + j].Value = (superTotalActual + superPatrimonioActual).ToString("N0", formato);
                            ws.Cells["E" + j].Value = (superTotalAnterior + superPatrimonioAnterior).ToString("N0", formato);

                        }



                    }


                    ws.Cells[ws.Dimension.Address].AutoFitColumns();//siempre al final de todo. le da tamaño ajustado a cada columna


                }

                else if (informe == 5)
                {
                    var ingresos = saldos.Where(s => s.sal.CODIGO.StartsWith("11") && s.sal.SALDO != 0).GroupBy(s => new
                    {
                        s.sal.CODIGO,
                        s.cuentas.NOMBRE,
                        s.sal.MES,
                        s.sal.ANO
                    }).Select(g => new { codigo = g.Key.CODIGO, nombre = g.Key.NOMBRE, mes = g.Key.MES, año = g.Key.ANO, saldo = g.Sum(s => s.sal.SALDO) }).ToList();

                    var ingr = saldos.Where(s => s.sal.CODIGO.StartsWith("4") && s.sal.SALDO != 0).GroupBy(s => new
                    {
                        s.sal.CODIGO,
                        s.cuentas.NOMBRE,
                        s.sal.MES,
                        s.sal.ANO
                    }).Select(g => new { codigo = g.Key.CODIGO, nombre = g.Key.NOMBRE, mes = g.Key.MES, año = g.Key.ANO, saldo = g.Sum(s => s.sal.SALDO) }).ToList();

                    var gastos = saldos.Where(s => s.sal.CODIGO.StartsWith("5") && s.sal.SALDO != 0).GroupBy(s => new
                    {
                        s.sal.CODIGO,
                        s.cuentas.NOMBRE,
                        s.sal.MES,
                        s.sal.ANO
                    }).Select(g => new { codigo = g.Key.CODIGO, nombre = g.Key.NOMBRE, mes = g.Key.MES, año = g.Key.ANO, saldo = g.Sum(s => s.sal.SALDO) }).ToList();

                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("flujodecaja");
                    ws.Cells["A" + 1].Value = "CUENTA_ING";
                    ws.Cells["B" + 1].Value = "NOM_ING";
                    ws.Cells["C" + 1].Value = "SAL_ING";
                    ws.Cells["D" + 1].Value = "MES";
                    ws.Cells["E" + 1].Value = "AÑO";
                    int i = 2;
                    decimal saldo_ant = 0;
                    decimal saldo_act = 0;
                    decimal ing_tot = 0;
                    decimal ingr_tot = 0;
                    decimal gas_tot = 0;
                    foreach (var item in ingresos)
                    {
                        if (desdeSaldo == 0) saldo_ant = GetSaldos(desdeSaldoAno, desdeSaldo, item.codigo, documento);
                        else saldo_ant = 0;

                        saldo_act = item.saldo + saldo_ant;

                        ws.Cells["A" + i].Value = item.codigo;
                        ws.Cells["B" + i].Value = item.nombre;
                        ws.Cells["C" + i].Value = saldo_act;
                        ws.Cells["D" + i].Value = item.mes;
                        ws.Cells["E" + i].Value = item.año;

                        ing_tot += saldo_act;
                        i++;
                    }

                    i++;
                    ws.Cells["B" + i].Value = "Total Ingresos Caja vs Bancos";
                    ws.Cells["C" + i].Value = ing_tot;
                    i = i + 2;

                    foreach (var item in ingr)
                    {
                        if (desdeSaldo == 0) saldo_ant = GetSaldos(desdeSaldoAno, desdeSaldo, item.codigo, documento);
                        else saldo_ant = 0;

                        saldo_act = item.saldo + saldo_ant;

                        ws.Cells["A" + i].Value = item.codigo;
                        ws.Cells["B" + i].Value = item.nombre;
                        ws.Cells["C" + i].Value = saldo_act;
                        ws.Cells["D" + i].Value = item.mes;
                        ws.Cells["E" + i].Value = item.año;

                        ingr_tot += saldo_act;
                        i++;
                    }

                    i++;
                    ws.Cells["B" + i].Value = "Total Ingresos";
                    ws.Cells["C" + i].Value = ingr_tot;
                    i = i + 2;

                    foreach (var item in gastos)
                    {
                        if (desdeSaldo == 0) saldo_ant = GetSaldos(desdeSaldoAno, desdeSaldo, item.codigo, documento);
                        else saldo_ant = 0;

                        saldo_act = item.saldo + saldo_ant;

                        ws.Cells["A" + i].Value = item.codigo;
                        ws.Cells["B" + i].Value = item.nombre;
                        ws.Cells["C" + i].Value = saldo_act;
                        ws.Cells["D" + i].Value = item.mes;
                        ws.Cells["E" + i].Value = item.año;

                        gas_tot += saldo_act;
                        i++;
                    }

                    i++;


                    ws.Cells["B" + i].Value = "Total Gastos";
                    ws.Cells["C" + i].Value = gas_tot;

                    i = i + 2;

                    ws.Cells["B" + i].Value = "Saldo por Flujo de Efectivo";
                    ws.Cells["C" + i].Value = ing_tot + ingr_tot - gas_tot;

                }

                else if (informe == 11)
                {
                    var ingresos = saldos.Where(s => s.sal.CODIGO.StartsWith("11") && s.sal.SALDO != 0).GroupBy(s => new
                    {
                        s.sal.CODIGO,
                        s.cuentas.CTANIIF,
                        s.sal.MES,
                        s.sal.ANO
                    }).Select(g => new { codigo = g.Key.CODIGO, nombre = g.Key.CTANIIF, mes = g.Key.MES, año = g.Key.ANO, saldo = g.Sum(s => s.sal.SALDO) }).ToList();

                    {
                        var hingresos = Hsaldos.Where(s => s.Hsal.CODIGO.StartsWith("11") && s.Hsal.SALDO != 0).GroupBy(s => new
                        {
                            s.Hsal.CODIGO,
                            s.cuentas.CTANIIF,
                            s.Hsal.MES,
                            s.Hsal.ANO
                        }).Select(g => new { codigo = g.Key.CODIGO, nombre = g.Key.CTANIIF, mes = g.Key.MES, año = g.Key.ANO, saldo = g.Sum(s => s.Hsal.SALDO) }).ToList();


                        var ingr = saldos.Where(s => s.sal.CODIGO.StartsWith("4") && s.sal.SALDO != 0).GroupBy(s => new
                        {
                            s.sal.CODIGO,
                            s.cuentas.CTANIIF,
                            s.sal.MES,
                            s.sal.ANO
                        }).Select(g => new { codigo = g.Key.CODIGO, nombre = g.Key.CTANIIF, mes = g.Key.MES, año = g.Key.ANO, saldo = g.Sum(s => s.sal.SALDO) }).ToList();

                        var hingr = Hsaldos.Where(s => s.Hsal.CODIGO.StartsWith("4") && s.Hsal.SALDO != 0).GroupBy(s => new
                        {
                            s.Hsal.CODIGO,
                            s.cuentas.CTANIIF,
                            s.Hsal.MES,
                            s.Hsal.ANO
                        }).Select(g => new { codigo = g.Key.CODIGO, nombre = g.Key.CTANIIF, mes = g.Key.MES, año = g.Key.ANO, saldo = g.Sum(s => s.Hsal.SALDO) }).ToList();

                        ExcelWorksheet ws = pack.Workbook.Worksheets.Add("EstadodeFlujodeEfectivo");
                        ws.Cells["A" + 1].Value = "";
                        ws.Cells["B" + 1].Value = "NOM_ING";
                        ws.Cells["C" + 1].Value = "SAL_ING";
                        ws.Cells["D" + 1].Value = "MES";
                        ws.Cells["E" + 1].Value = "AÑO";
                        int i = 2;
                        int b = 2;

                        decimal saldo_ant = 0;
                        decimal saldo_act = 0;
                        decimal ing_tot = 0;
                        decimal hing_tot = 0;
                        decimal ingr_tot = 0;
                        decimal hingr_tot = 0;
                        decimal gas_tot = 0;
                        foreach (var item in ingresos)
                        {
                            if (desdeSaldo == 0) saldo_ant = GetSaldos(desdeSaldoAno, desdeSaldo, item.codigo, documento);
                            else saldo_ant = 0;

                            saldo_act = item.saldo + saldo_ant;

                            //ws.Cells["A" + i].Value = item.codigo;
                            ws.Cells["B" + i].Value = item.nombre;
                            ws.Cells["C" + i].Value = saldo_act;
                            ws.Cells["D" + i].Value = item.mes;
                            ws.Cells["E" + i].Value = item.año;

                            ing_tot += saldo_act;
                            i++;
                        }

                        i++;
                        ws.Cells["B" + i].Value = "Total Ingresos Caja vs Bancos";
                        ws.Cells["C" + i].Value = ing_tot;
                        i = i + 2;

                        foreach (var item in hingresos)
                        {
                            if (desdeSaldo == 0) saldo_ant = GetSaldos(desdeSaldoAno, desdeSaldo, item.codigo, documento);
                            else saldo_ant = 0;

                            saldo_act = item.saldo + saldo_ant;

                            //ws.Cells["A" + i].Value = item.codigo;
                            ws.Cells["G" + b].Value = item.nombre;
                            ws.Cells["H" + b].Value = saldo_act;
                            ws.Cells["I" + b].Value = item.mes;
                            ws.Cells["J" + b].Value = item.año;

                            hing_tot += saldo_act;
                            b++;
                        }

                        b++;
                        ws.Cells["G" + b].Value = "Total Ingresos Caja vs Bancos";
                        ws.Cells["H" + b].Value = hing_tot;
                        b = b + 2;

                        foreach (var item in ingr)
                        {
                            if (desdeSaldo == 0) saldo_ant = GetSaldos(desdeSaldoAno, desdeSaldo, item.codigo, documento);
                            else saldo_ant = 0;

                            saldo_act = item.saldo + saldo_ant;

                            //ws.Cells["A" + i].Value = item.codigo;
                            ws.Cells["B" + i].Value = item.nombre;
                            ws.Cells["C" + i].Value = saldo_act;
                            ws.Cells["D" + i].Value = item.mes;
                            ws.Cells["E" + i].Value = item.año;

                            ingr_tot += saldo_act;
                            i++;
                        }

                        i++;
                        ws.Cells["B" + i].Value = "Flujo de Efectivo en Actividades de Operacion";
                        ws.Cells["C" + i].Value = ingr_tot;
                        i = i + 2;



                        ws.Cells["B" + i].Value = "Saldo por Flujo de Efectivo";
                        ws.Cells["C" + i].Value = ing_tot + ingr_tot - gas_tot;

                        foreach (var item in hingr)
                        {
                            if (desdeSaldo == 0) saldo_ant = GetSaldos(desdeSaldoAno, desdeSaldo, item.codigo, documento);
                            else saldo_ant = 0;

                            saldo_act = item.saldo + saldo_ant;

                            //ws.Cells["A" + i].Value = item.codigo;
                            ws.Cells["G" + b].Value = item.nombre;
                            ws.Cells["H" + b].Value = saldo_act;
                            ws.Cells["I" + b].Value = item.mes;
                            ws.Cells["J" + b].Value = item.año;

                            hingr_tot += saldo_act;
                            b++;
                        }

                        b++;
                        ws.Cells["G" + b].Value = "Flujo de Efectivo en Actividades de Operacion";
                        ws.Cells["H" + b].Value = hingr_tot;
                        b = b + 2;



                        ws.Cells["G" + b].Value = "Saldo por Flujo de Efectivo";
                        ws.Cells["H" + b].Value = hing_tot + hingr_tot - gas_tot;

                    }
                }

                else if (informe == 14)
                {
                    List<Movimiento> movtos = new List<Movimiento>();
                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("Estado de Resultados Integral");
                    ws.Cells["A" + 2].Value = "COOPERATIVA DE APORTE Y CRÉDITO ASOPASCUALINOS";
                    ws.Cells["A" + 3].Value = "ESTADO DE RESULTADOS INTEGRAL";

                    ws.Cells["A2:C2"].Merge = true;//une columnas en una fila
                    ws.Cells["A3:C3"].Merge = true;

                    var fDesde = coll["fechaDesde"];
                    var fHasta = coll["fechaHasta"];

                    if (fDesde != "" && fHasta != "")
                    {
                        DateTime fh = Convert.ToDateTime(fHasta);
                        DateTime fd = Convert.ToDateTime(fDesde);
                        DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                        DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                        movtos = db.Movimientos.Where(x => (x.FECHAMOVIMIENTO >= fechDesde && x.FECHAMOVIMIENTO <= fechHasta) && x.Comprobante.ANULADO == false).ToList();
                    }
                    else if (fDesde != "" && fHasta == "")
                    {
                        DateTime fd = Convert.ToDateTime(fDesde);
                        DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                        movtos = db.Movimientos.Where(x => x.FECHAMOVIMIENTO >= fechDesde && x.Comprobante.ANULADO == false).ToList();
                    }
                    else if (fDesde == "" && fHasta != "")
                    {
                        DateTime fh = Convert.ToDateTime(fHasta);
                        DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                        movtos = db.Movimientos.Where(x => x.FECHAMOVIMIENTO <= fechHasta && x.Comprobante.ANULADO == false).ToList();
                    }

                    if (movtos.Count > 0)
                    {
                        var agencia = coll["agencia"];
                        var tipoInforme = coll["typeInforme"];
                        decimal credito = 0, debito = 0, totalIngresos = 0, totalGastos = 0, totGastosOrdinarios = 0, totOtrosGastos = 0, totOtros = 0, totalCostos = 0;
                        if (agencia != "")
                        {
                            int agency = Convert.ToInt32(agencia);
                            movtos = movtos.Where(x => x.terceroFK.DEPENDENCIA == agency).ToList();
                        }

                        if (tipoInforme != "")
                        {
                            int tipoinforme = Convert.ToInt32(tipoInforme);
                            if (tipoinforme == 1)
                            {
                                ws.Cells["B" + 5].Value = "INGRESOS ORDINARIOS";
                                ws.Cells["C" + 5].Value = "SALDO";

                                int j = 7;

                                //ingresos

                                var ingresos = movtos.Where(x => x.CUENTA.StartsWith("4")).ToList();
                                var cicloIngresos = (from ing in ingresos
                                                     orderby ing.CUENTA
                                                     select new { ing.CUENTA, ing.cuentaFK }
                                                     ).Distinct().ToList();
                                foreach (var item in cicloIngresos)
                                {
                                    var data = ingresos.Where(x => x.CUENTA == item.CUENTA).ToList();
                                    credito = data.Select(x => x.CREDITO).Sum();
                                    debito = data.Select(x => x.DEBITO).Sum();
                                    totalIngresos += credito - debito;

                                    ws.Cells["B" + j].Value = item.cuentaFK.NOMBRE;
                                    ws.Cells["C" + j].Value = (credito - debito).ToString("N0", formato);
                                    j++;
                                }
                                j++;
                                ws.Cells["B" + j].Value = "Total Ingresos Ordinarios";
                                ws.Cells["C" + j].Value = totalIngresos.ToString("N0", formato);

                                j += 2;

                                //gastos
                                ws.Cells["B" + j].Value = "GASTOS ORDINARIOS";
                                j += 2;
                                var gastos = movtos.Where(x => x.CUENTA.StartsWith("51")).ToList();
                                var cicloGastos = (from ing in gastos
                                                   orderby ing.CUENTA
                                                   select new { ing.CUENTA, ing.cuentaFK }
                                                     ).Distinct().ToList();

                                foreach (var item in cicloGastos)
                                {
                                    var data = gastos.Where(x => x.CUENTA == item.CUENTA).ToList();
                                    credito = data.Select(x => x.CREDITO).Sum();
                                    debito = data.Select(x => x.DEBITO).Sum();
                                    totGastosOrdinarios += debito - credito;

                                    ws.Cells["B" + j].Value = item.cuentaFK.NOMBRE;
                                    ws.Cells["C" + j].Value = (debito - credito).ToString("N0", formato);
                                    j++;
                                }
                                j++;
                                ws.Cells["B" + j].Value = "Total Gastos Ordinarios";
                                ws.Cells["C" + j].Value = totGastosOrdinarios.ToString("N0", formato);

                                j += 2;

                                ws.Cells["B" + j].Value = "OTROS GASTOS";
                                j += 2;
                                gastos = movtos.Where(x => x.CUENTA.StartsWith("52")).ToList();
                                cicloGastos = (from ing in gastos
                                               orderby ing.CUENTA
                                               select new { ing.CUENTA, ing.cuentaFK }
                                                     ).Distinct().ToList();

                                foreach (var item in cicloGastos)
                                {
                                    var data = gastos.Where(x => x.CUENTA == item.CUENTA).ToList();
                                    credito = data.Select(x => x.CREDITO).Sum();
                                    debito = data.Select(x => x.DEBITO).Sum();
                                    totOtrosGastos += debito - credito;

                                    ws.Cells["B" + j].Value = item.cuentaFK.NOMBRE;
                                    ws.Cells["C" + j].Value = (debito - credito).ToString("N0", formato);
                                    j++;
                                }
                                j++;
                                ws.Cells["B" + j].Value = "Total Otros Gastos";
                                ws.Cells["C" + j].Value = totOtrosGastos.ToString("N0", formato);

                                j += 2;

                                ws.Cells["B" + j].Value = "OTROS";
                                j += 2;
                                gastos = movtos.Where(x => x.CUENTA.StartsWith("53") || x.CUENTA.StartsWith("54") || x.CUENTA.StartsWith("55") || x.CUENTA.StartsWith("56") || x.CUENTA.StartsWith("57") || x.CUENTA.StartsWith("58") || x.CUENTA.StartsWith("59")).ToList();
                                cicloGastos = (from ing in gastos
                                               orderby ing.CUENTA
                                               select new { ing.CUENTA, ing.cuentaFK }
                                                     ).Distinct().ToList();

                                foreach (var item in cicloGastos)
                                {
                                    var data = gastos.Where(x => x.CUENTA == item.CUENTA).ToList();
                                    credito = data.Select(x => x.CREDITO).Sum();
                                    debito = data.Select(x => x.DEBITO).Sum();
                                    totOtros += debito - credito;

                                    ws.Cells["B" + j].Value = item.cuentaFK.NOMBRE;
                                    ws.Cells["C" + j].Value = (debito - credito).ToString("N0", formato);
                                    j++;
                                }
                                j++;
                                ws.Cells["B" + j].Value = "Total Otros";
                                ws.Cells["C" + j].Value = totOtros.ToString("N0", formato);

                                j += 2;
                                totalGastos = totGastosOrdinarios + totOtrosGastos + totOtros;
                                ws.Cells["B" + j].Value = "Total Gastos";
                                ws.Cells["C" + j].Value = totalGastos.ToString("N0", formato);

                                j += 2;

                                //costos
                                ws.Cells["B" + j].Value = "COSTOS";
                                j += 2;
                                var costos = movtos.Where(x => x.CUENTA.StartsWith("6")).ToList();
                                var cicloCostos = (from ctos in costos
                                                   orderby ctos.CUENTA
                                                   select new { ctos.CUENTA, ctos.cuentaFK }
                                                     ).Distinct().ToList();
                                foreach (var item in cicloCostos)
                                {
                                    var data = costos.Where(x => x.CUENTA == item.CUENTA).ToList();
                                    credito = data.Select(x => x.CREDITO).Sum();
                                    debito = data.Select(x => x.DEBITO).Sum();
                                    totalCostos += debito - credito;

                                    ws.Cells["B" + j].Value = item.cuentaFK.NOMBRE;
                                    ws.Cells["C" + j].Value = (debito - credito).ToString("N0", formato);
                                    j++;
                                }
                                j++;
                                ws.Cells["B" + j].Value = "Total Costos";
                                ws.Cells["C" + j].Value = totalCostos.ToString("N0", formato);

                                j += 2;

                                ws.Cells["B" + j].Value = "Utilidad del Ejercicio";
                                ws.Cells["C" + j].Value = (totalIngresos - totalGastos - totalCostos).ToString("N0", formato);
                            }//tipo informe == 1
                            else
                            {
                                var ingresos = movtos.Where(x => x.CUENTA.StartsWith("4")).ToList();
                                var gastos = movtos.Where(x => x.CUENTA.StartsWith("51")).ToList();
                                var otrosGastos = movtos.Where(x => x.CUENTA.StartsWith("52")).ToList();
                                var otros = movtos.Where(x => x.CUENTA.StartsWith("53") || x.CUENTA.StartsWith("54") || x.CUENTA.StartsWith("55") || x.CUENTA.StartsWith("56") || x.CUENTA.StartsWith("57") || x.CUENTA.StartsWith("58") || x.CUENTA.StartsWith("59")).ToList();
                                var costos = movtos.Where(x => x.CUENTA.StartsWith("6")).ToList();

                                credito = ingresos.Select(x => x.CREDITO).Sum();
                                debito = ingresos.Select(x => x.DEBITO).Sum();
                                totalIngresos = credito - debito;
                                ws.Cells["B" + 5].Value = "INGRESOS ORDINARIOS";
                                ws.Cells["C" + 5].Value = totalIngresos.ToString("N0", formato);

                                credito = gastos.Select(x => x.CREDITO).Sum();
                                debito = gastos.Select(x => x.DEBITO).Sum();
                                totGastosOrdinarios = debito - credito;
                                ws.Cells["B" + 6].Value = "GASTOS ORDINARIOS";
                                ws.Cells["C" + 6].Value = totGastosOrdinarios.ToString("N0", formato);

                                credito = otrosGastos.Select(x => x.CREDITO).Sum();
                                debito = otrosGastos.Select(x => x.DEBITO).Sum();
                                totOtrosGastos = debito - credito;
                                ws.Cells["B" + 7].Value = "OTROS GASTOS";
                                ws.Cells["C" + 7].Value = totOtrosGastos.ToString("N0", formato);

                                credito = otros.Select(x => x.CREDITO).Sum();
                                debito = otros.Select(x => x.DEBITO).Sum();
                                totOtros = debito - credito;
                                ws.Cells["B" + 8].Value = "OTROS";
                                ws.Cells["C" + 8].Value = totOtros.ToString("N0", formato);

                                totalGastos = totGastosOrdinarios + totOtrosGastos + totOtros;

                                //costos
                                credito = costos.Select(x => x.CREDITO).Sum();
                                debito = costos.Select(x => x.DEBITO).Sum();
                                totalCostos = debito - credito;
                                ws.Cells["B" + 9].Value = "COSTOS";
                                ws.Cells["C" + 9].Value = totalCostos.ToString("N0", formato);

                                ws.Cells["B" + 11].Value = "UTILIDAD DEL EJERCICIO";
                                ws.Cells["C" + 11].Value = (totalIngresos - totalGastos - totalCostos).ToString("N0", formato);
                            }

                            ws.Cells[ws.Dimension.Address].AutoFitColumns();//siempre al final de todo. le da tamaño ajustado a cada columna
                        }

                    }

                }
                else if (informe == 7)
                {
                    var movimiento = db.Movimientos.Where(x => x.Comprobante.ANULADO == false).ToList();
                    var movimiento2 = movimiento;
                    var fechaDesde = coll["fechaDesde"];
                    var fechaHasta = coll["fechaHasta"];
                    string periodo = "";
                    if (fechaDesde != "")
                    {
                        DateTime auxfd = Convert.ToDateTime(fechaDesde);
                        DateTime fd = new DateTime(auxfd.Year, auxfd.Month, auxfd.Day, 0, 0, 0);
                        movimiento2 = movimiento2.Where(x => x.FECHAMOVIMIENTO < fd).ToList();
                        periodo = fechaDesde;
                        if (fechaHasta != "")
                        {
                            DateTime auxfh = Convert.ToDateTime(fechaHasta);
                            DateTime fh = new DateTime(auxfh.Year, auxfh.Month, auxfh.Day, 23, 59, 59);
                            movimiento = movimiento.Where(X => X.FECHAMOVIMIENTO >= fd && X.FECHAMOVIMIENTO <= fh).ToList();
                            periodo += " - " + fechaHasta;
                        }
                        else
                        {

                            movimiento = movimiento.Where(X => X.FECHAMOVIMIENTO.Year == fd.Year && X.FECHAMOVIMIENTO.Month == fd.Month && X.FECHAMOVIMIENTO.Day == fd.Day).ToList();
                        }
                    }

                    var mov = (from m in movimiento
                               orderby m.CUENTA
                               select new { m.CUENTA, m.cuentaFK }).Distinct().ToList();

                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("Libro Mayor y Balances");
                    ws.Cells["A" + 1].Value = "LIBRO MAYOR Y BALANCES";
                    ws.Cells["A" + 2].Value = periodo;

                    ws.Cells["A" + 4].Value = "CÓDIGO";
                    ws.Cells["B" + 4].Value = "DETALLE";
                    ws.Cells["C" + 4].Value = "SALDO INICIAL";
                    ws.Cells["D" + 4].Value = "DEBE";
                    ws.Cells["E" + 4].Value = "HABER";
                    ws.Cells["F" + 4].Value = "SALDO FINAL";

                    int j = 5;
                    decimal debito = 0, credito = 0, debitoAnterior = 0, creditoAnterior = 0, totalDebito = 0, totalCredito = 0, saldo = 0, saldoAnterior = 0;
                    foreach (var item in mov)
                    {

                        var data = movimiento.Where(x => x.CUENTA == item.CUENTA).ToList();
                        var data2 = movimiento2.Where(x => x.CUENTA == item.CUENTA).ToList();
                        debito = data.Select(x => x.DEBITO).Sum();
                        credito = data.Select(x => x.CREDITO).Sum();
                        debitoAnterior = data2.Select(x => x.DEBITO).Sum();
                        creditoAnterior = data2.Select(x => x.CREDITO).Sum();
                        if (item.cuentaFK.NATURALEZA == "D")
                        {
                            saldoAnterior = debitoAnterior - creditoAnterior;
                            saldo = (debito - credito) + saldoAnterior;
                        }
                        else
                        {
                            saldoAnterior = creditoAnterior - debitoAnterior;
                            saldo = (credito - debito) + saldoAnterior;
                        }

                        totalCredito += credito;
                        totalDebito += debito;

                        ws.Cells["A" + j].Value = item.CUENTA;
                        ws.Cells["B" + j].Value = item.cuentaFK.NOMBRE;
                        ws.Cells["C" + j].Value = saldoAnterior.ToString("N0", formato);
                        ws.Cells["D" + j].Value = debito.ToString("N0", formato);
                        ws.Cells["E" + j].Value = credito.ToString("N0", formato);
                        ws.Cells["F" + j].Value = saldo.ToString("N0", formato);

                        j++;
                    }
                    j++;
                    ws.Cells["C" + j].Value = "TOTAL";
                    ws.Cells["D" + j].Value = totalDebito.ToString("N0", formato);
                    ws.Cells["E" + j].Value = totalCredito.ToString("N0", formato);
                }
                else if (informe == 6)
                {

                    //var movimiento = db.Movimientos.Where(x => x.Comprobante.ANULADO == false).ToList();
                    var fDesde = coll["fechaDesde"];
                    var fHasta = coll["fechaHasta"];
                    //if(fechaDesde != "")
                    //{
                    //    DateTime auxfd = Convert.ToDateTime(fechaDesde);
                    //    DateTime fd = new DateTime(auxfd.Year, auxfd.Month, auxfd.Day, 0, 0, 0);
                    //    movimiento = movimiento.Where(X => X.FECHAMOVIMIENTO.Year == fd.Year && X.FECHAMOVIMIENTO.Month == fd.Month && X.FECHAMOVIMIENTO.Day == fd.Day).ToList();
                    //    if(fechaHasta != "")
                    //    {
                    //        DateTime auxfh = Convert.ToDateTime(fechaHasta);
                    //        DateTime fh = new DateTime(auxfh.Year, auxfh.Month, auxfh.Day, 23, 59, 59);
                    //        movimiento = movimiento.Where(X => X.FECHAMOVIMIENTO >= fd && X.FECHAMOVIMIENTO <= fh).ToList();
                    //    }
                    //}

                    List<Movimiento> movtos = new List<Movimiento>();

                    if (fDesde != "" && fHasta != "")
                    {
                        DateTime fh = Convert.ToDateTime(fHasta);
                        DateTime fd = Convert.ToDateTime(fDesde);
                        DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                        DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                        movtos = db.Movimientos.Where(x => (x.FECHAMOVIMIENTO >= fechDesde && x.FECHAMOVIMIENTO <= fechHasta) && x.Comprobante.ANULADO == false).ToList();
                    }
                    else if (fDesde != "" && fHasta == "")
                    {
                        DateTime fd = Convert.ToDateTime(fDesde);
                        DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                        movtos = db.Movimientos.Where(x => x.FECHAMOVIMIENTO >= fechDesde && x.Comprobante.ANULADO == false).ToList();
                    }
                    else if (fDesde == "" && fHasta != "")
                    {
                        DateTime fh = Convert.ToDateTime(fHasta);
                        DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                        movtos = db.Movimientos.Where(x => x.FECHAMOVIMIENTO <= fechHasta && x.Comprobante.ANULADO == false).ToList();
                    }

                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("librodiario");
                    ws.Cells["A" + 1].Value = "TIPO";
                    ws.Cells["B" + 1].Value = "NUMERO";
                    ws.Cells["C" + 1].Value = "FECHA";
                    ws.Cells["D" + 1].Value = "CUENTA";
                    ws.Cells["E" + 1].Value = "TERCERO";
                    ws.Cells["F" + 1].Value = "NOMBRE TERCERO";
                    ws.Cells["G" + 1].Value = "DETALLE";
                    ws.Cells["H" + 1].Value = "DEBITO";
                    ws.Cells["I" + 1].Value = "CREDITO";
                    ws.Cells["J" + 1].Value = "CCOSTO";
                    ws.Cells["K" + 1].Value = "BASE";

                    if (movtos.Count > 0)
                    {
                        int i = 2;

                        var tupla = movtos.FirstOrDefault();
                        string tipo = tupla.TIPO;
                        string numero = tupla.NUMERO;
                        decimal debito = 0;
                        decimal credito = 0;

                        foreach (var item in movtos)
                        {
                            if (item.TIPO == tipo && item.NUMERO == numero)
                            {
                                debito += item.DEBITO;
                                credito += item.CREDITO;
                            }
                            else
                            {
                                ws.Cells["G" + i].Value = "TOTAL";
                                ws.Cells["H" + i].Value = debito.ToString("N0", formato);
                                ws.Cells["I" + i].Value = credito.ToString("N0", formato);
                                debito = item.DEBITO;
                                credito = item.CREDITO;
                                tipo = item.TIPO;
                                numero = item.NUMERO;
                                i += 2;
                            }

                            ws.Cells["A" + i].Value = item.TIPO;
                            ws.Cells["B" + i].Value = item.NUMERO;
                            ws.Cells["C" + i].Value = item.FECHAMOVIMIENTO.ToString("yyyy-MM-dd");
                            ws.Cells["D" + i].Value = item.CUENTA;
                            ws.Cells["E" + i].Value = item.TERCERO;
                            ws.Cells["F" + i].Value = item.terceroFK.NOMBRE1 + " " + item.terceroFK.NOMBRE2 + " " + item.terceroFK.APELLIDO1 + " " + item.terceroFK.APELLIDO2;
                            ws.Cells["G" + i].Value = item.DETALLE;
                            ws.Cells["H" + i].Value = item.DEBITO.ToString("N0", formato);
                            ws.Cells["I" + i].Value = item.CREDITO.ToString("N0", formato);
                            ws.Cells["J" + i].Value = item.CCOSTO;
                            ws.Cells["K" + i].Value = item.BASE;

                            i++;


                        }
                    }


                }

                else if (informe == 3)
                {

                    List<Movimiento> movtos = new List<Movimiento>();
                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("Estado de Resultados");
                    ws.Cells["A" + 2].Value = "COOPERATIVA DE APORTE Y CRÉDITO ASOPASCUALINOS";
                    ws.Cells["A" + 3].Value = "ESTADO DE RESULTADOS";

                    ws.Cells["A2:C2"].Merge = true;//une columnas en una fila
                    ws.Cells["A3:C3"].Merge = true;

                    var fDesde = coll["fechaDesde"];
                    var fHasta = coll["fechaHasta"];

                    if (fDesde != "" && fHasta != "")
                    {
                        DateTime fh = Convert.ToDateTime(fHasta);
                        DateTime fd = Convert.ToDateTime(fDesde);
                        DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                        DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                        movtos = db.Movimientos.Where(x => (x.FECHAMOVIMIENTO >= fechDesde && x.FECHAMOVIMIENTO <= fechHasta) && x.Comprobante.ANULADO == false).ToList();
                    }
                    else if (fDesde != "" && fHasta == "")
                    {
                        DateTime fd = Convert.ToDateTime(fDesde);
                        DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                        movtos = db.Movimientos.Where(x => x.FECHAMOVIMIENTO >= fechDesde && x.Comprobante.ANULADO == false).ToList();
                    }
                    else if (fDesde == "" && fHasta != "")
                    {
                        DateTime fh = Convert.ToDateTime(fHasta);
                        DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                        movtos = db.Movimientos.Where(x => x.FECHAMOVIMIENTO <= fechHasta && x.Comprobante.ANULADO == false).ToList();
                    }

                    if (movtos.Count > 0)
                    {
                        var agencia = coll["agencia"];
                        decimal credito = 0, debito = 0, totalIngresos = 0, totalGastos = 0, totGastosOrdinarios = 0, totOtrosGastos = 0, totOtros = 0, totalCostos = 0;
                        if (agencia != "")
                        {
                            int agency = Convert.ToInt32(agencia);
                            movtos = movtos.Where(x => x.terceroFK.DEPENDENCIA == agency).ToList();
                        }

                        ws.Cells["A" + 5].Value = "CUENTA";
                        ws.Cells["B" + 5].Value = "INGRESOS";
                        ws.Cells["C" + 5].Value = "SALDO";

                        int j = 7;

                        //ingresos

                        var ingresos = movtos.Where(x => x.CUENTA.StartsWith("4")).ToList();
                        var cicloIngresos = (from ing in ingresos
                                             orderby ing.CUENTA
                                             select new { ing.CUENTA, ing.cuentaFK }
                                             ).Distinct().ToList();
                        foreach (var item in cicloIngresos)
                        {
                            var data = ingresos.Where(x => x.CUENTA == item.CUENTA).ToList();
                            credito = data.Select(x => x.CREDITO).Sum();
                            debito = data.Select(x => x.DEBITO).Sum();
                            totalIngresos += credito - debito;
                            ws.Cells["A" + j].Value = item.CUENTA;
                            ws.Cells["B" + j].Value = item.cuentaFK.NOMBRE;
                            ws.Cells["C" + j].Value = (credito - debito).ToString("N0", formato);
                            j++;
                        }
                        j++;
                        ws.Cells["B" + j].Value = "Total Ingresos Ordinarios";
                        ws.Cells["C" + j].Value = totalIngresos.ToString("N0", formato);

                        j += 2;

                        //gastos
                        ws.Cells["B" + j].Value = "GASTOS";
                        j += 2;
                        var gastos = movtos.Where(x => x.CUENTA.StartsWith("5")).ToList();
                        var cicloGastos = (from ing in gastos
                                           orderby ing.CUENTA
                                           select new { ing.CUENTA, ing.cuentaFK }
                                             ).Distinct().ToList();

                        foreach (var item in cicloGastos)
                        {
                            var data = gastos.Where(x => x.CUENTA == item.CUENTA).ToList();
                            credito = data.Select(x => x.CREDITO).Sum();
                            debito = data.Select(x => x.DEBITO).Sum();
                            totalGastos += debito - credito;
                            ws.Cells["A" + j].Value = item.CUENTA;
                            ws.Cells["B" + j].Value = item.cuentaFK.NOMBRE;
                            ws.Cells["C" + j].Value = (debito - credito).ToString("N0", formato);
                            j++;
                        }
                        j++;
                        ws.Cells["B" + j].Value = "Total Gastos";
                        ws.Cells["C" + j].Value = totalGastos.ToString("N0", formato);



                        j += 2;

                        //costos
                        ws.Cells["B" + j].Value = "COSTOS";
                        j += 2;
                        var costos = movtos.Where(x => x.CUENTA.StartsWith("6")).ToList();
                        var cicloCostos = (from ctos in costos
                                           orderby ctos.CUENTA
                                           select new { ctos.CUENTA, ctos.cuentaFK }
                                             ).Distinct().ToList();
                        foreach (var item in cicloCostos)
                        {
                            var data = costos.Where(x => x.CUENTA == item.CUENTA).ToList();
                            credito = data.Select(x => x.CREDITO).Sum();
                            debito = data.Select(x => x.DEBITO).Sum();
                            totalCostos += debito - credito;

                            ws.Cells["A" + j].Value = item.CUENTA;
                            ws.Cells["B" + j].Value = item.cuentaFK.NOMBRE;
                            ws.Cells["C" + j].Value = (debito - credito).ToString("N0", formato);
                            j++;
                        }
                        j++;
                        ws.Cells["B" + j].Value = "Total Costos";
                        ws.Cells["C" + j].Value = totalCostos.ToString("N0", formato);

                        j += 2;

                        ws.Cells["B" + j].Value = "Utilidad del Ejercicio";
                        ws.Cells["C" + j].Value = (totalIngresos - totalGastos - totalCostos).ToString("N0", formato);


                        ws.Cells[ws.Dimension.Address].AutoFitColumns();//siempre al final de todo. le da tamaño ajustado a cada columna


                    }


                }

                else if (informe == 13)
                {
                    var patrimonio = saldos.Where(s => s.sal.CODIGO.StartsWith("3") && s.sal.SALDO != 0).GroupBy(s => new { s.sal.CODIGO, s.cuentas.CTANIIF, s.sal.MES, s.sal.ANO, s.sal.SALDO }).Select(a => new
                    {
                        cuenta = a.Key.CODIGO,
                        nombre = a.Key.CTANIIF,
                        saldo = a.Key.SALDO,
                        mes = a.Key.MES,
                        año = a.Key.ANO
                    }).OrderBy(a => a.cuenta).ToList();
                    {
                        var hpatrimonio = Hsaldos.Where(s => s.Hsal.CODIGO.StartsWith("3") && s.Hsal.SALDO != 0).GroupBy(s => new { s.Hsal.CODIGO, s.cuentas.CTANIIF, s.Hsal.MES, s.Hsal.ANO, s.Hsal.SALDO }).Select(a => new
                        {
                            cuenta = a.Key.CODIGO,
                            nombre = a.Key.CTANIIF,
                            saldo = a.Key.SALDO,
                            mes = a.Key.MES,
                            año = a.Key.ANO
                        }).OrderBy(a => a.cuenta).ToList();

                        ExcelWorksheet ws = pack.Workbook.Worksheets.Add("CambiosenelPatrimonio");
                        ws.Cells["B" + 1].Value = "";
                        ws.Cells["C" + 1].Value = "SALDO";
                        ws.Cells["D" + 1].Value = "MES";
                        ws.Cells["E" + 1].Value = "AÑO";

                        int i = 2;
                        int z = 2;
                        decimal saldo_ant = 0;
                        decimal saldo_act = 0;
                        decimal hsaldo_act = 0;
                        decimal ing_tot = 0;
                        decimal hing_tot = 0;
                        foreach (var item in patrimonio)


                        {
                            if (desdeSaldo == 0) saldo_ant = GetSaldos(desdeSaldoAno, desdeSaldo, item.cuenta, documento);
                            else saldo_ant = 0;

                            saldo_act = item.saldo + saldo_ant;

                            ws.Cells["B" + i].Value = item.nombre;
                            ws.Cells["C" + i].Value = saldo_act;
                            ws.Cells["D" + i].Value = item.mes;
                            ws.Cells["E" + i].Value = item.año;

                            ing_tot += saldo_act;
                            i++;
                        }
                        i++;
                        ws.Cells["B" + i].Value = "Saldo Final Patrimonio";
                        ws.Cells["c" + i].Value = ing_tot;
                        i = i + 2;

                        ws.Cells["G" + 1].Value = "";
                        ws.Cells["H" + 1].Value = "SALDO";
                        ws.Cells["I" + 1].Value = "MES";
                        ws.Cells["J" + 1].Value = "AÑO";

                        foreach (var item in hpatrimonio)
                        {
                            if (desdeSaldo == 0) saldo_ant = GetSaldos(desdeSaldoAno, desdeSaldo, item.cuenta, documento);
                            else saldo_ant = 0;

                            hsaldo_act = item.saldo + saldo_ant;

                            ws.Cells["G" + z].Value = item.nombre;
                            ws.Cells["H" + z].Value = hsaldo_act;
                            ws.Cells["I" + z].Value = item.mes;
                            ws.Cells["J" + z].Value = item.año;

                            hing_tot += hsaldo_act;
                            z++;

                        }
                        z++;
                        ws.Cells["G" + z].Value = "Saldo Final Patrimonio";
                        ws.Cells["H" + z].Value = hing_tot;
                        z = z + 2;

                        i = i + 2;

                        ws.Cells["B" + i].Value = "Total Cambios";
                        ws.Cells["C" + i].Value = ing_tot - hing_tot;
                    }

                }
                else if (informe == 17)
                {

                    var agencia = coll["agencia"];
                    if (agencia != "")
                    {
                        int agency = Convert.ToInt32(agencia);
                        morosidad2 = morosidad2.Where(x => x.Terceros.DEPENDENCIA == agency).ToList();
                    }

                    if (fechaDesde3 != "")
                    {
                        DateTime fechDesde3 = Convert.ToDateTime(fechaDesde3);
                        DateTime fechaActual = DateTime.Now;
                        DateTime fechaActual2 = new DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day, 23, 59, 59);
                        morosidad2 = morosidad2.Where(x => x.fechaApertura >= fechDesde3 && x.fechaApertura <= fechaActual2).ToList();

                        if (fechaHasta3 != "")
                        {

                            DateTime fech5 = Convert.ToDateTime(fechaHasta3);
                            DateTime fech6 = new DateTime(fech5.Year, fech5.Month, fech5.Day, 23, 59, 59);
                            morosidad2 = morosidad2.Where(x => x.fechaApertura >= fechDesde3 && x.fechaApertura <= fech6).ToList();
                        }

                    }

                    morosidad2 = morosidad2.OrderBy(m => m.id).ThenBy(m => m.fechaApertura.GetValueOrDefault()).ToList();

                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("Morosidad de aportes");
                    ws.Cells["A" + 1].Value = "CUENTA";
                    ws.Cells["B" + 1].Value = "IDENTIFICACIÓN";
                    ws.Cells["C" + 1].Value = "NOMBRE";
                    ws.Cells["D" + 1].Value = "FORMA DE PAGO";
                    ws.Cells["E" + 1].Value = "VALOR";
                    ws.Cells["F" + 1].Value = "TOTAL APORTES";
                    ws.Cells["G" + 1].Value = "FECHA DE AFILIACIÓN";
                    ws.Cells["H" + 1].Value = "CUOTAS EN MORA";
                    ws.Cells["I" + 1].Value = "DEUDAD TOTAL";
                    ws.Cells["J" + 1].Value = "ESTADO";
                    ws.Cells["K" + 1].Value = "CELULAR";
                    ws.Cells["L" + 1].Value = "AGENCIA";

                    int i = 2;

                    foreach (var item in morosidad2)
                    {

                        if (item.activa == true)
                        {
                            //tomo el dato de valorCuota aunque deberia ser tomado del campo valor
                            //String valorCuota = item.fichas.valorCuota;
                            //int valorCuota1 = 0;
                            //bool result = int.TryParse(valorCuota, out valorCuota1);
                            //int total_pagos_realizados = total_aportes1 / valorCuota1;

                            //String valor = item.valor;
                            //String total_aportes = item.totalAportes;
                            //int valor1 = 0;
                            //int total_aportes1 = 0;
                            //bool result = int.TryParse(valor, out valor1);
                            //bool result1 = int.TryParse(total_aportes, out total_aportes1);
                            //int total_pagos_realizados = total_aportes1 / valor1;

                            /* String fecha_ini;
                             string fecha_act = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");                      
                             fecha_ini = item.fichas.fechaApertura.ToString();
                             String fecha_inicial_num_mes;
                             String fecha_actual_num_mes;
                              fecha_inicial_num_mes = fecha_ini.Substring(3, 2);
                             fecha_actual_num_mes = fecha_act.Substring(3, 2);
                             bool result3 = int.TryParse(fecha_inicial_num_mes, out fecha_inicial_num_mes1);
                             bool result4 = int.TryParse(fecha_actual_num_mes, out fecha_actual_num_mes1);                     
                             */

                            //DateTime fecha_inicial_num_año = item.fechaApertura.GetValueOrDefault();
                            //int fecha_inicial_num_año1 = fecha_inicial_num_año.Year;

                            //DateTime fecha_inicial_num_mes = item.fechaApertura.GetValueOrDefault();
                            //int fecha_inicial_num_mes1 = fecha_inicial_num_mes.Month;


                            //DateTime fecha_actual_num_año = DateTime.Now;
                            //int fecha_actual_num_año1 = fecha_actual_num_año.Year;

                            //DateTime fecha_actual_num_mes = DateTime.Now;
                            //int fecha_actual_num_mes1 = fecha_actual_num_mes.Month;


                            //int numero_aportes_a_realizar_año = fecha_actual_num_año1 - fecha_inicial_num_año1;
                            //int numero_aportes_a_realizar_mes = fecha_actual_num_mes1 - fecha_inicial_num_mes1;


                            //if (numero_aportes_a_realizar_año > 0)
                            //{
                            //    numero_aportes_a_realizar_mes = (numero_aportes_a_realizar_año * 12) + (numero_aportes_a_realizar_mes);
                            //}


                            //int cuotas_mora = numero_aportes_a_realizar_mes - total_pagos_realizados;
                            //if (cuotas_mora < 0)
                            //{
                            //    cuotas_mora = 0;
                            //}
                            //int deuda_total = cuotas_mora * valor1;


                            //obtener numero de meses desde que abrio la ficha de aporte hasta la fecha actual
                            int diferenciaMeses = 0;
                            int diferenciaanios = 0;

                            DateTime fechApertura = Convert.ToDateTime(item.fechaApertura);
                            string fechNow = "";
                            if (fechaHasta3 != "")
                            {
                                DateTime f3 = Convert.ToDateTime(fechaHasta3);
                                DateTime f4 = new DateTime(f3.Year, f3.Month, f3.Day, 23, 59, 59);
                                fechNow = f4.ToString();

                            }
                            else
                            {
                                DateTime f3 = DateTime.Now;
                                fechNow = f3.ToString();
                            }

                            DateTime fechNow2 = Convert.ToDateTime(fechNow);

                            var dateSpan = DateTimeSpan.CompareDates(fechApertura, fechNow2);
                            diferenciaMeses = dateSpan.Months;
                            diferenciaanios = dateSpan.Years;

                            if (diferenciaanios > 0)
                            {
                                diferenciaMeses = diferenciaMeses + (diferenciaanios * 12);
                            }
                            //.......

                            //obtener deuda total

                            //obtener deuda total
                            var dataPagosFichasAporte = db.FactOpcaja.Where(x => x.nit_propietario_cuenta == item.idPersona).ToList();

                            int num = 0;
                            int deudaTotal = 0, n = 0;
                            if (dataPagosFichasAporte != null)
                            {
                                num = dataPagosFichasAporte.Count();
                                deudaTotal = Convert.ToInt32(item.valor) * ((diferenciaMeses + 1) - num);

                                n = (diferenciaMeses + 1) - num;
                                if (n < 0)
                                {
                                    n = 0;
                                    deudaTotal = 0;
                                }
                            }
                            else
                            {
                                num = 0;
                                deudaTotal = Convert.ToInt32(item.valor) * ((diferenciaMeses + 1) - num);

                                n = (diferenciaMeses + 1) - num;
                                if (n < 0)
                                {
                                    n = 0;
                                    deudaTotal = 0;
                                }
                            }
                            //

                            //verificamos el estado
                            var estado = "";
                            if (n > 0)
                            {
                                estado = "EN MORA";
                            }
                            else
                            {
                                estado = "AL DIA";
                            }
                            //....



                            // DateTime fecha_ini1 = DateTime.Parse(fecha_ini)
                            // var dias = (DateTime.Now - fecha_ini1).TotalDays;
                            ws.Cells["A" + i].Value = item.numeroCuenta;
                            ws.Cells["B" + i].Value = item.idPersona;
                            ws.Cells["C" + i].Value = item.Terceros.NOMBRE1 + " " + item.Terceros.NOMBRE2 + " " + item.Terceros.APELLIDO1 + " " + item.Terceros.APELLIDO2;
                            ws.Cells["D" + i].Value = item.tipoPago;
                            ws.Cells["E" + i].Value = item.valor;
                            ws.Cells["F" + i].Value = item.totalAportes;
                            ws.Cells["G" + i].Value = item.fechaApertura.ToString();
                            ws.Cells["H" + i].Value = n;
                            ws.Cells["I" + i].Value = deudaTotal;
                            ws.Cells["J" + i].Value = estado;
                            ws.Cells["K" + i].Value = item.Terceros.TELMOVIL;
                            ws.Cells["L" + i].Value = item.Terceros.agenciaFK.nombreagencia;

                            i++;
                        }


                    }
                }
                else if (informe == 2)
                {

                    var movimiento = db.Movimientos.Where(x => x.Comprobante.ANULADO == false).ToList();
                    var fechaDesde = coll["fechaDesde"];
                    var fechaHasta = coll["fechaHasta"];
                    if (cuenta != "")
                    {
                        movimiento = movimiento.Where(x => x.CUENTA == cuenta).ToList();
                    }
                    if (fechaDesde != "")
                    {
                        DateTime auxfd = Convert.ToDateTime(fechaDesde);
                        DateTime fd = new DateTime(auxfd.Year, auxfd.Month, auxfd.Day, 0, 0, 0);
                        if (fechaHasta != "")
                        {
                            DateTime auxfh = Convert.ToDateTime(fechaHasta);
                            DateTime fh = new DateTime(auxfh.Year, auxfh.Month, auxfh.Day, 23, 59, 59);
                            movimiento = movimiento.Where(X => X.FECHAMOVIMIENTO >= fd && X.FECHAMOVIMIENTO <= fh).ToList();
                        }
                        else
                        {

                            movimiento = movimiento.Where(X => X.FECHAMOVIMIENTO.Year == fd.Year && X.FECHAMOVIMIENTO.Month == fd.Month && X.FECHAMOVIMIENTO.Day == fd.Day).ToList();
                        }
                    }


                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("AuxiliarCuentas");
                    ws.Cells["A" + 1].Value = "CÓDIGO";
                    ws.Cells["B" + 1].Value = "NOMBRE";
                    ws.Cells["C" + 1].Value = "COMPROBANTE";
                    ws.Cells["D" + 1].Value = "FECHA";
                    ws.Cells["E" + 1].Value = "TERCERO";
                    ws.Cells["F" + 1].Value = "NOMBRE TERCERO";
                    ws.Cells["G" + 1].Value = "DÉBITO";
                    ws.Cells["H" + 1].Value = "CRÉDITO";
                    ws.Cells["I" + 1].Value = "SALDO";

                    var mov = (from m in movimiento
                               orderby m.CUENTA
                               select new { m.CUENTA, m.cuentaFK }).Distinct().ToList();
                    int i = 3;
                    decimal saldo = 0, saldoTotal = 0;
                    foreach (var item in mov)
                    {
                        var dataMov = movimiento.Where(x => x.CUENTA == item.CUENTA).OrderBy(x => x.FECHAMOVIMIENTO).ToList();
                        decimal debito = dataMov.Select(x => x.DEBITO).Sum();
                        decimal credito = dataMov.Select(x => x.CREDITO).Sum();
                        ws.Cells["A" + i].Value = item.CUENTA;
                        ws.Cells["B" + i].Value = item.cuentaFK.NOMBRE;
                        string naturaleza = item.cuentaFK.NATURALEZA;
                        i++;
                        foreach (var item2 in dataMov)
                        {
                            if (naturaleza == "D")
                            {
                                saldo = item2.DEBITO - item2.CREDITO;
                            }
                            else
                            {
                                saldo = item2.CREDITO - item2.DEBITO;
                            }

                            ws.Cells["C" + i].Value = item2.TIPO + " " + item2.NUMERO;
                            ws.Cells["D" + i].Value = item2.FECHAMOVIMIENTO.ToString("yyyy-MM-dd");
                            ws.Cells["E" + i].Value = item2.TERCERO;
                            ws.Cells["F" + i].Value = item2.terceroFK.NOMBRE1 + " " + item2.terceroFK.NOMBRE2 + " " + item2.terceroFK.APELLIDO1 + " " + item2.terceroFK.APELLIDO2;
                            ws.Cells["G" + i].Value = item2.DEBITO.ToString("N0", formato);
                            ws.Cells["H" + i].Value = item2.CREDITO.ToString("N0", formato);
                            ws.Cells["I" + i].Value = saldo.ToString("N0", formato);
                            i++;
                        }
                        if (naturaleza == "D")
                        {
                            saldoTotal = debito - credito;
                        }
                        else
                        {
                            saldoTotal = credito - debito;
                        }
                        ws.Cells["F" + i].Value = "TOTAL";
                        ws.Cells["G" + i].Value = debito.ToString("N0", formato);
                        ws.Cells["H" + i].Value = credito.ToString("N0", formato);
                        ws.Cells["I" + i].Value = saldoTotal.ToString("N0", formato);
                        i += 2;
                    }

                }
                else if (informe == 30)
                {
                    //var aportes = (from F in ctx.FichasAportes
                    //               select new { F }).ToList();
                    var opcion = Convert.ToInt16(coll["selectPersonal"].ToString());





                    #region old code
                    //if (chktodos != "on")
                    //{
                    //    if (desdeSaldo == 0)
                    //    {
                    //        if (hastaSaldo != 0)
                    //        {

                    //            aportes = aportes.Where(m => m.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno && m.fechaApertura.GetValueOrDefault().Month <= hastaSaldo).ToList();
                    //        }
                    //        else
                    //        {

                    //            aportes = aportes.Where(m => m.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno).ToList();
                    //        }

                    //    }
                    //    else
                    //    {
                    //        if (hastaSaldo == 0)
                    //        {
                    //            aportes = aportes.Where(m => m.fechaApertura.GetValueOrDefault().Month == desdeSaldo && m.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno).ToList();
                    //        }
                    //        if (desdeSaldo <= hastaSaldo)
                    //        {
                    //            aportes = aportes.Where(m => m.fechaApertura.GetValueOrDefault().Month >= desdeSaldo && m.fechaApertura.GetValueOrDefault().Month <= hastaSaldo && m.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno).ToList();
                    //        }
                    //        else
                    //        {
                    //            aportes = aportes.Where(m => m.fechaApertura.GetValueOrDefault().Month >= desdeSaldo && m.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno).ToList();
                    //        }
                    //    }
                    //}
                    #endregion

                    //aportes = aportes.OrderBy(m => m.fechaApertura).ThenBy(x => x.numeroCuenta).ToList();

                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("TERCEROS");
                    ws.Cells["A" + 1].Value = "Tipo Documento";
                    ws.Cells["B" + 1].Value = "Documento";
                    ws.Cells["C" + 1].Value = "DV";
                    ws.Cells["D" + 1].Value = "Fecha Exp ID";
                    ws.Cells["E" + 1].Value = "Lugar Exp. ID";
                    ws.Cells["F" + 1].Value = "PrimerA";
                    ws.Cells["G" + 1].Value = "SegundoA";
                    ws.Cells["H" + 1].Value = "PrimerN";
                    ws.Cells["I" + 1].Value = "SegundoN";
                    ws.Cells["J" + 1].Value = "Nombre";
                    ws.Cells["K" + 1].Value = "Teléfono";
                    ws.Cells["L" + 1].Value = "Dirección";
                    ws.Cells["M" + 1].Value = "Email";
                    ws.Cells["N" + 1].Value = "Sexo";
                    ws.Cells["O" + 1].Value = "Fecha Nac";
                    ws.Cells["P" + 1].Value = "Estado Civil";
                    ws.Cells["Q" + 1].Value = "Profesión";
                    ws.Cells["R" + 1].Value = "Tel Movil";
                    ws.Cells["S" + 1].Value = "Municipio";
                    ws.Cells["T" + 1].Value = "Barrio";
                    ws.Cells["U" + 1].Value = "Fecha afiliación";
                    ws.Cells["V" + 1].Value = "Agencia";
                    ws.Cells["W" + 1].Value = "Valor Cuota";
                    ws.Cells["X" + 1].Value = "Total Aportes";
                    //ws.Cells["P" + 1].Value = "Valor aporte";
                    //ws.Cells["Q" + 1].Value = "CUOTAS MORA";
                    //ws.Cells["R" + 1].Value = "DEUDA";
                    //ws.Cells["S" + 1].Value = "ESTADO";

                    if (opcion == 2)
                    {
                        var data = (from p in db.Terceros //left join
                                    join f in db.FichasAportes on p.NIT equals f.idPersona
                                    into asociados
                                    from a in asociados.DefaultIfEmpty()
                                    select new { a, p }
                                       ).ToList();
                        int i = 2;


                        foreach (var item in data)
                        {
                            if (item.a != null)
                            {
                                if (item.a.Terceros.CLASEID == "31") { ws.Cells["A" + i].Value = "NIT"; } else { ws.Cells["A" + i].Value = "CC"; }
                                ws.Cells["B" + i].Value = item.a.Terceros.NIT;
                                ws.Cells["C" + i].Value = item.a.Terceros.DIGVER;
                                ws.Cells["D" + i].Value = item.a.Terceros.FECHAEXP.ToString();
                                ws.Cells["E" + i].Value = item.a.Terceros.lugarExpedFK.Nom_muni;
                                ws.Cells["F" + i].Value = item.a.Terceros.APELLIDO1;
                                ws.Cells["G" + i].Value = item.a.Terceros.APELLIDO2;
                                ws.Cells["H" + i].Value = item.a.Terceros.NOMBRE1;
                                ws.Cells["I" + i].Value = item.a.Terceros.NOMBRE2;
                                ws.Cells["J" + i].Value = item.a.Terceros.NombreComercial;
                                ws.Cells["K" + i].Value = item.a.Terceros.TEL;
                                ws.Cells["L" + i].Value = item.a.Terceros.DIR;
                                ws.Cells["M" + i].Value = item.a.Terceros.EMAIL;
                                ws.Cells["N" + i].Value = item.a.Terceros.SEXO;
                                ws.Cells["O" + i].Value = item.a.Terceros.FECHANAC.ToString();
                                ws.Cells["P" + i].Value = item.a.Terceros.ESTADOCIVIL;
                                ws.Cells["Q" + i].Value = item.a.Terceros.profesionFK.Nom_prof;
                                ws.Cells["R" + i].Value = item.a.Terceros.TELMOVIL;
                                ws.Cells["S" + i].Value = item.a.Terceros.municipioFK.Nom_muni;
                                ws.Cells["T" + i].Value = item.a.Terceros.BARRIO;
                                ws.Cells["U" + i].Value = item.a.fechaApertura.ToString();
                                ws.Cells["V" + i].Value = item.a.Terceros.agenciaFK.nombreagencia;
                                ws.Cells["W" + i].Value = item.a.valor;
                                ws.Cells["X" + i].Value = item.a.totalAportes;

                                i++;
                            }
                            else
                            {
                                if (item.p.CLASEID == "31") { ws.Cells["A" + i].Value = "NIT"; } else { ws.Cells["A" + i].Value = "CC"; }
                                ws.Cells["B" + i].Value = item.p.NIT;
                                ws.Cells["C" + i].Value = item.p.DIGVER;
                                ws.Cells["D" + i].Value = item.p.FECHAEXP.ToString();
                                ws.Cells["E" + i].Value = item.p.lugarExpedFK.Nom_muni;
                                ws.Cells["F" + i].Value = item.p.APELLIDO1;
                                ws.Cells["G" + i].Value = item.p.APELLIDO2;
                                ws.Cells["H" + i].Value = item.p.NOMBRE1;
                                ws.Cells["I" + i].Value = item.p.NOMBRE2;
                                ws.Cells["J" + i].Value = item.p.NombreComercial;
                                ws.Cells["K" + i].Value = item.p.TEL;
                                ws.Cells["L" + i].Value = item.p.EMAIL;
                                ws.Cells["M" + i].Value = item.p.EMAIL;
                                ws.Cells["N" + i].Value = item.p.SEXO;
                                ws.Cells["O" + i].Value = item.p.FECHANAC.ToString();
                                ws.Cells["P" + i].Value = item.p.ESTADOCIVIL;
                                ws.Cells["Q" + i].Value = item.p.profesionFK.Nom_prof;
                                ws.Cells["R" + i].Value = item.p.TELMOVIL;
                                if (item.p.NACIO != null) { ws.Cells["S" + i].Value = item.p.municipioFK.Nom_muni; }
                                ws.Cells["T" + i].Value = item.p.BARRIO;
                                ws.Cells["U" + i].Value = "";
                                ws.Cells["V" + i].Value = "";
                                ws.Cells["W" + i].Value = "";
                                ws.Cells["X" + i].Value = "";

                                i++;
                            }


                        }
                    }
                    else
                    {
                        var data = db.FichasAportes.ToList();

                        int i = 2;


                        foreach (var item in data)
                        {

                            if (item.Terceros.CLASEID == "31") { ws.Cells["A" + i].Value = "NIT"; } else { ws.Cells["A" + i].Value = "CC"; }
                            ws.Cells["B" + i].Value = item.Terceros.NIT;
                            ws.Cells["C" + i].Value = item.Terceros.DIGVER;
                            ws.Cells["D" + i].Value = item.Terceros.FECHAEXP.ToString();
                            ws.Cells["E" + i].Value = item.Terceros.lugarExpedFK.Nom_muni;
                            ws.Cells["F" + i].Value = item.Terceros.APELLIDO1;
                            ws.Cells["G" + i].Value = item.Terceros.APELLIDO2;
                            ws.Cells["H" + i].Value = item.Terceros.NOMBRE1;
                            ws.Cells["I" + i].Value = item.Terceros.NOMBRE2;
                            ws.Cells["J" + i].Value = item.Terceros.NombreComercial;
                            ws.Cells["K" + i].Value = item.Terceros.TEL;
                            ws.Cells["L" + i].Value = item.Terceros.DIR;
                            ws.Cells["M" + i].Value = item.Terceros.EMAIL;
                            ws.Cells["N" + i].Value = item.Terceros.SEXO;
                            ws.Cells["O" + i].Value = item.Terceros.FECHANAC.ToString();
                            ws.Cells["P" + i].Value = item.Terceros.ESTADOCIVIL;
                            ws.Cells["Q" + i].Value = item.Terceros.profesionFK.Nom_prof;
                            ws.Cells["R" + i].Value = item.Terceros.TELMOVIL;
                            ws.Cells["S" + i].Value = item.Terceros.municipioFK.Nom_muni;
                            ws.Cells["T" + i].Value = item.Terceros.BARRIO;
                            ws.Cells["U" + i].Value = item.fechaApertura.ToString();
                            ws.Cells["V" + i].Value = item.Terceros.agenciaFK.nombreagencia;
                            ws.Cells["W" + i].Value = item.valor;
                            ws.Cells["X" + i].Value = item.totalAportes;
                            //ws.Cells["O" + i].Value = item.Terceros.FECHAAFILIACION.ToString();



                            i++;
                        }
                    }



                }
                else if (informe == 31)
                {

                    var aportes = (from F in ctx.FichasAportes
                                   join T in ctx.Terceros on F.idPersona equals T.NIT
                                   select new { F, T }).ToList().Distinct();

                    if (desdeSaldo == 0)
                    {
                        if (hastaSaldo != 0)
                        {

                            aportes = aportes.Where(m => m.F.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno && m.F.fechaApertura.GetValueOrDefault().Month <= hastaSaldo).ToList();
                        }
                        else
                        {

                            aportes = aportes.Where(m => m.F.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno).ToList();
                        }

                    }
                    else
                    {
                        if (hastaSaldo == 0)
                        {
                            aportes = aportes.Where(m => m.F.fechaApertura.GetValueOrDefault().Month == desdeSaldo && m.F.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno).ToList();
                        }
                        if (desdeSaldo <= hastaSaldo)
                        {
                            aportes = aportes.Where(m => m.F.fechaApertura.GetValueOrDefault().Month >= desdeSaldo && m.F.fechaApertura.GetValueOrDefault().Month <= hastaSaldo && m.F.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno).ToList();
                        }
                        else
                        {
                            aportes = aportes.Where(m => m.F.fechaApertura.GetValueOrDefault().Month >= desdeSaldo && m.F.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno).ToList();
                        }
                    }

                    aportes = aportes.OrderBy(m => m.F.id).ThenBy(m => m.F.fechaApertura.GetValueOrDefault()).ToList();



                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("individualDeAportes");
                    ws.Cells["A" + 1].Value = "TIPO ID";
                    ws.Cells["B" + 1].Value = "NUMERO DE CUENTA";
                    ws.Cells["C" + 1].Value = "IDENTIFICACION";
                    ws.Cells["D" + 1].Value = "NOMBRE";
                    ws.Cells["E" + 1].Value = "FORMA DE PAGO";
                    ws.Cells["F" + 1].Value = "% CUOTA";
                    ws.Cells["G" + 1].Value = "CUOTA";
                    ws.Cells["H" + 1].Value = "SALDO APORTES";
                    ws.Cells["I" + 1].Value = "FECHA DE INICIO";
                    ws.Cells["J" + 1].Value = "ESTADO";
                    int i = 2;


                    foreach (var item in aportes)
                    {
                        ws.Cells["A" + i].Value = item.F.id;
                        ws.Cells["B" + i].Value = item.F.numeroCuenta;
                        ws.Cells["C" + i].Value = item.F.idPersona;
                        ws.Cells["D" + i].Value = item.T.NOMBRE1 + " " + item.T.NOMBRE2 + " " + item.T.APELLIDO1 + " " + item.T.APELLIDO2;
                        ws.Cells["E" + i].Value = item.F.tipoPago;
                        ws.Cells["F" + i].Value = item.F.porcentaje;
                        ws.Cells["G" + i].Value = item.F.valorCuota;
                        ws.Cells["H" + i].Value = item.F.totalAportes;
                        ws.Cells["I" + i].Value = item.F.fechaApertura.ToString();
                        if (item.F.activa == true)
                        {
                            ws.Cells["J" + i].Value = "ACTIVO";
                        }
                        else
                        {
                            ws.Cells["J" + i].Value = "INACTIVO";
                        }



                        i++;
                    }
                }
                else if (informe == 32)
                {
                    var creditos = (from C in ctx.Creditos
                                    join P in ctx.Prestamos on C.Prestamo_Id equals P.id
                                    select new
                                    {
                                        C.Creditos_Cedula,
                                        C.Capital,
                                        C.Pagare,
                                        C.Destino.Destino_Descripcion,
                                        P.Interes,
                                        P.Plazo,
                                        P.fechadesembolso,
                                        P.NOMBRE
                                    }).ToList().Distinct();

                    if (desdeSaldo == 0)
                    {
                        if (hastaSaldo != 0)
                        {

                            creditos = creditos.Where(m => m.fechadesembolso.Year == desdeSaldoAno && m.fechadesembolso.Month <= hastaSaldo).ToList();
                        }
                        else
                        {

                            creditos = creditos.Where(m => m.fechadesembolso.Year == desdeSaldoAno).ToList();
                        }

                    }
                    else
                    {
                        if (hastaSaldo == 0)
                        {
                            creditos = creditos.Where(m => m.fechadesembolso.Month == desdeSaldo && m.fechadesembolso.Year == desdeSaldoAno).ToList();
                        }
                        if (desdeSaldo <= hastaSaldo)
                        {
                            creditos = creditos.Where(m => m.fechadesembolso.Month >= desdeSaldo && m.fechadesembolso.Month <= hastaSaldo && m.fechadesembolso.Year == desdeSaldoAno).ToList();
                        }
                        else
                        {
                            creditos = creditos.Where(m => m.fechadesembolso.Month >= desdeSaldo && m.fechadesembolso.Year == desdeSaldoAno).ToList();
                        }
                    }

                    creditos = creditos.OrderBy(m => m.Creditos_Cedula).ThenBy(m => m.fechadesembolso).ToList();

                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("creditos");
                    ws.Cells["A" + 1].Value = "DOCUMENTO";
                    ws.Cells["B" + 1].Value = "NOMBRE";
                    ws.Cells["C" + 1].Value = "CAPITAL";
                    ws.Cells["D" + 1].Value = "PAGARE";
                    ws.Cells["E" + 1].Value = "DESTINO";
                    ws.Cells["F" + 1].Value = "INTERES";
                    ws.Cells["G" + 1].Value = "PLAZO";
                    ws.Cells["H" + 1].Value = "FECHA DESEMBOLSO";
                    ws.Cells["I" + 1].Value = "VALOR CUOTA";
                    ws.Cells["J" + 1].Value = "SALDO CAPITAL";
                    int i = 2;
                    foreach (var item in creditos)
                    {
                        ws.Cells["A" + i].Value = item.Creditos_Cedula;
                        ws.Cells["B" + i].Value = item.NOMBRE;
                        ws.Cells["C" + i].Value = item.Capital;
                        ws.Cells["D" + i].Value = item.Pagare;
                        ws.Cells["E" + i].Value = item.Destino_Descripcion;
                        ws.Cells["F" + i].Value = item.Interes;
                        ws.Cells["G" + i].Value = item.Plazo;
                        ws.Cells["H" + i].Value = item.fechadesembolso.ToString("yyyy/MM/dd");
                        var data = ctx.Amortizaciones.OrderByDescending(b => b.id).Where(a => a.pagare == item.Pagare).First();
                        if (data != null) { ws.Cells["I" + i].Value = data.valorCuota; }
                        var data2 = ctx.Amortizaciones.OrderByDescending(b => b.id).Where(a => a.pagare == item.Pagare && a.calendarioDePagos != "Fecha").Count();
                        if (data2 > 0)
                        {
                            var data3 = ctx.Amortizaciones.OrderByDescending(b => b.id).Where(a => a.pagare == item.Pagare && a.calendarioDePagos != "Fecha").First();
                            if (data3 != null) { ws.Cells["J" + i].Value = data3.saldoCapital; }
                        }
                        else
                        {
                            ws.Cells["J" + i].Value = item.Capital;
                        }



                        i++;
                    }

                }
                else if (informe == 33)
                {
                    var ahorros = (from C in ctx.FichasAhorros
                                   select new
                                   {
                                       C.id,
                                       C.numeroCuenta,
                                       C.idPersona,
                                       C.Terceros.NOMBRE,
                                       C.tipoPago,
                                       C.porcentaje,
                                       C.valorCuota,
                                       C.totalAhorros,
                                       C.fechaApertura,
                                       C.activo
                                   }).ToList();

                    if (desdeSaldo == 0)
                    {
                        if (hastaSaldo != 0)
                        {

                            ahorros = ahorros.Where(m => m.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno && m.fechaApertura.GetValueOrDefault().Month <= hastaSaldo).ToList();
                        }
                        else
                        {

                            ahorros = ahorros.Where(m => m.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno).ToList();
                        }

                    }
                    else
                    {
                        if (hastaSaldo == 0)
                        {
                            ahorros = ahorros.Where(m => m.fechaApertura.GetValueOrDefault().Month == desdeSaldo && m.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno).ToList();
                        }
                        if (desdeSaldo <= hastaSaldo)
                        {
                            ahorros = ahorros.Where(m => m.fechaApertura.GetValueOrDefault().Month >= desdeSaldo && m.fechaApertura.GetValueOrDefault().Month <= hastaSaldo && m.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno).ToList();
                        }
                        else
                        {
                            ahorros = ahorros.Where(m => m.fechaApertura.GetValueOrDefault().Month >= desdeSaldo && m.fechaApertura.GetValueOrDefault().Year == desdeSaldoAno).ToList();
                        }
                    }

                    ahorros = ahorros.OrderBy(m => m.fechaApertura).ThenBy(m => m.fechaApertura.GetValueOrDefault()).ToList();

                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("ahorros");
                    ws.Cells["A" + 1].Value = "TIPO ID";
                    ws.Cells["B" + 1].Value = "NUMERO DE AHORRO";
                    ws.Cells["C" + 1].Value = "IDENTIFICACION";
                    ws.Cells["D" + 1].Value = "NOMBRE";
                    ws.Cells["E" + 1].Value = "FORMA DE PAGO";
                    ws.Cells["F" + 1].Value = "PORCENTAJE";
                    ws.Cells["G" + 1].Value = "CUOTA";
                    ws.Cells["H" + 1].Value = "SALDO AHORROS";
                    ws.Cells["I" + 1].Value = "FECHA DE INICIO";
                    ws.Cells["J" + 1].Value = "ESTADO";
                    int i = 2;
                    foreach (var item in ahorros)
                    {
                        ws.Cells["A" + i].Value = item.id;
                        ws.Cells["B" + i].Value = item.numeroCuenta;
                        ws.Cells["C" + i].Value = item.idPersona;
                        ws.Cells["D" + i].Value = item.NOMBRE;
                        ws.Cells["E" + i].Value = item.tipoPago;
                        ws.Cells["F" + i].Value = item.porcentaje;
                        ws.Cells["G" + i].Value = item.valorCuota;
                        ws.Cells["H" + i].Value = item.totalAhorros;
                        ws.Cells["I" + i].Value = item.fechaApertura.ToString();
                        ws.Cells["J" + i].Value = item.activo;
                        i++;
                    }

                }
                else if (informe == 36)
                {

                    var datosPrestamos = db.Prestamos.ToList(); //datos de la tabla prestamos 
                    var datosCredito = db.Creditos.ToList(); //datos de la tabla Bcreditos

                    if (chkfechaDesembolso != "on")
                    {
                        #region filtro normal

                        var creditos = new List<HistorialCreditos>();

                        var fDesde = coll["fechaDesde"];
                        var fHasta = coll["fechaHasta"];

                        if (fDesde != "" && fHasta != "")
                        {
                            DateTime fh = Convert.ToDateTime(fHasta);
                            DateTime fd = Convert.ToDateTime(fDesde);
                            DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                            DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                            creditos = db.HistorialCreditos.Where(x => x.fecha >= fechDesde && x.fecha <= fechHasta).ToList();
                        }
                        else if (fDesde != "" && fHasta == "")
                        {
                            DateTime fd = Convert.ToDateTime(fDesde);
                            DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                            creditos = db.HistorialCreditos.Where(x => x.fecha >= fechDesde).ToList();
                        }
                        else if (fDesde == "" && fHasta != "")
                        {
                            DateTime fh = Convert.ToDateTime(fHasta);
                            DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                            creditos = db.HistorialCreditos.Where(x => x.fecha <= fechHasta).ToList();
                        }


                        ExcelWorksheet ws = pack.Workbook.Worksheets.Add("creditos");
                        ws.Cells["A" + 1].Value = "Fecha De Desembolso";
                        ws.Cells["B" + 1].Value = "Nit";
                        ws.Cells["C" + 1].Value = "Nombres y Apellidos";
                        ws.Cells["D" + 1].Value = "Pagaré";
                        ws.Cells["E" + 1].Value = "Taza Interés Corriente";
                        ws.Cells["F" + 1].Value = "Taza Interés Mora";
                        ws.Cells["G" + 1].Value = "Plazo";
                        ws.Cells["H" + 1].Value = "Valor Costo Por Cuota";
                        ws.Cells["I" + 1].Value = "Capital Inicial";
                        ws.Cells["J" + 1].Value = "Cuotas Pagadas";
                        ws.Cells["K" + 1].Value = "Cuotas Pendientes";
                        ws.Cells["L" + 1].Value = "Dia de Pago";
                        ws.Cells["M" + 1].Value = "Valor Cuota";
                        ws.Cells["N" + 1].Value = "Estado Actual";
                        ws.Cells["O" + 1].Value = "Abono Capital";
                        ws.Cells["P" + 1].Value = "Abono Interés Corriente";
                        ws.Cells["Q" + 1].Value = "Abono Interés Mora";
                        ws.Cells["R" + 1].Value = "Abono Costos Adicionales";
                        ws.Cells["S" + 1].Value = "Saldo Capital";
                        ws.Cells["T" + 1].Value = "Capital en Mora";
                        ws.Cells["U" + 1].Value = "Interés Corriente en Mora";
                        ws.Cells["V" + 1].Value = "Días en Mora";
                        ws.Cells["W" + 1].Value = "Interés Corriente Pendiente";
                        ws.Cells["X" + 1].Value = "Interés de Mora Pendiente";

                        //totales
                        decimal TabonoCapital = 0, TabonoInteresCorriente = 0, TabonoInteresMora = 0, TabonoCostosAdicionales = 0, TsaldoCapital = 0, TcapitalMora = 0, TinteresCorrienteMora = 0, TinteresCorrintePendiente = 0, TinteresMoraPendiente = 0;
                        long TcapitalInicial = 0;
                        //

                        int j = 2;

                        if (creditos != null)
                        {

                            var creditos2 = creditos.Select(x => x.pagare).Distinct().ToList();
                            foreach (var item in creditos2)
                            {

                                var creditos3 = creditos.Where(x => x.pagare == item).ToList();
                                string estado = "";
                                decimal abonoCapital = 0, abonoInteresCorriente = 0, abonoInteresMora = 0, abonoCostoAdicional = 0, saldoCapital = 0, capitalEnMora = 0, interesCorrienteMora = 0, interesCorrientePendiente = 0, interesMoraPendiente = 0;
                                int diasMora = 0;
                                var tercero = creditos3.FirstOrDefault();
                                var dt = db.Terceros.Where(x => x.NIT == tercero.NIT).FirstOrDefault(); //dt: datos tercero
                                var AuxdatosPrestamos = datosPrestamos.Where(x => x.Pagare == item).FirstOrDefault(); //datos de la tabla prestamos 
                                var AuxdatosCredito = datosCredito.Where(x => x.Pagare == item).FirstOrDefault(); //datos de la tabla Bcreditos
                                int plazo = Convert.ToInt32(AuxdatosCredito.Creditos_Plazo);
                                int cuotasPagadas = creditos3.Where(x => x.estado == "pazYsalvo").Count();
                                int cuotasEnMora = creditos3.Where(x => x.estado == "enMora").Count();
                                int cuotasEnPYS = creditos3.Where(x => x.estado == "pazYsalvo").Count();
                                int cuotasDC = creditos3.Where(x => x.estado == "diasTerminados").Count();//cuotas dias cumplidos




                                saldoCapital = creditos3.Where(x => x.estado != "pazYsalvo").Select(x => x.saldoCapital).FirstOrDefault();
                                TsaldoCapital += saldoCapital;

                                if (cuotasPagadas > 0)
                                {
                                    abonoCapital = creditos3.Where(x => x.estado == "pazYsalvo").Select(x => x.abonoCapital).Sum();
                                    abonoInteresCorriente = creditos3.Where(x => x.estado == "pazYsalvo").Select(x => x.AbonoInteresCorriente).Sum();
                                    abonoInteresMora = creditos3.Where(x => x.estado == "pazYsalvo").Select(x => x.abonoInteresMora).Sum();
                                    abonoCostoAdicional = creditos3.Where(x => x.estado == "pazYsalvo").Select(x => x.valorCosto).Sum();


                                    TabonoCapital += abonoCapital;
                                    TabonoInteresCorriente += abonoInteresCorriente;
                                    TabonoInteresMora += abonoInteresMora;
                                    TabonoCostosAdicionales += abonoCostoAdicional;

                                }
                                if (cuotasEnMora > 0)
                                {
                                    capitalEnMora = creditos3.Where(x => x.estado == "enMora").Select(x => x.capitalEnMora).Sum();
                                    interesCorrienteMora = creditos3.Where(x => x.estado == "enMora").Select(x => x.interesCorrienteMora).Sum();
                                    diasMora = creditos3.Where(x => x.estado == "enMora").OrderByDescending(x => x.diasEnMora).Select(x => x.diasEnMora).FirstOrDefault();
                                    interesMoraPendiente = creditos3.Where(x => x.estado == "enMora").Select(x => x.interesMora).Sum();

                                    TcapitalMora += capitalEnMora;
                                    TinteresCorrienteMora += interesCorrienteMora;
                                    TinteresMoraPendiente += interesMoraPendiente;
                                }
                                if (cuotasEnPYS > 0 || cuotasDC > 0)
                                {

                                    interesCorrientePendiente = creditos3.Where(x => x.estado == "normal" || x.estado == "diasTerminados").Select(x => x.interesCorriente).Sum();
                                    TinteresCorrintePendiente += interesCorrientePendiente;
                                }


                                //obtenemos el estado del credito
                                int num = creditos3.Where(x => x.estado == "liquidado").Count();
                                if (num > 0)
                                {
                                    estado = "LIQUIDADO";
                                }
                                else
                                {
                                    num = creditos3.Where(x => x.estado == "enMora").Count();
                                    if (num > 0)
                                    {
                                        estado = "EN MORA";
                                    }
                                    else
                                    {
                                        estado = "AL DIA";
                                    }
                                }

                                //.........

                                //algunos acumuladores
                                TcapitalInicial += AuxdatosCredito.Capital;
                                //.....

                                ws.Cells["A" + j].Value = AuxdatosPrestamos.fechadesembolso.ToString("yyyy-MM-dd");
                                ws.Cells["B" + j].Value = tercero.NIT;
                                ws.Cells["C" + j].Value = dt.NOMBRE1 + " " + dt.NOMBRE2 + " " + dt.APELLIDO1 + " " + dt.APELLIDO2;
                                ws.Cells["D" + j].Value = item;
                                ws.Cells["E" + j].Value = AuxdatosPrestamos.Interes;
                                ws.Cells["F" + j].Value = AuxdatosCredito.Creditos_Interes_Mora;
                                ws.Cells["G" + j].Value = plazo;
                                ws.Cells["H" + j].Value = tercero.valorCosto.ToString("N3", formato);
                                ws.Cells["I" + j].Value = AuxdatosCredito.Capital.ToString("N0", formato);
                                ws.Cells["J" + j].Value = cuotasPagadas;
                                ws.Cells["K" + j].Value = plazo - cuotasPagadas;
                                ws.Cells["L" + j].Value = tercero.fecha.Day;
                                ws.Cells["M" + j].Value = tercero.proximaCuota.ToString("N3", formato);
                                ws.Cells["N" + j].Value = estado;
                                ws.Cells["O" + j].Value = abonoCapital.ToString("N0", formato);
                                ws.Cells["P" + j].Value = abonoInteresCorriente.ToString("N0", formato);
                                ws.Cells["Q" + j].Value = abonoInteresMora.ToString("N0", formato);
                                ws.Cells["R" + j].Value = abonoCostoAdicional.ToString("N0", formato);
                                ws.Cells["S" + j].Value = saldoCapital.ToString("N0", formato);
                                ws.Cells["T" + j].Value = capitalEnMora.ToString("N0", formato);
                                ws.Cells["U" + j].Value = interesCorrienteMora.ToString("N0", formato);
                                ws.Cells["V" + j].Value = diasMora;
                                ws.Cells["W" + j].Value = interesCorrientePendiente.ToString("N0", formato);
                                ws.Cells["X" + j].Value = interesMoraPendiente.ToString("N0", formato);



                                j++;

                            }//fin foreach
                            j++;


                            ws.Cells["I" + j].Value = "Capital Inicial";

                            ws.Cells["O" + j].Value = "Abono Capital";
                            ws.Cells["P" + j].Value = "Abono Interés Corriente";
                            ws.Cells["Q" + j].Value = "Abono Interés Mora";
                            ws.Cells["R" + j].Value = "Abono Costos Adicionales";
                            ws.Cells["S" + j].Value = "Saldo Capital";
                            ws.Cells["T" + j].Value = "Capital en Mora";
                            ws.Cells["U" + j].Value = "Interés Corriente en Mora";
                            ws.Cells["W" + j].Value = "Interés Corriente Pendiente";
                            ws.Cells["X" + j].Value = "Interés de Mora Pendiente";
                            j++;
                            ws.Cells["C" + j].Value = "TOTAL";
                            ws.Cells["I" + j].Value = TcapitalInicial.ToString("N0", formato);
                            ws.Cells["O" + j].Value = TabonoCapital.ToString("N0", formato);
                            ws.Cells["P" + j].Value = TabonoInteresCorriente.ToString("N0", formato);
                            ws.Cells["Q" + j].Value = TabonoInteresMora.ToString("N0", formato);
                            ws.Cells["R" + j].Value = TabonoCostosAdicionales.ToString("N0", formato);
                            ws.Cells["S" + j].Value = TsaldoCapital.ToString("N0", formato);
                            ws.Cells["T" + j].Value = TcapitalMora.ToString("N0", formato);
                            ws.Cells["U" + j].Value = TinteresCorrienteMora.ToString("N0", formato);
                            ws.Cells["W" + j].Value = TinteresCorrintePendiente.ToString("N0", formato);
                            ws.Cells["X" + j].Value = TinteresMoraPendiente.ToString("N0", formato);

                        }//fin if != null

                        #endregion filtro normal
                    }
                    else
                    {
                        #region filtro por fecha desembolso

                        var creditos = new List<Prestamos>();

                        var fDesde = coll["fechaDesde"];
                        var fHasta = coll["fechaHasta"];

                        if (fDesde != "" && fHasta != "")
                        {
                            DateTime fh = Convert.ToDateTime(fHasta);
                            DateTime fd = Convert.ToDateTime(fDesde);
                            DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                            DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                            creditos = db.Prestamos.Where(x => x.fechadesembolso >= fechDesde && x.fechadesembolso <= fechHasta).ToList();
                        }
                        else if (fDesde != "" && fHasta == "")
                        {
                            DateTime fd = Convert.ToDateTime(fDesde);
                            DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                            creditos = db.Prestamos.Where(x => x.fechadesembolso >= fechDesde).ToList();
                        }
                        else if (fDesde == "" && fHasta != "")
                        {
                            DateTime fh = Convert.ToDateTime(fHasta);
                            DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                            creditos = db.Prestamos.Where(x => x.fechadesembolso <= fechHasta).ToList();
                        }



                        ExcelWorksheet ws = pack.Workbook.Worksheets.Add("creditos");
                        ws.Cells["A" + 1].Value = "Fecha De Desembolso";
                        ws.Cells["B" + 1].Value = "Nit";
                        ws.Cells["C" + 1].Value = "Nombres y Apellidos";
                        ws.Cells["D" + 1].Value = "Pagaré";
                        ws.Cells["E" + 1].Value = "Taza Interés Corriente";
                        ws.Cells["F" + 1].Value = "Taza Interés Mora";
                        ws.Cells["G" + 1].Value = "Plazo";
                        ws.Cells["H" + 1].Value = "Valor Costo Por Cuota";
                        ws.Cells["I" + 1].Value = "Capital Inicial";
                        ws.Cells["J" + 1].Value = "Cuotas Pagadas";
                        ws.Cells["K" + 1].Value = "Cuotas Pendientes";
                        ws.Cells["L" + 1].Value = "Dia de Pago";
                        ws.Cells["M" + 1].Value = "Valor Cuota";
                        ws.Cells["N" + 1].Value = "Estado Actual";
                        ws.Cells["O" + 1].Value = "Abono Capital";
                        ws.Cells["P" + 1].Value = "Abono Interés Corriente";
                        ws.Cells["Q" + 1].Value = "Abono Interés Mora";
                        ws.Cells["R" + 1].Value = "Abono Costos Adicionales";
                        ws.Cells["S" + 1].Value = "Saldo Capital";
                        ws.Cells["T" + 1].Value = "Capital en Mora";
                        ws.Cells["U" + 1].Value = "Interés Corriente en Mora";
                        ws.Cells["V" + 1].Value = "Días en Mora";
                        ws.Cells["W" + 1].Value = "Interés Corriente Pendiente";
                        ws.Cells["X" + 1].Value = "Interés de Mora Pendiente";

                        //totales
                        decimal TabonoCapital = 0, TabonoInteresCorriente = 0, TabonoInteresMora = 0, TabonoCostosAdicionales = 0, TsaldoCapital = 0, TcapitalMora = 0, TinteresCorrienteMora = 0, TinteresCorrintePendiente = 0, TinteresMoraPendiente = 0;
                        long TcapitalInicial = 0;
                        //

                        int j = 2;

                        if (creditos != null)
                        {

                            var creditos2 = creditos.Select(x => x.Pagare).Distinct().ToList();
                            foreach (var item in creditos2)
                            {

                                var creditos3 = db.HistorialCreditos.Where(x => x.pagare == item).ToList();
                                string estado = "";
                                decimal abonoCapital = 0, abonoInteresCorriente = 0, abonoInteresMora = 0, abonoCostoAdicional = 0, saldoCapital = 0, capitalEnMora = 0, interesCorrienteMora = 0, interesCorrientePendiente = 0, interesMoraPendiente = 0;
                                int diasMora = 0;
                                var tercero = creditos3.FirstOrDefault();
                                var dt = db.Terceros.Where(x => x.NIT == tercero.NIT).FirstOrDefault(); //dt: datos tercero
                                var AuxdatosPrestamos = datosPrestamos.Where(x => x.Pagare == item).FirstOrDefault(); //datos de la tabla prestamos 
                                var AuxdatosCredito = datosCredito.Where(x => x.Pagare == item).FirstOrDefault(); //datos de la tabla Bcreditos
                                int plazo = Convert.ToInt32(AuxdatosCredito.Creditos_Plazo);
                                int cuotasPagadas = creditos3.Where(x => x.estado == "pazYsalvo").Count();
                                int cuotasEnMora = creditos3.Where(x => x.estado == "enMora").Count();
                                int cuotasEnPYS = creditos3.Where(x => x.estado == "pazYsalvo").Count();
                                int cuotasDC = creditos3.Where(x => x.estado == "diasTerminados").Count();//cuotas dias cumplidos




                                saldoCapital = creditos3.Where(x => x.estado != "pazYsalvo").Select(x => x.saldoCapital).FirstOrDefault();
                                TsaldoCapital += saldoCapital;

                                if (cuotasPagadas > 0)
                                {
                                    abonoCapital = creditos3.Where(x => x.estado == "pazYsalvo").Select(x => x.abonoCapital).Sum();
                                    abonoInteresCorriente = creditos3.Where(x => x.estado == "pazYsalvo").Select(x => x.AbonoInteresCorriente).Sum();
                                    abonoInteresMora = creditos3.Where(x => x.estado == "pazYsalvo").Select(x => x.abonoInteresMora).Sum();
                                    abonoCostoAdicional = creditos3.Where(x => x.estado == "pazYsalvo").Select(x => x.valorCosto).Sum();


                                    TabonoCapital += abonoCapital;
                                    TabonoInteresCorriente += abonoInteresCorriente;
                                    TabonoInteresMora += abonoInteresMora;
                                    TabonoCostosAdicionales += abonoCostoAdicional;

                                }
                                if (cuotasEnMora > 0)
                                {
                                    capitalEnMora = creditos3.Where(x => x.estado == "enMora").Select(x => x.capitalEnMora).Sum();
                                    interesCorrienteMora = creditos3.Where(x => x.estado == "enMora").Select(x => x.interesCorrienteMora).Sum();
                                    diasMora = creditos3.Where(x => x.estado == "enMora").OrderByDescending(x => x.diasEnMora).Select(x => x.diasEnMora).FirstOrDefault();
                                    interesMoraPendiente = creditos3.Where(x => x.estado == "enMora").Select(x => x.interesMora).Sum();

                                    TcapitalMora += capitalEnMora;
                                    TinteresCorrienteMora += interesCorrienteMora;
                                    TinteresMoraPendiente += interesMoraPendiente;
                                }
                                if (cuotasEnPYS > 0 || cuotasDC > 0)
                                {

                                    interesCorrientePendiente = creditos3.Where(x => x.estado == "normal" || x.estado == "diasTerminados").Select(x => x.interesCorriente).Sum();
                                    TinteresCorrintePendiente += interesCorrientePendiente;
                                }


                                //obtenemos el estado del credito
                                int num = creditos3.Where(x => x.estado == "liquidado").Count();
                                if (num > 0)
                                {
                                    estado = "LIQUIDADO";
                                }
                                else
                                {
                                    num = creditos3.Where(x => x.estado == "enMora").Count();
                                    if (num > 0)
                                    {
                                        estado = "EN MORA";
                                    }
                                    else
                                    {
                                        estado = "AL DIA";
                                    }
                                }

                                //.........

                                //algunos acumuladores
                                TcapitalInicial += AuxdatosCredito.Capital;
                                //.....

                                ws.Cells["A" + j].Value = AuxdatosPrestamos.fechadesembolso.ToString("yyyy-MM-dd");
                                ws.Cells["B" + j].Value = tercero.NIT;
                                ws.Cells["C" + j].Value = dt.NOMBRE1 + " " + dt.NOMBRE2 + " " + dt.APELLIDO1 + " " + dt.APELLIDO2;
                                ws.Cells["D" + j].Value = item;
                                ws.Cells["E" + j].Value = AuxdatosPrestamos.Interes;
                                ws.Cells["F" + j].Value = AuxdatosCredito.Creditos_Interes_Mora;
                                ws.Cells["G" + j].Value = plazo;
                                ws.Cells["H" + j].Value = tercero.valorCosto.ToString("N3", formato);
                                ws.Cells["I" + j].Value = AuxdatosCredito.Capital.ToString("N0", formato);
                                ws.Cells["J" + j].Value = cuotasPagadas;
                                ws.Cells["K" + j].Value = plazo - cuotasPagadas;
                                ws.Cells["L" + j].Value = tercero.fecha.Day;
                                ws.Cells["M" + j].Value = tercero.proximaCuota.ToString("N3", formato);
                                ws.Cells["N" + j].Value = estado;
                                ws.Cells["O" + j].Value = abonoCapital.ToString("N0", formato);
                                ws.Cells["P" + j].Value = abonoInteresCorriente.ToString("N0", formato);
                                ws.Cells["Q" + j].Value = abonoInteresMora.ToString("N0", formato);
                                ws.Cells["R" + j].Value = abonoCostoAdicional.ToString("N0", formato);
                                ws.Cells["S" + j].Value = saldoCapital.ToString("N0", formato);
                                ws.Cells["T" + j].Value = capitalEnMora.ToString("N0", formato);
                                ws.Cells["U" + j].Value = interesCorrienteMora.ToString("N0", formato);
                                ws.Cells["V" + j].Value = diasMora;
                                ws.Cells["W" + j].Value = interesCorrientePendiente.ToString("N0", formato);
                                ws.Cells["X" + j].Value = interesMoraPendiente.ToString("N0", formato);



                                j++;

                            }//fin foreach
                            j++;


                            ws.Cells["I" + j].Value = "Capital Inicial";

                            ws.Cells["O" + j].Value = "Abono Capital";
                            ws.Cells["P" + j].Value = "Abono Interés Corriente";
                            ws.Cells["Q" + j].Value = "Abono Interés Mora";
                            ws.Cells["R" + j].Value = "Abono Costos Adicionales";
                            ws.Cells["S" + j].Value = "Saldo Capital";
                            ws.Cells["T" + j].Value = "Capital en Mora";
                            ws.Cells["U" + j].Value = "Interés Corriente en Mora";
                            ws.Cells["W" + j].Value = "Interés Corriente Pendiente";
                            ws.Cells["X" + j].Value = "Interés de Mora Pendiente";
                            j++;
                            ws.Cells["C" + j].Value = "TOTAL";
                            ws.Cells["I" + j].Value = TcapitalInicial.ToString("N0", formato);
                            ws.Cells["O" + j].Value = TabonoCapital.ToString("N0", formato);
                            ws.Cells["P" + j].Value = TabonoInteresCorriente.ToString("N0", formato);
                            ws.Cells["Q" + j].Value = TabonoInteresMora.ToString("N0", formato);
                            ws.Cells["R" + j].Value = TabonoCostosAdicionales.ToString("N0", formato);
                            ws.Cells["S" + j].Value = TsaldoCapital.ToString("N0", formato);
                            ws.Cells["T" + j].Value = TcapitalMora.ToString("N0", formato);
                            ws.Cells["U" + j].Value = TinteresCorrienteMora.ToString("N0", formato);
                            ws.Cells["W" + j].Value = TinteresCorrintePendiente.ToString("N0", formato);
                            ws.Cells["X" + j].Value = TinteresMoraPendiente.ToString("N0", formato);

                        }//fin if != null

                        #endregion filtro por fecha desembolso
                    }



                }
                else if (informe == 4)
                {
                    List<Movimiento> saldosCuentas = new List<Movimiento>();
                    var PlanCuentas = db.PlanCuentas.ToList();

                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("Balance General");
                    ws.Cells["A" + 1].Value = "BALANCE";
                    ws.Cells["B" + 1].Value = "GENERAL";
                    ws.Cells["C" + 1].Value = "ASOPASCUALINOS";

                    int J = 3; decimal totalActivo = 0, totalPasivo = 0;
                    int num = db.SaldosCuentas.Count();
                    if (num > 0)
                    {
                        //int iniAnio = saldosCuentas.OrderBy(x => x.ANO).Select(x => x.ANO).FirstOrDefault();

                        if (desdeSaldo != 0 && hastaSaldo != 0 && desdeSaldoAno != 0)
                        {
                            DateTime fechDesde = new DateTime(desdeSaldoAno, desdeSaldo, 1, 0, 0, 0);
                            int days = DateTime.DaysInMonth(desdeSaldoAno, hastaSaldo);
                            DateTime fechHasta = new DateTime(desdeSaldoAno, hastaSaldo, days, 23, 59, 59);
                            saldosCuentas = db.Movimientos.Where(x => (x.FECHAMOVIMIENTO >= fechDesde && x.FECHAMOVIMIENTO <= fechHasta) && x.Comprobante.ANULADO == false).ToList();

                        }
                        else if (desdeSaldo == 0 && hastaSaldo != 0 && desdeSaldoAno != 0)
                        {
                            int days = DateTime.DaysInMonth(desdeSaldoAno, hastaSaldo);
                            DateTime fechHasta = new DateTime(desdeSaldoAno, hastaSaldo, days, 23, 59, 59);
                            saldosCuentas = db.Movimientos.Where(x => x.FECHAMOVIMIENTO <= fechHasta && x.Comprobante.ANULADO == false).ToList();
                            //saldosCuentas = saldosCuentas.Where(x => (x.ANO >= iniAnio && x.ANO < desdeSaldoAno && x.MES <= 12) || (x.ANO == desdeSaldoAno && x.MES <= hastaSaldo)).ToList();
                            //saldosCuentas = saldosCuentas.Where(x => (x.ANO >= iniAnio && x.ANO <= desdeSaldoAno && x.MES <= 12)).ToList();
                        }

                        var auxactivos = (from ax in saldosCuentas
                                          where ax.CUENTA.StartsWith("1")
                                          orderby ax.CUENTA
                                          select new { ax.CUENTA, ax.cuentaFK }
                                          ).Distinct().ToList();
                        //saldosCuentas.Where(x => x.CUENTA.StartsWith("1")).OrderBy(x => x.CUENTA).Select(x => x.CUENTA).Distinct().ToList();
                        var activos = saldosCuentas.Where(x => x.CUENTA.StartsWith("1")).ToList();
                        foreach (var item in auxactivos)
                        {
                            decimal saldo = 0;
                            var data = activos.Where(x => x.CUENTA == item.CUENTA).ToList();
                            saldo = data.Select(x => x.DEBITO).Sum() - data.Select(x => x.CREDITO).Sum();
                            totalActivo += saldo;



                            ws.Cells["A" + J].Value = item.CUENTA;
                            ws.Cells["B" + J].Value = item.cuentaFK.NOMBRE;
                            ws.Cells["C" + J].Value = saldo;
                            ws.Cells["D" + J].Value = item.cuentaFK.NATURALEZA;

                            J++;
                        }
                        J = 3;
                        var auxPasivos = (from ax in saldosCuentas
                                          where ax.CUENTA.StartsWith("2") || ax.CUENTA.StartsWith("3")
                                          orderby ax.CUENTA
                                          select new { ax.CUENTA, ax.cuentaFK }
                        ).Distinct().ToList();
                        //var auxPasivos = saldosCuentas.Where(x => x.CODIGO.StartsWith("2") || x.CODIGO.StartsWith("3")).OrderBy(x => x.CODIGO).Select(x => x.CODIGO).Distinct().ToList();
                        var pasivos = saldosCuentas.Where(x => x.CUENTA.StartsWith("2") || x.CUENTA.StartsWith("3")).ToList();
                        foreach (var item in auxPasivos)
                        {

                            if (item.CUENTA != "350505001")
                            {
                                decimal saldo = 0;
                                var data = pasivos.Where(x => x.CUENTA == item.CUENTA).ToList();
                                saldo = saldo = data.Select(x => x.CREDITO).Sum() - data.Select(x => x.DEBITO).Sum();
                                totalPasivo += saldo;

                                ws.Cells["H" + J].Value = saldo;

                                ws.Cells["F" + J].Value = item.CUENTA;
                                ws.Cells["G" + J].Value = item.cuentaFK.NOMBRE;
                                ws.Cells["I" + J].Value = item.cuentaFK.NATURALEZA;

                                J++;
                            }
                        }

                        decimal saldopasivo = 0;
                        var dataa = pasivos.Where(x => x.CUENTA == "350505001").ToList();
                        if (dataa.Count > 0)
                        {
                            saldopasivo = dataa.Select(x => x.CREDITO).Sum() - dataa.Select(x => x.DEBITO).Sum();
                        }
                        decimal ingresos = saldosCuentas.Where(x => x.CUENTA.StartsWith("4")).Select(x => x.CREDITO).Sum() - saldosCuentas.Where(x => x.CUENTA.StartsWith("4")).Select(x => x.DEBITO).Sum();
                        decimal gastos = (saldosCuentas.Where(x => x.CUENTA.StartsWith("5")).Select(x => x.DEBITO).Sum() - saldosCuentas.Where(x => x.CUENTA.StartsWith("5")).Select(x => x.CREDITO).Sum()) + ((saldosCuentas.Where(x => x.CUENTA.StartsWith("6")).Select(x => x.DEBITO).Sum() - saldosCuentas.Where(x => x.CUENTA.StartsWith("6")).Select(x => x.CREDITO).Sum()));
                        decimal exedente = ingresos - gastos;
                        ws.Cells["H" + J].Value = saldopasivo + exedente;
                        totalPasivo += exedente + saldopasivo;

                        ws.Cells["F" + J].Value = "350505001";
                        ws.Cells["G" + J].Value = "EXCEDENTES DEL EJERCICIO";
                        ws.Cells["I" + J].Value = "C";



                        J++;
                        ws.Cells["B" + J].Value = "ACTIVO";
                        ws.Cells["C" + J].Value = totalActivo;
                        ws.Cells["G" + J].Value = "PASIVO+PATRIMONIO";
                        ws.Cells["H" + J].Value = totalPasivo;

                    }
                }
                else if (informe == 39)
                {

                    var fechaDesde = coll["fechaDesde"];
                    var fechaHasta = coll["fechaHasta"];

                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("balanceGeneral");
                    ws.Cells["A" + 1].Value = "NIT";
                    ws.Cells["B" + 1].Value = "NOMBRES Y APELLIDOS";
                    ws.Cells["C" + 1].Value = "FECHA DE INICIO";
                    ws.Cells["D" + 1].Value = "SALDO DE APORTES";

                    int j = 2;


                    if (fechaDesde != "")
                    {
                        DateTime fd = Convert.ToDateTime(fechaDesde);
                        DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);

                        if (fechaHasta != "")
                        {
                            DateTime fh = Convert.ToDateTime(fechaHasta);
                            DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);

                            var dataConsulta = db.FactOpcaja.Where(x => x.fecha >= fechDesde && x.fecha <= fechHasta).ToList();
                            if (dataConsulta.Count > 0)
                            {
                                //var dataAsociados = dataConsulta.Select(x => x.nit_propietario_cuenta).Distinct().ToList();
                                var dataAsociados = (from DA in dataConsulta
                                                     select new { DA.nit_propietario_cuenta, DA.numero_cuenta }).Distinct().ToList();

                                foreach (var item in dataAsociados)
                                {

                                    var dataAportes = db.FichasAportes.Where(x => x.idPersona == item.nit_propietario_cuenta && x.numeroCuenta == item.numero_cuenta).FirstOrDefault();
                                    var dataCaja = dataConsulta.Where(x => x.nit_propietario_cuenta == item.nit_propietario_cuenta && x.numero_cuenta == item.numero_cuenta).ToList();
                                    decimal totalAportes = dataCaja.Select(x => x.total).Sum();

                                    ws.Cells["A" + j].Value = item.nit_propietario_cuenta;
                                    ws.Cells["B" + j].Value = dataAportes.Terceros.NOMBRE1 + " " + dataAportes.Terceros.NOMBRE2 + " " + dataAportes.Terceros.APELLIDO1 + " " + dataAportes.Terceros.APELLIDO2;
                                    ws.Cells["C" + j].Value = dataAportes.fechaApertura.ToString();
                                    ws.Cells["D" + j].Value = totalAportes.ToString("N0", formato);
                                    j++;
                                }

                            }

                        }
                        else
                        {
                            var dataConsulta = db.FactOpcaja.Where(x => x.fecha.Year == fd.Year && x.fecha.Month == fd.Month && x.fecha.Day == fd.Day).ToList();
                            if (dataConsulta.Count > 0)
                            {
                                //var dataAsociados = dataConsulta.Select(x => x.nit_propietario_cuenta).Distinct().ToList();
                                var dataAsociados = (from DA in dataConsulta
                                                     select new { DA.nit_propietario_cuenta, DA.numero_cuenta }).Distinct().ToList();

                                foreach (var item in dataAsociados)
                                {

                                    var dataAportes = db.FichasAportes.Where(x => x.idPersona == item.nit_propietario_cuenta && x.numeroCuenta == item.numero_cuenta).FirstOrDefault();
                                    var dataCaja = dataConsulta.Where(x => x.nit_propietario_cuenta == item.nit_propietario_cuenta && x.numero_cuenta == item.numero_cuenta).ToList();
                                    decimal totalAportes = dataCaja.Select(x => x.total).Sum();

                                    ws.Cells["A" + j].Value = item.nit_propietario_cuenta;
                                    ws.Cells["B" + j].Value = dataAportes.Terceros.NOMBRE1 + " " + dataAportes.Terceros.NOMBRE2 + " " + dataAportes.Terceros.APELLIDO1 + " " + dataAportes.Terceros.APELLIDO2;
                                    ws.Cells["C" + j].Value = dataAportes.fechaApertura.ToString();
                                    ws.Cells["D" + j].Value = totalAportes.ToString("N0", formato);
                                    j++;
                                }

                            }
                        }
                    }

                }
                else if (informe == 40)
                {
                    var fichasAportes = db.FichasAportes.ToList();

                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("AfilicacionesPorAsesor");
                    ws.Cells["A" + 1].Value = "ID ASESOR";
                    ws.Cells["B" + 1].Value = "NOMBRE ASESOR";
                    ws.Cells["C" + 1].Value = "ID CLIENTE";
                    ws.Cells["D" + 1].Value = "NOMBRE CLIENTE";
                    ws.Cells["E" + 1].Value = "FECHA APERTURA";
                    ws.Cells["F" + 1].Value = "VALOR CUOTA";

                    var fechaDesde = coll["fechaDesde"];
                    var fechaHasta = coll["fechaHasta"];

                    if (fechaDesde != "" && fechaHasta != "")
                    {
                        DateTime f = Convert.ToDateTime(fechaDesde);
                        DateTime fd = new DateTime(f.Year, f.Month, f.Day, 0, 0, 0);
                        DateTime hasta = Convert.ToDateTime(fechaHasta);
                        DateTime fh = new DateTime(hasta.Year, hasta.Month, hasta.Day, 23, 59, 59);

                        fichasAportes = fichasAportes.Where(x => x.fechaApertura >= fd && x.fechaApertura <= fh).ToList();

                    }
                    else if (fechaDesde != "")
                    {
                        DateTime f = Convert.ToDateTime(fechaDesde);
                        fichasAportes = fichasAportes.Where(x => x.fechaApertura.GetValueOrDefault().Year == f.Year && x.fechaApertura.GetValueOrDefault().Month == f.Month && x.fechaApertura.GetValueOrDefault().Day == f.Day).ToList();
                    }

                    if (fichasAportes.Count > 0)
                    {
                        int j = 2;
                        foreach (var item in fichasAportes)
                        {
                            string idAsesor = "";
                            string nomAsesor = "";

                            if (item.asesor != null && item.asesor != "")
                            {
                                var tercero = db.ControlAccesos.Where(x => x.usuario == item.asesor).FirstOrDefault();
                                if (tercero != null)
                                {
                                    nomAsesor = tercero.nombre;
                                }


                                idAsesor = item.asesor;
                            }


                            ws.Cells["A" + j].Value = idAsesor;
                            ws.Cells["B" + j].Value = nomAsesor;
                            ws.Cells["C" + j].Value = item.idPersona;
                            ws.Cells["D" + j].Value = item.Terceros.NOMBRE1 + " " + item.Terceros.NOMBRE2 + " " + item.Terceros.APELLIDO1 + " " + item.Terceros.APELLIDO2 + " ";
                            ws.Cells["E" + j].Value = item.fechaApertura.ToString();
                            ws.Cells["F" + j].Value = item.valorCuota;

                            j++;
                        }
                    }
                }
                else if (informe == 41)
                {
                    var fichasAportes = db.FichasAportes.ToList();

                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("AfilicacionesPorAgencia");
                    ws.Cells["A" + 1].Value = "FECHA DE AFILIACIÓN";
                    ws.Cells["B" + 1].Value = "DOCUMENTO";
                    ws.Cells["C" + 1].Value = "NOMBRE ASOCIADO";
                    ws.Cells["D" + 1].Value = "VALOR CUOTA";
                    ws.Cells["E" + 1].Value = "TOTAL APORTES";
                    ws.Cells["F" + 1].Value = "AGENCIA";

                    var fechaDesde = coll["fechaDesde"];
                    var fechaHasta = coll["fechaHasta"];
                    var agencia = coll["agencia"];

                    if (fechaDesde != "" && fechaHasta != "")
                    {

                        DateTime f = Convert.ToDateTime(fechaDesde);
                        DateTime fd = new DateTime(f.Year, f.Month, f.Day, 0, 0, 0);
                        DateTime hasta = Convert.ToDateTime(fechaHasta);
                        DateTime fh = new DateTime(hasta.Year, hasta.Month, hasta.Day, 23, 59, 59);

                        fichasAportes = fichasAportes.Where(x => x.fechaApertura >= fd && x.fechaApertura <= fh).ToList();

                    }
                    else if (fechaDesde != "")
                    {
                        DateTime f = Convert.ToDateTime(fechaDesde);
                        fichasAportes = fichasAportes.Where(x => x.fechaApertura.GetValueOrDefault().Year == f.Year && x.fechaApertura.GetValueOrDefault().Month == f.Month && x.fechaApertura.GetValueOrDefault().Day == f.Day).ToList();
                    }

                    if (agencia != "")
                    {
                        int agency = Convert.ToInt32(agencia);
                        fichasAportes = fichasAportes.Where(x => x.Terceros.DEPENDENCIA == agency).ToList();
                    }

                    if (fichasAportes.Count > 0)
                    {
                        int j = 2;
                        foreach (var item in fichasAportes)
                        {

                            string nomAgencia = "";
                            var infoAgencia = db.agencias.Where(x => x.codigoagencia == item.Terceros.DEPENDENCIA).FirstOrDefault();
                            if (infoAgencia != null)
                            {
                                nomAgencia = infoAgencia.nombreagencia;
                            }

                            ws.Cells["A" + j].Value = item.fechaApertura.ToString();
                            ws.Cells["B" + j].Value = item.idPersona;
                            ws.Cells["C" + j].Value = item.Terceros.NOMBRE1 + " " + item.Terceros.NOMBRE2 + " " + item.Terceros.APELLIDO1 + " " + item.Terceros.APELLIDO2 + " ";
                            ws.Cells["D" + j].Value = item.valorCuota;
                            ws.Cells["E" + j].Value = item.totalAportes;
                            ws.Cells["F" + j].Value = nomAgencia;

                            j++;
                        }
                    }
                }
                else if (informe == 42)
                {
                    //var saldoTercero = ctx.Movimientos.ToList();
                    //int iniAnio = saldoTercero.OrderBy(x => x.FECHAMOVIMIENTO.Year).Select(x => x.FECHAMOVIMIENTO.Year).FirstOrDefault();
                    //int finAnio = anioInicio;


                    //if (hastaMes != 0)
                    //{
                    //    saldoTercero = saldoTercero.Where(x => (x.FECHAMOVIMIENTO.Year >= iniAnio && x.FECHAMOVIMIENTO.Year < finAnio && x.FECHAMOVIMIENTO.Month <= 12) || (x.FECHAMOVIMIENTO.Year == finAnio && x.FECHAMOVIMIENTO.Month <= hastaMes)).ToList();
                    //}
                    //else
                    //{
                    //    saldoTercero = saldoTercero.Where(x => (x.FECHAMOVIMIENTO.Year >= iniAnio && x.FECHAMOVIMIENTO.Year <= finAnio && x.FECHAMOVIMIENTO.Month <= 12)).ToList();
                    //}

                    //var cuentasTerceros = (from st in saldoTercero
                    //                       select new { st.CUENTA, st.TERCERO, st.terceroFK }).Distinct().OrderBy(x => x.CUENTA).ThenBy(x => x.TERCERO).ToList();

                    //ExcelWorksheet ws = pack.Workbook.Worksheets.Add("Auxiliar Por Tercero Detallado");
                    //ws.Cells["A" + 1].Value = "CUENTA";
                    //ws.Cells["B" + 1].Value = "TIPO";
                    //ws.Cells["C" + 1].Value = "NÚMERO";
                    //ws.Cells["D" + 1].Value = "FECHA";
                    //ws.Cells["E" + 1].Value = "DOCUMENTO";
                    //ws.Cells["F" + 1].Value = "NOMBRE";
                    //ws.Cells["G" + 1].Value = "DETALLE";
                    //ws.Cells["H" + 1].Value = "SALDO ANTERIOR";
                    //ws.Cells["I" + 1].Value = "DÉBITO";
                    //ws.Cells["J" + 1].Value = "CRÉDITO";
                    //ws.Cells["K" + 1].Value = "SALDO";
                    //ws.Cells["L" + 1].Value = "BASE";

                    //int i = 2;

                    //foreach (var item in cuentasTerceros)
                    //{
                    //    decimal credito = 0, debito = 0, saldo = 0, saldoAnterior = 0;
                    //    var datos = saldoTercero.Where(x => x.CUENTA == item.CUENTA && x.TERCERO == item.TERCERO).OrderBy(x => x.FECHAMOVIMIENTO.Year).ThenBy(x => x.FECHAMOVIMIENTO.Month).ToList();
                    //    var ultimaTupla = datos.OrderByDescending(x => x.FECHAMOVIMIENTO.Year).ThenByDescending(x => x.FECHAMOVIMIENTO.Month).FirstOrDefault();

                    //     string nomTercero = item.terceroFK.NOMBRE1 + " " + item.terceroFK.NOMBRE2 + " " + item.terceroFK.APELLIDO1 + " " + item.terceroFK.APELLIDO2;

                    //    //int num2 = datos.Count();

                    //    foreach (var item2 in datos)
                    //    {
                    //        saldoAnterior = saldo;

                    //        debito = item2.DEBITO;
                    //        credito = item2.CREDITO;

                    //        if (item2.cuentaFK.NATURALEZA == "D")
                    //        {
                    //            saldo = saldoAnterior + (debito - credito);
                    //        }
                    //        else
                    //        {
                    //            saldo = saldoAnterior + (credito - debito);
                    //        }

                    //        ws.Cells["A" + i].Value = item.CUENTA;
                    //        ws.Cells["B" + i].Value = item2.TIPO;
                    //        ws.Cells["C" + i].Value = item2.NUMERO;
                    //        ws.Cells["D" + i].Value = item2.FECHAMOVIMIENTO.ToString("yyyy-MM-dd");
                    //        ws.Cells["E" + i].Value = item2.TERCERO;
                    //        ws.Cells["F" + i].Value = nomTercero;
                    //        ws.Cells["G" + i].Value = item2.DETALLE;
                    //        ws.Cells["H" + i].Value = saldoAnterior.ToString("N2", formato);
                    //        ws.Cells["I" + i].Value = item2.DEBITO.ToString("N2", formato);
                    //        ws.Cells["J" + i].Value = item2.CREDITO.ToString("N2", formato);
                    //        ws.Cells["K" + i].Value = saldo.ToString("N2", formato);

                    //        i++;


                    //    }

                    //}
                }
                else if (informe == 45)
                {
                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("ReporteUIAF");
                    ws.Cells["A" + 2].Value = "COOPERATIVA DE APORTE Y CRÉDITO ASOPASCUALINOS";
                    ws.Cells["A" + 3].Value = "REPORTE TRANSACCIONES EN EFECTIVO - C.E. 014 DE 2018 PARA ORGANIZACIONES DE LA ECONOMÍA SOLIDARIA QUE NO EJERCEN ACTIVIDAD FINANCIERA DEL COPERATIVISMO";
                    ws.Cells["A" + 4].Value = "";


                    ws.Cells["A2:X2"].Merge = true;//une columnas en una fila
                    ws.Cells["A3:X3"].Merge = true;//une columnas en una fila
                    ws.Cells["A4:X4"].Merge = true;//une columnas en una fila

                    ws.Cells["B" + 6].Value = "CONSECUTIVO";
                    ws.Cells["C" + 6].Value = "FECHA TRANSACCIÓN";
                    ws.Cells["D" + 6].Value = "VALOR TRANSACCIÓN";
                    ws.Cells["E" + 6].Value = "TIPO MONEDA";
                    ws.Cells["F" + 6].Value = "CÓDIGO OFICINA";
                    ws.Cells["G" + 6].Value = "CÓDIGO DPTO/MPIO";
                    ws.Cells["H" + 6].Value = "TIPO PRODUCTO";
                    ws.Cells["I" + 6].Value = "TIPO TRANSACCIÓN";
                    ws.Cells["J" + 6].Value = "Nro CUENTA O PRODUCTO";
                    ws.Cells["K" + 6].Value = "TIPO IDENTIFICACIÓN DEL TITULAR";
                    ws.Cells["L" + 6].Value = "Nro IDENTIFICACIÓN DEL TITULAR";
                    ws.Cells["M" + 6].Value = "1er. APELLIDO DEL TITULAR";
                    ws.Cells["N" + 6].Value = "2do. APELLIDO DEL TITULAR";
                    ws.Cells["O" + 6].Value = "1er. NOMBRE DEL TITULAR";
                    ws.Cells["P" + 6].Value = "OTROS NOMBRES DEL TITULAR";
                    ws.Cells["Q" + 6].Value = "RAZÓN SOCIAL DEL TITULAR";
                    ws.Cells["R" + 6].Value = "ACTIVIDAD ECONÓMICA DEL TITULAR";
                    ws.Cells["S" + 6].Value = "INGRESO MENSUAL DEL TITULAR";
                    ws.Cells["T" + 6].Value = "TIPO IDENTIFICACIÓN PERSONA QUE REALIZA LA TRANSACCIÓN INDIVIDUAL";
                    ws.Cells["U" + 6].Value = "Nro IDENTIFICACIÓN PERSONA QUE REALIZA LA TRANSACCIÓN INDIVIDUAL";
                    ws.Cells["V" + 6].Value = "1er. APELLIDO PERSONA QUE REALIZA LA TRANSACCIÓN INDIVIDUAL";
                    ws.Cells["W" + 6].Value = "2do. APELLIDO PERSONA QUE REALIZA LA TRANSACCIÓN INDIVIDUAL";
                    ws.Cells["X" + 6].Value = "1er. NOMBRE PERSONA QUE REALIZA LA TRANSACCIÓN INDIVIDUAL";
                    ws.Cells["Y" + 6].Value = "OTROS NOMBRES PERSONA QUE REALIZA LA TRANSACCIÓN INDIVIDUAL";

                    var periodo = Convert.ToInt32(coll["periodoTrimestral"]);
                    var desdeAnio = Convert.ToInt32(coll["year2"]);
                    if (periodo != 0 && desdeSaldoAno != 0)
                    {
                        DateTime fechaActual = DateTime.Now;
                        ws.Cells["A" + 4].Value = fechaActual.ToString("yyyy-MMMM");
                        List<FactOpcaja> facturas = new List<FactOpcaja>();
                        if (periodo == 1)
                        {
                            DateTime fechaIni = new DateTime(desdeAnio, 01, 01, 0, 0, 0);
                            DateTime fechaFin = new DateTime(desdeAnio, 03, 31, 23, 59, 59);
                            facturas = db.FactOpcaja.Where(x => x.fecha >= fechaIni && x.fecha <= fechaFin).ToList();
                        }
                        else if (periodo == 2)
                        {
                            DateTime fechaIni = new DateTime(desdeAnio, 04, 01, 0, 0, 0);
                            DateTime fechaFin = new DateTime(desdeAnio, 06, 30, 23, 59, 59);
                            facturas = db.FactOpcaja.Where(x => x.fecha >= fechaIni && x.fecha <= fechaFin).ToList();
                        }
                        else if (periodo == 3)
                        {
                            DateTime fechaIni = new DateTime(desdeAnio, 07, 01, 0, 0, 0);
                            DateTime fechaFin = new DateTime(desdeAnio, 09, 30, 23, 59, 59);
                            facturas = db.FactOpcaja.Where(x => x.fecha >= fechaIni && x.fecha <= fechaFin).ToList();
                        }
                        else if (periodo == 4)
                        {
                            DateTime fechaIni = new DateTime(desdeAnio, 10, 01, 0, 0, 0);
                            DateTime fechaFin = new DateTime(desdeAnio, 12, 31, 23, 59, 59);
                            facturas = db.FactOpcaja.Where(x => x.fecha >= fechaIni && x.fecha <= fechaFin).ToList();
                        }

                        facturas = facturas.OrderBy(x => x.fecha).ToList();

                        int i = 1;
                        int j = 7;
                        foreach (var item in facturas)
                        {
                            ws.Cells["B" + j].Value = i.ToString();
                            ws.Cells["C" + j].Value = item.fecha.ToString();
                            ws.Cells["D" + j].Value = item.total.ToString("N0", formato);
                            ws.Cells["E" + j].Value = "1";
                            ws.Cells["F" + j].Value = item.Caja.agencia;
                            if (item.Caja.agencia == 1)
                            {
                                ws.Cells["G" + j].Value = "52356";
                            }
                            else if (item.Caja.agencia == 2)
                            {
                                ws.Cells["G" + j].Value = "52227";
                            }

                            ws.Cells["H" + j].Value = 13.ToString();
                            ws.Cells["I" + j].Value = 2.ToString();
                            ws.Cells["J" + j].Value = item.numero_cuenta;
                            ws.Cells["K" + j].Value = 13.ToString();
                            ws.Cells["L" + j].Value = item.nit_propietario_cuenta;
                            ws.Cells["M" + j].Value = item.terceroFK.APELLIDO1;
                            ws.Cells["N" + j].Value = item.terceroFK.APELLIDO2;
                            ws.Cells["O" + j].Value = item.terceroFK.NOMBRE1;
                            ws.Cells["P" + j].Value = "";
                            ws.Cells["Q" + j].Value = "";
                            ws.Cells["R" + j].Value = item.terceroFK.profesionFK.Nom_prof;
                            ws.Cells["S" + j].Value = Convert.ToInt32(item.terceroFK.SALARIO).ToString("N0", formato);
                            ws.Cells["T" + j].Value = 13.ToString();
                            ws.Cells["U" + j].Value = item.nit_propietario_cuenta;
                            ws.Cells["V" + j].Value = "";
                            ws.Cells["W" + j].Value = "";
                            ws.Cells["X" + j].Value = "";
                            ws.Cells["Y" + j].Value = "";

                            i++;
                            j++;

                        }
                        ws.Cells[ws.Dimension.Address].AutoFitColumns();
                    }


                }
                else if (informe == 46)
                {
                    var agencia = coll["agencia"];

                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("Informe Individual de Cartera de Crédito");
                    ws.Cells["A" + 2].Value = "COOPERATIVA DE APORTE Y CRÉDITO ASOPASCUALINOS";
                    ws.Cells["A" + 3].Value = "INFORME INDIVIDUAL DE CARTERA DE CRÉDITO";
                    ws.Cells["A" + 4].Value = "";


                    ws.Cells["A2:X2"].Merge = true;//une columnas en una fila
                    ws.Cells["A3:X3"].Merge = true;//une columnas en una fila
                    ws.Cells["A4:X4"].Merge = true;//une columnas en una fila

                    ws.Cells["B" + 6].Value = "TIPO IDENTIFICACIÓN";
                    ws.Cells["C" + 6].Value = "NIT";
                    ws.Cells["D" + 6].Value = "CÓDIGO CONTABLE";
                    ws.Cells["E" + 6].Value = "MODIFICACIONES AL CRÉDITO";
                    ws.Cells["F" + 6].Value = "N° CRÉDITO";
                    ws.Cells["G" + 6].Value = "FECHA DESEMBOLSO INICIAL";
                    ws.Cells["H" + 6].Value = "FECHA VENCIMIENTO";
                    ws.Cells["I" + 6].Value = "MOROSIDAD";
                    ws.Cells["J" + 6].Value = "TIPO CUOTA";
                    ws.Cells["K" + 6].Value = "ALTURA CUOTA";
                    ws.Cells["L" + 6].Value = "AMORTIZACIÓN";
                    ws.Cells["M" + 6].Value = "MODALIDAD";
                    ws.Cells["N" + 6].Value = "TASA INTERÉS NOMINAL";
                    ws.Cells["O" + 6].Value = "TASA INTERÉS EFECTIVA";
                    ws.Cells["P" + 6].Value = "VALOR PRÉSTAMO";
                    ws.Cells["Q" + 6].Value = "VALOR CUOTA FIJA";
                    ws.Cells["R" + 6].Value = "SALDO CAPITAL";
                    ws.Cells["S" + 6].Value = "SALDO INTERESES";
                    ws.Cells["T" + 6].Value = "OTROS SALDOS";
                    ws.Cells["U" + 6].Value = "GARANTÍA";
                    ws.Cells["V" + 6].Value = "FECHA AVAÚO";
                    ws.Cells["W" + 6].Value = "PROVISIÓN";
                    ws.Cells["X" + 6].Value = "PROVISIÓN INTERÉS";
                    ws.Cells["Y" + 6].Value = "CONTINGENCIA";
                    ws.Cells["Z" + 6].Value = "VALOR CUOTAS EXTRA";
                    ws.Cells["AA" + 6].Value = "MESE CUOTAS EXTRA";
                    ws.Cells["AB" + 6].Value = "FECHA ÚLTIMO PAGO";
                    ws.Cells["AC" + 6].Value = "CLASE GARANTÍA";
                    ws.Cells["AD" + 6].Value = "DESTINO CRÉDITO";
                    ws.Cells["AE" + 6].Value = "CÓDIGO OFICINA";
                    ws.Cells["AF" + 6].Value = "AMORTIZACIÓN CAPITAL";
                    ws.Cells["AG" + 6].Value = "VALOR MORA";
                    ws.Cells["AH" + 6].Value = "TIPO VIVIENDA";
                    ws.Cells["AI" + 6].Value = "VIS";
                    ws.Cells["AJ" + 6].Value = "RANGO TIPO";
                    ws.Cells["AK" + 6].Value = "ENTIDAD DE DESCUENTO";
                    ws.Cells["AL" + 6].Value = "MARGEN DE DESCUENTO";
                    ws.Cells["AM" + 6].Value = "SUBSIDIO";
                    ws.Cells["AN" + 6].Value = "DESEMBOLSO";
                    ws.Cells["AO" + 6].Value = "MONEDA";
                    ws.Cells["AP" + 6].Value = "FECHA REESTRUCTURACIÓN";
                    ws.Cells["AQ" + 6].Value = "CATEGORÍA REESTRUCTURACIÓN";
                    ws.Cells["AR" + 6].Value = "APORTES SOCIALES";
                    ws.Cells["AS" + 6].Value = "LÍNEACREDENTIDAD";
                    ws.Cells["AT" + 6].Value = "NUM MODIFICACIONES";
                    ws.Cells["AU" + 6].Value = "ESTADO CRÉDITO";
                    ws.Cells["AV" + 6].Value = "NIT PATRONAL";
                    ws.Cells["AW" + 6].Value = "NOMBRE PATRONAL";
                    ws.Cells["AX" + 6].Value = "MODCREDCE1120";
                    ws.Cells["AY" + 6].Value = "TIPOMODCE1120";
                    ws.Cells["AZ" + 6].Value = "FECHAMODCE1120";
                    ws.Cells["BA" + 6].Value = "CALIFANTEMODCE1120";
                    ws.Cells["BB" + 6].Value = "PERIOGRACIA";
                    ws.Cells["BC" + 6].Value = "TARJCREDCUPROT";
                    ws.Cells["BD" + 6].Value = "ENTOTORGARANT";
                    ws.Cells["BE" + 6].Value = "MODCREDCE1720";

                    var creditos = db.Creditos.OrderBy(x => x.Creditos_Id).ToList();
                    if (agencia != "")
                    {
                        int agency = Convert.ToInt32(agencia);
                        creditos = creditos.Where(x => x.terceroFK.DEPENDENCIA == agency).ToList();
                    }
                    var historial = db.HistorialCreditos.ToList();
                    var amortizacion = (from am in db.Amortizaciones
                                        select new { am, am.pagare }
                                        ).Distinct().ToList();
                    var pagosCreditos = db.factOpCajaConsCuotaCredito.ToList();
                    int j = 7;
                    foreach (var item in creditos)
                    {

                        int plazo = Convert.ToInt32(item.prestamoFK.Plazo);
                        string nCuota = plazo.ToString();
                        DateTime fechaVencimiento = item.prestamoFK.fechadesembolso.AddMonths(plazo);
                        var dataCredito = historial.Where(x => x.pagare == item.Pagare && (x.estado == "enMora" || x.estado == "normal" || x.estado == "diasTerminados") && x.numeroCuota != 0).ToList();
                        int numeroCuota = plazo;
                        decimal valorCuota = Convert.ToDecimal(amortizacion.Where(x => x.pagare == item.Pagare).Select(x => x.am.valorCuota).FirstOrDefault());
                        decimal interesMora = 0, saldoCapital = 0, interesCorriente = 0, interesTotal = 0;
                        string fechaUltPago = "";
                        if (dataCredito != null)
                        {
                            interesMora = dataCredito.Select(x => x.interesMora).Sum();
                            interesCorriente = dataCredito.Select(x => x.interesCorriente).Sum();
                            numeroCuota = dataCredito.OrderByDescending(x => x.id).Select(x => x.numeroCuota).FirstOrDefault();
                            saldoCapital = dataCredito.Select(x => x.saldoCapital).FirstOrDefault();
                            nCuota = numeroCuota.ToString();
                        }
                        if (nCuota == "0") //la razón de este condicional es porque extrañamente si no entra en el if de arriba se vuelve cero
                        {
                            nCuota = plazo.ToString();
                        }


                        var dataUltPago = pagosCreditos.Where(x => x.pagare == item.Pagare).OrderByDescending(x => x.id).FirstOrDefault();
                        if (dataUltPago != null)
                        {
                            fechaUltPago = dataUltPago.fecha.ToString("dd-MM-yyyy");
                        }
                        interesTotal = interesMora + interesCorriente;

                        ws.Cells["B" + j].Value = "C";
                        ws.Cells["C" + j].Value = item.Creditos_Cedula;
                        ws.Cells["D" + j].Value = "14110505001";
                        ws.Cells["E" + j].Value = "";
                        ws.Cells["F" + j].Value = item.Pagare;
                        ws.Cells["G" + j].Value = item.prestamoFK.fechadesembolso.ToString("dd-MM-yyyy");
                        ws.Cells["H" + j].Value = fechaVencimiento.ToString("dd-MM-yyyy");
                        ws.Cells["I" + j].Value = interesMora.ToString("N2", formato);
                        ws.Cells["J" + j].Value = "constante";
                        ws.Cells["K" + j].Value = nCuota;
                        ws.Cells["L" + j].Value = "m";
                        ws.Cells["M" + j].Value = "";
                        ws.Cells["N" + j].Value = item.prestamoFK.Interes.ToString("N2", formato);
                        ws.Cells["O" + j].Value = item.prestamoFK.Interes.ToString("N2", formato);
                        ws.Cells["P" + j].Value = item.Capital.ToString("N2", formato);
                        ws.Cells["Q" + j].Value = valorCuota.ToString("N2", formato);
                        ws.Cells["R" + j].Value = saldoCapital.ToString("N2", formato);
                        ws.Cells["S" + j].Value = interesTotal.ToString("N2", formato);
                        ws.Cells["T" + j].Value = "";
                        ws.Cells["U" + j].Value = "";
                        ws.Cells["V" + j].Value = "";
                        ws.Cells["W" + j].Value = "";
                        ws.Cells["X" + j].Value = "";
                        ws.Cells["Y" + j].Value = "";
                        ws.Cells["Z" + j].Value = "";
                        ws.Cells["AA" + j].Value = "";
                        ws.Cells["AB" + j].Value = fechaUltPago;
                        ws.Cells["AC" + j].Value = "";
                        ws.Cells["AD" + j].Value = item.Destino.Destino_Descripcion;
                        ws.Cells["AE" + j].Value = "1";
                        ws.Cells["AF" + j].Value = "";
                        ws.Cells["AG" + j].Value = "";
                        ws.Cells["AH" + j].Value = "";
                        ws.Cells["AI" + j].Value = "";
                        ws.Cells["AJ" + j].Value = "";
                        ws.Cells["AK" + j].Value = "";
                        ws.Cells["AL" + j].Value = "";
                        ws.Cells["AM" + j].Value = "";
                        ws.Cells["AN" + j].Value = "";
                        ws.Cells["AO" + j].Value = "COP";
                        ws.Cells["AP" + j].Value = "";
                        ws.Cells["AQ" + j].Value = "";
                        ws.Cells["AR" + j].Value = "";
                        ws.Cells["AS" + j].Value = "";
                        ws.Cells["AT" + j].Value = "";
                        ws.Cells["AU" + j].Value = "";
                        ws.Cells["AV" + j].Value = "";
                        ws.Cells["AW" + j].Value = "";
                        ws.Cells["AX" + j].Value = "";
                        ws.Cells["AY" + j].Value = "";
                        ws.Cells["AZ" + j].Value = "";
                        ws.Cells["BA" + j].Value = "";
                        ws.Cells["BB" + j].Value = "";
                        ws.Cells["BC" + j].Value = "";
                        ws.Cells["BD" + j].Value = "";
                        ws.Cells["BE" + j].Value = "";



                        j++;
                    }//fin foreach




                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
                }
                else
                {
                    var activos = saldos.Where(s => s.sal.CODIGO.StartsWith("1")).GroupBy(s => new { s.sal.CODIGO, s.cuentas.NOMBRE, s.cuentas.NATURALEZA }).Select(a => new
                    {
                        cuenta = a.Key.CODIGO,
                        nombre = a.Key.NOMBRE,
                        saldo = a.Sum(sm => sm.sal.MDEBITO - sm.sal.MCREDITO),
                        naturaleza = a.Key.NATURALEZA
                    }).OrderBy(a => a.cuenta).ToList();

                    var pasivos = saldos.Where(s => s.sal.CODIGO.StartsWith("2") || s.sal.CODIGO.StartsWith("3")).GroupBy(s => new { s.sal.CODIGO, s.cuentas.NOMBRE, s.cuentas.NATURALEZA }).Select(a => new
                    {
                        cuenta = a.Key.CODIGO,
                        nombre = a.Key.NOMBRE,
                        saldo = a.Sum(sm => sm.sal.SALDO),
                        naturaleza = a.Key.NATURALEZA
                    }).OrderBy(a => a.cuenta).ToList();


                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("balanceGeneral");
                    ws.Cells["A" + 1].Value = "BALANCE";
                    ws.Cells["B" + 1].Value = "GENERAL";
                    ws.Cells["C" + 1].Value = "ASOPASCUALINOS";
                    ws.Cells["A" + 3].Value = "CUENTA_ACT";
                    ws.Cells["B" + 3].Value = "NOM_ACT";
                    ws.Cells["C" + 3].Value = "VAL_ACT";
                    ws.Cells["D" + 3].Value = "NAT_ACT";
                    ws.Cells["E" + 3].Value = "CUENTA_PAS";
                    ws.Cells["F" + 3].Value = "NOM_PAS";
                    ws.Cells["G" + 3].Value = "VAL_PAS";
                    ws.Cells["H" + 3].Value = "NAT_PAS";
                    int ia = 3;
                    int ip = 3;
                    int it = 3;
                    decimal saldo_ant = 0;
                    decimal saldo_act = 0;
                    decimal tot_act = 0;
                    decimal tot_pas = 0;
                    foreach (var item in activos)
                    {
                        saldo_ant = saldo_ant = GetSaldos(desdeSaldoAno, desdeSaldo, item.cuenta, documento);
                        saldo_act = saldo_ant + item.saldo;
                        ws.Cells["A" + ia].Value = item.cuenta;
                        ws.Cells["B" + ia].Value = item.nombre;
                        ws.Cells["C" + ia].Value = saldo_act;
                        ws.Cells["D" + ia].Value = item.naturaleza;
                        tot_act += saldo_act;
                        ia++;
                    }
                    foreach (var item in pasivos)
                    {
                        saldo_ant = saldo_ant = GetSaldos(desdeSaldoAno, desdeSaldo, item.cuenta, documento);
                        saldo_act = saldo_ant + item.saldo;
                        ws.Cells["E" + ip].Value = item.cuenta;
                        ws.Cells["F" + ip].Value = item.nombre;
                        ws.Cells["G" + ip].Value = saldo_act;
                        ws.Cells["H" + ip].Value = item.naturaleza;
                        tot_pas += saldo_act;
                        ip++;
                    }
                    if (ia >= ip) it = ia;
                    else it = ip;
                    it++;

                    ws.Cells["B" + it].Value = "ACTIVO";
                    ws.Cells["C" + it].Value = tot_act;
                    ws.Cells["F" + it].Value = "PASIVO + PATRIMONIO";
                    ws.Cells["G" + it].Value = tot_pas;
                }

                var ms = new System.IO.MemoryStream();
                pack.SaveAs(ms);
                ms.WriteTo(Response.OutputStream);
            }
            //Response.Flush();
            Response.End();

            return RedirectToAction("../Informes/Index");

        }



        public ActionResult ExcelTerceros()
        {
            using (var ctx = new AccountingContext())
            {
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=listadoTerceros.xlsx");

                var terceros = ctx.Terceros.Select(t => new {
                    Tid = t.CLASEID == "13" ? "C" : "N",
                    Nid = t.NIT,
                    Fid = t.FECHAEXP,
                    Lid = t.LUGAREXP,
                    Ap1 = t.APELLIDO1,
                    Ap2 = t.APELLIDO2,
                    Nombre = t.NOMBRE,
                    Tel = t.TEL,
                    Dir = t.DIR,
                    Email = t.EMAIL,
                    Sexo = t.SEXO == "M" ? 1 : t.SEXO == "F" ? 2 : 3,
                    Fnac = t.FECHANAC,
                    Ecivil = t.ESTADOCIVIL == "S" ? "1" : t.ESTADOCIVIL == "C" ? "2" : t.ESTADOCIVIL == "L" ? "3" : t.ESTADOCIVIL == "U" ? "4" : t.ESTADOCIVIL == "D" ? "5" : "",
                    Prof = t.PROFESION,
                    Movil = t.TELMOVIL,
                    Muni = t.MUNICIPIO,
                    Barrio = t.BARRIO
                });

                using (ExcelPackage pack = new ExcelPackage())
                {
                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("TERCEROS");
                    ws.Cells["A" + 1].Value = "Tipo ID";
                    ws.Cells["B" + 1].Value = "Numero ID";
                    ws.Cells["C" + 1].Value = "Fecha Exp ID";
                    ws.Cells["D" + 1].Value = "Lugar Exp. ID";
                    ws.Cells["E" + 1].Value = "1er Apellido";
                    ws.Cells["F" + 1].Value = "2do Apellido";
                    ws.Cells["G" + 1].Value = "Nombre";
                    ws.Cells["H" + 1].Value = "Fecha Ingreso";
                    ws.Cells["I" + 1].Value = "Teléfono";
                    ws.Cells["J" + 1].Value = "Dirección";
                    ws.Cells["K" + 1].Value = "Act. Económica";
                    ws.Cells["L" + 1].Value = "Email";
                    ws.Cells["M" + 1].Value = "Sexo";
                    ws.Cells["N" + 1].Value = "Empleado";
                    ws.Cells["O" + 1].Value = "Tipo Contrato";
                    ws.Cells["P" + 1].Value = "Nivel ingresos";
                    ws.Cells["Q" + 1].Value = "Fecha Nac";
                    ws.Cells["R" + 1].Value = "Estado Civil";
                    ws.Cells["S" + 1].Value = "Mujer Cabeza de Flia";
                    ws.Cells["T" + 1].Value = "Profesion";
                    ws.Cells["U" + 1].Value = "Tel. Movil";
                    ws.Cells["V" + 1].Value = "Municipio";
                    ws.Cells["W" + 1].Value = "Barrio";

                    int i = 2;

                    foreach (var item in terceros)
                    {
                        ws.Cells["A" + i].Value = item.Tid;
                        ws.Cells["B" + i].Value = item.Nid;
                        ws.Cells["C" + i].Value = item.Fid.ToShortDateString();
                        ws.Cells["D" + i].Value = item.Lid;
                        ws.Cells["E" + i].Value = item.Ap1;
                        ws.Cells["F" + i].Value = item.Ap2;
                        ws.Cells["G" + i].Value = item.Nombre;
                        //ws.Cells["H" + i].Value = "Fecha Ingreso";
                        ws.Cells["I" + i].Value = item.Tel;
                        ws.Cells["J" + i].Value = item.Dir;
                        ws.Cells["K" + i].Value = "00";
                        ws.Cells["L" + i].Value = item.Email;
                        ws.Cells["M" + i].Value = item.Sexo;
                        ws.Cells["N" + i].Value = "0";
                        //ws.Cells["O" + i].Value = "Tipo Contrato";
                        //ws.Cells["P" + i].Value = "Nivel ingresos";
                        ws.Cells["Q" + i].Value = item.Fnac.Value.ToShortDateString();
                        ws.Cells["R" + i].Value = item.Ecivil;
                        //ws.Cells["S" + i].Value = "Mujer Cabeza de Flia";
                        ws.Cells["T" + i].Value = item.Prof;
                        ws.Cells["U" + i].Value = item.Movil;
                        ws.Cells["V" + i].Value = item.Muni;
                        ws.Cells["W" + i].Value = item.Barrio;
                        i++;
                    }

                    var ms = new System.IO.MemoryStream();
                    pack.SaveAs(ms);
                    ms.WriteTo(Response.OutputStream);
                }
                //Response.Flush();
                Response.End();

                return RedirectToAction("../Informes/Index");
            }
        }
        public ActionResult ExcelAportes()
        {
            using (var ctx = new AccountingContext())
            {
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=listadoFichasAportes.xlsx");

                var Aporte = ctx.FichasAportes.Select(a => new
                {
                    Aid = a.id,
                    nC = a.numeroCuenta,
                    Ip = a.idPersona,
                    Fp = a.tipoPago,
                    P = a.porcentaje,
                    Vc = a.valorCuota,
                    Ta = a.totalAportes,
                    Fa = a.fechaApertura,
                    Ea = a.activa,
                });

                using (ExcelPackage pack = new ExcelPackage())

                {
                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("FichasAportes");
                    ws.Cells["A" + 1].Value = "Tipo ID";
                    ws.Cells["B" + 1].Value = "Numero Cuenta";
                    ws.Cells["C" + 1].Value = "Identificacion";
                    ws.Cells["D" + 1].Value = "Forma de Pago";
                    ws.Cells["E" + 1].Value = "% Cuota";
                    ws.Cells["F" + 1].Value = "Cuota";
                    ws.Cells["G" + 1].Value = "Saldo Aportes";
                    ws.Cells["H" + 1].Value = "Fecha de Inicio";
                    ws.Cells["I" + 1].Value = "Estado";
                    int j = 3;

                    foreach (var item in Aporte)
                    {
                        var estado = "";
                        ws.Cells["A" + j].Value = item.Aid;
                        ws.Cells["B" + j].Value = item.nC;
                        ws.Cells["C" + j].Value = item.Ip;
                        ws.Cells["D" + j].Value = item.Fp;
                        ws.Cells["E" + j].Value = item.P;
                        ws.Cells["F" + j].Value = item.Vc;
                        ws.Cells["G" + j].Value = item.Ta;
                        ws.Cells["H" + j].Value = item.Fa.Value.ToShortDateString();
                        if (item.Ea == false)
                        {
                            estado = "INACTIVO";
                        }
                        else
                        {
                            estado = "ACTIVO";
                        }

                        ws.Cells["I" + j].Value = estado;
                        j++;
                    }

                    var ms = new System.IO.MemoryStream();
                    pack.SaveAs(ms);
                    ms.WriteTo(Response.OutputStream);
                }
                //Response.Flush();
                Response.End();

                return RedirectToAction("../Informes/Index");
            }
        }
        public ActionResult ExcelAhorros()
        {
            using (var ctx = new AccountingContext())
            {
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=listadoFichasAhorros.xlsx");
                var Ahorro = ctx.FichasAhorros.Select(j => new
                {
                    ID = j.id,
                    nc = j.numeroCuenta,
                    ip = j.idPersona,
                    Tp = j.tipoPago,
                    Pah = j.porcentaje,
                    vaho = j.valorCuota,
                    Ta = j.totalAhorros,
                    Fa = j.fechaApertura,
                    a = j.activo,
                });

                using (ExcelPackage pack = new ExcelPackage())

                {
                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("FichasAhorros");
                    ws.Cells["A" + 1].Value = "Tipo ID";
                    ws.Cells["B" + 1].Value = "Numero de Ahorro";
                    ws.Cells["C" + 1].Value = "Identificacion";
                    ws.Cells["D" + 1].Value = "Forma de Pago";
                    ws.Cells["E" + 1].Value = "Porcentaje Ah";
                    ws.Cells["F" + 1].Value = "Cuota";
                    ws.Cells["G" + 1].Value = "SaldoAhorros";
                    ws.Cells["H" + 1].Value = "Fecha de Inicio";
                    ws.Cells["I" + 1].Value = "Estado";
                    int j = 2;

                    foreach (var item in Ahorro)
                    {
                        ws.Cells["A" + j].Value = item.ID;
                        ws.Cells["B" + j].Value = item.nc;
                        ws.Cells["c" + j].Value = item.ip;
                        ws.Cells["D" + j].Value = item.Tp;
                        ws.Cells["E" + j].Value = item.Pah;
                        ws.Cells["F" + j].Value = item.vaho;
                        ws.Cells["G" + j].Value = item.Ta;
                        ws.Cells["H" + j].Value = item.Fa.Value.ToShortDateString();
                        ws.Cells["I" + j].Value = item.a;


                        j++;
                    }

                    var ms = new System.IO.MemoryStream();
                    pack.SaveAs(ms);
                    ms.WriteTo(Response.OutputStream);
                }
                //Response.Flush();
                Response.End();
                return RedirectToAction("../Informes/Index");
            }
        }


        public ActionResult ExcelPuc()
        {
            using (var ctx = new AccountingContext())
            {
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=CatalogCuentas.xlsx");
                //var saldo = (from mov in ctx.Movimientos
                //             join cuen in ctx.PlanCuentas on mov.CUENTA equals cuen.CODIGO
                //             select new { mov, cuen }).ToList();
                //var PUC = ctx.PlanCuentas.Select(j => new
                //{
                //    c = j.CODIGO,
                //    N = j.NOMBRE,
                //    s = j.Saldo,

                //});
                var PUC = ctx.PlanCuentas.ToList();


                using (ExcelPackage pack = new ExcelPackage())

                {
                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("Catalogo de Cuentas");
                    ws.Cells["A" + 1].Value = "CODIGO";
                    ws.Cells["B" + 1].Value = "NOMBRE";
                    ws.Cells["c" + 1].Value = "SALDO";
                    int j = 2;

                    foreach (var item in PUC)
                    {

                        ws.Cells["A" + j].Value = item.CODIGO;
                        ws.Cells["B" + j].Value = item.NOMBRE;
                        ws.Cells["c" + j].Value = item.Saldo;


                        j++;
                    }

                    var ms = new System.IO.MemoryStream();
                    pack.SaveAs(ms);
                    ms.WriteTo(Response.OutputStream);
                }
                //Response.Flush();
                Response.End();
                PUC = null;
                return RedirectToAction("../Informes/Index");
            }
        }


        private decimal GetSaldos(int ano, int mes, String cuenta, String tercero)
        {
            using (var ctx = new AccountingContext())
            {
                var saldos = (from mov in ctx.Movimientos
                              join cuen in ctx.PlanCuentas on mov.CUENTA equals cuen.CODIGO
                              select new { mov, cuen }).ToList();

                var saldos2 = (from mov in ctx.Movimientos
                               join cuen in ctx.PlanCuentas on mov.CUENTA equals cuen.CODIGO
                               select new { mov, cuen }).ToList();

                if (tercero != "")
                {
                    saldos = saldos.Where(s => s.mov.TERCERO == tercero).ToList();
                    saldos2 = saldos.Where(s => s.mov.TERCERO == tercero).ToList();
                }

                saldos = saldos.Where(s => s.mov.CUENTA == cuenta).ToList();
                saldos2 = saldos.Where(s => s.mov.CUENTA == cuenta).ToList();

                saldos2.Clear();

                if (mes != 0)
                {
                    saldos = saldos.Where(s => (s.mov.FECHAMOVIMIENTO.Year == ano && s.mov.FECHAMOVIMIENTO.Month < mes) || s.mov.FECHAMOVIMIENTO.Year < ano).ToList();
                    foreach (var item in saldos)
                    {

                        bool esAnulado = (from pc in ctx.Comprobantes where pc.TIPO == item.mov.TIPO && pc.NUMERO == item.mov.NUMERO select pc.ANULADO).Single();
                        if (esAnulado == false)
                        {
                            saldos2.Add(item);
                        }
                    }
                }
                else
                {
                    saldos = saldos.Where(s => s.mov.FECHAMOVIMIENTO.Year < ano).ToList();
                    foreach (var item in saldos)
                    {
                        bool esAnulado = (from pc in ctx.Comprobantes where pc.TIPO == item.mov.TIPO && pc.NUMERO == item.mov.NUMERO select pc.ANULADO).Single();
                        if (esAnulado == false)
                        {
                            saldos2.Add(item);
                        }
                    }
                }

                var movs = saldos2.GroupBy(s => new { s.mov.CUENTA, s.cuen.NATURALEZA }).Select(s => new { cuenta = s.Key.CUENTA, naturaleza = s.Key.NATURALEZA, debito = s.Sum(sc => sc.mov.DEBITO), credito = s.Sum(sc => sc.mov.CREDITO) }).ToList();

                decimal saldo = 0;
                foreach (var item in movs)
                {
                    //var anulado = (from pc in ctx.Comprobantes where pc.TIPO == item. select pc.Lineas_Id).Single();

                    if (item.naturaleza == "D")
                    {
                        saldo = saldo + item.debito - item.credito;
                    }
                    else
                    {
                        saldo = saldo + item.credito - item.debito;
                    }
                }

                return saldo;

            }
        }
    }

    public class Formato
    { }
}