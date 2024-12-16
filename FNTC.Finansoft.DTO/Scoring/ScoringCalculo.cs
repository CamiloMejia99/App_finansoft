using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringCalculos")]
    public class ScoringCalculo

    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Nombre de Formula")]
        public string NombreFormula { get; set; }

        [Required]
        [Display(Name = "Concepto")]
        public string Concepto { get; set; }

        [Required]
        [Display(Name = "Formula")]
        public string Formula { get; set; }

        [Required]
        [Display(Name = "Porcentaje")]
        public int Porcentaje { get; set; }

    }

}
