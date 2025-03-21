using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.Informes
{
    public class BalanceComprobacionActualizado
    {
        public string CUENTA { get; set; }
        public string TERCERO { get; set; }
        public string NOMBRE { get; set; }
        public decimal DebitoAnterior { get; set; }
        public decimal CreditoAnterior { get; set; }
        public decimal DebitoAntAux { get; set; }
        public decimal CreditoAntAux { get; set; }
        public decimal DebitoActual { get; set; }
        public decimal CreditoActual { get; set; }

    }
}
