
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.Fichas
{
    [Table("apo.ConfiguracionAportesEx")]
    public partial class Configuracion2Ex
    {
        [Key]
        public int IdConfiguracionAportesEx { get; set; }
        public string nombreAbreviatura { get; set; }
        public string idCuenta { get; set; }
        public string UsuarioActualizacion { get; set; }
        public Nullable<bool> estado { get; set; }
        public Nullable<System.DateTime> fechaRegistro { get; set; }
    }
}


 