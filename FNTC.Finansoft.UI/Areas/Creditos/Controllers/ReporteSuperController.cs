using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.MCreditos;

namespace FNTC.Finansoft.UI.Areas.Creditos.Controllers
{
    public class ReporteSuperController : Controller
    {
        private AccountingContext db = new AccountingContext();
        //
        // GET: /ReporteSuper/
        public ActionResult Index()
        {
            return View();

        }
        [HttpPost]
        public ActionResult Previa(string FI, string FF)
        {
            var FechaI = Convert.ToDateTime(FI);

            var FechaF = Convert.ToDateTime(FF);

            List<ViewModelReporteSuper> ViewModelReporteSuper = new List<ViewModelReporteSuper>();
            var ListaReporte = (from prestamo in db.Prestamos
                                    // join real in db.Real on prestamo.Pagare equals real.PagareId
                                join tercer in db.Terceros on prestamo.NIT equals tercer.NIT
                                join subdest in db.SubDestinos on prestamo.Subdestino_Id equals subdest.Subdestino_Id
                                join destino in db.Destinos on subdest.Destino_Id equals destino.Destino_Id
                                join linea in db.Lineas on destino.Lineas_Id equals linea.Lineas_Id
                                join forma in db.Forma_Pago on prestamo.Forma_Pago_Id equals forma.Forma_Pago_Id


                                select new
                                {
                                    prestamo.Pagare,
                                    prestamo.Fecha_Prestamo,
                                    prestamo.Capital,
                                    prestamo.Interes,
                                    prestamo.Plazo,
                                    tercer.NIT,
                                    subdest.Subdestino_Codigo,
                                    destino.Destino_Codigo,
                                    linea.Lineas_Codigo,
                                    forma.Forma_Pago_Id,
                                    // real.Real_Id,
                                    // real.Real_Valor
                                }).ToList();

            var Reporte = from datos in ListaReporte
                          where ((Convert.ToDateTime(datos.Fecha_Prestamo).Date >= FechaI.Date) && (Convert.ToDateTime(datos.Fecha_Prestamo).Date <= FechaF.Date))
                          select datos;

            foreach (var item in Reporte)
            {
                ViewModelReporteSuper obj = new ViewModelReporteSuper();
                obj.Pagare = item.Pagare;
                obj.Fecha_Prestamo = item.Fecha_Prestamo.ToShortDateString();
                obj.Capital = item.Capital;
                obj.Interes = item.Interes;
                obj.Plazo = item.Plazo;
                obj.NIT = item.NIT;
                obj.Subdestino_Codigo = item.Subdestino_Codigo;
                obj.Destino_Codigo = item.Destino_Codigo;
                obj.Lineas_Codigo = item.Lineas_Codigo;
                obj.Forma_Pago_Id = item.Forma_Pago_Id;
                //  obj.Real_Id = item.Real_Id;
                // obj.Real_Valor = item.Real_Valor;


                ViewModelReporteSuper.Add(obj);
            }
            return View(ViewModelReporteSuper);
        }

