using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableAgencias")]
    public class ScoringVariableAgencia

    {
        [Required]
        public int id { get; set; }

        [ForeignKey("agencias")]
        [Required]
        public int IdAgencia { get; set; }

        [Display(Name = "Descripcion Agencias")]
        public string DescripcionAgencia { get; set; }

        [Required]
        [Display(Name = "Puntaje Agencia")]
        public int PuntajeAgencia { get; set; }



        public virtual agencias agencias { get; set; }


    }
}
