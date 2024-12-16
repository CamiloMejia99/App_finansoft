using AutoMapper;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Accounting;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.Result;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FNTC.Finansoft.Accounting.DAL.Comprobantes
{
    public class FormasPagoDAL
    {
        AccountingContext ctx;

        public FormasPagoDAL()
        {

        }

        public Result CreateFormaDePago(FormasDePagoDTO fpDTO)
        {
            using (ctx = new AccountingContext())
            {
                //automapper
                var configToDTO =
              new MapperConfiguration(cfg =>
              {
                  cfg.CreateMap<FormasDePagoDTO, FormasPago>();
              });

                var fp = configToDTO.CreateMapper().Map<FormasPago>(fpDTO);
                try
                {
                    ctx.FormasPago.Add(fp);
                    int rows = ctx.SaveChanges();
                    var result = new Result() { ResultCode = ResultCode.Added, RowsAffected = rows };
                    return result;
                }
                catch (Exception ex)
                {
                    var result = new Result() { ResultCode = ResultCode.Error };
                    result.Errors.Add(1, ex.Message);
                    result.Errors.Add(2, ex.InnerException.Message);
                    return result;
                }
            }
        }

        public Result UpdateFormaDePago(FormasDePagoDTO fpDTO)
        {
            using (ctx = new AccountingContext())
            {
                //automapper
                var configToDTO =
              new MapperConfiguration(cfg =>
              {
                  cfg.CreateMap<FormasDePagoDTO, FormasPago>();
              });


                var fp = configToDTO.CreateMapper().Map<FormasPago>(fpDTO);
                try
                {

                    ctx.FormasPago.Attach(fp);
                    ctx.Entry(fp).State = System.Data.Entity.EntityState.Modified;
                    int rows = ctx.SaveChanges();
                    var result = new Result() { ResultCode = ResultCode.Updated, RowsAffected = rows };
                    return result;
                }
                catch (Exception ex)
                {
                    var result = new Result() { ResultCode = ResultCode.Error };
                    result.Errors.Add(1, ex.Message);
                    result.Errors.Add(2, ex.InnerException.Message);
                    return result;
                }

            }
        }

        public bool DeleteFormaDePago(FormasDePagoDTO fp)
        {
            throw new NotImplementedException();
        }

        public FormasDePagoDTO GetFormaDePagobyId(int fpId)
        {
            var fp = new AccountingContext().FormasPago.Find(fpId);
            //automapper 
            //var dtp = this.mapToDto(fp);
            var configToDTO =
           new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<FormasPago, FormasDePagoDTO>();
           });
            var mapper = configToDTO.CreateMapper();
            var dto = mapper.Map<FormasDePagoDTO>(fp);


            return dto;
        }


        public List<FormasDePagoDTO> GetAllFormaDePago()
        {

            List<FormasDePagoDTO> formasDTO = new List<FormasDePagoDTO>();
            using (ctx = new AccountingContext())
            {
                var configToDTO =
             new MapperConfiguration(cfg =>
             {
                 cfg.CreateMap<FormasPago, FormasDePagoDTO>();
             });

                // get all formasDePago
                var formasPago = ctx.FormasPago.ToList();

                var mapper = configToDTO.CreateMapper();
                foreach (var item in formasPago)
                {
                    // automapper
                    formasDTO.Add(mapper.Map<FormasDePagoDTO>(item));
                }
            }

            return formasDTO;


        }

        public List<FormasDePagoDTO> GetFormaDePagoByClaseComprobanteId(string claseComprobante)
        {
            // ,[AplicaParaReciboCaja_Ingresos]
            //,[AplicaPara_ComprobanteEgreso_Pagos]


            List<FormasDePagoDTO> formasDTO = this.GetAllFormaDePago();

            //esto esta muy mal planteado
            if (claseComprobante.Equals("CE"))
            {
                formasDTO = formasDTO.Where(x => x.AplicaPara_ComprobanteEgreso_Pagos == true).ToList();
                return formasDTO;
            }
            if (claseComprobante.Equals("RC"))
            {
                formasDTO = formasDTO.Where(x => x.AplicaParaReciboCaja_Ingresos == true).ToList();
                return formasDTO;
            }

            return new List<FormasDePagoDTO>();

        }
    }
}
