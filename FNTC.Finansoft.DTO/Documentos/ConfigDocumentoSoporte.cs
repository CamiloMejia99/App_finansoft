using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Documentos
{
    [Table("dbo.ConfigDocumentoSoporte")]
    public class ConfigDocumentoSoporte
    {
        [Key]
        public int id { get; set; }

        [Required]
        [DisplayName("Tipo Documento Soporte")]
        [ForeignKey("tipoComprobanteFK")]
        public string tipoComprobante { get; set; }

        [Required]
        [StringLength(10)]
        [DisplayName("Prefijo")]
        public string prefijo { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Número Documento")]
        public string numDocumento { get; set; }

        [Required]
        [DisplayName("Fecha Emisión")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime fechaEmision { get; set; }

        [Required]
        [DisplayName("Vigencia")]
        public int vigencia { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Rango Desde")]
        public string rangoDesde { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Rango Hasta")]
        public string rangoHasta { get; set; }

        public bool estado { set; get; }


        public virtual TipoComprobante tipoComprobanteFK { get; set; }
    }
}
