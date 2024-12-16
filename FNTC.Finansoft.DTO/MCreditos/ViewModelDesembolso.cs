using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class ViewModelDesembolso
    {
        public int id { get; set; }
        public Prestamos Prestamos { get; set; }
        [Display(Name = "Valor del Prestamo")]
        public string Capital { get; set; }

        //public Terceros Terceros { get; set; }
        [Display(Name = "NIT Asosciado")]
        public string NIT { get; set; }
        [Display(Name = "Nombre Asocioado")]
        public string NOMBRE { get; set; }

        [Display(Name = "Saldo Credito Desembolsado")]
        public string saldoCreditoDesembolsado { get; set; }

        public string cuentaCreditoCod { get; set; }
        public string cuentaDebitoCod { get; set; }
        public string cuentaCreditoDescripcion { get; set; }
        public string cuentaDebitoDescripcion { get; set; }
        public DateTime fechadesembolso { get; set; }
        public int diapago { get; set; }
        public string BANCO { get; set; }
        public string cajain { get; set; }
        public string Interes { get; set; }
        public string Resultado { get; set; } 
    } 
}