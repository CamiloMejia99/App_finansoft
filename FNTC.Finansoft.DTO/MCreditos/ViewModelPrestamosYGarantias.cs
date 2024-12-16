using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class ViewModelPrestamosYGarantias
    {
        public int id { get; set; }

        [Required]
        public int garantia_id { get; set; }

        public int mivalor { get; set; }

        public int codeudor_nit { get; set; }

        public string nombre_codeudor { get; set; }

        [Required]
        public int valor_credito { get; set; }

        [Display(Name = "Pagaré")]
        [Remote("ValidacionPagare", "Prestamos", ErrorMessage = "El Pagare Ya Existe")]
        public string Pagare { get; set; }
        [Required]

        [Display(Name = "Capital")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public long Capital { get; set; }
        [Required]

        [Display(Name = "Intéres (%)")]

        public decimal Interes { get; set; }
        [Required]

        [Display(Name = "Plazo")]
        public decimal Plazo { get; set; }
        [Required]

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]

        [Display(Name = "Fecha del Prestamo")]
        [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$", ErrorMessage = "La Fecha No Tiene el Formato Correcto")]

        public string Fecha_Prestamo { get; set; }
        [Required]

        [Display(Name = "Periodo")]

        public int Tipo_Periodo_Id { get; set; }
        public virtual Tipo_Periodo Tipo_Periodo { get; set; }

        [Display(Name = "Fecha Primer Pago")]
        [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])[\/\-](0?[1-9]|1[012])[\/\-]\d{4}$", ErrorMessage = "La Fecha No Tiene el Formato Correcto")]
        public String Fecha_Primer_Pago { get; set; }
        [Required]

        [Display(Name = "Forma de Pago")]

        public int Forma_Pago_Id { get; set; }
        public virtual Forma_Pago Forma_Pago { get; set; }

        [Display(Name = "Subdestino")]
        [Required]
        public int Subdestino_Id { get; set; }
        public virtual SubDestinos SubDestinos { get; set; }

        [Display(Name = "NIT")]
        [Required]
        public string NIT { get; set; }
        //public virtual Terceros Terceros { get; set; }

        [Display(Name = "NOMBRE")]
        [Required]
        public string NOMBRE { get; set; }

        [Display(Name = "SALARIO")]
        [Required]
        public string SALARIO { get; set; }

        [Required]
        public int Destino_Id { get; set; }

        public int myselect { get; set; }
        public decimal ValorPeriodo { get; set; }
        public decimal ValorSeguro { get; set; }
        public decimal ValorSeguroPorcentaje { get; set; }
        public int ValDiasInt { get; set; }
        public int difdias { get; set; }
        public string fechadesembolso { get; set; }

        public decimal costoAdicionalEnEltiempo { get; set; }
        public decimal costoAdicionalAnticipado { get; set; }
        public decimal costoAdicionalPrimeraCuota { get; set; }
        public decimal costoAdicionalDividoEnElTiempo { get; set; }
        public decimal ValorPorcentajeCostoAnticipado { get; set; }
        public decimal ValorPorcentajeCostoEnCadaCuota { get; set; }

        [NotMapped]
        public string auxInteres { get; set; }

    }
}
