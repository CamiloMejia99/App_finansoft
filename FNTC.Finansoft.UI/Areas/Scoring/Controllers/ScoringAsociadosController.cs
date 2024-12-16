using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Scoring;
using FNTC.Finansoft.Accounting.DTO.FormularioVinculacion;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.UI.Areas.formularioVinculacion.Controllers;
using System.Text.RegularExpressions;
using FNTC.Finansoft.UI.Areas.Terceros.Controllers;

namespace FNTC.Finansoft.UI.Areas.Scoring.Controllers
{
    [Authorize]
    public class ScoringAsociadosController : Controller
    {
        private AccountingContext db = new AccountingContext();
      
        // GET: Scoring/ScoringAsociados
        public ActionResult Index()
        {
            return View(db.ScoringVariableCapacidadPagos.ToList());
        }

        // GET: Scoring/ScoringAsociados/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringVariableCapacidadPago scoringVariableCapacidadPago = db.ScoringVariableCapacidadPagos.Find(id);
            if (scoringVariableCapacidadPago == null)
            {
                return HttpNotFound();
            }
            return View(scoringVariableCapacidadPago);
        }

         private IEnumerable<SelectListItem> Vinculaciones;
        // GET: Scoring/ScoringAsociados/Create
        public ActionResult Create()
        {
           Vinculaciones = new TercerosController().ConsultarTerceros().ToList().Select(p => new SelectListItem { Text = p.NIT + " || " + p.NOMBRE + " " + p.APELLIDO1 + " " + p.APELLIDO2, Value = p.NIT, Selected = false }); ;
            
            ViewBag.Vinculaciones = Vinculaciones;
            List<SelectListItem> Categorias = new List<SelectListItem>();   // Creo una lista
            //Categorias.Add(new SelectListItem { Text = "Seleccione Una Categoria", Value = "" });
            IList<ScoringVariableCategoria> ListaDeCategorias = db.ScoringVariableCategorias.ToList();// extraigo los elementos desde la DB
            //Categorias = ListaDeCategorias.ToList().Select(p => new SelectListItem { Text = p.NombreCategoria, Selected = false }); 
            foreach (var item in ListaDeCategorias)		// recorro los elementos de la db
            {
                Categorias.Add(new SelectListItem { Text = item.NombreCategoria });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Categorias = Categorias;

            List<SelectListItem> Garantias = new List<SelectListItem>();   // Creo una lista
            Garantias.Add(new SelectListItem { Text = "Seleccione Una Garantia", Value = "" });
            IList<Garantias> ListaDeScoringVariableGarantia = db.Garantias.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeScoringVariableGarantia)		// recorro los elementos de la db
            {
                Garantias.Add(new SelectListItem { Text = item.Garantias_Descripcion.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Garantias = Garantias;

            List<SelectListItem> AntiguedadLs = new List<SelectListItem>();   // Creo una lista
            AntiguedadLs.Add(new SelectListItem { Text = "Seleccione Un Rango de Antiguedad", Value = "" });
            IList<ScoringVariableAntiguedadLaboral> ListaDeAntiguedades = db.ScoringVariableAntiguedadLaborales.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeAntiguedades)		// recorro los elementos de la db
            {
                AntiguedadLs.Add(new SelectListItem { Text =  item.DesdeAntiguedad + " a " + item.HastaAntiguedad + " Meses", Value = item.id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.AntiguedadLs = AntiguedadLs;

            List<SelectListItem> Reestructurados = new List<SelectListItem>();   // Creo una lista
            Reestructurados.Add(new SelectListItem { Text = "El Crédito a sido Reestructurado?", Value = "" });
            IList<ScoringVariableReestructurado> ListaScoringVariableReestructurado = db.ScoringVariableReestructurados.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaScoringVariableReestructurado)		// recorro los elementos de la db
            {
                Reestructurados.Add(new SelectListItem { Text = item.EstadoReestructurado.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Reestructurados = Reestructurados;

            List<SelectListItem> FormaPagos = new List<SelectListItem>();   // Creo una lista
            FormaPagos.Add(new SelectListItem { Text = "Seleccione Una Forma de Pago", Value = "" });
            IList<ScoringVariableFormaPago> ListaDeFormaPago = db.ScoringVariableFormaPagos.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeFormaPago)		// recorro los elementos de la db
            {
                FormaPagos.Add(new SelectListItem { Text = item.Forma_Pago.Forma_Pago_Descripcion.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.FormaPagos = FormaPagos;

            List<SelectListItem> Plazos = new List<SelectListItem>();   // Creo una lista
            Plazos.Add(new SelectListItem { Text = "Seleccione Un Plazo", Value = "" });
            IList<ScoringVariableMesesPlazo> ListaDePlazos = db.ScoringVariableMesesPlazos.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDePlazos)		// recorro los elementos de la db
            {
                Plazos.Add(new SelectListItem { Text = item.MesesPlazo.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Plazos = Plazos;

      
            List<SelectListItem> Montos = new List<SelectListItem>();   // Creo una lista

            //formatoVinculacion.ingresobasico = Convert.ToDecimal(formatoVinculacion.ingresobasico).ToString("#,##");//Aqui formateamos el saldo
            
            Montos.Add(new SelectListItem { Text = "Seleccione Un Monto", Value = "" });
            IList<ScoringVariableMonto> ListaDeMontos = db.ScoringVariableMontos.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeMontos)		// recorro los elementos de la db
            {
                var FormateoNumeroDesde = item.DesdeMonto;
                if (FormateoNumeroDesde != "0")
                {
                    FormateoNumeroDesde = Convert.ToDecimal(FormateoNumeroDesde).ToString("#,##");
                }
                var FormateoNumeroHasta = item.HastaMonto;
                double num = 0;
                bool isNum = double.TryParse(FormateoNumeroHasta, out num);
                if (isNum)
                {
                    FormateoNumeroHasta = Convert.ToDecimal(FormateoNumeroHasta).ToString("#,##");
                }
                Montos.Add(new SelectListItem { Text = " Desde " + FormateoNumeroDesde + " Hasta " + FormateoNumeroHasta, Value = item.id.ToString() });  // agrego los elementos de la db a la primera lista que cree
                
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.Montos = Montos;
            return View();
        }

        // POST: Scoring/ScoringAsociados/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PuntajeMonto,PuntajeScoring,CapacidadPago,Endeudamiento,PuntajeEstadoCivil")] ViewModelScoringAsociados viewModelScoringAsociados)
        {
            if (ModelState.IsValid)
            {
               // db.ScoringScoringRealizados.Add(viewModelScoringAsociados);
                //db.SaveChanges();
               // return RedirectToAction("Index");
            }

            return View(viewModelScoringAsociados);
        }

        // GET: Scoring/ScoringAsociados/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringVariableCapacidadPago scoringVariableCapacidadPago = db.ScoringVariableCapacidadPagos.Find(id);
            if (scoringVariableCapacidadPago == null)
            {
                return HttpNotFound();
            }
            return View(scoringVariableCapacidadPago);
        }

        // POST: Scoring/ScoringAsociados/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,NombreCapacidadPago,DescripcionCapacidadPago,PuntajeCapacidadPagos")] ScoringVariableCapacidadPago scoringVariableCapacidadPago)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scoringVariableCapacidadPago).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scoringVariableCapacidadPago);
        }

        // GET: Scoring/ScoringAsociados/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScoringVariableCapacidadPago scoringVariableCapacidadPago = db.ScoringVariableCapacidadPagos.Find(id);
            if (scoringVariableCapacidadPago == null)
            {
                return HttpNotFound();
            }
            return View(scoringVariableCapacidadPago);
        }

        //Autocompletar Cliente

        public JsonResult GetDatosEstadoCuenta(string NIT)
        {
            try
            {
                var tercero = (from pc in db.Terceros where pc.NIT == NIT select pc).Single();
                var TerceroInfoAdicional = db.InfoTerceroAdicional.Include(c => c.estrato).Include(c => c.NivelEstudio).Include(y => y.Tercero).Where(pc => pc.NitTercero == NIT.ToString()).FirstOrDefault();
                var TerceroInfoFinanciera = db.InfoTerceroFinanciera.Include(c => c.Tercero).Where(pc => pc.NitTercero == NIT.ToString()).FirstOrDefault();
                if (TerceroInfoAdicional != null && TerceroInfoFinanciera != null)
                {


                    var estrato = TerceroInfoAdicional.estrato.Estrato.ToString();
                    var nivelEducativo = TerceroInfoAdicional.NivelEstudio.Nestudio;
                    var edad1 = (from pc in db.ScoringVariableEdades where pc.id == 1 select pc).Single();
                    var edad2 = (from pc in db.ScoringVariableEdades where pc.id == 2 select pc).Single();
                    var edad3 = (from pc in db.ScoringVariableEdades where pc.id == 3 select pc).Single();
                    var edad4 = (from pc in db.ScoringVariableEdades where pc.id == 4 select pc).Single();
                    var edad5 = (from pc in db.ScoringVariableEdades where pc.id == 5 select pc).Single();
                    var AntiguedadLaboral1 = (from pc in db.ScoringVariableAntiguedadLaborales where pc.id == 1 select pc).Single();
                    var AntiguedadLaboral2 = (from pc in db.ScoringVariableAntiguedadLaborales where pc.id == 2 select pc).Single();
                    var AntiguedadLaboral3 = (from pc in db.ScoringVariableAntiguedadLaborales where pc.id == 3 select pc).Single();
                    var AntiguedadLaboral4 = (from pc in db.ScoringVariableAntiguedadLaborales where pc.id == 4 select pc).Single();
                    var AntiguedadLaboral5 = (from pc in db.ScoringVariableAntiguedadLaborales where pc.id == 5 select pc).Single();
                    var Ingresos1 = (from pc in db.ScoringVariableIngresosTotales where pc.id == 1 select pc).Single();
                    var Ingresos2 = (from pc in db.ScoringVariableIngresosTotales where pc.id == 2 select pc).Single();
                    var Ingresos3 = (from pc in db.ScoringVariableIngresosTotales where pc.id == 3 select pc).Single();
                    var AntiguedadCoop1 = (from pc in db.ScoringVariableAntiguedadCooperativas where pc.id == 1 select pc).Single();
                    var AntiguedadCoop2 = (from pc in db.ScoringVariableAntiguedadCooperativas where pc.id == 2 select pc).Single();
                    var AntiguedadCoop3 = (from pc in db.ScoringVariableAntiguedadCooperativas where pc.id == 3 select pc).Single();
                    var AntiguedadCoop4 = (from pc in db.ScoringVariableAntiguedadCooperativas where pc.id == 4 select pc).Single();
                    var Sexo = (from pc in db.Parameters where pc.Codigo == tercero.SEXO select pc.Valor).Single();
                    var Estadocivil = (from pc in db.Parameters where pc.Codigo == tercero.ESTADOCIVIL select pc.Valor).Single();
                    var Vivienda = (from pc in db.Parameters where pc.Codigo == tercero.VIVIENDA select pc.Valor).Single();
                    var TipoContrato = TerceroInfoAdicional.Contrato.TipoContrato;
                    var Ocupacion = TerceroInfoAdicional.Ocupacion;
                    var Personasacargo = TerceroInfoAdicional.PersonasCargo;

                    if (Personasacargo > 5)
                    {
                        Personasacargo = 5;
                    }

                    string PC = Personasacargo.ToString();

                    //Fin Personas a cargo


                    var TotalIngresos = TerceroInfoFinanciera.IngresosMensuales.ToString();
                    var Agencias = (from pc in db.agencias where pc.codigoagencia == tercero.DEPENDENCIA select pc.nombreagencia).Single();
                    var PuntajeSexo = (from pc in db.ScoringVariableSexos where pc.NombreSexo == Sexo select pc.PuntajeSexo).Single();
                    var PuntajePersonasCargo = (from pc in db.ScoringVariablePersonasACargos where pc.NombrePersonasACargo == PC select pc.PuntajePersonasACargo).Single();
                    var DescripcionPersonasCargo = (from pc in db.ScoringVariablePersonasACargos where pc.NombrePersonasACargo == PC select pc.DescripcionPersonasACargo).Single();
                    var PuntajeEstadoCivil = (from pc in db.ScoringVariableEstadosCiviles where pc.NombreEstadoCivil == Estadocivil select pc.PuntajeEstadoCivil).Single();
                    var PuntajeVivienda = (from pc in db.ScoringVariableTipoViviendas where pc.NombreTipoVivienda == Vivienda select pc.PuntajeTipoVivienda).Single();
                    var PuntajeAgencia = (from pc in db.ScoringVariableAgencias where pc.IdAgencia == tercero.DEPENDENCIA select pc).Single();
                    //var fichaAporte = (from fp in db.FichasAportes where fp.idPersona == NIT select fp).Single();
                    var PuntajeEstrato = (from pc in db.ScoringVariableEstratos where pc.Codigo == estrato select pc.PuntajeEstrato).Single();
                    var PuntajeNivelEducativo = (from pc in db.ScoringVariableNivelesEducativos where pc.NombreNivelEducativo == nivelEducativo select pc.PuntajeNivelEducativo).Single();



                    var PuntajeTipoContrato = (from pc in db.ScoringVariableTipoContratos where pc.NombreTipoContrato == TipoContrato select pc.PuntajeTipoContrato).Single();
                    var PuntajeOcupacion = (from pc in db.ScoringVariableOcupaciones where pc.NombreOcupacion == Ocupacion select pc.PuntajeOcupacion).Single();

                    //Edad usuario
                    int fecha_dia = DateTime.Now.Day;
                    int fecha_mes = DateTime.Now.Month;
                    int fecha_ano = DateTime.Now.Year;
                    DateTime edadini = tercero.FECHANAC.GetValueOrDefault();
                    int edadini_dia = edadini.Day;
                    int edadini_mes = edadini.Month;
                    int edadini_ano = edadini.Year;
                    if ((edadini_mes == fecha_mes) && (edadini_dia >= fecha_dia))
                    {
                        fecha_ano = fecha_ano - 1;
                    }
                    if (edadini_mes > fecha_mes)
                    {
                        fecha_ano = fecha_ano - 1;
                    }
                    int edad = fecha_ano - edadini_ano;
                    //Fin edad usuario
                    //Antiguedad Laboral
                    string diferenciaMesesEtiquetaL;
                    String fechaAntiguendadLaboralSinFormato = TerceroInfoAdicional.Fechalaboral.ToString();//aqui
                    var FechaAntiguedadLaboral = Convert.ToDateTime(fechaAntiguendadLaboralSinFormato);
                    var FechaAntiguedadLaboralActual = DateTime.Now;


                    var diferenciaMesesL = (FechaAntiguedadLaboralActual.Month - FechaAntiguedadLaboral.Month) + 12 * (FechaAntiguedadLaboralActual.Year - FechaAntiguedadLaboral.Year);
                    TimeSpan diferenciaAl;

                    diferenciaAl = FechaAntiguedadLaboralActual - FechaAntiguedadLaboral;

                    var diferenciaDiasL = diferenciaAl.Days;
                    if (diferenciaMesesL != 0)
                    {
                        var divicionDiasL = diferenciaDiasL / diferenciaMesesL;
                        if (divicionDiasL < 30)
                        {
                            diferenciaMesesL = diferenciaMesesL - 1;
                        }

                        if (diferenciaMesesL > 1)
                        {
                            diferenciaMesesEtiquetaL = (diferenciaMesesL.ToString() + " Meses");
                        }
                        else
                        {
                            diferenciaMesesEtiquetaL = (diferenciaMesesL.ToString() + " Mes");
                        }
                    }
                    else
                    {
                        diferenciaMesesEtiquetaL = ("0" + " Mes");
                    }

                    //Fin Antiguedad Laboral
                    //Puntaje Antiguedad laboral
                    int AntiguedadLaboral_desde1 = Int32.Parse(AntiguedadLaboral1.DesdeAntiguedad);
                    int AntiguedadLaboral_desde2 = Int32.Parse(AntiguedadLaboral2.DesdeAntiguedad);
                    int AntiguedadLaboral_desde3 = Int32.Parse(AntiguedadLaboral3.DesdeAntiguedad);
                    int AntiguedadLaboral_desde4 = Int32.Parse(AntiguedadLaboral4.DesdeAntiguedad);
                    int AntiguedadLaboral_desde5 = Int32.Parse(AntiguedadLaboral5.DesdeAntiguedad);
                    int AntiguedadLaboral_hasta1 = Int32.Parse(AntiguedadLaboral1.HastaAntiguedad);
                    int AntiguedadLaboral_hasta2 = Int32.Parse(AntiguedadLaboral2.HastaAntiguedad);
                    int AntiguedadLaboral_hasta3 = Int32.Parse(AntiguedadLaboral3.HastaAntiguedad);
                    int AntiguedadLaboral_hasta4 = Int32.Parse(AntiguedadLaboral4.HastaAntiguedad);
                    int PuntajeAntiguedadL = 0;
                    if ((diferenciaMesesL >= AntiguedadLaboral_desde1) && (diferenciaMesesL <= AntiguedadLaboral_hasta1))
                    {
                        PuntajeAntiguedadL = AntiguedadLaboral1.PuntajeAntiguedadL;
                    }
                    else
                    if ((diferenciaMesesL >= AntiguedadLaboral_desde2) && (diferenciaMesesL <= AntiguedadLaboral_hasta2))
                    {
                        PuntajeAntiguedadL = AntiguedadLaboral2.PuntajeAntiguedadL;
                    }
                    else
                     if ((diferenciaMesesL >= AntiguedadLaboral_desde3) && (diferenciaMesesL <= AntiguedadLaboral_hasta3))
                    {
                        PuntajeAntiguedadL = AntiguedadLaboral3.PuntajeAntiguedadL;
                    }
                    else
                     if ((diferenciaMesesL >= AntiguedadLaboral_desde4) && (diferenciaMesesL <= AntiguedadLaboral_hasta4))
                    {
                        PuntajeAntiguedadL = AntiguedadLaboral4.PuntajeAntiguedadL;
                    }

                    if ((diferenciaMesesL >= AntiguedadLaboral_desde5))
                    {
                        PuntajeAntiguedadL = AntiguedadLaboral5.PuntajeAntiguedadL;
                    }
                    //Fin Puntaje Antiguedad laboral
                    //Puntaje edad
                    int edad_desde1 = Int32.Parse(edad1.EdadDesde);
                    int edad_desde2 = Int32.Parse(edad2.EdadDesde);
                    int edad_desde3 = Int32.Parse(edad3.EdadDesde);
                    int edad_desde4 = Int32.Parse(edad4.EdadDesde);
                    int edad_desde5 = Int32.Parse(edad5.EdadDesde);
                    int edad_hasta1 = Int32.Parse(edad1.EdadHasta);
                    int edad_hasta2 = Int32.Parse(edad2.EdadHasta);
                    int edad_hasta3 = Int32.Parse(edad3.EdadHasta);
                    int edad_hasta4 = Int32.Parse(edad4.EdadHasta);
                    int PuntajeEdad = 0;
                    if ((edad >= edad_desde1) && (edad <= edad_hasta1))
                    {
                        PuntajeEdad = edad1.PuntajeEdad;
                    }
                    else
                    if ((edad >= edad_desde2) && (edad <= edad_hasta2))
                    {
                        PuntajeEdad = edad2.PuntajeEdad;
                    }
                    else
                    if ((edad >= edad_desde3) && (edad <= edad_hasta3))
                    {
                        PuntajeEdad = edad3.PuntajeEdad;
                    }
                    else
                    if ((edad >= edad_desde4) && (edad <= edad_hasta4))
                    {
                        PuntajeEdad = edad4.PuntajeEdad;
                    }
                    else
                    if ((edad >= edad_desde5))
                    {
                        PuntajeEdad = edad5.PuntajeEdad;
                    }
                    //Fin Puntaje edad
                    //Puntaje ingresos
                    int ingresos_desde1 = Int32.Parse(Ingresos1.IngresoTotalDesde);
                    int ingresos_desde2 = Int32.Parse(Ingresos2.IngresoTotalDesde);
                    int ingresos_desde3 = Int32.Parse(Ingresos3.IngresoTotalDesde);
                    int ingresos_hasta1 = Int32.Parse(Ingresos1.IngresoTotalHasta);
                    int ingresos_hasta2 = Int32.Parse(Ingresos2.IngresoTotalHasta);
                    int Puntajeingresos = 0;
                    string patron = @"[^\w]";
                    Regex regex = new Regex(patron);
                    TotalIngresos = regex.Replace(TotalIngresos, "");
                    int TotalIngresos1 = Int32.Parse(TotalIngresos);
                    TotalIngresos1 = TotalIngresos1 / 100;
                    if ((TotalIngresos1 >= ingresos_desde1) && (TotalIngresos1 <= ingresos_hasta1))
                    {
                        Puntajeingresos = Ingresos1.PuntajeIngresoTotal;
                    }
                    else
                    if ((TotalIngresos1 >= ingresos_desde2) && (TotalIngresos1 <= ingresos_hasta2))
                    {
                        Puntajeingresos = Ingresos2.PuntajeIngresoTotal;
                    }
                    else

                    if ((TotalIngresos1 >= ingresos_desde3))
                    {

                        Puntajeingresos = Ingresos3.PuntajeIngresoTotal;
                    }
                    //Fin puntaje ingresos


                    //Fecha afiliacion
                    var FAfiliacion = (tercero.FECHAR != null) ? tercero.FECHAR.ToString() : DateTime.Now.ToString();
                    var fechaHasta = Convert.ToDateTime(FAfiliacion);//aqui
                    var fechaDesde = DateTime.Now;
                    string diferenciaMesesEtiqueta;
                    var diferenciaMeses = (fechaDesde.Month - fechaHasta.Month) + 12 * (fechaDesde.Year - fechaHasta.Year);
                    TimeSpan diferencia;

                    diferencia = fechaDesde - fechaHasta;

                    var diferenciaDias = diferencia.Days;
                    if (diferenciaMeses != 0)
                    {
                        var divicionDias = diferenciaDias / diferenciaMeses;
                        if (divicionDias < 30)
                        {
                            diferenciaMeses = diferenciaMeses - 1;
                        }

                        if (diferenciaMeses > 1)
                        {
                            diferenciaMesesEtiqueta = (diferenciaMeses.ToString() + " Meses");
                        }
                        else
                        {
                            diferenciaMesesEtiqueta = (diferenciaMeses.ToString() + " Mes");
                        }
                    }
                    else
                    {
                        diferenciaMesesEtiqueta = ("0" + " Mes");
                    }

                    //Fin Fecha afiliacion

                    //Puntaje Fecha afiliacion
                    int AntiguedadCoop_desde1 = Int32.Parse(AntiguedadCoop1.DesdeAntiguedadCooperativa);
                    int AntiguedadCoop_desde2 = Int32.Parse(AntiguedadCoop2.DesdeAntiguedadCooperativa);
                    int AntiguedadCoop_desde3 = Int32.Parse(AntiguedadCoop3.DesdeAntiguedadCooperativa);
                    int AntiguedadCoop_desde4 = Int32.Parse(AntiguedadCoop4.DesdeAntiguedadCooperativa);
                    int AntiguedadCoop_hasta1 = Int32.Parse(AntiguedadCoop1.HastaAntiguedadCooperativa);
                    int AntiguedadCoop_hasta2 = Int32.Parse(AntiguedadCoop2.HastaAntiguedadCooperativa);
                    int AntiguedadCoop_hasta3 = Int32.Parse(AntiguedadCoop3.HastaAntiguedadCooperativa);
                    int PuntajeAntiguedadC = 0;
                    if ((diferenciaMeses >= AntiguedadCoop_desde1) && (diferenciaMeses <= AntiguedadCoop_hasta1))
                    {
                        PuntajeAntiguedadC = AntiguedadCoop1.PuntajeAntiguedadC;
                    }
                    else
                    if ((diferenciaMeses >= AntiguedadCoop_desde2) && (diferenciaMeses <= AntiguedadCoop_hasta2))
                    {
                        PuntajeAntiguedadC = AntiguedadCoop2.PuntajeAntiguedadC;
                    }
                    else
                     if ((diferenciaMeses >= AntiguedadCoop_desde3) && (diferenciaMeses <= AntiguedadCoop_hasta3))
                    {
                        PuntajeAntiguedadC = AntiguedadCoop3.PuntajeAntiguedadC;
                    }

                    if ((diferenciaMeses >= AntiguedadCoop_desde4))
                    {
                        PuntajeAntiguedadC = AntiguedadCoop4.PuntajeAntiguedadC;
                    }
                    //Fin Puntaje Fecha afiliacion
                    //Personas a cargo
                    string PersonasCargo1 = Personasacargo.ToString();
                    if (Personasacargo > 5)
                    {

                        PersonasCargo1 = Personasacargo.ToString() + " " + DescripcionPersonasCargo;
                    }
                    //FIn personas a cargo
                    //Capacidad de pago
                    var gastosMensuales = TerceroInfoFinanciera.GastosMensuales;
                    var totalIngresos = TerceroInfoFinanciera.IngresosMensuales;
                   
                    string FormateoNumeroCapacidadPago;
                    if (gastosMensuales <= 1)
                    {
                        FormateoNumeroCapacidadPago = "El Usuario no Tiene Registrado Gastos Mensuales.";
                    }
                    else
                    {

                        var ConsultaPorcentaje = (from pc in db.ScoringCalculos where pc.id == 1 select pc).Single();
                        int Porcentaje2 = ConsultaPorcentaje.Porcentaje;
                        double Porcentaje = (double)Porcentaje2;
                        double PorcentajeCapacidadPago = Porcentaje * 0.01;
                        double CapacidadPago = (((double)totalIngresos - (double)gastosMensuales) * PorcentajeCapacidadPago);
                        double CapacidadPago1 = Math.Truncate(CapacidadPago);
                        FormateoNumeroCapacidadPago = Convert.ToDecimal(CapacidadPago1).ToString("#,##");
                    }
                    if (FormateoNumeroCapacidadPago == "")
                    {
                        FormateoNumeroCapacidadPago = "0";
                    }

                    //FIn Capacidad de pago
                    //Porcentaje de endeudamiento

                    var totalPasivos = TerceroInfoFinanciera.PasivosTotales;
                    if (TerceroInfoFinanciera.PasivosTotales != null)
                    {
                        totalPasivos = TerceroInfoFinanciera.PasivosTotales;
                    }
                    else
                    {

                        totalPasivos = 0;
                    }

                    var totalActivos = TerceroInfoFinanciera.ActivosTotales;

                    string FormateoNumeroEndeudamiento;
                    string PorcentajeEndeudamiento;
                    if (totalActivos <= 1)
                    {
                        FormateoNumeroEndeudamiento = "El Usuario no Tiene Registrado sus Activos.";
                    }
                    else
                    {
                        

                        double Endeudamiento = (double)totalPasivos / ((double)totalActivos+(double)totalPasivos) ;
                        Endeudamiento = Endeudamiento * 100;
                        FormateoNumeroEndeudamiento = Convert.ToDecimal(Endeudamiento).ToString("#,##");
                        if (totalPasivos == 0 || FormateoNumeroEndeudamiento == "" )
                        {
                            FormateoNumeroEndeudamiento = "0";
                        }

                    }

                    PorcentajeEndeudamiento = FormateoNumeroEndeudamiento + "%";

                    if (totalActivos <= 1)

                    {
                        PorcentajeEndeudamiento = FormateoNumeroEndeudamiento;
                    }

                    //FIn Capacidad de pago



                    List<string> codigos = new List<string>();


                    codigos.Add(tercero.NOMBRE1 + " " + tercero.NOMBRE2 + " " + tercero.APELLIDO1 + " " + tercero.APELLIDO2);
                    codigos.Add(Sexo);
                    codigos.Add(Estadocivil);
                    codigos.Add(edad.ToString());
                    codigos.Add(Vivienda);
                    codigos.Add(TotalIngresos1.ToString());
                    codigos.Add(diferenciaMesesEtiqueta);
                    codigos.Add(Agencias);
                    codigos.Add(PuntajeSexo.ToString());
                    codigos.Add(PuntajeEstadoCivil.ToString());
                    codigos.Add(PuntajeEdad.ToString());
                    codigos.Add(PuntajeVivienda.ToString());
                    codigos.Add(Puntajeingresos.ToString());
                    codigos.Add(PuntajeAgencia.PuntajeAgencia.ToString());
                    codigos.Add(PuntajeAntiguedadC.ToString());
                    codigos.Add(PersonasCargo1.ToString());
                    codigos.Add(PuntajePersonasCargo.ToString());
                    codigos.Add(FormateoNumeroCapacidadPago.ToString());
                    codigos.Add(PorcentajeEndeudamiento.ToString());
                    codigos.Add(estrato.ToString());
                    codigos.Add(PuntajeEstrato.ToString());
                    codigos.Add(nivelEducativo.ToString());
                    codigos.Add(PuntajeNivelEducativo.ToString());
                    codigos.Add(TipoContrato.ToString());
                    codigos.Add(PuntajeTipoContrato.ToString());
                    codigos.Add(Ocupacion.ToString());
                    codigos.Add(PuntajeOcupacion.ToString());
                    codigos.Add(diferenciaMesesEtiquetaL);
                    codigos.Add(PuntajeAntiguedadL.ToString());
                    return Json(codigos, JsonRequestBehavior.AllowGet);
                }

                List<string> Vacio = new List<string>() { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                return Json(Vacio, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                List<string> Vacio = new List<string>() { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };


                return Json(Vacio, JsonRequestBehavior.AllowGet);
                
            }


           
        }
       


        public JsonResult GetDatosAsociado(string NIT, string NombreG, string PuntajeG, string PuntajeD, string PuntajeT, string CapacidadPagoG, string PorcentajeEndeudamientoG)
        {
            int CedulaInt = Int32.Parse(NIT);
            int PuntajeInt = Int32.Parse(PuntajeG);
            int PuntajeDInt = Int32.Parse(PuntajeD);
            int PuntajeTInt = Int32.Parse(PuntajeT);
            var fechaActual = DateTime.Now;
            var scoringScoringRealizado = new ScoringScoringRealizado()
            {
                Cedula = CedulaInt,
                Nombre = NombreG,
                Puntaje = PuntajeInt,
                PuntajeDatacredito = PuntajeDInt,
                PuntajeTotal = PuntajeTInt,
                CapacidadPago = CapacidadPagoG,
                PorcentajeEndeudamiento = PorcentajeEndeudamientoG,
                Fecha = fechaActual,
            };

            db.ScoringScoringRealizados.Add(scoringScoringRealizado);
            db.SaveChanges();
            //return RedirectToAction("Index");
            return Json(false, JsonRequestBehavior.AllowGet);
            //return Json(codigos, JsonRequestBehavior.AllowGet);
        }
            public JsonResult GetDatosCategorias(string NombreCategoria)
        {
            var Categoria = (from pc in db.ScoringVariableCategorias where pc.NombreCategoria == NombreCategoria select pc).Single();
            List<string> codigos = new List<string>();
            codigos.Add(Categoria.PuntajeCategorias.ToString());
            return Json(codigos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDatosGarantias(string NombreGarantia)
        {
            var garantia = (from pc in db.Garantias where pc.Garantias_Descripcion == NombreGarantia select pc).Single();      
            var DatosGarantia = (from pc in db.ScoringVariableGarantias where pc.Garantias_Id == garantia.Garantias_Id select pc).Single();

            List<string> codigos = new List<string>();
            codigos.Add(DatosGarantia.PuntajeGarantia.ToString());
            return Json(codigos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDatosOcupacion(string NombreOcupacion)
        {
            var Ocupacion = (from pc in db.ScoringVariableOcupaciones where pc.NombreOcupacion == NombreOcupacion select pc).Single();
            List<string> codigos = new List<string>();
            codigos.Add(Ocupacion.PuntajeOcupacion.ToString());
            return Json(codigos, JsonRequestBehavior.AllowGet);
        }
   
       // public JsonResult GetDatosTipoContrato(string NombreTipoContrato)
        //{
          
           // var TipoContrato = (from pc in db.ScoringVariableTipoContratos where pc.NombreTipoContrato == NombreTipoContrato select pc).Single();
           // List<string> codigos = new List<string>();
            //codigos.Add(TipoContrato.PuntajeTipoContrato.ToString());
            //return Json(codigos, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult GetDatosReestructurado(string EstadoReestructurado)
        {
          
            var Reestructurado = (from pc in db.ScoringVariableReestructurados where pc.EstadoReestructurado == EstadoReestructurado select pc).Single();
            List<string> codigos = new List<string>();
            codigos.Add(Reestructurado.PuntajeReestructurado.ToString());
            return Json(codigos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDatosFormaPago(string NombreFormaPago)
        {

            var FormaPago = (from pc in db.ScoringVariableFormaPagos where pc.Forma_Pago.Forma_Pago_Descripcion == NombreFormaPago select pc).Single();
            List<string> codigos = new List<string>();
            codigos.Add(FormaPago.PuntajeFormaPago.ToString());
            return Json(codigos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDatosPlazo(string MesesPlazo)
        {

            var Plazo = (from pc in db.ScoringVariableMesesPlazos where pc.MesesPlazo == MesesPlazo select pc).Single();
            List<string> codigos = new List<string>();
            codigos.Add(Plazo.PuntajeMesesPlazo.ToString());
            return Json(codigos, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDatosMonto(string id)
        {
            int id1 = Int32.Parse(id);
            var Monto = (from pc in db.ScoringVariableMontos where pc.id == id1 select pc).Single();
            List<string> codigos = new List<string>();
            codigos.Add(Monto.PuntajeMonto.ToString());
            return Json(codigos, JsonRequestBehavior.AllowGet);
        }

        // POST: Scoring/ScoringAsociados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ScoringVariableCapacidadPago scoringVariableCapacidadPago = db.ScoringVariableCapacidadPagos.Find(id);
            db.ScoringVariableCapacidadPagos.Remove(scoringVariableCapacidadPago);
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
