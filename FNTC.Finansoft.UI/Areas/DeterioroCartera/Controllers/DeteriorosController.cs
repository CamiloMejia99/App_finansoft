using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.DeterioroCartera;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.BLL;

namespace FNTC.Finansoft.UI.Areas.DeterioroCartera.Controllers
{
    [Authorize]
    public class DeteriorosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: DeterioroCartera/Deterioros
        //[Authorize]
        public ActionResult Index()
        {
            
            var deterioros = db.Deterioros.Include(d => d.DeterioroPar);
            return View(deterioros.ToList());
        }

        // GET: DeterioroCartera/Deterioros/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deterioro deterioro = db.Deterioros.Find(id);
            if (deterioro == null)
            {
                return HttpNotFound();
            }
            return View(deterioro);
        }

        // GET: DeterioroCartera/Deterioros/Create
        public ActionResult Create()
        {
            ViewBag.IdRango = new DeterioroParametrosController().parametrosprovision().Select(p => new SelectListItem { Text = p.TipoProvision + " || Rango " + p.Rango + " || Desde " + p.Desde + " días Hasta " + p.Hasta + " días", Value = p.Id.ToString(), Selected = false }); ;
            var metodos = new SelectList(new[]
                                         {
                                              new {ID="1",Name="Deterioro Directo"}
                                          },
                           "ID", "Name", 1);

            ViewBag.Metodo = metodos;
            
            //publica
            var deterioros = db.Deterioros.Include(d => d.DeterioroPar);
            ViewBag.deterioros = deterioros;
            return View();
        }

        // POST: DeterioroCartera/Deterioros/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdRango,Metodo,ValorSuma,observacion")] Deterioro deterioro)
        {
            if (ModelState.IsValid)
            {
                deterioro.FechaGenerada = DateTime.Now;
                int id= deterioro.IdRango;
                var parametro = new DeterioroParametrosController().parametrosid(id);
                deterioro.Metodo = "Deterioro Directo";
                var ValorDeterioroCartera = GenerarDeterioro(parametro[0].Desde, parametro[0].Hasta, parametro[0].TipoProvision); 
                //var ValorDeterioroCartera= SumaDeterioro(parametro[0].Desde, parametro[0].Hasta);
                var porcentaje =Convert.ToDecimal(parametro[0].PProvision)/100;
                ValorDeterioroCartera = ValorDeterioroCartera * porcentaje;
                deterioro.ValorSuma = ValorDeterioroCartera.ToString("0.00");
                db.Deterioros.Add(deterioro);
                db.SaveChanges();

                ContabilizarDeterioroCartera(ValorDeterioroCartera);

                return RedirectToAction("Index");
            }
            
            

            var deterioros = db.Deterioros.Include(d => d.DeterioroPar);
            ViewBag.deterioros = deterioros;
            ViewBag.IdRango = new SelectList(db.DeterioroPars, "Id", "Rango", deterioro.IdRango);
            return View(deterioro);
        }

        // GET: DeterioroCartera/Deterioros/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deterioro deterioro = db.Deterioros.Find(id);
            if (deterioro == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdRango = new SelectList(db.DeterioroPars, "Id", "Rango", deterioro.IdRango);
            return View(deterioro);
        }

        // POST: DeterioroCartera/Deterioros/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdRango,Metodo,ValorSuma,observacion,FechaGenerada")] Deterioro deterioro)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deterioro).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdRango = new SelectList(db.DeterioroPars, "Id", "Rango", deterioro.IdRango);
            return View(deterioro);
        }

        // GET: DeterioroCartera/Deterioros/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Deterioro deterioro = db.Deterioros.Find(id);
            if (deterioro == null)
            {
                return HttpNotFound();
            }
            return View(deterioro);
        }

        // POST: DeterioroCartera/Deterioros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Deterioro deterioro = db.Deterioros.Find(id);
            db.Deterioros.Remove(deterioro);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public decimal SumaDeterioro(string desde, string hasta ) {
            //funcion provisional que no tiene encuenta los tipos de provisión=lineas de credito
            int desde2 = Int32.Parse(desde);
            int hasta2 = Int32.Parse(hasta);
            var PagareMora = db.HistorialCreditos.GroupBy(t => t.estado == "enMora" &&  t.diasEnMora>=desde2 && t.diasEnMora<= hasta2)
                .Select(x=>new { 
            x.Key,
            totalsum =x.Select(y=> y.capitalEnMora).Sum()
            });
            decimal  sumafin=0;
            
            foreach (var item in PagareMora)
            {
                if (item.Key)
                {
                    sumafin = item.totalsum;
                }
                    
            }

            
            return sumafin;
        }

        public decimal GenerarDeterioro(string desde, string hasta, string linea)
        {
            //revisa que tipo de linea tiene cada pagare para generar el deterioro por tipo de provisión 
            int desde2 = Int32.Parse(desde);
            int hasta2 = Int32.Parse(hasta);
            decimal TotalCapitalMora = 0;
            var lin = db.Lineas.Where(x => x.Lineas_Descripcion == linea).FirstOrDefault();
            try
            {
             var lineabase = (from pc in db.Lineas where pc.Lineas_Descripcion == linea select pc).Single();
            var Creditoslinea = db.Creditos.Where(t => t.Lineas_Id == lineabase.Lineas_Id).ToList();
             var PagareMora2 = db.TotalesCreditos.Where(t => t.Estado == "EM" && t.DiasMora >= desde2 && t.DiasMora <= hasta2)
                .ToList();
            var PagareMoraLinea = from c in Creditoslinea
                                 join p in PagareMora2
                                on c.Pagare equals p.Pagare
                                select new {c.Pagare,p.CapitalMoraPendiente,p.DiasMora };

            TotalCapitalMora = PagareMoraLinea.Sum(y => y.CapitalMoraPendiente);
            return TotalCapitalMora;

            }
            catch (Exception)
            {
                TotalCapitalMora =0;
                ViewBag.nota = "Aun no se encuentra creado la linea de credito " + linea;
                return TotalCapitalMora;
            }


        }
       

        public ActionResult ObtenerDeterioros()
        {
            var deterioros = db.Deterioros.Include(c => c.DeterioroPar).ToList();
            
            var dataTabledata = deterioros.Select((x, index)
            => new[] { x.Id.ToString(), x.DeterioroPar.Rango, x.DeterioroPar.TipoProvision,x.DeterioroPar.Desde,x.DeterioroPar.Hasta,x.Metodo,x.DeterioroPar.PProvision+ "%", String.Format("{0:C}", x.ValorSuma),x.observacion, x.FechaGenerada.ToString()
            });
            return Json(new { data = dataTabledata }, JsonRequestBehavior.AllowGet);


           

        }


        
        public void  ContabilizarDeterioroCartera(decimal ValorDeterioroCartera)
        {
            

            //CONTRUIR EL COMPROBANTE

            var cuenta = (from pc in db.CuentaDeterioroCartera where pc.Id == 1 select pc).Single();
                var TComprobante = db.TiposComprobantes.FirstOrDefault(x => x.CODIGO == cuenta.TipoComprobante.CODIGO & x.INACTIVO == false);
            //var cajaPago = (from pc in db.Caja where pc.Codigo_caja == factOpCajaConsCuotaCredito.codigoCaja select pc).Single();
            //var credito = (from pc in db.Creditos where pc.Creditos_Cedula == factOpCajaConsCuotaCredito.NIT select pc).Single();

            var comprobanteNew = new Comprobante()
            
                {
                    TIPO = cuenta.TipoComprobante.CODIGO.ToString(),
                    NUMERO = TComprobante.CONSECUTIVO.ToString(),
                    ANO = Convert.ToString(DateTime.Now.Year),
                    MES = Convert.ToString(DateTime.Now.Month),
                    DIA = Convert.ToString(DateTime.Now.Day),
                    CCOSTO = "00",
                    DETALLE = "Deterioro de Cartera",
                    TERCERO = "891280005",
                    CTAFPAGO = cuenta.CuentaGastosProvision.CODIGO,
                    VRTOTAL = ValorDeterioroCartera,
                    SUMDBCR = 0,
                    FECHARealiz = DateTime.Now,
                    ANULADO = false
                };

                db.Comprobantes.Add(comprobanteNew);
                
                //CONSTRUIR LA LISTA DE MOVIMIENTOS
                List<Movimiento> listaDeMovimientos = new List<Movimiento>();
                //CAJA
                var mov1 = new Movimiento()
                {
                    TIPO = cuenta.TipoComprobante.CODIGO,
                    NUMERO = TComprobante.CONSECUTIVO,
                    CUENTA = cuenta.CuentaDeterioro.CODIGO,
                    TERCERO = "891280005",
                    DETALLE = "Deterioro de cartera mov 1",
                    DEBITO = ValorDeterioroCartera,
                    CREDITO = 0,
                    BASE = 0,
                    CCOSTO = "00",//Session["cc_transacciones" + User.Identity.Name].ToString(),
                    FECHAMOVIMIENTO = DateTime.Now
                 //   DOCUMENTO = item.id.ToString()
                };
            listaDeMovimientos.Add(mov1);
            var mov2 = new Movimiento()
            {
                TIPO = cuenta.TipoComprobante.CODIGO,
                NUMERO = TComprobante.CONSECUTIVO,
                CUENTA = cuenta.CuentaGastosProvision.CODIGO,
                TERCERO = "891280005",
                DETALLE = "Valor Detrioro Cartera mov 2",
                DEBITO = 0,
                CREDITO = ValorDeterioroCartera,
                BASE = 0,
                CCOSTO = "00",//Session["cc_transacciones" + User.Identity.Name].ToString(),
                        FECHAMOVIMIENTO = DateTime.Now
                        //DOCUMENTO = item.id.ToString()
                    };
                    listaDeMovimientos.Add(mov2);

            var result = false;

            var comprobanteConst = new ComprobanteBO();
            result = comprobanteConst.AsentarConsignacionCaja(listaDeMovimientos, Convert.ToInt32(TComprobante.CONSECUTIVO), cuenta.TipoComprobante.CODIGO);

            if (result)
            {
               // paramFactOpCaja.fecha = DateTime.Now;
                //db.factOpCajaConsCuotaCreditoEntidadDos.Add(paramFactOpCaja);
                db.SaveChanges();
            }


        }


        public Boolean HayLinea(int linea  )
        {
            var parametros = db.DeterioroPars.Find(linea);
            var dto= db.Lineas.Where(x => x.Lineas_Descripcion == parametros.TipoProvision).FirstOrDefault();
            if (dto != null)
             {
                    return true;
             }
             

            return false;
        }

    }
        }
    
   
