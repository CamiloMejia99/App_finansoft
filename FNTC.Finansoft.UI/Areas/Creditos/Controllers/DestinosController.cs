using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.MCreditos;

namespace FNTC.Finansoft.UI.Areas.Creditos.Controllers
{
    public class DestinosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Creditos/Destinos
        public ActionResult Index()
        {
            var destinos = db.Destinos.Include(d => d.Lineas);
            return View(destinos.ToList());
        }

        // GET: Creditos/Destinos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destinos destinos = db.Destinos.Find(id);
            if (destinos == null)
            {
                return HttpNotFound();
            }
            return View(destinos);
        }

        // GET: Creditos/Destinos/Create
        public ActionResult Create()
        {
            // IList<Lineas> ListaLineas = (from l in db.Lineas where l.Lineas_Activo == true select l).ToList();
            ViewBag.Lineas_Id = new SelectList((from l in db.Lineas where l.Lineas_Activo == true select l), "Lineas_Id", "Lineas_Descripcion");
            return View();
        }

        // POST: Creditos/Destinos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Destino_Id,Destino_Codigo,Destino_Valor_Maximo,Destino_Valor_Minimo,Destino_Tasa_Minima,Destino_Tasa_Maxima,Lineas_Id,Destino_Descripcion,Destino_Tasa_Mora,Destino_Periodo_Maximo,Destino_Periodo_Minimo,Destino_Financia_Interes_Corriente,Destino_Interes_Abono_Extra_Dias,Destino_Interes_Igual_Plan_Pagos,Destino_Causal_Interes_Anticipado,Destino_Dias_Periodo_Gracia,Destino_Pagare_Automatico,Destino_Fecha_Automatica,Destino_Cobra_Festivos,Destino_Activo")] Destinos destinos)
        {
            if (ModelState.IsValid)
            {
                db.Destinos.Add(destinos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Lineas_Id = new SelectList(db.Lineas, "Lineas_Id", "Lineas_Descripcion", destinos.Lineas_Id);
            return View(destinos);
        }

        // GET: Creditos/Destinos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destinos destinos = db.Destinos.Find(id);
            if (destinos == null)
            {
                return HttpNotFound();
            }
            ViewBag.Lineas_Id = new SelectList((from l in db.Lineas where l.Lineas_Activo == true select l), "Lineas_Id", "Lineas_Descripcion", destinos.Lineas_Id);
            return View(destinos);
        }

        // POST: Creditos/Destinos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Destino_Id,Destino_Codigo,Destino_Valor_Maximo,Destino_Valor_Minimo,Destino_Tasa_Minima,Destino_Tasa_Maxima,Lineas_Id,Destino_Descripcion,Destino_Tasa_Mora,Destino_Periodo_Maximo,Destino_Periodo_Minimo,Destino_Financia_Interes_Corriente,Destino_Interes_Abono_Extra_Dias,Destino_Interes_Igual_Plan_Pagos,Destino_Causal_Interes_Anticipado,Destino_Dias_Periodo_Gracia,Destino_Pagare_Automatico,Destino_Fecha_Automatica,Destino_Cobra_Festivos,Destino_Activo")] Destinos destinos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(destinos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Lineas_Id = new SelectList(db.Lineas, "Lineas_Id", "Lineas_Descripcion", destinos.Lineas_Id);
            return View(destinos);
        }

        // GET: Creditos/Destinos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destinos destinos = db.Destinos.Find(id);
            if (destinos == null)
            {
                return HttpNotFound();
            }
            return View(destinos);
        }

        // POST: Creditos/Destinos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Destinos destinos = db.Destinos.Find(id);
            db.Destinos.Remove(destinos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Destinos(int Lineas_Id)
        {
            var dest = (from m in db.Destinos
                        where (m.Lineas_Id == Lineas_Id)
                        select new { m.Destino_Id, m.Destino_Descripcion }).Distinct().ToList();

            return Json(dest
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Destino_Descripcion,
                        Value = x.Destino_Id.ToString()
                    }).ToList().OrderBy(x => x.Value), JsonRequestBehavior.AllowGet);
        }

    }
}
