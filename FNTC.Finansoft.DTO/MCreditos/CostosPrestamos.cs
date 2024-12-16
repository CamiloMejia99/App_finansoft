using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FNTC.Finansoft.Accounting.DTO.MCreditos
{
    public class CostosPrestamos
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id_CostoPretamo { get; set; }

        public int? CA_Id { get; set; }
        public virtual Costos_Adicionales Costos_Adicionales { get; set; }

        public string Pagare { get; set; }
        public int seCobraComo { get; set; }
        //public virtual Prestamos Prestamos { get; set; }
    }
}
