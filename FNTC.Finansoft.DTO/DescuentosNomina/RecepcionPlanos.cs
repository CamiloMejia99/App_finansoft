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
    [Table("nom.DescuentosNominaRecepcionPlanos")]
    public class RecepcionPlanos
    {
        [Key]
        public int IdRecepcionPlanos { get; set; }
        public int IdRelacionPlano { get; set; }
        public string direccionDocumento { get; set; }
        public bool Extencion { get; set; }
    }
}
 