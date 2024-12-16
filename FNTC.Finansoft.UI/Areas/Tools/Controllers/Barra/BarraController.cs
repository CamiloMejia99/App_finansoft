using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Tools.Controllers
{
    public class BarraController : Controller
    {
        // GET: Tools/Barra
        public ActionResult Index()
        {
            return PartialView();
        }
    }


}
