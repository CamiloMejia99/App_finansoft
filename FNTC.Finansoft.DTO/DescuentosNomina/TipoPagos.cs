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
    [Table("nom.DescuentosNominaTiposDePagos")]
    public class TipoPagos
    {
        [Key]
        public string IdTiposPagos { get; set; }
        public string NombrePago { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UserControl { get; set; }
        public int Orden { get; set; }
    }
}
