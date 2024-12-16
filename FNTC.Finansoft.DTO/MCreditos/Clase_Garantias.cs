using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class Clase_Garantias
    {
        [Key]
        [Display(Name = "Clase Garantia")]
        public int Clase_Garantias_Id { get; set; }

        [Display(Name = "Descripción de la Garantia")]
        [Required]
        public string Clase_Garantias_Descripcion { get; set; }

        public virtual ICollection<Garantias> Garantias { get; set; }
    }
}
