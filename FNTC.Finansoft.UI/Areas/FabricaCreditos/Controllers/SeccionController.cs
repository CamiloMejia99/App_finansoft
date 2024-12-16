
using FNTC.Finansoft.Accounting.DTO.ControlCartera;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FNTC.Finansoft.UI.Areas.FabricaCreditos.Controllers
{
    public class SeccionController : Controller
    {
        private Seccion sec = new Seccion();
        //--------------------------------------INDEX -------------------------------------------
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.alerta = "info";
            ViewBag.res = "Registrar Nueva Seccion";
            return View(sec.listar());
        }

        [HttpPost] // -- Inserta un Seccion
        public ActionResult Index(string nu, bool es, string fe, int cu)
        {
            if (sec.Insertar(nu, es, fe, cu, l_indices))
            {
                ViewBag.alerta = "success";
                ViewBag.res = "Seccion Registrado";
            }
            else
            {
                ViewBag.alerta = "danger";
                ViewBag.res = "Seccion No Registrado";
            }
            return View(sec.listar());
        }

        [HttpGet]

        public ActionResult Actualizar(int id)   //--Solo carga un registro
        {
            ViewBag.alerta = "info";
            ViewBag.res = "Modificar Datos de Seccion";
            return View(sec.un_registro(id));
        }
        [HttpPost]
        public ActionResult Actualizar(int id, string nu, bool es, string fe, int cu)
        {
            if (sec.Actualizar(id, nu, es, fe, cu, l_indices))
            {
                ViewBag.alerta = "success";
                ViewBag.res = "Seccion Actualizada";
            }
            else
            {
                ViewBag.alerta = "danger";
                ViewBag.res = "Ocurrio un error: please try again!";
            }
            return View(sec.un_registro(id));
        }

        //------------------------------------- ELIMINAR -------------------------------------------
        [HttpGet]
        public ActionResult Eliminar(string id) // -- Solo carga el libro a eliminar
        {
            ViewBag.alerta = "info";
            ViewBag.res = "Datos de la Seccion a Eliminar";
            return View(sec.un_registro(Int32.Parse(id)));
        }

        [HttpPost]//--ELIMINAR SECCION

        public ActionResult Eliminar(int id)
        {
            if (sec.Eliminar(id))
            {
                return RedirectToAction("Index", "Seccion");
            }
            else
            {
                ViewBag.alerta = "danger";
                ViewBag.res = "La seccion no pudo eliminar ya que existen alumnos en el aula ";
                return View(sec.un_registro(id));
            }
        }

        private static List<string> l_indices = new List<string>();
        private static List<string> l_alumnos = new List<string>();

        //--------busqueda de alumno
        [HttpPost]
        public String bus_atr(string dato_bus)
        {
            string res = "";
            var alumnos = new List<Alumno>();
            alumnos = sec.bus_atr(dato_bus);
            foreach (var a in alumnos)
            {
                int id = a.id_alu;
                string dni = a.dni_alu;
                string nom = a.nombre_alu;
                string ape = a.apellidos_alu;
                string boton_sel = "<button class=\"btn btn-warning\" type='button'"
                    + " onclick=\"agr_atr('" + id + "','" + dni + "','" + nom + "','" + ape + "')\""
                    + " data-dismiss='modal'><span class=\"glyphicon glyphicon-check\"> Añadir</span></button>";
                res = res +
                    "<tr><td>" + id + "</td>"
                    + "<td>" + dni + "</td>"
                    + "<td>" + nom + "</td>"
                    + "<td>" + ape + "</td>"
                    + "<td>" + boton_sel + "</td></tr>";
            }
            return res;
        }
        //------------Agregar Alumno 
        public String agr_atr(string id, string dni, string nom, string ape)
        {
            string res = "";
            int cont = 0;
            foreach (var w in l_indices)
            {
                if (w.Equals(id))
                {
                    cont++;
                }
            }
            if (cont == 0)
            {
                if (l_alumnos.Count < 8)
                {
                    string boton_bor = "<button class=\"btn btn-danger\" type='button'"
                        + " onclick=\"bor_atr('" + id + "')\""
                        + "><span class=\"glyphicon glyphicon-trash\"> Borrar</span></button>"; //--Esta variable boton_bor representa un boton
                    l_alumnos.Add(
                        "<tr><td>" + id + "</td>"
                        + "<td>" + dni + "</td>"
                        + "<td>" + nom + "</td>"
                        + "<td>" + ape + "</td>"
                        + "<td>" + boton_bor + "</td></tr>"
                        );
                    l_indices.Add(id);
                }
            }
            foreach (var a in l_alumnos)
            {
                res = res + a;
            }
            return res;
        }

        //--------------Limpieza--------------------
        [HttpPost]
        public void limpiar_atr()
        {
            l_alumnos.Clear();
            l_indices.Clear();
        }
        //-------------- Borrar Alumno de Lista -----------
        public String bor_atr(string id)
        {
            string res = "";
            l_alumnos.RemoveAt(l_indices.IndexOf(id));
            l_indices.RemoveAt(l_indices.IndexOf(id));
            foreach (var a in l_alumnos)
            {
                res = res + a;
            }
            return res;
        }

        //----Detalle Actualizar Alumno

        public String det_actualizar_sec(string id)
        {
            string res = "";
            var alum = new List<Alumno>();
            alum = sec.det_sec(id);
            foreach (var a in alum)
            {
                string boton_bor = "<button class=\"btn btn-danger\" type='button'"
                        + " onclick=\"bor_atr('" + a.id_alu + "')\""
                        + "><span class=\"glyphicon glyphicon-trash\"> Borrar</span></button>";
                l_alumnos.Add(
                        "<tr><td>" + a.id_alu + "</td>"
                        + "<td>" + a.dni_alu + "</td>"
                        + "<td>" + a.nombre_alu + "</td>"
                        + "<td>" + a.apellidos_alu + "</td>"
                        + "<td>" + boton_bor + "</td></tr>"
                        );
                l_indices.Add(a.id_alu.ToString());
            }
            foreach (var a in l_alumnos)
            {
                res = res + a;
            }
            return res;
        }

        //------------ Detalle Seccion ----
        [HttpPost]
        public String detalle_seccion(string id_sec)
        {
            string res = "";
            var alumnos = new List<Alumno>();
            alumnos = sec.detalle_seccion(id_sec);
            foreach (var a in alumnos)
            {
                int id = a.id_alu;
                string dni = a.dni_alu;
                string des = a.nombre_alu + " " + a.apellidos_alu;
                res = res +
                    "<tr><td>" + id + "</td>"
                    + "<td>" + dni + "</td>"
                    + "<td>" + des + "</td></tr>";
            }
            return res;
        }

        //----------------------------Busqueda General-------------------
        [HttpPost]
        public String bus_sec(string tipo_bus, string dato_bus_sec)
        {
            string res = "";
            string tipo = "";
            if (tipo_bus.Equals("1"))
            {
                tipo = "id_sec";
            }
            else if (tipo_bus.Equals("2"))
            {
                tipo = "id_cur";
            }
            var lista = new List<Seccion>();
            lista = sec.bus_sec(tipo, dato_bus_sec);
            var cur = new FNTC.Finansoft.Accounting.DTO.ControlCartera.Model1().Curso.ToList();
            foreach (var l in lista)
            {
                string boton1 = "<button class='btn btn-secondary' type='button' "
                            + "data-target='#modal_detalle_seccion' data-toggle='modal' "
                            + "data-backdrop='static' data-keyboard='false' "
                            + "onclick=\"detalle_seccion('" + l.id_sec + "')\"><span class=\"glyphicon glyphicon-eye-open\"> Detalle Seccion</span></button>";

                string boton2 = "<button class='btn btn-warning' type='button' id='btn_act' "
                            + "name='btn_act' onclick=\"location.href='../Seccion/Actualizar?id=" + l.id_sec + "'\">"
                            + "<span class=\"glyphicon glyphicon-edit\"> Actualizar</span></button>";

                string boton3 = "<button class='btn btn-danger' type='button' id='btn_eli' name='btn_eli' "
                            + "onclick=\"location.href='../Seccion/Eliminar?id=" + l.id_sec + "'\"><span class=\"glyphicon glyphicon-trash\"> Eliminar</span></button>";
                string estado = "Activo";
                if (l.estado_sec.Equals(false))
                {
                    estado = "No " + estado;
                }
                foreach (var c in cur)
                {
                    if (l.id_cur == c.id_cur)
                    {
                        res = res +
                    "<tr><td>" + l.id_sec + "</td>"
                    + "<td>" + l.aula_sec + "</td>"
                    + "<td>" + c.descripcion_cur + "</td>"
                    + "<td>" + estado + "</td>"
                    + "<td>" + l.fecha_registro_sec.ToString("dd/MM/yyyy") + "</td>"
                    + "<td>" + boton1 + " " + boton2 + " " + boton3 + "</td></tr>";
                    }
                }

            }
            return res;
        }
    }
}