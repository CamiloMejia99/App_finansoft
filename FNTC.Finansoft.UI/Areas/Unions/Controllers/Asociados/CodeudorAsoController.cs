using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



using System.Dynamic;

namespace Ingenio.Controllers.Asociados
{
    public class CodeudorAsoController : Controller
    {
        
        //CodeudorAsoBLL CodeAsoBLL = new CodeudorAsoBLL();
        //BeneficiarioBLL BenBLL = new BeneficiarioBLL();
        //AsociadoVetadoBLL AsoVetBLL = new AsociadoVetadoBLL();
        //AsociadoBLL AsoBLL = new AsociadoBLL();
       

        //// GET: Codeudor/Create
        //[HttpGet]
        //public ActionResult Create(int id = 0)
        //{
        //    if (id > 0)
        //    {
        //        Personas_Fac a = BenBLL.GetPerson(id);
        //        ViewBag.nombre = a.Primer_Nom + " " + (a.Segundo_Nom ?? "") + " " + a.Primer_Ape + " " + (a.Segundo_Ape ?? "");
        //        ViewBag.cedula = a.Nit_CC;
        //        ViewBag.id = id;
        //    }

        //    ConfiguracionsEmpresa_Con confemp = AsoVetBLL.GetConfiguracionEmpresa();

        //    ViewBag.numerocodeudores = confemp.NumeroCodeudores;
        //    ViewBag.pais = AsoBLL.getListaPaises();
        //    ViewBag.reg = AsoBLL.getListaTipoEstudios();
        //    ViewBag.Tipoi = AsoBLL.getListaIdentificaciones();
        //    ViewBag.Tipoc = AsoBLL.getListaTipoContribuyente();
        //    ViewBag.Tipoest = AsoBLL.getListaTipoEstudios();
        //    ViewBag.Tipoestciv = AsoBLL.getListaEstadoCiviles(); 
        //    return View();
        //}

        //// POST: Codeudor/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection, Personas_Fac per, Codeudores_Aso cod, PersonasEstados perest, AsociadosCodeudores_Aso asocode)
        //{
        //    try
        //    {
        //        asocode.Id = 0;
        //        perest.Id = 0;
        //        per.Id = 0;
        //        cod.Id = 0;
        //        string direccion = collection["Direccion"].ToUpper().Trim();
        //        string correo = collection["Correo"].ToUpper().Trim();
        //        string telefono = collection["Telefono"];
        //        string extencion = collection["ext"];
        //        string celular = collection["Celular"];
        //        string fechaInicioAso = collection["Fecha_Inicio_Aso"];
        //        string fechaInicioEmp = collection["Fecha_Inicio_Emp"];
                

                

        //        string[] ubicaciones = new string[5];
        //        ubicaciones[0] = direccion;
        //        ubicaciones[1] = correo;
        //        ubicaciones[2] = telefono;
        //        ubicaciones[3] = extencion;
        //        ubicaciones[4] = celular;

        //        per.Activo = true;
        //        asocode.Activo = true;

        //        int idaso = asocode.Id_Asociado;


        //        bool res = CodeAsoBLL.CrearCodeudor(ubicaciones, per, cod, perest, asocode);
        //        return RedirectToAction("Details", "Asociado", new { id = idaso });
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}

        //// GET: Codeudor/Edit/5
        //public ActionResult Edit(int id, int idasociado)
        //{
        //    ViewBag.idAsociado = idasociado;
        //    Personas_Fac per = AsoVetBLL.GetPersona(id); 
        //    Codeudores_Aso codeudor = per.Codeudores_Aso.FirstOrDefault();


        //    ViewBag.idciudadex = per.Codeudores_Aso.FirstOrDefault().Ciudades_Fac.Id;
        //    ViewBag.ciudadex = per.Codeudores_Aso.FirstOrDefault().Ciudades_Fac.Descripcion;
        //    ViewBag.iddepartamentoex = per.Codeudores_Aso.FirstOrDefault().Ciudades_Fac.Departamentos_Fac.Id;
        //    ViewBag.departamentoex = per.Codeudores_Aso.FirstOrDefault().Ciudades_Fac.Departamentos_Fac.Descripcion;
        //    ViewBag.idpaisex = per.Codeudores_Aso.FirstOrDefault().Ciudades_Fac.Departamentos_Fac.Paises_Fac.Id;
        //    ViewBag.paisex = per.Codeudores_Aso.FirstOrDefault().Ciudades_Fac.Departamentos_Fac.Paises_Fac.Descripcion;

