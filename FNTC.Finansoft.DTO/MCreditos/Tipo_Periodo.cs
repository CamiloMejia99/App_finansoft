using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class Tipo_Periodo
    {
        [Key]
        public int Tipo_Periodo_Id { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        public string Tipo_Periodo_Descripcion { get; set; }

        [Display(Name = "Valor")]
        [Required]
        public decimal Tipo_Periodo_Valor { get; set; }

        public virtual ICollection<Prestamos> Prestamos { get; set; }
    }
}
