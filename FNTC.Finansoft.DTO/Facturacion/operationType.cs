using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Facturacion
{
    [Table("fac.operationType")]
    public class operationType
    {
        public int id { get; set; }
        public string name { get; set; }
        public int tipo { get; set; }
    }
}