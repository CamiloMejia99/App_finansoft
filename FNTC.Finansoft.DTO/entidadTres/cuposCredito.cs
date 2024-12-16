using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.entidadTres
{
    [Table(name: "dbo.cuposCreditoEntidadTres")]
    public class cuposCredito
    {
        [Key]
        public string NIT { get; set; }

        [Required]
        public decimal cupo { get; set; }
    }
}
