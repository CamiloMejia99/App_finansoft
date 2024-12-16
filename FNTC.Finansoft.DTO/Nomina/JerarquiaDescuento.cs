namespace FNTC.Finansoft.Accounting.DTO.Nomina
{
    using FNTC.Finansoft.Accounting.DTO.Contabilidad;

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("nom.DescuentosNominaJerarquiaDescuento")]
    public class JerarquiaDescuento
    {
        [Key]
        [StringLength(255)]
        [Required(ErrorMessage = "Requerido: Debe tener 9 digitos")]
        [Display(Name = "Codigo Cuenta")]
        public string CODIGO { get; set; }


        public short ORDEN { get; set; }
        [ForeignKey("CODIGO")]

        public CuentaAuxiliar Cuenta { get; set; }
    }
}
