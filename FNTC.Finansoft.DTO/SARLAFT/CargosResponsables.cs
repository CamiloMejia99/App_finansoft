using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.SARLAFT
{
    [Table("spe.CargosResponsables")]
    public class CargosResponsables
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        public string cargo { get; set; }


        public bool estado { get; set; }
    }
}
