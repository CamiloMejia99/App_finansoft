using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;

namespace FNTC.Finansoft.Accounting.DTO.DescuentosNomina
{
    [Table("nom.ControlDeMovimientos")]
    public class ControlDeMovimientos
    {
        [Key]
        public int IdControl { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UserControl { get; set; }
        public string Detalles { get; set; }

    }
}

