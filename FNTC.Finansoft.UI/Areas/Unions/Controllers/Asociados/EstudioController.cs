using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;




namespace Ingenio.Controllers.Asociados
{
    public class EstudioController : Controller
    {

        //EstudioBLL EstBLL = new EstudioBLL();
        //BeneficiarioBLL BenBLL = new BeneficiarioBLL();

        //// GET: CEDULA
        //public JsonResult GetCedula(string term)
        //{
        //    ICollection<Personas_Fac> cad =
        //        cad = EstBLL.GetCedula(term);
        //    List<object> cad2 = new List<object>();
        //    foreach (var item in cad)
        //    {
        //        int idp = item.Id;

        //        string asoci_p = EstBLL.GetProfesion(idp);

        //        List<string> asoci_e = EstBLL.GetEstudio(idp);

        //        List<int> asoci_e2 = EstBLL.GetIdEstudios(idp);

        //        int idaso = EstBLL.GetIdAsociado(idp);

        //        string nom = item.Primer_Nom + ' ' + (item.Segundo_Nom ?? "") + ' ' + item.Primer_Ape + ' ' + (item.Segundo_Ape ?? "");
        //        cad2.Add(new
        //        {
        //            value = item.Nit_CC,
        //            label = nom,
        //            estudios = asoci_e,
        //            profesion = asoci_p,
        //            idest = asoci_e2,
        //            idaso = idaso

        //        });
        //    }
        //    return Json(cad2, JsonRequestBehavior.AllowGet);
        //}


        //// GET: Estudio/Create
        //[HttpGet]
        //public ActionResult Create(int id = 0)
        //{
        //    if (id > 0)
        //    {
        //        Personas_Fac a = BenBLL.GetPerson(id);

        //        ViewBag.nombre = a.Primer_Nom + " " + (a.Segundo_Nom ?? "") + " " + a.Primer_Ape + " " + (a.Segundo_Ape ?? "");
        //        ViewBag.cedula = a.Nit_CC;
        //        ViewBag.id = id;
        //        ViewBag.idEstudio = a.Asociados_Aso.ElementAt(0).Estudios_Aso.Id;
        //        ViewBag.descripcion = a.Asociados_Aso.ElementAt(0).Estudios_Aso.Descripcion;
        //    }
        //    return View();
        //}

        //// POST: Estudio/Create
        //[HttpPost]
        //public ActionResult Create(AsocaidosEstudios_Aso asoest)
        //{
        //    try
        //    {
        //        AsocaidosEstudios_Aso res = EstBLL.CrearEstudio(asoest);
        //        return RedirectToAction("Details", "Asociado", new { id = asoest.Id_Asociado });
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}

        //// GET: Estudio/Edit/5
        //[HttpGet]
        //public ActionResult Edit(int id)
        //{
        //    AsocaidosEstudios_Aso estudios = EstBLL.GetAsociadoEstudios(id);

        //    if (estudios == null)
        //    {
        //        return RedirectToAction("Create", new { id = id });
        //    }

        //    int idp = estudios.Id_Asociado;

        //    string asoci_e = EstBLL.GetNombreEstudio(idp);

        //    int asoci_e2 = EstBLL.GetIdEstudio(idp);

        //    Personas_Fac a = BenBLL.GetPerson(id); 

        //    ViewBag.estudio = asoci_e;
        //    ViewBag.idest = asoci_e2;
        //    ViewBag.institucion = estudios.Institucion;
        //    ViewBag.fecha_grado = estudios.Fecha_Grado;
        //    ViewBag.observaciones = estudios.Observaciones;

        //    ViewBag.nombre = a.Primer_Nom + " " + (a.Segundo_Nom ?? "") + " " + a.Primer_Ape + " " + (a.Segundo_Ape ?? "");
        //    ViewBag.cedula = a.Nit_CC;
        //    ViewBag.id = id;
        //    return View();
        //}

        //// POST: Estudio/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection, AsocaidosEstudios_Aso asoest)
        //{
        //    try
        //    {
        //        int idaso = Convert.ToInt32(collection["Id_Asociado"]);
        //        bool res = EstBLL.EditarEstudio(id, asoest);
        //        return RedirectToAction("Details", "Asociado", new { id = idaso });
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}

        //// GET: Estudio/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}


    }
}
