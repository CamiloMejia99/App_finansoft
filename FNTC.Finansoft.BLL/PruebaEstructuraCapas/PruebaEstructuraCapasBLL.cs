using FNTC.Finansoft.Accounting.DAL.PruebaEstructuraCapas;
using FNTC.Finansoft.Accounting.DTO.PruebaEstructuraCapas;
using System.Collections.Generic;

namespace FNTC.Finansoft.Accounting.BLL.PruebaEstructuraCapas
{
    public class PruebaEstructuraCapasBLL
    {
        public List<PruebaEstructura> GetPruebaEstructuras()
        {
            return new PruebaEstructuraCapasDAL().GetPruebaEstructuras();
        }

        public bool Create(PruebaEstructura pruebaEstructura)
        {
            return new PruebaEstructuraCapasDAL().Create(pruebaEstructura);
        }
    }
}
