using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using FNTC.Finansoft.Accounting.DTO.Parametros;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Tesoreria
{
    [Table("dbo.TsorMatricularBancos")]
    public class TsorMatricularBanco
    {
        [Key]
        public int id { get; set; }
        [DisplayName("Codigo Banco")]
        [ForeignKey("TsorBancos")]
        public int codigo { get; set; }
        public string NIT { get; set; }
        [DisplayName("Numero de Cuenta")]
        public string numeroCuenta { get; set; }
        [DisplayName("Tipo de Cuenta")]
        [ForeignKey("Parameter")]
        public int tipoCuenta { get; set; }
        [DisplayName("Agencia")]
        [ForeignKey("agencias")]
        public int codigoagencia { get; set; }
        [DisplayName("Comprobante")]
        public string formatoComprobante { get; set; }
        [DisplayName("Formato de Impresion")]
        [ForeignKey("Parameter1")]
        public int formatoImpresion { get; set; }

        public virtual agencias agencias { get; set; }
        public virtual Parameter Parameter { get; set; }
        public virtual Parameter Parameter1 { get; set; }
        public virtual TsorBanco TsorBancos { get; set; }
        //public virtual Parameter parameter { get; set; }
    }
}
