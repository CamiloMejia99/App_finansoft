using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.SARLAFT;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.SIAR.Controllers.SARLAFT
{
    public class ProcesoController : Controller
    {
        AccountingContext db = new AccountingContext();
        // GET: SIAR/Proceso
        public ActionResult Index()
        {
            var data = db.speProceso.ToList();

            return View(data);
        }

        public ActionResult Create()
        {
            //inicio select list para responsables
            List<SelectListItem> responsables = new List<SelectListItem>();
            responsables.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            IList<CargosResponsables> listadoResponsables = db.CargosResponsables.Where(x => x.estado == true).ToList();
            foreach (var item in listadoResponsables)
            {
                responsables.Add(new SelectListItem { Text = item.cargo, Value = item.id.ToString() });

            }
            //fin select list para responsables

            //inicio select list para macroprocesos
            List<SelectListItem> macroprocesos = new List<SelectListItem>();
            macroprocesos.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            IList<Macroprocesos> listadoMacroprocesos = db.Macroprocesos.Where(x => x.estado == true).ToList();
            foreach (var item in listadoMacroprocesos)
            {
                macroprocesos.Add(new SelectListItem { Text = item.codigo + " | " + item.nombre, Value = item.id.ToString() });

            }
            //fin select list para responsables


            ViewBag.responsables = responsables;
            ViewBag.macroprocesos = macroprocesos;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([System.Web.Http.FromBody] Procesos Procesos)
        {
            //inicio select list para responsables
            List<SelectListItem> responsables = new List<SelectListItem>();
            responsables.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            IList<CargosResponsables> listadoResponsables = db.CargosResponsables.Where(x => x.estado == true).ToList();
            foreach (var item in listadoResponsables)
            {
                responsables.Add(new SelectListItem { Text = item.cargo, Value = item.id.ToString() });

            }
            //fin select list para responsables

            //inicio select list para macroprocesos
            List<SelectListItem> macroprocesos = new List<SelectListItem>();
            macroprocesos.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            IList<Macroprocesos> listadoMacroprocesos = db.Macroprocesos.Where(x => x.estado == true).ToList();
            foreach (var item in listadoMacroprocesos)
            {
                macroprocesos.Add(new SelectListItem { Text = item.codigo + " | " + item.nombre, Value = item.id.ToString() });

            }
            //fin select list para responsables


            ViewBag.responsables = responsables;
            ViewBag.macroprocesos = macroprocesos;

            if (ModelState.IsValid)
            {
                using (var ctx = new AccountingContext())
                {
                    try
                    {
                        Procesos.estado = true;
                        ctx.speProceso.Add(Procesos);
                        ctx.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException dbE)
                    {

                    }
                }
            }

            return View(Procesos);
        }


        public ActionResult Edit(int id)
        {
            //inicio select list para responsables
            List<SelectListItem> responsables = new List<SelectListItem>();
            responsables.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            IList<CargosResponsables> listadoResponsables = db.CargosResponsables.Where(x => x.estado == true).ToList();
            foreach (var item in listadoResponsables)
            {
                responsables.Add(new SelectListItem { Text = item.cargo, Value = item.id.ToString() });

            }
            //fin select list para responsables

            //inicio select list para macroprocesos
            List<SelectListItem> macroprocesos = new List<SelectListItem>();
            macroprocesos.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            IList<Macroprocesos> listadoMacroprocesos = db.Macroprocesos.Where(x => x.estado == true).ToList();
            foreach (var item in listadoMacroprocesos)
            {
                macroprocesos.Add(new SelectListItem { Text = item.codigo + " | " + item.nombre, Value = item.id.ToString() });

            }
            //fin select list para responsables


            ViewBag.responsables = responsables;
            ViewBag.macroprocesos = macroprocesos;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.speProceso.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([System.Web.Http.FromBody] Procesos Procesos)
        {
            //inicio select list para responsables
            List<SelectListItem> responsables = new List<SelectListItem>();
            responsables.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            IList<CargosResponsables> listadoResponsables = db.CargosResponsables.Where(x => x.estado == true).ToList();
            foreach (var item in listadoResponsables)
            {
                responsables.Add(new SelectListItem { Text = item.cargo, Value = item.id.ToString() });

            }
            //fin select list para responsables

            //inicio select list para macroprocesos
            List<SelectListItem> macroprocesos = new List<SelectListItem>();
            macroprocesos.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            IList<Macroprocesos> listadoMacroprocesos = db.Macroprocesos.Where(x => x.estado == true).ToList();
            foreach (var item in listadoMacroprocesos)
            {
                macroprocesos.Add(new SelectListItem { Text = item.codigo + " | " + item.nombre, Value = item.id.ToString() });

            }
            //fin select list para responsables


            ViewBag.responsables = responsables;
            ViewBag.macroprocesos = macroprocesos;

            if (ModelState.IsValid)
            {
                db.Entry(Procesos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(Procesos);

        }

        [HttpPost]
        public JsonResult Details(int id)
        {

            var datos = db.speProceso.Find(id);
            if (datos != null)
            {

                return new JsonResult { Data = new { status = true, datos } };
            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }

        }

        [HttpPost]
        public JsonResult EstablecerEstado(int id, int value)
        {

            var data = db.speProceso.Find(id);
            if (data != null)
            {
                if (value == 1)
                {
                    data.estado = false;
                }
                else
                {
                    data.estado = true;
                }
                db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return new JsonResult { Data = new { status = true } };
            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }

        }

        public JsonResult verificarCodigo(string codigo)
        {
            return Json(!db.speProceso.Any(lo => lo.codigo == codigo), JsonRequestBehavior.AllowGet);
        }

    }
}