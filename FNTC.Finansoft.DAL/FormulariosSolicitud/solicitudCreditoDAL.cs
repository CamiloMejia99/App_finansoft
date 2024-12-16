using FNTC.Finansoft.Accounting.DTO.FormulariosSolicitud;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;

namespace FNTC.Finansoft.Accounting.DAL.FormulariosSolicitud
{
    public class solicitudCreditoDAL
    {
        public List<solicitudCredito> GetSolicitudCreditos()
        {
            using (var ctx = new DTO.AccountingContext())
            {
                return ctx.SolicitudCredito.ToList();
            }
        }


        public bool Create(solicitudCredito solicitudCredito)
        {
            using (var ctx = new DTO.AccountingContext())
            {
                try
                {
                    ctx.SolicitudCredito.Add(solicitudCredito);
                    ctx.SaveChanges();
                    return true;
                }
                catch (DbEntityValidationException dbE)
                {
                    return false;
                }
            }
        }


        public List<solicitudCredito> AutorizacionDescuentoLibranza()
        {
            using (var ctx = new DTO.AccountingContext())
            {
                return ctx.SolicitudCredito.ToList();
            }
        }

    }
}
