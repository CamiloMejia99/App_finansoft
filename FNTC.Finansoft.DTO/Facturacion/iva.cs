using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Facturacion
{
    [Table("fac.iva")]
    public class iva
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal value { get; set; }
    }
}
