using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace FNTC.Finansoft.UI.Areas.TimerSession.Controllers
{
    public partial class timerController : System.Web.UI.Page
    {
        // GET: TimerSession/timmer
        //public ActionResult Index()
        //{
        //    return View();
        //}

        [WebMethod()]
        public static bool KeepActiveSession(string nit)
        {
            

            if (HttpContext.Current.Session["fecha" + nit] != null
                && HttpContext.Current.Session["nit" + nit] != null
                && HttpContext.Current.Session["IDcuadre" + nit] != null
                && HttpContext.Current.Session["cod_caja" + nit] != null
                && HttpContext.Current.Session["Serie" + nit] != null
                && HttpContext.Current.Session["serie" + nit] != null
                && HttpContext.Current.Session["agencia" + nit] != null
                && HttpContext.Current.Session["tope_maximo" + nit] != null
                && HttpContext.Current.Session["Factura" + nit] != null
                && HttpContext.Current.Session["CUENTA" + nit] != null
                && HttpContext.Current.Session["retiros_efectivo" + nit] != null
                && HttpContext.Current.Session["retiros_cheque" + nit] != null
                && HttpContext.Current.Session["consignacion_efectivo" + nit] != null
                && HttpContext.Current.Session["consignacion_cheque" + nit] != null
                && HttpContext.Current.Session["tope" + nit] != null
                && HttpContext.Current.Session["nombre_caja" + nit] != null
                && HttpContext.Current.Session["nombre" + nit] != null
                && HttpContext.Current.Session["cta_cheques" + nit] != null
                && HttpContext.Current.Session["cta_efectivo" + nit] != null
                && HttpContext.Current.Session["comp_ingreso" + nit] != null
                && HttpContext.Current.Session["comp_egreso" + nit] != null
                && HttpContext.Current.Session["cc_transacciones" + nit] != null
                && HttpContext.Current.Session["con_fin" + nit] != null
                && HttpContext.Current.Session["con_ini" + nit] != null)
                return true;
            else
                return false;
        }

    }
}