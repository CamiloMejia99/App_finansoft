using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DAL;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Terceros;

namespace FNTC.Finansoft.UI.Areas.Terceros.Controllers
{
    [Authorize]
    public class TercerosAdicionalesController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Terceros/TercerosAdicionales
        public ActionResult Index()
        {
            var infoTerceroAdicional = db.InfoTerceroAdicional.Include(i => i.Contrato).Include(i => i.estrato).Include(i => i.NivelEstudio).Include(i => i.Tercero);
            return View(infoTerceroAdicional.ToList());
        }

        // GET: Terceros/TercerosAdicionales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InfoTerceroAdicional infoTerceroAdicional = db.InfoTerceroAdicional.Find(id);
            if (infoTerceroAdicional == null)
            {
                return HttpNotFound();
            }
            return View(infoTerceroAdicional);
        }

        // GET: Terceros/TercerosAdicionales/Create
        public ActionResult Create()
        {
            ViewBag.IdContrato = new SelectList(db.Contrato, "Id", "TipoContrato");
            ViewBag.IdEstrato = new SelectList(db.estrato, "Id", "Estrato");
            ViewBag.IdNivelEstudio = new SelectList(db.NivelEstudio, "Id", "Nestudio");

            ViewBag.NitTercero = new TercerosController().ConsultarTerceros().ToList().Select(p => new SelectListItem { Text = p.NIT + " || " + p.NOMBRE + " " + p.APELLIDO1 + " " + p.APELLIDO2, Value = p.NIT, Selected = false }); ;

            return View();
        }

        // POST: Terceros/TercerosAdicionales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include= "Id, NitTercero, IdEstrato, IdContrato, IdNivelEstudio, PersonasCargo, Ocupacion,Fechalaboral, Detalle")] InfoTerceroAdicional infoTerceroAdicional)
        {
            if (ModelState.IsValid)
            {
                db.InfoTerceroAdicional.Add(infoTerceroAdicional);
               db.SaveChanges();
                return RedirectToAction("Index");
            }
              
            ViewBag.IdContrato = new SelectList(db.Contrato, "Id", "TipoContrato", infoTerceroAdicional.IdContrato);
            ViewBag.IdEstrato = new SelectList(db.estrato, "Id", "Estrato", infoTerceroAdicional.IdEstrato);
            ViewBag.IdNivelEstudio = new SelectList(db.NivelEstudio, "Id", "Nestudio", infoTerceroAdicional.IdNivelEstudio);

            ViewBag.NitTercero = new TercerosController().ConsultarTerceros().ToList().Select(p => new SelectListItem { Text = p.NIT + " || " + p.NOMBRE + " " + p.APELLIDO1 + " " + p.APELLIDO2, Value = p.NIT, Selected = false }); ;





            return View(infoTerceroAdicional);
        }

        // GET: Terceros/TercerosAdicionales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InfoTerceroAdicional infoTerceroAdicional = db.InfoTerceroAdicional.Find(id);
            if (infoTerceroAdicional == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdContrato = new SelectList(db.Contrato, "Id", "TipoContrato", infoTerceroAdicional.IdContrato);
            ViewBag.IdEstrato = new SelectList(db.estrato, "Id", "Estrato", infoTerceroAdicional.IdEstrato);
            ViewBag.IdNivelEstudio = new SelectList(db.NivelEstudio, "Id", "Nestudio", infoTerceroAdicional.IdNivelEstudio);
            ViewBag.NitTercero = new SelectList(db.Terceros, "NIT", "NIT", infoTerceroAdicional.NitTercero);
            return View(infoTerceroAdicional);
        }

        // POST: Terceros/TercerosAdicionales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,NitTercero,IdEstrato,IdContrato,IdNivelEstudio,PersonasCargo,Ocupacion,Fechalaboral,Detalle")] InfoTerceroAdicional infoTerceroAdicional)
        {
            if (ModelState.IsValid)
            {
                db.Entry(infoTerceroAdicional).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdContrato = new SelectList(db.Contrato, "Id", "TipoContrato", infoTerceroAdicional.IdContrato);
            ViewBag.IdEstrato = new SelectList(db.estrato, "Id", "Estrato", infoTerceroAdicional.IdEstrato);
            ViewBag.IdNivelEstudio = new SelectList(db.NivelEstudio, "Id", "Nestudio", infoTerceroAdicional.IdNivelEstudio);
            ViewBag.NitTercero = new SelectList(db.Terceros, "NIT", "NIT", infoTerceroAdicional.NitTercero);
            return View(infoTerceroAdicional);
        }

        // GET: Terceros/TercerosAdicionales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InfoTerceroAdicional infoTerceroAdicional = db.InfoTerceroAdicional.Find(id);
            if (infoTerceroAdicional == null)
            {
                return HttpNotFound();
            }
            return View(infoTerceroAdicional);
        }

        // POST: Terceros/TercerosAdicionales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InfoTerceroAdicional infoTerceroAdicional = db.InfoTerceroAdicional.Find(id);
            db.InfoTerceroAdicional.Remove(infoTerceroAdicional);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult GetTerceroADByNIT(string nit = "")
        {

            if (nit.Length > 0)
            {

                var dto = GetTerceroAdicionalesByNIT(nit);
                if (dto != null)
                {
                   var asynhc= Json(dto, JsonRequestBehavior.AllowGet);
                    return Json(dto, JsonRequestBehavior.AllowGet);
                }

                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public Boolean HayDatos(string nit = "")
        {
            if (nit.Length > 0)
            {
                var dto = GetTerceroAdicionalesByNIT(nit);
                if (dto != null)
                {
                    return true;
                }
                 return false;
            }

            return false;
        }


        public InfoTerceroAdicional GetTerceroAdicionalesByNIT(string nit)
        {
            using (var ctx = new AccountingContext())
            {
                var terceroAD = ctx.InfoTerceroAdicional.Where(x=> x.NitTercero==nit).FirstOrDefault();
                return terceroAD;
            }
            
        }

        public ActionResult GetTercerosAdicionales(string term = "", string outputFormat = "datatables")
        {
            var terceros = db.InfoTerceroAdicional.ToList();
            var _tercerosDTO = terceros;
        
            switch (outputFormat)
            {
                case "datatables":
                    var dataTabledata = _tercerosDTO.Select((x, index)
                    => new[] {  x.NitTercero.ToString(), x.Tercero.NOMBRE1 + " " + x.Tercero.NOMBRE2 + " " + x.Tercero.APELLIDO1 + " " + x.Tercero.APELLIDO2,  x.NivelEstudio.Nestudio.ToString(), x.estrato.Estrato.ToString() ,x.Ocupacion.ToString(),  x.PersonasCargo.ToString(),  x.Contrato.TipoContrato.ToString(),  BotonEditar(x.Id.ToString()),  BotonEliminar(x.Id.ToString())});
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
            var botonEditar = @"<a href='/Terceros/TercerosAdicionales/Edit/" + id + "' class='btn btn-default fa fa-pencil edit'></a>";
            return botonEditar;
        }

        private string BotonEliminar(string id)
        {
            var botonEliminar = @"<a href='/Terceros/TercerosAdicionales/Delete/" + id + "' class='btn btn-danger glyphicon glyphicon-trash'></a>";
            return botonEliminar;
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
