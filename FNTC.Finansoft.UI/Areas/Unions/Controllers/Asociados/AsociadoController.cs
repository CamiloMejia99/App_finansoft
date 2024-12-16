using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;






using System.Dynamic;

namespace Ingenio.Controllers.Asociados
{
    //[Allow(action = "ASOCIADOS")]
    public class AsociadoController : Controller
    {
        //ModeloContainer db = new ModeloContainer();
        //AsociadoBLL AsoBLL = new AsociadoBLL();

        //public ActionResult Index()
        //{
        //    ICollection<AsociadosViewModel> asociado = AsoBLL.GetListaAsociados();
        //    return View("", asociado);
        //}

        //public ActionResult GetAsociado(string term )
        //{
        //    ICollection<Asociados_Aso> res = AsoBLL.GetSimilarPorCedula(term);

        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}       


        //public ActionResult Edit(int id)
        //{
        //    AsociadosViewModel asociado = AsoBLL.GetAsociado(id);

        //    return View("",asociado);
        //}

        //// GET: PROFESION
        //public JsonResult GetProfesion(string term)
        //{
        //    ICollection<Profesiones_Aso> cad =
        //        cad = AsoBLL.GetProfesion(term);           
        //    List<object> cad2 = new List<object>();
        //    foreach (var item in cad)
        //    {                
        //        cad2.Add(new
        //        {
        //            value = item.Id,
        //            label = item.Descripcion                    
        //        });
        //    }
        //    return Json(cad2, JsonRequestBehavior.AllowGet);
        //}

        ////GET: EMPRESA

        //public JsonResult GetEmpresa(string term)
        //{
        //    ICollection<Empresas_Aso> cad =
        //        cad = AsoBLL.GetEmpresa(term);
        //    List<object> cad2 = new List<object>();
        //    foreach (var item in cad)
        //    {
        //        cad2.Add(new
        //        {
        //            value = item.Id,
        //            label = item.Nombre
        //        });
        //    }
        //    return Json(cad2, JsonRequestBehavior.AllowGet);
        //}
        ////GET: TRABAJA EN

        //public JsonResult GetTrabajaEn(string term)
        //{
        //    ICollection<Oficinas_Con> cad =
        //        cad = AsoBLL.GetTrabajaEn(term);

        //    List<object> cad2 = new List<object>();
        //    foreach (var item in cad)
        //    {
        //        cad2.Add(new
        //        {
        //            value = item.Id,
        //            label = item.Nombre
        //        });
        //    }

            

        //    return Json(cad2, JsonRequestBehavior.AllowGet);
        //}
        ////GET: CARGO

        //public JsonResult GetCargo(string term)
        //{
        //    ICollection<Cargos_Aso> cad =
        //        cad = AsoBLL.GetCargo(term);
        //    List<object> cad2 = new List<object>();
        //    foreach (var item in cad)
        //    {                
        //        cad2.Add(new
        //        {
        //            value = item.Id,
        //            label = item.Descripcion
        //        });
        //    }
        //    return Json(cad2, JsonRequestBehavior.AllowGet);
        //}

        ////GET: BANCO

        //public JsonResult GetBanco(string term)
        //{
        //    ICollection<Bancos_Aso> cad =
        //        cad = AsoBLL.GetBanco(term);
        //    List<object> cad2 = new List<object>();
        //    foreach (var item in cad)
        //    {
        //        cad2.Add(new
        //        {
        //            value = item.Id,
        //            label = item.Descripcion
        //        });
        //    }
        //    return Json(cad2, JsonRequestBehavior.AllowGet);
        //}
        ////GET: CIIU

        //public JsonResult GetCiiu(string term)
        //{
        //    ICollection<CIIU_Aso> cad =
        //        cad = AsoBLL.GetCiiu(term);
        //    List<object> cad2 = new List<object>();
        //    foreach (var item in cad)
        //    {
        //        cad2.Add(new
        //        {
        //            value = item.Id,
        //            label = item.Descripcion
        //        });
        //    }
        //    return Json(cad2, JsonRequestBehavior.AllowGet);
        //}
        ////GET: DEPENDENCIA

        //public JsonResult GetDependencia(int id, string term)
        //{
        //    ICollection<Dependencias_Con> cad =
        //        cad = AsoBLL.GetDependencia(id,term);
        //    List<object> cad2 = new List<object>();
        //    foreach (var item in cad)
        //    {
        //        cad2.Add(new
        //        {
        //            value = item.Id,
        //            label = item.Descripcion
        //        });
        //    }
        //    return Json(cad2, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult GetPais()
        //{
        //    List<Paises_Fac> dep = AsoBLL.getListaPaises();

            
        //    List<object> cad2 = new List<object>();

        //    foreach (var item in dep)
        //    {
        //        cad2.Add(new
        //        {
        //            value = item.Id,
        //            label = item.Descripcion
        //        });
        //    }
        //    return Json(cad2, JsonRequestBehavior.AllowGet);
        //}
        ////PAIS - DEPTO
        // [HttpPost]
        //public ActionResult Dep_Pais(int id)
        //{
        //    List<Departamentos_Fac> dep = new List<Departamentos_Fac>();

        //    if (id >= 1)
        //    {
        //        dep = AsoBLL.getListaDepartamentos(id);
        //    }

        //    List<object> cad2 = new List<object>();

        //    foreach (var item in dep)
        //    {
        //        cad2.Add(new
        //        {
        //            value = item.Id,
        //            label = item.Descripcion
        //        });
        //    }
        //    return Json(cad2, JsonRequestBehavior.AllowGet);
        //}
        ////DEPTO - CIUDAD
        // [HttpPost]
        // public ActionResult Ciu_Dep(int id)
        // {
        //     List<Ciudades_Fac> ciud = new List<Ciudades_Fac>();

        //     if (id >= 1)
        //     {
        //         ciud = AsoBLL.getListaCiudades(id);
                 
        //     }

        //     List<object> cad2 = new List<object>();

        //     foreach (var item in ciud)
        //     {
        //         cad2.Add(new
        //         {
        //             value = item.Id,
        //             label = item.Descripcion
        //         });
        //     }
        //     return Json(cad2, JsonRequestBehavior.AllowGet);
        // }

       
        //// GET: Asociado/Details/5
        //public ActionResult Details(int id)
        //{
        //    ViewBag.idAsociado = id;
        //    PersCargoBLL persCargoBll = new PersCargoBLL();
        //    AsociadosViewModel aso = AsoBLL.GetAsociado(id);
        //    //******************PERSONAS A CARGO
        //    ICollection<Personas_Fac> per = persCargoBll.PersonasACargo(id);

