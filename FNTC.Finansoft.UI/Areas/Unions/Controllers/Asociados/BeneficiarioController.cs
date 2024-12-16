using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using System.Dynamic;


namespace Ingenio.Controllers.Asociados
{
    public class BeneficiarioController : Controller
    {
        //public static int porcentajeben { get; set; }
        //public static int mensaje { get; set; }

        //BeneficiarioBLL BenBLL = new BeneficiarioBLL();
        //AsociadoBLL AsoBLL = new AsociadoBLL();
        //AsociadoVetadoBLL AsoVetBLL = new AsociadoVetadoBLL();

        //// GET: CEDULA
        //public JsonResult GetCedula(string term)
        //{
        //    ICollection<Personas_Fac>
        //        cad = BenBLL.GetCedula(term);
        //    List<object> cad2 = new List<object>();
        //    foreach (var item in cad)
        //    {
        //        int idp = item.Id;

        //        int idaso = BenBLL.GetIdAsociado(idp);

        //        string nom = item.Primer_Nom + ' ' + (item.Segundo_Nom ?? "") + ' ' + item.Primer_Ape + ' ' + (item.Segundo_Ape ?? "");
        //        cad2.Add(new
        //        {
        //            value = item.Nit_CC,
        //            label = nom,
        //            idaso = idaso

        //        });
        //    }
        //    return Json(cad2, JsonRequestBehavior.AllowGet);
        //}
        ////GET PERSONAS A CARGO

        //public JsonResult PersonasACargo(int id, string term)
        //{
        //    ICollection<Personas_Fac>
        //        cad = BenBLL.PersonasACargo(id, term);
        //    List<object> cad2 = new List<object>();
        //    foreach (var item in cad)
        //    {
        //        int idp = item.Id;
        //        DateTime fecha = item.PersonasaCargo_Aso.ElementAt(0).FechaNacimiento;
        //        cad2.Add(new
        //        {
        //            value = item.Nit_CC,
        //            tipoidentificacion = item.Id_TipoIdentificacion,
        //            nombre1 = item.Primer_Nom,
        //            nombre2 = (item.Segundo_Nom ?? ""),
        //            apellido1 = item.Primer_Ape,
        //            apellido2 = (item.Segundo_Ape ?? ""),
        //            genero = item.Genero ? true : false,
        //            parentesco = item.PersonasaCargo_Aso.ElementAt(0).Parentescos_Aso.Id,
        //            ciudad = item.Ciudades_Fac.Descripcion,
        //            departamento = item.Ciudades_Fac.Departamentos_Fac.Descripcion,
        //            idPais = item.Ciudades_Fac.Departamentos_Fac.Paises_Fac.Id,// estos de aqui
        //            pais = item.Ciudades_Fac.Departamentos_Fac.Paises_Fac.Descripcion,
        //            estado = item.Activo ? true : false,
        //            fechanac = fecha.ToString("yyyy-MM-dd"),
        //            label = item.Primer_Nom + ' ' + (item.Segundo_Nom ?? "") + ' ' + item.Primer_Ape + ' ' + (item.Segundo_Ape ?? ""),
        //            idpc = item.Id
        //        });
        //    }
        //    return Json(cad2, JsonRequestBehavior.AllowGet);
        //}

        ////PAIS - DEPTO
        //[HttpPost]
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
        //[HttpPost]
        //public ActionResult Ciu_Dep(int id)
        //{
        //    List<Ciudades_Fac> ciud = new List<Ciudades_Fac>();

        //    if (id >= 1)
        //    {
        //        ciud = AsoBLL.getListaCiudades(id);
        //    }

        //    List<object> cad2 = new List<object>();

        //    foreach (var item in ciud)
        //    {
        //        cad2.Add(new
        //        {
        //            value = item.Id,
        //            label = item.Descripcion
        //        });
        //    }
        //    return Json(cad2, JsonRequestBehavior.AllowGet);
        //}
        //// GET: Beneficiario
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: Beneficiario/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Beneficiario/Create
        //[HttpGet]
        //public ActionResult Create(int id = 0)
        //{

