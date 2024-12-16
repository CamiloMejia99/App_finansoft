using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableEdades")]
    public class ScoringVariableEdad

    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Edad desde")]
        public string EdadDesde { get; set; }

        [Required]
        [Display(Name = "Edad Hasta")]
        public string EdadHasta { get; set; }

        [Display(Name = "Descripcion edad")]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Puntaje Edad")]
        public int PuntajeEdad { get; set; }
    }
}
