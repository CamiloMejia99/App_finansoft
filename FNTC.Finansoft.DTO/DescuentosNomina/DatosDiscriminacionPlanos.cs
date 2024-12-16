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
    [Table("nom.DescuentosNominaDatosDiscriminacionPlanos")]
    public class DatosDiscriminacionPlanos
    {
        [Key]
        public int IdDisPlanos { get; set; }
        public string NitEmpresa { get; set; }
        public string DigitoVerificacion { get; set; }
        public string TipoDeEstadoProceso { get; set; }
        public string NitAsociado { get; set; }
        public string NombreCompleto { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegunoApellido { get; set; }
        public decimal TotalAportes { get; set; }
        public int idPlano { get; set; }
        public int NoDiscriminacion { get; set; }
        public int idEmpresaRelacion { get; set; }
        public string PeriodoDeduccion { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UserControl { get; set; } 
        public bool EstadoDisPlanoAsociado { get; set; }
        public string EstadoContable { get; set; }



    }
}
