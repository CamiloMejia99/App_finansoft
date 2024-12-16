using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.LiquidacionesDefinitivas;

namespace FNTC.Finansoft.UI.Areas.LiquidacionDefinitiva.Controllers
{
    public class LiquidacionDefinitivaController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: LiquidacionDefinitiva/LiquidacionDefinitiva
        public ActionResult Index()
        {
            return View(db.LiquidacionDefinitiva.ToList());
        }

        public JsonResult GetNombre(string NIT)
        {
            var tercero = (from pc in db.Terceros where pc.NIT == NIT select pc).Single();
            var nombre = tercero.NOMBRE1 + " " + tercero.NOMBRE2 + " " + tercero.APELLIDO1 + " " + tercero.APELLIDO2;
            return Json(nombre, JsonRequestBehavior.AllowGet);
            
        }

        // GET: LiquidacionDefinitiva/LiquidacionDefinitiva/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LiquidacionDefinitivaAso liquidacionDefinitiva = db.LiquidacionDefinitiva.Find(id);
            if (liquidacionDefinitiva == null)
            {
                return HttpNotFound();
            }
            return View(liquidacionDefinitiva);
        }

        //fecha actual
        public JsonResult GetFechaActual()
        {
            return Json(DateTime.Now, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTerceroInfo(string terceroId)
        {
            var countfichaAporte = (from pc in db.FichasAportes where pc.idPersona == terceroId select pc).Count();
            if(countfichaAporte > 0)
            {
                var fichaAporte = (from pc in db.FichasAportes where pc.idPersona == terceroId select pc).First();
                var tercero = (from pc in db.Terceros where pc.NIT == terceroId select pc).First();
                var usuario = "";
                if ((string)Session["usuariomilogin"] != null)
                {
                    usuario = Session["usuariomilogin"].ToString();
                }

                var response2 = new List<object>
                {
                    new{
                            nombre =tercero.NOMBRE1+" "+tercero.NOMBRE2+" "+tercero.APELLIDO1+" "+tercero.APELLIDO2,
                            agencia="Ipiales",
                            aportesSociales=fichaAporte.totalAportes,
                            creditos="0",
                            asesor=usuario
                    }
                };
                return Json(response2);
            }
            else
            {
                var response2 = new List<object>
                {
                    new{}
                };
                return Json(response2);
            }

        }

        // GET: LiquidacionDefinitiva/LiquidacionDefinitiva/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LiquidacionDefinitiva/LiquidacionDefinitiva/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fechaLiquidacion,NIT,agencia,asesor,totalAhorros,aportesSociales,totalCreditos,creditoConsumo,saldoAFavor")] LiquidacionDefinitivaAso liquidacionDefinitiva)
        {
            if (ModelState.IsValid)
            {
                db.LiquidacionDefinitiva.Add(liquidacionDefinitiva);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(liquidacionDefinitiva);
        }

        // GET: LiquidacionDefinitiva/LiquidacionDefinitiva/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LiquidacionDefinitivaAso liquidacionDefinitiva = db.LiquidacionDefinitiva.Find(id);
            if (liquidacionDefinitiva == null)
            {
                return HttpNotFound();
            }
            return View(liquidacionDefinitiva);
        }

        // POST: LiquidacionDefinitiva/LiquidacionDefinitiva/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fechaLiquidacion,NIT,agencia,asesor,totalAhorros,aportesSociales,totalCreditos,creditoConsumo,saldoAFavor")] LiquidacionDefinitivaAso liquidacionDefinitiva)
        {
            if (ModelState.IsValid)
            {
                db.Entry(liquidacionDefinitiva).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(liquidacionDefinitiva);
        }

        // GET: LiquidacionDefinitiva/LiquidacionDefinitiva/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LiquidacionDefinitivaAso liquidacionDefinitiva = db.LiquidacionDefinitiva.Find(id);
            if (liquidacionDefinitiva == null)
            {
                return HttpNotFound();
            }
            return View(liquidacionDefinitiva);
        }

        // POST: LiquidacionDefinitiva/LiquidacionDefinitiva/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LiquidacionDefinitivaAso liquidacionDefinitiva = db.LiquidacionDefinitiva.Find(id);
            db.LiquidacionDefinitiva.Remove(liquidacionDefinitiva);
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
