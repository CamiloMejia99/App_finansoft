using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.OperativaDeCaja
{
    [Table("dbo.convenio")]
    public class convenio
    {
        [Key]
        public string codigo { get; set; }
        public string nombre_convenio { get; set; }
        public string cuenta { get; set; }

        public virtual CuentaMayor PlanCuentas { get; set; }

    }
}