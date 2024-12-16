using FNTC.Finansoft.Accounting.DTO.Terceros;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FNTC.Finansoft.Accounting.DTO.OperativaDeCaja
{
    [Table("dbo.factOpCajaConsCuotaCredito")]
    public class factOpCajaConsCuotaCredito
    {
        [Key]
        public int id { get; set; }
        public DateTime fecha { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        public string factura { get; set; }

        [ForeignKey("Terceros")]
        public string NIT { get; set; }
        public string codigoCaja { get; set; }
        public string pagare { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [DataType(DataType.Currency)]
        public string valorConsignado { get; set; }
        public string nitCajero { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [DataType(DataType.Currency)]
        public string abonoCapital { get; set; }
        public string numeroCuota { get; set; }
        public string interesCorriente { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [DataType(DataType.Currency)]
        public string interesMora { get; set; }
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [DataType(DataType.Currency)]
        public string seguros { get; set; }

        public string CtoAdmon { get; set; }

        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [DataType(DataType.Currency)]
        public string saldoCapital { get; set; }
        public string FormaPago { get; set; }

        [StringLength(10)]
        public string TipoComprobante { get; set; } 

        [StringLength(255)]
        public string NumeroComprobante { get; set; }

        [ForeignKey("NIT")]
        public virtual Tercero Terceros { get; set; }
    }
}