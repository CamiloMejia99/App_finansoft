using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Geo
{

    [Table("geo.Pais")]
    public class Pais
    {
        [Key]
        [Required]
        public int Id_pais { get; set; }

        [StringLength(60)]
        public string Nom_pais { get; set; }
    }
}
