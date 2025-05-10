using System;
using System.Linq;
using System.Web.Mvc;
using FNTC.Finansoft.Data;

namespace FNTC.Finansoft.UI.Controllers
{
    public class PrestamosController : Controller
    {
        public ActionResult VerificarTabla(string nombreTabla)
        {
            using (var ctx = new AccountingContext())
            {
                try
                {
                    // Consulta SQL para verificar si la tabla existe
                    var query = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{nombreTabla}'";
                    var existe = ctx.Database.SqlQuery<int>(query).FirstOrDefault() > 0;
                    
                    return Json(new { existe = existe }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }
    }
} 