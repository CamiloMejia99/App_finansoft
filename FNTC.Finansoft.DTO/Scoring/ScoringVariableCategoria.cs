using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Scoring
{
    [Table("dbo.ScoringVariableCategorias")]
    public class ScoringVariableCategoria

    {
        [Required]
        public int id { get; set; }

        [Required]
        [Display(Name = "Nombre de Categoria")]
        public string NombreCategoria { get; set; }

        [Display(Name = "Descripcion de Categoria")]
        public string DescripcionCategoria { get; set; }

        [Required]
        [Display(Name = "Puntaje Categorias")]
        public int PuntajeCategorias { get; set; }
    }
}
