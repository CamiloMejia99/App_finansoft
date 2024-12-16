using System.ComponentModel.DataAnnotations;

namespace FNTC.Finansoft.Accounting.DTO.GestionDocumental
{
    public class GDClass1
    {
        public int id { get; set; }
        [Required]
        [Display(Name = "Codigo Cuenta")]
        public string Cuenta_Cod { get; set; }
    }
}
