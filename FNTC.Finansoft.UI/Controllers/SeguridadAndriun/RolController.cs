using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;




using Newtonsoft.Json;

namespace Ingenio.Controllers.Seguridad
{
    //[Allow(action = "USER-ROL")]
    public class RolController : Controller
    {
        //RolesBll rolesbll = new RolesBll();
        //public ActionResult Index()
        //{
        //    ICollection<Roles_Usu> roles = rolesbll.GetRoles();
        //    return View("",roles);
        //}

        ///// <summary>
        ///// Obtiene los modulos a los que determinadosrol puede ingresar
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public ActionResult Details(int id)
        //{
        //    OficinaBll oficinaBll = new OficinaBll();

        //    ICollection<Modulos_Usu> modulos = rolesbll.getModulos();
        //    ICollection<Modulos_Usu> marcados = rolesbll.GetModulosRol(id);

        //    ICollection<Oficinas_Con> Oficinas = oficinaBll.GetOficinas();
        //    ICollection<Oficinas_Con> RolesOficinas_Usu = rolesbll.GetRolesOficinas(id);

        //    ICollection<object> lista = new List<object>();
        //    foreach (var item in modulos)
        //    {
        //        //bool a = marcados.Contains(item);
        //        lista.Add(new
        //        {
        //            nombre = item.Nombre,
        //            id = item.Id,
        //            activo = marcados.Contains(item)
        //        });
        //    }

        //    ICollection<object> lista2 = new List<object>();
        //    foreach (var item in Oficinas)
        //    {
        //        //bool a = RolesOficinas_Usu.Where(x => (x.Id == item.Id)).Select(x=>x).Count()>0?true:false;
        //        lista2.Add(new
        //        {
        //            nombre = item.Nombre,
        //            id = item.Id,
        //            activo = RolesOficinas_Usu.Where(x => (x.Id == item.Id)).Select(x => x).Count() > 0 ? true : false
        //        });
        //    }

        //    var response = new
        //    {
        //        modulos = lista,
        //        oficinas = lista2
        //    };
        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}

        ////
        //// GET: /Rol/Create
        //public ActionResult Create()
        //{
        //    ViewBag.Modulos = rolesbll.getModulos();
        //    return View();
        //}

        ////
        //// POST: /Rol/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        IList<int> modulos = new List<int>();
        //        foreach (var item in collection.AllKeys)
        //        {
        //            if (collection[item]=="on")
        //            {
        //                modulos.Add(Convert.ToInt32(item));
        //            }
        //        }
        //        bool res = rolesbll.Create(collection["Nombre"],modulos);
        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        ////
        //// GET: /Rol/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        ////
        //// POST: /Rol/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        
        //[HttpPost]
        //public JsonResult Delete(int id)
        //{
        //    try
        //    {
        //        bool res = rolesbll.Delete(id);
        //        return Json(res, JsonRequestBehavior.AllowGet);
        //    }
        //    catch
        //    {
        //        return Json(false, JsonRequestBehavior.AllowGet);
        //    }
        //}

        //public ActionResult CambiarPermisos(string data, int rol,string nombre)
        //{
        //    dynamic json = JsonConvert.DeserializeObject(data);
        //    ICollection<RolesModulos_Usu> modulos = new List<RolesModulos_Usu>();
        //    foreach (var item in json.modulos)
        //    {
        //        modulos.Add(new RolesModulos_Usu { 
        //            Id_Modulo = item,
        //            Id_rol = rol
        //        });
        //    }

        //    ICollection<RolesOficinas_Usu> oficinas = new List<RolesOficinas_Usu>();
        //    foreach (var item in json.oficinas)
        //    {
        //        oficinas.Add(new RolesOficinas_Usu
        //        {
        //            Id_Oficina = item,
        //            Id_Rol = rol
        //        });
        //    }
            
        //    rolesbll.SetNombre(rol,nombre);

        //    return Json(rolesbll.CambiarPermisos(rol,modulos,oficinas), JsonRequestBehavior.AllowGet);
        //}
    }
}
