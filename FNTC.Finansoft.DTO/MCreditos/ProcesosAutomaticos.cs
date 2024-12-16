using System;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class ProcesosAutomaticos
    {
        public int id { get; set; }

        [Display(Name = "Causacion Prestamos")]
        [Required]
        public bool causacionPrestamos { get; set; }

        [Display(Name = "Causacion Ahorros")]
        [Required]
        public bool causacionAhorros { get; set; }

        [Display(Name = "Realizar Debito Automatico")]
        [Required]
        public bool realizarDebitoAutomatico { get; set; }

        [Display(Name = "Realizar Cierres Activos Fijos")]
        [Required]
        public bool realizarCierresActivosFijos { get; set; }

        [Display(Name = "Realizar Cierres Activos Diferidos")]
        [Required]
        public bool realizarCierresActivosDiferidos { get; set; }

        [Display(Name = "Hora del Proceso")]
        [Required]
        public TimeSpan horaProceso { get; set; }

        [Display(Name = "Realizar Copia de Seguridad Automaticamente")]
        [Required]
        public bool copiaSeguridadAutomatica { get; set; }
    }
}
