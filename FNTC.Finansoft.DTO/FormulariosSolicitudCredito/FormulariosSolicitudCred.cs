using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Configuration;

namespace FNTC.Finansoft.Accounting.DTO.FormulariosSolicitudCredito
{
    [Table("cre.FormulariosSolicitudCred")]
    public class FormulariosSolicitudCred
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
        public DateTime FechaAfiliacion { get; set; }
        [Required]
        public DateTime FechaSistema { get; set; }
        [Required]
        public string UserLog { get; set; }
        [Required]
        public bool Estado { get; set; }

    }
}

