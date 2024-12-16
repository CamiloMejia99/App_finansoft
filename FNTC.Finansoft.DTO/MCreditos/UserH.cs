using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    [Table("dbo.User")]
    public class UserH
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public byte[] Huella { get; set; }
        public string Cedula { get; set; }
        public byte[] TemplateBytes { get; set; }
        public int TemplateSize { get; set; }
    }
}


