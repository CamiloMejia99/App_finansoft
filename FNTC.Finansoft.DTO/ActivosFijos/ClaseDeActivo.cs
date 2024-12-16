using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.ActivosFijos
{
    public class ClaseDeActivo
    {
        public int id { get; set; }
        [Required]
        public int codigo { get; set; }
        [Required]
        public string nombre { get; set; }
    }
}
