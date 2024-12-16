using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.DescuentosNomina;
using FNTC.Finansoft.Accounting.BLL.DescuentosNomina;
using System.Data;
using Rotativa;
using OfficeOpenXml;
using Newtonsoft.Json;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using System.Globalization;
using OfficeOpenXml.Style;
using System.Drawing;
using SpreadsheetLight;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace FNTC.Finansoft.UI.Areas.DescuentosNomina.Controllers
{
    [Authorize]
    public class GestionPlanosController : Controller
    {
        private AccountingContext db = new AccountingContext();
        public ActionResult ListaDeEmpresas()
        {

            return View(db.RelacionPlanosEmpresa.Where(a => a.CodigoPlano == 0 && a.EstadoRelacionPlanosEmpresa == true).ToList());
        }
        public ActionResult ListaPlanosEmpresa(int idRelacion)
        {

            var Empresa = (from s in db.RelacionPlanosEmpresa where s.IdRelacionPlanosEmpresa == idRelacion select s.CodigoEmpresa).FirstOrDefault();
            var NombreEmpresa = (from s in db.Terceros where s.NIT == Empresa select s.NOMBRE).FirstOrDefault();
            ViewBag.NombreEmpresa = NombreEmpresa;
            return View(db.RelacionPlanosEmpresa.Where(a => a.CodigoEmpresa == Empresa && a.CodigoPlano != 0 && a.EstadoRelacionPlanosEmpresa == true).ToList());

        }
        public ActionResult DataRelacionplanosEmpresa(int idRelacion)
        {
            var Plano = (from s in db.RelacionPlanosEmpresa where s.IdRelacionPlanosEmpresa == idRelacion select s.CodigoPlano).FirstOrDefault();
            var Empresa = (from s in db.RelacionPlanosEmpresa where s.IdRelacionPlanosEmpresa == idRelacion select s.CodigoEmpresa).FirstOrDefault();
            var idPlano = (from s in db.RelacionPlanosEmpresa where s.IdRelacionPlanosEmpresa == idRelacion select s.CodigoPlano).FirstOrDefault();
            var NombrePlano = (from s in db.EstructuraPlanos where s.IdEstructuraPlanos == idPlano select s.NombreEstructuraPlanos).FirstOrDefault();
            ViewBag.NombrePlano = NombrePlano;
            ViewBag.Relacion = idRelacion;
            return View(db.RelacionPlanosDiscriminacion.Where(a => a.IdPlano == Plano && a.IdEmpresa == Empresa).ToList());

        }
        public ActionResult DetallesDiscriminacion(int idRelacionPD)
        {
            var datos = (from s in db.RelacionPlanosDiscriminacion where s.IdRelacionPD == idRelacionPD select s).FirstOrDefault();
            var plano = datos.IdPlano;
            var empresa = datos.IdEmpresa;
            var NoDiscriminacion = datos.NoDiscriminacionPlano;
            var periodo = datos.PeriodoDeduccion;
            var idRetorno = (from s in db.RelacionPlanosEmpresa where s.CodigoEmpresa == empresa && s.CodigoPlano == plano select s.IdRelacionPlanosEmpresa).FirstOrDefault();
            ViewBag.idRetorno = idRetorno;
            return View(db.DatosDiscriminacionPlanos.Where(a => a.idPlano == plano && a.NitEmpresa == empresa && a.NoDiscriminacion == NoDiscriminacion && a.PeriodoDeduccion == periodo).ToList());

        }
        public ActionResult GenerarDatosPlano(int idRelacion)
        {
            var DataRelacion = (from s in db.RelacionPlanosEmpresa where s.IdRelacionPlanosEmpresa == idRelacion select s).FirstOrDefault();
            var codigoEmpresa = DataRelacion.CodigoEmpresa;
            ViewBag.NombreEmpresa = (from s in db.Terceros where s.NIT == codigoEmpresa select s).FirstOrDefault().NOMBRE;
            var codigoPlano = DataRelacion.CodigoPlano;
            ViewBag.NombrePlano = (from s in db.EstructuraPlanos where s.IdEstructuraPlanos == codigoPlano select s).FirstOrDefault().NombreEstructuraPlanos;
            ViewBag.Relacion = idRelacion;
            return View();
        }
        [HttpPost]
        public ActionResult GenerarDatosPlano(RelacionPlanosDiscriminacion relacionPlanosDiscriminacion, DatosDiscriminacionPlanos datosDiscriminacionPlanos)
        {
            string Periodo = relacionPlanosDiscriminacion.PeriodoDeduccion.ToString();
            string DateA = DateTime.Now.ToString("yyyy");
            string UnionPeriodo = Periodo + ":" + DateA;

            var DataRelacion = (from s in db.RelacionPlanosEmpresa where s.IdRelacionPlanosEmpresa == relacionPlanosDiscriminacion.IdRelacionEmpresa select s).FirstOrDefault();
            var PlanoID = DataRelacion.CodigoPlano;
            var EmpresaID = DataRelacion.CodigoEmpresa;

            string CurrentDate = "01/01/0001 12:00:00 a. m.";

            DateTime DateObject = Convert.ToDateTime(CurrentDate);

            try
            {

                using (var ctx = new AccountingContext())
                {
                    if ((from s in ctx.RelacionPlanosDiscriminacion where s.IdPlano == PlanoID && s.IdEmpresa == EmpresaID && s.PeriodoDeduccion == UnionPeriodo select s).Count() != 0)
                    {
                        return Json(new { ok = false, msg = "PeriodoYaSeleccionado" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (relacionPlanosDiscriminacion.FechaInicial == DateObject && relacionPlanosDiscriminacion.FechaFinal == DateObject)
                        {
                            var respuesta = new GestionConfiguracionBLL().AgregarNuevaDiscriminacion(relacionPlanosDiscriminacion, datosDiscriminacionPlanos);
                            if (respuesta == true)
                            {
                                return Json(new { ok = true, toRedirect = Url.Action("DataRelacionplanosEmpresa", "GestionPlanos", new { idRelacion = relacionPlanosDiscriminacion.IdRelacionEmpresa }) }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { ok = false, msg = "ErrorAlGuardar" }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            if (relacionPlanosDiscriminacion.FechaInicial == DateObject && relacionPlanosDiscriminacion.FechaFinal != DateObject)
                            {
                                return Json(new { ok = false, msg = "InicialObligatorio" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {

                                if (relacionPlanosDiscriminacion.FechaInicial != DateObject && relacionPlanosDiscriminacion.FechaFinal == DateObject)
                                {
                                    var respuesta = new GestionConfiguracionBLL().AgregarNuevaDiscriminacion(relacionPlanosDiscriminacion, datosDiscriminacionPlanos);
                                    if (respuesta == true)
                                    {
                                        return Json(new { ok = true, toRedirect = Url.Action("DataRelacionplanosEmpresa", "GestionPlanos", new { idRelacion = relacionPlanosDiscriminacion.IdRelacionEmpresa }) }, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        return Json(new { ok = false, msg = "ErrorAlGuardar" }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                else
                                {
                                    if (relacionPlanosDiscriminacion.FechaInicial != DateObject && relacionPlanosDiscriminacion.FechaFinal != DateObject)
                                    {
                                        if (relacionPlanosDiscriminacion.FechaInicial <= relacionPlanosDiscriminacion.FechaFinal)
                                        {
                                            var respuesta = new GestionConfiguracionBLL().AgregarNuevaDiscriminacion(relacionPlanosDiscriminacion, datosDiscriminacionPlanos);
                                            if (respuesta == true)
                                            {
                                                return Json(new { ok = true, toRedirect = Url.Action("DataRelacionplanosEmpresa", "GestionPlanos", new { idRelacion = relacionPlanosDiscriminacion.IdRelacionEmpresa }) }, JsonRequestBehavior.AllowGet);
                                            }
                                            else
                                            {
                                                return Json(new { ok = false, msg = "ErrorAlGuardar" }, JsonRequestBehavior.AllowGet);
                                            }
                                        }
                                        else
                                        {
                                            return Json(new { ok = false, msg = "fechainicialmayor" }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else
                                    {
                                        var respuesta = new GestionConfiguracionBLL().AgregarNuevaDiscriminacion(relacionPlanosDiscriminacion, datosDiscriminacionPlanos);
                                        if (respuesta == true)
                                        {
                                            return Json(new { ok = true, toRedirect = Url.Action("DataRelacionplanosEmpresa", "GestionPlanos", new { idRelacion = relacionPlanosDiscriminacion.IdRelacionEmpresa }) }, JsonRequestBehavior.AllowGet);
                                        }
                                        else
                                        {
                                            return Json(new { ok = false, msg = "ErrorAlGuardar" }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex + "Error al Guardar" }, JsonRequestBehavior.AllowGet);

            }

        }
        public ActionResult EditarDetallesAsociados(int IdDis)
        {
            var DataInter = (from s in db.DatosDiscriminacionPlanos where s.IdDisPlanos == IdDis select s).FirstOrDefault();
            var RelacionInt = (from s in db.RelacionPlanosDiscriminacion where s.IdPlano == DataInter.idPlano && s.NoDiscriminacionPlano == DataInter.NoDiscriminacion && s.IdRelacionEmpresa == DataInter.idEmpresaRelacion && s.PeriodoDeduccion == DataInter.PeriodoDeduccion select s).FirstOrDefault();
            ViewBag.IdRelacionPDIn = RelacionInt.IdRelacionPD;
            try
            {
                using (var db = new AccountingContext())
                {
                    DatosDiscriminacionPlanos datosDiscriminacionPlanos = db.DatosDiscriminacionPlanos.Where(a => a.IdDisPlanos == IdDis).FirstOrDefault();
                    return View(datosDiscriminacionPlanos);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult EditarDetallesAsociados(DatosDiscriminacionPlanos datosDiscriminacionPlanos, ControlDeMovimientos controlDeMovimientos)
        {
            try
            {

                using (var ctx = new AccountingContext())
                {

                    var respuesta = new GestionConfiguracionBLL().EditarDetallesAso(datosDiscriminacionPlanos, controlDeMovimientos);
                    if (respuesta == true)
                    {
                        return Json(new { ok = true, toRedirect = Url.Action("DetallesDiscriminacion", "GestionPlanos", new { idRelacionPD = datosDiscriminacionPlanos.idEmpresaRelacion }) }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { ok = false, msg = "ErrorAlGuardar" }, JsonRequestBehavior.AllowGet);
                    }

                }


            }
            catch (Exception ex)
            {
                return Json(new { ok = false, msg = ex + "Error al Guardar" }, JsonRequestBehavior.AllowGet);

            }

        }
        public ActionResult DetallesInfo(int IdDis)
        {
            var DataInter = (from s in db.DatosDiscriminacionPlanos where s.IdDisPlanos == IdDis select s).FirstOrDefault();
            var RelacionInt = (from s in db.RelacionPlanosDiscriminacion where s.IdPlano == DataInter.idPlano && s.NoDiscriminacionPlano == DataInter.NoDiscriminacion && s.IdRelacionEmpresa == DataInter.idEmpresaRelacion && s.PeriodoDeduccion == DataInter.PeriodoDeduccion select s).FirstOrDefault();
            ViewBag.plano = (from s in db.EstructuraPlanos where s.IdEstructuraPlanos == DataInter.idPlano select s).FirstOrDefault().NombreEstructuraPlanos;
            ViewBag.empresa = (from s in db.Terceros where s.NIT == DataInter.NitEmpresa select s).FirstOrDefault().NOMBRE;
            ViewBag.IdRelacionPDIn = RelacionInt.IdRelacionPD;
            try
            {
                using (var db = new AccountingContext())
                {
                    DatosDiscriminacionPlanos datosDiscriminacionPlanos = db.DatosDiscriminacionPlanos.Where(a => a.IdDisPlanos == IdDis).FirstOrDefault();
                    return View(datosDiscriminacionPlanos);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        //----------------------------------------------- Exportar -------------------------------------------------
        #region Exportar

        //public ActionResult ExportarInfo(int IdRelacionPlanoEmpresaDis)
        //{

        //    var idPlanoI = db.Discriminacion.Where(p => p.ID == ID).Select(p => p.PLANO).FirstOrDefault();
        //    var idPlano = idPlanoI.ToString();
        //    var idEmpresaI = db.Discriminacion.Where(p => p.ID == ID).Select(p => p.EMPRESA).FirstOrDefault();
        //    var idEmpresa = idEmpresaI.ToString();

        //    Response.Clear();
        //    Response.ClearContent();
        //    Response.ClearHeaders();
        //    Response.Buffer = true;
        //    Response.ContentEncoding = System.Text.Encoding.UTF8;
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //    Response.AddHeader("content-disposition", "attachment;filename=Reporte.xlsx");

        //    using (ExcelPackage pack = new ExcelPackage())
        //    {
        //        int IDPLANO = Int32.Parse(idPlano);
        //        List<string> camps = db.ArchivoPlano.Where(p => p.CLASEPLANO == IDPLANO).OrderBy(p => p.ORDEN).Select(p => p.TipoDeCampo.DESCRIPCION).ToList();
        //        List<string> Encabezados = db.ArchivoPlano.Where(p => p.CLASEPLANO == IDPLANO).OrderBy(p => p.ORDEN).Select(p => p.CONCEPTO).ToList();

        //        var consulta = (from pc in db.ConsolidadoNomina where pc.EMPRESA.ToString() == idEmpresa select pc).ToList();


        //        ExcelWorksheet ws = pack.Workbook.Worksheets.Add("ArchivoPlano");
        //        int contar = camps.Count();

        //        if (contar == 6)
        //        {
        //            ws.Cells["A" + 1].Value = Encabezados[0];
        //            ws.Cells["B" + 1].Value = Encabezados[1];
        //            ws.Cells["C" + 1].Value = Encabezados[2];
        //            ws.Cells["D" + 1].Value = Encabezados[3];
        //            ws.Cells["E" + 1].Value = Encabezados[4];
        //            ws.Cells["F" + 1].Value = "Codconcepto";
        //            ws.Cells["G" + 1].Value = "FECHA INICIAL";
        //            ws.Cells["H" + 1].Value = "FECHA FINAL";
        //            ws.Cells["I" + 1].Value = Encabezados[5];
        //            ws.Cells["J" + 1].Value = "Valor Total";
        //            ws.Cells["K" + 1].Value = "Valor acumulado";
        //            ws.Cells["L" + 1].Value = "Tipo de acto - Libranza";
        //            ws.Cells["M" + 1].Value = "Fecha de Acto";
        //            ws.Cells["N" + 1].Value = "Número de Acto ú Obligacion";
        //            ws.Cells["O" + 1].Value = "Observaciones";

        //            ws.Cells["A" + 1].Style.Font.Bold = true;
        //            ws.Cells["B" + 1].Style.Font.Bold = true;
        //            ws.Cells["C" + 1].Style.Font.Bold = true;
        //            ws.Cells["D" + 1].Style.Font.Bold = true;
        //            ws.Cells["E" + 1].Style.Font.Bold = true;
        //            ws.Cells["F" + 1].Style.Font.Bold = true;
        //            ws.Cells["G" + 1].Style.Font.Bold = true;
        //            ws.Cells["H" + 1].Style.Font.Bold = true;
        //            ws.Cells["I" + 1].Style.Font.Bold = true;
        //            ws.Cells["J" + 1].Style.Font.Bold = true;
        //            ws.Cells["K" + 1].Style.Font.Bold = true;
        //            ws.Cells["L" + 1].Style.Font.Bold = true;
        //            ws.Cells["M" + 1].Style.Font.Bold = true;
        //            ws.Cells["N" + 1].Style.Font.Bold = true;
        //            ws.Cells["O" + 1].Style.Font.Bold = true;

        //            int i = 2;

        //            foreach (var item in consulta)
        //            {
        //                string c1 = camps[0];
        //                char[] charsToTrim = { '"', ' ', '\'' };
        //                string banner = c1;
        //                string result = banner.Trim(charsToTrim);
        //                Console.WriteLine("Trimmmed\n   {0}\nto\n   '{1}'", banner, result);


        //                var campos = String.Join(camps[0], "item." + camps[0]);
        //                var array = camps.ToArray();

        //                String option = camps[0];
        //                dynamic data0 = "";
        //                switch (option)
        //                {
        //                    case "NITEMPRESA":
        //                        data0 = item.NITEMPRESA;
        //                        break;
        //                    case "DigitoVerificacion":
        //                        data0 = item.DigitoVerificacion;
        //                        break;
        //                    case "TipoDeEstado":
        //                        data0 = item.TipoDeEstado;
        //                        break;
        //                    case "idPersona":
        //                        data0 = item.idPersona;
        //                        break;
        //                    case "NOMBRE1":
        //                        data0 = item.NOMBRE1;
        //                        break;
        //                    case "NOMBRE2":
        //                        data0 = item.NOMBRE2;
        //                        break;
        //                    case "APELLIDO1":
        //                        data0 = item.APELLIDO1;
        //                        break;
        //                    case "NumeroCuenta":
        //                        data0 = item.NumeroCuenta;
        //                        break;
        //                    case "totalAportes":
        //                        data0 = item.totalAportes;
        //                        break;
        //                    case "NombreCompleto":
        //                        data0 = item.NombreCompleto;
        //                        break;

        //                    default:
        //                        break;
        //                }

        //                String option1 = camps[1];
        //                dynamic data1 = "";
        //                switch (option1)
        //                {
        //                    case "NITEMPRESA":
        //                        data1 = item.NITEMPRESA;
        //                        break;
        //                    case "DigitoVerificacion":
        //                        data1 = item.DigitoVerificacion;
        //                        break;
        //                    case "TipoDeEstado":
        //                        data1 = item.TipoDeEstado;
        //                        break;
        //                    case "idPersona":
        //                        data1 = item.idPersona;
        //                        break;
        //                    case "NOMBRE1":
        //                        data1 = item.NOMBRE1;
        //                        break;
        //                    case "NOMBRE2":
        //                        data1 = item.NOMBRE2;
        //                        break;
        //                    case "APELLIDO1":
        //                        data1 = item.APELLIDO1;
        //                        break;
        //                    case "NumeroCuenta":
        //                        data1 = item.NumeroCuenta;
        //                        break;
        //                    case "totalAportes":
        //                        data1 = item.totalAportes;
        //                        break;
        //                    case "NombreCompleto":
        //                        data1 = item.NombreCompleto;
        //                        break;

        //                    default:
        //                        break;
        //                }

        //                String option2 = camps[2];
        //                dynamic data2 = "";
        //                switch (option2)
        //                {
        //                    case "NITEMPRESA":
        //                        data2 = item.NITEMPRESA;
        //                        break;
        //                    case "DigitoVerificacion":
        //                        data2 = item.DigitoVerificacion;
        //                        break;
        //                    case "TipoDeEstado":
        //                        data2 = item.TipoDeEstado;
        //                        break;
        //                    case "idPersona":
        //                        data2 = item.idPersona;
        //                        break;
        //                    case "NOMBRE1":
        //                        data2 = item.NOMBRE1;
        //                        break;
        //                    case "NOMBRE2":
        //                        data2 = item.NOMBRE2;
        //                        break;
        //                    case "APELLIDO1":
        //                        data2 = item.APELLIDO1;
        //                        break;
        //                    case "NumeroCuenta":
        //                        data2 = item.NumeroCuenta;
        //                        break;
        //                    case "totalAportes":
        //                        data2 = item.totalAportes;
        //                        break;
        //                    case "NombreCompleto":
        //                        data2 = item.NombreCompleto;
        //                        break;

        //                    default:
        //                        break;
        //                }

        //                String option3 = camps[3];
        //                dynamic data3 = "";
        //                switch (option3)
        //                {
        //                    case "NITEMPRESA":
        //                        data3 = item.NITEMPRESA;
        //                        break;
        //                    case "DigitoVerificacion":
        //                        data3 = item.DigitoVerificacion;
        //                        break;
        //                    case "TipoDeEstado":
        //                        data3 = item.TipoDeEstado;
        //                        break;
        //                    case "idPersona":
        //                        data3 = item.idPersona;
        //                        break;
        //                    case "NOMBRE1":
        //                        data3 = item.NOMBRE1;
        //                        break;
        //                    case "NOMBRE2":
        //                        data3 = item.NOMBRE2;
        //                        break;
        //                    case "APELLIDO1":
        //                        data3 = item.APELLIDO1;
        //                        break;
        //                    case "NumeroCuenta":
        //                        data3 = item.NumeroCuenta;
        //                        break;
        //                    case "totalAportes":
        //                        data3 = item.totalAportes;
        //                        break;
        //                    case "NombreCompleto":
        //                        data3 = item.NombreCompleto;
        //                        break;

        //                    default:
        //                        break;
        //                }

        //                String option4 = camps[4];
        //                dynamic data4 = "";
        //                switch (option4)
        //                {
        //                    case "NITEMPRESA":
        //                        data4 = item.NITEMPRESA;
        //                        break;
        //                    case "DigitoVerificacion":
        //                        data4 = item.DigitoVerificacion;
        //                        break;
        //                    case "TipoDeEstado":
        //                        data4 = item.TipoDeEstado;
        //                        break;
        //                    case "idPersona":
        //                        data4 = item.idPersona;
        //                        break;
        //                    case "NOMBRE1":
        //                        data4 = item.NOMBRE1;
        //                        break;
        //                    case "NOMBRE2":
        //                        data4 = item.NOMBRE2;
        //                        break;
        //                    case "APELLIDO1":
        //                        data4 = item.APELLIDO1;
        //                        break;
        //                    case "NumeroCuenta":
        //                        data4 = item.NumeroCuenta;
        //                        break;
        //                    case "totalAportes":
        //                        data4 = item.totalAportes;
        //                        break;
        //                    case "NombreCompleto":
        //                        data4 = item.NombreCompleto;
        //                        break;

        //                    default:
        //                        break;
        //                }

        //                String option5 = camps[5];
        //                dynamic data5 = "";
        //                switch (option5)
        //                {
        //                    case "NITEMPRESA":
        //                        data5 = item.NITEMPRESA;
        //                        break;
        //                    case "DigitoVerificacion":
        //                        data5 = item.DigitoVerificacion;
        //                        break;
        //                    case "TipoDeEstado":
        //                        data5 = item.TipoDeEstado;
        //                        break;
        //                    case "idPersona":
        //                        data5 = item.idPersona;
        //                        break;
        //                    case "NOMBRE1":
        //                        data5 = item.NOMBRE1;
        //                        break;
        //                    case "NOMBRE2":
        //                        data5 = item.NOMBRE2;
        //                        break;
        //                    case "APELLIDO1":
        //                        data5 = item.APELLIDO1;
        //                        break;
        //                    case "NumeroCuenta":
        //                        data5 = item.NumeroCuenta;
        //                        break;
        //                    case "totalAportes":
        //                        data5 = item.totalAportes;
        //                        break;
        //                    case "NombreCompleto":
        //                        data5 = item.NombreCompleto;
        //                        break;

        //                    default:
        //                        break;
        //                }



        //                ws.Cells["A" + i].Value = data0;
        //                ws.Cells["B" + i].Value = data1;
        //                ws.Cells["C" + i].Value = data2;
        //                ws.Cells["D" + i].Value = data3;
        //                ws.Cells["E" + i].Value = data4;
        //                ws.Cells["F" + i].Value = "";
        //                ws.Cells["G" + i].Value = "20/01/2022";
        //                ws.Cells["H" + i].Value = "";
        //                ws.Cells["I" + i].Value = data5;

        //                i++;
        //            }

        //        }


        //        var ms = new System.IO.MemoryStream();
        //        pack.SaveAs(ms);
        //        ms.WriteTo(Response.OutputStream);

        //    }
        //    Response.End();
        //    return RedirectToAction("../Informes/Index");


        //}
        public ActionResult ExportarInfo(int IdRelacionPlanoEmpresaDis)
        {
            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;

            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";

            DateTime FechaActual = DateTime.Now;
            string FechaString = FechaActual.ToString("yyyy-MM-dd");

            var DataInter = (from s in db.RelacionPlanosDiscriminacion where s.IdRelacionPD == IdRelacionPlanoEmpresaDis select s).FirstOrDefault();


            var NombrePlano = (from s in db.EstructuraPlanos where s.IdEstructuraPlanos == DataInter.IdPlano select s).FirstOrDefault().NombreEstructuraPlanos;
            var NombreEmpresa = (from s in db.Terceros where s.NIT == DataInter.IdEmpresa select s).FirstOrDefault().NOMBRE;

            var Datos = db.DatosDiscriminacionPlanos.Where(x => x.idPlano == DataInter.IdPlano && x.NoDiscriminacion == DataInter.NoDiscriminacionPlano && x.PeriodoDeduccion == DataInter.PeriodoDeduccion && x.idEmpresaRelacion == DataInter.IdRelacionEmpresa).ToList();


            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=" + "Plano/Empresa: " + NombrePlano + "/" + NombreEmpresa + ".xlsx");

            using (ExcelPackage pack = new ExcelPackage())
            {
                ExcelWorksheet ws = pack.Workbook.Worksheets.Add("Plano");

                ws.Cells["C1:D1"].Merge = true;
                ws.Cells["C1:D1"].Value = "DESCUENTOS DE NOMINA";
                ws.Cells["C1:D1"].Style.Font.Bold = true;
                ws.Cells["C1:D1"].Style.Font.Size = 14;
                ws.Cells["C1:D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;

                ws.Cells["C2:D2"].Merge = true;
                ws.Cells["C2:D2"].Value = NombreEmpresa;
                ws.Cells["C2:D2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                ws.Cells["C2:D2"].Style.Font.Bold = true;
                ws.Cells["C2:D2"].Style.Font.Size = 12;

                ws.Cells["C3:D3"].Merge = true;
                ws.Cells["C3:D3"].Value = "Plano: " + NombrePlano;
                ws.Cells["C3:D3"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                ws.Cells["C3:D3"].Style.Font.Bold = true;
                ws.Cells["C3:D3"].Style.Font.Size = 12;

                ws.Cells["C4"].Value = "Fecha De Descarga:";
                ws.Cells["C4"].Style.Font.Size = 10;
                ws.Cells["C4"].Style.Font.Bold = true;

                ws.Cells["D" + 4].Value = FechaString;
                ws.Cells["D" + 4].Style.Font.Size = 10;
                ws.Cells["D" + 4].Style.Font.Bold = true;

                ws.Cells["C5:D5"].Merge = true;
                ws.Cells["C5:D5"].Value = DataInter.Identificador;
                ws.Cells["C5:D5"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                ws.Cells["C5:D5"].Style.Font.Bold = true;
                ws.Cells["C5:D5"].Style.Font.Size = 8;

                //____________________________________________________________________________________________//

                ws.Cells["A6"].Value = "Nit Empresa";
                ws.Cells["A6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                ws.Cells["A6"].Style.Font.Bold = true;
                ws.Cells["A6"].Style.Font.Size = 10;

                ws.Cells["B6"].Value = "Digito verificación";
                ws.Cells["B6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                ws.Cells["B6"].Style.Font.Bold = true;
                ws.Cells["B6"].Style.Font.Size = 10;

                ws.Cells["C6"].Value = "Tipo";
                ws.Cells["C6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                ws.Cells["C6"].Style.Font.Bold = true;
                ws.Cells["C6"].Style.Font.Size = 10;

                ws.Cells["D6"].Value = "Cedula Asociado";
                ws.Cells["D6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                ws.Cells["D6"].Style.Font.Bold = true;
                ws.Cells["D6"].Style.Font.Size = 10; 

                ws.Cells["E6"].Value = "Codconcepto";
                ws.Cells["E6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                ws.Cells["E6"].Style.Font.Bold = true;
                ws.Cells["E6"].Style.Font.Size = 10;

                ws.Cells["F6"].Value = "Fecha Inicial";
                ws.Cells["F6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                ws.Cells["F6"].Style.Font.Bold = true;
                ws.Cells["F6"].Style.Font.Size = 10;

                ws.Cells["G6"].Value = "Fecha Final";
                ws.Cells["G6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                ws.Cells["G6"].Style.Font.Bold = true;
                ws.Cells["G6"].Style.Font.Size = 10;

                ws.Cells["H6"].Value = "Valor Novedad";
                ws.Cells["H6"].Style.HorizontalAlignment = ExcelHorizontalAlignment.CenterContinuous;
                ws.Cells["H6"].Style.Font.Bold = true;
                ws.Cells["H6"].Style.Font.Size = 10;


                int j = 7;


                foreach (var item in Datos)
                {
                    //var EstadoAfi = "";
                    //if (item.Estado == true)
                    //{
                    //    EstadoAfi = "Activo";
                    //}
                    //else
                    //{
                    //    EstadoAfi = "Inactivo";
                    //}

                    //var NombreAfi = "";
                    //if (item.idPersona != "")
                    //{
                    //    var IdAso = item.idPersona;

                    //    var contextoFinansoft = new AccountingContext();
                    //    var nombre = contextoFinansoft.Terceros.Where(t => t.NIT == IdAso).FirstOrDefault();
                    //    if (nombre != null)
                    //    {
                    //        NombreAfi = nombre.NOMBRE1 + " " + nombre.NOMBRE2 + " " + nombre.APELLIDO1 + " " + nombre.APELLIDO2;
                    //    }
                    //}

                    ws.Cells["A" + j].Value = item.NitEmpresa;
                    ws.Cells["B" + j].Value = item.DigitoVerificacion;
                    ws.Cells["C" + j].Value = item.TipoDeEstadoProceso;
                    ws.Cells["D" + j].Value = item.NitAsociado;
                    ws.Cells["E" + j].Value = "FERAJAP";
                    ws.Cells["F" + j].Value = item.FechaInicial;
                    ws.Cells["F" + j].Style.Numberformat.Format = "yyyy-mm-dd";
                    ws.Cells["G" + j].Value = item.FechaFinal;
                    ws.Cells["G" + j].Style.Numberformat.Format = "yyyy-mm-dd";

                    ws.Cells["H" + j].Value = item.TotalAportes;
                    ws.Cells["H" + j].Style.Numberformat.Format = "$#,##0.00";

                    j++;
                }

                j += 1;

                var TOTALENTRADAS = db.DatosDiscriminacionPlanos.Where(x => x.idPlano == DataInter.IdPlano && x.NoDiscriminacion == DataInter.NoDiscriminacionPlano && x.PeriodoDeduccion == DataInter.PeriodoDeduccion && x.idEmpresaRelacion == DataInter.IdRelacionEmpresa).Select(x => x.TotalAportes).Sum();

                ws.Cells["A" + j].Value = "TOTALES";
                ws.Cells["A" + j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                ws.Cells["A" + j].Style.Font.Bold = true;
                ws.Cells["A" + j].Style.Font.Size = 10;


                ws.Cells["H" + j].Value = TOTALENTRADAS;
                ws.Cells["H" + j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Cells["H" + j].Style.Font.Bold = true;
                ws.Cells["H" + j].Style.Font.Size = 10;
                ws.Cells["H" + j].Style.Numberformat.Format = "$#,##0.00";


                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                var ms = new System.IO.MemoryStream();
                pack.SaveAs(ms);
                ms.WriteTo(Response.OutputStream);
            }
            Response.End();

            return RedirectToAction("../GestionPlanos/DataRelacionplanosEmpresa", new { idRelacionPD = IdRelacionPlanoEmpresaDis });
        }
        #endregion

        public ActionResult ImportarPlanos()
        {

            SLDocument sl = new SLDocument(@"C:\Users\Danilo\Documents\ProyectosWork\FerajunapActual\Ferajunap\FNTC.Finansoft.UI\File\DescuentosNomina\Plano_Empresa_ PLANO NO.1_SOFTFINANTEC SAS (6).xlsx");
            SLWorksheetStatistics propiedades = sl.GetWorksheetStatistics();

            int UltimaFila = propiedades.EndRowIndex;

            var DatExcel = (sl.GetCellValueAsString("H6"));

            return View();
        }
    }

}