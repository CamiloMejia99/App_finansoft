using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.Informes
{
    public partial class spBalanceComprobacionL5
    {
        //[Key]
        //public int id { get; set; }

        //[DataType(DataType.Date)]
        //public DateTime fecha { get; set; }

        [Key]
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string DocumentoTercero { get; set; }
        public string NombreTercero { get; set; }

        public string SaldoInicial { get; set; }
        public string Debito { get; set; }
        public string Credito { get; set; }
        public string Saldo { get; set; }
    }
}
