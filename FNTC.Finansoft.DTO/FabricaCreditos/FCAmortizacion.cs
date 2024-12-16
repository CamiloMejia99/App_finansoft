using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("Fcred.Amortizacion")]
    public class FCAmortizacion
    {
        public string mes { get; set; }
        public string saldo { get; set; }
        public string amortizacion { get; set; }
        public string interes { get; set; }
        public string cuota { get; set; }
    }
}
