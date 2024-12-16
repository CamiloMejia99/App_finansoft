using System;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.DTO.Ahorros
{
    public class DTOFichasAhorros
    {
        public int id { get; set; }
        public string tipoFicha { get; set; }
        public Nullable<int> idConfiguracion { get; set; }
        public string numeroCuenta { get; set; }
        [Required]
        [Display(Name = "Identificacion:")]
        public string idPersona { get; set; }
        [Required]
        [Display(Name = "Forma de Pago:")]
        public string tipoPago { get; set; }
        [Required]
        [Display(Name = "Porcentaje:")]
        public string porcentaje { get; set; }
        [Required]
        [Display(Name = "Valor:")]
        public string valor { get; set; }
        [Required]
        [Display(Name = "Valor de la cuota:")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Solo números")]
        public string valorCuota { get; set; }
        public string totalAhorros { get; set; }
        public Nullable<int> tipoCalculoCuota { get; set; }
        [Required]
        public string CDAT { get; set; }
        [Required]
        [Display(Name = "Beneficiario 1:")]
        public string idBeneficiario1 { get; set; }
        [Required]
        [Display(Name = "Beneficiario 2:")]
        public string idBeneficiario2 { get; set; }
        [Required]
        [Display(Name = "Forma devolucion:")]
        public string tipoDevolucion { get; set; }
        [Required]
        [Display(Name = "Valor Titulo:")]
        public string valorTitulo { get; set; }
        [Required]
        [Display(Name = "Plazo:")]
        public string plazo { get; set; }
        [Required]
        [Display(Name = "Titulo Pignorado:")]
        public Nullable<bool> tituloPignorado { get; set; }
        [Required]
        [Display(Name = "Capitaliza Intereses:")]
        public Nullable<bool> capitalizaInteres { get; set; }
        [Required]
        [Display(Name = "Linea de deposito:")]
        public Nullable<int> idLineaDeposito { get; set; }
        [Required]
        [Display(Name = "Contractual:")]
        public string contractual { get; set; }
        [Required]
        [Display(Name = "Activo:")]
        public Nullable<bool> activo { get; set; }
        public Nullable<System.DateTime> fechaApertura { get; set; }

        #region Añadidos
        public string nit { get; set; }
        public string nombres { get; set; }
        public string empresa { get; set; }
        public string dependencia { get; set; }
        #endregion

        #region 13032020 ViewModel
        [RegularExpression(@"^(\d{1}\.)?(\d+\.?)+(,\d{2})?$", ErrorMessage = "Solo números")]
        [Display(Name = "Tasa de Interés:")]
        public decimal Tasa_interes { get; set; }
        [RegularExpression(@"^\d+$", ErrorMessage = "Solo números")]
        [Display(Name = "Plazo en Meses:")]
        public int? Plazo_meses { get; set; }
        [Display(Name = "Fecha de Vencimiento:")]
        public DateTime? Fecha_Vencimiento { get; set; }
        #endregion

    }
}
