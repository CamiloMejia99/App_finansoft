using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("Fcred.Documentos")]
    public class FCDocumentos
    {
        [Key]
        [Display(Name = "Codigo")]
        public int idDocumento { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string nombreDocumento { get; set; }
        [Required]
        [Display(Name = "Descripcion")]
        public string descripcionDocumento { get; set; }
        [Required]
        [Display(Name = "Dias Vigente")]
        public int? diasVigente { get; set; }
        [Required]
        [Display(Name = "¿El Documento Vence?")]
        public string Vence { get; set; }
    }
}
