using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.ActivosFijos
{
    [Table(name: "dbo.HistorialActivosFijos")]
    public class HistorialActivosFijos
    {
        public int id { get; set; }

        [Required]
        public int idActivo { get; set; }

        [Required]
        public DateTime fecha { get; set; }

        [Required]
        public string concepto { get; set; }

        [Required]
        public decimal valorEnLibros { get; set; }

        [Required]
        public decimal valorMovimiento { get; set; }

        [Required]
        public string tipoComprobante { get; set; }

        [Required]
        public string numeroComprobante { get; set; }

        [Required]
        public int tipoMovimiento { get; set; }

    }
}
