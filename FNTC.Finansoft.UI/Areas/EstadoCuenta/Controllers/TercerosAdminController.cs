using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Fichas;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FNTC.Finansoft.UI.Areas.EstadoCuenta.Controllers
{
    public class TercerosAdmin
    {
        public List<Tercero> Consultar()
        {


            using (var contexto = new AccountingContext())
            {
                var terceros = contexto.Terceros.AsNoTracking().ToList();
                return terceros;
            }


        }

    }
}