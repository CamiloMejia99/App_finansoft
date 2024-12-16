using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Parametros
{
    [Table("par.Parameters")]
    public class Parameter
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Categoria { get; set; }


        [Required]
        public string NombreParametro { get; set; }

        [Required]
        public string Codigo { get; set; }


        public string Descripcion { get; set; }

        public string Valor { get; set; }

        public string TipoValor { get; set; }
    }
}
