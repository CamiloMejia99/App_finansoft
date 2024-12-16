using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.EstadoDeCuenta
{
    public class ViewModelAporteExtraEstadoCuenta
    {
        public string Cuenta { get; set; }
        public string FechaApertura { get; set; }
        public string SaldoActual { get; set; }
        public string Estado { get; set; }
    }
}
