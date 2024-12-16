using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.TercerosOtrasEntidades
{
    [Table("dbo.tercerosEntidadDos")]
    public class tercerosEntidadDos
    {
        [Key]
        public string cedula { get; set; }
        public string nombre { get; set; }
        public string cuota { get; set; }
    }
}