using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Terceros
{
    [Table("ter.InfoTerceroFinanciera")]
    public class InfoTerceroFinanciera
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [ForeignKey("Tercero")]
        [Required]
        [Display(Name = "NIT Tercero")]
        [StringLength(20)]
        public string NitTercero { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:N3}")]
        [Display(Name = "Ingresos Mensuales")]
        public decimal IngresosMensuales { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:N3}")]
        [Display(Name = "Gastos Mensuales")]
        public decimal GastosMensuales { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:N3}")]
        [Display(Name = "Pasivos Totales")]
        public decimal PasivosTotales { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:N3}")]
        [Display(Name = "Activos Totales")]
        public decimal ActivosTotales { get; set; }


        public virtual Tercero Tercero { get; set; }
    }
}

