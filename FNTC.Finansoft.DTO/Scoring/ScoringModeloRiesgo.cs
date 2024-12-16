using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringModeloRiesgos")]
    public class ScoringModeloRiesgo

    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Modelo de Riesgo")]
        public string ModeloRiesgo { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public bool Estado { get; set; }
    }
}
