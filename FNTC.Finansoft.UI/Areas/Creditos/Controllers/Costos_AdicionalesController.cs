
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.MCreditos;

namespace FNTC.Finansoft.UI.Areas.Creditos.Controllers
{
    public class Costos_AdicionalesController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: /Costos_Adicionales/
        public ActionResult Index()
        {
            List<ViewModelCostos> ViewModelCostos = new List<ViewModelCostos>();
            var ListaCostos = (from costo in db.Costos_Adicionales
                               join destinos in db.Destinos on costo.Destino_Id equals destinos.Destino_Id
                               join lineas in db.Lineas on costo.Lineas_Id equals lineas.Lineas_Id
                               join tipocosto in db.Tipo_Costo on costo.Tipo_Costo_Id equals tipocosto.Tipo_Costo_Id
                               select new {costo.CA_Id, destinos.Destino_Descripcion, costo.Cuenta_Cod, costo.CA_Nombre, costo.CA_Valor, costo.CA_Porcentaje, costo.CA_estado,
                               lineas.Lineas_Descripcion, tipocosto.Tipo_Costo_Descripcion}).ToList();
            foreach (var item in ListaCostos)
            {
                ViewModelCostos obj = new ViewModelCostos();
                obj.CA_Id = item.CA_Id;
                obj.Destino_Descripcion = item.Destino_Descripcion;
                obj.Cuenta_Cod = item.Cuenta_Cod;
                obj.CA_Nombre = item.CA_Nombre;
                obj.CA_Valor = item.CA_Valor;
                obj.CA_Porcentaje = item.CA_Porcentaje;
                obj.CA_estado = item.CA_estado;
                obj.Lineas_Descripcion = item.Lineas_Descripcion;
                obj.Tipo_Costo_Descripcion = item.Tipo_Costo_Descripcion;

                ViewModelCostos.Add(obj);
            }
            return View(ViewModelCostos);

        }

