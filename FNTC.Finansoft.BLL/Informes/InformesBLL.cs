using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.BLL.Informes
{
    public class InformesBLL
    {
        public void BalanceDetallado(DateTime pi, DateTime pf, int[] niveles, bool verCodigos=true, bool verTerceros = true, bool verCC = true)
        {
            pf = DateTime.Now;
            pi = new DateTime(2015, 12, 30);
            niveles = new int[] { 1, 2, 4, 6, 9 };
            verCodigos = true;


            var saldos = MayorizarGroupBy(niveles);
            //cuentas 1 a 3 totalizadas a 
            //cuentas del 1
            //return View();

        }

        public static string MayorizarGroupBy(int[] levels)
        {
            var cuentas = new List<Movimiento>();
            using (var ctx = new AccountingContext())
            {
                foreach (var item in levels)
                {
                    if (item == 3 || item == 5 || item == 7 || item == 8)
                        continue;
                    var clase = ctx.Movimientos.GroupBy(m => m.CUENTA.Substring(0, item));
                    foreach (var cta in clase)
                    {
                        var debitos = cta.Sum(it => it.DEBITO);
                        var creditos = cta.Sum(it => it.CREDITO);
                        cuentas.Add(new Movimiento() { CUENTA = cta.Key, DEBITO = debitos, CREDITO = creditos });
                    }
                }

                var sb = new StringBuilder();
                cuentas.
                    //OrderBy(c => c.CUENTA.Length).
                    OrderBy(c => c.CUENTA).
                    ThenBy(cueo => cueo.CUENTA).
                    ToList().
                    //ForEach(cue => Console.WriteLine(" {0,5} {1,10} DEBITO {2,15} CREDITO {3,15}",
                     ForEach(cue => sb.AppendFormat(" {0,5} {1,10} DEBITO {2,15} CREDITO {3,15}",
                        ctx.PlanCuentas.Find(cue.CUENTA).NOMBRE,
                        cue.CUENTA, cue.DEBITO, cue.CREDITO));


                return sb.ToString();
            }
        }

        public static List<BalanceModel> MayorizarByClase(string clase)
        {
            var cuentas = new List<Movimiento>();
            using (var ctx = new AccountingContext())
            {
                foreach (var item in ctx.Movimientos.Where( m => m.CUENTA.StartsWith(clase)))
                {
                 
                    var clases = ctx.Movimientos.GroupBy(m => m.CUENTA.Substring(0,1));
                    foreach (var cta in clases)
                    {
                        var debitos = cta.Sum(it => it.DEBITO);
                        var creditos = cta.Sum(it => it.CREDITO);
                        cuentas.Add(new Movimiento() { CUENTA = cta.Key, DEBITO = debitos, CREDITO = creditos });
                    }
                }


                List<BalanceModel> balance  = new List<BalanceModel>();
                cuentas.
                    //OrderBy(c => c.CUENTA.Length).
                    OrderBy(c => c.CUENTA).
                    ThenBy(cueo => cueo.CUENTA).
                    ToList().
                    //ForEach(cue => Console.WriteLine(" {0,5} {1,10} DEBITO {2,15} CREDITO {3,15}",
                    //    ctx.PlanCuentas.Find(cue.CUENTA).NOMBRE,
                    //    cue.CUENTA, cue.DEBITO, cue.CREDITO));
                    ForEach(cue => balance.Add(new BalanceModel() 
                        {
                            Codigo  = cue.CUENTA,
                            Nombre = ctx.PlanCuentas.Find(cue.CUENTA).NOMBRE,
                            Saldo = ctx.PlanCuentas.Find(cue.CUENTA).NATURALEZA.Equals('D')?cue.CREDITO - cue.DEBITO:cue.DEBITO - cue.CREDITO,
                            SubTotal = 0

                        }
                    ));

                return balance;
            }


        }


        private static void BalanceGeneral()
        {
            using (var ctx = new AccountingContext())
            {

            }
            
        }

        public class BalanceModel
           {
               public string Codigo { get; set; }
               public string Nombre { get; set; }
               public decimal Saldo { get; set; }
               public decimal SubTotal { get; set; }
              // public int MyProperty { get; set; }
        }

        public class EstadodeSituacionFinancieraModel 
        {
            public string Codigo { get; set; }
            public string Nombre { get; set; }
            public decimal Saldo { get; set; }
            public decimal SubTotal { get; set; }
            // public int MyProperty { get; set; }
        }

        private static void EstadodeSituacionFinanciera()
        {
            using (var ctx = new AccountingContext())
            {

            }

        }

    }
}
