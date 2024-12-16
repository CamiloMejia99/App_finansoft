using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace FNTC.Finansoft.Accounting.DTO.SARLAFT
{
    [Table("spe.Macroprocesos")]
    public class Macroprocesos
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Código")]
        [Remote("verificarCodigo", "Sarlaft", ErrorMessage = "{0} duplicado!")]
        public string codigo { get; set; }

        [Required]
        [DisplayName("Nombre")]
        public string nombre { get; set; }

        [Required]
        [DisplayName("Objetivo")]
        public string objetivo { get; set; }
        public bool estado { get; set; }



    }
}
