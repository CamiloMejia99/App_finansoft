using System;

namespace FNTC.Finansoft.DTO.Ahorros
{
    public class DTODetallesFichas
    {
        public int id { get; set; }
        public string numeroFicha { get; set; }
        public string valorPagado { get; set; }
        public Nullable<System.DateTime> fechaPago { get; set; }

        //adicional
        public string saldoTotal { get; set; }
    }
}
