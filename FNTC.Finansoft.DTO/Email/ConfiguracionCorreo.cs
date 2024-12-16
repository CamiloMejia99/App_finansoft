namespace FNTC.Finansoft.Accounting.DTO.Email
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    [Table("ConfiguracionCorreo")]
    public partial class ConfiguracionCorreo
    {
        [Key]
        [Column(Order = 0)]
        public int id_email { get; set; }

        [Display(Name = "Correo electrónico")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
             ErrorMessage = "Dirección de Correo electrónico incorrecta.")]
        [StringLength(100, ErrorMessage = "Longitud máxima 100")]
        public string email { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string password { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string smtp { get; set; }

        public int puerto { get; set; }
        [StringLength(10)]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string estado { get; set; }

    }

}