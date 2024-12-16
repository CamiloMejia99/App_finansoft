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
    [Table("nom.DescuentosNominaCamposRelacionDis")]
    public class CamposRelacionDis
    {
        [Key]
        public int IdRelacionDis { get; set; }
        public string NombreDisCampo { get; set; }
        public bool EstadoRelacionDis { get; set; }
    }
}
