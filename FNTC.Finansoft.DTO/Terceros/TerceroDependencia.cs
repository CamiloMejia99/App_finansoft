using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Terceros
{
    [Table("ter.Tercerodep")]
    public class TerceroDependencia
    {
        [Key, Column(Order = 0), ForeignKey("Tercero")]
        public string Nit_tercero { get; set; }

        [Key, Column(Order = 1), ForeignKey("Dependencia")]
        public int Cod_dependencia { get; set; }

        [Key, Column(Order = 2), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Tipo_dep { get; set; }

        public Tercero Tercero { get; set; }
        public Dependencia Dependencia { get; set; }
    }
}
