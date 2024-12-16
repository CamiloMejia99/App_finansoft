using FNTC.Finansoft.Accounting.BLL.ProcesosCrediticios;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FNTC.Finansoft.UI.Areas.Creditos.Controllers
{
    
    public class ProcesosCrediticiosController : Controller
    {
        NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;
        [AllowAnonymous]
        public void RealizarCausacion()
        {
            var respuesta = new  ProcesoCrediticioBLL().RealizarCausacion();
            
        }

        [Authorize]
        public JsonResult GetCuotaActual(string Pagare)
        {
            var respuesta = new ProcesoCrediticioBLL().GetCuotaActual(Pagare);

            return respuesta;
            
        }

        [Authorize]
        public JsonResult Pago(string Pagare, string Opcion, string ValorRecibido, string FormaPago,string FechaPago,string NumFactura)
        {
            string UsuarioActual = User.Identity.Name;
            var respuesta = new ProcesoCrediticioBLL().Pago(Pagare,Opcion,ValorRecibido,UsuarioActual,FormaPago,FechaPago,NumFactura);
            return respuesta;
        }

        [Authorize]
        public JsonResult Abono(string Pagare,string ValorConsignado, string ValorRecibido, string FormaPago, string FechaPago,string NumFactura)
        {
            string UsuarioActual = User.Identity.Name;
            var respuesta = new ProcesoCrediticioBLL().Abono(Pagare,ValorConsignado, ValorRecibido, UsuarioActual,FormaPago,FechaPago,NumFactura);
            return respuesta;
        }

        [Authorize]
        public JsonResult ValidarValores(string ValorPagar, string ValorRecibido)
        {
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            decimal VP = 0,VR=0,Cambio=0;

            VP = (ValorPagar != null && ValorPagar != "") ? Convert.ToDecimal(ValorPagar.Replace(".", "")) : 0;
            VR = (ValorRecibido != null && ValorRecibido != "") ? Convert.ToDecimal(ValorRecibido.Replace(".", "")) : 0;

            if((VR-VP)>=0)
            {
                Cambio = VR - VP;
                var C = Cambio.ToString("N0", formato);
                return new JsonResult { Data = new { status = true,Cambio=C } };
            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }
            
        }

        [Authorize]
        public JsonResult VerificaValorAbono(string Pagare, string ValorConsignado)
        {
            var respuesta = new ProcesoCrediticioBLL().VerificaValorAbono(Pagare, ValorConsignado);
            return respuesta;
        }

        [Authorize]
        public ActionResult FacturaExcel(int Id)
        {
            //imprime la factura en Excel
            using(var ctx = new AccountingContext())
            {
                var Factura = ctx.factOpCajaConsCuotaCredito.Find(Id);
                if(Factura!=null)
                {
                    Response.Clear();
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.Buffer = true;
                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=" + Factura.pagare + ".xlsx");

                    using (ExcelPackage pack = new ExcelPackage())
                    {
                        ExcelWorksheet ws = pack.Workbook.Worksheets.Add(Factura.pagare);

                        ws.Cells["A" + 1].Value = "FECHA";
                        ws.Cells["B" + 1].Value = "NOMBRE";
                        ws.Cells["C" + 1].Value = "NIT";
                        ws.Cells["D" + 1].Value = "PAGARE";
                        ws.Cells["E" + 1].Value = "FORMA PAGO";
                        ws.Cells["F" + 1].Value = "VALOR CONSIGNADO";
                        ws.Cells["G" + 1].Value = "ABONO CAPITAL";
                        ws.Cells["H" + 1].Value = "INTERES CORRIENTE";
                        ws.Cells["I" + 1].Value = "INTERES MORA";
                        ws.Cells["J" + 1].Value = "SEGURO";
                        ws.Cells["K" + 1].Value = "ADMINISTRACIÓN";
                        ws.Cells["L" + 1].Value = "SALDO CAPITAL";

                        ws.Cells["A" + 2].Value = Factura.fecha.ToString("dd/MM/yyyy");
                        ws.Cells["B" + 2].Value = (Factura.Terceros!=null) ? Factura.Terceros.NombreComercial+" "+Factura.Terceros.APELLIDO1 + " " + Factura.Terceros.APELLIDO2 + " " + Factura.Terceros.NOMBRE1 + " " + Factura.Terceros.NOMBRE2 : "";
                        ws.Cells["C" + 2].Value = Factura.NIT;
                        ws.Cells["D" + 2].Value = Factura.pagare;
                        ws.Cells["E" + 2].Value = Factura.FormaPago;
                        ws.Cells["F" + 2].Value = Factura.valorConsignado;
                        ws.Cells["G" + 2].Value = Factura.abonoCapital;
                        ws.Cells["H" + 2].Value = Factura.interesCorriente;
                        ws.Cells["I" + 2].Value = Factura.interesMora;
                        ws.Cells["J" + 2].Value = Factura.seguros;
                        ws.Cells["K" + 2].Value = Factura.CtoAdmon;
                        ws.Cells["L" + 2].Value = Factura.saldoCapital;

                        ws.Cells[ws.Dimension.Address].AutoFitColumns();
                        var ms = new System.IO.MemoryStream();
                        pack.SaveAs(ms);
                        ms.WriteTo(Response.OutputStream);
                    }
                    Response.End();

                }
                
            }

            return RedirectToAction("../InformesCartera/Index?="+Id);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ModificarValoresCredito()
        {
            var Pagares = ConsultarPagares().ToList().Select(p => new SelectListItem { Text = p.Pagare, Value = p.Pagare, Selected = false });
            ViewBag.Pagares = Pagares;
            return View();
        }

        [Authorize]
        public List<TotalesCreditos> ConsultarPagares()
        {
            var respuesta = new ProcesoCrediticioBLL().ConsultarPagares();
            return respuesta;
        }
        
        [Authorize]
        public JsonResult GetCuotasCredito(string Pagare)
        {
            var respuesta = new ProcesoCrediticioBLL().GetCuotasCredito(Pagare);
            return respuesta;
        }

        [Authorize]
        public JsonResult CalcularIM(int Id,int DiasMora)
        {
            var respuesta = new ProcesoCrediticioBLL().CalcularIM(Id, DiasMora);
            return respuesta;
        }

        [Authorize(Roles ="Admin")]
        public JsonResult GuardarValores(int Id, string IC, string IM,string seguro, string admon)
        {
            var respuesta = new ProcesoCrediticioBLL().GuardarValores(Id,IC,IM,seguro,admon);
            return respuesta;
        }

    }
}