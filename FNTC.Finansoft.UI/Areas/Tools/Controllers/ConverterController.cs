using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Tools.Controllers
{
    public class ConverterController : Controller
    {
        // GET: Tools/Converter
        public ActionResult Numeros2Letras(string numero, bool m = false)
        {
            var letras = Framework.Converters.Numbers.NumerosALetras.Convertir(numero, m);

            return Json(letras, JsonRequestBehavior.AllowGet);
            //return View();
        }
    }
}