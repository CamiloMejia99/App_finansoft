using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.SARLAFT
{
    [Table("spe.ContextoEmpresarial")]
    public class ContextoEmpresarial
    {
        [Key]
        public int id { get; set; }

        [Required]
        public string descripcion { get; set; }
        public string mision { get; set; }
        public string vision { get; set; }
        public string objetivo { get; set; }
        public string contextoExterno { get; set; }
        public string contextoInterno { get; set; }
    }
}
