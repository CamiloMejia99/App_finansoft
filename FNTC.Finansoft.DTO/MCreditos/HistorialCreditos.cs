using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    [Table(name: "dbo.HistorialCreditos")]
    public class HistorialCreditos
    {
        public int id { get; set; }
        public DateTime fecha { get; set; }
        public DateTime fechaProximoPago { get; set; }
        public int idFactura { get; set; }
        public string NIT { get; set; }
        public string pagare { get; set; }
        public decimal abonoCapital { get; set; }
        public decimal abonoInteresMora { get; set; }
        public decimal AbonoInteresCorriente { get; set; }
        public decimal valorCosto { get; set; }
        public decimal saldoCapital { get; set; }
        public decimal proximaCuota { get; set; }
        public decimal capitalEnMora { get; set; }
        public decimal TazaInteresMora { get; set; }
        public decimal TazaInteresCorriente { get; set; }
        public int diasCausados { get; set; }
        public int diasEnMora { get; set; }
        public int numeroCuota { get; set; }
        public decimal interesCorrienteMora { get; set; }
        public decimal interesCorriente { get; set; }
        public decimal interesMora { get; set; }
        public string estado { get; set; }
    }
}
