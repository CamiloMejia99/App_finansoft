using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class Lineas
    {
        [Key]
        [Display(Name = "Lineas")]
        public int Lineas_Id { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        public string Lineas_Descripcion { get; set; }

        [Display(Name = "Código")]
        [Required]
        public string Lineas_Codigo { get; set; }

        [Display(Name = "Activo")]

        public bool Lineas_Activo { get; set; }

        public virtual ICollection<BCreditos> creditos { get; set; }
        public virtual ICollection<Destinos> Destinos { get; set; }
    }
}
