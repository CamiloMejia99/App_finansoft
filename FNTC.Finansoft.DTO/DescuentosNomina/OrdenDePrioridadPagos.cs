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
    [Table("nom.DescuentosNominaOrdenDePrioridadPagos")]
    public class OrdenDePrioridadPagos
    {
        [Key]
        public string IdOrdenPrioridadPagos { get; set; }
        [ForeignKey("cuentaMayor")]
        public string CodigoCuenta { get; set; }
       
        public string DescripcionPagos { get; set; }
        [ForeignKey("tipoPagos")]
        public string OrdenPagos { get; set; }
        public string UserControlPagos { get; set; }
        public DateTime FechaCreacionPagos { get; set; }

        public virtual CuentaMayor cuentaMayor { get; set; }
        public virtual TipoPagos tipoPagos { get; set; }
    }
}
