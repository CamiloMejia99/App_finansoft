using FNTC.Finansoft.Accounting.DTO.MCreditos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableGarantias")]
    public class ScoringVariableGarantia

    {
        [Required]
        public int id { get; set; }

        [ForeignKey("Garantias")]
        [Required]
        public int Garantias_Id { get; set; }

        [Display(Name = "Descripcion de Garantia")]
        public string DescripcionGarantia { get; set; }

        [Required]
        [Display(Name = "Puntaje Garantias")]
        public int PuntajeGarantia { get; set; }

        public virtual Garantias Garantias { get; set; }
    }
}
