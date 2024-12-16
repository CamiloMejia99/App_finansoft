using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Contabilidad
{
    [Table("acc.ClaseComprobante")]
    public class ClaseComprobante
    {
        [Key]
        public string Codigo { get; set; }
        public string Nombre { get; set; }
    }
}
