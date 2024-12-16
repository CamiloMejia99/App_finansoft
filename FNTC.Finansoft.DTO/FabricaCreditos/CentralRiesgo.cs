using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("Fcred.CentralesRiesgo")]
    public class CentralRiesgo
    {
        [Key]
        public int idCentralRiesgo { get; set; }
        [Required]
        [Display(Name = "Nombre Central")]
        public string nombreCentral { get; set; }
        [Required]
        [Display(Name = "Descripción")]
        public string descripcionCentral { get; set; }
        [Display(Name = "Valor Consulta")]
        [RegularExpression("(^[0-9,]+$)", ErrorMessage = "Solo se permiten números")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El número es obligatorio")]
        [DataType(DataType.Currency)]
        public decimal costoConsulta { get; set; }
    }
}

