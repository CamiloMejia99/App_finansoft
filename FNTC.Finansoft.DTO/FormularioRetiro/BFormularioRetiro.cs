using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.FormularioRetiro
{
    public class BFormularioRetiro
    {
        public int id { get; set; }

        [Required]
        [Display(Name = "Fecha de Solicitud")]
        public string fecha_solicitud { get; set; }

        [Display(Name = "Por mi situación económica necesito disponer de mis aportes")]
        public bool motivoRetiro1 { get; set; }

        [Display(Name = "Por retiro de la entidad donde laboro")]
        public bool motivoRetiro2 { get; set; }

        [Display(Name = "Otra entidad me ofrece compra de mi cartera")]
        public bool motivoRetiro3 { get; set; }

        [Display(Name = "Tengo dificultades para seguir aportando")]
        public bool motivoRetiro4 { get; set; }

        [Display(Name = "Otro")]
        public bool motivoRetiro5 { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required]
        [Display(Name = "Cedula")]
        public string nit { get; set; }

        [Display(Name = "Telefono")]
        public string telefono { get; set; }

        [Display(Name = "Celular")]
        public string celular { get; set; }

        [Required]
        [Display(Name = "Ciudad")]
        public string ciudad { get; set; }

        [Required]
        [Display(Name = "Correo Electronico")]
        public string correo { get; set; }
    }
}
