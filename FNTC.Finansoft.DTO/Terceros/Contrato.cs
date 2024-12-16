using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Terceros
{
    [Table("ter.contrato")]
    public class Contrato
    {

        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Tipo Contrato")]
        [StringLength(30)]
        public string TipoContrato { get; set; }
        [Display(Name = "detalle")]
        [StringLength(50)]
        public string Detalle { get; set; }
    }
}
