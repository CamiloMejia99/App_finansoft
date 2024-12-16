using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class Destinos
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Destino_Id { get; set; }

        [Display(Name = "Codigo Destino")]
        [Required]
        public string Destino_Codigo { get; set; }

        [Display(Name = "Capital Maximo")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required]
        public long Destino_Valor_Maximo { get; set; }

        [Display(Name = "Capital Minimo")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Required]
        public long Destino_Valor_Minimo { get; set; }

        [Display(Name = "Interes Minimo (%)")]
        [Required]
        public decimal Destino_Tasa_Minima { get; set; }

        [Display(Name = "Interes Maximo (%)")]
        [Required]
        public decimal Destino_Tasa_Maxima { get; set; }

        [Display(Name = "Lineas")]
        [Required]
        public int Lineas_Id { get; set; }
        public virtual Lineas Lineas { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        public string Destino_Descripcion { get; set; }

        [Display(Name = "Tasa Mora")]
        [Required]
        public decimal Destino_Tasa_Mora { get; set; }

        [Display(Name = "Plazo Maximo")]
        [Required]
        public decimal Destino_Periodo_Maximo { get; set; }

        [Display(Name = "Plazo Minimo")]
        [Required]
        public decimal Destino_Periodo_Minimo { get; set; }

        [Display(Name = "Interes Corriente")]
        public bool Destino_Financia_Interes_Corriente { get; set; }

        [Display(Name = "Abono Interes")]
        public bool Destino_Interes_Abono_Extra_Dias { get; set; }

        [Display(Name = "Plan de Pagos")]
        public bool Destino_Interes_Igual_Plan_Pagos { get; set; }

        [Display(Name = "Interes Minimo (%)")]
        public bool Destino_Causal_Interes_Anticipado { get; set; }

        [Display(Name = "Periodo de Gracia")]
        public int Destino_Dias_Periodo_Gracia { get; set; }

        [Display(Name = "Pagare Automático")]
        public bool Destino_Pagare_Automatico { get; set; }

        [Display(Name = "Fecha Automatica")]
        public bool Destino_Fecha_Automatica { get; set; }

        [Display(Name = "Festivos")]
        public bool Destino_Cobra_Festivos { get; set; }

        [Display(Name = "Activo?")]
        public bool Destino_Activo { get; set; }


        [Display(Name = "Creditos")]
        public virtual ICollection<BCreditos> creditos { get; set; }

        [Display(Name = "Subdestinos")]
        public virtual ICollection<SubDestinos> Subdestinos { get; set; }


        public virtual ICollection<Costos_Adicionales> Costos_Adicionales { get; set; }

    }
}
