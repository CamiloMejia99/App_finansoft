using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class GarantiasCreditos
    {
        public int id { get; set; }

        [Required]
        public int garantia_id { get; set; }

        public int Real_Valor { get; set; }
        [Required]
        public string pagare { get; set; }

        public int codeudor_nit { get; set; }

        public string nombre_codeudor { get; set; }

        [Required]
        public int valor_credito { get; set; }
    }
}