        //    ICollection<dynamic> res = new List<dynamic>();
        //    foreach (var item in per)
        //    {
        //        dynamic d = new ExpandoObject();
        //        d.id = item.Id;
        //        d.tipoId = item.Tipos_Identificaciones_Aso.Descripcion;
        //        d.identificacion = item.Nit_CC;
        //        d.nombre = item.Primer_Nom + " " + (item.Segundo_Nom ?? "") + " " + item.Primer_Ape + " " + (item.Segundo_Ape?? "");
        //        d.genero = item.Genero ? "M" : "F";
        //        d.parentesco = item.PersonasaCargo_Aso.FirstOrDefault().Parentescos_Aso.Descripcion;
        //        d.estudiante = item.PersonasaCargo_Aso.FirstOrDefault().Estudiante ? "Si" : "No";
        //        d.activo = item.PersonasaCargo_Aso.FirstOrDefault().Activo;
        //        res.Add(d);
        //    }
        //    //*********************BENEFICIARIO
        //    BeneficiarioBLL BeneficiarioBLL = new BeneficiarioBLL();
        //    ICollection<Personas_Fac> ben = BeneficiarioBLL.GetBeneficiario(id);

        //    int pt = 0;

        //    ICollection<dynamic> res2 = new List<dynamic>();
        //    foreach (var item2 in ben)
        //    {
        //        dynamic b = new ExpandoObject();
        //        b.id = item2.Id;
        //        b.tipoId = item2.Tipos_Identificaciones_Aso.Descripcion;
        //        b.identificacion = item2.Nit_CC;
        //        b.nombre = item2.Primer_Nom + " " + (item2.Segundo_Nom?? "") + " " + item2.Primer_Ape + " " + (item2.Segundo_Ape?? "");
        //        b.genero = item2.Genero ? "M" : "F";
        //        b.parentesco = item2.AsociadosBeneficiarios_Aso.FirstOrDefault().Parentescos_Aso.Descripcion;
        //        b.porcentaje = item2.AsociadosBeneficiarios_Aso.FirstOrDefault().Porcentaje+"%";
        //        b.porcentaje2 = item2.AsociadosBeneficiarios_Aso.FirstOrDefault().Porcentaje;
        //        b.idbeneficiario = item2.AsociadosBeneficiarios_Aso.FirstOrDefault().Id;
        //        b.activo = item2.AsociadosBeneficiarios_Aso.FirstOrDefault().Activo;

        //        if (item2.AsociadosBeneficiarios_Aso.FirstOrDefault().Activo == true)
        //        {
        //            pt = item2.AsociadosBeneficiarios_Aso.FirstOrDefault().Porcentaje + pt;
        //        }
                
        //        res2.Add(b);
        //    }

        //    ////*********************EGRESOS
        //    EgresosAsoBLL EgresosAsoBLL = new EgresosAsoBLL();
        //    dynamic egre = EgresosAsoBLL.GetEgresos(id);

        //    ////*********************INGRESOS
        //    IngresosAsoBLL IngresosAsoBLL = new IngresosAsoBLL();
        //    dynamic ingre = IngresosAsoBLL.GetIngresos(id);

        //    ////***********************ESTUDIOS
        //    EstudioBLL EstudiosBLL = new EstudioBLL();
        //    dynamic estu = EstudiosBLL.GetEstudios(id);

        //    ////***********************REFERENCIAS
        //    ReferenciasAsoBLL ReferenciasBLL = new ReferenciasAsoBLL();
        //    ICollection<Personas_Fac> refe = ReferenciasBLL.GetReferencias(id);

        //    ICollection<dynamic> res3 = new List<dynamic>();
        //    foreach (var item2 in refe)
        //    {
        //        dynamic b = new ExpandoObject();
        //        b.id = item2.Id;
        //        b.tipoId = item2.Tipos_Identificaciones_Aso.Descripcion;
        //        b.identificacion = item2.Nit_CC;
        //        b.nombre = item2.Primer_Nom + " " + (item2.Segundo_Nom?? "") + " " + item2.Primer_Ape + " " + (item2.Segundo_Ape?? "");
        //        b.genero = item2.Genero ? "M" : "F";
        //        b.trabaja = item2.ReferenciasAsociados_Aso.FirstOrDefault().Trabaja;
        //        b.nota = item2.ReferenciasAsociados_Aso.FirstOrDefault().Nota;
        //        b.cuenta = item2.ReferenciasAsociados_Aso.FirstOrDefault().Cuenta;
        //        b.verificacion_referencia = item2.ReferenciasAsociados_Aso.FirstOrDefault().VerificacionReferencia ? "Si":"No";
        //        b.activo = item2.ReferenciasAsociados_Aso.FirstOrDefault().Activo;
        //        res3.Add(b);
        //    }

        //    ////***********************codeudores
        //    CodeudorAsoBLL CodeudorAsoBLL = new CodeudorAsoBLL();
        //    ICollection<Personas_Fac> code = CodeudorAsoBLL.GetCodeudor(id);

        //    ICollection<dynamic> res4 = new List<dynamic>();
        //    foreach (var item3 in code)
        //    {
        //        dynamic b = new ExpandoObject();
               
        //        b.id = item3.Id;
        //        b.id2 = item3.Codeudores_Aso.FirstOrDefault().AsociadosCodeudores_Aso.Where(x => (x.Id_Asociado == id)).Select(x => x).FirstOrDefault().Id;
        //        b.tipoId = item3.Tipos_Identificaciones_Aso.Descripcion;
        //        b.identificacion = item3.Nit_CC;
        //        b.nombre = item3.Primer_Nom + " " + (item3.Segundo_Nom ?? "") + " " + item3.Primer_Ape + " " + (item3.Segundo_Ape ?? "");
        //        b.genero = item3.Genero ? "M" : "F";
        //        b.estrato = item3.Codeudores_Aso.FirstOrDefault().Id_Estrato;
        //        b.ciiu = item3.Codeudores_Aso.FirstOrDefault().CIIU_Aso.Descripcion;
        //        b.activo = item3.Codeudores_Aso.FirstOrDefault().AsociadosCodeudores_Aso.Where(x => (x.Id_Asociado == id)).Select(x => x).FirstOrDefault().Activo;
        //        res4.Add(b);
        //    }

        //    ViewBag.codeudores = res4;
        //    ViewBag.referencias = res3;
        //    ViewBag.estudios = estu;
        //    ViewBag.ingresos = ingre;
        //    ViewBag.egresos = egre;
        //    ViewBag.asociado = aso;
        //    ViewBag.personas = res;
        //    ViewBag.beneficiario = res2;
        //    ViewBag.idaso = id;
        //    ViewBag.porcentajetotal = pt;
        //    return View();

        //}

        //[HttpGet]
        //public ActionResult Create()
        //{
        //    List<Paises_Fac> dep = AsoBLL.getListaPaises();
        //    ViewBag.pais = dep;

        //    List<Tipos_Identificaciones_Aso> tipident = AsoBLL.getListaIdentificaciones();
        //    ViewBag.Tipoi = tipident;

        //    List<Tipo_Contribuyente_Fac> tipcontr = AsoBLL.getListaTipoContribuyente();
        //    ViewBag.Tipoc = tipcontr;

        //    List<Estudios_Aso> tipoest = AsoBLL.getListaTipoEstudios();
        //    ViewBag.Tipoest = tipoest;

        //    List<Tipo_EstadoCivil_Aso> tipoestciv = AsoBLL.getListaEstadoCiviles();
        //    ViewBag.Tipoestciv = tipoestciv;
        //    return View();
        //}

        //// POST: Asociado/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection, Personas_Fac per, Asociados_Aso aso, PersonasEmpresas peremp, PersonasBancos perbanc, PersonasEstados perest)
        //{
        //    try
        //    {
             
