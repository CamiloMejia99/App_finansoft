using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.Fichas;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.Aportes
{
    public class ViewModelAporteByMovimiento
    {
        public bool Correcto { get; set; }

        public FactOpcaja Factura { get; set; }

        public Movimiento Movimiento { get; set; }
        public FichasAportes FichaAporte { get; set; }
    }
}
