using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Nomina;

using Rotativa;
using OfficeOpenXml;
using Newtonsoft.Json;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;


namespace FNTC.Finansoft.UI.Areas.Nomina
{
    public class CorreccionNominasController : Controller
    {
        private AccountingContext db = new AccountingContext();

        public CorreccionNominasController()
        {
            ViewBag.Concepto = new List<SelectListItem>()
            {
                new SelectListItem(){Value = "CREDITOS", Text = "CREDITOS"},
                new SelectListItem(){Value = "APORTES", Text = "APORTES"},
                new SelectListItem(){Value = "AHORROS", Text = "AHORROS"},

            };



        }
        public ActionResult List()//(string nombre)
        {
            return PartialView(db.SeleccionCuenta.Include(p => p.Cuenta).OrderBy(p => p.CODIGO).ToList());
        }
        public ActionResult VistaCorreccion()
        {
            return View(db.SeleccionCuenta.Include(p => p.Cuenta).OrderBy(p => p.CODIGO).ToList());
        }

        // GET: Nomina/CorreccionNominas
        public ActionResult Index()
        {
            ViewBag.Concepto = new List<SelectListItem>()
            {
                new SelectListItem(){Value = "CREDITOS", Text = "CREDITOS"},
                new SelectListItem(){Value = "APORTES", Text = "APORTES"},
                new SelectListItem(){Value = "AHORROS", Text = "AHORROS"},

            };
            var EMPRES = new Finansoft.Accounting.BLL.PlanoEmpresas.PlanoEmpresasBLL().GetPlanoEmpresa();
            ViewBag.EMP = EMPRES;
            var tipo = new FNTC.Finansoft.Accounting.BLL.ArchivoPlanos.ArchivoPlanosBLL().GetArchivoPlanos();
            ViewBag.AP = tipo;
            var plano = new Finansoft.Accounting.BLL.ClaseDePlanos.ClaseDePlanosBLL().GetClaseDePlano();
            ViewBag.CP = plano;


            List<SelectListItem> IDASOCIADO = new List<SelectListItem>();   // Creo una lista
            IDASOCIADO.Add(new SelectListItem { Text = "Seleccione Un asociado", Value = "" });
            IList<ConsolidadoNomina> ListaDeConsolidados = db.ConsolidadoNomina.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeConsolidados)		// recorro los elementos de la db
            {
                IDASOCIADO.Add(new SelectListItem { Text = "|| PLANO: "+ item.clase_plano + " || CC."+ item.idPersona + " || NOMBRE: " + item.NOMBRE1 + " " + item.APELLIDO1, Value = item.idPersona + " " + item.EMPRESA });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.IDASOCIADO = IDASOCIADO;
            //return PartialView(db.CorreccionNomina.ToList());
            //return Create(); 
            return View("Create");
        }

        // GET: Nomina/CorreccionNominas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CorreccionNomina correccionNomina = db.CorreccionNomina.Find(id);
            if (correccionNomina == null)
            {
                return HttpNotFound();
            }
            return View(correccionNomina);
        }

        // GET: Nomina/CorreccionNominas/Create
        public ActionResult Create()
        {
            var EMPRES = new Finansoft.Accounting.BLL.PlanoEmpresas.PlanoEmpresasBLL().GetPlanoEmpresa();
            ViewBag.EMP = EMPRES;
            var tipo = new FNTC.Finansoft.Accounting.BLL.ArchivoPlanos.ArchivoPlanosBLL().GetArchivoPlanos();
            ViewBag.AP = tipo;
            var plano = new Finansoft.Accounting.BLL.ClaseDePlanos.ClaseDePlanosBLL().GetClaseDePlano();
            ViewBag.CP = plano;




            List<SelectListItem> IDASOCIADO = new List<SelectListItem>();   // Creo una lista
            IDASOCIADO.Add(new SelectListItem { Text = "Seleccione Un asociado", Value = "" });
            IList<ConsolidadoNomina> ListaDeConsolidados = db.ConsolidadoNomina.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeConsolidados)		// recorro los elementos de la db
            {
                IDASOCIADO.Add(new SelectListItem { Text = item.idPersona.ToString() });  // agrego los elementos de la db a la primera lista que cree

            }

