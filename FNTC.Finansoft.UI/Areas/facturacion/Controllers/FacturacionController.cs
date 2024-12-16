using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Facturacion;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.facturacion.Controllers
{
    public class FacturacionController : Controller
    {
        AccountingContext db = new AccountingContext();
        // GET: facturacion/Facturacion
        public ActionResult Index()
        {
            var terceros = new List<SelectListItem>();
            var dataTerceros = db.Terceros.ToList();
            foreach(var item in dataTerceros)
            {
                terceros.Add(new SelectListItem { Text = item.NIT + "|" + item.NOMBRE1 + " " + item.NOMBRE2 + " " + item.APELLIDO1 + " " + item.APELLIDO2, Value = item.NIT });
            }

            var formasPago = new List<SelectListItem>();
            var dataFP = db.FacFormasDePago.ToList();
            foreach (var item in dataFP)
            {
                formasPago.Add(new SelectListItem { Text = item.nombre, Value = item.id.ToString() });
            }

            var productos = db.producto.ToList();

            foreach (var item in productos)
            {
                var productInOperation = (from pc in db.operation where pc.productId == item.id select pc).ToList();
                var cantidad = 0;
                foreach (var operation in productInOperation)
                {
                    if (operation.operationType.tipo == 1)
                    {
                        cantidad = cantidad + operation.quantity;
                    }
                    else if (operation.operationType.tipo == 2)
                    {
                        cantidad = cantidad - operation.quantity;
                    }
                }
                item.inventarioInicial = cantidad;
            }

            ViewBag.clientes = terceros;
            ViewBag.formasPago = formasPago;

            return View(productos);
        }

        public ActionResult getVendiendoCaja()
        {
            
            //lista de Costos Adicionales
            List<SelectListItem> pedidoContado = new List<SelectListItem>();   // Creo una lista
            var listaOperaciones = db.operation.Where(p => p.operationTypeId == 14).ToList();

            List<pedidosViewModel> enviarPedidosContado = new List<pedidosViewModel>();
            foreach (var item in listaOperaciones)
            {
                decimal total = 0;
                total = item.quantity * item.price;
                var productos = new pedidosViewModel()
                {
                    cantidad = item.quantity,
                    nombre = item.productoFK.nomProducto,
                    unidad = item.price,
                    total = total,
                    operatioId = item.id
                };

                enviarPedidosContado.Add(productos);
            }

            string json = JsonConvert.SerializeObject(enviarPedidosContado);

            return Json(json);
        }


        public JsonResult DeleteProductoContado(int id)
        {
            operation operation = db.operation.Find(id);
            db.operation.Remove(operation);
            db.SaveChanges();

            return Json(1, JsonRequestBehavior.AllowGet);
        }


        public JsonResult verificarExistencia(int id, int cantidadPedida)
        {
            var productInOperation = (from pc in db.operation where pc.productId == id select pc).ToList();
            var cantidad = 0;
            foreach (var operation in productInOperation)
            {
                if (operation.operationType.tipo == 1)
                {
                    cantidad = cantidad + operation.quantity;
                }
                else if (operation.operationType.tipo == 2)
                {
                    cantidad = cantidad - operation.quantity;
                }
            }
            if (cantidad < cantidadPedida)
            {
                cantidad = 0;
            }

            return Json(cantidad, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddProductoVendiendoCaja(int id, int cantidad)
        {
            var producto = (from pc in db.producto where pc.id == id select pc).FirstOrDefault();

            decimal precioColocar = producto.precioSalida;
            
            var nuevaOpercion = new operation()
            {
                productId = id,
                quantity = cantidad,
                operationTypeId = 14,
                date = DateTime.Now,
                price = precioColocar
            };

            db.operation.Add(nuevaOpercion);
            db.SaveChanges();

            return Json(1, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult terminarFacturaVenderCaja(string cliente, decimal efectivo,int formaPago)
        {
            
            var listaOperaciones = db.operation.Where(p => p.operationTypeId == 14).ToList();
            int siConvenio = 0;
            decimal valorDestinado = 0;
            decimal valorCashBack = 0;

            decimal totalCompra = 0;
            decimal totalBaseIva19 = 0;
            decimal totalIva19 = 0;
            decimal totalBaseIva5 = 0;
            decimal totalIva5 = 0;
            decimal totalExcentos = 0;
            decimal totalExcluidos = 0;
         
            foreach (var item in listaOperaciones)
            {
                decimal ivaTemp19 = 0;
                decimal ivaTemp5 = 0;
                totalCompra = totalCompra + item.quantity * item.price;

                ivaTemp19 = 0;
                ivaTemp5 = 0;


                if (item.productoFK.iva == 1)
                {

                    decimal baseTemp = item.price/Convert.ToDecimal(1.19);
                    decimal ivaTemp = item.price - baseTemp;

                    totalBaseIva19 += baseTemp;
                    totalIva19 += ivaTemp;
                    
                }else if(item.productoFK.iva==2)
                {
                    decimal baseTemp = item.price / Convert.ToDecimal(1.05);
                    decimal ivaTemp = item.price - baseTemp;

                    totalBaseIva5 += baseTemp;
                    totalIva5 += ivaTemp;
                }else if(item.productoFK.iva==3)
                {
                    totalExcentos += item.price;
                }else if(item.productoFK.iva==4)
                {
                    totalExcluidos += item.price;
                }



            }//FIN foreach

            decimal totalSinIva = totalCompra - totalIva19 - totalIva5;

            var persona = db.Terceros.Where(x => x.NIT == cliente);
            var consecutivoFactura = db.consecutivosFacturas.Find(1);
            var nuevaFactura = new factura()
            {
                personId = cliente,
                operationTypeId = 15,
                date = DateTime.Now.AddHours(2),
                total = totalCompra,
                tipo = 1,//1 equivale a contado
                fechaPagoCredito = "N",
                cash = efectivo,
                codConsecutivo = consecutivoFactura.cod,
                numeroFactura = consecutivoFactura.actual,
                valorTotalExcentos = totalExcentos,
                valorTotalExcluidos = totalExcluidos,
                baseIVA19 = totalBaseIva19,
                baseIVA5 = totalBaseIva5,
                valorIVA19 = totalIva19,
                valorIVA5 = totalIva5,
                totalBolsas = 0,
                valorConvenio = 0,
                idFormaPago = formaPago
            };
            db.factura.Add(nuevaFactura);
            consecutivoFactura.actual = consecutivoFactura.actual + 1;
            db.Entry(consecutivoFactura).State = System.Data.Entity.EntityState.Modified;//PARA QUE SIRVE ESTA LINEA?
            db.SaveChanges();

            foreach (var item in listaOperaciones)
            {
                item.operationTypeId = 15;
                item.facturaId = nuevaFactura.id;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }

            //var Comprobante = (from pc in db.TipoComprobantes where pc.codigo == "CC1" select pc).Single();
            var fechaActual = DateTime.Now.AddHours(2);

            

            db.SaveChanges();
            return Json(nuevaFactura.id, JsonRequestBehavior.AllowGet);
        }

        public ActionResult imprimirFacturaCaja(int id)
        {
            var factura = (from pc in db.factura where pc.id == id select pc).FirstOrDefault();

            List<SelectListItem> pedidoContado = new List<SelectListItem>();   // Creo una lista
            var listaOperaciones = db.operation.Where(p => p.operationTypeId == 15 && p.facturaId == id).ToList();

            List<pedidosViewModel2> enviarPedidosContado = new List<pedidosViewModel2>();

            foreach (var item in listaOperaciones)
            {
                decimal total = 0;
                total = item.quantity * item.price;

                var productos = new pedidosViewModel2()
                {
                    cod = item.productoFK.id,
                    cantidad = item.quantity,
                    nombre = item.productoFK.nomProducto,
                    unidad = Convert.ToDecimal(item.price).ToString("#,##"),
                    iva = item.productoFK.ivaFK.name,
                    total = Convert.ToDecimal(total).ToString("#,##"),
                };

                enviarPedidosContado.Add(productos);
            }


            //datos de empresa
            string nombre = "";
            string dirEmpresa = "";
            string ciuDep = "";
            string telefono = "";
            string correo = "";
            var dataEmpresa = db.Empresa.FirstOrDefault();
            if(dataEmpresa!=null)
            {
                nombre = dataEmpresa.nombre.ToUpper();
                dirEmpresa = dataEmpresa.direccion.ToUpper();
                ciuDep = (dataEmpresa.terceroFK.municipioFK.Nom_muni);
                var dataDepto = db.Departamento.Where(x => x.Id_dep == dataEmpresa.terceroFK.municipioFK.Dep_muni).FirstOrDefault();
                if(dataDepto!=null)
                {
                    ciuDep += (" - "+dataDepto.Nom_dep);
                }
                telefono = dataEmpresa.telefono;
                correo = dataEmpresa.correo;
            }


            ViewBag.identificacion = factura.terceroFK.NIT;
            ViewBag.cliente = factura.terceroFK.NOMBRE1 + " " + factura.terceroFK.NOMBRE2 + " " + factura.terceroFK.APELLIDO1 + " " + factura.terceroFK.APELLIDO2; ;
            ViewBag.direccion = factura.terceroFK.DIR;
            ViewBag.celular = factura.terceroFK.TEL;
            ViewBag.correo = factura.terceroFK.EMAIL;
            ViewBag.vendedor = nombre.ToUpper();
            ViewBag.numeroFactura = factura.numeroFactura;
            ViewBag.fecha = factura.date;
            ViewBag.valorTotalExcentos = Convert.ToDecimal(factura.valorTotalExcentos).ToString("#,##");
            ViewBag.valorTotalExcluidos = Convert.ToDecimal(factura.valorTotalExcluidos).ToString("#,##");
            ViewBag.baseIVA19 = Convert.ToDecimal(factura.baseIVA19).ToString("#,##");
            ViewBag.valorIVA19 = Convert.ToDecimal(factura.valorIVA19).ToString("#,##");
            ViewBag.baseIVA5 = Convert.ToDecimal(factura.baseIVA5).ToString("#,##");
            ViewBag.valorIVA5 = Convert.ToDecimal(factura.valorIVA5).ToString("#,##");
            ViewBag.totalBolsas = Convert.ToDecimal(factura.totalBolsas).ToString("#,##");
            ViewBag.valorConvenio = Convert.ToDecimal(factura.valorConvenio).ToString("#,##");
            ViewBag.total = Convert.ToDecimal(factura.total).ToString("#,##");
            ViewBag.formaPago = factura.formaPagoFK.nombre;

            ViewBag.nomEmpresa = nombre;
            ViewBag.dirEmpresa = dirEmpresa;
            ViewBag.ciuDep = ciuDep.ToUpper();
            ViewBag.telEmpresa = telefono;
            ViewBag.emailEmpresa = correo;
          
            if (factura.valorConvenio > 0)
            {
                ViewBag.beneficio = 1;
            }
            else
            {
                ViewBag.beneficio = 2;
            }


            return View(enviarPedidosContado);
        }
    }
}