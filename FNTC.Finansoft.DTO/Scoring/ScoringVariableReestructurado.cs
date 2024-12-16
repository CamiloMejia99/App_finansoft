using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableReestructurados")]
    public class ScoringVariableReestructurado

    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Estado Reestructurado")]
        public string EstadoReestructurado { get; set; }

        [Display(Name = "Descripcion de Reestructurados")]
        public string DescripcionReestructurado { get; set; }

        [Required]
        [Display(Name = "Puntaje Reestructurado")]
        public int PuntajeReestructurado { get; set; }
    }
}
