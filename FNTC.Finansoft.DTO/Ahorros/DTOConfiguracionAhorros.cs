using FNTC.Finansoft.DTO.Respuestas;
using System;
using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.DTO.Ahorros
{
    public class DTOConfiguracionAhorros
    {
        public DTORespuesta Respuesta { get; set; }
        public int id { get; set; }
        public string tipoAhorro { get; set; }
        [Required]
        [Display(Name = "Codigo Cuenta Contable:")]
        public Nullable<int> cuentaContable { get; set; }
        [Required]
        [Display(Name = "Nombre(Abreviatura):")]
        public string nombre { get; set; }
        [Required]
        [Display(Name = "Porcentaje Para Ahorros:")]
        public string porcentajeParaAhorros { get; set; }
        [Required]
        [Display(Name = "Tasa Efectiva:")]
        public string tasaEfectiva { get; set; }
        [Required]
        [Display(Name = "Multiplicador:")]
        public string multiplicador { get; set; }
        [Required]
        [Display(Name = "Periodo De Liquidacion Minimo")]
        public string peridoLiquidacionMinima { get; set; }
        [Required]
        [Display(Name = "Valor Minimo")]
        public string valorMinimo { get; set; }
        [Required]
        [Display(Name = "Valor Maximo")]
        public string valorMaximo { get; set; }
        [Required]
        [Display(Name = "Dias De Gracia")]
        public string diasGracia { get; set; }
        [Required]
        [Display(Name = "Perido De Gracia Renovable")]
        public string periodoGraciaRenovable { get; set; }
        [Required]
        [Display(Name = "Perido De Inactividad Minima")]
        public string periodoInactividadMinima { get; set; }
        [Required]
        [Display(Name = "Cuotas Morosas Para Liquidar Titulo")]
        public string cuotasMorosasLiquidacionTitulo { get; set; }
        [Required]
        [Display(Name = "Modalidad")]
        public string modalidad { get; set; }
        [Required]
        [Display(Name = "Tipo De Retencion")]
        public string tipoRetencion { get; set; }
        [Required]
        [Display(Name = "Fuente Contable")]
        public string fuenteContable { get; set; }
        [Required]
        [Display(Name = "Cuenta Para Retencion")]
        public string cuentaRetencion { get; set; }
        [Required]
        [Display(Name = "Cuenta Para Gastos")]
        public string cuentaGastoInteres { get; set; }
        [Required]
        [Display(Name = "Cuenta Para Causacion")]
        public string cuentaCausacion { get; set; }
        [Required]
        [Display(Name = "Cuenta Corto Plazo")]
        public string cuentaCortoPlazo { get; set; }
        [Required]
        [Display(Name = "Cuenta Para Ingresos")]
        public string cuentaIngresos { get; set; }
        [Required]
        [Display(Name = "Cuenta Largo Plazo Activa")]
        public string cuentaLargoPlazoActiva { get; set; }
        [Required]
        [Display(Name = "Cuenta Corto Plazo Inactiva")]
        public string cuentaCortoPlazoInactiva { get; set; }
        [Required]
        [Display(Name = "Cuenta Largo Plazo Inactiva")]
        public string cuentaLargoPlazoInactiva { get; set; }
        [Required]
        [Display(Name = "Cuenta Mayor A 6 y Menor a 12 Meses")]
        public string cuentaMayor6Menor12 { get; set; }
        [Required]
        [Display(Name = "Cuenta Mayor A 12 y Menor a 18 Meses")]
        public string cuentaMayor12Menor18 { get; set; }
        [Required]
        [Display(Name = "Cuenta Mayor A 18")]
        public string cuentaMayor18 { get; set; }
        [Required]
        [Display(Name = "¿Genera Interes Causacion?")]
        public Nullable<bool> generaInteresCausacion { get; set; }
        [Required]
        [Display(Name = "¿Permite Consignacion De Saldo?")]
        public Nullable<bool> permiteConsignacionSaldo { get; set; }
        [Required]
        [Display(Name = "¿Reconoce Interes?")]
        public Nullable<bool> reconoceInteres { get; set; }
        [Required]
        [Display(Name = "¿Permite Asociado Diferente?")]
        public Nullable<bool> permiteAsociadoDiferente { get; set; }
        [Required]
        [Display(Name = "¿Permite Cambiar Fecha De Creacion?")]
        public Nullable<bool> permiteCambiarFechaCreacion { get; set; }
        [Required]
        [Display(Name = "¿Liquida Interes Estado Inactivo?")]
        public Nullable<bool> liquidaInteresEstadoInactivo { get; set; }
        [Required]
        [Display(Name = "¿Capitaliza Interes Linea Ahorros?")]
        public Nullable<bool> capitalizaInteresLineaAhorros { get; set; }
        [Required]
        [Display(Name = "Rango desde:")]
        public long RangoDesde { get; set; }

        [Required]
        [Display(Name = "Rango hasta:")]
        public long RangoHasta { get; set; }

        public long ConsecutivoActual { get; set; }

        [Required]
        [Display(Name = "Activo")]
        public Nullable<bool> activo { get; set; }
        public Nullable<System.DateTime> fechaRegistro { get; set; }
    }
}
