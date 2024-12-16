using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableEstratos")]
    public class ScoringVariableEstrato

    {
        [Required]
        public int id { get; set; }

        //[ForeignKey("Parameters3")]
        [Required]
        public string Codigo { get; set; }

        [Display(Name = "Descripcion de Estrato")]
        public string DescripcionEstrato { get; set; }

        [Required]
        [Display(Name = "Puntaje Estrato")]
        public int PuntajeEstrato { get; set; }

        //  public virtual Parameter Parameters3 { get; set; }
    }
}
