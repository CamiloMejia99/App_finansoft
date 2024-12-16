using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO.Shared;
using FNTC.Finansoft.BLL.Aportes;
using FNTC.Finansoft.DTO.Ahorros;
using FNTC.Finansoft.Accounting.DTO.FormularioVinculacion;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Fichas;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using FNTC.Finansoft.Accounting.DTO.Geo;
using System.Web.UI.WebControls;
using Rotativa;
using Rotativa.Options;

namespace FNTC.Finansoft.UI.Areas.formularioVinculacion.Controllers
{
    public class formatoVinculacionsController : Controller
    {
        private AccountingContext db = new AccountingContext();

        public ActionResult GetUsuario()
        {

            if (!User.Identity.IsAuthenticated)
            {
                var usuario = "noRegistrado";
                return Json(usuario, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var usuario = User.Identity.Name;
                return Json(usuario, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: formularioVinculacion/formatoVinculacions
        public ActionResult Index()
        {
            return View(db.formatoVinculacions.ToList());
        }

        public ActionResult imprimir()
        {
            return new ViewAsPdf("DetailsImprimir", new { id = "1075308758" })
            {
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Portrait,
                FileName = "contrato.pdf"
            };        
        }

        public ActionResult DetailsImprimir(int id)
        {
            formatoVinculacion formatoVinculacion = db.formatoVinculacions.Find(id);
            return View(formatoVinculacion);
        }

        // GET: formularioVinculacion/formatoVinculacions/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            formatoVinculacion formatoVinculacion = db.formatoVinculacions.Find(id);
            if (formatoVinculacion == null)
            {
                return HttpNotFound();
            }
            formatoVinculacion.ingresobasico = Convert.ToDecimal(formatoVinculacion.ingresobasico).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.otrosingresos = Convert.ToDecimal(formatoVinculacion.otrosingresos).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.totalingresos = Convert.ToDecimal(formatoVinculacion.totalingresos).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.gastosmensuales = Convert.ToDecimal(formatoVinculacion.gastosmensuales).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.totalactivos = Convert.ToDecimal(formatoVinculacion.totalactivos).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.totalpasivos = Convert.ToDecimal(formatoVinculacion.totalpasivos).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.bancarias = Convert.ToDecimal(formatoVinculacion.bancarias).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.bancarias1 = Convert.ToDecimal(formatoVinculacion.bancarias1).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.entidadesfinancieras = Convert.ToDecimal(formatoVinculacion.entidadesfinancieras).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.entidadesfinancieras1 = Convert.ToDecimal(formatoVinculacion.entidadesfinancieras1).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.otrosegresos = Convert.ToDecimal(formatoVinculacion.otrosegresos).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.otrosegresos1 = Convert.ToDecimal(formatoVinculacion.otrosegresos1).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.variaraporte = Convert.ToDecimal(formatoVinculacion.variaraporte).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.aportemensual = Convert.ToDecimal(formatoVinculacion.aportemensual).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.sueldobasico = Convert.ToDecimal(formatoVinculacion.sueldobasico).ToString("#,##");//Aqui formateamos el saldo

            return View(formatoVinculacion);
        }

		// GET: formularioVinculacion/formatoVinculacions/Create

		[HttpPost]
		public JsonResult GetTerceroInfo(string terceroId)
		{
            //int idaportes = db.Database.SqlQuery<int>("SELECT id FROM apo.FichasAportes WHERE idPersona='" + terceroId + "'").FirstOrDefault();
            //FichasAportes fichaaporte = db.FichasAportes.Find(idaportes);

            Tercero terceros = db.Terceros.Find(terceroId);
            if(terceros !=null)
            {
                string genero = "";
                string estadoCivil = "";
                string vivienda = "";
                string numeroRegistro = "";
                if(terceros.SEXO == "F")
                {
                    genero = "Femenino";
                }
                else
                {
                    genero = "Masculino";
                }

                var nveces = db.formatoVinculacions.Where(x => x.cedulaaso == terceroId).Count();
                if (nveces > 0) { numeroRegistro = terceroId + "-" + (nveces + 1); } else { numeroRegistro = terceroId; }

                var dataECivil = db.Parameters.Where(x => x.Codigo == terceros.ESTADOCIVIL).FirstOrDefault();
                if (dataECivil != null) { estadoCivil = dataECivil.Valor; }

                var dataVivienda = db.Parameters.Where(x => x.Codigo == terceros.VIVIENDA).FirstOrDefault();
                if (dataVivienda != null) { vivienda = dataVivienda.Valor; }

                return new JsonResult { Data = new { status = true,
                    nombre = terceros.NOMBRE1+" "+terceros.NOMBRE2,
                    apellido1 = terceros.APELLIDO1,
                    apellido2 = terceros.APELLIDO2,
                    fechalugarexp = Convert.ToString(terceros.FECHAEXP.ToString("yyyy-MM-dd") + " - " + terceros.lugarExpedFK.Nom_muni),
                    fechanac = Convert.ToDateTime(terceros.FECHANAC).ToString("yyyy-MM-dd"),
                    muniNac = terceros.lugarNacimientoFK.Nom_muni,
                    direccion = terceros.DIR,
                    barrio = terceros.BARRIO,
                    telefono = terceros.TEL,
                    celular = terceros.TELMOVIL,
                    correo = terceros.EMAIL,
                    municipioresidencia = terceros.municipioFK.Nom_muni,
                    genero = genero,
                    estadocivil = estadoCivil,
                    vivienda = vivienda,
                    numeroRegistro = numeroRegistro,
                    departamento = terceros.municipioFK.departamentoFK.Nom_dep,
                    sueldobasico = Convert.ToDecimal(terceros.SALARIO).ToString("#,##"),
                } };
            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }

           


            

        }
		public ActionResult Create()
        {
			var fech = DateTime.Now.Date;
			Session["fecha"+User.Identity.Name] = fech.ToShortDateString();
			Session["ano" + User.Identity.Name] = Convert.ToString(DateTime.Now.Year);
			Session["mes" + User.Identity.Name] = Convert.ToString(DateTime.Now.Month);
			Session["dia" + User.Identity.Name] = Convert.ToString(DateTime.Now.Day);

            #region section listItem

            //inicio select list para personas a cargo
            List<SelectListItem> PersonasCargo = new List<SelectListItem>();
            for (int i = 0; i <= 10; i++)
            {
                PersonasCargo.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            //inicio select list para sexo
            List<SelectListItem> sexo = new List<SelectListItem>();
            sexo.Add(new SelectListItem { Text = "--Seleccione", Value = "" });
            sexo.Add(new SelectListItem { Text = "Femenino", Value = "F" });
            sexo.Add(new SelectListItem { Text = "Masculino", Value = "M" });

            //inicio select list para estrato
            List<SelectListItem> estrato = new List<SelectListItem>();
            for (int i = 1; i <= 6; i++)
            {
                estrato.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            //inicio select list para edad
            List<SelectListItem> edad = new List<SelectListItem>();
            for (int i = 0; i <= 110; i++)
            {
                edad.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            ViewBag.PersonasCargo = PersonasCargo;
            ViewBag.sexo = sexo;
            ViewBag.estrato = estrato;
            ViewBag.edad = edad;

            #endregion

            return View();
        }

        // POST: formularioVinculacion/formatoVinculacions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([System.Web.Http.FromBody] formatoVinculacion formatoVinculacion)
        {

            if (formatoVinculacion.ingresobasico != null)
            {
                formatoVinculacion.ingresobasico = formatoVinculacion.ingresobasico.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.otrosingresos != null)
            {
                formatoVinculacion.otrosingresos = formatoVinculacion.otrosingresos.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.totalingresos != null)
            {
                formatoVinculacion.totalingresos = formatoVinculacion.totalingresos.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.gastosmensuales != null)
            {
                formatoVinculacion.gastosmensuales = formatoVinculacion.gastosmensuales.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.totalactivos != null)
            {
                formatoVinculacion.totalactivos = formatoVinculacion.totalactivos.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.totalpasivos != null)
            {
                formatoVinculacion.totalpasivos = formatoVinculacion.totalpasivos.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.bancarias != null)
            {
                formatoVinculacion.bancarias = formatoVinculacion.bancarias.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.bancarias1 != null)
            {
                formatoVinculacion.bancarias1 = formatoVinculacion.bancarias1.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.entidadesfinancieras != null)
            {
                formatoVinculacion.entidadesfinancieras = formatoVinculacion.entidadesfinancieras.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.entidadesfinancieras1 != null)
            {
                formatoVinculacion.entidadesfinancieras1 = formatoVinculacion.entidadesfinancieras1.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.otrosegresos != null)
            {
                formatoVinculacion.otrosegresos = formatoVinculacion.otrosegresos.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.otrosegresos1 != null)
            {
                formatoVinculacion.otrosegresos1 = formatoVinculacion.otrosegresos1.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.aportemensual != null)
            {
                formatoVinculacion.aportemensual = formatoVinculacion.aportemensual.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.variaraporte != null)
            {
                formatoVinculacion.variaraporte = formatoVinculacion.variaraporte.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.sueldobasico != null)
            {
                formatoVinculacion.sueldobasico = formatoVinculacion.sueldobasico.Replace(','.ToString(), "");
            }

            int anio = Convert.ToInt32(formatoVinculacion.ano);
            int mes = Convert.ToInt32(formatoVinculacion.mes);
            int dia = Convert.ToInt32(formatoVinculacion.dia);
            formatoVinculacion.fechasolicitud = new DateTime(anio, mes, dia);

            var errors = ModelState.Values.SelectMany(v => v.Errors);

            if (ModelState.IsValid)
			{

                //si el usuario ya tiene un formulario de vinculacion se modifica la fecha en ficha de aportes con la del nuevo formulario
                var dataFormulario = db.formatoVinculacions.Where(x => x.cedulaaso == formatoVinculacion.cedulaaso).ToList();
                if(dataFormulario.Count>0)
                {
                    var fichaAporte = db.FichasAportes.Where(x => x.idPersona == formatoVinculacion.cedulaaso).FirstOrDefault();
                    if(fichaAporte != null)
                    {
                        fichaAporte.fechaApertura = new DateTime(anio, mes, dia);
                        db.Entry(fichaAporte).State = System.Data.Entity.EntityState.Modified;
                    }
                }


                db.formatoVinculacions.Add(formatoVinculacion);
				db.SaveChanges();
				return RedirectToAction("Details/" + formatoVinculacion.id);
			}

            #region section listItem

            //inicio select list para personas a cargo
            List<SelectListItem> PersonasCargo = new List<SelectListItem>();
            for(int i = 0; i<=10;i++)
            {
                PersonasCargo.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            //inicio select list para sexo
            List<SelectListItem> sexo = new List<SelectListItem>(); 
            sexo.Add(new SelectListItem { Text = "--Seleccione", Value = "" });
            sexo.Add(new SelectListItem { Text = "Femenino", Value = "F" });
            sexo.Add(new SelectListItem { Text = "Masculino", Value = "M" });

            //inicio select list para estrato
            List<SelectListItem> estrato = new List<SelectListItem>();
            for (int i = 1; i <= 6; i++)
            {
                estrato.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            //inicio select list para edad
            List<SelectListItem> edad = new List<SelectListItem>();
            for (int i = 0; i <= 110; i++)
            {
                edad.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            ViewBag.PersonasCargo = PersonasCargo;
            ViewBag.sexo = sexo;
            ViewBag.estrato = estrato;
            ViewBag.edad = edad;

            #endregion

            return View(formatoVinculacion);
        }

        // GET: formularioVinculacion/formatoVinculacions/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            formatoVinculacion formatoVinculacion = db.formatoVinculacions.Find(id);
            if (formatoVinculacion == null)
            {
                return HttpNotFound();
            }

            formatoVinculacion.ingresobasico = Convert.ToDecimal(formatoVinculacion.ingresobasico).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.otrosingresos = Convert.ToDecimal(formatoVinculacion.otrosingresos).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.totalingresos = Convert.ToDecimal(formatoVinculacion.totalingresos).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.gastosmensuales = Convert.ToDecimal(formatoVinculacion.gastosmensuales).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.totalactivos = Convert.ToDecimal(formatoVinculacion.totalactivos).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.totalpasivos = Convert.ToDecimal(formatoVinculacion.totalpasivos).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.bancarias = Convert.ToDecimal(formatoVinculacion.bancarias).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.bancarias1 = Convert.ToDecimal(formatoVinculacion.bancarias1).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.entidadesfinancieras = Convert.ToDecimal(formatoVinculacion.entidadesfinancieras).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.entidadesfinancieras1 = Convert.ToDecimal(formatoVinculacion.entidadesfinancieras1).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.otrosegresos = Convert.ToDecimal(formatoVinculacion.otrosegresos).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.otrosegresos1 = Convert.ToDecimal(formatoVinculacion.otrosegresos1).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.aportemensual = Convert.ToDecimal(formatoVinculacion.aportemensual).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.variaraporte = Convert.ToDecimal(formatoVinculacion.variaraporte).ToString("#,##");//Aqui formateamos el saldo
            formatoVinculacion.sueldobasico = Convert.ToDecimal(formatoVinculacion.sueldobasico).ToString("#,##");//Aqui formateamos el saldo


            #region section listItem

            //inicio select list para personas a cargo
            List<SelectListItem> PersonasCargo = new List<SelectListItem>();
            for (int i = 0; i <= 10; i++)
            {
                PersonasCargo.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            //inicio select list para sexo
            List<SelectListItem> sexo = new List<SelectListItem>();
            sexo.Add(new SelectListItem { Text = "--Seleccione", Value = "0" });
            sexo.Add(new SelectListItem { Text = "Femenino", Value = "F" });
            sexo.Add(new SelectListItem { Text = "Masculino", Value = "M" });

            //inicio select list para estrato
            List<SelectListItem> estrato = new List<SelectListItem>();
            for (int i = 1; i <= 6; i++)
            {
                estrato.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            //inicio select list para edad
            List<SelectListItem> edad = new List<SelectListItem>();
            for (int i = 0; i <= 110; i++)
            {
                edad.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }

            ViewBag.PersonasCargo = PersonasCargo;
            ViewBag.sexo = sexo;
            ViewBag.estrato = estrato;
            ViewBag.edad = edad;

            #endregion

            return View(formatoVinculacion);
        }

        // POST: formularioVinculacion/formatoVinculacions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([System.Web.Http.FromBody] formatoVinculacion formatoVinculacion)
        {
            if(formatoVinculacion.ingresobasico != null)
            {
                formatoVinculacion.ingresobasico = formatoVinculacion.ingresobasico.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.otrosingresos != null)
            {
                formatoVinculacion.otrosingresos = formatoVinculacion.otrosingresos.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.totalingresos != null)
            {
                formatoVinculacion.totalingresos = formatoVinculacion.totalingresos.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.gastosmensuales != null)
            {
                formatoVinculacion.gastosmensuales = formatoVinculacion.gastosmensuales.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.totalactivos != null)
            {
                formatoVinculacion.totalactivos = formatoVinculacion.totalactivos.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.totalpasivos != null)
            {
                formatoVinculacion.totalpasivos = formatoVinculacion.totalpasivos.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.bancarias != null)
            {
                formatoVinculacion.bancarias = formatoVinculacion.bancarias.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.bancarias1 != null)
            {
                formatoVinculacion.bancarias1 = formatoVinculacion.bancarias1.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.entidadesfinancieras != null)
            {
                formatoVinculacion.entidadesfinancieras = formatoVinculacion.entidadesfinancieras.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.entidadesfinancieras1 != null)
            {
                formatoVinculacion.entidadesfinancieras1 = formatoVinculacion.entidadesfinancieras1.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.otrosegresos != null)
            {
                formatoVinculacion.otrosegresos = formatoVinculacion.otrosegresos.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.otrosegresos1 != null)
            {
                formatoVinculacion.otrosegresos1 = formatoVinculacion.otrosegresos1.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.aportemensual != null)
            {
                formatoVinculacion.aportemensual = formatoVinculacion.aportemensual.Replace(','.ToString(), "");
                ////actualizar datos de fichasAportes
                //var dataAportes = db.FichasAportes.Where(x => x.idPersona == formatoVinculacion.cedulaaso).FirstOrDefault();
                //if(dataAportes!=null)
                //{
                //    dataAportes.valor = formatoVinculacion.aportemensual;
                //    dataAportes.valor = dataAportes.valor.Replace(".","");
                //    db.Entry(dataAportes).State = System.Data.Entity.EntityState.Modified;
                //}
                
            }
            if (formatoVinculacion.variaraporte != null)
            {
                formatoVinculacion.variaraporte = formatoVinculacion.variaraporte.Replace(','.ToString(), "");
            }
            if (formatoVinculacion.sueldobasico != null)
            {
                formatoVinculacion.sueldobasico = formatoVinculacion.sueldobasico.Replace(','.ToString(), "");
            }

            //int anio = Convert.ToInt32(formatoVinculacion.ano);
            //int mes = Convert.ToInt32(formatoVinculacion.mes);
            //int dia = Convert.ToInt32(formatoVinculacion.dia);
            //formatoVinculacion.fechasolicitud = new DateTime(anio, mes, dia);

            if (ModelState.IsValid)
            {
                ////actualizamos datos del asociado si estos se han modificado
                //var dataTercero = db.Terceros.Where(x => x.NIT == formatoVinculacion.cedulaaso).FirstOrDefault();
                //if(dataTercero!=null)
                //{
                //    string name = formatoVinculacion.nombreaso;
                //    char delimitador = ' ';
                //    string[] valores = name.Split(delimitador);
                //    int contador = 1;
                //    bool bandera = false;
                //    foreach (var item in valores)
                //    {
                //        if (contador == 1)
                //        {
                //            dataTercero.NOMBRE1 = item.ToUpper();
                //            dataTercero.NOMBRE2 = "";
                //        }
                //        else
                //        {
                //            if(!bandera)
                //            {
                //                dataTercero.NOMBRE2 = item.ToUpper();
                //                bandera = true;
                //            }
                //            else
                //            {
                //                dataTercero.NOMBRE2 += " " + item.ToUpper();
                //            }

                //        }
                //        contador++;
                //    }
                //    dataTercero.APELLIDO1 = formatoVinculacion.apellidoaso1.ToUpper();
                //    dataTercero.APELLIDO2 = formatoVinculacion.apellidoaso2.ToUpper();
                //    dataTercero.NOMBRE = dataTercero.NOMBRE1 + " " + dataTercero.APELLIDO1;
                //    dataTercero.DIR = formatoVinculacion.direccionresidencia;
                //    dataTercero.BARRIO = formatoVinculacion.barrioresidencia;
                //    dataTercero.TEL = formatoVinculacion.telefonoresidencia;
                //    dataTercero.TELMOVIL = formatoVinculacion.celularresidencia;
                //}

                #region section listItem

                //inicio select list para personas a cargo
                List<SelectListItem> PersonasCargo = new List<SelectListItem>();
                for (int i = 0; i <= 10; i++)
                {
                    PersonasCargo.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                }

                //inicio select list para sexo
                List<SelectListItem> sexo = new List<SelectListItem>();
                sexo.Add(new SelectListItem { Text = "--Seleccione", Value = "" });
                sexo.Add(new SelectListItem { Text = "Femenino", Value = "F" });
                sexo.Add(new SelectListItem { Text = "Masculino", Value = "M" });

                //inicio select list para estrato
                List<SelectListItem> estrato = new List<SelectListItem>();
                for (int i = 1; i <= 6; i++)
                {
                    estrato.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                }

                //inicio select list para edad
                List<SelectListItem> edad = new List<SelectListItem>();
                for (int i = 0; i <= 110; i++)
                {
                    edad.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
                }

                ViewBag.PersonasCargo = PersonasCargo;
                ViewBag.sexo = sexo;
                ViewBag.estrato = estrato;
                ViewBag.edad = edad;

                #endregion


                db.Entry(formatoVinculacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(formatoVinculacion);
        }

        // GET: formularioVinculacion/formatoVinculacions/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            formatoVinculacion formatoVinculacion = db.formatoVinculacions.Find(id);
            if (formatoVinculacion == null)
            {
                return HttpNotFound();
            }
            return View(formatoVinculacion);
        }

        // POST: formularioVinculacion/formatoVinculacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            formatoVinculacion formatoVinculacion = db.formatoVinculacions.Find(id);
            db.formatoVinculacions.Remove(formatoVinculacion);
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

        public void setFechaSolicitud()
        {
            var data = db.formatoVinculacions.ToList();
            foreach(var item in data)
            {
                var update = db.formatoVinculacions.Where(x => x.cedulaaso == item.cedulaaso).FirstOrDefault();
                int mes = Convert.ToInt32(item.mes);
                int dia = Convert.ToInt32(item.dia);
                int anio = Convert.ToInt32(item.ano);
                

                update.fechasolicitud = new DateTime(anio,mes,dia);
                db.Entry(update).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
        }

        [HttpPost]
        public JsonResult GetEstadoCivil(string data)
        {

            var result = db.Parameters.Where(x => x.Codigo == data).FirstOrDefault();
            if (result != null)
            {
                string estadoCivil = result.Valor;

                return new JsonResult { Data = new { status = true,estadoCivil } };

            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }

        }

        [HttpPost]
        public JsonResult GetTipoVivienda(string data)
        {

            var result = db.Parameters.Where(x => x.Codigo == data).FirstOrDefault();
            if (result != null)
            {
                string tipoVivienda = result.Valor;

                return new JsonResult { Data = new { status = true, tipoVivienda } };

            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }

        }

        public IList<formatoVinculacion>ConsultarFormatoVinculacion()
        {

            using (var contexto = new AccountingContext())
            {
               
                return contexto.formatoVinculacions.Include(c => c.TerceroFK).ToList();
            }
        }


    }
}
