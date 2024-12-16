using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DAL.Nomina;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Nomina;

namespace FNTC.Finansoft.UI.Areas.Nomina.Controllers
{
    public class ArchivoPlanosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        public ArchivoPlanosController()
        {
            ViewBag.TipoDato = new List<SelectListItem>()
            {
                new SelectListItem(){Value = "CARACTER", Text = "CARACTER"},
                new SelectListItem(){Value = "FECHA", Text = "FECHA"}
            };
            ViewBag.Alineación = new List<SelectListItem>()
            {
                new SelectListItem(){Value = "IZQUIERDA", Text = "IZQUIERDA"},
                new SelectListItem(){Value = "DERECHA", Text = "DERECHA"},
                new SelectListItem(){Value = "CENTRO", Text = "CENTRO"},
               
            };
            ViewBag.Relleno = new List<SelectListItem>()
            {
                new SelectListItem(){Value = "BLANCOS", Text = "BLANCOS"},
                new SelectListItem(){Value = "CEROS", Text = "CEROS"},
          
            };
            ViewBag.ValPredeterminado = new List<SelectListItem>()
            {
                new SelectListItem(){Value = "SI", Text = "SI"},
                new SelectListItem(){Value = "NO", Text = "NO"}
            };

        }

        // GET: Nomina/ArchivoPlanos
        public ActionResult Index()
        {
            /*return View();*/
            // return PartialView(db.ArchivoPlano.OrderBy(p => p.ORDEN).ToList());
            return PartialView(db.ArchivoPlano.Include(p => p.ClaseDePlanos1).Include(p => p.TipoDeCampo).OrderBy(p => p.ORDEN).ToList());


        }

