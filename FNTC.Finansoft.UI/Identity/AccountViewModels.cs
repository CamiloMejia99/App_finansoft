using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace IdentitySample.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Código")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Recordarme")]
        public bool RememberBrowser { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginUserViewModel
    {
        [Required(ErrorMessage = "Nombre de Usuario Obligatorio.")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Contraseña Obligatoria.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        
        [DataType(DataType.DateTime)]
        [Display(Name = "Ultimo acceso")]
        public string LastActivityDate { get; set; }

        [Display(Name = "Recordarme")]
        public bool RememberMe { get; set; }
    }

    public class RegisterUserViewModel
    {
       // [Required]
        [Display(Name = "Nombres")]
        public string Nombres { get; set; }

       // [Required]
        [Display(Name = "Usuario")]
        //[System.Web.Mvc.Remote("VerificarUsuario", "Account", HttpMethod = "POST", AdditionalFields = "Password", ErrorMessage = "Nombre de usuario ya está en uso")]
        [Remote("ValidacionUser", "UsersAdmin", ErrorMessage = "El Usuario Ya Existe")]
        [RegularExpression(@"(\S)+", ErrorMessage = "No se admite espacios en blanco")]
        public string UserName { get; set; }

        [Display(Name = "Cargo")]
        public int Cargo_id { get; set; }

        [Display(Name = "Dependencia")]
        public int Dependencia_id { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        [RegularExpression(@"^[^@]+@[^@]+\.[a-zA-Z]{2,}$", ErrorMessage = "El registro no corresponde a un E-mail")]
        [Remote("ValidacionEmail", "UsersAdmin", ErrorMessage = "El Correo Ya Existe")]
        public string Email { get; set; }

        [Required]
        //[StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
        [RegularExpression(@"^(?=.{6,}$)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?\W).*$", ErrorMessage = "Mínimo 6 caracteres, una mayuscula y un caracter especial")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.{6,}$)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?\W).*$", ErrorMessage = "Mínimo 6 caracteres, una mayuscula y un caracter especial")]
        [Display(Name = "Confirmar")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }

        // Add the new address properties:
        [Display(Name = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Municipio")]
        public string City { get; set; }

        [Display(Name = "Departamento")]
        public string State { get; set; }

        // Use a sensible display name for views:
        [Display(Name = "Código Postal")]
        public string PostalCode { get; set; }

        [Display(Name = "Teléfono")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Municipio")]
        [Required(ErrorMessage = "El Municipio es obligatorio.")]
        public int Municipio_id { get; set; }

        [Display(Name = "Cambio Password")]
        public DateTime LastPasswordChangedDate { get; set; }

        public DateTime Fecha_Registro {get; set; }


        public string Cedula { get; set; }
        public string Code { get; set; }
        public DateTime FechaCreacion
        {
            set
            {
                FechaCreacion = DateTime.Now;
            }
        }
    }

    public class ResetPasswordViewModelJme
    {
        [Required]
        [EmailAddress]
        [RegularExpression(@"^[^@]+@[^@]+\.[a-zA-Z]{2,}$", ErrorMessage = "El registro no corresponde a un E-mail")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La {0} debe tener al menos {2} caracteres de longitud.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        [RegularExpression(@"^(?=.{6,}$)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?\W).*$", ErrorMessage = "Mínimo 6 caracteres, una mayuscula y un caracter especial")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        //[Compare("Password", ErrorMessage = "La contraseña y la confirmación no coinciden")]
        [RegularExpression(@"^(?=.{6,}$)(?=.*?[a-z])(?=.*?[A-Z])(?=.*?[0-9])(?=.*?\W).*$", ErrorMessage = "Mínimo 6 caracteres, una mayuscula y un caracter especial")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Required]
        //[Display(Name = "Usuario")]
        //public string UserName { get; set; }
    }
}