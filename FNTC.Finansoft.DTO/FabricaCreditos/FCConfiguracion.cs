using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("Fcred.Configuracion")]
    public class FCConfiguracion
    {
        [Key]
        [Display(Name = "Codigo")]
        public int idConfiguracion { get; set; }
        [Required]
        [Display(Name = "Nombre Configuracion")]
        public string nomConfig { get; set; }
        [Required]
        [Display(Name = "Tiempo Minimo (En Horas)")]
        public int tiempoRespuestaSolMin { get; set; }
        [Required]
        [Display(Name = "Tiempo Maximo (En Horas)")]
        public int tiempoRespuestaSolMax { get; set; }
        [Required]
        [Display(Name = "Tiempo Maximo (En Dias)")]
        public int tiempoMaxOtorgarCredito { get; set; }
        [Required]
        [Display(Name = "Edad Minima")]
        public int edadMinCredito { get; set; }
        [Required]
        [Display(Name = "Edad Maxima")]
        public int edadMaxCredito { get; set; }
        [Required]
        [Display(Name = "Actualmente Activa")]
        public string activa { get; set; }
    }
}
