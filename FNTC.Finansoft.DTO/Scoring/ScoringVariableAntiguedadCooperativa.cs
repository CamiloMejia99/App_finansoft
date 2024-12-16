using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableAntiguedadCooperativas")]
    public class ScoringVariableAntiguedadCooperativa

    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Desde Antiguedad Cooperativa")]
        public string DesdeAntiguedadCooperativa { get; set; }

        [Required]
        [Display(Name = "Hasta Antiguedad Cooperativa")]
        public string HastaAntiguedadCooperativa { get; set; }

        [Display(Name = "Descripcion Antiguedad Cooperativa")]
        public string DescripcionAntiguedadCooperativa { get; set; }

        [Required]
        [Display(Name = "Puntaje Antiguedad Cooperativa")]
        public int PuntajeAntiguedadC { get; set; }
    }
}
