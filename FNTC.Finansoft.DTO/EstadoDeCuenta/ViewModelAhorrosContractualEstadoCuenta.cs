using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.EstadoDeCuenta
{
    public class ViewModelAhorrosContractualEstadoCuenta
    {
        public string Cuenta { get; set; }
        public string TipoAhorro  { get; set; }
        public string Plazo  { get; set; }
        public string FechaApertura  { get; set; }
        public string FechaVencimiento  { get; set; }
        public string TEM { get; set; }
        public string ValorCuota { get; set; }
        public string TotalAhorros { get; set; }
        public string Rendimientos { get; set; }
        public string SaldoTotal { get; set; }
        public string Estado { get; set; }
    }
}
