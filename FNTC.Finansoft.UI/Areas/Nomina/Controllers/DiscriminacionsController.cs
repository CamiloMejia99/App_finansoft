using AutoMapper;
using ExcelLibrary.SpreadSheet;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Nomina;

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;
using System.Data.SqlClient;
using OfficeOpenXml;
using System.Web;


namespace FNTC.Finansoft.UI.Areas.Nomina.Controllers
{
    public class DiscriminacionsController : Controller
    {
        private AccountingContext db = new AccountingContext();


        public DiscriminacionsController()
        {
            ViewBag.PeriodoDed = new List<SelectListItem>()
            {

                new SelectListItem(){Value = "MENSUAL", Text = "MENSUAL"}
            };



        }
        public ActionResult List()//(string nombre)
        {
            return PartialView(db.Discriminacion.Include(p => p.ID).OrderBy(p => p.ID).ToList());
        }
        public ActionResult ListadoDisNomina()
        {
            return View(db.Discriminacion.ToList().OrderByDescending(p => p.FECHACREACIONDIS)); 
        }
        public ActionResult ListaEmpresa()
        {
            using (var db = new AccountingContext())
            {
                return PartialView(db.PlanoEmpresa.ToList().Distinct());
            }
        }

        // GET: Nomina/Discriminacions
        //public ActionResult Index()
        //{
        //    var EMPRES = new Finansoft.Accounting.BLL.PlanoEmpresas.PlanoEmpresasBLL().GetPlanoEmpresa();
        //    ViewBag.EMP = EMPRES;
        //    var tipo = new FNTC.Finansoft.Accounting.BLL.ArchivoPlanos.ArchivoPlanosBLL().GetArchivoPlanos();
        //    ViewBag.AP = tipo;
        //    var clase = new FNTC.Finansoft.Accounting.BLL.ClaseDePlanos.ClaseDePlanosBLL().GetClaseDePlano();
        //    ViewBag.CP = clase;


        //    List<SelectListItem> IDEMPRESA = new List<SelectListItem>();   // Creo una lista
        //    IDEMPRESA.Add(new SelectListItem { Text = "Seleccione Una Empresa", Value = "" });
        //    IList<PlanoEmpresa> ListaEMPRESA = db.PlanoEmpresa.ToList();// extraigo los elementos desde la DB

        //    foreach (var item in ListaEMPRESA)		// recorro los elementos de la db
        //    {
        //        string id_emp = item.id.ToString();
        //        IDEMPRESA.Add(new SelectListItem { Text = item.NOMBREMP, Value = id_emp });  // agrego los elementos de la db a la primera lista que cree
        //        //text: el texto que se muestra
        //        //value: el valor interno del dropdown
        //    }

        //    ViewBag.IDEMPRESA = IDEMPRESA;


        //    List<SelectListItem> CLASEPLANO = new List<SelectListItem>();   // Creo una lista
        //                                                                    // CLASEPLANO.Add(new SelectListItem { Text = "Seleccione Un Plano", Value = "" });
        //                                                                    //var FormatoVinculacion = (from pc in db.formatoVinculacions select pc).Single();
        //                                                                    //var Discriminacion = (from pc in db.ArchivoPlano where pc.CLASEPLANO == pc.ClaseDePlanos1.ID select pc).ToList();
        //    var PLANOSLISTA = (from pc in db.ClaseDePlano select pc).ToList();
        //    //var ClasePlanos = (from pc in db.ClaseDePlano where db.ArchivoPlano == pc.ID select pc);
        //    //int contar = (from pc in db.Discriminacion where pc.EMPRESA == NIT select pc).Count();
        //    IList<ClaseDePlano> ListaDePlanos = new List<ClaseDePlano>();
        //    foreach (var item in PLANOSLISTA)		// recorro los elementos de la db

        //    {
        //        var IDCLASEPLANO = item.ID;
        //        int ArchivoPlano = (from pc in db.ArchivoPlano where pc.CLASEPLANO == IDCLASEPLANO select pc).Count();
        //        if (ArchivoPlano > 0)
        //        {
        //            ListaDePlanos.Add(item);
        //        }
        //    }
        //    List<SelectListItem> PLANOS = new List<SelectListItem>();   // Creo una lista
        //    //IList<ClaseDePlano> PLANOS = new List<ClaseDePlano>();
        //    foreach (var item in ListaDePlanos)		// recorro los elementos de la db
        //    {

        //        string ID1 = item.ID.ToString();
        //        // int contar = (from pc in db.ArchivoPlano where pc.ID == item.ID select pc).Count();

        //        PLANOS.Add(new SelectListItem { Text = item.NOMBRE, Value = ID1 });  // agrego los elementos de la db a la primera lista que cree

