using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("Fcred.DocumentosPorAsociados")]

    public class FCDocumentosAsociados
    {
        [Key]
        public int idDoc { get; set; }
        public int idAsociado { get; set; }

        [ForeignKey("Documentos")]
        public int idDocumento { get; set; }

        public int idSolicitud { get; set; }
        public string NombreDocumento { get; set; }
        public string Extencion { get; set; }
        public string direccionDocumento { get; set; }
        public string documentoVerificado { get; set; }
        public string comentarios { get; set; }
        public string Estado { get; set; }
        public string MotivosEliminacion { get; set; }

        public virtual FCDocumentos Documentos { get; set; }
    }
}
