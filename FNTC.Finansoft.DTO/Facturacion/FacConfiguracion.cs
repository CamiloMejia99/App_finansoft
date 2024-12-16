using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Facturacion
{
    [Table("fac.Configuracion")]
    public class FacConfiguracion
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }

        [Required]
        [ForeignKey("idTipoComprobanteFK")]
        [DisplayName("TIPO COMPROBANTE")]
        public string idTipoComprobante { get; set; }
        [Required]
        [ForeignKey("idFPEfectivoFK")]
        [DisplayName("EFECTIVO")]
        public string idFPEfectivo { get; set; }
        [Required]
        [ForeignKey("idFPTarjetaCFK")]
        [DisplayName("TARJETA CRÉDITO")]
        public string idFPTarjetaC { get; set; }
        [Required]
        [ForeignKey("idFPTarjetaDFK")]
        [DisplayName("TARJETA DÉBITO")]
        public string idFPTarjetaD { get; set; }
        [Required]
        [ForeignKey("idFPConsigFK")]
        [DisplayName("CONSIGNACIÓN")]
        public string idFPConsig { get; set; }
        [Required]
        [ForeignKey("idFPtransfFK")]
        [DisplayName("TRANSFERENCIA")]
        public string idFPtransf { get; set; }
        [Required]
        [ForeignKey("idCONiva19FK")]
        [DisplayName("IVA 19%")]
        public string idCONiva19 { get; set; }
        [Required]
        [ForeignKey("idCONiva5FK")]
        [DisplayName("IVA 5%")]
        public string idCONiva5 { get; set; }
        [Required]
        [ForeignKey("idCONexcluidosFK")]
        [DisplayName("EXCLUIDOS")]
        public string idCONexcluidos { get; set; }
        [Required]
        [ForeignKey("idCONexcentosFK")]
        [DisplayName("EXCENTOS")]
        public string idCONexcentos { get; set; }
        [Required]
        [ForeignKey("idCONventaIva19FK")]
        [DisplayName("VENTAS IVA 19%")]
        public string idCONventaIva19 { get; set; }
        [Required]
        [ForeignKey("idCONventaIva5FK")]
        [DisplayName("VENTAS IVA 5%")]
        public string idCONventaIva5 { get; set; }
        [Required]
        [ForeignKey("idCONventaExcluidosFK")]
        [DisplayName("VENTAS EXCLUIDOS")]
        public string idCONventaExcluidos { get; set; }
        [Required]
        [ForeignKey("idCONventaExcentosFK")]
        [DisplayName("VENTAS EXCENTOS")]
        public string idCONventaExcentos { get; set; }
        [Required]
        [ForeignKey("idCONcostoIva19FK")]
        [DisplayName("COSTO IVA 19%")]
        public string idCONcostoIva19 { get; set; }
        [Required]
        [ForeignKey("idCONcostoIva5FK")]
        [DisplayName("COSTO IVA 5%")]
        public string idCONcostoIva5 { get; set; }
        [Required]
        [ForeignKey("idCONcostoExcluidosFK")]
        [DisplayName("COSTO EXCLUIDOS")]
        public string idCONcostoExcluidos { get; set; }
        [Required]
        [ForeignKey("idCONcostoExcentosFK")]
        [DisplayName("COSTO EXCENTOS")]
        public string idCONcostoExcentos { get; set; }
        [Required]
        [ForeignKey("idCONinventarioIva19FK")]
        [DisplayName("INVENTARIO IVA 19%")]
        public string idCONinventarioIva19 { get; set; }
        [Required]
        [ForeignKey("idCONinventarioIva5FK")]
        [DisplayName("INVENTARIO IVA 5%")]
        public string idCONinventarioIva5 { get; set; }
        [Required]
        [ForeignKey("idCONinventarioExcluidosFK")]
        [DisplayName("INVENTARIO EXCLUIDOS")]
        public string idCONinventarioExcluidos { get; set; }
        [Required]
        [ForeignKey("idCONinventarioExcentosFK")]
        [DisplayName("INVENTARIO EXCENTOS")]
        public string idCONinventarioExcentos { get; set; }

        public virtual TipoComprobante idTipoComprobanteFK { get; set; }
        public virtual CuentaMayor idFPEfectivoFK { get; set; }
        public virtual CuentaMayor idFPTarjetaCFK { get; set; }
        public virtual CuentaMayor idFPTarjetaDFK { get; set; }
        public virtual CuentaMayor idFPConsigFK { get; set; }
        public virtual CuentaMayor idFPtransfFK { get; set; }
        public virtual CuentaMayor idCONiva19FK { get; set; }
        public virtual CuentaMayor idCONiva5FK { get; set; }
        public virtual CuentaMayor idCONexcluidosFK { get; set; }
        public virtual CuentaMayor idCONexcentosFK { get; set; }
        public virtual CuentaMayor idCONventaIva19FK { get; set; }
        public virtual CuentaMayor idCONventaIva5FK { get; set; }
        public virtual CuentaMayor idCONventaExcluidosFK { get; set; }
        public virtual CuentaMayor idCONventaExcentosFK { get; set; }
        public virtual CuentaMayor idCONcostoIva19FK { get; set; }
        public virtual CuentaMayor idCONcostoIva5FK { get; set; }
        public virtual CuentaMayor idCONcostoExcluidosFK { get; set; }
        public virtual CuentaMayor idCONcostoExcentosFK { get; set; }
        public virtual CuentaMayor idCONinventarioIva19FK { get; set; }
        public virtual CuentaMayor idCONinventarioIva5FK { get; set; }
        public virtual CuentaMayor idCONinventarioExcluidosFK { get; set; }
        public virtual CuentaMayor idCONinventarioExcentosFK { get; set; }


    }
}
