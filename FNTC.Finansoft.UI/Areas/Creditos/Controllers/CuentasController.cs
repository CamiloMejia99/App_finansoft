using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.UI.Areas.Terceros.Controllers;
using FNTC.Framework.Params;
using FNTC.Framework.Paras.DAL;

namespace FNTC.Finansoft.UI.Areas.Creditos.Controllers
{
    [Authorize]//autorizacion en login
    public class CuentasController : Controller
    {

        private AccountingContext db = new AccountingContext();
        private ParametersModel parm = new ParametersModel();

        // GET: Creditos/Cuentas
        public ActionResult Index()
        {
            var cuentas = db.Cuentas.ToList();
            return View(cuentas);
        }

        // GET: Creditos/Cuentas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuentas cuentas = db.Cuentas.Find(id);
            if (cuentas == null)
            {
                return HttpNotFound();
            }
            return View(cuentas);
        }

        // GET: Creditos/Cuentas/Create
        public ActionResult Create()
        {
            ViewData["funcionCuenta"] = new SelectList(parm.Parametros.Where(p => p.NombreParametro == "FUNCREDITOS"), "Codigo", "Valor");

            ViewData["tiposComprobantes"] = new SelectList(db.TiposComprobantes.Where(p => p.INACTIVO == false).Select(x=> new { CODIGO=x.CODIGO,NOMBRE=x.NOMBRE+" ("+x.CODIGO+")"}), "CODIGO", "NOMBRE");

            ViewData["lineas"] = new SelectList(db.Lineas, "Lineas_Id", "Lineas_Descripcion");;

            return View();
        }

        
        // POST: Creditos/Cuentas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Cuenta_Cod,Cuenta_Descripcion,Funcion,TipoComprobante,Destino_Id")] Cuentas cuentas)
        {
            try
            {
                ViewBag.funcionCuenta = new SelectList((from a in parm.Parametros
                                                        where a.NombreParametro == "FUNCREDITOS"
                                                        select new { value = a.Codigo, text = a.Valor }).ToList(), "value", "text");


                ViewBag.tiposComprobantes = new SelectList((from a in db.TiposComprobantes
                                                            where a.INACTIVO == false
                                                            select new { value = a.CODIGO, text = a.NOMBRE+" ("+a.CODIGO+")" }).ToList(), "value", "text");

                ViewData["lineas"] = new SelectList(db.Lineas, "Lineas_Id", "Lineas_Descripcion");

  
                var verificacion = db.Cuentas.Where(x => x.Cuenta_Cod == cuentas.Cuenta_Cod && x.Destino_Id == cuentas.Destino_Id && x.Funcion == cuentas.Funcion).Any();

                if (!verificacion)
                {
                    if (ModelState.IsValid)
                    {
                        cuentas.NombreFuncion = (from pc in parm.Parametros where pc.Codigo == cuentas.Funcion select pc.Valor).Single();
                        db.Cuentas.Add(cuentas);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["error"] = "Ha ocurrido un error al agregar la cuenta";
                        return View(cuentas);
                    }
                }
                else
                {
                    TempData["error"] = "La cuenta ya se encuentra registrada";
                    return View(cuentas);
                }
               
            }
            catch (Exception ex)
            {
                TempData["error"] = "Ha ocurrido un error al agregar la cuenta";
                return View(cuentas);
            }
            
        }

        // GET: Creditos/Cuentas/Edit/5
        public ActionResult Edit(int? id)
        {
            var cuentaActual = (from cu in db.Cuentas where cu.id == id select cu).First();
            var tipoComprobanteActual = (from tc in db.TiposComprobantes where tc.CODIGO == cuentaActual.TipoComprobante select tc).First();
            var linea = db.Destinos.Find(cuentaActual.Destino_Id);

            ViewData["funcionCuenta"] = new SelectList(parm.Parametros.Where(p => p.NombreParametro == "FUNCREDITOS"), "Codigo", "Valor", cuentaActual.Funcion);
            ViewData["tiposComprobantes"] = new SelectList(db.TiposComprobantes.Where(p => p.INACTIVO == false).Select(x => new { CODIGO = x.CODIGO, NOMBRE = x.NOMBRE + " (" + x.CODIGO + ")" }), "CODIGO", "NOMBRE");
            ViewData["lineas"] = new SelectList(db.Lineas, "Lineas_Id", "Lineas_Descripcion",linea.Lineas_Id);


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuentas cuentas = db.Cuentas.Find(id);
            if (cuentas == null)
            {
                return HttpNotFound();
            }
            return View(cuentas);
        }

        // POST: Creditos/Cuentas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Cuenta_Cod,Cuenta_Descripcion,Funcion,TipoComprobante,Destino_Id")] Cuentas cuentas)
        {
            try
            {
                ViewData["funcionCuenta"] = new SelectList(parm.Parametros.Where(p => p.NombreParametro == "FUNCREDITOS"), "Codigo", "Valor");

                ViewData["tiposComprobantes"] = new SelectList(db.TiposComprobantes.Where(p => p.INACTIVO == false).Select(x => new { CODIGO = x.CODIGO, NOMBRE = x.NOMBRE + " (" + x.CODIGO + ")" }), "CODIGO", "NOMBRE");

                ViewData["lineas"] = new SelectList(db.Lineas, "Lineas_Id", "Lineas_Descripcion");

                var verificacionAux = db.Cuentas.Where(x => x.Cuenta_Cod == cuentas.Cuenta_Cod && x.Destino_Id == cuentas.Destino_Id && x.id != cuentas.id).Any();

                if (!verificacionAux)
                {
                    if (ModelState.IsValid)
                    {
                        cuentas.NombreFuncion = (from pc in parm.Parametros where pc.Codigo == cuentas.Funcion select pc.Valor).Single();
                        db.Entry(cuentas).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["error"] = "Ha ocurrido un error en la actualización";
                        return View(cuentas);
                    }
                }
                else
                {
                    TempData["error"] = "La cuenta ya se encuentra registrada";
                    return View(cuentas);
                }
                
                
            }
            catch (Exception ex)
            {
                TempData["error"] = "Ha ocurrido un error en la actualización";
                return View(cuentas);
            }
            
        }

        // GET: Creditos/Cuentas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cuentas cuentas = db.Cuentas.Find(id);
            if (cuentas == null)
            {
                return HttpNotFound();
            }
            return View(cuentas);
        }

        // POST: Creditos/Cuentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cuentas cuentas = db.Cuentas.Find(id);
            db.Cuentas.Remove(cuentas);
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


        public JsonResult ValidacionFuncion(string func)
        {
            //  var query=(from s in db.Prestamos orderby s.id descending select s)
            var query = (from s in db.Cuentas where s.Funcion == func select s.Funcion).Count();

            if (query == 0 || func=="F8")
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}
