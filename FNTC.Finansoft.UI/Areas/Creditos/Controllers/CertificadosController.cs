using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Certificados;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Creditos.Controllers
{
    public class CertificadosController : Controller
    {
        AccountingContext db = new AccountingContext();
        public ActionResult Index()
        {
            //inicio select list para terceros que ya acabaron credito
            List<SelectListItem> terceros = new List<SelectListItem>();
            terceros.Add(new SelectListItem { Text = "Documento / Pagaré", Value = "0" });
            IList<HistorialCreditos> listadoTerceros = db.HistorialCreditos.Where(x => x.saldoCapital == 0).ToList();

            foreach (var item in listadoTerceros)		// recorro los elementos de la db
            {
                var ter = db.Terceros.Find(item.NIT);
                var nom = "";
                if (ter != null) { nom = ter.NOMBRE; }
                terceros.Add(new SelectListItem { Text = item.pagare + " | " + item.NIT +" | "+nom, Value = item.id.ToString() });  // agrego los elementos de la db a la primera lista que cree

            }
            //fin 

            //inicio select list para terceros que tienen crédito
            List<SelectListItem> terceros2 = new List<SelectListItem>();
            terceros2.Add(new SelectListItem { Text = "Documento / Pagaré", Value = "0" });
            IList<BCreditos> listadoTerceros2 = db.Creditos.ToList();


            foreach (var item in listadoTerceros2)		// recorro los elementos de la db
            {
                var ter = db.Terceros.Find(item.Creditos_Cedula);
                var nom = "";
                if (ter != null) { nom = ter.NOMBRE; }
                terceros2.Add(new SelectListItem { Text = item.Creditos_Cedula + " | " + nom, Value = item.Creditos_Cedula });  // agrego los elementos de la db a la primera lista que cree

            }
            //fin 

            //inicio select list para lineas
            List<SelectListItem> lineas = new List<SelectListItem>();
            lineas.Add(new SelectListItem { Text = "Línea", Value = "0" });
            IList<Lineas> listadoLineas = db.Lineas.ToList();


            foreach (var item in listadoLineas)		// recorro los elementos de la db
            {
                lineas.Add(new SelectListItem { Text = item.Lineas_Descripcion, Value = item.Lineas_Id.ToString() });  // agrego los elementos de la db a la primera lista que cree
            }
            //fin 


            ViewBag.terceros = terceros;
            ViewBag.terceros2 = terceros2;
            ViewBag.lineas = lineas;

            return View();
        }

       

        public ActionResult pazYsalvo(int id)
        {
            var HC = db.HistorialCreditos.Find(id);
            var dataTercero = db.Terceros.Where(x => x.NIT == HC.NIT).FirstOrDefault();
            var ciudad = "";
            var dpto = "";
            var m = db.Municipio.Where(x => x.Id_muni == dataTercero.MUNICIPIO).FirstOrDefault();
            var d = db.Departamento.Where(x => x.Id_dep == m.Dep_muni).FirstOrDefault();
            if(m != null) { ciudad = m.Nom_muni; }
            if (d != null) { dpto = d.Nom_dep; }

            string dia = enletras((Convert.ToInt32(DateTime.Now.ToString("dd"))).ToString());
            string anio = enletras(DateTime.Now.ToString("yyyy")).ToString();

            ViewBag.nombre = dataTercero.NOMBRE1 + " " + dataTercero.NOMBRE2 + " " + dataTercero.APELLIDO1 + " " + dataTercero.APELLIDO2;
            ViewBag.cedula = dataTercero.NIT;
            ViewBag.ciudad = ciudad;
            ViewBag.dpto = dpto;
            ViewBag.pagare = HC.pagare;
            ViewBag.dia = Convert.ToInt32(DateTime.Now.ToString("dd"));
            ViewBag.diaL = dia.ToLower();
            ViewBag.mes = DateTime.Now.ToString("MMMM",CultureInfo.CreateSpecificCulture("es-CO"));
            ViewBag.anio = Convert.ToInt32(DateTime.Now.ToString("yyyy"));
            ViewBag.anioL = anio.ToLower();
            return View("viewpazYsalvo");
           // return View();
        }

        public ActionResult liquidacion(string id, int idd)
        {
            //List<Fund> fundList = db.Funds.ToList();
            //ViewBag.Funds = fundList;
            DateTime oldFech = DateTime.Now;
            string cedula = "";
            

            var ListLiquidacion = new List<ViewModelLiquidacion>();
            var prestamos = db.Creditos.Where(x => x.Creditos_Cedula == id && x.Lineas_Id == idd).ToList();

            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;

            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";

            foreach (var item in prestamos)
            {
                var pagare = "";
                long salIni = 0;
                decimal saldoA = 0;
                int valCuota = 0;
                decimal intPagados = 0;

                pagare = item.Pagare;
                salIni = item.Capital;

                var sal = db.HistorialCreditos.Where(x => x.pagare == item.Pagare).OrderByDescending(b => b.fecha).FirstOrDefault();
                if(sal != null) {
                    saldoA = sal.saldoCapital;
                    decimal interes = (from x in db.HistorialCreditos
                                       where x.pagare == item.Pagare
                                       select x.AbonoInteresCorriente).Sum();
                    intPagados = interes;

                }
                
                var cuota = db.Amortizaciones.Where(x => x.pagare == item.Pagare).FirstOrDefault();
                valCuota = Convert.ToInt32(cuota.valorCuota);
                

                var f = db.Prestamos.Where(x => x.Pagare == item.Pagare).FirstOrDefault();
                if(f.fechadesembolso < oldFech) { oldFech = f.fechadesembolso; }

                cedula = item.Creditos_Cedula;
                

                //agregando datos a viewmodel
                var c = new ViewModelLiquidacion();

                c.pagare = pagare;
                c.saldoInicial = salIni.ToString("N0", formato); 
                c.saldoA = saldoA.ToString("N", formato);
                c.valCuota = valCuota.ToString("N0", formato);
                c.intPagados = intPagados.ToString("N", formato);
                c.fechaDesembolso = f.fechadesembolso.ToString("yyyy/MM/dd");

                ListLiquidacion.Add(c);
                //.....


            }

            

            var nombre = db.Terceros.Where(x => x.NIT == id).FirstOrDefault();
            string fechNow = DateTime.Now.ToString("yyyy/MM/dd");

            //zona de ViewBags
            ViewBag.Listado = ListLiquidacion;
            ViewBag.oldFech = oldFech.ToString("yyyy/MM/dd");
            ViewBag.nowFech = fechNow;
            ViewBag.documento = cedula;
            ViewBag.nombre = nombre.NOMBRE1+" "+nombre.NOMBRE2+" "+nombre.APELLIDO1+" "+nombre.APELLIDO2;
            



            return View("liquidacion");
            // return View();
        }

        [HttpPost]
        public JsonResult verificaCredito(string id,int idd)
        {
            //int idd = Convert.ToInt32(id);
            int prestamos = db.Creditos.Where(x => x.Creditos_Cedula == id && x.Lineas_Id == idd).Count();
            if (prestamos > 0)
            {
                return new JsonResult { Data = new { status = true } };

            }
            else
            {
                //return Json(1, JsonRequestBehavior.AllowGet);
                return new JsonResult { Data = new { status = false } };
            }

        }


        public string enletras(string num)
        {
            string res, dec = "";
            Int64 entero;
            int decimales;
            double nro;

            try
            {
                nro = Convert.ToDouble(num);
            }
            catch

            {
                return "";
            }

            entero = Convert.ToInt64(Math.Truncate(nro));
            decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
            if (decimales > 0)
            {
                dec = " CON " + decimales.ToString() + "/100";
            }

            res = toText(Convert.ToDouble(entero)) + dec;
            return res;
        }

        private string toText(double value)
        {
            string Num2Text = "";
            value = Math.Truncate(value);
            if (value == 0) Num2Text = "CERO";
            else if (value == 1) Num2Text = "UNO";
            else if (value == 2) Num2Text = "DOS";
            else if (value == 3) Num2Text = "TRES";
            else if (value == 4) Num2Text = "CUATRO";
            else if (value == 5) Num2Text = "CINCO";
            else if (value == 6) Num2Text = "SEIS";
            else if (value == 7) Num2Text = "SIETE";
            else if (value == 8) Num2Text = "OCHO";
            else if (value == 9) Num2Text = "NUEVE";
            else if (value == 10) Num2Text = "DIEZ";
            else if (value == 11) Num2Text = "ONCE";
            else if (value == 12) Num2Text = "DOCE";
            else if (value == 13) Num2Text = "TRECE";
            else if (value == 14) Num2Text = "CATORCE";
            else if (value == 15) Num2Text = "QUINCE";
            else if (value < 20) Num2Text = "DIECI" + toText(value - 10);
            else if (value == 20) Num2Text = "VEINTE";
            else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);
            else if (value == 30) Num2Text = "TREINTA";
            else if (value == 40) Num2Text = "CUARENTA";
            else if (value == 50) Num2Text = "CINCUENTA";
            else if (value == 60) Num2Text = "SESENTA";
            else if (value == 70) Num2Text = "SETENTA";
            else if (value == 80) Num2Text = "OCHENTA";
            else if (value == 90) Num2Text = "NOVENTA";
            else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);
            else if (value == 100) Num2Text = "CIEN";
            else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);
            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";
            else if (value == 500) Num2Text = "QUINIENTOS";
            else if (value == 700) Num2Text = "SETECIENTOS";
            else if (value == 900) Num2Text = "NOVECIENTOS";
            else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);
            else if (value == 1000) Num2Text = "MIL";
            else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);
            else if (value < 1000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";
                if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
            }

            else if (value == 1000000) Num2Text = "UN MILLON";
            else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);
            else if (value < 1000000000000)
            {
                Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";
                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
            }

            else if (value == 1000000000000) Num2Text = "UN BILLON";
            else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            else
            {
                Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";
                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
            }
            return Num2Text;

        }
    }
}