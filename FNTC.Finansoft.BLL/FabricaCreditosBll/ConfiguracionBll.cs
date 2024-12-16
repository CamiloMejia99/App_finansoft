using AutoMapper;
using FNTC.Finansoft.Accounting.DAL.FabricaCreditosDal;
using FNTC.Finansoft.Accounting.DTO.FabricaCreditos;
using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
using System;
using System.Collections.Generic;

namespace FNTC.Finansoft.Accounting.BLL.FabricaCreditosBll
{
    public class ConfiguracionBll
    {
        ConfiguracionDal dalConfig = new ConfiguracionDal();

        public List<agencias> obtenerAgencias()
        {
            var dalAgencias = dalConfig.obtenerAgencias();
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<agencias, agencias>());
            var mapper = configMapper.CreateMapper();
            return mapper.Map<List<agencias>>(dalAgencias);
        }




        public List<FCDependencias> obtenerDependencias()
        {
            var dalDependencias = dalConfig.obtenerDependencias();
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<FCDependencias, FCDependencias>());
            var mapper = configMapper.CreateMapper();
            return mapper.Map<List<FCDependencias>>(dalDependencias);
        }

        public List<FCSedes> obtenerSedes()
        {
            var dalSedes = dalConfig.obtenerSedes();
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<FCSedes, FCSedes>());
            var mapper = configMapper.CreateMapper();
            return mapper.Map<List<FCSedes>>(dalSedes);
        }
        public List<Prestamos> obtenerGruposCreditoPorIdLinea(string id)
        {
            var dalCredito = dalConfig.obtenerGruposCreditoPorIdLinea(id);
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<Prestamos, Prestamos>());
            var mapper = configMapper.CreateMapper();
            return mapper.Map<List<Prestamos>>(dalCredito);
        }

        public List<CentralRiesgo> obtenerCentralesRiesgo()
        {
            var dalCentralesRiesgo = dalConfig.obtenerCentralesRiesgo();
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<CentralRiesgo, CentralRiesgo>());
            var mapper = configMapper.CreateMapper();
            return mapper.Map<List<CentralRiesgo>>(dalCentralesRiesgo);
        }

        public List<FCActividades> obtenerActividades()
        {
            var dalActividades = dalConfig.obtenerActividades();
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<FCActividades, FCActividades>());
            var mapper = configMapper.CreateMapper();
            return mapper.Map<List<FCActividades>>(dalActividades);
        }

        public bool guardarActividades(FCActividades actividad, string modo)
        {
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<FCActividades, FCActividades>());
            var mapper = configMapper.CreateMapper();
            var dalActividad = mapper.Map<FCActividades>(actividad);
            return dalConfig.guardarActividad(dalActividad, modo);
        }

        public bool guardarDocumentosPorActividad(string[] idsDocumentos, int idActividad)
        {
            var listaDocumentos = new List<FCDocumentosActividad>();
            if (idsDocumentos != null)
            {
                foreach (var id in idsDocumentos)
                {
                    var dalDocumentoPorActividad = new FCDocumentosActividad();
                    dalDocumentoPorActividad.idActividad = idActividad;
                    dalDocumentoPorActividad.idDocumento = int.Parse(id);
                    listaDocumentos.Add(dalDocumentoPorActividad);
                }
            }
            else
            {
                listaDocumentos.Add(new FCDocumentosActividad { idActividad = idActividad, idDocumento = 0 });
            }
            return dalConfig.guardarDocumentosPorActividad(listaDocumentos);
        }

        public List<FCDocumentos> obtenerDocumentos()
        {
            var dalDocumentos = dalConfig.obtenerDocumentos();
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<FCDocumentos, FCDocumentos>());
            var mapper = configMapper.CreateMapper();
            return mapper.Map<List<FCDocumentos>>(dalDocumentos);
        }

        public List<FCDocumentosActividad> obtenerDocumentosPorActividad(string idActividad)
        {
            var dalDocumentosPorActividad = dalConfig.obtenerDocumentosPorActividad(idActividad);
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<FCDocumentosActividad, FCDocumentosActividad>());
            var mapper = configMapper.CreateMapper();
            return mapper.Map<List<FCDocumentosActividad>>(dalDocumentosPorActividad);
        }

        /// Configuracion Operario
        public FCConfiguracion obtenerConfiguracionPredeterminada()
        {
            var dalConfiguracion = dalConfig.obtenerConfiguracionPredeterminada();
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<FCConfiguracion, FCConfiguracion>());
            var mapper = configMapper.CreateMapper();
            return mapper.Map<FCConfiguracion>(dalConfiguracion);
        }
        public List<FCMotivosDevolucion> obtenerMotivosDevolucion()
        {
            var dalMotivosDevolucion = dalConfig.obtenerMotivosDevolucion();
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<FCMotivosDevolucion, FCMotivosDevolucion>());
            var mapper = configMapper.CreateMapper();
            return mapper.Map<List<FCMotivosDevolucion>>(dalMotivosDevolucion);
        }

        public string ObtenerNombresAsociado(string ruta)
        {
            return dalConfig.ObtenerNombresAsociado(ruta);
        }
        public string ObtenerNombreAsociadoPrestamos(string id)
        {
            return dalConfig.ObtenerNombreAsociadoPrestamos(id); 
        }
        public string ObtenerTokenAsociado(string id)
        {
            return dalConfig.ObtenerTokenAsociado(id);
        }
        public string ObtenerTokenCodeudor(string id)
        {
            return dalConfig.ObtenerTokenCodeudor(id);
        }
        public string ObtenerNombreCodeudor(string id)
        {
            return dalConfig.ObtenerNombreCodeudor(id); 
        }
        public string ObtenerNombreDelAsociado(int id)
        {
            return dalConfig.ObtenerNombreDelAsociado(id);
        }
        public string ObtenerNombre(string id)
        {
            return dalConfig.ObtenerNombre(id);
        }
        public string ObtenerNombres(string id)
        {
            return dalConfig.ObtenerNombres(id);
        }

        public string ObtenerNoIdetificacion(string id)
        {
            return dalConfig.ObtenerNoIdetificacion(id);
        }
        public string ObtenerNombreAsociadoCR(string id)
        {
            return dalConfig.ObtenerNombreAsociadoCR(id);
        }
        public int ObtenerIdTotalesCre(string id)
        {
            return dalConfig.ObtenerIdTotalesCre(id);
        }
        public string ObtenerPagareCR(int id)
        {
            return dalConfig.ObtenerPagareCR(id);
        }
        public long ObtenerVTDelPrestamo(string id)
        {
            return dalConfig.ObtenerVTDelPrestamo(id);
        }
        public decimal ObtenerPlazoCR(string id)
        {
            return dalConfig.ObtenerPlazoCR(id);
        }
        public int ObtenerIdPrestamoCR(string id)
        {
            return dalConfig.ObtenerIdPrestamoCR(id);
        }
        public decimal ObtenerCapitalTotalCR(string id)
        {
            return dalConfig.ObtenerCapitalTotalCR(id);
        }
        public string ObtenerEstadoCredito(string id)
        {
            return dalConfig.ObtenerEstadoCredito(id);
        }
        public decimal ObtenerSaldoCapitalCR(string id)
        {
            return dalConfig.ObtenerSaldoCapitalCR(id);
        }
        public decimal ObtenerTotalInteCorrie(string id)
        {
            return dalConfig.ObtenerTotalInteCorrie(id);
        }
        public DateTime ObtenerFechaProximoPago(string id)
        {
            return dalConfig.ObtenerFechaProximoPago(id);
        }

        public decimal ObtenerIntereMoraTotal(string id)
        {
            return dalConfig.ObtenerIntereMoraTotal(id);
        }
        public decimal ObtenerIntereMoraPendiente(string id)
        {
            return dalConfig.ObtenerIntereMoraPendiente(id);
        }
        public int ObtenerDiasMora(string id)
        {
            return dalConfig.ObtenerDiasMora(id);
        }
        public string ObtenerEstadoCre(string id)
        {
            return dalConfig.ObtenerEstadoCre(id);
        }

        public string ObtenerEX(string id)
        {
            return dalConfig.ObtenerEX(id);
        }
        //ObtenerEX
        public string ObtenerEXTENCION(string id)
        {
            return dalConfig.ObtenerEXTENCION(id);
        }

        public string ObtenerPermiso(string id)
        {
            return dalConfig.ObtenerPermiso(id);
        }
        public string ObtenerPermisoA(string id)
        {
            return dalConfig.ObtenerPermisoA(id);
        }
        public string ObtenerPermisoE(string id)
        {
            return dalConfig.ObtenerPermisoE(id);
        }
        public string ObtenerPermisoI(string id)
        {
            return dalConfig.ObtenerPermisoI(id);
        }
        public int ObtenerDependencia(string useractual)
        {
            return dalConfig.ObtenerDependencia(useractual);
        }
        public string Permiso(string id)
        {
            return dalConfig.Permiso(id);
        }
        public string ObtenerAutorizacion(int id)
        {
            return dalConfig.ObtenerAutorizacion(id);
        }
        public int ObtenerValorPrestamo(int id)
        {
            return dalConfig.ObtenerValorPrestamo(id);
        }
        public int ObtenerSalario(int id)
        {
            return dalConfig.ObtenerSalario(id);
        }
        public string ObtenerEstado(int id)
        {
            return dalConfig.ObtenerEstado(id);
        }
        public string ObtenerPreEstado(int id)
        {
            return dalConfig.ObtenerPreEstado(id);
        }
        public string ObtenerEstadoRC(int id)
        {
            return dalConfig.ObtenerEstadoRC(id);
        }
        public string ObtenerEstadoDoc(int id)
        {
            return dalConfig.ObtenerEstadoDoc(id);
        }
        public string ObtenerEstados(int id)
        {
            return dalConfig.ObtenerEstados(id);
        }
        public string ObtenerEstadoAnRC(int id)
        {
            return dalConfig.ObtenerEstadoAnRC(id);
        }
        public string ObtenerEstadoAnDoc(int id)
        {
            return dalConfig.ObtenerEstadoAnDoc(id);
        }
        public int ObtenerNoCuotasPrestamo(int id)
        {
            return dalConfig.ObtenerNoCuotasPrestamo(id);
        }
        public int ObtenerMin(int id)
        {
            return dalConfig.ObtenerMin(id);
        }
        public int ObtenerMax(int id)
        {
            return dalConfig.ObtenerMax(id);
        }
        public string ObtenerNombreDependencia(int id)
        {
            return dalConfig.ObtenerNombreDependencia(id);
        }
        //ReferenciasCodeudorObtener
        public FCActividades ReferenciasCodeudorObtener(int id)
        {
            var dalActividad = dalConfig.obtenerActividadPorIdSolicitud(id);
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<FCActividades, FCActividades>());
            var mapper = configMapper.CreateMapper();
            return mapper.Map<FCActividades>(dalActividad);
        }
        public int ObtenerAsociado(int id)
        {
            return dalConfig.ObtenerAsociado(id);
        }
        public int ObtenerSolicitudE(int id)
        {
            return dalConfig.ObtenerSolicitudE(id);
        }
        public string ObtenerNombreAso(int idVerific)
        {
            return dalConfig.ObtenerNombreAso(idVerific);
        }
        public string ObtenerNombreRefe(int IDREFCODE)
        {
            return dalConfig.ObtenerNombreRefe(IDREFCODE);
        }
        public int ObtenerSolicitudAso(int idVerific)
        {
            return dalConfig.ObtenerSolicitudAso(idVerific);
        }
        public int ObtenerIdSoliA(int IDREFCODE)
        {
            return dalConfig.ObtenerIdSoliA(IDREFCODE);
        }
        //ObtenerAsociado
        public int ObteneridAso(int idVerific)
        {
            return dalConfig.ObteneridAso(idVerific);
        }
        //ObtenerAsociado
        public DateTime ObtenerFechaSol(int idVerific)
        {
            return dalConfig.ObtenerFechaSol(idVerific);
        }
        //ObtenerAsociado
        public int ObtenerSolicitud(int id)
        {
            return dalConfig.ObtenerSolicitud(id);
        }
        //ObtenerAsociado
        public string ObtenerNombresMunicipio(string id)
        {
            return dalConfig.ObtenerNombresMunicipio(id);
        }

        public bool TieneSolicitudesPendientes(string id)
        {
            return dalConfig.TieneSolicitudesPendientes(id);
        }
        public List<FCConfiguracion> obtenerConfiguraciones()
        {
            var dalConfiguraciones = dalConfig.obtenerConfiguraciones();
            var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<FCConfiguracion, FCConfiguracion>());
            var mapper = configMapper.CreateMapper();
            return mapper.Map<List<FCConfiguracion>>(dalConfiguraciones);
        }


    }
}
