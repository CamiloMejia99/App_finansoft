using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.SIAR
{
    [Table("spe.parametros")]
    public class SIARparametros
    {
        [Key]
        public int id { get; set; }


        public int clasificacion { get; set; }
        public int linea { get; set; }
        public int subclasificacion { get; set; }

        [DataType(DataType.Date)]
        public DateTime fechaCorte { get; set; }
        public bool estado { get; set; }

    }
}
