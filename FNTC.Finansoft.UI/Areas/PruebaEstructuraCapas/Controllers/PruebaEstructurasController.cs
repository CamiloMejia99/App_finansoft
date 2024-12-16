using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FNTC.Finansoft.Accounting.BLL;
using FNTC.Finansoft.Accounting.BLL.PruebaEstructuraCapas;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.Accounting.DTO.PruebaEstructuraCapas;

namespace FNTC.Finansoft.UI.Areas.PruebaEstructuraCapas.Controllers
{
    public class PruebaEstructurasController : Controller
    {
        private AccountingContext db = new AccountingContext();

        public void causarTodos()
        {
            var fechaActual = DateTime.Now;
            var creditos = db.Creditos.ToList();
            foreach (var item in creditos)
            {
                var historiales = (from d in db.HistorialCreditos where d.pagare == item.Pagare && d.estado != "pazYsalvo" select d).ToList();
                foreach (var item2 in historiales)
                {
                    if (item2.estado == "enMora")
                    {
                        decimal valorInteresMoraUnDiaGlobal = 0;

                        decimal valorInteresMora = 0;

                        var prestamo = (from d in db.Prestamos where d.Pagare == item2.pagare select d).First();
                        var destino = (from d in db.Destinos where d.Destino_Id == prestamo.Destino_Id select d).First();

                        var interesMoraDeUnDia = (destino.Destino_Tasa_Mora / 30) / 100;
                        valorInteresMora = (item2.interesCorrienteMora + item2.capitalEnMora) * interesMoraDeUnDia;
                        valorInteresMoraUnDiaGlobal = valorInteresMoraUnDiaGlobal + valorInteresMora;

                        item2.interesMora = item2.interesMora + valorInteresMora;
                        item2.diasCausados = item2.diasCausados + 1;
                        item2.diasEnMora = item2.diasEnMora + 1;

                        db.Entry(item2).State = System.Data.Entity.EntityState.Modified;


                        //COMPROBANTE Y MOVIMIENTOS INTERES DE MORA
                        if (valorInteresMoraUnDiaGlobal > 0)
                        {
                            var interesCausadoAnteriorCount = (from d in db.interescausadoprestamos where d.pagare == item2.pagare orderby d.id descending select d).Count();
                            decimal interesCorrienteCausadoHastaAhora = 0;
                            decimal interesMoraCausadoHastaAhora = 0;

                            if (interesCausadoAnteriorCount > 0)
                            {
                                var interesCausadoAnterior = (from d in db.interescausadoprestamos where d.pagare == item2.pagare orderby d.id descending select d).First();
                                interesCorrienteCausadoHastaAhora = interesCausadoAnterior.intcorriente;
                                interesMoraCausadoHastaAhora = interesCausadoAnterior.intmora;
                            }


                            string ComprobanteUsar = "";
                            var traerConsecutivo = new ComprobanteBO();
                            ComprobanteUsar = traerConsecutivo.proximoConsecutivo("CI4");

                            var comprobanteIntMora = new Comprobante()
                            {
                                TIPO = "CI4",
                                NUMERO = ComprobanteUsar,
                                ANO = Convert.ToString(DateTime.Now.Year),
                                MES = Convert.ToString(DateTime.Now.Month),
                                DIA = Convert.ToString(DateTime.Now.Day),
                                CCOSTO = "00",
                                DETALLE = "CAUSACION CREDITOS INTERES MORA",
                                TERCERO = item2.NIT,
                                CTAFPAGO = "165505002",//CAMBIAR CUENTA PARA INTERES DE MORA
                                VRTOTAL = valorInteresMoraUnDiaGlobal,
                                SUMDBCR = 0,
                                FECHARealiz = fechaActual,
                                ANULADO = false
                            };
                            db.Comprobantes.Add(comprobanteIntMora);

                            //CONSTRUIR LA LISTA DE MOVIMIENTOS
                            List<Movimiento> listaDeMovimientos = new List<Movimiento>();

                            var mov1 = new Movimiento()
                            {
                                TIPO = "CI4",
                                NUMERO = ComprobanteUsar,
                                CUENTA = "165505002",
                                TERCERO = item2.NIT,
                                DETALLE = "CAUSACION CREDITOS INTERES MORA",
                                DEBITO = valorInteresMoraUnDiaGlobal,
                                CREDITO = 0,
                                BASE = 0,
                                CCOSTO = "00",
                                FECHAMOVIMIENTO = fechaActual,
                            };
                            listaDeMovimientos.Add(mov1);

                            var mov2 = new Movimiento()
                            {
                                TIPO = "CI4",
                                NUMERO = ComprobanteUsar,
                                CUENTA = "418510003",
                                TERCERO = item2.NIT,
                                DETALLE = "CAUSACION CREDITOS INTERES MORA",
                                DEBITO = 0,
                                CREDITO = valorInteresMoraUnDiaGlobal,
                                BASE = 0,
                                CCOSTO = "00",
                                FECHAMOVIMIENTO = fechaActual,
                            };
                            listaDeMovimientos.Add(mov2);

                            var causador = new interescausadoprestamos()
                            {
                                pagare = item2.pagare,
                                intcorriente = interesCorrienteCausadoHastaAhora,
                                intmora = interesMoraCausadoHastaAhora + valorInteresMoraUnDiaGlobal,
                                fechasistema = fechaActual,
                                agenciaId = 1,
                                usuarioCauso = "AUTOMATICO",
                                Tasainteres = item2.TazaInteresCorriente,
                                numeroCuota = item2.numeroCuota,
                                comprabante = "CI4",
                                consecutivo = Convert.ToInt32(ComprobanteUsar),
                                codcuentaingresosctes = "165505001",
                                codcuentaingresosmora = "165505002"
                            };
                            db.interescausadoprestamos.Add(causador);

                            var result = false;

                            var comprobanteConst = new ComprobanteBO();
                            result = comprobanteConst.AsentarCausacion(listaDeMovimientos, Convert.ToInt32(ComprobanteUsar), "CI4");

                            if (result)
                            {
                                db.SaveChanges();
                            }
                        }
                    }

                    if (item2.estado == "diasTerminados")
                    {
                        if (fechaActual > item2.fechaProximoPago)
                        {
                            var idHistorial = item2.id;
                            var historioal = (from d in db.HistorialCreditos where d.id == idHistorial select d).First();
                            var amortizacion = (from d in db.Amortizaciones where d.pagare == historioal.pagare select d).First();

                            decimal valorPagado = Convert.ToDecimal(amortizacion.valorCuota);
                            decimal valorSeguro = Convert.ToDecimal(amortizacion.valorCosto);

                            var historialCredit = (from d in db.HistorialCreditos where d.id == idHistorial select d).First();

                            var interesCorriente = historialCredit.interesCorriente;
                            var abonoCapital = valorPagado - interesCorriente - valorSeguro;

                            var nuevoSaldoCapital = historialCredit.saldoCapital - abonoCapital;

                            var fechaProxPago = historialCredit.fechaProximoPago.AddMonths(1);

                            historialCredit.fecha = fechaActual;
                            historialCredit.interesCorrienteMora = historialCredit.interesCorriente;
                            historialCredit.capitalEnMora = abonoCapital;
                            historialCredit.estado = "enMora";

                            db.SaveChanges();

                            var historioal2 = (from d in db.HistorialCreditos where d.id == idHistorial select d).First();

                            decimal valorInteresMoraUnDiaGlobal = 0;

                            decimal valorInteresMora = 0;

                            var prestamo = (from d in db.Prestamos where d.Pagare == historioal2.pagare select d).First();
                            var destino = (from d in db.Destinos where d.Destino_Id == prestamo.Destino_Id select d).First();

                            var interesMoraDeUnDia = (destino.Destino_Tasa_Mora / 30) / 100;
                            valorInteresMora = (historioal2.interesCorrienteMora + historioal2.capitalEnMora) * interesMoraDeUnDia;
                            valorInteresMoraUnDiaGlobal = valorInteresMoraUnDiaGlobal + valorInteresMora;

                            historioal2.interesMora = historioal2.interesMora + valorInteresMora;
                            historioal2.diasCausados = historioal2.diasCausados + 1;
                            historioal2.diasEnMora = historioal2.diasEnMora + 1;

                            db.Entry(historioal2).State = System.Data.Entity.EntityState.Modified;


                            //COMPROBANTE Y MOVIMIENTOS INTERES DE MORA
                            if (valorInteresMoraUnDiaGlobal > 0)
                            {
                                var interesCausadoAnteriorCount = (from d in db.interescausadoprestamos where d.pagare == historioal2.pagare orderby d.id descending select d).Count();
                                decimal interesCorrienteCausadoHastaAhora = 0;
                                decimal interesMoraCausadoHastaAhora = 0;

                                if (interesCausadoAnteriorCount > 0)
                                {
                                    var interesCausadoAnterior = (from d in db.interescausadoprestamos where d.pagare == historioal2.pagare orderby d.id descending select d).First();
                                    interesCorrienteCausadoHastaAhora = interesCausadoAnterior.intcorriente;
                                    interesMoraCausadoHastaAhora = interesCausadoAnterior.intmora;
                                }


                                string ComprobanteUsar = "";
                                var traerConsecutivo = new ComprobanteBO();
                                ComprobanteUsar = traerConsecutivo.proximoConsecutivo("CI4");

                                var comprobanteIntMora = new Comprobante()
                                {
                                    TIPO = "CI4",
                                    NUMERO = ComprobanteUsar,
                                    ANO = Convert.ToString(DateTime.Now.Year),
                                    MES = Convert.ToString(DateTime.Now.Month),
                                    DIA = Convert.ToString(DateTime.Now.Day),
                                    CCOSTO = "00",
                                    DETALLE = "CAUSACION CREDITOS INTERES MORA",
                                    TERCERO = historioal2.NIT,
                                    CTAFPAGO = "165505002",//CAMBIAR CUENTA PARA INTERES DE MORA
                                    VRTOTAL = valorInteresMoraUnDiaGlobal,
                                    SUMDBCR = 0,
                                    FECHARealiz = fechaActual,
                                    ANULADO = false
                                };
                                db.Comprobantes.Add(comprobanteIntMora);

                                //CONSTRUIR LA LISTA DE MOVIMIENTOS
                                List<Movimiento> listaDeMovimientos = new List<Movimiento>();

                                var mov1 = new Movimiento()
                                {
                                    TIPO = "CI4",
                                    NUMERO = ComprobanteUsar,
                                    CUENTA = "165505002",
                                    TERCERO = historioal2.NIT,
                                    DETALLE = "CAUSACION CREDITOS INTERES MORA",
                                    DEBITO = valorInteresMoraUnDiaGlobal,
                                    CREDITO = 0,
                                    BASE = 0,
                                    CCOSTO = "00",
                                    FECHAMOVIMIENTO = fechaActual,
                                };
                                listaDeMovimientos.Add(mov1);

                                var mov2 = new Movimiento()
                                {
                                    TIPO = "CI4",
                                    NUMERO = ComprobanteUsar,
                                    CUENTA = "418510003",
                                    TERCERO = historioal2.NIT,
                                    DETALLE = "CAUSACION CREDITOS INTERES MORA",
                                    DEBITO = 0,
                                    CREDITO = valorInteresMoraUnDiaGlobal,
                                    BASE = 0,
                                    CCOSTO = "00",
                                    FECHAMOVIMIENTO = fechaActual,
                                };
                                listaDeMovimientos.Add(mov2);

                                var causador = new interescausadoprestamos()
                                {
                                    pagare = historioal2.pagare,
                                    intcorriente = interesCorrienteCausadoHastaAhora,
                                    intmora = interesMoraCausadoHastaAhora + valorInteresMoraUnDiaGlobal,
                                    fechasistema = fechaActual,
                                    agenciaId = 1,
                                    usuarioCauso = "AUTOMATICO",
                                    Tasainteres = historioal2.TazaInteresCorriente,
                                    numeroCuota = historioal2.numeroCuota,
                                    comprabante = "CI4",
                                    consecutivo = Convert.ToInt32(ComprobanteUsar),
                                    codcuentaingresosctes = "165505001",
                                    codcuentaingresosmora = "165505002"
                                };
                                db.interescausadoprestamos.Add(causador);

                                var result = false;

                                var comprobanteConst = new ComprobanteBO();
                                result = comprobanteConst.AsentarCausacion(listaDeMovimientos, Convert.ToInt32(ComprobanteUsar), "CI4");

                                if (result)
                                {
                                    db.SaveChanges();
                                }
                            }
                        }
                    }
                  
                    if (item2.estado == "normal")
                    {
                        if(item2.saldoCapital > 0 && item2.numeroCuota != 0)
                        {
                            if (item2.diasCausados < 30)
                            {
                                decimal valorInteresCorrienteUnDiaGlobal = 0;
                                decimal valorInteresCorrienteUnDia = 0;

                                var prestamo = (from d in db.Prestamos where d.Pagare == item2.pagare select d).First();
                                var destino = (from d in db.Destinos where d.Destino_Id == prestamo.Destino_Id select d).First();

                                var interesCorrienteDeUnDia = (prestamo.Interes / 30) / 100;

                                valorInteresCorrienteUnDia = interesCorrienteDeUnDia * item2.saldoCapital;
                                valorInteresCorrienteUnDiaGlobal = valorInteresCorrienteUnDiaGlobal + valorInteresCorrienteUnDia;

                                item2.diasCausados = item2.diasCausados + 1;
                                item2.interesCorriente = item2.interesCorriente + valorInteresCorrienteUnDia;
                                db.Entry(item2).State = System.Data.Entity.EntityState.Modified;

                                //COMPROBANTE Y MOVIMIENTOS INTERES CORRIENTE
                                if (valorInteresCorrienteUnDiaGlobal != 0)
                                {
                                    var interesCausadoAnteriorCount = (from d in db.interescausadoprestamos where d.pagare == item2.pagare orderby d.id descending select d).Count();
                                    decimal interesCorrienteCausadoHastaAhora = 0;
                                    decimal interesMoraCausadoHastaAhora = 0;

                                    if (interesCausadoAnteriorCount > 0)
                                    {
                                        var interesCausadoAnterior = (from d in db.interescausadoprestamos where d.pagare == item2.pagare orderby d.id descending select d).First();
                                        interesCorrienteCausadoHastaAhora = interesCausadoAnterior.intcorriente;
                                        interesMoraCausadoHastaAhora = interesCausadoAnterior.intmora;
                                    }

                                    string ComprobanteUsar = "";
                                    var traerConsecutivo = new ComprobanteBO();
                                    ComprobanteUsar = traerConsecutivo.proximoConsecutivo("CI3");

                                    var comprobanteIntCorriente = new Comprobante()
                                    {
                                        TIPO = "CI3",
                                        NUMERO = ComprobanteUsar,
                                        ANO = Convert.ToString(DateTime.Now.Year),
                                        MES = Convert.ToString(DateTime.Now.Month),
                                        DIA = Convert.ToString(DateTime.Now.Day),
                                        CCOSTO = "00",
                                        DETALLE = "CAUSACION CREDITOS INTERES CORRIENTE",
                                        TERCERO = item2.NIT,
                                        CTAFPAGO = "165505001",
                                        VRTOTAL = valorInteresCorrienteUnDiaGlobal,
                                        SUMDBCR = 0,
                                        FECHARealiz = fechaActual,
                                        ANULADO = false
                                    };
                                    db.Comprobantes.Add(comprobanteIntCorriente);

                                    //CONSTRUIR LA LISTA DE MOVIMIENTOS
                                    List<Movimiento> listaDeMovimientos = new List<Movimiento>();

                                    var mov1 = new Movimiento()
                                    {
                                        TIPO = "CI3",
                                        NUMERO = ComprobanteUsar,
                                        CUENTA = "165505001",
                                        TERCERO = item2.NIT,
                                        DETALLE = "CAUSACION CREDITOS INTERES CORRIENTE",
                                        DEBITO = valorInteresCorrienteUnDiaGlobal,
                                        CREDITO = 0,
                                        BASE = 0,
                                        CCOSTO = "00",
                                        FECHAMOVIMIENTO = fechaActual,
                                    };
                                    listaDeMovimientos.Add(mov1);

                                    var mov2 = new Movimiento()
                                    {
                                        TIPO = "CI3",
                                        NUMERO = ComprobanteUsar,
                                        CUENTA = "418516001",
                                        TERCERO = item2.NIT,
                                        DETALLE = "CAUSACION CREDITOS INTERES CORRIENTE",
                                        DEBITO = 0,
                                        CREDITO = valorInteresCorrienteUnDiaGlobal,
                                        BASE = 0,
                                        CCOSTO = "00",
                                        FECHAMOVIMIENTO = fechaActual,
                                    };
                                    listaDeMovimientos.Add(mov2);

                                    var result = false;

                                    var causador = new interescausadoprestamos()
                                    {
                                        pagare = item2.pagare,
                                        intcorriente = interesCorrienteCausadoHastaAhora + valorInteresCorrienteUnDiaGlobal,
                                        intmora = interesMoraCausadoHastaAhora,
                                        fechasistema = fechaActual,
                                        agenciaId = 1,
                                        usuarioCauso = "AUTOMATICO",
                                        Tasainteres = item2.TazaInteresCorriente,
                                        numeroCuota = item2.numeroCuota,
                                        comprabante = "CI3",
                                        consecutivo = Convert.ToInt32(ComprobanteUsar),
                                        codcuentaingresosctes = "165505001",
                                        codcuentaingresosmora = "165505002"
                                    };
                                    db.interescausadoprestamos.Add(causador);

                                    var comprobanteConst = new ComprobanteBO();
                                    result = comprobanteConst.AsentarCausacion(listaDeMovimientos, Convert.ToInt32(ComprobanteUsar), "CI3");

                                    if (result)
                                    {
                                        db.SaveChanges();
                                    }
                                }
                            }
                            else if (fechaActual > item2.fechaProximoPago)
                            {
                                int idNuevoHistorial = 0;
                                bool bandera = false;


                                var pagare = item2.pagare;
                                var amortizacion = (from d in db.Amortizaciones where d.pagare == pagare select d).First();

                                decimal valorPagado = Convert.ToDecimal(amortizacion.valorCuota);
                                decimal valorSeguro = Convert.ToDecimal(amortizacion.valorCosto);

                                var historialCredit = (from d in db.HistorialCreditos where d.pagare == pagare orderby d.id descending select d).First();

                                var interesCorriente = historialCredit.interesCorriente;
                                var abonoCapital = valorPagado - interesCorriente - valorSeguro;

                                var nuevoSaldoCapital = historialCredit.saldoCapital - abonoCapital;

                                var fechaProxPago = historialCredit.fechaProximoPago.AddMonths(1);

                                historialCredit.fecha = fechaActual;
                                historialCredit.interesCorrienteMora = historialCredit.interesCorriente;
                                historialCredit.capitalEnMora = abonoCapital;
                                historialCredit.estado = "enMora";

                                if (nuevoSaldoCapital <= 0)
                                {
                                    var nuevoHistorial = new HistorialCreditos()
                                    {
                                        fecha = fechaActual,
                                        idFactura = 0,
                                        NIT = historialCredit.NIT,
                                        pagare = historialCredit.pagare,
                                        abonoCapital = 0,
                                        abonoInteresMora = 0,
                                        AbonoInteresCorriente = 0,
                                        valorCosto = 0,
                                        saldoCapital = 0,
                                        proximaCuota = 0,
                                        capitalEnMora = 0,
                                        TazaInteresMora = historialCredit.TazaInteresMora,
                                        TazaInteresCorriente = historialCredit.TazaInteresCorriente,
                                        diasCausados = 0,
                                        diasEnMora = 0,
                                        numeroCuota = 0,
                                        fechaProximoPago = fechaProxPago,
                                        interesCorrienteMora = 0,
                                        interesCorriente = 0,
                                        estado = "normal",
                                        interesMora = 0
                                    };
                                    db.HistorialCreditos.Add(nuevoHistorial);
                                }
                                else
                                {
                                    var nuevoHistorial = new HistorialCreditos()
                                    {
                                        fecha = fechaActual,
                                        idFactura = 0,
                                        NIT = historialCredit.NIT,
                                        pagare = historialCredit.pagare,
                                        abonoCapital = 0,
                                        abonoInteresMora = 0,
                                        AbonoInteresCorriente = 0,
                                        valorCosto = 0,
                                        saldoCapital = nuevoSaldoCapital,
                                        proximaCuota = historialCredit.proximaCuota,
                                        capitalEnMora = 0,
                                        TazaInteresMora = historialCredit.TazaInteresMora,
                                        TazaInteresCorriente = historialCredit.TazaInteresCorriente,
                                        diasCausados = 0,
                                        diasEnMora = 0,
                                        numeroCuota = historialCredit.numeroCuota + 1,
                                        fechaProximoPago = fechaProxPago,
                                        interesCorrienteMora = 0,
                                        interesCorriente = 0,
                                        estado = "normal",
                                        interesMora = 0
                                    };
                                    db.HistorialCreditos.Add(nuevoHistorial);

                                    db.SaveChanges();

                                    idNuevoHistorial = nuevoHistorial.id;
                                    bandera = true;
                                }

                                

                                var historioal2 = (from d in db.HistorialCreditos where d.id == item2.id select d).First();

                                decimal valorInteresMoraUnDiaGlobal = 0;

                                decimal valorInteresMora = 0;

                                var prestamo = (from d in db.Prestamos where d.Pagare == historioal2.pagare select d).First();
                                var destino = (from d in db.Destinos where d.Destino_Id == prestamo.Destino_Id select d).First();

                                var interesMoraDeUnDia = (destino.Destino_Tasa_Mora / 30) / 100;
                                valorInteresMora = (historioal2.interesCorrienteMora + historioal2.capitalEnMora) * interesMoraDeUnDia;
                                valorInteresMoraUnDiaGlobal = valorInteresMoraUnDiaGlobal + valorInteresMora;

                                historioal2.interesMora = historioal2.interesMora + valorInteresMora;
                                historioal2.diasCausados = historioal2.diasCausados + 1;
                                historioal2.diasEnMora = historioal2.diasEnMora + 1;

                                db.Entry(historioal2).State = System.Data.Entity.EntityState.Modified;


                                //COMPROBANTE Y MOVIMIENTOS INTERES DE MORA
                                if (valorInteresMoraUnDiaGlobal > 0)
                                {
                                    var interesCausadoAnteriorCount = (from d in db.interescausadoprestamos where d.pagare == historioal2.pagare orderby d.id descending select d).Count();
                                    decimal interesCorrienteCausadoHastaAhora = 0;
                                    decimal interesMoraCausadoHastaAhora = 0;

                                    if (interesCausadoAnteriorCount > 0)
                                    {
                                        var interesCausadoAnterior = (from d in db.interescausadoprestamos where d.pagare == historioal2.pagare orderby d.id descending select d).First();
                                        interesCorrienteCausadoHastaAhora = interesCausadoAnterior.intcorriente;
                                        interesMoraCausadoHastaAhora = interesCausadoAnterior.intmora;
                                    }


                                    string ComprobanteUsar = "";
                                    var traerConsecutivo = new ComprobanteBO();
                                    ComprobanteUsar = traerConsecutivo.proximoConsecutivo("CI4");

                                    var comprobanteIntMora = new Comprobante()
                                    {
                                        TIPO = "CI4",
                                        NUMERO = ComprobanteUsar,
                                        ANO = Convert.ToString(DateTime.Now.Year),
                                        MES = Convert.ToString(DateTime.Now.Month),
                                        DIA = Convert.ToString(DateTime.Now.Day),
                                        CCOSTO = "00",
                                        DETALLE = "CAUSACION CREDITOS INTERES MORA",
                                        TERCERO = historioal2.NIT,
                                        CTAFPAGO = "165505002",//CAMBIAR CUENTA PARA INTERES DE MORA
                                        VRTOTAL = valorInteresMoraUnDiaGlobal,
                                        SUMDBCR = 0,
                                        FECHARealiz = fechaActual,
                                        ANULADO = false
                                    };
                                    db.Comprobantes.Add(comprobanteIntMora);

                                    //CONSTRUIR LA LISTA DE MOVIMIENTOS
                                    List<Movimiento> listaDeMovimientos = new List<Movimiento>();

                                    var mov1 = new Movimiento()
                                    {
                                        TIPO = "CI4",
                                        NUMERO = ComprobanteUsar,
                                        CUENTA = "165505002",
                                        TERCERO = historioal2.NIT,
                                        DETALLE = "CAUSACION CREDITOS INTERES MORA",
                                        DEBITO = valorInteresMoraUnDiaGlobal,
                                        CREDITO = 0,
                                        BASE = 0,
                                        CCOSTO = "00",
                                        FECHAMOVIMIENTO = fechaActual,
                                    };
                                    listaDeMovimientos.Add(mov1);

                                    var mov2 = new Movimiento()
                                    {
                                        TIPO = "CI4",
                                        NUMERO = ComprobanteUsar,
                                        CUENTA = "418510003",
                                        TERCERO = historioal2.NIT,
                                        DETALLE = "CAUSACION CREDITOS INTERES MORA",
                                        DEBITO = 0,
                                        CREDITO = valorInteresMoraUnDiaGlobal,
                                        BASE = 0,
                                        CCOSTO = "00",
                                        FECHAMOVIMIENTO = fechaActual,
                                    };
                                    listaDeMovimientos.Add(mov2);

                                    var causador = new interescausadoprestamos()
                                    {
                                        pagare = historioal2.pagare,
                                        intcorriente = interesCorrienteCausadoHastaAhora,
                                        intmora = interesMoraCausadoHastaAhora + valorInteresMoraUnDiaGlobal,
                                        fechasistema = fechaActual,
                                        agenciaId = 1,
                                        usuarioCauso = "AUTOMATICO",
                                        Tasainteres = historioal2.TazaInteresCorriente,
                                        numeroCuota = historioal2.numeroCuota,
                                        comprabante = "CI4",
                                        consecutivo = Convert.ToInt32(ComprobanteUsar),
                                        codcuentaingresosctes = "165505001",
                                        codcuentaingresosmora = "165505002"
                                    };
                                    db.interescausadoprestamos.Add(causador);

                                    var result = false;

                                    var comprobanteConst = new ComprobanteBO();
                                    result = comprobanteConst.AsentarCausacion(listaDeMovimientos, Convert.ToInt32(ComprobanteUsar), "CI4");

                                    if (result)
                                    {
                                        db.SaveChanges();
                                    }
                                }

                                if(bandera)
                                {
                                    var historioalNormal = (from d in db.HistorialCreditos where d.id == idNuevoHistorial select d).First();

                                    decimal valorInteresCorrienteUnDiaGlobal = 0;
                                    decimal valorInteresCorrienteUnDia = 0;

                                    var prestamoNormal = (from d in db.Prestamos where d.Pagare == historioalNormal.pagare select d).First();
                                    var interesCorrienteDeUnDia = (prestamoNormal.Interes / 30) / 100;

                                    valorInteresCorrienteUnDia = interesCorrienteDeUnDia * historioalNormal.saldoCapital;
                                    valorInteresCorrienteUnDiaGlobal = valorInteresCorrienteUnDiaGlobal + valorInteresCorrienteUnDia;

                                    historioalNormal.diasCausados = historioalNormal.diasCausados + 1;
                                    historioalNormal.interesCorriente = historioalNormal.interesCorriente + valorInteresCorrienteUnDia;
                                    db.Entry(historioalNormal).State = System.Data.Entity.EntityState.Modified;

                                    //COMPROBANTE Y MOVIMIENTOS INTERES CORRIENTE
                                    if (valorInteresCorrienteUnDiaGlobal != 0)
                                    {
                                        var interesCausadoAnteriorCount = (from d in db.interescausadoprestamos where d.pagare == historioalNormal.pagare orderby d.id descending select d).Count();
                                        decimal interesCorrienteCausadoHastaAhora = 0;
                                        decimal interesMoraCausadoHastaAhora = 0;

                                        if (interesCausadoAnteriorCount > 0)
                                        {
                                            var interesCausadoAnterior = (from d in db.interescausadoprestamos where d.pagare == historioalNormal.pagare orderby d.id descending select d).First();
                                            interesCorrienteCausadoHastaAhora = interesCausadoAnterior.intcorriente;
                                            interesMoraCausadoHastaAhora = interesCausadoAnterior.intmora;
                                        }

                                        string ComprobanteUsar = "";
                                        var traerConsecutivo = new ComprobanteBO();
                                        ComprobanteUsar = traerConsecutivo.proximoConsecutivo("CI3");

                                        var comprobanteIntCorriente = new Comprobante()
                                        {
                                            TIPO = "CI3",
                                            NUMERO = ComprobanteUsar,
                                            ANO = Convert.ToString(DateTime.Now.Year),
                                            MES = Convert.ToString(DateTime.Now.Month),
                                            DIA = Convert.ToString(DateTime.Now.Day),
                                            CCOSTO = "00",
                                            DETALLE = "CAUSACION CREDITOS INTERES CORRIENTE",
                                            TERCERO = historioalNormal.NIT,
                                            CTAFPAGO = "165505001",
                                            VRTOTAL = valorInteresCorrienteUnDiaGlobal,
                                            SUMDBCR = 0,
                                            FECHARealiz = fechaActual,
                                            ANULADO = false
                                        };
                                        db.Comprobantes.Add(comprobanteIntCorriente);

                                        //CONSTRUIR LA LISTA DE MOVIMIENTOS
                                        List<Movimiento> listaDeMovimientos = new List<Movimiento>();

                                        var mov1 = new Movimiento()
                                        {
                                            TIPO = "CI3",
                                            NUMERO = ComprobanteUsar,
                                            CUENTA = "165505001",
                                            TERCERO = historioalNormal.NIT,
                                            DETALLE = "CAUSACION CREDITOS INTERES CORRIENTE",
                                            DEBITO = valorInteresCorrienteUnDiaGlobal,
                                            CREDITO = 0,
                                            BASE = 0,
                                            CCOSTO = "00",
                                            FECHAMOVIMIENTO = fechaActual,
                                        };
                                        listaDeMovimientos.Add(mov1);

                                        var mov2 = new Movimiento()
                                        {
                                            TIPO = "CI3",
                                            NUMERO = ComprobanteUsar,
                                            CUENTA = "418516001",
                                            TERCERO = historioalNormal.NIT,
                                            DETALLE = "CAUSACION CREDITOS INTERES CORRIENTE",
                                            DEBITO = 0,
                                            CREDITO = valorInteresCorrienteUnDiaGlobal,
                                            BASE = 0,
                                            CCOSTO = "00",
                                            FECHAMOVIMIENTO = fechaActual,
                                        };
                                        listaDeMovimientos.Add(mov2);

                                        var result = false;

                                        var causador = new interescausadoprestamos()
                                        {
                                            pagare = historioalNormal.pagare,
                                            intcorriente = interesCorrienteCausadoHastaAhora + valorInteresCorrienteUnDiaGlobal,
                                            intmora = interesMoraCausadoHastaAhora,
                                            fechasistema = fechaActual,
                                            agenciaId = 1,
                                            usuarioCauso = "AUTOMATICO",
                                            Tasainteres = historioalNormal.TazaInteresCorriente,
                                            numeroCuota = historioalNormal.numeroCuota,
                                            comprabante = "CI3",
                                            consecutivo = Convert.ToInt32(ComprobanteUsar),
                                            codcuentaingresosctes = "165505001",
                                            codcuentaingresosmora = "165505002"
                                        };
                                        db.interescausadoprestamos.Add(causador);

                                        var comprobanteConst = new ComprobanteBO();
                                        result = comprobanteConst.AsentarCausacion(listaDeMovimientos, Convert.ToInt32(ComprobanteUsar), "CI3");

                                        if (result)
                                        {
                                            db.SaveChanges();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                var pagare = item2.pagare;

                                var amortizacion = (from d in db.Amortizaciones where d.pagare == pagare select d).First();

                                decimal valorPagado = Convert.ToDecimal(amortizacion.valorCuota);
                                decimal valorSeguro = Convert.ToDecimal(amortizacion.valorCosto);

                                var historialCredit = (from d in db.HistorialCreditos where d.pagare == pagare orderby d.id descending select d).First();

                                var interesCorriente = historialCredit.interesCorriente;
                                var abonoCapital = valorPagado - interesCorriente - valorSeguro;

                                var nuevoSaldoCapital = historialCredit.saldoCapital - abonoCapital;

                                var fechaProxPago = historialCredit.fechaProximoPago.AddMonths(1);

                                historialCredit.fecha = fechaActual;
                                historialCredit.estado = "diasTerminados";

                                if(nuevoSaldoCapital<=0)
                                {
                                    var nuevoHistorial = new HistorialCreditos()
                                    {
                                        fecha = fechaActual,
                                        idFactura = 0,
                                        NIT = historialCredit.NIT,
                                        pagare = historialCredit.pagare,
                                        abonoCapital = 0,
                                        abonoInteresMora = 0,
                                        AbonoInteresCorriente = 0,
                                        valorCosto = 0,
                                        saldoCapital = 0,
                                        proximaCuota = 0,
                                        capitalEnMora = 0,
                                        TazaInteresMora = historialCredit.TazaInteresMora,
                                        TazaInteresCorriente = historialCredit.TazaInteresCorriente,
                                        diasCausados = 0,
                                        diasEnMora = 0,
                                        numeroCuota = 0,
                                        fechaProximoPago = fechaProxPago,
                                        interesCorrienteMora = 0,
                                        interesCorriente = 0,
                                        estado = "normal",
                                        interesMora = 0
                                    };
                                    db.HistorialCreditos.Add(nuevoHistorial);
                                }
                                else
                                {
                                    var nuevoHistorial = new HistorialCreditos()
                                    {
                                        fecha = fechaActual,
                                        idFactura = 0,
                                        NIT = historialCredit.NIT,
                                        pagare = historialCredit.pagare,
                                        abonoCapital = 0,
                                        abonoInteresMora = 0,
                                        AbonoInteresCorriente = 0,
                                        valorCosto = 0,
                                        saldoCapital = nuevoSaldoCapital,
                                        proximaCuota = historialCredit.proximaCuota,
                                        capitalEnMora = 0,
                                        TazaInteresMora = historialCredit.TazaInteresMora,
                                        TazaInteresCorriente = historialCredit.TazaInteresCorriente,
                                        diasCausados = 0,
                                        diasEnMora = 0,
                                        numeroCuota = historialCredit.numeroCuota + 1,
                                        fechaProximoPago = fechaProxPago,
                                        interesCorrienteMora = 0,
                                        interesCorriente = 0,
                                        estado = "normal",
                                        interesMora = 0
                                    };
                                    db.HistorialCreditos.Add(nuevoHistorial);

                                    db.SaveChanges();

                                    decimal valorInteresCorrienteUnDiaGlobal = 0;
                                    decimal valorInteresCorrienteUnDia = 0;

                                    var prestamo = (from d in db.Prestamos where d.Pagare == nuevoHistorial.pagare select d).First();
                                    var destino = (from d in db.Destinos where d.Destino_Id == prestamo.Destino_Id select d).First();

                                    var interesCorrienteDeUnDia = (prestamo.Interes / 30) / 100;

                                    valorInteresCorrienteUnDia = interesCorrienteDeUnDia * nuevoHistorial.saldoCapital;
                                    valorInteresCorrienteUnDiaGlobal = valorInteresCorrienteUnDiaGlobal + valorInteresCorrienteUnDia;

                                    nuevoHistorial.diasCausados = nuevoHistorial.diasCausados + 1;
                                    nuevoHistorial.interesCorriente = nuevoHistorial.interesCorriente + valorInteresCorrienteUnDia;
                                    db.Entry(nuevoHistorial).State = System.Data.Entity.EntityState.Modified;

                                    //COMPROBANTE Y MOVIMIENTOS INTERES CORRIENTE
                                    if (valorInteresCorrienteUnDiaGlobal != 0)
                                    {
                                        var interesCausadoAnteriorCount = (from d in db.interescausadoprestamos where d.pagare == nuevoHistorial.pagare orderby d.id descending select d).Count();
                                        decimal interesCorrienteCausadoHastaAhora = 0;
                                        decimal interesMoraCausadoHastaAhora = 0;

                                        if (interesCausadoAnteriorCount > 0)
                                        {
                                            var interesCausadoAnterior = (from d in db.interescausadoprestamos where d.pagare == nuevoHistorial.pagare orderby d.id descending select d).First();
                                            interesCorrienteCausadoHastaAhora = interesCausadoAnterior.intcorriente;
                                            interesMoraCausadoHastaAhora = interesCausadoAnterior.intmora;
                                        }

                                        string ComprobanteUsar = "";
                                        var traerConsecutivo = new ComprobanteBO();
                                        ComprobanteUsar = traerConsecutivo.proximoConsecutivo("CI3");

                                        var comprobanteIntCorriente = new Comprobante()
                                        {
                                            TIPO = "CI3",
                                            NUMERO = ComprobanteUsar,
                                            ANO = Convert.ToString(DateTime.Now.Year),
                                            MES = Convert.ToString(DateTime.Now.Month),
                                            DIA = Convert.ToString(DateTime.Now.Day),
                                            CCOSTO = "00",
                                            DETALLE = "CAUSACION CREDITOS INTERES CORRIENTE",
                                            TERCERO = nuevoHistorial.NIT,
                                            CTAFPAGO = "165505001",
                                            VRTOTAL = valorInteresCorrienteUnDiaGlobal,
                                            SUMDBCR = 0,
                                            FECHARealiz = fechaActual,
                                            ANULADO = false
                                        };
                                        db.Comprobantes.Add(comprobanteIntCorriente);

                                        //CONSTRUIR LA LISTA DE MOVIMIENTOS
                                        List<Movimiento> listaDeMovimientos = new List<Movimiento>();

                                        var mov1 = new Movimiento()
                                        {
                                            TIPO = "CI3",
                                            NUMERO = ComprobanteUsar,
                                            CUENTA = "165505001",
                                            TERCERO = nuevoHistorial.NIT,
                                            DETALLE = "CAUSACION CREDITOS INTERES CORRIENTE",
                                            DEBITO = valorInteresCorrienteUnDiaGlobal,
                                            CREDITO = 0,
                                            BASE = 0,
                                            CCOSTO = "00",
                                            FECHAMOVIMIENTO = fechaActual,
                                        };
                                        listaDeMovimientos.Add(mov1);

                                        var mov2 = new Movimiento()
                                        {
                                            TIPO = "CI3",
                                            NUMERO = ComprobanteUsar,
                                            CUENTA = "418516001",
                                            TERCERO = nuevoHistorial.NIT,
                                            DETALLE = "CAUSACION CREDITOS INTERES CORRIENTE",
                                            DEBITO = 0,
                                            CREDITO = valorInteresCorrienteUnDiaGlobal,
                                            BASE = 0,
                                            CCOSTO = "00",
                                            FECHAMOVIMIENTO = fechaActual,
                                        };
                                        listaDeMovimientos.Add(mov2);

                                        var result = false;

                                        var causador = new interescausadoprestamos()
                                        {
                                            pagare = nuevoHistorial.pagare,
                                            intcorriente = interesCorrienteCausadoHastaAhora + valorInteresCorrienteUnDiaGlobal,
                                            intmora = interesMoraCausadoHastaAhora,
                                            fechasistema = fechaActual,
                                            agenciaId = 1,
                                            usuarioCauso = "AUTOMATICO",
                                            Tasainteres = nuevoHistorial.TazaInteresCorriente,
                                            numeroCuota = nuevoHistorial.numeroCuota,
                                            comprabante = "CI3",
                                            consecutivo = Convert.ToInt32(ComprobanteUsar),
                                            codcuentaingresosctes = "165505001",
                                            codcuentaingresosmora = "165505002"
                                        };
                                        db.interescausadoprestamos.Add(causador);

                                        var comprobanteConst = new ComprobanteBO();
                                        result = comprobanteConst.AsentarCausacion(listaDeMovimientos, Convert.ToInt32(ComprobanteUsar), "CI3");

                                        if (result)
                                        {
                                            db.SaveChanges();
                                        }
                                    }
                                }

                                db.SaveChanges();
                                
                            }
                        }
                        
                    }
                }
            }
        }

        public void agregarHoras()
        {
            var historialCreditos = db.HistorialCreditos.ToList();
            foreach (var item in historialCreditos)
            {
                item.fechaProximoPago = item.fechaProximoPago.AddHours(-2);
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();

        }

        public void quitarInteresMora()
        {
            DateTime fechaDesde = new DateTime(2020, 3, 29);
            var historialCreditos = db.HistorialCreditos.Where(a => a.fecha >= fechaDesde && a.estado == "enMora").ToList();
            foreach (var item in historialCreditos)
            {
                item.interesMora = 0;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
            }
            db.SaveChanges();
        }

        public void colocarCostos()
        {
            var creditos = db.Creditos.ToList();
            foreach (var item in creditos)
            {
                var costo = Convert.ToDecimal(item.Capital * 0.006);
                var historiales = (from d in db.HistorialCreditos where d.pagare == item.Pagare select d).ToList();
                foreach (var item2 in historiales)
                {
                    item2.valorCosto = costo;
                    db.Entry(item2).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        //public ActionResult listaSeguros()
        //{
        //    var creditos = db.Creditos.ToList();
        //    List<HistorialCreditos> ListaHistorialCreditos = new List<HistorialCreditos>();

        //    foreach (var item in creditos)
        //    {
        //        var historial = (from d in db.HistorialCreditos where d.pagare == item.Pagare orderby d.id descending select d).First();
        //        if(historial.valorCosto == 0)
        //        {
        //            ListaHistorialCreditos.Add(historial);
        //        }
        //    }
        //    return View(ListaHistorialCreditos);
        //}

        public ActionResult listaSeguros()
        {
            var creditos = db.Creditos.ToList();
            List<HistorialCreditos> ListaHistorialCreditos = new List<HistorialCreditos>();

            foreach (var item in creditos)
            {
                var historial = (from d in db.HistorialCreditos where d.pagare == item.Pagare orderby d.id descending select d).First();
                if (historial.valorCosto != 0)
                {
                    ListaHistorialCreditos.Add(historial);
                }
            }
            return View(ListaHistorialCreditos);
        }

        // GET: PruebaEstructuraCapas/PruebaEstructuras
        public JsonResult cambioFecha()
        {
            //var actualizarFecha = (from d in db.mifechas where d.id == 1 select d).First();
            //DateTime oldDate = new DateTime(2020, 3, 13);
            //actualizarFecha.mifechafecha = new DateTime(2020, 5, 4);
            //db.Entry(actualizarFecha).State = System.Data.Entity.EntityState.Modified;
            //db.SaveChanges();
            return Json(1, JsonRequestBehavior.AllowGet);
        }


        public ActionResult vistaInteresCausado()
        {
            var creditos = db.Creditos.ToList();
            List<interescausadoprestamos> listadeinteres = new List<interescausadoprestamos>();

            foreach (var item in creditos)
            {
                var ListadoCount = (from d in db.interescausadoprestamos where d.pagare == item.Pagare orderby d.id descending select d).Count();
                if (ListadoCount > 0)
                {
                    var Listado = (from d in db.interescausadoprestamos where d.pagare == item.Pagare orderby d.id descending select d).First();
                    listadeinteres.Add(Listado);
                }               
            }
            return View(listadeinteres);
        }

        public ActionResult Index()
        {
            var Listado = new PruebaEstructuraCapasBLL().GetPruebaEstructuras();
            return View(Listado);
        }

        ///PruebaEstructuraCapas/PruebaEstructuras/vistacausar
        public ActionResult vistacausar()
        {
            return View();
        }

        public JsonResult colocarpazysalvo(string pagare, int idhistorial, int idFactura)
        {
            var amortizacion = (from d in db.Amortizaciones where d.pagare == pagare select d).First();

            decimal valorPagado = Convert.ToDecimal(amortizacion.valorCuota);
            decimal valorSeguro = Convert.ToDecimal(amortizacion.valorCosto);

            var fechaActual = (from d in db.mifechas where d.id == 1 select d.mifechafecha).Single();
            var historialCreditMora = (from d in db.HistorialCreditos where d.id == idhistorial select d).First();

            var interesCorriente = historialCreditMora.interesCorriente;
            var interesMora = historialCreditMora.interesMora;
            var abonoCapital = valorPagado - interesCorriente - valorSeguro - interesMora;

            var nuevoSaldoCapital = historialCreditMora.saldoCapital - abonoCapital;

            var fechaProxPago = historialCreditMora.fechaProximoPago.AddMonths(1);

            historialCreditMora.fecha = fechaActual;
            historialCreditMora.idFactura = idFactura;
            historialCreditMora.abonoCapital = abonoCapital;
            historialCreditMora.abonoInteresMora = interesMora;
            historialCreditMora.AbonoInteresCorriente = interesCorriente;
            historialCreditMora.valorCosto = valorSeguro;
            historialCreditMora.estado = "pazYsalvo";

            var historialCredit = (from d in db.HistorialCreditos where d.pagare == pagare orderby d.id descending select d).First();

            historialCredit.saldoCapital = historialCredit.saldoCapital + interesMora;
            db.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }


        public JsonResult colocarpazysalvoDiasTerminados(int idhistorial, int idFactura)
        {
            var historioal = (from d in db.HistorialCreditos where d.id == idhistorial select d).First();
            var amortizacion = (from d in db.Amortizaciones where d.pagare == historioal.pagare select d).First();

            decimal valorPagado = Convert.ToDecimal(amortizacion.valorCuota);
            decimal valorSeguro = Convert.ToDecimal(amortizacion.valorCosto);

            var fechaActual = (from d in db.mifechas where d.id == 1 select d.mifechafecha).Single();
            var historialCredi = (from d in db.HistorialCreditos where d.id == idhistorial select d).First();

            var interesCorriente = historialCredi.interesCorriente;
            var abonoCapital = valorPagado - interesCorriente - valorSeguro;

            var nuevoSaldoCapital = historialCredi.saldoCapital - abonoCapital;

            var fechaProxPago = historialCredi.fechaProximoPago.AddMonths(1);

            historialCredi.fecha = fechaActual;
            historialCredi.idFactura = idFactura;
            historialCredi.abonoCapital = abonoCapital;
            historialCredi.AbonoInteresCorriente = interesCorriente;
            historialCredi.valorCosto = valorSeguro;
            historialCredi.estado = "pazYsalvo";

            db.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        public JsonResult colocarenmora(string pagare, int numeroCuota)
        {
            var amortizacion = (from d in db.Amortizaciones where d.pagare == pagare select d).First();

            decimal valorPagado = Convert.ToDecimal(amortizacion.valorCuota);
            decimal valorSeguro = Convert.ToDecimal(amortizacion.valorCosto);

            var fechaActual = (from d in db.mifechas where d.id == 1 select d.mifechafecha).Single();
            var historialCredit = (from d in db.HistorialCreditos where d.pagare == pagare orderby d.id descending select d).First();

            var interesCorriente = historialCredit.interesCorriente;
            var abonoCapital = valorPagado - interesCorriente - valorSeguro;

            var nuevoSaldoCapital = historialCredit.saldoCapital - abonoCapital;

            var fechaProxPago = historialCredit.fechaProximoPago.AddMonths(1);

            historialCredit.fecha = fechaActual;
            historialCredit.interesCorrienteMora = historialCredit.interesCorriente;
            historialCredit.capitalEnMora = abonoCapital;
            historialCredit.estado = "enMora";

            var nuevoHistorial = new HistorialCreditos()
            {
                fecha = fechaActual,
                idFactura = 0,
                NIT = historialCredit.NIT,
                pagare = historialCredit.pagare,
                abonoCapital = 0,
                abonoInteresMora = 0,
                AbonoInteresCorriente = 0,
                //jme
                valorCosto = valorSeguro,
                saldoCapital = nuevoSaldoCapital,
                proximaCuota = historialCredit.proximaCuota,
                capitalEnMora = 0,
                TazaInteresMora = historialCredit.TazaInteresMora,
                TazaInteresCorriente = historialCredit.TazaInteresCorriente,
                diasCausados = 0,
                diasEnMora = 0,
                numeroCuota = numeroCuota,
                fechaProximoPago = fechaProxPago,
                interesCorrienteMora = 0,
                interesCorriente = 0,
                estado = "normal",
                interesMora = 0
            };
            db.HistorialCreditos.Add(nuevoHistorial);
            db.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        public JsonResult diasterminados(string pagare, int numeroCuota)
        {
            var amortizacion = (from d in db.Amortizaciones where d.pagare == pagare select d).First();

            decimal valorPagado = Convert.ToDecimal(amortizacion.valorCuota);
            decimal valorSeguro = Convert.ToDecimal(amortizacion.valorCosto);

            var fechaActual = (from d in db.mifechas where d.id == 1 select d.mifechafecha).Single();
            var historialCredit = (from d in db.HistorialCreditos where d.pagare == pagare orderby d.id descending select d).First();

            var interesCorriente = historialCredit.interesCorriente;
            var abonoCapital = valorPagado - interesCorriente - valorSeguro;

            var nuevoSaldoCapital = historialCredit.saldoCapital - abonoCapital;

            var fechaProxPago = historialCredit.fechaProximoPago.AddMonths(1);

            historialCredit.fecha = fechaActual;
            historialCredit.estado = "diasTerminados";

            var nuevoHistorial = new HistorialCreditos()
            {
                fecha = fechaActual,
                idFactura = 0,
                NIT = historialCredit.NIT,
                pagare = historialCredit.pagare,
                abonoCapital = 0,
                abonoInteresMora = 0,
                AbonoInteresCorriente = 0,
                valorCosto = 0,
                saldoCapital = nuevoSaldoCapital,
                proximaCuota = historialCredit.proximaCuota,
                capitalEnMora = 0,
                TazaInteresMora = historialCredit.TazaInteresMora,
                TazaInteresCorriente = historialCredit.TazaInteresCorriente,
                diasCausados = 0,
                diasEnMora = 0,
                numeroCuota = numeroCuota,
                fechaProximoPago = fechaProxPago,
                interesCorrienteMora = 0,
                interesCorriente = 0,
                estado = "normal",
                interesMora = 0
            };
            db.HistorialCreditos.Add(nuevoHistorial);
            db.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        public JsonResult colocarenmoraDiasTerminados(int idHistorial)
        {
            var historioal = (from d in db.HistorialCreditos where d.id == idHistorial select d).First();
            var amortizacion = (from d in db.Amortizaciones where d.pagare == historioal.pagare select d).First();

            decimal valorPagado = Convert.ToDecimal(amortizacion.valorCuota);
            decimal valorSeguro = Convert.ToDecimal(amortizacion.valorCosto);

            var fechaActual = (from d in db.mifechas where d.id == 1 select d.mifechafecha).Single();
            var historialCredit = (from d in db.HistorialCreditos where d.id == idHistorial select d).First();

            var interesCorriente = historialCredit.interesCorriente;
            var abonoCapital = valorPagado - interesCorriente - valorSeguro;

            var nuevoSaldoCapital = historialCredit.saldoCapital - abonoCapital;

            var fechaProxPago = historialCredit.fechaProximoPago.AddMonths(1);

            historialCredit.fecha = fechaActual;
            historialCredit.interesCorrienteMora = historialCredit.interesCorriente;
            historialCredit.capitalEnMora = abonoCapital;
            historialCredit.estado = "enMora";

            db.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        public JsonResult pagoantesocumplido(string pagare, int idFactura, int numeroCuota)
        {
            var amortizacion = (from d in db.Amortizaciones where d.pagare == pagare select d).First();

            decimal valorPagado = Convert.ToDecimal(amortizacion.valorCuota);
            decimal valorSeguro = Convert.ToDecimal(amortizacion.valorCosto);

            var fechaActual = (from d in db.mifechas where d.id == 1 select d.mifechafecha).Single();
            var historialCredit = (from d in db.HistorialCreditos where d.pagare == pagare orderby d.id descending select d).First();

            var interesCorriente = historialCredit.interesCorriente;
            var abonoCapital = valorPagado - interesCorriente - valorSeguro;

            var nuevoSaldoCapital = historialCredit.saldoCapital - abonoCapital;

            var fechaProxPago = historialCredit.fechaProximoPago.AddMonths(1);

            historialCredit.fecha = fechaActual;
            historialCredit.idFactura = idFactura;
            historialCredit.abonoCapital = abonoCapital;
            historialCredit.AbonoInteresCorriente = interesCorriente;
            historialCredit.valorCosto = valorSeguro;
            historialCredit.estado = "pazYsalvo";

            var nuevoHistorial = new HistorialCreditos()
            {
                fecha = fechaActual,
                idFactura = 0,
                NIT = historialCredit.NIT,
                pagare = historialCredit.pagare,
                abonoCapital = 0,
                abonoInteresMora = 0,
                AbonoInteresCorriente = 0,
                //jme
                valorCosto = valorSeguro,
                saldoCapital = nuevoSaldoCapital,
                proximaCuota = historialCredit.proximaCuota,
                capitalEnMora = 0,
                TazaInteresMora = historialCredit.TazaInteresMora,
                TazaInteresCorriente = historialCredit.TazaInteresCorriente,
                diasCausados = 0,
                diasEnMora = 0,
                numeroCuota = numeroCuota,
                fechaProximoPago = fechaProxPago,
                interesCorrienteMora = 0,
                interesCorriente = 0,
                estado = "normal",
                interesMora = 0
            };
            db.HistorialCreditos.Add(nuevoHistorial);
            db.SaveChanges();

            return Json("ok", JsonRequestBehavior.AllowGet);
        }

        public JsonResult causar(string pagare)
        {
            //for (int i = 0; i < 25; i++)
            //{
                var fechaActual = (from d in db.mifechas where d.id == 1 select d).Single();

                var creditosACausarList = (from d in db.HistorialCreditos where d.pagare == pagare orderby d.id descending select d).ToList();
                var datosCredito = (from d in db.HistorialCreditos where d.pagare == pagare orderby d.id descending select d).First();

                decimal valorInteresCorrienteUnDiaGlobal = 0;
                decimal valorInteresMoraUnDiaGlobal = 0;

                foreach (var creditosACausar in creditosACausarList)
                {
                    if (creditosACausar.estado == "enMora")
                    {
                        decimal valorInteresMora = 0;

                        var prestamo = (from d in db.Prestamos where d.Pagare == creditosACausar.pagare select d).First();
                        var destino = (from d in db.Destinos where d.Destino_Id == prestamo.Destino_Id select d).First();

                        var interesMoraDeUnDia = (destino.Destino_Tasa_Mora / 30) / 100;
                        valorInteresMora = (creditosACausar.interesCorrienteMora + creditosACausar.capitalEnMora) * interesMoraDeUnDia;
                        valorInteresMoraUnDiaGlobal = valorInteresMoraUnDiaGlobal + valorInteresMora;

                        creditosACausar.interesMora = creditosACausar.interesMora + valorInteresMora;
                        creditosACausar.diasCausados = creditosACausar.diasCausados + 1;
                        creditosACausar.diasEnMora = creditosACausar.diasEnMora + 1;

                        db.Entry(creditosACausar).State = System.Data.Entity.EntityState.Modified;
                    }

                    if (creditosACausar.estado == "normal")
                    {
                        decimal valorInteresCorrienteUnDia = 0;

                        var prestamo = (from d in db.Prestamos where d.Pagare == creditosACausar.pagare select d).First();
                        var destino = (from d in db.Destinos where d.Destino_Id == prestamo.Destino_Id select d).First();

                        var interesCorrienteDeUnDia = (prestamo.Interes / 30) / 100;

                        valorInteresCorrienteUnDia = interesCorrienteDeUnDia * creditosACausar.saldoCapital;
                        valorInteresCorrienteUnDiaGlobal = valorInteresCorrienteUnDiaGlobal + valorInteresCorrienteUnDia;

                        creditosACausar.diasCausados = creditosACausar.diasCausados + 1;
                        creditosACausar.interesCorriente = creditosACausar.interesCorriente + valorInteresCorrienteUnDia;
                    //Jme
                        creditosACausar.valorCosto = prestamo.costoAdicionalEnEltiempo;
                        db.Entry(creditosACausar).State = System.Data.Entity.EntityState.Modified;
                    }
                }

                //COMPROBANTE Y MOVIMIENTOS INTERES DE MORA
                if (valorInteresMoraUnDiaGlobal > 0)
                {
                    var interesCausadoAnteriorCount = (from d in db.interescausadoprestamos where d.pagare == datosCredito.pagare orderby d.id descending select d).Count();
                    decimal interesCorrienteCausadoHastaAhora = 0;
                    decimal interesMoraCausadoHastaAhora = 0;

                    if (interesCausadoAnteriorCount > 0)
                    {
                        var interesCausadoAnterior = (from d in db.interescausadoprestamos where d.pagare == datosCredito.pagare orderby d.id descending select d).First();
                        interesCorrienteCausadoHastaAhora = interesCausadoAnterior.intcorriente;
                        interesMoraCausadoHastaAhora = interesCausadoAnterior.intmora;
                    }

                    
                    var ComprobanteUsar = db.TiposComprobantes.FirstOrDefault(x => x.CODIGO == "CI4" & x.INACTIVO == false);

                    var comprobanteIntMora = new Comprobante()
                    {
                        TIPO = "CI4",
                        NUMERO = ComprobanteUsar.CONSECUTIVO,
                        ANO = Convert.ToString(DateTime.Now.Year),
                        MES = Convert.ToString(DateTime.Now.Month),
                        DIA = Convert.ToString(DateTime.Now.Day),
                        CCOSTO = "00",
                        DETALLE = "CAUSACION CREDITOS INTERES MORA",
                        TERCERO = datosCredito.NIT,
                        CTAFPAGO = "165505002",//CAMBIAR CUENTA PARA INTERES DE MORA
                        VRTOTAL = valorInteresMoraUnDiaGlobal,
                        SUMDBCR = 0,
                        FECHARealiz = fechaActual.mifechafecha,
                        ANULADO = false
                    };
                    db.Comprobantes.Add(comprobanteIntMora);

                    //CONSTRUIR LA LISTA DE MOVIMIENTOS
                    List<Movimiento> listaDeMovimientos = new List<Movimiento>();

                    var mov1 = new Movimiento()
                    {
                        TIPO = "CI4",
                        NUMERO = ComprobanteUsar.CONSECUTIVO,
                        CUENTA = "165505002",
                        TERCERO = datosCredito.NIT,
                        DETALLE = "CAUSACION CREDITOS INTERES MORA",
                        DEBITO = valorInteresMoraUnDiaGlobal,
                        CREDITO = 0,
                        BASE = 0,
                        CCOSTO = "00",
                        FECHAMOVIMIENTO = fechaActual.mifechafecha,
                    };
                    listaDeMovimientos.Add(mov1);

                    var mov2 = new Movimiento()
                    {
                        TIPO = "CI4",
                        NUMERO = ComprobanteUsar.CONSECUTIVO,
                        CUENTA = "418510003",
                        TERCERO = datosCredito.NIT,
                        DETALLE = "CAUSACION CREDITOS INTERES MORA",
                        DEBITO = 0,
                        CREDITO = valorInteresMoraUnDiaGlobal,
                        BASE = 0,
                        CCOSTO = "00",
                        FECHAMOVIMIENTO = fechaActual.mifechafecha,
                    };
                    listaDeMovimientos.Add(mov2);

                    var causador = new interescausadoprestamos()
                    {
                        pagare = datosCredito.pagare,
                        intcorriente = interesCorrienteCausadoHastaAhora,
                        intmora = interesMoraCausadoHastaAhora + valorInteresMoraUnDiaGlobal,
                        fechasistema = fechaActual.mifechafecha,
                        agenciaId = 1,
                        usuarioCauso = "AUTOMATICO",
                        Tasainteres = datosCredito.TazaInteresCorriente,
                        numeroCuota = datosCredito.numeroCuota,
                        comprabante = "CI4",
                        consecutivo = Convert.ToInt32(ComprobanteUsar.CONSECUTIVO),
                        codcuentaingresosctes = "165505001",
                        codcuentaingresosmora = "165505002"
                    };
                    db.interescausadoprestamos.Add(causador);

                    var result = false;

                    var comprobanteConst = new ComprobanteBO();
                    result = comprobanteConst.AsentarCausacion(listaDeMovimientos, Convert.ToInt32(ComprobanteUsar.CONSECUTIVO), "CI4");

                    if (result)
                    {
                        db.SaveChanges();
                    }
                }

                //COMPROBANTE Y MOVIMIENTOS INTERES CORRIENTE
                if (valorInteresCorrienteUnDiaGlobal != 0)
                {
                    var interesCausadoAnteriorCount = (from d in db.interescausadoprestamos where d.pagare == datosCredito.pagare orderby d.id descending select d).Count();
                    decimal interesCorrienteCausadoHastaAhora = 0;
                    decimal interesMoraCausadoHastaAhora = 0;

                    if (interesCausadoAnteriorCount > 0)
                    {
                        var interesCausadoAnterior = (from d in db.interescausadoprestamos where d.pagare == datosCredito.pagare orderby d.id descending select d).First();
                        interesCorrienteCausadoHastaAhora = interesCausadoAnterior.intcorriente;
                        interesMoraCausadoHastaAhora = interesCausadoAnterior.intmora;
                    }

                    string ComprobanteUsar = "";
                    var traerConsecutivo = new ComprobanteBO();
                    ComprobanteUsar = traerConsecutivo.proximoConsecutivo("CI3");

                    var comprobanteIntCorriente = new Comprobante()
                    {
                        TIPO = "CI3",
                        NUMERO = ComprobanteUsar,
                        ANO = Convert.ToString(DateTime.Now.Year),
                        MES = Convert.ToString(DateTime.Now.Month),
                        DIA = Convert.ToString(DateTime.Now.Day),
                        CCOSTO = "00",
                        DETALLE = "CAUSACION CREDITOS INTERES CORRIENTE",
                        TERCERO = datosCredito.NIT,
                        CTAFPAGO = "165505001",
                        VRTOTAL = valorInteresCorrienteUnDiaGlobal,
                        SUMDBCR = 0,
                        FECHARealiz = fechaActual.mifechafecha,
                        ANULADO = false
                    };
                    db.Comprobantes.Add(comprobanteIntCorriente);

                    //CONSTRUIR LA LISTA DE MOVIMIENTOS
                    List<Movimiento> listaDeMovimientos = new List<Movimiento>();

                    var mov1 = new Movimiento()
                    {
                        TIPO = "CI3",
                        NUMERO = ComprobanteUsar,
                        CUENTA = "165505001",
                        TERCERO = datosCredito.NIT,
                        DETALLE = "CAUSACION CREDITOS INTERES CORRIENTE",
                        DEBITO = valorInteresCorrienteUnDiaGlobal,
                        CREDITO = 0,
                        BASE = 0,
                        CCOSTO = "00",
                        FECHAMOVIMIENTO = fechaActual.mifechafecha,
                    };
                    listaDeMovimientos.Add(mov1);

                    var mov2 = new Movimiento()
                    {
                        TIPO = "CI3",
                        NUMERO = ComprobanteUsar,
                        CUENTA = "418516001",
                        TERCERO = datosCredito.NIT,
                        DETALLE = "CAUSACION CREDITOS INTERES CORRIENTE",
                        DEBITO = 0,
                        CREDITO = valorInteresCorrienteUnDiaGlobal,
                        BASE = 0,
                        CCOSTO = "00",
                        FECHAMOVIMIENTO = fechaActual.mifechafecha,
                    };
                    listaDeMovimientos.Add(mov2);

                    var result = false;

                    var causador = new interescausadoprestamos()
                    {
                        pagare = datosCredito.pagare,
                        intcorriente = interesCorrienteCausadoHastaAhora + valorInteresCorrienteUnDiaGlobal,
                        intmora = interesMoraCausadoHastaAhora,
                        fechasistema = fechaActual.mifechafecha,
                        agenciaId = 1,
                        usuarioCauso = "AUTOMATICO",
                        Tasainteres = datosCredito.TazaInteresCorriente,
                        numeroCuota = datosCredito.numeroCuota,
                        comprabante = "CI3",
                        consecutivo = Convert.ToInt32(ComprobanteUsar),
                        codcuentaingresosctes = "165505001",
                        codcuentaingresosmora = "165505002"
                    };
                    db.interescausadoprestamos.Add(causador);

                    var comprobanteConst = new ComprobanteBO();
                    result = comprobanteConst.AsentarCausacion(listaDeMovimientos, Convert.ToInt32(ComprobanteUsar), "CI3");

                    if (result)
                    {
                        fechaActual.mifechafecha = fechaActual.mifechafecha.AddDays(1);
                        db.Entry(fechaActual).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            //}
             
            return Json("ok", JsonRequestBehavior.AllowGet);

        }

        // GET: PruebaEstructuraCapas/PruebaEstructuras/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PruebaEstructuraCapas/PruebaEstructuras/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nombre,cedula")] PruebaEstructura pruebaEstructura)
        {
            if (ModelState.IsValid)
            {
                var respuesta = new PruebaEstructuraCapasBLL().Create(pruebaEstructura);
                if(respuesta)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    //enviar una alerta ademas de las automaticas o algun otro procesos llamadolo desde BLL con logica en DAL
                }               
            }
            return View(pruebaEstructura);
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
