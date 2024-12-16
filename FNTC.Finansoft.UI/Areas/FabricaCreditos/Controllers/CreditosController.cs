
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.BLL;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
//using .Terceros;

namespace FNTC.Finansoft.UI.Areas.FabricaCreditos.Controllers
{
    public class CreditosController : Controller
    {
        private AccountingContext db = new AccountingContext();

        // GET: Creditos/Creditos
        public ActionResult Index(string alerta)
        {
            if (alerta == "" || alerta == null)
            {
                ViewBag.alerta = "false";
                return View();
            }
            else
            {
                ViewBag.alerta = "true";
                return View();
            }
        }

        //Obtener Creditos
        public JsonResult ObtenerCreditos()
        {
            var listacreditos = new List<ViewModelPrestaTerceros>();

            var prestamos = db.Prestamos.ToList();

            foreach (var prestamo in prestamos)
            {

                var presta = new ViewModelPrestaTerceros();

                presta.id = prestamo.id;

                presta.Pagare = prestamo.Pagare;
                presta.Fecha_Prestamo = prestamo.Fecha_Prestamo.ToString("yyyy-MM-dd");
                presta.Capital = prestamo.Capital;
                presta.Plazo = prestamo.Plazo;
                presta.Interes = prestamo.Interes;
                presta.NOMBRE = prestamo.terceroFK.NOMBRE1 + " " + prestamo.terceroFK.NOMBRE2 + " " + prestamo.terceroFK.APELLIDO1 + " " + prestamo.terceroFK.APELLIDO2;
                presta.NIT = prestamo.NIT;
                presta.estado = prestamo.estado;

                listacreditos.Add(presta);
            }

            var respuesta = new DTODataTables<ViewModelPrestaTerceros>
            {
                data = listacreditos
            };
            return Json(respuesta, JsonRequestBehavior.AllowGet);
            // return Json(new { data = respuesta }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ImprimirAmortizacion(int id)
        {
            List<ViewModelCreditos> _Amortizacion = new List<ViewModelCreditos>();
            /*
            var companiaLista = (from s in db.Compania where id == 1 select s).First();
            var nombreCompania = companiaLista.nombrecompañia;
            var nitcompania = companiaLista.nit;
            */
            var prestam = db.Prestamos.FirstOrDefault(j => j.id == id);
            var prestamosdestinoid = prestam.Destino_Id;

            var DestinoLista = db.Destinos.FirstOrDefault(j => j.Destino_Id == prestamosdestinoid);
            var DestinosLineaId = DestinoLista.Lineas_Id;
            var LineaLista = db.Lineas.FirstOrDefault(j => j.Lineas_Id == DestinosLineaId);
            var NombreDestino = DestinoLista.Destino_Descripcion;
            var NombreLinea = LineaLista.Lineas_Descripcion;

            var NITTercero = prestam.NIT;

            var companiaLista = db.Compania.FirstOrDefault(j => j.id == 1);
            var nombreCompania = companiaLista.nombrecompañia;
            var nitcompania = companiaLista.nit;

            var dtoTercero = new FNTC.Finansoft.Accounting.DAL.TercerosDAL().GetTerceros(NITTercero).First();


            var ncreditos = (from prestamo in db.Prestamos
                             join costospres in db.CostosPrestamos on prestamo.Pagare equals costospres.Pagare
                             join costoadicional in db.Costos_Adicionales on costospres.CA_Id equals costoadicional.CA_Id
                             join periodo in db.Tipo_Periodo on prestamo.Tipo_Periodo_Id equals periodo.Tipo_Periodo_Id
                             where prestamo.id == id
                             select prestamo.id).Count();

            if (ncreditos == 0)
            {
                var ListaCre = (from prestamo in db.Prestamos

                                join periodo in db.Tipo_Periodo on prestamo.Tipo_Periodo_Id equals periodo.Tipo_Periodo_Id
                                join gartantiasCreditos in db.GarantiasCreditos on prestamo.Pagare equals gartantiasCreditos.pagare
                                where prestamo.id == id
                                select new
                                {
                                    prestamo.id,
                                    prestamo.Capital,
                                    prestamo.Interes,
                                    prestamo.Plazo,
                                    prestamo.Fecha_Prestamo,
                                    periodo.Tipo_Periodo_Valor,
                                    prestamo.myselect,
                                    prestamo.ValorPeriodo,
                                    prestamo.ValorSeguro,
                                    prestamo.ValorSeguroPorcentaje,
                                    prestamo.ValDiasInt,
                                    prestamo.difdias,
                                    prestamo.fechadesembolso,

                                    prestamo.costoAdicionalEnEltiempo,
                                    prestamo.costoAdicionalAnticipado,
                                    prestamo.costoAdicionalPrimeraCuota,
                                    prestamo.costoAdicionalDividoEnElTiempo,
                                    prestamo.ValorPorcentajeCostoAnticipado,
                                    prestamo.ValorPorcentajeCostoEnCadaCuota,

                                    gartantiasCreditos.garantia_id,
                                    gartantiasCreditos.Real_Valor,
                                    gartantiasCreditos.codeudor_nit,
                                    gartantiasCreditos.nombre_codeudor

                                }).ToList();
                foreach (var item in ListaCre)
                {
                    ViewModelCreditos obj = new ViewModelCreditos();
                    obj.id = item.id;
                    obj.Capital = item.Capital;
                    obj.Interes = item.Interes;
                    obj.Plazo = item.Plazo;
                    obj.Fecha_Prestamo = item.Fecha_Prestamo.ToShortDateString();
                    obj.CA_Valor = Convert.ToString(0);
                    obj.CA_Porcentaje = Convert.ToString(0);
                    obj.Tipo_Periodo_Valor = item.Tipo_Periodo_Valor;
                    obj.myselect = item.myselect;
                    obj.ValorPeriodo = item.ValorPeriodo;
                    obj.ValorSeguro = item.ValorSeguro;
                    obj.ValorSeguroPorcentaje = item.ValorSeguroPorcentaje;
                    obj.ValDiasInt = item.ValDiasInt;
                    obj.difdias = item.difdias;
                    obj.fechadesembolso = item.fechadesembolso.ToShortDateString();
                    obj.nombreEmpresa = nombreCompania;
                    obj.nitEmpresa = nitcompania;
                    obj.nombreTercero = dtoTercero.NOMBRE1 + " " + dtoTercero.NOMBRE2 + " " + dtoTercero.APELLIDO1 + " " + dtoTercero.APELLIDO2;
                    obj.celularTercero = dtoTercero.TELMOVIL;
                    obj.nitTercero = dtoTercero.NIT;
                    obj.linea = NombreLinea;
                    obj.destino = NombreDestino;
                    obj.garatiaId = (from pc in db.Garantias where pc.Garantias_Id == item.garantia_id select pc.Garantias_Descripcion).Single();
                    obj.realValor = item.Real_Valor;
                    obj.codeudorNit = item.codeudor_nit;
                    obj.nombreCodeudor = item.nombre_codeudor;
                    obj.costoAdicionalEnEltiempo = item.costoAdicionalEnEltiempo;
                    obj.costoAdicionalAnticipado = item.costoAdicionalAnticipado;
                    obj.costoAdicionalPrimeraCuota = item.costoAdicionalPrimeraCuota;
                    obj.costoAdicionalDividoEnElTiempo = item.costoAdicionalDividoEnElTiempo;
                    obj.ValorPorcentajeCostoAnticipado = item.ValorPorcentajeCostoAnticipado;
                    obj.ValorPorcentajeCostoEnCadaCuota = item.ValorPorcentajeCostoEnCadaCuota;

                    _Amortizacion.Add(obj);
                }
                return View("ImprimirAmortizacion", _Amortizacion);
            }
            else
            {

                var ListaCreditos = (from prestamo in db.Prestamos

                                     join costospres in db.CostosPrestamos on prestamo.Pagare equals costospres.Pagare
                                     join costoadicional in db.Costos_Adicionales on costospres.CA_Id equals costoadicional.CA_Id
                                     join gartantiasCreditos in db.GarantiasCreditos on prestamo.Pagare equals gartantiasCreditos.pagare
                                     join periodo in db.Tipo_Periodo on prestamo.Tipo_Periodo_Id equals periodo.Tipo_Periodo_Id

                                     where prestamo.id == id
                                     select new
                                     {
                                         prestamo.id,
                                         prestamo.Capital,
                                         prestamo.Interes,
                                         prestamo.Plazo,
                                         prestamo.Fecha_Prestamo,
                                         costoadicional.CA_Valor,
                                         costoadicional.CA_Porcentaje,
                                         periodo.Tipo_Periodo_Valor,
                                         prestamo.myselect,
                                         prestamo.ValorPeriodo,
                                         prestamo.ValorSeguro,
                                         prestamo.ValorSeguroPorcentaje,
                                         prestamo.ValDiasInt,
                                         prestamo.fechadesembolso,
                                         prestamo.difdias,

                                         prestamo.costoAdicionalEnEltiempo,
                                         prestamo.costoAdicionalAnticipado,
                                         prestamo.costoAdicionalPrimeraCuota,
                                         prestamo.costoAdicionalDividoEnElTiempo,
                                         prestamo.ValorPorcentajeCostoAnticipado,
                                         prestamo.ValorPorcentajeCostoEnCadaCuota,

                                         gartantiasCreditos.garantia_id,
                                         gartantiasCreditos.Real_Valor,
                                         gartantiasCreditos.codeudor_nit,
                                         gartantiasCreditos.nombre_codeudor


                                     }).ToList();

                foreach (var item in ListaCreditos)
                {
                    ViewModelCreditos obj = new ViewModelCreditos();
                    obj.id = item.id;
                    obj.Capital = item.Capital;
                    obj.Interes = item.Interes;
                    obj.Plazo = item.Plazo;
                    obj.Fecha_Prestamo = item.Fecha_Prestamo.ToString("dd/MM/yyy");
                    obj.CA_Valor = item.CA_Valor;
                    obj.CA_Porcentaje = item.CA_Porcentaje;
                    obj.Tipo_Periodo_Valor = item.Tipo_Periodo_Valor;
                    obj.myselect = item.myselect;
                    obj.ValorPeriodo = item.ValorPeriodo;
                    obj.ValorSeguro = item.ValorSeguro;
                    obj.ValorSeguroPorcentaje = item.ValorSeguroPorcentaje;
                    obj.ValDiasInt = item.ValDiasInt;
                    obj.difdias = item.difdias;
                    obj.fechadesembolso = item.fechadesembolso.ToString("dd/MM/yyy");
                    obj.nombreEmpresa = nombreCompania;
                    obj.nitEmpresa = nitcompania;
                    obj.nombreTercero = dtoTercero.NOMBRE1 + " " + dtoTercero.NOMBRE2 + " " + dtoTercero.APELLIDO1 + " " + dtoTercero.APELLIDO2;
                    obj.celularTercero = dtoTercero.TELMOVIL;
                    obj.nitTercero = dtoTercero.NIT;
                    obj.linea = NombreLinea;
                    obj.destino = NombreDestino;
                    obj.garatiaId = (from pc in db.Garantias where pc.Garantias_Id == item.garantia_id select pc.Garantias_Descripcion).Single();
                    obj.realValor = item.Real_Valor;
                    obj.codeudorNit = item.codeudor_nit;
                    obj.nombreCodeudor = item.nombre_codeudor;
                    obj.costoAdicionalEnEltiempo = item.costoAdicionalEnEltiempo;
                    obj.costoAdicionalAnticipado = item.costoAdicionalAnticipado;
                    obj.costoAdicionalPrimeraCuota = item.costoAdicionalPrimeraCuota;
                    obj.costoAdicionalDividoEnElTiempo = item.costoAdicionalDividoEnElTiempo;
                    obj.ValorPorcentajeCostoAnticipado = item.ValorPorcentajeCostoAnticipado;
                    obj.ValorPorcentajeCostoEnCadaCuota = item.ValorPorcentajeCostoEnCadaCuota;

                    _Amortizacion.Add(obj);
                }
                return View("ImprimirAmortizacion", _Amortizacion);

            }

        }

        public ActionResult _Amortizacion(int id)
        {
            List<ViewModelCreditos> _Amortizacion = new List<ViewModelCreditos>();

            var ncreditos = (from prestamo in db.Prestamos
                             join costospres in db.CostosPrestamos on prestamo.Pagare equals costospres.Pagare
                             join costoadicional in db.Costos_Adicionales on costospres.CA_Id equals costoadicional.CA_Id
                             join periodo in db.Tipo_Periodo on prestamo.Tipo_Periodo_Id equals periodo.Tipo_Periodo_Id
                             where prestamo.id == id
                             select prestamo.id).Count();

            if (ncreditos == 0)
            {

                var ListaCre = (from prestamo in db.Prestamos

                                join periodo in db.Tipo_Periodo on prestamo.Tipo_Periodo_Id equals periodo.Tipo_Periodo_Id
                                join gartantiasCreditos in db.GarantiasCreditos on prestamo.Pagare equals gartantiasCreditos.pagare

                                where prestamo.id == id
                                select new
                                {
                                    prestamo.id,
                                    prestamo.Capital,
                                    prestamo.Interes,
                                    prestamo.Plazo,
                                    prestamo.Fecha_Prestamo,
                                    periodo.Tipo_Periodo_Valor,
                                    prestamo.myselect,
                                    prestamo.ValorPeriodo,
                                    prestamo.ValorSeguro,
                                    prestamo.ValorSeguroPorcentaje,
                                    prestamo.ValDiasInt,
                                    prestamo.difdias,
                                    prestamo.fechadesembolso,

                                    prestamo.costoAdicionalEnEltiempo,
                                    prestamo.costoAdicionalAnticipado,
                                    prestamo.costoAdicionalPrimeraCuota,
                                    prestamo.costoAdicionalDividoEnElTiempo,
                                    prestamo.ValorPorcentajeCostoAnticipado,
                                    prestamo.ValorPorcentajeCostoEnCadaCuota,

                                    gartantiasCreditos.garantia_id,
                                    gartantiasCreditos.Real_Valor,
                                    gartantiasCreditos.codeudor_nit,
                                    gartantiasCreditos.nombre_codeudor

                                }).ToList();

                foreach (var item in ListaCre)
                {
                    ViewModelCreditos obj = new ViewModelCreditos();
                    obj.id = item.id;
                    obj.Capital = item.Capital;
                    obj.Interes = item.Interes;
                    obj.Plazo = item.Plazo;
                    obj.Fecha_Prestamo = item.Fecha_Prestamo.ToString("dd/MM/yyy");
                    obj.CA_Valor = Convert.ToString(0);
                    obj.CA_Porcentaje = Convert.ToString(0);
                    obj.Tipo_Periodo_Valor = item.Tipo_Periodo_Valor;
                    obj.myselect = item.myselect;
                    obj.ValorPeriodo = item.ValorPeriodo;
                    obj.ValorSeguro = item.ValorSeguro;
                    obj.ValorSeguroPorcentaje = item.ValorSeguroPorcentaje;
                    obj.ValDiasInt = item.ValDiasInt;
                    obj.difdias = item.difdias;
                    obj.fechadesembolso = item.fechadesembolso.ToString("dd/MM/yyy");
                    obj.garatiaId = (from pc in db.Garantias where pc.Garantias_Id == item.garantia_id select pc.Garantias_Descripcion).Single();
                    obj.realValor = item.Real_Valor;
                    obj.codeudorNit = item.codeudor_nit;
                    obj.nombreCodeudor = item.nombre_codeudor;
                    obj.costoAdicionalEnEltiempo = item.costoAdicionalEnEltiempo;
                    obj.costoAdicionalAnticipado = item.costoAdicionalAnticipado;
                    obj.costoAdicionalPrimeraCuota = item.costoAdicionalPrimeraCuota;
                    obj.costoAdicionalDividoEnElTiempo = item.costoAdicionalDividoEnElTiempo;
                    obj.ValorPorcentajeCostoAnticipado = item.ValorPorcentajeCostoAnticipado;
                    obj.ValorPorcentajeCostoEnCadaCuota = item.ValorPorcentajeCostoEnCadaCuota;

                    _Amortizacion.Add(obj);
                }
                return PartialView("_Amortizacion", _Amortizacion);
            }
            else
            {

                var ListaCreditos = (from prestamo in db.Prestamos

                                     join costospres in db.CostosPrestamos on prestamo.Pagare equals costospres.Pagare
                                     join costoadicional in db.Costos_Adicionales on costospres.CA_Id equals costoadicional.CA_Id
                                     join gartantiasCreditos in db.GarantiasCreditos on prestamo.Pagare equals gartantiasCreditos.pagare
                                     join periodo in db.Tipo_Periodo on prestamo.Tipo_Periodo_Id equals periodo.Tipo_Periodo_Id

                                     where prestamo.id == id
                                     select new
                                     {
                                         prestamo.id,
                                         prestamo.Capital,
                                         prestamo.Interes,
                                         prestamo.Plazo,
                                         prestamo.Fecha_Prestamo,
                                         costoadicional.CA_Valor,
                                         costoadicional.CA_Porcentaje,
                                         periodo.Tipo_Periodo_Valor,
                                         prestamo.myselect,
                                         prestamo.ValorPeriodo,
                                         prestamo.ValorSeguro,
                                         prestamo.ValorSeguroPorcentaje,
                                         prestamo.ValDiasInt,
                                         prestamo.fechadesembolso,
                                         prestamo.difdias,
                                         gartantiasCreditos.garantia_id,
                                         gartantiasCreditos.Real_Valor,
                                         gartantiasCreditos.codeudor_nit,
                                         gartantiasCreditos.nombre_codeudor,

                                         prestamo.costoAdicionalEnEltiempo,
                                         prestamo.costoAdicionalAnticipado,
                                         prestamo.costoAdicionalPrimeraCuota,
                                         prestamo.costoAdicionalDividoEnElTiempo,
                                         prestamo.ValorPorcentajeCostoAnticipado,
                                         prestamo.ValorPorcentajeCostoEnCadaCuota

                                     }).ToList();

                foreach (var item in ListaCreditos)
                {
                    ViewModelCreditos obj = new ViewModelCreditos();
                    obj.id = item.id;
                    obj.Capital = item.Capital;
                    obj.Interes = item.Interes;
                    obj.Plazo = item.Plazo;
                    obj.Fecha_Prestamo = item.Fecha_Prestamo.ToString("dd/MM/yyy");
                    obj.CA_Valor = item.CA_Valor;
                    obj.CA_Porcentaje = item.CA_Porcentaje;
                    obj.Tipo_Periodo_Valor = item.Tipo_Periodo_Valor;
                    obj.myselect = item.myselect;
                    obj.ValorPeriodo = item.ValorPeriodo;
                    obj.ValorSeguro = item.ValorSeguro;
                    obj.ValorSeguroPorcentaje = item.ValorSeguroPorcentaje;
                    obj.ValDiasInt = item.ValDiasInt;
                    obj.difdias = item.difdias;
                    obj.fechadesembolso = item.fechadesembolso.ToString("dd/MM/yyy");
                    obj.garatiaId = (from pc in db.Garantias where pc.Garantias_Id == item.garantia_id select pc.Garantias_Descripcion).Single();
                    obj.realValor = item.Real_Valor;
                    obj.codeudorNit = item.codeudor_nit;
                    obj.nombreCodeudor = item.nombre_codeudor;
                    obj.costoAdicionalEnEltiempo = item.costoAdicionalEnEltiempo;
                    obj.costoAdicionalAnticipado = item.costoAdicionalAnticipado;
                    obj.costoAdicionalPrimeraCuota = item.costoAdicionalPrimeraCuota;
                    obj.costoAdicionalDividoEnElTiempo = item.costoAdicionalDividoEnElTiempo;
                    obj.ValorPorcentajeCostoAnticipado = item.ValorPorcentajeCostoAnticipado;
                    obj.ValorPorcentajeCostoEnCadaCuota = item.ValorPorcentajeCostoEnCadaCuota;
                    _Amortizacion.Add(obj);
                }
                return PartialView("_Amortizacion", _Amortizacion);
            }
        }

        public ActionResult _Desembolso(int id)
        {
            var pagare = (from pc in db.Prestamos where pc.id == id select pc.Pagare).Single();

            var credito = (from pc in db.Creditos where pc.Pagare == pagare select pc).Count();

            if (credito == 0)
            {
                List<ViewModelDesembolso> _Desembolso = new List<ViewModelDesembolso>();
                var ListaDesembolso = (from pres in db.Prestamos
                                       join asociados in db.Terceros on pres.NIT equals asociados.NIT
                                       where pres.id == id
                                       select new
                                       {
                                           pres.Capital,
                                           asociados.NIT,
                                           asociados.NOMBRE
                                       }).ToList();

                var CDesembolsoCredito = (from pc in db.Cuentas where pc.Funcion == "F3" select pc).Count();
                if (CDesembolsoCredito == 0)
                {
                    var des = new ViewModelDesembolso();
                    des.NOMBRE = "No hay una cuenta configurada para Desembolso(Credito)";
                    return PartialView("_AlertaCuentas", des);
                }

                var CDesembolsoDebito = (from pc in db.Cuentas where pc.Funcion == "F1" select pc).Count();
                if (CDesembolsoDebito == 0)
                {
                    var desc = new ViewModelDesembolso();
                    desc.NOMBRE = "No hay una cuenta configurada para Desembolso(Debito)";
                    return PartialView("_AlertaCuentas", desc);
                }

                var cuentaCreditoCod = (from pc in db.Cuentas where pc.Funcion == "F1" select pc.Cuenta_Cod).Single();
                var cuentaDebitoCod = (from pc in db.Cuentas where pc.Funcion == "F3" select pc.Cuenta_Cod).Single();
                var cuentaCreditoDescripcion = (from pc in db.Cuentas where pc.Funcion == "F1" select pc.Cuenta_Descripcion).Single();
                var cuentaDebitoDescripcion = (from pc in db.Cuentas where pc.Funcion == "F3" select pc.Cuenta_Descripcion).Single();

                foreach (var item in ListaDesembolso)
                {
                    ViewModelDesembolso obj = new ViewModelDesembolso();

                    obj.id = id;
                    obj.Capital = (item.Capital).ToString("#,##");//Aqui formateamos el saldo;
                    obj.NIT = item.NIT;
                    obj.NOMBRE = item.NOMBRE;
                    obj.cuentaCreditoCod = cuentaCreditoCod;
                    obj.cuentaDebitoCod = cuentaDebitoCod;
                    obj.cuentaCreditoDescripcion = cuentaCreditoDescripcion;
                    obj.cuentaDebitoDescripcion = cuentaDebitoDescripcion;

                    _Desembolso.Add(obj);
                }

                return PartialView("_Desembolso", _Desembolso);
            }
            else
            {
                return PartialView("Nada");
            }
        }

        public ActionResult _EliminarCredito(int id)
        {
            var eliminar = (from prestamos in db.Prestamos where prestamos.id == id && prestamos.estado == false select prestamos).FirstOrDefault();
            if (eliminar == null)
            {
                ViewBag.msj = "No se puede eliminar, crédito desembolsado";
                return PartialView("_ErrorEliminarCredito");
            }
            else
            {
                ViewBag.Pagare = eliminar.Pagare;
                ViewBag.Id = eliminar.id;
                ViewBag.nombre = eliminar.NOMBRE;

                return PartialView("_EliminarCredito");
            }

        }

        [HttpPost]
        public ActionResult _EliminarCreditoPost(int id, string pagare)
        {

            Prestamos prestamos = db.Prestamos.Find(id);
            if (prestamos != null)
            {
                db.Prestamos.Remove(prestamos);
                db.SaveChanges();
            }

            var deleteCostoPrestamos = from Costoprestamo in db.CostosPrestamos
                                       where Costoprestamo.Pagare == pagare
                                       select Costoprestamo;
            if (deleteCostoPrestamos != null)
            {

                foreach (var CostoP in deleteCostoPrestamos)
                {
                    db.CostosPrestamos.Remove(CostoP);
                }

                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    return View(e);
                }

                return new JsonResult { Data = new { status = true } };
            }
            else
            {
                return new JsonResult { Data = new { status = false } };
            }
        }

