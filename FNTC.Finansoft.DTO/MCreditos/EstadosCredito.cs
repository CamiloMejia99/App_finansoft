using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    [Table("dbo.EstadosCredito")]
    public class EstadosCredito
    {
        [Key]
        public int id { get; set; }

        public string nombre { get; set; }

        public string descripcion { get; set; }
    }
}
