using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableNivelesEducativos")]
    public class ScoringVariableNivelesEducativo

    {
        [Key]
        public int id { get; set; }


        [Required]
        [Display(Name = "Nombre de Nivel Educativo")]
        //[ForeignKey("Parameters2")]
        public string NombreNivelEducativo { get; set; }

        [Display(Name = "Descripcion de Nivel Educativo")]
        public string DescripcionNivelEducativo { get; set; }

        [Required]
        [Display(Name = "Puntaje Nivel Educativo")]
        public int PuntajeNivelEducativo { get; set; }

        //public virtual Parameter Parameters2 { get; set; }
    }
}