        //        string direccion = collection["Direccion"].ToUpper().Trim();
        //        string correo = collection["Correo"].ToUpper().Trim();
        //        string telefono = collection["Telefono"];
        //        string extencion = collection["ext"];
        //        string celular = collection["Celular"];                
        //        string fechaInicioAso = collection["Fecha_Inicio_Aso"];
        //        string fechaInicioEmp = collection["Fecha_Inicio_Emp"];
        //        int depen = Convert.ToInt32( collection["Id_Dependencia"]);
                
                              
        //        string[] ubicaciones = new string[5];
        //        ubicaciones[0] = direccion;
        //        ubicaciones[1] = correo;
        //        ubicaciones[2] = telefono;
        //        ubicaciones[3] = extencion;
        //        ubicaciones[4] = celular;

        //        per.Activo = true;
                

        //        aso.Fecha_Inicio = Convert.ToDateTime(fechaInicioAso);
        //        peremp.Fecha_Inicio = Convert.ToDateTime(fechaInicioEmp);
        //        bool res = AsoBLL.CrearPersona(ubicaciones, per, aso, peremp, perbanc, perest, depen);
                
        //        return RedirectToAction("Index");
        //    }
        //    catch (Excepciones e)
        //    {
        //        ViewBag.mensaje = e.Descripcion;
        //        return RedirectToAction("Create");
        //    }
        //}


        //// POST: Asociado/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection, Personas_Fac per, Asociados_Aso aso, PersonasEmpresas peremp, PersonasBancos perbanc)
        //{
        //    try
        //    {
        //        string direccion = collection["Direccion"].ToUpper().Trim();
        //        string correo = collection["Correo"].ToUpper().Trim();
        //        string telefono = collection["Telefono"];
        //        string extencion = collection["ext"];
        //        string celular = collection["Celular"];
        //        string fechaInicioAso = collection["Fecha_Inicio_Aso"];
        //        string fechaInicioEmp = collection["Fecha_Inicio_Emp"];
        //        int depen = Convert.ToInt32(collection["Id_Dependencia"]);
        //        int trabajaEn = peremp.Id_Agencia;



        //        string[] ubicaciones = new string[5];
        //        ubicaciones[0] = direccion;
        //        ubicaciones[1] = correo;
        //        ubicaciones[2] = telefono;
        //        ubicaciones[3] = extencion;
        //        ubicaciones[4] = celular;


        //        aso.Fecha_Inicio = Convert.ToDateTime(fechaInicioAso);
        //        peremp.Fecha_Inicio = Convert.ToDateTime(fechaInicioEmp);

