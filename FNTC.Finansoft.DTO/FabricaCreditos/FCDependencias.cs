using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("Fcred.Dependencias")]
    public class FCDependencias
    {
        [Key]
        [Display(Name = "Codigo")]
        public int idDependencia { get; set; }
        [Required]
        [Display(Name = "Nombre")]
        public string nombreDependencia { get; set; }
        [Display(Name = "Valor Minimo")]
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal montoMinimo { get; set; }
        [Display(Name = "Valor Maximo")]
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal montoMaximo { get; set; }
    }
}
