using FNTC.Finansoft.Accounting.DTO.MCreditos;
using FNTC.Finansoft.Accounting.DTO.Terceros;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FNTC.Finansoft.Accounting.DTO.Ahorros
{
    [Table("aho.FichasAhorroContractual")]
    public class FichaAhorroContractual
    {
        [Key]
        public string Id { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey("ConfACFK")]
        public int IdConfiguracion { get; set; }

        
        [Required(ErrorMessage = "Campo obligatorio")]
        [ForeignKey("TerceroFK")]
        public string IdAsociado { get; set; }

        public string NumeroCuenta { get; set; }
        public decimal ValorCuota { get; set; }
        public decimal TotalAhorro { get; set; } = 0;   
        public decimal Interes { get; set; } = 0;

        [Required(ErrorMessage = "Campo obligatorio")]
        [RegularExpression(@"^\d*$", ErrorMessage = "Campo numérico.")]
        public int Plazo { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]

        [ForeignKey("TipoPeriodoFK")]
        public int IdTipoPeriodo { get; set; }

        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Formato no válido.")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public decimal TasaEfectiva { get; set; }   

        [Required(ErrorMessage = "Campo obligatorio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]

        public DateTime FechaApertura { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime FechaVencimiento { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public DateTime FechaSistema { get; set; }

        public bool Estado { get; set; }    
        public string UserId { get; set; }


        //valores auxiliares
        [NotMapped]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string AuxValorCuota { get; set; }

        [NotMapped]
        public string AuxFechaVencimiento { get; set; }

        [NotMapped]
        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Formato no válido.")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string AuxTasaEfectiva { get; set; }

        //llaves foráneas
        public virtual ConfigAhorroContractual ConfACFK { get; set; }
        public virtual Tercero TerceroFK { get; set; }
        public virtual Tipo_Periodo TipoPeriodoFK { get; set; }


        public void SetearCamposNoMapeados()
        {
            this.AuxTasaEfectiva = "0";
            this.AuxValorCuota = "0";
        }
    }
}
