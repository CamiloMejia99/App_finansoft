
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Framework;
using FNTC.Framework.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.Tools.Controllers
{
    [Compress]
    public class GeoController : Controller
    {
        // GET: Tools/Geo

        GeoDAL geoDal;

        public GeoController()
        {
            geoDal = new GeoDAL();
        }

        public ActionResult Index()
        {

            var methods = new List<string>();
            var type = typeof(GeoController);

            StringBuilder sb = new StringBuilder();
            foreach (var method in type.GetMethods())
            {
                var parameters = method.GetParameters();
                var parameterDescriptions = string.Join
                    (", ", method.GetParameters()
                                 .Select(x => x.ParameterType + " " + x.Name)
                                 .ToArray());
                if (method.IsPublic && method.ReturnType == typeof(ActionResult))
                {
                    sb.AppendFormat("{0} {1} ({2} ({3})",
                                     method.ReturnType,
                                     method.Name,
                                     
                                     parameterDescriptions,

                                     method.Attributes);
                }
            }
            
            return Content(sb.ToString());
        }

        [Compress]
        public ActionResult GetDepartamentosByPaisId(string paisId, bool fullInfo = false)
        {
            var deptos = new List<GeoBO>();
            deptos = geoDal.GetDepartamentosByPaisId(paisId);

            return Json(deptos, JsonRequestBehavior.AllowGet);
        }

        [Compress]
        public ActionResult GetDepartamentosByPaisId2(string paisId)
        {
            var pais = Convert.ToInt32(paisId);
            using (var ctx = new AccountingContext())
            {
                var deptos = ctx.Departamento.Where(d => d.Pais_dep == pais);

                return Json(deptos
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Nom_dep + " - " + x.Id_dep,
                        Value = x.Id_dep.ToString()
                    }).ToList().OrderBy(x => x.Value), JsonRequestBehavior.AllowGet);
            }
        }
        [Compress]
        public ActionResult GetPlanos(string EmpresaId)
        {
            var CodEmpresa = Convert.ToInt32(EmpresaId);
            using (var ctx = new AccountingContext())
            {

                var EmpresaCod = ctx.PlanoEmpresa.Where(s => s.id == CodEmpresa).FirstOrDefault().CODIGOEMP;
                var EMPlano = ctx.PlanoEmpresa.Where(d => d.CODIGOEMP == EmpresaCod);

                return Json(EMPlano
                    .Select(x => new SelectListItem()
                    {
                        Text = x.NOMPLANO.ToString(),
                        Value = x.NOMPLANO.ToString()
                    }).ToList().OrderBy(x => x.Value), JsonRequestBehavior.AllowGet);
            }
        }

        [Compress]
        public ActionResult GetDepartamentoById(string deptoId)
        {
            var depto = geoDal.GetDepartamentoById(deptoId);

            return Json(depto, JsonRequestBehavior.AllowGet);

        }

        [Compress]
        public ActionResult GetMunicipiosbyDeptoId(string deptoId)
        {
            var municipios = new List<GeoBO>();

           // municipios = geo;
           municipios = geoDal.GetMunicipiosbyDeptoId(deptoId);

            return Json(municipios, JsonRequestBehavior.AllowGet);

        }

        
        public ActionResult GetMunicipiosbyDeptoId2(string deptoId)
        {
            var depto = Convert.ToInt32(deptoId);
            using (var ctx = new AccountingContext())
            {
                var municipios = ctx.Municipio.Where(d => d.Dep_muni == depto);

                return Json(municipios
                    .Select(x => new SelectListItem()
                    {
                        Text = x.Nom_muni + " - " + x.Id_muni,
                        Value = x.Id_muni.ToString()
                    }).ToList().OrderBy(x => x.Value), JsonRequestBehavior.AllowGet);
            }
        }

        
        private List<GeoBO> GetAllMunicipiosbyPaisId(string paisId="169")
        {
            var allMunicipios = System.Web.HttpRuntime.Cache["AllMunicipios"] as List<GeoBO>;
            

            if (allMunicipios == null)
            {
                allMunicipios = geoDal.GetAllMunicipiosbyPaisId(paisId);
                System.Web.HttpRuntime.Cache["AllMunicipios"] = allMunicipios;


            }

            return allMunicipios;
            //return Json(allMunicipios.Select(x => new SelectListItem() { Text = x.MunicipioNombre + " - " + x.MunicipioId, Value = x.MunicipioId }).OrderBy(x => x.Value), JsonRequestBehavior.AllowGet);

        }
        public void setAllMunicipiosToCache()
        {
            //GetAllMunicipiosbyPaisId
        }


        public ActionResult Search(string term)
        {

            var results = geoDal.Search(term);
            return Json(results, JsonRequestBehavior.AllowGet);

        }

    }
}