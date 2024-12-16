using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

using FNTC.Finansoft.Accounting.DTO.Terceros;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using Newtonsoft.Json;
using FNTC.Finansoft.Accounting.DTO;
using System.Globalization;

namespace FNTC.Finansoft.UI.Areas.Creditos.Controllers
{
    public class PrestamosController : Controller
    {
        private AccountingContext db = new AccountingContext();
        NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;

        
        // GET: /Prestamos/
        public ActionResult Index()
        {
            var prestamos = db.Prestamos.Include(p => p.Forma_Pago).Include(p => p.Tipo_Periodo);
            return View(prestamos.ToList());
        }

        //metodo para sacar el ultimo id asignado a la tabla prestamos
        public ActionResult AutoHidden()
        {
            var ultimpid = (from s in db.Prestamos orderby s.id descending select s).First();

            ViewBag.ultimoid = ultimpid;
            return View();
        }
        //Validacion Remota Pagare
        public JsonResult ValidacionPagare(string Pagare)
        {
            //  var query=(from s in db.Prestamos orderby s.id descending select s)
            var query = (from s in db.Prestamos where s.Pagare == Pagare select s.Pagare).Count();

            if (query == 0)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json("El Pagaré Ya Existe", JsonRequestBehavior.AllowGet);
        }

