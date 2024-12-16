using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    public class ViewModelScoringCalculos

    {


        [Display(Name = "Nombre de Formula")]
        public string NombreFormula { get; set; }

        [Display(Name = "Concepto")]
        public string Concepto { get; set; }

        [Display(Name = "Formula")]
        public string Formula { get; set; }

        [Display(Name = "Porcentaje")]
        public int Porcentaje { get; set; }

        [Display(Name = "Nombre de Formula")]
        public string NombreFormula2 { get; set; }

        [Display(Name = "Concepto")]
        public string Concepto2 { get; set; }

        [Display(Name = "Formula")]
        public string Formula2 { get; set; }

        [Display(Name = "Porcentaje")]
        public int Porcentaje2 { get; set; }

    }
}
