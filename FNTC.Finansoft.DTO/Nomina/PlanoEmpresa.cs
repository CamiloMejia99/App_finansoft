namespace FNTC.Finansoft.Accounting.DTO.Nomina
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("nom.DescuentosNominaPlanoEmpresas")]

    public class PlanoEmpresa
    {
        [Key]
        public int id { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "CODIGO EMPRESA")]
        public string CODIGOEMP { get; set; }

        [ForeignKey("DescuentosNominaClaseDePlanos")]
        [Display(Name = "NOMBRE DEL PLANO")]
        [Required]
        public int NOMPLANO { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "NOMBRE EMPRESA")]
        public string NOMBREMP { get; set; }

        public virtual ClaseDePlano DescuentosNominaClaseDePlanos { get; set; }
    }
}
