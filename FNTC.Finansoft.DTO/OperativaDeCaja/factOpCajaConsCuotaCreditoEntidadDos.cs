using FNTC.Finansoft.Accounting.DTO.TercerosOtrasEntidades;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.OperativaDeCaja
{
    [Table("dbo.factOpCajaConsCuotaCreditoEntidadDos")]
    public class factOpCajaConsCuotaCreditoEntidadDos
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public string factura { get; set; }

        [ForeignKey("tercerosEntidadDos")]
        public string cedula { get; set; }
        public string codigoCaja { get; set; }
        public string valorResibido { get; set; }
        public string valorConsignado { get; set; }
        public string nitCajero { get; set; }

        [ForeignKey("NIT")]
        public virtual tercerosEntidadDos tercerosEntidadDos { get; set; }
    }
}