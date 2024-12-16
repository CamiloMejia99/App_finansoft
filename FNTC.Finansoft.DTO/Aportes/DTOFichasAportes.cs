using System;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.DTO.Aportes
{
    public class DTOFichasAportes
    {
        public int id { get; set; }
        public string numeroCuenta { get; set; }
        public Nullable<int> IdConfiguracion { get; set; }
        [Required]
        [Display(Name = "Identificacion:")]
        public string idPersona { get; set; }
        public string tipoPago { get; set; }
        [Required]
        [Display(Name = "Porcentaje:")]
        public string porcentaje { get; set; }
        [Required]
        [Display(Name = "Valor:")]
        public string valor { get; set; }
        [Required]
        [Display(Name = "Valor de la cuota:")]
        public string valorCuota { get; set; }
        public string totalAportes { get; set; }
        public Nullable<System.DateTime> FechaApertura { get; set; }
        [Required]
        [Display(Name = "Activa:")]
        public Nullable<bool> Activa { get; set; }
        public string asesor { get; set; }

    }
}
