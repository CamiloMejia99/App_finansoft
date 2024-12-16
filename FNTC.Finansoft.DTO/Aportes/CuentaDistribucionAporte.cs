using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.Aportes
{
    [Table("apo.CuentasDistribucionAportes")]
    public class CuentaDistribucionAporte
    {
        [Key]
        public int Id { get; set; } 

        [ForeignKey("CuentaFK")]
        [Required]
        public string Cuenta { get; set; }


        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Porcentaje { get; set; }

        [NotMapped]
        [Required]
        [Display(Name ="Porcentaje")]
        public string AuxPorcentaje { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime FechaRegistro { get; set; }


        public bool Estado { get; set; }    


        public virtual CuentaMayor CuentaFK { get; set; }
    }
}
