using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Terceros.Controllers
{
    public class TercerosFallecidosController : Controller
    {
        AccountingContext db = new AccountingContext();
        // GET: Terceros/TercerosFallecidos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {

            List<SelectListItem> terceros = new List<SelectListItem>();
            IList<Tercero> listTerceros = db.Terceros.ToList();


            terceros.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            foreach (var item in listTerceros)		// recorro los elementos de la db
            {
                terceros.Add(new SelectListItem { Text = item.NIT+" | "+ item.NOMBRE1+" "+item.NOMBRE2+" "+item.APELLIDO1+" "+item.APELLIDO2, Value = item.NIT });

            }
            //fin select list para rutas

            ViewBag.terceros = terceros;
            ViewBag.Post2 = ViewBag.action = "Create";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([System.Web.Http.FromBody] TercerosFallecidos TercerosFallecidos)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new AccountingContext())
                {
                    try
                    {
                        ctx.TercerosFallecidos.Add(TercerosFallecidos);
                        ctx.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException dbE)
                    {

                    }
                }
            }

            List<SelectListItem> terceros = new List<SelectListItem>();
            IList<Tercero> listTerceros = db.Terceros.ToList();


            terceros.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            foreach (var item in listTerceros)		// recorro los elementos de la db
            {
                terceros.Add(new SelectListItem { Text = item.NIT + " | " + item.NOMBRE1 + " " + item.NOMBRE2 + " " + item.APELLIDO1 + " " + item.APELLIDO2, Value = item.NIT });

            }
            //fin select list para rutas

            ViewBag.terceros = terceros;

            return View(TercerosFallecidos);
        }

        [Compress]
        public ActionResult getTercerosFallecidos(string term = "", string outputFormat = "datatables")
        {
            var productos = db.TercerosFallecidos.ToList();
            var _productosDTO = productos;

            switch (outputFormat)
            {
                case "datatables":
                    var dataTabledata = _productosDTO.Select((x, index)
                    => new[] { x.nit, x.terceroFK.NOMBRE1, x.terceroFK.NOMBRE2, x.terceroFK.APELLIDO1, x.terceroFK.APELLIDO2,x.fechaFallecido.ToString("yyyy-MM-dd"), BotonEditar(x.id), BotonEliminar(x.id) });
                    return Json(new { data = dataTabledata }, JsonRequestBehavior.AllowGet);

                default:
                    return Json(_productosDTO, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Edit(int id)
        {
         

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TercerosFallecidos tercerosFallecidos = db.TercerosFallecidos.Find(id);
            if (tercerosFallecidos == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> terceros = new List<SelectListItem>();
            IList<Tercero> listTerceros = db.Terceros.ToList();


            terceros.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            foreach (var item in listTerceros)		// recorro los elementos de la db
            {
                terceros.Add(new SelectListItem { Text = item.NIT + " | " + item.NOMBRE1 + " " + item.NOMBRE2 + " " + item.APELLIDO1 + " " + item.APELLIDO2, Value = item.NIT });

            }
            //fin select list para rutas

            ViewBag.terceros = terceros;

            return View(tercerosFallecidos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([System.Web.Http.FromBody] TercerosFallecidos TercerosFallecidos)
        {

            if (ModelState.IsValid)
            {
                db.Entry(TercerosFallecidos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            List<SelectListItem> terceros = new List<SelectListItem>();
            IList<Tercero> listTerceros = db.Terceros.ToList();


            terceros.Add(new SelectListItem { Text = "--Seleccione--", Value = "" });
            foreach (var item in listTerceros)		// recorro los elementos de la db
            {
                terceros.Add(new SelectListItem { Text = item.NIT + " | " + item.NOMBRE1 + " " + item.NOMBRE2 + " " + item.APELLIDO1 + " " + item.APELLIDO2, Value = item.NIT });

            }
            //fin select list para rutas

            ViewBag.terceros = terceros;

            return View(TercerosFallecidos);

        }

        [HttpPost]
        public JsonResult VerificaAsociadoFallecido(string id)
        {
            //int idd = Convert.ToInt32(id);
            var tercero = db.TercerosFallecidos.Where(x => x.nit == id).FirstOrDefault();
            if (tercero == null)
            {
                return new JsonResult { Data = new { status = false } };

            }
            else
            {
                return new JsonResult { Data = new { status = true } };
            }

        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            //int idd = Convert.ToInt32(id);
            var tercero = db.TercerosFallecidos.Find(id);
            if (tercero == null)
            {
                return new JsonResult { Data = new { status = false } };

            }
            else
            {
                 db.TercerosFallecidos.Remove(tercero);
                 db.SaveChanges();
                 return new JsonResult { Data = new { status = true } };
            }

        }
        private string BotonEditar(int id)
        {
            var botonEditar = @"<a href='/Terceros/TercerosFallecidos/Edit?id=" + id + "' class='btn btn-default fa fa-pencil'></a>";
            return botonEditar;
        }
        private string BotonEliminar(int id)
        {
            //<a href="/Terceros/terceros/Create" class="AddUser btn btn-success btn-xs btnnuevo" data-toggle="modal" data-target="#centro">Nuevo</a>

            //  var botonEditar = "<button id=" + id + " class='fa fa-pencil edit' onclick='edit(this);'></button>";
            var botonEliminar = @"<button value='" + id + "' class='btnEliminar btn btn-danger glyphicon glyphicon-trash '></button>";
            return botonEliminar;
        }

        [HttpPost]
        public JsonResult VerificaFallecido(string nit)
        {
            //int idd = Convert.ToInt32(id);
            var tercero = db.TercerosFallecidos.Where(x => x.nit == nit).FirstOrDefault();
            if (tercero == null)
            {
                return new JsonResult { Data = new { status = false } };

            }
            else
            {
                return new JsonResult { Data = new { status = true } };
            }

        }

        [HttpPost]
        public JsonResult VerificaFallecido2(string cuenta)
        {
            string documento = "";
            var fichasAportes = db.FichasAportes.Where(x => x.numeroCuenta == cuenta).FirstOrDefault();
            if(fichasAportes!=null)
            {
                documento = fichasAportes.Terceros.NIT;
            }
            var tercero = db.TercerosFallecidos.Where(x => x.nit == documento).FirstOrDefault();
            if (tercero == null)
            {
                return new JsonResult { Data = new { status = false } };

            }
            else
            {
                return new JsonResult { Data = new { status = true } };
            }

        }

    }
}