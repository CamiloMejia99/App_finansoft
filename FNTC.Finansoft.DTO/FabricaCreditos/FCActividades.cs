using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("Fcred.ActividadesAsociado")]
    public class FCActividades
    {
        [Key]
        [Display(Name = "Codigo")]
        public int idActividadAso { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string nombreActividad { get; set; }
        [Required]
        [Display(Name = "Descripcion")]
        public string descripcionActividad { get; set; }
        [Required]
        [Display(Name = "No. Referencias")]
        public int nReferencias { get; set; }
        [Required]
        [Display(Name = "No. Codeudores")]
        public int nCodeudores { get; set; }
    }
}
