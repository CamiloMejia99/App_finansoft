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
    [Table("nom.DescuentosNominaSaldosSobrantes")]
    public class SaldosSobrantes
    {
        [Key]
        public int IdSaldosSobrantes { get; set; }
        [ForeignKey("CuentaFK")]
        public string CodigoCuenta { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UserControl { get; set; }
        public bool Estado { get; set; }

        public virtual CuentaMayor CuentaFK { get; set; }
    }
}
