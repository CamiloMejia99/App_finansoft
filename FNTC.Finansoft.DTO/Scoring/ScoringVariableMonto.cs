using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableMontos")]
    public class ScoringVariableMonto

    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Desde Monto")]
        public string DesdeMonto { get; set; }

        [Required]
        [Display(Name = "Hasta Monto")]
        public string HastaMonto { get; set; }

        [Display(Name = "Descripcion de Monto")]
        public string DescripcionMonto { get; set; }

        [Required]
        [Display(Name = "Puntaje Monto")]
        public int PuntajeMonto { get; set; }
    }
}
