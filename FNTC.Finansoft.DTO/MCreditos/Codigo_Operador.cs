using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class Codigo_Operador
    {
        [Key]
        public int Codigo_Operador_Id { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        public string Codigo_Operador_Descripcion { get; set; }

        //[Display(Name = "Creditos")]
        //public virtual ICollection<Creditos> creditos { get; set; }
    }
}
