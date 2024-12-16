using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace Ingenio.Controllers.Asociados
{
    public class ConfiguracionEmpresaController : Controller
    {

        //ConfiguracionEmpresaBLL confempreBLL = new ConfiguracionEmpresaBLL();
        //AsociadoVetadoBLL AsoVet = new AsociadoVetadoBLL();
             

        //// GET: ConfiguracionEmpresa/Create
        //[HttpGet]
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: ConfiguracionEmpresa/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection, ConfiguracionsEmpresa_Con confemp)
        //{
        //    try
        //    {
        //        confemp.Id_Empresa = 2;
        //        var control = collection["Aso_ControlEgresoIngreso"];
                
        //        if (control == "on")
        //        {
        //            confemp.Aso_ControlEgresoIngreso = true;
        //        }
        //        if (control == "off")
        //        {
        //            confemp.Aso_ControlEgresoIngreso = false;
        //        }

        //        if(confemp.Dias == 0)
        //        {
        //            confemp.Dias = null;
        //        }

        //        bool res = confempreBLL.CrearConfiguracionEmpresa(confemp);
        //        return RedirectToAction("Index");
                                
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}

        //// GET: ConfiguracionEmpresa/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    ConfiguracionsEmpresa_Con confemp = AsoVet.GetConfiguracionEmpresa();
        //    return View("", confemp);
        //}

        //// POST: ConfiguracionEmpresa/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection, ConfiguracionsEmpresa_Con confemp)
        //{
        //    try
        //    {
        //        confemp.Id_Empresa = 2;
        //        var control = collection["Aso_ControlEgresoIngreso"];

        //        if (control == "on")
        //        {
        //            confemp.Aso_ControlEgresoIngreso = true;
        //        }
        //        if (control == "off")
        //        {
        //            confemp.Aso_ControlEgresoIngreso = false;
        //        }

        //        if (confemp.Dias == 0)
        //        {
        //            confemp.Dias = null;
        //        }

        //        bool res = confempreBLL.EditarConfiguracionEmpresa(id, confemp);
        //        return RedirectToAction("Index");

                
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}
      
    }
}
