namespace FNTC.Finansoft.Accounting.DTO.Nomina
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("nom.DescuentosNominaCorreccionNomina")]
    public class CorreccionNomina
    {


        [Key]
        public int ID { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Empresa")]
        public string EMPRESA { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Cédula")]
        public string CEDULA { get; set; }


        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Agencia")]
        public string AGENCIA { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Periodo ")]
        public string PERIODO { get; set; }



        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Que tipo de Concepto se desea Agregar ")]
        public string CONCEPTO { get; set; }

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
