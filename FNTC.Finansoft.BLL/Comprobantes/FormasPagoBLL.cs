using FNTC.Finansoft.Accounting.DTO.Accounting;
using FNTC.Finansoft.Accounting.DTO.Result;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FNTC.Finansoft.Accounting.BLL.Comprobantes
{
    public class FormasPagoBLL
    {

        private DAL.Comprobantes.FormasPagoDAL dal;
        //crud dela forma de pago
        public FormasPagoBLL()
        {
            dal = new DAL.Comprobantes.FormasPagoDAL();
        }

        public Result CreateFormaDePago(FormasDePagoDTO fp)
        {
            //verifico si existe uuna forma de pago con el mismo nombre
            //var existe = dal.GetAllFormaDePago().Where(x => x.Nombre == fp.Nombre || x.CodigoCuenta == fp.CodigoCuenta).Count() > 0 ? true : false;
            var existe = dal.GetAllFormaDePago().Where(x => x.Nombre == fp.Nombre).Count() > 0 ? true : false;
            Result result;
            if (existe)
            {
                result = new Result() { ResultCode = ResultCode.Error, RowsAffected = 0 };
                result.ErrorsWithKey.Add("nombre", "Ya existe una forma de pago con ese nombre");
                return result;
            }
            else
            {
                result = dal.CreateFormaDePago(fp);
                return result;
            }
        }


        public bool UpdateFormaDePago(FormasDePagoDTO fp)
        {
            return dal.UpdateFormaDePago(fp).ResultCode == ResultCode.Updated ? true : false;
        }


        public bool DeleteFormaDePago(FormasDePagoDTO fp)
        {

            throw new NotImplementedException();
        }


        public FormasDePagoDTO GetAllFormaDePago(FormasDePagoDTO fp)
        {
            throw new NotImplementedException();
        }

        public FormasDePagoDTO GetFormaDePago(int id)
        {
            var dto = dal.GetFormaDePagobyId(id);
            return dto;

        }

        public List<FormasDePagoDTO> GetFormaDePagoByClaseComprobante(string claseComprobante)
        {
            var dto = new List<FormasDePagoDTO>();
            switch (claseComprobante)
            {
                case "RC":
                    dto = dal.GetAllFormaDePago().Where(x => x.AplicaParaReciboCaja_Ingresos).ToList();
                    break;
                case "CE":
                    dto = dal.GetAllFormaDePago().Where(x => x.AplicaPara_ComprobanteEgreso_Pagos).ToList();
                    break;
                default:
                    break;
            }

            return dto;
        }


        public List<FormasDePagoDTO> GetAllFormaDePago()
        {
            var dal = new DAL.Comprobantes.FormasPagoDAL();
            return dal.GetAllFormaDePago();

        }
    }
}
