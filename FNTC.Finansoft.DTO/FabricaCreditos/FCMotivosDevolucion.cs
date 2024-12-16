using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("Fcred.MotivosDevolucion")]
    public class FCMotivosDevolucion
    {
        [Key]
        [Display(Name = "Codigo")]
        public int idMotivoDevolucion { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string nombreMotivo { get; set; }
        [Required]
        [Display(Name = "Descripcion")]
        public string descripcionMotivo { get; set; }
    }
}
