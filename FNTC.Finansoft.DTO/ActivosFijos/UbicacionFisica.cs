using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.ActivosFijos
{
    public class UbicacionFisica
    {
        public int id { get; set; }
        [Required]
        public int codigo { get; set; }
        [Required]
        public string nombre { get; set; }
    }
}