        //Guardar Pagare y Costos Adicionales
        [HttpPost]
        public JsonResult AddCostos(CostosPrestamos costosprestamos)
        {
            if (ModelState.IsValid)
            {
                db.CostosPrestamos.Add(costosprestamos);
                db.SaveChanges();
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        //VALIDA CAPITAL, INTERES Y PLAZO DE ACUERDO A UN DESTINO SELECCIONADO , decimal Interes, decimal Plazo
        //====================================================================
        [HttpPost]
        public JsonResult QueryCap(int Destino, long Capital)
        {
            bool status = false;

            var querydestino = (from destinos in db.Destinos where destinos.Destino_Id.Equals(Destino) select destinos.Destino_Id).Count();

            if (querydestino != 0)
            {
                //retorna valores del capital
                var qcapital = (from destinos in db.Destinos
                                where destinos.Destino_Id.Equals(Destino) &&
                                (Capital >= destinos.Destino_Valor_Minimo) && (Capital <= destinos.Destino_Valor_Maximo)
                                select destinos.Destino_Id).Count();
                if (qcapital == 0)
                {
                    status = true;
                    var capital = from destinos in db.Destinos
                                  where destinos.Destino_Id == Destino
                                  select new { destinos.Destino_Descripcion, destinos.Destino_Valor_Minimo, destinos.Destino_Valor_Maximo };

                    return new JsonResult { Data = new { status = status, capital } };
                }

                //retorna valoeres del interes
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
            //var querydestino = (from destinos in db.Destinos
            //                    where destinos.Destino_Id.Equals(Destino)
            //                      && (Capital >= destinos.Destino_Valor_Minimo) && (Capital <= destinos.Destino_Valor_Maximo)
            //                      && (Interes >= destinos.Destino_Tasa_Minima) && (Interes <= destinos.Destino_Tasa_Maxima)
            //                      && (Plazo >= destinos.Destino_Periodo_Minimo) && (Plazo <= destinos.Destino_Periodo_Maximo)
            //                    select destinos.Destino_Id).Count();

            //if (querydestino==null || querydestino==0)
            //{
            //    status = true;
            //    var querydestino2 = from destino in db.Destinos where destino.Destino_Id == Destino select new 
            //    {
            //        destino.Destino_Descripcion,
            //        destino.Destino_Valor_Minimo,
            //        destino.Destino_Valor_Maximo,
            //        destino.Destino_Tasa_Minima,
            //        destino.Destino_Tasa_Maxima,
            //        destino.Destino_Periodo_Minimo,
            //        destino.Destino_Periodo_Maximo
            //    };
            //    return new JsonResult { Data = new { status = status, querydestino2 } };
            //}
            //else
            //{
            //    return Json(true, JsonRequestBehavior.AllowGet);
            //}




        }//fin metodo query destino

        //DESTINO INTERES
        //==============
        [HttpPost]
        public JsonResult QueryInt(int Destino, decimal Interes)
        {
            bool status = false;

            var querydestinoInt = (from destinos in db.Destinos where destinos.Destino_Id.Equals(Destino) select destinos.Destino_Id).Count();

            if (querydestinoInt != 0)
            {
                var qInteres = (from destinos in db.Destinos
                                where destinos.Destino_Id.Equals(Destino) &&
                                (Interes >= destinos.Destino_Tasa_Minima) && (Interes <= destinos.Destino_Tasa_Maxima)
                                select destinos.Destino_Id).Count();
                if (qInteres == 0)
                {
                    status = true;
                    var interes = from destinos in db.Destinos
                                  where destinos.Destino_Id == Destino
                                  select new { destinos.Destino_Descripcion, destinos.Destino_Tasa_Minima, destinos.Destino_Tasa_Maxima };

                    return new JsonResult { Data = new { status = status, interes } };
                }
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);

        }

        //DESTINO PLAZO
        //=============
        [HttpPost]
        public JsonResult QueryPlazo(int Destino, decimal Plazo)
        {
            bool status = false;

            var querydestinoPlazo = (from destinos in db.Destinos where destinos.Destino_Id.Equals(Destino) select destinos.Destino_Id).Count();

            if (querydestinoPlazo != 0)
            {

                var qPlazo = (from destinos in db.Destinos
                              where destinos.Destino_Id.Equals(Destino) &&
                              (Plazo >= destinos.Destino_Periodo_Minimo) && (Plazo <= destinos.Destino_Periodo_Maximo)
                              select destinos.Destino_Id).Count();
                if (qPlazo == 0)
                {
                    status = true;
                    var plazo = from destinos in db.Destinos
                                where destinos.Destino_Id == Destino
                                select new { destinos.Destino_Descripcion, destinos.Destino_Periodo_Minimo, destinos.Destino_Periodo_Maximo };

                    return new JsonResult { Data = new { status = status, plazo } };
                }


            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);

        }

        //Autocompletar Cliente
        [HttpPost]
        public JsonResult GetCedula(string Prefix)
        {

            var cad = (from terceros in db.Terceros where terceros.NIT.StartsWith(Prefix) select new { terceros.NIT, terceros.NOMBRE, terceros.SALARIO });


            return Json(cad, JsonRequestBehavior.AllowGet);
        }



        // GET: /Prestamos/Details/5
        public ActionResult Details(int? id, int? periodo)
        {



            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prestamos prestamos = db.Prestamos.Find(id);
            if (prestamos == null)
            {
                return HttpNotFound();
            }
            return View(prestamos);

        }

        [HttpGet]

        // GET: /Prestamos/Create
        public ActionResult Create()
        {
            //ViewBag.ClientesId = new SelectList(db.clientes, "id", "Nombre");
            ViewBag.Forma_Pago_Id = new SelectList(db.Forma_Pago, "Forma_Pago_Id", "Forma_Pago_Descripcion");
            ViewBag.Tipo_Periodo_Id = new SelectList(db.Tipo_Periodo, "Tipo_Periodo_Id", "Tipo_Periodo_Descripcion");

            //lista de garantias
            List<SelectListItem> items3 = new List<SelectListItem>();   // Creo una lista
            items3.Add(new SelectListItem { Text = "Seleccione Garantia", Value = "" });
            IList<Garantias> lista4 = db.Garantias.ToList();// extraigo los elementos desde la DB

            //IList<Garantias> lista4 = db.Garantias.Where(m => m.Garantias_Id == 1 && m.Garantias_Codeudor == true).ToList();
            foreach (var item in lista4)		// recorro los elementos de la db
            {
                //items3.Add(new SelectListItem { Text = item.Garantias_Descripcion + " | " + item.Garantias_Codeudor, Value = item.Garantias_Id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                items3.Add(new SelectListItem { Text = item.Garantias_Descripcion, Value = item.Garantias_Id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Garantia_Id = items3;

            //saca el ultimo id de de la tabla prestamos
            //var ultimpid = db.Prestamos.Max(u => u.id);
            //ViewBag.ultimoid = ultimpid;


            //lista de Codeudores
            List<SelectListItem> items3C = new List<SelectListItem>();   // Creo una lista
            items3C.Add(new SelectListItem { Text = "Seleccione Codeudor", Value = "" });
            IList<Tercero> lista4C = db.Terceros.ToList();// extraigo los elementos desde la DB

            var test = from s in lista4C where s.ESCODEUDOR == true select s;



            foreach (var item in lista4C)		// recorro los elementos de la db
            {
                items3C.Add(new SelectListItem { Text = item.NOMBRE + " | " + item.NIT + item.ESCODEUDOR, Value = item.NIT.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.NIT = items3C;


            //lista de Lineas
            List<SelectListItem> Lineas = new List<SelectListItem>();   // Creo una lista
            Lineas.Add(new SelectListItem { Text = "Seleccione Una Linea", Value = "" });
            //IList<Lineas> ListaLineas = db.Lineas.ToList();// extraigo los elementos desde la DB
            IList<Lineas> ListaLineas = (from l in db.Lineas where l.Lineas_Activo == true select l).ToList();

            foreach (var item in ListaLineas)		// recorro los elementos de la db
            {

                Lineas.Add(new SelectListItem { Text = item.Lineas_Descripcion, Value = item.Lineas_Id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Lineas_Id = Lineas;

            return View();
        }
        //obtener el id de la garantia por Post

        [HttpPost]
        public ActionResult Filtro(int input)
        {
            //IList<Garantias> garantias = new List<Garantias>();
            List<Garantias> Garantias = new List<Garantias>();
            IList<Garantias> lista4 = db.Garantias.ToList();

            var test = (from s in lista4 where s.Garantias_Id == input && s.Garantias_Codeudor == true select s).Count();


            if (test == 1)
            {
                var cod = 0;

                return Json(new { name = cod });
            }
            else
            {
                var hip = 1;
                return Json(new { name = hip });
            }
        }


        //fecha actual
        public JsonResult GetFechaActual()
        {
            return Json(DateTime.Now, JsonRequestBehavior.AllowGet);
        }


        //long pagare, int CA_Id
        [HttpPost]
        public ActionResult qDeleteCA(string pagare, int CA_Id)
        {
            //lista de Costos Adicionales
            List<SelectListItem> costos = new List<SelectListItem>();   // Creo una lista
            IList<CostosPrestamos> ListaCostos = db.CostosPrestamos.ToList();

            var test = (from s in ListaCostos
                        where s.CA_Id == CA_Id && s.Pagare == pagare

                        select new { s.Id_CostoPretamo, s.CA_Id, s.Costos_Adicionales.Cuenta_Cod, s.Costos_Adicionales.CA_Nombre, s.Costos_Adicionales.CA_Valor, s.Costos_Adicionales.CA_Porcentaje });

            string json = JsonConvert.SerializeObject(test);

            //return PartialView("_DeleteCA", json);
            return Json(json);
        }

        //ELIMINAR PAGARE Y CA_ID DE LA TABLAS COSTOPRESTAMOS
        //===================================================
        [HttpPost]
        public ActionResult DeleteCA(int id)
        {
            CostosPrestamos CostosPrestamos = db.CostosPrestamos.Find(id);
            db.CostosPrestamos.Remove(CostosPrestamos);
            db.SaveChanges();
            return Json("delete", JsonRequestBehavior.AllowGet);
        }
        //SUSTRACCION DE COSTOS ADICIONALES ACUMULADOS===VALOR FIJO
        //==================================================

        [HttpPost]
        public ActionResult qDecrecienteCA(string pagare, int CA_Id, long valor, int idCoPre)
        {

            var qDecreciente = from d in db.CostosPrestamos
                               where d.Pagare == pagare && d.CA_Id == CA_Id && d.Id_CostoPretamo == idCoPre
                               select new { d.Costos_Adicionales.CA_Valor };

            foreach (var item in qDecreciente)
            {
                var x = Convert.ToDecimal(item.CA_Valor);
                var res = valor - x;
                return Json(res, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);


        }

        //SUSTRACCION DE COSTOS ADICIONALES ACUMULADOS===PORCENTAJE
        //=========================================================
        [HttpPost]
        public ActionResult qDecrep(string pagare, int CA_Id, string porciento, int idCoPre)
        {
            var porc = Convert.ToDecimal(porciento);

            var qDecreciente = from d in db.CostosPrestamos
                               where d.Pagare == pagare && d.CA_Id == CA_Id && d.Id_CostoPretamo == idCoPre
                               select new { d.Costos_Adicionales.CA_Porcentaje };

            foreach (var item in qDecreciente)
            {
                var x = Convert.ToDecimal(item.CA_Porcentaje);
                var res = porc - x;
                return Json(res, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);


        }
        //METODO SUSTRACCION VALOR Y % ACUMULADOS
        //========================================
        [HttpPost]
        public ActionResult qDecrepVP(string pagare, int CA_Id, long valor, string porciento, int idCoPre)
        {
            var porc = Convert.ToDecimal(porciento);

            var qDecreciente = from d in db.CostosPrestamos
                               where d.Pagare == pagare && d.CA_Id == CA_Id && d.Id_CostoPretamo == idCoPre
                               select new { d.Costos_Adicionales.CA_Porcentaje, d.Costos_Adicionales.CA_Valor };

            foreach (var item in qDecreciente)
            {
                var val = Convert.ToDecimal(item.CA_Valor);
                var por = Convert.ToDecimal(item.CA_Porcentaje);
                var resPor = porc - por;
                var resVal = valor - val;
                var result = new { POR = resPor, VAL = resVal };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            return Json(true, JsonRequestBehavior.AllowGet);


        }
        //obtiene los costos adicionales
        [HttpPost]
        public ActionResult GetCostosAdicionales(int Costos)
        {

            //lista de Costos Adicionales
            List<SelectListItem> costos = new List<SelectListItem>();   // Creo una lista
            IList<Costos_Adicionales> ListaCostos = db.Costos_Adicionales.ToList();

            var test = from s in ListaCostos
                       where s.Destino_Id == Costos && s.CA_estado == true
                       select new { s.CA_Id, s.Cuenta_Cod, s.CA_Nombre, s.CA_Valor, s.CA_Porcentaje };

            string json = JsonConvert.SerializeObject(test);

            return Json(json);
            //return Json(json, JsonRequestBehavior.AllowGet);
        }

        //Metodo Para Obtener el Valor de Un Periodo
        [HttpPost]
        public ActionResult GetValorTipoPeriodo(int IdPeriodo)
        {
            var Valorp = from s in db.Tipo_Periodo where s.Tipo_Periodo_Id == IdPeriodo select s.Tipo_Periodo_Valor;
            return Json(Valorp, JsonRequestBehavior.AllowGet);
        }
        // POST: /Prestamos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Pagare,Capital,Interes,Plazo,Fecha_Prestamo,Tipo_Periodo_Id,Fecha_Primer_Pago,Forma_Pago_Id,Subdestino_Id,NIT,NOMBRE,SALARIO,Destino_Id,garantia_id,mivalor,codeudor_nit,nombre_codeudor,valor_credito,myselect,ValorPeriodo,ValorSeguro,ValorSeguroPorcentaje,ValDiasInt,difdias,fechadesembolso,costoAdicionalEnEltiempo,costoAdicionalAnticipado,costoAdicionalPrimeraCuota,costoAdicionalDividoEnElTiempo,ValorPorcentajeCostoAnticipado,ValorPorcentajeCostoEnCadaCuota,auxInteres")] ViewModelPrestamosYGarantias PrestamosYGarantias, FormCollection coll)
        {

            #region ANOTACIONES
            //El valor del seguro y el costo de administración
            #endregion

            string auxCostoADET = coll["auxCostoAdicionalDividoEnElTiempo"].ToString();
            string auxCostoAET = coll["auxCostoAdicionalEnEltiempo"].ToString();
            string auxValorPCA = coll["auxValorPorcentajeCostoAnticipado"].ToString();
            int costoADET = 0, costoAET = 0;
            decimal valorPCA=0;
            if(auxCostoADET!=null && auxCostoADET != "") { costoADET = Convert.ToInt32(auxCostoADET); }
            if(auxCostoAET != null && auxCostoAET != "") { costoAET = Convert.ToInt32(auxCostoAET); }
            if(auxValorPCA != null && auxValorPCA != "") { valorPCA = Convert.ToDecimal(auxValorPCA)/10; }

            int periodo = (int)PrestamosYGarantias.ValorPeriodo / 30;//calcula la periodicidad del crédito (mensual,trimestral,semestral...)

            PrestamosYGarantias.auxInteres = PrestamosYGarantias.auxInteres.Replace(".", ",");
            var VPrestamos = new Prestamos()
            {
                Pagare = PrestamosYGarantias.Pagare,
                Capital = PrestamosYGarantias.Capital,
                Interes = Convert.ToDecimal(PrestamosYGarantias.auxInteres),
                Plazo = PrestamosYGarantias.Plazo,
                Fecha_Prestamo = Convert.ToDateTime(PrestamosYGarantias.Fecha_Prestamo),
                Tipo_Periodo_Id = PrestamosYGarantias.Tipo_Periodo_Id,
                Fecha_Primer_Pago = PrestamosYGarantias.Fecha_Primer_Pago,
                Forma_Pago_Id = PrestamosYGarantias.Forma_Pago_Id,
                Subdestino_Id = PrestamosYGarantias.Subdestino_Id,
                NIT = PrestamosYGarantias.NIT,
                NOMBRE = PrestamosYGarantias.NOMBRE,
                SALARIO = PrestamosYGarantias.SALARIO,
                Destino_Id = PrestamosYGarantias.Destino_Id,
                myselect = PrestamosYGarantias.myselect,
                ValorPeriodo = PrestamosYGarantias.ValorPeriodo,
                ValorSeguro =(PrestamosYGarantias.Destino_Id==2)? (Convert.ToInt32(PrestamosYGarantias.Capital * decimal.Divide((decimal)0.059, 100)) * periodo):0,//Convert.ToInt32((costoAET + costoADET)/PrestamosYGarantias.Plazo),
                ValorSeguroPorcentaje = PrestamosYGarantias.ValorSeguroPorcentaje,
                ValDiasInt = PrestamosYGarantias.ValDiasInt,
                difdias = PrestamosYGarantias.difdias,
                fechadesembolso = Convert.ToDateTime( PrestamosYGarantias.fechadesembolso),
                costoAdicionalEnEltiempo = costoAET,
                costoAdicionalAnticipado = PrestamosYGarantias.costoAdicionalAnticipado,
                costoAdicionalPrimeraCuota = PrestamosYGarantias.costoAdicionalPrimeraCuota,
                costoAdicionalDividoEnElTiempo = costoADET,
                ValorPorcentajeCostoAnticipado = valorPCA,
                ValorPorcentajeCostoEnCadaCuota = PrestamosYGarantias.ValorPorcentajeCostoEnCadaCuota,
                CtoAdmon = (PrestamosYGarantias.Destino_Id==2)? GetCostoAdministracion((int)PrestamosYGarantias.Capital,(int)PrestamosYGarantias.Plazo, periodo) : 0

        };
                db.Prestamos.Add(VPrestamos);

                var VGarantiasCreditos = new GarantiasCreditos()
                {
                    garantia_id = PrestamosYGarantias.garantia_id,
                    Real_Valor = PrestamosYGarantias.mivalor,
                    pagare = PrestamosYGarantias.Pagare,
                    codeudor_nit = unchecked((int)PrestamosYGarantias.codeudor_nit),
                    nombre_codeudor = PrestamosYGarantias.nombre_codeudor,
                    valor_credito = unchecked((int)PrestamosYGarantias.Capital)
                };

                db.GarantiasCreditos.Add(VGarantiasCreditos);
                db.SaveChanges();
            /*
                var consecutivo = db.CConsecutivos.FirstOrDefault(j => j.idDestino == PrestamosYGarantias.Destino_Id & j.estado == true);
                consecutivo.consecutivoPagareActual = consecutivo.consecutivoPagareActual + 1;
                db.SaveChanges();
            */
                //return RedirectToAction("Index");
            

           /* if (ModelState.IsValid)
            {
                db.Prestamos.Add(prestamos);
                db.SaveChanges();

                var consecutivo = db.CConsecutivos.FirstOrDefault(x => x.idDestino == prestamos.Destino_Id);
                consecutivo.consecutivoPagareActual = consecutivo.consecutivoPagareActual + 1;
                db.SaveChanges();
                return RedirectToAction("Index");          
            }
            */
            ViewBag.Forma_Pago_Id = new SelectList(db.Forma_Pago, "Forma_Pago_Id", "Forma_Pago_Descripcion", PrestamosYGarantias.Forma_Pago_Id);
            ViewBag.Tipo_Periodo_Id = new SelectList(db.Tipo_Periodo, "Tipo_Periodo_Id", "Tipo_Periodo_Descripcion", PrestamosYGarantias.Tipo_Periodo_Id);

            //lista de Codeudores
            List<SelectListItem> items3C = new List<SelectListItem>();   // Creo una lista
            items3C.Add(new SelectListItem { Text = "Seleccione Codeudor", Value = "" });
            IList<Tercero> lista4C = db.Terceros.ToList();// extraigo los elementos desde la DB

            var test = from s in lista4C where s.ESCODEUDOR == true select s;

            foreach (var item in lista4C)		// recorro los elementos de la db
            {
                items3C.Add(new SelectListItem { Text = item.NOMBRE + " | " + item.ESCODEUDOR, Value = item.NIT.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.NIT = items3C;

            //lista de garantias
            List<SelectListItem> items3 = new List<SelectListItem>();   // Creo una lista
            items3.Add(new SelectListItem { Text = "Seleccione Garantia", Value = "" });
            IList<Garantias> lista4 = db.Garantias.ToList();// extraigo los elementos desde la DB

            //IList<Garantias> lista4 = db.Garantias.Where(m => m.Garantias_Id == 1 && m.Garantias_Codeudor == true).ToList();
            foreach (var item in lista4)		// recorro los elementos de la db
            {
                //items3.Add(new SelectListItem { Text = item.Garantias_Descripcion + " | " + item.Garantias_Codeudor, Value = item.Garantias_Id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                items3.Add(new SelectListItem { Text = item.Garantias_Descripcion, Value = item.Garantias_Id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Garantia_Id = items3;

            //lista de Lineas
            List<SelectListItem> Lineas = new List<SelectListItem>();   // Creo una lista
            Lineas.Add(new SelectListItem { Text = "Seleccione Una Linea", Value = "" });
            //IList<Lineas> ListaLineas = db.Lineas.ToList();// extraigo los elementos desde la DB
            IList<Lineas> ListaLineas = (from l in db.Lineas where l.Lineas_Activo == true select l).ToList();

            foreach (var item in ListaLineas)		// recorro los elementos de la db
            {

                Lineas.Add(new SelectListItem { Text = item.Lineas_Descripcion, Value = item.Lineas_Id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Lineas_Id = Lineas;
            //return View(PrestamosYGarantias);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarGarantia([Bind(Include = "id,garantia_id,valor,pagare,codeudor_nit,nombre_codeudor,valor_credito")] GarantiasCreditos GarantiasCreditos)
        {
            if (ModelState.IsValid)
            {
                db.GarantiasCreditos.Add(GarantiasCreditos);
                db.SaveChanges();
            }

            return View("Index");
        }

        public FileResult Imagen()
        {
            var rutas = System.AppDomain.CurrentDomain.BaseDirectory + "File/AUTORIZACION.docx";
            var ex = ".docx";
            return File(rutas, "aplication/" + ex, "AutorizacionDataCredito" + ex);
        }
        // GET: /Prestamos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prestamos prestamos = db.Prestamos.Find(id);
            if (prestamos == null)
            {
                return HttpNotFound();
            }

            ViewBag.Forma_Pago_Id = new SelectList(db.Forma_Pago, "Forma_Pago_Id", "Forma_Pago_Descripcion", prestamos.Forma_Pago_Id);
            ViewBag.Tipo_Periodo_Id = new SelectList(db.Tipo_Periodo, "Tipo_Periodo_Id", "Tipo_Periodo_Descripcion", prestamos.Tipo_Periodo_Id);
            return View(prestamos);
        }

        // POST: /Prestamos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Capital,Interes,Plazo,ClientesId,Fecha_Prestamo,Tipo_Periodo_Id,Fecha_Primer_Pago,Valor_Garantia,Forma_Pago_Id,Creditos_Id")] Prestamos prestamos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(prestamos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Forma_Pago_Id = new SelectList(db.Forma_Pago, "Forma_Pago_Id", "Forma_Pago_Descripcion", prestamos.Forma_Pago_Id);
            ViewBag.Tipo_Periodo_Id = new SelectList(db.Tipo_Periodo, "Tipo_Periodo_Id", "Tipo_Periodo_Descripcion", prestamos.Tipo_Periodo_Id);
            return View(prestamos);
        }

        // GET: /Prestamos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prestamos prestamos = db.Prestamos.Find(id);
            if (prestamos == null)
            {
                return HttpNotFound();
            }
            return View(prestamos);
        }

        // POST: /Prestamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prestamos prestamos = db.Prestamos.Find(id);
            db.Prestamos.Remove(prestamos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public int GetCostoAdministracion(int capital,int plazo,int periodo)
        {
            int CtoAdmon = (Convert.ToInt32(capital*0.01)/plazo)*periodo;
            //if (capital >= 0 && capital <= 10000000) { CtoAdmon = (Convert.ToInt32(capital * 0.015) / plazo) * periodo; }
            //else if (capital > 10000000 && capital <= 15000000) { CtoAdmon = (Convert.ToInt32(capital * 0.012) / plazo) * periodo; }
            //else { CtoAdmon = (Convert.ToInt32(capital * 0.01) / plazo) * periodo; }

            return CtoAdmon;
        }

        [HttpPost]
        public JsonResult GetAmortizacion(int seleccion, int capital, string interes, int plazo, DateTime FechaPrestamo, string VPS, int costoAdicionalPrimeraCuota, int costoAdicionalDividoEnElTiempo,
            string ValorPorcentajeCostoAnticipado, string ValorPorcentajeCostoEnCadaCuota, int costoAdicionalEnEltiempo, int costoAdicionalAnticipado, int ValDiasInt, int Periodo,int IdDestino)
        {

            Periodo = Periodo / 30;

            //obtenemos costo de administracion
            int CtoAdmon = (IdDestino==2) ? GetCostoAdministracion(capital, plazo, Periodo) : 0; //se agrega condición sobre el destino, si es diferente a destino de crédito ordinario su valor debe ser cero.


            //VPS: valor porcentaje seguro
            decimal InteresAux = Convert.ToDecimal(interes);
            double VPCECC = Convert.ToDouble(ValorPorcentajeCostoEnCadaCuota);//valor porcentaje costo en cada cuota
            double VPCA = Convert.ToDouble(ValorPorcentajeCostoAnticipado);//valor porcentaje costo anticipado

            double valorInteres = (Convert.ToDouble(interes) / 1000)*Periodo;
            double porcentajeSeguro = Convert.ToDouble(VPS);
            double ValorPorcentajeParaTabla = 0;
            if (porcentajeSeguro != 0)
            {
                porcentajeSeguro = porcentajeSeguro / 100;
                ValorPorcentajeParaTabla = capital * porcentajeSeguro;
            }
            else
            {
                porcentajeSeguro = 1;
                ValorPorcentajeParaTabla = 0;
            }

            if (seleccion == 2 && ValDiasInt != 0)
            {
                costoAdicionalEnEltiempo = costoAdicionalEnEltiempo + (ValDiasInt / plazo);
            }
            else if (seleccion == 3)
            {
                costoAdicionalPrimeraCuota = costoAdicionalPrimeraCuota + ValDiasInt;
            }

            //int valorCostoFijo = ((Convert.ToInt32(costoAdicionalEnEltiempo + costoAdicionalPrimeraCuota + costoAdicionalDividoEnElTiempo)) / 12) * plazo;
            int valorCostoFijo = costoAdicionalEnEltiempo;
            int valorCostoFijoCuota = (IdDestino==2) ? (Convert.ToInt32(capital * decimal.Divide((decimal)0.059,100))*Periodo) : 0;//(valorCostoFijo / plazo)*Periodo;


            double cuota = (capital * (valorInteres / (1 - Math.Pow(1 + valorInteres, -(plazo/Periodo)))));
            double abonoInteres = capital * valorInteres;
            double abonoCapital = cuota - abonoInteres;

            int valorCuota = 0;
            valorCuota = Convert.ToInt32(abonoCapital + abonoInteres + costoAdicionalPrimeraCuota + costoAdicionalDividoEnElTiempo + ValorPorcentajeParaTabla+valorCostoFijoCuota+CtoAdmon);
            int valorCuotaSinCostoAdicionalPrimeraCuota = Convert.ToInt32(abonoCapital + abonoInteres + costoAdicionalEnEltiempo + costoAdicionalDividoEnElTiempo + porcentajeSeguro);
            int ValorCostoFijoSinCostoPrimeraCuotaSinInteresAnticipado = costoAdicionalEnEltiempo + costoAdicionalDividoEnElTiempo;
            double saldoCapital = 0;
            saldoCapital = capital - abonoCapital;

            var list = new List<Array>();
            double interesMensual = 0;
            double capitalAux = capital;


            int J = 1;
            for (int i = Periodo; i <= plazo; i+=Periodo)
            {
                interesMensual = Math.Round(valorInteres * capital, 2);
                capitalAux = Math.Round(capital - cuota + interesMensual, 2);
                FechaPrestamo = FechaPrestamo.AddMonths(Periodo);

                string[] array = new string[8];
                array[0] = J.ToString();
                array[1] = FechaPrestamo.ToString("dd/MM/yyyy");
                array[2] = valorCuota.ToString("N2", formato);
                array[3] = (abonoCapital).ToString("N2", formato);
                array[4] = abonoInteres.ToString("N2", formato);
                array[5] = saldoCapital.ToString("N2", formato);
                array[6] = valorCostoFijoCuota.ToString("N2", formato);
                //array[7] = ValorPorcentajeParaTabla.ToString("N2", formato);
                array[7] = CtoAdmon.ToString("N2", formato);

                abonoInteres = saldoCapital * valorInteres;
                abonoCapital = cuota - abonoInteres;
                saldoCapital = saldoCapital - abonoCapital;
                
                if (ValorPorcentajeParaTabla != 0)
                {
                    ValorPorcentajeParaTabla = saldoCapital - porcentajeSeguro;
                }
                valorCuota = Convert.ToInt32(abonoCapital + abonoInteres + ValorPorcentajeParaTabla+valorCostoFijoCuota+CtoAdmon);

                list.Add(array);
                J++;
            }


            return new JsonResult { Data = new { status = true,list } };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
