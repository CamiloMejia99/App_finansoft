using AutoMapper;
using FNTC.Finansoft.Accounting.DAL;
using FNTC.Finansoft.Accounting.DAL.Tools;
using FNTC.Finansoft.Accounting.DTO;
using FNTC.Finansoft.Accounting.DTO.Ahorros;
using FNTC.Finansoft.Accounting.DTO.Fichas;
using FNTC.Finansoft.DTO.Ahorros;
using FNTC.Finansoft.DTO.Aportes;
using FNTC.Finansoft.DTO.Respuestas;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FNTC.Finansoft.DAL.Ahorros
{
    public class DALAhorros
    {
        NumberFormatInfo formato = new CultureInfo("es-CO").NumberFormat;

        public List<DTOPersonas> BuscarAsociadosNoAfilados(string busqueda, string tipoFicha)
        {
            //quien no este afiliado en aportes no podra crea un ahorro permanente
            using (var contextoFinansoft = new AccountingContext())
            {
                var personasNoAfiliadas = contextoFinansoft.FichasAportes.Where(f => f.activa == true).Where(fAportes => !contextoFinansoft.FichasAhorros.Where(fAhorros => fAhorros.idPersona == fAportes.idPersona && fAhorros.tipoFicha == tipoFicha && fAportes.activa == true).Any());

                if (personasNoAfiliadas == null) return null;
                //Armar where de ors // nits -> NIT = '1' OR NIT = '100' OR ...
                var where = "";
                foreach (var persona in personasNoAfiliadas)
                {
                    where += "OR NIT='" + persona.idPersona + "' ";
                }
                where = where.Substring(3);
                busqueda = busqueda.ToUpper();
                var AsociadosFiltrados = TercerosDAL.getAllAsociados(busqueda, where);

                //var AsociadosFiltrados = personasNoAfiliadas.Where(p => p.Terceros.NIT.Contains(busqueda)
                //                                            || p.Personas_Fac.Primer_Nom.Contains(busqueda)
                //                                            || p.Personas_Fac.Segundo_Nom.Contains(busqueda)
                //                                            || p.Personas_Fac.Primer_Ape.Contains(busqueda)
                //                                            || p.Personas_Fac.Segundo_Ape.Contains(busqueda)).ToList();
                var listaAsociados = new List<DTOPersonas>();
                foreach (var asociado in AsociadosFiltrados)
                {
                    //var empresa = asociado.Personas_Fac.Asociados_Aso.FirstOrDefault().Empresas_Afiliadas;
                    var dependencia = TercerosDAL.getNomDependenciaTercero(asociado.NIT);

                    var personas = new DTOPersonas()
                    {
                        id = asociado.NIT,
                        nit = asociado.NIT,
                        nombres = asociado.NOMBRE,
                        //empresa = empresa.Empresas_Aso.Nombre,
                        dependencia = dependencia == null ? "No Registra" : dependencia,
                        salario = asociado.SALARIO.ToString()
                    };
                    listaAsociados.Add(personas);
                }
                return listaAsociados;
            }
        }


        /// <summary>
        /// Obtener Configuracion
        /// </summary>
        /// <param name="tipoAhorro">FAP - Ficha Ahorro Permanente, FACDAT - Ficha Ahorro CDAT, FAC - Ficha Ahorro Contractual</param>
        /// <returns></returns>
        public List<Configuracion> ObtenerConfiguracion(string tipoAhorro)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                return contextoFinansoft.Configuracion.Where(c => c.tipoAhorro == tipoAhorro && c.activo == true).ToList();
            }
        }

        public DTORespuesta ActualizarCuotasAhorros(bool porcentajeEquivalente, bool cuotaFija, string porcentaje, string valor)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                var fichasAhorros = contextoFinansoft.FichasAhorros.Where(f => f.tipoFicha == "FAP" && f.activo == true);

                var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<FichasAhorros, FichasAhorros>()
                    .ForMember(x => x.id, opt => opt.Ignore())
                    .ForMember(x => x.Configuracion, opt => opt.Ignore())
                    .ForMember(x => x.TiposCalculo, opt => opt.Ignore())
                    .ForMember(x => x.TiposFichas, opt => opt.Ignore())
                    );
                var mapper = configMapper.CreateMapper();
                var nuevasFichas = mapper.Map<List<FichasAhorros>>(fichasAhorros);

                //Inactivando fichas

                foreach (var ficha in fichasAhorros)
                {
                    ficha.activo = false;
                }

                if (cuotaFija)
                {
                    foreach (var ficha in nuevasFichas)
                    {
                        ficha.valor = valor;
                        var cuota = double.Parse(ficha.valor) * double.Parse(ficha.porcentaje) / 100;
                        ficha.valorCuota = cuota.ToString();
                    }
                }
                else
                {
                    foreach (var ficha in nuevasFichas)
                    {
                        var salarioAsociado = TercerosDAL.getAsociado(ficha.idPersona).SALARIO;//ficha.Personas_Fac.Asociados_Aso.FirstOrDefault().Salario;                        
                        var valorCalculado = salarioAsociado * double.Parse(porcentaje) / 100;
                        ficha.valor = valorCalculado.ToString();
                        var cuota = valorCalculado * double.Parse(ficha.porcentaje) / 100;
                        ficha.valorCuota = cuota.ToString();
                    }
                }

                //agregando nuevas fichas
                foreach (var ficha in nuevasFichas)
                {
                    contextoFinansoft.FichasAhorros.Add(ficha);
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

        public DTORespuesta ActualizarCuotasAhorros(bool cuotaFija, string porcentajeAhorros, string porcentajeSalario, int idConfiguracion)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                var fichasAhorros = contextoFinansoft.FichasAhorros.Where(f => f.tipoFicha == "FAP" && f.activo == true);

                var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<FichasAhorros, FichasAhorros>()
                    .ForMember(x => x.id, opt => opt.Ignore())
                    .ForMember(x => x.Configuracion, opt => opt.Ignore())
                    .ForMember(x => x.TiposCalculo, opt => opt.Ignore())
                    .ForMember(x => x.TiposFichas, opt => opt.Ignore())
                    );
                var mapper = configMapper.CreateMapper();
                var nuevasFichas = mapper.Map<List<FichasAhorros>>(fichasAhorros);

                //Inactivando fichas

                foreach (var ficha in fichasAhorros)
                {
                    ficha.activo = false;
                }

                if (cuotaFija)
                {
                    foreach (var ficha in nuevasFichas)
                    {
                        var valor = ficha.valor;
                        var cuota = double.Parse(valor) * double.Parse(porcentajeAhorros) / 100;
                        ficha.valorCuota = cuota.ToString();
                        ficha.porcentaje = porcentajeAhorros;
                        ficha.idConfiguracion = idConfiguracion;
                    }
                }
                else
                {
                    foreach (var ficha in nuevasFichas)
                    {
                        var salarioAsociado = TercerosDAL.getAsociado(ficha.idPersona).SALARIO;//ficha.Personas_Fac.Asociados_Aso.FirstOrDefault().Salario;
                        var valorCalculado = salarioAsociado * double.Parse(porcentajeSalario) / 100;
                        var cuota = valorCalculado * double.Parse(porcentajeAhorros) / 100;
                        ficha.valor = valorCalculado.ToString();
                        ficha.valorCuota = cuota.ToString();
                        ficha.porcentaje = porcentajeAhorros;
                        ficha.idConfiguracion = idConfiguracion;
                    }
                }

                //agregando nuevas fichas
                foreach (var ficha in nuevasFichas)
                {
                    contextoFinansoft.FichasAhorros.Add(ficha);
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

        public DTORespuesta GuardarConfiguracion(Configuracion configuracion)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                if (configuracion.tipoAhorro == "FAP")
                {
                    var configuraciones = contextoFinansoft.Configuracion.Where(c => c.tipoAhorro == "FAP");
                    foreach (var config in configuraciones)
                    {
                        config.activo = false;
                    }
                }
                contextoFinansoft.Configuracion.Add(configuracion);
                try
                {
                    contextoFinansoft.SaveChanges();
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
        public List<DTOFichasAhorros> ObtenerFichasAhorros(string tipoFicha)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                var consulta = contextoFinansoft.FichasAhorros.Where(f => f.tipoFicha == tipoFicha && f.activo == true);
                var listaFichasAhorros = new List<DTOFichasAhorros>();
                foreach (var ficha in consulta)
                {
                    //var asociado = "";//ficha.Personas_Fac.Asociados_Aso.FirstOrDefault();
                    //var empresaAfiliada = "";// contextoFinansoft.Empresas_Afiliadas.Where(e => e.Id == asociado.Id_EmpresaPaga).FirstOrDefault();
                    //var empresa = "";// contextoFinansoft.Empresas_Aso.Where(e => e.Id == empresaAfiliada.Id_Empresa).FirstOrDefault();
                    //var oficina = "";//contextoFinansoft.Oficinas_Con.Where(o => o.Empresa_Id == empresa.Id).FirstOrDefault();

                    var fichaAhorro = new DTOFichasAhorros()
                    {
                        numeroCuenta = ficha.numeroCuenta,
                        idPersona = ficha.idPersona.ToString(),
                        tipoPago = ficha.tipoPago,
                        porcentaje = ficha.porcentaje,
                        valor = ficha.valor,
                        valorCuota = ficha.valorCuota,
                        totalAhorros = ficha.totalAhorros,
                        tipoCalculoCuota = ficha.tipoCalculoCuota,
                        CDAT = ficha.CDAT,
                        idBeneficiario1 = ficha.idBeneficiario1,
                        idBeneficiario2 = ficha.idBeneficiario2,
                        tipoDevolucion = ficha.tipoDevolucion,
                        valorTitulo = ficha.valorTitulo,
                        plazo = ficha.plazo,
                        tituloPignorado = ficha.tituloPignorado,
                        capitalizaInteres = ficha.capitalizaInteres,
                        idLineaDeposito = ficha.idLineaDeposito,
                        contractual = ficha.contractual,
                        activo = ficha.activo,
                        fechaApertura = ficha.fechaApertura,
                        //añadidos
                        nit = ficha.idPersona,
                        nombres = TercerosDAL.getAsociado(ficha.idPersona).NOMBRE,
                        //empresa = empresa.Nombre,
                        dependencia = TercerosDAL.getNomDependenciaTercero(ficha.idPersona),
                        Tasa_interes = ficha.Tasa_interes,
                        Plazo_meses = ficha.Plazo_meses,
                        Fecha_Vencimiento = ficha.Fecha_Vencimiento
                    };
                    listaFichasAhorros.Add(fichaAhorro);
                }
                return listaFichasAhorros;
            }
        }
        public DTORespuesta CrearFichaAhorro(FichasAhorros fichaAhorro, string modo)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                var configuracion = contextoFinansoft.Configuracion.Where(c => c.activo == true).FirstOrDefault();
                if (configuracion.ConsecutivoActual > configuracion.RangoHasta)
                {
                    var respuesta = new DTORespuesta().GenerarRespuestaBasica(false);
                    respuesta.Mensaje = "Numeracion agotada para afiliacion de fichas ahorros. Por favor actualice su rango de numeracion.";
                    return respuesta;
                }
                configuracion.ConsecutivoActual++;
                contextoFinansoft.Entry(configuracion).State = System.Data.Entity.EntityState.Modified;

                //se inactivan las demás fichas de ahorro si las tiene
                var FichasExistentes = contextoFinansoft.FichasAhorros.Where(x => x.idPersona == fichaAhorro.idPersona).ToList();
                foreach (var item in FichasExistentes)
                {
                    item.activo = false;
                    contextoFinansoft.Entry(item).State = System.Data.Entity.EntityState.Modified;
                }

                if (modo == "registrar")
                {
                    contextoFinansoft.FichasAhorros.Add(fichaAhorro);
                }
                if (modo == "modificar")
                {
                    var anteriorFicha = contextoFinansoft.FichasAhorros.Where(f => f.idPersona == fichaAhorro.idPersona && f.tipoFicha == fichaAhorro.tipoFicha).FirstOrDefault();
                    anteriorFicha.valor = fichaAhorro.valor;
                    anteriorFicha.valorCuota = fichaAhorro.valorCuota;
                    anteriorFicha.porcentaje = fichaAhorro.porcentaje;
                    anteriorFicha.tipoPago = fichaAhorro.tipoPago;
                    anteriorFicha.activo = fichaAhorro.activo;
                    anteriorFicha.Plazo_meses = fichaAhorro.Plazo_meses;
                    anteriorFicha.Tasa_interes = fichaAhorro.Tasa_interes;
                    anteriorFicha.Fecha_Vencimiento = fichaAhorro.Fecha_Vencimiento;

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

        public DTORespuesta GuardarConfiguracionFACDAT(Configuracion configuracionFACDAT, string modo)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                if (modo == "Registrar") contextoFinansoft.Configuracion.Add(configuracionFACDAT);
                if (modo == "Modificar")
                {
                    var anteriorConfig = contextoFinansoft.Configuracion.Where(c => c.id == configuracionFACDAT.id).First();
                    configuracionFACDAT.tipoAhorro = anteriorConfig.tipoAhorro;
                    configuracionFACDAT.fechaRegistro = anteriorConfig.fechaRegistro;
                    contextoFinansoft.Entry(anteriorConfig).CurrentValues.SetValues(configuracionFACDAT);
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

        public DTORespuesta CrearFACDAT(FichasAhorros fichaAhorrosCDAT, string modo)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                if (modo == "Registrar") contextoFinansoft.FichasAhorros.Add(fichaAhorrosCDAT);
                if (modo == "Modificar")
                {
                    contextoFinansoft.Entry(fichaAhorrosCDAT).State = EntityState.Modified;
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

        public Configuracion ObtenerConfiguracion()
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                return contextoFinansoft.Configuracion.Where(c => c.activo == true).FirstOrDefault();
            }
        }
        public string ObtenerNumeroCuentaAhorroPermanente(string prefijo)
        {
            var configuracion = ObtenerConfiguracion();
            string consecutivo = new Util().ObtenerConsecutivoString(configuracion.ConsecutivoActual.ToString());
            return prefijo + consecutivo;
        }


        //seguir investigando sobre automapper para agregar el nombre de la oficina
        public List<DTOFichasAhorros> ObtenerAfiliadosConfiguracionesAhorros(int idConfiguracion)
        {
            using (var contextoFinansoft = new AccountingContext())
            {
                var fichasAhorro = contextoFinansoft.FichasAhorros.Where(f => f.idConfiguracion == idConfiguracion).ToList();

                var configMapper = new MapperConfiguration(cfg => cfg.CreateMap<FichasAhorros, DTOFichasAhorros>()
                  //.ForMember(
                  //dest => dest.nit,
                  //opt => opt.MapFrom(src => src.Personas_Fac.Nit_CC)
                  //)
                  //.ForMember(
                  //dest => dest.nombres, 
                  //opt => opt.MapFrom(src => src.Personas_Fac.Primer_Nom + " " + src.Personas_Fac.Segundo_Nom + " " + src.Personas_Fac.Primer_Ape + " " + src.Personas_Fac.Segundo_Ape)
                  //)
                  //.ForMember(
                  //dest => dest.empresa,
                  //opt => opt.MapFrom(src => src.Personas_Fac.Empresas_Afiliadas.FirstOrDefault().Empresas_Aso.Nombre)
                  //)
                  //.ForMember(
                  //dest => dest.oficina,
                  //opt => opt.MapFrom(src => contextoFinansoft.Oficinas_Con.Where(o => o.Empresa_Id == src.Personas_Fac.Empresas_Afiliadas.FirstOrDefault().Empresas_Aso.Id).FirstOrDefault().Nombre)
                  //)                  
                  );
                var mapper = configMapper.CreateMapper();
                return mapper.Map<List<DTOFichasAhorros>>(fichasAhorro);
            }
        }

        public async Task<List<FichasAhorros>> GetFichasAhorroByNitAsync(string nit)
        {
            var fichas = new List<FichasAhorros>();
            using (var ctx = new AccountingContext())
            {
                fichas = await ctx.FichasAhorros.Where(x => x.idPersona == nit && x.activo == true).Include(x => x.Configuracion).ToListAsync();
            }
            return fichas;
        }


        #region ahorro contractual

        public async Task<bool> GuardarConfAhoCont(ConfigAhorroContractual model) //Guarda una configuración de ahorro contractual
        {
            bool bandera = false;
            try
            {
                using (var ctx = new AccountingContext())
                {
                    ctx.ConfigAhorrosContractuales.Add(model);
                    await ctx.SaveChangesAsync();
                    bandera = true;
                }
            }
            catch (Exception ex)
            {
                return bandera;
            }

            return bandera;
        }

        public async Task<bool> EditarConfAhoCont(ConfigAhorroContractual model)
        {
            bool bandera = false;
            try
            {
                using (var ctx = new AccountingContext())
                {
                    ctx.Entry(model).State = EntityState.Modified;
                    await ctx.SaveChangesAsync();
                    bandera= true;
                }
            }
            catch (Exception ex)
            {

            }
            return bandera;
        }

        public async Task<bool> GuardarFichaAC(FichaAhorroContractual model) //Guarda una ficha de ahorro contractual
        {
            bool bandera = false;
            try
            {
                using (var ctx = new AccountingContext())
                {
                    ctx.FichasAhorroContractual.Add(model);
                    await ctx.SaveChangesAsync();
                    bandera = true;
                }
            }
            catch (Exception ex)
            {
                return bandera;
            }

            return bandera;
        }

        public async Task<bool> EditarFichaAC(FichaAhorroContractual model)
        {
            bool bandera = false;
            try
            {
                using (var ctx = new AccountingContext())
                {
                    ctx.Entry(model).State = EntityState.Modified;
                    await ctx.SaveChangesAsync();
                    bandera = true;
                }
            }
            catch (Exception ex)
            {

            }
            return bandera;
        }

        public async Task<VMConfigAhorroContractual> GetConfigAhoCont(int id)
        {
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            var model = new VMConfigAhorroContractual();
            using (var ctx = new AccountingContext()) {
                try
                {
                    var data = await ctx.ConfigAhorrosContractuales.FindAsync(id);
                    model.Id = data.Id;
                    model.NombreConfiguracion = data.NombreConfiguracion;
                    model.Prefijo = data.Prefijo;
                    model.ValorMinimo = data.ValorMinimo;
                    model.ValorMaximo = data.ValorMaximo;
                    model.IdComprobante = data.IdComprobante;
                    model.IdCuenta = data.IdCuenta;
                    model.SeCausa = data.SeCausa;
                    model.TasaEfectiva = data.TasaEfectivaMinima;
                    model.Morosidad = data.Morosidad;
                    model.Estado = data.Estado;
                    model.FechaRegistro = data.FechaRegistro.ToString("yyyy-MM-dd");
                    model.UserId = data.UserId;
                    model.AuxTasaEfectiva = data.TasaEfectivaMinima.ToString("N2");
                    model.AuxValorMinimo = data.ValorMinimo.ToString("N0", formato);
                    model.AuxValorMaximo = data.ValorMaximo.ToString("N0", formato);
                }
                catch (Exception)
                {

                }
            }
            return model;
        }

        public async Task<List<string>> GetPlazoAndTasa(int id)
        {
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            var list = new List<string>();
            try
            {
                using (var ctx = new AccountingContext())
                {
                    var data = await ctx.ConfigAhorrosContractuales.FindAsync(id);
                    if (data != null)
                    { 
                        list.Add(data.PlazoMinimo.ToString());
                        list.Add(data.TasaEfectivaMinima.ToString());
                        list.Add(data.ValorMinimo.ToString("N0",formato));
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return list;   
        }

        public async Task<JsonResult> VerificaRangoCuotaAC(int idConfig,string valor)
        {
            formato.CurrencyGroupSeparator = ".";
            formato.NumberDecimalSeparator = ",";
            bool bandera = false;
            string encabezado = "", mensaje = "";
            try
            {
                using (var ctx = new AccountingContext())
                {
                    var data = await ctx.ConfigAhorrosContractuales.FindAsync(idConfig);
                    if (data != null)
                    {
                        var nuevoValor = Convert.ToDecimal(valor.Replace(".", ""));
                        if (nuevoValor >= data.ValorMinimo && nuevoValor <= data.ValorMaximo)
                        {
                            bandera = true;
                        }
                        else
                        {
                            string tipoAhorro = data.NombreConfiguracion + " (" + data.Prefijo + ")";
                            encabezado = "TIPO DE AHORRO: \""+tipoAhorro+"\"";
                            mensaje = "RANGO: DESDE $"+ data.ValorMinimo.ToString("N0", formato) + " HASTA $"+ data.ValorMaximo.ToString("N0", formato) + "";
                        }
                    }    
                }
            }
            catch (Exception ex) { }
            

            return new JsonResult { Data = new { status = bandera,encabezado,mensaje } };
        }

        public async Task<JsonResult> VerificaRangoPlazoAC(int idConfig, string valor)
        {
            bool bandera = false;
            string encabezado = "", mensaje = "";
            try
            {
                using (var ctx = new AccountingContext())
                {
                    var data = await ctx.ConfigAhorrosContractuales.FindAsync(idConfig);
                    if (data != null)
                    {
                        var nuevoValor = Convert.ToInt64(valor);
                        if (nuevoValor>=data.PlazoMinimo)
                        {
                            bandera = true;
                        }
                        else
                        {
                            string tipoAhorro = data.NombreConfiguracion + " (" + data.Prefijo + ")";
                            encabezado = "TIPO DE AHORRO: \"" + tipoAhorro + "\"";
                            mensaje = "RANGO MÍNIMO: "+data.PlazoMinimo+" meses";
                        }
                    }
                }
            }
            catch (Exception ex) { }


            return new JsonResult { Data = new { status = bandera, encabezado, mensaje } };
        }

        public async Task<JsonResult> VerificaRangoTasaAC(int idConfig, string valor)
        {
            bool bandera = false;
            string encabezado = "", mensaje = "";
            try
            {
                using (var ctx = new AccountingContext())
                {
                    var data = await ctx.ConfigAhorrosContractuales.FindAsync(idConfig);
                    if (data != null)
                    {
                        var nuevoValor = Convert.ToDecimal(valor.Replace(".",","));
                        if (nuevoValor >= data.TasaEfectivaMinima && nuevoValor <= data.TasaEfectivaMaxima)
                        {
                            bandera = true;
                        }
                        else
                        {
                            string tipoAhorro = data.NombreConfiguracion + " (" + data.Prefijo + ")";
                            encabezado = "TIPO DE AHORRO: \"" + tipoAhorro + "\"";
                            mensaje = "RANGO: DESDE $" + data.TasaEfectivaMinima.ToString("N2", formato) + " HASTA $" + data.TasaEfectivaMaxima.ToString("N2", formato) + "";
                        }
                    }
                }
            }
            catch (Exception ex) { }


            return new JsonResult { Data = new { status = bandera, encabezado, mensaje } };
        }

        public async Task<List<FichaAhorroContractual>> GetFichasAhorroContractualByNitAsync(string nit,string cuenta)
        {
            var fichas = new List<FichaAhorroContractual>();
            using (var ctx = new AccountingContext())
            {
                fichas = await ctx.FichasAhorroContractual.Where(x => x.IdAsociado == nit && x.Estado && x.ConfACFK.IdCuenta == cuenta).Include(x => x.ConfACFK).ToListAsync();
            }
            return fichas;
        }

        #endregion


    }//
}