            ViewBag.IDASOCIADO = IDASOCIADO;



            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CorreccionNomina correccionNomina)
        {

            string[] words = correccionNomina.EMPRESA.Split(' ');

            string idAsociado = words[0];
            string empresa = words[1];

            ExcelTerceros();

            return View();

        }

        public ActionResult Exportar(int ID)
        {

            var idPlanoI = db.Discriminacion.Where(p => p.ID == ID).Select(p => p.PLANO).FirstOrDefault();
            var idPlano = idPlanoI.ToString();
            var idEmpresaI = db.Discriminacion.Where(p => p.ID == ID).Select(p => p.EMPRESA).FirstOrDefault();
            var idEmpresa = idEmpresaI.ToString();

            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename=Reporte.xlsx");

            using (ExcelPackage pack = new ExcelPackage())
            {
                int IDPLANO = Int32.Parse(idPlano);
                List<string> camps = db.ArchivoPlano.Where(p => p.CLASEPLANO == IDPLANO).OrderBy(p => p.ORDEN).Select(p => p.TipoDeCampo.DESCRIPCION).ToList();
                List<string> Encabezados = db.ArchivoPlano.Where(p => p.CLASEPLANO == IDPLANO).OrderBy(p => p.ORDEN).Select(p => p.CONCEPTO).ToList();

                var consulta = (from pc in db.ConsolidadoNomina where pc.EMPRESA.ToString() == idEmpresa select pc).ToList();


                ExcelWorksheet ws = pack.Workbook.Worksheets.Add("ArchivoPlano");
                int contar = camps.Count();
                if (contar == 1)
                {
                    ws.Cells["A" + 1].Value = camps[0];
                }
                if (contar == 2)
                {
                    ws.Cells["A" + 1].Value = camps[0];
                    ws.Cells["B" + 1].Value = camps[1];
                }
                if (contar == 3)
                {
                    ws.Cells["A" + 1].Value = camps[0];
                    ws.Cells["B" + 1].Value = camps[1];
                    ws.Cells["C" + 1].Value = camps[2];

                }
                if (contar == 4)
                {
                    ws.Cells["A" + 1].Value = camps[0];
                    ws.Cells["B" + 1].Value = camps[1];
                    ws.Cells["C" + 1].Value = camps[2];
                    ws.Cells["D" + 1].Value = camps[3];
                }
                if (contar == 5)
                {
                    ws.Cells["A" + 1].Value = Encabezados[0];
                    ws.Cells["B" + 1].Value = Encabezados[1];
                    ws.Cells["C" + 1].Value = Encabezados[2];
                    ws.Cells["D" + 1].Value = Encabezados[3];
                    ws.Cells["E" + 1].Value = "CODIGO GOBERNACION";
                    ws.Cells["F" + 1].Value = "FECHA INICIAL";
                    ws.Cells["G" + 1].Value = "FECHA FINAL";
                    ws.Cells["H" + 1].Value = Encabezados[4];

                    ws.Cells["A" + 1].Style.Font.Bold = true;
                    ws.Cells["B" + 1].Style.Font.Bold = true;
                    ws.Cells["C" + 1].Style.Font.Bold = true;
                    ws.Cells["D" + 1].Style.Font.Bold = true;
                    ws.Cells["E" + 1].Style.Font.Bold = true;
                    ws.Cells["F" + 1].Style.Font.Bold = true;
                    ws.Cells["G" + 1].Style.Font.Bold = true;
                    ws.Cells["H" + 1].Style.Font.Bold = true;



                    int i = 2;

                    foreach (var item in consulta)
                    {
                        string c1 = camps[0];
                        char[] charsToTrim = { '"', ' ', '\'' };
                        string banner = c1;
                        string result = banner.Trim(charsToTrim);
                        Console.WriteLine("Trimmmed\n   {0}\nto\n   '{1}'", banner, result);


                        var campos = String.Join(camps[0], "item." + camps[0]);
                        var array = camps.ToArray();

                        String option = camps[0];
                        dynamic data0 = "";
                        switch (option)
                        {
                            case "NITEMPRESA":
                                data0 = item.NITEMPRESA;
                                break;
                            case "DigitoVerificacion":
                                data0 = item.DigitoVerificacion;
                                break;
                            case "TipoDeEstado":
                                data0 = item.TipoDeEstado;
                                break;
                            case "idPersona":
                                data0 = item.idPersona;
                                break;
                            case "NOMBRE1":
                                data0 = item.NOMBRE1;
                                break;
                            case "NOMBRE2":
                                data0 = item.NOMBRE2;
                                break;
                            case "APELLIDO1":
                                data0 = item.APELLIDO1;
                                break;
                            case "NumeroCuenta":
                                data0 = item.NumeroCuenta;
                                break;
                            case "totalAportes":
                                data0 = item.totalAportes;
                                break;
                            case "NombreCompleto":
                                data0 = item.NombreCompleto;
                                break;

                            default:
                                break;
                        }

                        String option1 = camps[1];
                        dynamic data1 = "";
                        switch (option1)
                        {
                            case "NITEMPRESA":
                                data1 = item.NITEMPRESA;
                                break;
                            case "DigitoVerificacion":
                                data1 = item.DigitoVerificacion;
                                break;
                            case "TipoDeEstado":
                                data1 = item.TipoDeEstado;
                                break;
                            case "idPersona":
                                data1 = item.idPersona;
                                break;
                            case "NOMBRE1":
                                data1 = item.NOMBRE1;
                                break;
                            case "NOMBRE2":
                                data1 = item.NOMBRE2;
                                break;
                            case "APELLIDO1":
                                data1 = item.APELLIDO1;
                                break;
                            case "NumeroCuenta":
                                data1 = item.NumeroCuenta;
                                break;
                            case "totalAportes":
                                data1 = item.totalAportes;
                                break;
                            case "NombreCompleto":
                                data1 = item.NombreCompleto;
                                break;

                            default:
                                break;
                        }

                        String option2 = camps[2];
                        dynamic data2 = "";
                        switch (option2)
                        {
                            case "NITEMPRESA":
                                data2 = item.NITEMPRESA;
                                break;
                            case "DigitoVerificacion":
                                data2 = item.DigitoVerificacion;
                                break;
                            case "TipoDeEstado":
                                data2 = item.TipoDeEstado;
                                break;
                            case "idPersona":
                                data2 = item.idPersona;
                                break;
                            case "NOMBRE1":
                                data2 = item.NOMBRE1;
                                break;
                            case "NOMBRE2":
                                data2 = item.NOMBRE2;
                                break;
                            case "APELLIDO1":
                                data2 = item.APELLIDO1;
                                break;
                            case "NumeroCuenta":
                                data2 = item.NumeroCuenta;
                                break;
                            case "totalAportes":
                                data2 = item.totalAportes;
                                break;
                            case "NombreCompleto":
                                data2 = item.NombreCompleto;
                                break;

                            default:
                                break;
                        }

                        String option3 = camps[3];
                        dynamic data3 = "";
                        switch (option3)
                        {
                            case "NITEMPRESA":
                                data3 = item.NITEMPRESA;
                                break;
                            case "DigitoVerificacion":
                                data3 = item.DigitoVerificacion;
                                break;
                            case "TipoDeEstado":
                                data3 = item.TipoDeEstado;
                                break;
                            case "idPersona":
                                data3 = item.idPersona;
                                break;
                            case "NOMBRE1":
                                data3 = item.NOMBRE1;
                                break;
                            case "NOMBRE2":
                                data3 = item.NOMBRE2;
                                break;
                            case "APELLIDO1":
                                data3 = item.APELLIDO1;
                                break;
                            case "NumeroCuenta":
                                data3 = item.NumeroCuenta;
                                break;
                            case "totalAportes":
                                data3 = item.totalAportes;
                                break;
                            case "NombreCompleto":
                                data3 = item.NombreCompleto;
                                break;

                            default:
                                break;
                        }

                        String option4 = camps[4];
                        dynamic data4 = "";
                        switch (option4)
                        {
                            case "NITEMPRESA":
                                data4 = item.NITEMPRESA;
                                break;
                            case "DigitoVerificacion":
                                data4 = item.DigitoVerificacion;
                                break;
                            case "TipoDeEstado":
                                data4 = item.TipoDeEstado;
                                break;
                            case "idPersona":
                                data4 = item.idPersona;
                                break;
                            case "NOMBRE1":
                                data4 = item.NOMBRE1;
                                break;
                            case "NOMBRE2":
                                data4 = item.NOMBRE2;
                                break;
                            case "APELLIDO1":
                                data4 = item.APELLIDO1;
                                break;
                            case "NumeroCuenta":
                                data4 = item.NumeroCuenta;
                                break;
                            case "totalAportes":
                                data4 = item.totalAportes;
                                break;
                            case "NombreCompleto":
                                data4 = item.NombreCompleto;
                                break;

                            default:
                                break;
                        }

                        

                        ws.Cells["A" + i].Value = data0;
                        ws.Cells["B" + i].Value = data1;
                        ws.Cells["C" + i].Value = data2;
                        ws.Cells["D" + i].Value = data3;
                        ws.Cells["E" + i].Value = "FONDEGO";
                        ws.Cells["F" + i].Value = "20/01/2022";
                        ws.Cells["G" + i].Value = "20/01/2028";
                        ws.Cells["H" + i].Value = data4;


                        i++;
                    }

                }
                if (contar == 6)
                {
                    ws.Cells["A" + 1].Value = Encabezados[0];
                    ws.Cells["B" + 1].Value = Encabezados[1];
                    ws.Cells["C" + 1].Value = Encabezados[2];
                    ws.Cells["D" + 1].Value = Encabezados[3];
                    ws.Cells["E" + 1].Value = Encabezados[4];
                    ws.Cells["F" + 1].Value = "Codconcepto";
                    ws.Cells["G" + 1].Value = "FECHA INICIAL";
                    ws.Cells["H" + 1].Value = "FECHA FINAL";
                    ws.Cells["I" + 1].Value = Encabezados[5];
                    ws.Cells["J" + 1].Value = "Valor Total";
                    ws.Cells["K" + 1].Value = "Valor acumulado";
                    ws.Cells["L" + 1].Value = "Tipo de acto - Libranza";
                    ws.Cells["M" + 1].Value = "Fecha de Acto";
                    ws.Cells["N" + 1].Value = "Número de Acto ú Obligacion";
                    ws.Cells["O" + 1].Value = "Observaciones";

                    ws.Cells["A" + 1].Style.Font.Bold = true;
                    ws.Cells["B" + 1].Style.Font.Bold = true;
                    ws.Cells["C" + 1].Style.Font.Bold = true;
                    ws.Cells["D" + 1].Style.Font.Bold = true;
                    ws.Cells["E" + 1].Style.Font.Bold = true;
                    ws.Cells["F" + 1].Style.Font.Bold = true;
                    ws.Cells["G" + 1].Style.Font.Bold = true;
                    ws.Cells["H" + 1].Style.Font.Bold = true;
                    ws.Cells["I" + 1].Style.Font.Bold = true;
                    ws.Cells["J" + 1].Style.Font.Bold = true;
                    ws.Cells["K" + 1].Style.Font.Bold = true;
                    ws.Cells["L" + 1].Style.Font.Bold = true;
                    ws.Cells["M" + 1].Style.Font.Bold = true;
                    ws.Cells["N" + 1].Style.Font.Bold = true;
                    ws.Cells["O" + 1].Style.Font.Bold = true;

                    int i = 2;

                    foreach (var item in consulta)
                    {
                        string c1 = camps[0];
                        char[] charsToTrim = { '"', ' ', '\'' };
                        string banner = c1;
                        string result = banner.Trim(charsToTrim);
                        Console.WriteLine("Trimmmed\n   {0}\nto\n   '{1}'", banner, result);


                        var campos = String.Join(camps[0], "item." + camps[0]);
                        var array = camps.ToArray();

                        String option = camps[0];
                        dynamic data0 = "";
                        switch (option)
                        {
                            case "NITEMPRESA":
                                data0 = item.NITEMPRESA;
                                break;
                            case "DigitoVerificacion":
                                data0 = item.DigitoVerificacion;
                                break;
                            case "TipoDeEstado":
                                data0 = item.TipoDeEstado;
                                break;
                            case "idPersona":
                                data0 = item.idPersona;
                                break;
                            case "NOMBRE1":
                                data0 = item.NOMBRE1;
                                break;
                            case "NOMBRE2":
                                data0 = item.NOMBRE2;
                                break;
                            case "APELLIDO1":
                                data0 = item.APELLIDO1;
                                break;
                            case "NumeroCuenta":
                                data0 = item.NumeroCuenta;
                                break;
                            case "totalAportes":
                                data0 = item.totalAportes;
                                break;
                            case "NombreCompleto":
                                data0 = item.NombreCompleto;
                                break;

                            default:
                                break;
                        }

                        String option1 = camps[1];
                        dynamic data1 = "";
                        switch (option1)
                        {
                            case "NITEMPRESA":
                                data1 = item.NITEMPRESA;
                                break;
                            case "DigitoVerificacion":
                                data1 = item.DigitoVerificacion;
                                break;
                            case "TipoDeEstado":
                                data1 = item.TipoDeEstado;
                                break;
                            case "idPersona":
                                data1 = item.idPersona;
                                break;
                            case "NOMBRE1":
                                data1 = item.NOMBRE1;
                                break;
                            case "NOMBRE2":
                                data1 = item.NOMBRE2;
                                break;
                            case "APELLIDO1":
                                data1 = item.APELLIDO1;
                                break;
                            case "NumeroCuenta":
                                data1 = item.NumeroCuenta;
                                break;
                            case "totalAportes":
                                data1 = item.totalAportes;
                                break;
                            case "NombreCompleto":
                                data1 = item.NombreCompleto;
                                break;

                            default:
                                break;
                        }

                        String option2 = camps[2];
                        dynamic data2 = "";
                        switch (option2)
                        {
                            case "NITEMPRESA":
                                data2 = item.NITEMPRESA;
                                break;
                            case "DigitoVerificacion":
                                data2 = item.DigitoVerificacion;
                                break;
                            case "TipoDeEstado":
                                data2 = item.TipoDeEstado;
                                break;
                            case "idPersona":
                                data2 = item.idPersona;
                                break;
                            case "NOMBRE1":
                                data2 = item.NOMBRE1;
                                break;
                            case "NOMBRE2":
                                data2 = item.NOMBRE2;
                                break;
                            case "APELLIDO1":
                                data2 = item.APELLIDO1;
                                break;
                            case "NumeroCuenta":
                                data2 = item.NumeroCuenta;
                                break;
                            case "totalAportes":
                                data2 = item.totalAportes;
                                break;
                            case "NombreCompleto":
                                data2 = item.NombreCompleto;
                                break;

                            default:
                                break;
                        }

                        String option3 = camps[3];
                        dynamic data3 = "";
                        switch (option3)
                        {
                            case "NITEMPRESA":
                                data3 = item.NITEMPRESA;
                                break;
                            case "DigitoVerificacion":
                                data3 = item.DigitoVerificacion;
                                break;
                            case "TipoDeEstado":
                                data3 = item.TipoDeEstado;
                                break;
                            case "idPersona":
                                data3 = item.idPersona;
                                break;
                            case "NOMBRE1":
                                data3 = item.NOMBRE1;
                                break;
                            case "NOMBRE2":
                                data3 = item.NOMBRE2;
                                break;
                            case "APELLIDO1":
                                data3 = item.APELLIDO1;
                                break;
                            case "NumeroCuenta":
                                data3 = item.NumeroCuenta;
                                break;
                            case "totalAportes":
                                data3 = item.totalAportes;
                                break;
                            case "NombreCompleto":
                                data3 = item.NombreCompleto;
                                break;

                            default:
                                break;
                        }

                        String option4 = camps[4];
                        dynamic data4 = "";
                        switch (option4)
                        {
                            case "NITEMPRESA":
                                data4 = item.NITEMPRESA;
                                break;
                            case "DigitoVerificacion":
                                data4 = item.DigitoVerificacion;
                                break;
                            case "TipoDeEstado":
                                data4 = item.TipoDeEstado;
                                break;
                            case "idPersona":
                                data4 = item.idPersona;
                                break;
                            case "NOMBRE1":
                                data4 = item.NOMBRE1;
                                break;
                            case "NOMBRE2":
                                data4 = item.NOMBRE2;
                                break;
                            case "APELLIDO1":
                                data4 = item.APELLIDO1;
                                break;
                            case "NumeroCuenta":
                                data4 = item.NumeroCuenta;
                                break;
                            case "totalAportes":
                                data4 = item.totalAportes;
                                break;
                            case "NombreCompleto":
                                data4 = item.NombreCompleto;
                                break;

                            default:
                                break;
                        }

                        String option5 = camps[5];
                        dynamic data5 = "";
                        switch (option5)
                        {
                            case "NITEMPRESA":
                                data5 = item.NITEMPRESA;
                                break;
                            case "DigitoVerificacion":
                                data5 = item.DigitoVerificacion;
                                break;
                            case "TipoDeEstado":
                                data5 = item.TipoDeEstado;
                                break;
                            case "idPersona":
                                data5 = item.idPersona;
                                break;
                            case "NOMBRE1":
                                data5 = item.NOMBRE1;
                                break;
                            case "NOMBRE2":
                                data5 = item.NOMBRE2;
                                break;
                            case "APELLIDO1":
                                data5 = item.APELLIDO1;
                                break;
                            case "NumeroCuenta":
                                data5 = item.NumeroCuenta;
                                break;
                            case "totalAportes":
                                data5 = item.totalAportes;
                                break;
                            case "NombreCompleto":
                                data5 = item.NombreCompleto;
                                break;

                            default:
                                break;
                        }

                        

                        ws.Cells["A" + i].Value = data0;
                        ws.Cells["B" + i].Value = data1;
                        ws.Cells["C" + i].Value = data2;
                        ws.Cells["D" + i].Value = data3;
                        ws.Cells["E" + i].Value = data4;
                        ws.Cells["F" + i].Value = "";
                        ws.Cells["G" + i].Value = "20/01/2022";
                        ws.Cells["H" + i].Value = "";
                        ws.Cells["I" + i].Value = data5;

                        i++;
                    }

                }
                if (contar == 7)
                {
                    ws.Cells["A" + 1].Value = Encabezados[0];
                    ws.Cells["B" + 1].Value = Encabezados[1];
                    ws.Cells["C" + 1].Value = Encabezados[2];
                    ws.Cells["D" + 1].Value = Encabezados[3];
                    ws.Cells["E" + 1].Value = Encabezados[4];
                    ws.Cells["F" + 1].Value = Encabezados[5];
                    ws.Cells["G" + 1].Value = Encabezados[6];


                    int i = 2;

                    foreach (var item in consulta)
                    {
                        string c1 = camps[0];
                        char[] charsToTrim = { '"', ' ', '\'' };
                        string banner = c1;
                        string result = banner.Trim(charsToTrim);
                        Console.WriteLine("Trimmmed\n   {0}\nto\n   '{1}'", banner, result);


                        var campos = String.Join(camps[0], "item." + camps[0]);
                        var array = camps.ToArray();

                        String option = camps[0];
                        dynamic data0 = "";
                        switch (option)
                        {
                            case "NITEMPRESA":
                                data0 = item.NITEMPRESA;
                                break;
                            case "DigitoVerificacion":
                                data0 = item.DigitoVerificacion;
                                break;
                            case "TipoDeEstado":
                                data0 = item.TipoDeEstado;
                                break;
                            case "idPersona":
                                data0 = item.idPersona;
                                break;
                            case "NOMBRE1":
                                data0 = item.NOMBRE1;
                                break;
                            case "NOMBRE2":
                                data0 = item.NOMBRE2;
                                break;
                            case "APELLIDO1":
                                data0 = item.APELLIDO1;
                                break;
                            case "NumeroCuenta":
                                data0 = item.NumeroCuenta;
                                break;
                            case "totalAportes":
                                data0 = item.totalAportes;
                                break;
                            case "NombreCompleto":
                                data0 = item.NombreCompleto;
                                break;

                            default:
                                break;
                        }

                        String option1 = camps[1];
                        dynamic data1 = "";
                        switch (option1)
                        {
                            case "NITEMPRESA":
                                data1 = item.NITEMPRESA;
                                break;
                            case "DigitoVerificacion":
                                data1 = item.DigitoVerificacion;
                                break;
                            case "TipoDeEstado":
                                data1 = item.TipoDeEstado;
                                break;
                            case "idPersona":
                                data1 = item.idPersona;
                                break;
                            case "NOMBRE1":
                                data1 = item.NOMBRE1;
                                break;
                            case "NOMBRE2":
                                data1 = item.NOMBRE2;
                                break;
                            case "APELLIDO1":
                                data1 = item.APELLIDO1;
                                break;
                            case "NumeroCuenta":
                                data1 = item.NumeroCuenta;
                                break;
                            case "totalAportes":
                                data1 = item.totalAportes;
                                break;
                            case "NombreCompleto":
                                data1 = item.NombreCompleto;
                                break;

                            default:
                                break;
                        }

                        String option2 = camps[2];
                        dynamic data2 = "";
                        switch (option2)
                        {
                            case "NITEMPRESA":
                                data2 = item.NITEMPRESA;
                                break;
                            case "DigitoVerificacion":
                                data2 = item.DigitoVerificacion;
                                break;
                            case "TipoDeEstado":
                                data2 = item.TipoDeEstado;
                                break;
                            case "idPersona":
                                data2 = item.idPersona;
                                break;
                            case "NOMBRE1":
                                data2 = item.NOMBRE1;
                                break;
                            case "NOMBRE2":
                                data2 = item.NOMBRE2;
                                break;
                            case "APELLIDO1":
                                data2 = item.APELLIDO1;
                                break;
                            case "NumeroCuenta":
                                data2 = item.NumeroCuenta;
                                break;
                            case "totalAportes":
                                data2 = item.totalAportes;
                                break;
                            case "NombreCompleto":
                                data2 = item.NombreCompleto;
                                break;

                            default:
                                break;
                        }

                        String option3 = camps[3];
                        dynamic data3 = "";
                        switch (option3)
                        {
                            case "NITEMPRESA":
                                data3 = item.NITEMPRESA;
                                break;
                            case "DigitoVerificacion":
                                data3 = item.DigitoVerificacion;
                                break;
                            case "TipoDeEstado":
                                data3 = item.TipoDeEstado;
                                break;
                            case "idPersona":
                                data3 = item.idPersona;
                                break;
                            case "NOMBRE1":
                                data3 = item.NOMBRE1;
                                break;
                            case "NOMBRE2":
                                data3 = item.NOMBRE2;
                                break;
                            case "APELLIDO1":
                                data3 = item.APELLIDO1;
                                break;
                            case "NumeroCuenta":
                                data3 = item.NumeroCuenta;
                                break;
                            case "totalAportes":
                                data3 = item.totalAportes;
                                break;
                            case "NombreCompleto":
                                data3 = item.NombreCompleto;
                                break;

                            default:
                                break;
                        }

                        String option4 = camps[4];
                        dynamic data4 = "";
                        switch (option4)
                        {
                            case "NITEMPRESA":
                                data4 = item.NITEMPRESA;
                                break;
                            case "DigitoVerificacion":
                                data4 = item.DigitoVerificacion;
                                break;
                            case "TipoDeEstado":
                                data4 = item.TipoDeEstado;
                                break;
                            case "idPersona":
                                data4 = item.idPersona;
                                break;
                            case "NOMBRE1":
                                data4 = item.NOMBRE1;
                                break;
                            case "NOMBRE2":
                                data4 = item.NOMBRE2;
                                break;
                            case "APELLIDO1":
                                data4 = item.APELLIDO1;
                                break;
                            case "NumeroCuenta":
                                data4 = item.NumeroCuenta;
                                break;
                            case "totalAportes":
                                data4 = item.totalAportes;
                                break;
                            case "NombreCompleto":
                                data4 = item.NombreCompleto;
                                break;

                            default:
                                break;
                        }

                        String option5 = camps[5];
                        dynamic data5 = "";
                        switch (option5)
                        {
                            case "NITEMPRESA":
                                data5 = item.NITEMPRESA;
                                break;
                            case "DigitoVerificacion":
                                data5 = item.DigitoVerificacion;
                                break;
                            case "TipoDeEstado":
                                data5 = item.TipoDeEstado;
                                break;
                            case "idPersona":
                                data5 = item.idPersona;
                                break;
                            case "NOMBRE1":
                                data5 = item.NOMBRE1;
                                break;
                            case "NOMBRE2":
                                data5 = item.NOMBRE2;
                                break;
                            case "APELLIDO1":
                                data5 = item.APELLIDO1;
                                break;
                            case "NumeroCuenta":
                                data5 = item.NumeroCuenta;
                                break;
                            case "totalAportes":
                                data5 = item.totalAportes;
                                break;
                            case "NombreCompleto":
                                data5 = item.NombreCompleto;
                                break;

                            default:
                                break;
                        }

                        String option6 = camps[6];
                        dynamic data6 = "";
                        switch (option6)
                        {
                            case "NITEMPRESA":
                                data6 = item.NITEMPRESA;
                                break;
                            case "DigitoVerificacion":
                                data6 = item.DigitoVerificacion;
                                break;
                            case "TipoDeEstado":
                                data6 = item.TipoDeEstado;
                                break;
                            case "idPersona":
                                data6 = item.idPersona;
                                break;
                            case "NOMBRE1":
                                data6 = item.NOMBRE1;
                                break;
                            case "NOMBRE2":
                                data6 = item.NOMBRE2;
                                break;
                            case "APELLIDO1":
                                data6 = item.APELLIDO1;
                                break;
                            case "NumeroCuenta":
                                data6 = item.NumeroCuenta;
                                break;
                            case "totalAportes":
                                data6 = item.totalAportes;
                                break;
                            case "NombreCompleto":
                                data6 = item.NombreCompleto;
                                break;

                            default:
                                break;
                        }

                        ws.Cells["A" + i].Value = data0;
                        ws.Cells["B" + i].Value = data1;
                        ws.Cells["C" + i].Value = data2;
                        ws.Cells["D" + i].Value = data3;
                        ws.Cells["E" + i].Value = data4;
                        ws.Cells["F" + i].Value = data5;
                        ws.Cells["G" + i].Value = data6;

                        i++;
                    }

                }

                var ms = new System.IO.MemoryStream();
                pack.SaveAs(ms);
                ms.WriteTo(Response.OutputStream);

            }
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
        public JsonResult GetDatosAsociado(string NIT)
        {
            string[] words = NIT.Split(' ');

            string id = words[0];
            string empresa = words[1];




            var Datos = (from pc in db.ConsolidadoNomina where pc.idPersona == id && pc.EMPRESA.ToString() == empresa select pc).Single();

            // List<SelectListItem> IDASOCIADO = new List<SelectListItem>();   // Creo una lista

            var consulta = (from pc in db.ConsolidadoNomina where pc.EMPRESA.ToString() == empresa select pc).ToList();

            IList<ConsolidadoNomina> ListaDeConsolidados = consulta.ToList();// extraigo los elementos desde la DB
            decimal total = 0;
            foreach (var item in ListaDeConsolidados)		// recorro los elementos de la db
            {

                total = total + Decimal.Parse(item.totalAportes);
            }
            string total1 = total.ToString();
            var FormateoNumeroTotal = total1;
            FormateoNumeroTotal = Convert.ToDecimal(FormateoNumeroTotal).ToString("#,##");

            List<string> codigos = new List<string>();

            codigos.Add(Datos.DescuentosNominaPlanoEmpresas.NOMBREMP);
            codigos.Add(Datos.agencias.nombreagencia);
            codigos.Add(Datos.PERIODO);
            codigos.Add(FormateoNumeroTotal);
            codigos.Add(Datos.clase_plano.ToString());
            codigos.Add(Datos.EMPRESA.ToString());



            return Json(codigos, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetDatosActualizar(string NIT, string TotalI)
        {

            string[] words = NIT.Split(' ');

            string id = words[0];
            string empresa = words[1];

            var Datos = (from pc in db.ConsolidadoNomina where pc.idPersona == id && pc.EMPRESA.ToString() == empresa select pc).Single();

            // List<SelectListItem> IDASOCIADO = new List<SelectListItem>();   // Creo una lista

            //int TotalAsociado = Int32.Parse(Datos.totalAportes);
            if (Datos != null)
            {
                Datos.totalAportes = TotalI;
                db.SaveChanges();
            }


            var consulta = (from pc in db.ConsolidadoNomina where pc.EMPRESA.ToString() == empresa select pc).ToList();
            IList<ConsolidadoNomina> ListaDeConsolidados = consulta.ToList();// extraigo los elementos desde la DB
            int total = 0;
            foreach (var item in ListaDeConsolidados)		// recorro los elementos de la db
            {

                total = total + Int32.Parse(item.totalAportes);
            }
            string total1 = total.ToString();
            var FormateoNumeroTotal = total1;
            FormateoNumeroTotal = Convert.ToDecimal(FormateoNumeroTotal).ToString("#,##");




            List<string> codigos = new List<string>();
            codigos.Add(FormateoNumeroTotal);

            return Json(codigos, JsonRequestBehavior.AllowGet);
        }


        // GET: Nomina/CorreccionNominas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CorreccionNomina correccionNomina = db.CorreccionNomina.Find(id);
            if (correccionNomina == null)
            {
                return HttpNotFound();
            }
            return View(correccionNomina);
        }

        // POST: Nomina/CorreccionNominas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CorreccionNomina correccionNomina)
        {
            if (ModelState.IsValid)
            {
                db.Entry(correccionNomina).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(correccionNomina);
        }

        // GET: Nomina/CorreccionNominas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CorreccionNomina correccionNomina = db.CorreccionNomina.Find(id);
            if (correccionNomina == null)
            {
                return HttpNotFound();
            }
            return View(correccionNomina);
        }

        // POST: Nomina/CorreccionNominas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CorreccionNomina correccionNomina = db.CorreccionNomina.Find(id);
            db.CorreccionNomina.Remove(correccionNomina);
            db.SaveChanges();
            return RedirectToAction("Index");
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
