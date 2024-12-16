using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class Cuentas
    {
        public int id { get; set; }

        [Required]
        [ForeignKey("PlanCuentas")] //
        [Display(Name = "Codigo Cuenta")]
        public string Cuenta_Cod { get; set; }
        public virtual CuentaMayor PlanCuentas { get; set; }

        [Required]
        [Display(Name = "Descripcion Cuenta")]
        public string Cuenta_Descripcion { get; set; }

        [Required]
        [Display(Name = "Funcion")]
        public string Funcion { get; set; }

        [Display(Name = "Funcion")]
        public string NombreFuncion { get; set; }

        [Required]
        [ForeignKey("TiposComprobantes")]
        [Display(Name = "Tipo Comprobante")]
        public string TipoComprobante { get; set; }

        public virtual TipoComprobante TiposComprobantes { get; set; }

        [Required]
        [ForeignKey("Destinos")]
        [Display(Name = "Destino")]
        public int Destino_Id { get; set; }

        public virtual Destinos Destinos { get; set; }
    }
}
