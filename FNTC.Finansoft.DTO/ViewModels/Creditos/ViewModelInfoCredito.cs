using FNTC.Finansoft.Accounting.DTO.MCreditos;
using System.Collections.Generic;

namespace FNTC.Finansoft.Accounting.DTO.ViewModels.Creditos
{
    public class ViewModelInfoCredito
    {
        public string Pagare { get; set; }

        //datos del cajero
        public string CodigoCaja { get; set; }
        public string NitCajero { get; set; }
        public string NombreCajero { get; set; }

        //datos del usuario
        public string NitUsuario { get; set; }
        public string NombreUsuario { get; set; }

        //datos del crédito
        public string SaldoCapital { get; set; }
        public string CapitalMora { get; set; }
        public string ICPendiente { get; set; }
        public string IMPendiente { get; set; }
        public string SeguroPendiente { get; set; }
        public string CtoAdmonPendiente { get; set; }
        public string Estado { get; set; }
        public string TotalCreditoPendiente { get; set; }
        public string TotalCreditoLiquidar { get; set; }
        public string CuotaActual { get; set; }

        public List<ControlCreditos> Cuotas { get; set; }

    }
}
