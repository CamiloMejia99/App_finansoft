using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.Accounting
{
    public class CentroCostoDTO
    {
        [Key]
        public string Codigo { get; set; }

        private int consecutivo { get; set; }

        [Required]
        public string Nombre { get; set; }

        public bool Activo { get; set; }

        public CentroCostoDTO()
        {
            Activo = true;
        }
    }
}
