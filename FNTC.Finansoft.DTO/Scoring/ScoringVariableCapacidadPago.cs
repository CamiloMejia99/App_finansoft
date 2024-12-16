using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableCapacidadPagos")]
    public class ScoringVariableCapacidadPago

    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Nombre Capacidad de Pago")]
        public string NombreCapacidadPago { get; set; }

        [Display(Name = "Descripcion Capacidad de Pago")]
        public string DescripcionCapacidadPago { get; set; }

        [Required]
        [Display(Name = "Puntaje Capacidad de Pago")]
        public int PuntajeCapacidadPagos { get; set; }
    }
}
