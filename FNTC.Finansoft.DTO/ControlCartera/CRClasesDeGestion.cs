using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.ControlCartera
{
    [Table("cart.ClasesDeGestion")]
    public class CRClasesDeGestion
    {
        [Key]
        public int idClase { get; set; }
        [Required]
        [Display(Name = "Clases De Gestion")]
        public string ClasesDeGestion { get; set; }
        [Required]
        [Display(Name = "Estado")]
        public string Estado { get; set; }
    }
}