        //        bool res = AsoBLL.EditarPersona(id, ubicaciones, per, aso, peremp, perbanc, depen, trabajaEn);
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}

        ////// GET: Asociado/Delete/5
        ////public ActionResult Delete(int id)
        ////{
        ////    return View();
        ////}

        //// POST: Asociado/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
        
        //public ExcelResult AsociadosExcel()
        //{
        //    var workbook = new XLWorkbook();
        //    var worksheet = workbook.Worksheets.Add("Sheet 1");

        //    worksheet.Cell("A1").Value = "Cooperativa";

        //    //ICollection<dynamic> asociado = AsoBLL.GetListaAsociadosExcel();

        //    var asociado = (from p in db.Personas_Fac
        //                        join a in db.Asociados_Aso on p.Id equals a.Id_Persona
        //                        select new { a, p });

        //    worksheet.Cell(4, 1).Value = "Tipo de identificación";
        //    worksheet.Cell(4, 2).Value = "Número de identificación";
        //    worksheet.Cell(4, 3).Value = "Primer apellido";
        //    worksheet.Cell(4, 4).Value = "Segundo apellido";
        //    worksheet.Cell(4, 5).Value = "Nombres"; 
        //    worksheet.Cell(4, 6).Value = "Fecha de ingreso";
        //    worksheet.Cell(4, 7).Value = "Teléfono";
        //    worksheet.Cell(4, 8).Value = "Dirección";
        //    worksheet.Cell(4, 9).Value = "Asociado";
        //    worksheet.Cell(4, 10).Value = "Activo";
        //    worksheet.Cell(4, 11).Value = "Actividad económica";
        //    worksheet.Cell(4, 12).Value = "Código Municipio";
        //    worksheet.Cell(4, 13).Value = "EMail";
        //    worksheet.Cell(4, 14).Value = "Genero";
        //    worksheet.Cell(4, 15).Value = "Empleado";
        //    worksheet.Cell(4, 16).Value = "TipoContrato";
        //    worksheet.Cell(4, 17).Value = "NivelEscolaridad";
        //    worksheet.Cell(4, 18).Value = "Estrato";
        //    worksheet.Cell(4, 19).Value = "NivelIngresos";
        //    worksheet.Cell(4, 20).Value = "FechaNacimiento";
        //    worksheet.Cell(4, 21).Value = "EstadoCivil";
        //    worksheet.Cell(4, 22).Value = "MujerCabezaFamilia";
        //    worksheet.Cell(4, 23).Value = "Ocupacion";
        //    worksheet.Cell(4, 24).Value = "Sector Economico";
        //    worksheet.Cell(4, 25).Value = "Jornada Laboral";
        //    worksheet.Cell(4, 26).Value = "Fecha de Retiro (ExAsociado)";
        //    worksheet.Cell(4, 27).Value = "AsistioUltAsamblea";

        //    int smlv = AsoBLL.GetSalarioMinimo();
                
                

        //    int i = 5;
        //    foreach (var item in asociado)
        //    {
        //        var p = item.p;
        //        var a = item.a;
        //        string identifi = "";
                              
        //        if (p.Id_TipoIdentificacion == 1)
        //        {
        //            identifi = "C";
        //        }
        //        if ( p.Id_TipoIdentificacion == 2)
        //        {
        //            identifi = "P";
        //        }
        //        if (p.Id_TipoIdentificacion == 3)
        //        {
        //            identifi = "E";
        //        }
        //        if (p.Id_TipoIdentificacion == 4)
        //        {
        //            identifi = "N";
        //        }

        //        worksheet.Cell(i, 1).Value = p.Id_TipoIdentificacion;// falta
        //        worksheet.Cell(i, 2).Value = p.Nit_CC;
        //        worksheet.Cell(i, 3).Value = p.Primer_Ape;
        //        worksheet.Cell(i, 4).Value = p.Segundo_Ape;
        //        worksheet.Cell(i, 5).Value = p.Primer_Nom + " " + (p.Segundo_Nom ?? "");
        //        worksheet.Cell(i, 6).Value = p.Fecha_Ingreso;

        //        Personas_Fac p2 = p;

        //        worksheet.Cell(i, 7).Value = p2.PersonasUbicaciones.Where (m => (m.Id_TUbicacion == 3)).Select (m => m.Descripcion);
        //        worksheet.Cell(i, 8).Value = p2.PersonasUbicaciones.Where(m => (m.Id_TUbicacion == 1)).Select(m => m.Descripcion);

        //        int aso;
        //        if(a.Fecha_Termina!=null)
        //        {
        //            aso = 0;
        //        }
        //        else
        //        {
        //            aso = 1;
        //        }
        //        worksheet.Cell(i, 9).Value = aso; 

        //        worksheet.Cell(i, 10).Value = p.Activo?1:0;
        //        worksheet.Cell(i, 11).Value = a.Id_CIIU;
        //        worksheet.Cell(i, 12).Value = p.Id_Residencia;
        //        worksheet.Cell(i, 13).Value = p2.PersonasUbicaciones.Where(m => (m.Id_TUbicacion == 2)).Select(m => m.Descripcion);
        //        worksheet.Cell(i, 14).Value = p.Genero?1:0;

        //        int empleado;
        //        if(a.Id_CIIU == 1542){
        //            empleado = 1;
        //        }
        //        else
        //        {
        //            empleado = 0;
        //        }

        //        worksheet.Cell(i, 15).Value = empleado;



        //        worksheet.Cell(i, 16).Value = p.PersonasEmpresas.FirstOrDefault().Id_Contrato;
        //        worksheet.Cell(i, 17).Value = a.Id_TipoEstudio;
        //        worksheet.Cell(i, 18).Value = a.Id_Estrato;

        //        int nivelingre;

        //        Asociados_Aso a2 = a;

        //        if (a2.AsociadosIngresos_Aso.Sum(x => x.Valor) == 0)
        //        {
        //            nivelingre = 1;
        //        }
        //        else
        //        {
        //            nivelingre = 1 + a2.AsociadosIngresos_Aso.Sum(x => x.Valor) / smlv;
        //        }

        //        worksheet.Cell(i, 19).Value = nivelingre;
        //        worksheet.Cell(i, 20).Value = a.Fecha_Nacimiento;
        //        worksheet.Cell(i, 21).Value = a.Id_EstadoCivil;
        //        worksheet.Cell(i, 22).Value = a.Id_EstadoCivil.Equals(4)?1:0;
        //        worksheet.Cell(i, 23).Value = 0;//ocupacion
        //        worksheet.Cell(i, 24).Value = 1;//          siempre 1: intervension economica
        //        worksheet.Cell(i, 25).Value = 0;//          jornada laboral no aplica
        //        worksheet.Cell(i, 26).Value = a.Fecha_Termina;
        //        worksheet.Cell(i, 27).Value = 0;//asistio asamblea

        //        i++;
        //    }
        //    worksheet.Columns().AdjustToContents();
        //    return new ExcelResult(workbook, "ASOCIADOS-EXCEL");
        //}


        //public ExcelResult DetallesExcel(int id)
        //{
        //    var workbook = new XLWorkbook();

        //    var worksheet = workbook.Worksheets.Add("DATOS ASOCIADO");

        //    worksheet.Range("A1:F2").Merge().SetValue("DATOS DEL ASOCIADO").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center); 
        //    AsociadosViewModel aso = AsoBLL.GetAsociado(id);

        //    worksheet.Cell(3, 1).SetValue("Nombre:").Style.Font.SetBold(true);
        //    worksheet.Range("B3:E3").Merge().SetValue(aso.Primer_Ape + " " + (aso.Segundo_Ape ?? "") + " " + aso.Primer_Nom + " " + (aso.Segundo_Nom ?? "")); worksheet.Cell(4, 1).SetValue(aso.TipoIdentificacion + ":").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 2).Value = aso.Nit_CC;
        //    worksheet.Cell(4, 3).SetValue("Expedida en:").Style.Font.SetBold(true);
        //    worksheet.Range("D4:F4").Merge().SetValue(aso.LExpedicion + ", " + aso.DeptoExpedicion + ", " + aso.PaisExpedicion);
        //    worksheet.Cell(5, 1).SetValue("Genero:").Style.Font.SetBold(true);
        //    worksheet.Cell(5, 2).Value = aso.GeneroDes;
        //    worksheet.Cell(6, 1).SetValue("Fecha de Nacimiento:").Style.Font.SetBold(true);
        //    worksheet.Cell(6, 2).Value = aso.Fecha_Nacimiento;
        //    worksheet.Cell(7, 1).SetValue("Fecha de Afiliación:").Style.Font.SetBold(true);
        //    worksheet.Cell(7, 2).Value = aso.Fecha_InicioAso;
        //    worksheet.Cell(7, 3).SetValue("Acta de Ingreso").Style.Font.SetBold(true);
        //    worksheet.Cell(7, 4).Value = aso.Acta;
        //    worksheet.Cell(8, 1).SetValue("Tipo Vivienda:").Style.Font.SetBold(true);
        //    worksheet.Cell(8, 2).Value = aso.TipoVivienda;

        //    worksheet.Range("A10:F10").Merge().SetValue("INFORMACIÓN DEL CONTACTO").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center); 
        //    worksheet.Cell(11, 1).SetValue("Lugar de Residencia:").Style.Font.SetBold(true);
        //    worksheet.Range("B11:D11").Merge().SetValue(aso.Residencia + ", " + aso.DeptoResidencia + ", " + aso.PaisResidencia);
        //    worksheet.Cell(12, 1).SetValue("Dirección:").Style.Font.SetBold(true);
        //    worksheet.Cell(12, 2).Value = aso.Direccion;
        //    worksheet.Cell(13, 1).SetValue("Telefono").Style.Font.SetBold(true);
        //    worksheet.Cell(13, 2).Value = aso.Telefono;
        //    worksheet.Cell(13, 3).SetValue("Extension").Style.Font.SetBold(true);
        //    worksheet.Cell(14, 4).Value = (aso.Extension ?? "");
        //    worksheet.Cell(14, 1).SetValue("Celular").Style.Font.SetBold(true);
        //    worksheet.Cell(14, 2).Value = aso.Celular;
        //    worksheet.Cell(15, 1).SetValue("Email").Style.Font.SetBold(true);
        //    worksheet.Range("B15:D15").Merge().Value = aso.Correo;

        //    worksheet.Range("A17:F17").Merge().SetValue("ESTUDIOS E INFORMACIÓN LABORAL").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet.Cell(18, 1).SetValue("Tipo Estudios_Aso").Style.Font.SetBold(true);
        //    worksheet.Cell(18, 2).Value = aso.TipoEstudio;
        //    worksheet.Cell(18, 4).SetValue("Profesion").Style.Font.SetBold(true);
        //    worksheet.Cell(18, 5).Value = aso.Profesion;
        //    worksheet.Cell(19, 1).SetValue("Empresa").Style.Font.SetBold(true);
        //    worksheet.Range("B19:F19").Merge().Value = aso.EmpresaPaga;
        //    worksheet.Cell(20, 1).SetValue("Trabaja en").Style.Font.SetBold(true);
        //    worksheet.Range("B20:F20").Merge().Value = aso.TrabajaEn;
        //    worksheet.Cell(21, 1).SetValue("Dependencia").Style.Font.SetBold(true);
        //    worksheet.Range("B21:D21").Value = aso.Dependencia;
        //    worksheet.Cell(21, 4).SetValue("Tipo de Contrato").Style.Font.SetBold(true);
        //    worksheet.Cell(21, 5).Value = aso.Contrato;
        //    worksheet.Cell(22, 1).SetValue("Fecha Inicia Empresa").Style.Font.SetBold(true);
        //    worksheet.Cell(22, 2).Value = aso.FechaIniEmp;
        //    worksheet.Cell(22, 4).SetValue("Fecha Fin Empresa").Style.Font.SetBold(true);

        //    string fechafinemp = aso.FechaFinEmp.ToString("yyyy-MM-dd");

        //    if (fechafinemp.Equals(01 / 01 / 1800))
        //    {
        //        fechafinemp = "";
        //    }


        //    worksheet.Cell(22, 5).Value = fechafinemp;
        //    worksheet.Cell(23, 1).SetValue("Cargo").Style.Font.SetBold(true);
        //    worksheet.Cell(23, 2).Value = aso.Cargo;
        //    worksheet.Cell(24, 1).SetValue("Salario").Style.Font.SetBold(true);
        //    worksheet.Cell(24, 2).Value = aso.Salario;
        //    worksheet.Cell(25, 1).SetValue("Nº Cuentas_Con").Style.Font.SetBold(true);
        //    worksheet.Cell(25, 2).Value = aso.Cuentas_Con;
        //    worksheet.Cell(25, 3).SetValue("Tipo Cuentas_Con").Style.Font.SetBold(true);
        //    worksheet.Cell(25, 4).Value = aso.Tipo;
        //    worksheet.Cell(25, 5).SetValue("Banco").Style.Font.SetBold(true);
        //    worksheet.Cell(25, 6).Value = aso.Banco;
        //    worksheet.Cell(26, 1).SetValue("CIIU").Style.Font.SetBold(true);
        //    worksheet.Cell(26, 2).Value = aso.CIIU;

        //    //*****************PERSONAS A CARGO EXCEL

        //    var worksheet2 = workbook.Worksheets.Add("PERSONAS A CARGO");

        //    PersCargoBLL persCargoBll = new PersCargoBLL();
        //    ICollection<Personas_Fac> per = persCargoBll.PersonasACargo(id);
        //    worksheet2.Range("A1:F3").Merge().SetValue("PERSONAS A CARGO").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet2.Cell(4, 1).SetValue("Tipo de identificación").Style.Font.SetBold(true);
        //    worksheet2.Cell(4, 2).SetValue("Número de identificación").Style.Font.SetBold(true);
        //    worksheet2.Cell(4, 3).SetValue("Nombre").Style.Font.SetBold(true);
        //    worksheet2.Cell(4, 4).SetValue("Genero").Style.Font.SetBold(true);
        //    worksheet2.Cell(4, 5).SetValue("Parentesco").Style.Font.SetBold(true);
        //    worksheet2.Cell(4, 6).SetValue("Estudiante").Style.Font.SetBold(true);           
             

        //     ICollection<dynamic> res = new List<dynamic>();
        //     foreach (var item in per)
        //     {
        //         dynamic d = new ExpandoObject();
        //         d.id = item.Id;
        //         d.tipoId = item.Tipos_Identificaciones_Aso.Descripcion;
        //         d.identificacion = item.Nit_CC;
        //         d.nombre = item.Primer_Nom + " " + (item.Segundo_Nom ?? "") + " " + item.Primer_Ape + " " + (item.Segundo_Ape ?? "");
        //         d.genero = item.Genero ? "M" : "F";
        //         d.parentesco = item.PersonasaCargo_Aso.FirstOrDefault().Parentescos_Aso.Descripcion;
        //         d.estudiante = item.PersonasaCargo_Aso.FirstOrDefault().Estudiante ? "Si" : "No";
        //         res.Add(d);
        //     }

        //     int i = 5;
        //    foreach (var item in res)
        //                {
        //                    worksheet2.Cell(i, 1).Value = item.tipoId;
        //                    worksheet2.Cell(i, 2).Value = item.identificacion;
        //                    worksheet2.Cell(i, 3).Value = item.nombre;
        //                    worksheet2.Cell(i, 4).Value = item.genero;
        //                    worksheet2.Cell(i, 5).Value = item.parentesco;
        //                    worksheet2.Cell(i, 6).Value = item.estudiante;
        //                    i++;
        //                }


        //    //*********************BENEFICIARIO
        //    BeneficiarioBLL BeneficiarioBLL = new BeneficiarioBLL();
        //    ICollection<Personas_Fac> ben = BeneficiarioBLL.GetBeneficiario(id);

        //    var worksheet3 = workbook.Worksheets.Add("BENEFICIARIOS");
                       
        //    worksheet3.Range("A1:F3").Merge().SetValue("BENEFICIARIOS").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet3.Cell(4, 1).SetValue("Tipo de identificación").Style.Font.SetBold(true);
        //    worksheet3.Cell(4, 2).SetValue("Número de identificación").Style.Font.SetBold(true);
        //    worksheet3.Cell(4, 3).SetValue("Nombre").Style.Font.SetBold(true);
        //    worksheet3.Cell(4, 4).SetValue("Genero").Style.Font.SetBold(true);
        //    worksheet3.Cell(4, 5).SetValue("Parentesco").Style.Font.SetBold(true);
        //    worksheet3.Cell(4, 6).SetValue("Porcentaje").Style.Font.SetBold(true);           

        //    ICollection<dynamic> res2 = new List<dynamic>();
        //    foreach (var item2 in ben)
        //    {
        //        dynamic b = new ExpandoObject();
        //        b.id = item2.Id;
        //        b.tipoId = item2.Tipos_Identificaciones_Aso.Descripcion;
        //        b.identificacion = item2.Nit_CC;
        //        b.nombre = item2.Primer_Nom + " " + (item2.Segundo_Nom ?? "") + " " + item2.Primer_Ape + " " + (item2.Segundo_Ape ?? "");
        //        b.genero = item2.Genero ? "M" : "F";
        //        b.parentesco = item2.AsociadosBeneficiarios_Aso.FirstOrDefault().Parentescos_Aso.Descripcion;
        //        b.porcentaje = item2.AsociadosBeneficiarios_Aso.FirstOrDefault().Porcentaje + "%";
        //        b.idbeneficiario = item2.AsociadosBeneficiarios_Aso.FirstOrDefault().Id;
        //        res2.Add(b);
        //    }

        //    i = 5;
        //    foreach (var item in res2)
        //    {
        //        worksheet3.Cell(i, 1).Value = item.tipoId;
        //        worksheet3.Cell(i, 2).Value = item.identificacion;
        //        worksheet3.Cell(i, 3).Value = item.nombre;
        //        worksheet3.Cell(i, 4).Value = item.genero;
        //        worksheet3.Cell(i, 5).Value = item.parentesco;
        //        worksheet3.Cell(i, 6).Value = item.porcentaje;
        //        i++;
        //    }

        //    ////*********************EGRESOS
        //    EgresosAsoBLL EgresosAsoBLL = new EgresosAsoBLL();
        //    dynamic egre = EgresosAsoBLL.GetEgresos(id);

        //    var worksheet4 = workbook.Worksheets.Add("EGRESOS E INGRESOS");
        //    worksheet4.Range("A1:B3").Merge().SetValue("EGRESOS:").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        //    worksheet4.Cell(4, 1).SetValue("Descripción").Style.Font.SetBold(true);
        //    worksheet4.Cell(4, 2).SetValue("Valor").Style.Font.SetBold(true);

        //    int totalegre = 0;
        //    i = 5;
        //    foreach (var item in egre)
        //            {
        //               worksheet4.Cell(i, 1).Value = item.Descripcion;
        //               worksheet4.Cell(i, 2).Value = item.Valor;
        //               totalegre = totalegre + item.Valor;
        //               i++; 
        //            }

        //    worksheet4.Cell(i + 3, 1).SetValue("Total Ingresos :").Style.Font.SetBold(true);
        //    worksheet4.Cell(i + 3, 2).Value = totalegre;
        //    ////*********************INGRESOS
        //    IngresosAsoBLL IngresosAsoBLL = new IngresosAsoBLL();
        //    dynamic ingre = IngresosAsoBLL.GetIngresos(id);
        //    worksheet4.Range("D1:E3").Merge().SetValue("INGRESOS:").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet4.Cell(4, 4).SetValue("Descripción").Style.Font.SetBold(true);
        //    worksheet4.Cell(4, 5).SetValue("Valor").Style.Font.SetBold(true);
        //    int totalingre = 0;
        //    i = 5;
        //    foreach (var item in ingre)
        //    {
        //        worksheet4.Cell(i, 4).Value = item.Descripcion;
        //        worksheet4.Cell(i, 5).Value = item.Valor;
        //        totalingre = totalingre + item.Valor;
        //        i++;
        //    }
        //    worksheet4.Cell(i + 3, 4).SetValue("Total Egresos :").Style.Font.SetBold(true);
        //    worksheet4.Cell(i + 3, 5).Value = totalingre;
        //    ///***********************ESTUDIOS
        //    EstudioBLL EstudiosBLL = new EstudioBLL();
        //    dynamic estu = EstudiosBLL.GetEstudios(id);

        //    var worksheet5 = workbook.Worksheets.Add("ESTUDIOS");
                       
        //    worksheet5.Range("A1:C3").Merge().SetValue("ESTUDIOS").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet5.Cell(4, 1).SetValue("Tipo de Estudio").Style.Font.SetBold(true);
        //    worksheet5.Cell(4, 2).SetValue("Institución").Style.Font.SetBold(true);
        //    worksheet5.Cell(4, 3).SetValue("Observaciones").Style.Font.SetBold(true);

        //    i = 5;
        //    foreach (var item in estu)
        //    {
        //        worksheet5.Cell(i, 1).Value = item.Estudio;
        //        worksheet5.Cell(i, 2).Value = item.Institucion;
        //        worksheet5.Cell(i, 3).Value = item.Observaciones;
        //    }



        //    ////***********************REFERENCIAS
        //    ReferenciasAsoBLL ReferenciasBLL = new ReferenciasAsoBLL();
        //    var worksheet6 = workbook.Worksheets.Add("REFERENCIAS");

        //    worksheet6.Range("A1:H3").Merge().SetValue("REFERENCIAS").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        //    worksheet6.Cell(4, 1).SetValue("Tipo de identificación").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet6.Cell(4, 2).SetValue("Número de identificación").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet6.Cell(4, 3).SetValue("Nombre").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet6.Cell(4, 4).SetValue("Genero").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet6.Cell(4, 5).SetValue("Trabaja").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet6.Cell(4, 6).SetValue("Nota").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet6.Cell(4, 7).SetValue("Cuentas_Con").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet6.Cell(4, 8).SetValue("Verificacion de Referencia").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        //    ICollection<Personas_Fac> refe = ReferenciasBLL.GetReferencias(id);

        //    ICollection<dynamic> res3 = new List<dynamic>();
        //    foreach (var item2 in refe)
        //    {
        //        dynamic b = new ExpandoObject();
        //        b.id = item2.Id;
        //        b.tipoId = item2.Tipos_Identificaciones_Aso.Descripcion;
        //        b.identificacion = item2.Nit_CC;
        //        b.nombre = item2.Primer_Nom + " " + (item2.Segundo_Nom ?? "") + " " + item2.Primer_Ape + " " + (item2.Segundo_Ape ?? "");
        //        b.genero = item2.Genero ? "M" : "F";
        //        b.trabaja = item2.ReferenciasAsociados_Aso.FirstOrDefault().Trabaja;
        //        b.nota = item2.ReferenciasAsociados_Aso.FirstOrDefault().Nota;
        //        b.cuenta = item2.ReferenciasAsociados_Aso.FirstOrDefault().Cuenta;
        //        b.verificacion_referencia = item2.ReferenciasAsociados_Aso.FirstOrDefault().VerificacionReferencia ? "Si" : "No";
        //        res3.Add(b);
        //    }
            
        //    i = 5;
        //    foreach (var item in res3)
        //    {
        //        worksheet6.Cell(i, 1).Value = item.tipoId;
        //        worksheet6.Cell(i, 2).Value = item.identificacion;
        //        worksheet6.Cell(i, 3).Value = item.nombre;
        //        worksheet6.Cell(i, 4).Value = item.genero;
        //        worksheet6.Cell(i, 5).Value = item.trabaja;
        //        worksheet6.Cell(i, 6).Value = item.nota;
        //        worksheet6.Cell(i, 7).Value = item.cuenta;
        //        worksheet6.Cell(i, 8).Value = item.verificacion_referencia;
        //        i++;
                                
        //     }
        //    //////////////////////CODEUDORES

        //    CodeudorAsoBLL CodeudorAsoBLL = new CodeudorAsoBLL();
        //    var worksheet7 = workbook.Worksheets.Add("CODEUDORES");

        //    worksheet7.Range("A1:H3").Merge().SetValue("CODEUDORES").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);


        //    worksheet7.Cell(4, 1).SetValue("Tipo de identificación").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet7.Cell(4, 2).SetValue("Número de identificación").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet7.Cell(4, 3).SetValue("Nombre").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet7.Cell(4, 4).SetValue("Genero").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet7.Cell(4, 5).SetValue("Estrato").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet7.Cell(4, 6).SetValue("CIIU").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

        //    worksheet7.Cell(4, 7).SetValue("Lugar Residencia").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet7.Cell(4, 8).SetValue("Dirección").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet7.Cell(4, 9).SetValue("Email").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet7.Cell(4, 10).SetValue("Telefono").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet7.Cell(4, 11).SetValue("extension").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    worksheet7.Cell(4, 12).SetValue("celular").Style.Font.SetBold(true).Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
           
           
        //    ICollection<Personas_Fac> code = CodeudorAsoBLL.GetCodeudor(id);

        //    ICollection<dynamic> res4 = new List<dynamic>();
        //    foreach (var item3 in code)
        //    {
        //        dynamic b = new ExpandoObject();                
        //        b.tipoId = item3.Tipos_Identificaciones_Aso.Descripcion;
        //        b.identificacion = item3.Nit_CC;
        //        b.nombre = item3.Primer_Nom + " " + (item3.Segundo_Nom ?? "") + " " + item3.Primer_Ape + " " + (item3.Segundo_Ape ?? "");
        //        b.genero = item3.Genero ? "M" : "F";
        //        b.estrato = item3.Codeudores_Aso.FirstOrDefault().Id_Estrato;                
        //        b.ciiu = item3.Codeudores_Aso.FirstOrDefault().CIIU_Aso.Descripcion;

        //        b.residencia = item3.Ciudades_Fac.Descripcion + ", " + item3.Ciudades_Fac.Departamentos_Fac.Descripcion + ", " + item3.Ciudades_Fac.Departamentos_Fac.Paises_Fac.Descripcion;
        //        b.direccion = item3.PersonasUbicaciones.Where(m => (m.Id_TUbicacion == 1)).Select(m => m.Descripcion);
        //        b.correo = item3.PersonasUbicaciones.Where(m => (m.Id_TUbicacion == 3)).Select(m => m.Descripcion);
        //        b.telefono = item3.PersonasUbicaciones.Where(m => (m.Id_TUbicacion == 4)).Select(m => m.Descripcion);
        //        b.extension = item3.PersonasUbicaciones.Where(m => (m.Id_TUbicacion == 5)).Select(m => m.Descripcion);
        //        b.celular = item3.PersonasUbicaciones.Where(m => (m.Id_TUbicacion == 2)).Select(m => m.Descripcion);


        //        res4.Add(b);
        //    }

        //    i = 5;
        //    foreach (var item in res4)
        //    {
        //        worksheet7.Cell(i, 1).Value = item.tipoId;
        //        worksheet7.Cell(i, 2).Value = item.identificacion;
        //        worksheet7.Cell(i, 3).Value = item.nombre;
        //        worksheet7.Cell(i, 4).Value = item.genero;
        //        worksheet7.Cell(i, 5).Value = item.estrato;
        //        worksheet7.Cell(i, 6).Value = item.ciiu;

        //        worksheet7.Cell(i, 7).Value = item.residencia;
        //        worksheet7.Cell(i, 8).Value = item.direccion;
        //        worksheet7.Cell(i, 9).Value = item.correo;
        //        worksheet7.Cell(i, 10).Value = item.telefono;
        //        worksheet7.Cell(i, 11).Value = item.extension;
        //        worksheet7.Cell(i, 12).Value = item.celular;
        //        i++;

        //    }

        //    /////////////////////
        //    worksheet.Columns().AdjustToContents();
        //    worksheet2.Columns().AdjustToContents();
        //    worksheet3.Columns().AdjustToContents();
        //    worksheet4.Columns().AdjustToContents();
        //    worksheet5.Columns().AdjustToContents();
        //    worksheet6.Columns().AdjustToContents();
        //    worksheet7.Columns().AdjustToContents();
        //    return new ExcelResult(workbook, "DETALLES ASOCIADO");
        //}



        //public ExcelResult AsociadosExcel2()
        //{
        //    var workbook = new XLWorkbook();
        //    var worksheet = workbook.Worksheets.Add("ASOCIADOS");

        //    worksheet.Cell("A1").Value = "Cooperativa";



        //    //ICollection<dynamic> asociado = AsoBLL.GetListaAsociadosExcel();

        //    var asociado = (from p in db.Personas_Fac
        //                        join a in db.Asociados_Aso on p.Id equals a.Id_Persona
        //                        select new { a, p });
            
        //    worksheet.Cell(4, 1).SetValue("Tipo de identificación:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 2).SetValue("Número de identificación:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 3).SetValue("Primer apellido:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 4).SetValue("Segundo apellido:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 5).SetValue("Nombres:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 6).SetValue("Activo:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 7).SetValue("Genero:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 8).SetValue("EstadoCivil:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 9).SetValue("Fecha de Nacimiento:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 10).SetValue("Fecha de ingreso:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 11).SetValue("Acta:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 12).SetValue("Tipo Vivienda:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 13).SetValue("Lugar de Residencia:").Style.Font.SetBold(true);            
        //    worksheet.Cell(4, 14).SetValue("Dirección:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 15).SetValue("Teléfono:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 16).SetValue("Ext:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 17).SetValue("Celular:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 18).SetValue("Email:").Style.Font.SetBold(true);            
        //    worksheet.Cell(4, 19).SetValue("Tipo de Estudios_Aso:").Style.Font.SetBold(true);            
        //    worksheet.Cell(4, 20).SetValue("Profesion:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 21).SetValue("Empresa:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 22).SetValue("Trabaja en:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 23).SetValue("Dependencia:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 24).SetValue("Tipo de Contrato:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 25).SetValue("Fecha Inicia Empresa:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 26).SetValue("Fecha Termina Empresa:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 27).SetValue("Cargo:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 28).SetValue("Salario:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 29).SetValue("Nº Cuentas_Con:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 30).SetValue("Tipo Cuentas_Con Bancaria:").Style.Font.SetBold(true);
        //    worksheet.Cell(4, 31).SetValue("Banco:").Style.Font.SetBold(true);           

        //    int i = 5;
        //    foreach (var item in asociado)
        //    {
        //        var p = item.p;
        //        var a = item.a;
        //        worksheet.Cell(i, 1).Value = p.Tipos_Identificaciones_Aso.Descripcion;
        //        worksheet.Cell(i, 2).Value = p.Nit_CC;
        //        worksheet.Cell(i, 3).Value = p.Primer_Ape;
        //        worksheet.Cell(i, 4).Value = p.Segundo_Ape;
        //        worksheet.Cell(i, 5).Value = p.Primer_Nom + " " + (p.Segundo_Nom ?? "");                
        //        worksheet.Cell(i, 6).Value = p.Activo ? "SI" : "NO";
        //        worksheet.Cell(i, 7).Value = p.Genero ? "Masculino" : "Femenino";
        //        worksheet.Cell(i, 8).Value = a.Tipo_EstadoCivil_Aso.Descripcion;
        //        worksheet.Cell(i, 9).Value = a.Fecha_Nacimiento.ToString("yyyy-MM-dd");
        //        worksheet.Cell(i, 10).Value = a.Fecha_Inicio.ToString("yyyy-MM-dd");
        //        worksheet.Cell(i, 11).Value = a.Acta;
        //        worksheet.Cell(i, 12).Value = a.Tipos_Vidiendas_Aso.Descripcion;
        //        worksheet.Cell(i, 13).Value = p.Ciudades_Fac.Descripcion + ", " + p.Ciudades_Fac.Departamentos_Fac.Descripcion + ", " + p.Ciudades_Fac.Departamentos_Fac.Paises_Fac.Descripcion;

        //        Personas_Fac p2 = p;

        //        worksheet.Cell(i, 14).Value = p2.PersonasUbicaciones.Where(m => (m.Id_TUbicacion == 1)).Select(m => m.Descripcion);
        //        worksheet.Cell(i, 15).Value = p2.PersonasUbicaciones.Where(m => (m.Id_TUbicacion == 3)).Select(m => m.Descripcion);
        //        worksheet.Cell(i, 16).Value = p2.PersonasUbicaciones.Where(m => (m.Id_TUbicacion == 4)).Select(m => m.Descripcion);
        //        worksheet.Cell(i, 17).Value = p2.PersonasUbicaciones.Where(m => (m.Id_TUbicacion == 5)).Select(m => m.Descripcion);
        //        worksheet.Cell(i, 18).Value = p2.PersonasUbicaciones.Where(m => (m.Id_TUbicacion == 2)).Select(m => m.Descripcion);
        //        worksheet.Cell(i, 19).Value = a.Estudios_Aso.Descripcion;
        //        worksheet.Cell(i, 20).Value = a.Profesiones_Aso.Descripcion;
        //        worksheet.Cell(i, 21).Value = a.Empresas_Afiliadas.Empresas_Aso.Nombre;
        //        worksheet.Cell(i, 22).Value = "";//////////////////////////////PREGUNTAR AL CHEPE
        //        worksheet.Cell(i, 23).Value = p.PersonasEmpresas.FirstOrDefault().OficinasDependencias_Con.Dependencias_Con.Descripcion;//??????????????
        //        worksheet.Cell(i, 24).Value = p.PersonasEmpresas.FirstOrDefault().Contratos_Aso.Descripcion;
        //        worksheet.Cell(i, 25).Value = p.PersonasEmpresas.FirstOrDefault().Fecha_Inicio.ToString("yyyy-MM-dd");
                
        //        DateTime fechafin = p.PersonasEmpresas.FirstOrDefault().Fecha_Fin == null ? new DateTime(1800, 01, 01) : (DateTime)p.PersonasEmpresas.FirstOrDefault().Fecha_Fin;
        //        string fechafinal = fechafin.ToString("yyyy-MM-dd");
        //        if (fechafinal == "1800-01-01")
        //        {
        //            fechafinal = "";
        //        }

        //        worksheet.Cell(i, 26).Value = fechafinal;
        //        worksheet.Cell(i, 27).Value = p.PersonasEmpresas.FirstOrDefault().Cargos_Aso.Descripcion;
        //        worksheet.Cell(i, 28).Value = a.Salario;
        //        var banc = p.PersonasBancos.FirstOrDefault();
        //        worksheet.Cell(i, 29).Value = banc == null ? "" : banc.Cuenta;
        //        worksheet.Cell(i, 30).Value = banc == null ? "" : banc.Tipos_CuentasBancarias.Descripcion;
        //        worksheet.Cell(i, 31).Value = banc == null ? "" : banc.Bancos_Aso.Descripcion;
                
        //        i++;
        //    }

        //    /*******************************///////////

            
        //    var worksheet2 = workbook.Worksheets.Add("VETADOS");

        //    worksheet2.Cell("A1").Value = "Cooperativa";

        //    //ICollection<dynamic> vetados = AsoBLL.GetListaVetados();

        //    var vetados = ((from p in db.Personas_Fac
        //                    join e in db.PersonasEstados on p.Id equals e.Id_Persona
        //                    select new { p, e }
        //                            )
        //                            .ToList())
        //                            .OrderBy(test => test.e.Id)
        //                            .GroupBy(test => test.e.Id_Persona)
        //                            .Select(test => test.Last())
        //                            .Where(test => (test.e.Id_Estado == 2));

            
        //    worksheet2.Cell(4, 1).SetValue("Tipo de identificación:").Style.Font.SetBold(true);
        //    worksheet2.Cell(4, 2).SetValue("Número de identificación:").Style.Font.SetBold(true);
        //    worksheet2.Cell(4, 3).SetValue("Primer apellido:").Style.Font.SetBold(true);
        //    worksheet2.Cell(4, 4).SetValue("Segundo apellido:").Style.Font.SetBold(true);
        //    worksheet2.Cell(4, 5).SetValue("Nombres:").Style.Font.SetBold(true);
        //    worksheet2.Cell(4, 6).SetValue("Asociado Vetado: RAZON : NOTA ").Style.Font.SetBold(true);
        //    worksheet2.Cell(4, 7).SetValue("Fecha:").Style.Font.SetBold(true);
            
        //    i = 5;
        //    foreach (var item in vetados)
        //    {
        //        var p = item.p;
        //        var e = item.e;

        //        worksheet2.Cell(i, 1).Value = p.Tipos_Identificaciones_Aso.Descripcion;
        //        worksheet2.Cell(i, 2).Value = p.Nit_CC;
        //        worksheet2.Cell(i, 3).Value = p.Primer_Ape;
        //        worksheet2.Cell(i, 4).Value = p.Segundo_Ape;
        //        worksheet2.Cell(i, 5).Value = p.Primer_Nom + " " + (p.Segundo_Nom ?? "");

        //        worksheet2.Cell(i, 6).Value = e.Nota;
        //        worksheet2.Cell(i, 7).Value = e.Fecha;
        //        i++;
        //    }

        //    /*******************************/
        //    //////////


        //    var worksheet3 = workbook.Worksheets.Add("MUERTOS");

        //    worksheet3.Cell("A1").Value = "Cooperativa";
        //    ICollection<dynamic> muertos = AsoBLL.GetAsociadosMuertos();
                
                


        //    worksheet3.Cell(4, 1).SetValue("Tipo de identificación:").Style.Font.SetBold(true);
        //    worksheet3.Cell(4, 2).SetValue("Número de identificación:").Style.Font.SetBold(true);
        //    worksheet3.Cell(4, 3).SetValue("Primer apellido:").Style.Font.SetBold(true);
        //    worksheet3.Cell(4, 4).SetValue("Segundo apellido:").Style.Font.SetBold(true);
        //    worksheet3.Cell(4, 5).SetValue("Nombres:").Style.Font.SetBold(true);
        //    worksheet3.Cell(4, 6).SetValue("Causa:").Style.Font.SetBold(true);
        //    worksheet3.Cell(4, 7).SetValue("Fecha Fallecimiento:").Style.Font.SetBold(true);
        //    worksheet3.Cell(4, 8).SetValue("Fecha Informe:").Style.Font.SetBold(true);
        //    worksheet3.Cell(4, 9).SetValue("Enviado a Aseguradora:").Style.Font.SetBold(true);

        //    i = 5;
        //    foreach (var item in muertos)
        //    {
        //        var p = item.p;
        //        var a = item.a;
        //        var m = item.m;

        //        worksheet3.Cell(i, 1).Value = p.Tipos_Identificaciones_Aso.Descripcion;
        //        worksheet3.Cell(i, 2).Value = p.Nit_CC;
        //        worksheet3.Cell(i, 3).Value = p.Primer_Ape;
        //        worksheet3.Cell(i, 4).Value = p.Segundo_Ape;
        //        worksheet3.Cell(i, 5).Value = p.Primer_Nom + " " + (p.Segundo_Nom ?? "");

        //        worksheet3.Cell(i, 6).Value = m.Causas_Muertes_Aso.Descripcion;
        //        worksheet3.Cell(i, 7).Value = m.FechaFallesimiento.ToString("yyyy-MM-dd");
        //        worksheet3.Cell(i, 8).Value = m.FechaFallesimiento.ToString("yyyy-MM-dd");
        //        worksheet3.Cell(i, 9).Value = (m.EnviadoAseguradora ? "si":"no");
        //        i++;
        //    }
        //    worksheet.Columns().AdjustToContents();
        //    worksheet2.Columns().AdjustToContents();
        //    worksheet3.Columns().AdjustToContents();
        //    return new ExcelResult(workbook, "TOTAL ASOCIADOS-EXCEL");
        //}

        //public JsonResult CedulaValida(string id)
        //{
        //    bool res = AsoBLL.CedulaValida(id);
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}
    }
}

