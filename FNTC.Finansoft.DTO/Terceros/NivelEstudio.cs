using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Terceros
{
    [Table("ter.nivelestudio")]
    public class NivelEstudio
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Nivel Estudio")]
        [StringLength(25)]
        public string Nestudio { get; set; }
        [Display(Name = "detalle")]
        [StringLength(50)]
        public string Detalle { get; set; }



    }
}
