using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.DeterioroCartera
{
    [Table("DCar.DeterioroCartera")]
    public class Deterioro
    {

        [Key]
        [Required]
        public int Id { get; set; }
        [ForeignKey("DeterioroPar")]
        [Required]
        [Display(Name = "Rango")]
        public int IdRango { get; set; }


        [Display(Name = "Método")]
        [StringLength(20)]
        public string Metodo { get; set; }

        [Display(Name = "Deterioro Cartera")]
        [StringLength(50)]
        public string ValorSuma { get; set; }

        [Display(Name = "Observación Deterioro")]
        [StringLength(250)]
        public string observacion { get; set; }



        [Required]
        [Display(Name = "Fecha Generado")]
        [DataType(DataType.DateTime)]
        public DateTime FechaGenerada { get; set; }

        public virtual DeterioroPar DeterioroPar { get; set; }

    }
}
