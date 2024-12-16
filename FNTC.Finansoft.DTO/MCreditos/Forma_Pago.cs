using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class Forma_Pago
    {
        [Key]
        public int Forma_Pago_Id { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        public string Forma_Pago_Descripcion { get; set; }

        [Display(Name = "Prestamo")]
        public virtual ICollection<Prestamos> Prestamos { get; set; }
    }
}