        // GET: /Costos_Adicionales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Costos_Adicionales costos_adicionales = db.Costos_Adicionales.Find(id);
            if (costos_adicionales == null)
            {
                return HttpNotFound();
            }
            return View(costos_adicionales);
        }

        // GET: /Costos_Adicionales/Create
        public ActionResult Create()
        {

            //lista de Lineas
            List<SelectListItem> Lineas = new List<SelectListItem>();   // Creo una lista
            Lineas.Add(new SelectListItem { Text = "Seleccione Una Linea", Value = "" });
           // IList<Lineas> ListaLineas = db.Lineas.ToList();// extraigo los elementos desde la DB
            IList<Lineas> ListaLineas = (from l in db.Lineas where l.Lineas_Activo == true select l).ToList();

            foreach (var item in ListaLineas)		// recorro los elementos de la db
            {

                Lineas.Add(new SelectListItem { Text = item.Lineas_Descripcion, Value = item.Lineas_Id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Lineas_Id = Lineas;

            ////lista de Destinos
            //List<SelectListItem> Destinos = new List<SelectListItem>();   // Creo una lista
            //Destinos.Add(new SelectListItem { Text = "Seleccione Un Destino", Value = "" });
            //IList<Destinos> ListaDestinos = db.Destinos.ToList();// extraigo los elementos desde la DB


            //foreach (var item in ListaDestinos)		// recorro los elementos de la db
            //{

            //    Destinos.Add(new SelectListItem { Text = item.Destino_Descripcion, Value = item.Destino_Id.ToString() });  // agrego los elementos de la db a la primera lista que cree
            //    //text: el texto que se muestra
            //    //value: el valor interno del dropdown
            //}

            //ViewBag.Destino_Id = Destinos;



            //lista de Cuentas
            List<SelectListItem> Cuentas = new List<SelectListItem>();   // Creo una lista
            Cuentas.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });
            IList<Cuentas> ListaCuentas = db.Cuentas.ToList();// extraigo los elementos desde la DB


            foreach (var item in ListaCuentas)		// recorro los elementos de la db
            {

                Cuentas.Add(new SelectListItem { Text = item.Cuenta_Descripcion, Value = item.Cuenta_Cod.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Cuenta_Cod = Cuentas;

            //lista de Tipos de Costo
            List<SelectListItem> TipoCosto = new List<SelectListItem>();   // Creo una lista
            TipoCosto.Add(new SelectListItem { Text = "Seleccione Un Tipo de Costo", Value = "" });
            IList<Tipo_Costo> ListaTipoCosto = db.Tipo_Costo.ToList();// extraigo los elementos desde la DB


            foreach (var item in ListaTipoCosto)		// recorro los elementos de la db
            {

                TipoCosto.Add(new SelectListItem { Text = item.Tipo_Costo_Descripcion, Value = item.Tipo_Costo_Id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Tipo_Costo_Id = TipoCosto;

            //lista de Incrementos
            List<SelectListItem> Incremento = new List<SelectListItem>();   // Creo una lista
            Incremento.Add(new SelectListItem { Text = "Seleccione Un Incremento", Value = "" });
            IList<Incrementa> ListaIncremento = db.Incrementa.ToList();// extraigo los elementos desde la DB


            foreach (var item in ListaIncremento)		// recorro los elementos de la db
            {

                Incremento.Add(new SelectListItem { Text = item.Incrementa_Descripcion, Value = item.Incrementa_Id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Incrementa_Id = Incremento;

            return View();
        }

        //metodo que filtra lineas de codeudores
        //public JsonResult GetCountries()
        // {
        //     var Lineas = db.Lineas.OrderBy(a => a.Lineas_Id)
        //                  .Select(c => new { Lineas_Id = c.Lineas_Id, Lineas_Descripcion = c.Lineas_Descripcion })
        //                  .ToList();
        //     return new JsonResult { Data = Lineas, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        // }

        public JsonResult GetStatesByCountryId(int CountryId)
        {
            //int Id = Convert.ToInt32(countryId);


            //var states = (from a in db.Destinos  where a.Lineas_Id == Id select a).ToList();
            //return Json(new { data = states });



            //lista de Destinos
            List<SelectListItem> Destinos = new List<SelectListItem>();   // Creo una lista
            Destinos.Add(new SelectListItem { Text = "Seleccione Un Destino", Value = "" });
            IList<Destinos> ListaDestinos = (from a in db.Destinos where a.Lineas_Id == CountryId && a.Destino_Activo==true select a).ToList();// extraigo los elementos desde la DB


            foreach (var item in ListaDestinos)		// recorro los elementos de la db
            {

                Destinos.Add(new SelectListItem { Text = item.Destino_Descripcion, Value = item.Destino_Id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            // var data = Destinos;
            // return Json(new SelectList(data, JsonRequestBehavior.AllowGet));
            // return Json(new { data = Destinos }, JsonRequestBehavior.AllowGet);
            // return Json(new { success, JsonRequestBehavior.AllowGet });
            return Json(Destinos, JsonRequestBehavior.AllowGet);


        }

        public JsonResult GetStatesBysubdestinoId(int SubId)
        {

            //lista de Destinos
            List<SelectListItem> SubDestinos = new List<SelectListItem>();   // Creo una lista
            SubDestinos.Add(new SelectListItem { Text = "Seleccione Un SubDestino", Value = "" });
            IList<SubDestinos> ListaDestinos = (from a in db.SubDestinos where a.Destino_Id == SubId select a).ToList();// extraigo los elementos desde la DB


            foreach (var item in ListaDestinos)		// recorro los elementos de la db
            {

                SubDestinos.Add(new SelectListItem { Text = item.Subdestino_Descripcion, Value = item.Subdestino_Id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }


            return Json(SubDestinos, JsonRequestBehavior.AllowGet);


        }








        // POST: /Costos_Adicionales/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CA_Id,Lineas_Id,Destino_Id,Cuenta_Cod,Tipo_Costo_Id,Incrementa_Id,CA_Nombre,CA_Valor,CA_estado,CA_Porcentaje")] Costos_Adicionales costos_adicionales)
        {
            //lista de Cuentas
            List<SelectListItem> Cuentas = new List<SelectListItem>();   // Creo una lista
            Cuentas.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });
            IList<Cuentas> ListaCuentas = db.Cuentas.ToList();// extraigo los elementos desde la DB


            foreach (var item in ListaCuentas)		// recorro los elementos de la db
            {

                Cuentas.Add(new SelectListItem { Text = item.Cuenta_Descripcion, Value = item.Cuenta_Cod.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Cuenta_Cod = Cuentas;

            //lista de Tipos de Costo
            List<SelectListItem> TipoCosto = new List<SelectListItem>();   // Creo una lista
            TipoCosto.Add(new SelectListItem { Text = "Seleccione Un Tipo de Costo", Value = "" });
            IList<Tipo_Costo> ListaTipoCosto = db.Tipo_Costo.ToList();// extraigo los elementos desde la DB


            foreach (var item in ListaTipoCosto)		// recorro los elementos de la db
            {

                TipoCosto.Add(new SelectListItem { Text = item.Tipo_Costo_Descripcion, Value = item.Tipo_Costo_Id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Tipo_Costo_Id = TipoCosto;

            //lista de Incrementos
            List<SelectListItem> Incremento = new List<SelectListItem>();   // Creo una lista
            Incremento.Add(new SelectListItem { Text = "Seleccione Un Incremento", Value = "" });
            IList<Incrementa> ListaIncremento = db.Incrementa.ToList();// extraigo los elementos desde la DB


            foreach (var item in ListaIncremento)		// recorro los elementos de la db
            {

                Incremento.Add(new SelectListItem { Text = item.Incrementa_Descripcion, Value = item.Incrementa_Id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Incrementa_Id = Incremento;

            //lista de Lineas
            List<SelectListItem> Lineas = new List<SelectListItem>();   // Creo una lista
            Lineas.Add(new SelectListItem { Text = "Seleccione Una Linea", Value = "" });
            // IList<Lineas> ListaLineas = db.Lineas.ToList();// extraigo los elementos desde la DB
            IList<Lineas> ListaLineas = (from l in db.Lineas where l.Lineas_Activo == true select l).ToList();

            foreach (var item in ListaLineas)		// recorro los elementos de la db
            {

                Lineas.Add(new SelectListItem { Text = item.Lineas_Descripcion, Value = item.Lineas_Id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Lineas_Id = Lineas;

            if (ModelState.IsValid)
            {
                db.Costos_Adicionales.Add(costos_adicionales);
                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(costos_adicionales);
        }


        //VISTA PARCIAL PARA EDITAR
        
        public ActionResult _Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Costos_Adicionales costos_adicionales = db.Costos_Adicionales.Find(id);

            if (costos_adicionales == null)
            {
                return HttpNotFound();
            }
            ViewBag.Lineas_Id = new SelectList((from l in db.Lineas where l.Lineas_Activo == true select l), "Lineas_Id", "Lineas_Descripcion", costos_adicionales.Lineas_Id);
            ViewBag.Destino_Id = new SelectList(db.Destinos, "Destino_Id", "Destino_Descripcion", costos_adicionales.Destino_Id);
            ViewBag.Cuenta_Cod = new SelectList(db.Cuentas, "Cuenta_Cod", "Cuenta_Descripcion", costos_adicionales.Cuenta_Cod);
            ViewBag.Tipo_Costo_Id = new SelectList(db.Tipo_Costo, "Tipo_Costo_Id", "Tipo_Costo_Descripcion", costos_adicionales.Tipo_Costo_Id);
            ViewBag.Incrementa_Id = new SelectList(db.Incrementa, "Incrementa_Id", "Incrementa_Descripcion", costos_adicionales.Incrementa_Id);
            return PartialView(costos_adicionales);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _Edit([Bind(Include = "CA_Id,Lineas_Id,Destino_Id,Cuenta_Cod,Tipo_Costo_Id,Incrementa_Id,CA_Nombre,CA_Valor,CA_estado,CA_Porcentaje")] Costos_Adicionales costos_adicionales)
        {
            if (ModelState.IsValid)
            {
                db.Entry(costos_adicionales).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(costos_adicionales);
        }
        //FIN VISTA PARCIAL EDITAR

        // GET: /Costos_Adicionales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Costos_Adicionales costos_adicionales = db.Costos_Adicionales.Find(id);
            
            if (costos_adicionales == null)
            {
                return HttpNotFound();
            }
            ViewBag.Lineas_Id = new SelectList(db.Lineas, "Lineas_Id", "Lineas_Descripcion", costos_adicionales.Lineas_Id);
            ViewBag.Destino_Id = new SelectList(db.Destinos, "Destino_Id", "Destino_Descripcion", costos_adicionales.Destino_Id);
            ViewBag.Cuenta_Cod = new SelectList(db.Cuentas, "Cuenta_Cod", "Cuenta_Descripcion", costos_adicionales.Cuenta_Cod);
            ViewBag.Tipo_Costo_Id = new SelectList(db.Tipo_Costo, "Tipo_Costo_Id", "Tipo_Costo_Descripcion", costos_adicionales.Tipo_Costo_Id);
            ViewBag.Incrementa_Id = new SelectList(db.Incrementa, "Incrementa_Id", "Incrementa_Descripcion", costos_adicionales.Incrementa_Id);
            return View(costos_adicionales);
        }

        // POST: /Costos_Adicionales/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CA_Id,Lineas_Id,Destino_Id,Cuenta_Cod,Tipo_Costo_Id,Incrementa_Id,CA_Nombre,CA_Valor,CA_estado,CA_Porcentaje")] Costos_Adicionales costos_adicionales)
        {
            if (ModelState.IsValid)
            {
                db.Entry(costos_adicionales).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(costos_adicionales);
        }

        // GET: /Costos_Adicionales/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Costos_Adicionales costos_adicionales = db.Costos_Adicionales.Find(id);
            if (costos_adicionales == null)
            {
                return HttpNotFound();
            }
            return View(costos_adicionales);
        }

        // POST: /Costos_Adicionales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Costos_Adicionales costos_adicionales = db.Costos_Adicionales.Find(id);
            db.Costos_Adicionales.Remove(costos_adicionales);
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
    }
}
