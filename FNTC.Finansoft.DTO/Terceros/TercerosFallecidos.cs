using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Terceros
{
    [Table("ter.TercerosFallecidos")]
    public class TercerosFallecidos
    {
        [Key]
        public int id { get; set; }

        [Required]
        [Display(Name = "ASOCIADO")]
        [ForeignKey("terceroFK")]
        public string nit { get; set; }

        [Required]
        [Display(Name = "FECHA DE DEFUNCIÓN")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechaFallecido { get; set; }

        public virtual Tercero terceroFK { get; set; }
    }
}