        [HttpPost]
        public ActionResult exportar(string FI, string FF)
        {
            var fi = FI;
            var ff = FF;

            var FechaI = Convert.ToDateTime(fi);

            var FechaF = Convert.ToDateTime(ff);


            DataTable dt = new DataTable("Creditos");
            dt.Columns.AddRange(new DataColumn[22] {
                new DataColumn("Pagare"),
                                            new DataColumn("Fecha"),
                                            new DataColumn("Capital"),
                                            new DataColumn("Capital Inicial"),
                                            new DataColumn("Interes"),
                                            new DataColumn("Plazo"),
                                            new DataColumn("NIT"),
                                            new DataColumn("Subdestino"),
                                            new DataColumn("Destino") ,
                                            new DataColumn("Linea"),
                                            new DataColumn("Forma de Pago"),
                                            new DataColumn("Cancelado"),
                                            new DataColumn("Int Corriente"),
                                            new DataColumn("Int Mora"),
                                            new DataColumn("Tipo Cuota"),
                                            new DataColumn("Asiento de Nomina"),
                                            new DataColumn("Cod Op"),
                                            new DataColumn("F Sistema"),
                                            new DataColumn("Valor Garantia"),
                                            new DataColumn("Codigo Garantia"),
                                            new DataColumn("Valor de La CUota"),
                                            new DataColumn("Saldo Capital")

            });

            var ListaReporte = (from prestamo in db.Prestamos

                                join tercer in db.Terceros on prestamo.NIT equals tercer.NIT
                                join subdest in db.SubDestinos on prestamo.Subdestino_Id equals subdest.Subdestino_Id
                                join destino in db.Destinos on subdest.Destino_Id equals destino.Destino_Id
                                join linea in db.Lineas on destino.Lineas_Id equals linea.Lineas_Id
                                join forma in db.Forma_Pago on prestamo.Forma_Pago_Id equals forma.Forma_Pago_Id

                                select prestamo

                          ).ToList();

            var Reporte = from datos in ListaReporte

                          where ((Convert.ToDateTime(datos.Fecha_Prestamo).Date >= FechaI.Date) && (Convert.ToDateTime(datos.Fecha_Prestamo).Date <= FechaF.Date))
                          select new
                          {

                              datos.Pagare,
                              datos.Fecha_Prestamo,
                              datos.Capital,
                              CapIn = datos.Capital,
                              datos.Interes,
                              datos.Plazo,
                              datos.NIT,
                              datos.SubDestinos.Subdestino_Codigo,
                              datos.SubDestinos.Destinos.Destino_Codigo,
                              datos.SubDestinos.Destinos.Lineas.Lineas_Codigo,
                              datos.Forma_Pago.Forma_Pago_Id,
                              cancelado = "S",
                              IntCorriente = "",
                              IntMora = "",
                              TipoCuota = "C",
                              AsientoN = "S",
                              CodOp = "001",
                              Fsistema = DateTime.Now,
                              query = "0",
                              query2 = "0",
                              cuota = "0",
                              SaldoCap = "0"
                          };




            foreach (var customer in Reporte)
            {
                dt.Rows.Add(
                    customer.Pagare,
                    customer.Fecha_Prestamo,
                    customer.Capital,
                    customer.CapIn,
                    customer.Interes,
                    customer.Plazo,
                    customer.NIT,
                    customer.Subdestino_Codigo,
                    customer.Destino_Codigo,
                    customer.Lineas_Codigo,
                    customer.Forma_Pago_Id,
                    customer.cancelado,
                    customer.IntCorriente,
                    customer.IntMora,
                    customer.TipoCuota,
                    customer.AsientoN,
                    customer.CodOp,
                    customer.Fsistema,
                    customer.query,
                    customer.query2,
                    customer.cuota,
                    customer.Capital
                   );
            }

            using (var wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Creditos.xlsx");
                }
            }



        }

        [HttpPost]
        public FileResult Export()
        {
            var FI = "05/12/2017";
            var FechaI = Convert.ToDateTime(FI);
            var FF = "13/12/2017";
            var FechaF = Convert.ToDateTime(FF);

            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[4] { new DataColumn("CustomerId"),
                                            new DataColumn("ContactName"),
                                            new DataColumn("City"),
                                            new DataColumn("Country") });

            //var customers = from customer in db.Prestamos.Take(10)
            //                select customer;
            var ListaReporte = (from prestamo in db.Prestamos
                                    //  join real in db.Real on prestamo.Pagare equals real.PagareId
                                join tercer in db.Terceros on prestamo.NIT equals tercer.NIT
                                join subdest in db.SubDestinos on prestamo.Subdestino_Id equals subdest.Subdestino_Id
                                join destino in db.Destinos on subdest.Destino_Id equals destino.Destino_Id
                                join linea in db.Lineas on destino.Lineas_Id equals linea.Lineas_Id
                                join forma in db.Forma_Pago on prestamo.Forma_Pago_Id equals forma.Forma_Pago_Id
                                // where prestamo.Fecha_Prestamo >= FechaI.Date && FechaF.Date >= prestamo.Fecha_Prestamo
                                select prestamo

                           ).ToList();

            //var Reporte = from datos in ListaReporte
            //              where Convert.ToDateTime(datos.Fecha_Prestamo) >= FechaI.Date && FechaF.Date >= Convert.ToDateTime(datos.Fecha_Prestamo)
            //              select datos;

            foreach (var customer in ListaReporte)
            {
                dt.Rows.Add(customer.Pagare, customer.NIT, customer.Plazo, customer.Interes);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Grid.xlsx");
                }
            }
        }



    }
}