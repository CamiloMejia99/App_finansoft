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
    [Table("nom.DescuentosNominaEstructuraPlanos")]
    public class EstructuraPlanos
    {
        [Key]
        public int IdEstructuraPlanos { get; set; }
        public string NombreEstructuraPlanos { get; set; }
        public DateTime FechaCreacionEstructuraPlanos { get; set; }
        public string UserControlEstructuraPlanos { get; set; }
        public bool EstadoEstructuraPlanos { get; set; }

    }
}

 