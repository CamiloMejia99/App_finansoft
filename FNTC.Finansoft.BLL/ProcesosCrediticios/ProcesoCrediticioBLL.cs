using FNTC.Finansoft.Accounting.DAL.Creditos;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.Accounting.DTO.ViewModels.Creditos;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FNTC.Finansoft.Accounting.BLL.ProcesosCrediticios
{
    [Authorize]
    public class ProcesoCrediticioBLL
    {
        public bool RealizarCausacion()
        {
            return new ProcesosCrediticiosDAL().RealizarCausacion();
        }


        public ViewModelInfoCredito GetInfoCredito(string NitCajero, string Pagare)
        {
            return new ProcesosCrediticiosDAL().GetInfoCredito(NitCajero, Pagare);
        }

        public JsonResult GetCuotaActual(string Pagare)
        {
            return new ProcesosCrediticiosDAL().GetCuotaActual(Pagare);
        }

        public JsonResult Pago(string Pagare, string Opcion, string ValorRecibido, string UsuarioActual, string FormaPago, string FechaPago, string NumFactura)
        {
            return new ProcesosCrediticiosDAL().Pago(Pagare, Opcion, ValorRecibido, UsuarioActual, FormaPago, FechaPago, NumFactura);
        }

        public JsonResult Abono(string Pagare, string ValorConsignado, string ValorRecibido, string UsuarioActual, string FormaPago, string FechaPago, string NumFactura)
        {
            return new ProcesosCrediticiosDAL().Abono(Pagare, ValorConsignado, ValorRecibido, UsuarioActual, FormaPago, FechaPago, NumFactura);
        }

        public JsonResult VerificaValorAbono(string Pagare, string ValorConsignado)
        {
            return new ProcesosCrediticiosDAL().VerificaValorAbono(Pagare, ValorConsignado);
        }

        public List<TotalesCreditos> ConsultarPagares()
        {
            return new ProcesosCrediticiosDAL().ConsultarPagares();
        }

        public JsonResult GetCuotasCredito(string Pagare)
        {
            return new ProcesosCrediticiosDAL().GetCuotasCredito(Pagare);
        }

        public JsonResult CalcularIM(int Id, int DiasMora)
        {
            return new ProcesosCrediticiosDAL().CalcularIM(Id, DiasMora);
        }

        public JsonResult GuardarValores(int Id, string IC, string IM,string seguro, string admon)
        {
            return new ProcesosCrediticiosDAL().GuardarValores(Id, IC, IM,seguro,admon);
        }
    }
}
