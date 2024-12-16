using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.Terceros
{
    [Table("ter.InfoTerceroAdicional")]
    public class InfoTerceroAdicional
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [ForeignKey("Tercero")]
        [Required]
        [Display(Name = "NIT Tercero")]
        [StringLength(20)]
        public string NitTercero { get; set; }
        [ForeignKey("estrato")]
        [Required]
        [Display(Name = "Id Estrato")]
        public int IdEstrato { get; set; }
        [ForeignKey("Contrato")]
        [Required]
        [Display(Name = "Id Contrato")]
        public int IdContrato { get; set; }
        [ForeignKey("NivelEstudio")]
        [Required]
        [Display(Name = "Id Nivel Estudio")]
        public int IdNivelEstudio { get; set; }
        [Required]
        [Display(Name = "Personas a Cargo")]
        [Range(0, short.MaxValue, ErrorMessage = "El valor {0} debe ser numérico.")]
        public int PersonasCargo { get; set; }
        [Required]
        [Display(Name = "Ocupación")]
        [StringLength(50)]
        public string Ocupacion { get; set; }
        [Required]
        [Display(Name = "Fecha Ingreso Laboral")]
        public DateTime Fechalaboral { get; set; }
        [Display(Name = "Detalle")]
        [StringLength(50)]
        public string Detalle { get; set; }
        public virtual estrato estrato { get; set; }
        public virtual Contrato Contrato { get; set; }
        public virtual NivelEstudio NivelEstudio { get; set; }
        public virtual Tercero Tercero { get; set; }
    }
}
