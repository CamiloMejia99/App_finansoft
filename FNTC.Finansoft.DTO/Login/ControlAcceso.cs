using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.Login
{
    public class ControlAcceso
    {
        public int id { get; set; }
        [Required]
        [Display(Name = "USUARIO: ")]
        public string usuario { get; set; }
        [Required]
        [Display(Name = "CONTRASEÑA: ")]
        public string password { get; set; }
        [Required]
        [Display(Name = "NOMBRE: ")]
        public string nombre { get; set; }
    }
}
