using FNTC.Finansoft.Accounting.DTO.Terceros;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.OperativaDeCaja
{
    [Table("dbo.CuadreCajaPorCajero")]
    public class CuadreCajaPorCajero
    {
        public int id { get; set; }
        public string fecha { get; set; }
        public string codigo_caja { get; set; }
        [ForeignKey("Terceros")]
        public string nit_cajero { get; set; }
        public decimal retiros_efectivo { get; set; }
        public decimal retiros_cheque { get; set; }
        public decimal consignacion_efectivo { get; set; }
        public decimal consignacion_cheque { get; set; }
        public int cierre { get; set; }
        public DateTime horacierre { get; set; }
        public decimal tope { get; set; }

        public virtual configCajero configCajero { get; set; }
        public virtual Caja Caja { get; set; }
        public virtual Tercero Terceros { get; set; }
    }
}