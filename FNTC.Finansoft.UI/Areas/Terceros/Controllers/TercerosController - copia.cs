using FNTC.Finansoft.Accounting.DAL;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Fichas;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using FNTC.Finansoft.Accounting.DTO.TercerosOtrasEntidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Terceros.Controllers
{
    public class TercerosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        [Compress]
        public JsonResult GetTercerosAutocompletar(string term = "", int page = 1, int count = 10, int type = 2)
        {
            var terceros = new List<Tercero>();

            using (var ctx = new AccountingContext())
            {
                terceros = ctx.Terceros.
                Where(pc => pc.NIT.Contains(term) || pc.NOMBRE.Contains(term))
                  .OrderBy(o => o.NOMBRE).ToList();
            }
            var ter = terceros;
            return Json(ter, JsonRequestBehavior.AllowGet);
        }

        [Compress]
        public ActionResult GetTerceros(string term = "", string outputFormat = "datatables")
        {
            //var dal = new FNTC.Finansoft.Accounting.DAL.TercerosDAL();
            // var terceros = dal.GetTerceros(term);
            var terceros = db.Terceros.ToList();
            //se daño el automapper
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<Tercero, TerceroDTO>();
            //    //  cfg.AddProfile<FooProfile>();
            //});

            //var map = config.CreateMapper();
            var _tercerosDTO = terceros;
            //terceros.ForEach(x => _tercerosDTO.Add(map.Map<TerceroDTO>(x)));


            switch (outputFormat)
            {
                case "datatables":
                    // var botonEditar = "<button class='fa fa-pencil edit' onclick='edit(this);'></button>";
                    var dataTabledata = _tercerosDTO.Select((x, index)
                    => new[] { x.NIT,  x.NOMBRE1 + " "+ x.NOMBRE2 +" "+  x.APELLIDO1 +" "+ x.APELLIDO2,    x.DIR,  x.TEL,  x.TELMOVIL,  x.EMAIL ,BotonEditar(x.NIT),BotonEliminar(x.NIT)
                    });
                    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    serializer.MaxJsonLength = int.MaxValue;
                    var json = Json(new { data = dataTabledata }, JsonRequestBehavior.AllowGet);
                    json.MaxJsonLength = int.MaxValue;
                    return json;

                default:
                    serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    serializer.MaxJsonLength = int.MaxValue;
                    json = Json(new { data = _tercerosDTO}, JsonRequestBehavior.AllowGet);
                    json.MaxJsonLength = int.MaxValue;
                    return json;
                    
            }
        }

        private string BotonEditar(string id)
        {
            //<a href="/Terceros/terceros/Create" class="AddUser btn btn-success btn-xs btnnuevo" data-toggle="modal" data-target="#centro">Nuevo</a>

            //  var botonEditar = "<button id=" + id + " class='fa fa-pencil edit' onclick='edit(this);'></button>";
            var botonEditar = @"<a href='/Terceros/terceros/Edit?nit=" + id + "' class='btn btn-default fa fa-pencil edit' data-toggle='modal' data-target='#centro'></a>";
            return botonEditar;
        }

        private string BotonEliminar(string id)
        {
            //<a href="/Terceros/terceros/Create" class="AddUser btn btn-success btn-xs btnnuevo" data-toggle="modal" data-target="#centro">Nuevo</a>

            //  var botonEditar = "<button id=" + id + " class='fa fa-pencil edit' onclick='edit(this);'></button>";
            var botonEliminar = @"<a href='/Terceros/terceros/Delete?nit=" + id + "' class='btn btn-danger glyphicon glyphicon-trash' data-toggle='modal' data-target='#centro'></a>";
            return botonEliminar;
        }

        public ActionResult Index()
        {
            //var dal = new FNTC.Finansoft.Accounting.DAL.TercerosDAL();
            //var terceros = dal.GetTerceros();

            ////se daño el automapper
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<Tercero, TerceroDTO>();
            //    //  cfg.AddProfile<FooProfile>();
            //});

            //var map = config.CreateMapper();
            //var _tercerosDTO = new List<TerceroDTO>();
            //terceros.ForEach(x => _tercerosDTO.Add(map.Map<TerceroDTO>(x)));

            //return View(_tercerosDTO);
            return View();

        }

        public ActionResult Create()
        {
            List<SelectListItem> Agencias = new List<SelectListItem>();   // Creo una lista
            IList<agencias> ListaDeTerceros = db.agencias.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeTerceros)		// recorro los elementos de la db
            {
                Agencias.Add(new SelectListItem { Text = item.nombreagencia, Value = item.codigoagencia.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.agenciasList = Agencias;

            var tc = new TerceroDTO
            {
                _clasesID = new FNTC.Framework.Params.DAL.ParamsDAL()
                .GetParamValues("TIPODOCUMENTO")
                .Select(x => new SelectListItem()
                {
                    Value = x.Codigo,
                    Text = x.Valor
                })
                    .ToList()
            };

            ViewBag.Post2 = ViewBag.action = "Create";
            //var action = (string)ViewBag.action;
            this.Init(ref tc);

            //tc._clasesID = this.GetClaseID; //esto debe actualizasrse sgun lo que se seleccione          
            return PartialView(tc);
        }

        [HttpPost]
        public ActionResult Create(TerceroDTO terceroDTO)
        {

            List<SelectListItem> Agencias = new List<SelectListItem>();   // Creo una lista
            IList<agencias> ListaDeTerceros = db.agencias.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeTerceros)		// recorro los elementos de la db
            {
                Agencias.Add(new SelectListItem { Text = item.nombreagencia, Value = item.codigoagencia.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.agenciasList = Agencias;

            /*
            var salari = terceroDTO.SALARIO;
            var replacement = salari.Replace(','.ToString(), "");
            terceroDTO.SALARIO = replacement;
            */
            if (terceroDTO.SALARIO != null)
            {
                terceroDTO.SALARIO = terceroDTO.SALARIO.Replace('.'.ToString(), "");
            }
            

            ViewBag.Guardar = "prueba";

            //si es personaj juridica nombre1 y apellido1 no son necesarios
            
            if (terceroDTO.ESASOCIADO == 3)
            {
                ModelState["NOMBRE1"].Errors.Clear();
                ModelState["APELLIDO1"].Errors.Clear();
                ModelState["ESTADOCIVIL"].Errors.Clear();
                ModelState["SEXO"].Errors.Clear();
                ModelState["PAISNAC"].Errors.Clear();
                ModelState["DEPTONAC"].Errors.Clear();
                ModelState["PROFESION"].Errors.Clear();
                ModelState["VIVIENDA"].Errors.Clear();
                terceroDTO.FECHANAC = DateTime.Now;

            }
            
            var errors = ModelState.Values.SelectMany(v => v.Errors);

           
                try
                {
                    var result = new FNTC.Finansoft.Accounting.DAL.TercerosDAL().CreateTercero(terceroDTO);

                    ViewBag.Guardar = "guardar";
                    ViewBag.ErrorMsg = "Datos Guardados Correctamente";

                    return RedirectToAction("Index");
                }
                catch (DbEntityValidationException ex)
                {
                    
                    this.Init(ref terceroDTO);

                    ModelState.AddModelError("", ex.Message);
                    this.Init(ref terceroDTO);
                    return View(terceroDTO);
                }
            

            ViewBag.Post2 = "Create";
            this.Init(ref terceroDTO);
            return View(terceroDTO);
        }

        public ActionResult Edit(string nit)
        {
            /*
            List<SelectListItem> Agencias = new List<SelectListItem>();   // Creo una lista
            IList<agencias> ListaDeTerceros = db.agencias.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeTerceros)		// recorro los elementos de la db
            {
                Agencias.Add(new SelectListItem { Text = item.nombreagencia, Value = item.codigoagencia.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.agenciasList = Agencias;
            */
            var idAgencia = (from pc in db.Terceros where pc.NIT == nit select pc.DEPENDENCIA).Single();

            ViewBag.agenciasList = new SelectList(db.agencias, "codigoagencia", "nombreagencia", idAgencia);


            var dto = new FNTC.Finansoft.Accounting.DAL.TercerosDAL().GetTerceros(nit).First();
            if(dto.SALARIO != null && dto.SALARIO != "")
            {
                dto.SALARIO = Convert.ToDecimal(dto.SALARIO).ToString("#,##");//Aqui formateamos el saldo
            }
            

            ViewBag.Post2 = ViewBag.action = "Edit";
            this.Init(ref dto);
            return PartialView(dto);
        }

        [HttpPost]
        public ActionResult Edit(TerceroDTO dto, FormCollection collection)
        {
            List<SelectListItem> Agencias = new List<SelectListItem>();   // Creo una lista
            IList<agencias> ListaDeTerceros = db.agencias.ToList();// extraigo los elementos desde la DB

            foreach (var item in ListaDeTerceros)		// recorro los elementos de la db
            {
                Agencias.Add(new SelectListItem { Text = item.nombreagencia, Value = item.codigoagencia.ToString() });  // agrego los elementos de la db a la primera lista que cree
                //text: el texto que se muestra
                //value: el valor interno del dropdown
            }

            ViewBag.agenciasList = Agencias;

            /*
            var salari = dto.SALARIO;
            var replacement = salari.Replace(','.ToString(), "");
            dto.SALARIO = replacement;
            */
            if (dto.SALARIO != null)
            {
                dto.SALARIO = dto.SALARIO.Replace('.'.ToString(), "");
            }           

            //si es personaj juridica nombre1 y apellido1 no son necesarios
            if (dto.EsPERJURIDICA)
            {
                ModelState["NOMBRE1"].Errors.Clear();
                ModelState["APELLIDO1"].Errors.Clear(); ModelState["NOMBRE1"].Errors.Clear();
                ModelState["APELLIDO1"].Errors.Clear();
                ModelState["ESTADOCIVIL"].Errors.Clear();
                ModelState["SEXO"].Errors.Clear();
                ModelState["PAISNAC"].Errors.Clear();
                ModelState["DEPTONAC"].Errors.Clear();
                ModelState["NACIO"].Errors.Clear();
                ModelState["PROFESION"].Errors.Clear();
                ModelState["VIVIENDA"].Errors.Clear();
                dto.FECHANAC = DateTime.Now;
            }

            IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
            

            if (ModelState.IsValid)
            {

                try
                {
                    var result =
                        new FNTC.Finansoft.Accounting.DAL.TercerosDAL().UpdateTercero(dto);
                   
                    return Json(result);//RedirectToAction("Index");
                   // return new EmptyResult();

                }
                catch
                {
                    //return View("Error");
                    //Response.StatusCode = 400;
                }

            }
            
            ViewBag.Post2 = "Edit?id=" + dto.NIT;
            this.Init(ref dto);
            return View(dto);

            /*
            var result = new FNTC.Finansoft.Accounting.DAL.TercerosDAL().UpdateTercero(dto);
            if (result)
            {
                ViewBag.Estado_ter = "actualizar";
                this.Init(ref dto);
                return View(dto);
            }
            else return HttpNotFound();
            */
            /*  ViewBag.estado_ter = "prueba var1";
              if (ModelState.IsValid)
               {
                  ViewBag.estado_ter = "prueba varnnn";
                  try
                   {
                      ViewBag.estado_ter = "prueba var3";
                      var result = new FNTC.Finansoft.Accounting.DAL.TercerosDAL().UpdateTercero(dto);
                       ViewBag.estado_ter = "actualizar";
                       ViewBag.ErrorMsg = "Datos Actualizados Correctamente";
                       return new EmptyResult();

                   }
                   catch
                   {
                      ViewBag.estado_ter = "actualizar2";
                      ViewBag.ErrorMsg = "Datos Actualizados Correctamente2";
                      return View("Error");
                       Response.StatusCode = 400;
                   }

               }
              ViewBag.estado_ter = "actualizar3";
              ViewBag.ErrorMsg = "Datos Actualizados Correctamente3";
              ViewBag.Post2 = "Edit?id=" + dto.NIT;
               this.Init(ref dto);
               return View(dto);*/
        }

        public ActionResult Delete(string nit, string mensaje)
        {
            var dto = new FNTC.Finansoft.Accounting.DAL.TercerosDAL().GetTerceros(nit).First();

            dto.SALARIO = Convert.ToDecimal(dto.SALARIO).ToString("#,##");//Aqui formateamos el saldo

            ViewBag.Post2 = ViewBag.action = "Edit";
            ViewBag.Post3 = ViewBag.action2 = mensaje;
            this.Init(ref dto);
            return PartialView(dto);
        }

        public ActionResult Mensaje ()
        {
            return View();
        }

        public ActionResult EliminarTercero(string nit)
        {
            string ficha = db.Database.SqlQuery<string>("SELECT idPersona FROM apo.FichasAportes WHERE idPersona='" + nit + "'").FirstOrDefault();
            if(ficha == null)
            {
            int noOfRowDeleted = db.Database.ExecuteSqlCommand("DELETE FROM ter.Terceros WHERE NIT='" + nit + "'");
            }
            else
            {
                return RedirectToAction("Mensaje");
            }
            return RedirectToAction("Index");
        }

        public List<SelectListItem> GetClaseID { get; set; }

        // [Compress]
        public ActionResult GetTerceros4S2(string term)
        {
            //var dal = new FNTC.Finansoft.Accounting.DAL.TercerosDAL();
            //var terceros = dal.GetTerceros(term);
            using (var ctx = new AccountingContext())
            {

                if (term != null && term == "")
                {
                    var result = new List<Tercero>();
                    var results = result.Select(x => new { id = x.NIT, text =x.NombreComercial+" "+ x.NOMBRE1+" "+x.NOMBRE2+" "+x.APELLIDO1+" "+x.APELLIDO2 });
                    return Content(JsonConvert.SerializeObject(new { results = results.ToList() }, Formatting.None), "application/json");

                }

                if (term != null)
                {
                    var result = ctx.Terceros.Where(x => x.NOMBRE.Contains(term) || x.NIT.Contains(term) || x.APELLIDO2.Contains(term) || x.NOMBRE2.Contains(term) || x.NOMBRE1.Contains(term) || x.APELLIDO1.Contains(term));
                    var results = result.Select(x => new { id = x.NIT, text = x.NombreComercial + " " + x.NOMBRE1 + " " + x.NOMBRE2 + " " + x.APELLIDO1 + " " + x.APELLIDO2 });
                    //return Json(new { results = results.ToList() }, JsonRequestBehavior.AllowGet);
                    return Content(JsonConvert.SerializeObject(new { results = results.ToList() }, Formatting.None), "application/json");
                }
                else
                {
                    var result = ctx.Terceros.ToList();
                    var results = result.Select(x => new { id = x.NIT, text = x.NombreComercial + " " + x.NOMBRE1 + " " + x.NOMBRE2 + " " + x.APELLIDO1 + " " + x.APELLIDO2 });
                    return Content(JsonConvert.SerializeObject(new { results = results.ToList() }, Formatting.None), "application/json");
                }

            }
        }


        public ActionResult GetTerceroByNIT(string nit = "")
        {

            if (nit.Length > 0)
            {

                var dto = new TercerosDAL().GetTerceroByNIT(nit);
                if (dto != null)
                {
                    return Json(dto, JsonRequestBehavior.AllowGet);
                }

                return Json(false, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id)
        {
            return View();
        }


        public ActionResult Nuevo()
        {
            var dal = new FNTC.Finansoft.Accounting.DAL.TercerosDAL();
            var terceros = dal.GetTiposTerceros();

            return View(terceros);

        }

        public ActionResult Test()
        {
            return View();
        }

        private void Init(ref TerceroDTO dto)
        {
            var ctx = new AccountingContext();
            dto._paises = ctx.Pais.Select(p => new SelectListItem() { Text = p.Nom_pais + "-" + p.Id_pais, Value = p.Id_pais.ToString() }).ToList();

            dto._empresas = ctx.Terceros.Where(t => t.EsPERJURIDICA == true).Select(t =>
            new SelectListItem() { Text = t.NombreComercial, Value = t.NIT }
            ).ToList();

            dto._profesiones = ctx.Profesion.Select(p =>
            new SelectListItem() { Text = p.Nom_prof + "-" + p.Id_prof, Value = p.Id_prof.ToString() }).ToList();

            dto._regimen = new FNTC.Framework.Params.DAL.ParamsDAL()
                .GetParamValues("REGIMEN").Select(x => new SelectListItem() { Text = x.Valor, Value = x.Codigo }).ToList();

            dto._sexo = new FNTC.Framework.Params.DAL.ParamsDAL()
                .GetParamValues("GENERO").Select(x => new SelectListItem() { Text = x.Valor, Value = x.Codigo }).ToList();

            dto._estadocivil = new FNTC.Framework.Params.DAL.ParamsDAL()
                .GetParamValues("ECIVIL").Select(x => new SelectListItem() { Text = x.Valor, Value = x.Codigo }).ToList();

            dto._tiposvivienda = new FNTC.Framework.Params.DAL.ParamsDAL()
                .GetParamValues("TIPOVIVIENDA").Select(x => new SelectListItem() { Text = x.Valor, Value = x.Codigo }).ToList();

            dto._clasesID = new FNTC.Framework.Params.DAL.ParamsDAL()
                .GetParamValues("TIPODOCUMENTO")
                .Select(x => new SelectListItem()
                {
                    Value = x.Codigo,
                    Text = x.Valor
                })
                    .ToList();

        }

        public List<Tercero> ConsultarTerceros()
        {
            using (var contexto = new AccountingContext())
            {
                return contexto.Terceros.ToList();
            }
        }


        public List<tercerosEntidadDos> ConsultartercerosEntidadDos()
        {
            using (var contexto = new AccountingContext())
            {
                return contexto.tercerosEntidadDos.ToList();
            }
        }


    }

}
