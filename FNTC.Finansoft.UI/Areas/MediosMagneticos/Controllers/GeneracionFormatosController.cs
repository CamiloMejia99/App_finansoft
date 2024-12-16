using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.MediosMagneticos;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.Informes;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using OfficeOpenXml;
using System.Data.SqlClient;
using System.Globalization;
using System.Data;
using System.Text;
using ClosedXML.Excel;
using System.IO;
using System.Drawing;
using OfficeOpenXml.Style;
using System.Collections;
using FNTC.Finansoft.UI.Tools;

namespace FNTC.Finansoft.UI.Areas.MediosMagneticos.Controllers
{
    public class GeneracionFormatosController : Controller
    {
        private AccountingContext db = new AccountingContext();
        decimal saldoFinal = 0, debito = 0, credito = 0, debito_credito = 0;

        

        public decimal Acumulado(int id)
        {
            switch (id)
            {
                case 1:
                    return debito;    
                case 2:
                    return credito;   
                case 3:
                    return debito_credito;   
                case 4:
                    return saldoFinal;    
            }  
            return 0;
        }

        public string NombrePais(int idpais)
        {
            string nomPais = "";
            switch (idpais)
            {
                case 1:
                    nomPais = "Colombia";
                    break;
                case 2:
                    nomPais = "Ecuador";
                    break;
                case 3:
                    nomPais = "Venezuela";
                    break;
            }
            return nomPais;
        }

        public int CodigoPais(int idpais)
        {
            int codPais = 0;
            switch (idpais)
            {
                case 1:
                    codPais = 169;
                    break;
                case 2:
                    codPais = 239;
                    break;
                case 3:
                    codPais = 850;
                    break;
            }
            return codPais;
        }


