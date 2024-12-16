using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringTipoCarteraConsumos")]
    public class ScoringTipoCarteraConsumo

    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Categoria")]
        public string Categoria { get; set; }

        [Required]
        [Display(Name = "Numero de dias")]
        public int NroDias { get; set; }

        [Required]
        [Display(Name = "Porcentaje de provision")]
        public double PorcentajeProvision { get; set; }

    }
}
