using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using FNTC.Finansoft.Accounting.DTO.Geo;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using FNTC.Finansoft.Accounting.DTO.FormularioRetiro;
using FNTC.Finansoft.Accounting.DTO.Fichas;
using FNTC.Finansoft.Accounting.DTO;

namespace FNTC.Finansoft.UI.Areas.FormularioRetiro.Controllers
{
    public class FormularioRetiroController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: FormularioRetiro/FormularioRetiro
        public ActionResult Index()
        {
            return View(db.BFormularioRetiro.ToList());
        }

        // GET: FormularioRetiro/FormularioRetiro/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BFormularioRetiro bFormularioRetiro = db.BFormularioRetiro.Find(id);
            if (bFormularioRetiro == null)
            {
                return HttpNotFound();
            }
            return View(bFormularioRetiro);
        }

        // GET: FormularioRetiro/FormularioRetiro/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FormularioRetiro/FormularioRetiro/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,fecha_solicitud,motivoRetiro1,motivoRetiro2,motivoRetiro3,motivoRetiro4,motivoRetiro5,nombre,nit,telefono,celular,ciudad,correo")] BFormularioRetiro bFormularioRetiro)
        {
            if (db.BFormularioRetiro.Any(a => a.nit == bFormularioRetiro.nit))
            {
                ModelState.AddModelError("nit", "El asociado ya se encuentra retirado");

            } else
            {
                if (ModelState.IsValid)
                {
                    db.BFormularioRetiro.Add(bFormularioRetiro);

                    string ced = bFormularioRetiro.nit;
                    var updateactivo = "UPDATE apo.FichasAportes SET activa=0 WHERE idPersona='" + ced + "'";
                    db.Database.ExecuteSqlCommand(updateactivo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(bFormularioRetiro);
        }

        // GET: FormularioRetiro/FormularioRetiro/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BFormularioRetiro bFormularioRetiro = db.BFormularioRetiro.Find(id);
            if (bFormularioRetiro == null)
            {
                return HttpNotFound();
            }
            return View(bFormularioRetiro);
        }

        // POST: FormularioRetiro/FormularioRetiro/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,fecha_solicitud,motivoRetiro1,motivoRetiro2,motivoRetiro3,motivoRetiro4,motivoRetiro5,nombre,nit,telefono,celular,ciudad,correo")] BFormularioRetiro bFormularioRetiro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bFormularioRetiro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bFormularioRetiro);
        }

        //[HttpPost]
        //public JsonResult GetTerceroInfo(string terceroId)
        //{
        //    int idaportes = db.Database.SqlQuery<int>("SELECT id FROM apo.FichasAportes WHERE idPersona='" + terceroId + "'").FirstOrDefault();

        //    Tercero terceros = db.Terceros.Find(terceroId);
        //    Municipio municipiored = db.Municipio.Find(terceros.MUNICIPIO);
        //    var response2 = new List<object>
        //    {
        //        new{
        //                nombre =terceros.NOMBRE1+" "+terceros.NOMBRE2+" "+terceros.APELLIDO1+" "+terceros.APELLIDO2,
        //                telefono=terceros.TEL,
        //                celular=terceros.TELMOVIL,
        //                correo=terceros.EMAIL,
        //                municipio=municipiored.Nom_muni
        //        }
        //    };
        //    return Json(response2);
        //}


        [HttpPost]
        public JsonResult GetTerceroInfo(string nit)
        {
            //int idd = Convert.ToInt32(id);
            var tercero = db.Terceros.Where(x => x.NIT == nit).FirstOrDefault();
            if (tercero == null)
            {
                return new JsonResult { Data = new { status = false } };
            }
            else
            {
                string nombre = tercero.NOMBRE1+" "+tercero.NOMBRE2+" "+tercero.APELLIDO1+" "+tercero.APELLIDO2;
                string telefono = tercero.TEL;
                string celular = tercero.TELMOVIL;
                string correo = tercero.EMAIL;
                string ciudad = tercero.municipioFK.Nom_muni;
                
                return new JsonResult { Data = new { status = true, nombre,telefono,celular,correo,ciudad } };
            }

        }

        //fecha actual
        public JsonResult GetFechaActual()
        {
            return Json(DateTime.Now, JsonRequestBehavior.AllowGet);
        }

        // GET: FormularioRetiro/FormularioRetiro/Delete/5
        [HttpPost]
        public JsonResult Delete(int id)
        {
            //int idd = Convert.ToInt32(id);
            var bFormularioRetiro = db.BFormularioRetiro.Find(id);

            if(bFormularioRetiro != null )
            {
                var liquidaDefinitiva = db.LiquidacionDefinitiva.Where(a => a.NIT == bFormularioRetiro.nit).FirstOrDefault();
                if(liquidaDefinitiva != null)
                {
                    return new JsonResult { Data = new { status = false } };
                }
                else
                {
                    var consulta = db.BFormularioRetiro.Find(id);
                    db.Entry(consulta).State = System.Data.Entity.EntityState.Deleted;

                    var consulta2 = db.FichasAportes.Where(b => b.idPersona == bFormularioRetiro.nit).FirstOrDefault();
                    consulta2.activa = true;
                    db.Entry(consulta2).State = System.Data.Entity.EntityState.Modified;


                    db.SaveChanges();

                    return new JsonResult { Data = new { status = true } };
                }
            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }

            
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
