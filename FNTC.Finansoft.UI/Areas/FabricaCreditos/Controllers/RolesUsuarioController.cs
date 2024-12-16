using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.DTO.FabricaCreditos;
using FNTC.Finansoft.Accounting.DTO.Email;
using FNTC.Finansoft.Accounting.DTO;
using System.Data.Entity;
using FNTC.Finansoft.Accounting.BLL.FabricaCreditosBll;

namespace FNTC.Finansoft.UI.Areas.FabricaCreditos.Controllers
{
    public class RolesUsuarioController : Controller
    {
        private AccountingContext db = new AccountingContext();
        // GET: FabricaCreditos/FabricaCreditos
        public ActionResult Index()
        {

            return View(db.FCRolesUsuario.ToList());
        }
        public ActionResult ListaUsuario()
        {
            using (var db = new AccountingContext())
            {
                return PartialView(db.AspNetUsersApp.ToList());
            }
        }
        public ActionResult ListaDependencia()
        {
            using (var db = new AccountingContext())
            {
                return PartialView(db.FCDependencias.ToList());
            }
        }
        public ActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Crear(FCRolesUsuario Roles)
        {
            if (!ModelState.IsValid)
                return View();

            try
            {
                using (AccountingContext db = new AccountingContext())
                {

                    db.FCRolesUsuario.Add(Roles);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error al Agregar, Campos Incompletos ");
                return View();
            }
        }
        public ActionResult Editar(int id)
        {
            try
            {
                using (var db = new AccountingContext())
                {
                    FCRolesUsuario Rol = db.FCRolesUsuario.Where(Roles => Roles.Id_Permisos == id).FirstOrDefault();
                    return View(Rol);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(FCRolesUsuario ROl)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                using (var db = new AccountingContext())
                {
                    FCRolesUsuario Con = db.FCRolesUsuario.Find(ROl.Id_Permisos);
                    Con.Rol_Operario = ROl.Rol_Operario;
                    Con.Rol_Analista = ROl.Rol_Analista;
                    Con.Rol_Ente = ROl.Rol_Ente;
                    Con.Rol_Informativo = ROl.Rol_Informativo;
                    Con.IdDependencia = ROl.IdDependencia;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}