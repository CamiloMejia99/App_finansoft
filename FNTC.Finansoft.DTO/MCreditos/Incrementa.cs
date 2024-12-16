using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class Incrementa
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Incrementa_Id { get; set; }

        [Required]
        [Display(Name = "Incremento")]
        public string Incrementa_Descripcion { get; set; }
    }
}
