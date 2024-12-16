using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;



namespace Ingenio.Controllers.Asociados
{
    public class EgresosAsoController : Controller
    {

        //EgresosAsoBLL EgreAsoBLL = new EgresosAsoBLL();
        //BeneficiarioBLL BenBLL = new BeneficiarioBLL();

        //// GET: EgresosAso
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: EgresosAso/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: EgresosAso/Create
        //[HttpGet]
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: EgresosAso/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        int alimentacion = Convert.ToInt32(collection["alimentacion"]);
        //        int educacion = Convert.ToInt32(collection["educacion"]);
        //        int transporte = Convert.ToInt32(collection["transporte"]);
        //        int cuota_domestica = Convert.ToInt32(collection["cuota_domestica"]);
        //        int deudas = Convert.ToInt32(collection["deudas"]);
        //        int prestamo_vivienda = Convert.ToInt32(collection["prestamo_vivienda"]);
        //        int tarjeta_credito = Convert.ToInt32(collection["tarjeta_credito"]);
        //        int servicios_publicos = Convert.ToInt32(collection["servicios_publicos"]);
        //        int arriendo = Convert.ToInt32(collection["arriendo"]);
        //        int salud = Convert.ToInt32(collection["salud"]);
        //        int otros_gastos = Convert.ToInt32(collection["otros_gastos"]);
        //        int otros_negocios = Convert.ToInt32(collection["otros_negocios"]);
        //        int prestamo_vehiculo = Convert.ToInt32(collection["prestamo_vehiculo"]);
        //        int otros_prestamos = Convert.ToInt32(collection["otros_prestamos"]);

        //        int idaso = Convert.ToInt32(collection["Id_Asociado"]);

        //        int[] egresos = new int[14];
        //        egresos[0] = alimentacion;
        //        egresos[1] = educacion;
        //        egresos[2] = transporte;
        //        egresos[3] = cuota_domestica;
        //        egresos[4] = deudas;
        //        egresos[5] = prestamo_vivienda;
        //        egresos[6] = tarjeta_credito;
        //        egresos[7] = servicios_publicos;
        //        egresos[8] = arriendo;
        //        egresos[9] = salud;
        //        egresos[10] = otros_gastos;
        //        egresos[11] = otros_negocios;
        //        egresos[12] = prestamo_vehiculo;
        //        egresos[13] = otros_prestamos;

 
        //        bool res = EgreAsoBLL.CrearEgreso(egresos, idaso);
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}

        //// GET: EgresosAso/Edit/5
        //[HttpGet]
        //public ActionResult Edit(int id)
        //{
        //    AsociadosEgresos_Aso alimentacion = EgreAsoBLL.GetEgreso(id, 1);                
                
        //    if (alimentacion == null)
        //    {
        //        ViewBag.alimentacion = 0;
        //    }
        //    else
        //    {
        //        ViewBag.alimentacion = alimentacion.Valor;
        //    }

        //    AsociadosEgresos_Aso educacion = EgreAsoBLL.GetEgreso(id, 2);       
        //    if (educacion == null)
        //    {
        //        ViewBag.educacion = 0;
        //    }
        //    else
        //    {
        //        ViewBag.educacion = educacion.Valor;
        //    }

        //    AsociadosEgresos_Aso transporte = EgreAsoBLL.GetEgreso(id, 3);       
        //    if (transporte == null)
        //    {
        //        ViewBag.transporte = 0;
        //    }
        //    else
        //    {
        //        ViewBag.transporte = transporte.Valor;
        //    }

        //    AsociadosEgresos_Aso cuota_domestica = EgreAsoBLL.GetEgreso(id, 4);       
        //    if (cuota_domestica == null)
        //    {
        //        ViewBag.cuota_domestica = 0;
        //    }
        //    else
        //    {
        //        ViewBag.cuota_domestica = cuota_domestica.Valor;
        //    }

        //    AsociadosEgresos_Aso deudas = EgreAsoBLL.GetEgreso(id, 5);       
        //    if (deudas == null)
        //    {
        //        ViewBag.deudas = 0;
        //    }
        //    else
        //    {
        //        ViewBag.deudas = deudas.Valor;
        //    }

        //    AsociadosEgresos_Aso prestamo_vivienda = EgreAsoBLL.GetEgreso(id, 6);       
        //    if (prestamo_vivienda == null)
        //    {
        //        ViewBag.prestamo_vivienda = 0;
        //    }
        //    else
        //    {
        //        ViewBag.prestamo_vivienda = prestamo_vivienda.Valor;
        //    }

        //    AsociadosEgresos_Aso tarjeta_credito = EgreAsoBLL.GetEgreso(id, 7);       
        //    if (tarjeta_credito == null)
        //    {
        //        ViewBag.tarjeta_credito = 0;
        //    }
        //    else
        //    {
        //        ViewBag.tarjeta_credito = tarjeta_credito.Valor;
        //    }

