using System;

namespace FNTC.Finansoft.Accounting.DTO.Nomina
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("nom.DescuentosNominaTipoDeCampos")]
    public class TipoDeCampo
    {
        [Key]
        public int ID { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Nombre del Campo")]
        public string NOMBRECAMPO { get; set; }

        [StringLength(255)]
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Descripción")]
        public string DESCRIPCION { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Fecha de Creación")]
        public DateTime FECHACREACION { get; set; }


    }
}
