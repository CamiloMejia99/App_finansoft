using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Scoring;

namespace FNTC.Finansoft.UI.Areas.Scoring.Controllers
{
    [Authorize]
    public class VariablesController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Scoring/Variables
        public ActionResult Index()
        {
            return View(db.ScoringVariableAgencias.ToList());
        }

        // GET: Scoring/Variables/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringVariableAgencia scoringVariableAgencia = db.ScoringVariableAgencias.Find(id);
            if (scoringVariableAgencia == null)
            {
                return HttpNotFound();
            }
            return View(scoringVariableAgencia);
        }

        // GET: Scoring/Variables/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Scoring/Variables/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,IdAgencia,DescripcionAgencia,PuntajeAgencia")] ScoringVariableAgencia scoringVariableAgencia)
        {
            if (ModelState.IsValid)
            {
                db.ScoringVariableAgencias.Add(scoringVariableAgencia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scoringVariableAgencia);
        }

        // GET: Scoring/Variables/Edit/5
        public ActionResult Edit()
        {
            

            //ScoringVariableAgencia ScoringVariableAgencia = db.ScoringVariableAgencia.Find(ScoringVariableAgencia);

            //ScoringVariableAgencia ScoringVariableAgencia = db.ScoringVariableAgencias.Find(id);
            using (var db = new AccountingContext())
            {
                var ScoringVariables = new ViewModelScoringVariables();
                {
                ScoringVariables.ScoringVariableAgencia = (from pc in db.ScoringVariableAgencias.Include(pc => pc.agencias) select pc).ToList();
                ScoringVariables.ScoringVariableCategoria = db.ScoringVariableCategorias.ToList();
                ScoringVariables.ScoringVariableGarantia = (from pc in db.ScoringVariableGarantias.Include(pc => pc.Garantias) select pc).ToList();
                    
                    var MontoSTR = db.ScoringVariableMontos.ToList();
                    for (int i = 0; i < MontoSTR.Count; i++)
                   {
                        MontoSTR[i].DesdeMonto=Convert.ToDecimal(MontoSTR[i].DesdeMonto).ToString("#,##");
                        MontoSTR[i].HastaMonto = Convert.ToDecimal(MontoSTR[i].HastaMonto).ToString("#,##");

                    }
                    ScoringVariables.ScoringVariableMonto = MontoSTR.ToList();
                    ScoringVariables.ScoringVariableReestructurado = db.ScoringVariableReestructurados.ToList();
                ScoringVariables.ScoringVariableEdad = db.ScoringVariableEdades.ToList();
                ScoringVariables.ScoringVariableOcupacion = db.ScoringVariableOcupaciones.ToList();
                ScoringVariables.ScoringVariableNivelesEducativo=db.ScoringVariableNivelesEducativos.ToList();
                ScoringVariables.ScoringVariableTipoContrato = db.ScoringVariableTipoContratos .ToList();
                    var IngresoSTR = db.ScoringVariableIngresosTotales.ToList();
                    for (int i = 0; i < IngresoSTR.Count; i++)
                    {
                        IngresoSTR[i].IngresoTotalDesde = Convert.ToDecimal(IngresoSTR[i].IngresoTotalDesde).ToString("#,##");
                        IngresoSTR[i].IngresoTotalHasta = Convert.ToDecimal(IngresoSTR[i].IngresoTotalHasta).ToString("#,##");

                    }
                
                ScoringVariables.ScoringVariableIngresosTotal =IngresoSTR.ToList();
                ScoringVariables.ScoringVariableEstrato = db.ScoringVariableEstratos.ToList();
                ScoringVariables.ScoringVariableAntiguedadLaboral = db.ScoringVariableAntiguedadLaborales.ToList();
                ScoringVariables.ScoringVariableEstadosCivil =db.ScoringVariableEstadosCiviles.ToList();
                ScoringVariables.ScoringVariableSexo = db.ScoringVariableSexos.ToList();
                ScoringVariables.ScoringVariablePersonasACargo =db.ScoringVariablePersonasACargos.ToList();
                ScoringVariables.ScoringVariableTipoVivienda =db.ScoringVariableTipoViviendas.ToList();
                
                ScoringVariables.ScoringVariableAntiguedadCooperativa =db.ScoringVariableAntiguedadCooperativas.ToList();
                ScoringVariables.ScoringVariableCapacidadPago =db.ScoringVariableCapacidadPagos.ToList();
                ScoringVariables.ScoringVariableMesesPlazo =db.ScoringVariableMesesPlazos.ToList();
                ScoringVariables.ScoringVariableFormaPago = db.ScoringVariableFormaPagos.ToList();



                }

                return View(ScoringVariables);
        
            }
           
        }

        // POST: Scoring/Variables/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       

        // GET: Scoring/Variables/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringVariableAgencia scoringVariableAgencia = db.ScoringVariableAgencias.Find(id);
            if (scoringVariableAgencia == null)
            {
                return HttpNotFound();
            }
            return View(scoringVariableAgencia);
        }

        // POST: Scoring/Variables/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScoringVariableAgencia scoringVariableAgencia = db.ScoringVariableAgencias.Find(id);
            db.ScoringVariableAgencias.Remove(scoringVariableAgencia);
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

        [HttpPost]
        public JsonResult EditVariablesScoring(string name,int value,int id)
        {
            
            switch(name)
            {
                case "svc":

                    var svc = db.ScoringVariableCategorias.Find(id);
                    svc.PuntajeCategorias = value;
                    db.Entry(svc).State = System.Data.Entity.EntityState.Modified;
                    break;
                case "sva":

                    var sva = db.ScoringVariableAgencias.Find(id);
                    sva.PuntajeAgencia = value;
                    db.Entry(sva).State = System.Data.Entity.EntityState.Modified;
                    break;

                case "svm":

                    var svm = db.ScoringVariableMontos.Find(id);
                    svm.PuntajeMonto = value;
                    db.Entry(svm).State = System.Data.Entity.EntityState.Modified;
                    break;

                case "svg":

                    var svg = db.ScoringVariableGarantias.Find(id);
                    svg.PuntajeGarantia = value;
                    db.Entry(svg).State = System.Data.Entity.EntityState.Modified;
                    break;
                case "svr":

                    var svr = db.ScoringVariableReestructurados.Find(id);
                    svr.PuntajeReestructurado = value;
                    db.Entry(svr).State = System.Data.Entity.EntityState.Modified;
                    break;

                case "sve":

                    var sve = db.ScoringVariableEdades.Find(id);
                    sve.PuntajeEdad = value;
                    db.Entry(sve).State = System.Data.Entity.EntityState.Modified;
                    break;

                case "svo":

                    var svo = db.ScoringVariableOcupaciones.Find(id);
                    svo.PuntajeOcupacion= value;
                    db.Entry(svo).State = System.Data.Entity.EntityState.Modified;
                    break;

                case "svn":

                    var svn = db.ScoringVariableNivelesEducativos.Find(id);
                    svn.PuntajeNivelEducativo = value;
                    db.Entry(svn).State = System.Data.Entity.EntityState.Modified;
                    break;

                case "svi":

                    var svi = db.ScoringVariableIngresosTotales.Find(id);
                    svi.PuntajeIngresoTotal = value;
                    db.Entry(svi).State = System.Data.Entity.EntityState.Modified;
                    break;

                case "sves":

                    var sves = db.ScoringVariableEstratos.Find(id);
                    sves.PuntajeEstrato = value;
                    db.Entry(sves).State = System.Data.Entity.EntityState.Modified;
                    break;

                case "sval":

                    var sval = db.ScoringVariableAntiguedadLaborales.Find(id);
                    sval.PuntajeAntiguedadL = value;
                    db.Entry(sval).State = System.Data.Entity.EntityState.Modified;
                    break;

                case "svec":
                    var svec = db.ScoringVariableEstadosCiviles.Find(id);
                    svec.PuntajeEstadoCivil = value;
                    db.Entry(svec).State = System.Data.Entity.EntityState.Modified;
                    break;

                case "svs":
                    var svs = db.ScoringVariableSexos.Find(id);
                    svs.PuntajeSexo = value;
                    db.Entry(svs).State = System.Data.Entity.EntityState.Modified;
                    break;
                case "svpa":
                    var svpa = db.ScoringVariablePersonasACargos.Find(id);
                    svpa.PuntajePersonasACargo = value;
                    db.Entry(svpa).State = System.Data.Entity.EntityState.Modified;
                    break;

                case "svtv":
                    var svtv = db.ScoringVariableTipoViviendas.Find(id);
                    svtv.PuntajeTipoVivienda = value;
                    db.Entry(svtv).State = System.Data.Entity.EntityState.Modified;
                    break;
                case "svtc":
                    var svtc = db.ScoringVariableTipoContratos.Find(id);
                    svtc.PuntajeTipoContrato = value;
                    db.Entry(svtc).State = System.Data.Entity.EntityState.Modified;
                    break;
                case "svac":
                    var svac = db.ScoringVariableAntiguedadCooperativas.Find(id);
                    svac.PuntajeAntiguedadC = value;
                    db.Entry(svac).State = System.Data.Entity.EntityState.Modified;
                    break;
                case "svcp":
                    var svcp = db.ScoringVariableCapacidadPagos.Find(id);
                    svcp.PuntajeCapacidadPagos = value;
                    db.Entry(svcp).State = System.Data.Entity.EntityState.Modified;
                    break;
                case "svmp":
                    var svmp = db.ScoringVariableMesesPlazos.Find(id);
                    svmp.PuntajeMesesPlazo = value;
                    db.Entry(svmp).State = System.Data.Entity.EntityState.Modified;
                    break;
                case "svfp":
                    var svfp = db.ScoringVariableFormaPagos.Find(id);
                    svfp.PuntajeFormaPago = value;
                    db.Entry(svfp).State = System.Data.Entity.EntityState.Modified;
                    break;
            }
            db.SaveChanges();
            return new JsonResult { Data = new { status = true } };
            

        }
    }
}
