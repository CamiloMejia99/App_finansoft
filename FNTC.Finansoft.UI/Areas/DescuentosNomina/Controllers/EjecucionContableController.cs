using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.DescuentosNomina;
using FNTC.Finansoft.Accounting.BLL.DescuentosNomina;

namespace FNTC.Finansoft.UI.Areas.DescuentosNomina.Controllers
{
    public class EjecucionContableController : Controller
    {
        private AccountingContext db = new AccountingContext();
        public ActionResult ListaDeEmpresasEC()
        {

            return View(db.RelacionPlanosEmpresa.Where(a => a.CodigoPlano == 0 && a.EstadoRelacionPlanosEmpresa == true).ToList());
        }
        public ActionResult ListaPlanosEmpresaEC(int idRelacion)
        {

            var Empresa = (from s in db.RelacionPlanosEmpresa where s.IdRelacionPlanosEmpresa == idRelacion select s.CodigoEmpresa).FirstOrDefault();
            var NombreEmpresa = (from s in db.Terceros where s.NIT == Empresa select s.NOMBRE).FirstOrDefault();
            ViewBag.NombreEmpresa = NombreEmpresa;
            return View(db.RelacionPlanosEmpresa.Where(a => a.CodigoEmpresa == Empresa && a.CodigoPlano != 0 && a.EstadoRelacionPlanosEmpresa == true).ToList());

        }
        public ActionResult DataRelacionplanosEmpresaEC(int idRelacion)
        {
            var Plano = (from s in db.RelacionPlanosEmpresa where s.IdRelacionPlanosEmpresa == idRelacion select s.CodigoPlano).FirstOrDefault();
            var Empresa = (from s in db.RelacionPlanosEmpresa where s.IdRelacionPlanosEmpresa == idRelacion select s.CodigoEmpresa).FirstOrDefault();
            var idPlano = (from s in db.RelacionPlanosEmpresa where s.IdRelacionPlanosEmpresa == idRelacion select s.CodigoPlano).FirstOrDefault();
            var NombrePlano = (from s in db.EstructuraPlanos where s.IdEstructuraPlanos == idPlano select s.NombreEstructuraPlanos).FirstOrDefault();
            ViewBag.NombrePlano = NombrePlano;
            ViewBag.Relacion = idRelacion;
            return View(db.RelacionPlanosDiscriminacion.Where(a => a.IdPlano == Plano && a.IdEmpresa == Empresa).ToList());

        }
    }
}