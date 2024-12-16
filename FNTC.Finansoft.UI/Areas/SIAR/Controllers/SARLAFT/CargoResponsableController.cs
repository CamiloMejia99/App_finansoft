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
    public class CargoResponsableController : Controller
    {
        AccountingContext db = new AccountingContext();
        // GET: SIAR/CargosResponsables
        public ActionResult Index()
        {
            var data = db.CargosResponsables.ToList();
            return View(data);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([System.Web.Http.FromBody] CargosResponsables CargosResponsables)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new AccountingContext())
                {
                    try
                    {
                        CargosResponsables.estado = true;
                        ctx.CargosResponsables.Add(CargosResponsables);
                        ctx.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException dbE)
                    {

                    }
                }
            }

            return View(CargosResponsables);
        }


        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.CargosResponsables.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([System.Web.Http.FromBody] CargosResponsables CargoResponsable)
        {

            if (ModelState.IsValid)
            {
                db.Entry(CargoResponsable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(CargoResponsable);

        }

        [HttpPost]
        public JsonResult Details(int id)
        {

            var datos = db.CargosResponsables.Find(id);
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

            var data = db.CargosResponsables.Find(id);
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

    }
}