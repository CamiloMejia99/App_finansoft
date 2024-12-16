using FNTC.Finansoft.Accounting.BLL.Comprobantes;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Accounting;
using FNTC.Finansoft.Accounting.DTO.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace FNTC.Finansoft.UI.Areas.Accounting.Controllers.Comprobantes
{
    public class FormasPagoController : Controller
    {
        private FormasPagoBLL bll { get; set; }
        public FormasPagoController()
        {
            this.bll = new FNTC.Finansoft.Accounting.BLL.Comprobantes.FormasPagoBLL();
        }

        // GET: Accounting/FormasPago
        public ActionResult Index()
        {
            var result = bll.GetAllFormaDePago();
            //devuelvo todas las vistas
            if (null == result)
            {
                result = new List<FormasDePagoDTO>();
            }
            return View(result);
        }

        // GET: Accounting/FormasPago/Create
        public ActionResult Create()
        {
            ViewBag.textButton = TempData["textButton"] == null ? "Crear" : TempData["textButton"]; ;
            ViewBag.PostTO = TempData["PostTO"] == null ? "Create" : TempData["PostTO"];
            //traigo la entidad
            var model = TempData["formaPago"] as FormasDePagoDTO;
            if (null == model)
            {
                model = new FormasDePagoDTO();
            }
            var ctasAuxiliares = new AccountingContext().Auxiliares.ToList();
            ViewBag.Auxiliares = GetAuxiliares();

            ViewBag.action = ViewBag.PostTO;
            ViewBag.boton = ViewBag.textButton;

            return PartialView("Create", model);
        }

        // POST: Accounting/FormasPago/Create
        [HttpPost]
        public ActionResult Create(FormasDePagoDTO formaPago)
        {

            var result = new Result();

            formaPago.Tipo = "1";
            ModelState.Remove("Tipo");
            try
            {
                // TODO: Add insert logic here
                if (ModelState.IsValid)
                {
                    //persistir
                    var bll = new FNTC.Finansoft.Accounting.BLL.Comprobantes.FormasPagoBLL();
                    result = bll.CreateFormaDePago(formaPago);
                    if (result.Errors.Count > 0)
                    {
                        ModelState.AddModelError("Nombre", "Ya existe un registro con el mismo nombre o cuenta");
                        ModelState.AddModelError("CodigoCuenta", "Ya existe un registro con el mismo nombre o cuenta");

                        ViewBag.textButton = "Crear";



                        ViewBag.PostTO = TempData["PostTO"] == null ? "Create" : TempData["PostTO"];

                        ViewBag.Auxiliares = GetAuxiliares();

                        return Json(false);

                    }
                    else
                    {
                        return Json(true);
                    }
                }
                // Response.Write("<script>$('#mostrar').load('Dashboard/Default/catalogos2?url=Accounting/FormasPago/index&menu=contabilidad') </script>");
                //return RedirectToRoute(new
                //{
                //    controller = "default",
                //    area = "dashboard",
                //    action = "catalogos",
                //    titulo = "catálogo",
                //    menu = "contabilidad"
                //});

                return Json(new { result = false, message = "Modelo no es valido" });
                return View("catalogos2.cshtml", new { Url = "url", menu = "contabilidad" });
                //return Json(result);
                //http://localhost:4607/Dashboard/Default/catalogos2?url=Accounting/FormasPago/index&menu=contabilidad
            }
            catch
            {
                return PartialView(formaPago);
            }
        }


        public object GetAuxiliares()
        {
            var ctasAuxiliares = new
                                AccountingContext()
                                .Auxiliares.ToList()
                                .Select(X => new { CODIGO = X.CODIGO, NOMBRE = X.CODIGO + " - " + X.NOMBRE + " - " + X.NATURALEZA });

            return ctasAuxiliares;
        }
        // GET: Accounting/FormasPago/Edit/5
        public ActionResult Edit(int id)
        {
            TempData["textButton"] = "Editar";
            TempData["PostTO"] = "Edit";
            //get object to edit
            var dto = bll.GetFormaDePago(id);
            dto.ID = id;
            TempData["formaPago"] = dto;


            ViewBag.Auxiliares = GetAuxiliares();


            ViewBag.action = "Edit";
            ViewBag.boton = "Editar";

            return PartialView("Create", dto);
        }

        // POST: Accounting/FormasPago/Edit/5
        [HttpPost]
        public ActionResult Edit(FormasDePagoDTO fpDTO)
        {
            try
            {
                // TODO: Add update logic here
                var r = bll.UpdateFormaDePago(fpDTO);
                if (r)
                {
                    return RedirectToRoute(new
                    {
                        controller = "default",
                        area = "dashboard",
                        action = "catalogos",
                        titulo = "catálogo",
                        menu = "contabilidad"
                    });

                    //return RedirectToAction("Index");

                }



                else
                {
                    ModelState.AddModelError("error", "No se pudo modificar");
                    return View(fpDTO);
                }
            }
            catch
            {
                return View("Index");
            }
        }

        // POST: Accounting/FormasPago/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            using (var ctx = new AccountingContext())
            {
                var fPago = ctx.FormasPago.Find(id);
                var _scc = ctx.Comprobantes.FirstOrDefault(x => x.FPAGO == fPago.CodigoCuenta);
                var tieneSaldos = _scc == null ? false : true;
                if (tieneSaldos)
                {
                    var r = new Result();
                    r.ResultCode = ResultCode.Error;
                    r.ErrorsWithKey.Add("FPAGO", "la forma de pago ya fue usada");
                    ViewBag.result = r;
                    ViewBag.success = false;
                    Response.StatusCode = 404;
                    return Json(r, JsonRequestBehavior.AllowGet);
                }
                else
                {



                    ctx.FormasPago.Remove(fPago);
                    try
                    {

                        var rows = ctx.SaveChanges();
                        return Json(rows, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception)
                    {
                        Response.StatusCode = 404;
                        return Json(false, JsonRequestBehavior.AllowGet);

                    }
                }
            }
        }

        public ActionResult GetFormasPagoByClaseComprobante(string claseComprobante)
        {
            var fp = new FNTC.Finansoft.Accounting.DAL.Comprobantes.FormasPagoDAL().GetFormaDePagoByClaseComprobanteId(claseComprobante);

            return Json(fp);
        }

        public ActionResult GetFormasPagoById(int id)
        {
            var fp = new FNTC.Finansoft.Accounting.DAL.Comprobantes.FormasPagoDAL().GetFormaDePagobyId(id);

            return Json(fp, JsonRequestBehavior.AllowGet);
        }

        #region TESTS
        public ActionResult Create2()
        {
            return View("Create2", new FormasDePagoDTO());
        }
        #endregion
    }
}
