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
    [Table("nom.DescuentosNominaContabilizacionPlanos")]
    public class ContabilizacionPlanos
    {
        [Key]
        public int IdContabilizacionPlanos { get; set; }
        public int IdRelacionPlanos { get; set; }
    }
}