        //    AsociadosEgresos_Aso servicios_publicos = EgreAsoBLL.GetEgreso(id, 8);       
        //    if (servicios_publicos == null)
        //    {
        //        ViewBag.servicios_publicos = 0;
        //    }
        //    else
        //    {
        //        ViewBag.servicios_publicos = servicios_publicos.Valor;
        //    }

        //    AsociadosEgresos_Aso arriendo = EgreAsoBLL.GetEgreso(id, 9);       
        //    if (arriendo == null)
        //    {
        //        ViewBag.arriendo = 0;
        //    }
        //    else
        //    {
        //        ViewBag.arriendo = arriendo.Valor;
        //    }

        //    AsociadosEgresos_Aso salud = EgreAsoBLL.GetEgreso(id, 10);       
        //    if (salud == null)
        //    {
        //        ViewBag.salud = 0;
        //    }
        //    else
        //    {
        //        ViewBag.salud = salud.Valor;
        //    }

        //    AsociadosEgresos_Aso otros_gastos = EgreAsoBLL.GetEgreso(id, 11);       
        //    if (otros_gastos == null)
        //    {
        //        ViewBag.otros_gastos = 0;
        //    }
        //    else
        //    {
        //        ViewBag.otros_gastos = otros_gastos.Valor;
        //    }

        //    AsociadosEgresos_Aso otros_negocios = EgreAsoBLL.GetEgreso(id, 12);       
        //    if (otros_negocios == null)
        //    {
        //        ViewBag.otros_negocios = 0;
        //    }
        //    else
        //    {
        //        ViewBag.otros_negocios = otros_negocios.Valor;
        //    }

        //    AsociadosEgresos_Aso prestamo_vehiculo = EgreAsoBLL.GetEgreso(id, 13);       
        //    if (prestamo_vehiculo == null)
        //    {
        //        ViewBag.prestamo_vehiculo = 0;
        //    }
        //    else
        //    {
        //        ViewBag.prestamo_vehiculo = prestamo_vehiculo.Valor;
        //    }

        //    AsociadosEgresos_Aso otros_prestamos = EgreAsoBLL.GetEgreso(id, 14);       
        //    if (otros_prestamos == null)
        //    {
        //        ViewBag.otros_prestamos = 0;
        //    }
        //    else
        //    {
        //        ViewBag.otros_prestamos = otros_prestamos.Valor;
        //    }

        //    Personas_Fac a = BenBLL.GetPerson(id); 
        //    ViewBag.nombre = a.Primer_Nom + " " + (a.Segundo_Nom?? "")  + " " + a.Primer_Ape + " " + (a.Segundo_Ape?? "") ;
        //    ViewBag.cedula = a.Nit_CC;
        //    ViewBag.id = id;


        //    return View("");
        //}

        //// POST: EgresosAso/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        int alimentacion = Convert.ToInt32(collection["alimentacion"]);
        //        int educacion = Convert.ToInt32(collection["educacion"]);
        //        int transporte = Convert.ToInt32(collection["transporte"]);
        //        int cuota_domestica = Convert.ToInt32(collection["cuota_domestica"]);
        //        int deudas = Convert.ToInt32(collection["deudas"]);
        //        int prestamo_vivienda = Convert.ToInt32(collection["prestamo_vivienda"]);
        //        int tarjeta_credito = Convert.ToInt32(collection["tarjeta_credito"]);
        //        int servicios_publicos = Convert.ToInt32(collection["servicios_publicos"]);
        //        int arriendo = Convert.ToInt32(collection["arriendo"]);
        //        int salud = Convert.ToInt32(collection["salud"]);
        //        int otros_gastos = Convert.ToInt32(collection["otros_gastos"]);
        //        int otros_negocios = Convert.ToInt32(collection["otros_negocios"]);
        //        int prestamo_vehiculo = Convert.ToInt32(collection["prestamo_vehiculo"]);
        //        int otros_prestamos = Convert.ToInt32(collection["otros_prestamos"]);


        //        int[] egresos = new int[14];
        //        egresos[0] = alimentacion;
        //        egresos[1] = educacion;
        //        egresos[2] = transporte;
        //        egresos[3] = cuota_domestica;
        //        egresos[4] = deudas;
        //        egresos[5] = prestamo_vivienda;
        //        egresos[6] = tarjeta_credito;
        //        egresos[7] = servicios_publicos;
        //        egresos[8] = arriendo;
        //        egresos[9] = salud;
        //        egresos[10] = otros_gastos;
        //        egresos[11] = otros_negocios;
        //        egresos[12] = prestamo_vehiculo;
        //        egresos[13] = otros_prestamos;


        //        bool res = EgreAsoBLL.EditarEgreso(egresos, id);
        //        return RedirectToAction("Details", "Asociado", new { id = id });
        //    }
        //    catch (Exception e)
        //    {
        //        return View("_Error", e);
        //    }
        //}

        //// GET: EgresosAso/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: EgresosAso/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    return View();
        //}
    }
}
