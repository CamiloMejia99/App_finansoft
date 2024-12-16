using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("Fcred.ReferenciasPorSolicitud")]
    public class FCReferenciasSolicitud
    {
        [Key]
        public int idReferencia { get; set; }

        public int IDSOLICITUD { get; set; }

        public string ACTIVIDADECONONICA { get; set; }

        public string PARENTESCO { get; set; }

        public string VERIFICACION { get; set; }

        public string INFADICIONAL { get; set; }
    }
}