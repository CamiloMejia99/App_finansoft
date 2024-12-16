using FNTC.Finansoft.DTO.Respuestas;
using System;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.DTO.Aportes
{
    public class DTOConfiguracionAportes
    {
        //respuesta
        public DTORespuesta Respuesta { get; set; }

        public int id { get; set; }

        [Required]
        [Display(Name = "Nombre(Abreviatura):")]
        public string Nombre { get; set; }
        [Required]
        [Display(Name = "Codigo Cuenta Contable:")]
        public Nullable<int> idCuenta { get; set; }
        [Required]
        [Display(Name = "Multiplicador:")]
        public Nullable<int> multiplicador { get; set; }
        [Required]
        [Display(Name = "Saldo Minimo:")]
        public string SaldoMinimo { get; set; }
        [Required]
        [Display(Name = "Cuota Calcula Sobre:")]
        public Nullable<int> idTipoCuotaCalculo { get; set; }
        [Required]
        [Display(Name = "Valor:")]
        public string valor { get; set; }
        [Display(Name = "Valor Cuota:")]
        public string valorCuota { get; set; }
        [Required]
        [Display(Name = "Porcentaje del valor:")]
        public string porcentaje { get; set; }
        [Required]
        [Display(Name = "Porcentaje de la cuota para aportes:")]
        public string porcentajeCuota { get; set; }
        public Nullable<bool> activo { get; set; }
        public Nullable<System.DateTime> fechaRegistro { get; set; }
        //añadidos        
        public string nombreCuenta { get; set; }
        public string nombreTipoCalculoCuota { get; set; }

        [Required]
        [Display(Name = "Rango desde:")]
        public long RangoDesde { get; set; }

        [Required]
        [Display(Name = "Rango hasta:")]
        public long RangoHasta { get; set; }

        public long ConsecutivoActual { get; set; }

    }
}
