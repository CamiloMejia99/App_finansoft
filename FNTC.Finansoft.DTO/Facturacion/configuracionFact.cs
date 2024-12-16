using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Facturacion
{
    [Table("dbo.configuracionFact")]
    public class configuracionFact
    {
        public int id { get; set; }

        public string codConsecutivo { get; set; }
        public int inicio { get; set; }
        public int final { get; set; }
        public int consecutivoActual { get; set; }
    }
}
