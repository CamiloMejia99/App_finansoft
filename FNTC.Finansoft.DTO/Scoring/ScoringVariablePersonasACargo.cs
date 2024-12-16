using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariablePersonasACargoes")]
    public class ScoringVariablePersonasACargo

    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Nombre Personas a Cargo")]
        public string NombrePersonasACargo { get; set; }

        [Display(Name = "Descripcion Personas a Cargo")]
        public string DescripcionPersonasACargo { get; set; }

        [Required]
        [Display(Name = "Puntaje Personas a Cargo")]
        public int PuntajePersonasACargo { get; set; }
    }
}
