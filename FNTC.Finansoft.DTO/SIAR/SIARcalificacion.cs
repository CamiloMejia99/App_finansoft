using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.SIAR
{
    [Table("spe.Calificaciones")]
    public class SIARcalificacion
    {
        [Key]
        public int id { get; set; }
        public int rangoIni { get; set; }
        public int rangoFin { get; set; }
        public string calificacion { get; set; }
        public string descripcionRango { get; set; }

        [ForeignKey("subclasificacionFK")]
        public int idSubclasificacion { get; set; }

        public decimal PDI { get; set; }

        public virtual SIARcalificacion subclasificacionFK { get; set; }
    }
}
