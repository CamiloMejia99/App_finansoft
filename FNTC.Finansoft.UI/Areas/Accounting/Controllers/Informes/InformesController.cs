using FNTC.Finansoft.Accounting.BLL.PlanCuentas;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.Informes;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using FNTC.Finansoft.UI.Tools;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Accounting.Controllers.Movimientos.Informes
{
    //comentario prueba
    public class InformesController : Controller
    {

        public IEnumerable<object> saldo { get; private set; }
        AccountingContext db = new AccountingContext();


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
            // niveles.Add(new SelectListItem { Text = "Nivel", Value = "" });
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

            var datosEmpresa = db.Empresa.ToList();

            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";

            var fechaDesde = coll["fechaDesde"];
            var fechaHasta = coll["fechaHasta"];
            var fechaDesdeBC = coll["fechaDesdeBC"];
            var fechaHastaBC = coll["fechaHastaBC"];
            var chktodos = coll["chkTodos"];
            var chkfechaDesembolso = coll["chkFechaDesembolso"];
            var desdeSaldo = Int32.Parse(coll["monthini"]);
            var desdeSaldoAno = Int32.Parse(coll["year"]);
            var hastaSaldo = Int32.Parse(coll["monthfin"]);
            var informe = Int32.Parse(coll["informe"]);
            var cuenta = coll["Cuenta"];
            var cuentaA = coll["CuentaA"];
            var cuentaB = coll["CuentaB"];
            var documento = coll["Tercero"];
            var archivo = "";
            //
            var nombre = datosEmpresa.Select(x => x.nombre).FirstOrDefault();
            var nit = datosEmpresa.Select(x => x.nit).FirstOrDefault();
            if (nit == null)
                nit = "";
            DateTime fechaAct = Fecha.GetFechaColombia();
            string filtro = "";
            //

            var ctx = new AccountingContext();
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
            else if (informe == 47) archivo = "attachment;filename=Catálogo-de-cuentas.xlsx";
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
                    case 2:
                        #region AUXILIAR POR CUENTAS ACTUALIZADO
                        var desdFAC = DateTime.Now;
                        var hastFAC = DateTime.Now;
                        var opAPC = 0;
                        var filtroCuentas2 = "";

                        List<SpAuxiliarPorTercero> datosAuxC = new List<SpAuxiliarPorTercero>();

                        #region FILTRO PARA BUSQUEDA DE DATOS 
                        if (fechaDesde != "" && fechaHasta != "")
                        {
                            desdFAC = Convert.ToDateTime(fechaDesde);
                            hastFAC = Convert.ToDateTime(fechaHasta);
                            filtro = desdFAC.ToShortDateString() + " - " + hastFAC.ToShortDateString();

                            if (cuenta != "" && cuentaA == "" && cuentaB == "")
                            {
                                filtroCuentas2 = "Filtro por cuenta: " + cuenta;
                                opAPC = 1;//filtro por fecha desde, fecha hasta y cuenta
                            }
                            else if (cuentaA != "" && cuentaB != "")
                            {
                                filtroCuentas2 = "Filtro desde la cuenta: " + cuentaA + " Hasta la cuenta " + cuentaB;
                                opAPC = 2;//filtro por fecha desde, fecha hasta, cuenta desde, cuenta hasta 
                            }

                            else if (cuentaA != "" && cuentaB == "")
                            {
                                filtroCuentas2 = "Filtro desde la cuenta: " + cuentaA;
                                opAPC = 3;//filtro por fecha desde, fecha hasta, cuenta desde
                            }
                            else if (cuentaA == "" && cuentaB != "")
                            {
                                filtroCuentas2 = "Filtro hasta la cuenta: " + cuentaB;
                                opAPC = 4;//filtro por fecha desde, fecha hasta, cuenta hasta 
                            }
                            else if (cuenta == "" && cuentaA == "" && cuentaB == "")
                            {
                                opAPC = 5;// filtro por fecha desde y fecha hasta trae todo
                            }
                        }

                        DateTime fechDesdAC = new DateTime(desdFAC.Year, desdFAC.Month, desdFAC.Day, 0, 0, 0);
                        DateTime fechHastAC = new DateTime(hastFAC.Year, hastFAC.Month, hastFAC.Day, 23, 59, 59);

                        #endregion

                        datosAuxC = db.Database.SqlQuery<SpAuxiliarPorTercero>(
                            "dbo.sp_AuxiliarPorCuentas  @cuenta, @cuentaA,@cuentaB,@FechaDesde, @FechaHasta, @opcion",
                            new SqlParameter("@cuenta", cuenta),
                            new SqlParameter("@cuentaA", cuentaA),
                            new SqlParameter("@cuentaB", cuentaB),
                            new SqlParameter("@FechaDesde", fechDesdAC),
                            new SqlParameter("@FechaHasta", fechHastAC),
                            new SqlParameter("@opcion", opAPC)
                            ).ToList();

                        ws = pack.Workbook.Worksheets.Add("AuxiliarCuentas");

                        #region encabezado excel
                        ws.Cells["A1:I1,A2:I2,A3:I3,A4:I4,A5:I5,A6:I6"].Merge = true;
                        ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.Font.Bold = true;
                        ws.Cells["A2:I2"].Style.Font.Name = "Arial";
                        ws.Cells["A2:I2"].Style.Font.Size = 14;
                        ws.Cells["A" + 2].Value = "AUXILIAR POR CUENTAS   " + filtro;
                        ws.Cells["A" + 3].Value = filtroCuentas2;
                        ws.Cells[1, 1, 6, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 6, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5,A6:I6"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A2:I2,A3:I3,A4:I4,A7:I7,A5:I5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A2:I2,A3:I3,A4:I4,A7:I7,A5:I5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.WrapText = true;
                        ws.Cells["A4:I4,A5:I5"].Style.Font.Size = 12;
                        ws.Cells["A3:I3"].Style.Font.Size = 13;
                        ws.Cells["A6:I6"].Style.Font.Size = 10;
                        ws.Cells["A" + 4].Value = nombre;
                        ws.Cells["A" + 5].Value = nit;
                        ws.Cells["A" + 6].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        ws.Cells["A7:I7"].Merge = true;
                        ws.Cells[7, 1, 7, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[7, 1, 7, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                        ws.Cells[7, 1, 7, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        ws.Cells[8, 1, 8, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        ws.Cells["A8:J8"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        ws.Cells["A8:I8"].Style.Font.Bold = true;
                        ws.Cells[8, 1, 8, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[8, 1, 8, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        #endregion

                        ws.Cells["A" + 8].Value = "CÓDIGO";
                        ws.Cells["B" + 8].Value = "NOMBRE";
                        ws.Cells["C" + 8].Value = "COMPROBANTE";
                        ws.Cells["D" + 8].Value = "FECHA";
                        ws.Cells["E" + 8].Value = "TERCERO";
                        ws.Cells["F" + 8].Value = "NOMBRE TERCERO";
                        ws.Cells["G" + 8].Value = "DÉBITO";
                        ws.Cells["H" + 8].Value = "CRÉDITO";
                        ws.Cells["I" + 8].Value = "SALDO";

                        var cuentasEnlistaAC = (from a in datosAuxC
                                                orderby a.CUENTA
                                                select new { a.CUENTA, a.NOMBRECUENTA, a.NATURALEZA }).Distinct().ToList();
                        i = 9;
                        saldo = 0;
                        decimal totalSaldoAC = 0;
                        foreach (var item in cuentasEnlistaAC)
                        {
                            var dataMov = datosAuxC.Where(x => x.CUENTA == item.CUENTA).OrderBy(x => x.FECHAMOVIMIENTO).ToList();
                            debito = dataMov.Select(x => x.DEBITO).Sum();
                            credito = dataMov.Select(x => x.CREDITO).Sum();
                            ws.Cells["A" + i].Value = item.CUENTA;
                            ws.Cells["B" + i].Value = item.NOMBRECUENTA;
                            string naturaleza = item.NATURALEZA;
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

                                ws.Cells["C" + i].Value = item2.COMPROBANTE;
                                ws.Cells["D" + i].Value = item2.FECHAMOVIMIENTO.ToString("yyyy-MM-dd");
                                ws.Cells["E" + i].Value = item2.TERCERO;
                                if (item2.TERCERO != null)
                                {
                                    ws.Cells["F" + i].Value = item2.NOMBRE;
                                }
                                else { ws.Cells["F" + i].Value = ""; }

                                ws.Cells["G" + i].Value = item2.DEBITO.ToString("N0", formato);
                                ws.Cells["H" + i].Value = item2.CREDITO.ToString("N0", formato);
                                ws.Cells["I" + i].Value = saldo.ToString("N0", formato);
                                i++;
                            }
                            if (naturaleza == "D")
                            {
                                totalSaldoAC = debito - credito;
                            }
                            else
                            {
                                totalSaldoAC = credito - debito;
                            }
                            ws.Cells["F" + i].Style.Font.Bold = true;
                            ws.Cells["F" + i].Value = "TOTAL";
                            ws.Cells["G" + i].Value = debito.ToString("N0", formato);
                            ws.Cells["H" + i].Value = credito.ToString("N0", formato);
                            ws.Cells["I" + i].Value = totalSaldoAC.ToString("N0", formato);
                            i += 2;

                        }

                        #endregion
                        break;
                    case 1:
                        #region informe2 AuxiliarCuentas

                        //List<MovimientoAuxiliar2> Movimientos = new List<MovimientoAuxiliar2>();
                        ////string periodos2 = "";

                        //if (fechaDesde != "" && fechaHasta != "")
                        //{
                        //    DateTime fh = Convert.ToDateTime(fechaHasta);
                        //    DateTime fd = Convert.ToDateTime(fechaDesde);
                        //    DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                        //    DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                        //    Movimientos = db.Database.SqlQuery<MovimientoAuxiliar2>(
                        //        "dbo.sp_Movimientos @fechaDesde,@fechaHasta,@opcion",
                        //        new SqlParameter("@fechaDesde", fechDesde),
                        //        new SqlParameter("@fechaHasta", fechHasta),
                        //        new SqlParameter("@opcion", 1)
                        //        ).ToList();

                        //    filtro = fd.ToShortDateString() + " - " + fh.ToShortDateString();
                        //}
                        //else if (fechaDesde != "" && fechaHasta == "")
                        //{
                        //    DateTime fd = Convert.ToDateTime(fechaDesde);
                        //    DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                        //    Movimientos = db.Database.SqlQuery<MovimientoAuxiliar2>(
                        //        "dbo.sp_Movimientos @fechaDesde,@fechaHasta,@opcion",
                        //        new SqlParameter("@fechaDesde", fechDesde),
                        //        new SqlParameter("@fechaHasta", fechDesde),
                        //        new SqlParameter("@opcion", 2)
                        //        ).ToList();

                        //    filtro = fd.ToShortDateString();
                        //}
                        //else if (fechaDesde == "" && fechaHasta != "")
                        //{
                        //    DateTime fh = Convert.ToDateTime(fechaHasta);
                        //    DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                        //    Movimientos = db.Database.SqlQuery<MovimientoAuxiliar2>(
                        //        "dbo.sp_Movimientos @fechaDesde,@fechaHasta,@opcion",
                        //        new SqlParameter("@fechaDesde", fechHasta),
                        //        new SqlParameter("@fechaHasta", fechHasta),
                        //        new SqlParameter("@opcion", 3)
                        //        ).ToList();
                        //    filtro = fh.ToShortDateString();
                        //}
                        //#region Cuentas AB
                        //if (cuentaA == "")
                        //{
                        //    cuentaA = "0";
                        //}
                        //var cuentaMin = cuentaA;

                        //var cuentalongmin = Convert.ToInt64(cuentaMin);
                        //if (cuentaB == "")
                        //{
                        //    cuentaB = "0";
                        //}
                        //var cuentaMax = cuentaB;

                        //var cuentalongmax = Convert.ToInt64(cuentaMax);
                        //#endregion

                        //#region CodigoFiltro
                        ////List<string> SelectMov = movimiento.Select(p => p.CUENTA).ToList();
                        ////long[] arregloMovie = SelectMov.Select(x => Convert.ToInt64(x)).ToArray();
                        ////var ArregloMov = arregloMovie.Where(n => n >= cuentalongmin && n <= cuentalongmax);
                        ////List<string> ArregloString = ArregloMov.Select(x => Convert.ToString(x)).ToList();

                        //// movimiento = (from movimientos in movimiento
                        ////                  select movimientos).ToList();
                        //#endregion
                        //if (cuentaA != "0" && cuentaB != "0")
                        //{
                        //    #region Filtros
                        //    //var movimientoInt = (from Arreg in ArregloString
                        //    //                     join Mov in movimiento on Arreg equals Mov.CUENTA into sm
                        //    //                     from a in sm
                        //    //                     orderby a.CUENTA ascending
                        //    //                     select new
                        //    //                     {
                        //    //                         CUENTA = Convert.ToInt64(a.CUENTA),
                        //    //                         a.NATURALEZA,
                        //    //                         a.NOMBRECUENTA,
                        //    //                         a.FECHAMOVIMIENTO,
                        //    //                         a.DEBITO,
                        //    //                         a.CREDITO,
                        //    //                         a.TIPO,
                        //    //                         a.TERCERO,
                        //    //                         a.NOMBRE,
                        //    //                         a.NUMERO
                        //    //                     }).Distinct().ToList();
                        //    #endregion
                        //    var MovimientoTemp = (from a in Movimientos
                        //                          select new
                        //                          {
                        //                              CUENTA = Convert.ToInt64(a.CUENTA),
                        //                              a.NATURALEZA,
                        //                              a.NOMBRECUENTA,
                        //                              a.FECHAMOVIMIENTO,
                        //                              a.DEBITO,
                        //                              a.CREDITO,
                        //                              a.TIPO,
                        //                              a.TERCERO,
                        //                              a.NOMBRE,
                        //                              a.NUMERO
                        //                          }).Distinct().ToList();

                        //    var MovimientosFiltro = (from a in MovimientoTemp
                        //                             where a.CUENTA >= cuentalongmin && a.CUENTA <= cuentalongmax
                        //                             select new
                        //                             {
                        //                                 CUENTA = Convert.ToInt64(a.CUENTA),
                        //                                 a.NATURALEZA,
                        //                                 a.NOMBRECUENTA,
                        //                                 a.FECHAMOVIMIENTO,
                        //                                 a.DEBITO,
                        //                                 a.CREDITO,
                        //                                 a.TIPO,
                        //                                 a.TERCERO,
                        //                                 a.NOMBRE,
                        //                                 a.NUMERO
                        //                             }).Distinct().ToList();

                        //    var AuxMovimiento = MovimientosFiltro;

                        //    ws = pack.Workbook.Worksheets.Add("AuxiliarCuentas");

                        //    // encabezado
                        //    ws.Cells["A1:I1,A2:I2,A3:I3,A4:I4,A5:I5"].Merge = true;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4"].Style.Font.Bold = true;
                        //    ws.Cells["A2:I2"].Style.Font.Name = "Arial";
                        //    ws.Cells["A2:I2"].Style.Font.Size = 14;
                        //    ws.Cells["A" + 2].Value = "AUXILIAR POR CUENTAS   " + filtro;
                        //    ws.Cells[1, 1, 5, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[1, 1, 5, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A7:I7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A7:I7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.WrapText = true;
                        //    ws.Cells["A3:I3,A4:I4"].Style.Font.Size = 12;
                        //    ws.Cells["A5:I5"].Style.Font.Size = 10;
                        //    ws.Cells["A" + 3].Value = nombre;
                        //    ws.Cells["A" + 4].Value = nit;
                        //    ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        //    ws.Cells["A6,I6"].Merge = true;
                        //    ws.Cells[6, 1, 6, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[6, 1, 6, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                        //    ws.Cells[6, 1, 6, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        //    ws.Cells[7, 1, 7, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        //    ws.Cells["A7:J7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        //    ws.Cells["A7:I7"].Style.Font.Bold = true;
                        //    ws.Cells[7, 1, 7, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[7, 1, 7, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        //    //fin encabezado

                        //    ws.Cells["A" + 7].Value = "CÓDIGO";
                        //    ws.Cells["B" + 7].Value = "NOMBRE";
                        //    ws.Cells["C" + 7].Value = "COMPROBANTE";
                        //    ws.Cells["D" + 7].Value = "FECHA";
                        //    ws.Cells["E" + 7].Value = "TERCERO";
                        //    ws.Cells["F" + 7].Value = "NOMBRE TERCERO";
                        //    ws.Cells["G" + 7].Value = "DÉBITO";
                        //    ws.Cells["H" + 7].Value = "CRÉDITO";
                        //    ws.Cells["I" + 7].Value = "SALDO";

                        //    var mov = (from m in AuxMovimiento
                        //               orderby m.CUENTA
                        //               select new { m.CUENTA, m.NATURALEZA, m.NOMBRECUENTA }).Distinct().ToList();
                        //    i = 8;
                        //    decimal saldoT = 0, saldoTotal = 0;
                        //    foreach (var item in mov)
                        //    {
                        //        var dataMov = AuxMovimiento.Where(x => x.CUENTA == item.CUENTA).OrderBy(x => x.FECHAMOVIMIENTO).ToList();
                        //        debito = dataMov.Select(x => x.DEBITO).Sum();
                        //        credito = dataMov.Select(x => x.CREDITO).Sum();
                        //        ws.Cells["A" + i].Value = item.CUENTA;
                        //        ws.Cells["B" + i].Value = item.NOMBRECUENTA;
                        //        string naturaleza = item.NATURALEZA;
                        //        i++;
                        //        foreach (var item2 in dataMov)
                        //        {
                        //            if (naturaleza == "D")
                        //            {
                        //                saldoT = item2.DEBITO - item2.CREDITO;
                        //            }
                        //            else
                        //            {
                        //                saldoT = item2.CREDITO - item2.DEBITO;
                        //            }

                        //            ws.Cells["C" + i].Value = item2.TIPO + " " + item2.NUMERO;
                        //            ws.Cells["D" + i].Value = item2.FECHAMOVIMIENTO.ToString("yyyy-MM-dd");
                        //            ws.Cells["E" + i].Value = item2.TERCERO;
                        //            if (item2.TERCERO != null)
                        //            {
                        //                ws.Cells["F" + i].Value = item2.NOMBRE;
                        //            }
                        //            else { ws.Cells["F" + i].Value = ""; }

                        //            ws.Cells["G" + i].Value = item2.DEBITO.ToString("N0", formato);
                        //            ws.Cells["H" + i].Value = item2.CREDITO.ToString("N0", formato);
                        //            ws.Cells["I" + i].Value = saldoT.ToString("N0", formato);
                        //            i++;
                        //        }
                        //        if (naturaleza == "D")
                        //        {
                        //            saldoTotal = debito - credito;
                        //        }
                        //        else
                        //        {
                        //            saldoTotal = credito - debito;
                        //        }
                        //        ws.Cells["F" + i].Value = "TOTAL";
                        //        ws.Cells["G" + i].Value = debito.ToString("N0", formato);
                        //        ws.Cells["H" + i].Value = credito.ToString("N0", formato);
                        //        ws.Cells["I" + i].Value = saldoTotal.ToString("N0", formato);
                        //        i += 2;
                        //    }
                        //    AuxMovimiento = null;

                        //}


                        //else if (cuentaA == "0" && cuentaB == "0" && cuenta == "")
                        //{

                        //    var AuxMovimiento = Movimientos;

                        //    ws = pack.Workbook.Worksheets.Add("AuxiliarCuentas");

                        //    // encabezado
                        //    ws.Cells["A1:I1,A2:I2,A3:I3,A4:I4,A5:I5"].Merge = true;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4"].Style.Font.Bold = true;
                        //    ws.Cells["A2:I2"].Style.Font.Name = "Arial";
                        //    ws.Cells["A2:I2"].Style.Font.Size = 14;
                        //    ws.Cells["A" + 2].Value = "AUXILIAR POR CUENTAS   " + filtro;
                        //    ws.Cells[1, 1, 5, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[1, 1, 5, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A7:I7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A7:I7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.WrapText = true;
                        //    ws.Cells["A3:I3,A4:I4"].Style.Font.Size = 12;
                        //    ws.Cells["A5:I5"].Style.Font.Size = 10;
                        //    ws.Cells["A" + 3].Value = nombre;
                        //    ws.Cells["A" + 4].Value = nit;
                        //    ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        //    ws.Cells["A6,I6"].Merge = true;
                        //    ws.Cells[6, 1, 6, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[6, 1, 6, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                        //    ws.Cells[6, 1, 6, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        //    ws.Cells[7, 1, 7, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        //    ws.Cells["A7:J7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        //    ws.Cells["A7:I7"].Style.Font.Bold = true;
                        //    ws.Cells[7, 1, 7, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[7, 1, 7, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        //    //fin encabezado

                        //    ws.Cells["A" + 7].Value = "CÓDIGO";
                        //    ws.Cells["B" + 7].Value = "NOMBRE";
                        //    ws.Cells["C" + 7].Value = "COMPROBANTE";
                        //    ws.Cells["D" + 7].Value = "FECHA";
                        //    ws.Cells["E" + 7].Value = "TERCERO";
                        //    ws.Cells["F" + 7].Value = "NOMBRE TERCERO";
                        //    ws.Cells["G" + 7].Value = "DÉBITO";
                        //    ws.Cells["H" + 7].Value = "CRÉDITO";
                        //    ws.Cells["I" + 7].Value = "SALDO";

                        //    var mov = (from m in AuxMovimiento
                        //               orderby m.CUENTA
                        //               select new { m.CUENTA, m.NATURALEZA, m.NOMBRECUENTA }).Distinct().ToList();
                        //    i = 8;
                        //    decimal saldoT = 0, saldoTotal = 0;
                        //    foreach (var item in mov)
                        //    {
                        //        var dataMov = AuxMovimiento.Where(x => x.CUENTA == item.CUENTA).OrderBy(x => x.FECHAMOVIMIENTO).ToList();
                        //        debito = dataMov.Select(x => x.DEBITO).Sum();
                        //        credito = dataMov.Select(x => x.CREDITO).Sum();
                        //        ws.Cells["A" + i].Value = item.CUENTA;
                        //        ws.Cells["B" + i].Value = item.NOMBRECUENTA;
                        //        string naturaleza = item.NATURALEZA;
                        //        i++;
                        //        foreach (var item2 in dataMov)
                        //        {
                        //            if (naturaleza == "D")
                        //            {
                        //                saldoT = item2.DEBITO - item2.CREDITO;
                        //            }
                        //            else
                        //            {
                        //                saldoT = item2.CREDITO - item2.DEBITO;
                        //            }

                        //            ws.Cells["C" + i].Value = item2.TIPO + " " + item2.NUMERO;
                        //            ws.Cells["D" + i].Value = item2.FECHAMOVIMIENTO.ToString("yyyy-MM-dd");
                        //            ws.Cells["E" + i].Value = item2.TERCERO;
                        //            if (item2.TERCERO != null)
                        //            {
                        //                ws.Cells["F" + i].Value = item2.NOMBRE;
                        //            }
                        //            else { ws.Cells["F" + i].Value = ""; }

                        //            ws.Cells["G" + i].Value = item2.DEBITO.ToString("N0", formato);
                        //            ws.Cells["H" + i].Value = item2.CREDITO.ToString("N0", formato);
                        //            ws.Cells["I" + i].Value = saldoT.ToString("N0", formato);
                        //            i++;
                        //        }
                        //        if (naturaleza == "D")
                        //        {
                        //            saldoTotal = debito - credito;
                        //        }
                        //        else
                        //        {
                        //            saldoTotal = credito - debito;
                        //        }
                        //        ws.Cells["F" + i].Value = "TOTAL";
                        //        ws.Cells["G" + i].Value = debito.ToString("N0", formato);
                        //        ws.Cells["H" + i].Value = credito.ToString("N0", formato);
                        //        ws.Cells["I" + i].Value = saldoTotal.ToString("N0", formato);
                        //        i += 2;
                        //    }
                        //    AuxMovimiento = null;
                        //}

                        //else if (cuentaA == "0" && cuentaB == "0" && cuenta != "")
                        //{
                        //    Movimientos = Movimientos.Where(x => x.CUENTA == cuenta).ToList();

                        //    var AuxMovimiento = Movimientos;

                        //    ws = pack.Workbook.Worksheets.Add("AuxiliarCuentas");

                        //    // encabezado
                        //    ws.Cells["A1:I1,A2:I2,A3:I3,A4:I4,A5:I5"].Merge = true;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4"].Style.Font.Bold = true;
                        //    ws.Cells["A2:I2"].Style.Font.Name = "Arial";
                        //    ws.Cells["A2:I2"].Style.Font.Size = 14;
                        //    ws.Cells["A" + 2].Value = "AUXILIAR POR CUENTAS   " + filtro;
                        //    ws.Cells[1, 1, 5, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[1, 1, 5, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A7:I7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A7:I7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.WrapText = true;
                        //    ws.Cells["A3:I3,A4:I4"].Style.Font.Size = 12;
                        //    ws.Cells["A5:I5"].Style.Font.Size = 10;
                        //    ws.Cells["A" + 3].Value = nombre;
                        //    ws.Cells["A" + 4].Value = nit;
                        //    ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        //    ws.Cells["A6,I6"].Merge = true;
                        //    ws.Cells[6, 1, 6, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[6, 1, 6, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                        //    ws.Cells[6, 1, 6, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        //    ws.Cells[7, 1, 7, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        //    ws.Cells["A7:J7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        //    ws.Cells["A7:I7"].Style.Font.Bold = true;
                        //    ws.Cells[7, 1, 7, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[7, 1, 7, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        //    //fin encabezado

                        //    ws.Cells["A" + 7].Value = "CÓDIGO";
                        //    ws.Cells["B" + 7].Value = "NOMBRE";
                        //    ws.Cells["C" + 7].Value = "COMPROBANTE";
                        //    ws.Cells["D" + 7].Value = "FECHA";
                        //    ws.Cells["E" + 7].Value = "TERCERO";
                        //    ws.Cells["F" + 7].Value = "NOMBRE TERCERO";
                        //    ws.Cells["G" + 7].Value = "DÉBITO";
                        //    ws.Cells["H" + 7].Value = "CRÉDITO";
                        //    ws.Cells["I" + 7].Value = "SALDO";

                        //    var mov = (from m in AuxMovimiento
                        //               orderby m.CUENTA
                        //               select new { m.CUENTA, m.NATURALEZA, m.NOMBRECUENTA }).Distinct().ToList();
                        //    i = 8;
                        //    decimal saldoT = 0, saldoTotal = 0;
                        //    foreach (var item in mov)
                        //    {
                        //        var dataMov = AuxMovimiento.Where(x => x.CUENTA == item.CUENTA).OrderBy(x => x.FECHAMOVIMIENTO).ToList();
                        //        debito = dataMov.Select(x => x.DEBITO).Sum();
                        //        credito = dataMov.Select(x => x.CREDITO).Sum();
                        //        ws.Cells["A" + i].Value = item.CUENTA;
                        //        ws.Cells["B" + i].Value = item.NOMBRECUENTA;
                        //        string naturaleza = item.NATURALEZA;
                        //        i++;
                        //        foreach (var item2 in dataMov)
                        //        {
                        //            if (naturaleza == "D")
                        //            {
                        //                saldoT = item2.DEBITO - item2.CREDITO;
                        //            }
                        //            else
                        //            {
                        //                saldoT = item2.CREDITO - item2.DEBITO;
                        //            }

                        //            ws.Cells["C" + i].Value = item2.TIPO + " " + item2.NUMERO;
                        //            ws.Cells["D" + i].Value = item2.FECHAMOVIMIENTO.ToString("yyyy-MM-dd");
                        //            ws.Cells["E" + i].Value = item2.TERCERO;
                        //            if (item2.TERCERO != null)
                        //            {
                        //                ws.Cells["F" + i].Value = item2.NOMBRE;
                        //            }
                        //            else { ws.Cells["F" + i].Value = ""; }

                        //            ws.Cells["G" + i].Value = item2.DEBITO.ToString("N0", formato);
                        //            ws.Cells["H" + i].Value = item2.CREDITO.ToString("N0", formato);
                        //            ws.Cells["I" + i].Value = saldoT.ToString("N0", formato);
                        //            i++;
                        //        }
                        //        if (naturaleza == "D")
                        //        {
                        //            saldoTotal = debito - credito;
                        //        }
                        //        else
                        //        {
                        //            saldoTotal = credito - debito;
                        //        }
                        //        ws.Cells["F" + i].Value = "TOTAL";
                        //        ws.Cells["G" + i].Value = debito.ToString("N0", formato);
                        //        ws.Cells["H" + i].Value = credito.ToString("N0", formato);
                        //        ws.Cells["I" + i].Value = saldoTotal.ToString("N0", formato);
                        //        i += 2;
                        //    }
                        //    AuxMovimiento = null;
                        //}
                        //else if (cuentaA != "0" && cuentaB == "0" && cuenta == "")
                        //{
                        //    #region Filtros
                        //    //var movimientoInt = (from Arreg in ArregloString
                        //    //                     join Mov in movimiento on Arreg equals Mov.CUENTA into sm
                        //    //                     from a in sm
                        //    //                     orderby a.CUENTA ascending
                        //    //                     select new
                        //    //                     {
                        //    //                         CUENTA = Convert.ToInt64(a.CUENTA),
                        //    //                         a.NATURALEZA,
                        //    //                         a.NOMBRECUENTA,
                        //    //                         a.FECHAMOVIMIENTO,
                        //    //                         a.DEBITO,
                        //    //                         a.CREDITO,
                        //    //                         a.TIPO,
                        //    //                         a.TERCERO,
                        //    //                         a.NOMBRE,
                        //    //                         a.NUMERO
                        //    //                     }).Distinct().ToList();
                        //    #endregion
                        //    var MovimientoTemp = (from a in Movimientos
                        //                          select new
                        //                          {
                        //                              CUENTA = Convert.ToInt64(a.CUENTA),
                        //                              a.NATURALEZA,
                        //                              a.NOMBRECUENTA,
                        //                              a.FECHAMOVIMIENTO,
                        //                              a.DEBITO,
                        //                              a.CREDITO,
                        //                              a.TIPO,
                        //                              a.TERCERO,
                        //                              a.NOMBRE,
                        //                              a.NUMERO
                        //                          }).Distinct().ToList();

                        //    var MovimientosFiltro = (from a in MovimientoTemp
                        //                             where a.CUENTA >= cuentalongmin
                        //                             select new
                        //                             {
                        //                                 CUENTA = Convert.ToInt64(a.CUENTA),
                        //                                 a.NATURALEZA,
                        //                                 a.NOMBRECUENTA,
                        //                                 a.FECHAMOVIMIENTO,
                        //                                 a.DEBITO,
                        //                                 a.CREDITO,
                        //                                 a.TIPO,
                        //                                 a.TERCERO,
                        //                                 a.NOMBRE,
                        //                                 a.NUMERO
                        //                             }).Distinct().ToList();

                        //    var AuxMovimiento = MovimientosFiltro;

                        //    ws = pack.Workbook.Worksheets.Add("AuxiliarCuentas");

                        //    // encabezado
                        //    ws.Cells["A1:I1,A2:I2,A3:I3,A4:I4,A5:I5"].Merge = true;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4"].Style.Font.Bold = true;
                        //    ws.Cells["A2:I2"].Style.Font.Name = "Arial";
                        //    ws.Cells["A2:I2"].Style.Font.Size = 14;
                        //    ws.Cells["A" + 2].Value = "AUXILIAR POR CUENTAS   " + filtro;
                        //    ws.Cells[1, 1, 5, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[1, 1, 5, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5,A7:I7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5,A7:I7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.WrapText = true;
                        //    ws.Cells["A3:I3,A4:I4"].Style.Font.Size = 12;
                        //    ws.Cells["A5:I5"].Style.Font.Size = 10;
                        //    ws.Cells["A" + 3].Value = nombre;
                        //    ws.Cells["A" + 4].Value = nit;
                        //    ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        //    ws.Cells["A6,I6"].Merge = true;
                        //    ws.Cells[6, 1, 6, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[6, 1, 6, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                        //    ws.Cells[6, 1, 6, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        //    ws.Cells[7, 1, 7, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        //    ws.Cells["A7:J7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        //    ws.Cells["A7:I7"].Style.Font.Bold = true;
                        //    ws.Cells[7, 1, 7, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[7, 1, 7, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        //    //fin encabezado

                        //    ws.Cells["A" + 7].Value = "CÓDIGO";
                        //    ws.Cells["B" + 7].Value = "NOMBRE";
                        //    ws.Cells["C" + 7].Value = "COMPROBANTE";
                        //    ws.Cells["D" + 7].Value = "FECHA";
                        //    ws.Cells["E" + 7].Value = "TERCERO";
                        //    ws.Cells["F" + 7].Value = "NOMBRE TERCERO";
                        //    ws.Cells["G" + 7].Value = "DÉBITO";
                        //    ws.Cells["H" + 7].Value = "CRÉDITO";
                        //    ws.Cells["I" + 7].Value = "SALDO";

                        //    var mov = (from m in AuxMovimiento
                        //               orderby m.CUENTA
                        //               select new { m.CUENTA, m.NATURALEZA, m.NOMBRECUENTA }).Distinct().ToList();
                        //    i = 8;
                        //    decimal saldoT = 0, saldoTotal = 0;
                        //    foreach (var item in mov)
                        //    {
                        //        var dataMov = AuxMovimiento.Where(x => x.CUENTA == item.CUENTA).OrderBy(x => x.FECHAMOVIMIENTO).ToList();
                        //        debito = dataMov.Select(x => x.DEBITO).Sum();
                        //        credito = dataMov.Select(x => x.CREDITO).Sum();
                        //        ws.Cells["A" + i].Value = item.CUENTA;
                        //        ws.Cells["B" + i].Value = item.NOMBRECUENTA;
                        //        string naturaleza = item.NATURALEZA;
                        //        i++;
                        //        foreach (var item2 in dataMov)
                        //        {
                        //            if (naturaleza == "D")
                        //            {
                        //                saldoT = item2.DEBITO - item2.CREDITO;
                        //            }
                        //            else
                        //            {
                        //                saldoT = item2.CREDITO - item2.DEBITO;
                        //            }

                        //            ws.Cells["C" + i].Value = item2.TIPO + " " + item2.NUMERO;
                        //            ws.Cells["D" + i].Value = item2.FECHAMOVIMIENTO.ToString("yyyy-MM-dd");
                        //            ws.Cells["E" + i].Value = item2.TERCERO;
                        //            if (item2.TERCERO != null)
                        //            {
                        //                ws.Cells["F" + i].Value = item2.NOMBRE;
                        //            }
                        //            else { ws.Cells["F" + i].Value = ""; }

                        //            ws.Cells["G" + i].Value = item2.DEBITO.ToString("N0", formato);
                        //            ws.Cells["H" + i].Value = item2.CREDITO.ToString("N0", formato);
                        //            ws.Cells["I" + i].Value = saldoT.ToString("N0", formato);
                        //            i++;
                        //        }
                        //        if (naturaleza == "D")
                        //        {
                        //            saldoTotal = debito - credito;
                        //        }
                        //        else
                        //        {
                        //            saldoTotal = credito - debito;
                        //        }
                        //        ws.Cells["F" + i].Value = "TOTAL";
                        //        ws.Cells["G" + i].Value = debito.ToString("N0", formato);
                        //        ws.Cells["H" + i].Value = credito.ToString("N0", formato);
                        //        ws.Cells["I" + i].Value = saldoTotal.ToString("N0", formato);
                        //        i += 2;
                        //    }
                        //    AuxMovimiento = null;
                        //}
                        //else if (cuentaA == "0" && cuentaB != "0" && cuenta == "")
                        //{
                        //    #region Filtros
                        //    //var movimientoInt = (from Arreg in ArregloString
                        //    //                     join Mov in movimiento on Arreg equals Mov.CUENTA into sm
                        //    //                     from a in sm
                        //    //                     orderby a.CUENTA ascending
                        //    //                     select new
                        //    //                     {
                        //    //                         CUENTA = Convert.ToInt64(a.CUENTA),
                        //    //                         a.NATURALEZA,
                        //    //                         a.NOMBRECUENTA,
                        //    //                         a.FECHAMOVIMIENTO,
                        //    //                         a.DEBITO,
                        //    //                         a.CREDITO,
                        //    //                         a.TIPO,
                        //    //                         a.TERCERO,
                        //    //                         a.NOMBRE,
                        //    //                         a.NUMERO
                        //    //                     }).Distinct().ToList();
                        //    #endregion
                        //    var MovimientoTemp = (from a in Movimientos
                        //                          select new
                        //                          {
                        //                              CUENTA = Convert.ToInt64(a.CUENTA),
                        //                              a.NATURALEZA,
                        //                              a.NOMBRECUENTA,
                        //                              a.FECHAMOVIMIENTO,
                        //                              a.DEBITO,
                        //                              a.CREDITO,
                        //                              a.TIPO,
                        //                              a.TERCERO,
                        //                              a.NOMBRE,
                        //                              a.NUMERO
                        //                          }).Distinct().ToList();

                        //    var MovimientosFiltro = (from a in MovimientoTemp
                        //                             where a.CUENTA <= cuentalongmax
                        //                             select new
                        //                             {
                        //                                 CUENTA = Convert.ToInt64(a.CUENTA),
                        //                                 a.NATURALEZA,
                        //                                 a.NOMBRECUENTA,
                        //                                 a.FECHAMOVIMIENTO,
                        //                                 a.DEBITO,
                        //                                 a.CREDITO,
                        //                                 a.TIPO,
                        //                                 a.TERCERO,
                        //                                 a.NOMBRE,
                        //                                 a.NUMERO
                        //                             }).Distinct().ToList();

                        //    var AuxMovimiento = MovimientosFiltro;

                        //    ws = pack.Workbook.Worksheets.Add("AuxiliarCuentas");

                        //    // encabezado
                        //    ws.Cells["A1:I1,A2:I2,A3:I3,A4:I4,A5:I5"].Merge = true;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4"].Style.Font.Bold = true;
                        //    ws.Cells["A2:I2"].Style.Font.Name = "Arial";
                        //    ws.Cells["A2:I2"].Style.Font.Size = 14;
                        //    ws.Cells["A" + 2].Value = "AUXILIAR POR CUENTAS   " + filtro;
                        //    ws.Cells[1, 1, 5, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[1, 1, 5, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5,A7:I7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5,A7:I7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.WrapText = true;
                        //    ws.Cells["A3:I3,A4:I4"].Style.Font.Size = 12;
                        //    ws.Cells["A5:I5"].Style.Font.Size = 10;
                        //    ws.Cells["A" + 3].Value = nombre;
                        //    ws.Cells["A" + 4].Value = nit;
                        //    ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        //    ws.Cells["A6,I6"].Merge = true;
                        //    ws.Cells[6, 1, 6, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[6, 1, 6, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                        //    ws.Cells[6, 1, 6, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        //    ws.Cells[7, 1, 7, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        //    ws.Cells["A7:J7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        //    ws.Cells["A7:I7"].Style.Font.Bold = true;
                        //    ws.Cells[7, 1, 7, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[7, 1, 7, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        //    //fin encabezado

                        //    ws.Cells["A" + 7].Value = "CÓDIGO";
                        //    ws.Cells["B" + 7].Value = "NOMBRE";
                        //    ws.Cells["C" + 7].Value = "COMPROBANTE";
                        //    ws.Cells["D" + 7].Value = "FECHA";
                        //    ws.Cells["E" + 7].Value = "TERCERO";
                        //    ws.Cells["F" + 7].Value = "NOMBRE TERCERO";
                        //    ws.Cells["G" + 7].Value = "DÉBITO";
                        //    ws.Cells["H" + 7].Value = "CRÉDITO";
                        //    ws.Cells["I" + 7].Value = "SALDO";

                        //    var mov = (from m in AuxMovimiento
                        //               orderby m.CUENTA
                        //               select new { m.CUENTA, m.NATURALEZA, m.NOMBRECUENTA }).Distinct().ToList();
                        //    i = 8;
                        //    decimal saldoT = 0, saldoTotal = 0;
                        //    foreach (var item in mov)
                        //    {
                        //        var dataMov = AuxMovimiento.Where(x => x.CUENTA == item.CUENTA).OrderBy(x => x.FECHAMOVIMIENTO).ToList();
                        //        debito = dataMov.Select(x => x.DEBITO).Sum();
                        //        credito = dataMov.Select(x => x.CREDITO).Sum();
                        //        ws.Cells["A" + i].Value = item.CUENTA;
                        //        ws.Cells["B" + i].Value = item.NOMBRECUENTA;
                        //        string naturaleza = item.NATURALEZA;
                        //        i++;
                        //        foreach (var item2 in dataMov)
                        //        {
                        //            if (naturaleza == "D")
                        //            {
                        //                saldoT = item2.DEBITO - item2.CREDITO;
                        //            }
                        //            else
                        //            {
                        //                saldoT = item2.CREDITO - item2.DEBITO;
                        //            }

                        //            ws.Cells["C" + i].Value = item2.TIPO + " " + item2.NUMERO;
                        //            ws.Cells["D" + i].Value = item2.FECHAMOVIMIENTO.ToString("yyyy-MM-dd");
                        //            ws.Cells["E" + i].Value = item2.TERCERO;
                        //            if (item2.TERCERO != null)
                        //            {
                        //                ws.Cells["F" + i].Value = item2.NOMBRE;
                        //            }
                        //            else { ws.Cells["F" + i].Value = ""; }

                        //            ws.Cells["G" + i].Value = item2.DEBITO.ToString("N0", formato);
                        //            ws.Cells["H" + i].Value = item2.CREDITO.ToString("N0", formato);
                        //            ws.Cells["I" + i].Value = saldoT.ToString("N0", formato);
                        //            i++;
                        //        }
                        //        if (naturaleza == "D")
                        //        {
                        //            saldoTotal = debito - credito;
                        //        }
                        //        else
                        //        {
                        //            saldoTotal = credito - debito;
                        //        }
                        //        ws.Cells["F" + i].Value = "TOTAL";
                        //        ws.Cells["G" + i].Value = debito.ToString("N0", formato);
                        //        ws.Cells["H" + i].Value = credito.ToString("N0", formato);
                        //        ws.Cells["I" + i].Value = saldoTotal.ToString("N0", formato);
                        //        i += 2;
                        //    }
                        //    AuxMovimiento = null;
                        //}
                        #endregion
                        break;
                    case 3:
                        #region informe Estado de resultados
                        List<Movimiento> movtosER = new List<Movimiento>();

                        var fDesde = coll["fechaDesde"];
                        var fHasta = coll["fechaHasta"];

                        if (fDesde != "" && fHasta != "")
                        {
                            DateTime fh = Convert.ToDateTime(fHasta);
                            DateTime fd = Convert.ToDateTime(fDesde);
                            DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                            DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                            movtosER = db.Movimientos.Where(x => (x.FECHAMOVIMIENTO >= fechDesde && x.FECHAMOVIMIENTO <= fechHasta) && x.Comprobante.ANULADO == false).ToList();
                            filtro = fd.ToShortDateString() + " - " + fh.ToShortDateString();
                        }
                        else if (fDesde != "" && fHasta == "")
                        {
                            DateTime fd = Convert.ToDateTime(fDesde);
                            DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                            movtosER = db.Movimientos.Where(x => x.FECHAMOVIMIENTO >= fechDesde && x.Comprobante.ANULADO == false).ToList();
                            filtro = fd.ToShortDateString();
                        }
                        else if (fDesde == "" && fHasta != "")
                        {
                            DateTime fh = Convert.ToDateTime(fHasta);
                            DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                            movtosER = db.Movimientos.Where(x => x.FECHAMOVIMIENTO <= fechHasta && x.Comprobante.ANULADO == false).ToList();
                            filtro = fh.ToShortDateString();
                        }

                        ws = pack.Workbook.Worksheets.Add("Estado de Resultados");
                        // encabezado
                        ws.Cells["A1:D1,A2:D2,A3:D3,A4:D4,A5:D5"].Merge = true;
                        ws.Cells["A2:D2,A3:D3,A4:D4"].Style.Font.Bold = true;
                        ws.Cells["A2:D2"].Style.Font.Name = "Arial";
                        ws.Cells["A2:D2"].Style.Font.Size = 14;
                        ws.Cells["A" + 2].Value = "ESTADO DE RESULTADOS   " + filtro;
                        ws.Cells[1, 1, 5, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 5, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        ws.Cells["A2:D2,A3:D3,A4:D4,A5:D5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A2:D2,A3:D3,A4:D4,A7:D7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A2:D2,A3:D3,A4:D4,A7:D7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A2:D2,A3:D3,A4:D4,A5:D5"].Style.WrapText = true;
                        ws.Cells["A3:D3,A4:D4"].Style.Font.Size = 12;
                        ws.Cells["A5:D5"].Style.Font.Size = 10;
                        ws.Cells["A" + 3].Value = nombre;
                        ws.Cells["A" + 4].Value = nit;
                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        ws.Cells["A6:D6"].Merge = true;

                        //fin encabezado



                        if (movtosER.Count > 0)
                        {
                            var agenciaER = coll["agencia"];
                            credito = 0; debito = 0;
                            decimal totalIngresos = 0, totalGastos = 0, totGastosOrdinarios = 0, totOtrosGastos = 0, totOtros = 0, totalCostos = 0;
                            if (agenciaER != "")
                            {
                                int agency = Convert.ToInt32(agenciaER);
                                movtosER = movtosER.Where(x => x.terceroFK.DEPENDENCIA == agency).ToList();
                            }

                            ws.Cells[6, 1, 6, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                            ws.Cells[7, 1, 7, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                            ws.Cells["C7:D7"].Merge = true;
                            ws.Cells["A7:E7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                            ws.Cells["A7:D7"].Style.Font.Bold = true;
                            ws.Cells[7, 1, 7, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[7, 1, 7, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                            ws.Cells["A" + 7].Value = "CUENTA";
                            ws.Cells["B" + 7].Value = "INGRESOS";
                            ws.Cells["C" + 7].Value = "SALDO";

                            j = 8;

                            //ingresos

                            var ingresos = movtosER.Where(x => x.CUENTA.StartsWith("4")).ToList();
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
                            var gastos = movtosER.Where(x => x.CUENTA.StartsWith("5")).ToList();
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
                            var costos = movtosER.Where(x => x.CUENTA.StartsWith("6")).ToList();
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
                            movtosER = null;

                        }
                        #endregion
                        break;


                    case 4:
                        #region informe4 Balance General
                        List<Movimiento> saldosCuentas = new List<Movimiento>();
                        var PlanCuentas = db.PlanCuentas.ToList();
                        // string per = "";
                        if (desdeSaldoAno != 0 && desdeSaldo != 0 && hastaSaldo != 0)
                            filtro = desdeSaldoAno + " : " + GetMes(desdeSaldo) + " - " + GetMes(hastaSaldo);
                        else if (desdeSaldoAno != 0 && desdeSaldo != 0 && hastaSaldo == 0)
                            filtro = desdeSaldoAno + " : " + GetMes(desdeSaldo);
                        else if (desdeSaldoAno != 0 && desdeSaldo == 0 && hastaSaldo != 0)
                            filtro = desdeSaldoAno + " : " + GetMes(hastaSaldo);
                        else
                            filtro = desdeSaldoAno.ToString();

                      

                        ws = pack.Workbook.Worksheets.Add("Balance General");
                        // encabezado
                        ws.Cells["A1:I1,A2:I2,A3:I3,A4:I4,A5:I5"].Merge = true;
                        ws.Cells["A2:I2,A3:I3,A4:I4"].Style.Font.Bold = true;
                        ws.Cells["A2:I2"].Style.Font.Name = "Arial";
                        ws.Cells["A2:I2"].Style.Font.Size = 14;
                        ws.Cells["A" + 2].Value = "BALANCE GENERAL   " + filtro;
                        ws.Cells[1, 1, 5, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 5, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A2:I2,A3:I3,A4:I4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A2:I2,A3:I3,A4:I4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.WrapText = true;
                        ws.Cells["A3:I3,A4:I4"].Style.Font.Size = 12;
                        ws.Cells["A5:I5"].Style.Font.Size = 10;
                        ws.Cells["A" + 3].Value = nombre;
                        ws.Cells["A" + 4].Value = nit;
                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        ws.Cells["A6:I6"].Merge = true;

                        //fin encabezado

                        

                        int J = 7, G = 7; decimal totalActivo = 0, totalPasivo = 0;
                        int numSC = db.SaldosCuentas.Count();
                        if (numSC > 0)
                        {
                            //int iniAnio = saldosCuentas.OrderBy(x => x.ANO).Select(x => x.ANO).FirstOrDefault();

                            if (desdeSaldo != 0 && hastaSaldo != 0 && desdeSaldoAno != 0)
                            {
                                DateTime fechDesde = new DateTime(desdeSaldoAno, desdeSaldo, 1, 0, 0, 0);
                                int days = DateTime.DaysInMonth(desdeSaldoAno, hastaSaldo);
                                DateTime fechHasta = new DateTime(desdeSaldoAno, hastaSaldo, days, 0, 0, 0).AddDays(1);
                                saldosCuentas = db.Movimientos.Where(x => (x.FECHAMOVIMIENTO >= fechDesde && x.FECHAMOVIMIENTO < fechHasta) && x.ANULADO == false).ToList();

                            }
                            else if (desdeSaldo == 0 && hastaSaldo != 0 && desdeSaldoAno != 0)
                            {
                                int days = DateTime.DaysInMonth(desdeSaldoAno, hastaSaldo);
                                DateTime fechHasta = new DateTime(desdeSaldoAno, hastaSaldo, days, 0, 0, 0).AddDays(1);
                                saldosCuentas = db.Movimientos.Where(x => x.FECHAMOVIMIENTO < fechHasta && x.ANULADO == false).ToList();

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
                                saldo = 0;
                                var data = activos.Where(x => x.CUENTA == item.CUENTA).ToList();
                                saldo = data.Select(x => x.DEBITO).Sum() - data.Select(x => x.CREDITO).Sum();
                                totalActivo += saldo;



                                ws.Cells["A" + G].Value = item.CUENTA;
                                ws.Cells["B" + G].Value = (item.cuentaFK == null) ? " " : item.cuentaFK.NOMBRE;
                                ws.Cells["C" + G].Value = saldo;
                                ws.Cells["D" + G].Value = (item.cuentaFK == null) ? " " : item.cuentaFK.NATURALEZA;

                                G++;
                            }
                            J = 7;
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
                                    saldo = 0;
                                    var data = pasivos.Where(x => x.CUENTA == item.CUENTA).ToList();
                                    saldo = saldo = data.Select(x => x.CREDITO).Sum() - data.Select(x => x.DEBITO).Sum();
                                    totalPasivo += saldo;

                                    ws.Cells["H" + J].Value = saldo;

                                    ws.Cells["F" + J].Value = item.CUENTA;
                                    ws.Cells["G" + J].Value = (item.cuentaFK == null) ? " " : item.cuentaFK.NOMBRE;
                                    ws.Cells["I" + J].Value = (item.cuentaFK == null) ? " " : item.cuentaFK.NATURALEZA;

                                    J++;
                                }
                            }
                            J++;
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

                            G++;
                            if (G > J)
                            {
                                G++;
                                ws.Cells["B" + G].Value = "ACTIVO";
                                ws.Cells["C" + G].Value = totalActivo;
                                ws.Cells["G" + G].Value = "PASIVO+PATRIMONIO";
                                ws.Cells["H" + G].Value = totalPasivo;
                            }
                            else
                            {
                                J += 3;
                                ws.Cells["B" + J].Value = "ACTIVO";
                                ws.Cells["C" + J].Value = totalActivo;
                                ws.Cells["G" + J].Value = "PASIVO+PATRIMONIO";
                                ws.Cells["H" + J].Value = totalPasivo;
                            }


                        }

                        saldosCuentas = null;
                        PlanCuentas = null;
                        #endregion
                        break;
                    case 6:
                        #region Informe6 Libro Diario

                        List<MovimientoAuxiliar2> movtos = new List<MovimientoAuxiliar2>();
                        // string fechas = ""; 

                        if (fechaDesde != "" && fechaHasta != "")
                        {
                            DateTime fh = Convert.ToDateTime(fechaHasta);
                            DateTime fd = Convert.ToDateTime(fechaDesde);
                            DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                            DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                            movtos = db.Database.SqlQuery<MovimientoAuxiliar2>(
                                "dbo.sp_Movimientos @fechaDesde,@fechaHasta,@opcion",
                                new SqlParameter("@fechaDesde", fechDesde),
                                new SqlParameter("@fechaHasta", fechHasta),
                                new SqlParameter("@opcion", 1)
                                ).ToList();
                            filtro = fd.ToShortDateString() + " - " + fh.ToShortDateString();
                        }
                        else if (fechaDesde != "" && fechaHasta == "")
                        {
                            DateTime fd = Convert.ToDateTime(fechaDesde);
                            DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                            movtos = db.Database.SqlQuery<MovimientoAuxiliar2>(
                                "dbo.sp_Movimientos @fechaDesde,@fechaHasta,@opcion",
                                new SqlParameter("@fechaDesde", fechDesde),
                                new SqlParameter("@fechaHasta", fechDesde),
                                new SqlParameter("@opcion", 2)
                                ).ToList();
                            filtro = fd.ToShortDateString();
                        }
                        else if (fechaDesde == "" && fechaHasta != "")
                        {
                            DateTime fh = Convert.ToDateTime(fechaHasta);
                            DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                            movtos = db.Database.SqlQuery<MovimientoAuxiliar2>(
                                "dbo.sp_Movimientos @fechaDesde,@fechaHasta,@opcion",
                                new SqlParameter("@fechaDesde", fechHasta),
                                new SqlParameter("@fechaHasta", fechHasta),
                                new SqlParameter("@opcion", 3)
                                ).ToList();
                            filtro = fh.ToShortDateString();
                        }

                        ws = pack.Workbook.Worksheets.Add("librodiario");
                        // encabezado
                        ws.Cells["A1:K1,A2:K2,A3:K3,A4:K4,A5:K5"].Merge = true;
                        ws.Cells["A2:K2,A3:K3,A4:K4"].Style.Font.Bold = true;
                        ws.Cells["A2:K2"].Style.Font.Name = "Arial";
                        ws.Cells["A2:K2"].Style.Font.Size = 14;
                        ws.Cells["A" + 2].Value = "LIBRO DIARIO   " + filtro;
                        ws.Cells[1, 1, 5, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 5, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        ws.Cells["A2:k2,A3:K3,A4:K4,A5:K5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A2:k2,A3:K3,A4:K4,A7:K7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A2:k2,A3:K3,A4:K4,A7:K7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A2:k2,A3:K3,A4:K4,A5:K5"].Style.WrapText = true;
                        ws.Cells["A3:K3,A4:K4"].Style.Font.Size = 12;
                        ws.Cells["A5:K5"].Style.Font.Size = 10;
                        ws.Cells["A" + 3].Value = nombre;
                        ws.Cells["A" + 4].Value = nit;
                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        ws.Cells["A6,K6"].Merge = true;
                        ws.Cells[6, 1, 6, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[6, 1, 6, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                        ws.Cells[6, 1, 6, 11].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        ws.Cells[7, 1, 7, 11].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        ws.Cells["A7:L7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        ws.Cells["A7:K7"].Style.Font.Bold = true;
                        ws.Cells[7, 1, 7, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[7, 1, 7, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        ws.Cells["A" + 7].Value = "TIPO";
                        ws.Cells["B" + 7].Value = "NÚMERO";
                        ws.Cells["C" + 7].Value = "FECHA";
                        ws.Cells["D" + 7].Value = "CUENTA";
                        ws.Cells["E" + 7].Value = "TERCERO";
                        ws.Cells["F" + 7].Value = "NOMBRE TERCERO";
                        ws.Cells["G" + 7].Value = "DETALLE";
                        ws.Cells["H" + 7].Value = "DÉBITO";
                        ws.Cells["I" + 7].Value = "CRÉDITO";
                        ws.Cells["J" + 7].Value = "CCOSTO";
                        ws.Cells["K" + 7].Value = "BASE";
                        //fin encabezado
                        if (movtos.Count > 0)
                        {
                            i = 8;

                            var tupla = movtos.FirstOrDefault();
                            string tipo = tupla.TIPO;
                            string numero = tupla.NUMERO;
                            debito = 0;
                            credito = 0;

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
                                ws.Cells["F" + i].Value = item.NOMBRE;
                                ws.Cells["G" + i].Value = item.DETALLE;
                                ws.Cells["H" + i].Value = item.DEBITO.ToString("N0", formato);
                                ws.Cells["I" + i].Value = item.CREDITO.ToString("N0", formato);
                                ws.Cells["J" + i].Value = item.CCOSTO;
                                ws.Cells["K" + i].Value = item.BASE;

                                i++;
                            }
                        }

                        movtos = null;
                        #endregion
                        break;
                    case 7:
                        #region Informe7 Libro mayor y balances
                        movtos = new List<MovimientoAuxiliar2>();
                        var movtos2 = new List<MovimientoAuxiliar2>();
                        // string periodo = "";
                        if (fechaDesde != "" && fechaHasta != "")
                        {
                            DateTime fh = Convert.ToDateTime(fechaHasta);
                            DateTime fd = Convert.ToDateTime(fechaDesde);
                            DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                            DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                            movtos = db.Database.SqlQuery<MovimientoAuxiliar2>(
                                "dbo.sp_Movimientos @fechaDesde,@fechaHasta,@opcion",
                                new SqlParameter("@fechaDesde", fechDesde),
                                new SqlParameter("@fechaHasta", fechHasta),
                                new SqlParameter("@opcion", 1)
                                ).ToList();
                            movtos2 = db.Database.SqlQuery<MovimientoAuxiliar2>(
                                "dbo.sp_Movimientos @fechaDesde,@fechaHasta,@opcion",
                                new SqlParameter("@fechaDesde", fechDesde),
                                new SqlParameter("@fechaHasta", fechHasta),
                                new SqlParameter("@opcion", 4)
                                ).ToList();

                            filtro = fd.ToShortDateString() + " - " + fh.ToShortDateString();

                        }
                        else if (fechaDesde != "" && fechaHasta == "")
                        {
                            DateTime fd = Convert.ToDateTime(fechaDesde);
                            DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                            movtos = db.Database.SqlQuery<MovimientoAuxiliar2>(
                                "dbo.sp_Movimientos @fechaDesde,@fechaHasta,@opcion",
                                new SqlParameter("@fechaDesde", fechDesde),
                                new SqlParameter("@fechaHasta", fechDesde),
                                new SqlParameter("@opcion", 2)
                                ).ToList();
                            movtos2 = db.Database.SqlQuery<MovimientoAuxiliar2>(
                                "dbo.sp_Movimientos @fechaDesde,@fechaHasta,@opcion",
                                new SqlParameter("@fechaDesde", fechDesde),
                                new SqlParameter("@fechaHasta", fechDesde),
                                new SqlParameter("@opcion", 4)
                                ).ToList();
                            filtro = fd.ToShortDateString();
                        }
                        else if (fechaDesde == "" && fechaHasta != "")
                        {
                            DateTime fh = Convert.ToDateTime(fechaHasta);
                            DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                            movtos = db.Database.SqlQuery<MovimientoAuxiliar2>(
                                "dbo.sp_Movimientos @fechaDesde,@fechaHasta,@opcion",
                                new SqlParameter("@fechaDesde", fechHasta),
                                new SqlParameter("@fechaHasta", fechHasta),
                                new SqlParameter("@opcion", 3)
                                ).ToList();
                            filtro = fh.ToShortDateString();
                        }


                        var movAux = (from m in movtos
                                      orderby m.CUENTA
                                      select new { m.CUENTA, m.NATURALEZA, m.NOMBRECUENTA }).Distinct().ToList();

                        ws = pack.Workbook.Worksheets.Add("Libro Mayor y Balances");
                        // encabezado
                        ws.Cells["A1:F1,A2:F2,A3:F3,A4:F4,A5:F5"].Merge = true;
                        ws.Cells["A2:F2,A3:F3,A4:F4"].Style.Font.Bold = true;
                        ws.Cells["A2:F2"].Style.Font.Name = "Arial";
                        ws.Cells["A2:F2"].Style.Font.Size = 14;
                        ws.Cells["A" + 2].Value = "LIBRO MAYOR Y BALANCES   " + filtro;
                        ws.Cells[1, 1, 5, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 5, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        ws.Cells["A2:F2,A3:F3,A4:F4,A5:F5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A2:F2,A3:F3,A4:F4,A7:F7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A2:F2,A3:F3,A4:F4,A7:F7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A2:F2,A3:F3,A4:F4,A5:F5"].Style.WrapText = true;
                        ws.Cells["A3:F3,A4:F4"].Style.Font.Size = 12;
                        ws.Cells["A5:F5"].Style.Font.Size = 10;
                        ws.Cells["A" + 3].Value = nombre;
                        ws.Cells["A" + 4].Value = nit;
                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        ws.Cells["A6,F6"].Merge = true;
                        ws.Cells[6, 1, 6, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[6, 1, 6, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                        ws.Cells[6, 1, 6, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        ws.Cells[7, 1, 7, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        ws.Cells["A7:G7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        ws.Cells["A7:F7"].Style.Font.Bold = true;
                        ws.Cells[7, 1, 7, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[7, 1, 7, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        ws.Cells["A" + 7].Value = "CÓDIGO";
                        ws.Cells["B" + 7].Value = "DETALLE";
                        ws.Cells["C" + 7].Value = "SALDO INICIAL";
                        ws.Cells["D" + 7].Value = "DEBE";
                        ws.Cells["E" + 7].Value = "HABER";
                        ws.Cells["F" + 7].Value = "SALDO FINAL";

                        //fin encabezado

                        j = 8;
                        debito = 0; credito = 0;
                        decimal debitoAnterior = 0, creditoAnterior = 0, totalDebito = 0, totalCredito = 0, saldoAnterior = 0;
                        foreach (var item in movAux)
                        {

                            var data = movtos.Where(x => x.CUENTA == item.CUENTA).ToList();
                            var data2 = movtos2.Where(x => x.CUENTA == item.CUENTA).ToList();
                            debito = data.Select(x => x.DEBITO).Sum();
                            credito = data.Select(x => x.CREDITO).Sum();
                            debitoAnterior = data2.Select(x => x.DEBITO).Sum();
                            creditoAnterior = data2.Select(x => x.CREDITO).Sum();
                            if (item.NATURALEZA == "D")
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
                            ws.Cells["B" + j].Value = item.NOMBRECUENTA;
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
                        #endregion
                        movtos = null;
                        movtos2 = null;
                        break;
                    case 8:
                        #region Estado de situación financiera
                        string dia = "", anio = "";
                        var fecha = coll["fechaHasta5"];
                        var costo = coll["costo"];

                        if (fecha != "")
                        {
                            DateTime f = Convert.ToDateTime(fecha);
                            filtro = f.ToShortDateString();
                        }

                        ws = pack.Workbook.Worksheets.Add("Estado de Situación Financiera");

                        // encabezado
                        ws.Cells["A1:E1,A2:E2,A3:E3,A4:E4,A5:E5"].Merge = true;
                        ws.Cells["A2:E2,A3:E3,A4:E4"].Style.Font.Bold = true;
                        ws.Cells["A2:E2"].Style.Font.Name = "Arial";
                        ws.Cells["A2:E2"].Style.Font.Size = 14;
                        ws.Cells["A" + 2].Value = "ESTADO DE SITUACIÓN FINANCIERA  " + filtro;
                        ws.Cells[1, 1, 5, 5].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 5, 5].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        ws.Cells["A2:E2,A3:E3,A4:E4,A5:E5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A2:E2,A3:E3,A4:E4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A2:E2,A3:E3,A4:E4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A2:E2,A3:E3,A4:E4,A5:E5"].Style.WrapText = true;
                        ws.Cells["A3:E3,A4:E4"].Style.Font.Size = 12;
                        ws.Cells["A5:E5"].Style.Font.Size = 10;
                        ws.Cells["A" + 3].Value = nombre;
                        ws.Cells["A" + 4].Value = nit;
                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        ws.Cells["A6:E6"].Merge = true;

                        //fin encabezado

                        if (fecha != "")
                        {
                            DateTime f = Convert.ToDateTime(fecha);
                            DateTime fechFinActual = new DateTime(f.Year, f.Month, f.Day, 23, 59, 59);
                            DateTime fechMovIniActual = new DateTime(f.Year, 1, 1, 0, 0, 0);
                            DateTime fechMovIniAnterior = new DateTime(f.Year - 1, 1, 1, 0, 0, 0);
                            DateTime fechFinAnterior = new DateTime(f.Year - 1, f.Month, f.Day, 23, 59, 59);
                            ws.Cells["B" + 7].Value = "Años terminados el " + f.Day.ToString() + " de " + GetMes(f.Month);
                            ws.Cells["D" + 7].Value = f.Day + " de " + GetMes(f.Month) + " de " + f.Year;
                            ws.Cells["E" + 7].Value = f.Day + " de " + GetMes(f.Month) + " de " + (f.Year - 1).ToString();
                            ws.Cells["B" + 9].Value = "ACTIVO";
                            j = 10;

                            //consultas

                            var listMovimientos = db.Movimientos.Where(x => ((x.FECHAMOVIMIENTO >= fechMovIniActual && x.FECHAMOVIMIENTO <= fechFinActual) || (x.FECHAMOVIMIENTO >= fechMovIniAnterior && x.FECHAMOVIMIENTO <= fechFinAnterior)) && x.Comprobante.ANULADO == false).ToList();
                            if (costo != "")
                            {
                                listMovimientos = listMovimientos.Where(x => x.CCOSTO == costo).ToList();
                            }

                            var planCuentas = db.PlanCuentas.ToList();

                            var cuentasNIIF = planCuentas.Where(x => x.EsCuentaNIIF == true).ToList();

                            credito = 0; debito = 0; saldo = 0;
                            decimal totalAnterior = 0, totalPatrimonioActual = 0, totalPatrimonioAnterior = 0, superPatrimonioActual = 0, superPatrimonioAnterior = 0;
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
                                    var cuentasESF = planCuentas.Where(x => x.CTANIIF == item.CODIGO).OrderBy(x => x.CODIGO).ToList();
                                    foreach (var item2 in cuentasESF)
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
                                    var cuentasESF = planCuentas.Where(x => x.CTANIIF == item.CODIGO).OrderBy(x => x.CODIGO).ToList();
                                    foreach (var item2 in cuentasESF)
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
                                    var cuentasESF = planCuentas.Where(x => x.CTANIIF == item.CODIGO).OrderBy(x => x.CODIGO).ToList();
                                    foreach (var item2 in cuentasESF)
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


                            listMovimientos = null;
                        }


                        ws.Cells[ws.Dimension.Address].AutoFitColumns();//siempre al final de todo. le da tamaño ajustado a cada columna

                        #endregion
                        break;
                    case 14:
                        #region Estado de Resultados Integral

                        if (fechaDesde != "" && fechaHasta != "")
                        {
                            DateTime fd = Convert.ToDateTime(fechaDesde);
                            DateTime fh = Convert.ToDateTime(fechaHasta);
                            filtro = fd.ToShortDateString() + " - " + fh.ToShortDateString();
                        }
                        else if (fechaDesde != "" && fechaHasta == "")
                        {
                            DateTime fd = Convert.ToDateTime(fechaDesde);
                            filtro = fd.ToShortDateString();
                        }
                        else if (fechaDesde == "" && fechaHasta != "")
                        {
                            DateTime fh = Convert.ToDateTime(fechaHasta);
                            filtro = fh.ToShortDateString();
                        }

                        ws = pack.Workbook.Worksheets.Add("Estado de Resultados Integral");

                        // encabezado
                        ws.Cells["A1:F1,A2:F2,A3:F3,A4:F4,A5:F5"].Merge = true;
                        ws.Cells["A2:F2,A3:F3,A4:F4"].Style.Font.Bold = true;
                        ws.Cells["A2:F2"].Style.Font.Name = "Arial";
                        ws.Cells["A2:F2"].Style.Font.Size = 12;
                        ws.Cells["A" + 2].Value = "ESTADO DE RESULTADOS INTEGRAL   " + filtro;
                        ws.Cells[1, 1, 5, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 5, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        ws.Cells["A2:F2,A3:F3,A4:F4,A5:F5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A2:F2,A3:F3,A4:F4"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A2:F2,A3:F3,A4:F4"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A2:F2,A3:F3,A4:F4,A5:F5"].Style.WrapText = true;
                        ws.Cells["A3:F3,A4:F4"].Style.Font.Size = 12;
                        ws.Cells["A5:F5"].Style.Font.Size = 10;
                        ws.Cells["A" + 3].Value = nombre;
                        ws.Cells["A" + 4].Value = nit;
                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        ws.Cells["A6:F6"].Merge = true;

                        //fin encabezado

                        fDesde = coll["fechaDesde"];
                        fHasta = coll["fechaHasta"];

                        movtos = new List<MovimientoAuxiliar2>();

                        if (fechaDesde != "" && fechaHasta != "")
                        {
                            DateTime fh = Convert.ToDateTime(fechaHasta);
                            DateTime fd = Convert.ToDateTime(fechaDesde);
                            DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                            DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                            movtos = db.Database.SqlQuery<MovimientoAuxiliar2>(
                                "dbo.sp_Movimientos @fechaDesde,@fechaHasta,@opcion",
                                new SqlParameter("@fechaDesde", fechDesde),
                                new SqlParameter("@fechaHasta", fechHasta),
                                new SqlParameter("@opcion", 1)
                                ).ToList();
                        }
                        else if (fechaDesde != "" && fechaHasta == "")
                        {
                            DateTime fd = Convert.ToDateTime(fechaDesde);
                            DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                            movtos = db.Database.SqlQuery<MovimientoAuxiliar2>(
                                "dbo.sp_Movimientos @fechaDesde,@fechaHasta,@opcion",
                                new SqlParameter("@fechaDesde", fechDesde),
                                new SqlParameter("@fechaHasta", fechDesde),
                                new SqlParameter("@opcion", 2)
                                ).ToList();

                        }
                        else if (fechaDesde == "" && fechaHasta != "")
                        {
                            DateTime fh = Convert.ToDateTime(fechaHasta);
                            DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                            movtos = db.Database.SqlQuery<MovimientoAuxiliar2>(
                                "dbo.sp_Movimientos",
                                new SqlParameter("@fechaDesde", fechHasta),
                                new SqlParameter("@fechaHasta", fechHasta),
                                new SqlParameter("@opcion", 3)
                                ).ToList();

                        }

                        if (movtos.Count > 0)
                        {
                            var agenciaERI = coll["agencia"].ToString();
                            var tipoInforme = coll["typeInforme"];
                            credito = 0; debito = 0;
                            decimal totalIngresos = 0, totalGastos = 0, totGastosOrdinarios = 0, totOtrosGastos = 0, totOtros = 0, totalCostos = 0;
                            if (agenciaERI != "")
                            {
                                int agency = Convert.ToInt32(agenciaERI);
                                movtos = movtos.Where(x => x.AGENCIA == agency).ToList();
                            }

                            if (tipoInforme != "")
                            {
                                int tipoinforme = Convert.ToInt32(tipoInforme);
                                if (tipoinforme == 1)
                                {
                                    ws.Cells["B" + 7].Value = "INGRESOS ORDINARIOS";
                                    ws.Cells["C" + 7].Value = "SALDO";

                                    j = 9;

                                    //ingresos

                                    var ingresos = movtos.Where(x => x.CUENTA.StartsWith("4")).ToList();
                                    var cicloIngresos = (from ing in ingresos
                                                         orderby ing.CUENTA
                                                         select new { ing.CUENTA, ing.NOMBRECUENTA }
                                                         ).Distinct().ToList();
                                    foreach (var item in cicloIngresos)
                                    {
                                        var data = ingresos.Where(x => x.CUENTA == item.CUENTA).ToList();
                                        credito = data.Select(x => x.CREDITO).Sum();
                                        debito = data.Select(x => x.DEBITO).Sum();
                                        totalIngresos += credito - debito;

                                        ws.Cells["B" + j].Value = item.NOMBRECUENTA;
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
                                                       select new { ing.CUENTA, ing.NOMBRECUENTA }
                                                         ).Distinct().ToList();

                                    foreach (var item in cicloGastos)
                                    {
                                        var data = gastos.Where(x => x.CUENTA == item.CUENTA).ToList();
                                        credito = data.Select(x => x.CREDITO).Sum();
                                        debito = data.Select(x => x.DEBITO).Sum();
                                        totGastosOrdinarios += debito - credito;

                                        ws.Cells["B" + j].Value = item.NOMBRECUENTA;
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
                                                   select new { ing.CUENTA, ing.NOMBRECUENTA }
                                                         ).Distinct().ToList();

                                    foreach (var item in cicloGastos)
                                    {
                                        var data = gastos.Where(x => x.CUENTA == item.CUENTA).ToList();
                                        credito = data.Select(x => x.CREDITO).Sum();
                                        debito = data.Select(x => x.DEBITO).Sum();
                                        totOtrosGastos += debito - credito;

                                        ws.Cells["B" + j].Value = item.NOMBRECUENTA;
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
                                                   select new { ing.CUENTA, ing.NOMBRECUENTA }
                                                         ).Distinct().ToList();

                                    foreach (var item in cicloGastos)
                                    {
                                        var data = gastos.Where(x => x.CUENTA == item.CUENTA).ToList();
                                        credito = data.Select(x => x.CREDITO).Sum();
                                        debito = data.Select(x => x.DEBITO).Sum();
                                        totOtros += debito - credito;

                                        ws.Cells["B" + j].Value = item.NOMBRECUENTA;
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
                                                       select new { ctos.CUENTA, ctos.NOMBRECUENTA }
                                                         ).Distinct().ToList();
                                    foreach (var item in cicloCostos)
                                    {
                                        var data = costos.Where(x => x.CUENTA == item.CUENTA).ToList();
                                        credito = data.Select(x => x.CREDITO).Sum();
                                        debito = data.Select(x => x.DEBITO).Sum();
                                        totalCostos += debito - credito;

                                        ws.Cells["B" + j].Value = item.NOMBRECUENTA;
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
                                    ws.Cells["B" + 7].Value = "INGRESOS ORDINARIOS";
                                    ws.Cells["C" + 7].Value = totalIngresos.ToString("N0", formato);

                                    credito = gastos.Select(x => x.CREDITO).Sum();
                                    debito = gastos.Select(x => x.DEBITO).Sum();
                                    totGastosOrdinarios = debito - credito;
                                    ws.Cells["B" + 8].Value = "GASTOS ORDINARIOS";
                                    ws.Cells["C" + 8].Value = totGastosOrdinarios.ToString("N0", formato);

                                    credito = otrosGastos.Select(x => x.CREDITO).Sum();
                                    debito = otrosGastos.Select(x => x.DEBITO).Sum();
                                    totOtrosGastos = debito - credito;
                                    ws.Cells["B" + 9].Value = "OTROS GASTOS";
                                    ws.Cells["C" + 9].Value = totOtrosGastos.ToString("N0", formato);

                                    credito = otros.Select(x => x.CREDITO).Sum();
                                    debito = otros.Select(x => x.DEBITO).Sum();
                                    totOtros = debito - credito;
                                    ws.Cells["B" + 10].Value = "OTROS";
                                    ws.Cells["C" + 10].Value = totOtros.ToString("N0", formato);

                                    totalGastos = totGastosOrdinarios + totOtrosGastos + totOtros;

                                    //costos
                                    credito = costos.Select(x => x.CREDITO).Sum();
                                    debito = costos.Select(x => x.DEBITO).Sum();
                                    totalCostos = debito - credito;
                                    ws.Cells["B" + 11].Value = "COSTOS";
                                    ws.Cells["C" + 11].Value = totalCostos.ToString("N0", formato);

                                    ws.Cells["B" + 13].Value = "UTILIDAD DEL EJERCICIO";
                                    ws.Cells["C" + 13].Value = (totalIngresos - totalGastos - totalCostos).ToString("N0", formato);
                                }

                                ws.Cells[ws.Dimension.Address].AutoFitColumns();//siempre al final de todo. le da tamaño ajustado a cada columna
                            }

                        }
                        movtos = null;
                        #endregion
                        break;

                    case 17:
                        #region informe17 Morosidad de aportes
                        var agencia = coll["agencia"];
                        var morosidad2 = db.Database.SqlQuery<sp_reporte17>("dbo.sp_reporte17").ToList();
                        //var morosidad2 = db.FichasAportes.Where(x => x.activa == true).ToList();
                        //var factCaja = db.FactOpcaja.ToList();
                        if (agencia != "")
                        {
                            int agency = Convert.ToInt32(agencia);
                            morosidad2 = morosidad2.Where(x => x.codigoagencia == agency).ToList();
                        }

                        if (fechaDesde != "")
                        {
                            DateTime fechDesde3 = Convert.ToDateTime(fechaDesde);
                            DateTime fechaActual = DateTime.Now;
                            DateTime fechaActual2 = new DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day, 23, 59, 59);
                            morosidad2 = morosidad2.Where(x => x.FechaApertura >= fechDesde3 && x.FechaApertura <= fechaActual2).ToList();

                            filtro = fechDesde3.ToShortDateString();
                            if (fechaHasta != "")
                            {

                                DateTime fech5 = Convert.ToDateTime(fechaHasta);
                                DateTime fech6 = new DateTime(fech5.Year, fech5.Month, fech5.Day, 23, 59, 59);
                                morosidad2 = morosidad2.Where(x => x.FechaApertura >= fechDesde3 && x.FechaApertura <= fech6).ToList();
                                filtro = fechDesde3.ToShortDateString() + " - " + fech5.ToShortDateString();
                            }

                        }

                        morosidad2 = morosidad2.OrderBy(m => m.FechaApertura).ToList();




                        ws = pack.Workbook.Worksheets.Add("Morosidad de aportes");

                        // encabezado
                        ws.Cells["A1:L1,A2:L2,A3:L3,A4:L4,A5:L5"].Merge = true;
                        ws.Cells["A2:L2,A3:L3,A4:L4"].Style.Font.Bold = true;
                        ws.Cells["A2:L2"].Style.Font.Name = "Arial";
                        ws.Cells["A2:L2"].Style.Font.Size = 14;
                        ws.Cells["A" + 2].Value = "MOROSIDAD POR APORTES   " + filtro;
                        ws.Cells[1, 1, 5, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 5, 12].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        ws.Cells["A2:L2,A3:L3,A4:L4,A5:L5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A2:L2,A3:L3,A4:L4,A7:L7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A2:L2,A3:L3,A4:L4,A7:L7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A2:L2,A3:L3,A4:L4,A5:L5"].Style.WrapText = true;
                        ws.Cells["A3:L3,A4:L4"].Style.Font.Size = 12;
                        ws.Cells["A5:L5"].Style.Font.Size = 10;
                        ws.Cells["A" + 3].Value = nombre;
                        ws.Cells["A" + 4].Value = nit;
                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        ws.Cells["A6:L6"].Merge = true;
                        ws.Cells[6, 1, 6, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[6, 1, 6, 12].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                        ws.Cells[6, 1, 6, 12].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        ws.Cells[7, 1, 7, 12].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        ws.Cells["A7:M7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        ws.Cells["A7:L7"].Style.Font.Bold = true;
                        ws.Cells[7, 1, 7, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[7, 1, 7, 12].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        //fin encabezado

                        ws.Cells["A" + 7].Value = "CUENTA";
                        ws.Cells["B" + 7].Value = "IDENTIFICACIÓN";
                        ws.Cells["C" + 7].Value = "NOMBRE";
                        ws.Cells["D" + 7].Value = "FORMA DE PAGO";
                        ws.Cells["E" + 7].Value = "VALOR";
                        ws.Cells["F" + 7].Value = "TOTAL APORTES";
                        ws.Cells["G" + 7].Value = "FECHA DE AFILIACIÓN";
                        ws.Cells["H" + 7].Value = "CUOTAS EN MORA";
                        ws.Cells["I" + 7].Value = "DEUDA TOTAL";
                        ws.Cells["J" + 7].Value = "ESTADO";
                        ws.Cells["K" + 7].Value = "CELULAR";
                        ws.Cells["L" + 7].Value = "AGENCIA";

                        i = 8;

                        foreach (var item in morosidad2)
                        {

                            int diferenciaMeses = 0;
                            int diferenciaanios = 0;

                            DateTime fechApertura = Convert.ToDateTime(item.FechaApertura);
                            string fechNow = "";
                            if (fechaHasta != "")
                            {
                                DateTime f3 = Convert.ToDateTime(fechaHasta);
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


                            int num = item.Npagos;
                            int deudaTotal = 0, n = 0;
                            deudaTotal = Convert.ToInt32(item.valor) * ((diferenciaMeses + 1) - num);

                            n = (diferenciaMeses + 1) - num;
                            if (n < 0)
                            {
                                n = 0;
                                deudaTotal = 0;
                            }

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
                            ws.Cells["A" + i].Value = item.NumeroCuenta;
                            ws.Cells["B" + i].Value = item.idPersona;
                            ws.Cells["C" + i].Value = item.nombre;
                            ws.Cells["D" + i].Value = item.tipoPago;
                            ws.Cells["E" + i].Value = item.valor;
                            ws.Cells["F" + i].Value = item.totalAportes;
                            ws.Cells["G" + i].Value = item.FechaApertura.ToString();
                            ws.Cells["H" + i].Value = n;
                            ws.Cells["I" + i].Value = deudaTotal;
                            ws.Cells["J" + i].Value = estado;
                            ws.Cells["K" + i].Value = item.TELMOVIL;
                            ws.Cells["L" + i].Value = item.nombreagencia;

                            i++;



                        }
                        morosidad2 = null;
                        #endregion
                        break;
                    case 22:
                        #region Informe AUXILIAR POR TERCERO ACTUALIZADO

                        var desdF = DateTime.Now;
                        var hastF = DateTime.Now;
                        var opAC = 0;
                        var filtroCuentas = "";

                        List<SpAuxiliarPorTercero> datosAuxT = new List<SpAuxiliarPorTercero>();

                        #region FILTRO PARA BUSQUEDA DE DATOS 
                        if (fechaDesde != "" && fechaHasta != "")
                        {
                            desdF = Convert.ToDateTime(fechaDesde);
                            hastF = Convert.ToDateTime(fechaHasta);
                            filtro = desdF.ToShortDateString() + " - " + hastF.ToShortDateString();

                            if (cuentaA != "" && cuentaB != "" && documento != "")
                            {
                                filtroCuentas = "Filtro desde cuenta: " + cuentaA + "  Hasta cuenta: " + cuentaB + " e Identificación: " + documento;
                                opAC = 1;// filtro por fecha desde, fecha hasta, cuenta desde, cuenta hasta y tercero
                            }
                            else if (cuenta != "" && cuentaA == "" && cuentaB == "" && documento != "")
                            {
                                filtroCuentas = "Filtro por Cuenta: " + cuenta + " e Identificación: " + documento;
                                opAC = 2;// filtro por fecha desde, fecha hasta, cuenta y tercero
                            }
                            else if (cuenta != "" && cuentaA == "" && cuentaB == "")
                            {
                                filtroCuentas = "Filtro por cuenta: " + cuenta;
                                opAC = 3;//filtro por fecha desde, fecha hasta y cuenta
                            }
                            else if (cuentaA != "" && cuentaB != "")
                            {
                                filtroCuentas = "Filtro desde la cuenta: " + cuentaA + " Hasta la cuenta " + cuentaB;
                                opAC = 4;//filtro por fecha desde, fecha hasta, cuenta desde, cuenta hasta 
                            }
                            else if (cuenta == "" && cuentaA == "" && cuentaB == "" && documento != "")
                            {
                                filtroCuentas = "Filtro por identificación: " + documento;
                                opAC = 5;//filtro por fecha desde, fecha hasta y tercero
                            }
                            else if (cuentaA != "" && cuentaB == "" && documento != "")
                            {
                                filtroCuentas = "Filtro desde cuenta" + cuentaA + " e identificacion" + documento;
                                opAC = 6;//filtro por fecha desde, fecha hasta, cuenta desde y tercero
                            }
                            else if (cuentaA != "" && cuentaB == "" && documento == "")
                            {
                                filtroCuentas = "Filtro desde la cuenta: " + cuentaA;
                                opAC = 7;//filtro por fecha desde, fecha hasta, cuenta desde
                            }
                            else if (cuentaA == "" && cuentaB != "" && documento != "")
                            {
                                filtroCuentas = "Filtro hasta la cuenta: " + cuentaB + " e identificación: " + documento;
                                opAC = 8;//filtro por fecha desde, fecha hasta, cuenta hasta y tercero
                            }
                            else if (cuentaA == "" && cuentaB != "" && documento == "")
                            {
                                filtroCuentas = "Filtro hasta la cuenta: " + cuentaB;
                                opAC = 9;//filtro por fecha desde, fecha hasta, cuenta hasta y tercero
                            }
                            else if (cuenta == "" && cuentaA == "" && cuentaB == "" && documento == "")
                            {
                                opAC = 10;// filtro por fecha desde y fecha hasta trae todo
                            }
                        }

                        DateTime fechDesd = new DateTime(desdF.Year, desdF.Month, desdF.Day, 0, 0, 0);
                        DateTime fechHast = new DateTime(hastF.Year, hastF.Month, hastF.Day, 23, 59, 59);

                        #endregion

                        datosAuxT = db.Database.SqlQuery<SpAuxiliarPorTercero>(
                            "dbo.sp_AuxiliarPorTerceros @documento, @cuenta, @cuentaA,@cuentaB,@FechaDesde, @FechaHasta, @opcion",
                            new SqlParameter("@documento", documento),
                            new SqlParameter("@cuenta", cuenta),
                            new SqlParameter("@cuentaA", cuentaA),
                            new SqlParameter("@cuentaB", cuentaB),
                            new SqlParameter("@FechaDesde", fechDesd),
                            new SqlParameter("@FechaHasta", fechHast),
                            new SqlParameter("@opcion", opAC)
                            ).ToList();

                        ws = pack.Workbook.Worksheets.Add("AuxiliarPorTerceros");

                        #region encabezado excel
                        ws.Cells["A1:I1,A2:I2,A3:I3,A4:I4,A5:I5,A6:I6"].Merge = true;
                        ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.Font.Bold = true;
                        ws.Cells["A2:I2"].Style.Font.Name = "Arial";
                        ws.Cells["A2:I2"].Style.Font.Size = 14;
                        ws.Cells["A" + 2].Value = "AUXILIAR POR TERCEROS   " + filtro;
                        ws.Cells["A" + 3].Value = filtroCuentas;
                        ws.Cells[1, 1, 6, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 6, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5,A6:I6"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A2:I2,A3:I3,A4:I4,A7:I7,A5:I5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A2:I2,A3:I3,A4:I4,A7:I7,A5:I5"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.WrapText = true;
                        ws.Cells["A4:I4,A5:I5"].Style.Font.Size = 12;
                        ws.Cells["A3:I3"].Style.Font.Size = 13;
                        ws.Cells["A6:I6"].Style.Font.Size = 10;
                        ws.Cells["A" + 4].Value = nombre;
                        ws.Cells["A" + 5].Value = nit;
                        ws.Cells["A" + 6].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        ws.Cells["A7:I7"].Merge = true;
                        ws.Cells[7, 1, 7, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[7, 1, 7, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                        ws.Cells[7, 1, 7, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        ws.Cells[8, 1, 8, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        ws.Cells["A8:J8"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        ws.Cells["A8:I8"].Style.Font.Bold = true;
                        ws.Cells[8, 1, 8, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[8, 1, 8, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        #endregion

                        ws.Cells["A" + 8].Value = "CÓDIGO";
                        ws.Cells["B" + 8].Value = "NOMBRE";
                        ws.Cells["C" + 8].Value = "TERCERO";
                        ws.Cells["D" + 8].Value = "NOMBRE TERCERO";
                        ws.Cells["E" + 8].Value = "COMPROBANTE";
                        ws.Cells["F" + 8].Value = "FECHA";
                        ws.Cells["G" + 8].Value = "DÉBITO";
                        ws.Cells["H" + 8].Value = "CRÉDITO";
                        ws.Cells["I" + 8].Value = "SALDO";

                        var cuentasEnlista = (from a in datosAuxT
                                              orderby a.CUENTA
                                              orderby a.TERCERO
                                              select new { a.CUENTA, a.TERCERO, a.NOMBRECUENTA, a.NOMBRE, a.NATURALEZA }).Distinct().ToList();
                        i = 9;
                        saldo = 0;
                        decimal totalSaldo = 0;

                        foreach (var item in cuentasEnlista)
                        {
                            var dataMov = datosAuxT.Where(x => x.CUENTA == item.CUENTA && x.TERCERO == item.TERCERO).OrderBy(x => x.FECHAMOVIMIENTO).ToList();
                            debito = dataMov.Select(x => x.DEBITO).Sum();
                            credito = dataMov.Select(x => x.CREDITO).Sum();
                            ws.Cells["A" + i].Value = item.CUENTA;
                            ws.Cells["B" + i].Value = item.NOMBRECUENTA;
                            ws.Cells["C" + i].Value = item.TERCERO;
                            ws.Cells["D" + i].Value = item.NOMBRE;
                            i++;
                            foreach (var item2 in dataMov)
                            {
                                if (item.NATURALEZA == "D")
                                {
                                    saldo = item2.DEBITO - item2.CREDITO;
                                }
                                else
                                {
                                    saldo = item2.CREDITO - item2.DEBITO;
                                }

                                ws.Cells["E" + i].Value = item2.COMPROBANTE;
                                ws.Cells["F" + i].Value = item2.FECHAMOVIMIENTO.ToString("yyyy-MM-dd");
                                ws.Cells["G" + i].Value = item2.DEBITO.ToString("N0", formato);
                                ws.Cells["H" + i].Value = item2.CREDITO.ToString("N0", formato);
                                ws.Cells["I" + i].Value = saldo.ToString("N0", formato);
                                i++;
                            }
                            if (item.NATURALEZA == "D")
                            {
                                totalSaldo = debito - credito;
                            }
                            else
                            {
                                totalSaldo = credito - debito;
                            }
                            ws.Cells["F" + i].Style.Font.Bold = true;
                            ws.Cells["F" + i].Value = "TOTAL";
                            ws.Cells["G" + i].Value = debito.ToString("N0", formato);
                            ws.Cells["H" + i].Value = credito.ToString("N0", formato);
                            ws.Cells["I" + i].Value = totalSaldo.ToString("N0", formato);
                            i += 2;
                        }

                        #endregion
                        break;
                    case 18:
                        #region Auxiliar por terceros CORREGIR REPORTE
                        //#region Cuentas AB
                        //string desdeF = "";
                        //string hastaF = "";

                        //if (cuentaA == "")
                        //{
                        //    cuentaA = "0";
                        //}
                        //var cuentaMinT = cuentaA;

                        //var cuentalongminT = Convert.ToInt64(cuentaMinT);
                        //if (cuentaB == "")
                        //{
                        //    cuentaB = "0";
                        //}
                        //var cuentaMaxT = cuentaB;

                        //var cuentalongmaxT = Convert.ToInt64(cuentaMaxT);
                        //#endregion

                        //var movimiento22T = db.Movimientos.Where(x => x.Comprobante.ANULADO == false).ToList();

                        //if (cuenta != "" && cuentaA == "0" && cuentaB == "0")
                        //{
                        //    movimiento22T = movimiento22T.Where(x => x.CUENTA == cuenta).ToList();
                        //}
                        //if (documento != "")
                        //{
                        //    movimiento22T = movimiento22T.Where(x => x.TERCERO == documento).ToList();
                        //}
                        //if (fechaDesde != "")
                        //{
                        //    DateTime auxfd = Convert.ToDateTime(fechaDesde);
                        //    DateTime fd = new DateTime(auxfd.Year, auxfd.Month, auxfd.Day, 0, 0, 0);
                        //    movimiento22T = movimiento22T.Where(X => X.FECHAMOVIMIENTO >= fd).ToList();
                        //    desdeF = auxfd.ToShortDateString();
                        //}
                        //if (fechaHasta != "")
                        //{
                        //    DateTime auxfh = Convert.ToDateTime(fechaHasta);
                        //    DateTime fh = new DateTime(auxfh.Year, auxfh.Month, auxfh.Day, 23, 59, 59);
                        //    movimiento22T = movimiento22T.Where(X => X.FECHAMOVIMIENTO <= fh).ToList();
                        //    hastaF = auxfh.ToShortDateString();
                        //}
                        //if (fechaDesde == "" && fechaHasta == "")
                        //{
                        //    //error cuando no se manda fechas
                        //    movimiento22T = movimiento22T.Where(x => x.Comprobante.ANULADO == false).ToList();
                        //}
                        //if (fechaDesde != "" && fechaHasta != "")
                        //    filtro = desdeF + " - " + hastaF;
                        //else if (fechaDesde != "" && fechaHasta == "")
                        //    filtro = desdeF;
                        //else if (fechaDesde == "" && fechaHasta != "")
                        //    filtro = hastaF;

                        //if (cuentaA == "0" && cuentaB == "0")
                        //{
                        //    var movimiento22 = movimiento22T;

                        //    ws = pack.Workbook.Worksheets.Add("AuxiliarPorTerceros");

                        //    // encabezado
                        //    ws.Cells["A1:I1,A2:I2,A3:I3,A4:I4,A5:I5"].Merge = true;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4"].Style.Font.Bold = true;
                        //    ws.Cells["A2:I2"].Style.Font.Name = "Arial";
                        //    ws.Cells["A2:I2"].Style.Font.Size = 14;
                        //    ws.Cells["A" + 2].Value = "AUXILIAR POR TERCEROS   " + filtro;
                        //    ws.Cells[1, 1, 5, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[1, 1, 5, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A7:I7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A7:I7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.WrapText = true;
                        //    ws.Cells["A3:I3,A4:I4"].Style.Font.Size = 12;
                        //    ws.Cells["A5:I5"].Style.Font.Size = 10;
                        //    ws.Cells["A" + 3].Value = nombre;
                        //    ws.Cells["A" + 4].Value = nit;
                        //    ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        //    ws.Cells["A6:I6"].Merge = true;
                        //    ws.Cells[6, 1, 6, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[6, 1, 6, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                        //    ws.Cells[6, 1, 6, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        //    ws.Cells[7, 1, 7, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        //    ws.Cells["A7:J7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        //    ws.Cells["A7:I7"].Style.Font.Bold = true;
                        //    ws.Cells[7, 1, 7, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[7, 1, 7, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        //    //fin encabezado

                        //    ws.Cells["A" + 7].Value = "CÓDIGO";
                        //    ws.Cells["B" + 7].Value = "NOMBRE";
                        //    ws.Cells["C" + 7].Value = "TERCERO";
                        //    ws.Cells["D" + 7].Value = "NOMBRE TERCERO";
                        //    ws.Cells["E" + 7].Value = "COMPROBANTE";
                        //    ws.Cells["F" + 7].Value = "FECHA";
                        //    ws.Cells["G" + 7].Value = "DÉBITO";
                        //    ws.Cells["H" + 7].Value = "CRÉDITO";
                        //    ws.Cells["I" + 7].Value = "SALDO";

                        //    var mov22 = (from m in movimiento22
                        //                 orderby m.CUENTA
                        //                 orderby m.TERCERO
                        //                 select new { m.CUENTA, m.TERCERO, m.cuentaFK, m.terceroFK }).Distinct().ToList();
                        //    i = 8;
                        //    saldo = 0;
                        //    decimal saldoTotal22 = 0;
                        //    foreach (var item in mov22)
                        //    {
                        //        var dataMov = movimiento22.Where(x => x.CUENTA == item.CUENTA && x.TERCERO == item.TERCERO).OrderBy(x => x.FECHAMOVIMIENTO).ToList();
                        //        debito = dataMov.Select(x => x.DEBITO).Sum();
                        //        credito = dataMov.Select(x => x.CREDITO).Sum();
                        //        ws.Cells["A" + i].Value = item.CUENTA;
                        //        ws.Cells["B" + i].Value = item.cuentaFK.NOMBRE;
                        //        ws.Cells["C" + i].Value = item.TERCERO;
                        //        if (item.terceroFK != null)
                        //            ws.Cells["D" + i].Value = item.terceroFK.NOMBRE1;// + " " + item.terceroFK.NOMBRE2 + " " + item.terceroFK.APELLIDO1 + " " + item.terceroFK.APELLIDO2; 
                        //        else
                        //            ws.Cells["D" + i].Value = "";
                        //        string naturaleza = item.cuentaFK.NATURALEZA;
                        //        i++;
                        //        foreach (var item2 in dataMov)
                        //        {
                        //            if (naturaleza == "D")
                        //            {
                        //                saldo = item2.DEBITO - item2.CREDITO;
                        //            }
                        //            else
                        //            {
                        //                saldo = item2.CREDITO - item2.DEBITO;
                        //            }

                        //            ws.Cells["E" + i].Value = item2.TIPO + " " + item2.NUMERO;
                        //            ws.Cells["F" + i].Value = item2.FECHAMOVIMIENTO.ToString("yyyy-MM-dd");
                        //            ws.Cells["G" + i].Value = item2.DEBITO.ToString("N0", formato);
                        //            ws.Cells["H" + i].Value = item2.CREDITO.ToString("N0", formato);
                        //            ws.Cells["I" + i].Value = saldo.ToString("N0", formato);
                        //            i++;
                        //        }
                        //        if (naturaleza == "D")
                        //        {
                        //            saldoTotal22 = debito - credito;
                        //        }
                        //        else
                        //        {
                        //            saldoTotal22 = credito - debito;
                        //        }
                        //        ws.Cells["F" + i].Value = "TOTAL";
                        //        ws.Cells["G" + i].Value = debito.ToString("N0", formato);
                        //        ws.Cells["H" + i].Value = credito.ToString("N0", formato);
                        //        ws.Cells["I" + i].Value = saldoTotal22.ToString("N0", formato);
                        //        i += 2;
                        //    }
                        //    movimiento22 = null;
                        //}

                        //if (cuentaA != "0" && cuentaB == "0")
                        //{
                        //    var MovimientoTemp = (from a in movimiento22T
                        //                          select new
                        //                          {
                        //                              CUENTA = Convert.ToInt64(a.CUENTA),
                        //                              a.cuentaFK,
                        //                              a.terceroFK,
                        //                              a.FECHAMOVIMIENTO,
                        //                              a.DEBITO,
                        //                              a.CREDITO,
                        //                              a.TIPO,
                        //                              a.TERCERO,
                        //                              a.NUMERO
                        //                          }).Distinct().ToList();

                        //    var MovimientosFiltro = (from a in MovimientoTemp
                        //                             where a.CUENTA >= cuentalongminT
                        //                             select new
                        //                             {
                        //                                 CUENTA = Convert.ToInt64(a.CUENTA),
                        //                                 a.cuentaFK,
                        //                                 a.terceroFK,
                        //                                 a.FECHAMOVIMIENTO,
                        //                                 a.DEBITO,
                        //                                 a.CREDITO,
                        //                                 a.TIPO,
                        //                                 a.TERCERO,
                        //                                 a.NUMERO
                        //                             }).Distinct().ToList();

                        //    var movimiento22 = MovimientosFiltro;

                        //    ws = pack.Workbook.Worksheets.Add("AuxiliarPorTerceros");
                        //    // encabezado
                        //    ws.Cells["A1:I1,A2:I2,A3:I3,A4:I4,A5:I5"].Merge = true;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4"].Style.Font.Bold = true;
                        //    ws.Cells["A2:I2"].Style.Font.Name = "Arial";
                        //    ws.Cells["A2:I2"].Style.Font.Size = 14;
                        //    ws.Cells["A" + 2].Value = "AUXILIAR POR TERCEROS   " + filtro;
                        //    ws.Cells[1, 1, 5, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[1, 1, 5, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5,A7:I7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5,A7:I7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.WrapText = true;
                        //    ws.Cells["A3:I3,A4:I4"].Style.Font.Size = 12;
                        //    ws.Cells["A5:I5"].Style.Font.Size = 10;
                        //    ws.Cells["A" + 3].Value = nombre;
                        //    ws.Cells["A" + 4].Value = nit;
                        //    ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        //    ws.Cells["A6:I6"].Merge = true;
                        //    ws.Cells[6, 1, 6, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[6, 1, 6, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                        //    ws.Cells[6, 1, 6, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        //    ws.Cells[7, 1, 7, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        //    ws.Cells["A7:J7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        //    ws.Cells["A7:I7"].Style.Font.Bold = true;
                        //    ws.Cells[7, 1, 7, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[7, 1, 7, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        //    //fin encabezado
                        //    ws.Cells["A" + 7].Value = "CÓDIGO";
                        //    ws.Cells["B" + 7].Value = "NOMBRE";
                        //    ws.Cells["C" + 7].Value = "TERCERO";
                        //    ws.Cells["D" + 7].Value = "NOMBRE TERCERO";
                        //    ws.Cells["E" + 7].Value = "COMPROBANTE";
                        //    ws.Cells["F" + 7].Value = "FECHA";
                        //    ws.Cells["G" + 7].Value = "DÉBITO";
                        //    ws.Cells["H" + 7].Value = "CRÉDITO";
                        //    ws.Cells["I" + 7].Value = "SALDO";

                        //    var mov22 = (from m in movimiento22
                        //                 orderby m.CUENTA
                        //                 orderby m.TERCERO
                        //                 select new { m.CUENTA, m.TERCERO, m.cuentaFK, m.terceroFK }).Distinct().ToList();
                        //    i = 8;
                        //    saldo = 0;
                        //    decimal saldoTotal22 = 0;
                        //    foreach (var item in mov22)
                        //    {
                        //        var dataMov = movimiento22.Where(x => x.CUENTA == item.CUENTA && x.TERCERO == item.TERCERO).OrderBy(x => x.FECHAMOVIMIENTO).ToList();
                        //        debito = dataMov.Select(x => x.DEBITO).Sum();
                        //        credito = dataMov.Select(x => x.CREDITO).Sum();
                        //        ws.Cells["A" + i].Value = item.CUENTA;
                        //        ws.Cells["B" + i].Value = item.cuentaFK.NOMBRE;
                        //        ws.Cells["C" + i].Value = item.TERCERO;
                        //        ws.Cells["D" + i].Value = item.terceroFK.NOMBRE1 + " " + item.terceroFK.NOMBRE2 + " " + item.terceroFK.APELLIDO1 + " " + item.terceroFK.APELLIDO2; ;
                        //        string naturaleza = item.cuentaFK.NATURALEZA;
                        //        i++;
                        //        foreach (var item2 in dataMov)
                        //        {
                        //            if (naturaleza == "D")
                        //            {
                        //                saldo = item2.DEBITO - item2.CREDITO;
                        //            }
                        //            else
                        //            {
                        //                saldo = item2.CREDITO - item2.DEBITO;
                        //            }

                        //            ws.Cells["E" + i].Value = item2.TIPO + " " + item2.NUMERO;
                        //            ws.Cells["F" + i].Value = item2.FECHAMOVIMIENTO.ToString("yyyy-MM-dd");
                        //            ws.Cells["G" + i].Value = item2.DEBITO.ToString("N0", formato);
                        //            ws.Cells["H" + i].Value = item2.CREDITO.ToString("N0", formato);
                        //            ws.Cells["I" + i].Value = saldo.ToString("N0", formato);
                        //            i++;
                        //        }
                        //        if (naturaleza == "D")
                        //        {
                        //            saldoTotal22 = debito - credito;
                        //        }
                        //        else
                        //        {
                        //            saldoTotal22 = credito - debito;
                        //        }
                        //        ws.Cells["F" + i].Value = "TOTAL";
                        //        ws.Cells["G" + i].Value = debito.ToString("N0", formato);
                        //        ws.Cells["H" + i].Value = credito.ToString("N0", formato);
                        //        ws.Cells["I" + i].Value = saldoTotal22.ToString("N0", formato);
                        //        i += 2;
                        //    }
                        //    movimiento22 = null;
                        //}
                        //if (cuentaA == "0" && cuentaB != "0")
                        //{
                        //    var MovimientoTemp = (from a in movimiento22T
                        //                          select new
                        //                          {
                        //                              CUENTA = Convert.ToInt64(a.CUENTA),
                        //                              a.cuentaFK,
                        //                              a.terceroFK,
                        //                              a.FECHAMOVIMIENTO,
                        //                              a.DEBITO,
                        //                              a.CREDITO,
                        //                              a.TIPO,
                        //                              a.TERCERO,
                        //                              a.NUMERO
                        //                          }).Distinct().ToList();

                        //    var MovimientosFiltro = (from a in MovimientoTemp
                        //                             where a.CUENTA <= cuentalongmaxT
                        //                             select new
                        //                             {
                        //                                 CUENTA = Convert.ToInt64(a.CUENTA),
                        //                                 a.cuentaFK,
                        //                                 a.terceroFK,
                        //                                 a.FECHAMOVIMIENTO,
                        //                                 a.DEBITO,
                        //                                 a.CREDITO,
                        //                                 a.TIPO,
                        //                                 a.TERCERO,
                        //                                 a.NUMERO
                        //                             }).Distinct().ToList();

                        //    var movimiento22 = MovimientosFiltro;

                        //    ws = pack.Workbook.Worksheets.Add("AuxiliarPorTerceros");
                        //    // encabezado
                        //    ws.Cells["A1:I1,A2:I2,A3:I3,A4:I4,A5:I5"].Merge = true;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4"].Style.Font.Bold = true;
                        //    ws.Cells["A2:I2"].Style.Font.Name = "Arial";
                        //    ws.Cells["A2:I2"].Style.Font.Size = 14;
                        //    ws.Cells["A" + 2].Value = "AUXILIAR POR TERCEROS   " + filtro;
                        //    ws.Cells[1, 1, 5, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[1, 1, 5, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5,A7:I7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5,A7:I7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.WrapText = true;
                        //    ws.Cells["A3:I3,A4:I4"].Style.Font.Size = 12;
                        //    ws.Cells["A5:I5"].Style.Font.Size = 10;
                        //    ws.Cells["A" + 3].Value = nombre;
                        //    ws.Cells["A" + 4].Value = nit;
                        //    ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        //    ws.Cells["A6:I6"].Merge = true;
                        //    ws.Cells[6, 1, 6, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[6, 1, 6, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                        //    ws.Cells[6, 1, 6, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        //    ws.Cells[7, 1, 7, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        //    ws.Cells["A7:J7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        //    ws.Cells["A7:I7"].Style.Font.Bold = true;
                        //    ws.Cells[7, 1, 7, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[7, 1, 7, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        //    //fin encabezado
                        //    ws.Cells["A" + 7].Value = "CÓDIGO";
                        //    ws.Cells["B" + 7].Value = "NOMBRE";
                        //    ws.Cells["C" + 7].Value = "TERCERO";
                        //    ws.Cells["D" + 7].Value = "NOMBRE TERCERO";
                        //    ws.Cells["E" + 7].Value = "COMPROBANTE";
                        //    ws.Cells["F" + 7].Value = "FECHA";
                        //    ws.Cells["G" + 7].Value = "DÉBITO";
                        //    ws.Cells["H" + 7].Value = "CRÉDITO";
                        //    ws.Cells["I" + 7].Value = "SALDO";

                        //    var mov22 = (from m in movimiento22
                        //                 orderby m.CUENTA
                        //                 orderby m.TERCERO
                        //                 select new { m.CUENTA, m.TERCERO, m.cuentaFK, m.terceroFK }).Distinct().ToList();
                        //    i = 8;
                        //    saldo = 0;
                        //    decimal saldoTotal22 = 0;
                        //    foreach (var item in mov22)
                        //    {
                        //        var dataMov = movimiento22.Where(x => x.CUENTA == item.CUENTA && x.TERCERO == item.TERCERO).OrderBy(x => x.FECHAMOVIMIENTO).ToList();
                        //        debito = dataMov.Select(x => x.DEBITO).Sum();
                        //        credito = dataMov.Select(x => x.CREDITO).Sum();
                        //        ws.Cells["A" + i].Value = item.CUENTA;
                        //        ws.Cells["B" + i].Value = item.cuentaFK.NOMBRE;
                        //        ws.Cells["C" + i].Value = item.TERCERO;
                        //        ws.Cells["D" + i].Value = item.terceroFK.NOMBRE1 + " " + item.terceroFK.NOMBRE2 + " " + item.terceroFK.APELLIDO1 + " " + item.terceroFK.APELLIDO2; ;
                        //        string naturaleza = item.cuentaFK.NATURALEZA;
                        //        i++;
                        //        foreach (var item2 in dataMov)
                        //        {
                        //            if (naturaleza == "D")
                        //            {
                        //                saldo = item2.DEBITO - item2.CREDITO;
                        //            }
                        //            else
                        //            {
                        //                saldo = item2.CREDITO - item2.DEBITO;
                        //            }

                        //            ws.Cells["E" + i].Value = item2.TIPO + " " + item2.NUMERO;
                        //            ws.Cells["F" + i].Value = item2.FECHAMOVIMIENTO.ToString("yyyy-MM-dd");
                        //            ws.Cells["G" + i].Value = item2.DEBITO.ToString("N0", formato);
                        //            ws.Cells["H" + i].Value = item2.CREDITO.ToString("N0", formato);
                        //            ws.Cells["I" + i].Value = saldo.ToString("N0", formato);
                        //            i++;
                        //        }
                        //        if (naturaleza == "D")
                        //        {
                        //            saldoTotal22 = debito - credito;
                        //        }
                        //        else
                        //        {
                        //            saldoTotal22 = credito - debito;
                        //        }
                        //        ws.Cells["F" + i].Value = "TOTAL";
                        //        ws.Cells["G" + i].Value = debito.ToString("N0", formato);
                        //        ws.Cells["H" + i].Value = credito.ToString("N0", formato);
                        //        ws.Cells["I" + i].Value = saldoTotal22.ToString("N0", formato);
                        //        i += 2;
                        //    }
                        //    movimiento22 = null;
                        //}
                        //if (cuentaA != "0" && cuentaB != "0")
                        //{
                        //    var MovimientoTemp = (from a in movimiento22T
                        //                          select new
                        //                          {
                        //                              CUENTA = Convert.ToInt64(a.CUENTA),
                        //                              a.cuentaFK,
                        //                              a.terceroFK,
                        //                              a.FECHAMOVIMIENTO,
                        //                              a.DEBITO,
                        //                              a.CREDITO,
                        //                              a.TIPO,
                        //                              a.TERCERO,
                        //                              a.NUMERO
                        //                          }).Distinct().ToList();

                        //    var MovimientosFiltro = (from a in MovimientoTemp
                        //                             where a.CUENTA >= cuentalongminT && a.CUENTA <= cuentalongmaxT
                        //                             select new
                        //                             {
                        //                                 CUENTA = Convert.ToInt64(a.CUENTA),
                        //                                 a.cuentaFK,
                        //                                 a.terceroFK,
                        //                                 a.FECHAMOVIMIENTO,
                        //                                 a.DEBITO,
                        //                                 a.CREDITO,
                        //                                 a.TIPO,
                        //                                 a.TERCERO,
                        //                                 a.NUMERO
                        //                             }).Distinct().ToList();

                        //    var movimiento22 = MovimientosFiltro;

                        //    ws = pack.Workbook.Worksheets.Add("AuxiliarPorTerceros");
                        //    // encabezado
                        //    ws.Cells["A1:I1,A2:I2,A3:I3,A4:I4,A5:I5"].Merge = true;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4"].Style.Font.Bold = true;
                        //    ws.Cells["A2:I2"].Style.Font.Name = "Arial";
                        //    ws.Cells["A2:I2"].Style.Font.Size = 14;
                        //    ws.Cells["A" + 2].Value = "AUXILIAR POR TERCEROS   " + filtro;
                        //    ws.Cells[1, 1, 5, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[1, 1, 5, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5,A7:I7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5,A7:I7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        //    ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.WrapText = true;
                        //    ws.Cells["A3:I3,A4:I4"].Style.Font.Size = 12;
                        //    ws.Cells["A5:I5"].Style.Font.Size = 10;
                        //    ws.Cells["A" + 3].Value = nombre;
                        //    ws.Cells["A" + 4].Value = nit;
                        //    ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        //    ws.Cells["A6:I6"].Merge = true;
                        //    ws.Cells[6, 1, 6, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[6, 1, 6, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                        //    ws.Cells[6, 1, 6, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        //    ws.Cells[7, 1, 7, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        //    ws.Cells["A7:J7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        //    ws.Cells["A7:I7"].Style.Font.Bold = true;
                        //    ws.Cells[7, 1, 7, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        //    ws.Cells[7, 1, 7, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        //    //fin encabezado
                        //    ws.Cells["A" + 7].Value = "CÓDIGO";
                        //    ws.Cells["B" + 7].Value = "NOMBRE";
                        //    ws.Cells["C" + 7].Value = "TERCERO";
                        //    ws.Cells["D" + 7].Value = "NOMBRE TERCERO";
                        //    ws.Cells["E" + 7].Value = "COMPROBANTE";
                        //    ws.Cells["F" + 7].Value = "FECHA";
                        //    ws.Cells["G" + 7].Value = "DÉBITO";
                        //    ws.Cells["H" + 7].Value = "CRÉDITO";
                        //    ws.Cells["I" + 7].Value = "SALDO";

                        //    var mov22 = (from m in movimiento22
                        //                 orderby m.CUENTA
                        //                 orderby m.TERCERO
                        //                 select new { m.CUENTA, m.TERCERO, m.cuentaFK, m.terceroFK }).Distinct().ToList();
                        //    i = 8;
                        //    saldo = 0;
                        //    decimal saldoTotal22 = 0;
                        //    foreach (var item in mov22)
                        //    {
                        //        var dataMov = movimiento22.Where(x => x.CUENTA == item.CUENTA && x.TERCERO == item.TERCERO).OrderBy(x => x.FECHAMOVIMIENTO).ToList();
                        //        debito = dataMov.Select(x => x.DEBITO).Sum();
                        //        credito = dataMov.Select(x => x.CREDITO).Sum();
                        //        ws.Cells["A" + i].Value = item.CUENTA;
                        //        ws.Cells["B" + i].Value = item.cuentaFK.NOMBRE;
                        //        ws.Cells["C" + i].Value = item.TERCERO;
                        //        ws.Cells["D" + i].Value = item.terceroFK.NOMBRE1 + " " + item.terceroFK.NOMBRE2 + " " + item.terceroFK.APELLIDO1 + " " + item.terceroFK.APELLIDO2; ;
                        //        string naturaleza = item.cuentaFK.NATURALEZA;
                        //        i++;
                        //        foreach (var item2 in dataMov)
                        //        {
                        //            if (naturaleza == "D")
                        //            {
                        //                saldo = item2.DEBITO - item2.CREDITO;
                        //            }
                        //            else
                        //            {
                        //                saldo = item2.CREDITO - item2.DEBITO;
                        //            }

                        //            ws.Cells["E" + i].Value = item2.TIPO + " " + item2.NUMERO;
                        //            ws.Cells["F" + i].Value = item2.FECHAMOVIMIENTO.ToString("yyyy-MM-dd");
                        //            ws.Cells["G" + i].Value = item2.DEBITO.ToString("N0", formato);
                        //            ws.Cells["H" + i].Value = item2.CREDITO.ToString("N0", formato);
                        //            ws.Cells["I" + i].Value = saldo.ToString("N0", formato);
                        //            i++;
                        //        }
                        //        if (naturaleza == "D")
                        //        {
                        //            saldoTotal22 = debito - credito;
                        //        }
                        //        else
                        //        {
                        //            saldoTotal22 = credito - debito;
                        //        }
                        //        ws.Cells["F" + i].Value = "TOTAL";
                        //        ws.Cells["G" + i].Value = debito.ToString("N0", formato);
                        //        ws.Cells["H" + i].Value = credito.ToString("N0", formato);
                        //        ws.Cells["I" + i].Value = saldoTotal22.ToString("N0", formato);
                        //        i += 2;
                        //    }
                        //    movimiento22 = null;
                        //}
                        #endregion
                        break;
                    case 30:
                        #region informe30 asociados,empleados y deudores

                        var opcion = Convert.ToInt16(coll["selectPersonal"].ToString());

                        if (opcion == 1)
                            filtro = "- SOLO ASOCIADOS";
                        else if (opcion == 2)
                            filtro = "- TODOS LOS REGISTROS";

                        ws = pack.Workbook.Worksheets.Add("TERCEROS");
                        // encabezado
                        ws.Cells["A1:X1,A2:X2,A3:X3,A4:X4,A5:X5"].Merge = true;
                        ws.Cells["A2:X2,A3:X3,A4:X4,A7:X7"].Style.Font.Bold = true;
                        ws.Cells["A2:X2,A3:X3,A4:X4,A5:X5"].Style.Font.Name = "Arial";
                        ws.Cells["A2:X2"].Style.Font.Size = 14;
                        ws.Cells["A" + 2].Value = "ASOCIADOS, EMPLEADOS Y DEUDORES " + filtro;
                        ws.Cells[1, 1, 5, 24].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 5, 24].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        ws.Cells["A:X2,A3:X3,A4:X4,A5:X5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A2:X2,A3:X3,A4:X4,A7:X7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A2:X2,A3:X3,A4:X4,A7:X7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A3:X3,A4:X4"].Style.Font.Size = 12;
                        ws.Cells["A5:X5"].Style.Font.Size = 10;
                        ws.Cells["A" + 3].Value = nombre;
                        ws.Cells["A" + 4].Value = nit;
                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        ws.Cells["A6:X6"].Merge = true;
                        ws.Cells[6, 1, 6, 24].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[6, 1, 6, 24].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                        ws.Cells[6, 1, 6, 24].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        ws.Cells[7, 1, 7, 24].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        ws.Cells["A7:Y7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        ws.Cells[7, 1, 7, 24].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[7, 1, 7, 24].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        ws.Cells["A7:X7"].Style.WrapText = true;
                        //fin encabezado
                        ws.Cells["A" + 7].Value = "Tipo Documento";
                        ws.Cells["B" + 7].Value = "Documento";
                        ws.Cells["C" + 7].Value = "DV";
                        ws.Cells["D" + 7].Value = "Fecha Exp ID";
                        ws.Cells["E" + 7].Value = "Lugar Exp. ID";
                        ws.Cells["F" + 7].Value = "PrimerA";
                        ws.Cells["G" + 7].Value = "SegundoA";
                        ws.Cells["H" + 7].Value = "PrimerN";
                        ws.Cells["I" + 7].Value = "SegundoN";
                        ws.Cells["J" + 7].Value = "Nombre";
                        ws.Cells["K" + 7].Value = "Teléfono";
                        ws.Cells["L" + 7].Value = "Dirección";
                        ws.Cells["M" + 7].Value = "Email";
                        ws.Cells["N" + 7].Value = "Sexo";
                        ws.Cells["O" + 7].Value = "Fecha Nac";
                        ws.Cells["P" + 7].Value = "Estado Civil";
                        ws.Cells["Q" + 7].Value = "Profesión";
                        ws.Cells["R" + 7].Value = "Tel Movil";
                        ws.Cells["S" + 7].Value = "Municipio";
                        ws.Cells["T" + 7].Value = "Barrio";
                        ws.Cells["U" + 7].Value = "Fecha afiliación";
                        ws.Cells["V" + 7].Value = "Agencia";
                        ws.Cells["W" + 7].Value = "Valor Cuota";
                        ws.Cells["X" + 7].Value = "Total Aportes";
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
                            i = 8;


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
                            data = null;
                        }
                        else
                        {
                            var data = db.FichasAportes.ToList();

                            i = 8;


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
                            data = null;
                        }

                        #endregion
                        break;
                    case 36:
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
                                filtro = fd.ToShortDateString() + " - " + fh.ToShortDateString();
                            }
                            else if (fechaDesde != "" && fechaHasta == "")
                            {
                                DateTime fd = Convert.ToDateTime(fechaDesde);
                                DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                                creditos = db.HistorialCreditos.Where(x => x.fecha >= fechDesde).ToList();
                                filtro = fd.ToShortDateString();
                            }
                            else if (fechaDesde == "" && fechaHasta != "")
                            {
                                DateTime fh = Convert.ToDateTime(fechaHasta);
                                DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                                creditos = db.HistorialCreditos.Where(x => x.fecha <= fechHasta).ToList();
                                filtro = fh.ToShortDateString();
                            }


                            ws = pack.Workbook.Worksheets.Add("creditos");

                            // encabezado
                            ws.Cells["A1:X1,A2:X2,A3:X3,A4:X4,A5:X5"].Merge = true;
                            ws.Cells["A2:X2,A3:X3,A4:X4"].Style.Font.Bold = true;
                            ws.Cells["A2:X2"].Style.Font.Name = "Arial";
                            ws.Cells["A2:X2"].Style.Font.Size = 14;
                            ws.Cells["A" + 2].Value = "CRÉDITOS   " + filtro;
                            ws.Cells[1, 1, 5, 24].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[1, 1, 5, 24].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                            ws.Cells["A2:X2,A3:X3,A4:X4,A5:X5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            ws.Cells["A2:X2,A3:X3,A4:X4,A7:X7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells["A2:X2,A3:X3,A4:X4,A7:X7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            // ws.Cells["A2:X2,A3:X3,A4:X4,A5:X5"].Style.WrapText = true;
                            ws.Cells["A3:X3,A4:X4"].Style.Font.Size = 12;
                            ws.Cells["A5:X5"].Style.Font.Size = 10;
                            ws.Cells["A" + 3].Value = nombre;
                            ws.Cells["A" + 4].Value = nit;
                            ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                            ws.Cells["A6:X6"].Merge = true;
                            ws.Cells[6, 1, 6, 24].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[6, 1, 6, 24].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            ws.Cells[6, 1, 6, 24].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                            ws.Cells[7, 1, 7, 24].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                            ws.Cells["A7:Y7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                            ws.Cells["A7:X7"].Style.Font.Bold = true;
                            ws.Cells[7, 1, 7, 24].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[7, 1, 7, 24].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                            //fin encabezado

                            ws.Cells["A7:X7"].Style.WrapText = true;
                            ws.Cells["A" + 7].Value = "Fecha De Desembolso";
                            ws.Cells["B" + 7].Value = "Nit";
                            ws.Cells["C" + 7].Value = "Nombres y Apellidos";
                            ws.Cells["D" + 7].Value = "Pagaré";
                            ws.Cells["E" + 7].Value = "Taza Interés Corriente";
                            ws.Cells["F" + 7].Value = "Taza Interés Mora";
                            ws.Cells["G" + 7].Value = "Plazo";
                            ws.Cells["H" + 7].Value = "Valor Costo Por Cuota";
                            ws.Cells["I" + 7].Value = "Capital Inicial";
                            ws.Cells["J" + 7].Value = "Cuotas Pagadas";
                            ws.Cells["K" + 7].Value = "Cuotas Pendientes";
                            ws.Cells["L" + 7].Value = "Dia de Pago";
                            ws.Cells["M" + 7].Value = "Valor Cuota";
                            ws.Cells["N" + 7].Value = "Estado Actual";
                            ws.Cells["O" + 7].Value = "Abono Capital";
                            ws.Cells["P" + 7].Value = "Abono Interés Corriente";
                            ws.Cells["Q" + 7].Value = "Abono Interés Mora";
                            ws.Cells["R" + 7].Value = "Abono Costos Adicionales";
                            ws.Cells["S" + 7].Value = "Saldo Capital";
                            ws.Cells["T" + 7].Value = "Capital en Mora";
                            ws.Cells["U" + 7].Value = "Interés Corriente en Mora";
                            ws.Cells["V" + 7].Value = "Días en Mora";
                            ws.Cells["W" + 7].Value = "Interés Corriente Pendiente";
                            ws.Cells["X" + 7].Value = "Interés de Mora Pendiente";

                            //totales
                            decimal TabonoCapital = 0, TabonoInteresCorriente = 0, TabonoInteresMora = 0, TabonoCostosAdicionales = 0, TsaldoCapital = 0, TcapitalMora = 0, TinteresCorrienteMora = 0, TinteresCorrintePendiente = 0, TinteresMoraPendiente = 0;
                            long TcapitalInicial = 0;
                            //
                            j = 8;

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
                                filtro = fd.ToShortDateString() + " - " + fh.ToShortDateString();
                            }
                            else if (fechaDesde != "" && fechaHasta == "")
                            {
                                DateTime fd = Convert.ToDateTime(fechaDesde);
                                DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                                creditos = db.Prestamos.Where(x => x.fechadesembolso >= fechDesde).ToList();
                                filtro = fd.ToShortDateString();
                            }
                            else if (fechaDesde == "" && fechaHasta != "")
                            {
                                DateTime fh = Convert.ToDateTime(fechaHasta);
                                DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                                creditos = db.Prestamos.Where(x => x.fechadesembolso <= fechHasta).ToList();
                                filtro = fh.ToShortDateString();
                            }



                            ws = pack.Workbook.Worksheets.Add("creditos");
                            // encabezado
                            ws.Cells["A1:X1,A2:X2,A3:X3,A4:X4,A5:X5"].Merge = true;
                            ws.Cells["A2:X2,A3:X3,A4:X4"].Style.Font.Bold = true;
                            ws.Cells["A2:X2"].Style.Font.Name = "Arial";
                            ws.Cells["A2:X2"].Style.Font.Size = 14;
                            ws.Cells["A" + 2].Value = "CRÉDITOS   " + filtro;
                            ws.Cells[1, 1, 5, 24].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[1, 1, 5, 24].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                            ws.Cells["A2:X2,A3:X3,A4:X4,A5:X5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            ws.Cells["A2:X2,A3:X3,A4:X4,A7:X7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells["A2:X2,A3:X3,A4:X4,A7:X7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells["A3:X3,A4:X4"].Style.Font.Size = 12;
                            ws.Cells["A5:X5"].Style.Font.Size = 10;
                            ws.Cells["A" + 3].Value = nombre;
                            ws.Cells["A" + 4].Value = nit;
                            ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                            ws.Cells["A6:X6"].Merge = true;
                            ws.Cells[6, 1, 6, 24].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[6, 1, 6, 24].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.White);
                            ws.Cells[6, 1, 6, 24].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                            ws.Cells[7, 1, 7, 24].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                            ws.Cells["A7:Y7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                            ws.Cells["A7:X7"].Style.Font.Bold = true;
                            ws.Cells[7, 1, 7, 24].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[7, 1, 7, 24].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                            //fin encabezado

                            ws.Cells["A7:X7"].Style.WrapText = true;
                            ws.Cells["A" + 7].Value = "Fecha De Desembolso";
                            ws.Cells["B" + 7].Value = "Nit";
                            ws.Cells["C" + 7].Value = "Nombres y Apellidos";
                            ws.Cells["D" + 7].Value = "Pagaré";
                            ws.Cells["E" + 7].Value = "Taza Interés Corriente";
                            ws.Cells["F" + 7].Value = "Taza Interés Mora";
                            ws.Cells["G" + 7].Value = "Plazo";
                            ws.Cells["H" + 7].Value = "Valor Costo Por Cuota";
                            ws.Cells["I" + 7].Value = "Capital Inicial";
                            ws.Cells["J" + 7].Value = "Cuotas Pagadas";
                            ws.Cells["K" + 7].Value = "Cuotas Pendientes";
                            ws.Cells["L" + 7].Value = "Dia de Pago";
                            ws.Cells["M" + 7].Value = "Valor Cuota";
                            ws.Cells["N" + 7].Value = "Estado Actual";
                            ws.Cells["O" + 7].Value = "Abono Capital";
                            ws.Cells["P" + 7].Value = "Abono Interés Corriente";
                            ws.Cells["Q" + 7].Value = "Abono Interés Mora";
                            ws.Cells["R" + 7].Value = "Abono Costos Adicionales";
                            ws.Cells["S" + 7].Value = "Saldo Capital";
                            ws.Cells["T" + 7].Value = "Capital en Mora";
                            ws.Cells["U" + 7].Value = "Interés Corriente en Mora";
                            ws.Cells["V" + 7].Value = "Días en Mora";
                            ws.Cells["W" + 7].Value = "Interés Corriente Pendiente";
                            ws.Cells["X" + 7].Value = "Interés de Mora Pendiente";

                            //totales
                            decimal TabonoCapital = 0, TabonoInteresCorriente = 0, TabonoInteresMora = 0, TabonoCostosAdicionales = 0, TsaldoCapital = 0, TcapitalMora = 0, TinteresCorrienteMora = 0, TinteresCorrintePendiente = 0, TinteresMoraPendiente = 0;
                            long TcapitalInicial = 0;
                            //

                            j = 8;

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

                    case 37:
                        #region NUEVO BALANCE COMPROBACION 
                        var chkTercero = coll["chkTercero"];
                        var nivel = coll["nivel"];
                        costo = coll["costo"];
                        var fhD = DateTime.Now;
                        var fhH = DateTime.Now;
                        var opcionPro = 0;
                        decimal saldoIni = 0, debitoAct = 0, creditoAct = 0;
                        debitoAnterior = 0; creditoAnterior = 0;
                        saldo = 0;

                        List<BalanceComprobacionActualizado> datoMovimiento = new List<BalanceComprobacionActualizado>();
                        #region Validaciones tipos de busqueda
                        if (fechaDesdeBC != "" && fechaHastaBC != "" && costo != "" && chkTercero == "on" && nivel != "")
                        {
                            fhD = Convert.ToDateTime(fechaDesdeBC);
                            fhH = Convert.ToDateTime(fechaHastaBC);
                            filtro = fhD.ToShortDateString() + " - " + fhH.ToShortDateString();
                            opcionPro = 1;

                        }
                        else if (fechaDesdeBC != "" && fechaHastaBC != "" && costo != "" && nivel != "")
                        {
                            fhD = Convert.ToDateTime(fechaDesdeBC);
                            fhH = Convert.ToDateTime(fechaHastaBC);
                            filtro = fhD.ToShortDateString() + " - " + fhH.ToShortDateString();
                            opcionPro = 2;
                        }
                        else if (fechaDesdeBC != "" && fechaHastaBC != "" && chkTercero == "on" && nivel != "")
                        {
                            fhD = Convert.ToDateTime(fechaDesdeBC);
                            fhH = Convert.ToDateTime(fechaHastaBC);
                            filtro = fhD.ToShortDateString() + " - " + fhH.ToShortDateString();
                            opcionPro = 3;

                        }
                        else if (fechaDesdeBC != "" && fechaHastaBC != "" && nivel != "")
                        {
                            fhD = Convert.ToDateTime(fechaDesdeBC);
                            fhH = Convert.ToDateTime(fechaHastaBC);
                            filtro = fhD.ToShortDateString() + " - " + fhH.ToShortDateString();
                            opcionPro = 4;
                        }

                        DateTime fechDes = new DateTime(fhD.Year, fhD.Month, fhD.Day, 0, 0, 0);
                        DateTime fechHas = new DateTime(fhH.Year, fhH.Month, fhH.Day, 23, 59, 59);
                        DateTime fechDesAux = new DateTime(fechDes.Year, 1, 1, 0, 0, 0);

                        #endregion

                        datoMovimiento = db.Database.SqlQuery<BalanceComprobacionActualizado>(
                             "dbo.sp_BalanceComprobacion2 @ccosto, @fechaDesde, @fechaDesdeAux,@fechaHasta, @opcion",
                             new SqlParameter("@ccosto", costo),
                             new SqlParameter("@fechaDesde", fechDes),
                             new SqlParameter("@fechaDesdeAux", fechDesAux),
                             new SqlParameter("@fechaHasta", fechHas),
                             new SqlParameter("@opcion", opcionPro)
                             ).ToList();

                        ws = pack.Workbook.Worksheets.Add("Balance De Comprobación");
                        #region ENCABEZADO EXCEL
                        // encabezado
                        ws.Cells["A1:H1,A2:H2,A3:H3,A4:H4,A5:H5"].Merge = true;
                        ws.Cells["A2:H2,A3:H3,A4:H4"].Style.Font.Bold = true;
                        ws.Cells["A2:H2"].Style.Font.Name = "Arial";
                        ws.Cells["A2:H2"].Style.Font.Size = 14;
                        ws.Cells["A" + 2].Value = "BALANCE DE COMPROBACIÓN   " + filtro;
                        ws.Cells[1, 1, 5, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 5, 8].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        ws.Cells["A2:H2,A3:H3,A4:H4,A5:H5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A2:H2,A3:H3,A4:H4,A7:H7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A2:H2,A3:H3,A4:H4,A7:H7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A2:H2,A3:H3,A4:H4,A5:H5"].Style.WrapText = true;
                        ws.Cells["A3:H3,A4:H4"].Style.Font.Size = 12;
                        ws.Cells["A5:H5"].Style.Font.Size = 10;
                        ws.Cells["A" + 3].Value = nombre;
                        ws.Cells["A" + 4].Value = nit;
                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        ws.Cells["A6:H6"].Merge = true;
                        //fin encabezado
                        #endregion

                        if (opcionPro == 2 || opcionPro == 4)
                        {
                            #region  Informacion SOLO CUENTAS sin o con  CENTRO DE COSTO
                            ws.Cells[6, 1, 6, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                            ws.Cells[7, 1, 7, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                            ws.Cells["A7:G7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                            ws.Cells["A7:H7"].Style.Font.Bold = true;
                            ws.Cells[7, 1, 7, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[7, 1, 7, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                            ws.Cells["A" + 7].Value = "CUENTA";
                            ws.Cells["B" + 7].Value = "NOMBRE CUENTA";
                            ws.Cells["C" + 7].Value = "SALDO INICIAL";
                            ws.Cells["D" + 7].Value = "DÉBITO";
                            ws.Cells["E" + 7].Value = "CRÉDITO";
                            ws.Cells["F" + 7].Value = "SALDO";

                            j = 8;
                            var cuentaPorNivel = new PlanCuentasBLL().GetCuentasPorNiveles(nivel);

                            if (cuentaPorNivel.Count() > 0 && datoMovimiento.Count() > 0)
                            {
                                foreach (var item in cuentaPorNivel)
                                {
                                    var hayCuentas = datoMovimiento.Where(x => x.CUENTA.StartsWith(item.CODIGO)).Count();

                                    if (hayCuentas > 0)
                                    {
                                        if (item.CODIGO.StartsWith("4") || item.CODIGO.StartsWith("5") || item.CODIGO.StartsWith("6") || item.CODIGO.StartsWith("7"))
                                        {
                                            debitoAnterior = datoMovimiento.Where(x => x.CUENTA.StartsWith(item.CODIGO)).Select(x => x.DebitoAntAux).Sum();
                                            creditoAnterior = datoMovimiento.Where(x => x.CUENTA.StartsWith(item.CODIGO)).Select(x => x.CreditoAntAux).Sum();
                                        }
                                        else
                                        {
                                            debitoAnterior = datoMovimiento.Where(x => x.CUENTA.StartsWith(item.CODIGO)).Select(x => x.DebitoAnterior).Sum();
                                            creditoAnterior = datoMovimiento.Where(x => x.CUENTA.StartsWith(item.CODIGO)).Select(x => x.CreditoAnterior).Sum();
                                        }

                                        debitoAct = datoMovimiento.Where(x => x.CUENTA.StartsWith(item.CODIGO)).Select(x => x.DebitoActual).Sum();
                                        creditoAct = datoMovimiento.Where(x => x.CUENTA.StartsWith(item.CODIGO)).Select(x => x.CreditoActual).Sum();

                                        if (item.NATURALEZA == "D")
                                        {
                                            saldoIni = debitoAnterior - creditoAnterior;
                                            saldo = (debitoAct - creditoAct) + saldoIni;
                                        }
                                        else
                                        {
                                            saldoIni = creditoAnterior - debitoAnterior;
                                            saldo = (creditoAct - debitoAct) + saldoIni;
                                        }
                                        ws.Cells["A" + j].Value = item.CODIGO;
                                        ws.Cells["B" + j].Value = item.NOMBRE;
                                        ws.Cells["C" + j].Value = saldoIni.ToString("N0", formato);
                                        ws.Cells["D" + j].Value = debitoAct.ToString("N0", formato);
                                        ws.Cells["E" + j].Value = creditoAct.ToString("N0", formato);
                                        ws.Cells["F" + j].Value = saldo.ToString("N0", formato);
                                        j++;

                                    }
                                }
                            }
                            #endregion
                        }
                        else if (opcionPro == 1 || opcionPro == 3)
                        {
                            #region Informacion con datos de TERCEROS sin o con CENTRO DE COSTOS
                            ws.Cells[6, 1, 6, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                            ws.Cells[7, 1, 7, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                            ws.Cells["A7:I7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                            ws.Cells["A7:H7"].Style.Font.Bold = true;
                            ws.Cells[7, 1, 7, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[7, 1, 7, 8].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                            ws.Cells["A" + 7].Value = "CUENTA";
                            ws.Cells["B" + 7].Value = "NOMBRE CUENTA";
                            ws.Cells["C" + 7].Value = "DOCUMENTO TERCERO";
                            ws.Cells["D" + 7].Value = "NOMBRE TERCERO";
                            ws.Cells["E" + 7].Value = "SALDO INICIAL";
                            ws.Cells["F" + 7].Value = "DÉBITO";
                            ws.Cells["G" + 7].Value = "CRÉDITO";
                            ws.Cells["H" + 7].Value = "SALDO";

                            j = 8;
                            var cuentaPorNivel = new PlanCuentasBLL().GetCuentasPorNiveles(nivel);

                            if (cuentaPorNivel.Count() > 0 && datoMovimiento.Count() > 0)
                            {
                                foreach (var item in cuentaPorNivel)
                                {
                                    var hayCuentas = datoMovimiento.Where(x => x.CUENTA.StartsWith(item.CODIGO)).Count();

                                    if (hayCuentas > 0)
                                    {
                                        #region validación para cuentas menores a 9 digitos

                                        if (item.CODIGO.Length != 9)
                                        {
                                            if (item.CODIGO.StartsWith("4") || item.CODIGO.StartsWith("5") || item.CODIGO.StartsWith("6") || item.CODIGO.StartsWith("7"))
                                            {
                                                debitoAnterior = datoMovimiento.Where(x => x.CUENTA.StartsWith(item.CODIGO)).Select(x => x.DebitoAntAux).Sum();
                                                creditoAnterior = datoMovimiento.Where(x => x.CUENTA.StartsWith(item.CODIGO)).Select(x => x.CreditoAntAux).Sum();
                                            }
                                            else
                                            {
                                                debitoAnterior = datoMovimiento.Where(x => x.CUENTA.StartsWith(item.CODIGO)).Select(x => x.DebitoAnterior).Sum();
                                                creditoAnterior = datoMovimiento.Where(x => x.CUENTA.StartsWith(item.CODIGO)).Select(x => x.CreditoAnterior).Sum();
                                            }

                                            debitoAct = datoMovimiento.Where(x => x.CUENTA.StartsWith(item.CODIGO)).Select(x => x.DebitoActual).Sum();
                                            creditoAct = datoMovimiento.Where(x => x.CUENTA.StartsWith(item.CODIGO)).Select(x => x.CreditoActual).Sum();

                                            if (item.NATURALEZA == "D")
                                            {
                                                saldoIni = debitoAnterior - creditoAnterior;
                                                saldo = (debitoAct - creditoAct) + saldoIni;
                                            }
                                            else
                                            {
                                                saldoIni = creditoAnterior - debitoAnterior;
                                                saldo = (creditoAct - debitoAct) + saldoIni;
                                            }

                                            ws.Cells["A" + j].Value = item.CODIGO;
                                            ws.Cells["B" + j].Value = item.NOMBRE;
                                            ws.Cells["E" + j].Value = saldoIni.ToString("N0", formato);
                                            ws.Cells["F" + j].Value = debitoAct.ToString("N0", formato);
                                            ws.Cells["G" + j].Value = creditoAct.ToString("N0", formato);
                                            ws.Cells["H" + j].Value = saldo.ToString("N0", formato);
                                            j++;

                                        }
                                        #endregion
                                        #region Validacion cuando la cuenta es igual a 9 digitos
                                        else
                                        {
                                            var info = datoMovimiento.Where(x => x.CUENTA == item.CODIGO).OrderBy(x => x.CUENTA).Distinct().ToList();

                                            foreach (var item2 in info)
                                            {

                                                if (item.CODIGO.StartsWith("4") || item.CODIGO.StartsWith("5") || item.CODIGO.StartsWith("6") || item.CODIGO.StartsWith("7"))
                                                {
                                                    debitoAct = datoMovimiento.Where(x => x.CUENTA == item.CODIGO && x.TERCERO == item2.TERCERO).Select(x => x.DebitoActual).FirstOrDefault();
                                                    creditoAct = datoMovimiento.Where(x => x.CUENTA == item.CODIGO && x.TERCERO == item2.TERCERO).Select(x => x.CreditoActual).FirstOrDefault();
                                                    debitoAnterior = datoMovimiento.Where(x => x.CUENTA == item.CODIGO && x.TERCERO == item2.TERCERO).Select(x => x.DebitoAntAux).FirstOrDefault();
                                                    creditoAnterior = datoMovimiento.Where(x => x.CUENTA == item.CODIGO && x.TERCERO == item2.TERCERO).Select(x => x.CreditoAntAux).FirstOrDefault();

                                                    if (item.NATURALEZA == "D")
                                                    {
                                                        saldoIni = debitoAnterior - creditoAnterior;
                                                        saldo = (debitoAct - creditoAct) + saldoIni;
                                                    }
                                                    else
                                                    {
                                                        saldoIni = creditoAnterior - debitoAnterior;
                                                        saldo = (creditoAct - debitoAct) + saldoIni;
                                                    }

                                                    ws.Cells["A" + j].Value = item.CODIGO;
                                                    ws.Cells["B" + j].Value = item.NOMBRE;
                                                    ws.Cells["C" + j].Value = item2.TERCERO;
                                                    ws.Cells["D" + j].Value = item2.NOMBRE;
                                                    ws.Cells["E" + j].Value = saldoIni.ToString("N0", formato);
                                                    ws.Cells["F" + j].Value = debitoAct.ToString("N0", formato);
                                                    ws.Cells["G" + j].Value = creditoAct.ToString("N0", formato);
                                                    ws.Cells["H" + j].Value = saldo.ToString("N0", formato);
                                                    j++;

                                                }
                                                else
                                                {
                                                    debitoAct = datoMovimiento.Where(x => x.CUENTA == item.CODIGO && x.TERCERO == item2.TERCERO).Select(x => x.DebitoActual).FirstOrDefault();
                                                    creditoAct = datoMovimiento.Where(x => x.CUENTA == item.CODIGO && x.TERCERO == item2.TERCERO).Select(x => x.CreditoActual).FirstOrDefault();
                                                    debitoAnterior = datoMovimiento.Where(x => x.CUENTA == item.CODIGO && x.TERCERO == item2.TERCERO).Select(x => x.DebitoAnterior).FirstOrDefault();
                                                    creditoAnterior = datoMovimiento.Where(x => x.CUENTA == item.CODIGO && x.TERCERO == item2.TERCERO).Select(x => x.CreditoAnterior).FirstOrDefault();

                                                    if (item.NATURALEZA == "D")
                                                    {
                                                        saldoIni = debitoAnterior - creditoAnterior;
                                                        saldo = (debitoAct - creditoAct) + saldoIni;
                                                    }
                                                    else
                                                    {
                                                        saldoIni = creditoAnterior - debitoAnterior;
                                                        saldo = (creditoAct - debitoAct) + saldoIni;
                                                    }

                                                    ws.Cells["A" + j].Value = item.CODIGO;
                                                    ws.Cells["B" + j].Value = item.NOMBRE;
                                                    ws.Cells["C" + j].Value = item2.TERCERO;
                                                    ws.Cells["D" + j].Value = item2.NOMBRE;
                                                    ws.Cells["E" + j].Value = saldoIni.ToString("N0", formato);
                                                    ws.Cells["F" + j].Value = debitoAct.ToString("N0", formato);
                                                    ws.Cells["G" + j].Value = creditoAct.ToString("N0", formato);
                                                    ws.Cells["H" + j].Value = saldo.ToString("N0", formato);
                                                    j++;
                                                }
                                            }
                                        }//fin else != 9
                                    }
                                    #endregion
                                }//fin for
                            }
                        }
                        #endregion
                        ws.Cells[ws.Dimension.Address].AutoFitColumns();//siempre al final de todo. le da tamaño ajustado a cada columna

                        #endregion
                        break;

                    case 38:
                        #region informe37 Balance de Comprobacion VERIFICAR DATOS DE ESTE REPORTE SE USAN EN CATALOGO DE CUENTAS

                        chkTercero = coll["chkTercero"];
                        nivel = coll["nivel"];
                        costo = coll["costo"];

                        if (fechaDesde != "" && fechaHasta != "")
                        {
                            DateTime df = Convert.ToDateTime(fechaDesde);
                            DateTime dh = Convert.ToDateTime(fechaHasta);
                            filtro = df.ToShortDateString() + " - " + dh.ToShortDateString();
                        }
                        else if (fechaDesde != "" && fechaHasta == "")
                        {
                            DateTime df = Convert.ToDateTime(fechaDesde);
                            filtro = df.ToShortDateString();
                        }
                        else if (fechaDesde == "" && fechaHasta != "")
                        {
                            DateTime dh = Convert.ToDateTime(fechaHasta);
                            filtro = dh.ToShortDateString();
                        }

                        ws = pack.Workbook.Worksheets.Add("Balance De Comprobación");
                        if (chkTercero == "on")
                        {
                            // encabezado
                            ws.Cells["A1:H1,A2:H2,A3:H3,A4:H4,A5:H5"].Merge = true;
                            ws.Cells["A2:H2,A3:H3,A4:H4"].Style.Font.Bold = true;
                            ws.Cells["A2:H2"].Style.Font.Name = "Arial";
                            ws.Cells["A2:H2"].Style.Font.Size = 14;
                            ws.Cells["A" + 2].Value = "BALANCE DE COMPROBACIÓN   " + filtro;
                            ws.Cells[1, 1, 5, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[1, 1, 5, 8].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                            ws.Cells["A2:H2,A3:H3,A4:H4,A5:H5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            ws.Cells["A2:H2,A3:H3,A4:H4,A7:H7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells["A2:H2,A3:H3,A4:H4,A7:H7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells["A2:H2,A3:H3,A4:H4,A5:H5"].Style.WrapText = true;
                            ws.Cells["A3:H3,A4:H4"].Style.Font.Size = 12;
                            ws.Cells["A5:H5"].Style.Font.Size = 10;
                            ws.Cells["A" + 3].Value = nombre;
                            ws.Cells["A" + 4].Value = nit;
                            ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                            ws.Cells["A6:H6"].Merge = true;
                            ws.Cells[6, 1, 6, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                            ws.Cells[7, 1, 7, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                            ws.Cells["A7:I7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                            ws.Cells["A7:H7"].Style.Font.Bold = true;
                            ws.Cells[7, 1, 7, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[7, 1, 7, 8].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                            //fin encabezado

                            ws.Cells["A" + 7].Value = "CUENTA";
                            ws.Cells["B" + 7].Value = "NOMBRE CUENTA";
                            ws.Cells["C" + 7].Value = "DOCUMENTO TERCERO";
                            ws.Cells["D" + 7].Value = "NOMBRE TERCERO";
                            ws.Cells["E" + 7].Value = "SALDO INICIAL";
                            ws.Cells["F" + 7].Value = "DÉBITO";
                            ws.Cells["G" + 7].Value = "CRÉDITO";
                            ws.Cells["H" + 7].Value = "SALDO";
                        }
                        else
                        {
                            // encabezado
                            ws.Cells["A1:F1,A2:F2,A3:F3,A4:F4,A5:F5"].Merge = true;
                            ws.Cells["A2:F2,A3:F3,A4:F4"].Style.Font.Bold = true;
                            ws.Cells["A2:F2"].Style.Font.Name = "Arial";
                            ws.Cells["A2:F2"].Style.Font.Size = 14;
                            ws.Cells["A" + 2].Value = "BALANCE DE COMPROBACIÓN   " + filtro;
                            ws.Cells[1, 1, 5, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[1, 1, 5, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                            ws.Cells["A2:F2,A3:F3,A4:F4,A5:F5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                            ws.Cells["A2:F2,A3:F3,A4:F4,A7:F7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            ws.Cells["A2:F2,A3:F3,A4:F4,A7:F7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells["A2:F2,A3:F3,A4:F4,A5:F5"].Style.WrapText = true;
                            ws.Cells["A3:F3,A4:H4"].Style.Font.Size = 12;
                            ws.Cells["A5:F5"].Style.Font.Size = 10;
                            ws.Cells["A" + 3].Value = nombre;
                            ws.Cells["A" + 4].Value = nit;
                            ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                            ws.Cells["A6:H6"].Merge = true;
                            ws.Cells[6, 1, 6, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                            ws.Cells[7, 1, 7, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                            ws.Cells["A7:G7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                            ws.Cells["A7:H7"].Style.Font.Bold = true;
                            ws.Cells[7, 1, 7, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[7, 1, 7, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                            //fin encabezado

                            ws.Cells["A" + 7].Value = "CUENTA";
                            ws.Cells["B" + 7].Value = "NOMBRE CUENTA";
                            ws.Cells["C" + 7].Value = "SALDO INICIAL";
                            ws.Cells["D" + 7].Value = "DÉBITO";
                            ws.Cells["E" + 7].Value = "CRÉDITO";
                            ws.Cells["F" + 7].Value = "SALDO";
                        }

                        j = 8;
                        List<MovimientoAuxiliar> movtosSaldos = new List<MovimientoAuxiliar>();
                        List<MovimientoAuxiliar> movtosActuales = new List<MovimientoAuxiliar>();
                        List<MovimientoAuxiliar> movtosAuxiliarActual = new List<MovimientoAuxiliar>();//sirve para cuando se escoge con terceros
                        List<CuentaMayor> auxiliar = new List<CuentaMayor>();


                        if (fechaDesde != "" && fechaHasta != "")
                        {
                            DateTime fh = Convert.ToDateTime(fechaHasta);
                            DateTime fd = Convert.ToDateTime(fechaDesde);
                            DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                            DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);

                            movtosActuales = db.Database.SqlQuery<MovimientoAuxiliar>(
                                "dbo.sp_BalanceComprobacion @fecha",
                                new SqlParameter("@fecha", fechHasta)
                                ).ToList();
                            //movtosActuales = db.Movimientos.Where(x => x.FECHAMOVIMIENTO <= fechHasta && x.Comprobante.ANULADO == false).ToList(); 267621 267545 
                            if (costo != "")
                            {
                                movtosActuales = movtosActuales.Where(x => x.CCOSTO == costo).ToList();
                            }
                            movtosAuxiliarActual = movtosActuales;
                            movtosSaldos = movtosActuales.Where(x => x.FECHAMOVIMIENTO < fechDesde).ToList();
                            movtosActuales = movtosActuales.Where(x => x.FECHAMOVIMIENTO >= fechDesde).ToList();

                        }
                        else if (fechaDesde != "")
                        {

                            DateTime fd = Convert.ToDateTime(fechaDesde);
                            DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                            movtosActuales = db.Database.SqlQuery<MovimientoAuxiliar>(
                                "dbo.sp_BalanceComprobacion @fecha",
                                new SqlParameter("@fecha", fechDesde)
                                ).ToList();
                            //movtosActuales = db.Movimientos.Where(x => x.FECHAMOVIMIENTO == fechDesde && x.Comprobante.ANULADO == false).ToList();
                            if (costo != "")
                            {
                                movtosActuales = movtosActuales.Where(x => x.CCOSTO == costo).ToList();
                            }
                            movtosAuxiliarActual = movtosActuales;
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
                        DateTime fdAuxilar = Convert.ToDateTime(fechaDesde);
                        DateTime fDesdeAuxiliar = new DateTime(fdAuxilar.Year, 1, 1, 0, 0, 0);


                        if (auxiliar.Count > 0)
                        {

                            if (chkTercero != "on")
                            {
                                decimal saldoInicial = 0, debitoActual = 0, creditoActual = 0;
                                debitoAnterior = 0; creditoAnterior = 0;
                                saldo = 0;
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
                                decimal saldoInicial = 0, debitoActual = 0, creditoActual = 0;
                                debitoAnterior = 0; creditoAnterior = 0;
                                saldo = 0;
                                foreach (var item2 in auxiliar)
                                {

                                    var dataActual = movtosActuales.Where(x => x.CUENTA.StartsWith(item2.CODIGO)).ToList();
                                    var dataAnterior = movtosSaldos.Where(x => x.CUENTA.StartsWith(item2.CODIGO)).ToList();
                                    var dataAnteriorAuxiliar = dataAnterior.Where(x => x.FECHAMOVIMIENTO >= fDesdeAuxiliar).ToList();


                                    if (dataActual.Count != 0 || dataAnterior.Count != 0)
                                    {
                                        //ws.Cells["B" + j].Value = item2.CODIGO;
                                        //ws.Cells["C" + j].Value = item2.NOMBRE;
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


                                            //ws.Cells["G" + j].Value = saldoInicial.ToString("N0", formato);
                                            //ws.Cells["H" + j].Value = debitoActual.ToString("N0", formato);
                                            //ws.Cells["I" + j].Value = creditoActual.ToString("N0", formato);
                                            //ws.Cells["J" + j].Value = saldo.ToString("N0", formato);

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
                                        }//fin if != 9
                                        else
                                        {
                                            if (dataAnterior.Count == 0 || item2.CODIGO.StartsWith("4") || item2.CODIGO.StartsWith("5") || item2.CODIGO.StartsWith("6") || item2.CODIGO.StartsWith("7"))
                                            {
                                                var info = (from da in movtosAuxiliarActual
                                                            select new { da.TERCERO, da.CUENTA, da.NOMBRE }
                                                            ).Where(x => x.CUENTA == item2.CODIGO).OrderBy(x => x.CUENTA).Distinct();

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


                                                    var objeto = new spBalanceComprobacionL5()
                                                    {
                                                        codigo = item2.CODIGO,
                                                        nombre = item2.NOMBRE,
                                                        DocumentoTercero = item3.TERCERO,
                                                        NombreTercero = item3.NOMBRE,
                                                        SaldoInicial = saldoInicial.ToString("N0", formato),
                                                        Debito = debitoActual.ToString("N0", formato),
                                                        Credito = creditoActual.ToString("N0", formato),
                                                        Saldo = saldo.ToString("N0", formato)
                                                    };
                                                    slPC.Add(objeto);

                                                    //j++;

                                                }

                                            }
                                            else
                                            {
                                                var info = (from da in movtosAuxiliarActual
                                                            select new { da.TERCERO, da.CUENTA, da.NOMBRE }
                                                            ).Where(x => x.CUENTA == item2.CODIGO).OrderBy(x => x.CUENTA).Distinct();

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

                                                    //ws.Cells["B" + j].Value = item2.CODIGO;
                                                    //ws.Cells["C" + j].Value = item2.NOMBRE;
                                                    //ws.Cells["E" + j].Value = item3.TERCERO;

                                                    //string nombreTercero = "";
                                                    //if (item3.terceroFK != null)
                                                    //{
                                                    //    //ws.Cells["F" + j].Value = item3.terceroFK.NOMBRE1 + " " + item3.terceroFK.NOMBRE2 + " " + item3.terceroFK.APELLIDO1 + " " + item3.terceroFK.APELLIDO2;
                                                    //    nombreTercero = item3.terceroFK.NOMBRE1 + " " + item3.terceroFK.NOMBRE2 + " " + item3.terceroFK.APELLIDO1 + " " + item3.terceroFK.APELLIDO2;
                                                    //}
                                                    //else
                                                    //{
                                                    //    ws.Cells["F" + j].Value = "";
                                                    //}
                                                    //ws.Cells["G" + j].Value = saldoInicial.ToString("N0", formato);
                                                    //ws.Cells["H" + j].Value = debitoActual.ToString("N0", formato);
                                                    //ws.Cells["I" + j].Value = creditoActual.ToString("N0", formato);
                                                    //ws.Cells["J" + j].Value = saldo.ToString("N0", formato);

                                                    var objeto = new spBalanceComprobacionL5()
                                                    {
                                                        codigo = item2.CODIGO,
                                                        nombre = item2.NOMBRE,
                                                        DocumentoTercero = item3.TERCERO,
                                                        NombreTercero = item3.NOMBRE,
                                                        SaldoInicial = saldoInicial.ToString("N0", formato),
                                                        Debito = debitoActual.ToString("N0", formato),
                                                        Credito = creditoActual.ToString("N0", formato),
                                                        Saldo = saldo.ToString("N0", formato)
                                                    };
                                                    slPC.Add(objeto);

                                                    //j++;

                                                }
                                            }


                                        }//fin else != 9

                                    }//fin if dataActual != 0

                                }//fin for item2
                            }


                        }

                        if (chkTercero != "on")
                        {
                            foreach (var ob in slPC)
                            {
                                ws.Cells["A" + j].Value = ob.codigo;
                                ws.Cells["B" + j].Value = ob.nombre;
                                ws.Cells["C" + j].Value = ob.SaldoInicial;
                                ws.Cells["D" + j].Value = ob.Debito;
                                ws.Cells["E" + j].Value = ob.Credito;
                                ws.Cells["F" + j].Value = ob.Saldo;
                                j++;
                            }
                        }
                        else
                        {
                            foreach (var ob in slPC)
                            {
                                ws.Cells["A" + j].Value = ob.codigo;
                                ws.Cells["B" + j].Value = ob.nombre;
                                ws.Cells["C" + j].Value = ob.DocumentoTercero;
                                ws.Cells["D" + j].Value = ob.NombreTercero;
                                ws.Cells["E" + j].Value = ob.SaldoInicial;
                                ws.Cells["F" + j].Value = ob.Debito;
                                ws.Cells["G" + j].Value = ob.Credito;
                                ws.Cells["H" + j].Value = ob.Saldo;
                                j++;
                            }
                        }

                        ws.Cells[ws.Dimension.Address].AutoFitColumns();//siempre al final de todo. le da tamaño ajustado a cada columna
                        movtosActuales = null;
                        movtosSaldos = null;
                        auxiliar = null;





                        #endregion

                        break;
                    case 39:
                        #region informe39 Saldo aportes

                        if (fechaDesde != "" && fechaHasta != "")
                        {
                            DateTime df = Convert.ToDateTime(fechaDesde);
                            DateTime dh = Convert.ToDateTime(fechaHasta);
                            filtro = df.ToShortDateString() + " - " + dh.ToShortDateString();
                        }
                        else if (fechaDesde != "" && fechaHasta == "")
                        {
                            DateTime df = Convert.ToDateTime(fechaDesde);
                            filtro = df.ToShortDateString();
                        }
                        else if (fechaDesde == "" && fechaHasta != "")
                        {
                            DateTime dh = Convert.ToDateTime(fechaHasta);
                            filtro = dh.ToShortDateString();
                        }
                        ws = pack.Workbook.Worksheets.Add("SaldoAportes");
                        // encabezado
                        ws.Cells["A1:D1,A2:D2,A3:D3,A4:D4,A5:D5"].Merge = true;
                        ws.Cells["A2:D2,A3:D3,A4:D4"].Style.Font.Bold = true;
                        ws.Cells["A2:D2"].Style.Font.Name = "Arial";
                        ws.Cells["A2:D2"].Style.Font.Size = 14;
                        ws.Cells["A" + 2].Value = "SALDO APORTES   " + filtro;
                        ws.Cells[1, 1, 5, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 5, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        ws.Cells["A2:D2,A3:D3,A4:D4,A5:D5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A2:D2,A3:D3,A4:D4,A7:D7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A2:D2,A3:D3,A4:D4,A7:D7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A2:D2,A3:D3,A4:D4,A5:D5"].Style.WrapText = true;
                        ws.Cells["A3:D3,A4:D4"].Style.Font.Size = 12;
                        ws.Cells["A5:D5"].Style.Font.Size = 10;
                        ws.Cells["A" + 3].Value = nombre;
                        ws.Cells["A" + 4].Value = nit;
                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        ws.Cells["A6:D6"].Merge = true;
                        ws.Cells[6, 1, 6, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        ws.Cells[7, 1, 7, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        ws.Cells["A7:E7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        ws.Cells["A7:D7"].Style.Font.Bold = true;
                        ws.Cells[7, 1, 7, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[7, 1, 7, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        //fin encabezado


                        ws.Cells["A" + 7].Value = "NIT";
                        ws.Cells["B" + 7].Value = "NOMBRES Y APELLIDOS";
                        ws.Cells["C" + 7].Value = "FECHA DE INICIO";
                        ws.Cells["D" + 7].Value = "SALDO DE APORTES";

                        j = 8;


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
                        #endregion
                        break;
                    case 40:
                        #region informe40 afiliaciones por asesor
                        var fichasAportes = db.FichasAportes.ToList();

                        if (fechaDesde != "" && fechaHasta != "")
                        {
                            DateTime fd = Convert.ToDateTime(fechaDesde);
                            DateTime fh = Convert.ToDateTime(fechaHasta);
                            filtro = fd.ToShortDateString() + " - " + fh.ToShortDateString();
                        }
                        else if (fechaDesde != "" && fechaHasta == "")
                        {
                            DateTime fd = Convert.ToDateTime(fechaDesde);
                            filtro = fd.ToShortDateString();
                        }
                        else if (fechaDesde == "" && fechaHasta != "")
                        {
                            DateTime fh = Convert.ToDateTime(fechaHasta);
                            filtro = fh.ToShortDateString();
                        }

                        ws = pack.Workbook.Worksheets.Add("AfilicacionesPorAsesor");

                        // encabezado
                        ws.Cells["A1:F1,A2:F2,A3:F3,A4:F4,A5:F5"].Merge = true;
                        ws.Cells["A2:F2,A3:F3,A4:F4"].Style.Font.Bold = true;
                        ws.Cells["A2:F2"].Style.Font.Name = "Arial";
                        ws.Cells["A2:F2"].Style.Font.Size = 14;
                        ws.Cells["A" + 2].Value = "AFILIACIONES POR ASESOR   " + filtro;
                        ws.Cells[1, 1, 5, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 5, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        ws.Cells["A2:F2,A3:F3,A4:F4,A5:F5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A2:F2,A3:F3,A4:F4,A7:F7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A2:F2,A3:F3,A4:F4,A7:F7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A2:F2,A3:F3,A4:F4,A5:F5"].Style.WrapText = true;
                        ws.Cells["A3:F3,A4:F4"].Style.Font.Size = 12;
                        ws.Cells["A5:F5"].Style.Font.Size = 10;
                        ws.Cells["A" + 3].Value = nombre;
                        ws.Cells["A" + 4].Value = nit;
                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        ws.Cells["A6:F6"].Merge = true;
                        ws.Cells[6, 1, 6, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        ws.Cells[7, 1, 7, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        ws.Cells["A7:G7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        ws.Cells["A7:F7"].Style.Font.Bold = true;
                        ws.Cells[7, 1, 7, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[7, 1, 7, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        //fin encabezado

                        ws.Cells["A" + 7].Value = "ID ASESOR";
                        ws.Cells["B" + 7].Value = "NOMBRE ASESOR";
                        ws.Cells["C" + 7].Value = "ID CLIENTE";
                        ws.Cells["D" + 7].Value = "NOMBRE CLIENTE";
                        ws.Cells["E" + 7].Value = "FECHA APERTURA";
                        ws.Cells["F" + 7].Value = "VALOR CUOTA";

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
                            j = 8;
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
                        fichasAportes = null;
                        #endregion
                        break;
                    case 41:
                        #region informe41 afiliaciones por agencia
                        fichasAportes = db.FichasAportes.ToList();

                        if (fechaDesde != "" && fechaHasta != "")
                        {
                            DateTime fd = Convert.ToDateTime(fechaDesde);
                            DateTime fh = Convert.ToDateTime(fechaHasta);
                            filtro = fd.ToShortDateString() + " - " + fh.ToShortDateString();
                        }
                        else if (fechaDesde != "" && fechaHasta == "")
                        {
                            DateTime fd = Convert.ToDateTime(fechaDesde);
                            filtro = fd.ToShortDateString();
                        }
                        else if (fechaDesde == "" && fechaHasta != "")
                        {
                            DateTime fh = Convert.ToDateTime(fechaHasta);
                            filtro = fh.ToShortDateString();
                        }

                        ws = pack.Workbook.Worksheets.Add("AfilicacionesPorAgencia");

                        // encabezado
                        ws.Cells["A1:F1,A2:F2,A3:F3,A4:F4,A5:F5"].Merge = true;
                        ws.Cells["A2:F2,A3:F3,A4:F4"].Style.Font.Bold = true;
                        ws.Cells["A2:F2"].Style.Font.Name = "Arial";
                        ws.Cells["A2:F2"].Style.Font.Size = 14;
                        ws.Cells["A" + 2].Value = "AFILIACIONES POR AGENCIA   " + filtro;
                        ws.Cells[1, 1, 5, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 5, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        ws.Cells["A2:F2,A3:F3,A4:F4,A5:F5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A2:F2,A3:F3,A4:F4,A7:F7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A2:F2,A3:F3,A4:F4,A7:F7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A2:F2,A3:F3,A4:F4,A5:F5"].Style.WrapText = true;
                        ws.Cells["A3:F3,A4:F4"].Style.Font.Size = 12;
                        ws.Cells["A5:F5"].Style.Font.Size = 10;
                        ws.Cells["A" + 3].Value = nombre;
                        ws.Cells["A" + 4].Value = nit;
                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        ws.Cells["A6:F6"].Merge = true;
                        ws.Cells[6, 1, 6, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        ws.Cells[7, 1, 7, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        ws.Cells["A7:G7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        ws.Cells["A7:F7"].Style.Font.Bold = true;
                        ws.Cells[7, 1, 7, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[7, 1, 7, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        //fin encabezado
                        ws.Cells["A" + 7].Value = "FECHA DE AFILIACIÓN";
                        ws.Cells["B" + 7].Value = "DOCUMENTO";
                        ws.Cells["C" + 7].Value = "NOMBRE ASOCIADO";
                        ws.Cells["D" + 7].Value = "VALOR CUOTA";
                        ws.Cells["E" + 7].Value = "TOTAL APORTES";
                        ws.Cells["F" + 7].Value = "AGENCIA";


                        agencia = coll["agencia"];

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
                            j = 8;
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
                        fichasAportes = null;
                        #endregion
                        break;
                    case 45:
                        #region informe45 Reporte UIAF


                        var anioF = Convert.ToInt32(coll["year2"]);
                        var periodoF = Convert.ToInt32(coll["periodoTrimestral"]);
                        DateTime fi;
                        DateTime ff;
                        if (anioF != 0 && periodoF != 0)
                        {
                            switch (periodoF)
                            {

                                case 1:
                                    fi = new DateTime(anioF, 01, 01, 0, 0, 0);
                                    ff = new DateTime(anioF, 03, 31, 23, 59, 59);
                                    filtro = fi.ToShortDateString() + " - " + ff.ToShortDateString();
                                    break;
                                case 2:
                                    fi = new DateTime(anioF, 04, 01, 0, 0, 0);
                                    ff = new DateTime(anioF, 06, 30, 23, 59, 59);
                                    filtro = fi.ToShortDateString() + " - " + ff.ToShortDateString();
                                    break;
                                case 3:
                                    fi = new DateTime(anioF, 07, 01, 0, 0, 0);
                                    ff = new DateTime(anioF, 09, 30, 23, 59, 59);
                                    filtro = fi.ToShortDateString() + " - " + ff.ToShortDateString();
                                    break;
                                case 4:
                                    fi = new DateTime(anioF, 10, 01, 0, 0, 0);
                                    ff = new DateTime(anioF, 12, 31, 23, 59, 59);
                                    filtro = fi.ToShortDateString() + " - " + ff.ToShortDateString();
                                    break;
                            }
                        }
                        else if (anioF != 0 && periodoF == 0)
                            filtro = anioF.ToString();

                        ws = pack.Workbook.Worksheets.Add("ReporteUIAF");

                        // encabezado
                        ws.Cells["A1:X1,A2:X2,A3:X3,A4:X4,A5:X5,A6:X6"].Merge = true;
                        ws.Cells["A2:X2,A3:X3,A4:X4,A5:X5"].Style.Font.Bold = true;
                        ws.Cells["A2:X2"].Style.Font.Name = "Arial";
                        ws.Cells["A2:X2"].Style.Font.Size = 14;
                        ws.Cells["A" + 2].Value = "REPORTE UIAF   " + filtro;
                        ws.Cells[1, 1, 6, 24].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 6, 24].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        ws.Cells["A2:X2,A3:X3,A4:X4,A5:X5,A6:X6"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A2:X2,A3:X3,A4:X4,A5:X5,A8:X8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A2:X2,A3:X3,A4:X4,A5:X5,A8:X8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A3:F3,A4:F4,A5:Y5"].Style.Font.Size = 12;
                        ws.Cells["A6:F6"].Style.Font.Size = 10;
                        ws.Cells["A" + 3].Value = nombre;
                        ws.Cells["A" + 4].Value = nit;
                        ws.Cells["A" + 5].Value = "REPORTE TRANSACCIONES EN EFECTIVO - C.E. 014 DE 2018 PARA ORGANIZACIONES DE LA ECONOMÍA SOLIDARIA QUE NO EJERCEN ACTIVIDAD FINANCIERA DEL COPERATIVISMO";
                        ws.Cells["A" + 6].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        ws.Cells["A7:X7"].Merge = true;
                        ws.Cells[7, 1, 7, 24].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        ws.Cells[8, 1, 8, 24].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        ws.Cells["A8:Y8"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        ws.Cells["A8:X8"].Style.Font.Bold = true;
                        ws.Cells[8, 1, 8, 24].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[8, 1, 8, 24].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        ws.Cells["A8:X8"].Style.WrapText = true;
                        //fin encabezado

                        ws.Cells["A" + 8].Value = "CONSECUTIVO";
                        ws.Cells["B" + 8].Value = "FECHA TRANSACCIÓN";
                        ws.Cells["C" + 8].Value = "VALOR TRANSACCIÓN";
                        ws.Cells["D" + 8].Value = "TIPO MONEDA";
                        ws.Cells["E" + 8].Value = "CÓDIGO OFICINA";
                        ws.Cells["F" + 8].Value = "CÓDIGO DPTO/MPIO";
                        ws.Cells["G" + 8].Value = "TIPO PRODUCTO";
                        ws.Cells["H" + 8].Value = "TIPO TRANSACCIÓN";
                        ws.Cells["I" + 8].Value = "Nro CUENTA O PRODUCTO";
                        ws.Cells["J" + 8].Value = "TIPO IDENTIFICACIÓN DEL TITULAR";
                        ws.Cells["K" + 8].Value = "Nro IDENTIFICACIÓN DEL TITULAR";
                        ws.Cells["L" + 8].Value = "1er. APELLIDO DEL TITULAR";
                        ws.Cells["M" + 8].Value = "2do. APELLIDO DEL TITULAR";
                        ws.Cells["N" + 8].Value = "1er. NOMBRE DEL TITULAR";
                        ws.Cells["O" + 8].Value = "OTROS NOMBRES DEL TITULAR";
                        ws.Cells["P" + 8].Value = "RAZÓN SOCIAL DEL TITULAR";
                        ws.Cells["Q" + 8].Value = "ACTIVIDAD ECONÓMICA DEL TITULAR";
                        ws.Cells["R" + 8].Value = "INGRESO MENSUAL DEL TITULAR";
                        ws.Cells["S" + 8].Value = "TIPO IDENTIFICACIÓN PERSONA QUE REALIZA LA TRANSACCIÓN INDIVIDUAL";
                        ws.Cells["T" + 8].Value = "Nro IDENTIFICACIÓN PERSONA QUE REALIZA LA TRANSACCIÓN INDIVIDUAL";
                        ws.Cells["U" + 8].Value = "1er. APELLIDO PERSONA QUE REALIZA LA TRANSACCIÓN INDIVIDUAL";
                        ws.Cells["V" + 8].Value = "2do. APELLIDO PERSONA QUE REALIZA LA TRANSACCIÓN INDIVIDUAL";
                        ws.Cells["W" + 8].Value = "1er. NOMBRE PERSONA QUE REALIZA LA TRANSACCIÓN INDIVIDUAL";
                        ws.Cells["X" + 8].Value = "OTROS NOMBRES PERSONA QUE REALIZA LA TRANSACCIÓN INDIVIDUAL";

                        var periodoUIAF = Convert.ToInt32(coll["periodoTrimestral"]);
                        var desdeAnio = Convert.ToInt32(coll["year2"]);
                        if (periodoUIAF != 0 && desdeSaldoAno != 0)
                        {
                            DateTime fechaActual = DateTime.Now;
                            // ws.Cells["A" + 4].Value = fechaActual.ToString("yyyy-MMMM");
                            List<FactOpcaja> facturas = new List<FactOpcaja>();
                            if (periodoUIAF == 1)
                            {
                                DateTime fechaIni = new DateTime(desdeAnio, 01, 01, 0, 0, 0);
                                DateTime fechaFin = new DateTime(desdeAnio, 03, 31, 23, 59, 59);
                                facturas = db.FactOpcaja.Where(x => x.fecha >= fechaIni && x.fecha <= fechaFin).ToList();
                            }
                            else if (periodoUIAF == 2)
                            {
                                DateTime fechaIni = new DateTime(desdeAnio, 04, 01, 0, 0, 0);
                                DateTime fechaFin = new DateTime(desdeAnio, 06, 30, 23, 59, 59);
                                facturas = db.FactOpcaja.Where(x => x.fecha >= fechaIni && x.fecha <= fechaFin).ToList();
                            }
                            else if (periodoUIAF == 3)
                            {
                                DateTime fechaIni = new DateTime(desdeAnio, 07, 01, 0, 0, 0);
                                DateTime fechaFin = new DateTime(desdeAnio, 09, 30, 23, 59, 59);
                                facturas = db.FactOpcaja.Where(x => x.fecha >= fechaIni && x.fecha <= fechaFin).ToList();
                            }
                            else if (periodoUIAF == 4)
                            {
                                DateTime fechaIni = new DateTime(desdeAnio, 10, 01, 0, 0, 0);
                                DateTime fechaFin = new DateTime(desdeAnio, 12, 31, 23, 59, 59);
                                facturas = db.FactOpcaja.Where(x => x.fecha >= fechaIni && x.fecha <= fechaFin).ToList();
                            }

                            facturas = facturas.OrderBy(x => x.fecha).ToList();

                            i = 1;
                            j = 9;
                            foreach (var item in facturas)
                            {
                                ws.Cells["A" + j].Value = i.ToString();
                                ws.Cells["B" + j].Value = item.fecha.ToString();
                                ws.Cells["C" + j].Value = item.total.ToString("N0", formato);
                                ws.Cells["D" + j].Value = "1";
                                ws.Cells["E" + j].Value = item.Caja.agencia;
                                ws.Cells["F" + j].Value = item.Caja.agencias.codciudad;
                                ws.Cells["G" + j].Value = 13.ToString();
                                ws.Cells["H" + j].Value = 2.ToString();
                                ws.Cells["I" + j].Value = item.numero_cuenta;
                                ws.Cells["J" + j].Value = 13.ToString();
                                ws.Cells["K" + j].Value = item.nit_propietario_cuenta;
                                ws.Cells["L" + j].Value = item.terceroFK.APELLIDO1;
                                ws.Cells["M" + j].Value = item.terceroFK.APELLIDO2;
                                ws.Cells["N" + j].Value = item.terceroFK.NOMBRE1;
                                ws.Cells["O" + j].Value = "";
                                ws.Cells["P" + j].Value = "";
                                ws.Cells["Q" + j].Value = item.terceroFK.profesionFK.Nom_prof;
                                ws.Cells["R" + j].Value = Convert.ToInt32(item.terceroFK.SALARIO).ToString("N0", formato);
                                ws.Cells["S" + j].Value = 13.ToString();
                                ws.Cells["T" + j].Value = item.nit_propietario_cuenta;
                                ws.Cells["U" + j].Value = "";
                                ws.Cells["V" + j].Value = "";
                                ws.Cells["W" + j].Value = "";
                                ws.Cells["X" + j].Value = "";

                                i++;
                                j++;

                            }
                            ws.Cells[ws.Dimension.Address].AutoFitColumns();
                            facturas = null;
                        }
                        #endregion
                        break;
                    case 47:
                        #region Catálogo de cuentas

                        var nivelCC = coll["nivel2"];
                        costo = coll["costo"];

                        if (fechaDesde != "" && fechaHasta != "")
                        {
                            DateTime df = Convert.ToDateTime(fechaDesde);
                            DateTime dh = Convert.ToDateTime(fechaHasta);
                            filtro = df.ToShortDateString() + " - " + dh.ToShortDateString();
                        }
                        else if (fechaDesde != "" && fechaHasta == "")
                        {
                            DateTime df = Convert.ToDateTime(fechaDesde);
                            filtro = df.ToShortDateString();
                        }
                        else if (fechaDesde == "" && fechaHasta != "")
                        {
                            DateTime dh = Convert.ToDateTime(fechaHasta);
                            filtro = dh.ToShortDateString();
                        }

                        ws = pack.Workbook.Worksheets.Add("Cátalogo_Cuentas");

                        // encabezado
                        ws.Cells["A1:D1,A2:D2,A3:D3,A4:D4,A5:D5"].Merge = true;
                        ws.Cells["A2:D2,A3:D3,A4:D4"].Style.Font.Bold = true;
                        ws.Cells["A2:D2"].Style.Font.Name = "Arial";
                        ws.Cells["A2:D2"].Style.Font.Size = 14;
                        ws.Cells["A" + 2].Value = "CATÁLOGO DE CUENTAS   " + filtro;
                        ws.Cells[1, 1, 5, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 5, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        ws.Cells["A2:D2,A3:D3,A4:D4,A5:D5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A2:D2,A3:D3,A4:D4,A7:D7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A2:D2,A3:D3,A4:D4,A7:D7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A2:D2,A3:D3,A4:D4,A5:D5"].Style.WrapText = true;
                        ws.Cells["A3:D3,A4:D4"].Style.Font.Size = 12;
                        ws.Cells["A5:D5"].Style.Font.Size = 10;
                        ws.Cells["A" + 3].Value = nombre;
                        ws.Cells["A" + 4].Value = nit;
                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        ws.Cells["A6:D6"].Merge = true;
                        ws.Cells[6, 2, 6, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        ws.Cells[7, 2, 7, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        ws.Cells["A7:E7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        ws.Cells["A7:D7"].Style.Font.Bold = true;
                        ws.Cells[7, 2, 7, 4].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[7, 2, 7, 4].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        //fin encabezado

                        ws.Cells["B" + 7].Value = "CUENTA";
                        ws.Cells["C" + 7].Value = "NOMBRE CUENTA";
                        ws.Cells["D" + 7].Value = "SALDO";

                        j = 8;
                        movtosSaldos = new List<MovimientoAuxiliar>();
                        movtosActuales = new List<MovimientoAuxiliar>();
                        movtosAuxiliarActual = new List<MovimientoAuxiliar>();//sirve para cuando se escoge con terceros
                        auxiliar = new List<CuentaMayor>();


                        if (fechaDesde != "" && fechaHasta != "")
                        {
                            DateTime fh = Convert.ToDateTime(fechaHasta);
                            DateTime fd = Convert.ToDateTime(fechaDesde);
                            DateTime fechHasta = new DateTime(fh.Year, fh.Month, fh.Day, 23, 59, 59);
                            DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);

                            movtosActuales = db.Database.SqlQuery<MovimientoAuxiliar>(
                                "dbo.sp_BalanceComprobacion @fecha",
                                new SqlParameter("@fecha", fechHasta)
                                ).ToList();
                            if (costo != "")
                            {
                                movtosActuales = movtosActuales.Where(x => x.CCOSTO == costo).ToList();
                            }
                            movtosAuxiliarActual = movtosActuales;
                            movtosSaldos = movtosActuales.Where(x => x.FECHAMOVIMIENTO < fechDesde).ToList();
                            movtosActuales = movtosActuales.Where(x => x.FECHAMOVIMIENTO >= fechDesde).ToList();

                        }
                        else if (fechaDesde != "")
                        {

                            DateTime fd = Convert.ToDateTime(fechaDesde);
                            DateTime fechDesde = new DateTime(fd.Year, fd.Month, fd.Day, 0, 0, 0);
                            movtosActuales = db.Database.SqlQuery<MovimientoAuxiliar>(
                                "dbo.sp_BalanceComprobacion @fecha",
                                new SqlParameter("@fecha", fechDesde)
                                ).ToList();
                            //movtosActuales = db.Movimientos.Where(x => x.FECHAMOVIMIENTO == fechDesde && x.Comprobante.ANULADO == false).ToList();
                            if (costo != "")
                            {
                                movtosActuales = movtosActuales.Where(x => x.CCOSTO == costo).ToList();
                            }
                            movtosAuxiliarActual = movtosActuales;
                            movtosSaldos = movtosActuales.Where(x => x.FECHAMOVIMIENTO < fechDesde).ToList();

                        }

                        cuentas = db.PlanCuentas.ToList();

                        //nivel 1
                        if (nivelCC == "1")
                        {
                            auxiliar = cuentas.Where(x => x.CODIGO.Length == 1).ToList();
                        }
                        else if (nivelCC == "2")
                        {
                            auxiliar = cuentas.Where(x => x.CODIGO.Length == 1 || x.CODIGO.Length == 2).ToList();
                        }
                        else if (nivelCC == "3")
                        {
                            auxiliar = cuentas.Where(x => x.CODIGO.Length == 1 || x.CODIGO.Length == 2 || x.CODIGO.Length == 4).ToList();
                        }
                        else if (nivelCC == "4")
                        {
                            auxiliar = cuentas.Where(x => x.CODIGO.Length == 1 || x.CODIGO.Length == 2 || x.CODIGO.Length == 4 || x.CODIGO.Length == 6).ToList();
                        }
                        else if (nivelCC == "5")
                        {
                            auxiliar = cuentas.Where(x => x.CODIGO.Length == 1 || x.CODIGO.Length == 2 || x.CODIGO.Length == 4 || x.CODIGO.Length == 6 || x.CODIGO.Length == 9).ToList();

                        }
                        slPC = new List<spBalanceComprobacionL5>();
                        fdAuxilar = Convert.ToDateTime(fechaDesde);
                        fDesdeAuxiliar = new DateTime(fdAuxilar.Year, 1, 1, 0, 0, 0);

                        if (auxiliar.Count > 0)
                        {
                            decimal saldoInicial = 0, debitoActual = 0, creditoActual = 0;
                            debitoAnterior = 0; creditoAnterior = 0;
                            saldo = 0;
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

                                    if (saldo != 0)
                                    {
                                        var objeto = new spBalanceComprobacionL5()
                                        {
                                            codigo = item2.CODIGO,
                                            nombre = item2.NOMBRE,
                                            SaldoInicial = "0",
                                            Debito = "0",
                                            Credito = "0",
                                            Saldo = saldo.ToString("N0", formato)
                                        };
                                        slPC.Add(objeto);
                                    }


                                    //j++;
                                }

                            }


                        }

                        foreach (var ob in slPC)
                        {
                            ws.Cells["B" + j].Value = ob.codigo;
                            ws.Cells["C" + j].Value = ob.nombre;
                            ws.Cells["D" + j].Value = ob.Saldo;
                            j++;
                        }

                        ws.Cells[ws.Dimension.Address].AutoFitColumns();//siempre al final de todo. le da tamaño ajustado a cada columna
                        movtosActuales = null;
                        movtosSaldos = null;
                        auxiliar = null;
                        #endregion
                        break;
                    default:
                        ws = pack.Workbook.Worksheets.Add("balanceGeneral");
                        // encabezado
                        ws.Cells["A1:H1,A2:H2,A3:H3,A4:H4,A5:H5"].Merge = true;
                        ws.Cells["A2:H2,A3:H3,A4:H4"].Style.Font.Bold = true;
                        ws.Cells["A2:H2"].Style.Font.Name = "Arial";
                        ws.Cells["A2:H2"].Style.Font.Size = 14;
                        ws.Cells["A" + 2].Value = "BALANCE GENERAL   ";
                        ws.Cells[1, 1, 5, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, 1, 5, 8].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                        ws.Cells["A2:H2,A3:H3,A4:H4,A5:H5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                        ws.Cells["A2:H2,A3:H3,A4:H4,A7:H7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        ws.Cells["A2:H2,A3:H3,A4:H4,A7:H7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        ws.Cells["A2:H2,A3:H3,A4:H4,A5:H5"].Style.WrapText = true;
                        ws.Cells["A3:H3,A4:H4"].Style.Font.Size = 12;
                        ws.Cells["A5:H5"].Style.Font.Size = 10;
                        ws.Cells["A" + 3].Value = nombre;
                        ws.Cells["A" + 4].Value = nit;
                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                        ws.Cells["A6:H6"].Merge = true;
                        ws.Cells[6, 1, 6, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                        ws.Cells[7, 1, 7, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                        ws.Cells["A7:I7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                        ws.Cells["A7:H7"].Style.Font.Bold = true;
                        ws.Cells[7, 1, 7, 8].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[7, 1, 7, 8].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                        //fin encabezado
                        ws.Cells["A" + 7].Value = "CUENTA_ACT";
                        ws.Cells["B" + 7].Value = "NOM_ACT";
                        ws.Cells["C" + 7].Value = "VAL_ACT";
                        ws.Cells["D" + 7].Value = "NAT_ACT";
                        ws.Cells["E" + 7].Value = "CUENTA_PAS";
                        ws.Cells["F" + 7].Value = "NOM_PAS";
                        ws.Cells["G" + 7].Value = "VAL_PAS";
                        ws.Cells["H" + 7].Value = "NAT_PAS";
                        break;
                }

                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                var ms = new System.IO.MemoryStream();
                pack.SaveAs(ms);
                ms.WriteTo(Response.OutputStream);
            }
            //Response.Flush();
            Response.End();

            return RedirectToAction("../Informes/Index");

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
                var PUC = ctx.PlanCuentas.Select(j => new
                {
                    c = j.CODIGO,
                    N = j.NOMBRE,
                    s = j.Saldo,

                });


                using (ExcelPackage pack = new ExcelPackage())

                {
                    ExcelWorksheet ws = pack.Workbook.Worksheets.Add("Catalogo de Cuentas");
                    ws.Cells["A" + 1].Value = "CODIGO";
                    ws.Cells["B" + 1].Value = "NOMBRE";
                    ws.Cells["c" + 1].Value = "SALDO";
                    int j = 2;

                    foreach (var item in PUC)
                    {

                        ws.Cells["A" + j].Value = item.c;
                        ws.Cells["B" + j].Value = item.N;
                        ws.Cells["c" + j].Value = item.s;


                        j++;
                    }
                    ws.Cells[ws.Dimension.Address].AutoFitColumns();
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



    }


}