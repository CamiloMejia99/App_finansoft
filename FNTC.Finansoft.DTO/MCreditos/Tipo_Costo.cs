using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class Tipo_Costo
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Tipo_Costo_Id { get; set; }

        [Required]
        [Display(Name = "Tipo de Costo")]
        public string Tipo_Costo_Descripcion { get; set; }
    }
}
