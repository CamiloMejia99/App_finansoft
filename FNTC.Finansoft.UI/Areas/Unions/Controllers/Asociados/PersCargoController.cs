using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using System.Dynamic;

namespace Ingenio.Controllers.Asociados
{
    public class PersCargoController : Controller
    {
        
        //PersCargoBLL PersonaCargoBLL = new PersCargoBLL();
        //AsociadoBLL AsoBLL = new AsociadoBLL();
        //BeneficiarioBLL BenBLL = new BeneficiarioBLL();
        //AsociadoVetadoBLL AsoVetBLL = new AsociadoVetadoBLL();


        //// GET: CEDULA
        //public JsonResult GetCedula(string term)
        //{
        //    ICollection<Personas_Fac> cad =
        //        cad = PersonaCargoBLL.GetCedula(term);
        //    List<object> cad2 = new List<object>();
        //    foreach (var item in cad)
        //    {
        //        int idp = item.Id;

        //        int idaso = PersonaCargoBLL.GetIdAsociado(idp);

        //        string nom = item.Primer_Nom + ' ' + (item.Segundo_Nom ?? "")  + ' ' + item.Primer_Ape + ' ' + (item.Segundo_Ape ?? "");
        //        cad2.Add(new
        //        {
        //            value = item.Nit_CC,
        //            label = nom,
        //            idaso = idaso

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

        

        //// GET: PersCargo/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: PersCargo/Create
        //[HttpGet]
        //public ActionResult Create(int id = 0)
        //{
        //    if (id > 0)
        //    {
        //        ICollection<Personas_Fac> per = PersonaCargoBLL.PersonasACargo(id);

        //        ICollection<dynamic> res = new List<dynamic>();
        //        foreach (var item in per)
        //        {
        //            dynamic d = new ExpandoObject();
        //            string discapacidad = PersonaCargoBLL.GetDiscapacidad(item.PersonasaCargo_Aso.First().Id);
        //            d.tipoId = item.Tipos_Identificaciones_Aso.Descripcion;
        //            d.identificacion = item.Nit_CC;
        //            d.nombre = item.Primer_Nom + " " + (item.Segundo_Nom ?? "")  + " " + item.Primer_Ape + " " + (item.Segundo_Ape ?? "") ;
        //            d.genero = item.Genero ? "M" : "F";
        //            d.fechaN = item.PersonasaCargo_Aso.ElementAt(0).FechaNacimiento + "";
        //            d.fechaI = item.Fecha_Ingreso + "";
        //            d.parentesco = item.PersonasaCargo_Aso.ElementAt(0).Parentescos_Aso.Descripcion;
        //            d.estudiante = item.PersonasaCargo_Aso.ElementAt(0).Estudiante ? "Si" : "No";
        //            d.discapacidad = discapacidad;
        //            res.Add(d);
        //        }

        //        Personas_Fac a = BenBLL.GetPerson(id);
        //        ViewBag.nombre = a.Primer_Nom + " " + (a.Segundo_Nom ?? "") + " " + a.Primer_Ape + " " + (a.Segundo_Ape ?? "");
        //        ViewBag.cedula = a.Nit_CC;
        //        ViewBag.id = id;
        //        ViewBag.res = res;
        //    }


        //    ViewBag.pais = AsoBLL.getListaPaises();
        //    ViewBag.Tipoi = AsoBLL.getListaIdentificaciones();
        //    return View();
        //}

        //// POST: PersCargo/Create--------------------------------------------------------------
        //[HttpPost]
        //public ActionResult Create(FormCollection collection, Personas_Fac per, PersonasaCargo_Aso pc, PersonasaCargoDiscapacidades_Aso pcd, PersonasEstados perest)
        //{
        //    try
        //    {
        //        int n = PersonaCargoBLL.GetNCedulas(per);

                
        //        if (n > 0)
        //        {
        //            ViewBag.pais = AsoBLL.getListaPaises();
        //            ViewBag.Tipoi = AsoBLL.getListaIdentificaciones();
        //            ViewBag.mensaje = "El numero de identificación ya existe";
        //            return View("Create");
        //        }


        //        int idaso = pc.Id_Asociado;
        //        var est = collection["Id_Estudiante"];

        //        if (est == "on")
        //        {
        //            pc.Estudiante = true;
        //        }
        //        if (est == "off")
        //        {
        //            pc.Estudiante = false;
        //        } 
                              
        //        per.Activo = true;
        //        pc.Activo = true;        

        //        bool res = PersonaCargoBLL.CrearPersona(per, pc, pcd, perest);
        //        return RedirectToAction("Details", "Asociado", new { id = idaso });
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}

        //// GET: PersCargo/Edit/5
        //[HttpGet]
        //public ActionResult Edit(int id)
        //{
        //    Personas_Fac per = AsoVetBLL.GetPersona(id);  
        //    return View("",per);
        //}

        //// POST: PersCargo/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection, Personas_Fac per, PersonasaCargo_Aso pc, PersonasaCargoDiscapacidades_Aso pcd)
        //{
        //    try
        //    {
        //        int idaso = Convert.ToInt32(collection["idasociado"]);
        //        var est = collection["Id_Estudiante"];

        //        if (est == "on")
        //        {
        //            pc.Estudiante = true;
        //        }
        //        if (est == "off")
        //        {
        //            pc.Estudiante = false;
        //        }               

        //        bool res = PersonaCargoBLL.EditarPersCargo(id, per, pc, pcd);
        //        return RedirectToAction("Details", "Asociado", new { id = idaso });
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}
               
        //// POST: PersCargo/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, PersonasEstados perest)
        //{
        //    try
        //    {
        //        bool res = PersonaCargoBLL.Delete(id, perest);
                
        //        return Json(res, JsonRequestBehavior.AllowGet); 
                
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}


        
        ////***********************************AUTOCOMPLETE BENEFICIARIOSS*********************
        //[HttpGet]
        //public JsonResult Beneficiarios(int id, string term)
        //{
        //    ICollection<Personas_Fac>
        //        cad = PersonaCargoBLL.Beneficiarios(id, term);
        //    List<object> cad2 = new List<object>();
        //    foreach (var item in cad)
        //    {
        //        int idp = item.Id;
        //        DateTime fecha = item.AsociadosBeneficiarios_Aso.ElementAt(0).Fecha_Nacimiento;
        //        cad2.Add(new
        //        {
        //            value = item.Nit_CC,
        //            tipoidentificacion = item.Id_TipoIdentificacion,
        //            nombre1 = item.Primer_Nom,
        //            nombre2 = (item.Segundo_Nom ?? ""),
        //            apellido1 = item.Primer_Ape,
        //            apellido2 = (item.Segundo_Ape ?? ""),
        //            genero = item.Genero ? true : false,
        //            parentesco = item.AsociadosBeneficiarios_Aso.ElementAt(0).Parentescos_Aso.Id,
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


        //// POST: PERSONAS A CARGO ACTIVAR
        //[HttpPost]
        //public ActionResult Activar(int id, PersonasEstados perest)
        //{
        //    try
        //    {
        //        bool res = PersonaCargoBLL.Activar(id, perest);
                
        //        return Json(res, JsonRequestBehavior.AllowGet); 
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}
        //////////////////////*****************

        //public JsonResult CedulaValida(string id, int asociado)
        //{
        //    bool res = PersonaCargoBLL.CedulaValida(id, asociado);
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}
        
    }
}