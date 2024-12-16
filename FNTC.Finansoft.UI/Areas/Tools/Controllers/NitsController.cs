
using FNTC.Framework.Linq;
using System.Web.Mvc;
using FNTC.Framework;
using FNTC.Framework.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FNTC.Finansoft.Accounting.DTO;

namespace FNTC.Finansoft.UI.Areas.Tools.Controllers
{
    public class NitsController : Controller
    {
        // GET: Tools/Nits
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetDigitoVerificacion(string nit="")
        {
            //if (nit.Length == 0)
            //{
            //    return Json(new
            //    {
            //        Nit = "",
            //        DV = ""
            //    }
            //    , JsonRequestBehavior.AllowGet);
            //}
            return Json(
                new
                {
                    Nit = nit,
                    DV = FNTC.Framework.NITS.Helpers.CalcularDigitoVerificacion_NIT(nit)
                }
                , JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult GetDependenciasbyEmpresa(string Nit)
        {
            //var nit = Convert.ToInt32(Nit);
            using (var ctx = new AccountingContext())
            {
                var dependencias = from terdep in ctx.TerceroDependencia
                                   join dep in ctx.Dependencia on terdep.Cod_dependencia equals dep.Id_dep
                                   join ter in ctx.Terceros on terdep.Nit_tercero equals ter.NIT
                                   where terdep.Nit_tercero == Nit && terdep.Tipo_dep == 1
                                   select new
                                   {
                                       dep.Id_dep,
                                       dep.Nom_dep,
                                       ter.NombreComercial,
                                   };
                                   
                return Json(dependencias
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Nom_dep + " - " + x.NombreComercial,
                        Value = x.Id_dep.ToString()
                    }).ToList().OrderBy(x => x.Value), JsonRequestBehavior.AllowGet);
            }
        }
    }
}