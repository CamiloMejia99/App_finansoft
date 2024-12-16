using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.OperativaDeCaja
{
    [Table("dbo.configCajero")]
    public class configCajero
    {
        [ForeignKey("Caja")]
        [Required(ErrorMessage = "Seleccione Opcion")]
        public string Codigo_caja { get; set; }

        [Required(ErrorMessage = "Seleccione Opcion")]
        [Key, ForeignKey("Terceros")]
        public string Nit_cajero { get; set; }

        [ForeignKey("TiposComprobantes")]
        [Required(ErrorMessage = "Seleccione Opcion")]
        public string Compr_ingreso { get; set; }

        [ForeignKey("TiposComprobantes1")]
        [Required(ErrorMessage = "Seleccione Opcion")]
        public string Compr_egreso { get; set; }

        [ForeignKey("PlanCuentas")]
        [Required(ErrorMessage = "Seleccione Opcion")]
        public string Contr_banco { get; set; }

        [ForeignKey("PlanCuentas1")]
        [Required(ErrorMessage = "Seleccione Opcion")]
        public string Contr_otro { get; set; }

        [ForeignKey("PlanCuentas2")]
        [Required(ErrorMessage = "Seleccione Opcion")]
        public string Cta_efectivo { get; set; }

        [ForeignKey("PlanCuentas3")]
        [Required(ErrorMessage = "Seleccione Opcion")]
        public string Cta_cheque { get; set; }

        [ForeignKey("CentrosCostos")]
        [Required(ErrorMessage = "Seleccione Opcion")]
        public string centrocosto { get; set; }

        [ForeignKey("CentrosCostos1")]
        [Required(ErrorMessage = "Seleccione Opcion")]
        public string CentroCostoCaja { get; set; }

        [ForeignKey("TiposComprobantes2")]
        [Required(ErrorMessage = "Seleccione Opcion")]
        public string Tipocomprobante_caja { get; set; }

        public virtual Tercero Terceros { get; set; }
        public virtual Caja Caja { get; set; }
        public virtual CuentaMayor PlanCuentas { get; set; }
        public virtual CuentaMayor PlanCuentas1 { get; set; }
        public virtual CuentaMayor PlanCuentas2 { get; set; }
        public virtual CuentaMayor PlanCuentas3 { get; set; }
        public virtual TipoComprobante TiposComprobantes { get; set; }
        public virtual TipoComprobante TiposComprobantes1 { get; set; }
        public virtual CentroCosto CentrosCostos { get; set; }
        public virtual CentroCosto CentrosCostos1 { get; set; }
        public virtual TipoComprobante TiposComprobantes2 { get; set; }
        public virtual ICollection<CuadreCajaPorCajero> CuadreCajaPorCajero { get; set; }

    }
}