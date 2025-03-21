using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.Informes
{
    public class TotalSaldoCuenta
    {
        [Key]
        public string CUENTA { get; set; }
        public decimal CREDITO { get; set; }
        public decimal DEBITO { get; set; }
    }
}
