using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableOcupaciones")]
    public class ScoringVariableOcupacion

    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Nombre de Ocupacion")]
        public string NombreOcupacion { get; set; }

        [Display(Name = "Descripcion de Ocupacion")]
        public string DescripcionOcupacion { get; set; }

        [Required]
        [Display(Name = "Puntaje Ocupacion")]
        public int PuntajeOcupacion { get; set; }
    }
}
