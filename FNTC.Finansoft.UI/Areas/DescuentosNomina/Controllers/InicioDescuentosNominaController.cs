using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace FNTC.Finansoft.UI.Areas.DescuentosNomina.Controllers
{
    [Authorize]
    public class InicioDescuentosNominaController : Controller
    {
        //SE CREA EL INICIO PARA DESCUENTOS DE NOMINA
        public ActionResult InicioDescuentosNomina()
        {
            return View();
        }
        public ActionResult ModulosDescuentosNomina()
        {
            return View();
        }
    }
}