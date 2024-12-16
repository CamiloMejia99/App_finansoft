using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace FNTC.Finansoft.UI.Areas.TimerSession.Controllers
{
    public partial class ValidarSessionController : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            Session["fecha" + User.Identity.Name] = true;
            Session["nit" + User.Identity.Name] = true;
            Session["IDcuadre" + User.Identity.Name] = true;
            Session["cod_caja" + User.Identity.Name] = true;
            Session["actual" + User.Identity.Name] = true;
            Session["Serie" + User.Identity.Name] = true;
            Session["serie" + User.Identity.Name] = true;
            Session["agencia" + User.Identity.Name] = true;
            Session["tope_maximo" + User.Identity.Name] = true;
            Session["Factura" + User.Identity.Name] = true;
            Session["CUENTA" + User.Identity.Name] = true;
            Session["retiros_efectivo" + User.Identity.Name] = true;
            Session["retiros_cheque" + User.Identity.Name] = true;
            Session["consignacion_efectivo" + User.Identity.Name] = true;
            Session["consignacion_cheque" + User.Identity.Name] = true;
            Session["tope" + User.Identity.Name] = true;
            Session["nombre_caja" + User.Identity.Name] = true;
            Session["nombre" + User.Identity.Name] = true;
            Session["cta_cheques" + User.Identity.Name] = true;
            Session["cta_efectivo" + User.Identity.Name] = true;
            Session["comp_ingreso" + User.Identity.Name] = true;
            Session["comp_egreso" + User.Identity.Name] = true;
            Session["cc_transacciones" + User.Identity.Name] = true;
            Session["con_fin" + User.Identity.Name] = true;
            Session["con_ini" + User.Identity.Name] = true;


        }

        [WebMethod()]
        public static bool KeepActiveSession(string nit)
        {

            if (HttpContext.Current.Session["fecha"+nit] != null
                && HttpContext.Current.Session["nit"+nit] != null
                && HttpContext.Current.Session["IDcuadre"+nit] != null
                && HttpContext.Current.Session["cod_caja"+nit] != null
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

        [WebMethod()]
        public static void SessionAbandon()
        {
            
            //HttpContext.Current.Session.Remove("datos");
            //HttpContext.Current.Session.Remove("datos");
            //HttpContext.Current.Session.Remove("datos");
            //HttpContext.Current.Session.Remove("datos");
            //HttpContext.Current.Session.Remove("datos");
            //HttpContext.Current.Session.Remove("datos");
            //HttpContext.Current.Session.Remove("datos");
            //HttpContext.Current.Session.Remove("datos");
            //HttpContext.Current.Session.Remove("datos");
            //HttpContext.Current.Session.Remove("datos");
            //HttpContext.Current.Session.Remove("datos");
            //HttpContext.Current.Session.Remove("datos");
            //HttpContext.Current.Session.Remove("datos");
            //HttpContext.Current.Session.Remove("datos");
            //HttpContext.Current.Session.Remove("datos");
            //HttpContext.Current.Session.Remove("datos");
            //HttpContext.Current.Session.Remove("datos");
            //HttpContext.Current.Session.Remove("datos");
            //HttpContext.Current.Session.Remove("datos");
        }
    }
}