using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Auditoria
{
    [Table("dbo.TipoEmpresa")]
    public class TipoEmpresa
    {
        [Key]
        public int id { get; set; }

        [StringLength(250)]
        public string tipo { get; set; }

    }
}
