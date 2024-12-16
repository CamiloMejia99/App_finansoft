using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO.FabricaCreditos;
using FNTC.Finansoft.Accounting.DTO;
using System.Data.Entity;
using FNTC.Finansoft.Accounting.BLL.FabricaCreditosBll;


namespace FNTC.Finansoft.UI.Areas.FabricaCreditos.Controllers
{
    public class CrearConfiguracionController : Controller

    {
        private AccountingContext db = new AccountingContext();

        // GET: FabricaCreditos/FabricaCreditos
        public ActionResult Vista()
        {

            return View(db.FCConfiguracion.ToList());
        }
        public ActionResult ListaSedes()
        {
            using (var db = new AccountingContext())
            {
                return PartialView(db.FCSedes.ToList());
            }
        }
        public ActionResult ListaRiesgos()
        {
            using (var db = new AccountingContext())
            {
                return PartialView(db.CentralRiesgo.ToList());
            }
        }
        // GET: FabricaCreditos/CrearConfiguracion
        public ActionResult Crear()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult crear(FCConfiguracion a)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (AccountingContext db = new AccountingContext())
                {

                    db.FCConfiguracion.Add(a);
                    db.SaveChanges();
                    return RedirectToAction("Vista");

                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al Agregar ");
                return View();
            }
        }
        public ActionResult Editar(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    FCConfiguracion Conf = db.FCConfiguracion.Where(a => a.idConfiguracion == id).FirstOrDefault();
                    return View(Conf);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(FCConfiguracion a)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    FCConfiguracion Con = db.FCConfiguracion.Find(a.idConfiguracion);
                    Con.tiempoRespuestaSolMin = a.tiempoRespuestaSolMin;
                    Con.tiempoRespuestaSolMax = a.tiempoRespuestaSolMax;
                    Con.tiempoMaxOtorgarCredito = a.tiempoMaxOtorgarCredito;
                    Con.edadMinCredito = a.edadMinCredito;
                    Con.edadMaxCredito = a.edadMaxCredito;

                    Con.activa = a.activa;
                    db.SaveChanges();
                    return RedirectToAction("Vista");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}