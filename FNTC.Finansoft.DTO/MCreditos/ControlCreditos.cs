using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    [Table("cre.ControlCreditos")]
    public class ControlCreditos
    {
        [Key]
        public int Id { get; set; }
        public string Pagare { get; set; }
        public int NumCuota { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime FechaPago { get; set; }
        public int DiasMora { get; set; }
        public int DiasCausados { get; set; }
        public decimal Capital { get; set; }
        public decimal SaldoCapitalEnCuota { get; set; }
        public decimal InteresCorriente { get; set; }
        public decimal InteresMora { get; set; }
        public decimal Seguro { get; set; }
        public decimal CtoAdmon { get; set; }

        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [DataType(DataType.Currency)]
        public decimal ValorCuota { get; set; }
        public string EstadoEnCredito { get; set; }
        public bool EstadoEnOperacion { get; set; }
    }
}
