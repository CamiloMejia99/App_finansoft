using FNTC.Finansoft.Accounting.DTO.Terceros;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.OperativaDeCaja
{
    [Table("dbo.factOpCajaDesembolsos")]
    public class factOpCajaDesembolsos
    {

        public int id { get; set; }
        public DateTime fecha { get; set; }
        public string factura { get; set; }

        [ForeignKey("Terceros")]
        public string NIT { get; set; }
        public string codigoCaja { get; set; }
        public string pagare { get; set; }
        public string valorDesembolsado { get; set; }
        public string nitCajero { get; set; }

        [ForeignKey("NIT")]
        public virtual Tercero Terceros { get; set; }
    }
}