using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.ControlCartera
{
    [Table("cart.ControlTransferenciaCartera")]
    public class CRControlTransferenciaCartera
    {

        [Key]
        public int IdCoTrCa { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public string Pagare { get; set; }
        public string Transaccion { get; set; }
        public string DetallesDeTransaccion { get; set; } 

    }
}


