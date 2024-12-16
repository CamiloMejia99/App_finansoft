using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.ActivosFijos
{
    public class GruposActivosFijos
    {
        public int id { get; set; }
        [Display(Name = "Codigo")]
        [Required]
        public int codigo { get; set; }
        [Display(Name = "Nombre")]
        [Required]
        public string nombre { get; set; }

    }
}
