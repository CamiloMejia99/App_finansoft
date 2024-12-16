using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Ingenio.Controllers.Asociados
{
    public class IngresosAsoController : Controller
    {
        
        //IngresosAsoBLL IngreAsoBLL = new IngresosAsoBLL();
        //BeneficiarioBLL BenBLL = new BeneficiarioBLL();

        //// GET: IngresosAso
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: IngresosAso/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: IngresosAso/Create
        //[HttpGet]
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: IngresosAso/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        int honorarios = Convert.ToInt32(collection["honorarios"]);
        //        int arriendos = Convert.ToInt32(collection["arriendos"]);
        //        int bonificaciones = Convert.ToInt32(collection["bonificaciones"]);
        //        int sueldo = Convert.ToInt32(collection["sueldo"]);
        //        int dividendos = Convert.ToInt32(collection["dividendos"]);
        //        int refrigerios = Convert.ToInt32(collection["refrigerios"]);
        //        int comisiones = Convert.ToInt32(collection["comisiones"]);
        //        int utilidad_negocio = Convert.ToInt32(collection["utilidad_negocio"]);
        //        int pensiones = Convert.ToInt32(collection["pensiones"]);
        //        int otros = Convert.ToInt32(collection["otros"]);
        //        int intereses_inversiones = Convert.ToInt32(collection["intereses_inversiones"]);
        //        int subsidio_localizacion = Convert.ToInt32(collection["subsidio_localizacion"]);
        //        int idaso = Convert.ToInt32(collection["Id_Asociado"]);

        //        int[] ingresos = new int[12];
        //        ingresos[0] = honorarios;
        //        ingresos[1] = arriendos;
        //        ingresos[2] = bonificaciones;
        //        ingresos[3] = sueldo;
        //        ingresos[4] = dividendos;
        //        ingresos[5] = refrigerios;
        //        ingresos[6] = comisiones;
        //        ingresos[7] = utilidad_negocio;
        //        ingresos[8] = pensiones;
        //        ingresos[9] = otros;
        //        ingresos[10] = intereses_inversiones;
        //        ingresos[11] = subsidio_localizacion;

        //        bool res = IngreAsoBLL.CrearIngreso(ingresos, idaso);
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}

        //// GET: IngresosAso/Edit/5
        //[HttpGet]
        //public ActionResult Edit(int id)
        //{
        //    AsociadosIngresos_Aso honorarios = IngreAsoBLL.GetIngreso(id, 1);

        //    if (honorarios == null)
        //    {
        //        ViewBag.honorarios = 0;
        //    }
        //    else
        //    {
        //        ViewBag.honorarios = honorarios.Valor;
        //    }

        //    AsociadosIngresos_Aso arriendos = IngreAsoBLL.GetIngreso(id, 2);
        //    if (arriendos == null)
        //    {
        //        ViewBag.arriendos = 0;
        //    }
        //    else
        //    {
        //        ViewBag.arriendos = arriendos.Valor;
        //    }

        //    AsociadosIngresos_Aso bonificaciones = IngreAsoBLL.GetIngreso(id, 3);
        //    if (bonificaciones == null)
        //    {
        //        ViewBag.bonificaciones = 0;
        //    }
        //    else
        //    {
        //        ViewBag.bonificaciones = bonificaciones.Valor;
        //    }

        //    AsociadosIngresos_Aso sueldo = IngreAsoBLL.GetIngreso(id, 4);
        //    if (sueldo == null)
        //    {
        //        ViewBag.sueldo = 0;
        //    }
        //    else
        //    {
        //        ViewBag.sueldo = sueldo.Valor;
        //    }

        //    AsociadosIngresos_Aso dividendos = IngreAsoBLL.GetIngreso(id, 5);
        //    if (dividendos == null)
        //    {
        //        ViewBag.dividendos = 0;
        //    }
        //    else
        //    {
        //        ViewBag.dividendos = dividendos.Valor;
        //    }

        //    AsociadosIngresos_Aso refrigerios = IngreAsoBLL.GetIngreso(id, 6);
        //    if (refrigerios == null)
        //    {
        //        ViewBag.refrigerios = 0;
        //    }
        //    else
        //    {
        //        ViewBag.refrigerios = refrigerios.Valor;
        //    }

        //    AsociadosIngresos_Aso comisiones = IngreAsoBLL.GetIngreso(id, 7);
        //    if (comisiones == null)
        //    {
        //        ViewBag.comisiones = 0;
        //    }
        //    else
        //    {
        //        ViewBag.comisiones = comisiones.Valor;
        //    }

        //    AsociadosIngresos_Aso utilidad_negocio = IngreAsoBLL.GetIngreso(id, 8);
        //    if (utilidad_negocio == null)
        //    {
        //        ViewBag.utilidad_negocio = 0;
        //    }
        //    else
        //    {
        //        ViewBag.utilidad_negocio = utilidad_negocio.Valor;
        //    }

        //    AsociadosIngresos_Aso pensiones = IngreAsoBLL.GetIngreso(id, 9);
        //    if (pensiones == null)
        //    {
        //        ViewBag.pensiones = 0;
        //    }
        //    else
        //    {
        //        ViewBag.pensiones = pensiones.Valor;
        //    }

        //    AsociadosIngresos_Aso otros = IngreAsoBLL.GetIngreso(id, 10);
        //    if (otros == null)
        //    {
        //        ViewBag.otros = 0;
        //    }
        //    else
        //    {
        //        ViewBag.otros = otros.Valor;
        //    }

        //    AsociadosIngresos_Aso intereses_inversiones = IngreAsoBLL.GetIngreso(id, 11);
        //    if (intereses_inversiones == null)
        //    {
        //        ViewBag.intereses_inversiones = 0;
        //    }
        //    else
        //    {
        //        ViewBag.intereses_inversiones = intereses_inversiones.Valor;
        //    }

        //    AsociadosIngresos_Aso subsidio_localizacion = IngreAsoBLL.GetIngreso(id, 12);
        //    if (subsidio_localizacion == null)
        //    {
        //        ViewBag.subsidio_localizacion = 0;
        //    }
        //    else
        //    {
        //        ViewBag.subsidio_localizacion = subsidio_localizacion.Valor;
        //    }



        //    Personas_Fac a = BenBLL.GetPerson(id);
        //    ViewBag.nombre = a.Primer_Nom + " " + (a.Segundo_Nom ?? "") + " " + a.Primer_Ape + " " + (a.Segundo_Ape ?? "");
        //    ViewBag.cedula = a.Nit_CC;
        //    ViewBag.id = id;


        //    return View("");

        //}

        //// POST: IngresosAso/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        int honorarios = Convert.ToInt32(collection["honorarios"]);
        //        int arriendos = Convert.ToInt32(collection["arriendos"]);
        //        int bonificaciones = Convert.ToInt32(collection["bonificaciones"]);
        //        int sueldo = Convert.ToInt32(collection["sueldo"]);
        //        int dividendos = Convert.ToInt32(collection["dividendos"]);
        //        int refrigerios = Convert.ToInt32(collection["refrigerios"]);
        //        int comisiones = Convert.ToInt32(collection["comisiones"]);
        //        int utilidad_negocio = Convert.ToInt32(collection["utilidad_negocio"]);
        //        int pensiones = Convert.ToInt32(collection["pensiones"]);
        //        int otros = Convert.ToInt32(collection["otros"]);
        //        int intereses_inversiones = Convert.ToInt32(collection["intereses_inversiones"]);
        //        int subsidio_localizacion = Convert.ToInt32(collection["subsidio_localizacion"]);

        //        int[] ingresos = new int[12];
        //        ingresos[0] = honorarios;
        //        ingresos[1] = arriendos;
        //        ingresos[2] = bonificaciones;
        //        ingresos[3] = sueldo;
        //        ingresos[4] = dividendos;
        //        ingresos[5] = refrigerios;
        //        ingresos[6] = comisiones;
        //        ingresos[7] = utilidad_negocio;
        //        ingresos[8] = pensiones;
        //        ingresos[9] = otros;
        //        ingresos[10] = intereses_inversiones;
        //        ingresos[11] = subsidio_localizacion;

        //        bool res = IngreAsoBLL.EditarIngreso(ingresos, id);
        //        return RedirectToAction("Details", "Asociado", new { id = id });
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}


    }
}
