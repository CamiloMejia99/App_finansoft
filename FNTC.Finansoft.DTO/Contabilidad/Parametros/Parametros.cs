using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.Contabilidad
{
    public class ParametrosGenerales
    {
        [Key]
        public int ID { get; set; }

        public string Entidad { get; set; }
        public string Detalle { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}