        //    if (id > 0)
        //    {
        //        ICollection<Personas_Fac> ben = BenBLL.GetBeneficiario(id);
        //        int total = 0;
        //        ICollection<dynamic> res = new List<dynamic>();
        //        foreach (var item in ben)
        //        {
        //            dynamic d = new ExpandoObject();
        //            d.tipoId = item.Tipos_Identificaciones_Aso.Descripcion;
        //            d.identificacion = item.Nit_CC;
        //            d.nombre = item.Primer_Nom + " " + (item.Segundo_Nom ?? "") + " " + item.Primer_Ape + " " + (item.Segundo_Ape ?? "");
        //            d.genero = item.Genero ? "M" : "F";
        //            d.parentesco = item.AsociadosBeneficiarios_Aso.ElementAt(0).Parentescos_Aso.Descripcion;
        //            d.porcentaje = item.AsociadosBeneficiarios_Aso.ElementAt(0).Porcentaje + "%";
        //            d.fechaN = item.AsociadosBeneficiarios_Aso.ElementAt(0).Fecha_Nacimiento.ToString("dd/MM/yyyy");
        //            d.fechaI = item.Fecha_Ingreso + "";
        //            d.activo = item.AsociadosBeneficiarios_Aso.ElementAt(0).Activo == true;
        //            if (item.AsociadosBeneficiarios_Aso.ElementAt(0).Activo == true)
        //            {
        //                total = item.AsociadosBeneficiarios_Aso.ElementAt(0).Porcentaje + total;
        //            }

        //            d.idbeneficiario = item.AsociadosBeneficiarios_Aso.ElementAt(0).Id;
        //            res.Add(d);
        //        }

        //        Personas_Fac a = BenBLL.GetPerson(id);
                    
                    
        //        ViewBag.nombre = a.Primer_Nom + " " + (a.Segundo_Nom ?? "") + " " + a.Primer_Ape + " " + (a.Segundo_Ape ?? "");
        //        ViewBag.cedula = a.Nit_CC;
        //        ViewBag.id = id;
        //        ViewBag.res = res;
        //        ViewBag.total = total;
        //        porcentajeben = total;
        //    }

        //    ViewBag.pais = AsoBLL.getListaPaises();
        //    ViewBag.Tipoi = AsoBLL.getListaIdentificaciones();
        //    return View();
        //}

        //// POST: Beneficiario/Create
        //[HttpPost]
        //public JsonResult Create(FormCollection collection, Personas_Fac per, AsociadosBeneficiarios_Aso ben, PersonasEstados perest)
        //{
        //    try
        //    {
        //        string cedula2 = (collection["_Nit_CC2"]);

        //        int n = BenBLL.ValidarCedulaBeneficiario(cedula2, per.Id_TipoIdentificacion);


        //        if (n > 0)
        //        {
        //            dynamic response = new
        //            {
        //                estado = false,
        //                mensaje = "El numero de identificación ya existe"
        //            };
        //            return Json(response, JsonRequestBehavior.AllowGet);
        //        }

       
        //        int porcentajetotal = porcentajeben + ben.Porcentaje;
        //        int idaso = ben.Id_Asociado;
        //        if (porcentajetotal > 100)
        //        {

        //            dynamic response = new
        //            {
        //                estado = false,
        //                mensaje = "El numero de identificación ya existe"
        //            };
        //            return Json(response, JsonRequestBehavior.AllowGet);

        //        }

        //        per.Activo = true;
        //        ben.Activo = true;

        //        string cedula = collection["_Nit_CC2"];
        //        bool res = BenBLL.CrearPersona(cedula, per, ben, perest);



        //        return Json(new
        //        {
        //            estado = res
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch 
        //    {
        //        dynamic response = new
        //        {
        //            estado = false,
        //            mensaje = "No se pudo guardar"
        //        };

        //        return Json(response, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //// GET: Beneficiario/Edit/5
        //[HttpGet]
        //public ActionResult Edit(int id)
        //{
        //    Personas_Fac per = AsoVetBLL.GetPersona(id);

        //    /*******lLISTA BENEFICIARIOS*********/

        //    id = per.AsociadosBeneficiarios_Aso.FirstOrDefault().Id_Asociado;

        //    ICollection<Personas_Fac> ben = BenBLL.GetBeneficiario(id);
        //    int total = 0;
        //    ICollection<dynamic> res = new List<dynamic>();
        //    foreach (var item in ben)
        //    {
        //        dynamic d = new ExpandoObject();

        //        if (item.AsociadosBeneficiarios_Aso.ElementAt(0).Activo == true)
        //        {
        //            total = item.AsociadosBeneficiarios_Aso.ElementAt(0).Porcentaje + total;
        //        }

        //        res.Add(d);
        //    }

        //    Personas_Fac a = BenBLL.GetPerson(id);
        //    ViewBag.res = res;
        //    ViewBag.total = total;

