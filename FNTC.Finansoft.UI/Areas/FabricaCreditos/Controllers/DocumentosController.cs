using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.FabricaCreditos;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.FabricaCreditos.Controllers
{
    public class DocumentosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: FabricaCreditos/Documentos
        public ActionResult Index()
        {
            return View(db.FCDocumentos.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create([System.Web.Http.FromBody] FCDocumentos FCDocumentos)
        {
            if (ModelState.IsValid)
            {
                db.FCDocumentos.Add(FCDocumentos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(FCDocumentos);

        }

        public ActionResult Edit(int id)
        {
            FCDocumentos ep = db.FCDocumentos.Find(id);
            return View(ep);
        }

        [HttpPost]
        public ActionResult Edit([System.Web.Http.FromBody] FCDocumentos FCDocumentos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(FCDocumentos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(FCDocumentos);
        }
    }
}