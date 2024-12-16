using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using System.Dynamic;


namespace Ingenio.Controllers.Asociados
{
    public class AsociadoMuerteController : Controller
    {
       
        //AsociadoMuerteBLL AsoMuerBLL = new AsociadoMuerteBLL();

        //// GET: AsociadoMuerte
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: AsociadoMuerte/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: AsociadoMuerte/Create
        //[HttpGet]
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: AsociadoMuerte/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection, AsociadosMuertes_Aso asomuer, PersonasEstados perest)
        //{
        //    try
        //    {
                
        //        var aseguradora = collection["EnviadoAseguradora"];

        //        if (aseguradora == "on")
        //        {
        //            asomuer.EnviadoAseguradora = true;
        //        }
        //        else
        //        {
        //            asomuer.EnviadoAseguradora = false;
        //        }                
                
        //        //FECHA ACTUAL - FECHA DE MUERTE
        //        DateTime fecha = DateTime.UtcNow;
        //        DateTime fechaactual = fecha.ToLocalTime();

        //        asomuer.FechaInforme = fechaactual;

        //        bool res = AsoMuerBLL.MatarAsociado(asomuer, perest);
        //        return RedirectToAction("Index", "Asociado");
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}

        //// GET: AsociadoMuerte/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: AsociadoMuerte/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: AsociadoMuerte/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: AsociadoMuerte/Delete/5
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

        ////GET: CAUSA

        //public JsonResult GetCausa(string term)
        //{
        //    ICollection<Causas_Muertes_Aso> cad =
        //        cad = AsoMuerBLL.GetCausa(term);
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

        //public JsonResult AddCausal(Causas_Muertes_Aso cm, string nombre)
        //{
            
        //    var nombre2 = nombre.ToUpper().Trim();

        //    int cad = AsoMuerBLL.BuscarNombre(nombre);

        //    if (cad == 0)
        //    {
        //        bool res = AsoMuerBLL.AddCausal(cm, nombre2);

                
        //        return Json(res, JsonRequestBehavior.AllowGet); 
        //    }
        //    else
        //    {
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //    }            

        //}

        //public JsonResult CedulaValida(string id)
        //{
        //    bool res = AsoMuerBLL.CedulaValida(id);
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}



    }
}
