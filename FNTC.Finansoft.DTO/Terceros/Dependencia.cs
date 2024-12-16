using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Terceros
{
    [Table("ter.Dependencia")]
    public class Dependencia
    {
        [Key]
        public int Id_dep { get; set; }

        [StringLength(255)]
        public string Nom_dep { get; set; }

        public List<TerceroDependencia> TerceroDependencia { get; set; }
    }
}
