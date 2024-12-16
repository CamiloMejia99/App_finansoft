using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    [Table("cre.TotalesCreditos")]
    public class TotalesCreditos
    {
        public int Id { get; set; }

        //[ForeignKey("creditoFK")]
        //public int IdCredito { get; set; }

        public string Pagare { get; set; }

        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [DataType(DataType.Currency)]
        public decimal CapitalTotal { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [DataType(DataType.Currency)]
        public decimal SaldoCapital { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [DataType(DataType.Currency)]
        public decimal CapitalMoraPendiente { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [DataType(DataType.Currency)]
        public decimal InteresCorrienteTotal { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [DataType(DataType.Currency)]
        public decimal InteresMoraTotal { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [DataType(DataType.Currency)]
        public decimal SeguroTotal { get; set; }
        public decimal CtoAdmonTotal { get; set; }

        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [DataType(DataType.Currency)]
        public decimal InteresCorrientePendiente { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [DataType(DataType.Currency)]
        public decimal InteresMoraPendiente { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [DataType(DataType.Currency)]
        public decimal SeguroPendiente { get; set; }
        public decimal CtoAdmonPendiente { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaProximoPago { get; set; }
        public int DiasMora { get; set; }
        public string Estado { get; set; }
        public string Gestion { get; set; } 

        //public virtual BCreditos creditoFK { get; set; }
    }
}
