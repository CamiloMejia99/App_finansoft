using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.Parametros
{
    [Table("dbo.TiposProducto")]
    public class TipoProducto
    {

        [Key]
        public int IdProducto { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

    }
}
