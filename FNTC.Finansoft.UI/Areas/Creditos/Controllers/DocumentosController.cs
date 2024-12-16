
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using FNTC.Finansoft.Accounting.BLL.FabricaCreditosBll;

using FNTC.Finansoft.Accounting.DTO.Shared;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using System.Globalization;

namespace FNTC.Finansoft.UI.Areas.Creditos.Controllers
{
    [Authorize]
    public class DocumentosController : Controller
    {

        private AccountingContext db = new AccountingContext();
        // GET: Creditos/Documentos
        public ActionResult DocumentacionACP()

        {
            var useractual = User.Identity.Name;
            ViewBag.User = useractual;
            
            return View(db.Prestamos.ToList());
        }
        public ActionResult DocumentacionAdvertencia() 

        {
            var useractual = User.Identity.Name;
            ViewBag.User = useractual;

            return View(db.Prestamos.ToList());
        }

        // GET: Creditos/Documentos/Details/5
        public ActionResult Autorizacion()
        {
            return View("AutorizacionDescuentoLibranza");
        }

        // GET: Creditos/Documentos/Create
        public ActionResult Carta(string id)
        {
            var datos = (from pc in db.Prestamos where pc.Pagare == id select pc).Single(); //capura datos de la tabla Prestamos....busqueda por pagaré
            long capital = datos.Capital;


            var cedula = (from pc in db.Prestamos where pc.Pagare == id select pc).Single();
            var nombres = (from pc in db.Terceros where pc.NIT == cedula.NIT select pc).Single();

            var nom1 = nombres.NOMBRE1;
            var nom2 = nombres.NOMBRE2;
            var ape1 = nombres.APELLIDO1;
            var ape2 = nombres.APELLIDO2;
            string ced = nombres.NIT;

            var NombreCompleto = nom1 + " " + nom2 + " " + ape1 + " " + ape2;

            DateTime today = DateTime.Today;
            var d=today.ToString("dd");
            var a = today.ToString("yyyy");

            string m = today.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-CO"));


            ViewBag.InfoCarta = id;
            ViewBag.Nombre = NombreCompleto;
            ViewBag.dia = d;
            ViewBag.mes = m.ToUpper(); 
            ViewBag.anio = a;
            ViewBag.NIT = ced;
            ViewBag.capital = capital;

            return View("CartaPagare");
        }


        public ActionResult Pagare(string id)
        {

            var datos = (from pc in db.Prestamos where pc.Pagare == id select pc).Single(); //capura datos de la tabla Prestamos....busqueda por pagaré
            long capital = datos.Capital; 

            var datosDestino = (from pc in db.Destinos where pc.Destino_Id == datos.Destino_Id select pc).Single(); //capura datos de la tabla Destinos....busqueda por Id del destino
            decimal tasa = datosDestino.Destino_Tasa_Minima;

            var datosTercero = (from pc in db.Terceros where pc.NIT == datos.NIT select pc).Single();
            string nom1 = datosTercero.NOMBRE1;
            string nom2 = datosTercero.NOMBRE2;
            string ape1 = datosTercero.APELLIDO1;
            string ape2 = datosTercero.APELLIDO2;
            string ced = datosTercero.NIT;
            int codMunicipio = Convert.ToInt32(datosTercero.MUNICIPIO);

            
            string NombreCompleto = nom1 + " " + nom2 + " " + ape1 + " " + ape2;
            string NombreApellido = nom1 + " " + ape1;

            var datosMunicipio = (from pc in db.Municipio where pc.Id_muni == codMunicipio select pc).Single();
            string nomMuni = datosMunicipio.Nom_muni;

            var datosDepto = (from pc in db.Departamento where pc.Id_dep == datosMunicipio.Dep_muni select pc).Single();
            string nomDpto = datosDepto.Nom_dep;

            var datosPais = (from pc in db.Pais where pc.Id_pais == datosDepto.Pais_dep select pc).Single();
            string nompais = datosPais.Nom_pais;
            string cuota = "0";

            var datosAmortizacionCount = (from pc in db.Amortizaciones where pc.pagare == id select pc).Count();
            if (datosAmortizacionCount != 0)
            {
                var datosAmortizacion = (from pc in db.Amortizaciones where pc.pagare == id select pc).FirstOrDefault();
                cuota = datosAmortizacion.valorCuota;
            }           

            DateTime today = DateTime.Today;
            var d = today.ToString("dd");
            var a = today.ToString("yyyy");

            string m = today.ToString("MMMM", CultureInfo.CreateSpecificCulture("es-CO"));

            ViewBag.dia = d;
            ViewBag.mes = m.ToUpper();
            ViewBag.anio = a;

            ViewBag.capital = capital;
            ViewBag.tasa = tasa;
            ViewBag.nombres = NombreCompleto;
            ViewBag.nombresApellido = NombreApellido;
            ViewBag.nomMunicipio = nomMuni;
            ViewBag.nomDpto = nomDpto;
            ViewBag.nomPais = nompais.ToUpper();
            ViewBag.cuota = cuota;
            ViewBag.NIT = ced;
            ViewBag.pagare = datos.Pagare;

            var TokenAsociado = new ConfiguracionBll().ObtenerTokenAsociado(id);
            var NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoPrestamos(id);

            var TokenCodeudor = new ConfiguracionBll().ObtenerTokenCodeudor(id);
            var NombreCodeudor = new ConfiguracionBll().ObtenerNombreCodeudor(id);

            ViewBag.TokenAsociado = new ConfiguracionBll().ObtenerTokenAsociado(id);
            ViewBag.NombreAsociado = new ConfiguracionBll().ObtenerNombreAsociadoPrestamos(id);

            ViewBag.TokenCodeudor = new ConfiguracionBll().ObtenerTokenCodeudor(id);
            ViewBag.NombreCodeudor = new ConfiguracionBll().ObtenerNombreCodeudor(id);

            //if (TokenAsociado == null && NombreAsociado == null && TokenCodeudor == null && NombreCodeudor == null)
            //{
            //    return RedirectToAction("DocumentacionAdvertencia");
            //}
            //else
            //{
                return View("Pagare");

           //
           //}


        }

        // POST: Creditos/Documentos/Create
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

        // GET: Creditos/Documentos/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Creditos/Documentos/Edit/5
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

        // GET: Creditos/Documentos/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Creditos/Documentos/Delete/5
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
    }
}

//public ActionResult Index()
//{          
//    //lista de terceros(responsable)
//    List<SelectListItem> Prestamos = new List<SelectListItem>();   // Creo una lista
//    Prestamos.Add(new SelectListItem { Text = "Seleccione Un Pagaré", Value = "" });
//    IList<Prestamos> listado = db.Prestamos.ToList();


//    foreach (var item in listado)		// recorro los elementos de la db
//    {
//        Prestamos.Add(new SelectListItem { Text = item.NIT + " | " + item.Pagare, Value = item.Pagare });  // agrego los elementos de la db a la primera lista que cree
//        //text: el texto que se muestra
//        //value: el valor interno del dropdown
//    }

//    ViewBag.InfoPagares = Prestamos;



//    return View("MenuDocumentos");
//}