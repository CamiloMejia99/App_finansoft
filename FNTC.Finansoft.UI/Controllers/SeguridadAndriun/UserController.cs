using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;




using Newtonsoft.Json;
using System.Text.RegularExpressions;



namespace Ingenio.Controllers.Seguridad
{
    //[Allow(action = "USER-ROL")]
    public class UserController : Controller
    {
        //UsuarioBll usuariobll = new UsuarioBll();
        //RolesBll rolesbll = new RolesBll();

        //public ActionResult Index()
        //{
        //    ICollection<Usuarios_Usu> user = usuariobll.GetUsuarios();
        //    return View("", user);
        //}
        //[HttpPost]
        //public ActionResult Create(string usuario, string password)
        //{
        //    Regex patron = new Regex("^[a-zA-Z0-9][a-zA-Z0-9_.-]{3,9}$");
        //    ///^[a-zA-Z0-9][a-zA-Z0-9_.-]{3,9}$/g
        //    Cifrado cifrado = new Cifrado();

        //    usuario = usuario.Trim().ToUpper();
        //    password = cifrado.EncodeSHA1(password);

        //    dynamic res = new
        //    {
        //        id = 0,
        //        added = false, 
        //        mensaje = "Nombre de usuario no valido"
        //    };
        //    if (!patron.IsMatch(usuario))
        //    {
        //        return Json(res, JsonRequestBehavior.AllowGet);
        //    }
                        
        //    Usuarios_Usu user = usuariobll.Create(usuario, password);
        //    if (user != null)
        //    {
        //        res = new
        //        {
        //            id = user.Id,
        //            added = true,
        //        };
        //    }
           

        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult RestorePassword(int id, string password)
        //{
        //    string u = Request.Cookies["user"]["user"];
        //    Cifrado cifrado = new Cifrado();
        //    password = cifrado.EncodeSHA1(password);
        //    dynamic res = new
        //    {
        //        estado = usuariobll.RestorePassword(id, password)
        //    };
        //    return Json(res, JsonRequestBehavior.AllowGet);
        //}

        
        //public ActionResult Details(int id)
        //{
        //    ICollection<Roles_Usu> roles = rolesbll.GetRoles();
        //    ICollection<Roles_Usu> marcados = usuariobll.GetUsuariosRoles(id);
        //    ICollection<object> lista = new List<object>();

        //    foreach (var item in roles)
        //    {
        //        bool ban = false;
        //        foreach (var item2 in marcados)
        //        {
        //            if (item.Id == item2.Id)
        //            {
        //                ban = true;
        //                break;
        //            }
                    
        //        }
        //        lista.Add(new
        //        {
        //            nombre = item.Nombre,
        //            id = item.Id,
        //            activo = ban
        //        });
        //    }
        //    return Json(lista, JsonRequestBehavior.AllowGet);
        //    //return Content(Newtonsoft.Json.JsonConvert.SerializeObject(modulos));
        //}

        //public ActionResult CambiarRoles(string data, int id)
        //{
        //    dynamic json = JsonConvert.DeserializeObject(data);
        //    ICollection<UsuariosRoles_Usu> userRol = new List<UsuariosRoles_Usu>();
        //    foreach (var item in json)
        //    {
        //        userRol.Add(new UsuariosRoles_Usu
        //        {
        //            Id_usuario = id,
        //            Id_rol = item
        //        });
        //    }
        //    return Json(usuariobll.CambiarRoles(id, userRol), JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        bool res = usuariobll.Delete(id);

        //        return Json(res, JsonRequestBehavior.AllowGet);
        //    }
        //    catch
        //    {
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //    }
        //}

        
	}
}