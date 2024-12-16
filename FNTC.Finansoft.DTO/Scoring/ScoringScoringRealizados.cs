using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringScoringRealizados")]
    public class ScoringScoringRealizado

    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Cedula")]
        public int Cedula { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Puntaje Parcial Scoring")]
        public int Puntaje { get; set; }

        [Required]
        [Display(Name = "Puntaje DataCrédito")]
        public int PuntajeDatacredito { get; set; }

        [Required]
        [Display(Name = "Puntaje Total Scoring")]
        public int PuntajeTotal { get; set; }

        [Required]
        [Display(Name = "Capacidad de Pago")]
        public string CapacidadPago { get; set; }

        [Required]
        [Display(Name = "Porcentaje de Endeudamiento")]
        public string PorcentajeEndeudamiento { get; set; }

        [Required]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }
    }
}
