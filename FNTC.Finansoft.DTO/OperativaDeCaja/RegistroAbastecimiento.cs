using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.OperativaDeCaja
{
    [Table("dbo.RegistroAbastecimientos")]
    public class RegistroAbastecimiento
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        [ForeignKey("Caja")]
        public string cod_caja { get; set; }
        [ForeignKey("PlanCuentas")]
        public string cta_abastecimiento { get; set; }
        public decimal abastecimiento { get; set; }
        [ForeignKey("agencias")]
        public int agencia { get; set; }

        public virtual CuentaMayor PlanCuentas { get; set; }
        public virtual agencias agencias { get; set; }
        public virtual Caja Caja { get; set; }

    }
}