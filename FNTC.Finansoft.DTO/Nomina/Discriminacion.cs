namespace FNTC.Finansoft.Accounting.DTO.Nomina
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("nom.DescuentosNominaTDiscriminados")]
    public class Discriminacion
    {
        [Key]
        public int ID { get; set; }


        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Empresa")]
        [ForeignKey("PlanoEmpresa")]
        public int EMPRESA { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Periodo Deducción")]
        public string PERDEDUCC { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Nro de Periodos")]
        public int PERIODO { get; set; }


        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Generar")]
        public string GENERAR { get; set; }
        [ForeignKey("ClaseDePlano")]
        public int PLANO { get; set; }
        public DateTime FECHACREACIONDIS { get; set; }


        public virtual PlanoEmpresa PlanoEmpresa { get; set; }
        public virtual ClaseDePlano ClaseDePlano { get; set; }

    }
}
