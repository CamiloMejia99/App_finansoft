using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.FabricaCreditos
{
    [Table("Fcred.ActividadesFinancierasAso")]
    public class FCActividadesFinancieras
    {
        [Key]
        public int idActividadFinanciera { get; set; }
        public string idAsociado { get; set; }
        public string otroCredito { get; set; }
        public string idAsociadoCodeudor { get; set; }
        public string ingresosMensuales { get; set; }
        public string EgresosMensuales { get; set; }
        public string activos { get; set; }
        public string pasivos { get; set; }
    }
}

