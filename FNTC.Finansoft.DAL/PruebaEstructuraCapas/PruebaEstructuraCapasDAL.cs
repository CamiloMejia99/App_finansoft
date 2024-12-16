using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.PruebaEstructuraCapas;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace FNTC.Finansoft.Accounting.DAL.PruebaEstructuraCapas
{
    public class PruebaEstructuraCapasDAL
    {
        public List<PruebaEstructura> GetPruebaEstructuras()
        {
            using (var ctx = new AccountingContext())
            {
                return ctx.PruebaEstructuras.ToList();
            }
        }

        public bool Create(PruebaEstructura pruebaEstructura)
        {
            using (var ctx = new AccountingContext())
            {
                try
                {
                    ctx.PruebaEstructuras.Add(pruebaEstructura);
                    ctx.SaveChanges();
                    return true;
                }
                catch (DbEntityValidationException dbE)
                {
                    return false;
                }
            }
        }

    }
}
