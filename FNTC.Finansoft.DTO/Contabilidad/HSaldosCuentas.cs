using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Contabilidad
{
    [Table("des.HSaldosCuentas")]
    public class HSaldosCuentas
    {
        [Key]
        public int ID { get; set; }

        //[ForeignKey("CODIGO")]
        public string CODIGO { get; set; }

        [ForeignKey("CODIGO")]
        public virtual CuentaAuxiliar Auxiliar { get; set; }

        public int ANO { get; set; }
        public int MES { get; set; }
        public decimal MDEBITO { get; set; }
        public decimal MCREDITO { get; set; }
        public decimal SALDO { get; set; }

    }
}
