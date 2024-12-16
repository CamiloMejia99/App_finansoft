using FNTC.Finansoft.Accounting.BLL;
using FNTC.Finansoft.Accounting.BLL.Comprobantes;
using FNTC.Finansoft.Accounting.DTO;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace FNTC.Finansoft.UI.Areas.Accounting.Controllers.Movimientos
{
    public class ComprobantesController : Controller
    {
        // GET: Accounting/Comprobantes
        public ActionResult Index()
        {
            using (var ctx = new AccountingContext())
            {
                var comprobantes = ctx.Comprobantes.ToList();
                return View(comprobantes);
            }
        }
        /*
        public ActionResult IndexMovimientosEditados()
        {
            using (var ctx = new FNTC.Finansoft.Accounting.DAL.Model.AccountingContext())
            {
                var comprobantes = ctx.Comprobantes.ToList();
                return View(comprobantes);
            }
        }
        */
        public ActionResult GetComprobante(string tipo, string numero)
        {
            //debio construit un comporbante BO
            var comprobante = new FNTC.Finansoft.Accounting.BLL.Comprobantes.ComprobantesBLL().GetComprobante(tipo, numero);
            Session["pruebas"] = comprobante;
            TempData["comprobante"] = comprobante;
            //return new Areas.Accounting.Controllers.Movimientos.MovimientosController().Pruebas();
            return RedirectToAction("../Accounting/Movimientos/Pruebas", new { output = "pdf" });
        }

        public ActionResult GetComprobanteEditado(string tipo, string numero, string vecesEditado)
        {
            //debio construit un comporbante BO
            var comprobante = new FNTC.Finansoft.Accounting.BLL.Comprobantes.ComprobantesBLL().GetComprobanteEditado(tipo, numero, vecesEditado);
            Session["pruebas"] = comprobante;
            TempData["comprobante"] = comprobante;
            //return new Areas.Accounting.Controllers.Movimientos.MovimientosController().Pruebas();
            return RedirectToAction("../Accounting/Movimientos/Pruebas", new { output = "pdf" });
        }

        public ActionResult EditarComprobante(string tipo, string numero)
        {
            //debio construit un comporbante BO
            var comprobante = new FNTC.Finansoft.Accounting.BLL.Comprobantes.ComprobantesBLL().GetComprobante(tipo, numero);
            //Session["pruebas"] = comprobante;
            //TempData["comprobante"] = comprobante;
            //return new Areas.Accounting.Controllers.Movimientos.MovimientosController().Pruebas();
            //return RedirectToAction("../Accounting/Movimientos/Pruebas", new { output = "pdf" });
            return RedirectToAction("EditComprobante", new { Controller = "Movimientos", Area = "Accounting", tipo = tipo, numero= numero});
        }

        //public ActionResult GetComprobantes(string term = "")
        //{
        //    //Estos no existe aun
        //    //new FNTC.Finansoft.Accounting.BLL.Comprobantes.ComprobantesBLL().GetComprobantes();
        //    using (var ctx = new AccountingContext())
        //    {
        //        var comprobantes = ctx.Comprobantes.Where(c => c.TIPO != "CI4" && c.TIPO != "CI3").OrderBy(x => x.ANO).ThenByDescending(m => m.MES).ThenByDescending(d => d.DIA).ToList();
        //        var anulado = "";
        //        var botones = "";                
        //        var result = comprobantes.Select(
        //            (x, index) => new[]
        //            {
        //                x.TIPO,
        //                x.NUMERO,
        //                x.ANO + "/" +x.MES+ "/" +x.DIA,
        //                x.DETALLE,
        //                x.VRTOTAL.ToString(),
        //                anulado = x.ANULADO == true ? "ANULADO" : "",
        //                botones = x.ANULADO == false ? "<a CLASS='btn btn-inline btn-xs btn-primary fa fa-pencil OPCIONES' onclick='EditarComprobanteJs(this)'></a>&nbsp;&nbsp;"+"<a CLASS='btn btn-inline btn-xs btn-success fa fa-eye OPCIONES' onclick='VerComprobante(this)'></a>&nbsp;&nbsp;"+
        //                "<a CLASS='btn btn-inline btn-xs btn-warning fa fa-trash OPCIONES' onclick='Anular(this)'></a>&nbsp;&nbsp;" : "<a CLASS='btn btn-inline btn-xs btn-success fa fa-eye OPCIONES' onclick='VerComprobante(this)'></a>&nbsp;&nbsp;"+
        //                "<a CLASS='btn btn-inline btn-xs btn-danger fa fa-trash OPCIONES btn_disabled' onclick='Anular(this)'></a>"

        //            });

        //        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        serializer.MaxJsonLength = 500000000;
        //        var json = Json(new { data = result }, JsonRequestBehavior.AllowGet);
        //        json.MaxJsonLength = 500000000;
        //        return json;
        //    }

        //}

        //nuevo
        [HttpPost]
        public ActionResult GetComprobantes(string tipo, string fechaDesde, string fechaHasta)
        {
            using (var ctx = new AccountingContext())
            {
                NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;
                formato.CurrencyGroupSeparator = ".";
                var comprobantes = new ComprobantesBLL().SPGetComprobantes(tipo, fechaDesde, fechaHasta);
                var anulado = "";
                var botones = "";
                var result = comprobantes.Select(
                    (x, index) => new[]
                    {
                        x.TIPO,
                        x.NUMERO,
                        x.ANO + "/" +x.MES+ "/" +x.DIA,
                        x.DETALLE,
                        x.VRTOTAL.ToString("N0",formato),
                        anulado = x.ANULADO == true ? "ANULADO" : "",
                        botones = x.ANULADO == false ? "<a CLASS='btn btn-inline btn-xs btn-primary fa fa-pencil OPCIONES' onclick='EditarComprobanteJs(this)'></a>&nbsp;&nbsp;"+"<a CLASS='btn btn-inline btn-xs btn-success fa fa-eye OPCIONES' onclick='VerComprobante(this)'></a>&nbsp;&nbsp;"+
                        "<a CLASS='btn btn-inline btn-xs btn-warning fa fa-trash OPCIONES' onclick='Anular(this)'></a>&nbsp;&nbsp;" : "<a CLASS='btn btn-inline btn-xs btn-success fa fa-eye OPCIONES' onclick='VerComprobante(this)'></a>&nbsp;&nbsp;"+
                        "<a CLASS='btn btn-inline btn-xs btn-danger fa fa-trash OPCIONES btn_disabled' onclick='Anular(this)'></a>"

                    });

                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                serializer.MaxJsonLength = 500000000;
                var json = Json(new { data = result }, JsonRequestBehavior.AllowGet);
                json.MaxJsonLength = 500000000;
                return json;
            }

        }

        public ActionResult GetComprobantesEditados(string term = "")
        {
            //Estos no existe aun
            //new FNTC.Finansoft.Accounting.BLL.Comprobantes.ComprobantesBLL().GetComprobantes();
            using (var ctx = new AccountingContext())
            {
                var comprobantesEditado = ctx.ComprobantesEditadosAC.OrderBy(x => x.ANO).ThenByDescending(m => m.MES).ThenByDescending(d => d.DIA).ToList();
                var anulado = "";
                var botones = "";

                var result = comprobantesEditado.Select(
                    (x, index) => new[]
                    {
                        x.TIPO,
                        x.NUMERO,
                        x.NUMEROEDITADO,
                        x.DETALLE,
                        x.VRTOTAL.ToString(),
                        x.FECHAMODIFICACION.ToString(),
                        botones = "<a CLASS='btn btn-primary fa fa-eye OPCIONES' onclick='VerComprobante(this)'></a>"
                    });
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}