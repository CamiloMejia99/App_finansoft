using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.Accounting
{
    public class FormasDePagoDTO
    {
        [Key]
        public int ID { get; set; }

        [DisplayName("Nombre")]
        [Required]
        public string Nombre { get; set; }

        public bool Activo { get; set; }

        [Required]
        [DisplayName("Cuenta")]
        public string CodigoCuenta { get; set; }

        [Required]
        public string Tipo { get; set; }

        public Dictionary<string, int> Tipos { get; set; }

        //public bool Caja { get; set; }

        //[DisplayName("Tarjeta débito")]
        //public bool TarjetaDebito { get; set; }

        //[DisplayName("Tarjeta de crédito")]
        //public bool TarjetaCredito { get; set; }

        //[DisplayName("Cuenta x Cobrar")]
        //public bool CuentaxCobrar { get; set; }

        //[DisplayName("Cuenta x Pagar")]
        //public bool CuentaXPagar { get; set; }

        //[DisplayName("Anticipo recibido")]
        //public bool AnticipoRecibido { get; set; }

        //[DisplayName("Anticipo entregado")]
        //public bool AnticipoEntregado { get; set; }

        [DisplayName("Recibo caja/ingresos")]
        public bool AplicaParaReciboCaja_Ingresos { get; set; }

        [DisplayName("Comprobante egreso/pagos")]
        public bool AplicaPara_ComprobanteEgreso_Pagos { get; set; }



        //esto debe venir de Params
        public FormasDePagoDTO()
        {
            Tipos = new Dictionary<string, int>();
            Tipos.Add("Caja", 1);
            Tipos.Add("Cuenta x Cobrar", 2);
            Tipos.Add("Cuenta x Pagar", 3);
            Tipos.Add("Tarjeta Debito", 4);
            Tipos.Add("Tarjeta Crédito", 5);
            Tipos.Add("Anticipo Recibido", 6);
            Tipos.Add("Antipo Entregado", 7);

        }
    }
}


