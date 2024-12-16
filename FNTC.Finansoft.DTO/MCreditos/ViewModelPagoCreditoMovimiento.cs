using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class ViewModelPagoCreditoMovimiento
    {
        public bool correcto { get; set; }

        public TotalesCreditos TotalesCreditos { get; set; }

        public List<ControlCreditos> ControlCreditos { get; set; }


        public List<Movimiento> Movimientos { get; set; }

        public factOpCajaConsCuotaCredito Factura { get; set; }


        public List<ControlCreditos> ControlCreditoRemove { get; set; }

    }
}
