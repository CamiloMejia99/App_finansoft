using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;




using Newtonsoft.Json;

using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Ingenio.Controllers.Seguridad
{
    public class AccountController : Controller
    {
        //UsuarioBll usuariobll = new UsuarioBll();
        //RolesBll rolesbll = new RolesBll();
        //AccountBll cuenta = new AccountBll();
        //public static Exception loginException { get; set; }

        //public Exception GetInnerException(Exception e)
        //{
        //    if (e.InnerException != null)
        //    {
        //        GetInnerException(e.InnerException);
        //    }
        //    return e;
        //}
        //public JsonResult VerificaError()
        //{
        //    Exception e = GetInnerException(loginException);
        //    return Json(new
        //    {
        //        e.Message,
        //        e.Source,
        //        e.Data
        //    }, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult Login()
        //{
        //    ViewBag.ban = false;
        //    ViewBag.show = false;
        //    return View("Login");
        //}

        //public JsonResult Cifrar(string cadena)
        //{
        //    Cifrado cifrado = new Cifrado();
        //    DateTime inicio = DateTime.Now;
        //    //DateTime fin = inicio.AddMonths(6);
        //    string serial = cifrado.EncodeSHA1(inicio.ToString() + cadena); // + fin.ToString()
        //    serial = cifrado.EncodeMD5(serial);

        //    serial = serial.Substring(0, 8) + "-" + serial.Substring(7, 8) + "-" + serial.Substring(15, 8) + "-" + serial.Substring(23, 8);

        //    return Json(new
        //    {
        //        Empresa = cadena,
        //        fechaInicio = inicio.ToString(),
        //        //fechaFin = fin.ToString(),
        //        serial = serial
        //    }, JsonRequestBehavior.AllowGet);
        //}
        //[HttpPost]
        //public ActionResult Login(string user, string password, string urlReturn, FormCollection col)
        //{
        //    Cifrado cifrado = new Cifrado();

        //    user = user.Trim().ToUpper();
        //    password = cifrado.EncodeSHA1(password);
        //    Usuarios_Usu res = null;
        //    try
        //    {
        //        res = cuenta.Login(user, password);
        //    }
        //    catch (Exception e)
        //    {
        //        loginException = e;
        //    }

        //    if (res != null)
        //    {
        //        ICollection<string> modulos = usuariobll.GetNombreModulos(res.Id);

        //        Usuarios_Usu u = usuariobll.GetUserByName(user);
        //        ICollection<Oficinas_Con> oficinas = cuenta.GetOficinas(u.Id);

        //        if (modulos == null || modulos.Count() == 0 || oficinas.Count() == 0)
        //        {
        //            ViewBag.ban = false;
        //            ViewBag.show = true;
        //            return View();
        //        }


        //        usuariobll.cambiarHash(u, ""); //Cambia la fecha para evitar cookies falsas 
        //        if (col["recuerdame"] == "on")
        //        {
        //            Response.Cookies["user"].Expires = DateTime.Now.AddHours(5);
        //        }

        //        Response.Cookies["user"].Value = cifrado.EncryptString(u.PasswordHash + u.UserName);
        //        Session["userInfo"] = u;
        //        Session["oficina"] = oficinas.FirstOrDefault().Id;
        //    }
        //    else
        //    {
        //        ViewBag.ban = false;
        //        ViewBag.show = true;
        //        return View();
        //    }
        //    if (Request.Cookies["urlReturn"] != null)
        //    {
        //        return Redirect(Request.Cookies["urlReturn"].Value);
        //    }
        //    return Redirect("/Home/Index");
        //}

        //public ActionResult AccessDenied()
        //{
        //    return View();
        //}

        //public ActionResult Logout()
        //{
        //    Session.Clear();
        //    Response.Cookies["user"].Expires = DateTime.Now.AddDays(-365);
        //    return RedirectToAction("Index", "Home");
        //}

    }

}