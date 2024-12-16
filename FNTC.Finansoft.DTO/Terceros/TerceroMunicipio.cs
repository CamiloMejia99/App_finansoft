using FNTC.Finansoft.Accounting.DTO.Geo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Terceros
{
    [Table("ter.Terceromunicipio")]
    public class TerceroMunicipio
    {
        [Key]
        public Tercero Tercero { get; set; }
        [Key]
        public Municipio Municipio { get; set; }
        [Key]
        public int TipoOrigen { get; set; }
    }
}
