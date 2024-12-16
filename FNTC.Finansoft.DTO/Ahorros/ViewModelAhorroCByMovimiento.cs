using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;

namespace FNTC.Finansoft.Accounting.DTO.Ahorros
{
    public class ViewModelAhorroCByMovimiento
    {
        public bool Correcto { get; set; }
        public FactOpcaja Factura { get; set; }

        public Movimiento Movimiento { get; set; }
        public FichaAhorroContractual FichaAhorroC { get; set; }
    }
}
