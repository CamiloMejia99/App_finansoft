using FNTC.Finansoft.Accounting.DTO.Terceros;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Facturacion
{
    [Table("fac.factura")]
    public class factura
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("terceroFK")]
        public string personId { get; set; }

        //[ForeignKey("usersTabla")]
        public string userId { get; set; }

        public int operationTypeId { get; set; }
        public decimal cash { get; set; }
        public decimal payCash { get; set; }
        public decimal payCredit { get; set; }
        public decimal payTdebit { get; set; }
        public decimal payTcredit { get; set; }
        public decimal totalDiscount { get; set; }
        public DateTime date { get; set; }
        public string observation { get; set; }
        public int stateId { get; set; }
        public decimal total { get; set; }
        public int tipo { get; set; }
        public decimal saldoCredito { get; set; }
        public string fechaPagoCredito { get; set; }

        public string codConsecutivo { get; set; }
        public int numeroFactura { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal valorTotalExcentos { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal valorTotalExcluidos { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal baseIVA19 { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal baseIVA5 { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal valorIVA19 { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal valorIVA5 { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal totalBolsas { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal valorConvenio { get; set; }

        public string facturaFisica { get; set; }

        [ForeignKey("formaPagoFK")]
        public int idFormaPago { get; set; }


        public virtual Tercero terceroFK { get; set; }
        public virtual FacFormasDePago formaPagoFK { get; set; }
        //public virtual usersTabla usersTabla { get; set; }
    }
}
