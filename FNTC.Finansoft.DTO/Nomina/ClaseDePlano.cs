namespace FNTC.Finansoft.Accounting.DTO.Nomina
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("nom.DescuentosNominaClaseDePlanos")]
    public class ClaseDePlano
    {

        [Key]
        public int ID { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Nombre del plano")]
        public string NOMBRE { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Tipo de Plano")]
        public string TIPOPLANO { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Tipo de recepción")]
        public string TIPORECEPCION { get; set; }

        [StringLength(10)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Extensión del Archivo")]
        public string EXTENSION { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Delimitador")]
        public string DELIMITADOR { get; set; }

    }
}
