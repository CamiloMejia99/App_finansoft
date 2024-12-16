using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableSexos")]
    public class ScoringVariableSexo
    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Nombre Sexo")]
        public string NombreSexo { get; set; }

        [Display(Name = "Descripcion Sexo")]
        public string DescripcionSexo { get; set; }

        [Required]
        [Display(Name = "Puntaje Sexo")]
        public int PuntajeSexo { get; set; }
    }
}
