namespace FNTC.Finansoft.Accounting.DTO.Nomina
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("nom.DescuentosNominaComparacionArchivos")]
    public class ComparacionArchivo
    {
        [Key]
        public int ID { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Empresa")]
        public string EMPRESA { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Periodo Deducción")]
        public string PERDEDUCC { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Nro de Periodos")]
        public int PERIODO { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Realizar Cambios")]
        public string CAMBIO { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Ordenar por")]
        public string ORDEND { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Plano a leer")]
        public string PLANO { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Ruta del Archivo")]
        public string RUTA { get; set; }


    }
}
