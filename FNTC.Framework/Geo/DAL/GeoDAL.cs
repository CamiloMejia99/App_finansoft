using AutoMapper;
using FNTC.Framework.BO;
using FNTC.Framework.Geo;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace FNTC.Framework
{
    /// <summary>
    /// Clase GEO
    /// </summary>
    public class GeoDAL : BaseDAL
    {
        public GeoDAL(DbContext ctx)
        {
            this.ctx = ctx;
        }

        public GeoDAL(string stringConnection)
        {
        }

        /// <summary>
        /// Contructor que configura el mapeo
        /// </summary>
        public GeoDAL()
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<DivisionPolitica, GeoBO>());
            mapper = config.CreateMapper();
        }

        public List<GeoBO> Search(string term)
        {
            List<GeoBO> results = new List<GeoBO>();
            using (var ctx = new Geo.GeoModel())
            {
                try
                {
                    ctx.divisionpoliticas
                        .Where(s =>
                            s.MunicipioNombre.Contains(term) ||
                            s.MunicipioId.Contains(term) ||
                            s.DeptoNombre.Contains(term) ||
                            s.DeptoId.Contains(term) ||
                            s.CentroPobladoTipo.Contains(term) ||
                            s.CentroPobladoId.Contains(term) ||
                            s.AreaMetropolitana.Contains(term) ||
                            s.Distrito.Contains(term) ||
                            s.CentroPobladoTipo.Contains(term)

                        )
                        .OrderBy(o => o.Id)
                       .ToList().ForEach(d =>
                           results.Add(mapper.Map<GeoBO>(d)));
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error obteniendo paises, GEODAL.Search() " + e.Message);
                }
                return results;
            }

        }

        /// <summary>
        /// Obtiene la lista de Deptos by Pais Id
        /// </summary>
        /// <param name="paisId">PaisId</param>
        /// <param name="fullInfo">Devueve todos lo campos</param>
        /// <returns>Lista de GeoBO</returns>
        public List<GeoBO> GetDepartamentosByPaisId(string paisId, bool fullInfo = false)
        {
            var _paisId = paisId;
            var deptos = new List<GeoBO>();

            using (var ctx = new Geo.GeoModel())
            {
                try
                {
                    ctx.divisionpoliticas
                       .GroupBy(x => x.DeptoId)
                       .Select(grp => grp.FirstOrDefault())
                       .ToList().ForEach(d =>
                           deptos.Add(mapper.Map<GeoBO>(d)));
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error obteniendo paises, GEODAL.GetDepatamentosbyUd() " + e.Message);
                }
                return deptos;
            }
        }


        public List<GeoBO> GetPaises(string paisId = "169")
        {
            //var deptoId = deptoId;
            var r = new List<GeoBO>();

            using (var ctx = new Geo.GeoModel()/*= new FNTC.Finansoft.Accounting.DAL.Model.AccountingContext()*/)
            {
                try
                {
                    ctx.divisionpoliticas
                       .GroupBy(x => x.PaisId)
                        .Select(g => g.FirstOrDefault())
                        .Where(c => c.PaisId.Equals(paisId))
                        .ToList()
                        .ForEach(x => r.Add(mapper.Map<GeoBO>(x)));

                    // r = mapper.Map<GeoBO>(r1);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error obteniendo paises, GEODAL.GetDepatamentosbyUd() " + e.Message);
                }
                return r;
            }
        }


        public GeoBO GetDepartamentoById(string deptoId)
        {
            //var deptoId = deptoId;
            GeoBO r = new GeoBO();

            using (var ctx = new Geo.GeoModel())
            {
                try
                {
                    var r1 = ctx.divisionpoliticas.Where(c => c.DeptoId.Equals(deptoId)).
                         Select(g => g).FirstOrDefault();

                    r = mapper.Map<GeoBO>(r1);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error obteniendo paises, GEODAL.GetDepatamentosbyUd() " + e.Message);
                }
                return r;
            }
        }

        public List<GeoBO> GetMunicipiosbyDeptoId(string deptoId)
        {
            // var _paisId = paisId;
            var deptos = new List<GeoBO>();

            using (var ctx = new Geo.GeoModel())
            {
                try
                {
                    ctx.divisionpoliticas.
                      GroupBy(grp => grp.MunicipioNombre).Select(g => g.FirstOrDefault()).Where(x => x.DeptoId.Equals(deptoId))
                      .OrderBy(o => o.MunicipioId)
                      .ToList().ForEach(d =>
                          deptos.Add(mapper.Map<GeoBO>(d))); ;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error obteniendo paises, GEODAL.GetDepatamentosbyUd() " + e.Message);
                }
                return deptos;
            }
        }

        public List<GeoBO> GetMunicipioById(int municipioId)
        {
            var deptos = new List<GeoBO>();

            using (var ctx = new Geo.GeoModel())
            {
                try
                {
                    ctx.divisionpoliticas.
                        Where(dp => dp.PaisId.Equals(municipioId)).
                        ToList().ForEach(d =>
                            deptos.Add(mapper.Map<GeoBO>(d)));
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error obteniendo paises, GEODAL.GetDepatamentosbyUd() " + e.Message);
                }
                return deptos;
            }
        }

        [Obsolete("Falta implementar", true)]
        public List<GeoBO> GetCentrosPobladosByMunicipioId(string municipioId)
        {
            //  var _paisId = paisId;
            var deptos = new List<GeoBO>();

            using (var ctx = new Geo.GeoModel())
            {
                try
                {
                    ctx.divisionpoliticas.
                        Where(dp => dp.PaisId.Equals(municipioId)).
                        ToList().ForEach(d =>
                            deptos.Add(mapper.Map<GeoBO>(d)));
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error obteniendo paises, GEODAL.GetDepatamentosbyUd() " + e.Message);
                }
                return deptos;
            }
        }


        public List<GeoBO> GetAllMunicipiosbyPaisId(string paisId = "169")
        {
            using (var ctx = new Geo.GeoModel())
            {

                var municipios = new List<GeoBO>();
                try
                {
                    ctx.divisionpoliticas.
                      GroupBy(grp => grp.MunicipioNombre).Select(g => g.FirstOrDefault()).Where(x => x.PaisId.Equals(paisId))
                      .OrderBy(o => o.MunicipioId)
                      .ToList().ForEach(d =>
                          municipios.Add(mapper.Map<GeoBO>(d))); ;
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error obteniendo paises, GEODAL.GetDepatamentosbyUd() " + e.Message);
                }
                return municipios;
            }
        }

    }
}
