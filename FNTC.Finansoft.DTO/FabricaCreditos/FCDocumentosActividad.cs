using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("Fcred.DocumentosPorActividad")]
    public class FCDocumentosActividad
    {
        [Key]
        [Required]
        public int id { get; set; }
        [Required]
        public int idActividad { get; set; }
        [Required]
        public int idDocumento { get; set; }
    }
}