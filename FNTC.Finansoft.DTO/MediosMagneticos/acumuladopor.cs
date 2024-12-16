using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.MediosMagneticos
{
    [Table("med.acumulado")]
    public class acumuladopor
    {
        [Key]
        [Required]
        public int idAcumulado { get; set; }
        [Required]
        public string nombre { get; set; }
        
    }
}