        //        //text: el texto que se muestra
        //        //value: el valor interno del dropdown
        //    }

        //    ViewBag.CLASEPLANO = PLANOS;
        //    //return View(db.Discriminacion.ToList());
        //    //return Create(); 
        //    return View("Create");
        //}

        // GET: Nomina/Discriminacions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discriminacion discriminacion = db.Discriminacion.Find(id);
            if (discriminacion == null)
            {
                return HttpNotFound();
            }
            return View(discriminacion);
        }

        // GET: Nomina/Discriminacions/Create

        
        public ActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Discriminacion discriminacion)
        {

            
            ViewBag.guardado = "N";
            var PlanoNum = db.PlanoEmpresa.Where(p => p.id == discriminacion.EMPRESA).Select(p => p.NOMPLANO).FirstOrDefault();
            int id_empresa = discriminacion.EMPRESA;
            var Periodo = discriminacion.PERDEDUCC;
            int mesDis = discriminacion.PERIODO;
            string generar = "DISCRIMINADO";

            if(PlanoNum != 0)
            { 
                discriminacion.PLANO = PlanoNum;
                discriminacion.FECHACREACIONDIS = DateTime.Now;
                discriminacion.GENERAR = "DISCRIMINADO";
                db.Discriminacion.Add(discriminacion);
                db.SaveChanges();
                ViewBag.guardado = "S";
                
                return Exportar(id_empresa, mesDis, generar, PlanoNum);

            }



            var EMPRES = new Finansoft.Accounting.BLL.PlanoEmpresas.PlanoEmpresasBLL().GetPlanoEmpresa();
            ViewBag.EMP = EMPRES;
            var tipo = new FNTC.Finansoft.Accounting.BLL.ArchivoPlanos.ArchivoPlanosBLL().GetArchivoPlanos();
            ViewBag.AP = tipo;
         
            var clase = new FNTC.Finansoft.Accounting.BLL.ClaseDePlanos.ClaseDePlanosBLL().GetClaseDePlano();
            ViewBag.CP = clase;
            
            return View(discriminacion);
        }

     
        public JsonResult GetDatosEmpresa(string NIT)
        {
            int nit = Int32.Parse(NIT);
            int Periodo = (from pc in db.Discriminacion where pc.EMPRESA == nit select pc).Count();
            List<string> codigos = new List<string>();
            codigos.Add(Periodo.ToString());
            //codigos.Add(Tercero.NOMBRE.ToString());
            return Json(codigos, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discriminacion discriminacion = db.Discriminacion.Find(id);
            if (discriminacion == null)
            {
                return HttpNotFound();
            }

            return PartialView(discriminacion);
        }

        // POST: Nomina/Discriminacions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,EMPRESA,PERDEDUCC,PERIODO,GENDIS,GENTOT")] Discriminacion discriminacion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(discriminacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(discriminacion);
        }

        // GET: Nomina/Discriminacions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discriminacion discriminacion = db.Discriminacion.Find(id);
            if (discriminacion == null)
            {
                return HttpNotFound();
            }
            return PartialView(discriminacion);
        }

        // POST: Nomina/Discriminacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Discriminacion discriminacion = db.Discriminacion.Find(id);
            db.Discriminacion.Remove(discriminacion);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public FileResult Exportar(int id_empresa, int periodo, string generar, int PlanoNum)
        {

            int clase_plano = PlanoNum;

            Workbook workbook = new Workbook();
            Worksheet ws = new Worksheet("DISCRIMINADO");
            using (var ctx = new AccountingContext())
            {

                //var dddd = "DISCRIMINADONOMINA";
                var ds = new DataSet("DISCRIMINADONOMINA");


                SqlConnection objConn = new SqlConnection(ctx.Database.Connection.ConnectionString);
                objConn.Open();
                SqlDataAdapter consolidado = new SqlDataAdapter(getSqlString(clase_plano, id_empresa, periodo, generar), objConn);

                //consolidado.FillSchema(ds, SchemaType.Source, "DescuentosNominaConsolidadosNominas");
                //consolidado.Fill(ds, "DescuentosNominaConsolidadosNominas");
                var DescuentosNominaConsolidadosNominas = "DescuentosNominaConsolidadosNominas";
                consolidado.FillSchema(ds, SchemaType.Source, DescuentosNominaConsolidadosNominas);
                consolidado.Fill(ds, DescuentosNominaConsolidadosNominas);

                DataTable dt;
                //dt = ds.Tables["DescuentosNominaConsolidadosNominas"];
                dt = ds.Tables["DescuentosNominaConsolidadosNominas"];
                // var dt = ctx.TipoDeCampo.CopyToDataTable();

                //ds.Tables.Add(dt);
                HasNull(ref dt);

                string serverPath = Server.MapPath("/bin");
                serverPath += "\\Plano.xls";
                try
                {
                    ExcelLibrary.DataSetHelper.CreateWorkbook(serverPath, ds);
                }
                catch (Exception)
                {
                    Console.WriteLine("ERROR!!");
                }

                return File(serverPath, "application/vnd.ms-excel", "DISCRIMINACION.xls");
            }


        }

        private string getSqlString(int clase_plano, int EMPRESA, int periodo, string generar)
        {
            string cad = "", campos = "*";
            List<string> camps = db.ArchivoPlano.Where(p => p.CLASEPLANO == clase_plano).OrderBy(p => p.ORDEN).Select(p => p.TipoDeCampo.DESCRIPCION).ToList();
            List<string> nitEm = db.PlanoEmpresa.Where(p => p.id == EMPRESA).Select(p => p.CODIGOEMP).ToList();


            campos = String.Join(", ", camps);

            var consulta = "Select " + campos + " From apo.FichasAportes INNER JOIN ter.Terceros ON ter.Terceros.NIT = apo.FichasAportes.idPersona";


            var Datos = (from pc in db.FichasAportes.Include(Pc => Pc.Terceros) select pc).ToList();
            IList<ConsolidadoNomina> Consolidado = db.ConsolidadoNomina.ToList();
            db.ConsolidadoNomina.RemoveRange(db.ConsolidadoNomina.Where(c => c.EMPRESA == EMPRESA));


            cad = "Select " + campos + " From nom.DescuentosNominaConsolidadosNominas where EMPRESA= " + EMPRESA + "";
            foreach (var item in Datos)

            {

                ConsolidadoNomina dp = new ConsolidadoNomina();
                var Cedula = item.idPersona;
                List<string> aportes = db.FichasAportes.Where(p => p.idPersona == Cedula && p.activa == true).Select(p => p.valor).ToList();
                int[] arregloInt2 = aportes.Select(x => Convert.ToInt32(x)).ToArray();
                int TotalAporte = arregloInt2.Sum();

                List<decimal> Creditos = db.Creditos.Where(p => p.Creditos_Cedula == Cedula).Select(p => p.ValorCuotaMes).ToList();
                var MoviTipEs = db.MovimientosTipoEstado.Where(p => p.Cedula == Cedula).Select(p => p.IDMovTipEs).Max();
                List<string> TipoDeEstad = db.MovimientosTipoEstado.Where(p => p.IDMovTipEs == MoviTipEs).Select(p => p.Estado).ToList();
                decimal TotalCreditos = Creditos.Sum();

                decimal TotalDescuento = TotalAporte + TotalCreditos;

                dp.NITEMPRESA = nitEm[0];
                dp.DigitoVerificacion = "1";
                dp.TipoDeEstado = TipoDeEstad[0];
                dp.idPersona = item.idPersona;
                dp.NombreCompleto = item.Terceros.NOMBRE1 + " " + item.Terceros.NOMBRE2 + " " + item.Terceros.APELLIDO1 + " " + item.Terceros.APELLIDO2;
                dp.NOMBRE1 = item.Terceros.NOMBRE1;
                dp.NOMBRE2 = item.Terceros.NOMBRE2;
                dp.APELLIDO1 = item.Terceros.APELLIDO1;
                dp.APELLIDO2 = item.Terceros.APELLIDO2;
                dp.NumeroCuenta = item.numeroCuenta;
                dp.totalAportes = TotalDescuento.ToString();
                dp.FechaApertura = item.fechaApertura.ToString();
                dp.FECHADISCRIMINACION = DateTime.Now.ToString();
                db.ConsolidadoNomina.Add(dp);

                dp.EMPRESA = EMPRESA;
                dp.PERIODO = periodo.ToString();
                dp.GENERAR = generar;
                dp.DEPENDENCIA = item.Terceros.DEPENDENCIA.GetValueOrDefault();
                dp.clase_plano = clase_plano;

                db.ConsolidadoNomina.Add(dp);
                db.SaveChanges();
                
            }
       
            db.SaveChanges();
          
            return cad;

        }

        public static bool HasNull(ref DataTable table)
        {
            foreach (DataColumn column in table.Columns)
            {
                if (table.Rows.OfType<DataRow>().Any(r => r.IsNull(column)))
                {
                    //replace with ""
                    var columIndex = column.Ordinal;
                    //var lista = 
                    table.Rows.OfType<DataRow>().Where(R => R.IsNull(column)).ToList()
                    .ForEach(r =>
                                 r[column] = column.DataType == typeof(System.Decimal) ? 0M : 0);


                }

            }

            return false;
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
