using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Controllers
{
    public class ParamsController : Controller
    {
        // GET: Params
        public ActionResult TiposNits()
        {
            //traer tipos nits
            var tiposNits = new List<SelectListItem> {
                new SelectListItem{
                Value = "1", Text = "C" },
                new  SelectListItem{
                Value = "2", Text = "E" },
                new SelectListItem {
                Value = "3", Text = "T" },
                new SelectListItem {
                Value = "4", Text = "P" },
                new  SelectListItem{
                Value = "5", Text = "R" },
                new  SelectListItem{
                Value = "6", Text = "N" }
            };
            var select = new SelectList(tiposNits);
         //   var helper = System.Web.Mvc.Html.SelectExtensions.DropDownList(new HtmlHelper(this.HttpContext,this.), "", new SelectList(tiposNits), new { });
          //  return View("_TiposNitsDDL",tiposNits);
            ViewBag.Tipos = tiposNits;
            return PartialView("_TiposNitsDDL");
        }


        public ActionResult TiposRegimenes()
        {
            //traer tipos nits
            var tiposRegimenes = new List<SelectListItem> {
                new SelectListItem{
                Value = "1", Text = "SIMPLIFICADO" },
                new  SelectListItem{
                Value = "2", Text = "COMUN" },
                new SelectListItem {
                Value = "3", Text = "GRAN CONTRIBUYENTE" },
                //new SelectListItem {
                //Value = "4", Text = "P" },
                //new  SelectListItem{
                //Value = "5", Text = "R" },
                //new  SelectListItem{
                //Value = "6", Text = "N" }
            };
            var select = new SelectList(tiposRegimenes);
            //   var helper = System.Web.Mvc.Html.SelectExtensions.DropDownList(new HtmlHelper(this.HttpContext,this.), "", new SelectList(tiposNits), new { });
            //  return View("_TiposNitsDDL",tiposNits);
            ViewBag.TiposRegimenes = tiposRegimenes;
            return PartialView("_TiposRegimenes");
        }
    }
}
