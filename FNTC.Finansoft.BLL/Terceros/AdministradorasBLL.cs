using FNTC.Finansoft.Accounting.DTO.Terceros;
using System.Collections.Generic;

namespace FNTC.Finansoft.Accounting.BLL.Terceros
{
    public class AdministradorasBLL
    {

        public List<TerceroDTO> Terceros { get { return new DAL.TercerosDAL().Terceros; } }
    }
}
