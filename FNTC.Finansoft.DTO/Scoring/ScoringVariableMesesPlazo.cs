using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableMesesPlazos")]
    public class ScoringVariableMesesPlazo

    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Desde meses de plazo")]
        public string MesesPlazo { get; set; }



        [Display(Name = "Descripcion de meses de plazo")]
        public string DescripcionMesesPlazo { get; set; }

        [Required]
        [Display(Name = "Puntaje Meses de Plazo")]
        public int PuntajeMesesPlazo { get; set; }
    }
}
