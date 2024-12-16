using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Terceros
{
    [Table("ter.Profesion")]
    public class Profesion
    {
        [Key]
        public int Id_prof { get; set; }

        [StringLength(60)]
        public string Nom_prof { get; set; }

        public virtual ICollection<Tercero> Terceros { get; set; }
    }
}
