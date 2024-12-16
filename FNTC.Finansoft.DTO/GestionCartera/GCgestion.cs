using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.GestionCartera
{
    [Table("Gcar.GCgestion")]
    public class GCgestion
    {
        [Key]
        
        public int Id { get; set; }
        [Required]
        public int idAsociado { get; set; }
        [Required]
        [Display(Name = "Clase Gestión")]
        [StringLength(20)]
        public string ClaseGestion{ get; set; }
        [Required]
        [Display(Name = "Fecha Gestión")]
        [DataType(DataType.DateTime)]
        public DateTime FechaGestion { get; set; }
        [Required]
        [Display(Name = "Respuesta Selección")]
        [StringLength(300)]
        public string RespuestaGestion { get; set; }
        [StringLength(20)]
        [Display(Name = "Respuesta con comentarios")]
        public string ResOpcionalGestion { get; set; }
        [Required]
        [Display(Name = "Contacto")]
        [StringLength(200)]
        public string ContactoGestion { get; set; }
        public Nullable<bool> GestionVerificada { get; set; }
        public List<GCcompromiso> Compromisos { get; set; }
    }
}
 