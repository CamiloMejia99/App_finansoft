using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.ActivosFijos
{
    public class BActivosFijos
    {
        public int id { get; set; }

        [Required]
        [ForeignKey("GruposActivosFijos")]
        [Display(Name = "Grupo")]
        public int grupoId { get; set; }

        [Required]
        [Display(Name = "Nombre del Activo")]
        public string nombreActivo { get; set; }

        [Required]
        [ForeignKey("ClaseDeActivo")]
        [Display(Name = "Clase de Activo")]
        public int claseActivoId { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        public string descripcion { get; set; }

        [Required]
        [ForeignKey("UbicacionFisica")]
        [Display(Name = "Ubicacion Fisica")]
        public int ubicacionFisicaId { get; set; }

        [Required]
        [ForeignKey("Terceros1")]
        [Display(Name = "Responsable")]
        public string responsableNIT { get; set; }

        [Required]
        [Display(Name = "N° Activo")]
        public int numeroActivo { get; set; }

        [Required]
        [Display(Name = "N° Serie de Activo")]
        public string numeroSerie { get; set; }

        [Required]
        [Display(Name = "Fecha de Compra")]
        public DateTime fechaDeCompra { get; set; }

        [Required]
        [ForeignKey("CentroCosto")]
        [Display(Name = "Centro de Costos")]
        public string centroCostosId { get; set; }

        [Required]
        [Display(Name = "Costo Historico")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal costoDeCompra { get; set; }

        [Required]
        [Display(Name = "Metodo de Depreciacion")]
        public string metodoDepreciacion { get; set; }

        [Required]
        [Display(Name = "Veces a Depreciar Fiscal")]
        public int vecesDepreciarFiscal { get; set; }

        [Required]
        [Display(Name = "Veces a Depreciar NIIF")]
        public int vecesDepreciarNIIF { get; set; }

        [Required]
        [Display(Name = "Valor De Salvamento Fiscal")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal valorSalvamentoFiscal { get; set; }

        [Required]
        [Display(Name = "Valor Razonable")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal valorRazonable { get; set; }

        [Required]
        [Display(Name = "Valor en Libros")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal valorLibros { get; set; }

        [Required]
        [Display(Name = "Depreciacion Anterior")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal depreciacionAnterior { get; set; }

        [Required]
        [ForeignKey("CuentaMayor1")]
        [Display(Name = "Cuenta Compra")]
        public string codCuentaGasto { get; set; }

        [Required]
        [ForeignKey("CuentaMayor2")]
        [Display(Name = "Cuenta Activo")]
        public string codCuentaActivo { get; set; }

        [Required]
        [ForeignKey("CuentaMayor3")]
        [Display(Name = "Cuenta Depreciacion")]
        public string codCuentaDepreciacion { get; set; }

        [Required]
        [ForeignKey("CuentaMayor4")]
        [Display(Name = "Cuenta Depreciacion Gasto")]
        public string codCuentaGastoDepreciacion { get; set; }

        [Required]
        [ForeignKey("Terceros2")]
        [Display(Name = "Tercero")]
        public string terceroMov { get; set; }

        [Required]
        [ForeignKey("TipoComprobante")]
        [Display(Name = "Tipo Comprobante")]
        public string tipoComprobanteMov { get; set; }

        public virtual TipoComprobante TipoComprobante { get; set; }
        public virtual GruposActivosFijos GruposActivosFijos { get; set; }
        public virtual CentroCosto CentroCosto { get; set; }
        public virtual ClaseDeActivo ClaseDeActivo { get; set; }
        public virtual UbicacionFisica UbicacionFisica { get; set; }
        public virtual CuentaMayor CuentaMayor1 { get; set; }
        public virtual CuentaMayor CuentaMayor2 { get; set; }
        public virtual CuentaMayor CuentaMayor3 { get; set; }
        public virtual CuentaMayor CuentaMayor4 { get; set; }
        public virtual Tercero Terceros1 { get; set; }
        public virtual Tercero Terceros2 { get; set; }
    }
}
