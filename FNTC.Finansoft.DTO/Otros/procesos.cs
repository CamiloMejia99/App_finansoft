using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Otros
{
    [Table(name: "dbo.procesos")]
    public class procesos
    {
        public int id { get; set; }
        [Required]
        public DateTime fecha { get; set; }
        [Required]
        public string proceso { get; set; }
    }
}
