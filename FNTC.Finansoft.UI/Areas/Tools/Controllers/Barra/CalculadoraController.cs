﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Tools.Controllers
{
    public class CalculadoraController : Controller
    {
        // GET: Tools/Calculadora
        public ActionResult Index()
        {
            return View("Calculadora");
        }

        // GET: Tools/Calculadora/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Tools/Calculadora/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tools/Calculadora/Create
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

        // GET: Tools/Calculadora/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Tools/Calculadora/Edit/5
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

        // GET: Tools/Calculadora/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Tools/Calculadora/Delete/5
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

        public ActionResult Calculadora()
        {
            return View();
        }

        public ActionResult calculadoramodal()
        {
            return View("calculadoramodal");
        }


    }
}
