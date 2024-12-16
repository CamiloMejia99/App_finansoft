using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Tesoreria
{
    [Table("dbo.TsorBancos")]
    public class TsorBanco
    {
        [Key]
        public int id { get; set; }
        [DisplayName("Codigo")]
        [Required]
        public int codigo { get; set; }
        [ForeignKey("CuentaMayor")]
        [Required]
        public string cuenta { get; set; }

        public virtual CuentaMayor CuentaMayor { get; set; }
    }
}
