using AutoMapper;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.Accounting;
using FNTC.Finansoft.Accounting.DTO.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;

namespace FNTC.Finansoft.UI.Areas.Accounting.Controllers
{
    public class CentroCostoController : Controller
    {

        public ActionResult Index()
        {
            var entities = new AccountingContext().CentrosCostos.ToList();

            //automapper
            var configToDTO =
                new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<CentroCosto, CentroCostoDTO>();
                });

            var mapper = configToDTO.CreateMapper();
            List<CentroCostoDTO> lista = new List<CentroCostoDTO>();

            foreach (var item in entities)
            {
                var dto = mapper.Map<CentroCostoDTO>(item);
                lista.Add(dto);
            }

            return PartialView(lista.OrderBy(x => Int32.Parse(x.Codigo)));
        }

        public ActionResult Create()
        {
            ViewBag.action = "Create";
            ViewBag.boton = "Crear";
            //obtengo todos los codigos. los convierto en numericos los ordeno y botengo el ultimo
            int consecutivo = 0;

            var codigos = new AccountingContext()
                .CentrosCostos.Select(x => x.Codigo).ToList();

            if (codigos.Count > 0)
            {
                List<int> f = new List<int>();
                foreach (var item in codigos)
                {
                    f.Add(Int32.Parse(item));
                }
                consecutivo = f.OrderByDescending(x => x).First() + 1;
            }

            //.OrderByDescending(x => x).First();
            CentroCostoDTO cc = new CentroCostoDTO();
            cc.Codigo = "0" + consecutivo;

            ViewBag.action = "Create";
            ViewBag.boton = "Nuevo";
            return View("CentroCostoForm", cc);

        }

        [HttpPost]
        public ActionResult Create(CentroCostoDTO cc)
        {
            try
            {
                var configToDTO =
                      new MapperConfiguration(cfg =>
                      {
                          cfg.CreateMap<CentroCostoDTO, CentroCosto>();
                      });

                var mapper = configToDTO.CreateMapper();

                var c = mapper.Map<CentroCosto>(cc);

                var ctx = new AccountingContext();

                ctx.CentrosCostos.Add(c);

                int r = ctx.SaveChanges();
                return Json(r, JsonRequestBehavior.AllowGet);
                //return View("Index");
                //  return Json("", JsonRequestBehavior.AllowGet);
                //  return RedirectToAction("Catalogos", "default", new { Area = "Dashboard" });
                //        ("~/Dashboard/Default/catalogos");
            }
            catch
            {
                return View("Error");
            }

        }

        public ActionResult Edit(string id)
        {
            ViewBag.action = "edit";
            ViewBag.boton = "Editar";
            //styo va asi pro rapides en el desarrollo
            var ctx = new AccountingContext();
            var c = ctx.CentrosCostos.Find(id);
            var configToDTO =
                 new MapperConfiguration(cfg =>
                 {
                     cfg.CreateMap<CentroCosto, CentroCostoDTO>();
                 });

            var mapper = configToDTO.CreateMapper();
            var ccDTO = mapper.Map<CentroCostoDTO>(c);

            return PartialView("CentroCostoForm", ccDTO);
        }

        [HttpPost]
        public ActionResult Edit(CentroCostoDTO ccDTO)
        {

            var codigp = ccDTO.Codigo;
            try
            {
                var ctx = new AccountingContext();
                var configToDTO =
                   new MapperConfiguration(cfg =>
                   {
                       cfg.CreateMap<CentroCostoDTO, CentroCosto>();
                   });

                var mapper = configToDTO.CreateMapper();

                var cc = mapper.Map<CentroCosto>(ccDTO);

                ctx.Entry(cc).State = System.Data.Entity.EntityState.Modified;

                int rows = ctx.SaveChanges();
                //  return RedirectToAction("Index");
                return Json("/Dashboard/Default/catalogos?titulo=catálogo&menu=contabilidad", JsonRequestBehavior.AllowGet);
                return Json(rows);
            }
            catch (Exception e)
            {
                return View("Create", ccDTO);
            }
        }

        [HttpPost]
        public ActionResult Delete(string codigo, FormCollection col)
        {
            //esto va en DAL --------------------PILAS
            //verifica si tiene saldos el centro de costos

            //obtengo el codigo

            using (var ctx = new AccountingContext())
            {
                var _scc = ctx.SaldosCCs.FirstOrDefault(x => x.CCOSTO == codigo);
                var tieneSaldos = _scc == null ? false : true;
                if (tieneSaldos)
                {
                    var r = new Result();
                    r.ResultCode = ResultCode.Error;
                    r.ErrorsWithKey.Add("CCOSTO", "El Centro de Costos ya tiene saldos");
                    ViewBag.result = r;
                    ViewBag.success = false;
                    return Json(r, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    var cc2BeRemoved = new CentroCosto() { Codigo = codigo };
                    ctx.CentrosCostos.Attach(cc2BeRemoved);
                    ctx.CentrosCostos.Remove(cc2BeRemoved);
                    try
                    {

                        var rows = ctx.SaveChanges();
                        return Json(rows, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception)
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);

                    }
                }
            }


        }


        public ActionResult GetCC4Selects(string term = "")
        {
            var entities = new AccountingContext().CentrosCostos.Where(x => x.Nombre.Contains(term) || x.Codigo.Contains(term)).ToList();



            return Json(new { results = entities.Select(x => new { id = x.Codigo, text = x.Nombre }) }, JsonRequestBehavior.AllowGet);
        }
    }
}
