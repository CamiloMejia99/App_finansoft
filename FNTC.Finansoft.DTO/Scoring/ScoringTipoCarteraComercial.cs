using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringTipoCarteraComerciales")]
    public class ScoringTipoCarteraComercial

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
        [Display(Name = "Porcentaje de Provision")]
        public double PorcentajeProvision { get; set; }

    }
}
