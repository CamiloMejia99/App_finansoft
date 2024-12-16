using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.SARLAFT;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.SIAR.Controllers
{
    public class SarlaftController : Controller
    {
        AccountingContext db = new AccountingContext();
        // GET: SIAR/Sarlaft
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewMatrizCalor()
        {
            return View();
        }


        #region CONTEXTO EMPRESARIAL

        public ActionResult ViewContextoEmpresarial()
        {
            var data = db.ContextoEmpresarial.ToList();

            return View(data);
        }

        

        [HttpPost]
        public JsonResult VerificaContextoEmpresa()
        {

            var data = db.ContextoEmpresarial.FirstOrDefault();
            if (data!=null)
            {
                return new JsonResult { Data = new { status = true } };

            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }

        }

        public ActionResult CreateContextoEmpresa()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateContextoEmpresa([System.Web.Http.FromBody] ContextoEmpresarial ContextoEmpresarial)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new AccountingContext())
                {
                    try
                    {
                        ctx.ContextoEmpresarial.Add(ContextoEmpresarial);
                        ctx.SaveChanges();
                        return RedirectToAction("ViewContextoEmpresarial");
                    }
                    catch (DbEntityValidationException dbE)
                    {

                    }
                }
            }

            return View(ContextoEmpresarial);
        }


        public ActionResult EditContextoEmpresa(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.ContextoEmpresarial.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditContextoEmpresa([System.Web.Http.FromBody] ContextoEmpresarial ContextoEmpresarial)
        {

            if (ModelState.IsValid)
            {
                db.Entry(ContextoEmpresarial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewContextoEmpresarial");
            }
            
            return View(ContextoEmpresarial);

        }

        [HttpPost]
        public JsonResult DetailsContexto(int id)
        {

            var datos = db.ContextoEmpresarial.Find(id);
            if(datos!=null)
            {

                return new JsonResult { Data = new { status = true,datos } };
            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }

            

        }

        #endregion

        #region Macroprocesos
        public ActionResult ViewMacroprocesos()
        {
            var data = db.Macroprocesos.ToList();

            return View(data);
        }

        public ActionResult CreateMacroproceso()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMacroproceso([System.Web.Http.FromBody] Macroprocesos Macroprocesos)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new AccountingContext())
                {
                    try
                    {
                        Macroprocesos.estado = true;
                        ctx.Macroprocesos.Add(Macroprocesos);
                        ctx.SaveChanges();
                        return RedirectToAction("ViewMacroprocesos");
                    }
                    catch (DbEntityValidationException dbE)
                    {

                    }
                }
            }

            return View(Macroprocesos);
        }

        public ActionResult EditMacroproceso(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var data = db.Macroprocesos.Find(id);
            if (data == null)
            {
                return HttpNotFound();
            }

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMacroproceso([System.Web.Http.FromBody] Macroprocesos Macroprocesos)
        {

            if (ModelState.IsValid)
            {
                db.Entry(Macroprocesos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewMacroprocesos");
            }

            return View(Macroprocesos);

        }

        [HttpPost]
        public JsonResult DetailsMacroproceso(int id)
        {

            var datos = db.Macroprocesos.Find(id);
            if (datos != null)
            {

                return new JsonResult { Data = new { status = true, datos } };
            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }

        }

        [HttpPost]
        public JsonResult EstablecerEstado(int id,int value)
        {

            var data = db.Macroprocesos.Find(id);
            if (data != null)
            {
                if(value == 1)
                {
                    data.estado = false;
                }
                else
                {
                    data.estado = true;
                }
                db.Entry(data).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return new JsonResult { Data = new { status = true } };
            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }

        }

        public JsonResult verificarCodigo(string codigo) //MÉTODO QUE SIRVE PARA QUE NO SE REPITA EL CAMPO CODIGO
        {
            return Json(!db.Macroprocesos.Any(lo => lo.codigo == codigo), JsonRequestBehavior.AllowGet);
        }

        #endregion



    }
}