using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.MediosMagneticos
{
    [Table("med.formatos")]
    public class formatos
    {
        [Key]
        [Required]
        public int idFormato { get; set; }
        [Required]
        public int codigoFormato { get; set; }
        [Required]
        public string descripcion { get; set; }
    }
}



   

