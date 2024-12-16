using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableTipoContratos")]
    public class ScoringVariableTipoContrato

    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Nombre Tipo Contratos")]
        //[ForeignKey("Parameters4")]
        public String NombreTipoContrato { get; set; }

        [Display(Name = "Descripcion Tipo Contratos")]
        public string DescripcionTipoContrato { get; set; }

        [Required]
        [Display(Name = "Puntaje Tipo Contrato")]
        public int PuntajeTipoContrato { get; set; }

        //public virtual Parameter Parameters4 { get; set; }
    }
}
