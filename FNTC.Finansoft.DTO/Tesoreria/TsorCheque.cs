using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Tesoreria
{
    [Table("dbo.TsorCheques")]
    public class TsorCheque
    {
        [Key]
        public int id { get; set; }
        public int codigoChequera { get; set; }
        public int consecutivo { get; set; }
        public DateTime fecha { get; set; }
        public string valor { get; set; }
        public bool confirmado { get; set; }
        public bool anulado { get; set; }
        public string NITTercero { get; set; }
        public string usuario { get; set; }
    }
}
