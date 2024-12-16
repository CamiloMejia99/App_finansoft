using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Terceros;

namespace FNTC.Finansoft.UI.Areas.Terceros.Controllers
{
    [Authorize]
    public class InfoTerceroFinancierasController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Terceros/InfoTerceroFinancieras
        public ActionResult Index()
        {
            var infoTerceroFinanciera = db.InfoTerceroFinanciera.Include(i => i.Tercero);
            return View(infoTerceroFinanciera.ToList());
        }

        // GET: Terceros/InfoTerceroFinancieras/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InfoTerceroFinanciera infoTerceroFinanciera = db.InfoTerceroFinanciera.Find(id);
            if (infoTerceroFinanciera == null)
            {
                return HttpNotFound();
            }
            return View(infoTerceroFinanciera);
        }

        // GET: Terceros/InfoTerceroFinancieras/Create
        public ActionResult Create()
        {

            ViewBag.NitTercero = new TercerosController().ConsultarTerceros().ToList().Select(p => new SelectListItem { Text = p.NIT + " || " + p.NOMBRE + " " + p.APELLIDO1 + " " + p.APELLIDO2, Value = p.NIT, Selected = false }); ;

            return View();
        }

        // POST: Terceros/InfoTerceroFinancieras/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,NitTercero,IngresosMensuales,GastosMensuales,PasivosTotales,ActivosTotales")] InfoTerceroFinanciera infoTerceroFinanciera)
        {
            if (ModelState.IsValid)
            {
                db.InfoTerceroFinanciera.Add(infoTerceroFinanciera);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            ViewBag.NitTercero = new TercerosController().ConsultarTerceros().ToList().Select(p => new SelectListItem { Text = p.NIT + " || " + p.NOMBRE + " " + p.APELLIDO1 + " " + p.APELLIDO2, Value = p.NIT, Selected = false }); ;

            return View(infoTerceroFinanciera);
        }

        // GET: Terceros/InfoTerceroFinancieras/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InfoTerceroFinanciera infoTerceroFinanciera = db.InfoTerceroFinanciera.Find(id);
            if (infoTerceroFinanciera == null)
            {
                return HttpNotFound();
            }
            ViewBag.NitTercero = new SelectList(db.Terceros, "NIT", "NIT", infoTerceroFinanciera.NitTercero);
            return View(infoTerceroFinanciera);
        }

        // POST: Terceros/InfoTerceroFinancieras/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NitTercero,IngresosMensuales,GastosMensuales,PasivosTotales,ActivosTotales")] InfoTerceroFinanciera infoTerceroFinanciera)
        {
            if (ModelState.IsValid)
            {
                db.Entry(infoTerceroFinanciera).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.NitTercero = new SelectList(db.Terceros, "NIT", "NIT", infoTerceroFinanciera.NitTercero);
            return View(infoTerceroFinanciera);
        }

        // GET: Terceros/InfoTerceroFinancieras/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InfoTerceroFinanciera infoTerceroFinanciera = db.InfoTerceroFinanciera.Find(id);
            if (infoTerceroFinanciera == null)
            {
                return HttpNotFound();
            }
            return View(infoTerceroFinanciera);
        }

        // POST: Terceros/InfoTerceroFinancieras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InfoTerceroFinanciera infoTerceroFinanciera = db.InfoTerceroFinanciera.Find(id);
            db.InfoTerceroFinanciera.Remove(infoTerceroFinanciera);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GetTerceroFinByNIT(string nit = "")
        {

            if (nit.Length > 0)
            {

                var dto = GetTerceroFinancieroByNIT(nit);
                if (dto != null)
                {
                    return Json(dto, JsonRequestBehavior.AllowGet);
                }

                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public InfoTerceroFinanciera GetTerceroFinancieroByNIT(string nit)
        {
            using (var ctx = new AccountingContext())
            {
                var terceroAD = ctx.InfoTerceroFinanciera.Where(x => x.NitTercero == nit).FirstOrDefault();
                return terceroAD;
            }

        }
        public ActionResult GetTercerosFinanciera(string term = "", string outputFormat = "datatables")
        {
            var terceros = db.InfoTerceroFinanciera.ToList();
            var _tercerosDTO = terceros;

            switch (outputFormat)
            {
                case "datatables":
                    var dataTabledata = _tercerosDTO.Select((x, index)

                    => new[] { x.NitTercero.ToString(), x.Tercero.NOMBRE1 + " " + x.Tercero.NOMBRE2 + " " + x.Tercero.APELLIDO1 + " " + x.Tercero.APELLIDO1, x.IngresosMensuales.ToString(), x.GastosMensuales.ToString(), x.ActivosTotales.ToString(), x.PasivosTotales.ToString(), BotonEditar(x.Id.ToString()), BotonEliminar(x.Id.ToString()) });

                    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    serializer.MaxJsonLength = int.MaxValue;
                    var json = Json(new { data = dataTabledata }, JsonRequestBehavior.AllowGet);
                    json.MaxJsonLength = int.MaxValue;
                    return json;

                default:
                    serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    serializer.MaxJsonLength = int.MaxValue;
                    json = Json(new { data = _tercerosDTO }, JsonRequestBehavior.AllowGet);
                    json.MaxJsonLength = int.MaxValue;
                    return json;

            }
        }

        private string BotonEditar(string id)
        {
            var botonEditar = @"<a href='/Terceros/InfoTerceroFinancieras/edit/" + id + "' class='btn btn-default fa fa-pencil edit' ></a>";
            return botonEditar;
        }

        private string BotonEliminar(string id)
        {
            var botonEliminar = @"<a href='/Terceros/InfoTerceroFinancieras/delete/" + id + "' class='btn btn-danger glyphicon glyphicon-trash'></a>";
            return botonEliminar;
        }



        public Boolean HayDatos(string nit = "")
        {
            if (nit.Length > 0)
            {
                var dto = GetTerceroFinancieroByNIT(nit);
                if (dto != null)
                {
                    return true;
                }
                return false;
            }

            return false;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
