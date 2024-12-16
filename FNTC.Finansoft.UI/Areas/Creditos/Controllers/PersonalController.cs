using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using FNTC.Finansoft.Accounting.DTO.Terceros;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.Accounting.DTO;

namespace FNTC.Finansoft.UI.Areas.Creditos.Controllers
{
    public class PersonalController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: /Personal/
        public ActionResult Index()
        {
            return View(db.Personal.ToList());
        }

        // GET: /Personal/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personal personal = db.Personal.Find(id);
            if (personal == null)
            {
                return HttpNotFound();
            }
            return View(personal);
        }

        // GET: /Personal/Create
        public ActionResult _Create()
        {

            //lista de Codeudores
            List<SelectListItem> items3 = new List<SelectListItem>();   // Creo una lista
            items3.Add(new SelectListItem { Text = "Seleccione Garantia", Value = "" });
            IList<Tercero> lista4 = db.Terceros.ToList();// extraigo los elementos desde la DB

            var test = from s in lista4 where s.ESCODEUDOR == true select s;



            foreach (var item in test)		// recorro los elementos de la db
            {
                items3.Add(new SelectListItem { Text = item.NOMBRE + " | " + item.ESCODEUDOR, Value = item.NIT.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.NITS = items3;


            return View();
        }



        // POST: /Personal/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NIT,PagarePer")] Personal personal)
        {
            if (ModelState.IsValid)
            {
                db.Personal.Add(personal);
                db.SaveChanges();
                return RedirectToAction("Index", "Lineas");
            }

            return View(personal);
        }


        // GET: /Personal/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personal personal = db.Personal.Find(id);
            if (personal == null)
            {
                return HttpNotFound();
            }
            return View(personal);
        }

        // POST: /Personal/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Personal_Id,NIT,Prestamos_id")] Personal personal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(personal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(personal);
        }

        // GET: /Personal/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Personal personal = db.Personal.Find(id);
            if (personal == null)
            {
                return HttpNotFound();
            }
            return View(personal);
        }

        // POST: /Personal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Personal personal = db.Personal.Find(id);
            db.Personal.Remove(personal);
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
    }
}
