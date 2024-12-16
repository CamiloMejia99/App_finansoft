using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.DeterioroCartera
{
    [Table("DCar.DeterioroPar")]
    public class DeterioroPar
    {

        public int Id { get; set; }
        [Required]
        [Display(Name = "Rango")]
        [StringLength(10)]
        public string Rango { get; set; }
        [Required]
        [Display(Name = "Desde")]
        [StringLength(10)]
        public string Desde { get; set; }
        [Required]
        [Display(Name = "Hasta")]
        [StringLength(10)]
        public string Hasta { get; set; }
        [Required]
        [Display(Name = "Tipo Provisión")]
        [StringLength(15)]
        public string TipoProvision { get; set; }
        [Required]
        [Display(Name = "% Provisión")]
        [StringLength(10)]
        public string PProvision { get; set; }


    }
}
