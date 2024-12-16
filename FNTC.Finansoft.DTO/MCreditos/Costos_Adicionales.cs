using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class Costos_Adicionales
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CA_Id { get; set; }

        [Required]
        [Display(Name = "Lineas")]
        public int Lineas_Id { get; set; }

        [Required]
        [Display(Name = "Destinos")]
        public int Destino_Id { get; set; }
        public virtual Destinos Destinos { get; set; }

        [Required]
        [Display(Name = "Cuenta Contable")]
        public long Cuenta_Cod { get; set; }

        //aqui deje el trabajo

        [Required]
        [Display(Name = "Tipo de Costo")]
        public int Tipo_Costo_Id { get; set; }

        [Required]
        [Display(Name = "Incremento")]
        public int Incrementa_Id { get; set; }

        [Required]
        [Display(Name = "Nombre del Costo Adicional")]
        public string CA_Nombre { get; set; }


        [Display(Name = "Valor")]
        public string CA_Valor { get; set; }

        [Display(Name = "Porcentaje")]
        public string CA_Porcentaje { get; set; }

        [Display(Name = "Activo")]
        public bool CA_estado { get; set; }
    }
}
