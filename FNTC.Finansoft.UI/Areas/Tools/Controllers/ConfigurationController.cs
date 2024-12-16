using FNTC.Finansoft.Accounting.DTO.Terceros;
using System.Linq;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Tools.Controllers
{
    public class ConfigurationController : Controller
    {

        //public TerceroDTO dto { get; set; }
        // GET: Tools/Configuration
        public ActionResult Index()
        {
            var dto = new TerceroDTO();
            init(ref dto);

            return View(dto);
        }

        // GET: Tools/Configuration/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tools/Configuration/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tools/Configuration/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Tools/Configuration/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Tools/Configuration/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Tools/Configuration/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Tools/Configuration/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        private void init(ref TerceroDTO dto)
        {
            dto._paises = new FNTC.Framework.GeoDAL().GetPaises()
                   .Select(x => new SelectListItem() { Text = x.PaisNombre + "-" + x.PaisId, Value = x.PaisId }).ToList();

            dto._regimen = new FNTC.Framework.Params.DAL.ParamsDAL()
                .GetParamValues("REGIMEN").Select(x => new SelectListItem() { Text = x.Valor, Value = x.Codigo }).ToList();


            dto._clasesID = new FNTC.Framework.Params.DAL.ParamsDAL()
                .GetParamValues("TIPODOCUMENTO")
                .Select(x => new SelectListItem()
                {
                    Value = x.Codigo,
                    Text = x.Valor
                })
                    .ToList();

        }
    }
}
