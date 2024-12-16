using FNTC.Finansoft.Accounting.DAL;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FNTC.Finansoft.Accounting.BLL.Movimientos
{
    public class MovimientosBLL
    {
        public List<Movimiento> GetMovimientos(string tipo, string numero)
        {
            var list = new MovimientosDAL().GetMovimientos(tipo,numero);
            return list;
        }

        public async Task<JsonResult> GetProductosAsociadoAsync(string NIT, string cuenta)
        { 
            return await new MovimientosDAL().GetProductosAsociadoAsync(NIT, cuenta);   
        }
    }
}
