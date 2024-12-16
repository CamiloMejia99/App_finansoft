using AutoMapper;
using ExcelLibrary.SpreadSheet;
using FNTC.Finansoft.Accounting.BLL.PlanCuentas;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.Result;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Accounting.Controllers.PlandeCuentas
{
    public class Select2Items //pasar a DTOS
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class PlanDeCuentasController : Controller
    {
        public PlanDeCuentasController()
        {
            ViewBag.Nat = new List<SelectListItem>()
			{
				new SelectListItem(){Value = "D", Text = "DEBITO"},
				new SelectListItem(){Value = "C", Text = "CREDITO"}
			};
        }

        public void init()
        {
            //obtengo el valor de session si esta nulo llo o actializop sino lo tomo de sesssion
            if (Session["ctasSelect"] == null)
            {
                var ctas =
                new Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentas("", 0, 0, 2).Select(x => new Select2Items { id = x.CODIGO, text = x.CODIGO + "-" + x.NOMBRE.ToUpper() }).ToList();
                Session["ctasSelect"] = ctas;
            }
        }

        // GET: Accounting/Auxiliares
        public ActionResult Auxiliares()
        {
            init();

            var cuentas = new List<CuentaMayor>();
            using (var ctx = new AccountingContext())
            {
                //cuentas = ctx.PlanCuentas.Where(x => x).ToList();
                var mayores = ctx.PlanCuentas.ToList().Except(ctx.PlanCuentas.OfType<CuentaMayor>());
                cuentas.AddRange(ctx.PlanCuentas.OfType<CuentaAuxiliar>());
                cuentas.AddRange(ctx.PlanCuentas.OfType<CuentaImpuestos>());

                var ctas2 = ctx.PlanCuentas.ToList().Where(x => x.GetType().Name != "CuentaMayor");

                return View(cuentas);
            }

        }


        public ActionResult Index(string term)
        {

            return View(model: term);
        }

        public ActionResult Edit(string id)
        {
            var todasCuentasNIIF = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentasNiif();
            //lista de cuentas No NIIF
            List<SelectListItem> CuentasNIIF = new List<SelectListItem>();   // Creo una lista
            var cuenta = "";
            using (var ctx = new AccountingContext())
            {
                var cuentaNiifDefinida = ctx.PlanCuentas.Find(id);
                cuenta = cuentaNiifDefinida.CTANIIF;
            }
            if (cuenta == "" || cuenta == null || cuenta == "NULL")
            {
                CuentasNIIF.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });
            }
            else
            {
                var nombreCuenta = "";
                using (var ctx = new AccountingContext())
                {
                    var cuentaNiif = ctx.PlanCuentas.Where(a => a.CODIGO == cuenta).Single();
                    nombreCuenta = cuentaNiif.NOMBRE;
                }
                CuentasNIIF.Add(new SelectListItem { Text = nombreCuenta, Value = cuenta });
            }


            foreach (var item in todasCuentasNIIF)		// recorro los elementos de la db
            {
                CuentasNIIF.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.lCuentasNIIF = CuentasNIIF;

            using (var ctx = new AccountingContext())
            {
                CuentaMayor cuentaMayorvar = ctx.PlanCuentas.Find(id);
                var todasCuentasNoNIIF = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentasParaGruposActivosFijos();
                ViewBag.CuentasNoNIIF = new SelectList(todasCuentasNoNIIF, "CODIGO", "NOMBRE", cuentaMayorvar.CTANIIF);
            }
            //
            //init();
            var cta = new AccountingContext().PlanCuentas.Find(id);

             if (cta.EsCuentaImpuesto)
             {                
                 cta = (CuentaImpuestos) new Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentaImpuestos(id);
                 //cta.(CuentaImpuestos.Base) = Convert.ToDecimal(cta.Base).ToString("#,##");//Aqui formateamos el saldo
             }
             
            //cabio los textos a Editar
            ViewBag.Titulo = "Editar";
            ViewBag.Boton = "Editar";

            ViewBag.tci = this.GetTiposCuentasImpuestos();
            return PartialView("Create", cta);
        }

        [HttpPost]
        public ActionResult Edit(CuentaMayor ctaMayor)
        {
            //ViewBag.Titulo = "Editar";
            //ViewBag.Boton = "Editar";
            //TempData["edit"] = true;
            return RedirectToAction("Create", ctaMayor);
        }

        [HttpPost]
        public JsonResult ValidacionCuenta(string Cuenta)
        {
            //  var query=(from s in db.Prestamos orderby s.id descending select s)
            using (var ctx = new AccountingContext())
            {
                if((from s in ctx.PlanCuentas where s.CTANIIF == Cuenta select s).Count() != 0)
                {
                    var MiCuenta = (from s in ctx.PlanCuentas where s.CTANIIF == Cuenta select s).Single();

                    return Json(MiCuenta, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }          
        }

        public ActionResult Create()
        {
            //cabio los textos a Editar
            ViewBag.Titulo = "Crear";
            ViewBag.Boton = "Crear";

            var todasCuentasNIIF = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentasNiif();
            //lista de cuentas No NIIF
            List<SelectListItem> CuentasNIIF = new List<SelectListItem>();   // Creo una lista
            CuentasNIIF.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });

            foreach (var item in todasCuentasNIIF)		// recorro los elementos de la db
            {
                CuentasNIIF.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.lCuentasNIIF = CuentasNIIF;

            ViewBag.tci = this.GetTiposCuentasImpuestos();
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(CuentaMayor ctaMayot, string action, FormCollection col)
        {

            var todasCuentasNIIF = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentasNiif();
            //lista de cuentas No NIIF
            List<SelectListItem> CuentasNIIF = new List<SelectListItem>();   // Creo una lista
            CuentasNIIF.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });

            foreach (var item in todasCuentasNIIF)		// recorro los elementos de la db
            {
                CuentasNIIF.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.lCuentasNIIF = CuentasNIIF;

            ViewBag.tci = this.GetTiposCuentasImpuestos();
            

            int rows = 0;
            //si tiene el check de impuestos  debo crerar cuenta de impuestos sino solo auxiliar
            //traigo la mayor para traer todos los campos del post

            try
            {
                if (ctaMayot.EsCuentaNIIF)
                {
                    var result = new Result();
                    var aux = MapToAuxiliar(ctaMayot);//automapper
                    if (action.Equals("Editar"))
                    {
                        result = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().UpdateCuentaAuxiliar(aux);
                        if (result.ResultCode == ResultCode.Error)
                        {

                            foreach (var item in result.ErrorsWithKey)
                            {
                                ModelState.AddModelError(item.Key, item.Value);
                            }
                            foreach (var item in result.Errors)
                            {
                                ModelState.AddModelError("", item.Value);
                            }

                            ModelState.AddModelError("", "Error: " + result.ResultCode);
                            ViewBag.Titulo = "Editar";
                            ViewBag.Boton = "Editar";
                            ViewBag.Mensaje = "Guardar";
                            return View("Create", ctaMayot);
                        }

                    }
                    else
                    {
                        /*
                        using (var ctx = new FNTC.Finansoft.Accounting.DAL.Model.AccountingContext())
                            {
                                //nueva
                                ctx.PlanCuentas.Add(ctaMayot);
                                rows = ctx.SaveChanges();
                            }
                            */
                        result = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().CreateCuentaNiif(aux);
                    }

                    if (result.ResultCode == ResultCode.Added || result.ResultCode == ResultCode.Updated)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        List<SelectListItem> Errores = new List<SelectListItem>();   // Creo una lista
                        Errores.Add(new SelectListItem { Text = "S", Value = "Error" });

                        foreach (var item in result.ErrorsWithKey)
                        {
                            ModelState.AddModelError(item.Key, item.Value);
                            Errores.Add(new SelectListItem { Text = "S", Value = item.Value });
                        }
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Value);
                        }

                        ModelState.AddModelError("", "Error: " + result.ResultCode);
                        /*
                        ViewBag.Titulo = "Crear";
                        ViewBag.Boton = "Crear";
                        ViewBag.Mensaje = "Guardar";
                        */
                        return Json(Errores, JsonRequestBehavior.AllowGet);
                        //return View("Create", ctaMayot);
                    }
                }
                else
                {
                    if (ctaMayot.EsCuentaImpuesto == true)
                    {
                        //automapper
                        var result = new Result();
                        if(col["lCuentasNIIF"] != "")
                        {
                            ctaMayot.CTANIIF = col["lCuentasNIIF"];
                        }
                        var imp = MapToImpuestos(ctaMayot);
                        var mbase = col["Base"].Replace(','.ToString(), "");
                        imp.Base = Decimal.Parse(mbase);
                        imp.Detalle = col["Detalle"].ToString();
                        //imp.Porcentaje = Decimal.Parse(col["Porcentaje"]);
                        imp.Tipo = col["Tipo"];

                        if (action.Equals("Editar"))
                        {
                            result = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().UpdateCuentaImpuestos(imp);
                            if (result.ResultCode == ResultCode.Error)
                            {

                                foreach (var item in result.ErrorsWithKey)
                                {
                                    ModelState.AddModelError(item.Key, item.Value);
                                }
                                foreach (var item in result.Errors)
                                {
                                    ModelState.AddModelError("", item.Value);
                                }

                                ModelState.AddModelError("", "Error: " + result.ResultCode);
                                ViewBag.Titulo = "Editar";
                                ViewBag.Boton = "Editar";
                                ViewBag.Mensaje = "Guardar";
                                return View("Create", ctaMayot);
                            }

                        }
                        else
                        {
                            result = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().CreateCuentaImpusto(imp);
                        }

                        if (result.ResultCode == ResultCode.Added || result.ResultCode == ResultCode.Updated)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            List<SelectListItem> Errores = new List<SelectListItem>();   // Creo una lista
                            Errores.Add(new SelectListItem { Text = "S", Value = "Error" });
                            foreach (var item in result.ErrorsWithKey)
                            {
                                ModelState.AddModelError(item.Key, item.Value);
                                Errores.Add(new SelectListItem { Text = "S", Value = item.Value });
                            }
                            foreach (var item in result.Errors)
                            {
                                ModelState.AddModelError("", item.Value);
                            }

                            ModelState.AddModelError("", "Error: " + result.ResultCode);

                            return Json(Errores, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var result = new Result();
                        if (col["lCuentasNIIF"] != "")
                        {
                            ctaMayot.CTANIIF = col["lCuentasNIIF"];
                        }
                        var aux = MapToAuxiliar(ctaMayot);//automapper
                        if (action.Equals("Editar"))
                        {
                            result = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().UpdateCuentaAuxiliar(aux);
                            if (result.ResultCode == ResultCode.Error)
                            {

                                foreach (var item in result.ErrorsWithKey)
                                {
                                    ModelState.AddModelError(item.Key, item.Value);
                                }
                                foreach (var item in result.Errors)
                                {
                                    ModelState.AddModelError("", item.Value);
                                }

                                ModelState.AddModelError("", "Error: " + result.ResultCode);
                                ViewBag.Titulo = "Editar";
                                ViewBag.Boton = "Editar";
                                ViewBag.Mensaje = "Guardar";
                                return View("Create", ctaMayot);
                            }

                        }
                        else
                        {
                            /*
                            using (var ctx = new FNTC.Finansoft.Accounting.DAL.Model.AccountingContext())
                                {
                                    //nueva
                                    ctx.PlanCuentas.Add(ctaMayot);
                                    rows = ctx.SaveChanges();
                                }
                                */
                            result = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().CreateCuentaAuxiliar(aux);
                        }

                        if (result.ResultCode == ResultCode.Added || result.ResultCode == ResultCode.Updated)
                        {
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            List<SelectListItem> Errores = new List<SelectListItem>();   // Creo una lista
                            Errores.Add(new SelectListItem { Text = "S", Value = "Error" });

                            foreach (var item in result.ErrorsWithKey)
                            {
                                ModelState.AddModelError(item.Key, item.Value);
                                Errores.Add(new SelectListItem { Text = "S", Value = item.Value });
                            }
                            foreach (var item in result.Errors)
                            {
                                ModelState.AddModelError("", item.Value);
                            }

                            ModelState.AddModelError("", "Error: " + result.ResultCode);
                            /*
                            ViewBag.Titulo = "Crear";
                            ViewBag.Boton = "Crear";
                            ViewBag.Mensaje = "Guardar";
                            */
                            return Json(Errores, JsonRequestBehavior.AllowGet);
                            //return View("Create", ctaMayot);
                        }
                    }
                }
            }   
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }finally
            {
                ResetCache();
                ViewBag.result = rows;
            }                            
            return RedirectToAction("Index");                      
        }
        public CuentaMayor MapToMayor(CuentaMayor mayor)
        {
            var conf = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CuentaMayor, CuentaMayor>();

            });

            var mapper = conf.CreateMapper();
            var aux = mapper.Map<CuentaMayor>(mayor);

            return aux;
        }

        public ActionResult CreateMayores()
        {
            //cabio los textos a Editar
            ViewBag.Titulo = "Crear";
            ViewBag.Boton = "Crear";

            var todasCuentasNIIF = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentasNiif();
            //lista de cuentas No NIIF
            List<SelectListItem> CuentasNIIF = new List<SelectListItem>();   // Creo una lista
            CuentasNIIF.Add(new SelectListItem { Text = "Seleccione Una Cuenta", Value = "" });

            foreach (var item in todasCuentasNIIF)		// recorro los elementos de la db
            {
                CuentasNIIF.Add(new SelectListItem { Text = item.NOMBRE + " || " + item.CODIGO, Value = item.CODIGO.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.lCuentasNIIF = CuentasNIIF;

            ViewBag.tci = this.GetTiposCuentasImpuestos();
            return PartialView();
        }
        [HttpPost]
        public ActionResult CreateMayores(CuentaMayor ctaMayot, string action, FormCollection col, string TipoCuenta)
        {


            int rows = 0;
            try
            {
                var result = new Result();
                var DataCM = MapToMayor(ctaMayot);

                result = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().CreateCuentaMayor(DataCM, TipoCuenta);

                if (result.ResultCode == ResultCode.Added || result.ResultCode == ResultCode.Updated)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    List<SelectListItem> Errores = new List<SelectListItem>();
                    Errores.Add(new SelectListItem { Text = "S", Value = "Error" });

                    foreach (var item in result.ErrorsWithKey)
                    {
                        ModelState.AddModelError(item.Key, item.Value);
                        Errores.Add(new SelectListItem { Text = "S", Value = item.Value });
                    }
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Value);
                    }

                    ModelState.AddModelError("", "Error: " + result.ResultCode);

                    return Json(Errores, JsonRequestBehavior.AllowGet);

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                ResetCache();
                ViewBag.result = rows;
            }
            return RedirectToAction("Index");
           

        }
        public ActionResult CreateCuentaImpuestos(CuentaImpuestos ctaImpuestos)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var ctx = new AccountingContext())
                    {
                        ctx.CuentasImpuestos.Add(ctaImpuestos);

                    }
                }
                catch (Exception)
                {

                    throw;
                }
            }
            // return Json(false);
            return View();
        }

        private void ResetCache()
        {
            var ctas = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentas("", 0, 0);
            System.Web.HttpRuntime.Cache["ctas"] = ctas;

        }

        public JsonResult GetCuentas4Selects(string term = "", int page = 1, int count = 10, int type = 2)
        {
            //	if (term == "") return Json(new List<SelectListItem>(),JsonRequestBehavior.AllowGet);
            var ctasAuxiliares =
                   new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentas(term, 0, 0, type).Select(x => new Select2Items { id = x.CODIGO, text = x.NOMBRE.ToUpper() }).ToList();

            return Json(new { results = ctasAuxiliares, pagination = new { more = false } }, JsonRequestBehavior.AllowGet);
        }
        [Compress]
        public JsonResult GetCuentas(string term = "", int page = 1, int count = 10, int type = 0)
        {

        #warning No sirve el cache porque varia con term
            //var todas = (List<CuentaMayor>)System.Web.HttpRuntime.Cache["todas"];
            //if (todas == null)
            //{
            var todas = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentas(term, 0, 0, type);
            //    System.Web.HttpRuntime.Cache["todas"] = todas;
            //}
            var ctas = todas;

            //var ctas =
            //    new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentas(term, 0, 0, type);

            //   var ctasJson = ctas.Select(x => new { query = term, suggestions = new { id = x.id, text = x.text } });

            //var dataTabledata = ctas.Select((x, index) => new[] { index.ToString(), x.CODIGO, x.NOMBRE, x.NATURALEZA, x.REQTERCERO.ToString(), x.REQCCOSTO.ToString(), x.CORRIENTE.ToString(), x.EsCuentaImpuesto.ToString(), x.Saldo.ToString() });

            //var ctasJson = ctas.Select((x, index) => new { data = new { DT_RowId = "row_" + index, first_name = x.CODIGO } });

            //Response.AppendHeader("Content-Encoding", "gzip");
            //Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);
            return Json(ctas, JsonRequestBehavior.AllowGet);
        }

        [Compress]
        public JsonResult GetCuentasAA(string term = "", int page = 1, int count = 10, int type = 2)
        {            
            var todas = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentas(term, 0, 0, type);
            //    System.Web.HttpRuntime.Cache["todas"] = todas;
            //}
            var ctas = todas;

            //var ctas =
            //    new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentas(term, 0, 0, type);

            //   var ctasJson = ctas.Select(x => new { query = term, suggestions = new { id = x.id, text = x.text } });

            //var dataTabledata = ctas.Select((x, index) => new[] { index.ToString(), x.CODIGO, x.NOMBRE, x.NATURALEZA, x.REQTERCERO.ToString(), x.REQCCOSTO.ToString(), x.CORRIENTE.ToString(), x.EsCuentaImpuesto.ToString(), x.Saldo.ToString() });

            //var ctasJson = ctas.Select((x, index) => new { data = new { DT_RowId = "row_" + index, first_name = x.CODIGO } });

            //Response.AppendHeader("Content-Encoding", "gzip");
            //Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);
            return Json(ctas, JsonRequestBehavior.AllowGet);
        }

        [Compress]
        public JsonResult GetCuentasNIIF()
        {
            var todas = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentasNIIF();
            //    System.Web.HttpRuntime.Cache["todas"] = todas;
            //}
            var ctas = todas;

            //var ctas =
            //    new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentas(term, 0, 0, type);

            //   var ctasJson = ctas.Select(x => new { query = term, suggestions = new { id = x.id, text = x.text } });

            //var dataTabledata = ctas.Select((x, index) => new[] { index.ToString(), x.CODIGO, x.NOMBRE, x.NATURALEZA, x.REQTERCERO.ToString(), x.REQCCOSTO.ToString(), x.CORRIENTE.ToString(), x.EsCuentaImpuesto.ToString(), x.Saldo.ToString() });

            //var ctasJson = ctas.Select((x, index) => new { data = new { DT_RowId = "row_" + index, first_name = x.CODIGO } });

            //Response.AppendHeader("Content-Encoding", "gzip");
            //Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);
            return Json(ctas, JsonRequestBehavior.AllowGet);
        }

        [Compress]
        public JsonResult GetCuentasParaGruposActivosFijos()
        {
            var todas = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentasParaGruposActivosFijos();
            //    System.Web.HttpRuntime.Cache["todas"] = todas;
            //}
            var ctas = todas;

            //var ctas =
            //    new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentas(term, 0, 0, type);

            //   var ctasJson = ctas.Select(x => new { query = term, suggestions = new { id = x.id, text = x.text } });

            //var dataTabledata = ctas.Select((x, index) => new[] { index.ToString(), x.CODIGO, x.NOMBRE, x.NATURALEZA, x.REQTERCERO.ToString(), x.REQCCOSTO.ToString(), x.CORRIENTE.ToString(), x.EsCuentaImpuesto.ToString(), x.Saldo.ToString() });

            //var ctasJson = ctas.Select((x, index) => new { data = new { DT_RowId = "row_" + index, first_name = x.CODIGO } });

            //Response.AppendHeader("Content-Encoding", "gzip");
            //Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);
            return Json(ctas, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetCuentas2(string term = "", int page = 1, int count = 10, int type = 0)
        {
            var ctas = (List<CuentaMayor>)System.Web.HttpRuntime.Cache["ctas"];
            if (ctas == null)
            {
                ctas = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentas(term, 0, 0, type);
                System.Web.HttpRuntime.Cache["ctas"] = ctas;
            }

            //var ctasJson = ctas.Select(x => new { query = term, suggestions = new { id = x.id, text = x.text } });

            var chulito = "<i class='fa fa-check'>";
            var equis = "<i class='fa fa-times'>";


            //var botonEditar = "<button class='fa fa-pencil'></button>";

            var dataTabledata = ctas.
                Select((x, index)
                    => new[] 
					{ 
						index.ToString(),
						x.CODIGO, x.NOMBRE,
						x.NATURALEZA,
						x.REQTERCERO?chulito:equis,
						x.REQCCOSTO?chulito:equis,
						x.CORRIENTE?chulito:equis,
						x.EsCuentaImpuesto?chulito:equis,
						//x.Saldo.ToString(),
						BotonEditar(x.CODIGO)
						//botonEditar
					});

            //  var ctasJson = ctas.Select((x, index) => new { data = new { DT_RowId = "row_" + index, first_name = x.CODIGO } });
            Response.AppendHeader("Content-Encoding", "gzip");

            Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);

            return Json(new { data = dataTabledata }, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetCuenta(string term)
        {

            var ctas = new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas.PlanDeCuentasDAL().GetCuentas(term, 0, 0, 4);
            if (ctas.Count > 0)
            {
                var cta = ctas.First();
                if (cta.EsCuentaImpuesto)
                {
                    //obtengo base y porcentaje
                    var imp = new { @base = 1, porcentaje = 0.16 };
                    return Json(new { cta = cta, imp = imp }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { cta = cta }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { cta = "null" }, JsonRequestBehavior.AllowGet);
        }

        public List<CuentaMayor> GetCuentasAuxiliares()
        {
            var respuesta = new PlanCuentasBLL().GetCuentasAuxiliares();
            return respuesta;
        }

        private string BotonEditar(string id)
        {

            var botonEditar = "<a id=" + id + " href='' class='btn btn-default fa fa-pencil editar' data-toggle='modal' data-target='#centro' onclick='edit(this);'></a>";


            if (id.Count() < 7)
                return "";
            else
                return botonEditar;
            //data-toggle="modal" data-target="#centro"
        }

        public List<SelectListItem> GetTiposCuentasImpuestos()
        {
            List<SelectListItem> tipos = new List<SelectListItem>();
            var _tipos = new FNTC.Framework.Params.DAL.ParamsDAL().GetParameters("TIPOCUENTAIMPUESTO");

            _tipos.ForEach(x => tipos.Add(new SelectListItem() { Text = x.Valor, Value = x.Codigo }));
            //tipos.Add( 
            //new SelectListItem()
            //{
            //    Value  = x.Codigo.ToString(),Text = x.Valor
            //}));
            return tipos;
        }



        public JsonResult GetCuentas4Tree(string term = "", int page = 1, int count = 10, int type = 0)
        {
            var ctas =
                new FNTC.Finansoft.Accounting.BLL.PlanCuentas.PlanCuentasBLL();

            return Json(ctas.GetCuentas4TreeView(term), JsonRequestBehavior.AllowGet);
        }

        #region jqgrid
        //[OutputCache(Duration = 100)]
        //[AcceptVerbs(HttpVerbs.Post)]
        //        public ActionResult PlanCuentas(JqGridRequest request)
        //        {

        //            /*
        //              PageIndex: 0
        //    PagesCount: null
        //    RecordsCount: 10
        //    Searching: false
        //    SearchingFilter: null
        //    SearchingFilters: null
        //    SortingName: "CODIGO"
        //    SortingOrder: Asc

        //             */
        //#warning debo paginar segun request desde DAL
        //            var cuentas =
        //                new FNTC.Finansoft.Accounting.DAL.PlanDeCuentas
        //                    .PlanDeCuentasDAL()
        //                    .GetCuentas("");
        //            int totalRecords = cuentas.Count();

        //            cuentas = cuentas.Skip(request.PageIndex * request.RecordsCount).Take(request.RecordsCount).ToList();
        //            var cuentasDTO = new List<CuentaDTO>();
        //            //convertir a ViewMOdel
        //            //var tercerosVM = terceros.Select(t => new TerceroViewModel(t));
        //            #region Mapper
        //            var config = new MapperConfiguration(cfg =>
        //            {
        //                cfg.CreateMap<CuentaDTO, CuentaMayor>(); //workaround
        //                cfg.CreateMap<CuentaMayor, CuentaDTO>(); //workaround
        //            });
        //            var mapper = config.CreateMapper();
        //            #endregion

        //            foreach (var item in cuentas)
        //            {
        //                var cuentaDTO = mapper.Map<CuentaDTO>(item);
        //                cuentasDTO.Add(cuentaDTO);
        //            }

        //            //int totalRecords = _ordersRepository.GetCount();


        //            JqGridResponse response = new JqGridResponse()
        //            {
        //                TotalPagesCount = (int)Math.Ceiling((float)totalRecords / (float)request.RecordsCount),
        //                PageIndex = request.PageIndex,
        //                TotalRecordsCount = totalRecords
        //            };
        //            //response.Records.AddRange(from order in _ordersRepository.FindRange(String.Format("{0} {1}", request.SortingName, request.SortingOrder), request.PageIndex * request.RecordsCount, request.RecordsCount)
        //            //                          select new JqGridRecord<OrderViewModel>(Convert.ToString(order.Id), new OrderViewModel(order)));

        //            response.Records.AddRange(cuentasDTO.Select(t => new JqGridRecord<CuentaDTO>(t.CODIGO, t)));


        //            return new JqGridJsonResult() { Data = response };
        //        }
        #endregion

        #region Mappers
        /// <summary>
        /// Esto deberia ir en los DTOs
        /// </summary>
        /// <param name="mayor"></param>
        /// <returns></returns>
        public CuentaAuxiliar MapToAuxiliar(CuentaMayor mayor)
        {
            var conf = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CuentaMayor, CuentaAuxiliar>();

            });

            var mapper = conf.CreateMapper();
            var aux = mapper.Map<CuentaAuxiliar>(mayor);

            return aux;
        }


        public CuentaImpuestos MapToImpuestos(CuentaMayor mayor)
        {
            var conf = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CuentaMayor, CuentaImpuestos>();

            });

            var mapper = conf.CreateMapper();
            var imp = mapper.Map<CuentaImpuestos>(mayor);

            return imp;
        }

        #endregion


        public FileResult Exportar()
        {

            //public void exportar(Informe Informe)
            //{

            // string file = "f:\\ANDREA\\FINANTEC_REPORTES\\reportes_nuevo\\2.comprobante.xls";
            Workbook workbook = new Workbook();
            Worksheet ws = new Worksheet("PUC");
            using (var ctx = new AccountingContext())
            {
                var ds = new DataSet("plan");

                var dt = ctx.PlanCuentas.CopyToDataTable();
                ds.Tables.Add(dt);
                HasNull(ref dt);

                string serverPath = Server.MapPath("/bin");
                serverPath += "\\plan.xls";
                try
                {
                    ExcelLibrary.DataSetHelper.CreateWorkbook(serverPath, ds);
                }
                catch (Exception ex)
                {

                    throw;
                }

                return File(serverPath, "application/vnd.ms-excel", "puc.xls");
                //eturn new FileResult(fileContents, "application/vnd.ms-excel");
            }


        }

        public static bool HasNull(ref DataTable table)
        {
            foreach (DataColumn column in table.Columns)
            {
                if (table.Rows.OfType<DataRow>().Any(r => r.IsNull(column)))
                {
                    //replace with ""
                    var columIndex = column.Ordinal;
                    //var lista = 
                    table.Rows.OfType<DataRow>().Where(R => R.IsNull(column)).ToList()
                    .ForEach(r =>
                                 r[column] = column.DataType == typeof(System.Decimal) ? 0M : 0);



                    // return true;

                }

            }

            return false;
        }


    }



    public static class DataSetLinqOperators
    {
        public static DataTable CopyToDataTable<T>(this IEnumerable<T> source)
        {
            return new ObjectShredder<T>().Shred(source, null, null);
        }

        public static DataTable CopyToDataTable<T>(this IEnumerable<T> source,
                                                    DataTable table, LoadOption? options)
        {
            return new ObjectShredder<T>().Shred(source, table, options);
        }

    }

    public class ObjectShredder<T>
    {
        private FieldInfo[] _fi;
        private PropertyInfo[] _pi;
        private Dictionary<string, int> _ordinalMap;
        private Type _type;

        public ObjectShredder()
        {
            _type = typeof(T);
            _fi = _type.GetFields();
            _pi = _type.GetProperties();
            _ordinalMap = new Dictionary<string, int>();
        }

        public DataTable Shred(IEnumerable<T> source, DataTable table, LoadOption? options)
        {
            if (typeof(T).IsPrimitive)
            {
                return ShredPrimitive(source, table, options);
            }


            if (table == null)
            {
                table = new DataTable(typeof(T).Name);
            }

            // now see if need to extend datatable base on the type T + build ordinal map
            table = ExtendTable(table, typeof(T));

            table.BeginLoadData();
            using (IEnumerator<T> e = source.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    if (options != null)
                    {
                        table.LoadDataRow(ShredObject(table, e.Current), (LoadOption)options);
                    }
                    else
                    {
                        table.LoadDataRow(ShredObject(table, e.Current), true);
                    }
                }
            }
            table.EndLoadData();
            return table;
        }

        public DataTable ShredPrimitive(IEnumerable<T> source, DataTable table, LoadOption? options)
        {
            if (table == null)
            {
                table = new DataTable(typeof(T).Name);
            }

            if (!table.Columns.Contains("Value"))
            {
                table.Columns.Add("Value", typeof(T));
            }

            table.BeginLoadData();
            using (IEnumerator<T> e = source.GetEnumerator())
            {
                Object[] values = new object[table.Columns.Count];
                while (e.MoveNext())
                {
                    values[table.Columns["Value"].Ordinal] = e.Current;

                    if (options != null)
                    {
                        table.LoadDataRow(values, (LoadOption)options);
                    }
                    else
                    {
                        table.LoadDataRow(values, true);
                    }
                }
            }
            table.EndLoadData();
            return table;
        }

        public DataTable ExtendTable(DataTable table, Type type)
        {
            // value is type derived from T, may need to extend table.
            foreach (FieldInfo f in type.GetFields())
            {
                if (!_ordinalMap.ContainsKey(f.Name))
                {
                    DataColumn dc = table.Columns.Contains(f.Name) ? table.Columns[f.Name]
                        : table.Columns.Add(f.Name, f.FieldType);
                    _ordinalMap.Add(f.Name, dc.Ordinal);
                }
            }
            foreach (PropertyInfo p in type.GetProperties())
            {
                if (!_ordinalMap.ContainsKey(p.Name))
                {
                    DataColumn dc = table.Columns.Contains(p.Name) ? table.Columns[p.Name]
                        : table.Columns.Add(p.Name, p.PropertyType);
                    _ordinalMap.Add(p.Name, dc.Ordinal);
                }
            }
            return table;
        }

        public object[] ShredObject(DataTable table, T instance)
        {

            FieldInfo[] fi = _fi;
            PropertyInfo[] pi = _pi;

            if (instance.GetType() != typeof(T))
            {
                ExtendTable(table, instance.GetType());
                fi = instance.GetType().GetFields();
                pi = instance.GetType().GetProperties();
            }

            Object[] values = new object[table.Columns.Count];
            foreach (FieldInfo f in fi)
            {
                values[_ordinalMap[f.Name]] = f.GetValue(instance);
            }

            foreach (PropertyInfo p in pi)
            {
                values[_ordinalMap[p.Name]] = p.GetValue(instance, null);
            }
            return values;
        }


    }
}



