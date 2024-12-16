using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.ControlCartera
{
    [Table("cart.GestionContacto")]
    public class CRGestionContacto
    {
        [Key]
        public int idGestionContacto { get; set; }
        [Required]
        [Display(Name = "Contacto")]
        public string Contacto { get; set; }
        [Required]
        [Display(Name = "Estado")]
        public string Estado { get; set; }
    }
}
