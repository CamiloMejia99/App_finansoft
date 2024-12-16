using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using System.Dynamic;

namespace Ingenio.Controllers.Asociados
{
    public class AsociadoVetadoController : Controller
    {
        
        //AsociadoVetadoBLL AsoVetBLL = new AsociadoVetadoBLL();
        //// GET: AsociadoVetado
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: AsociadoVetado/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: AsociadoVetado/Create
        //[HttpGet]
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: AsociadoVetado/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection, AsociadosVetados_Aso asovet, PersonasEstados perest)
        //{
        //    try
        //    {
        //        string nota = collection["Nota"].ToUpper().Trim();
        //        asovet.Nota = nota;

        //        //FECHA ACTUAL - FECHA DE VETO
        //        DateTime fecha = DateTime.UtcNow;
        //        DateTime fechaactual = fecha.ToLocalTime();

        //        asovet.Fecha = fechaactual;

        //        bool res = AsoVetBLL.VetarAsociado(asovet, perest);
        //        return RedirectToAction("Index","Asociado");
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}

        //// GET: AsociadoVetado/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}


        //[HttpPost]
        //public ActionResult Edit(int id, PersonasEstados pe)
        //{
        //    try
            
        //    {
        //        ConfiguracionsEmpresa_Con fechaconf = AsoVetBLL.GetConfiguracionEmpresa();

        //        int idper = AsoVetBLL.GetIdAsociado(id);

        //        PersonasEstados vetado = AsoVetBLL.GetPersonasEstados(idper);                

        //        //FECHA ACTUAL - FECHA DE VETO
        //        DateTime fecha = DateTime.UtcNow;
        //        DateTime fechaactual = fecha.ToLocalTime();

        //        if (fechaconf.Aso_ControlEgresoIngreso == true)
        //        {
        //            var dias = fechaconf.Dias;
        //            var diasv = vetado.Fecha;                    

        //            var dias2 = (fechaactual.Subtract(diasv)).Days;                   

        //            if (dias2 < dias)
        //            {
        //                return Json(false, JsonRequestBehavior.AllowGet);    
        //            }

        //        }


        //        Personas_Fac p = AsoVetBLL.GetPersona(idper);                
                
        //        p.Activo = true;                

        //        pe.Id_Persona = idper;
        //        pe.Id_Estado = 1;
        //        pe.Nota = "ASOCIADO ACTIVO NUEVAMENTE";
        //        pe.Fecha = fechaactual;      

        //        bool res = AsoVetBLL.Edit(p, pe);

        //        return Json(res, JsonRequestBehavior.AllowGet);                
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}

        //// GET: AsociadoVetado/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: AsociadoVetado/Delete/5
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


        ////GET: RAZON

        //public JsonResult GetRazon(string term)
        //{
        //    ICollection<RazonesVetados_Aso> cad =
        //        cad = AsoVetBLL.GetRazon(term);
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

        //public JsonResult AddRazon(RazonesVetados_Aso rv, string nombre)
        //{ 
        //    var nombre2 = nombre.ToUpper().Trim();

        //    int cad = AsoVetBLL.NRazonesVetados(nombre2);            

        //    if (cad == 0)
        //    {
        //        bool res = AsoVetBLL.AddRazon(rv, nombre2);

        //        return Json(res, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //    }

        //}



        //public JsonResult CedulaValida(string id)
        //{
        //    bool res = AsoVetBLL.CedulaValida(id);            
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}




    }
}
