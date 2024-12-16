using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("Fcred.Tareas")]
    public class FCTareas
    {
        [Key]
        public int idTarea { get; set; }
        public int idSolicitud { get; set; }
        public string idOperarioAnalista { get; set; }
        public string idOperarioEntidad { get; set; }
        public string estadoAnalista { get; set; }
        public string estadoEnte { get; set; }
    }
}
