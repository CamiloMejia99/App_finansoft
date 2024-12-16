using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.Accounting.DTO.FormularioVinculacion;
using FNTC.Finansoft.Accounting.DTO;

namespace FNTC.Finansoft.UI.Areas.Creditos.Controllers
{
    public class CConsecutivosController : Controller
    {
        private AccountingContext db = new AccountingContext();
        // GET: Creditos/CConsecutivos
        public ActionResult Index()
        {
            return View(db.CConsecutivos.ToList());
        }

        // GET: Creditos/CConsecutivos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CConsecutivos cConsecutivos = db.CConsecutivos.Find(id);
            if (cConsecutivos == null)
            {
                return HttpNotFound();
            }
            return View(cConsecutivos);
        }

        // GET: Creditos/CConsecutivos/Create
        public ActionResult Create()
        {           
            ViewBag.idAgencia = new SelectList((from l in db.agencias select l), "codigoagencia", "nombreagencia");
            ViewBag.idLinea = new SelectList((from l in db.Lineas select l), "Lineas_Id", "Lineas_Descripcion");
            //ViewBag.idDestino = new SelectList((from l in dbf.Destinos select l), "Destino_Id", "Destino_Descripcion");
            return View();
        }

        public JsonResult GetPagare(int paramIdDestino)
        {
            var midestino = db.Destinos.FirstOrDefault(x => x.Destino_Id == paramIdDestino);
            var esautomatico = midestino.Destino_Pagare_Automatico;
            var tminima = midestino.Destino_Tasa_Minima;
            var interesminimo = midestino.Destino_Causal_Interes_Anticipado;
            var esaut = 0;
            var intinteresminimo = 0;
            if (interesminimo == true)
            {
                intinteresminimo = 1;
            }
            else
            {
                intinteresminimo = 0;
            }
            if (esautomatico==true)
            {
                esaut = 1;
            }
            else
            {
                esaut = 0;
            }
            var retorno = new ViewModelGetPagare();
            var consecutivo = db.CConsecutivos.FirstOrDefault(x => x.idDestino == paramIdDestino & x.estado == true);
            if(consecutivo != null)
            {
                var cod = consecutivo.tipoCodPagare;
                var actual = consecutivo.consecutivoPagareActual;
                var prestamo = consecutivo.prestamo;
                
                retorno.esaut = esaut;
                retorno.cod = cod;
                retorno.actual = actual;
                retorno.tminima = tminima;
                retorno.interesminimo = intinteresminimo;
                retorno.prestamo = prestamo;

                consecutivo.prestamo = consecutivo.prestamo + 1;
                db.SaveChanges();
            }
            else
            {
                var cod = "Pagare no Configurado";
                var actual = 1;
                var prestamo = 1;

                retorno.esaut = esaut;
                retorno.cod = cod;
                retorno.actual = actual;
                retorno.tminima = tminima;
                retorno.prestamo = prestamo;
            }
            
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        // POST: Creditos/CConsecutivos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,idAgencia,tipoCodPagare,consecutivoPagareActual,tipoCodLibranza,consecutivoLibranzaActual,idLinea,idDestino,estado")] CConsecutivos cConsecutivos)
        {
            if (ModelState.IsValid)
            {
                db.CConsecutivos.Add(cConsecutivos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.idAgencia = new SelectList(db.agencias, "codigoagencia", "nombreagencia", cConsecutivos.idAgencia);
            ViewBag.idLinea = new SelectList(db.Lineas, "Lineas_Id", "Lineas_Descripcion", cConsecutivos.idLinea);
            ViewBag.idDestino = new SelectList(db.Destinos, "Destino_Id", "Destino_Descripcion", cConsecutivos.idDestino);
            return View(cConsecutivos);
        }

        // GET: Creditos/CConsecutivos/Edit/5
        public ActionResult Edit(int? id)
        {        
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CConsecutivos cConsecutivos = db.CConsecutivos.Find(id);

            ViewBag.idAgencia = new SelectList((from l in db.agencias select l), "codigoagencia", "nombreagencia", cConsecutivos.idAgencia);
            ViewBag.idLinea = new SelectList((from l in db.Lineas select l), "Lineas_Id", "Lineas_Descripcion", cConsecutivos.idLinea);
            ViewBag.idDestino = new SelectList((from l in db.Destinos select l), "Destino_Id", "Destino_Descripcion", cConsecutivos.idDestino);

            if (cConsecutivos == null)
            {
                return HttpNotFound();
            }
            
            return View(cConsecutivos);
        }

        //Validacion Remota Destino Pagare
        public JsonResult ValidacionDestinoPagare(int idDestino)
        {
            //  var query=(from s in db.Prestamos orderby s.id descending select s)
            var query = (from s in db.CConsecutivos where s.idDestino == idDestino select s.idDestino).Count();

            if (query == 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json("Ya se ha asignado un pagaré a este destino", JsonRequestBehavior.AllowGet);
        }

        // POST: Creditos/CConsecutivos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,idAgencia,tipoCodPagare,consecutivoPagareActual,tipoCodLibranza,consecutivoLibranzaActual,idLinea,idDestino,estado")] CConsecutivos cConsecutivos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cConsecutivos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cConsecutivos);
        }

        // GET: Creditos/CConsecutivos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CConsecutivos cConsecutivos = db.CConsecutivos.Find(id);
            if (cConsecutivos == null)
            {
                return HttpNotFound();
            }
            return View(cConsecutivos);
        }

        // POST: Creditos/CConsecutivos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CConsecutivos cConsecutivos = db.CConsecutivos.Find(id);
            db.CConsecutivos.Remove(cConsecutivos);
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