        public ActionResult FormatosExcel(int idConfigMedMag, decimal CMenor, FormCollection coll)
        {
            try
            {
                List<InformacionExogena> movimiento = new List<InformacionExogena>();

                var numeroFormato = 0;
                var formato = "";
                var baseConfiguracion = db.ConfigMedMag.Find(idConfigMedMag);
                var cuentaId = baseConfiguracion.CuentaContable; // numero de cuenta, parametro procedimiento almacenado
                var formatoId = baseConfiguracion.formato;
                var conceptoId = baseConfiguracion.concepto;
                var categoriaId = baseConfiguracion.categoria;//numero de categoria
                var acumuladoPorId = baseConfiguracion.acumuladopor;//tipo de acumulado para validacion
                var anioId = baseConfiguracion.anvigente;
                var baseConcepto = db.conceptos.Find(conceptoId);
                var codigoConcepto = baseConcepto.codigoConceptos; //codigo del concepto asociado
                var baseFormato = db.formatos.Find(formatoId);
                var codigoFormato = baseFormato.codigoFormato;// numero del formato
                //datos cuantia menor
                string nitCuantiaMenor = "2222222222";
                string razonmSocial = "Cuantías Menores";
                int tipoDocumentoCM = 43;
                int codDepCm = 52;
                int codMunCM = 52356;
                var digVer = 7;
                //
                var datos = db.Empresa.ToList();
                var nombre = datos.Select(x => x.nombre).FirstOrDefault();
                var nitE = datos.Select(x => x.nit).FirstOrDefault();
                DateTime fechaAct = Fecha.GetFechaColombia();

                movimiento = db.Database.SqlQuery<InformacionExogena>(
                    "dbo.sp_InformacionExogena @anioproceso,@numCuenta",
                    new SqlParameter("@anioproceso", anioId),
                    new SqlParameter("@numCuenta", cuentaId)
                    ).ToList();
                var aux = 0;
                var boton1 = coll["formato"];
                var boton2 = coll["info"];
                if (boton1 != null)
                    aux = 1;
                else if (boton2 != null)
                    aux = 2;
                switch (aux)
                {
                    case 1:
                        if (codigoFormato == 1001)
                        {
                            numeroFormato = 1;
                            formato = "attachment;filename=Formato_1001.xlsx";
                        }
                        else if (codigoFormato == 1003)
                        {
                            numeroFormato = 2;
                            formato = "attachment;filename=Formato_1003.xlsx";
                        }
                        else if (codigoFormato == 1004)
                        {
                            numeroFormato = 3;
                            formato = "attachment;filename=Formato_1004.xlsx";
                        }
                        else if (codigoFormato == 1005)
                        {
                            numeroFormato = 4;
                            formato = "attachment;filename=Formato_1005.xlsx";
                        }
                        else if (codigoFormato == 1006)
                        {
                            numeroFormato = 5;
                            formato = "attachment;filename=Formato_1006.xlsx";
                        }
                        else if (codigoFormato == 1007)
                        {
                            numeroFormato = 6;
                            formato = "attachment;filename=Formato_1007.xlsx";
                        }
                        else if (codigoFormato == 1008)
                        {
                            numeroFormato = 7;
                            formato = "attachment;filename=Formato_1008.xlsx";
                        }
                        else if (codigoFormato == 1009)
                        {
                            numeroFormato = 8;
                            formato = "attachment;filename=Formato_1009.xlsx";
                        }
                        else if (codigoFormato == 1010)
                        {
                            numeroFormato = 9;
                            formato = "attachment;filename=Formato_1010.xlsx";
                        }
                        else if (codigoFormato == 1011)
                        {
                            numeroFormato = 10;
                            formato = "attachment;filename=Formato_1011.xlsx";
                        }
                        else if (codigoFormato == 1012)
                        {
                            numeroFormato = 11;
                            formato = "attachment;filename=Formato_1012.xlsx";
                        }
                        else if (codigoFormato == 1056)
                        {
                            numeroFormato = 12;
                            formato = "attachment;filename=Formato_1056.xlsx";
                        }
                        else if (codigoFormato == 1647)
                        {
                            numeroFormato = 13;
                            formato = "attachment;filename=Formato_1647.xlsx";
                        }
                        else if (codigoFormato == 2275)
                        {
                            numeroFormato = 14;
                            formato = "attachment;filename=Formato_2275.xlsx";
                        }
                        else if (codigoFormato == 2276)
                        {
                            numeroFormato = 15;
                            formato = "attachment;filename=Formato_2276.xlsx";
                        }

                        using (var ctx = new AccountingContext())
                        {
                            Response.Clear();
                            Response.ClearContent();
                            Response.ClearHeaders();
                            Response.Buffer = true;
                            Response.ContentEncoding = System.Text.Encoding.UTF8;
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", formato);

                            using (ExcelPackage pack = new ExcelPackage())
                            {
                                ExcelWorksheet ws = null;
                                int i = 0;
                                var numCuentas = (from m in movimiento
                                                  orderby m.CUENTA
                                                  select new { m.Nit, m.DEBITO, m.CREDITO, m.CUENTA }).ToList();


                                switch (numeroFormato)
                                {
                                    case 1:
                                        #region formato 1001 
                                        ws = pack.Workbook.Worksheets.Add("1001");
                                        //// encabezado
                                        ws.Cells["A1:T1,A2:T2,A3:T3,A4:T4,A5:T5"].Merge = true;
                                        ws.Cells["A2:T2,A3:T3,A4:T4"].Style.Font.Bold = true;
                                        ws.Cells["A2:T2"].Style.Font.Name = "Arial";
                                        ws.Cells["A2:T2"].Style.Font.Size = 14;
                                        ws.Cells["A" + 2].Value = "FORMATO 1001 PAGOS O ABONOS EN CUENTA Y RETENCIONES PRACTICADAS   " + anioId;
                                        ws.Cells[1, 1, 5, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[1, 1, 5, 20].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                                        ws.Cells["A2:T2,A3:T3,A4:T4,A5:T5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                        ws.Cells["A2:T2,A3:T3,A4:T4,A7:T7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells["A2:T2,A3:T3,A4:T4,A7:T7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells["A2:T2,A3:T3,A4:T4,A5:T5"].Style.WrapText = true;
                                        ws.Cells["A3:T3,A4:T4"].Style.Font.Size = 12;
                                        ws.Cells["A5:T5"].Style.Font.Size = 10;
                                        ws.Cells["A" + 3].Value = nombre;
                                        ws.Cells["A" + 4].Value = nitE;
                                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                                        ws.Cells["A6:T6"].Merge = true;
                                        ws.Cells[6, 1, 6, 20].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                                        ws.Cells[7, 1, 7, 20].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                                        ws.Cells["A7:U7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                                        ws.Cells["A7:T7"].Style.Font.Bold = true;
                                        ws.Cells[7, 1, 7, 20].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[7, 1, 7, 20].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                                        ws.Cells["A7:T7"].Style.WrapText = true;
                                        //fin encabezado
                                        ws.Cells["A" + 7].Value = "Concepto";
                                        ws.Cells["B" + 7].Value = "Tipo de documento";
                                        ws.Cells["C" + 7].Value = "Número de identificación del informado";
                                        ws.Cells["D" + 7].Value = "Primer apellido del informado";
                                        ws.Cells["E" + 7].Value = "Segundo apellido  del informado";
                                        ws.Cells["F" + 7].Value = "Primer nombre del informado";
                                        ws.Cells["G" + 7].Value = "Otros nombres  del informado";
                                        ws.Cells["H" + 7].Value = "Razon social informado";
                                        ws.Cells["I" + 7].Value = "Dirección";
                                        ws.Cells["J" + 7].Value = "Codigo dpto.";
                                        ws.Cells["K" + 7].Value = "Codigo mcp";
                                        ws.Cells["L" + 7].Value = "Pais residencia o domicilio";
                                        ws.Cells["M" + 7].Value = "Pago o abono en cuenta deducible";
                                        ws.Cells["N" + 7].Value = "Pago o abono en cuenta NO deducible";
                                        ws.Cells["O" + 7].Value = "Iva mayor valor del costo o gasto deducible";
                                        ws.Cells["P" + 7].Value = "Iva mayor valor del costo o gasto no deducible";
                                        ws.Cells["Q" + 7].Value = "Retención en la fuente practicada en renta";
                                        ws.Cells["R" + 7].Value = "Retención en la fuente asumida en renta";
                                        ws.Cells["S" + 7].Value = "Retención en la fuente practicada IVA régimen común";
                                        ws.Cells["T" + 7].Value = "Retención en la fuente practicada IVA no domiciliados";

                                        i = 8;

                                        var auxNit = (from m in movimiento
                                                      orderby m.CUENTA
                                                      select new { m.Nit, m.CUENTA, m.TIPODOCUMENTO, m.APELLIDO1, m.APELLIDO2, m.NOMBRE1, m.NOMBRE2, m.Dir, m.Dep_muni, m.MUNICIPIO, m.Id_pais, m.NATURALEZA, m.NombreComercial }).Distinct().ToList();

                                        foreach (var item in auxNit)
                                        {
                                            var dataMov2 = numCuentas.Where(x => x.Nit == item.Nit).OrderBy(x => x.Nit).ToList();
                                            debito = dataMov2.Select(x => x.DEBITO).Sum(); //acumulado por debito
                                            credito = dataMov2.Select(x => x.CREDITO).Sum();//acumulado por credito
                                            int startIndex = 0;
                                            int length = 2;
                                            string substring = (item.CUENTA).Substring(startIndex, length);

                                            //acumulado por debito credito       
                                            if (item.NATURALEZA == "D")
                                                debito_credito = debito - credito;
                                            else
                                                debito_credito = credito - debito;

                                            //acumulado por saldo final cuentas por cobrar cuentas 16 y por pagar cuentas 24

                                            if (substring == "16")
                                                saldoFinal = debito - credito;

                                            if (substring == "24")
                                                saldoFinal = credito - debito;

                                            if (Acumulado(acumuladoPorId) > 0)
                                            {
                                                ws.Cells["A" + i].Value = codigoConcepto;
                                                //validacion cuantias menores  
                                                if ((Acumulado(acumuladoPorId)) < 100000)
                                                {
                                                    ws.Cells["B" + i].Value = tipoDocumentoCM;
                                                    ws.Cells["C" + i].Value = nitCuantiaMenor;
                                                    ws.Cells[i, 4, i, 7].Value = "";
                                                    ws.Cells["H" + i].Value = razonmSocial;
                                                    ws.Cells["I" + i].Value = "NA";
                                                    ws.Cells["J" + i].Value = codDepCm;
                                                    ws.Cells["K" + i].Value = codMunCM;
                                                    ws.Cells["L" + i].Value = NombrePais(1);
                                                }
                                                else
                                                {
                                                    ws.Cells["B" + i].Value = item.TIPODOCUMENTO;
                                                    ws.Cells["C" + i].Value = item.Nit;
                                                    ws.Cells["D" + i].Value = item.APELLIDO1;
                                                    ws.Cells["E" + i].Value = item.APELLIDO2;
                                                    ws.Cells["F" + i].Value = item.NOMBRE1;
                                                    ws.Cells["G" + i].Value = item.NOMBRE2;
                                                    ws.Cells["H" + i].Value = item.NombreComercial;
                                                    ws.Cells["I" + i].Value = item.Dir;
                                                    ws.Cells["J" + i].Value = item.Dep_muni;
                                                    ws.Cells["K" + i].Value = item.MUNICIPIO;
                                                    ws.Cells["L" + i].Value = NombrePais(item.Id_pais);
                                                }

                                                if (categoriaId == 82)
                                                {
                                                    ws.Cells["M" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 14, i, 20].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 83)
                                                {
                                                    ws.Cells["M" + i].Value = 0;
                                                    ws.Cells["N" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 15, i, 20].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 84)
                                                {
                                                    ws.Cells[i, 13, i, 14].Value = 0;
                                                    ws.Cells["O" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 16, i, 20].Value = 0;
                                                    i++;
                                                }

                                                else if (categoriaId == 85)
                                                {
                                                    ws.Cells[i, 13, i, 15].Value = 0;
                                                    ws.Cells["P" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 17, i, 20].Value = 0;
                                                    i++;

                                                }
                                                else if (categoriaId == 86)
                                                {
                                                    ws.Cells[i, 13, i, 16].Value = 0;
                                                    ws.Cells["Q" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 18, i, 20].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 87)
                                                {
                                                    ws.Cells[i, 13, i, 17].Value = 0;
                                                    ws.Cells["R" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 19, i, 20].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 88)
                                                {
                                                    ws.Cells[i, 13, i, 18].Value = 0;
                                                    ws.Cells["S" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells["T" + i].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 89)
                                                {
                                                    ws.Cells[i, 13, i, 19].Value = 0;
                                                    ws.Cells["T" + i].Value = Acumulado(acumuladoPorId);
                                                    i++;
                                                }
                                            }

                                        }
                                        if (CMenor > 0 && CMenor < 100000)
                                        {
                                            ws.Cells["A" + i].Value = codigoConcepto;
                                            ws.Cells["B" + i].Value = tipoDocumentoCM;
                                            ws.Cells["C" + i].Value = nitCuantiaMenor;
                                            ws.Cells[i, 4, i, 7].Value = "";
                                            ws.Cells["H" + i].Value = razonmSocial;
                                            ws.Cells["I" + i].Value = "NA";
                                            ws.Cells["J" + i].Value = codDepCm;
                                            ws.Cells["K" + i].Value = codMunCM;
                                            ws.Cells["L" + i].Value = NombrePais(1);

                                            if (categoriaId == 82)
                                            {
                                                ws.Cells["M" + i].Value = CMenor;
                                                ws.Cells[i, 14, i, 20].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 83)
                                            {
                                                ws.Cells["M" + i].Value = 0;
                                                ws.Cells["N" + i].Value = CMenor;
                                                ws.Cells[i, 15, i, 20].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 84)
                                            {
                                                ws.Cells[i, 13, i, 14].Value = 0;
                                                ws.Cells["O" + i].Value = CMenor;
                                                ws.Cells[i, 16, i, 20].Value = 0;
                                                i++;
                                            }

                                            else if (categoriaId == 85)
                                            {
                                                ws.Cells[i, 13, i, 15].Value = 0;
                                                ws.Cells["P" + i].Value = CMenor;
                                                ws.Cells[i, 17, i, 20].Value = 0;
                                                i++;

                                            }
                                            else if (categoriaId == 86)
                                            {
                                                ws.Cells[i, 13, i, 16].Value = 0;
                                                ws.Cells["Q" + i].Value = CMenor;
                                                ws.Cells[i, 18, i, 20].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 87)
                                            {
                                                ws.Cells[i, 13, i, 17].Value = 0;
                                                ws.Cells["R" + i].Value = CMenor;
                                                ws.Cells[i, 19, i, 20].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 88)
                                            {
                                                ws.Cells[i, 13, i, 18].Value = 0;
                                                ws.Cells["S" + i].Value = CMenor;
                                                ws.Cells["T" + i].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 89)
                                            {
                                                ws.Cells[i, 13, i, 19].Value = 0;
                                                ws.Cells["T" + i].Value = CMenor;
                                                i++;
                                            }
                                        }
                                        #endregion
                                        break;

                                    case 2:
                                        # region formato 1003
                                        ws = pack.Workbook.Worksheets.Add("1003");

                                        //// encabezado
                                        ws.Cells["A1:M1,A2:M2,A3:M3,A4:M4,A5:M5"].Merge = true;
                                        ws.Cells["A2:M2,A3:M3,A4:M4"].Style.Font.Bold = true;
                                        ws.Cells["A2:M2"].Style.Font.Name = "Arial";
                                        ws.Cells["A2:M2"].Style.Font.Size = 14;
                                        ws.Cells["A" + 2].Value = "FORMATO 1003 RETENCIONES EN LA FUENTE QUE LE PRACTICARON  " + anioId;
                                        ws.Cells[1, 1, 5, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[1, 1, 5, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                                        ws.Cells["A2:M2,A3:M3,A4:M4,A5:M5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                        ws.Cells["A2:M2,A3:M3,A4:M4,A7:M7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells["A2:M2,A3:M3,A4:M4,A7:M7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells["A2:M2,A3:M3,A4:M4,A5:M5"].Style.WrapText = true;
                                        ws.Cells["A3:M3,A4:M4"].Style.Font.Size = 12;
                                        ws.Cells["A5:M5"].Style.Font.Size = 10;
                                        ws.Cells["A" + 3].Value = nombre;
                                        ws.Cells["A" + 4].Value = nitE;
                                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                                        ws.Cells["A6:M6"].Merge = true;
                                        ws.Cells[6, 1, 6, 13].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                                        ws.Cells[7, 1, 7, 13].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                                        ws.Cells["A7:N7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                                        ws.Cells["A7:M7"].Style.Font.Bold = true;
                                        ws.Cells[7, 1, 7, 13].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[7, 1, 7, 13].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                                        ws.Cells["A7:M7"].Style.WrapText = true;
                                        ////fin encabezado

                                        ws.Cells["A" + 7].Value = "Concepto";
                                        ws.Cells["B" + 7].Value = "Tipo de documento";
                                        ws.Cells["C" + 7].Value = "Número de identificación del informado";
                                        ws.Cells["D" + 7].Value = "Primer apellido del informado";
                                        ws.Cells["E" + 7].Value = "Segundo apellido  del informado";
                                        ws.Cells["F" + 7].Value = "Primer nombre del informado";
                                        ws.Cells["G" + 7].Value = "Otros nombres  del informado";
                                        ws.Cells["H" + 7].Value = "Razon social informado";
                                        ws.Cells["I" + 7].Value = "Dirección";
                                        ws.Cells["J" + 7].Value = "Codigo dpto.";
                                        ws.Cells["K" + 7].Value = "Codigo mcp";
                                        ws.Cells["L" + 7].Value = "Valor acumulado del pago o abono sujeto a retención en la fuente";
                                        ws.Cells["M" + 7].Value = "Retención en la fuente que le practicaron";

                                        i = 8;
                                        var auxNit2 = (from m in movimiento
                                                       orderby m.CUENTA
                                                       select new { m.Nit, m.CUENTA, m.TIPODOCUMENTO, m.APELLIDO1, m.APELLIDO2, m.NOMBRE1, m.NOMBRE2, m.Dir, m.Dep_muni, m.MUNICIPIO, m.NATURALEZA, m.NombreComercial }).Distinct().ToList();

                                        foreach (var item in auxNit2)
                                        {
                                            var dataMov2 = numCuentas.Where(x => x.Nit == item.Nit).OrderBy(x => x.Nit).ToList();
                                            debito = dataMov2.Select(x => x.DEBITO).Sum(); //acumulado por debito
                                            credito = dataMov2.Select(x => x.CREDITO).Sum();//acumulado por credito
                                            int startIndex = 0;
                                            int length = 2;
                                            string substring = (item.CUENTA).Substring(startIndex, length);

                                            //acumulado por debito credito       
                                            if (item.NATURALEZA == "D")
                                                debito_credito = debito - credito;
                                            else
                                                debito_credito = credito - debito;

                                            //acumulado por saldo final cuentas por cobrar cuentas 16 y por pagar cuentas 24
                                            if (substring == "16")
                                                saldoFinal = debito - credito;

                                            else if (substring == "24")
                                                saldoFinal = credito - debito;

                                            if (Acumulado(acumuladoPorId) > 0)
                                            {
                                                ws.Cells["A" + i].Value = codigoConcepto;
                                                ws.Cells["B" + i].Value = item.TIPODOCUMENTO;
                                                ws.Cells["C" + i].Value = item.Nit;
                                                ws.Cells["D" + i].Value = item.APELLIDO1;
                                                ws.Cells["E" + i].Value = item.APELLIDO2;
                                                ws.Cells["F" + i].Value = item.NOMBRE1;
                                                ws.Cells["G" + i].Value = item.NOMBRE2;
                                                ws.Cells["H" + i].Value = item.NombreComercial;
                                                ws.Cells["I" + i].Value = item.Dir;
                                                ws.Cells["J" + i].Value = item.Dep_muni;
                                                ws.Cells["K" + i].Value = item.MUNICIPIO;

                                                if (categoriaId == 90)
                                                {
                                                    ws.Cells["l" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells["M" + i].Value = 0;
                                                    i++;

                                                }
                                                else if (categoriaId == 91)
                                                {
                                                    ws.Cells["L" + i].Value = 0;
                                                    ws.Cells["M" + i].Value = Acumulado(acumuladoPorId);
                                                    i++;
                                                }
                                            }
                                        }

                                        if (CMenor != 0)
                                        {
                                            ws.Cells["A" + i].Value = codigoConcepto;
                                            ws.Cells["B" + i].Value = tipoDocumentoCM;
                                            ws.Cells["C" + i].Value = nitCuantiaMenor;
                                            ws.Cells[i, 4, i, 7].Value = "";
                                            ws.Cells["H" + i].Value = razonmSocial;
                                            ws.Cells["I" + i].Value = "NA";
                                            ws.Cells["J" + i].Value = codDepCm;
                                            ws.Cells["K" + i].Value = codMunCM;
                                            ws.Cells["L" + i].Value = NombrePais(1);

                                            if (categoriaId == 90)
                                            {
                                                ws.Cells["l" + i].Value = CMenor;
                                                ws.Cells["M" + i].Value = 0;
                                                i++;

                                            }
                                            else if (categoriaId == 91)
                                            {
                                                ws.Cells["L" + i].Value = 0;
                                                ws.Cells["M" + i].Value = CMenor;
                                                i++;
                                            }
                                        }
                                        #endregion
                                        break;

                                    case 3:
                                        #region formato 1004
                                        ws = pack.Workbook.Worksheets.Add("1004");
                                        // encabezado
                                        ws.Cells["A1:O1,A2:O2,A3:O3,A4:O4,A5:O5"].Merge = true;
                                        ws.Cells["A2:O2,A3:O3,A4:O4"].Style.Font.Bold = true;
                                        ws.Cells["A2:O2"].Style.Font.Name = "Arial";
                                        ws.Cells["A2:O2"].Style.Font.Size = 14;
                                        ws.Cells["A" + 2].Value = "FORMATO 1004 DESCUENTOS TRIBUTARIOS   " + anioId;
                                        ws.Cells[1, 1, 5, 15].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[1, 1, 5, 15].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                                        ws.Cells["A2:O2,A3:O3,A4:O4,A5:O5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                        ws.Cells["A2:O2,A3:O3,A4:O4,A7:O7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells["A2:O2,A3:O3,A4:O4,A7:O7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells["A2:O2,A3:O3,A4:O4,A5:O5"].Style.WrapText = true;
                                        ws.Cells["A3:O3,A4:O4"].Style.Font.Size = 12;
                                        ws.Cells["A5:O5"].Style.Font.Size = 10;
                                        ws.Cells["A" + 3].Value = nombre;
                                        ws.Cells["A" + 4].Value = nitE;
                                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                                        ws.Cells["A6:O6"].Merge = true;
                                        ws.Cells[6, 1, 6, 15].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                                        ws.Cells[7, 1, 7, 15].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                                        ws.Cells["A7:P7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                                        ws.Cells["A7:O7"].Style.Font.Bold = true;
                                        ws.Cells[7, 1, 7, 15].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[7, 1, 7, 15].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                                        ws.Cells["A7:O7"].Style.WrapText = true;
                                        ////fin encabezado

                                        ws.Cells["A" + 7].Value = "Concepto";
                                        ws.Cells["B" + 7].Value = "Tipo de documento";
                                        ws.Cells["C" + 7].Value = "Número de identificación del informado";
                                        ws.Cells["D" + 7].Value = "Primer apellido del informado";
                                        ws.Cells["E" + 7].Value = "Segundo apellido  del informado";
                                        ws.Cells["F" + 7].Value = "Primer nombre del informado";
                                        ws.Cells["G" + 7].Value = "Otros nombres  del informado";
                                        ws.Cells["H" + 7].Value = "Razon social informado";
                                        ws.Cells["I" + 7].Value = "Dirección";
                                        ws.Cells["J" + 7].Value = "Codigo dpto.";
                                        ws.Cells["K" + 7].Value = "Codigo mcp";
                                        ws.Cells["L" + 7].Value = "Codigo Pais";
                                        ws.Cells["M" + 7].Value = "Correo electrónico";
                                        ws.Cells["N" + 7].Value = "Valor del pago o abono en cuenta";
                                        ws.Cells["O" + 7].Value = "Valor del descuento tributario";
                                  
                                        i = 8;

                                        var auxNit3 = (from m in movimiento
                                                       orderby m.CUENTA
                                                       select new { m.Nit, m.CUENTA, m.TIPODOCUMENTO, m.APELLIDO1, m.APELLIDO2, m.NOMBRE1, m.NOMBRE2, m.Dir, m.Dep_muni, m.MUNICIPIO, m.Id_pais, m.EMAIL, m.NATURALEZA, m.NombreComercial }).Distinct().ToList();

                                        foreach (var item in auxNit3)
                                        {
                                            var dataMov2 = numCuentas.Where(x => x.Nit == item.Nit).OrderBy(x => x.Nit).ToList();
                                            debito = dataMov2.Select(x => x.DEBITO).Sum(); //acumulado por debito
                                            credito = dataMov2.Select(x => x.CREDITO).Sum();//acumulado por credito
                                            int startIndex = 0;
                                            int length = 2;
                                            string substring = (item.CUENTA).Substring(startIndex, length);

                                            //acumulado por debito credito       
                                            if (item.NATURALEZA == "D")
                                                debito_credito = debito - credito;
                                            else
                                                debito_credito = credito - debito;

                                            //acumulado por saldo final cuentas por cobrar cuentas 16 y por pagar cuentas 24
                                            if (substring == "16")
                                                saldoFinal = debito - credito;

                                            else if (substring == "24")
                                                saldoFinal = credito - debito;



                                            if (Acumulado(acumuladoPorId) > 0)
                                            {
                                                ws.Cells["A" + i].Value = codigoConcepto;
                                                ws.Cells["B" + i].Value = item.TIPODOCUMENTO;
                                                ws.Cells["C" + i].Value = item.Nit;
                                                ws.Cells["D" + i].Value = item.APELLIDO1;
                                                ws.Cells["E" + i].Value = item.APELLIDO2;
                                                ws.Cells["F" + i].Value = item.NOMBRE1;
                                                ws.Cells["G" + i].Value = item.NOMBRE2;
                                                ws.Cells["H" + i].Value = item.NombreComercial;
                                                ws.Cells["I" + i].Value = item.Dir;
                                                ws.Cells["J" + i].Value = item.Dep_muni;
                                                ws.Cells["K" + i].Value = item.MUNICIPIO;
                                                ws.Cells["L" + i].Value = CodigoPais(item.Id_pais);
                                                ws.Cells["M" + i].Value = item.EMAIL;


                                                if (categoriaId == 92)
                                                {
                                                    ws.Cells["N" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells["O" + i].Value = 0;
                                                    i++;

                                                }
                                                else if (categoriaId == 93)
                                                {
                                                    ws.Cells["N" + i].Value = 0;
                                                    ws.Cells["O" + i].Value = Acumulado(acumuladoPorId);
                                                    i++;
                                                }
                                            }

                                        }
                                        if (CMenor != 0)
                                        {
                                            ws.Cells["A" + i].Value = codigoConcepto;
                                            ws.Cells["B" + i].Value = tipoDocumentoCM;
                                            ws.Cells["C" + i].Value = nitCuantiaMenor;
                                            ws.Cells[i, 4, i, 7].Value = "";
                                            ws.Cells["H" + i].Value = razonmSocial;
                                            ws.Cells["I" + i].Value = "NA";
                                            ws.Cells["J" + i].Value = codDepCm;
                                            ws.Cells["K" + i].Value = codMunCM;
                                            ws.Cells["L" + i].Value = CodigoPais(1);
                                            ws.Cells["M" + i].Value = "NA";

                                            if (categoriaId == 92)
                                            {
                                                ws.Cells["N" + i].Value = CMenor;
                                                ws.Cells["O" + i].Value = 0;
                                                i++;

                                            }
                                            else if (categoriaId == 93)
                                            {
                                                ws.Cells["N" + i].Value = 0;
                                                ws.Cells["O" + i].Value = CMenor;
                                                i++;
                                            }
                                        }
                                        #endregion
                                        break;

                                    case 4:
                                        #region formato 1005

                                        ws = pack.Workbook.Worksheets.Add("1005");
                                        //// encabezado
                                        ws.Cells["A1:K1,A2:K2,A3:K3,A4:K4,A5:K5"].Merge = true;
                                        ws.Cells["A2:K2,A3:K3,A4:K4"].Style.Font.Bold = true;
                                        ws.Cells["A2:K2"].Style.Font.Name = "Arial";
                                        ws.Cells["A2:K2"].Style.Font.Size = 14;
                                        ws.Cells["A" + 2].Value = "FORMATO 1005 IMPUESTO A LAS VENTAS POR PAGAR - DESCONTABLE   " + anioId;
                                        ws.Cells[1, 1, 5, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[1, 1, 5, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                                        ws.Cells["A2:K2,A3:K3,A4:K4,A5:K5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                        ws.Cells["A2:K2,A3:K3,A4:K4,A7:K7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells["A2:K2,A3:K3,A4:K4,A7:K7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells["A2:K2,A3:K3,A4:K4,A5:K5"].Style.WrapText = true;
                                        ws.Cells["A3:K3,A4:K4"].Style.Font.Size = 12;
                                        ws.Cells["A5:K5"].Style.Font.Size = 10;
                                        ws.Cells["A" + 3].Value = nombre;
                                        ws.Cells["A" + 4].Value = nitE;
                                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                                        ws.Cells["A6:K6"].Merge = true;
                                        ws.Cells[6, 1, 6, 11].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                                        ws.Cells[7, 1, 7, 11].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                                        ws.Cells["A7:L7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                                        ws.Cells["A7:K7"].Style.Font.Bold = true;
                                        ws.Cells[7, 1, 7, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[7, 1, 7, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                                        ws.Cells["A7:K7"].Style.WrapText = true;
                                        ////fin encabezado

                                        ws.Cells["A" + 7].Value = "Concepto";
                                        ws.Cells["B" + 7].Value = "Tipo de documento";
                                        ws.Cells["C" + 7].Value = "Número de identificación";
                                        ws.Cells["D" + 7].Value = "DV";
                                        ws.Cells["E" + 7].Value = "Primer apellido del informado";
                                        ws.Cells["F" + 7].Value = "Segundo apellido  del informado";
                                        ws.Cells["G" + 7].Value = "Primer nombre del informado";
                                        ws.Cells["H" + 7].Value = "Otros nombres  del informado";
                                        ws.Cells["I" + 7].Value = "Razon social informado";
                                        ws.Cells["J" + 7].Value = "Impuesto descontable";
                                        ws.Cells["K" + 7].Value = "IVA resultante por devoluciones en ventas anuladas, rescindidas o resueltas";

                                        /// 

                                        i = 8;

                                        var auxNit4 = (from m in movimiento
                                                       orderby m.CUENTA
                                                       select new { m.Nit, m.CUENTA, m.TIPODOCUMENTO, m.APELLIDO1, m.APELLIDO2, m.NOMBRE1, m.NOMBRE2, m.NATURALEZA, m.DIGVER, m.NombreComercial }).Distinct().ToList();

                                        foreach (var item in auxNit4)
                                        {
                                            var dataMov2 = numCuentas.Where(x => x.Nit == item.Nit).OrderBy(x => x.Nit).ToList();
                                            debito = dataMov2.Select(x => x.DEBITO).Sum(); //acumulado por debito
                                            credito = dataMov2.Select(x => x.CREDITO).Sum();//acumulado por credito
                                            int startIndex = 0;
                                            int length = 2;
                                            string substring = (item.CUENTA).Substring(startIndex, length);

                                            //acumulado por debito credito       
                                            if (item.NATURALEZA == "D")
                                                debito_credito = debito - credito;
                                            else
                                                debito_credito = credito - debito;

                                            //acumulado por saldo final cuentas por cobrar cuentas 16 y por pagar cuentas 24
                                            if (substring == "16")
                                                saldoFinal = debito - credito;

                                            if (substring == "24")
                                                saldoFinal = credito - debito;

                                            if (Acumulado(acumuladoPorId) > 0)
                                            {
                                                ws.Cells["A" + i].Value = codigoConcepto;

                                                ws.Cells["B" + i].Value = item.TIPODOCUMENTO;
                                                ws.Cells["C" + i].Value = item.Nit;
                                                ws.Cells["D" + i].Value = item.DIGVER;
                                                ws.Cells["E" + i].Value = item.APELLIDO1;
                                                ws.Cells["F" + i].Value = item.APELLIDO2;
                                                ws.Cells["G" + i].Value = item.NOMBRE1;
                                                ws.Cells["H" + i].Value = item.NOMBRE2;
                                                ws.Cells["I" + i].Value = item.NombreComercial;

                                                if (categoriaId == 94)
                                                {
                                                    ws.Cells["J" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells["K" + i].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 95)
                                                {
                                                    ws.Cells["J" + i].Value = 0;
                                                    ws.Cells["K" + i].Value = Acumulado(acumuladoPorId);
                                                    i++;
                                                }
                                            }

                                        }
                                        if (CMenor != 0)
                                        {
                                            ws.Cells["A" + i].Value = codigoConcepto;
                                            ws.Cells["B" + i].Value = tipoDocumentoCM;
                                            ws.Cells["C" + i].Value = nitCuantiaMenor;
                                            ws.Cells["D" + i].Value = digVer;
                                            ws.Cells[i, 5, i, 8].Value = "";
                                            ws.Cells["I" + i].Value = razonmSocial;

                                            if (categoriaId == 94)
                                            {
                                                ws.Cells["J" + i].Value = CMenor;
                                                ws.Cells["K" + i].Value = 0;
                                                i++;

                                            }
                                            else if (categoriaId == 95)
                                            {
                                                ws.Cells["J" + i].Value = 0;
                                                ws.Cells["K" + i].Value = CMenor;
                                                i++;
                                            }
                                        }
                                        #endregion
                                        break;

                                    case 5:
                                        #region formato 1006
                                        ws = pack.Workbook.Worksheets.Add("1006");
                                        //// encabezado
                                        ws.Cells["A1:L1,A2:L2,A3:L3,A4:L4,A5:L5"].Merge = true;
                                        ws.Cells["A2:L2,A3:L3,A4:L4"].Style.Font.Bold = true;
                                        ws.Cells["A2:L2"].Style.Font.Name = "Arial";
                                        ws.Cells["A2:L2"].Style.Font.Size = 14;
                                        ws.Cells["A" + 2].Value = "FORMATO 1006 IMPUESTO A LAS VENTAS POR PAGAR - GENERADO   " + anioId;
                                        ws.Cells[1, 1, 5, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[1, 1, 5, 12].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                                        ws.Cells["A2:L2,A3:L3,A4:L4,A5:L5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                        ws.Cells["A2:L2,A3:L3,A4:L4,A7:L7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells["A2:L2,A3:L3,A4:L4,A7:L7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells["A2:L2,A3:L3,A4:L4,A5:L5"].Style.WrapText = true;
                                        ws.Cells["A3:L3,A4:L4"].Style.Font.Size = 12;
                                        ws.Cells["A5:L5"].Style.Font.Size = 10;
                                        ws.Cells["A" + 3].Value = nombre;
                                        ws.Cells["A" + 4].Value = nitE;
                                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                                        ws.Cells["A6:L6"].Merge = true;
                                        ws.Cells[6, 1, 6, 12].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                                        ws.Cells[7, 1, 7, 12].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                                        ws.Cells["A7:M7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                                        ws.Cells["A7:L7"].Style.Font.Bold = true;
                                        ws.Cells[7, 1, 7, 12].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[7, 1, 7, 12].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                                        ws.Cells["A7:L7"].Style.WrapText = true;
                                        ////fin encabezado

                                        ws.Cells["A" + 7].Value = "Concepto";
                                        ws.Cells["B" + 7].Value = "Tipo de documento";
                                        ws.Cells["C" + 7].Value = "Número de identificación";
                                        ws.Cells["D" + 7].Value = "DV";
                                        ws.Cells["E" + 7].Value = "Primer apellido del informado";
                                        ws.Cells["F" + 7].Value = "Segundo apellido  del informado";
                                        ws.Cells["G" + 7].Value = "Primer nombre del informado";
                                        ws.Cells["H" + 7].Value = "Otros nombres  del informado";
                                        ws.Cells["I" + 7].Value = "Razon social informado";
                                        ws.Cells["J" + 7].Value = "Impuesto generado";
                                        ws.Cells["K" + 7].Value = "IVA recuperado por operaciones en devoluciones en compras anuladas, rescindidas o resueltas";
                                        ws.Cells["L" + 7].Value = "Impuesto al consumo";

                                        i = 8;

                                        var auxNit5 = (from m in movimiento
                                                       orderby m.CUENTA
                                                       select new { m.Nit, m.CUENTA, m.TIPODOCUMENTO, m.APELLIDO1, m.APELLIDO2, m.NOMBRE1, m.NOMBRE2, m.NATURALEZA, m.DIGVER, m.NombreComercial }).Distinct().ToList();

                                        foreach (var item in auxNit5)
                                        {
                                            var dataMov2 = numCuentas.Where(x => x.Nit == item.Nit).OrderBy(x => x.Nit).ToList();
                                            debito = dataMov2.Select(x => x.DEBITO).Sum(); //acumulado por debito
                                            credito = dataMov2.Select(x => x.CREDITO).Sum();//acumulado por credito
                                            int startIndex = 0;
                                            int length = 2;
                                            string substring = (item.CUENTA).Substring(startIndex, length);

                                            //acumulado por debito credito       
                                            if (item.NATURALEZA == "D")
                                                debito_credito = debito - credito;
                                            else
                                                debito_credito = credito - debito;

                                            //acumulado por saldo final cuentas por cobrar cuentas 16 y por pagar cuentas 24
                                            if (substring == "16")
                                                saldoFinal = debito - credito;

                                            else if (substring == "24")
                                                saldoFinal = credito - debito;

                                            if (Acumulado(acumuladoPorId) > 0)
                                            {
                                                ws.Cells["A" + i].Value = codigoConcepto;
                                                //Cuantias Menores
                                                if ((Acumulado(acumuladoPorId)) < 500000)
                                                {
                                                    ws.Cells["B" + i].Value = tipoDocumentoCM;
                                                    ws.Cells["C" + i].Value = nitCuantiaMenor;
                                                    ws.Cells["D" + i].Value = digVer;
                                                    ws.Cells[i, 5, i, 8].Value = "";
                                                    ws.Cells["I" + i].Value = razonmSocial;
                                                }
                                                else
                                                {
                                                    ws.Cells["B" + i].Value = item.TIPODOCUMENTO;
                                                    ws.Cells["C" + i].Value = item.Nit;
                                                    ws.Cells["D" + i].Value = item.DIGVER;
                                                    ws.Cells["E" + i].Value = item.APELLIDO1;
                                                    ws.Cells["F" + i].Value = item.APELLIDO2;
                                                    ws.Cells["G" + i].Value = item.NOMBRE1;
                                                    ws.Cells["H" + i].Value = item.NOMBRE2;
                                                    ws.Cells["I" + i].Value = item.NombreComercial;
                                                }
                                                if (categoriaId == 96)
                                                {
                                                    ws.Cells["J" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 11, i, 12].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 97)
                                                {
                                                    ws.Cells["J" + i].Value = 0;
                                                    ws.Cells["K" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells["L" + i].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 99)
                                                {
                                                    ws.Cells[i, 10, i, 11].Value = 0;
                                                    ws.Cells["L" + i].Value = Acumulado(acumuladoPorId);
                                                    i++;
                                                }
                                            }

                                        }

                                        if (CMenor > 0 && CMenor < 500000)
                                        {
                                            ws.Cells["A" + i].Value = codigoConcepto;
                                            ws.Cells["B" + i].Value = tipoDocumentoCM;
                                            ws.Cells["C" + i].Value = nitCuantiaMenor;
                                            ws.Cells["D" + i].Value = digVer;
                                            ws.Cells[i, 5, i, 8].Value = "";
                                            ws.Cells["I" + i].Value = razonmSocial;

                                            if (categoriaId == 96)
                                            {
                                                ws.Cells["J" + i].Value = CMenor;
                                                ws.Cells[i, 11, i, 12].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 97)
                                            {
                                                ws.Cells["J" + i].Value = 0;
                                                ws.Cells["K" + i].Value = CMenor;
                                                ws.Cells["L" + i].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 99)
                                            {
                                                ws.Cells[i, 10, i, 11].Value = 0;
                                                ws.Cells["L" + i].Value = CMenor;
                                                i++;
                                            }
                                        }

                                        #endregion
                                        break;

                                    case 6:
                                        #region FORMATO 1007
                                        ws = pack.Workbook.Worksheets.Add("1007");
                                        //// encabezado
                                        ws.Cells["A1:K1,A2:K2,A3:K3,A4:K4,A5:K5"].Merge = true;
                                        ws.Cells["A2:K2,A3:K3,A4:K4"].Style.Font.Bold = true;
                                        ws.Cells["A2:K2"].Style.Font.Name = "Arial";
                                        ws.Cells["A2:K2"].Style.Font.Size = 14;
                                        ws.Cells["A" + 2].Value = "FORMATO 1007 INGRESOS RECIBIDOS     " + anioId;
                                        ws.Cells[1, 1, 5, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[1, 1, 5, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                                        ws.Cells["A2:K2,A3:K3,A4:K4,A5:K5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                        ws.Cells["A2:K2,A3:K3,A4:K4,A7:K7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells["A2:K2,A3:K3,A4:K4,A7:K7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells["A2:K2,A3:K3,A4:K4,A5:K5"].Style.WrapText = true;
                                        ws.Cells["A3:K3,A4:K4"].Style.Font.Size = 12;
                                        ws.Cells["A5:K5"].Style.Font.Size = 10;
                                        ws.Cells["A" + 3].Value = nombre;
                                        ws.Cells["A" + 4].Value = nitE;
                                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                                        ws.Cells["A6:K6"].Merge = true;
                                        ws.Cells[6, 1, 6, 11].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                                        ws.Cells[7, 1, 7, 11].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                                        ws.Cells["A7:L7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                                        ws.Cells["A7:K7"].Style.Font.Bold = true;
                                        ws.Cells[7, 1, 7, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[7, 1, 7, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                                        ws.Cells["A7:K7"].Style.WrapText = true;
                                        ////fin encabezado

                                        ws.Cells["A" + 7].Value = "Concepto";
                                        ws.Cells["B" + 7].Value = "Tipo de documento";
                                        ws.Cells["C" + 7].Value = "Número de identificación";
                                        ws.Cells["D" + 7].Value = "Primer apellido del informado";
                                        ws.Cells["E" + 7].Value = "Segundo apellido  del informado";
                                        ws.Cells["F" + 7].Value = "Primer nombre del informado";
                                        ws.Cells["G" + 7].Value = "Otros nombres  del informado";
                                        ws.Cells["H" + 7].Value = "Razon social informado";
                                        ws.Cells["I" + 7].Value = "País de residencia o domicilio";
                                        ws.Cells["J" + 7].Value = "Ingresos brutos recibidos ";
                                        ws.Cells["K" + 7].Value = "Devoluciones, rebajas y descuentos";

                                        i = 8;

                                        var auxNit6 = (from m in movimiento
                                                       orderby m.CUENTA
                                                       select new { m.Nit, m.CUENTA, m.TIPODOCUMENTO, m.APELLIDO1, m.APELLIDO2, m.NOMBRE1, m.NOMBRE2, m.Id_pais, m.NATURALEZA, m.NombreComercial }).Distinct().ToList();

                                        foreach (var item in auxNit6)
                                        {
                                            var dataMov2 = numCuentas.Where(x => x.Nit == item.Nit).OrderBy(x => x.Nit).ToList();
                                            debito = dataMov2.Select(x => x.DEBITO).Sum(); //acumulado por debito
                                            credito = dataMov2.Select(x => x.CREDITO).Sum();//acumulado por credito
                                            int startIndex = 0;
                                            int length = 2;
                                            string substring = (item.CUENTA).Substring(startIndex, length);

                                            //acumulado por debito credito       
                                            if (item.NATURALEZA == "D")
                                                debito_credito = debito - credito;
                                            else
                                                debito_credito = credito - debito;

                                            //acumulado por saldo final cuentas por cobrar cuentas 16 y por pagar cuentas 24
                                            if (substring == "16")
                                                saldoFinal = debito - credito;

                                            else if (substring == "24")
                                                saldoFinal = credito - debito;

                                            if (Acumulado(acumuladoPorId) > 0)
                                            {
                                                ws.Cells["A" + i].Value = codigoConcepto;
                                                //validacion cuantias menores
                                                if ((Acumulado(acumuladoPorId)) < 500000)
                                                {
                                                    ws.Cells["B" + i].Value = tipoDocumentoCM;
                                                    ws.Cells["C" + i].Value = nitCuantiaMenor;
                                                    ws.Cells[i, 4, i, 7].Value = "";
                                                    ws.Cells["H" + i].Value = razonmSocial;
                                                    ws.Cells["I" + i].Value = NombrePais(1);
                                                }
                                                else
                                                {
                                                    ws.Cells["B" + i].Value = item.TIPODOCUMENTO;
                                                    ws.Cells["C" + i].Value = item.Nit;
                                                    ws.Cells["D" + i].Value = item.APELLIDO1;
                                                    ws.Cells["E" + i].Value = item.APELLIDO2;
                                                    ws.Cells["F" + i].Value = item.NOMBRE1;
                                                    ws.Cells["G" + i].Value = item.NOMBRE2;
                                                    ws.Cells["H" + i].Value = item.NombreComercial;
                                                    ws.Cells["I" + i].Value = NombrePais(item.Id_pais);
                                                }
                                                if (categoriaId == 100)
                                                {
                                                    ws.Cells["J" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells["K" + i].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 101)
                                                {
                                                    ws.Cells["J" + i].Value = 0;
                                                    ws.Cells["K" + i].Value = Acumulado(acumuladoPorId);
                                                    i++;
                                                }
                                            }
                                        }

                                        if (CMenor > 0 && CMenor < 500000)
                                        {
                                            ws.Cells["A" + i].Value = codigoConcepto;
                                            ws.Cells["B" + i].Value = tipoDocumentoCM;
                                            ws.Cells["C" + i].Value = nitCuantiaMenor;
                                            ws.Cells[i, 4, i, 7].Value = "";
                                            ws.Cells["H" + i].Value = razonmSocial;
                                            ws.Cells["I" + i].Value = NombrePais(1);

                                            if (categoriaId == 100)
                                            {
                                                ws.Cells["J" + i].Value = CMenor;
                                                ws.Cells["K" + i].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 101)
                                            {
                                                ws.Cells["J" + i].Value = 0;
                                                ws.Cells["K" + i].Value = CMenor;
                                                i++;
                                            }
                                        }

                                        #endregion
                                        break;

                                    case 7:
                                        #region formato 1008
                                        ws = pack.Workbook.Worksheets.Add("1008");
                                        //// encabezado
                                        ws.Cells["A1:N1,A2:N2,A3:N3,A4:N4,A5:N5"].Merge = true;
                                        ws.Cells["A2:N2,A3:N3,A4:N4"].Style.Font.Bold = true;
                                        ws.Cells["A2:N2"].Style.Font.Name = "Arial";
                                        ws.Cells["A2:N2"].Style.Font.Size = 14;
                                        ws.Cells["A" + 2].Value = "FORMATO 1008 SALDO DE CUENTAS POR COBRAR AL 31 DE DICIEMBRE    " + anioId;
                                        ws.Cells[1, 1, 5, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[1, 1, 5, 14].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                                        ws.Cells["A2:N2,A3:N3,A4:N4,A5:N5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                        ws.Cells["A2:N2,A3:N3,A4:N4,A7:N7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells["A2:N2,A3:N3,A4:N4,A7:N7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells["A2:N2,A3:N3,A4:N4,A5:N5"].Style.WrapText = true;
                                        ws.Cells["A3:N3,A4:N4"].Style.Font.Size = 12;
                                        ws.Cells["A5:N5"].Style.Font.Size = 10;
                                        ws.Cells["A" + 3].Value = nombre;
                                        ws.Cells["A" + 4].Value = nitE;
                                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                                        ws.Cells["A6:N6"].Merge = true;
                                        ws.Cells[6, 1, 6, 14].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                                        ws.Cells[7, 1, 7, 14].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                                        ws.Cells["A7:O7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                                        ws.Cells["A7:N7"].Style.Font.Bold = true;
                                        ws.Cells[7, 1, 7, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[7, 1, 7, 14].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                                        ws.Cells["A7:N7"].Style.WrapText = true;
                                        ////fin encabezadO
                                        ws.Cells["A" + 7].Value = "Concepto";
                                        ws.Cells["B" + 7].Value = "Tipo de documento";
                                        ws.Cells["C" + 7].Value = "Número identificación deudor";
                                        ws.Cells["D" + 7].Value = "DV";
                                        ws.Cells["E" + 7].Value = "Primer apellido deudor";
                                        ws.Cells["F" + 7].Value = "Segundo apellido deudor";
                                        ws.Cells["G" + 7].Value = "Primer nombre deudor";
                                        ws.Cells["H" + 7].Value = "Otros nombres deudor";
                                        ws.Cells["I" + 7].Value = "Razón social deudor";
                                        ws.Cells["J" + 7].Value = "Dirección";
                                        ws.Cells["K" + 7].Value = "Código dpto.";
                                        ws.Cells["L" + 7].Value = "Código mcp";
                                        ws.Cells["M" + 7].Value = "País de residencia o domicilio";
                                        ws.Cells["N" + 7].Value = "Saldo cuentas por cobrar al 31-12";

                                        i = 8;

                                        var auxNit7 = (from m in movimiento
                                                       orderby m.CUENTA
                                                       select new { m.Nit, m.CUENTA, m.TIPODOCUMENTO, m.APELLIDO1, m.APELLIDO2, m.NOMBRE1, m.NOMBRE2, m.Dir, m.Dep_muni, m.MUNICIPIO, m.Id_pais, m.NATURALEZA, m.DIGVER, m.NombreComercial }).Distinct().ToList();

                                        foreach (var item in auxNit7)
                                        {
                                            var dataMov2 = numCuentas.Where(x => x.Nit == item.Nit).OrderBy(x => x.Nit).ToList();
                                            debito = dataMov2.Select(x => x.DEBITO).Sum(); //acumulado por debito
                                            credito = dataMov2.Select(x => x.CREDITO).Sum();//acumulado por credito
                                            int startIndex = 0;
                                            int length = 2;
                                            string substring = (item.CUENTA).Substring(startIndex, length);

                                            //acumulado por debito credito       
                                            if (item.NATURALEZA == "D")
                                                debito_credito = debito - credito;
                                            else
                                                debito_credito = credito - debito;

                                            //acumulado por saldo final cuentas por cobrar cuentas 16 y por pagar cuentas 24

                                            if (substring == "16")
                                                saldoFinal = debito - credito;

                                            else if (substring == "24")
                                                saldoFinal = credito - debito;


                                            if (Acumulado(acumuladoPorId) > 0)
                                            {
                                                ws.Cells["A" + i].Value = codigoConcepto;
                                                //validacion cuantias menores
                                                if ((Acumulado(acumuladoPorId)) < 1000000)
                                                {
                                                    ws.Cells["B" + i].Value = tipoDocumentoCM;
                                                    ws.Cells["C" + i].Value = nitCuantiaMenor;
                                                    ws.Cells["D" + i].Value = digVer;
                                                    ws.Cells[i, 5, i, 8].Value = "";
                                                    ws.Cells["I" + i].Value = razonmSocial;
                                                    ws.Cells["J" + i].Value = "NA";
                                                    ws.Cells["k" + i].Value = codDepCm;
                                                    ws.Cells["l" + i].Value = codMunCM;
                                                    ws.Cells["M" + i].Value = NombrePais(1);
                                                }
                                                else
                                                {
                                                    ws.Cells["B" + i].Value = item.TIPODOCUMENTO;
                                                    ws.Cells["C" + i].Value = item.Nit;
                                                    ws.Cells["D" + i].Value = item.DIGVER;
                                                    ws.Cells["E" + i].Value = item.APELLIDO1;
                                                    ws.Cells["F" + i].Value = item.APELLIDO2;
                                                    ws.Cells["G" + i].Value = item.NOMBRE1;
                                                    ws.Cells["H" + i].Value = item.NOMBRE2;
                                                    ws.Cells["I" + i].Value = item.NombreComercial;
                                                    ws.Cells["J" + i].Value = item.Dir;
                                                    ws.Cells["K" + i].Value = item.Dep_muni;
                                                    ws.Cells["L" + i].Value = item.MUNICIPIO;
                                                    ws.Cells["M" + i].Value = NombrePais(item.Id_pais);
                                                }
                                                if (categoriaId == 102)
                                                {
                                                    ws.Cells["N" + i].Value = Acumulado(acumuladoPorId);
                                                    i++;
                                                }
                                            }

                                        }

                                        if (CMenor > 0 && CMenor < 1000000)
                                        {
                                            ws.Cells["A" + i].Value = codigoConcepto;
                                            ws.Cells["B" + i].Value = tipoDocumentoCM;
                                            ws.Cells["C" + i].Value = nitCuantiaMenor;
                                            ws.Cells["D" + i].Value = digVer;
                                            ws.Cells[i, 5, i, 8].Value = "";
                                            ws.Cells["I" + i].Value = razonmSocial;
                                            ws.Cells["J" + i].Value = "NA";
                                            ws.Cells["k" + i].Value = codDepCm;
                                            ws.Cells["l" + i].Value = codMunCM;
                                            ws.Cells["M" + i].Value = NombrePais(1);

                                            if (categoriaId == 102)
                                            {
                                                ws.Cells["N" + i].Value = CMenor;
                                                i++;
                                            }
                                        }

                                        #endregion
                                        break;

                                    case 8:
                                        #region Formato 1009
                                        ws = pack.Workbook.Worksheets.Add("1009");
                                        //// encabezado
                                        ws.Cells["A1:N1,A2:N2,A3:N3,A4:N4,A5:N5"].Merge = true;
                                        ws.Cells["A2:N2,A3:N3,A4:N4"].Style.Font.Bold = true;
                                        ws.Cells["A2:N2"].Style.Font.Name = "Arial";
                                        ws.Cells["A2:N2"].Style.Font.Size = 14;
                                        ws.Cells["A" + 2].Value = "FORMATO 1009 SALDO DE CUENTAS POR PAGAR AL 31 DE DICIEMBRE    " + anioId;
                                        ws.Cells[1, 1, 5, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[1, 1, 5, 14].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                                        ws.Cells["A2:N2,A3:N3,A4:N4,A5:N5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                        ws.Cells["A2:N2,A3:N3,A4:N4,A7:N7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells["A2:N2,A3:N3,A4:N4,A7:N7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells["A2:N2,A3:N3,A4:N4,A5:N5"].Style.WrapText = true;
                                        ws.Cells["A3:N3,A4:N4"].Style.Font.Size = 12;
                                        ws.Cells["A5:N5"].Style.Font.Size = 10;
                                        ws.Cells["A" + 3].Value = nombre;
                                        ws.Cells["A" + 4].Value = nitE;
                                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                                        ws.Cells["A6:N6"].Merge = true;
                                        ws.Cells[6, 1, 6, 14].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                                        ws.Cells[7, 1, 7, 14].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                                        ws.Cells["A7:O7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                                        ws.Cells["A7:N7"].Style.Font.Bold = true;
                                        ws.Cells[7, 1, 7, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[7, 1, 7, 14].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                                        ws.Cells["A7:N7"].Style.WrapText = true;
                                        ////fin encabezadO
                                        ws.Cells["A" + 7].Value = "Concepto";
                                        ws.Cells["B" + 7].Value = "Tipo de documento";
                                        ws.Cells["C" + 7].Value = "Número identificación acreedor";
                                        ws.Cells["D" + 7].Value = "DV";
                                        ws.Cells["E" + 7].Value = "Primer apellido acreedor";
                                        ws.Cells["F" + 7].Value = "Segundo apellido acreedor";
                                        ws.Cells["G" + 7].Value = "Primer nombre acreedor";
                                        ws.Cells["H" + 7].Value = "Otros nombres acreedor";
                                        ws.Cells["I" + 7].Value = "Razón social acreedor";
                                        ws.Cells["J" + 7].Value = "Dirección";
                                        ws.Cells["K" + 7].Value = "Código dpto.";
                                        ws.Cells["L" + 7].Value = "Código mcp";
                                        ws.Cells["M" + 7].Value = "País de residencia o domicilio";
                                        ws.Cells["N" + 7].Value = "Saldo cuentas por cobrar al 31 de Diciembre";

                                        i = 8;
                                        var auxNit8 = (from m in movimiento
                                                       orderby m.CUENTA
                                                       select new { m.Nit, m.CUENTA, m.TIPODOCUMENTO, m.APELLIDO1, m.APELLIDO2, m.NOMBRE1, m.NOMBRE2, m.Dir, m.Dep_muni, m.MUNICIPIO, m.Id_pais, m.NATURALEZA, m.DIGVER, m.NombreComercial }).Distinct().ToList();

                                        foreach (var item in auxNit8)
                                        {
                                            var dataMov2 = numCuentas.Where(x => x.Nit == item.Nit).OrderBy(x => x.Nit).ToList();
                                            debito = dataMov2.Select(x => x.DEBITO).Sum(); //acumulado por debito
                                            credito = dataMov2.Select(x => x.CREDITO).Sum();//acumulado por credito
                                            int startIndex = 0;
                                            int length = 2;
                                            string substring = (item.CUENTA).Substring(startIndex, length);

                                            //acumulado por debito credito       
                                            if (item.NATURALEZA == "D")
                                                debito_credito = debito - credito;
                                            else
                                                debito_credito = credito - debito;

                                            //acumulado por saldo final cuentas por cobrar cuentas 16 y por pagar cuentas 24
                                            if (substring == "16")
                                                saldoFinal = debito - credito;

                                            if (substring == "24")
                                                saldoFinal = credito - debito;


                                            if (Acumulado(acumuladoPorId) > 0)
                                            {
                                                ws.Cells["A" + i].Value = codigoConcepto;
                                                //validacion cuantias menores
                                                if ((Acumulado(acumuladoPorId)) < 1000000)
                                                {
                                                    ws.Cells["B" + i].Value = tipoDocumentoCM;
                                                    ws.Cells["C" + i].Value = nitCuantiaMenor;
                                                    ws.Cells["D" + i].Value = digVer;
                                                    ws.Cells[i, 5, i, 8].Value = "";
                                                    ws.Cells["I" + i].Value = razonmSocial;
                                                    ws.Cells["J" + i].Value = "NA";
                                                    ws.Cells["k" + i].Value = codDepCm;
                                                    ws.Cells["l" + i].Value = codMunCM;
                                                    ws.Cells["M" + i].Value = NombrePais(1);
                                                }
                                                else
                                                {
                                                    ws.Cells["B" + i].Value = item.TIPODOCUMENTO;
                                                    ws.Cells["C" + i].Value = item.Nit;
                                                    ws.Cells["D" + i].Value = item.DIGVER;
                                                    ws.Cells["E" + i].Value = item.APELLIDO1;
                                                    ws.Cells["F" + i].Value = item.APELLIDO2;
                                                    ws.Cells["G" + i].Value = item.NOMBRE1;
                                                    ws.Cells["H" + i].Value = item.NOMBRE2;
                                                    ws.Cells["I" + i].Value = item.NombreComercial;
                                                    ws.Cells["J" + i].Value = item.Dir;
                                                    ws.Cells["K" + i].Value = item.Dep_muni;
                                                    ws.Cells["L" + i].Value = item.MUNICIPIO;
                                                    ws.Cells["M" + i].Value = NombrePais(item.Id_pais);
                                                }
                                                if (categoriaId == 103)
                                                {
                                                    ws.Cells["N" + i].Value = Acumulado(acumuladoPorId);
                                                    i++;
                                                }
                                            }

                                        }
                                        if (CMenor > 0 && CMenor < 1000000)
                                        {
                                            ws.Cells["A" + i].Value = codigoConcepto;
                                            ws.Cells["B" + i].Value = tipoDocumentoCM;
                                            ws.Cells["C" + i].Value = nitCuantiaMenor;
                                            ws.Cells["D" + i].Value = digVer;
                                            ws.Cells[i, 5, i, 8].Value = "";
                                            ws.Cells["I" + i].Value = razonmSocial;
                                            ws.Cells["J" + i].Value = "NA";
                                            ws.Cells["k" + i].Value = codDepCm;
                                            ws.Cells["l" + i].Value = codMunCM;
                                            ws.Cells["M" + i].Value = NombrePais(1);

                                            if (categoriaId == 103)
                                            {
                                                ws.Cells["N" + i].Value = CMenor;
                                                i++;
                                            }
                                        }

                                        #endregion
                                        break;

                                    case 9:
                                        #region Formato 1010
                                        ws = pack.Workbook.Worksheets.Add("1010");
                                        //// encabezado
                                        ws.Cells["A1:P1,A2:P2,A3:P3,A4:P4,A5:P5"].Merge = true;
                                        ws.Cells["A2:P2,A3:P3,A4:P4"].Style.Font.Bold = true;
                                        ws.Cells["A2:P2"].Style.Font.Name = "Arial";
                                        ws.Cells["A2:P2"].Style.Font.Size = 14;
                                        ws.Cells["A" + 2].Value = "FORMATO 1010 INFORMACIÓN DE SOCIOS, ACCIONISTAS, COMUNEROS Y/O COOPERADOS   " + anioId;
                                        ws.Cells[1, 1, 5, 16].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[1, 1, 5, 16].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                                        ws.Cells["A2:P2,A3:P3,A4:P4,A5:P5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                        ws.Cells["A2:P2,A3:P3,A4:P4,A7:P7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells["A2:P2,A3:P3,A4:P4,A7:P7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells["A2:P2,A3:P3,A4:P4,A5:P5"].Style.WrapText = true;
                                        ws.Cells["A3:P3,A4:P4"].Style.Font.Size = 12;
                                        ws.Cells["A5:P5"].Style.Font.Size = 10;
                                        ws.Cells["A" + 3].Value = nombre;
                                        ws.Cells["A" + 4].Value = nitE;
                                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                                        ws.Cells["A6:P6"].Merge = true;
                                        ws.Cells[6, 1, 6, 16].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                                        ws.Cells[7, 1, 7, 16].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                                        ws.Cells["A7:Q7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                                        ws.Cells["A7:P7"].Style.Font.Bold = true;
                                        ws.Cells[7, 1, 7, 16].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[7, 1, 7, 16].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                                        ws.Cells["A7:P7"].Style.WrapText = true;
                                        ////fin encabezado
                                        ws.Cells["A" + 7].Value = "Concepto";
                                        ws.Cells["B" + 7].Value = "Tipo de documento";
                                        ws.Cells["C" + 7].Value = "Número identificación socio o accionista";
                                        ws.Cells["D" + 7].Value = "DV";
                                        ws.Cells["E" + 7].Value = "Primer apellido socio o accionista";
                                        ws.Cells["F" + 7].Value = "Segundo apellido socio o accionista";
                                        ws.Cells["G" + 7].Value = "Primer nombre socio o accionista";
                                        ws.Cells["H" + 7].Value = "Otros nombres socio o accionista";
                                        ws.Cells["I" + 7].Value = "Razón social ";
                                        ws.Cells["J" + 7].Value = "Dirección";
                                        ws.Cells["K" + 7].Value = "Código dpto.";
                                        ws.Cells["L" + 7].Value = "Código mcp";
                                        ws.Cells["M" + 7].Value = "País de residencia o domicilio";
                                        ws.Cells["N" + 7].Value = "Valor patrimonial acciones o aportes al 31-12";
                                        ws.Cells["O" + 7].Value = "Porcentaje de participación";
                                        ws.Cells["P" + 7].Value = "Porcentaje de participación (posición decimal)";

                                        i = 8;
                                        var auxNit9 = (from m in movimiento
                                                       orderby m.CUENTA
                                                       select new { m.Nit, m.CUENTA, m.TIPODOCUMENTO, m.APELLIDO1, m.APELLIDO2, m.NOMBRE1, m.NOMBRE2, m.Dir, m.Dep_muni, m.MUNICIPIO, m.Id_pais, m.NATURALEZA, m.DIGVER, m.NombreComercial }).Distinct().ToList();

                                        foreach (var item in auxNit9)
                                        {
                                            var dataMov2 = numCuentas.Where(x => x.Nit == item.Nit).OrderBy(x => x.Nit).ToList();
                                            debito = dataMov2.Select(x => x.DEBITO).Sum(); //acumulado por debito
                                            credito = dataMov2.Select(x => x.CREDITO).Sum();//acumulado por credito
                                            int startIndex = 0;
                                            int length = 2;
                                            string substring = (item.CUENTA).Substring(startIndex, length);

                                            //acumulado por debito credito       
                                            if (item.NATURALEZA == "D")
                                                debito_credito = debito - credito;
                                            else
                                                debito_credito = credito - debito;

                                            //acumulado por saldo final cuentas por cobrar cuentas 16 y por pagar cuentas 24
                                            if (substring == "16")
                                                saldoFinal = debito - credito;

                                            if (substring == "24")
                                                saldoFinal = credito - debito;

                                            //validacion si es menor a cero como se debe hacer

                                            if (Acumulado(acumuladoPorId) > 3000000)
                                            {
                                                ws.Cells["A" + i].Value = codigoConcepto;
                                                //validacion cuantias menores 

                                                ws.Cells["B" + i].Value = item.TIPODOCUMENTO;
                                                ws.Cells["C" + i].Value = item.Nit;
                                                ws.Cells["D" + i].Value = item.DIGVER;
                                                ws.Cells["E" + i].Value = item.APELLIDO1;
                                                ws.Cells["F" + i].Value = item.APELLIDO2;
                                                ws.Cells["G" + i].Value = item.NOMBRE1;
                                                ws.Cells["H" + i].Value = item.NOMBRE2;
                                                ws.Cells["I" + i].Value = item.NombreComercial;
                                                ws.Cells["J" + i].Value = item.Dir;
                                                ws.Cells["K" + i].Value = item.Dep_muni;
                                                ws.Cells["L" + i].Value = item.MUNICIPIO;
                                                ws.Cells["M" + i].Value = NombrePais(item.Id_pais);

                                                if (categoriaId == 104)
                                                {
                                                    ws.Cells["N" + i].Value = Acumulado(acumuladoPorId);
                                                    i++;
                                                }
                                            }

                                        }
                                        if (CMenor != 0)
                                        {
                                            ws.Cells["A" + i].Value = codigoConcepto;
                                            ws.Cells["B" + i].Value = tipoDocumentoCM;
                                            ws.Cells["C" + i].Value = nitCuantiaMenor;
                                            ws.Cells["D" + i].Value = digVer;
                                            ws.Cells[i, 5, i, 8].Value = "";
                                            ws.Cells["I" + i].Value = razonmSocial;
                                            ws.Cells["J" + i].Value = "NA";
                                            ws.Cells["k" + i].Value = codDepCm;
                                            ws.Cells["l" + i].Value = codMunCM;
                                            ws.Cells["M" + i].Value = NombrePais(1);

                                            if (categoriaId == 104)
                                            {
                                                ws.Cells["N" + i].Value = CMenor;
                                                i++;
                                            }
                                        }

                                        #endregion
                                        break;

                                    case 10:
                                        #region Formato 1011
                                        ws = pack.Workbook.Worksheets.Add("1011");
                                        //// encabezado
                                        ws.Cells["A1:I1,A2:I2,A3:I3,A4:I4,A5:I5"].Merge = true;
                                        ws.Cells["A2:I2,A3:I3,A4:I4"].Style.Font.Bold = true;
                                        ws.Cells["A2:I2"].Style.Font.Name = "Arial";
                                        ws.Cells["A2:I2"].Style.Font.Size = 12;
                                        ws.Cells["A" + 2].Value = "FORMATO 1011 INFORMACIÓN DE LAS DECLARACIONES TRIBUTARIAS   " + anioId;
                                        ws.Cells[1, 1, 5, 9].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[1, 1, 5, 9].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                                        ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                        ws.Cells["A2:I2,A3:I3,A4:I4,A7:I7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells["A2:I2,A3:I3,A4:I4,A7:I7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells["A2:I2,A3:I3,A4:I4,A5:I5"].Style.WrapText = true;
                                        ws.Cells["A3:I3,A4:I4"].Style.Font.Size = 12;
                                        ws.Cells["A5:I5"].Style.Font.Size = 10;
                                        ws.Cells["A" + 3].Value = nombre;
                                        ws.Cells["A" + 4].Value = nitE;
                                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                                        ws.Cells["A6:I6"].Merge = true;
                                        ws.Cells[6, 1, 6, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                                        ws.Cells[7, 1, 7, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                                        ws.Cells["A7:C7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                                        ws.Cells["A7:B7"].Style.Font.Bold = true;
                                        ws.Cells[7, 1, 7, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[7, 1, 7, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                                        ////fin encabezado

                                        ws.Cells["A" + 7].Value = "Concepto";
                                        ws.Cells["B" + 7].Value = "Saldos al -31-12";

                                        i = 8;
                                        var auxNit10 = (from m in movimiento
                                                        orderby m.CUENTA
                                                        select new { m.Nit, m.CUENTA, m.NATURALEZA }).Distinct().ToList();

                                        foreach (var item in auxNit10)
                                        {
                                            var dataMov2 = numCuentas.Where(x => x.Nit == item.Nit).OrderBy(x => x.Nit).ToList();
                                            debito = dataMov2.Select(x => x.DEBITO).Sum(); //acumulado por debito
                                            credito = dataMov2.Select(x => x.CREDITO).Sum();//acumulado por credito
                                            int startIndex = 0;
                                            int length = 2;
                                            string substring = (item.CUENTA).Substring(startIndex, length);

                                            //acumulado por debito credito       
                                            if (item.NATURALEZA == "D")
                                                debito_credito = debito - credito;
                                            else
                                                debito_credito = credito - debito;

                                            //acumulado por saldo final cuentas por cobrar cuentas 16 y por pagar cuentas 24
                                            if (substring == "16")
                                                saldoFinal = debito - credito;

                                            if (substring == "24")
                                                saldoFinal = credito - debito;

                                            if (Acumulado(acumuladoPorId) > 0)
                                            {
                                                ws.Cells["A" + i].Value = codigoConcepto;
                                                if (categoriaId == 105)
                                                {
                                                    ws.Cells["B" + i].Value = Acumulado(acumuladoPorId);
                                                    i++;
                                                }
                                            }

                                        }
                                        if (CMenor != 0)
                                        {
                                            ws.Cells["A" + i].Value = codigoConcepto;

                                            if (categoriaId == 105)
                                            {
                                                ws.Cells["B" + i].Value = CMenor;
                                                i++;
                                            }
                                        }

                                        #endregion
                                        break;

                                    case 11:
                                        #region formato 1012
                                        ws = pack.Workbook.Worksheets.Add("1012");
                                        //// encabezado
                                        ws.Cells["A1:K1,A2:K2,A3:K3,A4:K4,A5:K5,A6:K6"].Merge = true;
                                        ws.Cells["A2:K2,A3:K3,A4:K4,A5:K5"].Style.Font.Bold = true;
                                        ws.Cells["A2:K2,A3:K3,A4:K4,A5:K5,A6:K6"].Style.Font.Name = "Arial";
                                        ws.Cells["A2:K2"].Style.Font.Size = 12;
                                        ws.Cells["A" + 2].Value = "FORMATO 1012 INFORMACIÓN DE DECLARACIONES TRIBUTARIAS, ACCIONES, INVERSIONES";
                                        ws.Cells["A" + 3].Value = "EN BONOS TÍTULOS VALORES Y CUENTAS DE AHORRO Y CUENTAS CORRIENTES   " + anioId;
                                        ws.Cells[1, 1, 6, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[1, 1, 6, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                                        ws.Cells["A2:K2,A3:K3,A4:K4,A5:K5,A6:K6"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                        ws.Cells["A2:K2,A3:K3,A4:K4,A5:K5,A7:K7,A8:K8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells["A2:K2,A3:K3,A4:K4,A5:K5,A7:K7,A8:K8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells["A2:K2,A3:K3,A4:K4,A5:K5"].Style.WrapText = true;
                                        ws.Cells["A3:K3,A4:K4,A5:K5"].Style.Font.Size = 12;
                                        ws.Cells["A6:K6"].Style.Font.Size = 10;
                                        ws.Cells["A" + 4].Value = nombre;
                                        ws.Cells["A" + 5].Value = nitE;
                                        ws.Cells["A" + 6].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                                        ws.Cells["A7:K7"].Merge = true;
                                        ws.Cells[7, 1, 7, 11].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                                        ws.Cells[8, 1, 8, 11].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                                        ws.Cells["A8:L8"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                                        ws.Cells["A8:K8"].Style.Font.Bold = true;
                                        ws.Cells[8, 1, 8, 11].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[8, 1, 8, 11].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                                        ////fin encabezado

                                        ws.Cells["A8:J8"].Style.WrapText = true;
                                        ws.Cells["A" + 8].Value = "Concepto";
                                        ws.Cells["B" + 8].Value = "Tipo de documento";
                                        ws.Cells["C" + 8].Value = "Número identificación";
                                        ws.Cells["D" + 8].Value = "DV";
                                        ws.Cells["E" + 8].Value = "Primer apellido del informado";
                                        ws.Cells["F" + 8].Value = "Segundo apellido del informado";
                                        ws.Cells["G" + 8].Value = "Primer nombre del informado";
                                        ws.Cells["H" + 8].Value = "Otros nombres del informado";
                                        ws.Cells["I" + 8].Value = "Razón social del informado";
                                        ws.Cells["J" + 8].Value = "País de residencia o domicilio";
                                        ws.Cells["K" + 8].Value = "Valor al 31-12";


                                        i = 9;
                                        var auxNit11 = (from m in movimiento
                                                        orderby m.CUENTA
                                                        select new { m.Nit, m.CUENTA, m.TIPODOCUMENTO, m.APELLIDO1, m.APELLIDO2, m.NOMBRE1, m.NOMBRE2, m.Id_pais, m.NATURALEZA, m.DIGVER, m.NombreComercial }).Distinct().ToList();

                                        foreach (var item in auxNit11)
                                        {
                                            var dataMov2 = numCuentas.Where(x => x.Nit == item.Nit).OrderBy(x => x.Nit).ToList();
                                            debito = dataMov2.Select(x => x.DEBITO).Sum(); //acumulado por debito
                                            credito = dataMov2.Select(x => x.CREDITO).Sum();//acumulado por credito
                                            int startIndex = 0;
                                            int length = 2;
                                            string substring = (item.CUENTA).Substring(startIndex, length);

                                            //acumulado por debito credito       
                                            if (item.NATURALEZA == "D")
                                                debito_credito = debito - credito;
                                            else
                                                debito_credito = credito - debito;

                                            //acumulado por saldo final cuentas por cobrar cuentas 16 y por pagar cuentas 24
                                            if (substring == "16")
                                                saldoFinal = debito - credito;

                                            if (substring == "24")
                                                saldoFinal = credito - debito;


                                            if (Acumulado(acumuladoPorId) > 0)
                                            {
                                                ws.Cells["A" + i].Value = codigoConcepto;

                                                ws.Cells["B" + i].Value = item.TIPODOCUMENTO;
                                                ws.Cells["C" + i].Value = item.Nit;
                                                ws.Cells["D" + i].Value = item.DIGVER;
                                                ws.Cells["E" + i].Value = item.APELLIDO1;
                                                ws.Cells["F" + i].Value = item.APELLIDO2;
                                                ws.Cells["G" + i].Value = item.NOMBRE1;
                                                ws.Cells["H" + i].Value = item.NOMBRE2;
                                                ws.Cells["I" + i].Value = item.NombreComercial;
                                                ws.Cells["J" + i].Value = NombrePais(item.Id_pais);

                                                if (categoriaId == 106)
                                                {
                                                    ws.Cells["K" + i].Value = Acumulado(acumuladoPorId);
                                                    i++;
                                                }
                                            }

                                        }
                                        if (CMenor != 0)
                                        {
                                            ws.Cells["A" + i].Value = codigoConcepto;
                                            ws.Cells["B" + i].Value = tipoDocumentoCM;
                                            ws.Cells["C" + i].Value = nitCuantiaMenor;
                                            ws.Cells["D" + i].Value = digVer;
                                            ws.Cells[i, 5, i, 8].Value = "";
                                            ws.Cells["I" + i].Value = razonmSocial;
                                            ws.Cells["J" + i].Value = NombrePais(1);

                                            if (categoriaId == 106)
                                            {
                                                ws.Cells["K" + i].Value = CMenor;
                                                i++;
                                            }
                                        }
                                        #endregion formato 
                                        break;

                                    case 12:
                                        #region formato 1056
                                        ws = pack.Workbook.Worksheets.Add("1056");
                                        //// encabezado
                                        ws.Cells["A1:N1,A2:N2,A3:N3,A4:N4,A5:N5,A6:N6"].Merge = true;
                                        ws.Cells["A2:N2,A3:N3,A4:N4,A5:N5"].Style.Font.Bold = true;
                                        ws.Cells["A2:N2,A3:N3,A4:N4,A5:N5"].Style.Font.Name = "Arial";
                                        ws.Cells["A2:N2"].Style.Font.Size = 12;
                                        ws.Cells["A" + 2].Value = "FORMATO 1056 PAGOS O ABONOS EN CUENTA Y RETENCIONES ";
                                        ws.Cells["A" + 3].Value = "POR SECRETARIOS GENERALES QUE ADMNISTRAN RECURSOS DEL TESORO   " + anioId;
                                        ws.Cells[1, 1, 6, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[1, 1, 6, 14].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                                        ws.Cells["A2:N2,A3:N3,A4:N4,A5:N5,A6:N6"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                        ws.Cells["A2:N2,A3:N3,A4:N4,A5:N5,A7:N7,A8:N8"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells["A2:N2,A3:N3,A4:N4,A5:N5,A7:N7,A8:N8"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells["A2:N2,A3:N3,A4:N4,A5:N5"].Style.WrapText = true;
                                        ws.Cells["A3:N3,A4:N4,A5:N5,A6:N6"].Style.Font.Size = 12;
                                        ws.Cells["A6:N6"].Style.Font.Size = 10;
                                        ws.Cells["A" + 4].Value = nombre;
                                        ws.Cells["A" + 5].Value = nitE;
                                        ws.Cells["A" + 6].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                                        ws.Cells["A7:N7"].Merge = true;
                                        ws.Cells[7, 1, 7, 14].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                                        ws.Cells[8, 1, 8, 14].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                                        ws.Cells["A8:O8"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                                        ws.Cells["A8:N8"].Style.Font.Bold = true;
                                        ws.Cells[8, 1, 8, 14].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[8, 1, 8, 14].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                                        ////fin encabezado
                                        ws.Cells["A8:D8,H8:N8"].Style.WrapText = true;
                                        ws.Cells["A" + 8].Value = "Concepto";
                                        ws.Cells["B" + 8].Value = "Tipo de documento";
                                        ws.Cells["C" + 8].Value = "Número identificación";
                                        ws.Cells["D" + 8].Value = "Razón social del informado";
                                        ws.Cells["E" + 8].Value = "Dirección";
                                        ws.Cells["F" + 8].Value = "Código dpto.";
                                        ws.Cells["G" + 8].Value = "Código mcp";
                                        ws.Cells["H" + 8].Value = "País de residencia o domicilio";
                                        ws.Cells["I" + 8].Value = "Pago o abono en cuenta";
                                        ws.Cells["J" + 8].Value = "Iva mayor valor del costo o gasto";
                                        ws.Cells["K" + 8].Value = "Retención en la fuente practicada RENTA";
                                        ws.Cells["L" + 8].Value = "Retención en la fuente asumida en RENTA";
                                        ws.Cells["M" + 8].Value = "Retención en la fuente practicada IVA régimen común";
                                        ws.Cells["N" + 8].Value = "Retención en la fuente practicada IVA no domiciliados";

                                        i = 9;
                                        var auxNit12 = (from m in movimiento
                                                        orderby m.CUENTA
                                                        select new { m.Nit, m.CUENTA, m.TIPODOCUMENTO, m.APELLIDO1, m.APELLIDO2, m.NOMBRE1, m.NOMBRE2, m.Dir, m.Dep_muni, m.MUNICIPIO, m.Id_pais, m.NATURALEZA, m.NombreComercial }).Distinct().ToList();

                                        foreach (var item in auxNit12)
                                        {
                                            var dataMov2 = numCuentas.Where(x => x.Nit == item.Nit).OrderBy(x => x.Nit).ToList();
                                            debito = dataMov2.Select(x => x.DEBITO).Sum(); //acumulado por debito
                                            credito = dataMov2.Select(x => x.CREDITO).Sum();//acumulado por credito
                                            int startIndex = 0;
                                            int length = 2;
                                            string substring = (item.CUENTA).Substring(startIndex, length);

                                            //acumulado por debito credito       
                                            if (item.NATURALEZA == "D")
                                                debito_credito = debito - credito;
                                            else
                                                debito_credito = credito - debito;

                                            //acumulado por saldo final cuentas por cobrar cuentas 16 y por pagar cuentas 24
                                            if (substring == "16")
                                                saldoFinal = debito - credito;

                                            else if (substring == "24")
                                                saldoFinal = credito - debito;


                                            if (Acumulado(acumuladoPorId) > 0)
                                            {
                                                ws.Cells["A" + i].Value = codigoConcepto;

                                                ws.Cells["B" + i].Value = item.TIPODOCUMENTO;
                                                ws.Cells["C" + i].Value = item.Nit;
                                                ws.Cells["D" + i].Value = item.NombreComercial;
                                                ws.Cells["E" + i].Value = item.Dir;
                                                ws.Cells["F" + i].Value = item.Dep_muni;
                                                ws.Cells["G" + i].Value = item.MUNICIPIO;
                                                ws.Cells["H" + i].Value = NombrePais(item.Id_pais);

                                                if (categoriaId == 107)
                                                {
                                                    ws.Cells["I" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 10, i, 14].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 108)
                                                {
                                                    ws.Cells["I" + i].Value = 0;
                                                    ws.Cells["J" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 11, i, 14].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 109)
                                                {
                                                    ws.Cells[i, 9, i, 10].Value = 0;
                                                    ws.Cells["K" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 12, i, 14].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 110)
                                                {
                                                    ws.Cells[i, 9, i, 11].Value = 0;
                                                    ws.Cells["L" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 13, i, 14].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 111)
                                                {
                                                    ws.Cells[i, 9, i, 12].Value = 0;
                                                    ws.Cells["M" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells["N" + i].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 112)
                                                {
                                                    ws.Cells[i, 9, i, 13].Value = 0;
                                                    ws.Cells["N" + i].Value = Acumulado(acumuladoPorId);
                                                    i++;
                                                }
                                            }

                                        }
                                        if (CMenor != 0)
                                        {
                                            ws.Cells["A" + i].Value = codigoConcepto;
                                            ws.Cells["B" + i].Value = tipoDocumentoCM;
                                            ws.Cells["C" + i].Value = nitCuantiaMenor;
                                            ws.Cells["D" + i].Value = razonmSocial;
                                            ws.Cells["E" + i].Value = "NA";
                                            ws.Cells["F" + i].Value = codDepCm;
                                            ws.Cells["G" + i].Value = codMunCM;
                                            ws.Cells["H" + i].Value = NombrePais(1);

                                            if (categoriaId == 107)
                                            {
                                                ws.Cells["I" + i].Value = CMenor;
                                                ws.Cells[i, 10, i, 14].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 108)
                                            {
                                                ws.Cells["I" + i].Value = 0;
                                                ws.Cells["J" + i].Value = CMenor;
                                                ws.Cells[i, 11, i, 14].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 109)
                                            {
                                                ws.Cells[i, 9, i, 10].Value = 0;
                                                ws.Cells["K" + i].Value = CMenor;
                                                ws.Cells[i, 12, i, 14].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 110)
                                            {
                                                ws.Cells[i, 9, i, 11].Value = 0;
                                                ws.Cells["L" + i].Value = CMenor;
                                                ws.Cells[i, 13, i, 14].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 111)
                                            {
                                                ws.Cells[i, 9, i, 12].Value = 0;
                                                ws.Cells["M" + i].Value = CMenor;
                                                ws.Cells["N" + i].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 112)
                                            {
                                                ws.Cells[i, 9, i, 13].Value = 0;
                                                ws.Cells["N" + i].Value = CMenor;
                                                i++;
                                            }
                                        }

                                        #endregion
                                        break;

                                    case 13:
                                        #region Formato 1647
                                        ws = pack.Workbook.Worksheets.Add("1647");
                                        //// encabezado
                                        ws.Cells["A1:X1,A2:X2,A3:X3,A4:X4,A5:X5,A6:X6"].Merge = true;
                                        ws.Cells["A2:X2,A3:X3,A4:X4,A7:X7"].Style.Font.Bold = true;
                                        ws.Cells["A2:X2,A3:X3,A4:X4,A5:X5"].Style.Font.Name = "Arial";
                                        ws.Cells["A2:X2"].Style.Font.Size = 14;
                                        ws.Cells["A" + 2].Value = "FORMATO 1647 INGRESOS RECIBIDOS PARA TERCEROS    " + anioId;
                                        ws.Cells[1, 1, 5, 24].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[1, 1, 5, 24].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                                        ws.Cells["A2:X2,A3:X3,A4:X4,A5:X5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                        ws.Cells["A2:X2,A3:X3,A4:X4,A7:X7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells["A2:X2,A3:X3,A4:X4,A7:X7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells["A2:X2,A3:X3,A4:X4,A5:X5"].Style.WrapText = true;
                                        ws.Cells["A3:X3,A4:X4"].Style.Font.Size = 12;
                                        ws.Cells["A5:X5"].Style.Font.Size = 10;
                                        ws.Cells["A" + 3].Value = nombre;
                                        ws.Cells["A" + 4].Value = nitE;
                                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                                        ws.Cells["A6:X6"].Merge = true;
                                        ws.Cells[6, 1, 6, 24].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                                        ws.Cells[7, 1, 7, 24].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                                        ws.Cells["A7:Y7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                                        ws.Cells[7, 1, 7, 24].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[7, 1, 7, 24].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                                        ////fin encabezado

                                        ws.Cells["A7:X7"].Style.WrapText = true;
                                        ws.Cells["A" + 7].Value = "Concepto";
                                        ws.Cells["B" + 7].Value = "Tipo de documento de quien se recibió el ingreso";
                                        ws.Cells["C" + 7].Value = "Número identificación de quien recibe el ingreso";
                                        ws.Cells["D" + 7].Value = "DV";
                                        ws.Cells["E" + 7].Value = "Primer apellido de quien se recibe el ingreso";
                                        ws.Cells["F" + 7].Value = "Segundo apellido de quien se recibe el ingreso";
                                        ws.Cells["G" + 7].Value = "Primer nombre de quien se recibe el ingreso";
                                        ws.Cells["H" + 7].Value = "Otros nombres de quien se recibe el ingreso";
                                        ws.Cells["I" + 7].Value = "Razón social de quien se recibe el ingreso";
                                        ws.Cells["J" + 7].Value = "País de residencia o domicilio";
                                        ws.Cells["K" + 7].Value = "Valor total de la operación";
                                        ws.Cells["L" + 7].Value = "Valor ingreso reintegrado transferido distribuido al tercero";
                                        ws.Cells["M" + 7].Value = "Valor retención reintegrada transferida  distribuida al tercero";
                                        ws.Cells["N" + 7].Value = "Tipo de documento del tercero para quien se recibió el ingreso";
                                        ws.Cells["O" + 7].Value = "Identificación del tercero para quien se recibió el ingreso";
                                        ws.Cells["P" + 7].Value = "Primer apellido del tercero para quien se recibió el ingreso";
                                        ws.Cells["Q" + 7].Value = "Segundo apellido del tercero par quien se recibió el ingreso";
                                        ws.Cells["R" + 7].Value = "Primer nombre del tercero para quien se recibió el ingreso";
                                        ws.Cells["S" + 7].Value = "Otros nombres del tercero para quien se recibió el ingreso";
                                        ws.Cells["T" + 7].Value = "Razón social del tercero para quien se recibió el ingreso";
                                        ws.Cells["U" + 7].Value = "Dirección";
                                        ws.Cells["V" + 7].Value = "Código dpto.";
                                        ws.Cells["W" + 7].Value = "Código mcp";
                                        ws.Cells["X" + 7].Value = "País de residencia o domicilio";

                                        i = 8;
                                        var auxNit13 = (from m in movimiento
                                                        orderby m.CUENTA
                                                        select new { m.Nit, m.CUENTA, m.TIPODOCUMENTO, m.APELLIDO1, m.APELLIDO2, m.NOMBRE1, m.NOMBRE2, m.Dir, m.Dep_muni, m.MUNICIPIO, m.Id_pais, m.EMAIL, m.NATURALEZA, m.DIGVER, m.NombreComercial }).Distinct().ToList();

                                        foreach (var item in auxNit13)
                                        {
                                            var dataMov2 = numCuentas.Where(x => x.Nit == item.Nit).OrderBy(x => x.Nit).ToList();
                                            debito = dataMov2.Select(x => x.DEBITO).Sum(); //acumulado por debito
                                            credito = dataMov2.Select(x => x.CREDITO).Sum();//acumulado por credito
                                            int startIndex = 0;
                                            int length = 2;
                                            string substring = (item.CUENTA).Substring(startIndex, length);

                                            //acumulado por debito credito       
                                            if (item.NATURALEZA == "D")
                                                debito_credito = debito - credito;
                                            else
                                                debito_credito = credito - debito;

                                            //acumulado por saldo final cuentas por cobrar cuentas 16 y por pagar cuentas 24
                                            if (substring == "16")
                                                saldoFinal = debito - credito;

                                            else if (substring == "24")
                                                saldoFinal = credito - debito;


                                            if (Acumulado(acumuladoPorId) > 1000000)
                                            {
                                                ws.Cells["A" + i].Value = codigoConcepto;

                                                ws.Cells["B" + i].Value = item.TIPODOCUMENTO;
                                                ws.Cells["C" + i].Value = item.Nit;
                                                ws.Cells["D" + i].Value = item.DIGVER;
                                                ws.Cells["E" + i].Value = item.APELLIDO1;
                                                ws.Cells["F" + i].Value = item.APELLIDO2;
                                                ws.Cells["G" + i].Value = item.NOMBRE1;
                                                ws.Cells["H" + i].Value = item.NOMBRE2;
                                                ws.Cells["I" + i].Value = item.NombreComercial;
                                                ws.Cells["J" + i].Value = NombrePais(item.Id_pais);

                                                if (categoriaId == 113)
                                                {
                                                    ws.Cells["K" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 12, i, 13].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 114)
                                                {
                                                    ws.Cells["K" + i].Value = 0;
                                                    ws.Cells["L" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells["M" + i].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 115)
                                                {
                                                    ws.Cells[i, 11, i, 12].Value = 0;
                                                    ws.Cells["M" + i].Value = Acumulado(acumuladoPorId);
                                                    i++;
                                                }
                                            }
                                        }
                                        if (CMenor != 0)
                                        {
                                            ws.Cells["A" + i].Value = codigoConcepto;
                                            ws.Cells["B" + i].Value = tipoDocumentoCM;
                                            ws.Cells["C" + i].Value = nitCuantiaMenor;
                                            ws.Cells["D" + i].Value = digVer;
                                            ws.Cells[i, 5, i, 8].Value = "";
                                            ws.Cells["I" + i].Value = razonmSocial;
                                            ws.Cells["J" + i].Value = NombrePais(1);

                                            if (categoriaId == 113)
                                            {
                                                ws.Cells["K" + i].Value = CMenor;
                                                ws.Cells[i, 12, i, 13].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 114)
                                            {
                                                ws.Cells["K" + i].Value = 0;
                                                ws.Cells["L" + i].Value = CMenor;
                                                ws.Cells["M" + i].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 115)
                                            {
                                                ws.Cells[i, 11, i, 12].Value = 0;
                                                ws.Cells["M" + i].Value = CMenor;
                                                i++;
                                            }
                                        }

                                        #endregion
                                        break;

                                    case 14:
                                        #region Formato 2275
                                        ws = pack.Workbook.Worksheets.Add("2275");
                                        //// encabezado
                                        ws.Cells["A1:O1,A2:O2,A3:O3,A4:O4,A5:O5,A6:O6"].Merge = true;
                                        ws.Cells["A2:O2,A3:O3,A4:O4,A7:O7"].Style.Font.Bold = true;
                                        ws.Cells["A2:O2,A3:O3,A4:O4,A5:O5"].Style.Font.Name = "Arial";
                                        ws.Cells["A2:O2"].Style.Font.Size = 14;
                                        ws.Cells["A" + 2].Value = "FORMATO 2275 INGRESOS NO CONSTITUTIVOS DE RENTA NI GANANCIA OCASIONAL    " + anioId;
                                        ws.Cells[1, 1, 5, 15].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[1, 1, 5, 15].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                                        ws.Cells["A2:O2,A3:O3,A4:O4,A5:O5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                        ws.Cells["A2:O2,A3:O3,A4:O4,A7:O7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells["A2:O2,A3:O3,A4:O4,A7:O7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells["A2:O2,A3:O3,A4:O4,A5:O5"].Style.WrapText = true;
                                        ws.Cells["A3:O3,A4:O4"].Style.Font.Size = 12;
                                        ws.Cells["A5:O5"].Style.Font.Size = 10;
                                        ws.Cells["A" + 3].Value = nombre;
                                        ws.Cells["A" + 4].Value = nitE;
                                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                                        ws.Cells["A6:O6"].Merge = true;
                                        ws.Cells[6, 1, 6, 15].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                                        ws.Cells[7, 1, 7, 15].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                                        ws.Cells["A7:O7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                                        ws.Cells[7, 1, 7, 15].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[7, 1, 7, 15].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                                        ////fin encabezado

                                        ws.Cells["A7:O7"].Style.WrapText = true;
                                        ws.Cells["A" + 7].Value = "Concepto";
                                        ws.Cells["B" + 7].Value = "Tipo de documento del tercero";
                                        ws.Cells["C" + 7].Value = "Número identificación del tercero";
                                        ws.Cells["D" + 7].Value = "Primer apellido";
                                        ws.Cells["E" + 7].Value = "Segundo apellido";
                                        ws.Cells["F" + 7].Value = "Primer nombre";
                                        ws.Cells["G" + 7].Value = "Otros nombres";
                                        ws.Cells["H" + 7].Value = "Razón social";
                                        ws.Cells["I" + 7].Value = "Dirección";
                                        ws.Cells["J" + 7].Value = "Código dpto.";
                                        ws.Cells["K" + 7].Value = "Código mcp";
                                        ws.Cells["L" + 7].Value = "Código país";
                                        ws.Cells["M" + 7].Value = "Correo electrónico";
                                        ws.Cells["N" + 7].Value = "Valor total del ingreso";
                                        ws.Cells["O" + 7].Value = "Valor del ingreso no constitutivo de renta ni ganancia ocasional Solicitado ";

                                        i = 8;
                                        var auxNit14 = (from m in movimiento
                                                        orderby m.CUENTA
                                                        select new { m.Nit, m.CUENTA, m.TIPODOCUMENTO, m.APELLIDO1, m.APELLIDO2, m.NOMBRE1, m.NOMBRE2, m.Dir, m.Dep_muni, m.MUNICIPIO, m.Id_pais, m.EMAIL, m.NATURALEZA, m.NombreComercial }).Distinct().ToList();

                                        foreach (var item in auxNit14)
                                        {
                                            var dataMov2 = numCuentas.Where(x => x.Nit == item.Nit).OrderBy(x => x.Nit).ToList();
                                            debito = dataMov2.Select(x => x.DEBITO).Sum(); //acumulado por debito
                                            credito = dataMov2.Select(x => x.CREDITO).Sum();//acumulado por credito
                                            int startIndex = 0;
                                            int length = 2;
                                            string substring = (item.CUENTA).Substring(startIndex, length);

                                            //acumulado por debito credito       
                                            if (item.NATURALEZA == "D")
                                                debito_credito = debito - credito;
                                            else
                                                debito_credito = credito - debito;

                                            //acumulado por saldo final cuentas por cobrar cuentas 16 y por pagar cuentas 24
                                            if (substring == "16")
                                                saldoFinal = debito - credito;

                                            else if (substring == "24")
                                                saldoFinal = credito - debito;

                                            //validacion si es menor a cero como se debe hacer

                                            if (Acumulado(acumuladoPorId) > 0)
                                            {
                                                ws.Cells["A" + i].Value = codigoConcepto;

                                                ws.Cells["B" + i].Value = item.TIPODOCUMENTO;
                                                ws.Cells["C" + i].Value = item.Nit;
                                                ws.Cells["D" + i].Value = item.APELLIDO1;
                                                ws.Cells["E" + i].Value = item.APELLIDO2;
                                                ws.Cells["F" + i].Value = item.NOMBRE1;
                                                ws.Cells["G" + i].Value = item.NOMBRE2;
                                                ws.Cells["H" + i].Value = item.NombreComercial;
                                                ws.Cells["I" + i].Value = item.Dir;
                                                ws.Cells["J" + i].Value = item.Dep_muni;
                                                ws.Cells["K" + i].Value = item.MUNICIPIO;
                                                ws.Cells["L" + i].Value = CodigoPais(item.Id_pais);
                                                ws.Cells["M" + i].Value = item.EMAIL;

                                                if (categoriaId == 116)
                                                {
                                                    ws.Cells["N" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells["O" + i].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 117)
                                                {
                                                    ws.Cells["N" + i].Value = 0;
                                                    ws.Cells["O" + i].Value = Acumulado(acumuladoPorId);
                                                    i++;
                                                }
                                            }
                                        }
                                        if (CMenor != 0)
                                        {
                                            ws.Cells["A" + i].Value = codigoConcepto;
                                            ws.Cells["B" + i].Value = tipoDocumentoCM;
                                            ws.Cells["C" + i].Value = nitCuantiaMenor;
                                            ws.Cells[i, 4, i, 7].Value = "";
                                            ws.Cells["H" + i].Value = razonmSocial;
                                            ws.Cells["I" + i].Value = "NA";
                                            ws.Cells["J" + i].Value = codDepCm;
                                            ws.Cells["K" + i].Value = codMunCM;
                                            ws.Cells["L" + i].Value = CodigoPais(1);
                                            ws.Cells["M" + i].Value = "NA";
                                            if (categoriaId == 116)
                                            {
                                                ws.Cells["N" + i].Value = CMenor;
                                                ws.Cells["O" + i].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 117)
                                            {
                                                ws.Cells["N" + i].Value = 0;
                                                ws.Cells["O" + i].Value = CMenor;
                                                i++;
                                            }
                                        }

                                        #endregion
                                        break;

                                    case 15:
                                        #region Formato 2276
                                        ws = pack.Workbook.Worksheets.Add("2276");
                                        //// encabezado
                                        ws.Cells["A1:AK1,A2:AK2,A3:AK3,A4:AK4,A5:AK5,A6:AK6"].Merge = true;
                                        ws.Cells["A2:AK2,A3:AK3,A4:AK4,A7:AK7"].Style.Font.Bold = true;
                                        ws.Cells["A2:AK2,A3:AK3,A4:AK4,A5:AK5"].Style.Font.Name = "Arial";
                                        ws.Cells["A2:AK2"].Style.Font.Size = 14;
                                        ws.Cells["A" + 2].Value = "FORMATO 2276 INFORMACIÓN DE RENTAS DE TRABAJO Y PENSIONES    " + anioId;
                                        ws.Cells[1, 1, 5, 37].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[1, 1, 5, 37].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                                        ws.Cells["A2:AK2,A3:AK3,A4:AK4,A5:AK5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                        ws.Cells["A2:AK2,A3:AK3,A4:AK4,A7:AK7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                        ws.Cells["A2:AK2,A3:AK3,A4:AK4,A7:AK7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                        ws.Cells["A2:AK2,A3:AK3,A4:AK4,A5:AK5"].Style.WrapText = true;
                                        ws.Cells["A3:AK3,A4:AK4"].Style.Font.Size = 12;
                                        ws.Cells["A5:AK5"].Style.Font.Size = 10;
                                        ws.Cells["A" + 3].Value = nombre;
                                        ws.Cells["A" + 4].Value = nitE;
                                        ws.Cells["A" + 5].Value = "Fecha generado el reporte: " + fechaAct.ToShortDateString();
                                        ws.Cells["A6:AK6"].Merge = true;
                                        ws.Cells[6, 1, 6, 37].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                                        ws.Cells[7, 1, 7, 37].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                                        ws.Cells["A7:AK7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                                        ws.Cells[7, 1, 7, 37].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                        ws.Cells[7, 1, 7, 37].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                                        ////fin encabezado 
                                        ws.Cells["A7:AK7"].Style.WrapText = true;
                                        ws.Cells["A" + 7].Value = "Entidad Informante";
                                        ws.Cells["B" + 7].Value = "Tipo de documento del beneficiario";
                                        ws.Cells["C" + 7].Value = "Número identificación del beneficiario";
                                        ws.Cells["D" + 7].Value = "Primer apellido del beneficiario";
                                        ws.Cells["E" + 7].Value = "Segundo apellido del beneficiario";
                                        ws.Cells["F" + 7].Value = "Primer nombre del beneficiario";
                                        ws.Cells["G" + 7].Value = "Otros nombres del beneficiario";
                                        ws.Cells["H" + 7].Value = "Dirección del beneficiario";
                                        ws.Cells["I" + 7].Value = "Departamento del beneficiario.";
                                        ws.Cells["J" + 7].Value = "Municipio del beneficiario";
                                        ws.Cells["K" + 7].Value = "País del beneficiario";
                                        ws.Cells["L" + 7].Value = "Pagos por Salarios";
                                        ws.Cells["M" + 7].Value = "Pagos por emolumentos eclesiásticos";
                                        ws.Cells["N" + 7].Value = "Pagos por honorarios";
                                        ws.Cells["O" + 7].Value = "Pagos por servicios";
                                        ws.Cells["P" + 7].Value = "Pagos por comisiones";
                                        ws.Cells["Q" + 7].Value = "Pagos por prestaciones sociales";
                                        ws.Cells["R" + 7].Value = "Pagos por viáticos";
                                        ws.Cells["S" + 7].Value = "Pagos por gastos de representación";
                                        ws.Cells["T" + 7].Value = "Pagos por compensaciones trabajo asociado cooperativo";
                                        ws.Cells["U" + 7].Value = "Otros pagos";
                                        ws.Cells["V" + 7].Value = "Cesantías e intereses de cesantías efectivamente pagadas, consignadas o reconocidas en el periodo";
                                        ws.Cells["W" + 7].Value = "Pensiones de Jubilación, vejez o invalidez";
                                        ws.Cells["X" + 7].Value = "Total Ingresos brutos de rentas de trabajo y pensión";
                                        ws.Cells["Y" + 7].Value = "Aportes Obligatorios por Salud";
                                        ws.Cells["Z" + 7].Value = "Aportes obligatorios a fondos de pensiones y solidaridad pensional y Aportes voluntarios al - RAIS";
                                        ws.Cells["AA" + 7].Value = "Aportes voluntarios a fondos de pensiones voluntarias";
                                        ws.Cells["AB" + 7].Value = "Aportes a cuentas AFC.";
                                        ws.Cells["AC" + 7].Value = "Aportes a cuentas AVC.";
                                        ws.Cells["AD" + 7].Value = "Valor de las retenciones en la fuente por pagos de rentas de trabajo o pensiones";
                                        ws.Cells["AE" + 7].Value = "Pagos realizados con bonos electronicos o de papel de servicio, cheques, tarjetas, vales, etc.";
                                        ws.Cells["AF" + 7].Value = "Apoyos economicos no reembolsables o condonados, entregados por el Estado o financiados con recursos públicos, para financiar programas educativos";
                                        ws.Cells["AG" + 7].Value = "Pagos por alimentación mayores a 41 UVT";
                                        ws.Cells["AH" + 7].Value = "Pagos por alimentación hasta 41 UVT";
                                        ws.Cells["AI" + 7].Value = "Indicación del fedeicomiso o contrato";
                                        ws.Cells["AJ" + 7].Value = "Tipo de documento participante en contrato de colaboración";
                                        ws.Cells["AK" + 7].Value = "Identificación  participante en contrato de colaboración";

                                        i = 8;
                                        var auxNit15 = (from m in movimiento
                                                        orderby m.CUENTA
                                                        select new { m.Nit, m.CUENTA, m.TIPODOCUMENTO, m.APELLIDO1, m.APELLIDO2, m.NOMBRE1, m.NOMBRE2, m.Dir, m.Dep_muni, m.MUNICIPIO, m.Id_pais, m.EMAIL, m.NATURALEZA, m.NombreComercial }).Distinct().ToList();

                                        foreach (var item in auxNit15)
                                        {
                                            var dataMov2 = numCuentas.Where(x => x.Nit == item.Nit).OrderBy(x => x.Nit).ToList();
                                            debito = dataMov2.Select(x => x.DEBITO).Sum(); //acumulado por debito
                                            credito = dataMov2.Select(x => x.CREDITO).Sum();//acumulado por credito
                                            int startIndex = 0;
                                            int length = 2;
                                            string substring = (item.CUENTA).Substring(startIndex, length);

                                            //acumulado por debito credito       
                                            if (item.NATURALEZA == "D")
                                                debito_credito = debito - credito;
                                            else
                                                debito_credito = credito - debito;

                                            //acumulado por saldo final cuentas por cobrar cuentas 16 y por pagar cuentas 24
                                            if (substring == "16")
                                                saldoFinal = debito - credito;

                                            else if (substring == "24")
                                                saldoFinal = credito - debito;

                                            if (Acumulado(acumuladoPorId) > 0)
                                            {

                                                ws.Cells["B" + i].Value = item.TIPODOCUMENTO;
                                                ws.Cells["C" + i].Value = item.Nit;
                                                ws.Cells["D" + i].Value = item.APELLIDO1;
                                                ws.Cells["E" + i].Value = item.APELLIDO2;
                                                ws.Cells["F" + i].Value = item.NOMBRE1;
                                                ws.Cells["G" + i].Value = item.NOMBRE2;
                                                ws.Cells["H" + i].Value = item.Dir;
                                                ws.Cells["I" + i].Value = item.Dep_muni;
                                                ws.Cells["J" + i].Value = item.MUNICIPIO;
                                                ws.Cells["K" + i].Value = NombrePais(item.Id_pais);

                                                if (categoriaId == 59)
                                                {
                                                    ws.Cells["L" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 13, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 60)
                                                {
                                                    ws.Cells["L" + i].Value = 0;
                                                    ws.Cells["M" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 14, i, 34].Value = 0;
                                                    i++;
                                                }

                                                else if (categoriaId == 61)
                                                {
                                                    ws.Cells[i, 12, i, 13].Value = 0;
                                                    ws.Cells["N" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 15, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 62)
                                                {
                                                    ws.Cells[i, 12, i, 14].Value = 0;
                                                    ws.Cells["O" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 16, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 63)
                                                {
                                                    ws.Cells[i, 12, i, 15].Value = 0;
                                                    ws.Cells["P" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 17, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 64)
                                                {
                                                    ws.Cells[i, 12, i, 16].Value = 0;
                                                    ws.Cells["Q" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 18, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 65)
                                                {
                                                    ws.Cells[i, 12, i, 17].Value = 0;
                                                    ws.Cells["R" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 19, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 66)
                                                {
                                                    ws.Cells[i, 12, i, 18].Value = 0;
                                                    ws.Cells["S" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 20, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 67)
                                                {
                                                    ws.Cells[i, 12, i, 19].Value = 0;
                                                    ws.Cells["T" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 21, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 68)
                                                {
                                                    ws.Cells[i, 12, i, 20].Value = 0;
                                                    ws.Cells["U" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 22, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 69)
                                                {
                                                    ws.Cells[i, 12, i, 21].Value = 0;
                                                    ws.Cells["V" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 23, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 70)
                                                {
                                                    ws.Cells[i, 12, i, 22].Value = 0;
                                                    ws.Cells["W" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 24, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 71)
                                                {
                                                    ws.Cells[i, 12, i, 23].Value = 0;
                                                    ws.Cells["X" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 25, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 72)
                                                {
                                                    ws.Cells[i, 12, i, 24].Value = 0;
                                                    ws.Cells["Y" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 26, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 73)
                                                {
                                                    ws.Cells[i, 12, i, 25].Value = 0;
                                                    ws.Cells["Z" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 27, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 74)
                                                {
                                                    ws.Cells[i, 12, i, 26].Value = 0;
                                                    ws.Cells["AA" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 28, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 75)
                                                {
                                                    ws.Cells[i, 12, i, 27].Value = 0;
                                                    ws.Cells["AB" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 29, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 76)
                                                {
                                                    ws.Cells[i, 12, i, 28].Value = 0;
                                                    ws.Cells["AC" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 30, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 77)
                                                {
                                                    ws.Cells[i, 12, i, 29].Value = 0;
                                                    ws.Cells["AD" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 31, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 78)
                                                {
                                                    ws.Cells[i, 12, i, 30].Value = 0;
                                                    ws.Cells["AE" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 32, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 79)
                                                {
                                                    ws.Cells[i, 12, i, 31].Value = 0;
                                                    ws.Cells["AF" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells[i, 33, i, 34].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 80)
                                                {
                                                    ws.Cells[i, 12, i, 32].Value = 0;
                                                    ws.Cells["AG" + i].Value = Acumulado(acumuladoPorId);
                                                    ws.Cells["AH" + i].Value = 0;
                                                    i++;
                                                }
                                                else if (categoriaId == 81)
                                                {
                                                    ws.Cells[i, 12, i, 33].Value = 0;
                                                    ws.Cells["AH" + i].Value = Acumulado(acumuladoPorId);
                                                    i++;
                                                }
                                            }
                                        }
                                        if (CMenor != 0)
                                        {
                                            ws.Cells["A" + i].Value = "";
                                            ws.Cells["B" + i].Value = tipoDocumentoCM;
                                            ws.Cells["C" + i].Value = nitCuantiaMenor;
                                            ws.Cells[i, 4, i, 7].Value = "";
                                            ws.Cells["G" + i].Value = razonmSocial;
                                            ws.Cells["H" + i].Value = "NA";
                                            ws.Cells["I" + i].Value = codDepCm;
                                            ws.Cells["J" + i].Value = codMunCM;
                                            ws.Cells["K" + i].Value = NombrePais(1);

                                            if (categoriaId == 59)
                                            {
                                                ws.Cells["L" + i].Value = CMenor;
                                                ws.Cells[i, 13, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 60)
                                            {
                                                ws.Cells["L" + i].Value = "0";
                                                ws.Cells["M" + i].Value = CMenor;
                                                ws.Cells[i, 14, i, 34].Value = 0;
                                                i++;
                                            }

                                            else if (categoriaId == 61)
                                            {
                                                ws.Cells[i, 12, i, 13].Value = 0;
                                                ws.Cells["N" + i].Value = CMenor;
                                                ws.Cells[i, 15, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 62)
                                            {
                                                ws.Cells[i, 12, i, 14].Value = 0;
                                                ws.Cells["O" + i].Value = CMenor;
                                                ws.Cells[i, 16, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 63)
                                            {
                                                ws.Cells[i, 12, i, 15].Value = 0;
                                                ws.Cells["P" + i].Value = CMenor;
                                                ws.Cells[i, 17, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 64)
                                            {
                                                ws.Cells[i, 12, i, 16].Value = 0;
                                                ws.Cells["Q" + i].Value = CMenor;
                                                ws.Cells[i, 18, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 65)
                                            {
                                                ws.Cells[i, 12, i, 17].Value = 0;
                                                ws.Cells["R" + i].Value = CMenor;
                                                ws.Cells[i, 19, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 66)
                                            {
                                                ws.Cells[i, 12, i, 18].Value = 0;
                                                ws.Cells["S" + i].Value = CMenor;
                                                ws.Cells[i, 20, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 67)
                                            {
                                                ws.Cells[i, 12, i, 19].Value = 0;
                                                ws.Cells["T" + i].Value = CMenor;
                                                ws.Cells[i, 21, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 68)
                                            {
                                                ws.Cells[i, 12, i, 20].Value = 0;
                                                ws.Cells["U" + i].Value = CMenor;
                                                ws.Cells[i, 22, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 69)
                                            {
                                                ws.Cells[i, 12, i, 21].Value = 0;
                                                ws.Cells["V" + i].Value = CMenor;
                                                ws.Cells[i, 23, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 70)
                                            {
                                                ws.Cells[i, 12, i, 22].Value = 0;
                                                ws.Cells["W" + i].Value = CMenor;
                                                ws.Cells[i, 24, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 71)
                                            {
                                                ws.Cells[i, 12, i, 23].Value = 0;
                                                ws.Cells["X" + i].Value = CMenor;
                                                ws.Cells[i, 25, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 72)
                                            {
                                                ws.Cells[i, 12, i, 24].Value = 0;
                                                ws.Cells["Y" + i].Value = CMenor;
                                                ws.Cells[i, 26, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 73)
                                            {
                                                ws.Cells[i, 12, i, 25].Value = 0;
                                                ws.Cells["Z" + i].Value = CMenor;
                                                ws.Cells[i, 27, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 74)
                                            {
                                                ws.Cells[i, 12, i, 26].Value = 0;
                                                ws.Cells["AA" + i].Value = CMenor;
                                                ws.Cells[i, 28, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 75)
                                            {
                                                ws.Cells[i, 12, i, 27].Value = 0;
                                                ws.Cells["AB" + i].Value = CMenor;
                                                ws.Cells[i, 29, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 76)
                                            {
                                                ws.Cells[i, 12, i, 28].Value = 0;
                                                ws.Cells["AC" + i].Value = CMenor;
                                                ws.Cells[i, 30, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 77)
                                            {
                                                ws.Cells[i, 12, i, 29].Value = 0;
                                                ws.Cells["AD" + i].Value = CMenor;
                                                ws.Cells[i, 31, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 78)
                                            {
                                                ws.Cells[i, 12, i, 30].Value = 0;
                                                ws.Cells["AE" + i].Value = CMenor;
                                                ws.Cells[i, 32, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 79)
                                            {
                                                ws.Cells[i, 12, i, 31].Value = 0;
                                                ws.Cells["AF" + i].Value = CMenor;
                                                ws.Cells[i, 33, i, 34].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 80)
                                            {
                                                ws.Cells[i, 12, i, 32].Value = 0;
                                                ws.Cells["AG" + i].Value = CMenor;
                                                ws.Cells["AH" + i].Value = 0;
                                                i++;
                                            }
                                            else if (categoriaId == 81)
                                            {
                                                ws.Cells[i, 12, i, 33].Value = 0;
                                                ws.Cells["AH" + i].Value = CMenor;
                                                i++;
                                            }

                                        }

                                        #endregion
                                        break;
                                }
                                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                                var ms = new System.IO.MemoryStream();
                                pack.SaveAs(ms);
                                ms.WriteTo(Response.OutputStream);
                            }
                            Response.End();
                            return RedirectToAction("/MediosMagneticos/MMConfiguracion/Index");
                        }

                    case 2:
                        var j = 0;
                        var baseacum = db.acumuladopor.Find(acumuladoPorId);
                        var acum = baseacum.nombre;
                        using (var ctx = new AccountingContext())
                        {
                            Response.Clear();
                            Response.ClearContent();
                            Response.ClearHeaders();
                            Response.Buffer = true;
                            Response.ContentEncoding = System.Text.Encoding.UTF8;
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename=InformeCalculosRealizados.xlsx");


                            using (ExcelPackage pack = new ExcelPackage())

                            {
                                ExcelWorksheet ws = pack.Workbook.Worksheets.Add("Cálculos Realizados");
                                ws.Cells["A1:F1,A2:F2,A3:F3,A4:F4"].Merge = true;
                                ws.Cells["A2:F4,A3:F3,A4:F4,A7:F7"].Style.Font.Bold = true;
                                ws.Cells["A2:F4,A3:F3,A4:F4"].Style.Font.Name = "Arial";
                                ws.Cells["A2:F4,A3:F3,A4:F4"].Style.Font.Size = 12;
                                ws.Cells[1, 1, 5, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[1, 1, 5, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#08AED6"));
                                ws.Cells["A2:F2,A3:F3,A4:F4,A5:F5"].Style.Font.Color.SetColor(System.Drawing.Color.White);
                                ws.Cells["A2:F2,A3:F3,A4:F4,A7:F7"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                ws.Cells["A2:F2,A3:F3,A4:F4,A7:F7"].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                ws.Cells["A" + 2].Value = "INFORME DE CÁLCULOS REALIZADOS    "+anioId; 
                                ws.Cells["A" + 3].Value = "FORMATO: " + codigoFormato + " - CONCEPTO: " + codigoConcepto + " - CUENTA: " + cuentaId;
                                ws.Cells["A" + 4].Value = "ACUMULADO POR: " + acum;
                                ws.Cells["A5:F5"].Style.Font.Size = 10;
                                ws.Cells["A" + 5].Value = "Fecha generado el reporte " +fechaAct.ToShortDateString();
                                ws.Cells["A6:F6"].Merge = true;
                                ws.Cells[6, 1, 6, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde superior
                                ws.Cells[7, 1, 7, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;//borde inferior
                                ws.Cells["A7:G7"].Style.Border.Left.Style = ExcelBorderStyle.Thin;//bordes laterales
                                ws.Cells[7, 1, 7, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[7, 1, 7, 6].Style.Fill.BackgroundColor.SetColor(System.Drawing.ColorTranslator.FromHtml("#D3DFE1"));
                                ws.Cells["A" + 7].Value = "Formato";
                                ws.Cells["B" + 7].Value = "Concepto";
                                ws.Cells["C" + 7].Value = "Tipo de documento";
                                ws.Cells["D" + 7].Value = "Numero de Identificación";
                                ws.Cells["E" + 7].Value = "Débito";
                                ws.Cells["F" + 7].Value = "Crédito";

                                j = 8;
                                var numCuentas = (from m in movimiento
                                                  orderby m.CUENTA
                                                  select new { m.Nit, m.DEBITO, m.CREDITO, m.CUENTA }).ToList();
                                var auxNit = (from m in movimiento
                                              orderby m.CUENTA
                                              select new { m.Nit, m.CUENTA, m.TIPODOCUMENTO }).Distinct().ToList();

                                foreach (var item in auxNit)
                                {
                                    var dataMov2 = numCuentas.Where(x => x.Nit == item.Nit).OrderBy(x => x.Nit).ToList();
                                    debito = dataMov2.Select(x => x.DEBITO).Sum(); //acumulado por debito
                                    credito = dataMov2.Select(x => x.CREDITO).Sum();//acumulado por credito

                                    ws.Cells["A" + j].Value = codigoFormato;
                                    ws.Cells["B" + j].Value = codigoConcepto;
                                    ws.Cells["C" + j].Value = item.TIPODOCUMENTO;
                                    ws.Cells["D" + j].Value = item.Nit;
                                    ws.Cells["E" + j].Value = debito;
                                    ws.Cells["F" + j].Value = credito;

                                    j++;

                                }

                                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                                var ms = new System.IO.MemoryStream();
                                pack.SaveAs(ms);
                                ms.WriteTo(Response.OutputStream);
                            }
                            Response.End();
                            return RedirectToAction("MediosMagneticos/MMConfiguracion/Index");

                        }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["error"] = "Ha ocurrido un error";
                return RedirectToAction("/MMConfiguracion/ListaRegistros");
            }
        }
    }
}
        
