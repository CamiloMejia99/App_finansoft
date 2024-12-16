namespace FNTC.Finansoft.Accounting.DTO
{
    using FNTC.Finansoft.Accounting.DTO.ActivosFijos;
    using FNTC.Finansoft.Accounting.DTO.Ahorros;
    using FNTC.Finansoft.Accounting.DTO.Aportes;
    using FNTC.Finansoft.Accounting.DTO.Auditoria;
    using FNTC.Finansoft.Accounting.DTO.Compania;
    using FNTC.Finansoft.Accounting.DTO.Contabilidad;
    using FNTC.Finansoft.Accounting.DTO.ControlCartera;
    using FNTC.Finansoft.Accounting.DTO.DeterioroCartera;
    using FNTC.Finansoft.Accounting.DTO.Documentos;
    using FNTC.Finansoft.Accounting.DTO.Email;
    using FNTC.Finansoft.Accounting.DTO.FabricaCreditos;
    using FNTC.Finansoft.Accounting.DTO.Facturacion;
    using FNTC.Finansoft.Accounting.DTO.Fichas;
    using FNTC.Finansoft.Accounting.DTO.FormularioAperturaCtaAhorros;
    using FNTC.Finansoft.Accounting.DTO.FormularioRetiro;
    using FNTC.Finansoft.Accounting.DTO.FormulariosSolicitud;
    using FNTC.Finansoft.Accounting.DTO.FormulariosSolicitudCredito;
    using FNTC.Finansoft.Accounting.DTO.FormularioVinculacion;
    using FNTC.Finansoft.Accounting.DTO.Geo;
    using FNTC.Finansoft.Accounting.DTO.GestionDocumental;
    using FNTC.Finansoft.Accounting.DTO.Informes;
    using FNTC.Finansoft.Accounting.DTO.LiquidacionesDefinitivas;
    using FNTC.Finansoft.Accounting.DTO.Login;
    using FNTC.Finansoft.Accounting.DTO.MCreditos;
    using FNTC.Finansoft.Accounting.DTO.MediosMagneticos;
    using FNTC.Finansoft.Accounting.DTO.Nomina;
    using FNTC.Finansoft.Accounting.DTO.OperativaDeCaja;
    using FNTC.Finansoft.Accounting.DTO.Otros;
    using FNTC.Finansoft.Accounting.DTO.Parametros;
    using FNTC.Finansoft.Accounting.DTO.PruebaEstructuraCapas;
    using FNTC.Finansoft.Accounting.DTO.SARLAFT;
    using FNTC.Finansoft.Accounting.DTO.Scoring;
    using FNTC.Finansoft.Accounting.DTO.SIAR;
    using FNTC.Finansoft.Accounting.DTO.Terceros;
    using FNTC.Finansoft.Accounting.DTO.TercerosOtrasEntidades;
    using FNTC.Finansoft.Accounting.DTO.Tesoreria;
    using FNTC.Finansoft.Accounting.DTO.DescuentosNomina;
    using System.Data.Entity;

    public class AccountingContext : DbContext
    {
        public AccountingContext()
            : base("AccContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //CPalacios habilito cambios en la DB
            Database.SetInitializer<AccountingContext>(null);
            //  Database.SetInitializer(new MyDbContextInitializer());
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public DbSet<TipoProducto> TiposProducto { get; set; }

        #region Contabilidad
        //cuentas
        public virtual DbSet<CuentaMayor> PlanCuentas { get; set; }
        public virtual DbSet<CuentaAuxiliar> Auxiliares { get; set; }
        public virtual DbSet<CuentaImpuestos> CuentasImpuestos { get; set; }

        //movimientos        
        public virtual DbSet<Movimiento> Movimientos { get; set; }
        public virtual DbSet<MovimientosEditados> MovimientosEditadosAC { get; set; }

        //comprobantes
        public virtual DbSet<Comprobante> Comprobantes { get; set; }
        public virtual DbSet<ComprobantesEditados> ComprobantesEditadosAC { get; set; }
        public virtual DbSet<TipoComprobante> TiposComprobantes { get; set; }

        //saldos
        public virtual DbSet<SaldosTercero> SaldosTerceros { get; set; }
        public virtual DbSet<SaldoCuenta> SaldosCuentas { get; set; }
        public virtual DbSet<HSaldosCuentas> HSaldosCuentas { get; set; }
        public virtual DbSet<SaldoCC> SaldosCCs { get; set; }

        //centros de costo
        public virtual DbSet<CentroCosto> CentrosCostos { get; set; }

        public virtual DbSet<FormasPago> FormasPago { get; set; }

        //tablas temporales
        public virtual DbSet<Consecutivos> Consecutivos { get; set; }
        #region Parametros
        //public virtual DbSet<TipoCuentaImpuestos> TiposCuentasImpuestos { get; set; }
        public DbSet<Parameter> Parameters { get; set; }

        #endregion
        //TipoCuentaImpuestos

        #endregion

        #region Terceros
        public DbSet<Tercero> Terceros { get; set; }
        public DbSet<TercerosFallecidos> TercerosFallecidos { get; set; }
        //public DbSet<Administradora> Administradoras { get; set; }
        public DbSet<Profesion> Profesion { get; set; }
        public DbSet<Dependencia> Dependencia { get; set; }
        public DbSet<TerceroDependencia> TerceroDependencia { get; set; }
        public DbSet<InfoTerceroAdicional> InfoTerceroAdicional { get; set; }
        public DbSet<InfoTerceroFinanciera> InfoTerceroFinanciera { get; set; }
        public DbSet<Contrato> Contrato { get; set; }
        public DbSet<NivelEstudio> NivelEstudio { get; set; }
        public DbSet<estrato> estrato { get; set; }
        public DbSet<CargoEmpresaTer> CargoEmpresaTer { get; set; }

         

        #endregion

        #region Otras Entidades
        public DbSet<Pais> Pais { get; set; }
        public DbSet<Departamento> Departamento { get; set; }
        public DbSet<Municipio> Municipio { get; set; }
        #endregion

        #region Aportes y Ahorros 

        public DbSet<FichasAportes> FichasAportes { get; set; }
        public DbSet<CuentaDistribucionAporte> CuentasDistribucionAportes { get; set; }
        public DbSet<FichasAhorros> FichasAhorros { get; set; }

        public DbSet<ConfigAhorroContractual> ConfigAhorrosContractuales { get; set; }
        public DbSet<FichaAhorroContractual> FichasAhorroContractual { get; set; }

        public DbSet<FichaAfiliadosAporteEx> FichaAfiliadosAporteEx { get; set; }
        public DbSet<Configuracion2Ex> Configuracion2Ex { get; set; }
        
        public DbSet<FormulariosApertura> FormulariosApertura { get; set; }




        #endregion
        #region Descuentos De Nomina

        public DbSet<OrdenDePrioridadPagos> OrdenDePrioridadPagos { get; set; }
        public DbSet<TipoPagos> TipoPagos { get; set; }
        public DbSet<ControlDeMovimientos> ControlDeMovimientos { get; set; }
        public DbSet<ContraPartida> ContraPartida { get; set; }
        public DbSet<SaldosSobrantes> SaldosSobrantes { get; set; }
        public DbSet<EstructuraPlanos> EstructuraPlanos { get; set; }
        public DbSet<ConformacionDeLosPlanos> ConformacionDeLosPlanos { get; set; }
        public DbSet<RelacionPlanosEmpresa> RelacionPlanosEmpresa { get; set; }
        public DbSet<RelacionPlanosDiscriminacion> RelacionPlanosDiscriminacion { get; set; }
        public DbSet<DatosDiscriminacionPlanos> DatosDiscriminacionPlanos { get; set; }
        public DbSet<CamposRelacionDis> CamposRelacionDis { get; set; }
        
        
         
        #endregion

        public DbSet<des_nomina> Des_nomina { get; set; }
        public DbSet<JerarquiaDescuento> JerarquiaDescuento { get; set; }
        
        public DbSet<ClaseDePlano> ClaseDePlano { get; set; }
        public DbSet<MovimientosTipoEstado> MovimientosTipoEstado { get; set; }

        public DbSet<ArchivoPlano> ArchivoPlano { get; set; }
        public DbSet<TipoDeCampo> TipoDeCampo { get; set; }
        public DbSet<SeleccionCuenta> SeleccionCuenta { get; set; }
        public DbSet<PlanoEmpresa> PlanoEmpresa { get; set; }
        public DbSet<Discriminacion> Discriminacion { get; set; }
        public DbSet<CorreccionNomina> CorreccionNomina { get; set; }
        public DbSet<EnvioPlano> EnvioPlano { get; set; }
        public DbSet<ComparacionArchivo> ComparacionArchivo { get; set; }
        public DbSet<DescuentoNomina> DescuentoNomina { get; set; }
        public virtual DbSet<ConsolidadoNomina> ConsolidadoNomina { get; set; }

        #region Creditos

        public DbSet<Clase_Garantias> Clase_Garantias { get; set; }
        public DbSet<Lineas> Lineas { get; set; }
        public DbSet<Destinos> Destinos { get; set; }
        public DbSet<Costos_Adicionales> Costos_Adicionales { get; set; }
        public DbSet<CostosPrestamos> CostosPrestamos { get; set; }
        public DbSet<SubDestinos> SubDestinos { get; set; }
        public DbSet<Cuentas> Cuentas { get; set; }
        public DbSet<Forma_Pago> Forma_Pago { get; set; }
        public DbSet<Garantias> Garantias { get; set; }
        public DbSet<Incrementa> Incrementa { get; set; }
        public DbSet<Personal> Personal { get; set; }
        public DbSet<Prestamos> Prestamos { get; set; }
        public DbSet<Real> Real { get; set; }
        public DbSet<Tipo_Costo> Tipo_Costo { get; set; }
        public DbSet<Tipo_Periodo> Tipo_Periodo { get; set; }
        public DbSet<BCreditos> Creditos { get; set; }
        public DbSet<Codigo_Operador> Codigo_Operador { get; set; }
        public DbSet<CConsecutivos> CConsecutivos { get; set; }
        public DbSet<GarantiasCreditos> GarantiasCreditos { get; set; }
        public DbSet<ProcesosAutomaticos> ProcesosAutomaticos { get; set; }
        public DbSet<Amortizaciones> Amortizaciones { get; set; }
        public DbSet<TotalesCreditos> TotalesCreditos { get; set; }
        public DbSet<ControlCreditos> ControlCreditos { get; set; }
        public DbSet<ValidarHuella> ValidarHuella { get; set; }
        public DbSet<UserH> UserH { get; set; } 
        //public DbSet<TercerosPRB> TercerosPRB { get; set; }
        //public DbSet<ViewModelPrestaTerceros> PrestaTerceros { get; set; }
        //public DbSet<Tercero> TraerTerceros { get; set; }

        #endregion

        public DbSet<BFormularioRetiro> BFormularioRetiro { get; set; }
        public DbSet<compania> Compania { get; set; }
        public DbSet<ViewModelReporteSuper> ReporteS { get; set; }
        public DbSet<ControlAcceso> ControlAccesos { get; set; }

      

        #region Activos Fijos

        public DbSet<BActivosFijos> ActivosFijos { get; set; }
        public DbSet<HistorialActivosFijos> HistorialActivosFijos { get; set; }
        public DbSet<ClaseDeActivo> ClaseDeActivo { get; set; }
        public DbSet<GruposActivosFijos> GruposActivosFijos { get; set; }
        public DbSet<UbicacionFisica> UbicacionFisica { get; set; }

        #endregion Activos Fijos

        #region Gestion Documental

        public DbSet<GDClass1> mimodelo { get; set; }

        #endregion Gestion Documental

        //public DbSet<ConfiguracionAportes> configuracionAportes { get; set; }
        //public DbSet<TiposCalculoAportes> tiposCalculoAportes { get; set; }        

        public virtual DbSet<Configuracion> Configuracion { get; set; }
        public virtual DbSet<Configuracion1> Configuracion1 { get; set; }
        public virtual DbSet<TiposCalculo> TiposCalculo { get; set; }
        public virtual DbSet<TiposFichas> TiposFichas { get; set; }

        public DbSet<formatoVinculacion> formatoVinculacions { get; set; }

        public DbSet<FormularioSolicitudAfiliacion> FormularioSolicitudAfiliacion { get; set; }
        public DbSet<agencias> agencias { get; set; }

        public DbSet<PruebaEstructura> PruebaEstructuras { get; set; }

        #region Operativa De Caja

        public DbSet<Caja> Caja { get; set; }
        public DbSet<configCajero> configCajero { get; set; }
        public DbSet<CuadreCajaPorCajero> CuadreCajaPorCajero { get; set; }
        public DbSet<FactOpcaja> FactOpcaja { get; set; }
        public DbSet<CodigosBanco> CodigosBanco { get; set; }
        public DbSet<RegistroAbastecimiento> RegistroAbastecimientos { get; set; }
        public DbSet<ClaseComprobante> ClaseComprobante { get; set; }
        public DbSet<convenio> convenio { get; set; }


        #endregion Operativa De Caja

        #region Tesoreria

        public DbSet<TsorBanco> TsorBancos { get; set; }
        public DbSet<TsorMatricularBanco> TsorMatricularBancos { get; set; }
        public DbSet<TsorConsecutivosChequera> TsorConsecutivosChequeras { get; set; }
        public DbSet<TsorCheque> TsorCheques { get; set; }

        #endregion Tesoreria

        #region Scoring
        public DbSet<ScoringCalculo> ScoringCalculos { get; set; }
        public DbSet<ScoringModeloRiesgo> ScoringModeloRiesgos { get; set; }
        public DbSet<ScoringScoringRealizado> ScoringScoringRealizados { get; set; }
        public DbSet<ScoringTipoCarteraComercial> ScoringTipoCarteraComerciales { get; set; }
        public DbSet<ScoringTipoCarteraConsumo> ScoringTipoCarteraConsumos { get; set; }
        public DbSet<ScoringTipoCarteraMicrocredito> ScoringTipoCarteraMicrocreditos { get; set; }
        public DbSet<ScoringTipoCarteraVivienda> ScoringTipoCarteraViviendas { get; set; }
        public DbSet<ScoringVariableAgencia> ScoringVariableAgencias { get; set; }
        public DbSet<ScoringVariableAntiguedadCooperativa> ScoringVariableAntiguedadCooperativas { get; set; }
        public DbSet<ScoringVariableAntiguedadLaboral> ScoringVariableAntiguedadLaborales { get; set; }
        public DbSet<ScoringVariableCapacidadPago> ScoringVariableCapacidadPagos { get; set; }
        public DbSet<ScoringVariableCategoria> ScoringVariableCategorias { get; set; }
        public DbSet<ScoringVariableEdad> ScoringVariableEdades { get; set; }
        public DbSet<ScoringVariableEstadosCivil> ScoringVariableEstadosCiviles { get; set; }
        public DbSet<ScoringVariableEstrato> ScoringVariableEstratos { get; set; }
        public DbSet<ScoringVariableFormaPago> ScoringVariableFormaPagos { get; set; }
        public DbSet<ScoringVariableGarantia> ScoringVariableGarantias { get; set; }
        public DbSet<ScoringVariableIngresosTotal> ScoringVariableIngresosTotales { get; set; }
        public DbSet<ScoringVariableMesesPlazo> ScoringVariableMesesPlazos { get; set; }
        public DbSet<ScoringVariableMonto> ScoringVariableMontos { get; set; }
        public DbSet<ScoringVariableNivelesEducativo> ScoringVariableNivelesEducativos { get; set; }
        public DbSet<ScoringVariableOcupacion> ScoringVariableOcupaciones { get; set; }
        public DbSet<ScoringVariablePersonasACargo> ScoringVariablePersonasACargos { get; set; }
        public DbSet<ScoringVariableReestructurado> ScoringVariableReestructurados { get; set; }
        public DbSet<ScoringVariableSexo> ScoringVariableSexos { get; set; }
        public DbSet<ScoringVariableTipoContrato> ScoringVariableTipoContratos { get; set; }
        public DbSet<ScoringVariableTipoVivienda> ScoringVariableTipoViviendas { get; set; }

        #endregion Scoring

        #region Deterioro Cartera

        public DbSet<DeterioroPar> DeterioroPars { get; set; }
        public DbSet<Deterioro> Deterioros { get; set; }
        public DbSet<CuentaDeterioroCartera> CuentaDeterioroCartera { get; set; }

        #endregion Deterioro Cartera

        public DbSet<solicitudCredito> SolicitudCredito { get; set; }
        public DbSet<factOpCajaDesembolsos> factOpCajaDesembolso { get; set; }
        public DbSet<factOpCajaConsCuotaCredito> factOpCajaConsCuotaCredito { get; set; }
        public DbSet<factOpCajaConsCuotaCreditoEntidadDos> factOpCajaConsCuotaCreditoEntidadDos { get; set; }

        public DbSet<tercerosEntidadDos> tercerosEntidadDos { get; set; }

        public DbSet<LiquidacionDefinitivaAso> LiquidacionDefinitiva { get; set; }
        public DbSet<interescausadoprestamos> interescausadoprestamos { get; set; }
        public DbSet<procesos> procesos { get; set; }
        public DbSet<HistorialCreditos> HistorialCreditos { get; set; }

        public DbSet<mifecha> mifechas { get; set; }






        #region Fabrica De Creditos
        public DbSet<FCDependencias> FCDependencias { get; set; }
        public DbSet<FCConfiguracion> FCConfiguracion { get; set; }
        public DbSet<FCSedes> FCSedes { get; set; }
        public DbSet<FCRolesUsuario> FCRolesUsuario { get; set; }
        public DbSet<CentralRiesgo> CentralRiesgo { get; set; }
        public DbSet<FCActividades> FCActividades { get; set; }
        public DbSet<FCMotivosDevolucion> FCMotivosDevolucion { get; set; }
        public DbSet<FCDocumentos> FCDocumentos { get; set; }
        public DbSet<FCDocumentosActividad> FCDocumentosActividad { get; set; }
        public DbSet<FCDocumentosAsociados> FCDocumentosAsociados { get; set; }
        public DbSet<FCSolicitudes> FCSolicitudes { get; set; }
        public DbSet<FCTareas> FCTareas { get; set; }
        public DbSet<FCActividadesFinancieras> FCActividadesFinancieras { get; set; }
        public DbSet<DataReferenciasCodeudorFC> DataReferenciasCodeudorFC { get; set; }
        public DbSet<FCReferenciasSolicitud> FCReferenciasSolicitud { get; set; }
        public DbSet<FCPasosAp> FCPasosAp { get; set; }
        public DbSet<ControlAccesoFC> ControlAccesoFC { get; set; }
        public DbSet<VerificacionAsociado> VerificacionAsociado { get; set; }
        public DbSet<FormulariosSolicitudCred> FormulariosSolicitudCred { get; set; }
   

       
       

        #endregion
        #region Control Cartera
        public DbSet<CRClasesDeGestion> CRClasesDeGestion { get; set; }
        public DbSet<CRConveniosTipoDeConvenios> CRConveniosTipoDeConvenios { get; set; }
        public DbSet<CRGestionContacto> CRGestionContacto { get; set; }
        public DbSet<CRGestionRespuesta> CRGestionRespuesta { get; set; }
        public DbSet<CRNotificacionesCartera> CRNotificacionesCartera { get; set; }
        public DbSet<CRControlTransferenciaCartera> CRControlTransferenciaCartera { get; set; }

         
        #endregion

        #region Facturación
        public DbSet<producto> producto { get; set; }
        public DbSet<factura> factura { get; set; }
        public DbSet<configuracionFact> configuracionFact { get; set; }
        public DbSet<iva> iva { get; set; }
        public DbSet<FacFormasDePago> FacFormasDePago { get; set; }
        public virtual DbSet<consecutivosFacturas> consecutivosFacturas { get; set; }
        public virtual DbSet<operation> operation { get; set; }
        public virtual DbSet<FacConfiguracion> FacConfiguracion { get; set; }

        #endregion

       

        public DbSet<TipoEmpresa> TipoEmpresa { get; set; }
        public DbSet<Empresaa> Empresa { get; set; }

        public DbSet<EmailParameter> EmailParameter { get; set; }

        public DbSet<ConfiguracionCorreo> ConfiguracionCorreo { get; set; }

        public DbSet<AspNetUsers> AspNetUsersApp { get; set; }

        public object AspNetUsers { get; set; }


        #region modulo SIAR
        public DbSet<SIARsubclasificacion> SIARsubclasificacion { get; set; }
        public DbSet<SIARcalificacion> SIARcalificacion { get; set; }
        public DbSet<SIARparametros> SIARparametros { get; set; }
        public DbSet<SIARsubclasificacionCartera> SIARsubclasificacionCartera { get; set; }
        #endregion

        #region SARLAFT
        public DbSet<ContextoEmpresarial> ContextoEmpresarial { get; set; }
        public DbSet<Macroprocesos> Macroprocesos { get; set; }
        public DbSet<CargosResponsables> CargosResponsables { get; set; }
        public DbSet<Procesos> speProceso { get; set; }
        #endregion

        #region Documentos
        public DbSet<ConfigDocumentoSoporte> ConfigDocumentoSoporte { get; set; }
        #endregion


        #region procedimientos almacenados
        public DbSet<spBalanceComprobacionL5> spBalanceComprobacionL5 { get; set; }
        public object Tercero { get; set; }
        public object Asociados { get; set; }
        #endregion

        #region MediosMagneticos
        public DbSet<ConfigMedMag> ConfigMedMag { get; set; }
        public DbSet<acumuladopor> acumuladopor { get; set; }
        public DbSet<categorias> categorias { get; set; }
        public DbSet<conceptos> conceptos { get; set; }
        public DbSet<formatos> formatos { get; set; }

        #endregion

    }
}


//public class MyDbContextInitializer : DropCreateDatabaseIfModelChanges<AsociadosContext>
//{
//    protected override void Seed(AsociadosContext dbContext)
//    {
//        // seed data

//        base.Seed(dbContext);
//    }
//}
