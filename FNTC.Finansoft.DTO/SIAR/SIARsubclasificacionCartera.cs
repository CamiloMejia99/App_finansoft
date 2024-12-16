using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.SIAR
{
    [Table("spe.SubclasificacionCartera")]
    public class SIARsubclasificacionCartera
    {
        public int id { get; set; }
        public string descripcion { get; set; }
    }
}
