using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FormularioAperturaCtaAhorros
{
    [Table("aho.FormulariosApertura")]
    public class FormulariosApertura
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string IdTercero { get; set; }
        [Required]
        public string TokenPdf { get; set; }
        [Required]
        public string NombreArchivoPdf { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime FechaApertura { get; set; }
        [Required]
        public DateTime FechaSistema { get; set; }
        [Required]
        public string UserLog { get; set; }
        [Required]
        public bool Estado { get; set; }

    }
}




