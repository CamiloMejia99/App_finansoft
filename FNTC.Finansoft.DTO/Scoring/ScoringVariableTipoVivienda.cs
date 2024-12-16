using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableTipoViviendas")]
    public class ScoringVariableTipoVivienda

    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Nombre Tipo de Vivienda")]
        public string NombreTipoVivienda { get; set; }

        [Display(Name = "Descripcion Tipo de Vivienda")]
        public string DescripcionTipoVivienda { get; set; }

        [Required]
        [Display(Name = "Puntaje Tipo de Vivienda")]
        public int PuntajeTipoVivienda { get; set; }
    }
}
