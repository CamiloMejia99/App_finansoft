using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.ControlCartera
{
    [Table("cart.GestionRespuesta")]
    public class CRGestionRespuesta
    {
        [Key]
        public int idGestionRespuesta { get; set; }
        [Required]
        [Display(Name = "Respuesta")]
        public string Respuesta { get; set; }
        [Required]
        [Display(Name = "Estado")]
        public string Estado { get; set; }
    }
}
