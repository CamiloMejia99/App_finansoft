using FNTC.Finansoft.Accounting.DTO.Terceros;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Geo
{
    [Table("geo.Municipio")]
    public class Municipio
    {
        [Key]
        [Required, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id_muni { get; set; }

        [StringLength(60)]
        public string Nom_muni { get; set; }

        [ForeignKey("departamentoFK")]
        public int Dep_muni { get; set; }

        [ForeignKey("Dep_muni")]
        public Departamento Departamento { get; set; }

        public List<TerceroMunicipio> TerceroMunicipio { get; set; }

        public virtual Departamento departamentoFK { get; set; }




    }
}
