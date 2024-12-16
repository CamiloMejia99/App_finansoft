using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FNTC.Finansoft.Accounting.DTO.Terceros;

namespace FNTC.Finansoft.Accounting.DTO.DescuentosNomina
{
    [Table("nom.DescuentosNominaRelacionPlanosEmpresa")]
    public class RelacionPlanosEmpresa
    {
        [Key]
        public int IdRelacionPlanosEmpresa { get; set; }
        [ForeignKey("tercero")]
        public string CodigoEmpresa { get; set; }
        [ForeignKey("Estructura")]
        public int CodigoPlano { get; set; }
        public DateTime FechaCreacionRelacionPlanosEmpresa { get; set; }
        public string UserControlRelacionPlanosEmpresa { get; set; }
        public bool EstadoRelacionPlanosEmpresa { get; set; }

        public virtual EstructuraPlanos Estructura { get; set; }
        public virtual Tercero tercero { get; set; }
    }
}



