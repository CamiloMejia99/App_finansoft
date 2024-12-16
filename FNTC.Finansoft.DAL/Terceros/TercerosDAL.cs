using AutoMapper;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace FNTC.Finansoft.Accounting.DAL
{
    public class TercerosDAL
    {
        private List<TerceroDTO> _terceros { get; set; }

        public TercerosDAL()
        {
            /*
            if (!DTO.AutoMapperConf.TercerosConf.IsConfigured)
            {
                DTO.AutoMapperConf.TercerosConf.Configure();
            }
            */
        }
        public List<TerceroDTO> Terceros { get { return this.GetAllTerceros(); } }
        public string ObtenerNivel(string nit)
        {
            using (var db = new AccountingContext())
            {
                var nivel = db.Terceros.Where(r => r.NIT == nit).FirstOrDefault().NIVEL;

                if (nivel == "")
                { return "0"; }
                return nivel;

            }
        }//Obtener
        public string ObtenerCargoNombre(int cargoNom)
        {
            using (var db = new AccountingContext())
            {
                var nivel = db.CargoEmpresaTer.Where(r => r.IDCargo == cargoNom).FirstOrDefault().NombreCargo;

                if (nivel == "")
                { return "Seleccionar..."; }
                return nivel;

            }
        }//Obtener
        public int ObtenerCargo(string nit)
        {
            using (var db = new AccountingContext())
            {
                var cargo = db.Terceros.Where(r => r.NIT == nit).FirstOrDefault().CARGO;

                if (cargo == 0)
                { return 0; }
                return cargo;

            }
        }//Obtener
        public string ObtenerNivelNombre(string nivelSelecccionado)
        {
            using (var db = new AccountingContext())
            {
                var NivelSelec = "";
                switch (nivelSelecccionado)
                {
                    case "1":
                        NivelSelec = "Nivel 1";
                        break;
                    case "2":
                        NivelSelec = "Nivel 2";
                        break;
                    case "3":
                        NivelSelec = "Nivel 3";
                        break;
                    case "4":
                        NivelSelec = "Nivel 4";
                        break;
                    case "5":
                        NivelSelec = "Nivel 5";
                        break;
                    case "6":
                        NivelSelec = "Nivel 6";
                        break;
                    default:
                        break;
                }
                return NivelSelec;

            }
        }//Obtener
        public bool UpdateTercero(TerceroDTO tercero)
        {
            //var _tercero = AutoMapper.Mapper.Map<Tercero>(tercero);
            var _tercero = this.toEntity(tercero);
            using (var ctx = new AccountingContext())
            {
                //ctx.Terceros.Find(_tercero.NIT);
                ctx.Entry(_tercero).State = System.Data.Entity.EntityState.Modified;
                /*
                if(tercero.ESASOCIADO != 3)
                {
                    var dependencia = ctx.TerceroDependencia.Where(t => t.Nit_tercero == tercero.NIT && t.Tipo_dep == 2).FirstOrDefault();
                    var flag = false;
                    if (dependencia == null)
                    {
                        dependencia = new TerceroDependencia();
                        flag = true;
                    }

                    dependencia.Cod_dependencia = Convert.ToInt32(tercero.DEPENDENCIA);
                    dependencia.Nit_tercero = tercero.NIT;
                    dependencia.Tipo_dep = 2;

                    if (flag)
                    {
                        ctx.TerceroDependencia.Add(dependencia);
                    }
                }
                */
                //WORKAROUND
                if (_tercero.ESASOCIADO == 3)
                {
                    _tercero.NOMBRE = _tercero.NombreComercial;
                }
                else
                    _tercero.NOMBRE = _tercero.NOMBRE1 + " " + _tercero.APELLIDO1;

                /*try
                {
                    ctx.SaveChanges();
                    return true;
                }
               catch (Exception e)
                {
                    return false;
                }*/
                try
                {
                    return (ctx.SaveChanges()) > 0 ? true : false;
                }
                catch (DbEntityValidationException dbEx)
                {
                    Debug.WriteLine(dbEx.Message);
                    return false;
                }
                /* return () > 0 ? true : false;*/
            }
        }

        public bool CreateTercero(TerceroDTO terceroDTO)
        {
            var configToEntity =
               new MapperConfiguration(cfg =>
               {
                   cfg.CreateMap<TerceroDTO, Tercero>();
                   //cfg.CreateMap<TipoComprobante, TipoComprobanteDTO>()
                   //    .ForMember(dest => dest.FormaPago, opt => opt.MapFrom(src => src.FormaPago));
               });
            var mapper = configToEntity.CreateMapper();
            var entity = mapper.Map<Tercero>(terceroDTO);
            //return dto;

            using (var ctx = new AccountingContext())
            {

                ctx.Entry(entity).State = System.Data.Entity.EntityState.Added;
                /*
                if(terceroDTO.ESASOCIADO != 3)
                {
                    var dependencia = new TerceroDependencia();
                    dependencia.Cod_dependencia = Convert.ToInt32(terceroDTO.DEPENDENCIA);
                    dependencia.Nit_tercero = terceroDTO.NIT;
                    dependencia.Tipo_dep = 2;

                    ctx.TerceroDependencia.Add(dependencia);
                }*/
                //WORKAROUND
                if (entity.ESASOCIADO == 3)
                {
                    entity.NOMBRE = entity.NombreComercial;
                }
                else
                    entity.NOMBRE = entity.NOMBRE1 + " " + entity.APELLIDO1;

                try
                {
                    return (ctx.SaveChanges()) > 0 ? true : false;
                }
                catch (DbEntityValidationException dbEx)
                {

                    Debug.WriteLine(dbEx.Message);
                    return false;
                }
            }
        }

        public void TestPagedTerceros()
        {
            using (var ctx = new AccountingContext())
            {
                //skiprecords = page * rowNumber
                int skipRecords = 10;
                int takeRecords = 5;
                ctx.Terceros.OrderBy(u => u.NIT)
                    .Skip(skipRecords)
                    .Take(takeRecords)
                    .ToList();
            }
        }

        internal static object getAsociado(object p)
        {
            throw new NotImplementedException();
        }

        public List<string> GetTiposTerceros()
        {
            var tiposTerceros = new List<string>();
            var assembly = Assembly.GetAssembly(typeof(Tercero));
            var derivedType = typeof(Tercero);
            //var derived = 
            assembly.GetTypes()
            .Where(t =>
                t != derivedType &&
                derivedType.IsAssignableFrom(t)
                ).ToList().ForEach(tipo => tiposTerceros.Add(tipo.Name));


            return tiposTerceros;

        }

        private List<TerceroDTO> GetAllTerceros()
        {
            using (var ctx = new AccountingContext())
            {
                try
                {
                    if (ctx.Terceros.Count() == 0)
                    {
                        return new List<TerceroDTO>();
                    }
                    var iqueryable = ctx.Terceros as IQueryable;

                    _terceros = new List<TerceroDTO>();
                    //var _terceros = ctx.Terceros.ToList();

                    ctx.Terceros.ToList().ForEach(item =>
                        _terceros.Add(AutoMapper.Mapper.Map<TerceroDTO>(item)));
                }
                catch (Exception e)
                {

                    throw;
                }
            }
            return _terceros;
        }

        //AÑADIDO POR MALDRU
        //OBTIENE LISTA DE ASOCIADOS
        public static List<Tercero> getAllAsociados()
        {
            using (var ctx = new AccountingContext())
            {
                return ctx.Terceros.Where(t => t.ESASOCIADO == 1).ToList();
            }
        }

        //filtra asciados segun nits o nombres y apellidos
        public static List<Tercero> getAllAsociados(string filtro, string nits) // nits -> NIT = '1' OR NIT = '100' OR ...
        {
            using (var ctx = new AccountingContext())
            {
                //buscar todos los asociados que corresponda a los nits encontrados
                var asociados = ctx.Terceros.SqlQuery("SELECT * FROM FERAJUNAP_CONTA.ter.Terceros where " + nits).ToList();
                //filtrar segun nits, nombres o apellidos
                return asociados.Where(a => a.NIT.Contains(filtro) || a.NOMBRE1.Contains(filtro) || a.NOMBRE2.Contains(filtro) || a.APELLIDO1.Contains(filtro) || a.APELLIDO2.Contains(filtro)).ToList();
            }
        }

        //OBTIENE ASOCIADO
        public static Tercero getAsociado(string nit)
        {
            using (var ctx = new AccountingContext())
            {
                var persona = ctx.Terceros.Where(t => t.NIT.Equals(nit)).FirstOrDefault();
                if (persona == null) return null;
                return persona;
            }
        }

        //OBTIENE DEPENDENCIA DEL ASOC
        public static string getNomDependenciaTercero(string nit)
        {
            using (var ctx = new AccountingContext())
            {
                var persona = ctx.Terceros.Where(t => t.NIT.Equals(nit)).FirstOrDefault();
                if (persona == null) return null;
                var idDependecia = persona.DEPENDENCIA;
                var dependencia = ctx.Dependencia.Where(d => d.Id_dep == idDependecia).FirstOrDefault();
                if (dependencia == null) return null;
                return dependencia.Nom_dep;
            }
        }

        //OBTIENE EMPRESA DEL ASOC
        //public static string getEmpresaTercero(string nit)
        //{
        //    using (var ctx = new AccountingContext())
        //    {
        //        //1 encontar a la persona indicada por nit
        //        var persona = ctx.Terceros.Where(t => t.NIT.Equals(nit)).FirstOrDefault();

        //        if (persona == null || persona.DEPENDENCIA == null) return null; //verificar la existencia persona o dependencia

        //        //2 obtener dependencia del asociado o persona
        //        var dependencia = persona.DEPENDENCIA;




        //        //2 buscar la empresa de esa dependencia

        //    }
        //}

        //FIN AÑADIDO

        public List<TerceroDTO> GetTerceros(string term = "")
        {
            using (var ctx = new AccountingContext())
            {
                try
                {
                    if (ctx.Terceros.Count() == 0)
                    {
                        return new List<TerceroDTO>();
                    }
                    //  var iqueryable = ctx.Terceros as IQueryable;

                    _terceros = new List<TerceroDTO>();
                    var result = ctx.Terceros.Where(x => x.NOMBRE.Contains(term) || x.NIT.Contains(term));

                    //result.ForEach(item =>
                    //    _terceros.Add(this.toDTO(item)));
                    foreach (var item in result)
                    {
                        TerceroDTO ter = this.toDTO(item);

                        ter.DEPTODOC = getDepto(item.LUGAREXP);
                        ter.PAISDOC = getPais(Convert.ToInt32(ter.DEPTODOC));

                        if (item.NACIO != null)
                        {
                            ter.DEPTONAC = getDepto((int)item.NACIO);
                            ter.PAISNAC = getPais(Convert.ToInt32(ter.DEPTONAC));
                        }

                        if (item.MUNICIPIO != null)
                        {
                            ter.DEPTO = getDepto((int)item.MUNICIPIO);
                            ter.PAIS = getPais(Convert.ToInt32(ter.DEPTO));
                        }
                        /*
                        if(item.DEPENDENCIA != null)
                        {
                            var dep = Convert.ToInt32(item.DEPENDENCIA);
                            ter.EMPRESA = ctx.TerceroDependencia.Where(t => t.Cod_dependencia == item.DEPENDENCIA && t.Tipo_dep == 1).FirstOrDefault().Nit_tercero;
                        }
                        */
                        _terceros.Add(ter);

                    }

                }
                catch (Exception e)
                {

                    //log exc
                    return null;
                }
            }
            return _terceros;
        }

        public TerceroDTO GetTerceroByNIT(string nit)
        {
            using (var ctx = new AccountingContext())
            {
                var tercero = ctx.Terceros.Find(nit);
                return toDTO(tercero);
            }

        }

        private string getDepto(int muni)
        {
            using (var ctx = new AccountingContext())
            {
                var depto = ctx.Municipio.Where(m => m.Id_muni == muni).FirstOrDefault().Dep_muni.ToString();
                return depto;
            }
        }

        private string getPais(int depto)
        {
            using (var ctx = new AccountingContext())
            {
                var pais = ctx.Departamento.Where(m => m.Id_dep == depto).FirstOrDefault().Pais_dep.ToString();
                return pais;
            }
        }

        private TerceroDTO toDTO(Tercero tercerto)
        {
            var configToDTO =
               new MapperConfiguration(cfg =>
               {
                   cfg.CreateMap<Tercero, TerceroDTO>()
                   .ForMember(s => s._regimen, t => t.Ignore())
                   .ForMember(s => s._sexo, t => t.Ignore())
                   .ForMember(s => s._estadocivil, t => t.Ignore());
                   //cfg.CreateMap<TipoComprobante, TipoComprobanteDTO>()
                   //    .ForMember(dest => dest.FormaPago, opt => opt.MapFrom(src => src.FormaPago));
               });
            var mapper = configToDTO.CreateMapper();
            var dto = mapper.Map<TerceroDTO>(tercerto);
            return dto;
            // return new TerceroDTO();
        }
        /*
        private TerceroDTO loadDto(Tercero tercero)
        {
            using (var ctx = new AccountingContext())
            {
                TerceroDTO dto = new TerceroDTO();

                dto.NIT = tercero.NIT;
                dto.DIGVER = tercero.DIGVER;
                dto.CLASEID = tercero.CLASEID;

            }
        }*/
        private Tercero toEntity(TerceroDTO dto)
        {

            var configToEntity =
               new MapperConfiguration(cfg =>
               {
                   cfg.CreateMap<TerceroDTO, Tercero>();
                   //cfg.CreateMap<TipoComprobante, TipoComprobanteDTO>()
                   //    .ForMember(dest => dest.FormaPago, opt => opt.MapFrom(src => src.FormaPago));
               });
            var mapper = configToEntity.CreateMapper();
            var entity = mapper.Map<Tercero>(dto);
            return entity;
            // return new TerceroDTO();

        }



    }
}
