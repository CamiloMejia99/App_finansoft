using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace Ingenio.Controllers.Asociados
{
    public class ReferenciasAsoController : Controller
    {
    
        //ReferenciasAsoBLL ReferenciasAsoBLL = new ReferenciasAsoBLL();
        //BeneficiarioBLL BeneBLL = new BeneficiarioBLL();
        //AsociadoBLL AsoBLL = new AsociadoBLL();
        //AsociadoVetadoBLL AsoVet = new AsociadoVetadoBLL();

        //// GET: ReferenciasAso/Create
        //[HttpGet]
        //public ActionResult Create(int id = 0)
        //{
        //    if (id > 0)
        //    {
        //        Personas_Fac a = BeneBLL.GetPerson(id);
        //        ViewBag.nombre = a.Primer_Nom + " " + (a.Segundo_Nom ?? "") + " " + a.Primer_Ape + " " + (a.Segundo_Ape ?? "");
        //        ViewBag.cedula = a.Nit_CC;
        //        ViewBag.id = id;
        //    }

        //    ViewBag.pais = AsoBLL.getListaPaises();
        //    ViewBag.Tipoi = AsoBLL.getListaIdentificaciones();
        //    ViewBag.Tipoestciv = AsoBLL.getListaEstadoCiviles();
        //    return View();
        //}

        //// POST: ReferenciasAso/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection, ReferenciasAsociados_Aso refaso, Personas_Fac per, PersonasEstados perest)
        //{
        //    try
        //    {
        //        int idaso = refaso.Id_Asociado;
        //        var onoff = collection["Activo"];

        //        string cedula2 = collection["Nit_CC"];

        //        per.Activo = true;
        //        refaso.Activo = true;

        //        var verifi = collection["VerificacionReferencia"];

        //        if (verifi == "on")
        //        {
        //            refaso.VerificacionReferencia = true;
        //        }
        //        else
        //        {
        //            refaso.VerificacionReferencia = false;
        //        }

        //        bool res = ReferenciasAsoBLL.CrearReferencia(refaso, per, perest);
        //        return RedirectToAction("Details", "Asociado", new { id = idaso });
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}

        //// GET: ReferenciasAso/Edit/5
        //[HttpGet]
        //public ActionResult Edit(int id)
        //{
        //    Personas_Fac per = AsoVet.GetPersona(id);
        //    return View("", per);
        //}

        //// POST: ReferenciasAso/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, PersonasEstados perest)
        //{
        //    try
        //    {
        //        bool res = ReferenciasAsoBLL.Delete(id, perest);



        //        return Json(res, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}

        //// POST: ReferenciasAso/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection, ReferenciasAsociados_Aso refaso, Personas_Fac per)
        //{
        //    try
        //    {
        //        int idaso = Convert.ToInt32(collection["idasociado"]);


        //        var verifi = collection["VerificacionReferencia"];

        //        if (verifi == "on")
        //        {
        //            refaso.VerificacionReferencia = true;
        //        }
        //        else
        //        {
        //            refaso.VerificacionReferencia = false;
        //        }

        //        bool res = ReferenciasAsoBLL.EditarReferencia(id, refaso, per);
        //        return RedirectToAction("Details", "Asociado", new { id = idaso });
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}

        //// GET: CEDULA
        //public JsonResult GetCedulaRef(int id, string term)
        //{
        //    ICollection<Personas_Fac>
        //        cad = ReferenciasAsoBLL.GetCedulaRef(id, term);
        //    List<object> cad2 = new List<object>();
        //    foreach (var item in cad)
        //    {
        //        int idp = item.Id;

        //        string nom = item.Primer_Nom + ' ' + (item.Segundo_Nom ?? "") + ' ' + item.Primer_Ape + ' ' + (item.Segundo_Ape ?? "");
        //        var refe = item.ReferenciasAsociados_Aso.FirstOrDefault();
        //        string cuenta = refe == null ? "" : refe.Cuenta;
        //        string trabajaen = refe == null ? "" : refe.Trabaja;
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
        //            ciudad = item.Ciudades_Fac.Descripcion,
        //            departamento = item.Ciudades_Fac.Departamentos_Fac.Descripcion,
        //            idPais = item.Ciudades_Fac.Departamentos_Fac.Paises_Fac.Id,// estos de aqui
        //            pais = item.Ciudades_Fac.Departamentos_Fac.Paises_Fac.Descripcion,
        //            estado = item.Activo ? true : false,
        //            cuenta = (cuenta),
        //            trabajaen = (trabajaen),
        //            idper = idp
        //        });
        //    }
        //    return Json(cad2, JsonRequestBehavior.AllowGet);
        //}


        //// POST: Beneficiario/Delete/5
        //[HttpPost]
        //public ActionResult Activar(int id, PersonasEstados perest)
        //{
        //    try
        //    {
        //        bool res = ReferenciasAsoBLL.Activar(id, perest);
        //        return Json(res, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}


        //////////////////////
        //public JsonResult CedulaValida(string id, int asociado)
        //{
        //    bool res = ReferenciasAsoBLL.CedulaValida(id, asociado);
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}


    }
}
