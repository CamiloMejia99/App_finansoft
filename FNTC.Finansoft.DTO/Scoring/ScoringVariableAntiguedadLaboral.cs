using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableAntiguedadLaborales")]
    public class ScoringVariableAntiguedadLaboral

    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Desde Antiguedad Laboral")]
        public string DesdeAntiguedad { get; set; }

        [Required]
        [Display(Name = "Hasta Antiguedad Laboral")]
        public string HastaAntiguedad { get; set; }

        [Display(Name = "Descripcion Antiguedad Laboral")]
        public string DescripcionAntiguedad { get; set; }

        [Required]
        [Display(Name = "Puntaje Antiguedad Laboral")]
        public int PuntajeAntiguedadL { get; set; }
    }
}
