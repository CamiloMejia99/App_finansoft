using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Terceros
{
    [Table("ter.estrato")]
    public class estrato
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Estrato")]
        [StringLength(20)]
        public string Estrato { get; set; }
        [Display(Name = "detalle")]
        [StringLength(50)]
        public string Detalle { get; set; }

    }
}
