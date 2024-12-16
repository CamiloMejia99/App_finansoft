using FNTC.Finansoft.Accounting.DAL.Comprobantes;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FNTC.Finansoft.Accounting.BLL.Comprobantes
{
    public class ComprobantesBLL
    {

        private ComprobanteBO comprobante;

        public ComprobanteBO GetNuevoComprobante(string tipo)
        {
            comprobante = new ComprobanteBO(tipo);

            return comprobante;
        }


        public ComprobanteBO GetComprobante(string tipo, string numero)
        {
            var comprobanteBO = new ComprobanteBO();
            comprobanteBO = this.mapToBO(tipo, numero, comprobanteBO);
            return comprobanteBO;
        }

        public ComprobanteBO GetComprobanteEditado(string tipo, string numero, string vecesEditado)
        {
            var comprobanteBO = new ComprobanteBO();
            comprobanteBO = this.mapToBOEditado(tipo, numero, vecesEditado, comprobanteBO);
            return comprobanteBO;
        }

        public List<Comprobante> GetComprobantes(string clase = "", DateTime? fechaInicial = null, DateTime? fechaFinal = null)
        {
            using (var ctx = new AccountingContext())
            {

                //filtrado
                var comprobantes = ctx.Comprobantes.ToList();
                return comprobantes;
            }

        }

        //nuevo
        public List<Comprobante> SPGetComprobantes(string tipo, string fechaDesde, string fechaHasta)
        {
            var result = new ComprobantesDAL().GetComprobantes(tipo, fechaDesde, fechaHasta);
            return result;
        }


        public bool ActualizarSaldos(ref AccountingContext ctx)
        {

            var movimientos = ctx.Movimientos;

            this.ActualizarSaldosCuentas(ref ctx, movimientos);
            this.ActualizarSaldosCCs(ref ctx);
            this.ActualizarSaldosTerceros(ref ctx);

            return false;
        }
        //esto debe usar una lista en vez del contextoy hace una sola transaccion al final


        private bool ActualizarSaldosCuentas(ref AccountingContext ctx, DbSet<Movimiento> movimientos)
        {
            var grouped = movimientos.ToList().OrderBy(m => m.FECHAMOVIMIENTO)
                .GroupBy(g => new { g.FECHAMOVIMIENTO.Year, g.FECHAMOVIMIENTO.Month, g.CUENTA });

            ctx.SaldosCuentas.RemoveRange(ctx.SaldosCuentas);
            var rows = ctx.SaveChanges();
            decimal saldo = 0;
            foreach (var item in grouped)
            {


                var mcreditos = item.Sum(m => m.CREDITO);
                var mdebitos = item.Sum(m => m.DEBITO);

                var mes = item.Key.Month;
                var ano = item.Key.Year;
                var cuenta = item.Key.CUENTA;
                decimal saldoAnterior = saldo = 0;


                var movSaldoAnterior = ctx.SaldosCuentas.Where(m => m.CODIGO == cuenta && m.MES < mes).OrderByDescending(d => new { d.ANO }).FirstOrDefault();
                saldoAnterior = movSaldoAnterior == null ? 0 : movSaldoAnterior.SALDO;

                saldo = saldoAnterior + mdebitos - mcreditos;

                ctx.SaldosCuentas.Add(
                    //var saldoCta = 
                    new SaldoCuenta()
                    {
                        ANO = ano,
                        MES = mes,
                        MDEBITO = mdebitos,
                        MCREDITO = mcreditos,
                        CODIGO = cuenta,
                        SALDO = saldo
                    }
                                );

                rows = ctx.SaveChanges();
            }
            rows = ctx.SaveChanges();
            return false;
        }

        private bool ActualizarSaldosCCs(ref AccountingContext ctx)
        {
            return false;
        }
        private bool ActualizarSaldosTerceros(ref AccountingContext ctx)
        {
            return false;
        }
        private bool Mayorizar(ref AccountingContext ctx)
        {
            return false;
        }


        private ComprobanteBO mapToBO(string tipo, string numero, ComprobanteBO comprobanteBO)
        {
            //var comprobanteBO = new ComprobanteBO(tipo);
            using (var ctx = new AccountingContext())
            {
                var _comprobante = ctx.Comprobantes.FirstOrDefault(c => c.TIPO.Equals(tipo) && c.NUMERO.Equals(numero));
                if (_comprobante != null)
                {

                    var entradasa = ctx.Movimientos.Where(m => m.TIPO == tipo && m.NUMERO == numero).ToList();

                    //                    comprobanteBO = new ComprobanteBO(
                    // entries: entradasa,
                    //   comprobanteBO.TipoComprobante=_comprobante.TIPO;
                    //  var fc =
                    comprobanteBO.FechaComprobante = new DateTime(
                            Int32.Parse(_comprobante.ANO),
                            Int32.Parse(_comprobante.MES),
                            Int32.Parse(_comprobante.DIA));

                    comprobanteBO.Detalle = _comprobante.DETALLE;
                    comprobanteBO.CCosto = _comprobante.CCOSTO;
                    comprobanteBO.FPago = _comprobante.FPAGO;
                    comprobanteBO.NumeroExterno = _comprobante.NUMEXTERNO == null ? "" : _comprobante.NUMEXTERNO;
                    comprobanteBO.TipoComprobante = _comprobante.TIPO;
                    comprobanteBO.Consecutivo = _comprobante.NUMERO;
                    comprobanteBO.Anulado = _comprobante.ANULADO;

                    var clase = comprobanteBO.Clase;
                    comprobanteBO.Tercero = _comprobante.TERCERO;




                    Anotacion a = new Anotacion();
                    var index = 1;
                    /*
                    if (clase == "CE" || clase == "RC")
                    {
                        //la forma de pago va en el index 1 si es Ce o RC
                        var ctaFP = ctx.FormasPago.Find(Int32.Parse(comprobanteBO.FPago)).CodigoCuenta;
                        var FP = entradasa.First(cta => cta.CUENTA == ctaFP);
                        entradasa.Remove(FP);

                        comprobanteBO.AddEntry(new Anotacion()
                        {
                            Index = index++,
                            Cuenta = FP.CUENTA,
                            Descripcion = FP.DETALLE,

                            Tercero = FP.TERCERO,
                            CentroDeCosto = FP.CCOSTO,
                            Base = FP.BASE,
                            Debito = FP.DEBITO,
                            Credito = FP.CREDITO
                        }
                      );                      
                    }
                    */


                    foreach (var item in entradasa)
                    {

                        //map to Anotacion
                        a = new Anotacion()
                        {
                            Index = index++,
                            Cuenta = item.CUENTA,
                            Descripcion = item.DETALLE,

                            Tercero = item.TERCERO,
                            CentroDeCosto = item.CCOSTO,
                            Base = item.BASE,
                            Debito = item.DEBITO,
                            Credito = item.CREDITO

                        };
                        comprobanteBO.AddEntry(a);
                    }

                }
            }
            return comprobanteBO;
        }

        private ComprobanteBO mapToBOEditado(string tipo, string numero, string vecesEditado, ComprobanteBO comprobanteBO)
        {
            //var comprobanteBO = new ComprobanteBO(tipo);
            using (var ctx = new AccountingContext())
            {
                var _comprobante = ctx.ComprobantesEditadosAC.FirstOrDefault(c => c.TIPO.Equals(tipo) && c.NUMERO.Equals(numero) && c.NUMEROEDITADO.Equals(vecesEditado));
                if (_comprobante != null)
                {

                    var entradasa = ctx.MovimientosEditadosAC.Where(m => m.TIPO == tipo && m.NUMERO == numero && m.NUMEROEDITADO == vecesEditado).ToList();

                    //                    comprobanteBO = new ComprobanteBO(
                    // entries: entradasa,
                    //   comprobanteBO.TipoComprobante=_comprobante.TIPO;
                    //  var fc =
                    comprobanteBO.FechaComprobante = new DateTime(
                            Int32.Parse(_comprobante.ANO),
                            Int32.Parse(_comprobante.MES),
                            Int32.Parse(_comprobante.DIA));

                    comprobanteBO.Detalle = _comprobante.DETALLE;
                    comprobanteBO.CCosto = _comprobante.CCOSTO;
                    comprobanteBO.FPago = _comprobante.FPAGO;
                    comprobanteBO.NumeroExterno = _comprobante.NUMEXTERNO == null ? "" : _comprobante.NUMEXTERNO;
                    comprobanteBO.TipoComprobante = _comprobante.TIPO;
                    comprobanteBO.Consecutivo = _comprobante.NUMERO;
                    comprobanteBO.Anulado = _comprobante.ANULADO;

                    var clase = comprobanteBO.Clase;
                    comprobanteBO.Tercero = _comprobante.TERCERO;




                    Anotacion a = new Anotacion();
                    var index = 1;

                    if (clase == "CE" || clase == "RC")
                    {
                        //la forma de pago va en el index 1 si es Ce o RC
                        var ctaFP = ctx.FormasPago.Find(Int32.Parse(comprobanteBO.FPago)).CodigoCuenta;
                        var FP = entradasa.First(cta => cta.CUENTA == ctaFP);
                        entradasa.Remove(FP);

                        comprobanteBO.AddEntry(new Anotacion()
                        {
                            Index = index++,
                            Cuenta = FP.CUENTA,
                            Descripcion = FP.DETALLE,

                            Tercero = FP.TERCERO,
                            CentroDeCosto = FP.CCOSTO,
                            Base = FP.BASE,
                            Debito = FP.DEBITO,
                            Credito = FP.CREDITO
                        }
                      );

                    }


                    foreach (var item in entradasa)
                    {

                        //map to Anotacion
                        a = new Anotacion()
                        {
                            Index = index++,
                            Cuenta = item.CUENTA,
                            Descripcion = item.DETALLE,

                            Tercero = item.TERCERO,
                            CentroDeCosto = item.CCOSTO,
                            Base = item.BASE,
                            Debito = item.DEBITO,
                            Credito = item.CREDITO

                        };
                        comprobanteBO.AddEntry(a);
                    }

                }
            }
            return comprobanteBO;
        }
    }
}
