using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.Aportes
{
    public class RespuestaAporte
    {
        public bool Status { get; set; }
        public int Id { get; set; }
        public decimal PagoCuentas { get; set; }
        public List<CuentaDistribucionAporte> CuentasDistribucion { get; set; }
        public string Mensaje { get; set; }
    }

    public class RespuestaAhorro
    {
        public bool Status { get; set; }
        public int Id { get; set; }
        public string Mensaje { get; set; }
    }
}
