
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
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace FNTC.Finansoft.Areas.Aportes.Controllers
{
    public class AportesExtraController : Controller
    {
        NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;

       
        private AccountingContext db = new AccountingContext();
        // GET: Aportes/Aportes
        public ActionResult AportesExtra()
        {
            return View(db.FichaAfiliadosAporteEx.ToList());
        }
        public ActionResult ConfiguracionAportesExtra()
        {
            var useractual = User.Identity.Name;
            ViewBag.useractual = useractual;

            try
            {
                using (var db = new AccountingContext())
                {
                    Configuracion2Ex configuracion2Ex = db.Configuracion2Ex.Where(a => a.estado == true).FirstOrDefault();
                    return View(configuracion2Ex);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult ConfiguracionAportesExtra(Configuracion2Ex configuracionAportesEx)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                //Inhabilito configuraciones anteriores
                var configuracionesAnteriores = contextoFinansoft.Configuracion2Ex.Where(c => c.estado == true).ToList();
                var useractual = User.Identity.Name;
                foreach (var config in configuracionesAnteriores)
                {
                    config.estado = false;
                }

                contextoFinansoft.Configuracion2Ex.Add(configuracionAportesEx);

                try
                {
                    configuracionAportesEx.UsuarioActualizacion = useractual;
                    configuracionAportesEx.estado = true;
                    configuracionAportesEx.fechaRegistro = DateTime.Now;
                    contextoFinansoft.SaveChanges();
                    return RedirectToAction("AportesExtra");
                }
                catch (Exception)
                {
                    var configuracionGe = 1;
                    ViewBag.configuracionGeneral = configuracionGe;
                }

            }
            return View(configuracionAportesEx);
        }

        public ActionResult NuevoAfiliado()
        {
            var useractual = User.Identity.Name;
            ViewBag.useractual = useractual;
            ViewBag.FechaActual = DateTime.Now;


            //lista de Codeudores
            List<SelectListItem> items3C = new List<SelectListItem>();   // Creo una lista
            items3C.Add(new SelectListItem { Text = "Seleccione Asociado", Value = "" });
            IList<Tercero> lista4C = db.Terceros.ToList();// extraigo los elementos desde la DB

            var test = from s in lista4C where s.ESCODEUDOR == true select s;



            foreach (var item in lista4C)		// recorro los elementos de la db
            {
                items3C.Add(new SelectListItem { Text = item.NIT + "|" + item.NOMBRE1 + " " + item.NOMBRE2 + " " + item.APELLIDO1 + " " + item.APELLIDO2, Value = item.NIT.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.NIT = items3C;


            return View();
        }
        private FichaAfiliadosAporteEx DataInsert = new FichaAfiliadosAporteEx();
        [HttpPost]
        public ActionResult NuevoAfiliado(FichaAfiliadosAporteEx fichaAfiliadoEx)
        {
            var Configuracion = new BLLAportes().IdConfiguracion();
            var Abreviado = new BLLAportes().Abreviado();
            var useractual = User.Identity.Name;
            var consulta = new BLLAportes().ConsultaAsociado(fichaAfiliadoEx.idPersona);
            var consultaTercero = new BLLAportes().ConsultaAsociadoTercero(fichaAfiliadoEx.idPersona);
            var RespuestaG = DataInsert.Insertar(fichaAfiliadoEx, Configuracion, Abreviado, useractual, consulta, consultaTercero);
            if (RespuestaG == "Registrado Correctamente")
            {
                ViewBag.alerta = "success";
                ViewBag.res = RespuestaG;
                ViewBag.useractual = useractual;
            }
            else
            {
                ViewBag.alerta = "danger";
                ViewBag.res = RespuestaG;
                ViewBag.useractual = useractual;
            }
            return View("NuevoAfiliado");

        }
        public ActionResult EditarEX(int id)
        {
            try
            {
                ViewBag.Asociado = new BLLAportes().NombreAsociado(id);
                ViewBag.useractual = new BLLAportes().Asesor(id);
                ViewBag.FechaActual = new BLLAportes().FechaAfiliacion(id);

                using (var db = new AccountingContext())
                {
                    FichaAfiliadosAporteEx Conf = db.FichaAfiliadosAporteEx.Where(a => a.IdAfiliadosAporteEx == id).FirstOrDefault();
                    return View(Conf);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarEX(FichaAfiliadosAporteEx fichaAfiliadoEx)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    FichaAfiliadosAporteEx Con = db.FichaAfiliadosAporteEx.Find(fichaAfiliadoEx.IdAfiliadosAporteEx);

                    Con.Estado = fichaAfiliadoEx.Estado;
                    db.SaveChanges();
                    return RedirectToAction("AportesExtra");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult EliminarAfiliadoEx(int id)
        {
            try
            {
                ViewBag.Asociado = new BLLAportes().NombreAsociado(id);
                ViewBag.useractual = new BLLAportes().Asesor(id);
                ViewBag.FechaActual = new BLLAportes().FechaAfiliacion(id);
                ViewBag.TotalAportes = new BLLAportes().TotalAportes(id);
                ViewBag.EliminarMF = new BLLAportes().EliminarMF(id);

                using (var db = new AccountingContext())
                {
                    FichaAfiliadosAporteEx Conf = db.FichaAfiliadosAporteEx.Where(a => a.IdAfiliadosAporteEx == id).FirstOrDefault();
                    return View(Conf);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarAfiliadoEx(FichaAfiliadosAporteEx fichaAfiliadoEx)
        {
            var id = fichaAfiliadoEx.IdAfiliadosAporteEx;
            var Comprobacion = DataInsert.Eliminar(id);
            if (Comprobacion)
            {
                return RedirectToAction("AportesExtra");
            }
            return View("EliminarAfiliadoEx");
        }

        public ActionResult ReporteAporteExtra()
        {
            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;

            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            DateTime FechaActual = DateTime.Now;

            DateTime FechaAct = Convert.ToDateTime(FechaActual);
            DateTime Fecha = new DateTime(FechaAct.Year, FechaAct.Month, FechaAct.Day, 0, 0, 0);
            DateTime Fechas = Convert.ToDateTime(Fecha);

            string FechaString = Fechas.ToString("yyyy-MM-dd");

            var Productos = db.FichaAfiliadosAporteEx.Where(x => x.Estado == true || x.Estado == false).ToList();


            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=ReporteAfiliadosAporteExtraordinario.xlsx");

            using (ExcelPackage pack = new ExcelPackage())
            {
                ExcelWorksheet ws = pack.Workbook.Worksheets.Add("AfiliadosAporteExtraordinario");

                ws.Cells["C1:D1"].Merge = true;
                ws.Cells["C1:D1"].Value = "Afiliados - Aporte Extraordinario";
                ws.Cells["C1:D1"].Style.Font.Bold = true;
                ws.Cells["C1:D1"].Style.Font.Size = 14;
                ws.Cells["C1:D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

                ws.Cells["C2:D2"].Merge = true;
                ws.Cells["C2:D2"].Value = "ASOPASCUALINOS";
                ws.Cells["C2:D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                ws.Cells["C2:D2"].Style.Font.Bold = true;
                ws.Cells["C2:D2"].Style.Font.Size = 12;

               
                ws.Cells["C4"].Value = "Fecha del Informe:";
                ws.Cells["C4"].Style.Font.Size = 10;
                ws.Cells["C4"].Style.Font.Bold = true;

                ws.Cells["D" + 4].Value = FechaString;
                ws.Cells["D" + 4].Style.Font.Size = 10;
                ws.Cells["D" + 4].Style.Font.Bold = true;

                ws.Cells["A6"].Value = "No. CUENTA";
                ws.Cells["A6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                ws.Cells["A6"].Style.Font.Bold = true;
                ws.Cells["A6"].Style.Font.Size = 10;

                ws.Cells["B6"].Value = "NOMBRE AFILIADO";
                ws.Cells["B6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                ws.Cells["B6"].Style.Font.Bold = true;
                ws.Cells["B6"].Style.Font.Size = 10;

                ws.Cells["C6"].Value = "TOTAL APORTES";
                ws.Cells["C6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                ws.Cells["C6"].Style.Font.Bold = true;
                ws.Cells["C6"].Style.Font.Size = 10;

                ws.Cells["D6"].Value = "FECHA DE AFILIACION";
                ws.Cells["D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                ws.Cells["D6"].Style.Font.Bold = true;
                ws.Cells["D6"].Style.Font.Size = 10;

                ws.Cells["E6"].Value = "ASESOR";
                ws.Cells["E6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                ws.Cells["E6"].Style.Font.Bold = true;
                ws.Cells["E6"].Style.Font.Size = 10;

                ws.Cells["F6"].Value = " -- ESTADO -- ";
                ws.Cells["F6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                ws.Cells["F6"].Style.Font.Bold = true;
                ws.Cells["F6"].Style.Font.Size = 10;

               

                int j = 7;

              
                foreach (var item in Productos)
                {
                    var EstadoAfi = "";
                    if (item.Estado == true)
                    {
                        EstadoAfi = "Activo";
                    }
                    else
                    {
                        EstadoAfi = "Inactivo";
                    }

                    var NombreAfi = "";
                    if (item.idPersona != "")
                    {
                        var IdAso = item.idPersona;
                       
                        var contextoFinansoft = new AccountingContext();
                        var nombre = contextoFinansoft.Terceros.Where(t => t.NIT == IdAso).FirstOrDefault();
                        if (nombre != null)
                        {
                            NombreAfi = nombre.NOMBRE1 + " " + nombre.NOMBRE2 + " " + nombre.APELLIDO1 + " " + nombre.APELLIDO2;
                        }
                    }
                      
                    ws.Cells["A" + j].Value = item.NumeroCuenta;
                    ws.Cells["B" + j].Value = NombreAfi; 
                    ws.Cells["C" + j].Value = item.totalAportesEx;
                    ws.Cells["C" + j].Style.Numberformat.Format = "$#,##0.00";
                    ws.Cells["D" + j].Value = item.FechaApertura;
                    ws.Cells["D" + j].Style.Numberformat.Format = "yyyy-mm-dd";
                    ws.Cells["E" + j].Value = item.asesor;
                    ws.Cells["F" + j].Value = EstadoAfi; 

                    j++;
                }

                j += 2;

                var TOTALENTRADAS = Productos.Where(x => x.Estado == true).Select(x => x.totalAportesEx).Sum();

                ws.Cells["B" + j].Value = "TOTALES APORTES AFILIADOS";
                ws.Cells["B" + j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["B" + j].Style.Font.Bold = true;
                ws.Cells["B" + j].Style.Font.Size = 10;


                ws.Cells["C" + j].Value = TOTALENTRADAS;
                ws.Cells["C" + j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["C" + j].Style.Font.Bold = true;
                ws.Cells["C" + j].Style.Font.Size = 10;
                ws.Cells["C" + j].Style.Numberformat.Format = "$#,##0.00";


                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                var ms = new System.IO.MemoryStream();
                pack.SaveAs(ms);
                ms.WriteTo(Response.OutputStream);
            }
            Response.End();

            return RedirectToAction("../AportesExtra/AporteExtra");
        }

    }
}

