namespace FNTC.Finansoft.Accounting.DTO.Contabilidad
{
    using FNTC.Finansoft.Accounting.DTO.Terceros;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("acc.SaldosTerceros")]
    public partial class SaldosTercero
    {
        [Key]
        public int ID { get; set; }

        [StringLength(10)]
        [ForeignKey("cuentaFK")]
        public string CODIGO { get; set; }

        [StringLength(20)]
        [ForeignKey("terceroFK")]
        public string TERCERO { get; set; }

        public int ANO { get; set; }

        public int MES { get; set; }


        public decimal MDEBITO { get; set; }

        public decimal MCREDITO { get; set; }

        public decimal SALDO { get; set; }

        //public decimal MDEBITO2 { get; set; }

        //public decimal MCREDITO2 { get; set; }

        //public decimal SALDO2 { get; set; }

        public virtual CuentaMayor cuentaFK { get; set; }
        public virtual Tercero terceroFK { get; set; }
    }
}
