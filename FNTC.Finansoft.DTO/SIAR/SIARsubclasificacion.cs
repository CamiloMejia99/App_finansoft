using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.SIAR
{
    [Table("spe.subclasificacion")]
    public class SIARsubclasificacion
    {
        [Key]
        public int id { get; set; }

        public string descripcion { get; set; }
    }
}
