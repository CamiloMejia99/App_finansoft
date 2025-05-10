using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing;
using FNTC.Finansoft.Accounting.BLL;
using FNTC.Finansoft.Accounting.BLL.FabricaCreditosBll;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.UI.Areas.Accounting.Controllers.Movimientos.Informes;
using FNTC.Finansoft.UI.Tools;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using static FNTC.Finansoft.UI.Enums.EnumsProgram;
using System.Globalization;
//using .Terceros;

namespace FNTC.Finansoft.UI.Areas.Creditos.Controllers
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
                presta.NOMBRE = (prestamo.terceroFK!=null) ? prestamo.terceroFK.NOMBRE1+" "+ prestamo.terceroFK.NOMBRE2 + " "+ prestamo.terceroFK.APELLIDO1+" "+ prestamo.terceroFK.APELLIDO2 : "";
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
                                    gartantiasCreditos.nombre_codeudor,

                                    prestamo.Destino_Id

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
                    obj.destino = item.Destino_Id.ToString();
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
                                         gartantiasCreditos.nombre_codeudor,

                                         prestamo.Destino_Id
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
                    obj.destino = item.Destino_Id.ToString();
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
                                    gartantiasCreditos.nombre_codeudor,

                                    prestamo.Destino_Id

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
                    obj.destino = item.Destino_Id.ToString();
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
                                         prestamo.ValorPorcentajeCostoEnCadaCuota,

                                         prestamo.Destino_Id

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
                    obj.destino = item.Destino_Id.ToString();
                    _Amortizacion.Add(obj);
                }
                return PartialView("_Amortizacion", _Amortizacion);
            }
        }

        public ActionResult _Desembolso(int id)
        {


            var fechaRegistrob = DateTime.Now;
            DateTime Fechas = Convert.ToDateTime(fechaRegistrob);
            Fechas = Fechas.AddDays(-20);
            string Date = Fechas.ToString("yyyy-MM-dd");
            ViewBag.FechasAtras = Date;

            var FechaDespues = DateTime.Now;
            DateTime FechasD = Convert.ToDateTime(FechaDespues);
            string Date2 = FechasD.ToString("yyyy-MM-dd");

            ViewBag.FechasAdelante = Date2;
            ViewBag.Autorizacion = new ConfiguracionBll().ObtenerAutorizacion(id);

            var prestamo = db.Prestamos.Where(x => x.id == id).FirstOrDefault();
            var ValorCapital = prestamo.Capital;
            var ValorCapitalsaldo = prestamo.Capital.ToString();
            var valor = Convert.ToDecimal(ValorCapitalsaldo);
            var ValorPapeleria = prestamo.costoAdicionalAnticipado;
            var Interes = ValorCapital * 0.01;
            var ValorInteres = prestamo.Interes;
            var InteresDiario = Interes / 30;
            var dias = 20;
            var InteresAnticipado = InteresDiario * dias;
            var conversion = InteresAnticipado.ToString();
            var intereses = Convert.ToDecimal(conversion);
            
            var resultad = Convert.ToDecimal(InteresAnticipado);
            var decimalValu = decimal.Round(resultad, 0);
            ViewBag.ValorInteres = ValorInteres;
            ViewBag.valorcapital = ValorCapital;
            ViewBag.interesvalor = decimalValu;
            ViewBag.papeleriavalor = ValorPapeleria;

            var Calculoin = ValorCapital - ValorPapeleria;
            var decimalValue = decimal.Round(Calculoin, 0);
            ViewBag.cajavalor = decimalValue;

            var cuentaCreditoCodNC = (from pc in db.Cuentas where pc.Funcion == "F3" && pc.Destino_Id == prestamo.Destino_Id select pc).FirstOrDefault();
            var cuentaDebitoCodNC = (from pc in db.Cuentas where pc.Funcion == "F1" && pc.Destino_Id == prestamo.Destino_Id select pc).FirstOrDefault();
            var interesNC = (from pc in db.Cuentas where pc.Funcion == "F4" && pc.Destino_Id == prestamo.Destino_Id select pc).FirstOrDefault();
            var bancosNC = (from pc in db.Cuentas where pc.Funcion == "F16" && pc.Destino_Id == prestamo.Destino_Id select pc).FirstOrDefault();
            var papeleriaNC = (from pc in db.Cuentas where pc.Funcion == "F17" && pc.Destino_Id == prestamo.Destino_Id select pc).FirstOrDefault();

            var creditoC = cuentaDebitoCodNC.Cuenta_Cod;
            ViewBag.credito = creditoC;
            var caja = cuentaCreditoCodNC.Cuenta_Cod;
            ViewBag.caja = caja;
            var banco = bancosNC.Cuenta_Cod;
            ViewBag.banco = banco;
            var interes = interesNC.Cuenta_Cod;
            ViewBag.interes = interes;
            var papeleria = papeleriaNC.Cuenta_Cod;
            ViewBag.papeleria = papeleria;

            var creditoN = cuentaDebitoCodNC.Cuenta_Descripcion;
            ViewBag.creditoN = creditoN;
            var cajaN = cuentaCreditoCodNC.Cuenta_Descripcion;
            ViewBag.cajaN = cajaN;
            var bancoN = bancosNC.Cuenta_Descripcion;
            ViewBag.bancoN = bancoN;
            var interesN = interesNC.Cuenta_Descripcion;
            ViewBag.interesN = interesN;
            var papeleriaN = papeleriaNC.Cuenta_Descripcion;
            ViewBag.papeleriaN = papeleriaN;


            var credito = (from pc in db.Creditos where pc.Pagare == prestamo.Pagare select pc).Count();

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

                var CDesembolsoCredito = (from pc in db.Cuentas where pc.Funcion == "F3" && pc.Destino_Id == prestamo.Destino_Id select pc).Count();
                if (CDesembolsoCredito == 0)
                {
                    var des = new ViewModelDesembolso();
                    des.NOMBRE = "No hay una cuenta configurada para Desembolso(Credito)";
                    return PartialView("_AlertaCuentas", des);
                }

                var CDesembolsoDebito = (from pc in db.Cuentas where pc.Funcion == "F1" && pc.Destino_Id == prestamo.Destino_Id select pc).Count();
                if (CDesembolsoDebito == 0)
                {
                    var desc = new ViewModelDesembolso();
                    desc.NOMBRE = "No hay una cuenta configurada para Desembolso(Debito)";
                    return PartialView("_AlertaCuentas", desc);
                }

                var cuentaCreditoCod = (from pc in db.Cuentas where pc.Funcion == "F1" && pc.Destino_Id == prestamo.Destino_Id select pc.Cuenta_Cod).FirstOrDefault();
                var cuentaDebitoCod = (from pc in db.Cuentas where pc.Funcion == "F3" && pc.Destino_Id == prestamo.Destino_Id select pc.Cuenta_Cod).FirstOrDefault();
                var cuentaCreditoDescripcion = (from pc in db.Cuentas where pc.Funcion == "F1" && pc.Destino_Id == prestamo.Destino_Id select pc.Cuenta_Descripcion).FirstOrDefault();
                var cuentaDebitoDescripcion = (from pc in db.Cuentas where pc.Funcion == "F3" && pc.Destino_Id == prestamo.Destino_Id select pc.Cuenta_Descripcion).FirstOrDefault();

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
            if (prestamos!=null)
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
        public ActionResult RealizarDesembolso([Bind(Include = "saldoCreditoDesembolsado,id,fechadesembolso,diapago,BANCO,cajain,Interes,Resultado")] ViewModelDesembolso datosDesembolso)
        {
            
            var prestamo = db.Prestamos.FirstOrDefault(j => j.id == datosDesembolso.id);
            var cuentaCreditoCod = (from pc in db.Cuentas where pc.Funcion == "F3" && pc.Destino_Id == prestamo.Destino_Id select pc).FirstOrDefault();
            var cuentaDebitoCod = (from pc in db.Cuentas where pc.Funcion == "F1" && pc.Destino_Id == prestamo.Destino_Id select pc).FirstOrDefault();
            var bancosNC = (from pc in db.Cuentas where pc.Funcion == "F16" && pc.Destino_Id == prestamo.Destino_Id select pc).FirstOrDefault();
            var interesNC = (from pc in db.Cuentas where pc.Funcion == "F4" && pc.Destino_Id == prestamo.Destino_Id select pc).FirstOrDefault();
            var papeleriaNC = (from pc in db.Cuentas where pc.Funcion == "F17" && pc.Destino_Id == prestamo.Destino_Id select pc).FirstOrDefault();

            var Periodo = Convert.ToInt32(prestamo.Tipo_Periodo.Tipo_Periodo_Valor) / 30;
            
            prestamo.ValDiasInt = 0;
            prestamo.difdias = 0;
            string fecha = "1/01/0001";
            DateTime FechaDeDesembolso;
            if (datosDesembolso.fechadesembolso == (Convert.ToDateTime(fecha)))
            {

                FechaDeDesembolso = FechaLocal.GetFechaColombia();
            }
            else
            {
                FechaDeDesembolso = datosDesembolso.fechadesembolso;
            }
            
            prestamo.fechadesembolso = FechaDeDesembolso;
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

            var InteresDividido = (prestamo.Interes / 100)*Periodo;
            var opera = Math.Pow(Convert.ToDouble((1 + InteresDividido)), Convert.ToDouble(-prestamo.Plazo/Periodo));
            var Cuota = (prestamo.Capital) * (InteresDividido / (1 - Convert.ToDecimal(opera)));

            var costoAdicionalEnEltiempo = prestamo.costoAdicionalEnEltiempo;
            var ValorPorcentajeCostoAnticipado = prestamo.ValorPorcentajeCostoAnticipado;
            var costoAdicionalAnticipado = prestamo.costoAdicionalAnticipado;
            var costoAdicionalDividoEnElTiempo = prestamo.costoAdicionalDividoEnElTiempo;
            var costoAdicionalPrimeraCuota = prestamo.costoAdicionalPrimeraCuota;
            var ValorPorcentajeCostoEnCadaCuota = prestamo.ValorPorcentajeCostoEnCadaCuota;

            var Capital = Convert.ToDecimal(prestamo.Capital);
            var AbonoInteres = (Capital) * ((prestamo.Interes / 100)*Periodo);
            var AbonosInteres = (Capital) * (prestamo.Interes / 100)*Periodo;
            var AbonoCapital = Cuota - AbonoInteres;
            var Plazo = Convert.ToInt32(prestamo.Plazo);
            var seleccion = prestamo.myselect;
            var ValDiasInt = prestamo.ValDiasInt;
            var Per = prestamo.ValorPeriodo;

            var consecutivo = db.CConsecutivos.FirstOrDefault(j => j.idDestino == prestamo.Destino_Id & j.estado == true);
            var cod = consecutivo.tipoCodPagare;
            var actual = consecutivo.consecutivoPagareActual;
            string pagare = cod + actual;

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

            //var ValorCuota = (AbonoCapital + AbonoInteres + costoAdicionalPrimeraCuota + costoAdicionalDividoEnElTiempo+ ValorPorcentajeParaTabla);
            var ValorCuota = (AbonoCapital + AbonoInteres+prestamo.ValorSeguro+prestamo.CtoAdmon);
            var ValorCuotaSinCostoAdicionalPrimeraCuota = (AbonoCapital + AbonoInteres + costoAdicionalEnEltiempo + costoAdicionalDividoEnElTiempo + ValorPorcentaje);
            var ValorCostoFijo = costoAdicionalEnEltiempo + costoAdicionalPrimeraCuota + costoAdicionalDividoEnElTiempo;
            int ValorCostoFijoCuota = Convert.ToInt32( ValorCostoFijo / prestamo.Plazo)*Periodo;
            var ValorCostoFijoSinCostoPrimeraCuotaSinInteresAnticipado = costoAdicionalEnEltiempo + costoAdicionalDividoEnElTiempo;
            var SaldoCapital = Capital - AbonoCapital;

            

            DateTime FechaProxPag = new DateTime(prestamo.fechadesembolso.Year, prestamo.fechadesembolso.Month, datosDesembolso.diapago);
            prestamo.Fecha_Prestamo = FechaProxPag;
            DateTime FechaProxPago = FechaProxPag.AddMonths(Periodo);
            DateTime FechaProxPagoTotales = FechaProxPag.AddMonths(Periodo);

            var FechaDia = FechaLocal.GetFechaColombia();
            var DiasCausados = FechaDia - prestamo.fechadesembolso;
            int segundos = (((DiasCausados.Days * 24) * 3600) + (DiasCausados.Hours * 3600) + (DiasCausados.Minutes * 60) + (DiasCausados.Seconds));
            var DiasEXcausados = 0;
            if (segundos < 86400)
            {
                DiasEXcausados = 0;
            }
            else
            {
                DiasEXcausados = segundos / 86400;
            }


            //variables para linea de control creditos
            decimal AuxCapital = 0, AuxValorCuota = 0, AuxSaldoCapital = 0;

            //...
            int J = 1;
            for (var i = Periodo; i <= Plazo; i+=Periodo)
            {
                var InteresMensual = InteresDividido * Capital;
                //Capital = Math.Round((Capital - Cuota + InteresMensual), 0, MidpointRounding.ToEven);

                AuxCapital = AbonoCapital;
                AuxSaldoCapital = SaldoCapital+AbonoCapital;
                AuxValorCuota= ValorCuota;

                var lineaAmortizacion = new Amortizaciones()
                {
                    pagare = cod + actual,
                    calendarioDePagos = FechaProxPago.ToString(),
                    valorCuota = Convert.ToInt32(ValorCuota).ToString(),
                    abonoCapital = Convert.ToInt32(AbonoCapital).ToString(),
                    abonoInteres = Convert.ToInt32(AbonoInteres).ToString(),
                    saldoCapital = Convert.ToInt32(SaldoCapital).ToString(),
                    valorCosto = Convert.ToInt32(prestamo.ValorSeguro).ToString(),
                    valorCostoPorcentaje = Convert.ToInt32(ValorPorcentajeParaTabla).ToString()
                };
                db.Amortizaciones.Add(lineaAmortizacion);

                var LineaControlCredito = new ControlCreditos()
                {
                    Pagare = pagare,
                    NumCuota = J,
                    FechaPago = FechaProxPago,
                    DiasMora = 0,
                    DiasCausados = (J == 1) ? DiasEXcausados : 0,
                    Capital = Math.Round(AuxCapital, 0, MidpointRounding.ToEven),
                    SaldoCapitalEnCuota = Math.Round(AuxSaldoCapital, 0, MidpointRounding.ToEven),
                    InteresCorriente = (J == 1) ? Math.Round(AbonosInteres, 0, MidpointRounding.ToEven) : 0,
                    InteresMora = 0,
                    Seguro = (J==1) ? prestamo.ValorSeguro : 0,
                    CtoAdmon = (J==1) ? prestamo.CtoAdmon : 0,
                    ValorCuota = (J == 1) ? Math.Round(AuxValorCuota, 0, MidpointRounding.ToEven) : Math.Round(AuxValorCuota - AbonoInteres, 0, MidpointRounding.ToEven),
                    EstadoEnCredito = "AD",
                    EstadoEnOperacion = (J == 1) ? true : false
                };
                db.ControlCreditos.Add(LineaControlCredito);

                FechaProxPago = FechaProxPago.AddMonths(Periodo);
                AbonoInteres = SaldoCapital * InteresDividido;
                AbonoCapital = Cuota - AbonoInteres;
                SaldoCapital = SaldoCapital - AbonoCapital;
                ValorCostoFijo = prestamo.ValorSeguro;
                if (ValorPorcentajeParaTabla != 0)
                {
                    ValorPorcentajeParaTabla = Convert.ToInt32(SaldoCapital * ValorPorcentaje);
                }
                ValorCuota = (AbonoCapital + AbonoInteres) ;

                J++;
            }

            var TotalesCreditos = new TotalesCreditos()
            {
                Pagare = pagare,
                CapitalTotal = 0,
                SaldoCapital = prestamo.Capital,
                CapitalMoraPendiente = 0,
                InteresCorrienteTotal = 0,
                InteresMoraTotal = 0,
                SeguroTotal = 0,
                CtoAdmonTotal=0,
                InteresCorrientePendiente = Math.Round(AbonosInteres, 0, MidpointRounding.ToEven),
                InteresMoraPendiente = 0,
                SeguroPendiente = prestamo.ValorSeguro,
                CtoAdmonPendiente=prestamo.CtoAdmon,
                FechaProximoPago = FechaProxPagoTotales,
                DiasMora = 0,
                Estado = "AD"

            };
            db.TotalesCreditos.Add(TotalesCreditos);


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
            var proximoPago = FechaProxPag.AddMonths(Periodo);
            

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
                FECHARealiz = prestamo.fechadesembolso,
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
                            DETALLE = "DESEMBOLSO Ente",
                            DEBITO = 0,
                            CREDITO = valorCosto,
                            BASE = 0,
                            CCOSTO = "07",
                            FECHAMOVIMIENTO = prestamo.fechadesembolso,
                        };
                        sumaDeCostos = sumaDeCostos + Convert.ToInt32(valorCosto);

                        listaDeMovimientos.Add(mov);

                    }
                    else if (costo.seCobraComo == 5)
                    {
                        var costoAdicional = db.Costos_Adicionales.Where(b => b.CA_Id == costo.CA_Id).Single();

                        var cuentaCosto = Convert.ToString(costoAdicional.Cuenta_Cod);
                        var valorPorcentaje = Convert.ToDecimal(costoAdicional.CA_Porcentaje.Replace(".", ","));
                        //valorPorcentaje = (valorPorcentaje / 100) * prestamo.Capital;
                        valorPorcentaje = GetCostoAdicionalAnticipado(Capital);
                        var papeleriaAnt = prestamo.costoAdicionalAnticipado;
                        var mov = new Movimiento()
                        {
                            TIPO = cuentaCreditoCod.TipoComprobante,
                            NUMERO = consecutivoComprobante.CONSECUTIVO,
                            CUENTA = papeleriaNC.Cuenta_Cod,
                            TERCERO = prestamo.NIT,
                            DETALLE = "DESEMBOLSO PAGO ANTICIPADO PAPELERIA",
                            DEBITO = 0,
                            CREDITO = Convert.ToDecimal(papeleriaAnt),
                            BASE = 0,
                            CCOSTO = "07",
                            FECHAMOVIMIENTO = prestamo.fechadesembolso,
                        };
                        listaDeMovimientos.Add(mov);
                        sumaDeCostos = sumaDeCostos + Convert.ToInt32(valorPorcentaje);

                        ////calculamos el iva si está configurado al costo de estudio de crédito
                        //var cuentaIVA = db.Cuentas.Where(x => x.Funcion == "F10" && x.Destino_Id == prestamo.Destino_Id).FirstOrDefault();
                        //if (cuentaIVA != null)
                        //{
                        //    decimal valorIVA = valorPorcentaje * Convert.ToDecimal(0.19);
                        //    var movIVA = new Movimiento()
                        //    {
                        //        TIPO = cuentaCreditoCod.TipoComprobante,
                        //        NUMERO = consecutivoComprobante.CONSECUTIVO,
                        //        CUENTA = cuentaIVA.Cuenta_Cod,
                        //        TERCERO = prestamo.NIT,
                        //        DETALLE = "DESEMBOLSO",
                        //        DEBITO = 0,
                        //        CREDITO = valorIVA,
                        //        BASE = 0,
                        //        CCOSTO = "07",
                        //        FECHAMOVIMIENTO = prestamo.fechadesembolso,
                        //    };
                        //    listaDeMovimientos.Add(movIVA);
                        //    sumaDeCostos += Convert.ToInt32(valorIVA);
                        //}

                        
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
                DETALLE = "DESEMBOLSO CREDITO",
                DEBITO = 0,
                CREDITO = Convert.ToDecimal(datosDesembolso.cajain),
                BASE = 0,
                CCOSTO = "07",
                FECHAMOVIMIENTO = prestamo.fechadesembolso,
            };

            listaDeMovimientos.Add(mov1);
            var mov2 = new Movimiento()
            {
                TIPO = cuentaCreditoCod.TipoComprobante,
                NUMERO = consecutivoComprobante.CONSECUTIVO,
                CUENTA = cuentaDebitoCod.Cuenta_Cod,
                TERCERO = prestamo.NIT,
                DETALLE = "DESEMBOLSO DEBITO",
                DEBITO = prestamo.Capital,
                CREDITO = 0,
                BASE = 0,
                CCOSTO = "07",
                FECHAMOVIMIENTO = prestamo.fechadesembolso,
            };
            listaDeMovimientos.Add(mov2);



            if (datosDesembolso.Interes != "0")
            {
                var InterInterno = Convert.ToDecimal(datosDesembolso.Interes);
                prestamo.costoAdicionalAnticipado = prestamo.costoAdicionalAnticipado + InterInterno;
                var mov3 = new Movimiento()
                {
                    TIPO = cuentaCreditoCod.TipoComprobante,
                    NUMERO = consecutivoComprobante.CONSECUTIVO,
                    CUENTA = interesNC.Cuenta_Cod,
                    TERCERO = prestamo.NIT,
                    DETALLE = "DESEMBOLSO PAGO ANTICIPADO INTERESES",
                    DEBITO = 0,
                    CREDITO = Convert.ToDecimal(datosDesembolso.Interes),
                    BASE = 0,
                    CCOSTO = "07",
                    FECHAMOVIMIENTO = prestamo.fechadesembolso,
                };
                listaDeMovimientos.Add(mov3);
            }
            if (datosDesembolso.BANCO != "0")
            {
                
                var mov4 = new Movimiento()
                {
                    TIPO = cuentaCreditoCod.TipoComprobante,
                    NUMERO = consecutivoComprobante.CONSECUTIVO,
                    CUENTA = bancosNC.Cuenta_Cod,
                    TERCERO = prestamo.NIT,
                    DETALLE = "DESEMBOLSO BANCOS",
                    DEBITO = 0,
                    CREDITO = Convert.ToDecimal(datosDesembolso.BANCO),
                    BASE = 0,
                    CCOSTO = "07",
                    FECHAMOVIMIENTO = prestamo.fechadesembolso,
                };
                listaDeMovimientos.Add(mov4);
            }

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
                //prestamo.fechadesembolso = DateTime.Now;
                db.Entry(prestamo).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            return RedirectToAction("Index");
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

        private Int64 GetCostoAdicionalAnticipado(decimal capital)
        {
            Int64 valor = 0;
            if (capital >= 0 && capital <= 10000000) { valor = Convert.ToInt64(capital*(Convert.ToDecimal(1.5)/100)); }
            else if (capital > 10000000 && capital <= 15000000) { valor = Convert.ToInt64(capital * (Convert.ToDecimal(1.2) / 100)); }
            else { valor = Convert.ToInt64(capital * (Convert.ToDecimal(1.0) / 100)); }
            return valor;
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
