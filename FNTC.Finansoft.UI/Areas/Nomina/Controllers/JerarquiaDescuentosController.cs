using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using FNTC.Finansoft.Accounting.DTO.Nomina;
using FNTC.Finansoft.Accounting.DAL.Nomina;
using FNTC.Finansoft.Accounting.DTO;

namespace FNTC.Finansoft.UI.Areas.Nomina.Controllers
{
    public class JerarquiaDescuentosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        public ActionResult _list()//(string nombre)
        {
            //return PartialView(db.JerarquiaDescuento.Where(p=>p.NOMBRE.Contains(nombre)).ToList());
            //return PartialView(db.JerarquiaDescuento.OrderBy(p => p.ORDEN).ToList());
            return PartialView(db.JerarquiaDescuento.Include(p => p.Cuenta).OrderBy(p => p.ORDEN).ToList());
            //return PartialView(db.SeleccionCuenta.Include(p => p.Cuenta).OrderBy(p => p.CODIGO).ToList());
        }
        // GET: Nomina/JerarquiaDescuentoes
        public ActionResult Index()
        {
            //return View(db.JerarquiaDescuento.OrderBy(p => p.ORDEN).ToList());
            return View(db.JerarquiaDescuento.Include(p => p.Cuenta).OrderBy(p => p.ORDEN).ToList());
        }

        // GET: Nomina/JerarquiaDescuentoes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JerarquiaDescuento jerarquiaDescuento = db.JerarquiaDescuento.Find(id);
            if (jerarquiaDescuento == null)
            {
                return HttpNotFound();
            }
            return View(jerarquiaDescuento);
        }

        // GET: Nomina/JerarquiaDescuentoes/Create
        public ActionResult Create()
        {
            ViewBag.guardado = "N";
            return View();
        }

        // POST: Nomina/JerarquiaDescuentoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JerarquiaDescuento jerarquiaDescuento)
        {
            string Error = "";
            if (ModelState.IsValid)
            {
                JerarquiaDescuentosDAL ctx = new JerarquiaDescuentosDAL();
                try
                {
                    ctx.CreateJerarquiaDescuento(jerarquiaDescuento);
                    ViewBag.guardado = "S";
                    return PartialView();
                }
                catch (Exception e)
                {
                    ViewBag.Error = e.Message;

                }
            }

            return PartialView("Create");
        }

        [HttpPost]
        public ActionResult ValidacionCuenta(string Cuenta)
        {
            //  var query=(from s in db.Prestamos orderby s.id descending select s)
            using (var ctx = new AccountingContext())
            {
                if ((from s in ctx.JerarquiaDescuento where s.CODIGO == Cuenta select s).Count() != 0)
                {

                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
        }

        // GET: Nomina/JerarquiaDescuentoes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JerarquiaDescuento jerarquiaDescuento = db.JerarquiaDescuento.Find(id);
            if (jerarquiaDescuento == null)
            {
                return HttpNotFound();
            }
            //ViewBag.action = "Delete";
            //ViewBag.boton = "Eliminar";
            // ViewBag.guardado = "N";
            return PartialView(jerarquiaDescuento);

        }

        // POST: Nomina/JerarquiaDescuentoes/Delete/5
        [HttpPost]

        public ActionResult DeleteConfirmed(string id)
        {
            JerarquiaDescuento jerarquiaDescuento = db.JerarquiaDescuento.Find(id);
            db.JerarquiaDescuento.Remove(jerarquiaDescuento);

            db.SaveChanges();

            List<JerarquiaDescuento> lista = db.JerarquiaDescuento.Where(p => p.ORDEN > jerarquiaDescuento.ORDEN).OrderBy(p => p.ORDEN).ToList();
            foreach (JerarquiaDescuento obj in lista)
            {
                var orden = obj.ORDEN - 1;
                obj.ORDEN = (short)orden;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
            }

            //return PartialView();
            ViewBag.guardado = "S";
            //return PartialView(jerarquiaDescuento);
            //no se sabe porque tiene que regresar al delete pero asi funciona.
            return PartialView("Delete");
        }

        public ActionResult Move(string id, string action_)
        {///1ro se instancia el q se movió
            JerarquiaDescuento jerarquiaDescuento = db.JerarquiaDescuento.Find(id);
            var ordenM = jerarquiaDescuento.ORDEN;// se guarda el  orden para intercambiarlo con el registro q se va a reemplazar
            // 2do se verifica el nuevo orden
            var orden = 0;
            var max = db.JerarquiaDescuento.Max(p => p.ORDEN);
            if ((action_ == "Subir" && ordenM > 1) || (action_ == "Bajar" && ordenM < max))
            {
                if (action_ == "Subir")
                    orden = jerarquiaDescuento.ORDEN - 1;
                else
                    orden = jerarquiaDescuento.ORDEN + 1;
                // 3ro se trae el registro q esta en esa posicion 
                JerarquiaDescuento otroRegistro = db.JerarquiaDescuento.Where(p => p.ORDEN == orden).FirstOrDefault();


                // actualizar registros

                jerarquiaDescuento.ORDEN = (short)orden;
                db.Entry(jerarquiaDescuento).State = EntityState.Modified;
                db.SaveChanges();

                otroRegistro.ORDEN = (short)ordenM;
                db.Entry(jerarquiaDescuento).State = EntityState.Modified;
                db.SaveChanges();
            }


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