        // GET: Nomina/ArchivoPlanos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArchivoPlano archivoPlano = db.ArchivoPlano.Find(id);
            if (archivoPlano == null)
            {
                return HttpNotFound();
            }
            return PartialView(archivoPlano);
        }

        // GET: Nomina/ArchivoPlanos/Create
        public ActionResult Create()
        {

            var claseplanos = new FNTC.Finansoft.Accounting.BLL.ClaseDePlanos.ClaseDePlanosBLL().GetClaseDePlano();
            ViewBag.CP = claseplanos;
            var tipodecampo = new FNTC.Finansoft.Accounting.BLL.TipoDeCampos.TipoDeCamposBLL().GetTipoDeCampos();
            ViewBag.TC = tipodecampo;
            return View();
        }

        // POST: Nomina/ArchivoPlanos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CLASEPLANO,TIPCAMPO,CONCEPTO,TIPDATO,LONGITUD,ALINEACION,RELLENO,VALPREDETERINADO,DIGVALOR,NUMDECIMALES,ORDEN")] ArchivoPlano archivoPlano)
        {
            ViewBag.guardado = "N";
            if (ModelState.IsValid)
            {

                ArchivoPlanosDAL ctx = new ArchivoPlanosDAL();

                try
                {
                    ctx.CreateArchivosPlano(archivoPlano);
                    ViewBag.guardado = "S";

                }
                catch (Exception e)
                {
                    ViewBag.Error = e.Message;

                }

            }
            var claseplanos = new FNTC.Finansoft.Accounting.BLL.ClaseDePlanos.ClaseDePlanosBLL().GetClaseDePlano();
            ViewBag.CP = claseplanos;
            var tipodecampo = new FNTC.Finansoft.Accounting.BLL.TipoDeCampos.TipoDeCamposBLL().GetTipoDeCampos();
            ViewBag.TC = tipodecampo;
            return PartialView(archivoPlano);
        }


        //public JsonResult GetDatosPlano(string Plano)
        //{
        //    int Plano1 = Int32.Parse(Plano);

        //    var Extension = (from pc in db.ClaseDePlano where pc.ID == Plano1 select pc.EXTENSION).Single();
            
        //    List<string> codigos = new List<string>();
        //    codigos.Add(Extension.ToString());
        //    //codigos.Add(Tercero.NOMBRE.ToString());
        //    return Json(codigos, JsonRequestBehavior.AllowGet);
        //}

        // GET: Nomina/ArchivoPlanos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArchivoPlano archivoPlano = db.ArchivoPlano.Find(id);
            if (archivoPlano == null)
            {
                return HttpNotFound();
            }
            var claseplanos = new FNTC.Finansoft.Accounting.BLL.ClaseDePlanos.ClaseDePlanosBLL().GetClaseDePlano();
            ViewBag.CP = claseplanos;
            var tipodecampo = new FNTC.Finansoft.Accounting.BLL.TipoDeCampos.TipoDeCamposBLL().GetTipoDeCampos();
            ViewBag.TC = tipodecampo;
            return View(archivoPlano);

        }

        // POST: Nomina/ArchivoPlanos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ArchivoPlano archivoPlano)
        {
            if (ModelState.IsValid)
            {
                db.Entry(archivoPlano).State = EntityState.Modified;
                db.SaveChanges();

            }

            var claseplanos = new FNTC.Finansoft.Accounting.BLL.ClaseDePlanos.ClaseDePlanosBLL().GetClaseDePlano();
            ViewBag.CP = claseplanos;
            var tipodecampo = new FNTC.Finansoft.Accounting.BLL.TipoDeCampos.TipoDeCamposBLL().GetTipoDeCampos();
            ViewBag.TC = tipodecampo;
            return PartialView(archivoPlano);
        }

        // GET: Nomina/ArchivoPlanos/Delete/5
        public ActionResult Delete(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArchivoPlano archivoPlano = db.ArchivoPlano.Find(id);
            if (archivoPlano == null)
            {
                return HttpNotFound();
            }
            ViewBag.guardado = "N";
            return PartialView(archivoPlano);
        }

        // POST: Nomina/ArchivoPlanos/Delete/5
        [HttpPost]

        public ActionResult DeleteConfirmed(string id)
        {
            int id1 = 0;
            bool result = int.TryParse(id, out id1);
            ArchivoPlano archivoPlano = db.ArchivoPlano.Find(id1);
            db.ArchivoPlano.Remove(archivoPlano);
            db.SaveChanges();


            List<ArchivoPlano> lista = db.ArchivoPlano.Where(p => p.ORDEN > archivoPlano.ORDEN).OrderBy(p => p.ORDEN).ToList();
            foreach (ArchivoPlano obj in lista)
            {
                var orden = obj.ORDEN - 1;
                obj.ORDEN = (short)orden;
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.guardado = "S";
            return PartialView("Delete");
        }

        public ActionResult Move2(int? id, string action_)
        {///1ro se instancia el q se movió
            ArchivoPlano archivoPlano = db.ArchivoPlano.Find(id);
            var ordenM = archivoPlano.ORDEN;// se guarda el  orden para intercambiarlo con el registro q se va a reemplazar
            // 2do se verifica el nuevo orden
            var orden = 0;
            var max = db.ArchivoPlano.Max(p => p.ORDEN);
            if ((action_ == "Subir" && ordenM > 1) || (action_ == "Bajar" && ordenM < max))
            {
                if (action_ == "Subir")
                    orden = archivoPlano.ORDEN - 1;
                else
                    orden = archivoPlano.ORDEN + 1;
                // 3ro se trae el registro q esta en esa posicion 
                ArchivoPlano otroRegistro = db.ArchivoPlano.Where(p => p.ORDEN == orden).FirstOrDefault();


                // actualizar registros

                archivoPlano.ORDEN = (short)orden;
                db.Entry(archivoPlano).State = EntityState.Modified;
                db.SaveChanges();

                otroRegistro.ORDEN = (short)ordenM;
                db.Entry(archivoPlano).State = EntityState.Modified;
                db.SaveChanges();
            }


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

        private void Init(ref ArchivoPlano dto)
        {

            /*dto._claseplanos = new FNTC.Framework.Params.DAL.ParamsDAL()
                .GetParamValues("NOMBRE").Select(x => new SelectListItem() { Text = x.Valor, Value = x.Codigo}).ToList();
            dto._claseplanos = db.ClaseDePlano.Select(x => new SelectListItem() { Text = x.NOMBRE, Value = x.NOMBRE }).ToList();*/


        }
        /* public ActionResult GetClasePlanos()
         {
             List<ClaseDePlano> 
         }*/
    }
}
