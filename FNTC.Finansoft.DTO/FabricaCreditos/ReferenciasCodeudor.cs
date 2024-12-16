using System;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    public class ReferenciasCodeudor
    {
        [Required]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Ingresa un valor valido")]
        [Display(Name = "Identificacion")]
        public string nit { get; set; }
        [Required]
        [RegularExpression("([a-zA-Z ]+)", ErrorMessage = "Ingresa un valor valido")]
        [Display(Name = "Primer Nombre")]
        public string nombre1 { get; set; }
        [Required]
        [RegularExpression("([a-zA-Z ]+)", ErrorMessage = "Ingresa un valor valido")]
        [Display(Name = "Segundo Nombre")]
        public string nombre2 { get; set; }
        [Required]
        [RegularExpression("([a-zA-Z ]+)", ErrorMessage = "Ingresa un valor valido")]
        [Display(Name = "Primer Apellido")]
        public string apellido1 { get; set; }
        [Required]
        [RegularExpression("([a-zA-Z ]+)", ErrorMessage = "Ingresa un valor valido")]
        [Display(Name = "Segundo Apellido")]
        public string apellido2 { get; set; }
        [Required]
        [Display(Name = "Genero")]
        public string genero { get; set; }
        [Required]
        [Display(Name = "Fecha Nacimiento")]
        public Nullable<System.DateTime> fechaNacimiento { get; set; }
        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Ingresa un valor valido")]
        [Display(Name = "Telefono")]
        public string telefono { get; set; }
        [Required]
        [RegularExpression("([0-9]+)", ErrorMessage = "Ingresa un valor valido")]
        [Display(Name = "Telefono Movil")]
        public string telefonoMovil { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Correo Electronico")]
        public string correoElectronico { get; set; }
        public Nullable<bool> esAsociado { get; set; }
        public Nullable<bool> esReferencia { get; set; }
        public Nullable<bool> esEmpleado { get; set; }
        public Nullable<int> idActividad { get; set; }
        public string idPadre { get; set; }
        [Required]
        [Display(Name = "Direccion Residencia")]
        public string direccionResidencia { get; set; }
        public Nullable<bool> esCodeudor { get; set; }

    }
}
