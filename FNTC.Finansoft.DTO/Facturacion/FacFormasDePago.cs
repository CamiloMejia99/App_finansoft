using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Facturacion
{
    [Table("fac.FormasDePago")]
    public class FacFormasDePago
    {
        [Key]
        public int id { get; set; }

        public string nombre { get; set; }
    }
}
