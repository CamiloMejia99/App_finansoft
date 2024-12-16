using AutoMapper;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Accounting;
using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace FNTC.Finansoft.Accounting.DAL.Comprobantes
{


    public class ComprobantesDAL
    {
        private AccountingContext ctx;

        public TipoComprobanteDTO GetComprobanteByCODIGO(string CODIGO)
        {
            var tcomprobante = new TipoComprobante();
            using (ctx = new AccountingContext())
            {
                try
                {
                    tcomprobante = ctx.TiposComprobantes.Find(CODIGO);


                    if (CODIGO == "SI1" && tcomprobante == null)
                    {
                        var si = new TipoComprobanteDTO();
                        si.CLASEComprobante = "SI";
                        si.CONSECUTIVO = "0";
                        si.NOMBRE = "SALDOS INICIALES";
                        si.Owner = "CONTABILIDAD";
                        si.CODIGO = CODIGO;
                        new DAL.Comprobantes.ComprobantesDAL().CreateTipoComprobante(si);

                        tcomprobante = ctx.TiposComprobantes.Find(CODIGO);

                    }
                    else
                    {
                        //paila
                    }



                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return MapToDTO(tcomprobante);

        }

        public IEnumerable<TipoComprobanteDTO> GetAllTiposComprobantes()
        {
            List<TipoComprobanteDTO> listaDTO = new List<TipoComprobanteDTO>();
            using (ctx = new AccountingContext())
            {

                try
                {
                    var listTiposComprobantes = ctx.TiposComprobantes.ToList();
                    foreach (var item in listTiposComprobantes)
                    {
                        if (item.FormaPagoId != null)
                        {
                            item.FormaPago = ctx.FormasPago.Find(item.FormaPagoId);
                        }
                        var dto = MapToDTO(item);
                        listaDTO.Add(dto);
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
            }
            return listaDTO;

        }

        public IEnumerable<TipoComprobanteDTO> findComprobantes(string term)
        {
            List<TipoComprobanteDTO> listaDTO = new List<TipoComprobanteDTO>();
            using (ctx = new AccountingContext())
            {
                try
                {
                    var listTiposComprobantes = ctx.TiposComprobantes.Where(c => c.CODIGO.Contains(term) || c.NOMBRE.Contains(term)).ToList();
                    foreach (var item in listTiposComprobantes)
                    {
                        var dto = MapToDTO(item);
                        listaDTO.Add(dto);
                    }
                }
                catch (Exception e)
                {

                    throw;
                }
            }
            return listaDTO;

        }



        public bool CreateTipoComprobante(TipoComprobanteDTO tcDTO)
        {
            using (ctx = new AccountingContext())
            {
                var entity = this.MapToEntity(tcDTO);
                ctx.TiposComprobantes.Add(entity);
                int rows = ctx.SaveChanges();
                return rows > 0 ? true : false;
            }

        }

        public bool UpdateTipoComprobante(TipoComprobanteDTO tcDTO)
        {
            using (ctx = new AccountingContext())
            {
                tcDTO.FormaPago.Tipos = null;
                var entity = this.MapToEntity(tcDTO);

                entity.FormaPago = ctx.FormasPago.Find(entity.FormaPagoId);
                ctx.TiposComprobantes.Attach(entity);
                ctx.Entry(entity).State = System.Data.Entity.EntityState.Modified;
                int rows = ctx.SaveChanges();
                return rows > 0 ? true : false;
            }

        }



        private TipoComprobanteDTO MapToDTO(TipoComprobante tc)
        {
            var configToDTO =
               new MapperConfiguration(cfg =>
               {
                   cfg.CreateMap<FormasPago, FormasDePagoDTO>();
                   cfg.CreateMap<TipoComprobante, TipoComprobanteDTO>()
                     .ForMember(dest => dest.FormaPago, opt => opt.MapFrom(src => src.FormaPago));
               });
            var mapper = configToDTO.CreateMapper();
            var dto = mapper.Map<TipoComprobanteDTO>(tc);
            return dto;

        }

        private TipoComprobante MapToEntity(TipoComprobanteDTO tcDTO)
        {
            var configToEntity =
               new MapperConfiguration(cfg =>
               {
                   cfg.CreateMap<TipoComprobanteDTO, TipoComprobante>()
                       .ForMember(dest => dest.FormaPago, opt => opt.Ignore());
               });
            var mapper = configToEntity.CreateMapper();
            return mapper.Map<TipoComprobante>(tcDTO);
        }

        //nuevo
        public List<Comprobante> GetComprobantes(string tipo, string fechaDesde, string fechaHasta)
        {
            var comprobantes = new List<Comprobante>();
            using (var db = new AccountingContext())
            {
                try
                {
                    int opcion = 0;
                    if (tipo == "" && fechaDesde == "" && fechaHasta == "")
                    {
                        opcion = 4;
                    }
                    else if (tipo != "" && fechaDesde != "" && fechaHasta != "")
                    {
                        opcion = 1;
                    }
                    else if (tipo == "" && fechaDesde != "" && fechaHasta != "")
                    {
                        opcion = 2;
                    }
                    else { opcion = 3; }
                    DateTime Desde = fechaDesde != "" ? Convert.ToDateTime(fechaDesde) : DateTime.Now;
                    DateTime fechHasta = fechaHasta != "" ? Convert.ToDateTime(fechaHasta) : DateTime.Now;
                    DateTime Hasta = new DateTime(fechHasta.Year, fechHasta.Month, fechHasta.Day, 23, 59, 59);

                    comprobantes = db.Database.SqlQuery<Comprobante>(
                                        "dbo.sp_comprobantes @tipo,@fechaDesde,@fechaHasta,@opcion",
                                        new SqlParameter("@tipo", tipo),
                                        new SqlParameter("@fechaDesde", Desde),
                                        new SqlParameter("@fechaHasta", Hasta),
                                        new SqlParameter("@opcion", opcion)
                                        ).ToList();

                }
                catch (Exception ex)
                {
                   
                }
            }


            return comprobantes;
        }
    }
}
