namespace FNTC.Finansoft.Accounting.DTO.Contabilidad
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("acc.SaldosCCs")]
    public partial class SaldoCC
    {
        [Key]
        public int ID { get; set; }

        [StringLength(255)]
        public string CUENTA { get; set; }

        [StringLength(20)]
        public string TERCERO { get; set; }

        [StringLength(3)]
        public string CCOSTO { get; set; }

        //[StringLength(3)]
        //public string SCCOSTO { get; set; }

        public int ANO { get; set; }

        public int MES { get; set; }


        public decimal MDEBITO { get; set; }

        public decimal MCREDITO { get; set; }

        public decimal SALDO { get; set; }

        public string TIPO { get; set; }

    }
}
