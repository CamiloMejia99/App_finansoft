using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{

    public class ViewModelScoringModeloRiesgoRiesgos

    {


        [Display(Name = "Riesgo de Impago")]
        public bool RiesgoImpago { get; set; }

        [Display(Name = "Riesgo de Credito Individual")]
        public bool RiesgoCreditoIndividual { get; set; }

        [Display(Name = "Riesgo De Cartera")]
        public bool RiesgoDeCartera { get; set; }

        [Display(Name = "Riesgo De Calificacion")]
        public bool RiesgoDeCalificacion { get; set; }


    }
}
