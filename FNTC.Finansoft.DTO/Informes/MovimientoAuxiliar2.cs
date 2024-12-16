using System;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.Informes
{
    public class MovimientoAuxiliar2
    {
        [Key]
        public string TIPO { get; set; }
        public string NUMERO { get; set; }
        public string CUENTA { get; set; }
        public string TERCERO { get; set; }
        public string DETALLE { get; set; }
        public decimal DEBITO { get; set; }
        public decimal CREDITO { get; set; }
        public decimal BASE { get; set; }
        public string CCOSTO { get; set; }
        public DateTime FECHAMOVIMIENTO { get; set; }
        public string NOMBRE { get; set; }
        public string NATURALEZA { get; set; }
        public string NOMBRECUENTA { get; set; }
        public int AGENCIA { get; set; }
    }
}
