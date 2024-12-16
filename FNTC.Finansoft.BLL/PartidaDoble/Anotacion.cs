/*  
*/

using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.BLL
{
    [Serializable]
    public class Anotacion
    {
        public int Index { get; set; }
        //esto deberia tener una Auxiliar
        private Movimiento movimiento;

        public string Cuenta
        { get { return movimiento.CUENTA; } set { movimiento.CUENTA = value; } }


        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal Debito
        { get { return movimiento.DEBITO; } set { movimiento.DEBITO = value; } }
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal Credito
        { get { return movimiento.CREDITO; } set { movimiento.CREDITO = value; } }
        [DisplayFormat(DataFormatString = "{0:0.##}")]
        public decimal Base { get { return movimiento.BASE; } set { movimiento.BASE = value; } }

        public string CentroDeCosto { get { return movimiento.CCOSTO; } set { movimiento.CCOSTO = value; } }
        public string Descripcion { get { return movimiento.DETALLE; } set { movimiento.DETALLE = value; } }
        public string Tercero { get { return movimiento.TERCERO; } set { movimiento.TERCERO = value; } }
        public string cuentaPagare { get { return movimiento.cuentaPagare; } set { movimiento.cuentaPagare = value; } }

        public DateTime FechaMovimiento { get { return movimiento.FECHAMOVIMIENTO; } set { movimiento.FECHAMOVIMIENTO = value; } }

        public bool VerificaSaldos { get; set; }

        public bool NeedsVerification { get; private set; } // Requiere verificar la anotacion
        private bool isValid;


        public Anotacion(string cuenta, decimal debito, decimal credito, decimal _base, string cc, string descripcion, string tercero, string cuentaPagare)
        {
            movimiento = new Movimiento();

            this.Cuenta = cuenta;
            this.Debito = debito;
            this.Credito = credito;
            this.Base = _base;
            this.CentroDeCosto = cc;
            this.Descripcion = descripcion;
            this.Tercero = tercero;
            this.cuentaPagare = cuentaPagare;
        }


        public Anotacion()
        {
            this.movimiento = new Movimiento();
        }
        /// <summary>
        ///  Verifica la valides de la anotacion/apunto contable
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (NeedsVerification)
                    isValid = Verify(new List<string>());
                return isValid;
            }
            private set
            {
                isValid = value;
            }
        }

        public Anotacion(string account, decimal debit, decimal credit, string costcntr, string description, string tercero, DateTime fechaMovimiento)
        {
            movimiento = new Movimiento();
            movimiento.CUENTA = account;
            movimiento.DEBITO = debit;
            movimiento.CREDITO = credit;
            movimiento.CCOSTO = costcntr;
            movimiento.DETALLE = description;
            movimiento.TERCERO = tercero;
            movimiento.FECHAMOVIMIENTO = fechaMovimiento;


#warning tambien  debo actualizar SaldosCuenras
        }

        public Movimiento GetMovimiento()
        {
            return this.movimiento;
        }

        public bool Verify(List<string> errors)
        {

            var cta = new AccountingContext().PlanCuentas.Find(Cuenta);


            // TODO: verificar si la cuenta existe en la bd y no es una cuenta mayot
            // para esto solo traigo cuentas de saldosCuientas
            //


#warning Verificar si las cuentas requieren centro de costo
#warning Consutar si cuenta requiere tercvero
#warning Consutar si existe
#warning Consutar si es grupo
#warning Consutar si es mayor
#warning Consutar que aunque sea subcuenta no contiene auxiliar o asi


            if ((Debito == 0 && Credito == 0))
                errors.Add("Debito y/o Credito no pueden estar en 0 ");

            if (string.IsNullOrEmpty(Cuenta))
                errors.Add("el campo  Cuenta no puede estar vacia ");


            //if (string.IsNullOrEmpty(Cuenta))
            //    errors.Add("La cuenta requiere CC.");

            NeedsVerification = errors.Count != 0;
            isValid = errors.Count == 0;

            return isValid;
        }

        public Anotacion GetReverse()
        {
            return new Anotacion(Cuenta, Credito, Debito, CentroDeCosto, Descripcion, Tercero, FechaMovimiento);
        }


        public Anotacion GetAnotacionFromMovimiento(Movimiento movimiento)
        {



            return new Anotacion(movimiento.CUENTA, movimiento.DEBITO, movimiento.CREDITO, movimiento.CCOSTO, movimiento.DETALLE, movimiento.TERCERO, movimiento.FECHAMOVIMIENTO);
        }



    }
}
