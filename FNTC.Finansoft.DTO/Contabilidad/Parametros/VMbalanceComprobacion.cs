using FNTC.Finansoft.Accounting.DTO.Terceros;

namespace FNTC.Finansoft.Accounting.DTO.Contabilidad.Parametros
{
    public class VMbalanceComprobacion
    {

        public string codigoCuenta { set; get; }
        public string nombreCuenta { set; get; }


        public string tercero { set; get; }
        public string nombreTercero { set; get; }

        public decimal saldoAnterior { set; get; }
        public decimal debito { set; get; }
        public decimal credito { set; get; }
        public decimal saldo { set; get; }


        public virtual Tercero terceroFK { get; set; }
        public virtual CuentaMayor cuentaFK { get; set; }
    }
}
