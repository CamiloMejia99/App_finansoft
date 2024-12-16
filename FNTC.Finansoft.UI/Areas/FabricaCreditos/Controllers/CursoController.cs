using FNTC.Finansoft.Accounting.DTO.ControlCartera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.FabricaCreditos.Controllers
{
    public class CursoController : Controller
    {
        private Curso cur = new Curso();
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.alerta = "info";
            ViewBag.res = "Registrar Proceso";
            return View(cur.listar());
        }
        [HttpPost]
        public ActionResult Index(string des, bool est)
        {
            if (cur.Insertar(des, est))
            {
                ViewBag.alerta = "success";
                ViewBag.res = "El Proceso se registro correctamente";
            }
            else
            {
                ViewBag.alerta = "danger";
                ViewBag.res = "Hubo un problema al registrar el Proceso";
            }
            return View(cur.listar());
        }
        public ActionResult Editar(int id)
        {
            ViewBag.alerta = "info";
            ViewBag.res = "Actualizar Proceso";
            return View(cur.un_registro(id));
        }
        [HttpPost]
        public ActionResult Editar(int id, string des, bool est)
        {
            if (cur.Actualizar(id, des, est))
            {
                ViewBag.alerta = "success";
                ViewBag.res = "El Proceso se actualizo correctamente";
            }
            else
            {
                ViewBag.alerta = "danger";
                ViewBag.res = "Ocurrio un error al actualizar los datos del proceso";
            }
            return View(cur.un_registro(id));
        }

        public ActionResult Eliminar(int id)
        {
            if (cur.Eliminar(id))
            {
                return RedirectToAction("Index", "Curso");
            }
            else
            {
                ViewBag.alerta = "danger";
                ViewBag.res = "Ocurrio un error al eliminar el Proceso";
                return View(cur.un_registro(id));
            }
        }
    }
}