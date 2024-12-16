using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.BLL.FabricaCreditosBll;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.FabricaCreditos;
using FNTC.Finansoft.Accounting.DTO.MCreditos;

namespace FNTC.Finansoft.UI.Areas.FabricaCreditos.Controllers
{
    public class InicioCarteraController : Controller
    {
        // GET: FabricaCreditos/InicioCartera
        public ActionResult Index()
        {
            return View();
        }
    }
}


