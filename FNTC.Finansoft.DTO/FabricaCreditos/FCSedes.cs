using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("Fcred.Sedes")]
    public class FCSedes
    {
        [Key]
        public int idSede { get; set; }

        [ForeignKey("agencias")]
        [Display(Name = "Nombre Sede")]
        public int idAgencias { get; set; }

        [Required]
        [Display(Name = "Dirección")]
        public string direccion { get; set; }

        [Required]
        [Display(Name = "Telefono")]
        public string telefono { get; set; }


        public virtual agencias agencias { get; set; }
    }
}
