using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.MediosMagneticos
{
    [Table("med.conceptos")]
    public class conceptos
    {
        [Key]
        [Required]
        public int idConceptos { get; set; }
        [Required]
        public int codigo { get; set; }
        [Required]
        public int codigoConceptos { get; set; }
        [Required]
        public string descripcion { get; set; }
        [Required]
       
        public int idFormato { get; set; }
    }
}







