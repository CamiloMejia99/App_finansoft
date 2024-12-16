using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.DeterioroCartera
{
    [Table("DCar.CuentaDeterioroCartera")]
    public class CuentaDeterioroCartera
    {

        public int Id { get; set; }
        [ForeignKey("CuentaDeterioro")]
        [Display(Name = "Cuenta Deterioro")]
        public string IdPlanCuentaDeterioro { get; set; }
        [ForeignKey("CuentaGastosProvision")]
        [Display(Name = "Cuenta Gastos Provisión")]
        public string IdPlanCuentaGastosProvision { get; set; }
        [Display(Name = "Tipo Comprobante")]
        [ForeignKey("TipoComprobante")]
        public string TComprobante { get; set; }
        [Display(Name = "Observación")]
        [StringLength(150)]
        public string NombreSeleccion { get; set; }
        public virtual TipoComprobante TipoComprobante { get; set; }
        public virtual CuentaMayor CuentaDeterioro { get; set; }
        public virtual CuentaMayor CuentaGastosProvision { get; set; }
    }
}
