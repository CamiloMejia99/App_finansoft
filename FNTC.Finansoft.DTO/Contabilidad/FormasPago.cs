using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Contabilidad
{
    [Table("acc.FormasPago")]
    public class FormasPago
    {
        [Key]
        public int ID { get; set; }
        public string Nombre { get; set; }

        public bool Activo { get; set; }

        public string CodigoCuenta { get; set; }

        public bool Caja { get; set; }

        [Required]
        public int Tipo { get; set; }

        public bool TarjetaDebito { get; set; }
        public bool TarjetaCredito { get; set; }

        public bool CuentaxCobrar { get; set; }
        public bool CuentaXPagar { get; set; }

        public bool AnticipoRecibido { get; set; }
        public bool AnticipoEntregado { get; set; }



        public bool AplicaParaReciboCaja_Ingresos { get; set; }
        public bool AplicaPara_ComprobanteEgreso_Pagos { get; set; }
    }
}
