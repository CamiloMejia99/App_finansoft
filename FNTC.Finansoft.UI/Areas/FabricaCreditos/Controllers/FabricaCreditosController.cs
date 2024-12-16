using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.BLL.FabricaCreditosBll;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.FabricaCreditos;

namespace FNTC.Finansoft.UI.Areas.FabricaCreditos.Controllers
{
    public class FabricaCreditosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: FabricaCreditos/FabricaCreditos
        public ActionResult Index()
        {
            ViewBag.Agencias = new ConfiguracionBll().obtenerAgencias();
            ViewBag.Sedes = new ConfiguracionBll().obtenerSedes();
            ViewBag.CentralesRiesgo = new ConfiguracionBll().obtenerCentralesRiesgo();
            return View(db.FCConfiguracion.ToList());
        }

        // GET: FabricaCreditos/FabricaCreditos/Create
        public ActionResult Create()
        {
            ViewBag.Agencias = new ConfiguracionBll().obtenerAgencias();
            ViewBag.Sedes = new ConfiguracionBll().obtenerSedes();
            ViewBag.CentralesRiesgo = new ConfiguracionBll().obtenerCentralesRiesgo();
            return View();
        }

        // POST: FabricaCreditos/FabricaCreditos/Create
        [HttpPost]
        public ActionResult Create([System.Web.Http.FromBody] FCConfiguracion FCConfiguracion)
        {
            if (ModelState.IsValid)
            {
                db.FCConfiguracion.Add(FCConfiguracion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(FCConfiguracion);
        }

        // GET: FabricaCreditos/FabricaCreditos/Edit/5
        public ActionResult Edit(int id)
        {
            ViewBag.Agencias = new ConfiguracionBll().obtenerAgencias();
            ViewBag.Sedes = new ConfiguracionBll().obtenerSedes();
            ViewBag.CentralesRiesgo = new ConfiguracionBll().obtenerCentralesRiesgo();
            FCConfiguracion ep = db.FCConfiguracion.Find(id);
            return View(ep);
        }

        // POST: FabricaCreditos/FabricaCreditos/Edit/5
        [HttpPost]
        public ActionResult Edit([System.Web.Http.FromBody] FCConfiguracion FCConfiguracion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(FCConfiguracion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(FCConfiguracion);
        }

    }
}
