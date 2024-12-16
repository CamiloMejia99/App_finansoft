using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("Fcred.Control")]
    public class ControlAccesoFC
    {
        [Key]
        public int IdControl { get; set; }
        public string Usuario { get; set; }
        public string Actividad { get; set; }
        public DateTime fecha { get; set; }
        public int IdAsociado { get; set; }
        public int IdSolicitud { get; set; }
    }
}



