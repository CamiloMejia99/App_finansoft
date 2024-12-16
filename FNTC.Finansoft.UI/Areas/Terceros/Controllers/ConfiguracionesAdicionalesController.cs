using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.BLL.FabricaCreditosBll;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using FNTC.Finansoft.Accounting.DTO.MCreditos;

namespace FNTC.Finansoft.UI.Areas.Terceros.Controllers
{
    public class ConfiguracionesAdicionalesController : Controller

    {
        private AccountingContext db = new AccountingContext();
        // GET: Terceros/ConfiguracionesAdicionales
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Configuracion()
        {
            return View(db.CargoEmpresaTer.ToList());
        }
        public ActionResult CrearCargo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearCargo(CargoEmpresaTer cargoEmpresaTer)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (AccountingContext db = new AccountingContext())
                {

                    db.CargoEmpresaTer.Add(cargoEmpresaTer);
                    db.SaveChanges();
                    return RedirectToAction("Configuracion");

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
                    CargoEmpresaTer Configuracion = db.CargoEmpresaTer.Where(a => a.IDCargo == id).FirstOrDefault();
                    return View(Configuracion);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(CargoEmpresaTer cargoEmpresaTer)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    CargoEmpresaTer cargoEmpresa = db.CargoEmpresaTer.Find(cargoEmpresaTer.IDCargo);
                    cargoEmpresa.NombreCargo = cargoEmpresaTer.NombreCargo;
                    cargoEmpresa.EstadoCargo = cargoEmpresaTer.EstadoCargo;
                    db.SaveChanges();
                    return RedirectToAction("Configuracion");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
