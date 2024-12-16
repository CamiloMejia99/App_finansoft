using FNTC.Finansoft.Accounting.DAL.FormulariosSolicitud;
using FNTC.Finansoft.Accounting.DTO.FormulariosSolicitud;
using System.Collections.Generic;

namespace FNTC.Finansoft.Accounting.BLL.FormulariosSolicitud

{
    public class solicitudCreditoBLL
    {
        public List<solicitudCredito> GetSolicitudCreditos()
        {
            return new solicitudCreditoDAL().GetSolicitudCreditos();
        }

        public List<solicitudCredito> AutorizacionDescuentoLibranza()
        {
            return new solicitudCreditoDAL().AutorizacionDescuentoLibranza();
        }

        public bool Create(solicitudCredito solicitudCredito)
        {
            return new solicitudCreditoDAL().Create(solicitudCredito);
        }
    }
}
