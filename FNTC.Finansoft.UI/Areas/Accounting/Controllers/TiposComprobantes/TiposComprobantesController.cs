
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Accounting;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Accounting.Controllers
{
    public class TiposComprobantesController : Controller
    {

        [Compress]
        public JsonResult GetTipoComprobantes (string term = "", int page = 1, int count = 10, int type = 2)
        {
            var tipoComprobante = new List<TipoComprobante>();

            using (var ctx = new AccountingContext())
            {
                tipoComprobante = ctx.TiposComprobantes.
                Where(pc => pc.CODIGO.Contains(term) || pc.NOMBRE.Contains(term))
                  .OrderBy(o => o.CODIGO).ToList();
            }
            var tc = tipoComprobante;
            return Json(tc, JsonRequestBehavior.AllowGet);
        }

        public void Init()
        {
            //actualizo las axilires en sesion
            new FNTC.Finansoft.UI.Areas.Accounting.Controllers.PlandeCuentas.PlanDeCuentasController().init();
        }

        public ActionResult Index()
        {
            //POR REPAIDEZ LOS PONGO ACA VAN EN BLL-DAL
            IEnumerable<TipoComprobanteDTO> listatipos = new List<TipoComprobanteDTO>();
            var dal = new FNTC.Finansoft.Accounting.DAL.Comprobantes.ComprobantesDAL();
            {
                listatipos = dal.GetAllTiposComprobantes();
            }
            return View(listatipos);
        }

        //MALDRU
        [Compress]
        public JsonResult GetComprobantes(string term)
        {
            var todas = new Finansoft.Accounting.DAL.Comprobantes.ComprobantesDAL().findComprobantes(term);                          
            return Json(todas, JsonRequestBehavior.AllowGet);
        }//MALDRU

        // GET: Accounting/Comprobantes/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Accounting/Comprobantes/Create
        public ActionResult Create()
        {
            var tc = new TipoComprobanteDTO();

            tc._formaDePago = this.GetFormaDePago(); //esto debe actualizasrse sgun lo que se seleccione
            tc._clasesComprobante = this.GetClasesComprobantes();

            tc._clasesComprobante.RemoveAll(x => x.Value == "SI");
            //    var action = (string)ViewBag.action;
            ViewBag.action = "Create";
            ViewBag.boton = "Crear";
            return PartialView(tc);
        }

        // POST: Accounting/Comprobantes/Create

        //[HttpPost]
        //public ActionResult Create(FormCollection col)
        //{
        //    return Json(false);
        //}


        [HttpPost]
        public ActionResult Create(TipoComprobanteDTO tcDTO)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var dal = new FNTC.Finansoft.Accounting.DAL.Comprobantes.ComprobantesDAL();
                    {
                        var grabo = dal.CreateTipoComprobante(tcDTO);
                    }
                    //returnUrl
                    //return RedirectToAction("Index");
                    return Redirect("/Dashboard/Default/catalogos?titulo=cat%C3%A1logo&menu=contabilidad");
                }
                catch
                {
                    ViewBag.action = "Create";
                    ViewBag.boton = "Crear";
                    tcDTO._clasesComprobante = this.GetClasesComprobantes();
                    tcDTO._clasesComprobante.RemoveAll(x => x.Value == "SI");
                    return View(tcDTO);
                }
            }
            //el modelo no es valido
            tcDTO._formaDePago = this.GetFormaDePago(); //esto debe actualizasrse sgun lo que se seleccione
            tcDTO._clasesComprobante = this.GetClasesComprobantes();
            return View(tcDTO);
        }




        #region TODO
        // GET: Accounting/Comprobantes/Edit/5
        public ActionResult Edit(string CODIGO)
        {
            //var tp = new FNTC.Finansoft.Accounting.DAL.Model.AccountingContext().TiposComprobantes.Find(CODIGO);
            var dto = new TipoComprobanteDTO();
            var dal = new FNTC.Finansoft.Accounting.DAL.Comprobantes.ComprobantesDAL();
            {
                dto = dal.GetComprobanteByCODIGO(CODIGO);
            }

            dto._formaDePago = GetFormaDePago(); //esto debe actualizasrse sgun lo que se seleccione
            var clase = new List<SelectListItem>();
            clase.Add(new SelectListItem() { Value = dto.CLASEComprobante, Text = dto.CLASEComprobante });
            dto._clasesComprobante = clase;

            ViewBag.action = "Edit";
            ViewBag.boton = "Editar";

            return View(dto);
        }

        // POST: Accounting/Comprobantes/Edit/5
        [HttpPost]
        public ActionResult Edit(TipoComprobanteDTO dto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var dal = new FNTC.Finansoft.Accounting.DAL.Comprobantes.ComprobantesDAL();
                    {

                        var grabo = dal.UpdateTipoComprobante(dto);

                    }
                    Response.StatusCode = 200;
                    return View("Error");

                    //return RedirectToAction("Index");
                    // return Redirect("/Dashboard/Default/catalogos?titulo=cat%C3%A1logo&menu=contabilidad");
                }
                catch
                {
                    dto._formaDePago = this.GetFormaDePago(); //esto debe actualizasrse sgun lo que se seleccione
                    dto._clasesComprobante = this.GetClasesComprobantes();
                    //return Redirect("/Dashboard/Default/catalogos?titulo=cat%C3%A1logo&menu=contabilidad");
                    return View(dto);
                }
            }
            //el modelo no es valido

            dto._formaDePago = this.GetFormaDePago(); //esto debe actualizasrse sgun lo que se seleccione
            dto._clasesComprobante = this.GetClasesComprobantes();
            return View(dto);
        }

        //[HttpPost]
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: Accounting/Comprobantes/Delete/5
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            try
            {
                //en la proxima iteracion debo hacer refactor

                using (var ctx = new AccountingContext())
                {
                    //verifico si hay movimientos con ese tipo de comprobante
                    var count = ctx.Comprobantes.Where(x => x.TIPO == id).Count();
                    if (count == 0)
                    {
                        ctx.TiposComprobantes.Remove(ctx.TiposComprobantes.Find(id));
                        var r = ctx.SaveChanges() > 0 ? true : false;
                        return Json(r);
                    }


                    Response.StatusCode = 404;
                    return Json(false);
                }




                //return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }
        #endregion

        #region Helpers

        public ActionResult GetConsecutivoComprobanteSegunClase(string clase)
        {

            if (clase == "")
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            using (var ctx = new AccountingContext())
            {

                try
                {
                    var mayor = ctx.TiposComprobantes.Where(x => x.CLASEComprobante == clase).Select(y => y.CODIGO.Substring(2, y.CODIGO.Length - 2)).ToList().OrderByDescending(u => u).FirstOrDefault();
                    if (mayor == null)
                        mayor = "0";
                    var consecutivo = clase + (Int32.Parse(mayor) + 1);
                    return Json(consecutivo, JsonRequestBehavior.AllowGet);

                }
                catch (Exception)
                {

                    return Json("error", JsonRequestBehavior.AllowGet);
                }

            }


        }


        [Obsolete("Esto debe venir de una tabla parametros")]
        private List<SelectListItem> GetClasesComprobantes()
        {

            var lista = new List<SelectListItem>();
            using (var ctx = new AccountingContext())
            {
                ctx.ClaseComprobante.ToList()
                    .ForEach(x => lista
                    .Add(new SelectListItem()
                    {
                        Text = x.Nombre,
                        Value = x.Codigo
                    }));
            }


            return lista;


        }

        private List<SelectListItem> GetFormaDePago()
        {
            //   throw new NotImplementedException();
            var fpDTO = new FNTC.Finansoft.Accounting.DAL.Comprobantes.FormasPagoDAL().GetAllFormaDePago();
            var listaformas =
                fpDTO.Select(x => new SelectListItem() { Text = x.Nombre, Value = x.ID.ToString() }).ToList();
            return listaformas;
        }

        private List<SelectListItem> GetFormaDePagoByComprobante(string comprobanteId)
        {
            //  throw new NotImplementedException();
            var fpDTO = new FNTC.Finansoft.Accounting.DAL.Comprobantes.FormasPagoDAL().GetAllFormaDePago();
            var listaformas =
                fpDTO.Select(x => new SelectListItem() { Text = x.Nombre, Value = x.ID.ToString() }).ToList();
            return listaformas;
        }


        public ActionResult ValidateConsecutivo(string consecutivo, string tipoComprobante)
        {
            try
            {
                //en la proxima iteracion debo hacer refactor
                using (var ctx = new AccountingContext())
                {
                    //verifico si hay movimientos con ese tipo de comprobante
                    var tipoExiste =
                        ctx.TiposComprobantes.Find(tipoComprobante) == null ? false : true;

                    if (!tipoExiste)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }

                    var count =
                        ctx.Comprobantes.
                        Where(x => x.NUMERO == consecutivo && x.TIPO == tipoComprobante).
                        Count();
                    if (count == 0) //es valido
                    {
                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    Response.StatusCode = 404;
                    return Json(false, JsonRequestBehavior.AllowGet);
                }

                //return RedirectToAction("Index");
            }
            catch
            {
                return View("Error");
            }
        }
        #endregion
    }
}
