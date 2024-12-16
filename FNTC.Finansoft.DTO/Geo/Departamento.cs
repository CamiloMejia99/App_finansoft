using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Geo
{
    [Table("geo.Departamento")]
    public class Departamento
    {
        [Key]
        [Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_dep { get; set; }

        [StringLength(60)]
        public string Nom_dep { get; set; }

        public int Pais_dep { get; set; }

        [ForeignKey("Pais_dep")]
        Pais Pais { get; set; }
    }
}
