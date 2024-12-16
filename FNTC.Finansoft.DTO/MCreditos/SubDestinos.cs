using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class SubDestinos
    {
        [Key]
        public int Subdestino_Id { get; set; }

        [Display(Name = "Código")]
        [Required]
        public string Subdestino_Codigo { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        public string Subdestino_Descripcion { get; set; }

        public int Destino_Id { get; set; }
        public virtual Destinos Destinos { get; set; }

        [Display(Name = "Activo")]
        public bool Subdestino_Activo { get; set; }
    }
}
