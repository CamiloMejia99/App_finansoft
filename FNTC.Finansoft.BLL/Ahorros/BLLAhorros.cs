using AutoMapper;
using FNTC.Finansoft.Accounting.DTO.Ahorros;
using FNTC.Finansoft.Accounting.DTO.Fichas;
using FNTC.Finansoft.BLL.Aportes;
using FNTC.Finansoft.DAL.Ahorros;
using FNTC.Finansoft.DAL.Aportes;
using FNTC.Finansoft.DTO.Ahorros;
using FNTC.Finansoft.DTO.Aportes;
using FNTC.Finansoft.DTO.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FNTC.Finansoft.BLL.Ahorros
{
    public class BLLAhorros
    {
        //public List<DTOPersonas> BuscarAsociadosPersonasRegistradas(string busqueda)
        //{
        //    return new DALAhorros().BuscarAsociadosPersonasRegistradas(busqueda);
        //}

        public List<DTOPersonas> BuscarAsociadosNoAfilados(string busqueda, string tipoFicha)
        {
            return new DALAhorros().BuscarAsociadosNoAfilados(busqueda, tipoFicha);
        }

        /// <summary>
        /// ObtenerConfiguraciones
        /// </summary>
        /// <param name="tipoAhorro">FAP - Ficha Ahorro Permanente, FACDAT - Ficha Ahorro CDAT, FAC - Ficha Ahorro Contractual</param>
        /// <returns></returns>
        public List<DTOConfiguracionAhorros> ObtenerConfiguraciones(string tipoAhorro)
        {
            var configuraciones = new DALAhorros().ObtenerConfiguracion(tipoAhorro);
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<Configuracion, DTOConfiguracionAhorros>());
            var mapper = configMapper.CreateMapper();
            return mapper.Map<List<DTOConfiguracionAhorros>>(configuraciones);
        }

        #region Ahorro Permanente
        public DTORespuesta GuardarConfiguracion(DTOConfiguracionAhorros configuracion)
        {
            configuracion.ConsecutivoActual = configuracion.RangoDesde;
            var configuracionAportes = new BLLAportes().ObtenerConfiguracion();
            var configuracionAnteriorAhorros = new BLLAhorros().ObtenerConfiguraciones("FAP").FirstOrDefault();
            configuracion.valorMaximo = configuracion.valorMaximo.Replace(",", "");
            configuracion.valorMinimo = configuracion.valorMinimo.Replace(",", "");
            if (configuracionAportes.idTipoCuotaCalculo == 1 || configuracionAportes.idTipoCuotaCalculo == 2)
            {
                var porcentajeParaAportes = configuracionAportes.porcentajeCuota == null ? 0 : double.Parse(configuracionAportes.porcentajeCuota);
                var porcentajeAcumulado = double.Parse(configuracion.porcentajeParaAhorros) + porcentajeParaAportes;
                if (porcentajeAcumulado > 100) return new DTORespuesta() { Correcto = false, Mensaje = "El porcentaje para aportes: " + porcentajeParaAportes + "% y el porcentaje configurado en ahorros : " + configuracion.porcentajeParaAhorros + "% supera el 100%" };
            }



            configuracion.fechaRegistro = DateTime.Now;
            configuracion.activo = true;
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<DTOConfiguracionAhorros, Configuracion>());
            var mapper = configMapper.CreateMapper();
            var dalConfiguracion = mapper.Map<Configuracion>(configuracion);
            var respuesta = new DALAhorros().GuardarConfiguracion(dalConfiguracion);

            if (configuracionAnteriorAhorros != null && respuesta.Correcto)
            {
                if (configuracionAnteriorAhorros.porcentajeParaAhorros != configuracion.porcentajeParaAhorros)
                {
                    if (configuracionAportes.idTipoCuotaCalculo == 2)
                    {
                        respuesta = new DALAhorros().ActualizarCuotasAhorros(false, configuracion.porcentajeParaAhorros, configuracionAportes.porcentaje, int.Parse(respuesta.Mensaje));
                    }
                    else
                    {
                        respuesta = new DALAhorros().ActualizarCuotasAhorros(true, configuracion.porcentajeParaAhorros, configuracionAportes.porcentaje, int.Parse(respuesta.Mensaje));
                    }
                }
                else
                {
                    respuesta = respuesta.GenerarRespuestaBasica(true);
                }
            }
            else
            {
                respuesta = respuesta.GenerarRespuestaBasica(true);
            }
            return respuesta;
        }
        public List<DTOFichasAhorros> ObtenerFichasAhorros(string tipoFicha)
        {
            return new DALAhorros().ObtenerFichasAhorros(tipoFicha);
        }

        public DTORespuesta CrearFichaAhorroFAP(DTOFichasAhorros fichaAhorro, string modo)
        {
            //fichaAhorro.idPersona = new DALAportes().ObtenerIdPersonaPorIdentificacion(fichaAhorro.idPersona.ToString());
            if (modo == "registrar")
            {
                fichaAhorro.numeroCuenta = new DALAhorros().ObtenerNumeroCuentaAhorroPermanente("FAP");
                fichaAhorro.fechaApertura = DateTime.Now;
                fichaAhorro.activo = true;

            }

            var configuracionAportes = new BLLAportes().ObtenerConfiguracion();
            var configuracionAhorros = ObtenerConfiguraciones("FAP").FirstOrDefault();
            fichaAhorro.idConfiguracion = configuracionAhorros.id;
            double cuota = 0;
            //si la configuracion involucra porcenaje a algun valor            

            switch (configuracionAportes.idTipoCuotaCalculo)
            {
                case 1: //% al SMLV
                    fichaAhorro.valor = configuracionAportes.valor;
                    fichaAhorro.porcentaje = configuracionAhorros.porcentajeParaAhorros;
                    cuota = double.Parse(fichaAhorro.valor) * double.Parse(fichaAhorro.porcentaje) / 100;
                    fichaAhorro.valorCuota = cuota.ToString();
                    break;
                case 2: //% al Salario asociado                
                    fichaAhorro.valor = new DALAportes().ObtenerSalarioAsociado(fichaAhorro.idPersona); // de int? a int (utilizo la propiedad value)
                    var valorCalculado = double.Parse(fichaAhorro.valor) * double.Parse(configuracionAportes.porcentaje) / 100;
                    fichaAhorro.valor = valorCalculado.ToString();
                    fichaAhorro.porcentaje = configuracionAhorros.porcentajeParaAhorros;
                    cuota = valorCalculado * double.Parse(fichaAhorro.porcentaje) / 100;
                    fichaAhorro.valorCuota = cuota.ToString();
                    break;
                case 4: // cuota fija
                    fichaAhorro.valorCuota = fichaAhorro.valorCuota.Replace(",", "");
                    fichaAhorro.valor = fichaAhorro.valorCuota;
                    fichaAhorro.porcentaje = null;
                    break;
                case 5: // cuota y porcentaje fijos
                    fichaAhorro.valorCuota = fichaAhorro.valorCuota.Replace(",", "");
                    fichaAhorro.valor = fichaAhorro.valorCuota;
                    fichaAhorro.porcentaje = configuracionAhorros.porcentajeParaAhorros;
                    cuota = double.Parse(fichaAhorro.valor) * double.Parse(fichaAhorro.porcentaje) / 100;
                    fichaAhorro.valorCuota = cuota.ToString();
                    break;
                default: //% sobre un valor fijo                    
                    fichaAhorro.valorCuota = fichaAhorro.valorCuota.Replace(",", "");
                    fichaAhorro.porcentaje = configuracionAhorros.porcentajeParaAhorros;
                    cuota = double.Parse(fichaAhorro.valorCuota) * double.Parse(fichaAhorro.porcentaje) / 100;
                    fichaAhorro.valor = fichaAhorro.valorCuota;
                    fichaAhorro.valorCuota = cuota.ToString();
                    break;
            }

            if (double.Parse(fichaAhorro.valorCuota) < double.Parse(configuracionAhorros.valorMinimo) || double.Parse(fichaAhorro.valorCuota) > double.Parse(configuracionAhorros.valorMaximo)) return new DTORespuesta() { Correcto = false, Mensaje = "El valor de la cuota para ahorros no esta dentro del limite establecido en la configuracion de ahorros: valor minimo: " + configuracionAhorros.valorMinimo + " y valor maximo: " + configuracionAhorros.valorMaximo };

            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<DTOFichasAhorros, FichasAhorros>());
            var mapper = configMapper.CreateMapper();
            var dalFichaAhorro = mapper.Map<FichasAhorros>(fichaAhorro);

            return new DALAhorros().CrearFichaAhorro(dalFichaAhorro, modo);
        }

        #endregion

        public DTORespuesta GuardarConfiguracionFACDAT(DTOConfiguracionAhorros configuracion, string modo)
        {
            if (modo == "Registrar") configuracion.fechaRegistro = DateTime.Now;
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<DTOConfiguracionAhorros, Configuracion>());
            var mapper = configMapper.CreateMapper();
            var dalConfiguracion = mapper.Map<Configuracion>(configuracion);
            return new DALAhorros().GuardarConfiguracionFACDAT(dalConfiguracion, modo);
        }

        public DTORespuesta CrearFACDAT(DTOFichasAhorros fichaAhorrosCDAT, string modo)
        {
            //fichaAhorrosCDAT.idPersona = new DALAportes().ObtenerIdPersonaPorIdentificacion(fichaAhorrosCDAT.idPersona.ToString());
            fichaAhorrosCDAT.numeroCuenta = fichaAhorrosCDAT.tipoFicha + fichaAhorrosCDAT.idPersona;
            if (modo == "Registrar") fichaAhorrosCDAT.fechaApertura = DateTime.Now;
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<DTOFichasAhorros, FichasAhorros>());
            var mapper = configMapper.CreateMapper();
            var dalFichasAhorros = mapper.Map<FichasAhorros>(fichaAhorrosCDAT);
            return new DALAhorros().CrearFACDAT(dalFichasAhorros, modo);
        }

        public List<DTOFichasAhorros> ObtenerAfiliadosConfiguracionesAhorros(int idConfiguracion)
        {
            return new DALAhorros().ObtenerAfiliadosConfiguracionesAhorros(idConfiguracion);
            //var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<FichasAhorros, DTOFichasAhorros>());
            //var mapper = configMapper.CreateMapper();
            //return mapper.Map<List<DTOFichasAhorros>>(fichasAhorros);            
        }


        #region AHORRO CONTRACTUAL
        public async Task<bool> GuardarConfAhoCont(ConfigAhorroContractual model)
        {
            var respuesta = await new DALAhorros().GuardarConfAhoCont(model);
            return respuesta;
        }

        public async Task<bool> EditarConfAhoCont(ConfigAhorroContractual model)
        {
            var respuesta = await new DALAhorros().EditarConfAhoCont(model);
            return respuesta;
        }

        public async Task<bool> GuardarFichaAC(FichaAhorroContractual model)
        {
            var respuesta = await new DALAhorros().GuardarFichaAC(model);
            return respuesta;
        }

        public async Task<bool> EditarFichaAC(FichaAhorroContractual model)
        {
            var respuesta = await new DALAhorros().EditarFichaAC(model);
            return respuesta;
        }

        public async Task<VMConfigAhorroContractual> GetConfigAhoCont(int id) { 
            return await new DALAhorros().GetConfigAhoCont(id);
        }

        public async Task<List<string>> GetPlazoAndTasa(int id)
        {
            return await new DALAhorros().GetPlazoAndTasa(id);
        }

        public async Task<JsonResult> VerificaRangoCuotaAC(int idConfig, string valor)
        {
            return await new DALAhorros().VerificaRangoCuotaAC(idConfig,valor);
        }

        public async Task<JsonResult> VerificaRangoPlazoAC(int idConfig, string valor)
        {
            return await new DALAhorros().VerificaRangoPlazoAC(idConfig, valor);
        }

        public async Task<JsonResult> VerificaRangoTasaAC(int idConfig, string valor)
        {
            return await new DALAhorros().VerificaRangoTasaAC(idConfig, valor);
        }
        #endregion
    }
}
