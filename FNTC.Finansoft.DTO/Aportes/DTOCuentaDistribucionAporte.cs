using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.Aportes
{
    public class DTOCuentaDistribucionAporte
    {
        public string Id { get; set; }
        public string Cuenta { get; set; }
        public string NombreCuenta { get; set; }

        public string Porcentaje { get; set; }

        public bool Estado { get; set; }
    }
}
