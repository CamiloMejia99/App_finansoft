using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.ControlCartera
{
    [Table("cart.NotificacionesCartera")]
    public class CRNotificacionesCartera
    {
        [Key]
        public int idNotificacionesCR { get; set; }

        public string Pagare { get; set; }
        public int IdPrestamo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IdClase { get; set; }
        public int IdContacto { get; set; }
        public int IdRespuesta { get; set; }
        public string DetallesNotificacion { get; set; }
        public string EstadoCredito { get; set; }
        public int IdTipoDeConvenio { get; set; }
        public string Proceso { get; set; } 




    }
}