        //    ViewBag.direccion = per.PersonasUbicaciones.Where(x => (x.Id_TUbicacion == 1)).Select(x => x.Descripcion).FirstOrDefault() ?? "";
        //    ViewBag.correo = per.PersonasUbicaciones.Where(x => (x.Id_TUbicacion == 2)).Select(x => x.Descripcion).FirstOrDefault() ?? "";
        //    ViewBag.telefono = per.PersonasUbicaciones.Where(x => (x.Id_TUbicacion == 3)).Select(x => x.Descripcion).FirstOrDefault() ?? "";
        //    ViewBag.ext = per.PersonasUbicaciones.Where(x => (x.Id_TUbicacion == 4)).Select(x => x.Descripcion).FirstOrDefault() ?? "";
        //    ViewBag.celular = per.PersonasUbicaciones.Where(x => (x.Id_TUbicacion == 5)).Select(x => x.Descripcion).FirstOrDefault() ?? "";
            
        //    ViewBag.codeudores = codeudor;
        //    return View("",per);
        //}

        //// POST: Codeudor/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection, Personas_Fac per, Codeudores_Aso cod)
        //{
        //    try
        //    {                                
        //        per.Id = 0;
        //        cod.Id = 0;
        //        per.Activo = true;
        //        string direccion = collection["Direccion"].ToUpper().Trim();
        //        string correo = collection["Correo"].ToUpper().Trim();
        //        string telefono = collection["Telefono"];
        //        string extencion = collection["ext"];
        //        string celular = collection["Celular"];
        //        string fechaInicioAso = collection["Fecha_Inicio_Aso"];
        //        string fechaInicioEmp = collection["Fecha_Inicio_Emp"];
                
        //        int idaso = Convert.ToInt32(collection["idasociado"]);
        //        string[] ubicaciones = new string[5];
        //        ubicaciones[0] = direccion;
        //        ubicaciones[1] = correo;
        //        ubicaciones[2] = telefono;
        //        ubicaciones[3] = extencion;
        //        ubicaciones[4] = celular;

        //        per.Activo = true;

        //        bool res = CodeAsoBLL.EditarCodeudor(id, ubicaciones, per, cod);
                
