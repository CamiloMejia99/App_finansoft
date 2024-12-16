using FNTC.Finansoft.Accounting.DAL.Creditos;
using FNTC.Finansoft.Accounting.DAL.Tools;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Ahorros;
using FNTC.Finansoft.Accounting.DTO.Aportes;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using FNTC.Finansoft.Accounting.DTO.Respuesta;
using FNTC.Finansoft.DAL.Ahorros;
using FNTC.Finansoft.DAL.Aportes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FNTC.Finansoft.Accounting.DAL
{
    public class MovimientosDAL
    {
        public List<Movimiento> GetMovimientos(string tipo, string numero)
        {
            var list = new List<Movimiento>();
            try
            {
                using (var ctx = new AccountingContext())
                {
                    list = ctx.Movimientos.Where(x => x.TIPO == tipo && x.NUMERO == numero).ToList();
                }   
            }
            catch (System.Exception ex)
            {
            }
            return list;
        }


        public async Task<JsonResult> GetProductosAsociadoAsync(string NIT, string cuenta)
        {
            int n = cuenta.Length;
            cuenta = cuenta.Substring(n - 9);
            n = Convert.ToInt32(cuenta);
            var cuentas = new List<Array>();
            var respuesta = await GetProductoByCuentaAsync(cuenta);
            if (respuesta.Correcto)
            {
                switch (respuesta.IdProducto)
                {
                    case 1://caso si es cuenta de aporte ordinario
                        var fichasAporte = await new DALAportes().GetFichasAporteByNitAsync(NIT);
                        foreach (var item in fichasAporte)
                        {
                            string[] array = new string[2];
                            array[0] = item.numeroCuenta;
                            array[1] = new NumberFormat(Convert.ToDecimal(item.totalAportes),0).GetFormatNumber();
                            cuentas.Add(array);
                        }
                        break;
                    case 2://caso si es cuenta de aporte extraordinario
                        var fichasAporteExtra = await new DALAportes().GetFichasAporteExtraByNitAsync(NIT);
                        foreach (var item in fichasAporteExtra)
                        {
                            string[] array = new string[2];
                            array[0] = item.NumeroCuenta;
                            array[1] = new NumberFormat(item.totalAportesEx, 0).GetFormatNumber();
                            cuentas.Add(array);
                        }
                        break;
                    case 3://caso si es cuenta de ahorro permanente
                        var fichasAhorro = await new DALAhorros().GetFichasAhorroByNitAsync(NIT);
                        foreach (var item in fichasAhorro)
                        {
                            string[] array = new string[2];
                            array[0] = item.numeroCuenta;
                            array[1] = new NumberFormat(Convert.ToDecimal(item.totalAhorros), 0).GetFormatNumber();
                            cuentas.Add(array);
                        }
                        break;
                    case 4://casi si es cuenta de ahorro contractual
                        var fichasAhorroC = await new DALAhorros().GetFichasAhorroContractualByNitAsync(NIT, cuenta);
                        foreach (var item in fichasAhorroC)
                        {
                            string[] array = new string[2];
                            array[0] = item.NumeroCuenta;
                            array[1] = new NumberFormat(item.TotalAhorro, 0).GetFormatNumber();
                            cuentas.Add(array);
                        }
                        break;
                    case 5://si es cuenta de créditos
                        var creditos = await new ProcesosCrediticiosDAL().GetTotalesByNitCuentaAsync(NIT,cuenta);
                        foreach (var item in creditos)
                        {
                            string[] array = new string[2];
                            array[0] = item.Pagare;
                            array[1] = new NumberFormat(item.SaldoCapital+item.InteresCorrientePendiente+item.InteresMoraPendiente+item.SeguroPendiente+item.CtoAdmonPendiente, 0).GetFormatNumber();
                            cuentas.Add(array);
                        }
                        break;
                    default:
                        break;
                }
            }


            return new JsonResult { Data = new { result = cuentas} };
        }

        public async Task<DTORespuestaCuentasByProducto> GetProductoByCuentaAsync(string cuenta)
        { 
            var respuesta = new DTORespuestaCuentasByProducto().GenerarRespuestaIncorrecta();
            int IdProducto = 0;
            bool existeCuenta=false;

            using (var ctx = new AccountingContext())
            {
                try
                 {
                    //verificamos si la cuenta está en configuracíon de aporte ordinario
                    existeCuenta = await ctx.Configuracion1.Where(x =>x.idCuenta == cuenta && x.activo == true).AnyAsync();
                    if (existeCuenta)
                        IdProducto = 1;

                    //verificamos si la cuenta está en configuracíon de aporte extra ordinario
                    if (!existeCuenta)
                    {
                        existeCuenta = await ctx.Configuracion2Ex.Where(x => x.idCuenta == cuenta && x.estado == true).AnyAsync();
                        if (existeCuenta) IdProducto = 2;
                    }

                    //verificamos si la cuenta está en configuracíon de ahorro permanente
                    if (!existeCuenta)
                    {
                        Int64 cuentaAux = Convert.ToInt64(cuenta);
                        existeCuenta = await ctx.Configuracion.Where(x => x.cuentaContable == cuentaAux && x.activo == true).AnyAsync();
                        if (existeCuenta) IdProducto = 3;
                    }

                    //verificamos si la cuenta está en configuracíon de ahorro contractual
                    if (!existeCuenta)
                    {
                        existeCuenta = await ctx.ConfigAhorrosContractuales.Where(x => x.IdCuenta == cuenta && x.Estado == true).AnyAsync();
                        if (existeCuenta) IdProducto = 4;
                    }

                    //verificamos si la cuenta está en configuracíon de créditos
                    if (!existeCuenta)
                    {
                        existeCuenta = await ctx.Cuentas.Where(x => x.Cuenta_Cod == cuenta && x.Funcion == "F1").AnyAsync();
                        if (existeCuenta) IdProducto = 5;
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
            if (existeCuenta)
                respuesta=respuesta.GenerarRespuestaCorrecta(IdProducto);

            return respuesta;   
        }


        #region PAGOS POR MOVIMIENTOS
        public async Task<ViewModelAporteByMovimiento> PagarAporteByMovimientosAsync(string cuenta, decimal valor,DateTime fecha,string tipoComprobante,string numeroComprobante,string ccosto)
        {
            var model = new ViewModelAporteByMovimiento();

            using (var ctx = new AccountingContext())
            {
                try
                {
                    var fichaAporte = await ctx.FichasAportes.Where(x => x.numeroCuenta == cuenta).FirstOrDefaultAsync();
                    var configuracion =await ctx.Configuracion1.Where(x =>x.activo==true).FirstOrDefaultAsync();

                    //realizamos el pago a la ficha de aporte del asociado
                    fichaAporte.totalAportes = (Convert.ToDecimal(fichaAporte.totalAportes) + Convert.ToInt64(valor)).ToString();

                    var mov = new Movimiento() // movimiento que entra a la cuenta configurada en aportes
                    {
                        TIPO = tipoComprobante,
                        NUMERO = numeroComprobante,
                        CUENTA = configuracion.idCuenta,
                        TERCERO = fichaAporte.idPersona,
                        DETALLE = "CONSIGNACION APORTE",
                        DEBITO = 0,
                        CREDITO = valor,
                        BASE = 0,
                        CCOSTO = ccosto,
                        FECHAMOVIMIENTO = fecha
                    };
                    
                    //creamos la factura de aporte
                    string numeroFactura = "1-" + tipoComprobante + "-" + numeroComprobante + "-" + ccosto;
                    var facturaAporte = new FactOpcaja()
                    {
                        fecha = fecha,
                        factura = numeroFactura,
                        operacion = "Consignacion Movimiento",
                        codigo_caja = "1",
                        nit_cajero = "1",
                        numero_cuenta = fichaAporte.numeroCuenta,
                        IdProducto = 1,//aporte ordinario
                        nit_propietario_cuenta = fichaAporte.idPersona,
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
                        TIPO = tipoComprobante,
                        NUMERO = numeroComprobante
                    };

                    model.Movimiento = mov;
                    model.Factura = facturaAporte;
                    model.FichaAporte = fichaAporte;    
                    model.Correcto = true;
                }
                catch (Exception ex)
                {
                    model.Correcto = false;
                }

            }
            return model;
        }

        public async Task<ViewModelAhorroByMovimiento> PagarAhorroByMovimientosAsync(string cuenta, decimal valor, DateTime fecha, string tipoComprobante, string numeroComprobante, string ccosto)
        {
            var model = new ViewModelAhorroByMovimiento();

            using (var ctx = new AccountingContext())
            {
                try
                {
                    var fichaAhorro = await ctx.FichasAhorros.Where(x => x.numeroCuenta == cuenta).FirstOrDefaultAsync();
                    var configuracion = await ctx.Configuracion.Where(x => x.activo == true).FirstOrDefaultAsync();

                    //realizamos el pago a la ficha de aporte del asociado
                    fichaAhorro.totalAhorros = (Convert.ToDecimal(fichaAhorro.totalAhorros) + Convert.ToInt64(valor)).ToString();

                    var mov = new Movimiento() // movimiento que entra a la cuenta configurada en aportes
                    {
                        TIPO = tipoComprobante,
                        NUMERO = numeroComprobante,
                        CUENTA = configuracion.cuentaContable.ToString(),
                        TERCERO = fichaAhorro.idPersona,
                        DETALLE = "CONSIGNACION AHORRO",
                        DEBITO = 0,
                        CREDITO = valor,
                        BASE = 0,
                        CCOSTO = ccosto,
                        FECHAMOVIMIENTO = fecha
                    };

                    //creamos la factura de aporte
                    string numeroFactura = "1-" + tipoComprobante + "-" + numeroComprobante + "-" + ccosto;
                    var facturaAhorro = new FactOpcaja()
                    {
                        fecha = fecha,
                        factura = numeroFactura,
                        operacion = "Consignacion Movimiento",
                        codigo_caja = "1",
                        nit_cajero = "1",
                        numero_cuenta = fichaAhorro.numeroCuenta,
                        IdProducto = 3,//ahorro permanente
                        nit_propietario_cuenta = fichaAhorro.idPersona,
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
                        TIPO = tipoComprobante,
                        NUMERO = numeroComprobante
                    };

                    model.Movimiento = mov;
                    model.Factura = facturaAhorro;
                    model.FichaAhorro = fichaAhorro;
                    model.Correcto = true;
                }
                catch (Exception ex)
                {
                    model.Correcto = false;
                }

            }
            return model;
        }

        public async Task<ViewModelAporteExtraByMovimiento> PagarAporteExtraByMovimientosAsync(string cuenta, decimal valor, DateTime fecha, string tipoComprobante, string numeroComprobante, string ccosto)
        {
            var model = new ViewModelAporteExtraByMovimiento();

            using (var ctx = new AccountingContext())
            {
                try
                {
                    var fichaAporteExtra = await ctx.FichaAfiliadosAporteEx.Where(x => x.NumeroCuenta == cuenta).FirstOrDefaultAsync();
                    var configuracion = await ctx.Configuracion2Ex.Where(x => x.estado == true).FirstOrDefaultAsync();

                    //realizamos el pago a la ficha de aporte del asociado
                    fichaAporteExtra.totalAportesEx +=Convert.ToInt64(valor);

                    var mov = new Movimiento() 
                    {
                        TIPO = tipoComprobante,
                        NUMERO = numeroComprobante,
                        CUENTA = configuracion.idCuenta,
                        TERCERO = fichaAporteExtra.idPersona,
                        DETALLE = "CONSIGNACIÓN APORTE EXTRAORDINARIO",
                        DEBITO = 0,
                        CREDITO = valor,
                        BASE = 0,
                        CCOSTO = ccosto,
                        FECHAMOVIMIENTO = fecha
                    };

                    //creamos la factura de aporte
                    string numeroFactura = "1-" + tipoComprobante + "-" + numeroComprobante + "-" + ccosto;
                    var facturaAhorro = new FactOpcaja()
                    {
                        fecha = fecha,
                        factura = numeroFactura,
                        operacion = "Consignacion Movimiento",
                        codigo_caja = "1",
                        nit_cajero = "1",
                        numero_cuenta = fichaAporteExtra.NumeroCuenta,
                        IdProducto = 2,//aporte extraordinario
                        nit_propietario_cuenta = fichaAporteExtra.idPersona,
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
                        TIPO = tipoComprobante,
                        NUMERO = numeroComprobante
                    };

                    model.Movimiento = mov;
                    model.Factura = facturaAhorro;
                    model.FichaAporteExtra = fichaAporteExtra;
                    model.Correcto = true;
                }
                catch (Exception ex)
                {
                    model.Correcto = false;
                }

            }
            return model;
        }
        public async Task<ViewModelAhorroCByMovimiento> PagarAhorroCByMovimientosAsync(string cuenta, decimal valor, DateTime fecha, string tipoComprobante, string numeroComprobante, string ccosto)
        {
            var model = new ViewModelAhorroCByMovimiento();

            using (var ctx = new AccountingContext())
            {
                try
                {
                    var fichaAhorroC = await ctx.FichasAhorroContractual.Where(x => x.NumeroCuenta == cuenta).Include(x=>x.ConfACFK).FirstOrDefaultAsync();
                    fichaAhorroC.SetearCamposNoMapeados();//línea obligatoria: Se produce un error por campos no nulos no mapeados
                    //realizamos el pago a la ficha de ahorro contractual
                    fichaAhorroC.TotalAhorro +=valor;

                    var mov = new Movimiento()
                    {
                        TIPO = tipoComprobante,
                        NUMERO = numeroComprobante,
                        CUENTA = fichaAhorroC.ConfACFK.IdCuenta,
                        TERCERO = fichaAhorroC.IdAsociado,
                        DETALLE = "CONSIGNACIÓN AHORRO CONTRACTUAL",
                        DEBITO = 0,
                        CREDITO = valor,
                        BASE = 0,
                        CCOSTO = ccosto,
                        FECHAMOVIMIENTO = fecha
                    };

                    //creamos la factura de aporte
                    string numeroFactura = "1-" + tipoComprobante + "-" + numeroComprobante + "-" + ccosto;
                    var facturaAhorroC = new FactOpcaja()
                    {
                        fecha = fecha,
                        factura = numeroFactura,
                        operacion = "Consignacion Movimiento",
                        codigo_caja = "1",
                        nit_cajero = "1",
                        numero_cuenta = fichaAhorroC.NumeroCuenta,
                        IdProducto = 4,//ahorro extraordinario
                        nit_propietario_cuenta = fichaAhorroC.IdAsociado,
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
                        TIPO = tipoComprobante,
                        NUMERO = numeroComprobante
                    };

                    model.Movimiento = mov;
                    model.Factura = facturaAhorroC;
                    model.FichaAhorroC = fichaAhorroC;
                    model.Correcto = true;
                }
                catch (Exception ex)
                {
                    model.Correcto = false;
                }

            }
            return model;
        }
        #endregion
    }
}
