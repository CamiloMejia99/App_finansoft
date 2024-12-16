namespace FNTC.Finansoft.Accounting.DTO.Nomina
{
    using FNTC.Finansoft.Accounting.DTO.Contabilidad;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("nom.DescuentosNominaSeleccionCuentas")]
    public class SeleccionCuenta
    {

        [Key]
        public int ID { get; set; }


        [StringLength(255)]
        [Required(ErrorMessage = "Requerido: Debe tener 9 digitos")]
        [Display(Name = "CODIGO CUENTA")]
        public string CODIGO { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "TIPO CUENTA")]
        public string TIPOCUENTA { get; set; }

        //ESTO ES LA LLEVE FORANEA
        [ForeignKey("CODIGO")]
        public CuentaAuxiliar Cuenta { get; set; }

    }
}
