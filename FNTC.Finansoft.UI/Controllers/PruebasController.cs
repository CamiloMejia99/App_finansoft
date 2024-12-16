using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.UI.Controllers
{
    public class PruebaValidacion
    {

        public int id { get; set; }

        [Required]
        public string Nombre { get; set; }
    }

    public class PruebasController : Controller
    {


        // GET: Pruebas
        public ActionResult Validate()
        {
            return View();
        }

        // GET: Pruebas/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Pruebas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pruebas/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Pruebas/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Pruebas/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Pruebas/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Pruebas/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Pruebas()
        {
            return View();
        }


        public ActionResult Pruebas2()
        {
            return View();
        }
    }
}
