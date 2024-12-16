using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.Contabilidad
{
    public class CuentaImpuestos : CuentaMayor
    {
        [Required]
        public string Tipo { get; set; }

        //[ForeignKey("Tipo")]
        //private TipoCuentaImpuestos _tipos {get;set;}

        public string Detalle { get; set; }

        [Required]
        public decimal Base { get; set; }
        /*
        [Required]
        public decimal  Porcentaje { get; set; }
        */
    }
}