        //        return RedirectToAction("Details", "Asociado", new { id = idaso });
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}

       
        //// POST: Codeudor/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, PersonasEstados perest)
        //{
        //    try
        //    {
        //        bool res = CodeAsoBLL.Delete(id, perest);



        //        return Json(res, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}


        //// POST: ACTIVAR CODEUDOR
        //[HttpPost]
        //public ActionResult Activar(int id, PersonasEstados perest)
        //{
        //    try
        //    {
        //        bool res = CodeAsoBLL.Activar(id, perest);

        //        return Json(res, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}



        //// GET: CODEUDOR
        //public JsonResult GetCedulaCo(string term)
        //{
        //    ICollection<Personas_Fac>
        //        cad = CodeAsoBLL.GetCedulaCo(term);
        //    List<object> cad2 = new List<object>();
        //    foreach (var item in cad)
        //    {
        //        int idp = item.Id;
        //        int n = 0;

        //        Codeudores_Aso codeudor = CodeAsoBLL.GetCodeudorAso(idp);

        //        if (codeudor != null)
        //        {
        //            n = CodeAsoBLL.NCodeudores(idp, codeudor.Id);

        //        }
                
        //        string nom = item.Primer_Nom + ' ' + (item.Segundo_Nom ?? "") + ' ' + item.Primer_Ape + ' ' + (item.Segundo_Ape ?? "");
        //        var refe = item.ReferenciasAsociados_Aso.FirstOrDefault();
        //        string cuenta = refe == null ? "" : refe.Cuenta;
        //        string trabajaen = refe == null ? "" : refe.Trabaja;
        //        string fn = "";
        //        string cex = "";
        //        int idcex = -1;
        //        string dex = "";
        //        int idpex = -1; 
        //        string pex = "";

        //        string ciiudescrip= "";
        //        int ciiuidentif = -1;
        //        int estratoaso = -1;
        //         if (item.Id_Tipo == 6)
        //            {   
        //                fn = item.Asociados_Aso.FirstOrDefault().Fecha_Nacimiento.ToString("yyyy-MM-dd");
        //                idcex = item.Asociados_Aso.FirstOrDefault().Ciudades_Fac.Id;
        //                cex = item.Asociados_Aso.FirstOrDefault().Ciudades_Fac.Descripcion;
        //                dex = item.Asociados_Aso.FirstOrDefault().Ciudades_Fac.Departamentos_Fac.Descripcion;
        //                idpex = item.Asociados_Aso.FirstOrDefault().Ciudades_Fac.Departamentos_Fac.Paises_Fac.Id;
        //                pex = item.Asociados_Aso.FirstOrDefault().Ciudades_Fac.Departamentos_Fac.Paises_Fac.Descripcion;
        //                ciiudescrip = item.Asociados_Aso.FirstOrDefault().CIIU_Aso.Descripcion;
        //                ciiuidentif = item.Asociados_Aso.FirstOrDefault().Id_CIIU;
        //                estratoaso = item.Asociados_Aso.FirstOrDefault().Id_Estrato;
        //            }
        //        if (item.Id_Tipo == 7)
        //            {   
        //                fn = item.PersonasaCargo_Aso.FirstOrDefault().FechaNacimiento.ToString("yyyy-MM-dd");
        //            }
        //        if (item.Id_Tipo == 8)
        //            {   
        //                fn = item.AsociadosBeneficiarios_Aso.FirstOrDefault().Fecha_Nacimiento.ToString("yyyy-MM-dd");
        //            }

        //        if (item.Id_Tipo == 10 || codeudor != null)
        //        {
        //            fn = item.Codeudores_Aso.FirstOrDefault().FechaNacimiento.ToString("yyyy-MM-dd");
        //            cex = item.Codeudores_Aso.FirstOrDefault().Ciudades_Fac.Descripcion;
        //            dex = item.Codeudores_Aso.FirstOrDefault().Ciudades_Fac.Departamentos_Fac.Descripcion;
        //            idpex = item.Codeudores_Aso.FirstOrDefault().Ciudades_Fac.Departamentos_Fac.Paises_Fac.Id;
        //            pex = item.Codeudores_Aso.FirstOrDefault().Ciudades_Fac.Departamentos_Fac.Paises_Fac.Descripcion;
        //            ciiudescrip = item.Codeudores_Aso.FirstOrDefault().CIIU_Aso.Descripcion;
        //            ciiuidentif = item.Codeudores_Aso.FirstOrDefault().Id_CIIU;
        //            estratoaso = item.Codeudores_Aso.FirstOrDefault().Id_Estrato;
        //        }

                

        //        cad2.Add(new
        //        {
        //            value = item.Nit_CC,
        //            label = nom,
        //            tipoidentificacion = item.Id_TipoIdentificacion,
        //            nombre1 = item.Primer_Nom,
        //            nombre2 = (item.Segundo_Nom ?? ""),
        //            apellido1 = item.Primer_Ape,
        //            apellido2 = (item.Segundo_Ape ?? ""),
        //            genero = item.Genero ? true : false,
        //            fechanacimiento = fn,

        //            idciudadre = item.Ciudades_Fac.Id,
        //            ciudadre = item.Ciudades_Fac.Descripcion,
        //            departamentore = item.Ciudades_Fac.Departamentos_Fac.Descripcion,
        //            idPaisre = item.Ciudades_Fac.Departamentos_Fac.Paises_Fac.Id,
        //            paisre = item.Ciudades_Fac.Departamentos_Fac.Paises_Fac.Descripcion,

        //            idciudadex = idcex,
        //            ciudadex = cex,
        //            departamentoex = dex,
        //            idPaisex = idpex,
        //            paisex = pex,

        //            ciiudes = ciiudescrip,
        //            ciiuid = ciiuidentif,
        //            estrato = estratoaso,


        //            direccion = item.PersonasUbicaciones.Where(x => (x.Id_TUbicacion == 1)).Select(x => x.Descripcion).FirstOrDefault() ?? "",
        //            correo = item.PersonasUbicaciones.Where(x => (x.Id_TUbicacion == 2)).Select(x => x.Descripcion).FirstOrDefault() ?? "",
        //            telefono = item.PersonasUbicaciones.Where(x => (x.Id_TUbicacion == 3)).Select(x => x.Descripcion).FirstOrDefault() ?? "",
        //            ext = item.PersonasUbicaciones.Where(x => (x.Id_TUbicacion == 4)).Select(x => x.Descripcion).FirstOrDefault() ?? "",
        //            celular = item.PersonasUbicaciones.Where(x => (x.Id_TUbicacion == 5)).Select(x => x.Descripcion).FirstOrDefault() ?? "",
        //            tipopersona = item.Id_Tipo,
        //            idper = idp,
        //            numerocodeudor = n
        //        });
        //    }
        //    return Json(cad2, JsonRequestBehavior.AllowGet);
        //}


        //////////////////////
        //public JsonResult CedulaValida(string id, int asociado)
        //{
        //    bool res = CodeAsoBLL.CedulaValida(id, asociado);
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}
    }
}
