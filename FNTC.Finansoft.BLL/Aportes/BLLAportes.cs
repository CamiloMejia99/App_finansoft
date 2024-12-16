using AutoMapper;
using FNTC.Finansoft.Accounting.DTO.Aportes;
using FNTC.Finansoft.Accounting.DTO.Empresa;
using FNTC.Finansoft.Accounting.DTO.Fichas;
using FNTC.Finansoft.BLL.Ahorros;
using FNTC.Finansoft.DAL.Ahorros;
using FNTC.Finansoft.DAL.Aportes;
using FNTC.Finansoft.DTO.Ahorros;
using FNTC.Finansoft.DTO.Aportes;
using FNTC.Finansoft.DTO.Respuestas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FNTC.Finansoft.BLL.Aportes
{
    public class BLLAportes
    {
        //mientras entrega oscar
        public EmpresaDTO ObtenerConfiguracionGeneral()
        {
            return new EmpresaDTO() { id = 1, SMLV = "918000" };
        }

        public List<DTOPersonas> BuscarAsociadosNoAfilados(string busqueda)
        {
            return new DALAportes().BuscarAsociadosNoAfilados(busqueda);
        }

        public List<DTOAfiliadosAportes> ObtenerAfiliadosAportes()
        {
            return new DALAportes().ObtenerAfiliadosAportes();
        }

        public List<DTOTiposCalculoCuota> ObtenerTiposCuotasCalculo()
        {
            var tipos = new DALAportes().ObtenerTiposCuotasCalculo();
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<TiposCalculo, DTOTiposCalculoCuota>());
            var mapper = configMapper.CreateMapper();
            return mapper.Map<List<DTOTiposCalculoCuota>>(tipos);
        }
        public DTOConfiguracionAportes ObtenerConfiguracion()
        {
            var configuracion = new DALAportes().ObtenerConfiguracion();
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<Configuracion1, DTOConfiguracionAportes>());
            var mapper = configMapper.CreateMapper();
            return mapper.Map<DTOConfiguracionAportes>(configuracion);
        }
        #region AportesEx
        DALAportes dalConfig = new DALAportes();
        public int IdConfiguracion()
        {
            return dalConfig.IdConfiguracion();
        }
        public string Abreviado()
        {
            return dalConfig.Abreviado();
        }
        public string ConsultaAsociado(string id)
        {
            return dalConfig.ConsultaAsociado(id);
        }
        public string ConsultaAsociadoTercero(string id)
        {
            return dalConfig.ConsultaAsociadoTercero(id);
        }
        public string Asesor(int id)
        {
            return dalConfig.Asesor(id);
        }
        public string NombreAsociado(int id)
        {
            return dalConfig.NombreAsociado(id);
        }
        public DateTime FechaAfiliacion(int id)
        {
            return dalConfig.FechaAfiliacion(id);
        }
        public long TotalAportes(int id)
        {
            return dalConfig.TotalAportes(id);
        }
        public string EliminarMF(int id)
        {
            return dalConfig.EliminarMF(id);
        }
        #endregion

        #region APORTES ORDINARIOS
        public DTORespuesta GuardarConfiguracion(DTOConfiguracionAportes configuracion)
        {
            configuracion.ConsecutivoActual = configuracion.RangoDesde;
            var configuracionAhorros = new BLLAhorros().ObtenerConfiguraciones("FAP").FirstOrDefault();

            if (configuracion.idTipoCuotaCalculo == 1 || configuracion.idTipoCuotaCalculo == 2)
            {
                var porcentajeParaAhorros = configuracionAhorros == null ? 0 : double.Parse(configuracionAhorros.porcentajeParaAhorros);
                var porcentajeAcumulado = double.Parse(configuracion.porcentajeCuota) + porcentajeParaAhorros;
                if (porcentajeAcumulado > 100) return new DTORespuesta() { Correcto = false, Mensaje = "El porcentaje para aportes: " + configuracion.porcentajeCuota + "% y el porcentaje configurado en ahorros : " + porcentajeParaAhorros + "% supera el 100%" };
            }

            double salario = 0, cuota = 0;
            configuracion.activo = true;
            configuracion.fechaRegistro = DateTime.Now;
            configuracion.SaldoMinimo = configuracion.SaldoMinimo.Replace(",", "");

            switch (configuracion.idTipoCuotaCalculo)
            {
                case 1:
                    var configuracionGeneral = ObtenerConfiguracionGeneral();
                    salario = double.Parse(configuracionGeneral.SMLV);
                    configuracion.porcentaje = configuracion.porcentaje.Replace(".", ",");
                    configuracion.porcentajeCuota = configuracion.porcentajeCuota.Replace(".", ",");
                    var valorSalario = salario * double.Parse(configuracion.porcentaje) / 100;
                    configuracion.valor = valorSalario.ToString();
                    cuota = valorSalario * double.Parse(configuracion.porcentajeCuota) / 100;
                    configuracion.valorCuota = cuota.ToString();
                    //new DALAportes().ActualizarCuotasAportes(true, cuota.ToString(),configuracion.porcentaje,configuracion.porcentajeCuota,configuracion.valor);                    
                    //new DALAhorros().ActualizarCuotasAhorrosPermanentes(configuracion.porcentajeCuota,valorSalario.ToString());
                    break;
                //case 2:
                //new DALAportes().ActualizarCuotasAportes(false, null, configuracion.porcentaje,configuracion.porcentajeCuota,null);
                //new DALAhorros().ActualizarCuotasAhorrosPermanentes(configuracion.porcentajeCuota,"SalarioAsociado");
                //  break;
                case 3:
                    configuracion.valor = configuracion.valor.Replace(",", "");
                    configuracion.porcentaje = configuracion.porcentaje.Replace(".", ",");
                    configuracion.porcentajeCuota = configuracion.porcentaje;
                    cuota = double.Parse(configuracion.valor) * double.Parse(configuracion.porcentaje) / 100;
                    configuracion.valorCuota = cuota.ToString();
                    //new DALAportes().ActualizarCuotasAportes(true, cuota.ToString(), configuracion.porcentaje, configuracion.porcentajeCuota, configuracion.valor);
                    break;
                case 4:
                    configuracion.valor = configuracion.valor.Replace(",", "");
                    configuracion.valorCuota = configuracion.valor;
                    //new DALAportes().ActualizarCuotasAportes(true, configuracion.valor, "100",configuracion.porcentajeCuota,configuracion.valor);
                    break;
            }

            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<DTOConfiguracionAportes, Configuracion1>());
            var mapper = configMapper.CreateMapper();
            var dalConfiguracion = mapper.Map<Configuracion1>(configuracion);
            var respuesta = new DALAportes().GuardarConfiguracion(dalConfiguracion);

            if (!respuesta.Correcto) return respuesta;

            //actualizo fichas
            //switch (configuracion.idTipoCuotaCalculo)
            //{
            //    case 1:
            //        new DALAportes().ActualizarCuotasAportes(true, cuota.ToString(), configuracion.porcentaje, configuracion.porcentajeCuota, configuracion.valor, int.Parse(respuesta.Mensaje)); //en mensaje guardo el id de la nueva configuracion XD
            //        respuesta = new DALAhorros().ActualizarCuotasAhorros(false, true, configuracion.porcentaje, configuracion.valor);
            //        break;
            //    case 2:
            //        new DALAportes().ActualizarCuotasAportes(false, null, configuracion.porcentaje, configuracion.porcentajeCuota, null, int.Parse(respuesta.Mensaje));
            //        respuesta = new DALAhorros().ActualizarCuotasAhorros(false, false, configuracion.porcentaje, configuracion.valor);
            //        break;
            //    case 3:
            //        new DALAportes().ActualizarCuotasAportes(true, cuota.ToString(), configuracion.porcentaje, configuracion.porcentajeCuota, configuracion.valor, int.Parse(respuesta.Mensaje));
            //        respuesta = respuesta.GenerarRespuestaBasica(true);
            //        break;
            //    case 4:
            //        new DALAportes().ActualizarCuotasAportes(true, configuracion.valor, "100", configuracion.porcentajeCuota, configuracion.valor, int.Parse(respuesta.Mensaje));
            //        respuesta = respuesta.GenerarRespuestaBasica(true);
            //        break;
            //}

            //guardar exepciones en un log o algo...
            return respuesta;
        }

        public DTORespuesta CrearFichaAporte(DTOFichasAportes fichaAporte)
        {
            var crearFichaAhorro = false;
            var identificacion = fichaAporte.idPersona;
            if (fichaAporte.numeroCuenta == "AfiliarAhorros") crearFichaAhorro = true;
            //fichaAporte.idPersona = int.Parse(new DALAportes().ObtenerIdPersonaPorIdentificacion(fichaAporte.idPersona.ToString()));

            //CuentaAPortes
            fichaAporte.numeroCuenta = new DALAportes().ObtenerNumeroCuentaAporte("FA");
            fichaAporte.FechaApertura = DateTime.Now;

            var configuracion = new DALAportes().ObtenerConfiguracion();
            fichaAporte.IdConfiguracion = configuracion.id;

            double cuota = 0;

            switch (configuracion.idTipoCuotaCalculo)
            {
                case 2:
                    var salarioAsociado = new DALAportes().ObtenerSalarioAsociado(fichaAporte.idPersona);
                    var valorCalculado = double.Parse(salarioAsociado) * double.Parse(configuracion.porcentaje) / 100;
                    cuota = valorCalculado * double.Parse(configuracion.porcentajeCuota) / 100;
                    fichaAporte.porcentaje = configuracion.porcentajeCuota;
                    fichaAporte.valor = valorCalculado.ToString();
                    fichaAporte.valorCuota = cuota.ToString();
                    break;
                case 5:
                    fichaAporte.valor = fichaAporte.valor.Replace(",", "");
                    fichaAporte.porcentaje = fichaAporte.porcentaje.Replace(".", ",");
                    cuota = double.Parse(fichaAporte.valor) * double.Parse(fichaAporte.porcentaje) / 100;
                    fichaAporte.valorCuota = cuota.ToString();
                    break;
                default:
                    fichaAporte.porcentaje = configuracion.porcentajeCuota;
                    fichaAporte.valorCuota = configuracion.valor;
                    fichaAporte.valor = configuracion.valor;
                    break;
            }
            
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<DTOFichasAportes, FichasAportes>());
            var mapper = configMapper.CreateMapper();
            var dalCuenta = mapper.Map<FichasAportes>(fichaAporte);

            var respuesta = new DALAportes().CrearFichaAporte(dalCuenta);
            if (respuesta.Correcto)
            {
                if (crearFichaAhorro)
                {
                    var fichaAhorro = new DTOFichasAhorros()
                    {
                        tipoFicha = "FAP",
                        tipoPago = "Caja",
                        idPersona = identificacion
                    };
                    respuesta = new BLLAhorros().CrearFichaAhorroFAP(fichaAhorro, "registrar");
                    if (!respuesta.Correcto)
                    {
                        respuesta.Correcto = true;  
                        respuesta.Mensaje = "Ficha de aporte creada correctamente. NOTA: "+respuesta.Mensaje;
                    }
                }
            }
            return respuesta;
        }

        public DTORespuesta ActualizarFichaAporte(DTOFichasAportes fichaAportes)
        {
            //fichaAportes.idPersona = int.Parse(new DALAportes().ObtenerIdPersonaPorIdentificacion(fichaAportes.idPersona.ToString()));
            var configuracion = new DALAportes().ObtenerConfiguracion();

            if (configuracion.idTipoCuotaCalculo == 5)
            {
                fichaAportes.valor = fichaAportes.valor.Replace(",", "");
                fichaAportes.porcentaje = fichaAportes.porcentaje.Replace(".", ",");
                var cuota = double.Parse(fichaAportes.valor) * double.Parse(fichaAportes.porcentaje) / 100;
                fichaAportes.valorCuota = cuota.ToString();
            }

            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<DTOFichasAportes, FichasAportes>());
            var mapper = configMapper.CreateMapper();
            var dalfichaAporte = mapper.Map<FichasAportes>(fichaAportes);
            return new DALAportes().ActualizarFichaAporte(dalfichaAporte);
        }

        public List<DTODetallesFichas> ObtenerDetallesFichas(string numeroFicha)
        {
            var detallesFichas = new DALAportes().DetallesFichas(numeroFicha);
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<DTODetallesFichas, DTODetallesFichas>());
            var mapper = configMapper.CreateMapper();
            return mapper.Map<List<DTODetallesFichas>>(detallesFichas);

        }

        public string ObtenerCuotaSobreSalario(string nit) { 
            var cuota = new DALAportes().ObtenerCuotaSobreSalario(nit);
            return cuota;
        }

        public bool VerificaAporte(string IdPersona) {
            var respuesta = new DALAportes().VerificaAporte(IdPersona);
            return respuesta;
        }

        public List<DTOCuentaDistribucionAporte> ObtenerOtrasCuentasAportes()
        {
            return new DALAportes().ObtenerOtrasCuentasAportes();
        }

        public bool VerificaExisteOtrasCuentas(string cuenta)
        {
            var respuesta = new DALAportes().VerificaExisteOtrasCuentas(cuenta);
            return respuesta;
        }

        public DTORespuesta CreateOtrasCuentasAportes(CuentaDistribucionAporte CuentaDistribucion)
        { 
            var respuesta = new DALAportes().CreateOtrasCuentasAportes(CuentaDistribucion);
            return respuesta;   
        }

        public DTORespuesta CalcularPorcentaje(string porcentaje)
        {
            var respueta = new DALAportes().CalcularPorcentaje(porcentaje);
            return respueta;
        }

        public DTORespuesta VeriricarConfiguracion()
        {
            var respuesta = new DALAportes().VerificarConfiguracion();
            return respuesta;  
        }

        public CuentaDistribucionAporte GetCuentaDistribucion(int id)
        { 
            var respuesta = new DALAportes().GetCuentaDistribucion(id);
            return respuesta;
        }

        public DTORespuesta EliminarCuentaDistribucion(int Id)
        {
            var respuesta = new DALAportes().EliminarCuentaDistribucion(Id);
            return respuesta;
        }

        public static async Task<Int64> GetDeudaMoraAporteOrdinarioAsync(FichasAportes ficha)
        {
            return await DALAportes.GetDeudaMoraAporteOrdinarioAsync(ficha);
        }
        #endregion
    }
}
