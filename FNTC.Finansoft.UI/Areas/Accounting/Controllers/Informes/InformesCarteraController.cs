using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.Informes;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace FNTC.Finansoft.UI.Areas.Accounting.Controllers.Informes
{
    public class InformesCarteraController : Controller
    {
        public IEnumerable<object> saldo { get; private set; }
        AccountingContext db = new AccountingContext();

        //comentario de prueba git token


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
        public ActionResult Getyears()
        {
            using (var ctx = new AccountingContext())
            {
                var result = ctx.SaldosCuentas.Select(s => s.ANO).Distinct().OrderBy(x => x);
                return Json(new { years = result.ToList() }, JsonRequestBehavior.AllowGet);
            }
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

        [HttpPost]
        public ActionResult ExcelSaldos(FormCollection coll)
        {
            // Do some stuff
            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;

            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";

            //var anioInicio = Int32.Parse(coll["year2"]);
            //var hastaMes = Int32.Parse(coll["mes2"]);
            var fechaDesde = coll["fechaDesde"];
            var fechaHasta = coll["fechaHasta"];
            var chktodos = coll["chkTodos"];
            //var fechaDesde3 = coll["fechaDesde3"];
            //var fechaHasta3 = coll["fechaHasta3"];
            var chkfechaDesembolso = coll["chkFechaDesembolso"];
            var desdeSaldo = Int32.Parse(coll["monthini"]);
            var desdeSaldoAno = Int32.Parse(coll["year"]);
            var hastaSaldo = Int32.Parse(coll["monthfin"]);
            var informe = Int32.Parse(coll["informe"]);
            var cuenta = coll["Cuenta"];
            var documento = coll["Tercero"];
            var archivo = "";
            var ctx = new AccountingContext();
            if (informe == 1) archivo = "attachment;filename=creditos.xlsx";
            else if (informe == 2) archivo = "attachment;filename=creditos.xlsx";
            else if (informe == 3) archivo = "attachment;filename=reportedepago.xlsx";
            else if (informe == 4) archivo = "attachment;filename=cartera.xlsx";
            else if (informe == 5) archivo = "attachment;filename=recaudo.xlsx";
            else if (informe == 6) archivo = "attachment;filename=recaudoxpagare.xlsx";
            else archivo = "attachment;filename=balanceGeneral.xlsx";


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
                ExcelWorksheet ws = null;
                int j = 0, i = 0;
                decimal saldo = 0, debito = 0, credito = 0;
                switch (informe)
                {
                    
                    case 1:
                        #region informe36 creditos
                        var datosPrestamos = db.Prestamos.ToList(); //datos de la tabla prestamos 
                        var datosCredito = db.Creditos.ToList(); //datos de la tabla Bcreditos

                        if (chkfechaDesembolso != "on")
                        {
                            #region filtro normal

                            var creditos = new List<HistorialCreditos>();


                            if (fechaDesde != "" && fechaHasta != "")
                            {
                                DateTime fh = Convert.ToDateTime(fechaHasta);
                                DateTime fd = Convert.ToDateTime(fechaDesde);
                                DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                                DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                                creditos = db.HistorialCreditos.Where(x => x.fecha >= fechDesde && x.fecha <= fechHasta).ToList();
                            }
                            else if (fechaDesde != "" && fechaHasta == "")
                            {
                                DateTime fd = Convert.ToDateTime(fechaDesde);
                                DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                                creditos = db.HistorialCreditos.Where(x => x.fecha >= fechDesde).ToList();
                            }
                            else if (fechaDesde == "" && fechaHasta != "")
                            {
                                DateTime fh = Convert.ToDateTime(fechaHasta);
                                DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                                creditos = db.HistorialCreditos.Where(x => x.fecha <= fechHasta).ToList();
                            }


                            ws = pack.Workbook.Worksheets.Add("creditos");
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

                            j = 2;

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


                            if (fechaDesde != "" && fechaHasta != "")
                            {
                                DateTime fh = Convert.ToDateTime(fechaHasta);
                                DateTime fd = Convert.ToDateTime(fechaDesde);
                                DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                                DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                                creditos = db.Prestamos.Where(x => x.fechadesembolso >= fechDesde && x.fechadesembolso <= fechHasta).ToList();
                            }
                            else if (fechaDesde != "" && fechaHasta == "")
                            {
                                DateTime fd = Convert.ToDateTime(fechaDesde);
                                DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                                creditos = db.Prestamos.Where(x => x.fechadesembolso >= fechDesde).ToList();
                            }
                            else if (fechaDesde == "" && fechaHasta != "")
                            {
                                DateTime fh = Convert.ToDateTime(fechaHasta);
                                DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                                creditos = db.Prestamos.Where(x => x.fechadesembolso <= fechHasta).ToList();
                            }



                            ws = pack.Workbook.Worksheets.Add("creditos");
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

                            j = 2;

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
                        datosCredito = null;
                        datosPrestamos = null;
                        #endregion
                        break;
                    case 3:
                        #region informe 3 pagos diarios
                        
                        if (chkfechaDesembolso != "on")
                        {
                            #region filtro normal

                            var Pagos = new List<factOpCajaConsCuotaCredito>();
                            

                            if (fechaDesde != "" && fechaHasta != "")
                            {
                                DateTime fh = Convert.ToDateTime(fechaHasta);
                                DateTime fd = Convert.ToDateTime(fechaDesde);
                                DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                                DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                                Pagos = db.factOpCajaConsCuotaCredito.Where(x => x.fecha >= fechDesde && x.fecha <= fechHasta).OrderBy(x => x.fecha).ToList();
                            }
                            else if (fechaDesde != "" && fechaHasta == "")
                            {
                                DateTime fd = Convert.ToDateTime(fechaDesde);
                                DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                                Pagos = db.factOpCajaConsCuotaCredito.Where(x => x.fecha >= fechDesde).OrderBy(x => x.fecha).ToList();
                            }
                            else if (fechaDesde == "" && fechaHasta != "")
                            {
                                DateTime fh = Convert.ToDateTime(fechaHasta);
                                DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                                Pagos = db.factOpCajaConsCuotaCredito.Where(x => x.fecha <= fechHasta).OrderBy(x => x.fecha).ToList();
                            }

                            
                            ws = pack.Workbook.Worksheets.Add("Pagos Diarios");
                            ws.Cells["A" + 1].Value = "Item";
                            ws.Cells["B" + 1].Value = "# Crédito";
                            ws.Cells["C" + 1].Value = "No. Recibo";
                            ws.Cells["D" + 1].Value = "Nombre";
                            ws.Cells["E" + 1].Value = "Cédula";
                            ws.Cells["F" + 1].Value = "Fecha de pago";
                            ws.Cells["G" + 1].Value = "Tipo pago";
                            ws.Cells["H" + 1].Value = "Vr. Total pagado";
                            ws.Cells["I" + 1].Value = "N° Días mora";
                            ws.Cells["J" + 1].Value = "Vr. Pagado Intereses mora";
                            ws.Cells["K" + 1].Value = "Vr. Pagado Intereses corrientes";
                            ws.Cells["L" + 1].Value = "Seguró";
                            ws.Cells["M" + 1].Value = "Vr. Pagado Capital";
                            ws.Cells["N" + 1].Value = "Saldo Capital Real ";
                           
                            j = 2;

                            if (Pagos.Count > 0)
                            {/*
                                var Pagos1 = from d in Pagos
                                             group d by d.fecha into fechas
                                             select new
                                             { 
                                             pro=  fechas.Key,
                                             SaldoCapitalP=fechas.Sum(c => Convert.ToDecimal(c.saldoCapital))

                                             };
                                */
                              
                                var pagostotaldia = Pagos.GroupBy(x => new { fecha = x.fecha.ToShortDateString()}).Select(x => new
                                {
                                    seguros = x.Sum(s => Convert.ToDecimal(s.seguros)),
                                    
                                    saldoCapital = x.Sum(c => Convert.ToDecimal(c.saldoCapital)),
                                    valorConsignado = x.Sum(y => Convert.ToDecimal(y.valorConsignado)),
                                    interesMora= x.Sum(y => Convert.ToDecimal(y.interesMora)),
                                    interesCorriente = x.Sum(y => Convert.ToDecimal(y.interesCorriente)),
                                    abonoCapital = x.Sum(y => Convert.ToDecimal(y.abonoCapital)),
                                    fechas = x.Key.fecha

                                }).ToList();

                                var fechainicial = Pagos[0].fecha.ToShortDateString();
                                decimal  Avalorconsignado = 0;
                                decimal Ainteresmora = 0;
                                decimal Ainteresescorriente = 0;
                                decimal Aseguros = 0;
                                decimal Aabonocapital = 0;
                                decimal Asaldocapital = 0;
                                foreach (var item in Pagos)
                                {
                                    

                                    
                                    if (fechainicial == item.fecha.ToShortDateString())
                                    {
                                        Avalorconsignado += Convert.ToDecimal(item.valorConsignado);
                                        Ainteresmora += Convert.ToDecimal(item.interesMora);
                                        Ainteresescorriente += Convert.ToDecimal(item.interesCorriente);
                                        Aseguros += Convert.ToDecimal(item.seguros);
                                        Aabonocapital += Convert.ToDecimal(item.abonoCapital);
                                        Asaldocapital += Convert.ToDecimal(item.saldoCapital);


                                    }
                                    if (Avalorconsignado != 0)
                                    {
                                        if (fechainicial != item.fecha.ToShortDateString())
                                    {
                                            ws.Cells["D" + j].Value = "   TOTAL " + fechainicial;
                                        ws.Cells["H" + j].Value = Avalorconsignado.ToString("N0", formato);

                                            ws.Cells["J" + j].Value = Ainteresmora.ToString("N0", formato);
                                            ws.Cells["K" + j].Value = Ainteresescorriente.ToString("N0", formato);
                                            ws.Cells["L" + j].Value = Aseguros.ToString("N0", formato);
                                            ws.Cells["M" + j].Value = Aabonocapital.ToString("N0", formato); 
                                            ws.Cells["N" + j].Value = Asaldocapital.ToString("N0", formato);



                                            fechainicial = item.fecha.ToShortDateString();
                                        Avalorconsignado = Convert.ToDecimal(item.valorConsignado);
                                            Ainteresmora = Convert.ToDecimal(item.interesMora);
                                            Ainteresescorriente = Convert.ToDecimal(item.interesCorriente);
                                            Aseguros = Convert.ToDecimal(item.seguros);
                                            Aabonocapital = Convert.ToDecimal(item.abonoCapital);
                                            Asaldocapital = Convert.ToDecimal(item.saldoCapital);
                                            j++;
                                        }
                                    }
                                    var diamora = db.TotalesCreditos.Where(x => x.Pagare ==item.pagare).Select(x => x.DiasMora);
                                    ws.Cells["A" + j].Value = (j-1);
                                    ws.Cells["B" + j].Value = item.pagare.ToString();
                                    ws.Cells["C" + j].Value = item.factura.ToString(); 
                                    ws.Cells["D" + j].Value = item.Terceros.NOMBRE1 + " " + item.Terceros.NOMBRE2 + " " + item.Terceros.APELLIDO1 + " " + item.Terceros.APELLIDO2; 
                                    ws.Cells["E" + j].Value = item.NIT.ToString();
                                    ws.Cells["F" + j].Value = item.fecha.ToString("yyyy-MM-dd"); 
                                    ws.Cells["G" + j].Value = item.FormaPago.ToString();
                                    ws.Cells["H" + j].Value = Convert.ToDecimal(item.valorConsignado).ToString("N0",formato);
                                    ws.Cells["I" + j].Value = diamora;// dias mora
                                    ws.Cells["J" + j].Value = Convert.ToDecimal(item.interesMora).ToString("N0", formato);
                                    ws.Cells["K" + j].Value = Convert.ToDecimal(item.interesCorriente).ToString("N0", formato);
                                    ws.Cells["L" + j].Value = Convert.ToDecimal(item.seguros).ToString("N0", formato);
                                    ws.Cells["M" + j].Value = Convert.ToDecimal(item.abonoCapital).ToString("N0", formato);
                                    ws.Cells["N" + j].Value = Convert.ToDecimal(item.saldoCapital).ToString("N0", formato);


                                    j++;
                                    ws.Cells[ws.Dimension.Address].AutoFitColumns();//siempre al final de todo. le da tamaño ajustado a cada columna

                                }//fin foreach
                                ws.Cells["D" + j].Value = "   TOTAL " + fechainicial;
                                ws.Cells["H" + j].Value = Avalorconsignado.ToString("N0", formato);
                                ws.Cells["J" + j].Value = Ainteresmora.ToString("N0", formato);
                                ws.Cells["K" + j].Value = Ainteresescorriente.ToString("N0", formato);
                                ws.Cells["L" + j].Value = Aseguros.ToString("N0", formato);
                                ws.Cells["M" + j].Value = Aabonocapital.ToString("N0", formato);
                                ws.Cells["N" + j].Value = Asaldocapital.ToString("N0", formato);

                                j++;
                                j++;

                                
                                /*
                                ws.Cells["D" + j].Value = "TOTALES RECAUDO CUOTAS";
                                ws.Cells["D" + (j+1)].Value = "SUBTOTAL RECAUDO CUOTAS";
                                ws.Cells["D" + (j + 2)].Value = "SUBTOTAL INTERES MORA";
                                ws.Cells["D" + (j + 3)].Value = "SUBTOTAL INTERES CORRIENTE";
                                ws.Cells["D" + (j + 4)].Value = "SUBTOTAL SEGURO";
                                ws.Cells["D" + (j + 5)].Value = "SUBTOTAL VR PAG CAPITAL";
                                ws.Cells["E" + (j+1)].Value = Pagos.Sum(y => Convert.ToDecimal(y.interesMora)).ToString();
                                ws.Cells["E" + (j+2)].Value = Pagos.Sum(y => Convert.ToDecimal(y.interesCorriente)).ToString();
                                ws.Cells["E" + (j+3)].Value = Pagos.Sum(y => Convert.ToDecimal(y.seguros)).ToString();
                                ws.Cells["E" + (j+4)].Value = Pagos.Sum(y => Convert.ToDecimal(y.abonoCapital)).ToString();
                                ws.Cells["E" + (j+5)].Value = Pagos.Sum(y => Convert.ToDecimal(y.saldoCapital)).ToString();
                                */
                                ws.Cells["H" + j].Value = "Fecha";
                                ws.Cells["I" + j].Value = "Vr. Total pagado";
                                ws.Cells["J" + j].Value = "Vr. Pagado Intereses mora";
                                ws.Cells["K" + j].Value = "Vr. Pagado Intereses corrientes";
                                ws.Cells["L" + j].Value = "Seguró";
                                ws.Cells["M" + j].Value = "Vr. Pagado Capital";
                                ws.Cells["N" + j].Value = "Saldo Capital Real ";
                                j = j + 1;

                                foreach (var item in pagostotaldia)
                                {


                                    ws.Cells["H" + j].Value = item.fechas ;
                                    ws.Cells["I" + j].Value = item.valorConsignado.ToString("N0", formato);
                                    
                                    ws.Cells["J" + j].Value = item.interesMora.ToString("N0", formato);
                                    ws.Cells["K" + j].Value = item.interesCorriente.ToString("N0", formato);
                                    ws.Cells["L" + j].Value = item.seguros.ToString("N0", formato);
                                    ws.Cells["M" + j].Value = item.abonoCapital.ToString("N0", formato);
                                    ws.Cells["N" + j].Value = item.saldoCapital.ToString("N0", formato);




                                    j++;
                                    ws.Cells[ws.Dimension.Address].AutoFitColumns();//siempre al final de todo. le da tamaño ajustado a cada columna

                                }//fin foreach
                                
                                ws.Cells["H" + j].Value = "Totales";
                                ws.Cells["I" + j].Value = Pagos.Sum(y => Convert.ToDecimal(y.valorConsignado)).ToString("N0", formato);
                                //ws.Cells["I" + j].Value = "Abono Costos Adicionales";
                                ws.Cells["J" + j].Value = Pagos.Sum(y => Convert.ToDecimal(y.interesMora)).ToString("N0", formato);
                                ws.Cells["K" + j].Value = Pagos.Sum(y => Convert.ToDecimal(y.interesCorriente)).ToString("N0", formato);
                                ws.Cells["L" + j].Value = Pagos.Sum(y => Convert.ToDecimal(y.seguros)).ToString("N0", formato);
                                ws.Cells["M" + j].Value = Pagos.Sum(y => Convert.ToDecimal(y.abonoCapital)).ToString("N0", formato);
                                ws.Cells["N" + j].Value = Pagos.Sum(y => Convert.ToDecimal(y.saldoCapital)).ToString("N0", formato);
                                


                            }//fin if != null
                            
                            #endregion filtro normal
                        }

                        #endregion

                        break;
                    case 4:
                        

                        break;
                    case 5:
                        #region informe 5 informe recaudo

                        if (chkfechaDesembolso != "on")
                        {
                            #region filtro normal

                            var Pagos = new List<factOpCajaConsCuotaCredito>();


                            if (fechaDesde != "" && fechaHasta != "")
                            {
                                DateTime fh = Convert.ToDateTime(fechaHasta);
                                DateTime fd = Convert.ToDateTime(fechaDesde);
                                DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                                DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                                Pagos = db.factOpCajaConsCuotaCredito.Where(x => x.fecha >= fechDesde && x.fecha <= fechHasta).ToList();
                            }
                            else if (fechaDesde != "" && fechaHasta == "")
                            {
                                DateTime fd = Convert.ToDateTime(fechaDesde);
                                DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                                Pagos = db.factOpCajaConsCuotaCredito.Where(x => x.fecha >= fechDesde).ToList();
                            }
                            else if (fechaDesde == "" && fechaHasta != "")
                            {
                                DateTime fh = Convert.ToDateTime(fechaHasta);
                                DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                                Pagos = db.factOpCajaConsCuotaCredito.Where(x => x.fecha <= fechHasta).ToList();
                            }


                            ws = pack.Workbook.Worksheets.Add("Pagos Diarios");
                            ws.Cells["A" + 1].Value = "Item";
                            ws.Cells["B" + 1].Value = "# Crédito";
                            ws.Cells["C" + 1].Value = "No. Recibo";
                            ws.Cells["D" + 1].Value = "Nombre";
                            ws.Cells["E" + 1].Value = "Cédula";
                            ws.Cells["F" + 1].Value = "Fecha de pago";
                            ws.Cells["G" + 1].Value = "Tipo pago";
                            ws.Cells["H" + 1].Value = "Vr. Total pagado";
                            ws.Cells["I" + 1].Value = "N° Días mora";
                            ws.Cells["J" + 1].Value = "Vr. Pagado Intereses mora";
                            ws.Cells["K" + 1].Value = "Vr. Pagado Intereses corrientes";
                            ws.Cells["L" + 1].Value = "Seguró";
                            ws.Cells["M" + 1].Value = "Vr. Pagado Capital";
                            ws.Cells["N" + 1].Value = "Saldo Capital Real ";

                            j = 2;

                            if (Pagos != null)
                            {

                                foreach (var item in Pagos)
                                {
                                    var diamora = db.TotalesCreditos.Where(x => x.Pagare == item.pagare).Select(x => x.DiasMora);
                                    ws.Cells["A" + j].Value = (j - 1);
                                    ws.Cells["B" + j].Value = item.pagare.ToString();
                                    ws.Cells["C" + j].Value = item.factura.ToString();
                                    ws.Cells["D" + j].Value = item.Terceros.NOMBRE1 + " " + item.Terceros.NOMBRE2 + " " + item.Terceros.APELLIDO1 + " " + item.Terceros.APELLIDO2;
                                    ws.Cells["E" + j].Value = item.NIT.ToString();
                                    ws.Cells["F" + j].Value = item.fecha.ToString("yyyy-MM-dd");
                                    ws.Cells["G" + j].Value = item.FormaPago.ToString();//tipo de pago
                                    ws.Cells["H" + j].Value = item.valorConsignado.ToString();
                                    ws.Cells["I" + j].Value = diamora;// dias mora
                                    ws.Cells["J" + j].Value = item.interesMora.ToString();
                                    ws.Cells["K" + j].Value = item.interesCorriente.ToString();
                                    ws.Cells["L" + j].Value = item.seguros.ToString();
                                    ws.Cells["M" + j].Value = item.abonoCapital.ToString();
                                    ws.Cells["N" + j].Value = item.saldoCapital.ToString();




                                    j++;

                                }//fin foreach
                                j++;


                                ws.Cells["G" + j].Value = "Totales";

                                ws.Cells["H" + j].Value = Pagos.Sum(y => Convert.ToDecimal(y.valorConsignado));
                                //ws.Cells["I" + j].Value = "Abono Costos Adicionales";
                                ws.Cells["J" + j].Value = Pagos.Sum(y => Convert.ToDecimal(y.interesMora));
                                ws.Cells["K" + j].Value = Pagos.Sum(y => Convert.ToDecimal(y.interesCorriente));
                                ws.Cells["L" + j].Value = Pagos.Sum(y => Convert.ToDecimal(y.seguros));
                                ws.Cells["M" + j].Value = Pagos.Sum(y => Convert.ToDecimal(y.abonoCapital));
                                ws.Cells["N" + j].Value = Pagos.Sum(y => Convert.ToDecimal(y.saldoCapital));
                                j++;
                                ws.Cells["D" + (j + 1)].Value = "SUBTOTAL RECAUDO CUOTAS";
                                ws.Cells["D" + (j + 2)].Value = "SUBTOTAL INTERES MORA";
                                ws.Cells["D" + (j + 3)].Value = "SUBTOTAL INTERES CORRIENTE";
                                ws.Cells["D" + (j + 4)].Value = "SUBTOTAL SEGURO";
                                ws.Cells["D" + (j + 5)].Value = "SUBTOTAL VR PAG CAPITAL";
                                ws.Cells["E" + (j + 1)].Value = Pagos.Sum(y => Convert.ToDecimal(y.interesMora));
                                ws.Cells["E" + (j + 2)].Value = Pagos.Sum(y => Convert.ToDecimal(y.interesCorriente));
                                ws.Cells["E" + (j + 3)].Value = Pagos.Sum(y => Convert.ToDecimal(y.seguros));
                                ws.Cells["E" + (j + 4)].Value = Pagos.Sum(y => Convert.ToDecimal(y.abonoCapital));
                                ws.Cells["E" + (j + 5)].Value = Pagos.Sum(y => Convert.ToDecimal(y.saldoCapital));


                                //                                ws.Cells["X" + j].Value = TinteresMoraPendiente.ToString("N0", formato);

                            }//fin if != null

                            #endregion filtro normal
                        }

                        datosCredito = null;
                        datosPrestamos = null;
                        #endregion

                        break;
                    case 6:
                        #region informe 6 informe recaudo x pagare

                        if (chkfechaDesembolso != "on")
                        {
                            #region filtro normal

                            var Pagos = new List<factOpCajaConsCuotaCredito>();


                            if (fechaDesde != "" && fechaHasta != "")
                            {
                                DateTime fh = Convert.ToDateTime(fechaHasta);
                                DateTime fd = Convert.ToDateTime(fechaDesde);
                                DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                                DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                                Pagos = db.factOpCajaConsCuotaCredito.Where(x => x.fecha >= fechDesde && x.fecha <= fechHasta).ToList();
                            }
                            else if (fechaDesde != "" && fechaHasta == "")
                            {
                                DateTime fd = Convert.ToDateTime(fechaDesde);
                                DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                                Pagos = db.factOpCajaConsCuotaCredito.Where(x => x.fecha >= fechDesde).ToList();
                            }
                            else if (fechaDesde == "" && fechaHasta != "")
                            {
                                DateTime fh = Convert.ToDateTime(fechaHasta);
                                DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                                Pagos = db.factOpCajaConsCuotaCredito.Where(x => x.fecha <= fechHasta).ToList();
                            }





                            if (Pagos.Count > 0)
                            {
                                var pagares = Pagos.OrderBy(x => x.pagare).Select(x => x.pagare).Distinct().ToList();

                                foreach (var pagare in pagares)
                                {
                                    var pagosporpagare = Pagos.Where(x => x.pagare == pagare).OrderBy(x => x.fecha).ToList();
                                    ws = pack.Workbook.Worksheets.Add(pagare);
                                    ws.Cells["A" + 1].Value = "Item";
                                    ws.Cells["B" + 1].Value = "# Crédito";
                                    ws.Cells["C" + 1].Value = "No. Recibo";
                                    ws.Cells["D" + 1].Value = "Nombre";
                                    ws.Cells["E" + 1].Value = "Cédula";
                                    ws.Cells["F" + 1].Value = "Fecha de pago";
                                    ws.Cells["G" + 1].Value = "Tipo pago";
                                    ws.Cells["H" + 1].Value = "Vr. Total pagado";
                                    ws.Cells["I" + 1].Value = "N° Días mora";
                                    ws.Cells["J" + 1].Value = "Vr. Pagado Intereses mora";
                                    ws.Cells["K" + 1].Value = "Vr. Pagado Intereses corrientes";
                                    ws.Cells["L" + 1].Value = "Seguró";
                                    ws.Cells["M" + 1].Value = "Vr. Pagado Capital";
                                    ws.Cells["N" + 1].Value = "Saldo Capital Real ";

                                    j = 2;

                                    foreach (var item in pagosporpagare)
                                    {
                                        var diamora = db.TotalesCreditos.Where(x => x.Pagare == item.pagare).Select(x => x.DiasMora);
                                        ws.Cells["A" + j].Value = (j - 1);
                                        ws.Cells["B" + j].Value = item.pagare.ToString();
                                        ws.Cells["C" + j].Value = item.factura.ToString();
                                        ws.Cells["D" + j].Value = item.Terceros.NOMBRE1 + " " + item.Terceros.NOMBRE2 + " " + item.Terceros.APELLIDO1 + " " + item.Terceros.APELLIDO2;
                                        ws.Cells["E" + j].Value = item.NIT.ToString();
                                        ws.Cells["F" + j].Value = item.fecha.ToString("yyyy-MM-dd");
                                        ws.Cells["G" + j].Value = item.FormaPago.ToString();//tipo de pago
                                        ws.Cells["H" + j].Value = item.valorConsignado.ToString();
                                        ws.Cells["I" + j].Value = diamora;// dias mora
                                        ws.Cells["J" + j].Value = item.interesMora.ToString();
                                        ws.Cells["K" + j].Value = item.interesCorriente.ToString();
                                        ws.Cells["L" + j].Value = item.seguros.ToString();
                                        ws.Cells["M" + j].Value = item.abonoCapital.ToString();
                                        ws.Cells["N" + j].Value = item.saldoCapital.ToString();


                                        j++;



                                    }

                                }//fin foreach
                                j++;

                                

                            }//fin if != null
                            else
                            {
                                ws = pack.Workbook.Worksheets.Add("Hoja1");
                                ws.Cells["A" + 1].Value = "";
                            }

                            #endregion filtro normal
                        }

                        datosCredito = null;
                        datosPrestamos = null;
                        #endregion

                        break;
                    default:
                        ws = pack.Workbook.Worksheets.Add("balanceGeneral");
                        ws.Cells["A" + 1].Value = "BALANCE";
                        ws.Cells["B" + 1].Value = "GENERAL";
                        ws.Cells["C" + 1].Value = "COOOL";
                        ws.Cells["A" + 3].Value = "CUENTA_ACT";
                        ws.Cells["B" + 3].Value = "NOM_ACT";
                        ws.Cells["C" + 3].Value = "VAL_ACT";
                        ws.Cells["D" + 3].Value = "NAT_ACT";
                        ws.Cells["E" + 3].Value = "CUENTA_PAS";
                        ws.Cells["F" + 3].Value = "NOM_PAS";
                        ws.Cells["G" + 3].Value = "VAL_PAS";
                        ws.Cells["H" + 3].Value = "NAT_PAS";
                        break;
                }


                var ms = new System.IO.MemoryStream();
                pack.SaveAs(ms);
                ms.WriteTo(Response.OutputStream);
            }
            //Response.Flush();
            Response.End();

            return RedirectToAction("../Informes/Index");

        }

        public ActionResult ExcelRSVFF()
        {
            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;

            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            DateTime FechaActual = DateTime.Now;
            string Mes = GetMes(FechaActual.Month);
            int Year = FechaActual.Year;
            decimal Total = 0,SaldoCapital=0;
            int edad = 0;

            var DatosCreditos = db.Creditos.ToList();
            var TotalesCreditos = db.TotalesCreditos.ToList();

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=RelacionSegurosDeVida.xlsx");

            using(ExcelPackage pack = new ExcelPackage())
            {
                ExcelWorksheet ws = pack.Workbook.Worksheets.Add("Hoja1");
                ws.Cells["B" + 1].Value = "RELACIÓN SEGUROS DE VIDA FONDO DE FINANCIAMIENTO " +Mes+" "+Year ;
                ws.Cells["B1:G1"].Merge = true;

                ws.Cells["B" + 2].Value = "Cod. CREDITO";
                ws.Cells["C" + 2].Value = "NOMBRE ASEGURADO";
                ws.Cells["D" + 2].Value = "DOCUMENTO No.";
                ws.Cells["E" + 2].Value = "FECHA DE NACIMIENTO";
                ws.Cells["F" + 2].Value = "EDAD";
                ws.Cells["G" + 2].Value = "VALOR ASEGURADO";

                int j = 3;


                foreach(var item in DatosCreditos)
                {
                    if (item.terceroFK != null && item.terceroFK.FECHANAC!=null) {
                        var OldDate = Convert.ToDateTime(item.terceroFK.FECHANAC);
                        var DateSpan = DateTimeSpan.CompareDates(OldDate,DateTime.Now);
                        edad = DateSpan.Years;
                    }

                    SaldoCapital = TotalesCreditos.Where(x=> x.Pagare==item.Pagare).Select(x => x.SaldoCapital).FirstOrDefault();
                    Total += SaldoCapital;

                    ws.Cells["B" + j].Value = item.Pagare;
                    ws.Cells["C" + j].Value = (item.terceroFK!=null) ? item.terceroFK.NombreComercial+" "+ item.terceroFK.NOMBRE1 + " "+ item.terceroFK.NOMBRE2 + " "+ item.terceroFK.APELLIDO1 + " "+ item.terceroFK.APELLIDO2 + " " : "";
                    ws.Cells["D" + j].Value = item.Creditos_Cedula;
                    ws.Cells["E" + j].Value = (item.terceroFK != null && item.terceroFK.FECHANAC != null) ? Convert.ToDateTime(item.terceroFK.FECHANAC).ToString("yyyy-MM-dd") : "";
                    ws.Cells["F" + j].Value = edad.ToString();
                    ws.Cells["G" + j].Value = SaldoCapital.ToString("N0",formato);
                    j++;
                }

                j += 2;
                ws.Cells["F" + j].Value = "TOTAL";
                ws.Cells["G" + j].Value = Total.ToString("N0", formato);

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                var ms = new System.IO.MemoryStream();
                pack.SaveAs(ms);
                ms.WriteTo(Response.OutputStream);
            }
            Response.End();

            return RedirectToAction("../InformesCartera/Index");
        }

        public ActionResult Excelpagos(FormCollection coll)
        {
            // Do some stuff
            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;

            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";

            var archivo = "";
            var ctx = new AccountingContext();
            archivo = "attachment;filename=cartera.xlsx";


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
                ExcelWorksheet ws = null;
                int j = 0, i = 0;
                decimal saldo = 0, debito = 0, credito = 0;


                var totalescreditos = new List<TotalesCreditos>();
                totalescreditos = db.TotalesCreditos.ToList();


                ws = pack.Workbook.Worksheets.Add("Cartera");
                ws.Cells["A" + 1].Value = "Item";
                ws.Cells["B" + 1].Value = "# Crédito";
                ws.Cells["C" + 1].Value = "Fecha de vencimiento";
                ws.Cells["D" + 1].Value = "Días de Mora";
                ws.Cells["E" + 1].Value = "Saldo Capital Real";


                j = 2;

                if (totalescreditos.Count > 0 )
                {

                    foreach (var item in totalescreditos)
                    {

                        ws.Cells["A" + j].Value = (j - 1);
                        ws.Cells["B" + j].Value = item.Pagare.ToString();
                        ws.Cells["C" + j].Value = item.FechaProximoPago.ToString("yyyy-MM-dd");
                        ws.Cells["D" + j].Value = item.DiasMora.ToString();
                        ws.Cells["E" + j].Value = item.SaldoCapital.ToString("N0", formato);

                        j++;

                    }//fin foreach
                    j++;


                    ws.Cells["H" + 2].Value = "Por edad de mora";
                    ws.Cells["I" + 2].Value = "# Casos";
                    ws.Cells["J" + 2].Value = "Valor";
                    ws.Cells["K" + 2].Value = "%Creditos";
                    //ws.Cells["L" + 2].Value = "%Prestado";
                    //ws.Cells["M" + 2].Value = "%Total";

                    ws.Cells["H" + 3].Value = "Reactivados al día";
                    ws.Cells["H" + 4].Value = "Vencida 1 a 30 días";
                    ws.Cells["H" + 5].Value = "Vencida 31 a 60 días";
                    ws.Cells["H" + 6].Value = "Vencida 61 a 90 días";
                    ws.Cells["H" + 7].Value = "Vencida 91 a 120 días";
                    ws.Cells["H" + 8].Value = "Vencida 121 a 180 días";
                    ws.Cells["H" + 9].Value = "Vencida 181 a 360 días";
                    ws.Cells["H" + 10].Value = "Vencida mayor a 360 días";
                    ws.Cells["H" + 11].Value = "TOTAL";

                    ws.Cells["I" + 3].Value = totalescreditos.Where(x => x.DiasMora == 0).Count();
                    ws.Cells["I" + 4].Value = totalescreditos.Where(x => x.DiasMora >= 1 && x.DiasMora <= 30).Count();
                    ws.Cells["I" + 5].Value = totalescreditos.Where(x => x.DiasMora >= 31 && x.DiasMora <= 60).Count();
                    ws.Cells["I" + 6].Value = totalescreditos.Where(x => x.DiasMora >= 61 && x.DiasMora <= 90).Count();
                    ws.Cells["I" + 7].Value = totalescreditos.Where(x => x.DiasMora >= 91 && x.DiasMora <= 120).Count();
                    ws.Cells["I" + 8].Value = totalescreditos.Where(x => x.DiasMora >= 121 && x.DiasMora <= 180).Count();
                    ws.Cells["I" + 9].Value = totalescreditos.Where(x => x.DiasMora >= 181 && x.DiasMora <= 360).Count();
                    ws.Cells["I" + 10].Value = totalescreditos.Where(x => x.DiasMora >= 360).Count();
                    ws.Cells["I" + 11].Value = totalescreditos.Count();

                    ws.Cells["J" + 3].Value = totalescreditos.Where(x => x.DiasMora == 0).Sum(X => X.SaldoCapital).ToString("N0", formato);
                    ws.Cells["J" + 4].Value = totalescreditos.Where(x => x.DiasMora >= 1 && x.DiasMora <= 30).Sum(X => X.SaldoCapital).ToString("N0", formato);
                    ws.Cells["J" + 5].Value = totalescreditos.Where(x => x.DiasMora >= 31 && x.DiasMora <= 60).Sum(X => X.SaldoCapital).ToString("N0", formato);
                    ws.Cells["J" + 6].Value = totalescreditos.Where(x => x.DiasMora >= 61 && x.DiasMora <= 90).Sum(X => X.SaldoCapital).ToString("N0", formato);
                    ws.Cells["J" + 7].Value = totalescreditos.Where(x => x.DiasMora >= 91 && x.DiasMora <= 120).Sum(X => X.SaldoCapital).ToString("N0", formato);
                    ws.Cells["J" + 8].Value = totalescreditos.Where(x => x.DiasMora >= 121 && x.DiasMora <= 180).Sum(X => X.SaldoCapital).ToString("N0", formato);
                    ws.Cells["J" + 9].Value = totalescreditos.Where(x => x.DiasMora >= 181 && x.DiasMora <= 360).Sum(X => X.SaldoCapital).ToString("N0", formato);
                    ws.Cells["J" + 10].Value = totalescreditos.Where(x => x.DiasMora >= 360).Sum(X => X.SaldoCapital).ToString("N0", formato);
                    ws.Cells["J" + 11].Value = totalescreditos.Sum(X => X.SaldoCapital).ToString("N0", formato);


                    ws.Cells["K" + 3].Value = Convert.ToDecimal((totalescreditos.Where(x => x.DiasMora == 0).Count() * 100) / totalescreditos.Count()).ToString("N3", formato) + "%";
                    ws.Cells["K" + 4].Value = Convert.ToDecimal((totalescreditos.Where(x => x.DiasMora >= 1 && x.DiasMora <= 30).Count() * 100) / totalescreditos.Count()).ToString("N3", formato) + "%";
                    ws.Cells["K" + 5].Value = Convert.ToDecimal((totalescreditos.Where(x => x.DiasMora >= 31 && x.DiasMora <= 60).Count() * 100) / totalescreditos.Count()).ToString("N3", formato) + "%";
                    ws.Cells["K" + 6].Value = Convert.ToDecimal((totalescreditos.Where(x => x.DiasMora >= 61 && x.DiasMora <= 90).Count() * 100) / totalescreditos.Count()).ToString("N3", formato) + "%";
                    ws.Cells["K" + 7].Value = Convert.ToDecimal((totalescreditos.Where(x => x.DiasMora >= 91 && x.DiasMora <= 120).Count() * 100) / totalescreditos.Count()).ToString("N3", formato) + "%";
                    ws.Cells["K" + 8].Value = Convert.ToDecimal((totalescreditos.Where(x => x.DiasMora >= 121 && x.DiasMora <= 180).Count() * 100) / totalescreditos.Count()).ToString("N3", formato) + "%";
                    ws.Cells["K" + 9].Value = Convert.ToDecimal((totalescreditos.Where(x => x.DiasMora >= 181 && x.DiasMora <= 360).Count() * 100) / totalescreditos.Count()).ToString("N3", formato) + "%";
                    ws.Cells["K" + 10].Value = Convert.ToDecimal((totalescreditos.Where(x => x.DiasMora >= 360).Count() * 100) / totalescreditos.Count()).ToString("N3", formato) + "%";
                    ws.Cells["K" + 11].Value = Convert.ToDecimal((totalescreditos.Count() * 100) / totalescreditos.Count()).ToString("N3", formato) + "%";

                    /* Datos para %prestado

                    ws.Cells["L" + 3].Value = Convert.ToDecimal((totalescreditos.Where(x => x.DiasMora == 0).Sum(X => X.SaldoCapital) * 100) / totalescreditos.Sum(X => X.SaldoCapital)).ToString("N3",formato) +"%";
                    ws.Cells["L" + 4].Value = Convert.ToDecimal((totalescreditos.Where(x => x.DiasMora >= 1 && x.DiasMora <= 30).Sum(X => X.SaldoCapital) * 100) / totalescreditos.Sum(X => X.SaldoCapital)).ToString("N3", formato) + "%";
                    ws.Cells["L" + 5].Value = Convert.ToDecimal((totalescreditos.Where(x => x.DiasMora >= 31 && x.DiasMora <= 60).Sum(X => X.SaldoCapital) * 100) / totalescreditos.Sum(X => X.SaldoCapital)).ToString("N3", formato) + "%";
                    ws.Cells["L" + 6].Value = Convert.ToDecimal((totalescreditos.Where(x => x.DiasMora >= 61 && x.DiasMora <= 90).Sum(X => X.SaldoCapital) * 100) / totalescreditos.Sum(X => X.SaldoCapital)).ToString("N3", formato) + "%";
                    ws.Cells["L" + 7].Value = Convert.ToDecimal((totalescreditos.Where(x => x.DiasMora >= 91 && x.DiasMora <= 120).Sum(X => X.SaldoCapital) * 100) / totalescreditos.Sum(X => X.SaldoCapital)).ToString("N3", formato) + "%";
                    ws.Cells["L" + 8].Value = Convert.ToDecimal((totalescreditos.Where(x => x.DiasMora >= 121 && x.DiasMora <= 180).Sum(X => X.SaldoCapital) * 100) / totalescreditos.Sum(X => X.SaldoCapital)).ToString("N3", formato) + "%";
                    ws.Cells["L" + 9].Value = Convert.ToDecimal((totalescreditos.Where(x => x.DiasMora >= 181 && x.DiasMora <= 360).Sum(X => X.SaldoCapital) * 100) / totalescreditos.Sum(X => X.SaldoCapital)).ToString("N3", formato) + "%";
                    ws.Cells["L" + 10].Value = Convert.ToDecimal((totalescreditos.Where(x => x.DiasMora >= 360).Sum(X => X.SaldoCapital) * 100) / totalescreditos.Sum(X => X.SaldoCapital)).ToString("N3", formato) + "%";
                    ws.Cells["L" + 11].Value = Convert.ToDecimal((totalescreditos.Sum(X => X.SaldoCapital) * 100) / totalescreditos.Sum(X => X.SaldoCapital)).ToString("N3", formato) + "%";
                    */

                }//fin if != null


                var ms = new System.IO.MemoryStream();
                pack.SaveAs(ms);
                ms.WriteTo(Response.OutputStream);

                //Response.Flush();
                Response.End();

                return RedirectToAction("../Informes/Index");
            }
        }

    }
}