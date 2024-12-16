using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.BLL.FabricaCreditosBll;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.FabricaCreditos;
using FNTC.Finansoft.Accounting.DTO.MCreditos;


namespace FNTC.Finansoft.UI.Areas.FabricaCreditos.Controllers
{
    
    public class InicioFabricaCreditosController : Controller
    {
        // GET: FabricaCreditos/InicioFabricaCreditos
        public ActionResult Index()
        {

            var useractual = User.Identity.Name;
            ViewBag.User = useractual;
            var permisous = "";
            var permisou = new ConfiguracionBll().Permiso(useractual);

            if (permisou == null)
            {
                permisous = "SinAcceso";
            }
            else
            {
                permisous = new ConfiguracionBll().Permiso(useractual);
            }

            ViewBag.Operario = permisous;
            return View();
        }
        public ActionResult FabricaCredito()
        {
            var useractual = User.Identity.Name;
            ViewBag.User = useractual;
            ViewBag.Operario = new ConfiguracionBll().ObtenerPermiso(useractual);
            ViewBag.Analista = new ConfiguracionBll().ObtenerPermisoA(useractual);
            ViewBag.Ente = new ConfiguracionBll().ObtenerPermisoE(useractual);
            ViewBag.Info = new ConfiguracionBll().ObtenerPermisoI(useractual); 

            return View();
        }
    }
}