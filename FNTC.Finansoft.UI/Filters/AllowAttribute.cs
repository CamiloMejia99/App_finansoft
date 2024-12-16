using Ingenio.Controllers.Seguridad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace FNTC.Finansoft.UI.Filters
{
    public class AllowAttribute : ActionFilterAttribute
    {
        //Cifrado cifrado = new Cifrado();
        //public string action { get; set; }

        //public bool oficina { get; set; }
        //public string http { get; set; }

        //public override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    var controller = filterContext.Controller;
        //    string  action = filterContext.ActionDescriptor.ActionName;
        //    string  controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
        //    string urlReturn = "/" + con + "/" + action;
        //    var aaaa = filterContext.HttpContext.Handler;
        //    var contex = controller.ControllerContext.HttpContext;
        //    var g = filterContext.RequestContext;
            
        //    try
        //    {
        //        if (g.HttpContext.Request.Cookies["user"] != null)
        //        {
        //            var userInfo = cifrado.DecryptString(g.HttpContext.Request.Cookies["user"].Value);
        //            var contraseña = userInfo.Substring(0, 40);
        //            var user = userInfo.Substring(40);
                    
        //            //UsuariosRoles_Usu u = (UsuariosRoles_Usu)contex.Session["userInfo"];
        //            UsuarioBll usuariobll = new UsuarioBll();
        //            Usuarios_Usu u = usuariobll.GetUserByName(user);

        //            if (u == null)
        //            {
        //                Redirect(contex, filterContext, "/Home/Index");
        //                return;
        //            }

        //            //string aaa = System.Net.Dns.GetHostName();   
        //            DateTime fechaActual = DateTime.Now;
        //            DateTime fKey = u.DateKey == null ? fechaActual : (DateTime)u.DateKey;

        //            int fechaMenor = DateTime.Compare(fechaActual, fKey);
        //            string userKey = u.Key;
        //            if (((fechaMenor > 0) || (contraseña != u.PasswordHash)))
        //            {
        //                contex.Response.Cookies["urlReturn"].Value = urlReturn;
        //                filterContext.Result = new RedirectResult("/Account/Login");
        //                return;
        //            }

        //            AccountBll accountbll = new AccountBll();
        //            ICollection<Modulos_Usu> modulos = accountbll.GetModulos(u.Id);
        //            ICollection<Oficinas_Con> oficinas = accountbll.GetOficinas(u.Id);

        //            if (oficinas.Count()==0)
        //            {
        //                Redirect(contex, filterContext, "/Home/Index");
        //                return;
        //            }

        //            bool ofActual = oficinas.Where(x => (x.Id == Convert.ToInt32(contex.Session["oficina"])))
        //                .Select(x => x)
        //                .Count() == 0 ? true : false;

        //            if (ofActual && oficinas.Count() > 0)
        //            {
        //                contex.Session["oficina"] = oficinas.FirstOrDefault().Id;
        //            }

        //            if (modulos.Where(x => (x.Nombre == action)).Select(x => x).Count() == 0 && action != null || (oficinas.Count()==0 && oficina))
        //            {
        //                contex.Response.Redirect("/Account/AccessDenied");
        //                filterContext.Result = new RedirectResult("/Account/AccessDenied");
        //                return;
        //            }                    
        //            controller.ViewBag.user = userInfo;

        //            if (contex.Session["oficina"] == null)
        //            {
        //                contex.Session["oficina"] = oficinas.FirstOrDefault().Id;
        //            }
                     
        //        }
        //        else
        //        {
        //            contex.Response.Cookies["urlReturn"].Value = urlReturn;
        //            filterContext.Result = new RedirectResult("/Account/Login");
        //            return;
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}

        //private void Redirect(HttpContextBase contex, ActionExecutingContext filterContext,string ruta)
        //{
        //    contex.Response.Cookies["urlReturn"].Value = ruta;
        //    filterContext.Result = new RedirectResult("/Account/Login");
        //}
    }
}