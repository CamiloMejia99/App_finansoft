using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Facturacion;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.facturacion.Controllers
{
    public class ProductoController : Controller
    {
        NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;
        AccountingContext db = new AccountingContext();
        // GET: facturacion/Producto
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {

            List<SelectListItem> iva = new List<SelectListItem>();
            IList<iva> listIva = db.iva.ToList();


            foreach (var item in listIva)		// recorro los elementos de la db
            {
                iva.Add(new SelectListItem { Text = item.name, Value = item.id.ToString() });

            }
            //fin select list para rutas

            ViewBag.iva = iva;
            ViewBag.Post2 = ViewBag.action = "Create";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([System.Web.Http.FromBody] producto producto)
        {

            if (producto.auxPrecioEntrada != "")
            {
                producto.auxPrecioEntrada = producto.auxPrecioEntrada.Replace(".", "");
                producto.precioEntrada = Convert.ToDecimal(producto.auxPrecioEntrada);
            } else { producto.precioEntrada = 0; }

            if (producto.auxPrecioSalida != "")
            {
                producto.auxPrecioSalida = producto.auxPrecioSalida.Replace(".", "");
                producto.precioSalida = Convert.ToDecimal(producto.auxPrecioSalida);
            }
            else { producto.precioSalida = 0; }

            if (ModelState.IsValid)
            {
                using (var ctx = new AccountingContext())
                {
                    try
                    {

                        var operacion = new operation()
                        {
                            productId = producto.id,
                            quantity = producto.inventarioInicial,
                            operationTypeId = 1,
                            date = DateTime.Now,
                            price = producto.precioEntrada
                        };
                        ctx.operation.Add(operacion);

                        ctx.producto.Add(producto);
                        ctx.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (DbEntityValidationException dbE)
                    {

                    }
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);

            List<SelectListItem> iva = new List<SelectListItem>();
            IList<iva> listIva = db.iva.ToList();


            foreach (var item in listIva)		// recorro los elementos de la db
            {
                iva.Add(new SelectListItem { Text = item.name, Value = item.id.ToString() });

            }
            //fin select list para rutas

            ViewBag.iva = iva;

            return View(producto);
        }

        public ActionResult Edit(int id)
        {
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            producto producto = db.producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> iva = new List<SelectListItem>();
            IList<iva> listIva = db.iva.ToList();


            foreach (var item in listIva)		// recorro los elementos de la db
            {
                iva.Add(new SelectListItem { Text = item.name, Value = item.id.ToString() });

            }
            //fin select list para rutas

            ViewBag.iva = iva;
            ViewBag.precioEnt = producto.precioEntrada.ToString("N0",formato);
            ViewBag.precioSal = producto.precioSalida.ToString("N0",formato);

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([System.Web.Http.FromBody] producto producto)
        {

            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            if (producto.auxPrecioEntrada != "")
            {
                producto.auxPrecioEntrada = producto.auxPrecioEntrada.Replace(".", "");
                producto.precioEntrada = Convert.ToDecimal(producto.auxPrecioEntrada);
            }
            else { producto.precioEntrada = 0; }

            if (producto.auxPrecioSalida != "")
            {
                producto.auxPrecioSalida = producto.auxPrecioSalida.Replace(".", "");
                producto.precioSalida = Convert.ToDecimal(producto.auxPrecioSalida);
            }
            else { producto.precioSalida = 0; }


            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            List<SelectListItem> iva = new List<SelectListItem>();
            IList<iva> listIva = db.iva.ToList();


            foreach (var item in listIva)		// recorro los elementos de la db
            {
                iva.Add(new SelectListItem { Text = item.name, Value = item.id.ToString() });

            }
            //fin select list para rutas

            ViewBag.iva = iva;
            ViewBag.precioEnt = producto.precioEntrada.ToString("N0", formato);
            ViewBag.precioSal = producto.precioSalida.ToString("N0", formato);


            return View(producto);
        }

        [Compress]
        public ActionResult getProductos(string term="",string outputFormat = "datatables")
        {
            var productos = db.producto.ToList();
            var _productosDTO = productos;

            switch(outputFormat)
            {
                case "datatables":
                    var dataTabledata = _productosDTO.Select((x, index)
                    => new[] { x.id.ToString(),  x.nomProducto, x.precioEntrada.ToString("N0",formato)   ,  x.precioSalida.ToString("N0",formato),  x.ivaFK.name,  x.inventarioInicial.ToString() ,BotonEditar(x.id),BotonEliminar(x.id)});
                    return Json(new { data = dataTabledata }, JsonRequestBehavior.AllowGet);

                default:
                    return Json(_productosDTO, JsonRequestBehavior.AllowGet);
            }

        }


        private string BotonEditar(int id)
        {
            var botonEditar = @"<a href='/facturacion/Producto/Edit?id=" + id + "' class='btn btn-default fa fa-pencil'></a>";
            return botonEditar;
        }

        private string BotonEliminar(int id)
        {
            //<a href="/Terceros/terceros/Create" class="AddUser btn btn-success btn-xs btnnuevo" data-toggle="modal" data-target="#centro">Nuevo</a>

            //  var botonEditar = "<button id=" + id + " class='fa fa-pencil edit' onclick='edit(this);'></button>";
            var botonEliminar = @"<button value='"+id+ "' class='btnEliminar btn btn-danger glyphicon glyphicon-trash '></button>";
            return botonEliminar;
        }


        [HttpPost]
        public JsonResult getValoresAuxiliares(int id)
        {

            var producto = db.producto.Find(id);
            if (producto == null)
            {
                return new JsonResult { Data = new { status = false } };

            }
            else
            {
                string precioEntrada = producto.precioEntrada.ToString("N0", formato);
                string precioSalida = producto.precioSalida.ToString("N0", formato);
                string iva = producto.iva.ToString();
                return new JsonResult { Data = new { status = true,precioEntrada,precioSalida,iva } };
            }

        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            //int idd = Convert.ToInt32(id);
            var producto = db.producto.Find(id);
            if (producto == null)
            {
                return new JsonResult { Data = new { status = false } };

            }
            else
            {
                var dataOperacion = db.operation.Where(x => x.productId == producto.id && x.operationType.tipo == 2).ToList();
                if(dataOperacion.Count>0)
                {
                    return new JsonResult { Data = new { status = false } };
                }
                else
                {
                    
                    db.operation.RemoveRange(db.operation.Where(x => x.productId == producto.id));
                    db.producto.Remove(producto);
                    db.SaveChanges();
                    return new JsonResult { Data = new { status = true } };
                }

                
            }

        }

    }
}