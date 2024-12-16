using System;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.Informes
{
    public class sp_reporte17
    {
        [Key]
        public string NumeroCuenta { get; set; }


        public string idPersona { get; set; }
        public string nombre { get; set; }

        public string valor { get; set; }
        public string totalAportes { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaApertura { get; set; }
        public string tipoPago { get; set; }
        public string TELMOVIL { get; set; }
        public int codigoagencia { get; set; }
        public string nombreagencia { get; set; }
        public int Npagos { get; set; }



    }
}
