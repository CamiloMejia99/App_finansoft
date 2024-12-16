using FNTC.Finansoft.Accounting.BLL.FabricaCreditosBll;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.FabricaCreditos;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.FabricaCreditos.Controllers
{
    public class EntesAprobacionController : Controller
    {
        AccountingContext db = new AccountingContext();
        // GET: FabricaCreditos/EntesAprobacion
        public ActionResult Index()
        {
            ViewBag.Agencias = new ConfiguracionBll().obtenerAgencias();
            return View(db.FCSedes.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.Agencias = new ConfiguracionBll().obtenerAgencias();
            ViewBag.Dependencias = new ConfiguracionBll().obtenerDependencias();
            return View();
        }

        [HttpPost]
        public ActionResult Create([System.Web.Http.FromBody] FCSedes FCSedes)
        {
            if (ModelState.IsValid)
            {
                db.FCSedes.Add(FCSedes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(FCSedes);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Agencias = new ConfiguracionBll().obtenerAgencias();
            ViewBag.Dependencias = new ConfiguracionBll().obtenerDependencias();
            FCSedes ep = db.FCSedes.Find(id);
            return View(ep);
        }

        [HttpPost]
        public ActionResult Edit([System.Web.Http.FromBody] FCSedes FCSedes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(FCSedes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(FCSedes);
        }
    }
}