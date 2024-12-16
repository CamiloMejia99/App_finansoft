namespace FNTC.Finansoft.Accounting.DTO.Nomina
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("nom.DescuentosNominaDescuentosnominas")]
    public class DescuentoNomina
    {

        [Key]
        public int ID { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Cédula")]
        public string CEDULA { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Nombre")]
        public string NOMBRE { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Nº de cuenta")]
        public string CUENTA { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Concepto")]
        public string CONCEPTO { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Codigo Concepto")]
        public string CODCONCEPTO { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Valor ")]
        public string VALOR { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Total ")]
        public string TOTAL { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Empresa")]
        public string EMPRESA { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Periodo ")]
        public string PERIODO { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Fecha ")]
        public string FECHA { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Total Discriminación Nomina ")]
        public string TOTDISNOM { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Total Discriminación Nomina Individual ")]
        public string TOTDISNOMIND { get; set; }


    }
}
