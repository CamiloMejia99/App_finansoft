using AutoMapper;
using FNTC.Finansoft.Accounting.DAL;
using FNTC.Finansoft.Accounting.DAL.Tools;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Aportes;
using FNTC.Finansoft.Accounting.DTO.Fichas;
using FNTC.Finansoft.DTO.Aportes;
using FNTC.Finansoft.DTO.Respuestas;
using FNTC.Finansoft.UI.Tools;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FNTC.Finansoft.DAL.Aportes
{
    public class DALAportes
    {
        
        //Temporales
        //public List<TemporalCuentasContables> BuscarCuentasContables(string busqueda) {
        //    using (var contextoFinansoft = new ContextoFinansoft()) {
        //        return contextoFinansoft.TemporalCuentasContables.Where(c => c.id.ToString().Contains(busqueda) || c.nombre.Contains(busqueda)).ToList();
        //    }
        //}

        public List<DTOPersonas> BuscarAsociadosNoAfilados(string busqueda)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                var asociados = TercerosDAL.getAllAsociados();

                var personasNoAfiliadas = asociados.Where(a => !contextoFinansoft.FichasAportes.Where(f => f.idPersona.ToString().Equals(a.NIT) && f.activa == true).Any()).ToList();
                //var personasNoAfiliadas = contextoFinansoft.Asociados_Aso.Where(a => !contextoFinansoft.FichasAportes.Where(f => f.idPersona == a.Id_Persona).Any());

                busqueda = busqueda.ToUpper();

                var AsociadosFiltrados = personasNoAfiliadas.Where(p => p.NIT.Contains(busqueda)
                                                            || p.NOMBRE1.Contains(busqueda)
                                                            || p.NOMBRE2.Contains(busqueda)
                                                            || p.APELLIDO1.Contains(busqueda)
                                                            || p.APELLIDO2.Contains(busqueda)).ToList();
                var listaAsociados = new List<DTOPersonas>();

                foreach (var persona in AsociadosFiltrados)
                {
                    //var oficina = contextoFinansoft.Oficinas_Con.Where(o => o.Empresa_Id == persona.Empresas_Afiliadas.Id_Empresa).FirstOrDefault().Nombre;
                    var dependencia = TercerosDAL.getNomDependenciaTercero(persona.NIT);
                    var personas = new DTOPersonas()
                    {
                        id = persona.NIT,
                        nombres = string.Concat(persona.NOMBRE1, " ", persona.NOMBRE2, " ", persona.APELLIDO1, " ", persona.APELLIDO2),
                        dependencia = dependencia == null ? "No Registra" : dependencia,
                        //empresa = persona.Empresas_Afiliadas.Empresas_Aso.Nombre,                        
                        salario = persona.SALARIO.ToString()
                    };
                    listaAsociados.Add(personas);
                }
                return listaAsociados;
            }
        }

        public string ObtenerSalarioAsociado(string id)
        {
            var persona = TercerosDAL.getAsociado(id.ToString());
            if (persona == null) return null;
            return persona.SALARIO.ToString();
            //using (var contextoFinansoft = new ContextoFinansoft())
            //{
            //    var asociado = contextoFinansoft.Asociados_Aso.Where(a => a.Id_Persona == id).FirstOrDefault();
            //    if (asociado != null) return asociado.Salario.ToString();
            //    return null;
            //}
        }

        public List<DTOAfiliadosAportes> ObtenerAfiliadosAportes()
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                var configuracionAportes = contextoFinansoft.Configuracion1.Where(c => c.activo == true).FirstOrDefault();
                if (configuracionAportes == null) return null;

                //var fichasAportes = contextoFinansoft.FichasAportes.Where(f=>f.activa==true).ToList();
                var fichasAportes = contextoFinansoft.FichasAportes.Where(f => f.idConfiguracion == configuracionAportes.id).ToList();
                var listaFichasAportes = new List<DTOAfiliadosAportes>();

                foreach (var ficha in fichasAportes)
                {
                    //var asociado = contextoFinansoft.Asociados_Aso.Where(a => a.Id_Persona == ficha.idPersona).FirstOrDefault();
                    //var empresaAfiliada = contextoFinansoft.Empresas_Afiliadas.Where(e => e.Id == asociado.Id_EmpresaPaga).FirstOrDefault();
                    //var empresaAsociado = contextoFinansoft.Empresas_Aso.Where(e => e.Id == empresaAfiliada.Id_Empresa).FirstOrDefault();
                    //var oficina = contextoFinansoft.Oficinas_Con.Where(o => o.Empresa_Id == empresaAsociado.Id).FirstOrDefault();
                    var nomAsociado = TercerosDAL.getAsociado(ficha.idPersona.ToString());
                    var dependencia = TercerosDAL.getNomDependenciaTercero(ficha.idPersona.ToString());

                    var fichaRegistrada = new DTOAfiliadosAportes()
                    {
                        nit = ficha.idPersona.ToString(),
                        nombres = nomAsociado == null ? "No registra" : nomAsociado.NOMBRE,
                        //empresa = empresaAsociado.Nombre,
                        dependencia = dependencia,
                        //cuentas aporte
                        id = ficha.id,
                        numeroCuenta = ficha.numeroCuenta,
                        idPersona = ficha.idPersona.ToString(),
                        tipoPago = ficha.tipoPago,
                        valorCuota = ficha.valorCuota,
                        porcentaje = ficha.porcentaje,
                        valor = ficha.valor,
                        totalAportes = ficha.totalAportes,
                        fechaApertura = ficha.fechaApertura,
                        activa = ficha.activa,
                        //configuracion Aporte
                        idTipoCuotaCalculo = configuracionAportes.idTipoCuotaCalculo
                    };
                    listaFichasAportes.Add(fichaRegistrada);
                }
                return listaFichasAportes;
            }
        }
        #region AportesEx

        public long TotalAportes(int id)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                var totalAportes = contextoFinansoft.FichaAfiliadosAporteEx.Where(t => t.IdAfiliadosAporteEx == id).FirstOrDefault().totalAportesEx;
                
                return totalAportes;
            }
        }
        public string EliminarMF(int id)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                var IdAfiliado = contextoFinansoft.FichaAfiliadosAporteEx.Where(t => t.IdAfiliadosAporteEx == id).FirstOrDefault().idPersona;
                var mov = contextoFinansoft.Movimientos.Where(a => a.TERCERO == IdAfiliado && a.DETALLE == "CONSIGNACION APORTE EXTRAORDINARIO CAJA").FirstOrDefault();
                var caja = contextoFinansoft.FactOpcaja.Where(a => a.nit_propietario_cuenta == IdAfiliado && a.IdProducto == 2).FirstOrDefault();
                var respuesta = "";
                if(mov == null && caja == null)
                {
                    
                    respuesta = "Eliminar";
                }
                else
                {
                    respuesta = "NoEliminar";
                }
                return respuesta;
            }
        }

        public int IdConfiguracion()
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                var config = 0;
                var Configuracion = contextoFinansoft.Configuracion2Ex.Where(t => t.estado == true).FirstOrDefault();
                if (Configuracion != null)
                {
                    config = Configuracion.IdConfiguracionAportesEx;
                }

                return config;
            }
        }

        public DateTime FechaAfiliacion(int id)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                var FechaAfi = contextoFinansoft.FichaAfiliadosAporteEx.Where(a => a.IdAfiliadosAporteEx == id).FirstOrDefault().FechaApertura;

                return FechaAfi;
            }
        }
        public string Asesor(int id)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                var asesor = contextoFinansoft.FichaAfiliadosAporteEx.Where(a => a.IdAfiliadosAporteEx == id).FirstOrDefault().asesor;

                return asesor;
            }
        }
        public string NombreAsociado(int id)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                var tercero = contextoFinansoft.FichaAfiliadosAporteEx.Where(a => a.IdAfiliadosAporteEx == id).FirstOrDefault().idPersona;
                var persona = contextoFinansoft.Terceros.Where(t => t.NIT == tercero).FirstOrDefault();

                var nombreUsuario = persona.NOMBRE;

                return nombreUsuario;
            }
        }
        public string ConsultaAsociado(string id)
        {
            using (var contextoFinansoft = new AccountingContext())
            {

                var personaEncontrada = "";

                var persona = contextoFinansoft.FichaAfiliadosAporteEx.Where(t => t.idPersona == id).FirstOrDefault();
                if (persona == null)
                {
                    personaEncontrada = "UsuarioNoExistente";
                }
                var Respuesta = personaEncontrada;
                return Respuesta;
            }
        }
        public string ConsultaAsociadoTercero(string id)
        {
            using (var contextoFinansoft = new AccountingContext())
            {

                var personaEncontrada = "";

                var persona = contextoFinansoft.Terceros.Where(t => t.NIT == id).FirstOrDefault();
                if (persona == null)
                {
                    personaEncontrada = "TerceroNoExistente";
                }
                var Respuesta = personaEncontrada;
                return Respuesta;
            }
        }
        public string Abreviado()
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                var config = "";
                var Configuracion = contextoFinansoft.Configuracion2Ex.Where(t => t.estado == true).FirstOrDefault();
                if (Configuracion != null)
                {
                    config = Configuracion.nombreAbreviatura;
                }

                return config;
            }
        }
        public async Task<List<FichaAfiliadosAporteEx>> GetFichasAporteExtraByNitAsync(string nit)
        {
            var fichas = new List<FichaAfiliadosAporteEx>();
            using (var ctx = new AccountingContext())
            {
                fichas = await ctx.FichaAfiliadosAporteEx.Where(x => x.idPersona == nit && x.Estado == true).Include(x => x.Configuracion2ExFK).ToListAsync();
            }
            return fichas;
        }
        #endregion


        #region APORTES ORDINARIOS

        public List<TiposCalculo> ObtenerTiposCuotasCalculo()
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                return contextoFinansoft.TiposCalculo.ToList();
            }
        }

        public Configuracion1 ObtenerConfiguracion()
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                return contextoFinansoft.Configuracion1.Where(c => c.activo == true).FirstOrDefault();
            }
        }

        public DTORespuesta GuardarConfiguracion(Configuracion1 configuracion)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                //Inhabilito configuraciones anteriores
                var configuracionesAnteriores = contextoFinansoft.Configuracion1.Where(c => c.activo == true).ToList();
                foreach (var config in configuracionesAnteriores)
                {
                    config.activo = false;
                }

                contextoFinansoft.Configuracion1.Add(configuracion); //agrego nueva

                try
                {
                    contextoFinansoft.SaveChanges(); //guardo cambios
                    return new DTORespuesta() { Correcto = true, Mensaje = configuracion.id.ToString() };
                }
                catch (Exception e)
                {
                    var respuesta = new DTORespuesta().GenerarRespuestaBasica(false);
                    respuesta.Exepciones = e.Data;
                    return respuesta;
                }
            }
        }

        public DTORespuesta CrearFichaAporte(FichasAportes fichaAporte)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                var configuracion = contextoFinansoft.Configuracion1.Where(c => c.activo == true).FirstOrDefault();
                if (configuracion.ConsecutivoActual > configuracion.RangoHasta)
                {
                    var respuesta = new DTORespuesta().GenerarRespuestaBasica(false);
                    respuesta.Mensaje = "Numeracion agotada para afiliacion de fichas aportes. Por favor actualice su rango de numeracion.";
                    return respuesta;
                }
                configuracion.ConsecutivoActual++;
                contextoFinansoft.Entry(configuracion).State = System.Data.Entity.EntityState.Modified;

                //se inactivan las demás fichas si las tiene
                var FichasExistentes = contextoFinansoft.FichasAportes.Where(x => x.idPersona==fichaAporte.idPersona).ToList();
                foreach (var item in FichasExistentes)
                {
                    item.activa = false;
                    contextoFinansoft.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }

                //creamos la nueva ficha de aporte
                contextoFinansoft.FichasAportes.Add(fichaAporte);
                try
                {
                    contextoFinansoft.SaveChanges();
                    return new DTORespuesta().GenerarRespuestaBasica(true);
                }
                catch (Exception e)
                {
                    var respuesta = new DTORespuesta().GenerarRespuestaBasica(false);
                    respuesta.Exepciones = e.Data;
                    return respuesta;
                }
            }
        }

        public DTORespuesta ActualizarFichaAporte(FichasAportes fichaAporte)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                var fichaAnterior = contextoFinansoft.FichasAportes.Where(c => c.idPersona == fichaAporte.idPersona).FirstOrDefault();
                if (fichaAnterior == null) return null;
                fichaAnterior.tipoPago = fichaAporte.tipoPago;
                fichaAnterior.porcentaje = fichaAporte.porcentaje;
                fichaAnterior.valor = fichaAporte.valor;
                //fichaAnterior.valorCuota = fichaAporte.valorCuota;
                fichaAnterior.activa = fichaAporte.activa;

                try
                {
                    contextoFinansoft.SaveChanges();
                    return new DTORespuesta().GenerarRespuestaBasica(true);
                }
                catch (Exception e)
                {
                    var respuesta = new DTORespuesta().GenerarRespuestaBasica(false);
                    respuesta.Exepciones = e.Data;
                    return respuesta;
                }

            }
        }

        public DTORespuesta ActualizarCuotasAportes(bool cuotaFija, string valorCuota, string porcentaje, string porcentajeCuota, string valor, int idConfiguracion)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                var fichasAportes = contextoFinansoft.FichasAportes.Where(f => f.activa == true).ToList();

                var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<FichasAportes, FichasAportes>()
                    .ForMember(x => x.id, opt => opt.Ignore())
                    .ForMember(x => x.Configuracion1, opt => opt.Ignore())
                    );
                var mapper = configMapper.CreateMapper();
                var nuevasFichas = mapper.Map<List<FichasAportes>>(fichasAportes);

                //Inactivando fichas

                foreach (var ficha in fichasAportes)
                {
                    ficha.activa = false;
                }

                //modificando nuevas fichas con la nueva configuracion

                if (cuotaFija)
                {
                    foreach (var ficha in nuevasFichas)
                    {
                        ficha.valorCuota = valorCuota;
                        ficha.porcentaje = porcentajeCuota;
                        ficha.valor = valor;
                        ficha.idConfiguracion = idConfiguracion;
                    }
                }
                else
                {
                    foreach (var ficha in nuevasFichas)
                    {
                        //var salario = contextoFinansoft.Asociados_Aso.Where(a => a.Id_Persona == ficha.idPersona).FirstOrDefault().Salario.ToString();
                        var salario = TercerosDAL.getAsociado(ficha.idPersona.ToString());
                        if (salario == null)
                        {
                            var respuesta = new DTORespuesta().GenerarRespuestaBasica(false);
                            return respuesta;
                        }

                        var valorsalario = salario.SALARIO * double.Parse(porcentaje) / 100;
                        var cuota = valorsalario * double.Parse(porcentajeCuota) / 100;

                        ficha.valorCuota = cuota.ToString();
                        ficha.porcentaje = porcentajeCuota;
                        ficha.valor = valorsalario.ToString();
                        ficha.idConfiguracion = idConfiguracion;
                    }
                }

                //agregando nuevas fichas
                foreach (var ficha in nuevasFichas)
                {
                    contextoFinansoft.FichasAportes.Add(ficha);
                }

                try
                {
                    contextoFinansoft.SaveChanges();
                    return new DTORespuesta().GenerarRespuestaBasica(true);
                }
                catch (Exception e)
                {
                    var respuesta = new DTORespuesta().GenerarRespuestaBasica(false);
                    respuesta.Exepciones = e.Data;
                    return respuesta;
                }
            }
        }

        public string ObtenerNumeroCuentaAporte(string prefijo)
        {
            var configuracion = ObtenerConfiguracion();
            string consecutivo = new Util().ObtenerConsecutivoString(configuracion.ConsecutivoActual.ToString());
            return prefijo+consecutivo;
        }

        public async Task<List<FichasAportes>> GetFichasAporteByNitAsync(string nit)
        { 
            var fichas = new List<FichasAportes>();
            using (var ctx = new AccountingContext())
            { 
                fichas = await ctx.FichasAportes.Where(x =>x.idPersona == nit && x.activa==true).Include(x =>x.Configuracion1).ToListAsync();
            }
            return fichas;
        }

        public List<DTOCuentaDistribucionAporte> ObtenerOtrasCuentasAportes()
        {
            var list = new List<DTOCuentaDistribucionAporte>();
            try
            {
                using (var ctx = new AccountingContext())
                {
                    var cuentas = ctx.CuentasDistribucionAportes.Where(x => x.Estado).ToList();
                    foreach (var item in cuentas)
                    {
                        var registro = new DTOCuentaDistribucionAporte()
                        {
                            Id = item.Id.ToString(),
                            Cuenta = item.Cuenta,
                            NombreCuenta = item.CuentaFK.NOMBRE,
                            Porcentaje = item.Porcentaje.ToString(),
                            Estado = item.Estado
                        };
                        list.Add(registro);
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }

            return list;
        }
        public string ObtenerCuotaSobreSalario(string nit)
        {
            NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            string salario = "0";
            decimal salarioAuxiliar = 0;
            try
            {
                using (var ctx = new AccountingContext())
                {
                    var data = ctx.Terceros.Where(x => x.NIT == nit).FirstOrDefault();
                    var configuracion = ctx.Configuracion1.Where(x => x.activo == true).FirstOrDefault();
                    
                    if (data != null) {
                        if (data.SALARIO != null)
                        {
                            decimal porcentaje=decimal.Divide(Convert.ToDecimal(configuracion.porcentaje),100);
                            salarioAuxiliar = decimal.Multiply(Convert.ToDecimal(data.SALARIO), porcentaje);
                            porcentaje = decimal.Divide(Convert.ToDecimal(configuracion.porcentajeCuota), 100);
                            salarioAuxiliar = decimal.Multiply(salarioAuxiliar, porcentaje);
                            salario = Convert.ToInt32(Math.Round(salarioAuxiliar, 0, MidpointRounding.ToEven)).ToString();
                        }
                        else { salario = "0"; } 
                    }
                }
            }
            catch (Exception ex)
            {
                salario = "0";
            }
            return salario;
        }

        
        public bool VerificaAporte(string IdPersona)
        {
            bool HayAporte = false;
            try
            {
                using (var ctx = new AccountingContext())
                {
                    HayAporte = ctx.FichasAportes.Where(x => x.idPersona == IdPersona && x.activa == true).Any();
                }
                if (HayAporte) { return true; } else { return false; }
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }

        }

        public bool VerificaExisteOtrasCuentas(string cuenta)
        { 
            bool respuesta = false;
            using (var ctx = new AccountingContext())
            {
                respuesta = ctx.CuentasDistribucionAportes.Where(x => x.Cuenta == cuenta).Any();
            }
            return respuesta;
        }

        public DTORespuesta CreateOtrasCuentasAportes(CuentaDistribucionAporte CuentaDistribucion)
        {
            using (var ctx = new AccountingContext())
            {
                try
                { 
                    CuentaDistribucion.FechaRegistro = Fecha.GetFechaColombia();
                    CuentaDistribucion.Estado = true;
                    CuentaDistribucion.Porcentaje = CuentaDistribucion.AuxPorcentaje != "" && CuentaDistribucion.AuxPorcentaje !=null ? Convert.ToDecimal(CuentaDistribucion.AuxPorcentaje.Replace(".",",")) : 0;
                    ctx.CuentasDistribucionAportes.Add(CuentaDistribucion);
                    ctx.SaveChanges();
                    return new DTORespuesta().GenerarRespuestaBasica(true);
                
                }
                catch (Exception ex)
                {
                    var respuesta = new DTORespuesta().GenerarRespuestaBasica(false);
                    respuesta.Exepciones = ex.Data;
                    return respuesta;
                }
            }
        }

        public CuentaDistribucionAporte GetCuentaDistribucion(int id)
        {
            var modelo = new CuentaDistribucionAporte();
            using (var ctx = new AccountingContext())
            {
                try
                {
                    modelo = ctx.CuentasDistribucionAportes.Find(id);
                }
                catch (Exception ex)
                {


                }

            }
            return modelo;  
        }
        
        public DTORespuesta CalcularPorcentaje(string porcentaje)
        {
            var respuesta = new DTORespuesta().GenerarRespuestaBasica(false);
            using (var ctx = new AccountingContext())
            {
                try
                {
                    var configAportes = ctx.Configuracion1.Where(x => x.activo == true).FirstOrDefault();
                    var configAhorros = ctx.Configuracion.Where(x => x.activo == true).FirstOrDefault();
                    decimal porcentajeAportes = Convert.ToDecimal(configAportes.porcentajeCuota);
                    decimal porcentajeAhorros = Convert.ToDecimal(configAhorros.porcentajeParaAhorros);
                    decimal valorOtrasCuentas = ctx.CuentasDistribucionAportes.Where(x => x.Estado == true).ToList().Select(x => x.Porcentaje).Sum();
                    decimal acumulado =  porcentajeAportes+ porcentajeAhorros+valorOtrasCuentas;
                    decimal valorRestante = 100 - acumulado;
                    decimal valor = Convert.ToDecimal(porcentaje.Replace(".", ","));
                    if (valor <= valorRestante)
                    {
                        return new DTORespuesta().GenerarRespuestaBasica(true);
                    }
                    else { 
                        respuesta.Mensaje= "El valor supera el porcentaje total entre Aportes ("+porcentajeAportes+"%), Ahorros ("+porcentajeAhorros+"%), y Cuentas de distribubción ("+valorOtrasCuentas+"%)";
                    }
                }
                catch (Exception ex)
                {
                    return respuesta;
                }
            }
            return respuesta;
        }

        public DTORespuesta VerificarConfiguracion()
        {
            using (var ctx = new AccountingContext())
            {
                try
                {
                    bool hayConfigAporte = ctx.Configuracion1.Where(x => x.activo == true).Any();
                    bool hayConfigAhorro = ctx.Configuracion.Where(x => x.activo == true).Any();
                    if (hayConfigAporte && hayConfigAhorro) { return new DTORespuesta().GenerarRespuestaBasica(true); }
                }
                catch (Exception ex)
                {
                    var respuesta = new DTORespuesta().GenerarRespuestaBasica(false);
                    respuesta.Exepciones = ex.Data;
                    return respuesta;

                }
            }
            return new DTORespuesta().GenerarRespuestaBasica(false);
        }

        public object DetallesFichas(string numeroFicha)
        {
            throw new NotImplementedException();
        }

        public DTORespuesta EliminarCuentaDistribucion(int Id)
        {
            var respuesta = new DTORespuesta().GenerarRespuestaBasica(false);
            using (var ctx = new AccountingContext())
            {
                try
                {
                    var data = ctx.CuentasDistribucionAportes.Find(Id);
                    ctx.Entry(data).State = System.Data.Entity.EntityState.Deleted;
                    ctx.SaveChanges();
                    respuesta.Correcto = true;
                    respuesta.Mensaje = "Cuenta eliminada correctamente.";
                }
                catch (Exception ex)
                {
                    respuesta.Mensaje = "No se pudo eliminar esta cuenta.";
                    
                }
            }
            return respuesta;
        }

        public static async Task<Int64> GetDeudaMoraAporteOrdinarioAsync(FichasAportes ficha)
        {
            Int64 valor=0;
            try
            {
                using (var _ctx = new AccountingContext())
                {
                    //obtener numero de meses desde que abrio la ficha de aporte hasta la fecha actual
                    int diferenciaMeses = 0;
                    int diferenciaanios = 0;
                    DateTime fechApertura = Convert.ToDateTime(ficha.fechaApertura);
                    DateTime fechActual = Fecha.GetFechaColombia();
                    var dateSpan = Fecha.DateTimeSpan.CompareDates(fechApertura, fechActual);
                    diferenciaMeses = dateSpan.Months;
                    diferenciaanios = dateSpan.Years;
                    if (diferenciaanios > 0)
                    {
                        diferenciaMeses = diferenciaMeses + (diferenciaanios * 12);
                    }
                    //.......
                    //obtener deuda total
                    int num = await _ctx.FactOpcaja.Where(x => x.numero_cuenta == ficha.numeroCuenta).CountAsync();
                    int deudaTotal = 0, n = 0;

                    deudaTotal = Convert.ToInt32(ficha.valor) * ((diferenciaMeses + 1) - num);

                    n = (diferenciaMeses + 1) - num;
                    if (n < 0)
                    {
                        n = 0;
                        deudaTotal = 0;
                    }
                    valor = deudaTotal;
                }
            }
            catch (Exception ex)
            {
            }
            return valor;
        }



        #endregion

    }//
}
