using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.EstadoDeCuenta
{
    public class ViewModelAporteEstadoCuenta
    {
        public string Cuenta { get; set; }
        public string FechaApertura { get; set; }
        public string Cuota { get; set; }
        public string SaldoActual { get; set; }
        public string SaldoMora { get; set; }
        public string Intereses { get; set; }
        public string SaldoCanje { get; set; }
        public string Estado { get; set; }

        //public ViewModelAporteEstadoCuenta()
        //{
        //}

        //public ViewModelAporteEstadoCuenta(string Cuenta,string FechaApertura,string Cuota ,string SaldoActual,string SaldoMora,string Intereses,string SaldoCanje ,string Estado)
        //{ 
        //    this.Cuenta = Cuenta;
        //    this.FechaApertura = FechaApertura;
        //    this.Cuota = Cuota;
        //    this.SaldoActual = SaldoActual;
        //    this.SaldoMora = SaldoMora;
        //    this.Intereses = Intereses;
        //    this.SaldoCanje = SaldoCanje;
        //    this.Estado = Estado;   
        //}

    }
}
