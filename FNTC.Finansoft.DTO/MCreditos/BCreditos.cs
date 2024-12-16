using FNTC.Finansoft.Accounting.DTO.Terceros;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class BCreditos
    {
        [Key]
        public int Creditos_Id { get; set; }

        [ForeignKey("prestamoFK")]
        public int Prestamo_Id { get; set; }

        [Display(Name = "Identificación")]
        [ForeignKey("terceroFK")]
        [Required]
        public string Creditos_Cedula { get; set; }

        [Display(Name = "Lineas")]
        [ForeignKey("lineaFK")]
        public int Lineas_Id { get; set; }
        ///public virtual Lineas Lineas { get; set; }

        [Display(Name = "Destino")]
        public int Destino_Id { get; set; }
        public virtual Destinos Destino { get; set; }

        [Display(Name = "Garantias")]
        public int Garantias_Id { get; set; }
        public virtual Garantias Garantias { get; set; }

        [Display(Name = "Capital")]
        [Required]
        public long Capital { get; set; }

        [Display(Name = "Estado")]
        public bool Creditos_Estado { get; set; }

        [Display(Name = "Interes")]
        [Required]
        public decimal Creditos_Interes { get; set; }

        [Display(Name = "Plazo")]
        [Required]
        public decimal Creditos_Plazo { get; set; }

        [Display(Name = "Valor de la Cuota")]
        [Required]
        public string Creditos_Cuota { get; set; }

        [Display(Name = "Interes Mora")]
        [Required]
        public decimal Creditos_Interes_Mora { get; set; }

        [Display(Name = "Saldo Capital")]
        [Required]
        public string Creditos_Saldo_Capital { get; set; }

        public int Codigo_Operador_Id { get; set; }
        public string Pagare { get; set; }

        public decimal ValorCuotaMes { get; set; }

        public virtual Lineas lineaFK { get; set; }

        public virtual Prestamos prestamoFK { get; set; }

        public virtual Tercero terceroFK { get; set; }
    }
}
