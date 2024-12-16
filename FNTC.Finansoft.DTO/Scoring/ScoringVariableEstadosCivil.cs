using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableEstadosCiviles")]
    public class ScoringVariableEstadosCivil

    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Nombre de Estado Civil")]
        public string NombreEstadoCivil { get; set; }

        [Display(Name = "Descripcion de Estado Civil")]
        public string DescripcionEstadoCivil { get; set; }

        [Required]
        [Display(Name = "Puntaje Estado Civil")]
        public int PuntajeEstadoCivil { get; set; }
    }
}
