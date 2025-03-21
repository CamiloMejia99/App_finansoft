using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.Informes
{
    public class SpAuxiliarPorTercero
    {
        public string CUENTA { get; set; }
        public string NOMBRECUENTA { get; set; }
        public string TERCERO { get; set; }
        public string NOMBRE { get; set; }
        public string COMPROBANTE { get; set; }
        public DateTime FECHAMOVIMIENTO { get; set; }
        public Decimal DEBITO { get; set; }
        public Decimal CREDITO { get; set; }
        public string NATURALEZA { get; set; }

    }
}
