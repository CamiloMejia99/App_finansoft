using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Tesoreria
{
    [Table("dbo.TsorConsecutivosChequera")]
    public class TsorConsecutivosChequera
    {
        [Key]
        public int id { get; set; }

        [DisplayName("Codigo Banco Matriculado")]
        [Required]
        public int codigoBancoMatriculado { get; set; }

        [DisplayName("Codigo Chequera")]
        [Required]
        public int codigoChequera { get; set; }

        [DisplayName("Estado")]
        public bool estado { get; set; }

        [DisplayName("Consecutivo Inicial")]
        [Required]
        public int consecutivoInicial { get; set; }

        [DisplayName("Consecutivo Final")]
        [Required]
        public int consecutivoFinal { get; set; }

        [Required]
        [DisplayName("Consecutivo Actual")]
        public int consecutivoActual { get; set; }

        [DisplayName("Alerta Cheques Agotados")]
        public bool alertaChequesAgotados { get; set; }

        [DisplayName("Numero Alerta Cheques Agotados")]
        public int numeroAlertaChequesAgotados { get; set; }
    }
}
