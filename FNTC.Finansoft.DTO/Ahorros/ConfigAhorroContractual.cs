using FNTC.Finansoft.Accounting.DTO.Contabilidad;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FNTC.Finansoft.Accounting.DTO.Ahorros
{
    [Table("aho.ConfigAhorrosContractuales")]
    public class ConfigAhorroContractual
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public string NombreConfiguracion { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [StringLength(10,ErrorMessage ="Máximo 10 caracteres")]
        public string Prefijo { get; set; }

        
        public decimal ValorMinimo { get; set; }

        
        public decimal ValorMaximo { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey("TipoComprobanteFK")]
        public string IdComprobante { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey("CuentaFK")]
        public string IdCuenta { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey("CuentaCausacionFK")]
        public string IdCuentaCausacion { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey("CuentaGastoFK")]
        public string IdCuentaGasto { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public bool SeCausa { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public bool SeCausaEnMora { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public bool RetiroAnticipado { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [RegularExpression(@"^\d*$", ErrorMessage = "Campo numérico.")]
        [Range(0, 100, ErrorMessage = "Valor fuera de rango.")]
        public int CuotasGraciaMora { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [RegularExpression(@"^\d*$", ErrorMessage = "Campo numérico.")]//### , 0.## .## , 0 y ###.##
        [Range(0,1000, ErrorMessage = "Valor fuera de rango.")]
        public int PlazoMinimo { get; set; }

        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Formato no válido.")]//### , 0.## .## , 0 y ###.##
        //[Range(1,100, ErrorMessage = "Rango permitido: 1 - 100.")]
        [Required(ErrorMessage = "Campo obligatorio")]
        [NotMapped]
        public string AuxTasaEfectivaMinima { get; set; }

        
        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Formato no válido.")]//### , 0.## .## , 0 y ###.##
        //[Range(1,100, ErrorMessage = "Rango permitido: 1 - 100.")]
        [Required(ErrorMessage = "Campo obligatorio")]
        [NotMapped]
        public string AuxTasaEfectivaMaxima { get; set; }

        public decimal TasaEfectivaMinima { get; set; }
        public decimal TasaEfectivaMaxima { get; set; }


        [Required(ErrorMessage = "Campo obligatorio")]
        public bool Morosidad { get; set; } 

        public bool Estado { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime FechaRegistro { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }


        //AUXILIARES
        [NotMapped]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string AuxValorMinimo { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string AuxValorMaximo { get; set; }


        public virtual TipoComprobante TipoComprobanteFK { get; set; }
        public virtual CuentaMayor CuentaFK { get; set; }
        public virtual CuentaMayor CuentaCausacionFK { get; set; }
        public virtual CuentaMayor CuentaGastoFK { get; set; }
    }
}