        [HttpPost]
        public JsonResult ComprobarDesembolso(int id)
        {
            var pagare = (from pc in db.Prestamos where pc.id == id select pc.Pagare).Single();

            var credito = (from pc in db.Creditos where pc.Pagare == pagare select pc).Count();

            if (credito == 0)
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(1, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RealizarDesembolso([Bind(Include = "saldoCreditoDesembolsado,id")] ViewModelDesembolso datosDesembolso)
        {
            var cuentaCreditoCod = (from pc in db.Cuentas where pc.Funcion == "F3" select pc).Single();
            var cuentaDebitoCod = (from pc in db.Cuentas where pc.Funcion == "F1" select pc).Single();

            var prestamo = db.Prestamos.FirstOrDefault(j => j.id == datosDesembolso.id);
            var idLinea = (from pc in db.Destinos where pc.Destino_Id == prestamo.Destino_Id select pc.Lineas_Id).Single();
            var garantiasCreditos = db.GarantiasCreditos.FirstOrDefault(j => j.pagare == prestamo.Pagare);
            var usuarioid = "";
            if (User.Identity.IsAuthenticated)
            {
                usuarioid = User.Identity.Name.ToString();
            }
            else
            {
                return RedirectToAction("Index", new { Controller = "Default", Area = "Dashboard" });
            }

            var InteresDividido = prestamo.Interes / 100;
            var opera = Math.Pow(Convert.ToDouble((1 + InteresDividido)), Convert.ToDouble(-prestamo.Plazo));
            var Cuota = prestamo.Capital * (InteresDividido / (1 - Convert.ToDecimal(opera)));

            var costoAdicionalEnEltiempo = prestamo.costoAdicionalEnEltiempo;
            var ValorPorcentajeCostoAnticipado = prestamo.ValorPorcentajeCostoAnticipado;
            var costoAdicionalAnticipado = prestamo.costoAdicionalAnticipado;
            var costoAdicionalDividoEnElTiempo = prestamo.costoAdicionalDividoEnElTiempo;
            var costoAdicionalPrimeraCuota = prestamo.costoAdicionalPrimeraCuota;
            var ValorPorcentajeCostoEnCadaCuota = prestamo.ValorPorcentajeCostoEnCadaCuota;

            var AbonoInteres = prestamo.Capital * (prestamo.Interes / 100);
            var AbonoCapital = Cuota - AbonoInteres;
            var Plazo = Convert.ToInt32(prestamo.Plazo);
            var seleccion = prestamo.myselect;
            var ValDiasInt = prestamo.ValDiasInt;
            var Capital = Convert.ToDecimal(prestamo.Capital);
            var Per = prestamo.ValorPeriodo;

            var consecutivo = db.CConsecutivos.FirstOrDefault(j => j.idDestino == prestamo.Destino_Id & j.estado == true);
            var cod = consecutivo.tipoCodPagare;
            var actual = consecutivo.consecutivoPagareActual;

            if (costoAdicionalDividoEnElTiempo != 0)
            {
                costoAdicionalDividoEnElTiempo = costoAdicionalDividoEnElTiempo / Plazo;
            }

            var ValorPorcentaje = prestamo.ValorSeguroPorcentaje;
            var ValorPorcentajeParaTabla = 0;
            if (ValorPorcentaje != 0)
            {
                ValorPorcentaje = ValorPorcentaje / 100;
                ValorPorcentajeParaTabla = Convert.ToInt32(Capital * ValorPorcentaje);
            }
            else
            {
                ValorPorcentaje = 1;
                ValorPorcentajeParaTabla = 0;
            }

            if (ValorPorcentajeCostoEnCadaCuota != 0)
            {
                ValorPorcentajeCostoEnCadaCuota = ValorPorcentajeCostoEnCadaCuota / 100;
                ValorPorcentajeCostoEnCadaCuota = (Capital * ValorPorcentajeCostoEnCadaCuota);
                costoAdicionalEnEltiempo = costoAdicionalEnEltiempo + ValorPorcentajeCostoEnCadaCuota;
            }

            if (seleccion == 2 && ValDiasInt != 0)
            {
                costoAdicionalEnEltiempo = costoAdicionalEnEltiempo + (ValDiasInt / Plazo);
            }
            else if (seleccion == 3)
            {
                costoAdicionalPrimeraCuota = costoAdicionalPrimeraCuota + ValDiasInt;
            }

            var ValorCuota = (AbonoCapital + AbonoInteres + costoAdicionalEnEltiempo + costoAdicionalPrimeraCuota + costoAdicionalDividoEnElTiempo + ValorPorcentaje + ValorPorcentajeParaTabla);
            var ValorCuotaSinCostoAdicionalPrimeraCuota = (AbonoCapital + AbonoInteres + costoAdicionalEnEltiempo + costoAdicionalDividoEnElTiempo + ValorPorcentaje);
            var ValorCostoFijo = costoAdicionalEnEltiempo + costoAdicionalPrimeraCuota + costoAdicionalDividoEnElTiempo;
            var ValorCostoFijoSinCostoPrimeraCuotaSinInteresAnticipado = costoAdicionalEnEltiempo + costoAdicionalDividoEnElTiempo;
            var SaldoCapital = Capital - AbonoCapital;

            for (var i = 1; i <= Plazo; i++)
            {
                var InteresMensual = InteresDividido * Capital;
                Capital = Capital - Cuota + InteresMensual;

                var lineaAmortizacion = new Amortizaciones()
                {
                    pagare = cod + actual,
                    calendarioDePagos = "Fecha",
                    valorCuota = Convert.ToInt32(ValorCuota).ToString(),
                    abonoCapital = Convert.ToInt32(AbonoCapital).ToString(),
                    abonoInteres = Convert.ToInt32(AbonoInteres).ToString(),
                    saldoCapital = Convert.ToInt32(SaldoCapital).ToString(),
                    valorCosto = Convert.ToInt32(ValorCostoFijo).ToString(),
                    valorCostoPorcentaje = Convert.ToInt32(ValorPorcentajeParaTabla).ToString()
                };

                db.Amortizaciones.Add(lineaAmortizacion);

                AbonoInteres = SaldoCapital * InteresDividido;
                AbonoCapital = Cuota - AbonoInteres;
                SaldoCapital = SaldoCapital - AbonoCapital;
                ValorCostoFijo = ValorCostoFijoSinCostoPrimeraCuotaSinInteresAnticipado;
                if (ValorPorcentajeParaTabla != 0)
                {
                    ValorPorcentajeParaTabla = Convert.ToInt32(SaldoCapital * ValorPorcentaje);
                }
                ValorCuota = AbonoCapital + AbonoInteres + ValorCostoFijo + ValorPorcentajeParaTabla;
            }

            var garantiasDeCreditos = (from pc in db.GarantiasCreditos where pc.pagare == prestamo.Pagare select pc).Single(); //BUSCAR UN SOLO RESULTADO

            var cantidadDeCostosPrestamos = db.CostosPrestamos.Where(a => a.Pagare == prestamo.Pagare).Count();
            if (cantidadDeCostosPrestamos != 0)
            {
                var listaDeCostos = db.CostosPrestamos.Where(a => a.Pagare == prestamo.Pagare).ToList();
                foreach (var costo in listaDeCostos)
                {
                    costo.Pagare = cod + actual;
                    db.Entry(costo).State = System.Data.Entity.EntityState.Modified;
                }
            }

            var varBCreditos = new BCreditos()
            {
                Prestamo_Id = datosDesembolso.id,
                Creditos_Cedula = prestamo.NIT,
                Lineas_Id = idLinea,
                Destino_Id = prestamo.Destino_Id,
                Garantias_Id = garantiasCreditos.garantia_id,
                Capital = prestamo.Capital,
                Creditos_Estado = false,
                Creditos_Interes = prestamo.Interes,
                Creditos_Plazo = prestamo.Plazo,
                Creditos_Cuota = "pendiente",
                Creditos_Interes_Mora = (from pc in db.Destinos where pc.Destino_Id == prestamo.Destino_Id select pc.Destino_Tasa_Mora).Single(),
                Creditos_Saldo_Capital = datosDesembolso.saldoCreditoDesembolsado,
                Codigo_Operador_Id = Convert.ToInt32(User.Identity.Name),
                Pagare = cod + actual
            };
            db.Creditos.Add(varBCreditos);
            var valorInteresMora = Convert.ToDecimal(3.6);
            var proximoPago = prestamo.fechadesembolso.AddMonths(1);
            var nuevoHistorialCredito = new HistorialCreditos()
            {
                fecha = prestamo.fechadesembolso,
                idFactura = 0,
                NIT = prestamo.NIT,
                pagare = cod + actual,
                abonoCapital = 0,
                abonoInteresMora = 0,
                AbonoInteresCorriente = 0,
                valorCosto = 0,
                saldoCapital = prestamo.Capital,
                proximaCuota = ValorCuota,
                capitalEnMora = 0,
                TazaInteresMora = valorInteresMora,
                TazaInteresCorriente = prestamo.Interes,
                diasCausados = 0,
                diasEnMora = 0,
                numeroCuota = 1,
                fechaProximoPago = proximoPago,
                interesCorriente = 0,
                interesCorrienteMora = 0,
                estado = "normal",
                interesMora = 0
            };
            db.HistorialCreditos.Add(nuevoHistorialCredito);

            //CONTRUIR EL COMPROBANTE
            var consecutivoComprobante = db.TiposComprobantes.FirstOrDefault(x => x.CODIGO == cuentaCreditoCod.TipoComprobante & x.INACTIVO == false);

            var comprobantef = new Comprobante()
            {
                TIPO = cuentaCreditoCod.TipoComprobante,
                NUMERO = consecutivoComprobante.CONSECUTIVO,
                ANO = Convert.ToString(DateTime.Now.Year),
                MES = Convert.ToString(DateTime.Now.Month),
                DIA = Convert.ToString(DateTime.Now.Day),
                CCOSTO = "00",
                DETALLE = "DESEMBOLSO",
                TERCERO = prestamo.NIT,
                CTAFPAGO = cuentaCreditoCod.Cuenta_Cod,
                VRTOTAL = prestamo.Capital,
                SUMDBCR = 0,
                FECHARealiz = DateTime.Now,
                ANULADO = false
            };

            db.Comprobantes.Add(comprobantef);

            //COMTRUIR LA LISTA DE MOVIMIENTOS
            List<Movimiento> listaDeMovimientos = new List<Movimiento>();
            var sumaDeCostos = 0;
            if (cantidadDeCostosPrestamos != 0)
            {
                var listaDeCostos = db.CostosPrestamos.Where(a => a.Pagare == prestamo.Pagare).ToList();
                foreach (var costo in listaDeCostos)
                {
                    if (costo.seCobraComo == 4)
                    {
                        var costoAdicional = db.Costos_Adicionales.Where(b => b.CA_Id == costo.CA_Id).Single();

                        var cuentaCosto = Convert.ToString(costoAdicional.Cuenta_Cod);
                        var valorCosto = Convert.ToDecimal(costoAdicional.CA_Valor);

                        var mov = new Movimiento()
                        {
                            TIPO = cuentaCreditoCod.TipoComprobante,
                            NUMERO = consecutivoComprobante.CONSECUTIVO,
                            CUENTA = cuentaCosto,
                            TERCERO = prestamo.NIT,
                            DETALLE = "DESEMBOLSO",
                            DEBITO = 0,
                            CREDITO = valorCosto,
                            BASE = 0,
                            CCOSTO = "07",
                            FECHAMOVIMIENTO = DateTime.Now,
                        };
                        sumaDeCostos = sumaDeCostos + Convert.ToInt32(valorCosto);

                        listaDeMovimientos.Add(mov);

                    }
                    else if (costo.seCobraComo == 5)
                    {
                        var costoAdicional = db.Costos_Adicionales.Where(b => b.CA_Id == costo.CA_Id).Single();

                        var cuentaCosto = Convert.ToString(costoAdicional.Cuenta_Cod);
                        var valorPorcentaje = Convert.ToDecimal(costoAdicional.CA_Porcentaje.Replace(".", ","));
                        valorPorcentaje = (valorPorcentaje / 100) * prestamo.Capital;

                        var mov = new Movimiento()
                        {
                            TIPO = cuentaCreditoCod.TipoComprobante,
                            NUMERO = consecutivoComprobante.CONSECUTIVO,
                            CUENTA = cuentaCosto,
                            TERCERO = prestamo.NIT,
                            DETALLE = "DESEMBOLSO",
                            DEBITO = 0,
                            CREDITO = valorPorcentaje,
                            BASE = 0,
                            CCOSTO = "07",
                            FECHAMOVIMIENTO = DateTime.Now,
                        };
                        listaDeMovimientos.Add(mov);
                        sumaDeCostos = sumaDeCostos + Convert.ToInt32(valorPorcentaje);

                        //calculamos el iva si está configurado al costo de estudio de crédito
                        var cuentaIVA = db.Cuentas.Where(x => x.Funcion == "F10").FirstOrDefault();
                        if (cuentaIVA != null)
                        {
                            decimal valorIVA = valorPorcentaje * Convert.ToDecimal(0.19);
                            var movIVA = new Movimiento()
                            {
                                TIPO = cuentaCreditoCod.TipoComprobante,
                                NUMERO = consecutivoComprobante.CONSECUTIVO,
                                CUENTA = cuentaIVA.Cuenta_Cod,
                                TERCERO = prestamo.NIT,
                                DETALLE = "DESEMBOLSO",
                                DEBITO = 0,
                                CREDITO = valorIVA,
                                BASE = 0,
                                CCOSTO = "07",
                                FECHAMOVIMIENTO = DateTime.Now,
                            };
                            listaDeMovimientos.Add(movIVA);
                            sumaDeCostos += Convert.ToInt32(valorIVA);
                        }


                    }
                }
            }

            var desembolsoTotal = prestamo.Capital - sumaDeCostos;

            var mov1 = new Movimiento()
            {
                TIPO = cuentaCreditoCod.TipoComprobante,
                NUMERO = consecutivoComprobante.CONSECUTIVO,
                CUENTA = cuentaCreditoCod.Cuenta_Cod,
                TERCERO = prestamo.NIT,
                DETALLE = "DESEMBOLSO",
                DEBITO = 0,
                CREDITO = desembolsoTotal,
                BASE = 0,
                CCOSTO = "07",
                FECHAMOVIMIENTO = DateTime.Now,
            };

            listaDeMovimientos.Add(mov1);
            var mov2 = new Movimiento()
            {
                TIPO = cuentaCreditoCod.TipoComprobante,
                NUMERO = consecutivoComprobante.CONSECUTIVO,
                CUENTA = cuentaDebitoCod.Cuenta_Cod,
                TERCERO = prestamo.NIT,
                DETALLE = "DESEMBOLSO",
                DEBITO = prestamo.Capital,
                CREDITO = 0,
                BASE = 0,
                CCOSTO = "07",
                FECHAMOVIMIENTO = DateTime.Now,
            };
            listaDeMovimientos.Add(mov2);

            var result = false;


            var comprobanteConst = new ComprobanteBO();
            result = comprobanteConst.AsentarDesembolso(listaDeMovimientos, Convert.ToInt32(consecutivoComprobante.CONSECUTIVO), cuentaCreditoCod.TipoComprobante);

            if (result)
            {
                prestamo.Pagare = cod + actual;
                garantiasDeCreditos.pagare = cod + actual;
                consecutivo.consecutivoPagareActual = consecutivo.consecutivoPagareActual + 1;
                consecutivo.prestamo = 1;

                prestamo.estado = true;
                prestamo.fechadesembolso = DateTime.Now;
                db.Entry(prestamo).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return View("Index");
        }

        // GET: Creditos/Creditos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BCreditos bCreditos = db.Creditos.Find(id);
            if (bCreditos == null)
            {
                return HttpNotFound();
            }
            return View(bCreditos);
        }

        // GET: Creditos/Creditos/Create
        public ActionResult Create()
        {
            ViewBag.Codigo_Operador_Id = new SelectList(db.Codigo_Operador, "Codigo_Operador_Id", "Codigo_Operador_Descripcion");
            ViewBag.Destino_Id = new SelectList(db.Destinos, "Destino_Id", "Destino_Descripcion");
            ViewBag.Garantias_Id = new SelectList(db.Garantias, "Garantias_Id", "Garantias_Descripcion");
            ViewBag.Lineas_Id = new SelectList(db.Lineas, "Lineas_Id", "Lineas_Descripcion");
            return View();
        }

        // POST: Creditos/Creditos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Creditos_Id,Creditos_Cedula,Lineas_Id,Destino_Id,Garantias_Id,Capital,Creditos_Estado,Creditos_Interes,Creditos_Plazo,Creditos_Cuota,Creditos_Interes_Mora,Creditos_Saldo_Capital,Codigo_Operador_Id")] BCreditos bCreditos)
        {
            if (ModelState.IsValid)
            {
                db.Creditos.Add(bCreditos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Codigo_Operador_Id = new SelectList(db.Codigo_Operador, "Codigo_Operador_Id", "Codigo_Operador_Descripcion", bCreditos.Codigo_Operador_Id);
            ViewBag.Destino_Id = new SelectList(db.Destinos, "Destino_Id", "Destino_Descripcion", bCreditos.Destino_Id);
            ViewBag.Garantias_Id = new SelectList(db.Garantias, "Garantias_Id", "Garantias_Descripcion", bCreditos.Garantias_Id);
            ViewBag.Lineas_Id = new SelectList(db.Lineas, "Lineas_Id", "Lineas_Descripcion", bCreditos.Lineas_Id);
            return View(bCreditos);
        }

        // GET: Creditos/Creditos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BCreditos bCreditos = db.Creditos.Find(id);
            if (bCreditos == null)
            {
                return HttpNotFound();
            }
            ViewBag.Codigo_Operador_Id = new SelectList(db.Codigo_Operador, "Codigo_Operador_Id", "Codigo_Operador_Descripcion", bCreditos.Codigo_Operador_Id);
            ViewBag.Destino_Id = new SelectList(db.Destinos, "Destino_Id", "Destino_Descripcion", bCreditos.Destino_Id);
            ViewBag.Garantias_Id = new SelectList(db.Garantias, "Garantias_Id", "Garantias_Descripcion", bCreditos.Garantias_Id);
            ViewBag.Lineas_Id = new SelectList(db.Lineas, "Lineas_Id", "Lineas_Descripcion", bCreditos.Lineas_Id);
            return View(bCreditos);
        }

        // POST: Creditos/Creditos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Creditos_Id,Creditos_Cedula,Lineas_Id,Destino_Id,Garantias_Id,Capital,Creditos_Estado,Creditos_Interes,Creditos_Plazo,Creditos_Cuota,Creditos_Interes_Mora,Creditos_Saldo_Capital,Codigo_Operador_Id")] BCreditos bCreditos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bCreditos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Codigo_Operador_Id = new SelectList(db.Codigo_Operador, "Codigo_Operador_Id", "Codigo_Operador_Descripcion", bCreditos.Codigo_Operador_Id);
            ViewBag.Destino_Id = new SelectList(db.Destinos, "Destino_Id", "Destino_Descripcion", bCreditos.Destino_Id);
            ViewBag.Garantias_Id = new SelectList(db.Garantias, "Garantias_Id", "Garantias_Descripcion", bCreditos.Garantias_Id);
            ViewBag.Lineas_Id = new SelectList(db.Lineas, "Lineas_Id", "Lineas_Descripcion", bCreditos.Lineas_Id);
            return View(bCreditos);
        }

        // GET: Creditos/Creditos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BCreditos bCreditos = db.Creditos.Find(id);
            if (bCreditos == null)
            {
                return HttpNotFound();
            }
            return View(bCreditos);
        }

        // POST: Creditos/Creditos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BCreditos bCreditos = db.Creditos.Find(id);
            db.Creditos.Remove(bCreditos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}