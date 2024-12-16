using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.LiquidacionesDefinitivas
{
    [Table("dbo.LiquidacionDefinitiva")]
    public class LiquidacionDefinitivaAso
    {
        public int id { get; set; }
        public string fechaLiquidacion { get; set; }
        public string NIT { get; set; }
        public string agencia { get; set; }
        public string asesor { get; set; }
        public string totalAhorros { get; set; }
        public string aportesSociales { get; set; }
        public string totalCreditos { get; set; }
        public string creditoConsumo { get; set; }
        public string saldoAFavor { get; set; }
    }
}
