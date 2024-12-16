using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.DeterioroCartera;

namespace FNTC.Finansoft.UI.Areas.DeterioroCartera.Controllers
{
    [Authorize]
    public class DeterioroParametrosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: DeterioroCartera/DeterioroParametros
        public ActionResult Index()
        {

            ViewBag.Comercial = parametrostipo("COMERCIAL");
            ViewBag.Consumo = parametrostipo("CONSUMO");
            ViewBag.Vivienda = parametrostipo("VIVIENDA");
            ViewBag.Microcredito = parametrostipo("MICROCREDITOS");
                        
            return View(db.DeterioroPars.ToList());
        }

        // GET: DeterioroCartera/DeterioroParametros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeterioroPar deterioroPar = db.DeterioroPars.Find(id);
            if (deterioroPar == null)
            {
                return HttpNotFound();
            }
            return View(deterioroPar);
        }

        // GET: DeterioroCartera/DeterioroParametros/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DeterioroCartera/DeterioroParametros/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Rango,Desde,Hasta,PProvision")] DeterioroPar deterioroPar)
        {
            if (ModelState.IsValid)
            {
                db.DeterioroPars.Add(deterioroPar);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(deterioroPar);
        }

        // GET: DeterioroCartera/DeterioroParametros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeterioroPar deterioroPar = db.DeterioroPars.Find(id);
            if (deterioroPar == null)
            {
                return HttpNotFound();
            }
            return View(deterioroPar);
        }

        // POST: DeterioroCartera/DeterioroParametros/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Rango,Desde,Hasta,PProvision")] DeterioroPar deterioroPar)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deterioroPar).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(deterioroPar);
        }

        // GET: DeterioroCartera/DeterioroParametros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeterioroPar deterioroPar = db.DeterioroPars.Find(id);
            if (deterioroPar == null)
            {
                return HttpNotFound();
            }
            return View(deterioroPar);
        }

        // POST: DeterioroCartera/DeterioroParametros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeterioroPar deterioroPar = db.DeterioroPars.Find(id);
            db.DeterioroPars.Remove(deterioroPar);
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

        public List<DeterioroPar> parametrostipo(string tipo)
        {
            var parametros = db.DeterioroPars.Where(t => t.TipoProvision == tipo).ToList();
            if (parametros == null) return null;
            return parametros;
        }
        public List<DeterioroPar> parametrosprovision()
        {
            var parametros = db.DeterioroPars.ToList();
            if (parametros == null) return null;
            return parametros;
        }
        public List<DeterioroPar> parametrosid(int id)
        {
            var parametros = db.DeterioroPars.Where(t => t.Id == id).ToList();
            if (parametros == null) return null;
            return parametros;
        }
    }
}
