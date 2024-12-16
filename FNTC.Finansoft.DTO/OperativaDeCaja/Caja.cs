using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.OperativaDeCaja
{
    [Table("dbo.Caja")]
    public class Caja
    {
        [Key]
        [Required(ErrorMessage = "Ingrese código de caja"), MaxLength(10)]
        public string Codigo_caja { get; set; }
        [Required(ErrorMessage = "Ingrese un Nombre para Caja"), MaxLength(100)]
        public string Nombre_caja { get; set; }
        [Required(ErrorMessage = "Campo Requerido Para Facturar")]
        public int Consecutivo_ini { get; set; }
        [Required(ErrorMessage = "Campo Requerido Para Facturar")]
        public int consecutivo_fin { get; set; }
        [Required(ErrorMessage = "Campo Requerido Para Facturar")]
        public double TopeMaximo_caja { get; set; }
        [Required(ErrorMessage = "Campo Requerido Para Facturar")]
        public Nullable<int> consecutivo_actual { get; set; }
        [Required(ErrorMessage = "Campo Requerido Para Facturar")]
        public Nullable<int> Serie { get; set; }
        [Required(ErrorMessage = "Campo Requerido Para Facturar")]
        [ForeignKey("agencias")]
        public Nullable<int> agencia { get; set; }
        [Required(ErrorMessage = "Campo Requerido Para Facturar")]

        public string cta_abastecimiento { get; set; }

        [ForeignKey("cta_abastecimiento")]
        public virtual CuentaMayor PlanCuentas { get; set; }
        public virtual agencias agencias { get; set; }
        public virtual ICollection<configCajero> configCajero { get; set; }

        public virtual ICollection<CuadreCajaPorCajero> CuadreCajaPorCajero { get; set; }
        public virtual ICollection<RegistroAbastecimiento> RegistroAbastecimientos { get; set; }
        public virtual ICollection<FactOpcaja> FactOpcaja { get; set; }
    }
}