using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    [Table(name: "dbo.interescausadoprestamos")]
    public class interescausadoprestamos
    {
        public int id { get; set; }
        [Required]
        public string pagare { get; set; }
        public decimal intcorriente { get; set; }
        public decimal intmora { get; set; }
        public DateTime fechasistema { get; set; }
        [Required]
        public int agenciaId { get; set; }
        [Required]
        public string usuarioCauso { get; set; }
        public decimal Tasainteres { get; set; }
        public int numeroCuota { get; set; }
        public string comprabante { get; set; }
        public int consecutivo { get; set; }
        public string codcuentaingresosctes { get; set; }
        public string codcuentaingresosmora { get; set; }
        public decimal valorretencion { get; set; }
        public decimal porcentajeretencion { get; set; }
        public decimal TasainteresMora { get; set; }
    }
}
