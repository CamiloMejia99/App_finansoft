using FNTC.Finansoft.Accounting.DTO.MCreditos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableFormaPagos")]
    public class ScoringVariableFormaPago

    {
        [Required]
        public int id { get; set; }

        [ForeignKey("Forma_Pago")]
        [Required]
        public int Forma_Pago_Id { get; set; }

        [Display(Name = "Descripcion de Forma de Pago")]
        public string DescripcionFormaPago { get; set; }

        [Required]
        [Display(Name = "Puntaje Forma de pago")]
        public int PuntajeFormaPago { get; set; }

        public virtual Forma_Pago Forma_Pago { get; set; }
    }
}
