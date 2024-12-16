using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableIngresosTotales")]
    public class ScoringVariableIngresosTotal

    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Ingresos totales desde")]
        public string IngresoTotalDesde { get; set; }
        [Required]
        [Display(Name = "Ingresos totales hasta")]
        public string IngresoTotalHasta { get; set; }

        [Display(Name = "Descripcion del ingreso")]
        public string DescripcionIngresos { get; set; }

        [Required]
        [Display(Name = "Puntaje Ingresos totales")]
        public int PuntajeIngresoTotal { get; set; }
    }
}
