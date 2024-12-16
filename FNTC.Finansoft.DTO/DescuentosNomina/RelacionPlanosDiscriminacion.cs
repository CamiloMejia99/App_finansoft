using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;

namespace FNTC.Finansoft.Accounting.DTO.DescuentosNomina
{
    [Table("nom.DescuentosNominaRelacionPlanosDiscriminacion")]
    public class RelacionPlanosDiscriminacion
    {
        [Key]
        public int IdRelacionPD { get; set; }
        public int IdPlano { get; set; }
        public int NoDiscriminacionPlano { get; set; }
        public int IdRelacionEmpresa { get; set; }
        public string IdEmpresa { get; set; } 
        public string UserControl { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string PeriodoDeduccion { get; set; }
        public string EstadoContable { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public string Identificador { get; set; }


    }
}


