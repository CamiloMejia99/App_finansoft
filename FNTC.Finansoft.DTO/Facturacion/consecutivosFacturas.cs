using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Facturacion
{
    [Table("fac.consecutivosFacturas")]
    public class consecutivosFacturas
    {
        [Key]
        public int id { get; set; }
        public string cod { get; set; }
        public string descripcion { get; set; }
        public int desde { get; set; }
        public int hasta { get; set; }
        public int actual { get; set; }
    }
}
