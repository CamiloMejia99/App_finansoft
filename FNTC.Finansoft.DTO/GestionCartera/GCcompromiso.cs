using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.GestionCartera
{
    [Table("Gcar.CompromisoCartera")]
    public class GCcompromiso
    {
        [Key]
        [Required]
        public int Id{ get; set; }
        [Required]
        public int IdGestion { get; set; }
        [Required]
        [Display(Name = "Comentario compromiso")]
        [StringLength(300)]
        public string ObservacionCompromiso { get; set; }
        [Required]
        [Display(Name = "Fecha Compromiso")]
        [DataType(DataType.DateTime)]
        public DateTime FechaCompromiso { get; set; }
        [Required]
        [Display(Name = "Valor Compromiso")]
        public string ValorCompromiso { get; set; }
        [Display(Name = "Tipo Compromiso")]
        [StringLength(20)]
        public string TipoCompromiso { get; set; }
        public Nullable<bool> ValidacionCompromiso { get; set; }
        [ForeignKey(" IdGestion")] 
        public GCgestion GCgestion { get; set; }

    }
}
