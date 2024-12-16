using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace FNTC.Finansoft.Accounting.DTO.SARLAFT
{
    [Table("spe.Procesos")]
    public class Procesos
    {
        [Key]
        [Required]
        public int id { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Código")]
        [Remote("verificarCodigo", "Proceso", ErrorMessage = "{0} duplicado!")]
        public string codigo { get; set; }
        [Required]
        [DisplayName("Nombre")]
        public string nombre { get; set; }
        [Required]
        [DisplayName("Objetivo")]
        public string objetivo { get; set; }

        [DisplayName("Responsable")]
        [ForeignKey("responsableFK")]
        [Required]
        public int responsable { get; set; }


        [DisplayName("Macroproceso")]
        [ForeignKey("macroprocesoFK")]
        [Required]
        public int macroproceso { get; set; }
        public bool estado { get; set; }

        public virtual CargosResponsables responsableFK { get; set; }
        public virtual Macroprocesos macroprocesoFK { get; set; }

    }
}
