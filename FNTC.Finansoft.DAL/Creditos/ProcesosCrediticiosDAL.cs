using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using FNTC.Finansoft.Accounting.DTO.Parametros;
using FNTC.Finansoft.Accounting.DTO.ViewModels.Creditos;
using FNTC.Finansoft.UI.Tools;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FNTC.Finansoft.Accounting.DAL.Creditos
{
    [Authorize]
    public class ProcesosCrediticiosDAL
    {
        NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;
        public bool RealizarCausacion()
        {
            //Los estados para los créditos son los siguientes:
            //AD: estado cuota al dia
            //EM: estado cuota en mora
            //DT: estado cuota cuando llega a 30 dias de causacion pero aun no se cumple la fecha de pago
            //PZ: estado cuota a paz y salvo (cuando la cuota ya se encuentra pagada)
            //LQ: estado credito liquidado(sólo sirve para la tabla TotalesCreditos)
            using (var ctx = new DTO.AccountingContext())
            {
                try
                {
                    var DatosCreditos = ctx.Creditos.ToList();
                    var Creditos = ctx.TotalesCreditos.Where(x => x.Estado != "LQ").ToList();
                    var CuentasContables = ctx.Cuentas.ToList();
                    decimal SaldoCapital = 0, InteresCorrienteTotalPendiente = 0, InteresMoraTotalPendiente = 0, SeguroPendiente = 0, PorcentajeIC = 0, PorcentajeIM = 0;
                    decimal ValorInteresDia = 0;
                    decimal ValorSeguro = 0;
                    decimal ValorCtoAdmon = 0;
                    decimal ValorInteresMoraDia = 0;
                    string EstadoCuotaActual = "";
                    int MaxDiasMora = 0;
                    bool Continuar = true;
                    DateTime FechaActual = Fecha.GetFechaColombia();
                    FechaActual = new DateTime(FechaActual.Year, FechaActual.Month, FechaActual.Day);
                    

                    //información de los comprobantes
                    var InfoComprobanteMora = ctx.TiposComprobantes.Where(x => x.CODIGO == "CI4").FirstOrDefault();
                    var InfoComprobanteCorriente = ctx.TiposComprobantes.Where(x => x.CODIGO == "CI3").FirstOrDefault();

                    foreach (var item in Creditos)
                    {
                        var InfoCredito = DatosCreditos.Where(x => x.Pagare == item.Pagare).FirstOrDefault();
                        var cuentas = CuentasContables.Where(x => x.Destino_Id == InfoCredito.Destino_Id).ToList();
                        int Periodo = Convert.ToInt32(InfoCredito.prestamoFK.Tipo_Periodo.Tipo_Periodo_Valor)/30;
                        DateTime FechaCondicional = FechaActual.AddMonths(Periodo);
                        decimal TotalInteresCorriente = 0, TotalInteresMora = 0;
                        SaldoCapital = 0; InteresCorrienteTotalPendiente = 0; InteresMoraTotalPendiente = 0; SeguroPendiente = 0; PorcentajeIC = 0; PorcentajeIM = 0;
                        ValorInteresDia = 0;
                        ValorInteresMoraDia = 0;
                        MaxDiasMora = 0;
                        Continuar = true;
                        
                        PorcentajeIC = InfoCredito.Creditos_Interes*Periodo;
                        PorcentajeIM = InfoCredito.Creditos_Interes_Mora;
                        ValorSeguro = InfoCredito.prestamoFK.ValorSeguro;
                        ValorCtoAdmon = InfoCredito.prestamoFK.CtoAdmon;
                        var DataControl = ctx.ControlCreditos.Where(x => x.Pagare == item.Pagare && x.FechaPago <= FechaCondicional && x.EstadoEnCredito != "PZ").OrderBy(x => x.Id).ToList();

                        foreach (var Data in DataControl)
                        {
                            Continuar = true;
                            //condicional cuando la cuota llega a proceso de causacion
                            if (!Data.EstadoEnOperacion)
                            {
                                Data.EstadoEnOperacion = true;
                                Data.Seguro = ValorSeguro;
                                Data.ValorCuota += ValorSeguro;

                                //se agrega costo de administración
                                Data.CtoAdmon = ValorCtoAdmon;
                                Data.ValorCuota += ValorCtoAdmon;
                                

                                //calculamos el interes
                                ValorInteresDia = ((PorcentajeIC / 100) * Data.SaldoCapitalEnCuota);
                                ValorInteresDia = Math.Round(ValorInteresDia, 0, MidpointRounding.ToEven);
                                Data.InteresCorriente += ValorInteresDia;
                                Data.ValorCuota += ValorInteresDia;


                                item.SeguroPendiente += ValorSeguro;
                                item.CtoAdmonPendiente += ValorCtoAdmon;
                                item.InteresCorrientePendiente += ValorInteresDia;

                                TotalInteresCorriente += ValorInteresDia;

                                if (Data.FechaPago == FechaCondicional) { Continuar = false; }
                            }

                            if (Continuar)
                            {
                                Data.DiasCausados++;
                                ValorInteresDia = ((PorcentajeIC / 100) * Data.SaldoCapitalEnCuota) / 30;
                                ValorInteresDia = Math.Round(ValorInteresDia, 0, MidpointRounding.ToEven);

                                EstadoCuotaActual = Data.EstadoEnCredito;

                                switch (EstadoCuotaActual)
                                {
                                    case "AD":
                                        #region caso1: cuando la cuota está al dia
                                        if (FechaActual <= Data.FechaPago)
                                        {
                                            if (Data.DiasCausados <= 30)
                                            {
                                                
                                            }
                                            else
                                            {
                                                Data.EstadoEnCredito = "DT";
                                                Data.DiasCausados--;
                                            }
                                             
                                        }
                                        else
                                        {
                                            if (Data.DiasCausados > 30)
                                            {
                                                Data.EstadoEnCredito = "EM";
                                                Data.DiasMora++;
                                                ValorInteresMoraDia = (((Data.Capital * (PorcentajeIM / 100)) / 30) * Data.DiasMora);
                                                ValorInteresMoraDia = Math.Round(ValorInteresMoraDia, 0, MidpointRounding.ToEven);
                                                //Data.InteresMora += ValorInteresMoraDia;
                                                //Data.ValorCuota += ValorInteresMoraDia;

                                                //item.InteresMoraPendiente += ValorInteresMoraDia;
                                                item.CapitalMoraPendiente += Data.Capital;
                                                item.Estado = "EM";

                                                TotalInteresMora += ValorInteresMoraDia;

                                                if (Data.DiasMora > MaxDiasMora) { MaxDiasMora = Data.DiasMora; }
                                            }
                                        }
                                        #endregion
                                        break;
                                    case "EM":
                                        #region caso2: Cuando la cuota está en mora
                                        Data.DiasMora++;
                                        if (Data.DiasMora < 0)
                                        {
                                            ValorInteresMoraDia = (((Data.Capital * (PorcentajeIM / 100)) / 30));
                                            ValorInteresMoraDia = Math.Round(ValorInteresMoraDia, 0, MidpointRounding.ToEven);
                                            Data.InteresMora += ValorInteresMoraDia;
                                            Data.ValorCuota += ValorInteresMoraDia;

                                            item.InteresMoraPendiente += ValorInteresMoraDia;

                                            TotalInteresMora += ValorInteresMoraDia;
                                        }


                                        if (Data.DiasMora > MaxDiasMora) { MaxDiasMora = Data.DiasMora; }
                                        #endregion
                                        break;
                                    case "DT":
                                        #region caso3: Cuando la cuota ya cumplio 30 de causacion pero no ha vencido
                                        if (FechaActual > Data.FechaPago)
                                        {
                                            Data.EstadoEnCredito = "EM";
                                            Data.DiasMora++;
                                            ValorInteresMoraDia = (((Data.Capital * (PorcentajeIM / 100)) / 30) * Data.DiasMora);
                                            ValorInteresMoraDia = Math.Round(ValorInteresMoraDia, 0, MidpointRounding.ToEven);
                                            //Data.InteresMora += ValorInteresMoraDia;
                                            //Data.ValorCuota += ValorInteresMoraDia;

                                            //item.InteresMoraPendiente += ValorInteresMoraDia;
                                            item.CapitalMoraPendiente += Data.Capital;
                                            item.Estado = "EM";

                                            TotalInteresMora += ValorInteresMoraDia;

                                            if (Data.DiasMora > MaxDiasMora) { MaxDiasMora = Data.DiasMora; }
                                        }
                                        #endregion
                                        break;
                                    default:
                                        break;
                                }


                                //guardamos cambios en memoria
                                ctx.Entry(Data).State = System.Data.Entity.EntityState.Modified;
                            }


                        }//fin foreach DataControl

                        item.DiasMora = MaxDiasMora;

                        //construimos el comprobante y los movimientos
                        if (TotalInteresMora > 0)
                        {
                            InfoComprobanteMora.CONSECUTIVO = (Convert.ToInt64(InfoComprobanteMora.CONSECUTIVO) + 1).ToString();
                            var comprobanteIntMora = new Comprobante()
                            {
                                TIPO = cuentas.Where(x => x.Funcion == "F13").Select(x => x.TipoComprobante).FirstOrDefault(),
                                NUMERO = InfoComprobanteMora.CONSECUTIVO,
                                ANO = Convert.ToString(FechaActual.Year),
                                MES = Convert.ToString(FechaActual.Month),
                                DIA = Convert.ToString(FechaActual.Day),
                                CCOSTO = "00",
                                DETALLE = "CAUSACION CREDITOS INTERES MORA",
                                TERCERO = InfoCredito.Creditos_Cedula,
                                CTAFPAGO = cuentas.Where(x => x.Funcion == "F13").Select(x => x.Cuenta_Cod).FirstOrDefault(),//CAMBIAR CUENTA PARA INTERES DE MORA
                                VRTOTAL = TotalInteresMora,
                                SUMDBCR = 0,
                                FECHARealiz = FechaActual,
                                ANULADO = false
                            };
                            ctx.Comprobantes.Add(comprobanteIntMora);

                            var mov1 = new Movimiento()
                            {
                                TIPO = cuentas.Where(x => x.Funcion == "F13").Select(x => x.TipoComprobante).FirstOrDefault(),
                                NUMERO = InfoComprobanteMora.CONSECUTIVO,
                                CUENTA = cuentas.Where(x => x.Funcion == "F13").Select(x => x.Cuenta_Cod).FirstOrDefault(),
                                TERCERO = InfoCredito.Creditos_Cedula,
                                DETALLE = "CAUSACION CREDITOS INTERES MORA",
                                DEBITO = TotalInteresMora,
                                CREDITO = 0,
                                BASE = 0,
                                CCOSTO = "00",
                                FECHAMOVIMIENTO = FechaActual,
                            };
                            ctx.Movimientos.Add(mov1);

                            var mov2 = new Movimiento()
                            {
                                TIPO = cuentas.Where(x => x.Funcion == "F5").Select(x => x.TipoComprobante).FirstOrDefault(),
                                NUMERO = InfoComprobanteMora.CONSECUTIVO,
                                CUENTA = cuentas.Where(x => x.Funcion == "F5").Select(x => x.Cuenta_Cod).FirstOrDefault(),
                                TERCERO = InfoCredito.Creditos_Cedula,
                                DETALLE = "CAUSACION CREDITOS INTERES MORA",
                                DEBITO = 0,
                                CREDITO = TotalInteresMora,
                                BASE = 0,
                                CCOSTO = "00",
                                FECHAMOVIMIENTO = FechaActual,
                            };
                            ctx.Movimientos.Add(mov2);

                            var causador = new interescausadoprestamos()
                            {
                                pagare = item.Pagare,
                                intcorriente = TotalInteresCorriente,
                                intmora = TotalInteresMora,
                                fechasistema = FechaActual,
                                agenciaId = 1,
                                usuarioCauso = "AUTOMATICO",
                                Tasainteres = PorcentajeIM,
                                numeroCuota = 0,
                                comprabante = "CI4",
                                consecutivo = Convert.ToInt32(InfoComprobanteMora.CONSECUTIVO),
                                codcuentaingresosctes = "165505001",
                                codcuentaingresosmora = "165505002"
                            };
                            ctx.interescausadoprestamos.Add(causador);

                        }
                        if (TotalInteresCorriente > 0)
                        {
                            InfoComprobanteCorriente.CONSECUTIVO = (Convert.ToInt64(InfoComprobanteCorriente.CONSECUTIVO) + 1).ToString();
                            var comprobanteIntCorriente = new Comprobante()
                            {
                                TIPO = cuentas.Where(x => x.Funcion == "F4").Select(x => x.TipoComprobante).FirstOrDefault(),
                                NUMERO = InfoComprobanteCorriente.CONSECUTIVO,
                                ANO = Convert.ToString(FechaActual.Year),
                                MES = Convert.ToString(FechaActual.Month),
                                DIA = Convert.ToString(FechaActual.Day),
                                CCOSTO = "00",
                                DETALLE = "CAUSACION CREDITOS INTERES CORRIENTE",
                                TERCERO = InfoCredito.Creditos_Cedula,
                                CTAFPAGO = cuentas.Where(x => x.Funcion == "F4").Select(x => x.Cuenta_Cod).FirstOrDefault(),//CAMBIAR CUENTA PARA INTERES DE MORA
                                VRTOTAL = TotalInteresCorriente,
                                SUMDBCR = 0,
                                FECHARealiz = FechaActual,
                                ANULADO = false
                            };
                            ctx.Comprobantes.Add(comprobanteIntCorriente);

                            var mov1 = new Movimiento()
                            {
                                TIPO = cuentas.Where(x => x.Funcion == "F4").Select(x => x.TipoComprobante).FirstOrDefault(),
                                NUMERO = InfoComprobanteCorriente.CONSECUTIVO,
                                CUENTA = cuentas.Where(x => x.Funcion == "F4").Select(x => x.Cuenta_Cod).FirstOrDefault(),
                                TERCERO = InfoCredito.Creditos_Cedula,
                                DETALLE = "CAUSACION CREDITOS INTERES CORRIENTE",
                                DEBITO = TotalInteresCorriente,
                                CREDITO = 0,
                                BASE = 0,
                                CCOSTO = "00",
                                FECHAMOVIMIENTO = FechaActual,
                            };
                            ctx.Movimientos.Add(mov1);

                            var mov2 = new Movimiento()
                            {
                                TIPO = cuentas.Where(x => x.Funcion == "F2").Select(x => x.TipoComprobante).FirstOrDefault(),
                                NUMERO = InfoComprobanteCorriente.CONSECUTIVO,
                                CUENTA = cuentas.Where(x => x.Funcion == "F2").Select(x => x.Cuenta_Cod).FirstOrDefault(),
                                TERCERO = InfoCredito.Creditos_Cedula,
                                DETALLE = "CAUSACION CREDITOS INTERES CORRIENTE",
                                DEBITO = 0,
                                CREDITO = TotalInteresCorriente,
                                BASE = 0,
                                CCOSTO = "00",
                                FECHAMOVIMIENTO = FechaActual,
                            };
                            ctx.Movimientos.Add(mov2);


                            var causador = new interescausadoprestamos()
                            {
                                pagare = item.Pagare,
                                intcorriente = TotalInteresCorriente,
                                intmora = TotalInteresMora,
                                fechasistema = FechaActual,
                                agenciaId = 1,
                                usuarioCauso = "AUTOMATICO",
                                Tasainteres = PorcentajeIC,
                                numeroCuota = 0,
                                comprabante = "CI3",
                                consecutivo = Convert.ToInt32(InfoComprobanteCorriente.CONSECUTIVO),
                                codcuentaingresosctes = "165505001",
                                codcuentaingresosmora = "165505002"
                            };
                            ctx.interescausadoprestamos.Add(causador);
                        }


                        ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
                    }

                    ctx.Entry(InfoComprobanteMora).State = System.Data.Entity.EntityState.Modified;
                    ctx.Entry(InfoComprobanteCorriente).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                    return true;
                }
                catch (Exception dbE)
                {
                    return false;
                }
            }
        }



        public ViewModelInfoCredito GetInfoCredito(string NitCajero, string Pagare)
        {

            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";

            var Informacion = new ViewModelInfoCredito();

            using (var ctx = new DTO.AccountingContext())
            {
                var InfoCajero = ctx.configCajero.Where(x => x.Nit_cajero == NitCajero).Select(x => new { x.Codigo_caja, x.Nit_cajero, x.Terceros }).FirstOrDefault();
                var InfoUsuario = ctx.Creditos.Where(x => x.Pagare == Pagare).Select(x => new { x.Creditos_Cedula, x.terceroFK }).FirstOrDefault();
                var InfoCredito = ctx.TotalesCreditos.Where(x => x.Pagare == Pagare).FirstOrDefault();
                var CuotasDisponibles = ctx.ControlCreditos.Where(x => x.Pagare == Pagare && x.EstadoEnOperacion && x.EstadoEnCredito != "PZ").ToList();
                var CapitalEnCuotaList = (CuotasDisponibles.Where(x => x.Pagare == Pagare && (x.EstadoEnCredito == "AD" || x.EstadoEnCredito == "DT") && x.EstadoEnOperacion == true)
                    .OrderByDescending(x => x.Id).Select(x => x.Capital).ToList());
                decimal CapitalEnCuota = (CapitalEnCuotaList.Count > 0) ? CapitalEnCuotaList.Sum() : 0;
                decimal CuotaActual = (CuotasDisponibles.Count > 0) ? CuotasDisponibles.Select(x => x.ValorCuota).FirstOrDefault() : 0;

                if (InfoCajero != null && InfoUsuario != null && InfoCredito != null)
                {
                    Informacion.Pagare = Pagare;

                    Informacion.CodigoCaja = InfoCajero.Codigo_caja;
                    Informacion.NitCajero = InfoCajero.Nit_cajero;
                    Informacion.NombreCajero = (InfoCajero.Terceros != null) ? InfoCajero.Terceros.NOMBRE1 + " " + InfoCajero.Terceros.NOMBRE2 + " " + InfoCajero.Terceros.APELLIDO1 + " " + InfoCajero.Terceros.APELLIDO2 : "";

                    Informacion.NitUsuario = InfoUsuario.Creditos_Cedula;
                    Informacion.NombreUsuario = (InfoUsuario.terceroFK != null) ? InfoUsuario.terceroFK.NOMBRE1 + " " + InfoUsuario.terceroFK.NOMBRE2 + " " + InfoUsuario.terceroFK.APELLIDO1 + " " + InfoUsuario.terceroFK.APELLIDO2 : "";

                    Informacion.SaldoCapital = InfoCredito.SaldoCapital.ToString("N0", formato);
                    Informacion.CapitalMora = InfoCredito.CapitalMoraPendiente.ToString("N0", formato);
                    Informacion.ICPendiente = InfoCredito.InteresCorrientePendiente.ToString("N0", formato);
                    Informacion.IMPendiente = InfoCredito.InteresMoraPendiente.ToString("N0", formato);
                    Informacion.SeguroPendiente = InfoCredito.SeguroPendiente.ToString("N0", formato);
                    Informacion.CtoAdmonPendiente = InfoCredito.CtoAdmonPendiente.ToString("N0", formato);
                    Informacion.Estado = (InfoCredito.Estado == "AD") ? "AL DIA" : "EN MORA";
                    Informacion.TotalCreditoPendiente = (CapitalEnCuota + InfoCredito.CapitalMoraPendiente + InfoCredito.InteresCorrientePendiente + InfoCredito.InteresMoraPendiente + InfoCredito.SeguroPendiente+InfoCredito.CtoAdmonPendiente).ToString("N0", formato);
                    Informacion.TotalCreditoLiquidar = (InfoCredito.SaldoCapital + InfoCredito.InteresCorrientePendiente + InfoCredito.InteresMoraPendiente + InfoCredito.SeguroPendiente+InfoCredito.CtoAdmonPendiente).ToString("N0", formato);
                    Informacion.Cuotas = CuotasDisponibles;
                    Informacion.CuotaActual = CuotaActual.ToString("N0", formato);
                }

            }

            return Informacion;

        }

        public JsonResult GetCuotaActual(string Pagare)
        {
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            using (var ctx = new DTO.AccountingContext())
            {
                var Info = ctx.ControlCreditos.Where(x => x.EstadoEnOperacion && x.EstadoEnCredito != "PZ" && x.Pagare == Pagare).FirstOrDefault();
                string CuotaActual = (Info != null) ? Info.ValorCuota.ToString("N0", formato) : "0";
                return new JsonResult { Data = new { CuotaActual } };
            }
        }

        public JsonResult Pago(string Pagare, string Opcion, string ValorRecibido, string UsuarioActual, string FormaPago, string FechaPago, string NumFactura)
        {
            ValorRecibido = ValorRecibido.Replace(".", "");
            decimal vr = Convert.ToInt32(ValorRecibido);
            decimal Cambio = 0, Capital = 0, IC = 0, IM = 0, Seguro = 0,CtoAdmon=0, ValorTotal = 0, SaldoCapital = 0;
            string DetalleComprobante = "";
            string NumCuota = "";

            using (var ctx = new DTO.AccountingContext())
            {
                //Informacion del tercero asociado
                var InfoTercero = ctx.Creditos.Where(x => x.Pagare == Pagare).FirstOrDefault();
                try
                {

                    switch (Opcion)
                    {
                        case "PA":
                            #region CUOTA ACTUAL O DISPONIBLE
                            var CuotaActualPA = ctx.ControlCreditos.Where(x => x.Pagare == Pagare && x.EstadoEnOperacion && x.EstadoEnCredito != "PZ").FirstOrDefault();
                            var CuotaSiguientePA = ctx.ControlCreditos.Where(x => x.Pagare == Pagare && x.EstadoEnOperacion && x.EstadoEnCredito != "PZ" && x.Id != CuotaActualPA.Id).FirstOrDefault();
                            var TotalCreditoPA = ctx.TotalesCreditos.Where(x => x.Pagare == Pagare).FirstOrDefault();
                            string EstadoActual = CuotaActualPA.EstadoEnCredito;
                            Capital = CuotaActualPA.Capital;
                            IC = CuotaActualPA.InteresCorriente;
                            IM = CuotaActualPA.InteresMora;
                            Seguro = CuotaActualPA.Seguro;
                            CtoAdmon = CuotaActualPA.CtoAdmon;

                            ValorTotal = CuotaActualPA.ValorCuota;
                            Cambio = vr - ValorTotal;

                            //modificamos valores en la tabla ControlCreditos a la cuota correspondiente
                            CuotaActualPA.Capital = 0;
                            CuotaActualPA.InteresCorriente = 0;
                            CuotaActualPA.InteresMora = 0;
                            CuotaActualPA.Seguro = 0;
                            CuotaActualPA.CtoAdmon = 0;
                            CuotaActualPA.ValorCuota = 0;
                            CuotaActualPA.EstadoEnCredito = "PZ";

                            //modificamos valores en la tabla TotalesCreditos
                            TotalCreditoPA.CapitalTotal += Capital;
                            TotalCreditoPA.SaldoCapital -= Capital;
                            TotalCreditoPA.CapitalMoraPendiente -= (EstadoActual == "EM") ? Capital : 0;
                            TotalCreditoPA.InteresCorrienteTotal += IC;
                            TotalCreditoPA.InteresMoraTotal += IM;
                            TotalCreditoPA.SeguroTotal += Seguro;
                            TotalCreditoPA.CtoAdmonTotal += CtoAdmon;
                            TotalCreditoPA.InteresCorrientePendiente -= IC;
                            TotalCreditoPA.InteresMoraPendiente -= IM;
                            TotalCreditoPA.SeguroPendiente -= Seguro;
                            TotalCreditoPA.CtoAdmonPendiente -= CtoAdmon;
                            TotalCreditoPA.FechaProximoPago = (CuotaSiguientePA != null) ? CuotaSiguientePA.FechaPago : TotalCreditoPA.FechaProximoPago;
                            TotalCreditoPA.DiasMora = (CuotaSiguientePA != null) ? CuotaSiguientePA.DiasMora : TotalCreditoPA.DiasMora;
                            TotalCreditoPA.Estado = (CuotaSiguientePA != null) ? CuotaSiguientePA.EstadoEnCredito : TotalCreditoPA.Estado;

                            //verificamos si el saldo capital es 0 entonces el crédito se liquida
                            if (TotalCreditoPA.SaldoCapital <= 0)
                            {
                                TotalCreditoPA.SaldoCapital = 0;
                                TotalCreditoPA.DiasMora = 0;
                                TotalCreditoPA.Estado = "LQ";
                            }

                            DetalleComprobante = "CONSIGNACION CUOTA CREDITO";
                            NumCuota = CuotaActualPA.NumCuota + "/" + Convert.ToInt32(InfoTercero.Creditos_Plazo);
                            SaldoCapital = TotalCreditoPA.SaldoCapital;
                            ctx.Entry(CuotaActualPA).State = System.Data.Entity.EntityState.Modified;
                            ctx.Entry(TotalCreditoPA).State = System.Data.Entity.EntityState.Modified;
                            #endregion
                            break;
                        case "PC":
                            #region PONER CRÉDITO AL DIA

                            var CuotasPC = ctx.ControlCreditos.Where(x => x.Pagare == Pagare && x.EstadoEnOperacion && x.EstadoEnCredito != "PZ").ToList();
                            var TotalCreditoPC = ctx.TotalesCreditos.Where(x => x.Pagare == Pagare).FirstOrDefault();

                            Capital = CuotasPC.Select(x => x.Capital).Sum();
                            IC = CuotasPC.Select(x => x.InteresCorriente).Sum();
                            IM = CuotasPC.Select(x => x.InteresMora).Sum();
                            Seguro = CuotasPC.Select(x => x.Seguro).Sum();
                            CtoAdmon = CuotasPC.Select(x =>x.CtoAdmon).Sum();
                            ValorTotal = CuotasPC.Select(x => x.ValorCuota).Sum();
                            Cambio = vr - ValorTotal;

                            //modificamos el valor de las cuotas pendientes por pagar
                            foreach (var item in CuotasPC)
                            {
                                item.Capital = 0;
                                item.InteresCorriente = 0;
                                item.InteresMora = 0;
                                item.Seguro = 0;
                                item.CtoAdmon= 0;
                                item.ValorCuota = 0;
                                item.EstadoEnCredito = "PZ";

                                NumCuota += item.NumCuota + "/";

                                ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            }

                            //modificamos valores en la tabla TotalesCreditos
                            TotalCreditoPC.CapitalTotal += Capital;
                            TotalCreditoPC.SaldoCapital -= Capital;
                            TotalCreditoPC.CapitalMoraPendiente = 0;
                            TotalCreditoPC.InteresCorrienteTotal += IC;
                            TotalCreditoPC.InteresMoraTotal += IM;
                            TotalCreditoPC.SeguroTotal += Seguro;
                            TotalCreditoPC.CtoAdmonTotal += CtoAdmon;
                            TotalCreditoPC.InteresCorrientePendiente -= IC;
                            TotalCreditoPC.InteresMoraPendiente -= IM;
                            TotalCreditoPC.SeguroPendiente -= Seguro;
                            TotalCreditoPC.CtoAdmonPendiente -= CtoAdmon;
                            TotalCreditoPC.FechaProximoPago = TotalCreditoPC.FechaProximoPago;
                            TotalCreditoPC.DiasMora = 0;
                            TotalCreditoPC.Estado = "AD";


                            //verificamos si el saldo capital es 0 entonces el crédito se liquida
                            if (TotalCreditoPC.SaldoCapital <= 0)
                            {
                                TotalCreditoPC.SaldoCapital = 0;
                                TotalCreditoPC.Estado = "LQ";
                            }

                            DetalleComprobante = "CONSIGNACION CREDITO";
                            SaldoCapital = TotalCreditoPC.SaldoCapital;
                            ctx.Entry(TotalCreditoPC).State = System.Data.Entity.EntityState.Modified;
                            #endregion

                            break;
                        case "LC":
                            #region LIQUIDAR CREDITO
                            var CuotasLC = ctx.ControlCreditos.Where(x => x.Pagare == Pagare && x.EstadoEnCredito != "PZ").ToList();
                            var TotalCreditoLC = ctx.TotalesCreditos.Where(x => x.Pagare == Pagare).FirstOrDefault();

                            Capital = CuotasLC.Select(x => x.Capital).Sum();
                            IC = CuotasLC.Select(x => x.InteresCorriente).Sum();
                            IM = CuotasLC.Select(x => x.InteresMora).Sum();
                            Seguro = CuotasLC.Select(x => x.Seguro).Sum();
                            CtoAdmon=CuotasLC.Select(x => x.CtoAdmon).Sum();
                            ValorTotal = CuotasLC.Select(x => x.ValorCuota).Sum();
                            Cambio = vr - ValorTotal;

                            //modificamos el valor de las cuotas pendientes por pagar
                            foreach (var item in CuotasLC)
                            {
                                item.Capital = 0;
                                item.InteresCorriente = 0;
                                item.InteresMora = 0;
                                item.Seguro = 0;
                                item.CtoAdmon = 0;
                                item.ValorCuota = 0;
                                item.EstadoEnCredito = "PZ";

                                NumCuota += item.NumCuota + "/";

                                ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            }

                            //modificamos valores en la tabla TotalesCreditos
                            TotalCreditoLC.CapitalTotal += Capital;
                            TotalCreditoLC.SaldoCapital -= Capital;
                            TotalCreditoLC.CapitalMoraPendiente = 0;
                            TotalCreditoLC.InteresCorrienteTotal += IC;
                            TotalCreditoLC.InteresMoraTotal += IM;
                            TotalCreditoLC.SeguroTotal += Seguro;
                            TotalCreditoLC.CtoAdmonTotal += CtoAdmon;
                            TotalCreditoLC.InteresCorrientePendiente -= IC;
                            TotalCreditoLC.InteresMoraPendiente -= IM;
                            TotalCreditoLC.SeguroPendiente -= Seguro;
                            TotalCreditoLC.CtoAdmonPendiente -= CtoAdmon;
                            TotalCreditoLC.DiasMora = 0;
                            TotalCreditoLC.Estado = "LQ";

                            //verificamos si el saldo capital es 0 entonces el crédito se liquida
                            if (TotalCreditoLC.SaldoCapital <= 0)
                            {
                                TotalCreditoLC.SaldoCapital = 0;
                            }

                            DetalleComprobante = "CONSIGNACION CREDITO";
                            SaldoCapital = TotalCreditoLC.SaldoCapital;
                            ctx.Entry(TotalCreditoLC).State = System.Data.Entity.EntityState.Modified;
                            #endregion
                            break;
                        default:
                            break;
                    }

                    DateTime FechaActual = (FechaPago == "") ? Fecha.GetFechaColombia() : Convert.ToDateTime(FechaPago);
                    string FechaString = Fecha.GetFechaColombia().ToString("yyyy-MM-dd");


                    //Informacion del cajero y caja
                    var InfoCajero = ctx.configCajero.Where(x => x.Nit_cajero == UsuarioActual).FirstOrDefault();
                    var InfoCaja = ctx.Caja.Where(x => x.Codigo_caja == InfoCajero.Codigo_caja).FirstOrDefault();

                    //actualizamos consecutivo de caja y tope máximo
                    InfoCaja.consecutivo_actual++;
                    InfoCaja.TopeMaximo_caja += Convert.ToDouble(ValorTotal);
                    ctx.Entry(InfoCaja).State = System.Data.Entity.EntityState.Modified;


                    //Actualizamos el cuadre de caja
                    var CuadreCaja = ctx.CuadreCajaPorCajero.Where(x => x.fecha == FechaString && x.codigo_caja == InfoCaja.Codigo_caja && x.nit_cajero == UsuarioActual && x.cierre == 0).FirstOrDefault();
                    CuadreCaja.tope += ValorTotal;
                    ctx.Entry(CuadreCaja).State = System.Data.Entity.EntityState.Modified;

                    //creamos el comprobante
                    var InfoCuentas = ctx.Cuentas.ToList();
                    var Cuenta = InfoCuentas.Where(x => x.Funcion == "F14" && InfoTercero.Destino_Id == x.Destino_Id).FirstOrDefault();
                    var ComprobanteOperacion = ctx.TiposComprobantes.Where(x => x.CODIGO == Cuenta.TipoComprobante && x.INACTIVO == false).FirstOrDefault();
                    ComprobanteOperacion.CONSECUTIVO = (Convert.ToInt32(ComprobanteOperacion.CONSECUTIVO) + 1).ToString();
                    ctx.Entry(ComprobanteOperacion).State = System.Data.Entity.EntityState.Modified;
                    var NuevoComprobante = new Comprobante()
                    {
                        TIPO = Cuenta.TipoComprobante,
                        NUMERO = ComprobanteOperacion.CONSECUTIVO,
                        ANO = FechaActual.ToString("yyyy"),
                        MES = FechaActual.ToString("MM"),
                        DIA = FechaActual.ToString("dd"),
                        CCOSTO = InfoCajero.centrocosto,
                        DETALLE = "CONSIGNACION CUOTA CREDITO",
                        TERCERO = InfoTercero.Creditos_Cedula,
                        CTAFPAGO = InfoCaja.cta_abastecimiento,
                        VRTOTAL = ValorTotal,
                        SUMDBCR = 0,
                        FECHARealiz = FechaActual,
                        ANULADO = false
                    };
                    ctx.Comprobantes.Add(NuevoComprobante);

                    //creamos los movimientos
                    //caja
                    var mov1 = new Movimiento()
                    {
                        TIPO = Cuenta.TipoComprobante,
                        NUMERO = ComprobanteOperacion.CONSECUTIVO,
                        CUENTA = InfoCaja.cta_abastecimiento,
                        TERCERO = InfoTercero.Creditos_Cedula,
                        DETALLE = DetalleComprobante,
                        DEBITO = ValorTotal,
                        CREDITO = 0,
                        BASE = 0,
                        CCOSTO = InfoCajero.centrocosto,
                        FECHAMOVIMIENTO = FechaActual,
                        DOCUMENTO = ""
                    };
                    ctx.Movimientos.Add(mov1);
                    //capital
                    if (Capital > 0)
                    {
                        var CuentaCapital = InfoCuentas.Where(x => x.Funcion == "F1" && InfoTercero.Destino_Id == x.Destino_Id).FirstOrDefault();
                        var mov2 = new Movimiento()
                        {
                            TIPO = Cuenta.TipoComprobante,
                            NUMERO = ComprobanteOperacion.CONSECUTIVO,
                            CUENTA = CuentaCapital.Cuenta_Cod,
                            TERCERO = InfoTercero.Creditos_Cedula,
                            DETALLE = DetalleComprobante,
                            DEBITO = 0,
                            CREDITO = Capital,
                            BASE = 0,
                            CCOSTO = InfoCajero.centrocosto,
                            FECHAMOVIMIENTO = FechaActual,
                            DOCUMENTO = ""
                        };
                        ctx.Movimientos.Add(mov2);
                    }
                    //interes corriente
                    if (IC > 0)
                    {
                        var CuentaInteres = InfoCuentas.Where(x => x.Funcion == "F4" && InfoTercero.Destino_Id == x.Destino_Id).FirstOrDefault(); ;
                        var mov3 = new Movimiento()
                        {
                            TIPO = Cuenta.TipoComprobante,
                            NUMERO = ComprobanteOperacion.CONSECUTIVO,
                            CUENTA = CuentaInteres.Cuenta_Cod,
                            TERCERO = InfoTercero.Creditos_Cedula,
                            DETALLE = DetalleComprobante,
                            DEBITO = 0,
                            CREDITO = IC,
                            BASE = 0,
                            CCOSTO = InfoCajero.centrocosto,
                            FECHAMOVIMIENTO = FechaActual,
                            DOCUMENTO = ""
                        };
                        ctx.Movimientos.Add(mov3);
                    }
                    //interes mora
                    if (IM > 0)
                    {
                        var CuentaIM = InfoCuentas.Where(x => x.Funcion == "F6" && InfoTercero.Destino_Id == x.Destino_Id).FirstOrDefault();
                        var mov4 = new Movimiento()
                        {
                            TIPO = Cuenta.TipoComprobante,
                            NUMERO = ComprobanteOperacion.CONSECUTIVO,
                            CUENTA = CuentaIM.Cuenta_Cod,
                            TERCERO = InfoTercero.Creditos_Cedula,
                            DETALLE = DetalleComprobante,
                            DEBITO = 0,
                            CREDITO = IM,
                            BASE = 0,
                            CCOSTO = InfoCajero.centrocosto,
                            FECHAMOVIMIENTO = FechaActual,
                            DOCUMENTO = ""
                        };
                        ctx.Movimientos.Add(mov4);
                    }
                    //seguro
                    if (Seguro > 0)
                    {
                        var CuentaSeguro = InfoCuentas.Where(x => x.Funcion == "F9" && InfoTercero.Destino_Id == x.Destino_Id).FirstOrDefault();
                        var mov5 = new Movimiento()
                        {
                            TIPO = Cuenta.TipoComprobante,
                            NUMERO = ComprobanteOperacion.CONSECUTIVO,
                            CUENTA = CuentaSeguro.Cuenta_Cod,
                            TERCERO = InfoTercero.Creditos_Cedula,
                            DETALLE = DetalleComprobante,
                            DEBITO = 0,
                            CREDITO = Seguro,
                            BASE = 0,
                            CCOSTO = InfoCajero.centrocosto,
                            FECHAMOVIMIENTO = FechaActual,
                            DOCUMENTO = ""
                        };
                        ctx.Movimientos.Add(mov5);
                    }
                    //Costo de administración
                    if (CtoAdmon > 0)
                    {
                        var CuentaSeguro = InfoCuentas.Where(x => x.Funcion == "F15" && InfoTercero.Destino_Id == x.Destino_Id).FirstOrDefault();
                        var mov6 = new Movimiento()
                        {
                            TIPO = Cuenta.TipoComprobante,
                            NUMERO = ComprobanteOperacion.CONSECUTIVO,
                            CUENTA = CuentaSeguro.Cuenta_Cod,
                            TERCERO = InfoTercero.Creditos_Cedula,
                            DETALLE = DetalleComprobante,
                            DEBITO = 0,
                            CREDITO = CtoAdmon,
                            BASE = 0,
                            CCOSTO = InfoCajero.centrocosto,
                            FECHAMOVIMIENTO = FechaActual,
                            DOCUMENTO = ""
                        };
                        ctx.Movimientos.Add(mov6);
                    }


                    //Creamos la factura
                    string NFactura = (NumFactura != "") ? NumFactura : InfoCaja.agencia + "-" + InfoCaja.Codigo_caja + "-" + InfoCaja.Serie + "-" + InfoCaja.consecutivo_actual;
                    var Factura = new factOpCajaConsCuotaCredito
                    {
                        fecha = FechaActual,
                        factura = NFactura,
                        NIT = InfoTercero.Creditos_Cedula,
                        codigoCaja = InfoCaja.Codigo_caja,
                        pagare = Pagare,
                        valorConsignado = Convert.ToInt32(ValorTotal).ToString(),
                        nitCajero = UsuarioActual,
                        abonoCapital = Convert.ToInt32(Capital).ToString(),
                        numeroCuota = NumCuota,
                        interesCorriente = Convert.ToInt32(IC).ToString(),
                        interesMora = Convert.ToInt32(IM).ToString(),
                        seguros = Convert.ToInt32(Seguro).ToString(),
                        CtoAdmon = Convert.ToInt32(CtoAdmon).ToString(),
                        saldoCapital = Convert.ToInt32(SaldoCapital).ToString(),
                        FormaPago = FormaPago,
                        TipoComprobante = Cuenta.TipoComprobante,
                        NumeroComprobante = ComprobanteOperacion.CONSECUTIVO
                    };
                    ctx.factOpCajaConsCuotaCredito.Add(Factura);

                    ctx.SaveChanges();
                    return new JsonResult { Data = new { status = true, Id = Factura.id } };
                }
                catch (Exception ex)
                {
                    return new JsonResult { Data = new { status = false, Message = ex.Message } };
                    throw;
                }
            }


        }

        public JsonResult Abono(string Pagare, string ValorConsignado, string ValorRecibido, string UsuarioActual, string FormaPago, string FechaPago, string NumFactura)
        {
            ValorConsignado = ValorConsignado.Replace(".", "");
            ValorRecibido = ValorRecibido.Replace(".", "");
            decimal VC = Convert.ToDecimal(ValorConsignado);
            decimal VR = Convert.ToDecimal(ValorRecibido);
            decimal ValorAuxiliar = VC;
            decimal Cambio = 0, Capital = 0, IC = 0, IM = 0, Seguro = 0, CtoAdmon=0, SaldoCapital = 0;
            bool BanderaCapital = true;
            using (var ctx = new DTO.AccountingContext())
            {
                try
                {
                    var InfoTercero = ctx.Creditos.Where(x => x.Pagare == Pagare).FirstOrDefault();
                    var TotalCredito = ctx.TotalesCreditos.Where(x => x.Pagare == Pagare).FirstOrDefault();
                    var ControlCreditos = ctx.ControlCreditos.Where(x => x.Pagare == Pagare && x.EstadoEnCredito != "PZ").ToList();
                    decimal AbonoNormal = ControlCreditos.Where(x => x.EstadoEnOperacion).Select(x => x.ValorCuota).Sum();
                    decimal AbonoCapital = ControlCreditos.Where(x => x.EstadoEnOperacion == false).Select(x => x.ValorCuota).Sum();
                    string DetalleComprobante = "";

                    if (VC <= AbonoNormal)
                    {
                        foreach (var item in ControlCreditos)
                        {
                            //seccion pago a interes mora
                            if (ValorAuxiliar > 0)
                            {
                                if (ValorAuxiliar > item.InteresMora)
                                {
                                    ValorAuxiliar -= item.InteresMora;
                                    item.ValorCuota -= item.InteresMora;

                                    TotalCredito.InteresMoraPendiente -= item.InteresMora;
                                    TotalCredito.InteresMoraTotal += item.InteresMora;

                                    IM += item.InteresMora;

                                    item.InteresMora = 0;
                                }
                                else
                                {
                                    item.InteresMora -= ValorAuxiliar;
                                    item.ValorCuota -= ValorAuxiliar;

                                    TotalCredito.InteresMoraPendiente -= ValorAuxiliar;
                                    TotalCredito.InteresMoraTotal += ValorAuxiliar;

                                    IM += ValorAuxiliar;

                                    ValorAuxiliar = 0;
                                }
                            }

                            //sección pago a interes corriente
                            if (ValorAuxiliar > 0)
                            {
                                if (ValorAuxiliar > item.InteresCorriente)
                                {
                                    ValorAuxiliar -= item.InteresCorriente;
                                    item.ValorCuota -= item.InteresCorriente;

                                    TotalCredito.InteresCorrientePendiente -= item.InteresCorriente;
                                    TotalCredito.InteresCorrienteTotal += item.InteresCorriente;

                                    IC += item.InteresCorriente;

                                    item.InteresCorriente = 0;
                                }
                                else
                                {
                                    item.InteresCorriente -= ValorAuxiliar;
                                    item.ValorCuota -= ValorAuxiliar;

                                    TotalCredito.InteresCorrientePendiente -= ValorAuxiliar;
                                    TotalCredito.InteresCorrienteTotal += ValorAuxiliar;

                                    IC += ValorAuxiliar;

                                    ValorAuxiliar = 0;
                                }
                            }

                            //sección pago a seguro
                            if (ValorAuxiliar > 0)
                            {
                                if (ValorAuxiliar > item.Seguro)
                                {
                                    ValorAuxiliar -= item.Seguro;
                                    item.ValorCuota -= item.Seguro;

                                    TotalCredito.SeguroPendiente -= item.Seguro;
                                    TotalCredito.SeguroTotal += item.Seguro;

                                    Seguro += item.Seguro;

                                    item.Seguro = 0;
                                }
                                else
                                {
                                    item.Seguro -= ValorAuxiliar;
                                    item.ValorCuota -= ValorAuxiliar;

                                    TotalCredito.SeguroPendiente -= ValorAuxiliar;
                                    TotalCredito.SeguroTotal += ValorAuxiliar;

                                    Seguro += ValorAuxiliar;

                                    ValorAuxiliar = 0;
                                }
                            }

                            //sección pago a costo administración
                            if (ValorAuxiliar > 0)
                            {
                                if (ValorAuxiliar > item.CtoAdmon)
                                {
                                    ValorAuxiliar -= item.CtoAdmon;
                                    item.ValorCuota -= item.CtoAdmon;

                                    TotalCredito.CtoAdmonPendiente -= item.CtoAdmon;
                                    TotalCredito.CtoAdmonTotal += item.CtoAdmon;

                                    CtoAdmon += item.CtoAdmon;

                                    item.CtoAdmon = 0;
                                }
                                else
                                {
                                    item.CtoAdmon -= ValorAuxiliar;
                                    item.ValorCuota -= ValorAuxiliar;

                                    TotalCredito.CtoAdmonPendiente -= ValorAuxiliar;
                                    TotalCredito.CtoAdmonTotal += ValorAuxiliar;

                                    CtoAdmon += ValorAuxiliar;

                                    ValorAuxiliar = 0;
                                }
                            }

                            //sección pago a capital
                            if (ValorAuxiliar > 0)
                            {

                                if (ValorAuxiliar > item.Capital)
                                {
                                    ValorAuxiliar -= item.Capital;
                                    item.ValorCuota -= item.Capital;

                                    TotalCredito.SaldoCapital -= item.Capital;
                                    TotalCredito.CapitalTotal += item.Capital;

                                    Capital += item.Capital;

                                    if (item.EstadoEnCredito == "EM")
                                    {
                                        TotalCredito.CapitalMoraPendiente -= item.Capital;
                                    }

                                    item.Capital = 0;

                                    item.EstadoEnCredito = "PZ";


                                }
                                else
                                {
                                    item.Capital -= ValorAuxiliar;
                                    item.ValorCuota -= ValorAuxiliar;

                                    TotalCredito.SaldoCapital -= ValorAuxiliar;
                                    TotalCredito.CapitalTotal += ValorAuxiliar;

                                    Capital += ValorAuxiliar;

                                    if (item.EstadoEnCredito == "EM")
                                    {
                                        TotalCredito.CapitalMoraPendiente -= ValorAuxiliar;
                                    }

                                    ValorAuxiliar = 0;

                                    if (item.Capital <= 0)
                                    {
                                        item.EstadoEnCredito = "PZ";
                                    }

                                }
                            }

                            ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;

                        }//fin ciclo foreach

                    }
                    else if (VC > AbonoNormal && AbonoCapital > 0)
                    {
                        decimal SaldoCapitalAuxiliar = 0;
                        foreach (var item in ControlCreditos)
                        {
                            if (item.EstadoEnOperacion)
                            {
                                ValorAuxiliar -= item.ValorCuota;//disminuimos este valor por cada iteración, al final nos guardará el valor para realizar el abono a capital

                                Capital += item.Capital;
                                IC += item.InteresCorriente;
                                IM += item.InteresMora;
                                Seguro += item.Seguro;
                                CtoAdmon += item.CtoAdmon;

                                //modificamos valores en la tabla TotalesCreditos
                                TotalCredito.CapitalTotal += item.Capital;
                                TotalCredito.SaldoCapital -= item.Capital;
                                if (item.EstadoEnCredito == "EM")
                                {
                                    TotalCredito.CapitalMoraPendiente -= item.Capital;
                                }
                                TotalCredito.InteresCorrienteTotal += item.InteresCorriente;
                                TotalCredito.InteresMoraTotal += item.InteresMora;
                                TotalCredito.SeguroTotal += item.Seguro;
                                TotalCredito.CtoAdmonTotal += item.CtoAdmon;
                                TotalCredito.InteresCorrientePendiente -= item.InteresCorriente;
                                TotalCredito.InteresMoraPendiente -= item.InteresMora;
                                TotalCredito.SeguroPendiente -= item.Seguro;
                                TotalCredito.CtoAdmonPendiente -= item.CtoAdmon;

                                //modificamos la fila perteneciente a la cuota
                                item.Capital = 0;
                                item.InteresCorriente = 0;
                                item.InteresMora = 0;
                                item.Seguro = 0;
                                item.CtoAdmon = 0;
                                item.ValorCuota = 0;
                                item.DiasMora = 0;
                                item.EstadoEnCredito = "PZ";


                                ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
                            }
                            else
                            {
                                //en este else, se restructura las tuplas en estado false debido al abono capital
                                if (BanderaCapital)
                                {//condicional en el cual sólo debe ingresar una vez: se debe disminuir el valor de la primera tupla al saldo capital actual
                                    Capital += ValorAuxiliar;
                                    item.SaldoCapitalEnCuota -= ValorAuxiliar;
                                    TotalCredito.SaldoCapital -= ValorAuxiliar;
                                    TotalCredito.CapitalTotal += ValorAuxiliar;
                                    BanderaCapital = false;
                                }
                                else
                                {
                                    item.SaldoCapitalEnCuota = SaldoCapitalAuxiliar;
                                }

                                SaldoCapitalAuxiliar = item.SaldoCapitalEnCuota - item.Capital;
                                if (SaldoCapitalAuxiliar < 0)
                                {
                                    item.Capital = item.SaldoCapitalEnCuota;
                                    item.ValorCuota = item.Capital;
                                }

                                ctx.Entry(item).State = System.Data.Entity.EntityState.Modified;
                                if (item.SaldoCapitalEnCuota < 0)
                                {
                                    ctx.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                                }

                            }
                        }
                    }

                    //verificamos si el saldo capital es 0 entonces el crédito se liquida
                    if (TotalCredito.SaldoCapital <= 0)
                    {
                        TotalCredito.SaldoCapital = 0;
                        TotalCredito.Estado = "LQ";
                        TotalCredito.DiasMora = 0;
                    }
                    DetalleComprobante = "ABONO CREDITO";
                    SaldoCapital = TotalCredito.SaldoCapital;
                    ctx.Entry(TotalCredito).State = System.Data.Entity.EntityState.Modified;



                    DateTime FechaActual = (FechaPago == "") ? Fecha.GetFechaColombia() : Convert.ToDateTime(FechaPago);
                    string FechaString = Fecha.GetFechaColombia().ToString("yyyy-MM-dd");

                    //Informacion del cajero y caja
                    var InfoCajero = ctx.configCajero.Where(x => x.Nit_cajero == UsuarioActual).FirstOrDefault();
                    var InfoCaja = ctx.Caja.Where(x => x.Codigo_caja == InfoCajero.Codigo_caja).FirstOrDefault();

                    //actualizamos consecutivo de caja y tope máximo
                    InfoCaja.consecutivo_actual++;
                    InfoCaja.TopeMaximo_caja += Convert.ToDouble(VC);
                    ctx.Entry(InfoCaja).State = System.Data.Entity.EntityState.Modified;

                    //Actualizamos el cuadre de caja
                    var CuadreCaja = ctx.CuadreCajaPorCajero.Where(x => x.fecha == FechaString && x.codigo_caja == InfoCaja.Codigo_caja && x.nit_cajero == UsuarioActual && x.cierre == 0).FirstOrDefault();
                    CuadreCaja.tope += VC;
                    ctx.Entry(CuadreCaja).State = System.Data.Entity.EntityState.Modified;

                    //creamos el comprobante
                    var InfoCuentas = ctx.Cuentas.ToList();
                    var Cuenta = InfoCuentas.Where(x => x.Funcion == "F14" && InfoTercero.Destino_Id == x.Destino_Id).FirstOrDefault();
                    var ComprobanteOperacion = ctx.TiposComprobantes.Where(x => x.CODIGO == Cuenta.TipoComprobante && x.INACTIVO == false).FirstOrDefault();
                    ComprobanteOperacion.CONSECUTIVO = (Convert.ToInt32(ComprobanteOperacion.CONSECUTIVO) + 1).ToString();
                    ctx.Entry(ComprobanteOperacion).State = System.Data.Entity.EntityState.Modified;
                    var NuevoComprobante = new Comprobante()
                    {
                        TIPO = Cuenta.TipoComprobante,
                        NUMERO = ComprobanteOperacion.CONSECUTIVO,
                        ANO = FechaActual.ToString("yyyy"),
                        MES = FechaActual.ToString("MM"),
                        DIA = FechaActual.ToString("dd"),
                        CCOSTO = InfoCajero.centrocosto,
                        DETALLE = "ABONO CREDITO",
                        TERCERO = InfoTercero.Creditos_Cedula,
                        CTAFPAGO = InfoCaja.cta_abastecimiento,
                        VRTOTAL = VC,
                        SUMDBCR = 0,
                        FECHARealiz = FechaActual,
                        ANULADO = false
                    };
                    ctx.Comprobantes.Add(NuevoComprobante);

                    //creamos los movimientos
                    //caja
                    var mov1 = new Movimiento()
                    {
                        TIPO = Cuenta.TipoComprobante,
                        NUMERO = ComprobanteOperacion.CONSECUTIVO,
                        CUENTA = InfoCaja.cta_abastecimiento,
                        TERCERO = InfoTercero.Creditos_Cedula,
                        DETALLE = DetalleComprobante,
                        DEBITO = VC,
                        CREDITO = 0,
                        BASE = 0,
                        CCOSTO = InfoCajero.centrocosto,
                        FECHAMOVIMIENTO = FechaActual,
                        DOCUMENTO = ""
                    };
                    ctx.Movimientos.Add(mov1);
                    //capital
                    if (Capital > 0)
                    {
                        var CuentaCapital = InfoCuentas.Where(x => x.Funcion == "F1" && InfoTercero.Destino_Id == x.Destino_Id).FirstOrDefault();
                        var mov2 = new Movimiento()
                        {
                            TIPO = Cuenta.TipoComprobante,
                            NUMERO = ComprobanteOperacion.CONSECUTIVO,
                            CUENTA = CuentaCapital.Cuenta_Cod,
                            TERCERO = InfoTercero.Creditos_Cedula,
                            DETALLE = DetalleComprobante,
                            DEBITO = 0,
                            CREDITO = Capital,
                            BASE = 0,
                            CCOSTO = InfoCajero.centrocosto,
                            FECHAMOVIMIENTO = FechaActual,
                            DOCUMENTO = ""
                        };
                        ctx.Movimientos.Add(mov2);
                    }
                    //interes corriente
                    if (IC > 0)
                    {
                        var CuentaInteres = InfoCuentas.Where(x => x.Funcion == "F4" && InfoTercero.Destino_Id == x.Destino_Id).FirstOrDefault(); ;
                        var mov3 = new Movimiento()
                        {
                            TIPO = Cuenta.TipoComprobante,
                            NUMERO = ComprobanteOperacion.CONSECUTIVO,
                            CUENTA = CuentaInteres.Cuenta_Cod,
                            TERCERO = InfoTercero.Creditos_Cedula,
                            DETALLE = DetalleComprobante,
                            DEBITO = 0,
                            CREDITO = IC,
                            BASE = 0,
                            CCOSTO = InfoCajero.centrocosto,
                            FECHAMOVIMIENTO = FechaActual,
                            DOCUMENTO = ""
                        };
                        ctx.Movimientos.Add(mov3);
                    }
                    //interes mora
                    if (IM > 0)
                    {
                        var CuentaIM = InfoCuentas.Where(x => x.Funcion == "F6" && InfoTercero.Destino_Id == x.Destino_Id).FirstOrDefault();
                        var mov4 = new Movimiento()
                        {
                            TIPO = Cuenta.TipoComprobante,
                            NUMERO = ComprobanteOperacion.CONSECUTIVO,
                            CUENTA = CuentaIM.Cuenta_Cod,
                            TERCERO = InfoTercero.Creditos_Cedula,
                            DETALLE = DetalleComprobante,
                            DEBITO = 0,
                            CREDITO = IM,
                            BASE = 0,
                            CCOSTO = InfoCajero.centrocosto,
                            FECHAMOVIMIENTO = FechaActual,
                            DOCUMENTO = ""
                        };
                        ctx.Movimientos.Add(mov4);
                    }
                    //seguro
                    if (Seguro > 0)
                    {
                        var CuentaSeguro = InfoCuentas.Where(x => x.Funcion == "F9" && InfoTercero.Destino_Id == x.Destino_Id).FirstOrDefault();
                        var mov5 = new Movimiento()
                        {
                            TIPO = Cuenta.TipoComprobante,
                            NUMERO = ComprobanteOperacion.CONSECUTIVO,
                            CUENTA = CuentaSeguro.Cuenta_Cod,
                            TERCERO = InfoTercero.Creditos_Cedula,
                            DETALLE = DetalleComprobante,
                            DEBITO = 0,
                            CREDITO = Seguro,
                            BASE = 0,
                            CCOSTO = InfoCajero.centrocosto,
                            FECHAMOVIMIENTO = FechaActual,
                            DOCUMENTO = ""
                        };
                        ctx.Movimientos.Add(mov5);
                    }
                    //Costo administración
                    if (CtoAdmon > 0)
                    {
                        var CuentaSeguro = InfoCuentas.Where(x => x.Funcion == "F15" && InfoTercero.Destino_Id == x.Destino_Id).FirstOrDefault();
                        var mov6 = new Movimiento()
                        {
                            TIPO = Cuenta.TipoComprobante,
                            NUMERO = ComprobanteOperacion.CONSECUTIVO,
                            CUENTA = CuentaSeguro.Cuenta_Cod,
                            TERCERO = InfoTercero.Creditos_Cedula,
                            DETALLE = DetalleComprobante,
                            DEBITO = 0,
                            CREDITO = CtoAdmon,
                            BASE = 0,
                            CCOSTO = InfoCajero.centrocosto,
                            FECHAMOVIMIENTO = FechaActual,
                            DOCUMENTO = ""
                        };
                        ctx.Movimientos.Add(mov6);
                    }


                    //Creamos la factura
                    string NFactura = (NumFactura != "") ? NumFactura : InfoCaja.agencia + "-" + InfoCaja.Codigo_caja + "-" + InfoCaja.Serie + "-" + InfoCaja.consecutivo_actual;
                    var Factura = new factOpCajaConsCuotaCredito
                    {
                        fecha = FechaActual,
                        factura = NFactura,
                        NIT = InfoTercero.Creditos_Cedula,
                        codigoCaja = InfoCaja.Codigo_caja,
                        pagare = Pagare,
                        valorConsignado = Convert.ToInt32(VC).ToString(),
                        nitCajero = UsuarioActual,
                        abonoCapital = Convert.ToInt32(Capital).ToString(),
                        numeroCuota = "Abono",
                        interesCorriente = Convert.ToInt32(IC).ToString(),
                        interesMora = Convert.ToInt32(IM).ToString(),
                        seguros = Convert.ToInt32(Seguro).ToString(),
                        CtoAdmon = Convert.ToInt32(CtoAdmon).ToString(),
                        saldoCapital = Convert.ToInt32(SaldoCapital).ToString(),
                        FormaPago = FormaPago,
                        TipoComprobante = Cuenta.TipoComprobante,
                        NumeroComprobante = ComprobanteOperacion.CONSECUTIVO
                    };
                    ctx.factOpCajaConsCuotaCredito.Add(Factura);

                    ctx.SaveChanges();//guardamos cambios

                    //actualizamos la tabla control creditos
                    var cc = ctx.ControlCreditos.Where(x => x.Pagare == Pagare && x.EstadoEnCredito != "PZ").ToList();
                    //verificamos si el crédito está en mora
                    var tc = ctx.TotalesCreditos.Where(x => x.Pagare == Pagare).FirstOrDefault();
                    var cm = cc.Where(x => x.EstadoEnCredito == "EM").ToList();
                    var normal = cc.Where(x => x.EstadoEnCredito == "DT" || x.EstadoEnCredito == "AD").ToList();
                    if (tc.Estado != "LQ")
                    {
                        if (cm.Count > 0)
                        {
                            tc.Estado = "EM";
                            tc.DiasMora = cm.Select(x => x.DiasMora).FirstOrDefault();
                            tc.FechaProximoPago = cm.Select(x => x.FechaPago).FirstOrDefault();
                        }
                        else
                        {
                            tc.Estado = "AD";
                            tc.DiasMora = 0;
                            tc.FechaProximoPago = normal.Select(x => x.FechaPago).FirstOrDefault();
                        }
                    }
                    ctx.Entry(tc).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();

                    return new JsonResult { Data = new { status = true, Id = Factura.id } };
                }
                catch (Exception ex)
                {
                    return new JsonResult { Data = new { status = false, Message = ex.Message } };
                    throw;
                }
            }

        }


        public JsonResult VerificaValorAbono(string Pagare, string ValorConsignado)
        {
            ValorConsignado = ValorConsignado.Replace(".", "");
            decimal vc = Convert.ToDecimal(ValorConsignado);
            using (var ctx = new DTO.AccountingContext())
            {
                var DatosCredito = ctx.ControlCreditos.Where(x => x.Pagare == Pagare && x.EstadoEnCredito != "PZ").ToList();
                decimal ValorParaAbonar = DatosCredito.Select(x => x.ValorCuota).Sum();

                if (vc > ValorParaAbonar)
                {
                    return new JsonResult { Data = new { status = false } };
                }
                else
                {
                    return new JsonResult { Data = new { status = true } };
                }
            }


        }

        public List<TotalesCreditos> ConsultarPagares()
        {
            using (var ctx = new DTO.AccountingContext())
            {
                return ctx.TotalesCreditos.Where(x => x.Estado != "LQ").ToList();
            }
        }

        public JsonResult GetCuotasCredito(string Pagare)
        {
            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;

            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            string dataAsociado = "";

            using (var ctx = new DTO.AccountingContext())
            {
                var result = ctx.ControlCreditos.Where(x => x.Pagare == Pagare && x.EstadoEnCredito != "PZ" && x.EstadoEnOperacion).ToList();
                var infoCredito = ctx.Creditos.Where(x => x.Pagare == Pagare).Include(x => x.terceroFK).FirstOrDefault();
                List<Array> array = new List<Array>();
                foreach (var item in result)
                {
                    string[] data = new string[10];
                    data[0] = item.Id.ToString();
                    data[1] = item.NumCuota.ToString();
                    data[2] = item.Capital.ToString("N0", formato);
                    data[3] = item.InteresCorriente.ToString("N0", formato);
                    data[4] = item.InteresMora.ToString("N0", formato);
                    data[5] = item.Seguro.ToString("N0", formato);
                    data[6] = item.CtoAdmon.ToString("N0", formato);
                    data[7] = item.DiasMora.ToString();
                    data[8] = (item.EstadoEnCredito == "EM") ? "EN MORA" : "AL DIA";
                    data[9] = item.ValorCuota.ToString("N0", formato);

                    array.Add(data);
                }

                dataAsociado = infoCredito.terceroFK != null ? infoCredito.Creditos_Cedula + " - " + infoCredito.terceroFK.NOMBRE1 + " " + infoCredito.terceroFK.NOMBRE2 + " " + infoCredito.terceroFK.APELLIDO1 + " " + infoCredito.terceroFK.APELLIDO2 : infoCredito.Creditos_Cedula;

                return new JsonResult { Data = new { status = true, array,dataAsociado } };
            }
        }

        public JsonResult CalcularIM(int Id, int DiasMora)
        {
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            try
            {
                using (var ctx = new DTO.AccountingContext())
                {
                    var Info = ctx.ControlCreditos.Find(Id);
                    var InfoCredito = ctx.Creditos.Where(x => x.Pagare == Info.Pagare).FirstOrDefault();
                    decimal Capital = Info.Capital;
                    decimal PIM = InfoCredito.Creditos_Interes_Mora / 100;//porcentaje interes de mora
                    decimal Calculo = ((Capital * PIM) / 30) * DiasMora;
                    Calculo = Math.Round(Calculo, 0, MidpointRounding.ToEven);

                    return new JsonResult { Data = new { status = true, NuevoIM = Calculo.ToString("N0", formato) } };
                }

            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { status = false } };
                throw;
            }

        }

        public JsonResult GuardarValores(int Id, string IC, string IM, string seguro,string admon)
        {
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            try
            {
                IC = IC.Replace(".", "");
                IM = IM.Replace(".", "");
                seguro = seguro.Replace(".", "");
                admon = admon.Replace(".", "");
                using (var ctx = new DTO.AccountingContext())
                {
                    decimal Corriente = Convert.ToDecimal(IC);
                    decimal Mora = Convert.ToDecimal(IM);
                    decimal valorSeguro = Convert.ToDecimal(seguro);
                    decimal valorAdmon = Convert.ToDecimal(admon);

                    var ControlCredito = ctx.ControlCreditos.Find(Id);
                    var TotalCredito = ctx.TotalesCreditos.Where(x => x.Pagare == ControlCredito.Pagare).FirstOrDefault();

                    TotalCredito.InteresCorrientePendiente -= ControlCredito.InteresCorriente;
                    TotalCredito.InteresMoraPendiente -= ControlCredito.InteresMora;
                    TotalCredito.SeguroPendiente -= ControlCredito.Seguro;
                    TotalCredito.CtoAdmonPendiente -= ControlCredito.CtoAdmon;
                    TotalCredito.InteresCorrientePendiente += Corriente;
                    TotalCredito.InteresMoraPendiente += Mora;
                    TotalCredito.SeguroPendiente += valorSeguro;
                    TotalCredito.CtoAdmonPendiente += valorAdmon;

                    ControlCredito.ValorCuota -= ControlCredito.InteresCorriente;
                    ControlCredito.ValorCuota -= ControlCredito.InteresMora;
                    ControlCredito.ValorCuota -= ControlCredito.Seguro;
                    ControlCredito.ValorCuota -= ControlCredito.CtoAdmon;
                    ControlCredito.ValorCuota += Corriente;
                    ControlCredito.ValorCuota += Mora;
                    ControlCredito.ValorCuota += valorSeguro;
                    ControlCredito.ValorCuota += valorAdmon;
                    ControlCredito.InteresCorriente = Corriente;
                    ControlCredito.InteresMora = Mora;
                    ControlCredito.Seguro = valorSeguro;
                    ControlCredito.CtoAdmon = valorAdmon;


                    ctx.Entry(TotalCredito).State = System.Data.Entity.EntityState.Modified;
                    ctx.Entry(ControlCredito).State = System.Data.Entity.EntityState.Modified;
                    ctx.SaveChanges();
                    string NuevoIC = ControlCredito.InteresCorriente.ToString("N0", formato);
                    string NuevoIM = ControlCredito.InteresMora.ToString("N0", formato);
                    string NuevoSeguro = ControlCredito.Seguro.ToString("N0", formato);
                    string NuevoAdmon = ControlCredito.CtoAdmon.ToString("N0", formato);
                    string NuevoValorCuota = ControlCredito.ValorCuota.ToString("N0", formato);

                    
                    return new JsonResult { Data = new { status = true, NuevoIC, NuevoIM,NuevoSeguro,NuevoAdmon, NuevoValorCuota } };
                }
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = new { status = false } };
                throw;
            }
        }

        public async Task<List<TotalesCreditos>> GetTotalesByNitCuentaAsync(string nit,string cuenta)
        {
            var list = new List<TotalesCreditos>(); 
            using (var ctx = new AccountingContext())
            {
                var result = await (from cr in ctx.Creditos
                              join t in ctx.TotalesCreditos on cr.Pagare equals t.Pagare
                              join c in ctx.Cuentas on cr.Destino_Id equals c.Destino_Id
                              where cr.Creditos_Cedula == nit && c.Cuenta_Cod == cuenta && t.Estado != "LQ"
                              select t).ToListAsync();

                foreach (var item in result)
                { 
                    var tc = new TotalesCreditos() { 
                         Id = item.Id,
                         Pagare = item.Pagare,
                         SaldoCapital = item.SaldoCapital,
                         InteresCorrientePendiente = item.InteresCorrientePendiente,
                         InteresMoraPendiente = item.InteresMoraPendiente,
                         SeguroPendiente = item.SeguroPendiente,
                         CtoAdmonPendiente = item.CtoAdmonPendiente
                    };
                    list.Add(tc);
                }
            }
            return list;
        }

        public async Task<ViewModelPagoCreditoMovimiento> PagoCreditoByMovimiento(string Pagare, decimal ValorConsignado, DateTime FechaPago, string tipoComprobante,string numeroComprobante,string ccosto)
        {
            var model = new ViewModelPagoCreditoMovimiento();
            var controlCreditoRemove =new  List<ControlCreditos>();
            var listMovimientos = new List<Movimiento>();   
            decimal VC = Convert.ToDecimal(ValorConsignado);
            decimal ValorAuxiliar = VC;
            decimal Cambio = 0, Capital = 0, IC = 0, IM = 0, Seguro = 0, CtoAdmon = 0, SaldoCapital = 0;
            bool BanderaCapital = true;
            using (var ctx = new DTO.AccountingContext())
            {
                try
                {
                    var InfoTercero = await ctx.Creditos.Where(x => x.Pagare == Pagare).FirstOrDefaultAsync();
                    var TotalCredito = await ctx.TotalesCreditos.Where(x => x.Pagare == Pagare).FirstOrDefaultAsync();
                    var ControlCreditos =await ctx.ControlCreditos.Where(x => x.Pagare == Pagare && x.EstadoEnCredito != "PZ").ToListAsync();
                    decimal AbonoNormal = ControlCreditos.Where(x => x.EstadoEnOperacion).Select(x => x.ValorCuota).Sum();
                    decimal AbonoCapital = ControlCreditos.Where(x => x.EstadoEnOperacion == false).Select(x => x.ValorCuota).Sum();
                    string DetalleComprobante = "";

                    if (VC <= AbonoNormal)
                    {
                        foreach (var item in ControlCreditos)
                        {
                            //seccion pago a interes mora
                            if (ValorAuxiliar > 0)
                            {
                                if (ValorAuxiliar > item.InteresMora)
                                {
                                    ValorAuxiliar -= item.InteresMora;
                                    item.ValorCuota -= item.InteresMora;

                                    TotalCredito.InteresMoraPendiente -= item.InteresMora;
                                    TotalCredito.InteresMoraTotal += item.InteresMora;

                                    IM += item.InteresMora;

                                    item.InteresMora = 0;
                                }
                                else
                                {
                                    item.InteresMora -= ValorAuxiliar;
                                    item.ValorCuota -= ValorAuxiliar;

                                    TotalCredito.InteresMoraPendiente -= ValorAuxiliar;
                                    TotalCredito.InteresMoraTotal += ValorAuxiliar;

                                    IM += ValorAuxiliar;

                                    ValorAuxiliar = 0;
                                }
                            }

                            //sección pago a interes corriente
                            if (ValorAuxiliar > 0)
                            {
                                if (ValorAuxiliar > item.InteresCorriente)
                                {
                                    ValorAuxiliar -= item.InteresCorriente;
                                    item.ValorCuota -= item.InteresCorriente;

                                    TotalCredito.InteresCorrientePendiente -= item.InteresCorriente;
                                    TotalCredito.InteresCorrienteTotal += item.InteresCorriente;

                                    IC += item.InteresCorriente;

                                    item.InteresCorriente = 0;
                                }
                                else
                                {
                                    item.InteresCorriente -= ValorAuxiliar;
                                    item.ValorCuota -= ValorAuxiliar;

                                    TotalCredito.InteresCorrientePendiente -= ValorAuxiliar;
                                    TotalCredito.InteresCorrienteTotal += ValorAuxiliar;

                                    IC += ValorAuxiliar;

                                    ValorAuxiliar = 0;
                                }
                            }

                            //sección pago a seguro
                            if (ValorAuxiliar > 0)
                            {
                                if (ValorAuxiliar > item.Seguro)
                                {
                                    ValorAuxiliar -= item.Seguro;
                                    item.ValorCuota -= item.Seguro;

                                    TotalCredito.SeguroPendiente -= item.Seguro;
                                    TotalCredito.SeguroTotal += item.Seguro;

                                    Seguro += item.Seguro;

                                    item.Seguro = 0;
                                }
                                else
                                {
                                    item.Seguro -= ValorAuxiliar;
                                    item.ValorCuota -= ValorAuxiliar;

                                    TotalCredito.SeguroPendiente -= ValorAuxiliar;
                                    TotalCredito.SeguroTotal += ValorAuxiliar;

                                    Seguro += ValorAuxiliar;

                                    ValorAuxiliar = 0;
                                }
                            }

                            //sección pago a costo administración
                            if (ValorAuxiliar > 0)
                            {
                                if (ValorAuxiliar > item.CtoAdmon)
                                {
                                    ValorAuxiliar -= item.CtoAdmon;
                                    item.ValorCuota -= item.CtoAdmon;

                                    TotalCredito.CtoAdmonPendiente -= item.CtoAdmon;
                                    TotalCredito.CtoAdmonTotal += item.CtoAdmon;

                                    CtoAdmon += item.CtoAdmon;

                                    item.CtoAdmon = 0;
                                }
                                else
                                {
                                    item.CtoAdmon -= ValorAuxiliar;
                                    item.ValorCuota -= ValorAuxiliar;

                                    TotalCredito.CtoAdmonPendiente -= ValorAuxiliar;
                                    TotalCredito.CtoAdmonTotal += ValorAuxiliar;

                                    CtoAdmon += ValorAuxiliar;

                                    ValorAuxiliar = 0;
                                }
                            }

                            //sección pago a capital
                            if (ValorAuxiliar > 0)
                            {

                                if (ValorAuxiliar > item.Capital)
                                {
                                    ValorAuxiliar -= item.Capital;
                                    item.ValorCuota -= item.Capital;

                                    TotalCredito.SaldoCapital -= item.Capital;
                                    TotalCredito.CapitalTotal += item.Capital;

                                    Capital += item.Capital;

                                    if (item.EstadoEnCredito == "EM")
                                    {
                                        TotalCredito.CapitalMoraPendiente -= item.Capital;
                                    }

                                    item.Capital = 0;

                                    item.EstadoEnCredito = "PZ";


                                }
                                else
                                {
                                    item.Capital -= ValorAuxiliar;
                                    item.ValorCuota -= ValorAuxiliar;

                                    TotalCredito.SaldoCapital -= ValorAuxiliar;
                                    TotalCredito.CapitalTotal += ValorAuxiliar;

                                    Capital += ValorAuxiliar;

                                    if (item.EstadoEnCredito == "EM")
                                    {
                                        TotalCredito.CapitalMoraPendiente -= ValorAuxiliar;
                                    }

                                    ValorAuxiliar = 0;

                                    if (item.Capital <= 0)
                                    {
                                        item.EstadoEnCredito = "PZ";
                                    }

                                }
                            }

                            

                        }//fin ciclo foreach

                    }
                    else if (VC > AbonoNormal && AbonoCapital > 0)
                    {
                        decimal SaldoCapitalAuxiliar = 0;
                        foreach (var item in ControlCreditos)
                        {
                            if (item.EstadoEnOperacion)
                            {
                                ValorAuxiliar -= item.ValorCuota;//disminuimos este valor por cada iteración, al final nos guardará el valor para realizar el abono a capital

                                Capital += item.Capital;
                                IC += item.InteresCorriente;
                                IM += item.InteresMora;
                                Seguro += item.Seguro;
                                CtoAdmon += item.CtoAdmon;

                                //modificamos valores en la tabla TotalesCreditos
                                TotalCredito.CapitalTotal += item.Capital;
                                TotalCredito.SaldoCapital -= item.Capital;
                                if (item.EstadoEnCredito == "EM")
                                {
                                    TotalCredito.CapitalMoraPendiente -= item.Capital;
                                }
                                TotalCredito.InteresCorrienteTotal += item.InteresCorriente;
                                TotalCredito.InteresMoraTotal += item.InteresMora;
                                TotalCredito.SeguroTotal += item.Seguro;
                                TotalCredito.CtoAdmonTotal += item.CtoAdmon;
                                TotalCredito.InteresCorrientePendiente -= item.InteresCorriente;
                                TotalCredito.InteresMoraPendiente -= item.InteresMora;
                                TotalCredito.SeguroPendiente -= item.Seguro;
                                TotalCredito.CtoAdmonPendiente -= item.CtoAdmon;

                                //modificamos la fila perteneciente a la cuota
                                item.Capital = 0;
                                item.InteresCorriente = 0;
                                item.InteresMora = 0;
                                item.Seguro = 0;
                                item.CtoAdmon = 0;
                                item.ValorCuota = 0;
                                item.DiasMora = 0;
                                item.EstadoEnCredito = "PZ";


                                
                            }
                            else
                            {
                                //en este else, se restructura las tuplas en estado false debido al abono capital
                                if (BanderaCapital)
                                {//condicional en el cual sólo debe ingresar una vez: se debe disminuir el valor de la primera tupla al saldo capital actual
                                    Capital += ValorAuxiliar;
                                    item.SaldoCapitalEnCuota -= ValorAuxiliar;
                                    TotalCredito.SaldoCapital -= ValorAuxiliar;
                                    TotalCredito.CapitalTotal += ValorAuxiliar;
                                    BanderaCapital = false;
                                }
                                else
                                {
                                    item.SaldoCapitalEnCuota = SaldoCapitalAuxiliar;
                                }

                                SaldoCapitalAuxiliar = item.SaldoCapitalEnCuota - item.Capital;
                                if (SaldoCapitalAuxiliar < 0)
                                {
                                    item.Capital = item.SaldoCapitalEnCuota;
                                    item.ValorCuota = item.Capital;
                                }

                                if (item.SaldoCapitalEnCuota <= 0)
                                {
                                    controlCreditoRemove.Add(item);
                                }

                            }
                        }
                    }

                    //verificamos si el saldo capital es 0 entonces el crédito se liquida
                    if (TotalCredito.SaldoCapital <= 0)
                    {
                        TotalCredito.SaldoCapital = 0;
                        TotalCredito.Estado = "LQ";
                        TotalCredito.DiasMora = 0;
                    }
                    
                    SaldoCapital = TotalCredito.SaldoCapital;
                    

                    //creamos el comprobante
                    var InfoCuentas = await ctx.Cuentas.ToListAsync();
                    

                    //creamos los movimientos
                    //capital
                    if (Capital > 0)
                    {
                        var CuentaCapital = InfoCuentas.Where(x => x.Funcion == "F1" && InfoTercero.Destino_Id == x.Destino_Id).FirstOrDefault();
                        var mov2 = new Movimiento()
                        {
                            TIPO = tipoComprobante,
                            NUMERO = numeroComprobante,
                            CUENTA = CuentaCapital.Cuenta_Cod,
                            TERCERO = InfoTercero.Creditos_Cedula,
                            DETALLE = "CAPITAL",
                            DEBITO = 0,
                            CREDITO = Capital,
                            BASE = 0,
                            CCOSTO = ccosto,
                            FECHAMOVIMIENTO = FechaPago,
                            DOCUMENTO = ""
                        };
                        listMovimientos.Add(mov2);
                    }
                    //interes corriente
                    if (IC > 0)
                    {
                        var CuentaInteres = InfoCuentas.Where(x => x.Funcion == "F4" && InfoTercero.Destino_Id == x.Destino_Id).FirstOrDefault(); ;
                        var mov3 = new Movimiento()
                        {
                            TIPO = tipoComprobante,
                            NUMERO = numeroComprobante,
                            CUENTA = CuentaInteres.Cuenta_Cod,
                            TERCERO = InfoTercero.Creditos_Cedula,
                            DETALLE = "INTERES CORRIENTE",
                            DEBITO = 0,
                            CREDITO = IC,
                            BASE = 0,
                            CCOSTO = ccosto,
                            FECHAMOVIMIENTO = FechaPago,
                            DOCUMENTO = ""
                        };
                        listMovimientos.Add(mov3);
                    }
                    //interes mora
                    if (IM > 0)
                    {
                        var CuentaIM = InfoCuentas.Where(x => x.Funcion == "F6" && InfoTercero.Destino_Id == x.Destino_Id).FirstOrDefault();
                        var mov4 = new Movimiento()
                        {
                            TIPO = tipoComprobante,
                            NUMERO = numeroComprobante,
                            CUENTA = CuentaIM.Cuenta_Cod,
                            TERCERO = InfoTercero.Creditos_Cedula,
                            DETALLE = "INTERES MORA",
                            DEBITO = 0,
                            CREDITO = IM,
                            BASE = 0,
                            CCOSTO = ccosto,
                            FECHAMOVIMIENTO = FechaPago,
                            DOCUMENTO = ""
                        };
                        listMovimientos.Add(mov4);
                    }
                    //seguro
                    if (Seguro > 0)
                    {
                        var CuentaSeguro = InfoCuentas.Where(x => x.Funcion == "F9" && InfoTercero.Destino_Id == x.Destino_Id).FirstOrDefault();
                        var mov5 = new Movimiento()
                        {
                            TIPO = tipoComprobante,
                            NUMERO = numeroComprobante,
                            CUENTA = CuentaSeguro.Cuenta_Cod,
                            TERCERO = InfoTercero.Creditos_Cedula,
                            DETALLE = "SEGURO",
                            DEBITO = 0,
                            CREDITO = Seguro,
                            BASE = 0,
                            CCOSTO = ccosto,
                            FECHAMOVIMIENTO = FechaPago,
                            DOCUMENTO = ""
                        };
                        listMovimientos.Add(mov5);
                    }
                    //Costo administración
                    if (CtoAdmon > 0)
                    {
                        var CuentaSeguro = InfoCuentas.Where(x => x.Funcion == "F15" && InfoTercero.Destino_Id == x.Destino_Id).FirstOrDefault();
                        var mov6 = new Movimiento()
                        {
                            TIPO = tipoComprobante,
                            NUMERO = numeroComprobante,
                            CUENTA = CuentaSeguro.Cuenta_Cod,
                            TERCERO = InfoTercero.Creditos_Cedula,
                            DETALLE = "COSTO ADMINISTRACION",
                            DEBITO = 0,
                            CREDITO = CtoAdmon,
                            BASE = 0,
                            CCOSTO = ccosto,
                            FECHAMOVIMIENTO = FechaPago,
                            DOCUMENTO = ""
                        };
                        listMovimientos.Add(mov6);
                    }


                    //Creamos la factura
                    string NFactura = "1-"+tipoComprobante+"-"+numeroComprobante+"-"+ccosto;
                    var Factura = new factOpCajaConsCuotaCredito
                    {
                        fecha = FechaPago,
                        factura = NFactura,
                        NIT = InfoTercero.Creditos_Cedula,
                        codigoCaja = "1",//usuario sistema
                        pagare = Pagare,
                        valorConsignado = Convert.ToInt32(VC).ToString(),
                        nitCajero = "1",//usuario sistema
                        abonoCapital = Convert.ToInt32(Capital).ToString(),
                        numeroCuota = "Abono",
                        interesCorriente = Convert.ToInt32(IC).ToString(),
                        interesMora = Convert.ToInt32(IM).ToString(),
                        seguros = Convert.ToInt32(Seguro).ToString(),
                        CtoAdmon = Convert.ToInt32(CtoAdmon).ToString(),
                        saldoCapital = Convert.ToInt32(SaldoCapital).ToString(),
                        FormaPago = "CON",//consignación
                        TipoComprobante = tipoComprobante,
                        NumeroComprobante = numeroComprobante
                    };
                    

                    
                    //actualizamos la tabla control creditos
                    var cc = ControlCreditos.Where(x => x.Pagare == Pagare && x.EstadoEnCredito != "PZ").ToList();
                    //verificamos si el crédito está en mora
                    
                    var cm = cc.Where(x => x.EstadoEnCredito == "EM").ToList();
                    var normal = cc.Where(x => x.EstadoEnCredito == "DT" || x.EstadoEnCredito == "AD").ToList();
                    if (TotalCredito.Estado != "LQ")
                    {
                        if (cm.Count > 0)
                        {
                            TotalCredito.Estado = "EM";
                            TotalCredito.DiasMora = cm.Select(x => x.DiasMora).FirstOrDefault();
                            TotalCredito.FechaProximoPago = cm.Select(x => x.FechaPago).FirstOrDefault();
                        }
                        else
                        {
                            TotalCredito.Estado = "AD";
                            TotalCredito.DiasMora = 0;
                            TotalCredito.FechaProximoPago = normal.Select(x => x.FechaPago).FirstOrDefault();
                        }
                    }
                    model.Movimientos = listMovimientos;
                    model.TotalesCreditos = TotalCredito;
                    model.Factura = Factura;
                    model.ControlCreditos = ControlCreditos;
                    model.ControlCreditoRemove = controlCreditoRemove;
                    model.correcto = true;
                }
                catch (Exception ex)
                {
                    model.correcto = false;
                }
            }
            return  model;
        }

    }
}
