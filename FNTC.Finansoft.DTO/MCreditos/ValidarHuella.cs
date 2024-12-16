using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    [Table("dbo.ValidarHuella")]
    public class ValidarHuella
    {
       
        [Key]
        public int Id { get; set; }
        public int IdCedula { get; set; }
        public string Pagare { get; set; }
        public DateTime Fechaingreso { get; set; }
        public string Token { get; set; }
        public int idProceso { get; set; }
        public int idControlAcceso { get; set; }
        public string TipoDeUsuario { get; set; } 

    }
}
 