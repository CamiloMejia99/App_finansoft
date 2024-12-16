using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.ControlCartera
{
    [Table("cart.ConveniosTipoDeConvenios")]
    public class CRConveniosTipoDeConvenios
    {
        [Key]
        public int idTipoDeConvenios { get; set; }
        [Required]
        [Display(Name = "Tipo De Convenios")]
        public string TipoDeConvenios { get; set; }
        [Required]
        [Display(Name = "Estado")]
        public string Estado { get; set; }
    }
}
