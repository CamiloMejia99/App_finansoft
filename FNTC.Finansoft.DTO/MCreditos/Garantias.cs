using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class Garantias
    {
        [Key]
        public int Garantias_Id { get; set; }

        [Display(Name = "Activo")]
        public bool Garantias_Activo { get; set; }

        [Display(Name = "Codigo")]
        [Required]
        public string Garantias_Codigo { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        public string Garantias_Descripcion { get; set; }

        [Display(Name = "Tipo")]
        public bool Garantias_Tipo { get; set; }

        [Display(Name = "Clase Garantias")]
        public int Clase_Garantias_Id { get; set; }
        public virtual Clase_Garantias Clase_Garantias { get; set; }

        [Display(Name = "Codeudor")]
        public bool Garantias_Codeudor { get; set; }

        [Display(Name = "Garantias Hipotecarias")]
        public bool Garantias_Hipotecarias { get; set; }

        [Display(Name = "Porcentaje de Credito Pagado")]
        public long Garantias_Porcentaje_Credito_Pagado { get; set; }

        [Display(Name = "Créditos")]
        public virtual ICollection<BCreditos> creditos { get; set; }
    }
}
