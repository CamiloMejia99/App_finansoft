using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Ahorros;
using FNTC.Finansoft.Accounting.DTO.Aportes;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.Fichas;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using FNTC.Finansoft.DTO.Respuestas;
using FNTC.Finansoft.UI.Tools;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FNTC.Finansoft.Accounting.DAL.Caja
{
    public class OperacionesCaja
    {
        NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;

        
        
        public RespuestaAporte PagarAporte(string cuenta, string cajero, decimal valor)
        {
            var guid = Guid.NewGuid().ToString();

            var respuesta = new RespuestaAporte{ Status = true };
            decimal valorPago = Convert.ToDecimal(valor);
            decimal AuxValorPago = Convert.ToDecimal(valor);//valor referente sólo para cálculos
            decimal pagoAporte = 0,pagoCuentas = 0,pagoAhorro=0;
            var listComprobantes = new List<Comprobante>();
            var listMovimientos = new List<Movimiento>();
            using (var ctx = new AccountingContext())
            {
                try
                {
                    var fichaAporte = ctx.FichasAportes.Where(x => x.numeroCuenta == cuenta).FirstOrDefault();
                    var configuracionActualAporte = ctx.Configuracion1.Where(x => x.activo == true).FirstOrDefault();
                    var cuentasDistribucion = ctx.CuentasDistribucionAportes.Where(x => x.Estado == true).ToList();
                    var fichaAhorro = new FichasAhorros();

                    //se calcula el valor de pago de aporte según el porcentaje
                    decimal porcentajeAporte = Convert.ToDecimal(fichaAporte.porcentaje);
                    pagoAporte = decimal.Multiply(AuxValorPago, decimal.Divide(porcentajeAporte, 100));
                    pagoAporte = Math.Round(pagoAporte, 0, MidpointRounding.ToEven);
                    valorPago -= pagoAporte;
                    if (fichaAporte.Configuracion1.idTipoCuotaCalculo == 1 || fichaAporte.Configuracion1.idTipoCuotaCalculo == 2)
                    {

                        //se calcula el valor de pago de cuentas de distribución si las hay
                        if (valorPago > 0) {
                            if (cuentasDistribucion.Count() > 0)
                            {
                                decimal porcentajeCuentas = cuentasDistribucion.Select(x => x.Porcentaje).Sum();
                                pagoCuentas = decimal.Multiply(AuxValorPago, decimal.Divide(porcentajeCuentas, 100));
                                pagoCuentas = Math.Round(pagoCuentas,0, MidpointRounding.ToEven);
                                valorPago -=pagoCuentas;
                            }
                        }

                        //se calcula el valor de pago de ahorros permanente si las hay
                        fichaAhorro = ctx.FichasAhorros.Where(x => x.idPersona == fichaAporte.idPersona && x.tipoFicha == "FAP" && x.activo == true).FirstOrDefault();
                        if (fichaAporte != null)
                        {
                            decimal porcentajeAhorros = Convert.ToDecimal(fichaAhorro.Configuracion.porcentajeParaAhorros);
                            pagoAhorro = decimal.Multiply(AuxValorPago,decimal.Divide(porcentajeAhorros, 100));
                            pagoAhorro = Math.Round(pagoAhorro, 2, MidpointRounding.ToEven);
                            if (pagoAhorro > valorPago) { pagoAhorro = valorPago; }
                            valorPago -= pagoAhorro;
                        }

                        //si existe sobrante debido a que no se completa el 100% de porcentaje total se lo paga a aportes
                        if (valorPago > 0) { pagoAporte += valorPago; }

                        

                    }

                    //calculamos la fecha actual hora Colombia
                    DateTime fechaActual = Fecha.GetFechaColombia();
                    string fechaString = fechaActual.ToString("yyyy-MM-dd");

                    //información del cajero y caja
                    var InfoCajero = ctx.configCajero.Where(x => x.Nit_cajero == cajero).FirstOrDefault();
                    var InfoCaja = ctx.Caja.Where(x => x.Codigo_caja == InfoCajero.Codigo_caja).FirstOrDefault();

                    //actualizamos consecutivo de caja y tope máximo
                    InfoCaja.consecutivo_actual++;
                    InfoCaja.TopeMaximo_caja += Convert.ToDouble(pagoAporte+pagoCuentas);
                    if (InfoCaja.consecutivo_actual > InfoCaja.consecutivo_fin)
                    {
                        InfoCaja.consecutivo_actual = InfoCaja.Consecutivo_ini;
                        InfoCaja.Serie += 1;
                    }
                    ctx.Entry(InfoCaja).State = System.Data.Entity.EntityState.Modified;


                    //Actualizamos el cuadre de caja
                    var CuadreCaja = ctx.CuadreCajaPorCajero.Where(x => x.fecha == fechaString && x.codigo_caja == InfoCaja.Codigo_caja && x.nit_cajero == cajero && x.cierre == 0).FirstOrDefault();
                    CuadreCaja.tope += (pagoAporte+pagoCuentas);
                    CuadreCaja.consignacion_efectivo += (pagoAporte + pagoCuentas);
                    ctx.Entry(CuadreCaja).State = System.Data.Entity.EntityState.Modified;

                    #region pago de aporte y cuentas de distribución

                    //realizamos el pago a la ficha de aporte del asociado
                    fichaAporte.totalAportes = (Convert.ToDecimal(fichaAporte.totalAportes) + pagoAporte).ToString();
                    ctx.Entry(fichaAporte).State = System.Data.Entity.EntityState.Modified;

                    //construimos el comprobante
                    var tipoComprobante = ctx.TiposComprobantes.Where(x => x.CODIGO == InfoCajero.Compr_ingreso && x.INACTIVO == false).FirstOrDefault();
                    var comprobante = new Comprobante()
                    {
                        TIPO = InfoCajero.Compr_ingreso,
                        NUMERO = tipoComprobante.CONSECUTIVO,
                        ANO = fechaActual.Year.ToString(),
                        MES = fechaActual.Month.ToString(),
                        DIA = fechaActual.Day.ToString(),
                        CCOSTO = InfoCajero.centrocosto,
                        DETALLE = "CONSIGNACION APORTE CAJA",
                        TERCERO = fichaAporte.idPersona,
                        CTAFPAGO = configuracionActualAporte.idCuenta,
                        VRTOTAL = pagoAporte+pagoCuentas,
                        SUMDBCR = 0,
                        FECHARealiz = fechaActual,
                        ANULADO = false
                    };

                    listComprobantes.Add(comprobante);

                    //creamos los movimientos
                    var mov1 = new Movimiento() // movimiento que entra a caja
                    {
                        TIPO = InfoCajero.Compr_ingreso,
                        NUMERO = tipoComprobante.CONSECUTIVO,
                        CUENTA = InfoCajero.Cta_efectivo,
                        TERCERO = fichaAporte.idPersona,
                        DETALLE = "CONSIGNACION APORTE CAJA",
                        DEBITO = pagoAporte+pagoCuentas,
                        CREDITO = 0,
                        BASE = 0,
                        CCOSTO = InfoCajero.centrocosto,
                        FECHAMOVIMIENTO = fechaActual
                    };
                    listMovimientos.Add(mov1);




                    /*

                    var mov2 = new Movimiento() // movimiento que entra a la cuenta configurada en aportes
                    {
                        TIPO = InfoCajero.Compr_ingreso,
                        NUMERO = tipoComprobante.CONSECUTIVO,
                        CUENTA = configuracionActualAporte.idCuenta,
                        TERCERO = fichaAporte.idPersona,
                        DETALLE = "CONSIGNACION APORTE CAJA",
                        DEBITO = 0,
                        //CREDITO = pagoAporte,
                        BASE = 0,
                        CCOSTO = InfoCajero.centrocosto,
                        FECHAMOVIMIENTO = fechaActual
                    };

                    listMovimientos.Add(mov2);

                    */

                    foreach (var item in cuentasDistribucion)
                    {
                        var mov3 = new Movimiento()
                        {
                            TIPO = InfoCajero.Compr_ingreso,
                            NUMERO = tipoComprobante.CONSECUTIVO,
                            CUENTA = item.Cuenta,
                            TERCERO = fichaAporte.idPersona,
                            DETALLE = "CONSIGNACION APORTE CAJA",
                            DEBITO = 0,
                            CREDITO = Math.Round(decimal.Multiply(AuxValorPago,decimal.Divide(item.Porcentaje,100)),0, MidpointRounding.ToEven),
                            BASE = 0,
                            CCOSTO = InfoCajero.centrocosto,
                            FECHAMOVIMIENTO = fechaActual
                        };
                        listMovimientos.Add(mov3);
                    }

                    //creamos la factura de aporte
                    string numeroFactura = InfoCaja.agencia + "-" + InfoCaja.Codigo_caja + "-" + InfoCaja.Serie + "-" + InfoCaja.consecutivo_actual;
                    var facturaAporte = new FactOpcaja()
                    {
                        fecha = fechaActual,
                        factura = numeroFactura,
                        operacion = "Consignación",
                        codigo_caja = InfoCaja.Codigo_caja,
                        nit_cajero = cajero,
                        numero_cuenta = fichaAporte.numeroCuenta,
                        IdProducto=1,//aporte ordinario
                        nit_propietario_cuenta = fichaAporte.idPersona,
                        valor_recibido = pagoAporte,
                        valor_efectivo = pagoAporte,
                        vueltas = 0,
                        valor_cheque = 0,
                        consignacion = 0,
                        saldo_total_cuenta=0,
                        total = pagoAporte+pagoCuentas,
                        valor_cheque1 = 0,
                        valor_cheque2 = 0,
                        valor_cheque3 = 0,
                        valor_cheque4 = 0,
                        valor_cheque5 = 0,
                        total_cheques = 0,
                        TIPO = InfoCajero.Compr_ingreso,
                        NUMERO = tipoComprobante.CONSECUTIVO
                    };
                    ctx.FactOpcaja.Add(facturaAporte);

                    #endregion


                    #region pago de ahorro

                    if (pagoAhorro > 0)
                    {
                        var configuracionActualAhorro = ctx.Configuracion.Where(x => x.activo == true).FirstOrDefault();

                        //actualizamos consecutivo de caja y tope máximo
                        InfoCaja.consecutivo_actual++;
                        InfoCaja.TopeMaximo_caja += Convert.ToDouble(pagoAhorro);
                        if (InfoCaja.consecutivo_actual > InfoCaja.consecutivo_fin)
                        {
                            InfoCaja.consecutivo_actual = InfoCaja.Consecutivo_ini;
                            InfoCaja.Serie += 1;
                        }
                        ctx.Entry(InfoCaja).State = System.Data.Entity.EntityState.Modified;


                        //Actualizamos el cuadre de caja
                        CuadreCaja.tope += (pagoAhorro);
                        CuadreCaja.consignacion_efectivo += (pagoAhorro);
                        ctx.Entry(CuadreCaja).State = System.Data.Entity.EntityState.Modified;

                        //realizamos el pago a la ficha de ahorro del asociado
                        fichaAhorro.totalAhorros = (Convert.ToDecimal(fichaAhorro.totalAhorros) + pagoAhorro).ToString();
                        ctx.Entry(fichaAhorro).State = System.Data.Entity.EntityState.Modified;

                        //construimos el comprobante
                        //comprobante = new Comprobante()
                        //{
                        //    TIPO = InfoCajero.Compr_ingreso,
                        //    NUMERO = tipoComprobante.CONSECUTIVO,
                        //    ANO = fechaActual.Year.ToString(),
                        //    MES = fechaActual.Month.ToString(),
                        //    DIA = fechaActual.Day.ToString(),
                        //    CCOSTO = InfoCajero.centrocosto,
                        //    DETALLE = "CONSIGNACION AHORRO CAJA",
                        //    TERCERO = fichaAhorro.idPersona,
                        //    CTAFPAGO = configuracionActualAhorro.cuentaContable.ToString(),
                        //    VRTOTAL = pagoAhorro,
                        //    SUMDBCR = 0,
                        //    FECHARealiz = fechaActual,
                        //    ANULADO = false
                        //};

                        //listComprobantes.Add(comprobante);

                        //creamos los movimientos
                        //mov1 = new Movimiento() // movimiento que entra a caja
                        //{
                        //    TIPO = InfoCajero.Compr_ingreso,
                        //    NUMERO = tipoComprobante.CONSECUTIVO,
                        //    CUENTA = InfoCajero.Cta_efectivo,
                        //    TERCERO = fichaAhorro.idPersona,
                        //    DETALLE = "CONSIGNACION AHORRO CAJA",
                        //    DEBITO = pagoAhorro,
                        //    CREDITO = 0,
                        //    BASE = 0,
                        //    CCOSTO = InfoCajero.centrocosto,
                        //    FECHAMOVIMIENTO = fechaActual
                        //};
                        //listMovimientos.Add(mov1);
                        comprobante.VRTOTAL += pagoAhorro;
                        mov1.DEBITO += pagoAhorro;



                        /*

                         mov2 = new Movimiento() // movimiento que entra a la cuenta configurada en ahorros
                        {
                            TIPO = InfoCajero.Compr_ingreso,
                            NUMERO = tipoComprobante.CONSECUTIVO,
                            CUENTA = configuracionActualAhorro.cuentaContable.ToString(),
                            TERCERO = fichaAhorro.idPersona,
                            DETALLE = "CONSIGNACION AHORRO CAJA",
                            DEBITO = 0,
                            CREDITO = pagoAhorro,
                            BASE = 0,
                            CCOSTO = InfoCajero.centrocosto,
                            FECHAMOVIMIENTO = fechaActual
                        };
                        listMovimientos.Add(mov2);
                        */


                        //creamos la factura de ahorro
                        numeroFactura = InfoCaja.agencia + "-" + InfoCaja.Codigo_caja + "-" + InfoCaja.Serie + "-" + InfoCaja.consecutivo_actual;
                        var facturaAhorro = new FactOpcaja()
                        {
                            fecha = fechaActual,
                            factura = numeroFactura,
                            operacion = "Consignación",
                            codigo_caja = InfoCaja.Codigo_caja,
                            nit_cajero = cajero,
                            numero_cuenta = fichaAhorro.numeroCuenta,
                            IdProducto=3,//ahorro permanente
                            nit_propietario_cuenta = fichaAhorro.idPersona,
                            valor_recibido = pagoAhorro,
                            valor_efectivo = pagoAhorro,
                            vueltas = 0,
                            valor_cheque = 0,
                            consignacion = 0,
                            saldo_total_cuenta = 0,
                            total = pagoAhorro,
                            valor_cheque1 = 0,
                            valor_cheque2 = 0,
                            valor_cheque3 = 0,
                            valor_cheque4 = 0,
                            valor_cheque5 = 0,
                            total_cheques = 0,
                            TIPO = InfoCajero.Compr_ingreso,
                            NUMERO = tipoComprobante.CONSECUTIVO
                        };
                        ctx.FactOpcaja.Add(facturaAhorro);

                        
                    }

                    #endregion
                    //actualizamos el consecutivo del comprobante
                    tipoComprobante.CONSECUTIVO = (Convert.ToInt64(tipoComprobante.CONSECUTIVO) + 1).ToString();
                    ctx.Entry(tipoComprobante).State = System.Data.Entity.EntityState.Modified;

                    //guardamos en memoria los comprobantes
                    ctx.Comprobantes.AddRange(listComprobantes);
                    //guardamos en memoria los movimientos
                    ctx.Movimientos.AddRange(listMovimientos);

                    //guardamos los cambios en base de datos
                    ctx.SaveChanges();

                    respuesta.Id = facturaAporte.id;
                    respuesta.PagoCuentas = pagoCuentas;
                    respuesta.CuentasDistribucion = cuentasDistribucion;
                    return respuesta;
                }
                catch (Exception ex)
                {
                    string mensaje = "Ha ocurrido un error internamente";
                    respuesta.Status = false;
                    respuesta.Mensaje = mensaje;
                    return respuesta;
                }

            }
            
        }

        #region APORTE EXTRAORDINARIO
        public RespuestaAporte PagarAporteEx(string cuentacodigo_caja, string nit_cajero, string nit_propietario_cuenta, string numero_cuenta, string observacion, string operacion, decimal saldo_total_cuenta, decimal total)
        {
            var guid = Guid.NewGuid().ToString();

            var respuesta = new RespuestaAporte { Status = true };
            decimal valorPago = Convert.ToDecimal(total);
            decimal AuxValorPago = Convert.ToDecimal(total);//valor referente sólo para cálculos

            var listComprobantes = new List<Comprobante>();
            var listMovimientos = new List<Movimiento>();
            using (var ctx = new AccountingContext())
            {
                try
                {
                    var fichaAporteEX = ctx.FichaAfiliadosAporteEx.Where(x => x.NumeroCuenta == numero_cuenta).FirstOrDefault();
                    var DataTercero = ctx.Terceros.Where(x => x.NIT == fichaAporteEX.idPersona).FirstOrDefault();
                    var configuracionActualAporte = ctx.Configuracion2Ex.Where(x => x.estado == true).FirstOrDefault();
                    //var cuentasDistribucion = ctx.CuentasDistribucionAportes.Where(x => x.Estado == true).ToList(); //Pendiente--
                    var fichaAhorro = new FichasAhorros();

                    //se calcula el valor de pago de aporte según el porcentaje
                    //decimal porcentajeAporte = Convert.ToDecimal(fichaAporte.porcentaje);
                    //pagoAporte = decimal.Multiply(AuxValorPago, decimal.Divide(porcentajeAporte, 100));
                    //pagoAporte = Math.Round(pagoAporte, 0, MidpointRounding.ToEven);
                    //valorPago -= pagoAporte;
                    //if (fichaAporte.Configuracion1.idTipoCuotaCalculo == 1 || fichaAporte.Configuracion1.idTipoCuotaCalculo == 2)
                    //{

                    //    se calcula el valor de pago de cuentas de distribución si las hay
                    //    if (valorPago > 0)
                    //    {
                    //        if (cuentasDistribucion.Count() > 0)
                    //        {
                    //            decimal porcentajeCuentas = cuentasDistribucion.Select(x => x.Porcentaje).Sum();
                    //            pagoCuentas = decimal.Multiply(AuxValorPago, decimal.Divide(porcentajeCuentas, 100));
                    //            pagoCuentas = Math.Round(pagoCuentas, 0, MidpointRounding.ToEven);
                    //            valorPago -= pagoCuentas;
                    //        }
                    //    }

                    //    se calcula el valor de pago de ahorros permanente si las hay
                    //    fichaAhorro = ctx.FichasAhorros.Where(x => x.idPersona == fichaAporte.idPersona && x.tipoFicha == "FAP" && x.activo == true).FirstOrDefault();
                    //    if (fichaAporte != null)
                    //    {
                    //        decimal porcentajeAhorros = Convert.ToDecimal(fichaAhorro.Configuracion.porcentajeParaAhorros);
                    //        pagoAhorro = decimal.Multiply(AuxValorPago, decimal.Divide(porcentajeAhorros, 100));
                    //        pagoAhorro = Math.Round(pagoAhorro, 2, MidpointRounding.ToEven);
                    //        if (pagoAhorro > valorPago) { pagoAhorro = valorPago; }
                    //        valorPago -= pagoAhorro;
                    //    }

                    //    si existe sobrante debido a que no se completa el 100 % de porcentaje total se lo paga a aportes
                    //    if (valorPago > 0) { pagoAporte += valorPago; }



                    //}

                    //calculamos la fecha actual hora Colombia
                    DateTime fechaActual = Fecha.GetFechaColombia();
                    string fechaString = fechaActual.ToString("yyyy-MM-dd");

                    //información del cajero y caja
                    var InfoCajero = ctx.configCajero.Where(x => x.Nit_cajero == nit_cajero).FirstOrDefault();
                    var InfoCaja = ctx.Caja.Where(x => x.Codigo_caja == InfoCajero.Codigo_caja).FirstOrDefault();

                    //actualizamos consecutivo de caja y tope máximo
                    InfoCaja.consecutivo_actual++;
                    InfoCaja.TopeMaximo_caja += Convert.ToDouble(total);
                    if (InfoCaja.consecutivo_actual > InfoCaja.consecutivo_fin)
                    {
                        InfoCaja.consecutivo_actual = InfoCaja.Consecutivo_ini;
                        InfoCaja.Serie += 1;
                    }
                    ctx.Entry(InfoCaja).State = System.Data.Entity.EntityState.Modified;


                    //Actualizamos el cuadre de caja
                    var CuadreCaja = ctx.CuadreCajaPorCajero.Where(x => x.fecha == fechaString && x.codigo_caja == InfoCaja.Codigo_caja && x.nit_cajero == nit_cajero && x.cierre == 0).FirstOrDefault();
                    CuadreCaja.tope += (total);
                    CuadreCaja.consignacion_efectivo += (total);
                    ctx.Entry(CuadreCaja).State = System.Data.Entity.EntityState.Modified;

                    #region pago de aporte y cuentas de distribución

                    //realizamos el pago a la ficha de aporte del asociado
                    var TotalAnterior = Convert.ToDecimal(fichaAporteEX.totalAportesEx);
                    fichaAporteEX.totalAportesEx = Convert.ToInt64(TotalAnterior + total);
                    ctx.Entry(fichaAporteEX).State = System.Data.Entity.EntityState.Modified;

                    //construimos el comprobante
                    var tipoComprobante = ctx.TiposComprobantes.Where(x => x.CODIGO == InfoCajero.Compr_ingreso && x.INACTIVO == false).FirstOrDefault();
                    var comprobante = new Comprobante()
                    {
                        TIPO = InfoCajero.Compr_ingreso,
                        NUMERO = tipoComprobante.CONSECUTIVO,
                        ANO = fechaActual.Year.ToString(),
                        MES = fechaActual.Month.ToString(),
                        DIA = fechaActual.Day.ToString(),
                        CCOSTO = InfoCajero.centrocosto,
                        DETALLE = "CONSIGNACION APORTE EXTRAORDINARIO CAJA",
                        TERCERO = fichaAporteEX.idPersona,
                        CTAFPAGO = configuracionActualAporte.idCuenta,
                        VRTOTAL = total,
                        SUMDBCR = 0,
                        FECHARealiz = fechaActual,
                        ANULADO = false
                    };

                    listComprobantes.Add(comprobante);

                    //creamos los movimientos
                    var mov1 = new Movimiento() // movimiento que entra a caja
                    {
                        TIPO = InfoCajero.Compr_ingreso,
                        NUMERO = tipoComprobante.CONSECUTIVO,
                        CUENTA = InfoCajero.Cta_efectivo,
                        TERCERO = fichaAporteEX.idPersona,
                        DETALLE = "CONSIGNACION APORTE EXTRAORDINARIO CAJA",
                        DEBITO = total,
                        CREDITO = 0,
                        BASE = 0,
                        CCOSTO = InfoCajero.centrocosto,
                        FECHAMOVIMIENTO = fechaActual
                    };
                    listMovimientos.Add(mov1);

                    var mov2 = new Movimiento() // movimiento que entra a la cuenta configurada en aportes
                    {
                        TIPO = InfoCajero.Compr_ingreso,
                        NUMERO = tipoComprobante.CONSECUTIVO,
                        CUENTA = configuracionActualAporte.idCuenta,
                        TERCERO = fichaAporteEX.idPersona,
                        DETALLE = "CONSIGNACION APORTE EXTRAORDINARIO CAJA",
                        DEBITO = 0,
                        CREDITO = total,
                        BASE = 0,
                        CCOSTO = InfoCajero.centrocosto,
                        FECHAMOVIMIENTO = fechaActual
                    };
                    listMovimientos.Add(mov2);

                    //foreach (var item in cuentasDistribucion)
                    //{
                    //    var mov3 = new Movimiento()
                    //    {
                    //        TIPO = InfoCajero.Compr_ingreso,
                    //        NUMERO = tipoComprobante.CONSECUTIVO,
                    //        CUENTA = item.Cuenta,
                    //        TERCERO = fichaAporteEX.idPersona,
                    //        DETALLE = "CONSIGNACION APORTE EXTRAORDINARIO CAJA",
                    //        DEBITO = 0,
                    //        CREDITO = Math.Round(decimal.Multiply(AuxValorPago, decimal.Divide(item.Porcentaje, 100)), 0, MidpointRounding.ToEven),
                    //        BASE = 0,
                    //        CCOSTO = InfoCajero.centrocosto,
                    //        FECHAMOVIMIENTO = fechaActual
                    //    };
                    //    listMovimientos.Add(mov3);
                    //}

                    //creamos la factura de aporte
                    string numeroFactura = InfoCaja.agencia + "-" + InfoCaja.Codigo_caja + "-" + InfoCaja.Serie + "-" + InfoCaja.consecutivo_actual;

                    var facturaAporte = new FactOpcaja()
                    {

                        fecha = fechaActual,
                        factura = numeroFactura,
                        operacion = operacion,
                        codigo_caja = InfoCaja.Codigo_caja,
                        nit_cajero = nit_cajero,
                        numero_cuenta = fichaAporteEX.NumeroCuenta,
                        nit_propietario_cuenta = fichaAporteEX.idPersona,
                        valor_recibido = total,
                        valor_efectivo = total,
                        nombre_propietario_cuenta = DataTercero.NOMBRE1 + " " + DataTercero.APELLIDO1,
                        vueltas = 0,
                        IdProducto = 2,
                        observacion = observacion,
                        valor_cheque = 0,
                        consignacion = 0,
                        saldo_total_cuenta = saldo_total_cuenta,
                        total = saldo_total_cuenta,
                        valor_cheque1 = 0,
                        valor_cheque2 = 0,
                        valor_cheque3 = 0,
                        valor_cheque4 = 0,
                        valor_cheque5 = 0,
                        total_cheques = 0,
                        TIPO = InfoCajero.Compr_ingreso,
                        NUMERO = tipoComprobante.CONSECUTIVO
                    };
                    ctx.FactOpcaja.Add(facturaAporte);

                    //actualizamos el consecutivo del comprobante
                    tipoComprobante.CONSECUTIVO = (Convert.ToInt64(tipoComprobante.CONSECUTIVO) + 1).ToString();
                    ctx.Entry(tipoComprobante).State = System.Data.Entity.EntityState.Modified;
                    #endregion


                    //#region pago de ahorro

                    //if (pagoAhorro > 0)
                    //{
                    //    var configuracionActualAhorro = ctx.Configuracion.Where(x => x.activo == true).FirstOrDefault();

                    //    //actualizamos consecutivo de caja y tope máximo
                    //    InfoCaja.consecutivo_actual++;
                    //    InfoCaja.TopeMaximo_caja += Convert.ToDouble(pagoAhorro);
                    //    if (InfoCaja.consecutivo_actual > InfoCaja.consecutivo_fin)
                    //    {
                    //        InfoCaja.consecutivo_actual = InfoCaja.Consecutivo_ini;
                    //        InfoCaja.Serie += 1;
                    //    }
                    //    ctx.Entry(InfoCaja).State = System.Data.Entity.EntityState.Modified;


                    //    //Actualizamos el cuadre de caja
                    //    CuadreCaja.tope += (pagoAhorro);
                    //    CuadreCaja.consignacion_efectivo += (pagoAhorro);
                    //    ctx.Entry(CuadreCaja).State = System.Data.Entity.EntityState.Modified;

                    //    //realizamos el pago a la ficha de ahorro del asociado
                    //    fichaAhorro.totalAhorros = (Convert.ToDecimal(fichaAhorro.totalAhorros) + pagoAhorro).ToString();
                    //    ctx.Entry(fichaAhorro).State = System.Data.Entity.EntityState.Modified;

                    //    //construimos el comprobante
                    //    comprobante = new Comprobante()
                    //    {
                    //        TIPO = InfoCajero.Compr_ingreso,
                    //        NUMERO = tipoComprobante.CONSECUTIVO,
                    //        ANO = fechaActual.Year.ToString(),
                    //        MES = fechaActual.Month.ToString(),
                    //        DIA = fechaActual.Day.ToString(),
                    //        CCOSTO = InfoCajero.centrocosto,
                    //        DETALLE = "CONSIGNACION AHORRO CAJA",
                    //        TERCERO = fichaAhorro.idPersona,
                    //        CTAFPAGO = configuracionActualAhorro.cuentaContable.ToString(),
                    //        VRTOTAL = pagoAhorro,
                    //        SUMDBCR = 0,
                    //        FECHARealiz = fechaActual,
                    //        ANULADO = false
                    //    };

                    //    listComprobantes.Add(comprobante);

                    //    //creamos los movimientos
                    //    mov1 = new Movimiento() // movimiento que entra a caja
                    //    {
                    //        TIPO = InfoCajero.Compr_ingreso,
                    //        NUMERO = tipoComprobante.CONSECUTIVO,
                    //        CUENTA = InfoCajero.Cta_efectivo,
                    //        TERCERO = fichaAhorro.idPersona,
                    //        DETALLE = "CONSIGNACION AHORRO CAJA",
                    //        DEBITO = pagoAhorro,
                    //        CREDITO = 0,
                    //        BASE = 0,
                    //        CCOSTO = InfoCajero.centrocosto,
                    //        FECHAMOVIMIENTO = fechaActual
                    //    };
                    //    listMovimientos.Add(mov1);

                    //    mov2 = new Movimiento() // movimiento que entra a la cuenta configurada en ahorros
                    //    {
                    //        TIPO = InfoCajero.Compr_ingreso,
                    //        NUMERO = tipoComprobante.CONSECUTIVO,
                    //        CUENTA = configuracionActualAhorro.cuentaContable.ToString(),
                    //        TERCERO = fichaAhorro.idPersona,
                    //        DETALLE = "CONSIGNACION AHORRO CAJA",
                    //        DEBITO = 0,
                    //        CREDITO = pagoAhorro,
                    //        BASE = 0,
                    //        CCOSTO = InfoCajero.centrocosto,
                    //        FECHAMOVIMIENTO = fechaActual
                    //    };
                    //    listMovimientos.Add(mov2);

                    //    //creamos la factura de ahorro
                    //    numeroFactura = InfoCaja.agencia + "-" + InfoCaja.Codigo_caja + "-" + InfoCaja.Serie + "-" + InfoCaja.consecutivo_actual;
                    //    var facturaAhorro = new FactOpcaja()
                    //    {
                    //        fecha = fechaActual,
                    //        factura = numeroFactura,
                    //        operacion = "Consignación ahorro",
                    //        codigo_caja = InfoCaja.Codigo_caja,
                    //        nit_cajero = nit_cajero,
                    //        numero_cuenta = fichaAhorro.numeroCuenta,
                    //        nit_propietario_cuenta = fichaAhorro.idPersona,
                    //        valor_recibido = pagoAhorro,
                    //        valor_efectivo = pagoAhorro,
                    //        vueltas = 0,
                    //        valor_cheque = 0,
                    //        consignacion = 0,
                    //        saldo_total_cuenta = 0,
                    //        total = pagoAhorro,
                    //        valor_cheque1 = 0,
                    //        valor_cheque2 = 0,
                    //        valor_cheque3 = 0,
                    //        valor_cheque4 = 0,
                    //        valor_cheque5 = 0,
                    //        total_cheques = 0,
                    //        TIPO = InfoCajero.Compr_ingreso,
                    //        NUMERO = tipoComprobante.CONSECUTIVO 
                    //    };
                    //    ctx.FactOpcaja.Add(facturaAhorro);

                    //    //actualizamos el consecutivo del comprobante
                    //    tipoComprobante.CONSECUTIVO = (Convert.ToInt64(tipoComprobante.CONSECUTIVO) + 1).ToString();
                    //    ctx.Entry(tipoComprobante).State = System.Data.Entity.EntityState.Modified;
                    //}

                    //#endregion


                    //guardamos en memoria los comprobantes
                    ctx.Comprobantes.AddRange(listComprobantes);
                    //guardamos en memoria los movimientos
                    ctx.Movimientos.AddRange(listMovimientos);

                    //guardamos los cambios en base de datos
                    ctx.SaveChanges();

                    respuesta.Id = facturaAporte.id;
                    //respuesta.PagoCuentas = pagoCuentas;
                    //respuesta.CuentasDistribucion = cuentasDistribucion;
                    return respuesta;
                }
                catch (Exception ex)
                {
                    string mensaje = "Ha ocurrido un error internamente";
                    respuesta.Status = false;
                    respuesta.Mensaje = mensaje;
                    return respuesta;
                }

            }

        }
        #endregion



        #region AHORRO CONTRACTUAL
        public async Task<ViewModelInfoCajaAC> GetModelInfoCajaAC(string NumeroCuenta,string IdCajero)
        {
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            var model = new ViewModelInfoCajaAC();
            try
            {
                using (var ctx = new AccountingContext())
                {
                    var infoCajero = await ctx.configCajero.Where(x => x.Nit_cajero == IdCajero).FirstOrDefaultAsync();  
                    var infoFichaAC = await ctx.FichasAhorroContractual.Where(x =>x.NumeroCuenta == NumeroCuenta).FirstOrDefaultAsync();
                    string nombreCajero = infoCajero.Terceros.NOMBRE1 + " " + infoCajero.Terceros.NOMBRE2 + " " + infoCajero.Terceros.APELLIDO1 + " " + infoCajero.Terceros.APELLIDO2;
                    string nombreUsuario = infoFichaAC.TerceroFK.NOMBRE1 + " " + infoFichaAC.TerceroFK.NOMBRE2 + " " + infoFichaAC.TerceroFK.APELLIDO1 + " " + infoFichaAC.TerceroFK.APELLIDO2;
                    string valorCuota = infoFichaAC.ValorCuota.ToString("N0",formato);
                    string saldoActual = (infoFichaAC.TotalAhorro).ToString("N0",formato);

                    model = new ViewModelInfoCajaAC().GetViewModelInfoCajaAC(infoCajero.Codigo_caja,infoCajero.Caja.Nombre_caja,IdCajero,nombreCajero,infoFichaAC.IdAsociado,nombreUsuario,NumeroCuenta,valorCuota,saldoActual);
                    
                }
            }
            catch (Exception ex)
            { 
            }
            return model;
        }

        public async Task<RespuestaAhorro> PagarAhorroAC(string cuenta, string cajero, decimal valor,string observacion)
        {
            var guid = Guid.NewGuid().ToString();

            var respuesta = new RespuestaAhorro { Status = true };
            var listComprobantes = new List<Comprobante>();
            var listMovimientos = new List<Movimiento>();
            using (var ctx = new AccountingContext())
            {
                try
                {
                    var fichaAhorro = await ctx.FichasAhorroContractual.Where(x => x.NumeroCuenta == cuenta).FirstOrDefaultAsync();
                    fichaAhorro.SetearCamposNoMapeados(); //no borrar esta línea: agrega un valor por defecto a campos no mapeados pero obligatorios en formulario

                    //Se realiza el pago de ahorro contractual
                    fichaAhorro.TotalAhorro += valor;
                    ctx.Entry(fichaAhorro).State = System.Data.Entity.EntityState.Modified;

                    //calculamos la fecha actual hora Colombia
                    DateTime fechaActual = Fecha.GetFechaColombia();
                    string fechaString = fechaActual.ToString("yyyy-MM-dd");

                    //información del cajero y caja
                    var InfoCajero = ctx.configCajero.Where(x => x.Nit_cajero == cajero).FirstOrDefault();
                    var InfoCaja = ctx.Caja.Where(x => x.Codigo_caja == InfoCajero.Codigo_caja).FirstOrDefault();

                    //actualizamos consecutivo de caja y tope máximo
                    InfoCaja.consecutivo_actual++;
                    InfoCaja.TopeMaximo_caja += Convert.ToDouble(valor);
                    if (InfoCaja.consecutivo_actual > InfoCaja.consecutivo_fin)
                    {
                        InfoCaja.consecutivo_actual = InfoCaja.Consecutivo_ini;
                        InfoCaja.Serie += 1;
                    }
                    ctx.Entry(InfoCaja).State = System.Data.Entity.EntityState.Modified;


                    //Actualizamos el cuadre de caja
                    var CuadreCaja = ctx.CuadreCajaPorCajero.Where(x => x.fecha == fechaString && x.codigo_caja == InfoCaja.Codigo_caja && x.nit_cajero == cajero && x.cierre == 0).FirstOrDefault();
                    CuadreCaja.tope += (valor);
                    CuadreCaja.consignacion_efectivo += (valor);
                    ctx.Entry(CuadreCaja).State = System.Data.Entity.EntityState.Modified;

                    


                    //construimos el comprobante
                    var tipoComprobante = ctx.TiposComprobantes.Where(x => x.CODIGO == fichaAhorro.ConfACFK.IdComprobante && x.INACTIVO == false).FirstOrDefault();
                    var comprobante = new Comprobante()
                    {
                        TIPO = tipoComprobante.CODIGO,
                        NUMERO = tipoComprobante.CONSECUTIVO,
                        ANO = fechaActual.Year.ToString(),
                        MES = fechaActual.Month.ToString(),
                        DIA = fechaActual.Day.ToString(),
                        CCOSTO = InfoCajero.centrocosto,
                        DETALLE = "CONSIGNACION AHORRO CONTRACTUAL CAJA",
                        TERCERO = fichaAhorro.IdAsociado,
                        CTAFPAGO = fichaAhorro.ConfACFK.IdCuenta,
                        VRTOTAL = valor,
                        SUMDBCR = 0,
                        FECHARealiz = fechaActual,
                        ANULADO = false
                    };

                    listComprobantes.Add(comprobante);

                    //creamos los movimientos
                    var mov1 = new Movimiento() // movimiento que entra a caja
                    {
                        TIPO = tipoComprobante.CODIGO,
                        NUMERO = tipoComprobante.CONSECUTIVO,
                        CUENTA = InfoCajero.Cta_efectivo,
                        TERCERO = fichaAhorro.IdAsociado,
                        DETALLE = "CONSIGNACION AHORRO CONTRACTUAL CAJA",
                        DEBITO = valor,
                        CREDITO = 0,
                        BASE = 0,
                        CCOSTO = InfoCajero.centrocosto,
                        FECHAMOVIMIENTO = fechaActual
                    };
                    listMovimientos.Add(mov1);

                    var mov2 = new Movimiento() // movimiento que entra a la cuenta configurada en la ficha ahorro contractual
                    {
                        TIPO = tipoComprobante.CODIGO,
                        NUMERO = tipoComprobante.CONSECUTIVO,
                        CUENTA = fichaAhorro.ConfACFK.IdCuenta,
                        TERCERO = fichaAhorro.IdAsociado,
                        DETALLE = "CONSIGNACION AHORRO CONTRACTUAL CAJA",
                        DEBITO = 0,
                        CREDITO = valor,
                        BASE = 0,
                        CCOSTO = InfoCajero.centrocosto,
                        FECHAMOVIMIENTO = fechaActual
                    };
                    listMovimientos.Add(mov2);

                    

                    //creamos la factura de aporte
                    string numeroFactura = InfoCaja.agencia + "-" + InfoCaja.Codigo_caja + "-" + InfoCaja.Serie + "-" + InfoCaja.consecutivo_actual;
                    var facturaAhorroAC = new FactOpcaja()
                    {
                        fecha = fechaActual,
                        factura = numeroFactura,
                        operacion = "Consignación",
                        codigo_caja = InfoCaja.Codigo_caja,
                        nit_cajero = cajero,
                        numero_cuenta = fichaAhorro.NumeroCuenta,
                        IdProducto = 4,//ahorro contractual
                        nit_propietario_cuenta = fichaAhorro.IdAsociado,
                        valor_recibido = valor,
                        valor_efectivo = valor,
                        vueltas = 0,
                        valor_cheque = 0,
                        consignacion = 0,
                        saldo_total_cuenta = 0,
                        total = valor,
                        valor_cheque1 = 0,
                        valor_cheque2 = 0,
                        valor_cheque3 = 0,
                        valor_cheque4 = 0,
                        valor_cheque5 = 0,
                        total_cheques = 0,
                        observacion = observacion,
                        TIPO = tipoComprobante.CODIGO,
                        NUMERO = tipoComprobante.CONSECUTIVO
                    };
                    ctx.FactOpcaja.Add(facturaAhorroAC);

                    //actualizamos el consecutivo del comprobante
                    tipoComprobante.CONSECUTIVO = (Convert.ToInt64(tipoComprobante.CONSECUTIVO) + 1).ToString();
                    ctx.Entry(tipoComprobante).State = System.Data.Entity.EntityState.Modified;
                    


                    //guardamos en memoria los comprobantes
                    ctx.Comprobantes.AddRange(listComprobantes);
                    //guardamos en memoria los movimientos
                    ctx.Movimientos.AddRange(listMovimientos);

                    //guardamos los cambios en base de datos
                    await ctx.SaveChangesAsync();

                    respuesta.Id = facturaAhorroAC.id;
                    return respuesta;
                }
                catch (Exception ex)
                {
                    string mensaje = "Ha ocurrido un error internamente";
                    respuesta.Status = false;
                    respuesta.Mensaje = mensaje;
                    return respuesta;
                }

            }

        }
        #endregion
    }
}
