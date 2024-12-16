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
    [Table("nom.DescuentosConformacionDeLosPlanos")]
    public class ConformacionDeLosPlanos
    {
        [Key]
        public int IdConformacionDeLosPlanos { get; set; }
        [ForeignKey("estructura")]
        public int IdPlanos { get; set; }
        public string NombreCampo { get; set; }
        public bool ValorNulo { get; set; }
        public int OrdenCampo { get; set; }
        [ForeignKey("CampoRelacion")]
        public int Campo { get; set; }
        public DateTime FechaCreacionCampo { get; set; }
        public string UserControlCampo { get; set; }
        public bool EstadoCampo { get; set; }
        public bool ValorPredeterminado { get; set; }
        public string ContenidoValorPredeterminado { get; set; }

        public virtual EstructuraPlanos estructura { get; set; }
        public virtual CamposRelacionDis CampoRelacion { get; set; }
    }
}
