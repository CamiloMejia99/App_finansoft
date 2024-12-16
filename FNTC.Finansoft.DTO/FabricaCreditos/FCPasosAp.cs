using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("Fcred.PasosAp")]
    public class FCPasosAp

    {

        [Key]
        public int idAp { get; set; }
        public int idasociado { get; set; }
        public int idsolicitud { get; set; }
        public string EstadoOp { get; set; }
        public string ComentarioOp { get; set; }
        public string EstadoAnRC { get; set; }
        public string ComentarioAnRC { get; set; }
        public string EstadoAnDoc { get; set; }
        public string ComentarioAnDoc { get; set; }

    }
}