        //    /*/////////////////////////////////******/




        //    return View("", per);
        //}

        //// POST: Beneficiario/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection, Personas_Fac per, AsociadosBeneficiarios_Aso ben)
        //{
        //    try
        //    {
        //        int idaso = Convert.ToInt32(collection["idasociado"]);

        //        ICollection<AsociadosBeneficiarios_Aso> ben2 = BenBLL.GetListaAsociadosBeneficiarios(idaso); 
                    
        //        int porcentajeTotal = 0;
        //        foreach (var item in ben2)
        //        {
        //            if (item.Activo == true)
        //            {
        //                porcentajeTotal = porcentajeTotal + item.Porcentaje;
        //            }

        //        }

        //        int porcentajeOld = Convert.ToInt32(collection["PorcentajeOld"]);
        //        porcentajeTotal = porcentajeTotal - porcentajeOld + ben.Porcentaje;


        //        if (porcentajeTotal > 100)
        //        {
        //            ViewBag.pais = AsoBLL.getListaPaises();
        //            ViewBag.Tipoi = AsoBLL.getListaIdentificaciones(); 
        //            TempData["mensaje"] = "Los porcentajes son mayores que 100%";
        //            return RedirectToAction("Edit", new { id = id });
        //        }

        //        bool res = BenBLL.EditarBeneficiario(id, per, ben);
        //        return RedirectToAction("Details", "Asociado", new { id = idaso });
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}


        //// POST: Beneficiario/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, PersonasEstados perest)
        //{
        //    try
        //    {
        //        AsociadosBeneficiarios_Aso b = BenBLL.GetAsociadoBeneficiario(id);   
                
        //        b.Activo = false;

        //        //FECHA ACTUAL - FECHA DE MUERTE
        //        DateTime fecha = DateTime.UtcNow;
        //        DateTime fechaactual = fecha.ToLocalTime();

        //        perest.Id_Persona = b.Id_Beneficiario;
        //        perest.Id_Estado = 6;
        //        perest.Nota = "BENEFICIARIO INACTIVO";
        //        perest.Fecha = fechaactual;

        //        bool res = BenBLL.Delete(b, perest);

        //        return Json(res, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}
        ////OBTENER BENEFICIARIO
        //public JsonResult GetBeneficiario(int id)
        //{
        //    ICollection<Personas_Fac> per = BenBLL.GetBeneficiario(id);

        //    ICollection<dynamic> res = new List<dynamic>();
        //    porcentajeben = 0;
        //    foreach (var item in per)
        //    {
        //        porcentajeben = porcentajeben + item.AsociadosBeneficiarios_Aso.ElementAt(0).Porcentaje;
        //        res.Add(
        //            new
        //            {
        //                tipoId = item.Tipos_Identificaciones_Aso.Descripcion,
        //                identificacion = item.Nit_CC,
        //                nombre = item.Primer_Nom + " " + (item.Segundo_Nom ?? "") + " " + item.Primer_Ape + " " + (item.Segundo_Ape ?? ""),
        //                genero = item.Genero ? "M" : "F",
        //                fechaN = item.AsociadosBeneficiarios_Aso.ElementAt(0).Fecha_Nacimiento,
        //                fechaI = item.Fecha_Ingreso + "",
        //                parentesco = item.AsociadosBeneficiarios_Aso.ElementAt(0).Parentescos_Aso.Descripcion,
        //                porcentaje = item.AsociadosBeneficiarios_Aso.ElementAt(0).Porcentaje + "%",
        //                idbeneficiario = item.AsociadosBeneficiarios_Aso.ElementAt(0).Id
        //            });
        //    }
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}

        ////ACTUALIZAR PORCENTAJE
        //[HttpPost]
        //public ActionResult Porcentaje(int id, short porcentaje)
        //{
        //    try
        //    {
        //        bool res = BenBLL.Porcentaje(id, porcentaje);

        //        return null;
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}

        //// POST: Beneficiario/Delete/5
        //[HttpPost]
        //public ActionResult Activar(int id, PersonasEstados perest)
        //{
        //    try
        //    {
        //        //int res = 0;

        //        bool res = BenBLL.Activar(id, perest); 


        //        return Json(res, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}


        ///////////////////////////************************
        //public JsonResult CedulaValida(string id, int asociado)
        //{
        //    bool res = BenBLL.CedulaValida(id, asociado);
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}


    }
}
